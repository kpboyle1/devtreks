using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Errors = DevTreks.Exceptions.DevTreksErrors;
using DevTreksAppHelpers = DevTreks.Data.AppHelpers;
using DevTreksHelpers = DevTreks.Data.Helpers;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Aggregates the attributes of documents into delimited strings 
    ///             holding the number of observations in the new attribute.
    ///             Streams through standard DevTreks documents and publishes 
    ///             events that subscribers  can use to serialize 
    ///             and deserialize the current node being processed. Developers 
    ///             can override the protected and virtual members in base classes. 
    ///Author:		www.devtreks.org. 
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. The observations file follows DevTreks standard 
    ///             schemas and xml document structures. This allows the subsequent 
    ///             analysis publishers to stream the observations in a 
    ///             standard way (i.e. using standard DevTreks xml hierarchies),
    ///             raising standard events that subscribers use to run their 
    ///             analyses.
    ///             2. Alpha 8c added an event for adding linked views to the 
    ///             observation for resource use analyses. Linked views will 
    ///             be added to other types of analyses as the need arises. 
    ///             Note that the linked view's attributes are added to the 
    ///             parent node, not to a separate child xelement. Also note 
    ///             that the subscriber chooses which specific linked view to use.
    ///             
    /// </summary>
    public class ObservationBuilder
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected ObservationBuilder() { }
        public ObservationBuilder(CalculatorParameters calcParameters)
        {
            //set the class parameters
            this.ObsCalculatorParams = new CalculatorParameters(calcParameters);
            this.ObsArguments = new CustomEventArgs();
        }

        //calculator parameters
        public CalculatorParameters ObsCalculatorParams { get; set; }
        
        //declare the events that will be raised
        //serialize the currentElement into a subscriber's calculator 
        //or analyzer object
        public event EventHandler<CustomEventArgs> AddCurrentObservation;
        //add linked view attributes to current observation
        public event EventHandler<CustomEventArgs> AddLinkedViewToObservation;
        public CustomEventArgs ObsArguments { get; set; }

        //allows derived classes to override the event invocation behavior
        protected virtual void OnAddCurrentObservation(CustomEventArgs e)
        {
            //make a temporary copy of the event to avoid possibility of
            //a race condition if the last subscriber unsubscribes
            //immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = AddCurrentObservation;

            //event will be null if there are no subscribers
            if (handler != null)
            {
                //prepare the arguments to send inside the CustomEventArgs parameter
                e.CalculatorParams = this.ObsCalculatorParams;
                //use the () operator to raise the event.
                handler(this, e);
            }
        }
        //allows derived classes to override the event invocation behavior
        protected virtual void OnAddLinkedViewToObservation(CustomEventArgs e)
        {
            //make a temporary copy of the event to avoid possibility of
            //a race condition if the last subscriber unsubscribes
            //immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = AddLinkedViewToObservation;

            //event will be null if there are no subscribers
            if (handler != null)
            {
                //prepare the arguments to send inside the CustomEventArgs parameter
                e.CalculatorParams = this.ObsCalculatorParams;
                //use the () operator to raise the event.
                handler(this, e);
            }
        }
        //allows derived classes to override the default streaming 
        //and save method
        protected virtual bool StreamAndSaveObservation()
        {
            bool bHasObservation = false;
            //observations will be held in a new editable XElement
            this.ObsCalculatorParams.AnalyzerParms.ObservationElement 
                = new XElement(Constants.ROOT_PATH);
            //+1 because the first column holds the stats for all comparisons
            //int iFileComparisonsCount = fileOrFolderPaths.Count + 1;
            //subscribers set a "Files" attribute property using this property
            this.ObsCalculatorParams.AnalyzerParms.FilesComparisonsCount
                = this.ObsCalculatorParams.AnalyzerParms.FileOrFolderPaths.Count + 1;
            string sId = string.Empty;
            string sFileToAnalyze = string.Empty;
            int i = 0;
            //file order is not relevant in this addin,
            //but parallel won't work because of the way 
            //the observationRoot gets built
            foreach (KeyValuePair<string, string> kvp
                in this.ObsCalculatorParams.AnalyzerParms.FileOrFolderPaths)
            {
                sId = kvp.Key;
                sFileToAnalyze = kvp.Value;
                if (CalculatorHelpers.URIAbsoluteExists(
                    this.ObsCalculatorParams.ExtensionDocToCalcURI,
                    sFileToAnalyze))
                {
                    this.ObsCalculatorParams.AnalyzerParms.ObservationFile = sFileToAnalyze;
                    this.ObsCalculatorParams.AnalyzerParms.FilePositionIndex = i;
                    //stream and add the calculation (observation)
                    //to the observationsRoot
                    if (!CalculatorHelpers.URIAbsoluteExists(
                        this.ObsCalculatorParams.ExtensionDocToCalcURI,
                        this.ObsCalculatorParams.AnalyzerParms.ObservationFile))
                        return bHasObservation;
                    this.ObsCalculatorParams.DocToCalcReader 
                        = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                            this.ObsCalculatorParams.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                            this.ObsCalculatorParams.AnalyzerParms.ObservationFile);
                    if (this.ObsCalculatorParams.DocToCalcReader != null)
                    {
                        using (this.ObsCalculatorParams.DocToCalcReader)
                        {
                            this.ObsCalculatorParams.DocToCalcReader.MoveToContent();
                            this.ObsCalculatorParams.StartingDocToCalcNodeName = string.Empty;
                            this.ObsCalculatorParams.AnalyzerParms.AggregatingId = string.Empty;
                            this.ObsCalculatorParams.AnalyzerParms.Ancestors.Clear();
                            AddObservations();
                        }
                    }
                    if (this.ObsCalculatorParams.ErrorMessage != string.Empty)
                    {
                        //fix all errors before running an analysis
                        this.ObsCalculatorParams.ErrorMessage
                            = this.ObsCalculatorParams.ErrorMessage;
                        break;
                    }
                    i++;
                }
            }
            if (this.ObsCalculatorParams.ErrorMessage == string.Empty)
            {
                //save the observations file
                bHasObservation = SaveObservations();
                //error messages should be expanded 
                if (!bHasObservation)
                {
                    this.ObsCalculatorParams.ErrorMessage
                        += Errors.MakeStandardErrorMsg("ANALYSES_CANTRUN");
                }
            }
            return bHasObservation;
        }
        public virtual bool SaveObservations()
        {
            //save the intermediate file for extracting observations 
            //(for export as tsv files to statistical packages)
            string sObservationsDocPath
                = GetObservationFilePath(this.ObsCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            string sErrorMsg = string.Empty;
            CalculatorHelpers.SaveXmlInURI(
                this.ObsCalculatorParams.ExtensionDocToCalcURI,
                this.ObsCalculatorParams.AnalyzerParms.ObservationElement.CreateReader(),
               sObservationsDocPath, out sErrorMsg);
            this.ObsCalculatorParams.ErrorMessage = sErrorMsg;
            this.ObsCalculatorParams.AnalyzerParms.ObservationsPath = sObservationsDocPath;
            return true;
        }
        public static string GetObservationFilePath(string docToCalcTempDocPath)
        {
            string sObservationsDocPath
                = docToCalcTempDocPath.Replace(
                Constants.EXTENSION_XML,
                string.Concat(Constants.OBSERVATIONS, Constants.EXTENSION_XML));
            return sObservationsDocPath;
        }
        public void AddObservations()
        {
            string sCurrentNodeName = string.Empty;
            //move forward through all of the document's elements
            while (this.ObsCalculatorParams.DocToCalcReader.Read())
            {
                if (this.ObsCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.ObsCalculatorParams.DocToCalcReader.LocalName
                        == DevTreksAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        //skip the node using the while statement
                    }
                    else
                    {
                        InitCurrentObservationElement();
                        if (this.ObsCalculatorParams.CurrentElementNodeName
                            == Constants.ROOT_PATH
                            || this.ObsCalculatorParams.CurrentElementNodeName
                            == DevTreksAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            //these nodes are not needed in any analysis yet
                        }
                        else if (this.ObsCalculatorParams.CurrentElementNodeName
                            != string.Empty)
                        {
                            bool bIsGroupingNode
                                = CalculatorHelpers.IsGroupingNode(this.ObsCalculatorParams.CurrentElementNodeName);
                            //grouping nodes are automatically appended when missing from observationsRoot
                            if (!bIsGroupingNode)
                            {
                                bool bIsGoodNode = CalculatorHelpers.SetCurrentElementIds(
                                    this.ObsCalculatorParams, this.ObsCalculatorParams.CurrentElementNodeName);
                                if (bIsGoodNode)
                                {
                                    AddObservationToAnalysis();
                                    if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                                        != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                                    {
                                        //reset currentelement
                                        this.ObsCalculatorParams.CurrentElementNodeName
                                            = string.Empty;
                                        this.ObsCalculatorParams.CurrentElementURIPattern
                                            = string.Empty;
                                    }
                                    //stop reading if any errors on hand
                                    if (this.ObsCalculatorParams.ErrorMessage
                                        != string.Empty)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void InitCurrentObservationElement()
        {
            this.ObsCalculatorParams.CurrentElementNodeName 
                = this.ObsCalculatorParams.DocToCalcReader.Name;
            //skip over root-linkedview elements
            if (this.ObsCalculatorParams.CurrentElementNodeName
                == Constants.ROOT_PATH)
            {
                if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                {
                    this.ObsCalculatorParams.DocToCalcReader.Skip();
                    this.ObsCalculatorParams.CurrentElementNodeName
                        = this.ObsCalculatorParams.DocToCalcReader.Name;
                    //skip multiple sibling linkedviews
                    if (this.ObsCalculatorParams.CurrentElementNodeName
                        == Constants.ROOT_PATH)
                    {
                        InitCurrentObservationElement();
                    }
                }
                else
                {
                    //Input-based analyzers need linkedview element attributes added
                    //to the currentelement (note that they don't get added as typical 
                    //children xmldocs). Future releases may expand the analyses needing 
                    //linked view data.
                    string sCurrentNodeName = CalculatorHelpers.GetURIPatternNodeName(
                        this.ObsCalculatorParams.CurrentElementURIPattern);
                    if (!sCurrentNodeName.EndsWith(
                        Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        this.ObsCalculatorParams.DocToCalcReader.Skip();
                        this.ObsCalculatorParams.CurrentElementNodeName
                            = this.ObsCalculatorParams.DocToCalcReader.Name;
                    }
                }
            }
            if (this.ObsCalculatorParams.CurrentElementNodeName != string.Empty)
            {
                //npv calculated files use root-group structure
                if (this.ObsCalculatorParams.DocToCalcReader.Depth == 1)
                {
                    if (this.ObsCalculatorParams.StartingDocToCalcNodeName
                        == string.Empty
                        || this.ObsCalculatorParams.StartingDocToCalcNodeName
                        == Constants.NONE)
                    {
                        //starting node name will be budgetgroup, investmentgroup, 
                        //operationgroup, componentgroup, outcomegroup, inputgroup, or outputgroup
                        this.ObsCalculatorParams.StartingDocToCalcNodeName
                            = this.ObsCalculatorParams.CurrentElementNodeName;
                        this.ObsCalculatorParams.SubApplicationType
                            = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                            this.ObsCalculatorParams.CurrentElementNodeName);
                    }
                    this.ObsCalculatorParams.AnalyzerParms.IsTechnologyAnalysis
                    = (this.ObsCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.inputprices
                    || this.ObsCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outputprices)
                    ? false : true;
                }
            }
        }
       
        public virtual void AddObservationToAnalysis()
        {
            //does this element need totals?
            bool bNeedsFullTotals = false;
            //which attribute should be used for a name?
            string sAttributeNameForName = string.Empty;
            string sAnalysisNodeValue = string.Empty;
            //is this element needed in the observations file?
            bool bNeedsElementInObservationsDoc = false;
            //potential refactor: could raise an event that can 
            //be captured to allow analyzer developers more control
            if (this.ObsCalculatorParams.AnalyzerParms.IsTechnologyAnalysis == true)
            {
                bNeedsElementInObservationsDoc 
                    = SetTechAggregationAttributes(out sAttributeNameForName, 
                    out bNeedsFullTotals);
            }
            else
            {
                bNeedsElementInObservationsDoc 
                    = SetPriceAggregationAttributes(out sAttributeNameForName, 
                    out bNeedsFullTotals);
            }
            if (bNeedsElementInObservationsDoc 
                && this.ObsCalculatorParams.DocToCalcReader.HasAttributes)
            {
                int iNodeDepth = this.ObsCalculatorParams.DocToCalcReader.Depth;
                //set the attribute ids where observations go
                SetCurrentAggregatingElement();
                //add the observation
                SetObservation(sAttributeNameForName, bNeedsFullTotals);

                //hold off implementing -this will take time
                ////0.9.+ way to mimic using separate files
                //IncrementFilesCount(iNodeDepth);
            }
        }
        private void IncrementFilesCount(int nodeDepth)
        {
            if (nodeDepth == 2)
            {
                this.ObsCalculatorParams.AnalyzerParms.FilesComparisonsCount += 1;
                this.ObsCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            }
        }
        public virtual void AddLinkedViewToCurrentElement()
        {
            //process any xmldoc that has not been skipped during initcurrentelement
            if (this.ObsCalculatorParams.CurrentElementNodeName
                == Constants.ROOT_PATH)
            {
                //add a linkedview element's attributes to the 
                //current element
                AddLinkedViewAttributesToCurrentElement();
                //reset the currentElement
                this.ObsCalculatorParams.CurrentElementNodeName
                        = string.Empty;
                this.ObsCalculatorParams.CurrentElementURIPattern
                    = string.Empty;
            }
        }
        private void AddLinkedViewAttributesToCurrentElement()
        {
            string sCurrentNodeName = CalculatorHelpers.GetURIPatternNodeName(
                this.ObsCalculatorParams.CurrentElementURIPattern);
            if (!string.IsNullOrEmpty(sCurrentNodeName))
            {
                XElement root = new XElement(sCurrentNodeName);
                XElement xmlDocElement = XElement.Load(
                    this.ObsCalculatorParams.DocToCalcReader.ReadSubtree());
                root.Add(xmlDocElement);
                //add siblings
                while (this.ObsCalculatorParams.DocToCalcReader
                   .ReadToNextSibling(Constants.ROOT_PATH))
                {
                    if (this.ObsCalculatorParams.DocToCalcReader.NodeType
                        == XmlNodeType.Element)
                    {
                        XElement sibDocElement = XElement.Load(
                            this.ObsCalculatorParams.DocToCalcReader.ReadSubtree());
                        root.Add(sibDocElement);
                    }
                }
                if (root.HasElements)
                {
                    //subscriber chooses which linked view's attributes to add
                    AddLinkedViewAttributesToObservation(root);
                }
            }
        }
        public virtual XElement GetLinkedView(XElement rootCalculators)
        {
            //recommended method that subscriber uses to choose which 
            //linked view's attributes to add
            //use the same rules for finding related calculators as all other publishers
            XElement allyLinkedView = CalculatorHelpers.GetAllyCalculator(this.ObsCalculatorParams,
                rootCalculators, this.ObsCalculatorParams.CurrentElementNodeName,
                this.ObsCalculatorParams.CalculatorType, this.ObsCalculatorParams.RelatedCalculatorType,
                this.ObsCalculatorParams.RelatedCalculatorsType, this.ObsCalculatorParams.WhatIfScenario);
            return allyLinkedView;
        }
        private void SetCurrentAggregatingElement()
        {
            if (!string.IsNullOrEmpty(this.ObsCalculatorParams.AnalyzerParms.AggregatingId))
            {
                //the existing element where new observations will be added:
                string sCurrentId 
                    = CalculatorHelpers.GetURIPatternId(
                    this.ObsCalculatorParams.CurrentElementURIPattern);
                string sCurrentAggregatingURIPattern
                    = this.ObsCalculatorParams.CurrentElementURIPattern.Replace(sCurrentId,
                    this.ObsCalculatorParams.AnalyzerParms.AggregatingId);
                this.ObsCalculatorParams.CurrentElementURIPattern 
                    = sCurrentAggregatingURIPattern;
                //add to the ancestor array so that this node can
                //be appended to the right ancestor
                AddToAncestorArray();
                //and set the ParentURIPattern where it will be appended
                SetParentAncestor();
            }
        }
        private void AddToAncestorArray()
        {
            //note: currentURIPattern has been changed to the aggregatingId
            if (this.ObsCalculatorParams.AnalyzerParms.Ancestors.ContainsKey(
                this.ObsCalculatorParams.CurrentElementNodeName))
            {
                //replace it 
                this.ObsCalculatorParams.AnalyzerParms.Ancestors[this.ObsCalculatorParams.CurrentElementNodeName]
                    = this.ObsCalculatorParams.CurrentElementURIPattern;
            }
            else
            {
                //add it
                this.ObsCalculatorParams.AnalyzerParms.Ancestors.Add(this.ObsCalculatorParams.CurrentElementNodeName,
                   this.ObsCalculatorParams.CurrentElementURIPattern);
            }
        }
        private string GetCurrentAncestorValue()
        {
            string sAncestorURIPattern = string.Empty;
            if (this.ObsCalculatorParams.AnalyzerParms.Ancestors.ContainsKey(
                this.ObsCalculatorParams.CurrentElementNodeName))
            {
                this.ObsCalculatorParams.AnalyzerParms.Ancestors.TryGetValue(
                    this.ObsCalculatorParams.CurrentElementNodeName, out sAncestorURIPattern);
            }
            return sAncestorURIPattern;
        }
        private void RemoveFromAncestorArray()
        {
            if (this.ObsCalculatorParams.AnalyzerParms.Ancestors.ContainsKey(
                this.ObsCalculatorParams.CurrentElementNodeName))
            {
                this.ObsCalculatorParams.AnalyzerParms.Ancestors
                    .Remove(this.ObsCalculatorParams.CurrentElementNodeName);
            }
        }
        private void SetParentAncestor()
        {
            string sParentURIPattern = GetParentAncestor();
            this.ObsCalculatorParams.ParentElementURIPattern = sParentURIPattern;
        }
        private string GetParentAncestor()
        {
            string sParentURIPattern
                = this.ObsCalculatorParams.GetParentAncestorURIPattern(
                this.ObsCalculatorParams.CurrentElementNodeName);
            if (this.ObsCalculatorParams.AnalyzerParms.IsTechnologyAnalysis)
            {
                //outcomes and op/comps are siblings, not ancestors
                if (this.ObsCalculatorParams.CurrentElementNodeName
                    .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                    || this.ObsCalculatorParams.CurrentElementNodeName
                    .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
                {
                    string sParentNodeName
                        = CalculatorHelpers.GetURIPatternNodeName
                        (sParentURIPattern);
                    if (sParentNodeName.EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
                    {
                        sParentURIPattern
                            = this.ObsCalculatorParams.GetParentAncestorURIPattern(
                            sParentNodeName);
                    }
                }
                else if (this.ObsCalculatorParams.CurrentElementNodeName
                    .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
                {
                    string sParentNodeName
                        = CalculatorHelpers.GetURIPatternNodeName(
                        sParentURIPattern);
                    if (sParentNodeName
                        .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                        || sParentNodeName
                        .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
                    {
                        sParentURIPattern
                            = this.ObsCalculatorParams.GetParentAncestorURIPattern(
                            sParentNodeName);
                    }
                }
            }
            return sParentURIPattern;
        }

        //this could be a candidate to raise an event
        private bool SetTechAggregationAttributes(
            out string analysisAttributeForName, out bool needsFullTotals)
        {
            bool bNeedsElementInObservationsDoc = true;
            string sAggregatingAttributeName = string.Empty;
            string sAggregatingId = string.Empty;
            analysisAttributeForName = string.Empty;
            needsFullTotals = true;
            switch (this.ObsCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    if (this.ObsCalculatorParams.CurrentElementNodeName
                        .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName = string.Empty;
                            this.ObsCalculatorParams.AnalyzerParms.AggregatingId = string.Empty;
                            needsFullTotals = false;
                            bNeedsElementInObservationsDoc = false;
                        }
                        else
                        {
                            sAggregatingAttributeName = Calculator1.cLabel;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            FixId(ref sAggregatingId);
                            this.ObsCalculatorParams.AnalyzerParms.AggregatingId = sAggregatingId;
                            analysisAttributeForName
                                = Calculator1.cName;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName
                        .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName = string.Empty;
                            this.ObsCalculatorParams.AnalyzerParms.AggregatingId = string.Empty;
                            needsFullTotals = false;
                            bNeedsElementInObservationsDoc = false;
                        }
                        else
                        {
                            sAggregatingAttributeName = Calculator1.cLabel;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            FixId(ref sAggregatingId);
                            this.ObsCalculatorParams.AnalyzerParms.AggregatingId = sAggregatingId;
                            analysisAttributeForName
                                = Calculator1.cName;
                        }
                    }
                    else
                    {
                        //budgetgroups, investmentgroups, budgets, investments and timeperiods can be grouping nodes
                        //DevTreks convention is to use integers for Ids, not a string Label
                        sAggregatingAttributeName = Calculator1.cLabel;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        FixId(ref sAggregatingId);
                        this.ObsCalculatorParams.AnalyzerParms.AggregatingId = sAggregatingId;
                        needsFullTotals = true;
                        analysisAttributeForName = Calculator1.cName;
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    //only budgets will be analyzed or displayed (not budgetgroups)
                    if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName 
                            = Calculator1.cTypeId;
                        analysisAttributeForName 
                            = Calculator1.cTypeName;
                        //the id is needed as the aggregator for
                        //groups, budgets and budgettimeperiods
                        sAggregatingId 
                            = GetAggregatingId(
                        sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cTypeId;
                        analysisAttributeForName
                            = Calculator1.cTypeName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.BUDGET_TYPES.budget.ToString())
                    {
                        analysisAttributeForName
                            = Calculator1.cName;
                        //use the "typeid" set at the group node
                        sAggregatingId = this.ObsCalculatorParams.AnalyzerParms.AggregatingId;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
                    {
                        analysisAttributeForName
                            = Calculator1.cTypeName;
                        //use the "typeid" set at the group node
                        sAggregatingId = this.ObsCalculatorParams.AnalyzerParms.AggregatingId;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                        || this.ObsCalculatorParams.CurrentElementNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        //only node is needed (doesn't have a typeid)
                        analysisAttributeForName 
                            = Calculator1.cName;
                        //use the "typeid" set at the group node
                        sAggregatingId = this.ObsCalculatorParams.AnalyzerParms.AggregatingId;
                        //needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName
                        == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName
                            = Calculator1.cTypeId;
                        analysisAttributeForName
                            = Calculator1.cTypeName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName 
                            = Calculator1.cTypeId;
                        analysisAttributeForName
                            = Calculator1.cTypeName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName 
                            = Calculator1.cTypeId;
                        analysisAttributeForName
                            = Calculator1.cTypeName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName
                        .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
                    {
                        sAggregatingAttributeName
                            = Calculator1.cTypeId;
                        analysisAttributeForName
                            = Calculator1.cTypeName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cTypeId;
                        analysisAttributeForName 
                            = Calculator1.cTypeName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cTypeId;
                        analysisAttributeForName 
                            = Calculator1.cTypeName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName = string.Empty;
                            sAggregatingId = string.Empty;
                            bNeedsElementInObservationsDoc = false;
                            needsFullTotals = false;
                        }
                        else
                        {
                            sAggregatingAttributeName
                                = Calculator1.cTypeId;
                            analysisAttributeForName
                                = Calculator1.cTypeName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName = string.Empty;
                            sAggregatingId = string.Empty;
                            bNeedsElementInObservationsDoc = false;
                            needsFullTotals = false;
                        }
                        else
                        {
                            sAggregatingAttributeName
                                = Calculator1.cTypeId;
                            analysisAttributeForName
                                = Calculator1.cTypeName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    //only budgets can be grouped
                    if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName
                            = Calculator1.cTypeId;                         
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName 
                            = Calculator1.cTypeId;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.BUDGET_TYPES.budget.ToString())
                    {
                        //parent is a 'group' node
                        sAggregatingAttributeName 
                            = DevTreksAppHelpers.Economics1.BUDGETSYSTEM_ID;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
                    {
                        sAggregatingAttributeName 
                            = DevTreksAppHelpers.Economics1.INVESTMENTSYSTEM_ID;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                        || this.ObsCalculatorParams.CurrentElementNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName 
                            = Calculator1.cId;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = this.ObsCalculatorParams.AnalyzerParms.AggregatingId;
                        //needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName
                        == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName
                            = Calculator1.cId;
                        analysisAttributeForName
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        //only node is needed
                        sAggregatingAttributeName 
                            = Calculator1.cId;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cId;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = false;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName
                        .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
                    {
                        sAggregatingAttributeName
                            = DevTreksAppHelpers.Prices.OUTCOME_GROUP_ID;
                        analysisAttributeForName
                            = DevTreksAppHelpers.Prices.OUTCOME_GROUP_NAME;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
                    {
                        sAggregatingAttributeName 
                            = DevTreksAppHelpers.Prices.OPERATION_GROUP_ID;
                        analysisAttributeForName
                            = DevTreksAppHelpers.Prices.OPERATION_GROUP_NAME;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
                    {
                        sAggregatingAttributeName 
                            = DevTreksAppHelpers.Prices.COMPONENT_GROUP_ID;
                        analysisAttributeForName 
                            = DevTreksAppHelpers.Prices.COMPONENT_GROUP_NAME;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName = string.Empty;
                            sAggregatingId = string.Empty;
                            needsFullTotals = false;
                            bNeedsElementInObservationsDoc = false;
                        }
                        else
                        {
                            sAggregatingAttributeName
                                = DevTreksAppHelpers.Prices.OUTPUT_GROUP_ID;
                            analysisAttributeForName
                                = DevTreksAppHelpers.Prices.OUTPUT_GROUP_NAME;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName = string.Empty;
                            sAggregatingId = string.Empty;
                            needsFullTotals = false;
                            bNeedsElementInObservationsDoc = false;
                        }
                        else
                        {
                            sAggregatingAttributeName
                            = DevTreksAppHelpers.Prices.INPUT_GROUP_ID;
                            analysisAttributeForName
                                = DevTreksAppHelpers.Prices.INPUT_GROUP_NAME;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    break;
                default:
                    break;
            }
            this.ObsCalculatorParams.AnalyzerParms.AggregatingId = sAggregatingId;
            if (sAggregatingId == string.Empty)
                bNeedsElementInObservationsDoc = false;
            return bNeedsElementInObservationsDoc;
        }
        private bool SetPriceAggregationAttributes(
            out string analysisAttributeForName, 
            out bool needsFullTotals)
        {
            bool bNeedsElementInObservationsDoc = true;
            string sAggregatingAttributeName = string.Empty;
            string sAggregatingId = string.Empty;
            analysisAttributeForName = string.Empty;
            needsFullTotals = true;
            switch (this.ObsCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    //only series can't be used to aggregate
                    if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cLabel;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cLabel;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName.EndsWith(
                        Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cLabel;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName.EndsWith(
                        Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        sAggregatingAttributeName 
                            = Calculator1.cLabel;
                        analysisAttributeForName 
                            = Calculator1.cName;
                        sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                        needsFullTotals = true;
                    }
                    FixId(ref sAggregatingId);
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    //aggregation will use typeid found in pricegroup
                    if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cTypeId;
                            analysisAttributeForName 
                                = Calculator1.cTypeName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = string.Empty;
                            needsFullTotals = false;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cTypeId;
                            analysisAttributeForName 
                                = Calculator1.cTypeName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = string.Empty;
                            needsFullTotals = false;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName.EndsWith(
                        Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = DevTreksAppHelpers.Prices.INPUT_GROUP_ID;
                            analysisAttributeForName 
                                = DevTreksAppHelpers.Prices.INPUT_GROUP_NAME;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cTypeId;
                            analysisAttributeForName 
                                = Calculator1.cTypeName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName.EndsWith(
                        Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = DevTreksAppHelpers.Prices.OUTPUT_GROUP_ID;
                            analysisAttributeForName 
                                = DevTreksAppHelpers.Prices.OUTPUT_GROUP_NAME;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cTypeId;
                            analysisAttributeForName 
                                = Calculator1.cTypeName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    //aggregation will use the id found in pricegroup
                    if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = false;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName 
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = false;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName.EndsWith(
                        Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = DevTreksAppHelpers.Prices.INPUT_GROUP_ID;
                            analysisAttributeForName 
                                = DevTreksAppHelpers.Prices.INPUT_GROUP_NAME;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    else if (this.ObsCalculatorParams.CurrentElementNodeName.EndsWith(
                        Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        if (this.ObsCalculatorParams.AnalyzerParms.AnalyzerGeneralType 
                            != AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased)
                        {
                            sAggregatingAttributeName 
                                = Calculator1.cId;
                            analysisAttributeForName 
                                = Calculator1.cName;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                        else
                        {
                            sAggregatingAttributeName 
                                = DevTreksAppHelpers.Prices.OUTPUT_GROUP_ID;
                            analysisAttributeForName 
                                = DevTreksAppHelpers.Prices.OUTPUT_GROUP_NAME;
                            sAggregatingId = GetAggregatingId(sAggregatingAttributeName);
                            needsFullTotals = true;
                        }
                    }
                    break;
                default:
                    break;
            }
            this.ObsCalculatorParams.AnalyzerParms.AggregatingId = sAggregatingId;
            return bNeedsElementInObservationsDoc;
        }
        private void FixId(ref string currentId)
        {
            currentId = currentId.Replace(
                DevTreksHelpers.GeneralHelpers.WEBFILE_PATH_DELIMITER, string.Empty);
        }
        private string GetAggregatingId(string aggregatingAttName)
        {
            string sAggregatingId = string.Empty;
            //DevTreks convention is to use integers for Ids, not a string Label
            //1. Do a 'label' search of the analysis root to find the sibling integer Id
            string sAggregatingAttValue 
                = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(aggregatingAttName);
            //2. To ensure the uniqueness of the aggregating id, the parent has to be
            //included in the analysisroot search; get the parent from the ancestor array
            string sParentId = string.Empty;
            string sParentNodeName = string.Empty;
            GetParentIds(this.ObsCalculatorParams, out sParentId, out sParentNodeName);
            if (string.IsNullOrEmpty(sParentId))
            {
                if (this.ObsCalculatorParams.AnalyzerParms.AggregationType 
                    == AnalyzerHelper.AGGREGATION_OPTIONS.labels)
                {
                    sAggregatingId 
                        = CalculatorHelpers.GetElementIdUsingSiblingAttribute(
                        this.ObsCalculatorParams.AnalyzerParms.ObservationElement, 
                        this.ObsCalculatorParams.CurrentElementNodeName, 
                        aggregatingAttName, sAggregatingAttValue);
                }
                else 
                {
                    sAggregatingId = sAggregatingAttValue;
                }
            }
            else
            {
                if (this.ObsCalculatorParams.AnalyzerParms.AggregationType 
                    == AnalyzerHelper.AGGREGATION_OPTIONS.labels)
                {
                    //the aggregator for label aggregations is the sibling id of the label 
                    sAggregatingId = CalculatorHelpers
                        .GetElementIdUsingParentAndSibling(this.ObsCalculatorParams.AnalyzerParms.ObservationElement,
                        sParentId, sParentNodeName, this.ObsCalculatorParams.CurrentElementNodeName,
                        aggregatingAttName, sAggregatingAttValue);
                }
                else
                {
                    sAggregatingId = CalculatorHelpers
                        .GetElementIdUsingParentAndSibling(this.ObsCalculatorParams.AnalyzerParms.ObservationElement,
                        sParentId, sParentNodeName, this.ObsCalculatorParams.CurrentElementNodeName,
                        Calculator1.cId, sAggregatingAttValue);
                }
            }
            //3. If it's not in the analysis root, use the existing node's attributes
            if (string.IsNullOrEmpty(sAggregatingId))
            {
                if (this.ObsCalculatorParams.AnalyzerParms.AggregationType 
                    != AnalyzerHelper.AGGREGATION_OPTIONS.labels)
                {
                    sAggregatingId = sAggregatingAttValue;
                }
                if (string.IsNullOrEmpty(sAggregatingId))
                {
                    sAggregatingId 
                        = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(Calculator1.cId);
                }
            }
            return sAggregatingId;
        }
        private void GetParentIds(CalculatorParameters calcParams,  
            out string parentId, out string parentNodeName)
        {
            parentId = string.Empty;
            parentNodeName = string.Empty;
            //temporarily add to ancestor array so that parent can be obtained
            string sCurrentAncestorValue = GetCurrentAncestorValue();
            AddToAncestorArray();
            string sParentURIPattern = GetParentAncestor();
            //remove from ancestor array (needs aggregating id before adding)
            RemoveFromAncestorArray();
            if (sCurrentAncestorValue != string.Empty)
            {
                //put it back into ancestor array
                this.ObsCalculatorParams.AnalyzerParms.Ancestors.Add(
                    this.ObsCalculatorParams.CurrentElementNodeName,
                   sCurrentAncestorValue);
            }
            if (!string.IsNullOrEmpty(sParentURIPattern))
            {
                parentId = CalculatorHelpers.GetURIPatternId(sParentURIPattern);
                parentNodeName = CalculatorHelpers
                    .GetURIPatternNodeName(sParentURIPattern);
            }
        }
        private void SetObservation(string attributeNameForName,
            bool needsFullTotals)
        {
            string sAnalysisNodeValue = string.Empty;
            //get an existing observation using these specific ancestors
            bool bObservationExists = false;
            XElement existingObservation = CalculatorHelpers.GetDescendant(
                this.ObsCalculatorParams.AnalyzerParms.AggregatingId,
                this.ObsCalculatorParams.CurrentElementNodeName,
                this.ObsCalculatorParams.ParentElementURIPattern,
                this.ObsCalculatorParams.AnalyzerParms.Ancestors, 
                this.ObsCalculatorParams.AnalyzerParms.ObservationElement);
            //need a byref parameter
            XElement root = this.ObsCalculatorParams.AnalyzerParms.ObservationElement;
            bool bHasNewObservation = false;
            if (existingObservation == null)
            {
                XElement oNewElement
                    = new XElement(this.ObsCalculatorParams.CurrentElementNodeName);
                PrepNewNode(attributeNameForName, needsFullTotals,
                    out sAnalysisNodeValue, ref oNewElement);
                if (needsFullTotals == true)
                {
                    //set the properties that the event subscriber
                    //will use to add attributes to oNewElement
                    ObsArguments.CurrentElement = oNewElement;
                    OnAddCurrentObservation(ObsArguments);
                    //reset attributes that the subscriber may have 
                    //mistakenly set (Id, Name, Label ...)
                    //set attributes that subscriber should not set 
                    SetStandardElementAttributes(bObservationExists,
                        attributeNameForName, ObsArguments.CurrentElement);
                    //reset oNewElement to the changes made by 
                    //the event subscriber
                    bHasNewObservation = CalculatorHelpers.ReplaceOrInsertDescendantElement(
                        ObsArguments.CurrentElement, this.ObsCalculatorParams.ParentElementURIPattern,
                        this.ObsCalculatorParams.AnalyzerParms.Ancestors, ref root);
                }
                else
                {
                    bHasNewObservation = CalculatorHelpers.ReplaceOrInsertDescendantElement(
                        oNewElement, this.ObsCalculatorParams.ParentElementURIPattern,
                        this.ObsCalculatorParams.AnalyzerParms.Ancestors, ref root);
                }
            }
            else
            {
                if (needsFullTotals == true)
                {
                    bObservationExists = true;
                    //set the properties that the event subscriber
                    //will use to add attributes to oExistingElement
                    ObsArguments.CurrentElement = existingObservation;
                    OnAddCurrentObservation(ObsArguments);
                    //set attributes that subscriber should not set 
                    SetStandardElementAttributes(bObservationExists,
                        attributeNameForName, ObsArguments.CurrentElement);
                    //reset oExistingElement to the changes made by 
                    //the event subscriber
                    bHasNewObservation = CalculatorHelpers.ReplaceOrInsertDescendantElement(
                        ObsArguments.CurrentElement, this.ObsCalculatorParams.ParentElementURIPattern,
                        this.ObsCalculatorParams.AnalyzerParms.Ancestors, ref root);
                }
                else
                {
                    bHasNewObservation = CalculatorHelpers.ReplaceOrInsertDescendantElement(
                        existingObservation, this.ObsCalculatorParams.ParentElementURIPattern,
                        this.ObsCalculatorParams.AnalyzerParms.Ancestors, ref root);
                }
            }
            if (bHasNewObservation)
            {
                this.ObsCalculatorParams.AnalyzerParms.ObservationElement = root;
            }
            else
            {
                this.ObsCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("OBSERVATIONS_MISSING_REPLACEMENT");
            }
        }
        private void AddLinkedViewAttributesToObservation(XElement linkedViews)
        {
            string sAnalysisNodeValue = string.Empty;
            //get an existing observation using these specific ancestors
            string sCurrentElementNodeName
                = CalculatorHelpers.GetURIPatternNodeName(this.ObsCalculatorParams.CurrentElementURIPattern);
            XElement existingObservation = CalculatorHelpers.GetDescendant(
                this.ObsCalculatorParams.AnalyzerParms.AggregatingId,
                sCurrentElementNodeName,
                this.ObsCalculatorParams.ParentElementURIPattern,
                this.ObsCalculatorParams.AnalyzerParms.Ancestors,
                this.ObsCalculatorParams.AnalyzerParms.ObservationElement);
            //need a byref parameter
            XElement root = this.ObsCalculatorParams.AnalyzerParms.ObservationElement;
            bool bHasNewObservation = false;
            if (existingObservation != null)
            {
                //set the properties that the event subscriber
                //will use to add attributes to oExistingElement
                ObsArguments.CurrentElement = new XElement(existingObservation);
                OnAddCurrentObservation(ObsArguments);
                XElement existingAnalyzer = new XElement(this.ObsCalculatorParams.LinkedViewElement);
                this.ObsCalculatorParams.LinkedViewElement = linkedViews;
                string sExistingCalculatorType = this.ObsCalculatorParams.CalculatorType;
                string sCalculatorType = CalculatorHelpers.GetCalculatorType(
                    existingAnalyzer);
                this.ObsCalculatorParams.CalculatorType = sCalculatorType;
                //raise the event
                OnAddLinkedViewToObservation(ObsArguments);
                //reset the calculator
                this.ObsCalculatorParams.LinkedViewElement = existingAnalyzer;
                this.ObsCalculatorParams.CalculatorType = sExistingCalculatorType;
                //set attributes that subscriber should not set 
                SetStandardLinkedViewAttributes(ObsArguments.CurrentElement);
                //reset oExistingElement to the changes made by 
                //the event subscriber
                bHasNewObservation = CalculatorHelpers.ReplaceOrInsertDescendantElement(
                    ObsArguments.CurrentElement, this.ObsCalculatorParams.ParentElementURIPattern,
                    this.ObsCalculatorParams.AnalyzerParms.Ancestors, ref root);
            }
            if (bHasNewObservation)
            {
                this.ObsCalculatorParams.AnalyzerParms.ObservationElement = root;
            }
            else
            {
                this.ObsCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("OBSERVATIONS_MISSING_REPLACEMENT");
            }
        }
        private void PrepNewNode(string attributeNameForName, bool needsFullTotals, 
            out string analysisNodeValue, ref XElement newObservationsElement)
        {
            analysisNodeValue = string.Empty;
            //need id and name in first positions
            bool bAnalysisHasElement = false;
            SetAggregatorsAndName(bAnalysisHasElement, attributeNameForName, 
                ref newObservationsElement, out analysisNodeValue);
            //set units, dates (this could be moved to event subscribers)
            SetStandardElementAttributes(bAnalysisHasElement, 
                attributeNameForName, newObservationsElement);
        }
        private void SetStandardElementAttributes(bool analysisHasElement,
            string attributeNameForName, XElement observationElement)
        {
            string sNameForDescription = string.Empty;
            SetAggregatorsAndName(analysisHasElement, attributeNameForName,
                ref observationElement, out sNameForDescription);
            //set units, dates (this could be moved to event subscribers)
            SetNodeDescriptors(analysisHasElement, ref observationElement);
            if (analysisHasElement)
            {
                SetDescriptor(Calculator1.cName, Calculator1.cDescription, 
                    ref observationElement);
            }
            else
            {
                sNameForDescription = string.Concat(sNameForDescription,
                    Constants.FILENAME_DELIMITER,
                    this.ObsCalculatorParams.AnalyzerParms.FilePositionIndex.ToString());
                observationElement.SetAttributeValue(
                    Calculator1.cDescription, sNameForDescription);
            }
            SetFileCountAndObservationCount(ref observationElement);
        }
        private void SetNodeDescriptors(bool analysisHasElement, 
            ref XElement newObservationsElement)
        {
            string sUnitValue = GetUnitValue();
            if (!string.IsNullOrEmpty(sUnitValue))
            {
                newObservationsElement.SetAttributeValue(
                    DevTreksAppHelpers.General.UNIT, sUnitValue);
                if (analysisHasElement)
                {
                    SetDescriptor(DevTreksAppHelpers.General.UNIT, 
                        CostBenefitStatistic01.TRUnit, ref newObservationsElement);
                }
                else
                {
                    string sValueForUnit = string.Concat(sUnitValue,
                        Constants.FILENAME_DELIMITER,
                        this.ObsCalculatorParams.AnalyzerParms.FilePositionIndex.ToString());
                    newObservationsElement.SetAttributeValue(
                        CostBenefitStatistic01.TRUnit, sValueForUnit);
                }
            }
            string sNameValue = GetNameValue();
            if (!string.IsNullOrEmpty(sNameValue))
            {
                //don't change if the type or group names have already been set
                string sAggregatingName = CalculatorHelpers.GetAttribute(newObservationsElement, 
                    Calculator1.cName);
                if (string.IsNullOrEmpty(sAggregatingName))
                {
                    newObservationsElement.SetAttributeValue(
                        Calculator1.cName, sNameValue);
                }
                if (analysisHasElement)
                {
                    SetDescriptor(Calculator1.cName, CostBenefitStatistic01.TRName,
                        ref newObservationsElement);
                }
                else
                {
                    string sValueForName = string.Concat(sNameValue,
                        Constants.FILENAME_DELIMITER,
                        this.ObsCalculatorParams.AnalyzerParms.FilePositionIndex.ToString());
                    newObservationsElement.SetAttributeValue(
                        CostBenefitStatistic01.TRName, sValueForName);
                }
            }
            string sDate = GetDateValue(); 
            if (!string.IsNullOrEmpty(sDate))
            {
                newObservationsElement.SetAttributeValue(
                    Calculator1.cDate, sDate);
            }
            string sLabel2 = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                Calculator1.cLabel2);
            if (!string.IsNullOrEmpty(sLabel2))
            {
                newObservationsElement.SetAttributeValue(
                    Calculator1.cLabel2, sLabel2);
            }
        }
        private void SetAggregatorsAndName(bool analysisHasElement, 
            string attributeNameForName, ref XElement newObservationsElement, 
            out string analysisNodeValue)
        {
            //aggregators depend on ids
            newObservationsElement.SetAttributeValue(Calculator1.cId, 
                this.ObsCalculatorParams.AnalyzerParms.AggregatingId);
            //and labels
            string sLabelValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                Calculator1.cLabel);
            if (string.IsNullOrEmpty(sLabelValue)) sLabelValue
                = DevTreksHelpers.GeneralHelpers.NONE;
            newObservationsElement.SetAttributeValue(
                Calculator1.cLabel, sLabelValue);
            
            analysisNodeValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                attributeNameForName);
            if (string.IsNullOrEmpty(analysisNodeValue)) 
                analysisNodeValue = "NoName";
            newObservationsElement.SetAttributeValue(Calculator1.cName, 
                analysisNodeValue);
        }
        private string GetUnitValue()
        {
            string sUnitValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                DevTreksAppHelpers.General.UNIT);
            //if aohunit or capunit need to be displayed, this will be refactored
            if (string.IsNullOrEmpty(sUnitValue)) 
                sUnitValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                    DevTreksAppHelpers.Prices.OC_UNIT);
            if (string.IsNullOrEmpty(sUnitValue))
                sUnitValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                    DevTreksAppHelpers.Prices.OUTPUT_UNIT1);
            return sUnitValue;
        }
        private string GetNameValue()
        {
            string sNameValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                Calculator1.cName);
            return sNameValue;
        }
        private string GetDateValue()
        {
            string sDateValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                Calculator1.cDate);
            if (string.IsNullOrEmpty(sDateValue))
                sDateValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                    DevTreksAppHelpers.Prices.INPUT_DATE);
            if (string.IsNullOrEmpty(sDateValue))
                sDateValue = this.ObsCalculatorParams.DocToCalcReader.GetAttribute(
                    DevTreksAppHelpers.Prices.OUTPUT_DATE);
            return sDateValue;
        }
        private void SetFileCountAndObservationCount(ref XElement observationElement)
        {
            //Files attribute sets the column count, while Observations attribute
            //sets the statistic count
            if (observationElement.Attribute(CostBenefitStatistic01.FILES)
                == null)
            {
                observationElement.SetAttributeValue(CostBenefitStatistic01.FILES,
                    this.ObsCalculatorParams.AnalyzerParms.FilesComparisonsCount.ToString());
            }
            //the name attribute holds a delimited string holding the names
            //of each observation in this node
            //note that each aggregated statistic has an n statistic as well
            string sObservations = string.Empty;
            if (observationElement.Attribute(Calculator1.cDescription)
                != null)
            {
                sObservations = CalculatorHelpers.GetAttribute(
                    observationElement, Calculator1.cDescription);
                this.ObsCalculatorParams.AnalyzerParms.arrObservations
                    = sObservations.Split(Constants.STRING_DELIMITERS);
                if (this.ObsCalculatorParams.AnalyzerParms.arrObservations != null)
                {
                    sObservations 
                        = this.ObsCalculatorParams.AnalyzerParms.arrObservations.Length.ToString();
                }
            }
            if (string.IsNullOrEmpty(sObservations))
                //first observation
                sObservations = "1";
            observationElement.SetAttributeValue(CostBenefitStatistic01.OBSERVATIONS,
                sObservations);
        }
        private void SetDescriptor(string attNameToGet, string attNameToSet,
            ref XElement observationElement)
        {
            //descriptive name also identifies the file where the name came from
            string sName = this.ObsCalculatorParams.DocToCalcReader
                .GetAttribute(attNameToGet);
            if (!string.IsNullOrEmpty(sName))
            {
                string sDescription = attNameToSet;
                string sNewDescription = string.Empty;
                if (observationElement.Attribute(sDescription)
                    != null)
                {
                    //delimited array of names
                    StringBuilder oNewDescription = new StringBuilder();
                    oNewDescription.Append(CalculatorHelpers.GetAttribute(
                        observationElement, sDescription));
                    oNewDescription.Append(Constants.STRING_DELIMITER);
                    oNewDescription.Append(sName);
                    oNewDescription.Append(Constants.FILENAME_DELIMITER);
                    oNewDescription.Append(this.ObsCalculatorParams.AnalyzerParms.FilePositionIndex);
                    sNewDescription = oNewDescription.ToString();
                    oNewDescription = null;
                }
                observationElement.SetAttributeValue(sDescription, 
                    sNewDescription);
            }
            else
            {
                //older docs could be missing names
                sName = "legacy doc";
            }
        }
        
        private void AppendNewNode(XElement observationsRoot,
            XElement newObservationsElement)
        {
            string sGroupingNodeName = GetGroupingNodeName(
                this.ObsCalculatorParams.CurrentElementNodeName);
            bool bIsAppended = CalculatorHelpers.AppendElement(
                this.ObsCalculatorParams.ParentElementURIPattern,
                sGroupingNodeName, newObservationsElement, ref observationsRoot);
            if (!bIsAppended)
            {
                this.ObsCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("OBSERVATIONS_MISSING_AGGREGATOR");
            }
        }
        
        private static string GetGroupingNodeName(string currentNodeName)
        {
            string sGroupingNodeName = string.Empty;
            if (currentNodeName == BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString())
            {
                sGroupingNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString();
            }
            else if (currentNodeName == BudgetInvestment.INVESTMENT_TYPES.investmentoutcome.ToString())
            {
                sGroupingNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString();
            }
            else if (currentNodeName == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString())
            {
                sGroupingNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString();
            }
            else if (currentNodeName == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
            {
                sGroupingNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString();
            }
            return sGroupingNodeName;
        }
        private void SetStandardLinkedViewAttributes(XElement observationElement)
        {
            //use a calculator object to reset calculator attributes that may 
            //have been mistakenly set by subscriber
            CostBenefitCalculator cbCalculator = new CostBenefitCalculator();
            cbCalculator.InitCalculatorProperties();
            cbCalculator.SetCalculatorProperties(this.ObsCalculatorParams.LinkedViewElement);
            //not clear yet what these attributes need to do (i.e. row column text var?)
            observationElement.SetAttributeValue(
                Calculator1.cCalculatorId, cbCalculator.Id);
            observationElement.SetAttributeValue(
                Calculator1.cCalculatorType, cbCalculator.CalculatorType);
            observationElement.SetAttributeValue(
                Calculator1.cRelatedCalculatorsType, cbCalculator.RelatedCalculatorsType);
            observationElement.SetAttributeValue(
                Calculator1.cWhatIfTagName, cbCalculator.WhatIfTagName);
            cbCalculator = null;
        }
    }
}
