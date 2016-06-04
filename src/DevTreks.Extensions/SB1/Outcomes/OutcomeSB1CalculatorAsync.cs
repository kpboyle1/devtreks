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
    ///Purpose:		Streams through outcome documents 
    ///             and publishes events that subscribers 
    ///             can use to run calculations and analyses on 
    ///             each node in each document. Developers can override the 
    ///             protected members in base classes.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. Each calculation is mostly compute bound and run synchronously.
    ///             Exception is calcs that use DataURL property -they are hybrid sync and async.
    ///             2. Continue increasing the performance of 
    ///             streaming and async tasks.
    /// </summary>
    public class OutcomeSB1CalculatorAsync : SB1GeneralCalculatorAsync
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        public OutcomeSB1CalculatorAsync() { }
        public OutcomeSB1CalculatorAsync(CalculatorParameters calcParameters)
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
                    //writerSettings.Async = true;
                    using (XmlWriter writer = XmlWriter.Create(
                        output, writerSettings))
                    {
                        XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                        from el in StreamRoot()
                        select el);
                        root.Save(writer);
                        //alternative pattern
                        ////now process each task as it writes
                        //bool[] hasCalcs = await Task.WhenAll(_lvTasks);
                        //for (int i = 0; i < hasCalcs.Count(); i++)
                        //{
                        //    bool bHasCalc = hasCalcs[i];
                        //}
                    }
                }
            }
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
        protected virtual IEnumerable<IEnumerable<XElement>> StreamRoot()
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
                            from childElement in StreamOutcomeGroups()
                            select childElement;
                        yield return childElements;
                    }
                }
            }
        }
        protected virtual IEnumerable<XElement> StreamOutcomeGroups()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
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
                        //OnSetAncestorObjects(this.GCArguments);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamOutcomes()
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
        private IEnumerable<XElement> StreamOutcomes()
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
                        == Outcome.OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        //use streaming techniques to process descendants
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        //attach linked views separately (or the readdescendant syntax skips)
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //OnSetAncestorObjects(this.GCArguments);
                        IEnumerable<XElement> childElements = null;
                        if (this.GCCalculatorParams.DocToCalcReader.Name
                            == Outcome.OUTCOME_PRICE_TYPES.outcome.ToString())
                        {
                            childElements =
                                from childElement in StreamOutcomeOutputs()
                                select childElement;
                        }
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
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamOutcomeOutputs()
        {
            //not all analyses include outputs, so use ReadToDescendent syntax
            while (this.GCCalculatorParams.DocToCalcReader
                .ReadToDescendant(Outcome.OUTCOME_PRICE_TYPES.outcomeoutput.ToString()))
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
                    //process sibling outputs
                    while (this.GCCalculatorParams.DocToCalcReader
                        .ReadToNextSibling(Outcome.OUTCOME_PRICE_TYPES.outcomeoutput.ToString()))
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
                                == Outcome.OUTCOME_PRICE_TYPES.outcome.ToString())
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
                        == Outcome.OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        break;
                    }
                }
            }
        }
    }
}
