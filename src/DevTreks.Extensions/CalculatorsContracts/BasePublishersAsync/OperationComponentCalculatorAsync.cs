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
    ///Purpose:		Streams through operation and component documents 
    ///             and publishes events that subscribers 
    ///             can use to run calculations and analyses on 
    ///             each node in each document. Developers can override the 
    ///             protected members in base classes.
    ///Author:		www.devtreks.org
    ///Date:		2015, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. DevTreks 'app store' api will be continually improved.
    ///             2. Publishers should continue experimenting with 
    ///             XStreamingElements and increasing the performance of 
    ///             streaming and event handling.
    ///             
    /// </summary>
    public class OperationComponentCalculatorAsync : GeneralCalculator
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected OperationComponentCalculatorAsync() { }
        protected OperationComponentCalculatorAsync(CalculatorParameters calcParameters)
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
            //when writing to file, close it with the using
            StringWriter output = new StringWriter();
            this.GCCalculatorParams.DocToCalcReader 
                = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
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
                            from childElement in StreamOperationComponentGroups()
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
        protected virtual IEnumerable<XElement> StreamOperationComponentGroups()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
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
                        this.GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(this.GCArguments);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamOperationComponent()
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
        private IEnumerable<XElement> StreamOperationComponent()
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
                        == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
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
                        this.GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(this.GCArguments);
                        IEnumerable<XElement> childElements = null;
                        if (this.GCCalculatorParams.DocToCalcReader.Name
                            == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                        {
                            childElements =
                                from childElement in StreamOperationInput()
                                select childElement;
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.Name
                            == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
                        {
                            childElements =
                                from childElement in StreamComponentInput()
                                select childElement;
                        }
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
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                         || this.GCCalculatorParams.DocToCalcReader.Name
                         == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamOperationInput()
        {
            //not all analyses include inputs, so use ReadToDescendent syntax
            while (this.GCCalculatorParams.DocToCalcReader
                .ReadToDescendant(OperationComponent.OPERATION_PRICE_TYPES.operationinput.ToString()))
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
                        .ReadToNextSibling(OperationComponent.OPERATION_PRICE_TYPES.operationinput.ToString()))
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
                                == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()
                                || this.GCCalculatorParams.DocToCalcReader.Name
                                == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
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
                        == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamComponentInput()
        {
            //not all analyses include inputs, so use ReadToDescendent syntax
            while (this.GCCalculatorParams.DocToCalcReader
                .ReadToDescendant(OperationComponent.COMPONENT_PRICE_TYPES.componentinput.ToString()))
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
                        .ReadToNextSibling(OperationComponent.COMPONENT_PRICE_TYPES.componentinput.ToString()))
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
                                == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()
                                || this.GCCalculatorParams.DocToCalcReader.Name
                                == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
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
                        == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        break;
                    }
                }
            }
        }
    }
}