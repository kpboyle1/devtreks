using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Streams through input and output documents 
    ///             and publishes events that subscribers 
    ///             can use to run calculations and analyses on 
    ///             each node in each document. Developers can override the 
    ///             protected members in base classes.
    ///Author:		www.devtreks.org
    ///Date:		2015, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Each calculation is mostly compute bound and run synchronously.
    ///             Exception is calcs that use DataURL property -they are hybrid sync and async.
    ///             2. Publishers should continue experimenting with 
    ///             XStreamingElements and increasing the performance of 
    ///             streaming and event handling.
    ///             
    /// </summary>
    public class InputOutputCalculatorAsync : GeneralCalculator
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected InputOutputCalculatorAsync() { }
        protected InputOutputCalculatorAsync(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
        }

        //allows derived classes to override the default streaming 
        //and save method
        protected virtual async Task<bool> StreamAndSaveCalculationAsync()
        {
            bool bHasCalculations = false;
            //both calculators and analyzers use observationpath for initial file
            if (!CalculatorHelpers.URIAbsoluteExists(
                this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath
                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            if (!CalculatorHelpers.URIAbsoluteExists(
                this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                return bHasCalculations;
            //new temporary path to store calculator results
            //when writing to file, close it with the using
            StringWriter output = new StringWriter();
            this.GCCalculatorParams.DocToCalcReader 
                = await DevTreks.Data.Helpers.FileStorageIO.GetXmlReaderAsync(
                    this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                    this.GCCalculatorParams.AnalyzerParms.ObservationsPath);
            if (this.GCCalculatorParams.DocToCalcReader != null)
            {
                using (this.GCCalculatorParams.DocToCalcReader)
                {
                    XmlWriterSettings writerSettings = new XmlWriterSettings();
                    writerSettings.OmitXmlDeclaration = true;
                    writerSettings.Indent = true;
                    writerSettings.Async = true;
                    using (XmlWriter writer = XmlWriter.Create(
                        output, writerSettings))
                    {
                        //string sNewDocToCalcTempDocPath = CalculatorHelpers.GetFileToCalculate(this.GCCalculatorParams);
                        //descendant nodes are also processed using streaming techniques
                        //note that XStreamingElements were tested but had faults
                        XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                        from el in StreamRoot()
                        select el);
                        root.Save(output);
                    }
                }
            }
            using (output)
            {
                if (this.GCCalculatorParams.ErrorMessage == string.Empty
                        && this.GCArguments.HasCalculations)
                {
                    //move the new calculations to tempDocToCalcPath
                    //this returns an error msg
                    this.GCCalculatorParams.ErrorMessage = await CalculatorHelpers.MoveURIsAsync(
                        this.GCCalculatorParams.ExtensionDocToCalcURI, output,
                        this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
                    bHasCalculations = true;
                }
            }
            return bHasCalculations;
        }

        protected virtual IEnumerable<XElement> StreamRoot()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamInputOrOutputGroups()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            yield return currentElement;
                        }
                    }
                }
            }
        }
        protected virtual IEnumerable<XElement> StreamInputOrOutputGroups()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        this.GCCalculatorParams.ChangeStartingParams(
                            this.GCCalculatorParams.DocToCalcReader.Name);
                        //use streaming techniques to process descendants
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        //attach linked views separately (or the ancestor.linkedview can't be set)
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                            this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentBudget)
                        //that descendants use in their calculations
                        GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(GCArguments);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamInputOrOutput()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamInputOrOutput()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        //skip using while
                    }
                    else if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Input.INPUT_PRICE_TYPES.input.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == Output.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        //use streaming techniques to process descendants
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        //attach linked views separately (or the readdescendant syntax skips)
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                            this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentBudget)
                        //that descendants use in their calculations
                        GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(GCArguments);
                        if (this.GCCalculatorParams.DocToCalcReader.Name
                            == Input.INPUT_PRICE_TYPES.input.ToString())
                        {
                            IEnumerable<XElement> childElements =
                                from childElement in StreamInputSeries()
                                select childElement;
                            if (currentElement != null)
                            {
                                if (childElements != null)
                                {
                                    //add the child elements to the currentElement
                                    currentElement.Add(childElements);
                                }
                                this.RunCalculationsAndSetUpdates(ref currentElement);
                                yield return currentElement;
                            }
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.Name
                            == Output.OUTPUT_PRICE_TYPES.output.ToString())
                        {
                            IEnumerable<XElement> childElements =
                                from childElement in StreamOutputSeries()
                                select childElement;
                            if (currentElement != null)
                            {
                                if (childElements != null)
                                {
                                    //add the child elements to the currentElement
                                    currentElement.Add(childElements);
                                }
                                this.RunCalculationsAndSetUpdates(ref currentElement);
                                yield return currentElement;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                         || this.GCCalculatorParams.DocToCalcReader.Name
                         == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamInputSeries()
        {
            //not all analyses include inputs, so use ReadToDescendent syntax
            while (this.GCCalculatorParams.DocToCalcReader.ReadToDescendant(
                Input.INPUT_PRICE_TYPES.inputseries.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    XElement currentElement
                        = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                    if (currentElement != null)
                    {
                        this.RunCalculationsAndSetUpdates(ref currentElement);
                        yield return currentElement;
                    }
                    //process sibling inputs
                    while (this.GCCalculatorParams.DocToCalcReader
                        .ReadToNextSibling(Input.INPUT_PRICE_TYPES.inputseries.ToString()))
                    {
                        if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                            XElement siblingElement
                                = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                            if (siblingElement != null)
                            {
                                this.RunCalculationsAndSetUpdates(ref siblingElement);
                                yield return siblingElement;
                            }
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.EndElement)
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.Name
                                == Input.INPUT_PRICE_TYPES.input.ToString())
                            {
                                break;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Input.INPUT_PRICE_TYPES.input.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamOutputSeries()
        {
            //not all analyses include outputs, so use ReadToDescendent syntax
            while (this.GCCalculatorParams.DocToCalcReader.ReadToDescendant(
                Output.OUTPUT_PRICE_TYPES.outputseries.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    XElement currentElement
                        = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                    if (currentElement != null)
                    {
                        this.RunCalculationsAndSetUpdates(ref currentElement);
                        yield return currentElement;
                    }
                    //process sibling inputs
                    while (this.GCCalculatorParams.DocToCalcReader
                        .ReadToNextSibling(Output.OUTPUT_PRICE_TYPES.outputseries.ToString()))
                    {
                        if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                            XElement siblingElement
                                = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                            if (siblingElement != null)
                            {
                                this.RunCalculationsAndSetUpdates(ref siblingElement);
                                yield return siblingElement;
                            }
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.EndElement)
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.Name
                                == Output.OUTPUT_PRICE_TYPES.output.ToString())
                            {
                                break;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Output.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        break;
                    }
                }
            }
        }
    }
}
