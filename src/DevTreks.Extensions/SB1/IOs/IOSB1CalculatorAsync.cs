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
    ///             and runs calculations and analyses on 
    ///             each node in each document. 
    //Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. Each calculation is mostly compute bound and run synchronously.
    ///             Exception is calcs that use DataURL property -they are hybrid sync and async.
    ///             2. Continue increasing the performance of 
    ///             streaming and async tasks.
    /// </summary>
    public class IOSB1CalculatorAsync : SB1GeneralCalculatorAsync
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        public IOSB1CalculatorAsync() { }
        public IOSB1CalculatorAsync(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
        }
        
        //methods
        //subscribe to the events raised by the base class
        public async Task<bool> RunCalculatorAsync()
        {
            bool bHasAnalysis = false;
            //streams through xml and runscalcs on inputs and outputs
            bHasAnalysis = await this.StreamAndSaveCalculationAsync();
            return bHasAnalysis;
        }
        //async calcs to complete
        //List<Task<bool>> _lvTasks = new List<Task<bool>>();
        //allows derived classes to override the default streaming 
        //and save method
        protected virtual async Task<bool> StreamAndSaveCalculationAsync()
        {
            bool bHasCalculations = false;
            //both calculators and analyzers use observationpath for initial file
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath
                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI, 
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                return bHasCalculations;
            //new temporary path to store calculator results
            //when writing to file, close it with the using
            StringWriter output = new StringWriter();
            this.GCCalculatorParams.DocToCalcReader = await CalculatorHelpers.GetXmlReaderAsync(
                this.GCCalculatorParams.ExtensionDocToCalcURI, this.GCCalculatorParams.AnalyzerParms.ObservationsPath);
            if (this.GCCalculatorParams.DocToCalcReader != null)
            {
                using (this.GCCalculatorParams.DocToCalcReader)
                {
                    XmlWriterSettings writerSettings = new XmlWriterSettings();
                    writerSettings.OmitXmlDeclaration = true;
                    writerSettings.Indent = true;
                    ////can now write this this asynch
                    //writerSettings.Async = true;
                    using (XmlWriter writer = XmlWriter.Create(
                        output, writerSettings))
                    {
                        //180: switched to XStreaming, uses less memory until xml is written
                        XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                        from el in StreamingRoot()
                        select el);
                        //the write will omit xml declaration (allowing xslt transform)
                        //the write will also cause the calctasks to be completed
                        root.Save(writer);

                        //keep this code to verify that tasks are being completed once xml is serialized
                        //test whether or not this interferes with sequential completion of tasks for aggregation
                        ////now process each task as it writes
                        //bool[] hasCalcs = await Task.WhenAll(_lvTasks);
                        //for (int i = 0; i < hasCalcs.Count(); i++)
                        //{
                        //    bool bHasCalc = hasCalcs[i];
                        //}
                    }
                }
            }
            //subsequent async method calls have to use Wait() 
            //or the async returns here without the completed calculations
            //see SB1GeneralCalculatorAsync.RunCalculationsAndSetUpdatesAsync
            using (output)
            {
                if (this.GCCalculatorParams.ErrorMessage == string.Empty)
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
        protected virtual IEnumerable<IEnumerable<XElement>> StreamingRoot()
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
                        yield return childElements;
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
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //SetAncestorObjects(currentElement);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamInputOrOutputs()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            Task<bool> hascalc = this.RunCalculationsAndSetUpdatesAsync(currentElement);
                            //_lvTasks.Add(this.RunCalculationsAndSetUpdatesAsync(currentElement));
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
        private IEnumerable<XElement> StreamInputOrOutputs()
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
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //SetAncestorObjects(currentElement);
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
                                Task<bool> hascalc = this.RunCalculationsAndSetUpdatesAsync(currentElement);
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
                                Task<bool> hascalc = this.RunCalculationsAndSetUpdatesAsync(currentElement);
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
                        Task<bool> hascalc = this.RunCalculationsAndSetUpdatesAsync(currentElement);
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
                                Task<bool> hascalc = this.RunCalculationsAndSetUpdatesAsync(siblingElement);
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
                        Task<bool> hascalc = this.RunCalculationsAndSetUpdatesAsync(currentElement);
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
                                Task<bool> hascalc = this.RunCalculationsAndSetUpdatesAsync(siblingElement);
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
