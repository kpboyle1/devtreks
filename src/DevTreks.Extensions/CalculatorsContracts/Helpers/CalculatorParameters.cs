using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

using DataAppHelpers = DevTreks.Data.AppHelpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Class for holding general calculator and analyzer parameters and another 
    ///             class for running extension methods on the calculator parameters
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class CalculatorParameters
    {
        public CalculatorParameters()
        {
            this.ExtensionCalcDocURI = null;
            this.ExtensionDocToCalcURI = null;
            this.NeedsCalculators = false;
            this.NeedsCurrentCalculation = false;
            this.NeedsFullView = false;
            this.NeedsXmlDocOnly = false;
            this.Overwrite = false;
            this.UseSameCalculator = false;
            this.RunCalculatorType = CalculatorHelpers.RUN_CALCULATOR_TYPES.none;
            this.Version = string.Empty;
            this.CalculatorType = string.Empty;
            this.WhatIfScenario = string.Empty;
            this.RelatedCalculatorType = string.Empty;
            this.RelatedCalculatorsType = string.Empty;
            this.FileExtensionType = string.Empty;
            this.Stylesheet2Name = string.Empty;
            this.Stylesheet2ObjectNS = string.Empty;
            this.StepNumber = string.Empty;
            this.ErrorMessage = string.Empty;
            this.SubApplicationType = Constants.SUBAPPLICATION_TYPES.none;
            this.CurrentElement = null;
            this.LinkedViewElement = null;
            this.Updates = null;
            this.StartingDocToCalcNodeName = string.Empty;
            this.CalcDocId = string.Empty;
            this.DocToCalcNodeName = string.Empty;
            this.DocToCalcReplacementFile = string.Empty;
            this.IsCustomDoc = false;
            this.AttributeNeedsDbUpdate = false;
            this.MathResult = string.Empty;
            this.ParentElementNodeName = string.Empty;
            this.ParentElementURIPattern = string.Empty;
            this.CurrentElementNodeName = string.Empty;
            this.CurrentElementURIPattern = string.Empty;
            this.RndGenerator = null;
            this.AnalyzerParms = new AnalyzerParameters();
        }
        //properties 
        public ExtensionContentURI ExtensionCalcDocURI { get; set; }
        public ExtensionContentURI ExtensionDocToCalcURI { get; set; }

        public bool NeedsCalculators { get; set; }
        public bool NeedsCurrentCalculation { get; set; }
        //display summary view or full view?
        public bool NeedsFullView { get; set; }
        public bool NeedsXmlDocOnly { get; set; }
        //version 180 considers it better to decide on updatedb based only on these params
        //if Overwrite = true and UseSameCalculator and IsSelforDescendant. bNeedsDbUpdate = true
        public bool Overwrite { get; set; }
        public bool UseSameCalculator { get; set; }
        public XElement CurrentElement { get; set; }
        public XElement LinkedViewElement { get; set; }
        public IDictionary<string, string> Updates { get; set; }
        public IList<string> UrisToAnalyze { get; set; }
        public XmlReader DocToCalcReader { get; set; }
        public Constants.SUBAPPLICATION_TYPES SubApplicationType { get; set; }
        public CalculatorHelpers.RUN_CALCULATOR_TYPES RunCalculatorType { get; set; }
        public string Version { get; set; }
        public string CalculatorType { get; set; }
        public string WhatIfScenario { get; set; }
        public string RelatedCalculatorType { get; set; }
        public string RelatedCalculatorsType { get; set; }
        public string FileExtensionType { get; set; }
        public string Stylesheet2Name { get; set; }
        public string Stylesheet2ObjectNS { get; set; }
        public string StepNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string StartingDocToCalcNodeName { get; set; }
        public string CalcDocId { get; set; }
        public string DocToCalcNodeName { get; set; }
        public string DocToCalcReplacementFile { get; set; }
        public string ParentElementNodeName { get; set; }
        public string ParentElementURIPattern { get; set; }
        public string CurrentElementNodeName { get; set; }
        public string CurrentElementURIPattern { get; set; }
        public bool IsCustomDoc { get; set; }
        public bool AttributeNeedsDbUpdate { get; set; }
        ////188 stores mathresult in calcs and transfer to lv after all calcs run
        public string MathResult { get; set; }
        //random number generator 
        public Random RndGenerator { get; set; }
        //2.0.0 refactor
        //public ExtensionContentURI DocToCalcURI { get; set; }
        //public ExtensionContentURI CalcDocURI { get; set; }
        //properties from these objects are used to run calcs on descendants
        //and to accumulate totals
        public BudgetInvestmentGroup ParentBudgetInvestmentGroup { get; set; }
        public BudgetInvestment ParentBudgetInvestment { get; set; }
        public TimePeriod ParentTimePeriod { get; set; }
        public OperationComponent ParentOperationComponent { get; set; }
        public Outcome ParentOutcome { get; set; }
        public Output ParentOutput { get; set; }
        public Input ParentInput { get; set; }
        //version 1.4.5 moved analyzerparameters into this class
        public AnalyzerParameters AnalyzerParms { get; set; }

        public CalculatorParameters(ExtensionContentURI extDocToCalcURI,
            ExtensionContentURI extCalcDocURI, string stepNumber, 
            IList<string> urisToAnalyze, IDictionary<string, string> updates,
            XElement linkedViewElement)
        {
            //standard addin/extension parameters
            this.ExtensionDocToCalcURI = extDocToCalcURI;
            this.ExtensionCalcDocURI = extCalcDocURI;
            this.NeedsCalculators
                = CalculatorHelpers.NeedsAncestorCalculator(linkedViewElement);
            this.NeedsCurrentCalculation = false;
            //note that analyzers can set this bool
            this.NeedsFullView = false;
            this.NeedsXmlDocOnly = false;
            this.Overwrite = CalculatorHelpers.GetOverwrite(linkedViewElement);
            this.UseSameCalculator = CalculatorHelpers.GetUseSameCalculator(linkedViewElement);
            this.RunCalculatorType = CalculatorHelpers.RUN_CALCULATOR_TYPES.none;
            this.Version = CalculatorHelpers.GetVersion(linkedViewElement);
            this.CalculatorType = CalculatorHelpers.GetCalculatorType(linkedViewElement);
            this.WhatIfScenario = CalculatorHelpers.GetWhatIfScenario(linkedViewElement);
            this.RelatedCalculatorType = CalculatorHelpers.GetRelatedCalculatorType(linkedViewElement);
            this.RelatedCalculatorsType = CalculatorHelpers.GetRelatedCalculatorsType(linkedViewElement);
            this.FileExtensionType = this.CalculatorType.ToString();
            this.Stylesheet2Name = CalculatorHelpers.GetStylesheet2Name(linkedViewElement);
            this.Stylesheet2ObjectNS = CalculatorHelpers.GetStylesheet2ObjNS(linkedViewElement);
            this.StepNumber = stepNumber;
            this.ErrorMessage = extDocToCalcURI.ErrorMessage;
            this.SubApplicationType 
                = CalculatorHelpers.ConvertSubAppType(
                extDocToCalcURI.URIDataManager.SubAppType);
            if (linkedViewElement != null)
            {
                this.LinkedViewElement = new XElement(linkedViewElement);
            }
            this.UrisToAnalyze = urisToAnalyze;
            this.Updates = updates;

            //parameters used to manage addin state or run calculations
            this.StartingDocToCalcNodeName 
                = CalculatorHelpers.GetURIPatternNodeName(
                extDocToCalcURI.URIDataManager.StartingDocToCalcURIPattern);
            //parameter needed to find correct calculator in xmldocs
            this.CalcDocId = CalculatorHelpers.GetURIPatternId(extCalcDocURI.URIPattern);
            this.DocToCalcNodeName = CalculatorHelpers.GetURIPatternNodeName(extDocToCalcURI.URIPattern);
            string sFileExtensionType = CalculatorHelpers.GetURIPatternFileExtensionType(extDocToCalcURI.URIPattern);
            if (this.DocToCalcNodeName.StartsWith(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                || this.DocToCalcNodeName.StartsWith(DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                || sFileExtensionType == Data.Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //note that customdocs usually use StartingDocToCalc params rather than 
                //doctocalc and init with 'group' nodes (can't tell which node to start
                //so start at top of document)
                this.IsCustomDoc = true;
                //only the xmldoc and xmldocids get db updates for atts that have changed
                this.AttributeNeedsDbUpdate = false;
            }
            else
            {
                this.AttributeNeedsDbUpdate = true;
            }
            this.MathResult = string.Empty;
            this.ParentElementNodeName = string.Empty;
            this.ParentElementURIPattern = string.Empty;
            this.CurrentElementNodeName = string.Empty;
            this.CurrentElementURIPattern = string.Empty;
            this.RndGenerator = null;
            this.AnalyzerParms = new AnalyzerParameters();
        }
        //copy constructor (don't new any of the object copies; kiss because calcparams mostly need copies)
        public CalculatorParameters(CalculatorParameters calcParameters)
        {
            this.ExtensionDocToCalcURI = calcParameters.ExtensionDocToCalcURI;
            this.ExtensionCalcDocURI = calcParameters.ExtensionCalcDocURI;
            this.NeedsCalculators = calcParameters.NeedsCalculators;
            this.NeedsCurrentCalculation = calcParameters.NeedsCurrentCalculation;
            this.NeedsFullView = calcParameters.NeedsFullView;
            this.NeedsXmlDocOnly = calcParameters.NeedsXmlDocOnly;
            this.Overwrite = calcParameters.Overwrite;
            this.UseSameCalculator = calcParameters.UseSameCalculator;
            this.RunCalculatorType = calcParameters.RunCalculatorType;
            this.Version = calcParameters.Version;
            this.CalculatorType = calcParameters.CalculatorType;
            this.WhatIfScenario = calcParameters.WhatIfScenario;
            this.RelatedCalculatorType = calcParameters.RelatedCalculatorType;
            this.RelatedCalculatorsType = calcParameters.RelatedCalculatorsType;
            this.FileExtensionType = calcParameters.FileExtensionType;
            this.Stylesheet2Name = calcParameters.Stylesheet2Name;
            this.Stylesheet2ObjectNS = calcParameters.Stylesheet2ObjectNS;
            this.StepNumber = calcParameters.StepNumber;
            this.ErrorMessage = calcParameters.ErrorMessage;
            this.SubApplicationType = calcParameters.SubApplicationType;
            this.CurrentElement = calcParameters.CurrentElement;
            this.LinkedViewElement = calcParameters.LinkedViewElement;
            this.UrisToAnalyze = calcParameters.UrisToAnalyze;
            this.Updates = calcParameters.Updates;
            this.StartingDocToCalcNodeName = calcParameters.StartingDocToCalcNodeName;
            this.CalcDocId = calcParameters.CalcDocId;
            this.DocToCalcNodeName = calcParameters.DocToCalcNodeName;
            this.DocToCalcReplacementFile = calcParameters.DocToCalcReplacementFile;
            this.CurrentElementNodeName = calcParameters.CurrentElementNodeName;
            this.CurrentElementURIPattern = calcParameters.CurrentElementURIPattern;
            this.ParentElementNodeName = calcParameters.ParentElementNodeName;
            this.ParentElementURIPattern = calcParameters.ParentElementURIPattern;
            this.IsCustomDoc = calcParameters.IsCustomDoc;
            this.AttributeNeedsDbUpdate = calcParameters.AttributeNeedsDbUpdate;
            this.MathResult = calcParameters.MathResult;
            if (calcParameters.AnalyzerParms == null)
                calcParameters.AnalyzerParms = new AnalyzerParameters();
            this.AnalyzerParms = new AnalyzerParameters(calcParameters.AnalyzerParms);
        }
        // <summary>
        ///Purpose:		Class for holding general analyzer parameters 
        ///             and another class for running extensions on those params
        ///Author:		www.devtreks.org
        ///Date:		2013, November
        ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
        ///Notes        1. The cost and benefit collections use two indexers
        ///             to identify collection members: fileposition and nodeposition. 
        ///             In analyzers, the file position corresponds to a real data file, 
        ///             while the node position corresponds to an object holding 
        ///             statistical values. Simple basic statistical nodes may 
        ///             have only one object in the collection, holding mean 
        ///             and standard deviation statistics. Analyzers such as
        ///             effectiveness hold either a collection of outputs or 
        ///             inputs in the collection. 
        ///</summary>            
        public class AnalyzerParameters
        {
            //constructors
            public AnalyzerParameters()
            {
                this.ComparisonType = AnalyzerHelper.COMPARISON_OPTIONS.none;
                this.AggregationType = AnalyzerHelper.AGGREGATION_OPTIONS.none;
                this.SubFolderType = AnalyzerHelper.SUBFOLDER_OPTIONS.no;
                this.AnalyzerGeneralType = AnalyzerHelper.ANALYZER_GENERAL_TYPES.none;
                this.AnalyzerType = string.Empty;
                this.FilesToAnalyzeExtensionType = string.Empty;
                this.BaseCBStatistic = new CostBenefitStatistic01();
                this.FilesComparisonsCount = 0;
                this.FilePositionIndex = 0;
                this.BenefitObjectsCount = 0;
                this.CostObjectsCount = 0;
                this.TimePeriodObjectsCount = 0;
                this.BudgetObjectsCount = 0;
                this.NodeName = string.Empty;
                this.NodePositionIndex = 0;
                this.IsTechnologyAnalysis = false;
                this.AggregatingId = string.Empty;
                this.AggregatingOldId = string.Empty;
                //this.ErrorMessage = string.Empty;
                //this.RndGenerator = null;
                this.Ancestors = new Dictionary<string, string>();
                this.FileOrFolderPaths = new Dictionary<string, string>();
                //this.SubAppType = Constants.SUBAPPLICATION_TYPES.none;
                this.ObservationElement = null;
                this.ObservationFile = string.Empty;
                this.ObservationsPath = string.Empty;
                this.BenefitStatistics = null;
                this.CostStatistics = null;
            }
            public AnalyzerParameters(
                AnalyzerHelper.ANALYZER_GENERAL_TYPES analyzerType,
                AnalyzerHelper.COMPARISON_OPTIONS comparisonType,
                AnalyzerHelper.AGGREGATION_OPTIONS aggregationType,
                AnalyzerHelper.SUBFOLDER_OPTIONS subFolderType)
            {
                this.ComparisonType = comparisonType;
                this.AggregationType = aggregationType;
                this.SubFolderType = subFolderType;
                this.AnalyzerGeneralType = analyzerType;
                this.AnalyzerType = string.Empty;
                this.FilesToAnalyzeExtensionType = string.Empty;
                this.BaseCBStatistic = new CostBenefitStatistic01();
                this.FilesComparisonsCount = 0;
                this.FilePositionIndex = 0;
                this.BenefitObjectsCount = 0;
                this.CostObjectsCount = 0;
                this.TimePeriodObjectsCount = 0;
                this.BudgetObjectsCount = 0;
                this.NodeName = string.Empty;
                this.NodePositionIndex = 0;
                this.IsTechnologyAnalysis = false;
                this.AggregatingId = string.Empty;
                this.AggregatingOldId = string.Empty;
                //this.ErrorMessage = string.Empty;
                //this.RndGenerator = null;
                this.Ancestors = new Dictionary<string, string>();
                this.FileOrFolderPaths = new Dictionary<string, string>();
                //this.SubAppType = Constants.SUBAPPLICATION_TYPES.none;
                this.ObservationElement = null;
                this.ObservationFile = string.Empty;
                this.ObservationsPath = string.Empty;
                this.BenefitStatistics = null;
                this.CostStatistics = null;
            }
            public AnalyzerParameters(AnalyzerParameters analysisParameters)
            {
                this.ComparisonType = analysisParameters.ComparisonType;
                this.AggregationType = analysisParameters.AggregationType;
                this.SubFolderType = analysisParameters.SubFolderType;
                this.AnalyzerGeneralType = analysisParameters.AnalyzerGeneralType;
                this.AnalyzerType = analysisParameters.AnalyzerType;
                this.FilesToAnalyzeExtensionType = analysisParameters.FilesToAnalyzeExtensionType;
                this.BaseCBStatistic = new CostBenefitStatistic01(analysisParameters.BaseCBStatistic);
                this.NodeName = analysisParameters.NodeName;
                this.NodePositionIndex = analysisParameters.NodePositionIndex;
                this.FilesComparisonsCount = analysisParameters.FilesComparisonsCount;
                this.FilePositionIndex = analysisParameters.FilePositionIndex;
                this.BenefitObjectsCount = analysisParameters.BenefitObjectsCount;
                this.CostObjectsCount = analysisParameters.CostObjectsCount;
                this.TimePeriodObjectsCount = analysisParameters.TimePeriodObjectsCount;
                this.BudgetObjectsCount = analysisParameters.BudgetObjectsCount;
                this.IsTechnologyAnalysis = analysisParameters.IsTechnologyAnalysis;
                this.AggregatingId = analysisParameters.AggregatingId;
                this.AggregatingOldId = analysisParameters.AggregatingOldId;
                //this.ErrorMessage = analysisParameters.ErrorMessage;
                //this.RndGenerator = analysisParameters.RndGenerator;
                this.Ancestors = CalculatorHelpers.CopyDictionary(analysisParameters.Ancestors);
                this.FileOrFolderPaths = CalculatorHelpers.CopyDictionary(analysisParameters.FileOrFolderPaths);
                //this.SubAppType = analysisParameters.SubAppType;
                if (analysisParameters.ObservationElement != null)
                    this.ObservationElement = new XElement(analysisParameters.ObservationElement);
                this.ObservationFile = analysisParameters.ObservationFile;
                this.ObservationsPath = analysisParameters.ObservationsPath;
                //copy the collections
                this.BenefitStatistics = CopyStatistics(analysisParameters.BenefitStatistics);
                this.CostStatistics = CopyStatistics(analysisParameters.CostStatistics);
            }

            //properties
            public AnalyzerHelper.COMPARISON_OPTIONS ComparisonType { get; set; }
            public AnalyzerHelper.AGGREGATION_OPTIONS AggregationType { get; set; }
            public AnalyzerHelper.SUBFOLDER_OPTIONS SubFolderType { get; set; }
            public AnalyzerHelper.ANALYZER_GENERAL_TYPES AnalyzerGeneralType { get; set; }
            public string AnalyzerType { get; set; }
            public string FilesToAnalyzeExtensionType { get; set; }
            public CostBenefitStatistic01 BaseCBStatistic { get; set; }
            public int FilesComparisonsCount { get; set; }
            public int FilePositionIndex { get; set; }
            public int BenefitObjectsCount { get; set; }
            public int CostObjectsCount { get; set; }
            public int TimePeriodObjectsCount { get; set; }
            public int BudgetObjectsCount { get; set; }
            public string NodeName { get; set; }
            public int NodePositionIndex { get; set; }
            public bool IsTechnologyAnalysis { get; set; }
            public string AggregatingId { get; set; }
            public string AggregatingOldId { get; set; }
            //public string ErrorMessage { get; set; }
            //public Random RndGenerator { get; set; }
            //key = nodename, value = uripattern
            public IDictionary<string, string> Ancestors { get; set; }
            public IDictionary<string, string> FileOrFolderPaths { get; set; }
            //public Constants.SUBAPPLICATION_TYPES SubAppType { get; set; }
            //running stats
            public string ObservationAttributeName { get; set; }
            public string ObservationAttributeValue { get; set; }
            public string[] arrObservations = { };
            //refrain from using this property to run analyses (object programming 
            //is generally favored over attribute programming)
            public XElement ObservationElement { get; set; }
            //individual file being added to observations document
            public string ObservationFile { get; set; }
            //observations document
            public string ObservationsPath { get; set; }
            //benefits collection
            //int = file number, basestat position in list = basestat number
            //i.e. output 1 has a zero index position, output 2 a one index ...
            public IDictionary<int, List<CostBenefitStatistic01>> BenefitStatistics = null;
            //costs collection
            //i.e. component 1 has a zero index position, component 2 a one index ...
            public IDictionary<int, List<CostBenefitStatistic01>> CostStatistics = null;

            //methods
            public static IDictionary<int, List<CostBenefitStatistic01>> CopyStatistics(
                IDictionary<int, List<CostBenefitStatistic01>> list1)
            {
                IDictionary<int, List<CostBenefitStatistic01>> list2
                    = new Dictionary<int, List<CostBenefitStatistic01>>();
                if (list1 != null)
                {
                    foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                        in list1)
                    {
                        List<CostBenefitStatistic01> stats = new List<CostBenefitStatistic01>();
                        if (kvp.Value != null)
                        {
                            foreach (CostBenefitStatistic01 stat in kvp.Value)
                            {
                                stats.Add(stat);
                            }

                        }
                        list2.Add(kvp.Key, stats);
                    }
                }
                return list2;
            }
        }
        
    }
   
    //extension methods to CalculatorParameters()
    public static class CalculatorParametersExtensions
    {
        public static void ChangeStartingParams(
            this CalculatorParameters calcParams, 
            string docToCalcNodeName)
        {
            if (calcParams.IsCustomDoc)
            {
                //some calculators (i.e. stand alone) don't init from within a document
                //and don't have a startingdoctocalcuripattern; 
                //they use docToCalcGroupNodeName instead
                calcParams.StartingDocToCalcNodeName = docToCalcNodeName;
                string sId = calcParams.DocToCalcReader.GetAttribute(Calculator1.cId);
                if (!string.IsNullOrEmpty(sId))
                {
                    calcParams.ExtensionDocToCalcURI.URIDataManager.StartingDocToCalcURIPattern 
                        = CalculatorHelpers.GetNewURIPattern(
                        calcParams.ExtensionDocToCalcURI.URIPattern, sId, 
                        docToCalcNodeName);
                }
            }
            calcParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(calcParams.StartingDocToCalcNodeName);
        }
        public static XElement GetCurrentElement(
            this CalculatorParameters calcParameters)
        {
            XElement currentElement = null;
            string sDocToCalcNodeId = CalculatorHelpers
                .GetURIPatternId(calcParameters.ExtensionDocToCalcURI.URIPattern);
            //use streaming techniques to reduce memory footprint
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ConformanceLevel = ConformanceLevel.Document;
            string sId = string.Empty;
            XmlReader docToCalcReader = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                calcParameters.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (docToCalcReader != null)
            {
                using (docToCalcReader)
                {
                    bool bHasIdMatch = false;
                    docToCalcReader.MoveToContent();
                    //0.8.8: new way to read
                    while (docToCalcReader.Read())
                    {
                        if (docToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                            if (docToCalcReader.Name
                                == calcParameters.DocToCalcNodeName)
                            {
                                sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                docToCalcReader.MoveToElement();
                                if (sId != sDocToCalcNodeId)
                                {
                                    while (docToCalcReader.ReadToNextSibling(calcParameters.DocToCalcNodeName))
                                    {
                                        sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                        docToCalcReader.MoveToElement();
                                        if (sId == sDocToCalcNodeId)
                                        {
                                            bHasIdMatch = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    bHasIdMatch = true;
                                }
                            }
                        }
                        if (bHasIdMatch)
                        {
                            currentElement
                                = CalculatorHelpers.GetCurrentElementWithAttributes(docToCalcReader);
                            //see if an xmldoc can be added
                            docToCalcReader.Read();
                            if (docToCalcReader.Name == Constants.ROOT_PATH)
                            {
                                //attach calculators to parents
                                XElement xmlDocElement = XElement.Load(docToCalcReader.ReadSubtree());
                                currentElement.Add(xmlDocElement);
                            }
                            break;
                        }
                    }
                }

            }
            return currentElement;
        }
        //xstreaming wasn't used for full doctocalc calcs because 
        //the XStreaming XName constructor for descendants resulted in extra root nodes
        public static XElement GetTimePeriodsForBudgetInvestmentNode(
            this CalculatorParameters calcParameters, XElement budgetOrInvestmentNode)
        {
            XElement rootTimePeriods = null;
            string sId = string.Empty;
            int iBudgetInvestmentId = CalculatorHelpers.GetAttributeInt(
                budgetOrInvestmentNode, Calculator1.cId);
            XmlReader docToCalcReader = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                calcParameters.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (docToCalcReader != null)
            {
                using (docToCalcReader)
                {
                    bool bHasIdMatch = false;
                    docToCalcReader.MoveToContent();
                    //0.8.8: new way to read
                    while (docToCalcReader.Read())
                    {
                        if (docToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                            if (docToCalcReader.Name
                                == budgetOrInvestmentNode.Name.LocalName)
                            {
                                sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                docToCalcReader.MoveToElement();
                                if (sId != iBudgetInvestmentId.ToString())
                                {
                                    while (docToCalcReader.ReadToNextSibling(budgetOrInvestmentNode.Name.LocalName))
                                    {
                                        sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                        docToCalcReader.MoveToElement();
                                        if (sId == iBudgetInvestmentId.ToString())
                                        {
                                            bHasIdMatch = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    bHasIdMatch = true;
                                }
                            }
                        }
                        if (bHasIdMatch)
                        {
                            //use streaming techniques to process descendant nodes
                            //returns children timeperiod elements of this budget or investment node
                            //i.e. <root><timeperiod id='1' .../><timeperiod id='2' .../></root>
                            XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                                from timePeriodElement
                                in StreamBudgetInvestmentTimePeriod(docToCalcReader)
                                select timePeriodElement
                            );
                            //process the stream
                            rootTimePeriods = XElement.Parse(root.ToString());
                            break;
                        }
                    }
                }
            }
            return rootTimePeriods;
        }
        public static IEnumerable<XElement> StreamBudgetInvestmentTimePeriod(
            XmlReader docToCalcReader)
        {
            //loop through the descendants in document order
            while (docToCalcReader.Read())
            {
                if (docToCalcReader.NodeType == XmlNodeType.Element)
                {
                    if (docToCalcReader.Name 
                        == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                        || docToCalcReader.Name 
                        == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(docToCalcReader);
                        //don't process previously calculated nodes
                        bool bIsAnnuity = TimePeriod.IsAnnuity(currentElement);
                        if (!bIsAnnuity)
                        {
                            //set this.calcparams.xmldoc for attached calculators
                            AddChildXmlDoc(docToCalcReader, ref currentElement);
                            if (currentElement != null)
                            {
                                yield return currentElement;
                            }
                        }
                    }
                }
                else if (docToCalcReader.NodeType == XmlNodeType.EndElement)
                {
                    if (docToCalcReader.Name 
                        == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                        || docToCalcReader.Name 
                        == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private static void AddChildXmlDoc(XmlReader docToCalcReader,
            ref XElement currentElement)
        {
            //read the next descendant node (xmldocs are always stored as first child)
            docToCalcReader.Read();
            if (docToCalcReader.NodeType == XmlNodeType.Element)
            {
                if (docToCalcReader.Name == Constants.ROOT_PATH)
                {
                    //attach calculators to parents
                    currentElement.AddFirst(XElement.Load(docToCalcReader.ReadSubtree()));
                }
            }
        }
        //not necessary is annuities used to add outcomes to parentimeperiod
        //but keep for reference
        public static XElement GetOutcomeForTimeNode(
            this CalculatorParameters calcParameters, XElement timePeriodNode)
        {
            XElement rootOutcome = null;
            string sId = string.Empty;
            int iTimePeriodId = CalculatorHelpers.GetAttributeInt(
                timePeriodNode, Calculator1.cId);
             XmlReader docToCalcReader = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                calcParameters.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
             if (docToCalcReader != null)
             {
                 using (docToCalcReader)
                 {
                     bool bHasIdMatch = false;
                     docToCalcReader.MoveToContent();
                     while (docToCalcReader.Read())
                     {
                         if (docToCalcReader.NodeType
                             == XmlNodeType.Element)
                         {
                             if (docToCalcReader.Name
                                 == timePeriodNode.Name.LocalName)
                             {
                                 sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                 docToCalcReader.MoveToElement();
                                 if (sId != iTimePeriodId.ToString())
                                 {
                                     while (docToCalcReader.ReadToNextSibling(timePeriodNode.Name.LocalName))
                                     {
                                         sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                         docToCalcReader.MoveToElement();
                                         if (sId == iTimePeriodId.ToString())
                                         {
                                             bHasIdMatch = true;
                                             break;
                                         }
                                     }
                                 }
                                 else
                                 {
                                     bHasIdMatch = true;
                                 }
                             }
                         }
                         if (bHasIdMatch)
                         {
                             //use streaming techniques to process descendant nodes
                             //returns children output elements of this outcome node
                             //i.e. <root><output id='1' .../><output id='2' .../></root>
                             XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                                from timePeriodElement
                                    in StreamTimePeriodOutcome(docToCalcReader)
                                select timePeriodElement
                             );
                             //process the stream
                             rootOutcome = XElement.Parse(root.ToString());
                             break;
                         }
                     }
                 }
             }
            return rootOutcome;
        }
        public static XElement GetInputForOpOrCompNode(
            this CalculatorParameters calcParameters, XElement operationOrComponentNode)
        {
            XElement rootInput = null;
            string sId = string.Empty;
            int iOperationComponentId = CalculatorHelpers.GetAttributeInt(
                operationOrComponentNode, Calculator1.cId);
             XmlReader docToCalcReader = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                    calcParameters.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                    calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
             if (docToCalcReader != null)
             {
                 using (docToCalcReader)
                 {
                     bool bHasIdMatch = false;
                     docToCalcReader.MoveToContent();
                     //0.8.8: new way to read
                     while (docToCalcReader.Read())
                     {
                         if (docToCalcReader.NodeType
                             == XmlNodeType.Element)
                         {
                             if (docToCalcReader.Name
                                 == operationOrComponentNode.Name.LocalName)
                             {
                                 sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                 docToCalcReader.MoveToElement();
                                 if (sId != iOperationComponentId.ToString())
                                 {
                                     while (docToCalcReader.ReadToNextSibling(operationOrComponentNode.Name.LocalName))
                                     {
                                         sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                         docToCalcReader.MoveToElement();
                                         if (sId == iOperationComponentId.ToString())
                                         {
                                             bHasIdMatch = true;
                                             break;
                                         }
                                     }
                                 }
                                 else
                                 {
                                     bHasIdMatch = true;
                                 }
                             }
                         }
                         if (bHasIdMatch)
                         {
                             //use streaming techniques to process descendant nodes
                             //returns children input elements of this operation or component node
                             //i.e. <root><input id='1' .../><input id='2' .../></root>
                             XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                                from operationComponentElement
                                    in StreamOperationComponentInput(docToCalcReader)
                                select operationComponentElement
                             );
                             //process the stream
                             rootInput = XElement.Parse(root.ToString());
                             break;
                         }
                     }
                 }
             }
            return rootInput;
        }
        public static XElement GetOutputForOutcomeNode(
           this CalculatorParameters calcParameters, XElement outcomeNode)
        {
            XElement rootOutput = null;
            string sId = string.Empty;
            int iOutcomeId = CalculatorHelpers.GetAttributeInt(
                outcomeNode, Calculator1.cId);
             XmlReader docToCalcReader = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                    calcParameters.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                    calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
             if (docToCalcReader != null)
             {
                 using (docToCalcReader)
                 {
                     bool bHasIdMatch = false;
                     docToCalcReader.MoveToContent();
                     while (docToCalcReader.Read())
                     {
                         if (docToCalcReader.NodeType
                             == XmlNodeType.Element)
                         {
                             if (docToCalcReader.Name
                                 == outcomeNode.Name.LocalName)
                             {
                                 sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                 docToCalcReader.MoveToElement();
                                 if (sId != iOutcomeId.ToString())
                                 {
                                     while (docToCalcReader.ReadToNextSibling(outcomeNode.Name.LocalName))
                                     {
                                         sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                         docToCalcReader.MoveToElement();
                                         if (sId == iOutcomeId.ToString())
                                         {
                                             bHasIdMatch = true;
                                             break;
                                         }
                                     }
                                 }
                                 else
                                 {
                                     bHasIdMatch = true;
                                 }
                             }
                         }
                         if (bHasIdMatch)
                         {
                             //use streaming techniques to process descendant nodes
                             //returns children output elements of this outcome node
                             //i.e. <root><output id='1' .../><output id='2' .../></root>
                             XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                                from outcomeElement
                                    in StreamOutcomeOutput(docToCalcReader)
                                select outcomeElement
                             );
                             //process the stream
                             rootOutput = XElement.Parse(root.ToString());
                             break;
                         }
                     }
                 }
             }
            return rootOutput;
        }
        public static IEnumerable<XElement> StreamTimePeriodOutcome(
            XmlReader docToCalcReader)
        {
            //loop through the descendants in document order
            while (docToCalcReader.Read())
            {
                if (docToCalcReader.NodeType == XmlNodeType.Element)
                {
                    if (docToCalcReader.Name.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        //process input and any descendant xmldocs
                        XElement currentElement
                            = XElement.Load(docToCalcReader.ReadSubtree());
                        if (currentElement != null)
                        {
                            yield return currentElement;
                        }
                    }
                }
                else if (docToCalcReader.NodeType == XmlNodeType.EndElement)
                {
                    if (docToCalcReader.Name.EndsWith(BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString())
                        || docToCalcReader.Name.EndsWith(BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString()))
                    {
                        break;
                    }
                }
            }
        }
        public static IEnumerable<XElement> StreamOutcomeOutput(
            XmlReader docToCalcReader)
        {
            //loop through the descendants in document order
            while (docToCalcReader.Read())
            {
                if (docToCalcReader.NodeType == XmlNodeType.Element)
                {
                    if (docToCalcReader.Name.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        //process input and any descendant xmldocs
                        XElement currentElement
                            = XElement.Load(docToCalcReader.ReadSubtree());
                        if (currentElement != null)
                        {
                            yield return currentElement;
                        }
                    }
                }
                else if (docToCalcReader.NodeType == XmlNodeType.EndElement)
                {
                    if (docToCalcReader.Name.EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
                    {
                        break;
                    }
                }
            }
        }
        public static IEnumerable<XElement> StreamOperationComponentInput(
            XmlReader docToCalcReader)
        {
            //loop through the descendants in document order
            while (docToCalcReader.Read())
            {
                if (docToCalcReader.NodeType == XmlNodeType.Element)
                {
                    if (docToCalcReader.Name.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        //process input and any descendant xmldocs
                        XElement currentElement 
                            = XElement.Load(docToCalcReader.ReadSubtree());
                        if (currentElement != null)
                        {
                            yield return currentElement;
                        }
                    }
                }
                else if (docToCalcReader.NodeType == XmlNodeType.EndElement)
                {
                    if (docToCalcReader.Name.EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                        || docToCalcReader.Name.EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
                    {
                        break;
                    }
                }
            }
        }
        public static XElement GetSeriesForInputOrOutputNode(
            this CalculatorParameters calcParameters, XElement outputOrInputNode)
        {
            XElement rootInput = null;
            string sId = string.Empty;
            int iOutputOrInputId = CalculatorHelpers.GetAttributeInt(
                outputOrInputNode, Calculator1.cId);
             XmlReader docToCalcReader = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                    calcParameters.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                    calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
             if (docToCalcReader != null)
             {
                 using (docToCalcReader)
                 {
                     bool bHasIdMatch = false;
                     docToCalcReader.MoveToContent();
                     //0.8.8: new way to read
                     while (docToCalcReader.Read())
                     {
                         if (docToCalcReader.NodeType
                             == XmlNodeType.Element)
                         {
                             if (docToCalcReader.Name
                                 == outputOrInputNode.Name.LocalName)
                             {
                                 sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                 docToCalcReader.MoveToElement();
                                 if (sId != iOutputOrInputId.ToString())
                                 {
                                     while (docToCalcReader.ReadToNextSibling(outputOrInputNode.Name.LocalName))
                                     {
                                         sId = docToCalcReader.GetAttribute(Calculator1.cId);
                                         docToCalcReader.MoveToElement();
                                         if (sId == iOutputOrInputId.ToString())
                                         {
                                             bHasIdMatch = true;
                                             break;
                                         }
                                     }
                                 }
                                 else
                                 {
                                     bHasIdMatch = true;
                                 }
                             }
                         }
                         if (bHasIdMatch)
                         {
                             //use streaming techniques to process descendant nodes
                             //returns children input elements of this operation or component node
                             //i.e. <root><input id='1' .../><input id='2' .../></root>
                             XStreamingElement root = new XStreamingElement(Constants.ROOT_PATH,
                                from timePeriodElement
                                    in StreamInputOrOutput(docToCalcReader)
                                select timePeriodElement
                             );
                             //process the stream
                             rootInput = XElement.Parse(root.ToString());
                             break;
                         }
                     }
                 }
             }
            return rootInput;
        }
        public static IEnumerable<XElement> StreamInputOrOutput(
            XmlReader docToCalcReader)
        {
            //loop through the descendants in document order
            while (docToCalcReader.Read())
            {
                if (docToCalcReader.NodeType == XmlNodeType.Element)
                {
                    if (docToCalcReader.Name
                        == Input.INPUT_PRICE_TYPES.inputseries.ToString()
                        || docToCalcReader.Name
                        == Output.OUTPUT_PRICE_TYPES.outputseries.ToString())
                    {
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(docToCalcReader);
                        //set this.calcparams.xmldoc for attached calculators
                        AddChildXmlDoc(docToCalcReader, ref currentElement);
                        if (currentElement != null)
                        {
                            yield return currentElement;
                        }
                    }
                }
                else if (docToCalcReader.NodeType == XmlNodeType.EndElement)
                {
                    if (docToCalcReader.Name 
                        == Input.INPUT_PRICE_TYPES.input.ToString()
                        || docToCalcReader.Name 
                        == Output.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        break;
                    }
                }
            }
        }
        //version 1.4.5 formerly part of analyzer extension class
        public static string GetParentAncestorURIPattern(this CalculatorParameters calcParameters,
            string aggregatingKey)
        {
            string sParentURIPattern = string.Empty;
            if (calcParameters.AnalyzerParms.Ancestors != null)
            {
                if (calcParameters.AnalyzerParms.Ancestors.ContainsKey(aggregatingKey))
                {
                    foreach (KeyValuePair<string, string> kvp in
                        calcParameters.AnalyzerParms.Ancestors)
                    {
                        if (kvp.Key == aggregatingKey)
                        {
                            break;
                        }
                        sParentURIPattern = kvp.Value;
                    }
                }
            }
            return sParentURIPattern;
        }
        public static void SetFileExtensionType(this CalculatorParameters calcParameters)
        {
            //analyzers find the results of other analyzers using this attribute
            string sFileExtensionType
                = CalculatorHelpers.GetAttribute(calcParameters.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType
                = MakeAnalysisFileExtensionTypeForDbUpdate(calcParameters,
                    calcParameters.AnalyzerParms.FilesToAnalyzeExtensionType);
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                CalculatorHelpers.SetFileExtensionType(calcParameters,
                    sNeededFileExtensionType);
            }
        }
        public static string MakeAnalysisFileExtensionTypeForDbUpdate(this CalculatorParameters calcParameters,
            string filesToAnalyzeExtensionType)
        {
            //i.e. filetoanalyzeExttype = budget; analyzertype = stats01, comparisontype = none
            //= budgetstats01none
            string sFileExtensionType = string.Concat(filesToAnalyzeExtensionType,
               calcParameters.AnalyzerParms.AnalyzerType, calcParameters.AnalyzerParms.ComparisonType.ToString());
            return sFileExtensionType;
        }
    }
}
