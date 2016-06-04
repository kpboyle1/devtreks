using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		1. Group the MN elements by an AggregationId (TypeId, GroupId, Label)
    ///             2. RunAnalyses
    ///             3. Save the analyses as xml
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. 
    /// </summary>
    public class BIMN1StockAnalyzer : BudgetInvestmentCalculator
    {
        public BIMN1StockAnalyzer(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set Indic1Stock
            Init();
        }
        public BIMN1StockAnalyzer()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //set MN1DescendentStock state so that descendant stock totals will have good base properties
            //the base.Analyzer is set when the Save... methods are run
            this.MN1DescendentStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (this.GCCalculatorParams.LinkedViewElement != null)
            {
                this.MN1DescendentStock.SetDescendantMN1StockProperties(
                    this.GCCalculatorParams.LinkedViewElement);
            }
        }
        //stateful food nutrition costs
        public MNC1Calculator MNC1 { get; set; }
        //stateful food nutrition benefits
        public MNB1Calculator MNB1 { get; set; }
        //stateful analyzer used to set base.Analyzer properties in descendants (name, id)
        //and to hold descendent input and output stocks for analysis
        public MN1Stock MN1DescendentStock { get; set; }
        //stateful currentnodename
        private string CurrentNodeName { get; set; }

        //these objects hold collections of MN1s for running totals.
        public BudgetInvestmentGroup BudgetGroup { get; set; }
        public OperationComponentGroup OCGroup { get; set; }
        public OutcomeGroup OutcomeGroup { get; set; }
        public InputGroup InputGroup { get; set; }
        public OutputGroup OutputGroup { get; set; }
        public BudgetInvestment Budget { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public OperationComponent OpComp { get; set; }
        public Outcome Outcome { get; set; }
        public Output Output { get; set; }
        public Input Input { get; set; }
        //comparative analysis column counter
        private int ColumnIndex { get; set; }
        private int ColumnCount { get; set; }
        //this can be run asynchronously as well
        public bool SaveMN1StockTotals()
        {
            bool bHasCalculations = false;
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath
                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                return bHasCalculations;
            ////new temporary path to store calculator results
            //string sNewDocToCalcTempDocPath = CalculatorHelpers.GetFileToCalculate(
            //    this.GCCalculatorParams);
            StringWriter output = new StringWriter();
            XmlWriterSettings oXmlWriterSettings = new XmlWriterSettings();
            oXmlWriterSettings.Indent = true;
            oXmlWriterSettings.OmitXmlDeclaration = true;
            //store the results in tempdocpath
            using (XmlWriter writer
                = XmlWriter.Create(output,
                oXmlWriterSettings))
            {
                writer.WriteStartElement(Constants.ROOT_PATH);
                //one aggregating element for every unique node
                //i.e. five ops with same label = one op element with 5 observations in atts
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets
                    || this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    bHasCalculations = SaveBIMN1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.operationprices
                    || this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    bHasCalculations = SaveOCMN1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    bHasCalculations = SaveOutcomeMN1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    bHasCalculations = SaveInputMN1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    bHasCalculations = SaveOutputMN1StockTotals(writer);
                }
                writer.WriteEndElement();
            }
            using (output)
            {
                if (this.GCCalculatorParams.ErrorMessage == string.Empty
                        && bHasCalculations)
                {
                    //move the new calculations to tempDocToCalcPath
                    //this returns an error msg
                    this.GCCalculatorParams.ErrorMessage = CalculatorHelpers.MoveURIs(
                        this.GCCalculatorParams.ExtensionDocToCalcURI, output,
                        this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
                    bHasCalculations = true;
                }
            }
            return bHasCalculations;
        }
        
        
        private void SetCalculatorId(Calculator1 baseElement, MN1Stock stock)
        {
            //change and progress analyzers change the calcid using Change1.Stocks
            if (!MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                SetCalcId(baseElement, stock);
            }
        }
        private void SetCalcId(Calculator1 baseElement, MN1Stock stock)
        {
            //the initial collections don't store initial stock calculators
            //related calculator.Id are stored with the base element
            if (baseElement.CalculatorId != 0)
            {
                //the CalculatorId is used to set the Id (Id is the base element_
                stock.CalculatorId = baseElement.CalculatorId;
                stock.Id = baseElement.CalculatorId;
            }
            else
            {
                if (stock.CalculatorId != 0)
                {
                    //the stockid was changed to parent element, switch it back
                    stock.Id = stock.CalculatorId;
                }
                else
                {
                    if (stock.Id != 0)
                    {
                        //the stockid was changed to parent element, switch it back
                        stock.CalculatorId = stock.Id;
                    }
                }
            }
            //don't allow lvs to have a zero id
            if (stock.CalculatorId == 0)
            {
                stock.CalculatorId = baseElement.Id;
            }
        }
        //this is called one time after the stateful collections are built
        public bool SetBIMN1StockTotals()
        {
            bool bHasCalculations = false;
            if (this.BudgetGroup.BudgetInvestments != null)
            {
                //set the correct aggregation for the analysis
                //this aggregates bis, tps, opcomps, and outcomes
                bHasCalculations = SetBIAggregation(this.BudgetGroup.BudgetInvestments);
            }
            return bHasCalculations;
        }
        
        //this is called one time after the stateful collections are built
        public bool SetOCMN1StockTotals()
        {
            bool bHasCalculations = false;

            if (this.BudgetGroup != null)
            {
                if (this.BudgetGroup.BudgetInvestments != null)
                {
                    foreach (var b in this.BudgetGroup.BudgetInvestments)
                    {
                        if (b.TimePeriods != null)
                        {
                            //int iAncestorTPId = 0;
                            foreach (var tp in b.TimePeriods)
                            {
                                if (tp.OperationComponents != null)
                                {
                                    bHasCalculations = SetOCAggregation(tp.OperationComponents);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.OCGroup != null)
                {
                    if (this.OCGroup.OperationComponents != null)
                    {
                        //set the correct aggregation for the analysis
                        bHasCalculations = SetOCAggregation(this.OCGroup.OperationComponents);
                    }
                }
            }
            return bHasCalculations;
        }
        //this is called one time after the stateful collections are built
        public bool SetOutcomeMN1StockTotals()
        {
            bool bHasCalculations = false;
            if (this.BudgetGroup != null)
            {
                if (this.BudgetGroup.BudgetInvestments != null)
                {
                    foreach (var b in this.BudgetGroup.BudgetInvestments)
                    {
                        if (b.TimePeriods != null)
                        {
                            foreach (var tp in b.TimePeriods)
                            {
                                if (tp.Outcomes != null)
                                {
                                    bHasCalculations = SetOutcomeAggregation(tp.Outcomes);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.OutcomeGroup != null)
                {
                    if (this.OutcomeGroup.Outcomes != null)
                    {
                        //set the correct aggregation for the analysis
                        bHasCalculations = SetOutcomeAggregation(this.OutcomeGroup.Outcomes);
                    }
                }
            }
            return bHasCalculations;
        }
       
        //this is called one time after the stateful collections are built
        public bool SetInputMN1StockTotals()
        {
            bool bHasCalculations = false;
            if (this.InputGroup != null)
            {
                if (this.InputGroup.Inputs != null)
                {
                    //set the correct aggregation for the analysis
                    bHasCalculations = SetInputAggregation(this.InputGroup.Inputs);
                }
            }
            return bHasCalculations;
        }
        
        private void AddNewOutput(OutputGroup outGroup, Output output)
        {
            Output o = new Output(this.GCCalculatorParams, output);
            o.Calculators = new List<Calculator1>();
            if (output.Calculators != null)
            {
                MN1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
            }
            o.Outputs = new List<Extensions.Output>();
            if (output.Outputs != null)
            {
                foreach (Output outputseries in output.Outputs)
                {
                    Output outputseries2 = new Output(this.GCCalculatorParams, outputseries);
                    outputseries2.Calculators = new List<Calculator1>();
                    if (outputseries.Calculators != null)
                    {
                        MN1AnalyzerHelper.AddOutputCalculators(outputseries.Calculators, outputseries2.Calculators);
                    }
                    o.Outputs.Add(outputseries2);
                }
            }
            outGroup.Outputs.Add(o);
        }
        
        //this is called one time after the stateful collections are built
        public bool SetOutputMN1StockTotals()
        {
            bool bHasCalculations = false;
            if (this.OutputGroup != null)
            {
                if (this.OutputGroup.Outputs != null)
                {
                    //set the correct aggregation for the analysis
                    bHasCalculations = SetOutputAggregation(this.OutputGroup.Outputs);
                }
            }
            return bHasCalculations;
        }
        
        //budgets
        public bool SetBIAggregation(List<BudgetInvestment> bis)
        {
            bool bHasGroups = false;
            //change analyses agg by yr, alt, or id
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                     qry4 = bis
                    .OrderBy(m => m.Date.Year.ToString())
                    .GroupBy(m => m.Date.Year.ToString());
                SetBIMNsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    SetBIGroupMN1StockTotals();
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                     qry4 = bis
                    .OrderBy(m => m.AlternativeType)
                    .GroupBy(m => m.AlternativeType);
                SetBIMNsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    SetBIGroupMN1StockTotals();
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                    qry4 = bis
                    .GroupBy(m => m.Id.ToString());
                SetBIMNsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    SetBIGroupMN1StockTotals();
                }
            }
            else
            {
                switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
                {
                    case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry1 = bis
                            .GroupBy(m => m.TypeId.ToString());
                        SetBIMNsAggregation(qry1);
                        //BudgetInvestmentGroup biGroup1 = GetBIMNsAggregation(qry1);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupMN1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry2 = bis
                            .GroupBy(m => m.GroupId.ToString());
                        SetBIMNsAggregation(qry2);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupMN1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry3 = bis
                            .GroupBy(m => m.Label);
                        SetBIMNsAggregation(qry3);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupMN1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry4 = bis
                            .GroupBy(m => m.Id.ToString());
                        SetBIMNsAggregation(qry4);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupMN1StockTotals();
                        }
                        break;
                    default:
                        break;
                }
            }
            return bHasGroups;
        }
        private void SetBIMNsAggregation(
            IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>> qry)
        {
            //use new, not byref, objects
            this.GCCalculatorParams.ParentBudgetInvestmentGroup
                = new BudgetInvestmentGroup(this.GCCalculatorParams, this.BudgetGroup);
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                //calculators start with inputs and outputs
                this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                    = new List<BudgetInvestment>();
                //altern
                int a = 0;
                foreach (var g in qry)
                {
                    int i = 0;
                    foreach (BudgetInvestment bi in g)
                    {
                        if (i == 0)
                        {
                            this.GCCalculatorParams.ParentBudgetInvestment = new BudgetInvestment(this.GCCalculatorParams, bi);
                            this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods = new List<Extensions.TimePeriod>();
                            this.GCCalculatorParams.ParentBudgetInvestment.Calculators = new List<Calculator1>();
                            this.GCCalculatorParams.ParentBudgetInvestment.Alternative2 = a;
                            this.GCCalculatorParams.ParentBudgetInvestment.Observations = g.Count();
                        }
                        //calculators start with inputs and outputs
                        //this aggregates the tps, opcomps, and outcomes
                        //and adds stock totals to those collections
                        //this.GCCalculatorParams.ParentBudgetInvestment will be a new calculator holding agg tps
                        bool bHasTPTotals = SetTPAggregation(bi.TimePeriods);
                        i += 1;
                    }
                    //now run calcs for this.GCCalculatorParams.ParentBudgetInvestment.Calculators
                    //add the budget to the stateful bg, but don't make it by ref
                    bool bHasTotals = SetBIMN1StockTotal();
                    if (bHasTotals)
                    {
                        //avoid a byref by building a new bi
                        BudgetInvestment budInvest
                            = new BudgetInvestment(this.GCCalculatorParams, this.GCCalculatorParams.ParentBudgetInvestment);
                        budInvest.TimePeriods = new List<TimePeriod>();
                        budInvest.Calculators = new List<Calculator1>();
                        MN1AnalyzerHelper.AddCalculators(this.GCCalculatorParams.ParentBudgetInvestment.Calculators,
                            budInvest.Calculators);
                        foreach (var tp in this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods)
                        {
                            TimePeriod newTp = new TimePeriod(this.GCCalculatorParams, tp);
                            newTp.Calculators = new List<Calculator1>();
                            SetNewTPFromOldTp(newTp, tp);
                            //watch byref
                            MN1AnalyzerHelper.AddCalculators(tp.Calculators, newTp.Calculators);
                            budInvest.TimePeriods.Add(newTp);
                        }
                         this.GCCalculatorParams.ParentBudgetInvestmentGroup
                             .BudgetInvestments.Add(budInvest);
                         //keep them ordered by the type of analysis
                         if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                             == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
                         {
                             this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                                 = this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments.
                                 OrderBy(bi => bi.AlternativeType).ToList();
                             //no ChangebyYear at bi level and ChangeById is already ordered
                         }
                    }
                    a++;
                }
            }
        }
        private bool SetBIGroupMN1StockTotals()
        {
            bool bHasTotals = false;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock biGroupStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                biGroupStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(this.GCCalculatorParams.ParentBudgetInvestmentGroup, biGroupStock);
                //each bi holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var BIs = this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments.OrderBy(bi => bi.Date);
                //run the bi analysis
                if (BIs != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var bi in BIs)
                    {
                        MN1AnalyzerHelper.AddCalculators(bi.Calculators, calcs);
                    }
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biGroupStock.RunAnalyses(calcs);

                        ////cumulatives are always actuals
                        //biGroupStock.TargetType = Calculator1.TARGET_TYPES.actual.ToString();
                        ////bis already have cumulative totals
                        ////last stock has cumulatives, simple static transfer of totals
                        //bHasTotals = MN1Progress1.AddCumulative1Calcs(biGroupStock, calcs);
                    }
                    else
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biGroupStock.RunAnalyses(calcs);
                    }
                    
                }
                if (bHasTotals && biGroupStock != null)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToBIs(biGroupStock, BIs);
                    this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(this.GCCalculatorParams.ParentBudgetInvestmentGroup, biGroupStock);
                    this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators.Add(biGroupStock);
                    //replace this.budgetgroup
                    this.BudgetGroup = this.GCCalculatorParams.ParentBudgetInvestmentGroup;
                }
            }
            return bHasTotals;
        }
        
        private bool SetBIMN1StockTotal()
        {
            bool bHasTotals = false;
            if (this.GCCalculatorParams.ParentBudgetInvestment != null)
            {
                MN1Stock biStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                biStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(this.GCCalculatorParams.ParentBudgetInvestment, biStock);
                //only need to aggregate if more than one calculator is on hand (tps already aggregated using tempBI)
                if (this.GCCalculatorParams.ParentBudgetInvestment.Calculators.Count > 1)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    //parentbi holds one or more calculators holding results of agg tps
                    MN1AnalyzerHelper.AddCalculators(this.GCCalculatorParams.ParentBudgetInvestment.Calculators, calcs);
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biStock.RunAnalyses(calcs);

                        ////cumulatives are always actuals
                        //biStock.TargetType = Calculator1.TARGET_TYPES.actual.ToString();
                        ////bis already have cumulative totals
                        ////last stock has cumulatives, simple static transfer of totals
                        //bHasTotals = MN1Progress1.AddCumulative1Calcs(biStock, calcs);
                    }
                    else
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biStock.RunAnalyses(calcs);
                    }
                    if (bHasTotals && biStock != null)
                    {
                        this.GCCalculatorParams.ParentBudgetInvestment.Calculators = new List<Calculator1>();
                        //lcaStockMember.Total, or .Stats holds aggregated numbers
                        SetCalculatorId(this.GCCalculatorParams.ParentBudgetInvestment, biStock);
                        this.GCCalculatorParams.ParentBudgetInvestment.Calculators.Add(biStock);
                    }
                }
                else
                {
                    bHasTotals = true;
                }
            }
            return bHasTotals;
        }
        //tp aggregations
        public bool SetTPAggregation(List<TimePeriod> tps)
        {
            bool bHasGroups = false;
            BudgetInvestment tempBI = new BudgetInvestment();
            //change analyses agg by yr, alt, or id
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .OrderBy(m => m.Date.Year.ToString())
                    .GroupBy(m => m.Date.Year.ToString());
                tempBI = SetTPMNsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = SetTPMN1StockTotals(tempBI);
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .OrderBy(m => m.AlternativeType)
                    .GroupBy(m => m.AlternativeType);
                tempBI = SetTPMNsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = SetTPMN1StockTotals(tempBI);
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .GroupBy(m => m.Id.ToString());
                tempBI = SetTPMNsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = SetTPMN1StockTotals(tempBI);
                }
            }
            else
            {
                switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
                {
                    case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry1 = tps
                            .GroupBy(m => m.TypeId.ToString());
                        tempBI = SetTPMNsAggregation(qry1);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPMN1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry2 = tps
                            .GroupBy(m => m.GroupId.ToString());
                        tempBI = SetTPMNsAggregation(qry2);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPMN1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry3 = tps
                            .GroupBy(m => m.Label);
                        tempBI = SetTPMNsAggregation(qry3);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPMN1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry4 = tps
                            .GroupBy(m => m.Id.ToString());
                        tempBI = SetTPMNsAggregation(qry4);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPMN1StockTotals(tempBI);
                        }
                        break;
                    default:
                        break;
                }
            }
            return bHasGroups;
        }

        private BudgetInvestment SetTPMNsAggregation(IEnumerable<System.Linq.IGrouping<string, TimePeriod>> qry)
        {
            BudgetInvestment tempBI = new BudgetInvestment(this.GCCalculatorParams, this.GCCalculatorParams.ParentBudgetInvestment);
            tempBI.TimePeriods = new List<TimePeriod>();
            tempBI.Calculators = new List<Calculator1>();
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                //altern
                int a = 0;
                //changes use descendent ancestors not siblings
                foreach (var g in qry)
                {
                    int i = 0;
                    //the full collection gets aggregated into timeperiod
                    TimePeriod newTP = new TimePeriod();
                    foreach (TimePeriod tp in g)
                    {
                        //they all get aggregated into a new newTP
                        if (i == 0)
                        {
                            newTP = new TimePeriod(this.GCCalculatorParams, tp);
                            newTP.OperationComponents = new List<Extensions.OperationComponent>();
                            newTP.Outcomes = new List<Extensions.Outcome>();
                            newTP.Calculators = new List<Calculator1>();
                            newTP.Alternative2 = a;
                            newTP.Observations = g.Count();
                        }
                        //copy each tp's descendants to the new tp
                        //these will be aggregated separately below
                        SetNewTPFromOldTp(newTP, tp);
                        i += 1;
                    }
                    a++;
                    //now aggregate all of the descendent indicators
                    //tp will now have good ocs with no need for any further work
                    TimePeriod timeperiodTotals = new TimePeriod(this.GCCalculatorParams, newTP);
                    timeperiodTotals.OperationComponents = new List<Extensions.OperationComponent>();
                    timeperiodTotals.Outcomes = new List<Extensions.Outcome>();
                    //this is how descendents find ancestor budget.tps to make comparisons
                    this.GCCalculatorParams.AnalyzerParms.AggregatingId = timeperiodTotals.Label;
                    this.GCCalculatorParams.AnalyzerParms.AggregatingOldId = timeperiodTotals.Label;
                    //this aggregates and runs oc and outcome analyses
                    //otherwise awkward to change tp.oc collection while running tp analyses
                    bool bHasTotals = GetTPAggregation(newTP, timeperiodTotals);
                    if (bHasTotals)
                    {
                        //add to temporary aggregator (runs partial bi calcs)
                        tempBI.TimePeriods.Add(timeperiodTotals);
                        if (this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods == null)
                            this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods = new List<TimePeriod>();
                        this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods.Add(timeperiodTotals);
                        //byref to PT.TPs too  
                        OrderTPs(tempBI);
                    }
                }
            }
            return tempBI;
        }
        private void SetNewTPFromOldTp(TimePeriod newTP, TimePeriod oldTP)
        {
            if (newTP.OperationComponents == null)
            {
                newTP.OperationComponents = new List<Extensions.OperationComponent>();
            }
            if (oldTP.OperationComponents != null)
            {
                foreach (OperationComponent oc in oldTP.OperationComponents)
                {
                    AddNewOCToCollection(oc, newTP.OperationComponents);
                }
            }
            if (newTP.Outcomes == null)
            {
                newTP.Outcomes = new List<Extensions.Outcome>();
            }
            if (oldTP.Outcomes != null)
            {
                foreach (Outcome outcome in oldTP.Outcomes)
                {
                    AddNewOutcomeToCollection(outcome, newTP.Outcomes);
                }
            }
        }

        private bool GetTPAggregation(TimePeriod oldTP, TimePeriod newTP)
        {
            //this will group the ocs, run the analyses, and addthem to this.OCGroup
            bool bHasCalculations = SetOCAggregation(oldTP.OperationComponents);
            //add them to new timeperiod
            if (bHasCalculations)
            {
                if (this.OCGroup != null)
                {
                    if (this.OCGroup.OperationComponents != null)
                    {
                        foreach (var oc in this.OCGroup.OperationComponents)
                        {
                            //need the tp.amount from the correct tp (not the agg tp)
                            oc.Multiplier = oldTP.Amount;
                            AddNewOCToCollection(oc, newTP.OperationComponents);
                            //calculators start with inputs and outputs
                        }
                        //transfer ocgroup.calc to newTP.calc
                        if (this.OCGroup.Calculators != null)
                        {
                            //transfer correct tp multipliers to use
                            //note that stocks must use calculator.multiplier not parenttpstock.multiplier
                            foreach (var calc in this.OCGroup.Calculators)
                            {
                                calc.Multiplier = oldTP.Amount;
                                //these calculators use net tp amounts not totals 
                                //must distinguish outcomes from op/comps
                                //but only outcomes need their subapptype set
                                
                            }
                            if (newTP.Calculators == null)
                            {
                                newTP.Calculators = new List<Calculator1>();
                            }
                            AddStockCalculators(this.OCGroup.Calculators, newTP.Calculators);
                        }
                        //no need to keep two collections
                        this.OCGroup = null;
                    }
                }
            }
            //this will group the ocs, run the analyses, and addthem to this.OutcomeGroup
            bool bHasCalculations2 = SetOutcomeAggregation(oldTP.Outcomes);
            //add them to new timeperiod
            if (bHasCalculations2)
            {
                if (this.OutcomeGroup != null)
                {
                    if (this.OutcomeGroup.Outcomes != null)
                    {
                        foreach (var oc in this.OutcomeGroup.Outcomes)
                        {
                            //need the tp.amount from the correct tp (not the agg tp)
                            oc.Multiplier = oldTP.Amount;
                            AddNewOutcomeToCollection(oc, newTP.Outcomes);
                            //calculators start with inputs and outputs
                        }
                        //transfer ocgroup.calc to newTP.calc
                        if (this.OutcomeGroup.Calculators != null)
                        {
                            //transfer correct tp multipliers to use
                            //note that stocks must use calculator.multiplier not parenttpstock.multiplier
                            foreach (var calc in this.OutcomeGroup.Calculators)
                            {
                                calc.Multiplier = oldTP.Amount;
                                //must distinguish outcomes from op/comps
                                calc.SubApplicationType = Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString();
                            }
                            if (newTP.Calculators == null)
                            {
                                newTP.Calculators = new List<Calculator1>();
                            }
                            AddStockCalculators(this.OutcomeGroup.Calculators, newTP.Calculators);
                        }
                        //no need to keep two collections
                        this.OutcomeGroup = null;
                    }
                }
            }
            bool bHasCalcs = false;
            if (bHasCalculations || bHasCalculations2)
            {
                bHasCalcs = true;
            }
            return bHasCalcs;
        }
        private void AddNewOCToCollection(OperationComponent opComp, List<OperationComponent> ocs)
        {
            OperationComponent oc = new OperationComponent(this.GCCalculatorParams, opComp);
            //multiplier comes from correct preaggregated tp
            oc.Multiplier = opComp.Multiplier;
            oc.Calculators = new List<Calculator1>();
            if (opComp.Calculators != null)
            {
                AddStockCalculators(opComp.Calculators, oc.Calculators);
            }
            oc.Inputs = new List<Extensions.Input>();
            if (opComp.Inputs != null)
            {
                foreach (Input input in opComp.Inputs)
                {
                    Input i = new Input(this.GCCalculatorParams, input);
                    i.Calculators = new List<Calculator1>();
                    if (input.Calculators != null)
                    {
                        //add any base calculators
                        MN1AnalyzerHelper.AddInputCalculators(input.Calculators, i.Calculators);
                        //add any stock analyzers
                        AddStockInputCalculators(input.Calculators, i.Calculators);
                    }
                    oc.Inputs.Add(i);
                }
            }
            ocs.Add(oc);
        }
        private void AddNewOutcomeToCollection(Outcome outcome, List<Outcome> ocs)
        {
            Outcome oc = new Outcome(this.GCCalculatorParams, outcome);
            //multiplier comes from correct preaggregated tp
            oc.Multiplier = outcome.Multiplier;
            oc.Calculators = new List<Calculator1>();
            if (outcome.Calculators != null)
            {
                AddStockCalculators(outcome.Calculators, oc.Calculators);
            }
            oc.Outputs = new List<Extensions.Output>();
            if (outcome.Outputs != null)
            {
                foreach (Output output in outcome.Outputs)
                {
                    Output o = new Output(this.GCCalculatorParams, output);
                    o.Calculators = new List<Calculator1>();
                    if (output.Calculators != null)
                    {
                        //add any base calculators
                        MN1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
                        //add any stock analyzers
                        AddStockOutputCalculators(output.Calculators, o.Calculators);
                    }
                    oc.Outputs.Add(o);
                }
            }
            ocs.Add(oc);
        }
  
        private bool SetTPMN1StockTotals(BudgetInvestment tempBI)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //tempBI has to be used to run totals, because ParentBI.TPs could have TPs from previous tp aggregations
            if (tempBI.TimePeriods != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock biStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (tempBI.Calculators == null)
                    tempBI.Calculators = new List<Calculator1>();
                //need the options
                biStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(tempBI, biStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var TPs = tempBI.TimePeriods.OrderBy(t => t.Date);
                //run the tp analysis
                if (TPs != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var tp in TPs)
                    {
                        //tp already has the aggregated descendent calcs, 
                        //but still needs the tpstock.props and multiplier-adjusted totals set
                        bHasAnalysis = SetTPMN1StockTotals(tp);
                        MN1AnalyzerHelper.AddCalculators(tp.Alternative2, tp.Calculators, calcs);
                    }
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                    {
                        //add ancestors needed for progress analysis
                        AddAncestorTPCalcs(calcs);
                        //run the analyses by aggregating the calcs
                        bHasTotals = biStock.RunAnalyses(calcs);

                        ////cumulatives are always actuals
                        //biStock.TargetType = Calculator1.TARGET_TYPES.actual.ToString();
                        ////dig around for which totals to use
                        //bHasTotals = AddCumulative2Calcs(biStock, calcs);
                    }
                    else
                    {
                        //add ancestors needed for change analysis
                        AddAncestorTPCalcs(calcs);
                        //run the analyses by aggregating the calcs
                        bHasTotals = biStock.RunAnalyses(calcs);
                    }
                }
                if (bHasTotals && biStock != null)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToTPs(biStock, TPs);
                    //this adds the tps to parentbi
                    AddTPCalcsToParentBI(TPs);
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(this.GCCalculatorParams.ParentBudgetInvestment, biStock);
                    this.GCCalculatorParams.ParentBudgetInvestment.Calculators.Add(biStock);
                }
            }
            return bHasTotals;
        }
        private void OrderTPs(BudgetInvestment bi)
        {
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.AlternativeType).ToList();
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.Date.Year).ToList();
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.Id).ToList();
            }
            else
            {
                //keep the existing order
            }
        }
        private bool SetTPMN1StockTotals(TimePeriod tp)
        {
            //tp already has good calcs, but a needs tp.calc to change to tp.props and multiplied 
            bool bHasTotals = false;
            //add the oc stock totals using the OCs.Inputs collection that now hold inputstock calcs
            //add indicator1stocks to corresponding MandE element
            MN1Stock tpStockMember = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //need the options
            tpStockMember.CopyCalculatorProperties(this.MN1DescendentStock);
            CopyBaseElementProperties(tp, tpStockMember);
            //multiplier was added to children ocs and their totals multiplied 
            tpStockMember.Multiplier = 1; 
            //tps subtract stock02 outputs from stock01 inputs and use stock01 for subsequent analysis and display
            //more elaborate stock balances can follow
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                tpStockMember.CalcParameters.CurrentElementNodeName = BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString();
            }
            else
            {
                tpStockMember.CalcParameters.CurrentElementNodeName = BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString();
            }
            //run the analyses by aggregating the calcs
            bHasTotals = tpStockMember.RunAnalyses(tp.Calculators);
            //don't double count the multiplier when the progress analysis is run next
            tpStockMember.Multiplier = 1;
            if (tpStockMember != null)
            {
                //only need the tpstockmember totals; not it's stocks (those are for consistency in runanalyses)
                //reset tp.calcs - already have totals in tpstockmember
                tp.Calculators = new List<Calculator1>();
                //lcaStockMember.Total, or .Stats holds aggregated numbers
                SetCalculatorId(tp, tpStockMember);
                tp.Calculators.Add(tpStockMember);
            }
            return bHasTotals;
        }
        public bool AddTPCalcsToParentBI(IOrderedEnumerable<TimePeriod> tps)
        {
            bool bHasAdded = false;
            if (this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods == null)
                this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods = new List<TimePeriod>();
            if (tps != null)
            {
                foreach (var tp in tps)
                {
                    //add to the correct gp.parent (this includes children opcomps and outcomes)
                    if (!this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods.Any(t => t.Id == tp.Id))
                    {
                        //should have already been added or progress won't work
                        this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods.Add(tp);
                    }
                    else
                    {
                        //need any new calcs added
                        //don't get rid of old, they may be accumulating in setbi
                        if (tp.Calculators != null)
                        {
                            //tp is a byref to a this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods
                            
                        }

                    }
                }
                //keep the tps ordered by analysistype
                OrderTPs(this.GCCalculatorParams.ParentBudgetInvestment);
            }
            return bHasAdded;
        }

        
        //operations
        public bool SetOCAggregation(List<OperationComponent> ocs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry1 = ocs
                        .GroupBy(m => m.TypeId.ToString());
                    OperationComponentGroup ocGroup1 = GetOCMNsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = SetOCMN1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry2 = ocs
                        .GroupBy(m => m.GroupId.ToString());
                    OperationComponentGroup ocGroup2 = GetOCMNsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = SetOCMN1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry3 = ocs
                        .GroupBy(m => m.Label);
                    OperationComponentGroup ocGroup3 = GetOCMNsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = SetOCMN1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                        && (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                        || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices))
                    {
                        //id field will result in original structure
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        OperationComponentGroup ocGroup4 = GetOCMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOCMN1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                        && (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                        || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices))
                    {
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        OperationComponentGroup ocGroup4 = GetOCMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOCMN1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        //id field will result in original structure
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .GroupBy(m => m.Id.ToString());
                        OperationComponentGroup ocGroup4 = GetOCMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOCMN1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        
        private OperationComponentGroup GetOCMNsAggregation(
            IEnumerable<System.Linq.IGrouping<string, OperationComponent>> qry)
        {
            //use new, not byref, objects
            if (this.OCGroup == null)
                this.OCGroup = new OperationComponentGroup();
            OperationComponentGroup ocGroup2 = new OperationComponentGroup(this.GCCalculatorParams, this.OCGroup);
            AddStockCalculators(this.OCGroup.Calculators, ocGroup2.Calculators);
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                ocGroup2.Calculators = new List<Calculator1>();
                //calculators start with inputs and outputs
                ocGroup2.OperationComponents = new List<OperationComponent>();
                //altern
                int a = 0;
                foreach (var g in qry)
                {
                    int i = 0;
                    //the full collection gets aggregated into opComp
                    OperationComponent opComp = new OperationComponent();
                    foreach (OperationComponent oc in g)
                    {
                        if (i == 0)
                        {
                            opComp = new OperationComponent(this.GCCalculatorParams, oc);
                            opComp.Inputs = new List<Extensions.Input>();
                            opComp.Calculators = new List<Calculator1>();
                            opComp.Alternative2 = a;
                            opComp.Observations = g.Count();
                        }
                        //calculators start with inputs and outputs
                        if (oc.Inputs != null)
                        {
                            foreach (var input in oc.Inputs)
                            {
                                Input input1 = new Input(this.GCCalculatorParams, input);
                                //note this has to be i
                                input1.Alternative2 = i;
                                input1.Calculators = new List<Calculator1>();
                                //the parent oc.amount must come from correct oc, not the aggregated oc
                                input1.Multiplier = input1.Multiplier * oc.Multiplier;
                                MN1AnalyzerHelper.AddInputCalculators(input.Calculators, input1.Calculators);
                                opComp.Inputs.Add(input1);
                            }
                        }
                        i += 1;
                    }
                    ocGroup2.OperationComponents.Add(opComp);
                    a++;
                }
            }
            return ocGroup2;
        }
        private bool SetOCMN1StockTotals(OperationComponentGroup ocGroup)
        {
            bool bHasTotals = false;
            bool bHasAnalysis = false;
            if (ocGroup.OperationComponents != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock ocGroupStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                ocGroupStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(ocGroup, ocGroupStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var OCs = ocGroup.OperationComponents.OrderBy(oc => oc.Date);
                //run the oc analysis
                if (OCs != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var oc in OCs)
                    {
                        bHasAnalysis = SetOCMN1StockTotals(oc);
                        MN1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                    }
                    //add ancestors needed for change analysis
                    AddAncestorOCCalcs(calcs);
                    //run the analysis
                    bHasTotals = ocGroupStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOCs(ocGroupStock, OCs);
                    ocGroup.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(ocGroup, ocGroupStock);
                    ocGroup.Calculators.Add(ocGroupStock);
                    //replace this.ocgroup
                    this.OCGroup = ocGroup;
                }
            }
            return bHasTotals;
        }
        private bool SetOCMN1StockTotals(OperationComponent oc)
        {
            bool bHasTotals = false;
            //add the input stock totals using the oc
            //assume that good inputs means good totals
            bHasTotals = SetInputMN1StockTotals(oc);
            //add the oc stock totals using the OCs.Inputs collection that now hold inputstock calcs
            //add indicator1stocks to corresponding MandE element
            MN1Stock mn1StockMember = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //need the options
            mn1StockMember.CopyCalculatorProperties(this.MN1DescendentStock);
            CopyBaseElementProperties(oc, mn1StockMember);
            //existing oc.Multiplier comes from correct parent tp or ocgroup before tps were aggregated
            //oc.Multiplier not used because it was correctly added to children inputs and they have mults already
            mn1StockMember.Multiplier = 1; 
            if (oc.Inputs != null)
            {
                List<Calculator1> calcs = new List<Calculator1>();
                foreach (var input in oc.Inputs)
                {
                    MN1AnalyzerHelper.AddCalculators(input.Calculators, calcs);
                }
                //some analyzers only sum totals for the calcs (i.e. no changes in ins needed)
                mn1StockMember.CalcParameters.CurrentElementNodeName 
                    = Input.INPUT_PRICE_TYPES.input.ToString();
                //run the analyses by aggregating the input.calcs
                bool bHasAnalysis = mn1StockMember.RunAnalyses(calcs);
            }
            //don't double use the multiplier
            mn1StockMember.Multiplier = 1;
            oc.Calculators = new List<Calculator1>();
            //lcaStockMember.Total, or .Stats holds aggregated numbers
            SetCalculatorId(oc, mn1StockMember);
            oc.Calculators.Add(mn1StockMember);
            return bHasTotals;
        }
        private bool SetInputMN1StockTotals(OperationComponent oc)
        {
            bool bHasTotals = false;
            //run the input analysis using this.MN1DescendentStock (so that ancestors can use them)
            if (oc.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock mn1Stock = new MN1Stock(this.GCCalculatorParams,
                    this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                mn1Stock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(oc, mn1Stock);
                foreach (var input in oc.Inputs)
                {
                    //add indicator1stocks to corresponding MandE element
                    MN1Stock mn1StockMember = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    mn1StockMember.CopyCalculatorProperties(this.MN1DescendentStock);
                    CopyBaseElementProperties(input, mn1StockMember);
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        //but still needs input.MNCInput.ocamount, aohamount, and capamount
                        mn1StockMember.Multiplier = input.Multiplier;
                        mn1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                    }
                    else
                    {
                        //existing input.Multiplier comes from correct parent oc before ocs were aggregated
                        mn1StockMember.Multiplier = input.Multiplier;
                        mn1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                    }
                    if (input.Calculators != null)
                    {
                        mn1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                        //add a cost calculator to subp1s collection
                        //some analyzers use the subp1s (total1), so they must be copied correctly into new stock objects
                        mn1StockMember.MNSR1Stock.AddInputCalcsToStock(input.Calculators);
                        //MN1AnalyzerHelper.AddInputCalculators(input.Calculators, mn1StockMember.MNSR1Stock.FoodNutritionCalcs);
                    }
                    //add the stockmember to the base stock
                    bHasTotals = mn1StockMember.RunAnalyses();
                    //don't double use the multiplier
                    mn1StockMember.Multiplier = 1;
                    SetCalculatorId(input, mn1StockMember);
                    input.Calculators.Add(mn1StockMember);
                }
            }
            return bHasTotals;
        }
        
        //outcomes
        public bool SetOutcomeAggregation(List<Outcome> ocs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry1 = ocs
                        .GroupBy(m => m.TypeId.ToString());
                    OutcomeGroup ocGroup1 = GetOutcomeMNsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = SetOutcomeMN1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry2 = ocs
                        .GroupBy(m => m.GroupId.ToString());
                    OutcomeGroup ocGroup2 = GetOutcomeMNsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = SetOutcomeMN1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry3 = ocs
                        .GroupBy(m => m.Label);
                    OutcomeGroup ocGroup3 = GetOutcomeMNsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = SetOutcomeMN1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .OrderBy(m => m.Date.Year.ToString())
                                .GroupBy(m => m.Date.Year.ToString());
                        OutcomeGroup ocGroup4 = GetOutcomeMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOutcomeMN1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .OrderBy(m => m.AlternativeType)
                                .GroupBy(m => m.AlternativeType);
                        OutcomeGroup ocGroup4 = GetOutcomeMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOutcomeMN1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .GroupBy(m => m.Id.ToString());
                        OutcomeGroup ocGroup4 = GetOutcomeMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOutcomeMN1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }

        private OutcomeGroup GetOutcomeMNsAggregation(
            IEnumerable<System.Linq.IGrouping<string, Outcome>> qry)
        {
            //use new, not byref, objects
            if (this.OutcomeGroup == null)
                this.OutcomeGroup = new OutcomeGroup();
            OutcomeGroup ocGroup2 = new OutcomeGroup(this.GCCalculatorParams, this.OutcomeGroup);
            AddStockCalculators(this.OutcomeGroup.Calculators, ocGroup2.Calculators);
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                //calculators start with inputs and outputs
                ocGroup2.Outcomes = new List<Outcome>();
                //altern
                int a = 0;
                foreach (var g in qry)
                {
                    int i = 0;
                    //the full collection gets aggregated into outcome
                    Outcome outcome = new Outcome();
                    foreach (Outcome oc in g)
                    {
                        if (i == 0)
                        {
                            outcome = new Outcome(this.GCCalculatorParams, oc);
                            outcome.Outputs = new List<Extensions.Output>();
                            outcome.Calculators = new List<Calculator1>();
                            outcome.Alternative2 = a;
                            outcome.Observations = g.Count();
                        }
                        //calculators start with inputs and outputs
                        if (oc.Outputs != null)
                        {
                            foreach (var output in oc.Outputs)
                            {
                                Output output1 = new Output(this.GCCalculatorParams, output);
                                output1.Calculators = new List<Calculator1>();
                                //this must be i
                                output1.Alternative2 = i;
                                //the parent oc.amount must come from correct oc, not the aggregated oc
                                //since the outputs are being multiplied by parent, no need to double count in parent
                                output1.Multiplier = oc.Multiplier * output1.Multiplier;
                                if (output.Calculators != null)
                                {
                                    MN1AnalyzerHelper.AddOutputCalculators(output.Calculators, output1.Calculators);
                                }
                                outcome.Outputs.Add(output1);
                            }
                        }
                        i += 1;
                    }
                    ocGroup2.Outcomes.Add(outcome);
                    a++;
                }
            }
            return ocGroup2;
        }
        private bool SetOutcomeMN1StockTotals(OutcomeGroup ocGroup)
        {
            bool bHasTotals = false;
            bool bHasAnalysis = false;
            if (ocGroup.Outcomes != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock ocGroupStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                ocGroupStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(ocGroup, ocGroupStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var OCs = ocGroup.Outcomes.OrderBy(oc => oc.Date);
                //run the oc analysis
                if (OCs != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var oc in OCs)
                    {
                        bHasAnalysis = SetOutcomeMN1StockTotals(oc);
                        MN1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                    }
                    //add ancestors needed for change analysis
                    AddAncestorOutcomeCalcs(calcs);
                    //run the analysis
                    bHasTotals = ocGroupStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutcomes(ocGroupStock, OCs);
                    ocGroup.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(ocGroup, ocGroupStock);
                    ocGroup.Calculators.Add(ocGroupStock);
                    //replace this.outcomegroup
                    this.OutcomeGroup = ocGroup;
                }
            }
            return bHasTotals;
        }
        private bool SetOutcomeMN1StockTotals(Outcome oc)
        {
            bool bHasTotals = false;
            //add the output stock totals using the oc
            //assume that good outputs means good totals
            bHasTotals = SetOutputMN1StockTotals(oc);
            //add the oc stock totals using the OCs.Outputs collection that now hold outputstock calcs
            //add indicator1stocks to corresponding MandE element
            MN1Stock mn1StockMember = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //need the options
            mn1StockMember.CopyCalculatorProperties(this.MN1DescendentStock);
            CopyBaseElementProperties(oc, mn1StockMember);
            //children outputs were already multiplied by correct oc.Multiplier 
            mn1StockMember.Multiplier = 1; 
            if (oc.Outputs != null)
            {
                List<Calculator1> calcs = new List<Calculator1>();
                foreach (var output in oc.Outputs)
                {
                    MN1AnalyzerHelper.AddCalculators(output.Calculators, calcs);
                }
                //some analyzers only sum totals for the calcs (i.e. no changes in ins needed)
                mn1StockMember.CalcParameters.CurrentElementNodeName
                    = Output.OUTPUT_PRICE_TYPES.output.ToString();
                //run the analyses by aggregating the input.calcs
                bool bHasAnalysis = mn1StockMember.RunAnalyses(calcs);
            }
            //don't double use the multiplier
            mn1StockMember.Multiplier = 1;
            oc.Calculators = new List<Calculator1>();
            //lcaStockMember.Total, or .Stats holds aggregated numbers
            SetCalculatorId(oc, mn1StockMember);
            oc.Calculators.Add(mn1StockMember);
            return bHasTotals;
        }
        private bool SetOutputMN1StockTotals(Outcome oc)
        {
            bool bHasTotals = false;
            //run the output analysis using this.MN1DescendentStock (so that ancestors can use them)
            if (oc.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock mn1Stock = new MN1Stock(this.GCCalculatorParams,
                    this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                mn1Stock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(oc, mn1Stock);
                foreach (var output in oc.Outputs)
                {
                    //add indicator1stocks to corresponding MandE element
                    MN1Stock mn1StockMember = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    mn1StockMember.CopyCalculatorProperties(this.MN1DescendentStock);
                    CopyBaseElementProperties(output, mn1StockMember);
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        //but still needs ocamount, aohamount, and capamount
                        mn1StockMember.Multiplier = output.Multiplier;
                    }
                    else
                    {
                        //existing output.Multiplier comes from correct parent oc before ocs were aggregated
                        mn1StockMember.Multiplier = output.Multiplier;
                    }
                    if (output.Calculators != null)
                    {
                        mn1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                        //add a benefit calculator to subp1s collection
                        //some analyzers use the subp1s (total1), so they must be copied correctly into new stock objects
                        mn1StockMember.MNSR2Stock.AddOutputCalcsToStock(output.Calculators);
                        //MN1AnalyzerHelper.AddOutputCalculators(output.Calculators, mn1StockMember.MNSR2Stock.FoodNutritionCalcs);
                    }
                    //add the stockmember to the base stock
                    bHasTotals = mn1StockMember.RunAnalyses();
                    //don't double count the multiplier
                    mn1StockMember.Multiplier = 1;
                    SetCalculatorId(output, mn1StockMember);
                    output.Calculators.Add(mn1StockMember);
                }
            }
            return bHasTotals;
        }

        
        //inputs
        public bool SetInputAggregation(List<Input> inputs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry1 = inputs
                        .GroupBy(m => m.TypeId.ToString());
                    InputGroup ocGroup1 = GetInputMNsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = SetInputMN1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry2 = inputs
                        .GroupBy(m => m.GroupId.ToString());
                    InputGroup ocGroup2 = GetInputMNsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = SetInputMN1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry3 = inputs
                        .GroupBy(m => m.Label);
                    InputGroup ocGroup3 = GetInputMNsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = SetInputMN1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        InputGroup ocGroup4 = GetInputMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetInputMN1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        InputGroup ocGroup4 = GetInputMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetInputMN1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .GroupBy(m => m.Id.ToString());
                        InputGroup ocGroup4 = GetInputMNsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetInputMN1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private InputGroup GetInputMNsAggregation(
            IEnumerable<System.Linq.IGrouping<string, Input>> qry)
        {
            if (this.InputGroup == null)
                this.InputGroup = new InputGroup();
            //use new, not byref, objects
            InputGroup inputGroup = new InputGroup(this.GCCalculatorParams, this.InputGroup);
            AddStockCalculators(this.InputGroup.Calculators, inputGroup.Calculators);
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                //calculators start with inputs and outputs
                inputGroup.Inputs = new List<Input>();
                //altern
                int a = 0;
                foreach (var g in qry)
                {
                    int i = 0;
                    //the full collection gets aggregated into opComp
                    Input inputNew = new Input();
                    foreach (Input input in g)
                    {
                        if (i == 0)
                        {
                            inputNew = new Input(this.GCCalculatorParams, input);
                            inputNew.Inputs = new List<Extensions.Input>();
                            inputNew.Calculators = new List<Calculator1>();
                            inputNew.Alternative2 = a;
                            inputNew.Observations = g.Count();
                        }
                        if (input.Calculators != null)
                        {
                            MN1AnalyzerHelper.AddInputCalculators(input.Calculators, inputNew.Calculators);
                        }
                        if (input.Inputs != null)
                        {
                            foreach (var input2 in input.Inputs)
                            {
                                Input input1 = new Input(this.GCCalculatorParams, input2);
                                input1.Calculators = new List<Calculator1>();
                                //this must be i
                                input1.Alternative2 = i;
                                if (input2.Calculators != null)
                                {
                                    MN1AnalyzerHelper.AddInputCalculators(input2.Calculators, input1.Calculators);
                                }
                                inputNew.Inputs.Add(input1);
                            }
                        }
                        i += 1;
                    }
                    inputGroup.Inputs.Add(inputNew);
                    a++;
                }
            }
            return inputGroup;
        }
        private bool SetInputMN1StockTotals(InputGroup inputGroup)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (inputGroup.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock inGroupStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                inGroupStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(inputGroup, inGroupStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var Inputs = inputGroup.Inputs.OrderBy(oc => oc.Date);
                //run the oc analysis
                if (Inputs != null)
                {
                    //note that inputs don't run independent input calcs
                    //they only serve as input series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var input in Inputs)
                    {
                        //add the descendent calcs
                        bHasAnalysis = SetInputMN1StockTotals(input);
                        MN1AnalyzerHelper.AddCalculators(input.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = inGroupStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToInputs(inGroupStock, Inputs);
                    inputGroup.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(inputGroup, inGroupStock);
                    inputGroup.Calculators.Add(inGroupStock);
                    //reset this.InputGroup
                    this.InputGroup = inputGroup;
                }
            }
            return bHasTotals;
        }
        private bool SetInputMN1StockTotals(Input input)
        {
            bool bHasAnalysis = true;
            //run the input analysis
            if (input != null)
            {
                if ((this.GCCalculatorParams.StartingDocToCalcNodeName
                    == Input.INPUT_PRICE_TYPES.inputgroup.ToString()))
                {
                    //run the base calcs, but don't run analyses
                    bHasAnalysis = SetInputSeriesMN1BaseStockTotals(input);
                }
                else
                {
                    //ok to run analyses
                    if (input.Inputs != null)
                    {
                        //add analyses to input
                        bHasAnalysis = SetInputSeriesAggregation(input);

                    }
                }
            }
            return bHasAnalysis;
        }
        //can be run at both input and input series levels
        private bool SetInputSeriesMN1BaseStockTotals(Input inputserie)
        {
            bool bHasAnalysis = true;
            bool bHasTotals = false;
            if (inputserie.Calculators != null)
            {
                //each mnc calculator is considered a unique input observation needing a unique stock total
                //i.e. if this input is an aggregation of five inputs with mnccalcs, that's five observations
                List<Calculator1> stocks = new List<Calculator1>();
                foreach (Calculator1 calc in inputserie.Calculators)
                {
                    //add indicator1stocks to corresponding MandE element
                    MN1Stock mn1StockMember = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    mn1StockMember.CopyCalculatorProperties(this.MN1DescendentStock);
                    CopyBaseElementProperties(inputserie, calc);
                    //but keep unique id
                    mn1StockMember.Id = calc.Id;
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        //but still needs ocamount, aohamount, and capamount
                        mn1StockMember.Multiplier = inputserie.Multiplier;
                    }
                    else
                    {
                        mn1StockMember.Multiplier = inputserie.Multiplier;
                    }
                    mn1StockMember.MNSR1Stock.AddInputCalcToStock(calc);
                    //MN1AnalyzerHelper.AddInputCalculator(calc, mn1StockMember.MNSR1Stock.FoodNutritionCalcs);
                    if (mn1StockMember.MNSR1Stock.FoodNutritionCalcs.Count > 0)
                    {
                        bHasAnalysis = mn1StockMember.RunAnalyses();
                        //don't double count the multiplier
                        mn1StockMember.Multiplier = 1;
                        if (bHasAnalysis)
                        {
                            stocks.Add(mn1StockMember);
                            bHasTotals = true;
                        }
                    }
                }
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString()
                    || MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
                {
                    foreach (var stock in stocks)
                    {
                        //don't set calcid for multiple stocks
                        inputserie.Calculators.Add(stock);
                    }
                }
                else
                {
                    //this gives each input and output correct number of mnccalc observations
                    MN1Stock mn1StockMember2 = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //run the series analyses by aggregating the calcs
                    bHasTotals = mn1StockMember2.RunAnalyses(stocks);
                    if (bHasTotals)
                    {
                        inputserie.Calculators = new List<Calculator1>();
                        //each inputserie has the correct observation
                        SetCalculatorId(inputserie, mn1StockMember2);
                        inputserie.Calculators.Add(mn1StockMember2);
                        //to use same number of obs in parent would need to pass back the stocks
                        //and use that collection to run parent
                    }
                }
            }
            return bHasTotals;
        }
        //inputseries
        public bool SetInputSeriesAggregation(Input baseInput)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry1 = baseInput.Inputs
                        .GroupBy(m => m.TypeId.ToString());
                    Input input1 = GetInputSeriesMNsAggregation(baseInput, qry1);
                    if (input1 != null)
                    {
                        bHasGroups = SetInputSeriesMN1StockTotals(input1);
                        if (bHasGroups)
                        {
                            //copy inputseries w analyses to input
                            CopyInputSeriesToInput(baseInput, input1);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry2 = baseInput.Inputs
                        .GroupBy(m => m.GroupId.ToString());
                    Input input2 = GetInputSeriesMNsAggregation(baseInput, qry2);
                    if (input2 != null)
                    {
                        bHasGroups = SetInputSeriesMN1StockTotals(input2);
                        if (bHasGroups)
                        {
                            //copy inputseries w analyses to input
                            CopyInputSeriesToInput(baseInput, input2);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry3 = baseInput.Inputs
                        .GroupBy(m => m.Label);
                    Input input3 = GetInputSeriesMNsAggregation(baseInput, qry3);
                    if (input3 != null)
                    {
                        bHasGroups = SetInputSeriesMN1StockTotals(input3);
                        if (bHasGroups)
                        {
                            //copy inputseries w analyses to input
                            CopyInputSeriesToInput(baseInput, input3);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = baseInput.Inputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        Input input4 = GetInputSeriesMNsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = SetInputSeriesMN1StockTotals(input4);
                            if (bHasGroups)
                            {
                                //copy inputseries w analyses to input
                                CopyInputSeriesToInput(baseInput, input4);
                            }
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = baseInput.Inputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        Input input4 = GetInputSeriesMNsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = SetInputSeriesMN1StockTotals(input4);
                            if (bHasGroups)
                            {
                                //copy inputseries w analyses to input
                                CopyInputSeriesToInput(baseInput, input4);
                            }
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = baseInput.Inputs
                            .GroupBy(m => m.Id.ToString());
                        Input input4 = GetInputSeriesMNsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = SetInputSeriesMN1StockTotals(input4);
                            if (bHasGroups)
                            {
                                //copy inputseries w analyses to input
                                CopyInputSeriesToInput(baseInput, input4);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private void CopyInputSeriesToInput(Input baseInput, Input newInput)
        {
            //copy inputseries w analyses to input
            baseInput.Inputs = new List<Input>();
            foreach (var inputserie in newInput.Inputs)
            {
                baseInput.Inputs.Add(inputserie);
            }
            //copy stock calculators
            foreach (var calc in newInput.Calculators)
            {
                baseInput.Calculators.Add(calc);
            }
        }
        private Input GetInputSeriesMNsAggregation(Input baseInput,
            IEnumerable<System.Linq.IGrouping<string, Input>> qry)
        {
            //use new, not byref, objects
            Input newInput = new Input(this.GCCalculatorParams, baseInput);
            AddStockCalculators(baseInput.Calculators, newInput.Calculators);
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                //calculators start with inputs and outputs
                newInput.Inputs = new List<Input>();
                //altern
                int a = 0;
                foreach (var g in qry)
                {
                    int i = 0;
                    //the full collection gets aggregated into opComp
                    Input inputNew = new Input();
                    foreach (Input input in g)
                    {
                        if (i == 0)
                        {
                            inputNew = new Input(this.GCCalculatorParams, input);
                            inputNew.Inputs = new List<Extensions.Input>();
                            inputNew.Calculators = new List<Calculator1>();
                            inputNew.Alternative2 = a;
                            inputNew.Observations = g.Count();
                        }
                        if (input.Calculators != null)
                        {
                            MN1AnalyzerHelper.AddInputCalculators(input.Calculators, inputNew.Calculators);
                        }
                        i += 1;
                    }
                    newInput.Inputs.Add(inputNew);
                    a++;
                }
            }
            return newInput;
        }
        private bool SetInputSeriesMN1StockTotals(Input input)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (input.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock inputStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                inputStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(input, inputStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var InputSeries = input.Inputs.OrderBy(i => i.Date);
                //run the oc analysis
                if (InputSeries != null)
                {
                    //note that inputs don't run independent input calcs
                    //they only serve as input series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var inputserie in InputSeries)
                    {
                        //add the descendent calcs
                        bHasAnalysis = SetInputSeriesMN1BaseStockTotals(inputserie);
                        MN1AnalyzerHelper.AddCalculators(inputserie.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = inputStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToInputs(inputStock, InputSeries);
                    //set regular calcs
                    input.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(input, inputStock);
                    input.Calculators.Add(inputStock);
                    //unlike other patterns, this doesn't use this.Input because of byref
                }
            }
            return bHasTotals;
        }
        private void AddCalculatorsToInputs(MN1Stock inputStock, 
            IOrderedEnumerable<Input> inputs)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                //transfer inputStock.Change1.Stocks to input
                //keep inputStock.Change1
                if (inputStock.Stocks != null)
                {
                    foreach (var input in inputs)
                    {
                        if (input.Calculators != null)
                        {
                            foreach (var calc in input.Calculators)
                            {
                                bool bBreak = false;
                                foreach (var change in inputStock.Stocks)
                                {
                                    if (calc.Id == change.Id)
                                    {
                                        //replace the stock calculations
                                        //don't need siblings, so new list
                                        input.Calculators = new List<Calculator1>();
                                        SetCalcId(input, change);
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            SetCalcId(input, change.Progress1);
                                        }
                                        else
                                        {
                                            SetCalcId(input, change.Change1);
                                        }
                                        input.Calculators.Add(change);
                                        inputStock.Stocks.Remove(change);
                                        //leave the loops
                                        bBreak = true;
                                        break;
                                    }
                                }
                                if (bBreak)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddCalculatorsToOutputs(MN1Stock outputStock,
            IOrderedEnumerable<Output> outputs)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                //transfer outputStock.Stocks to output
                //keep outputStock.Change1
                if (outputStock.Stocks != null)
                {
                    foreach (var output in outputs)
                    {
                        if (output.Calculators != null)
                        {
                            foreach (var calc in output.Calculators)
                            {
                                bool bBreak = false;
                                foreach (var change in outputStock.Stocks)
                                {
                                    if (calc.Id == change.Id)
                                    {
                                        //replace the stock calculations
                                        //don't need siblings, so new list
                                        output.Calculators = new List<Calculator1>();
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            SetCalcId(output, change.Progress1);
                                        }
                                        else
                                        {
                                            SetCalcId(output, change.Change1);
                                        }
                                        SetCalcId(output, change);
                                        output.Calculators.Add(change);
                                        outputStock.Stocks.Remove(change);
                                        //leave the loops
                                        bBreak = true;
                                        break;
                                    }
                                }
                                if (bBreak)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddCalculatorsToOCs(MN1Stock ocStock,
            IOrderedEnumerable<OperationComponent> ocs)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                if (ocStock.Stocks != null)
                {
                    foreach (var oc in ocs)
                    {
                        if (oc.Calculators != null)
                        {
                            foreach (var calc in oc.Calculators)
                            {
                                bool bBreak = false;
                                foreach (var change in ocStock.Stocks)
                                {
                                    if (calc.Id == change.Id)
                                    {
                                        //replace the stock calculations
                                        //don't need siblings, so new list
                                        oc.Calculators = new List<Calculator1>();
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            SetCalcId(oc, change.Progress1);
                                        }
                                        else
                                        {
                                            SetCalcId(oc, change.Change1);
                                        }
                                        SetCalcId(oc, change);
                                        oc.Calculators.Add(change);
                                        ocStock.Stocks.Remove(change);
                                        //leave the loops
                                        bBreak = true;
                                        break;
                                    }
                                }
                                if (bBreak)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddCalculatorsToOutcomes(MN1Stock ocStock,
            IOrderedEnumerable<Outcome> ocs)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                if (ocStock.Stocks != null)
                {
                    foreach (var oc in ocs)
                    {
                        if (oc.Calculators != null)
                        {
                            foreach (var calc in oc.Calculators)
                            {
                                bool bBreak = false;
                                foreach (var change in ocStock.Stocks)
                                {
                                    if (calc.Id == change.Id)
                                    {
                                        //replace the stock calculations
                                        //don't need siblings, so new list
                                        oc.Calculators = new List<Calculator1>();
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            SetCalcId(oc, change.Progress1);
                                        }
                                        else
                                        {
                                            SetCalcId(oc, change.Change1);
                                        }
                                        SetCalcId(oc, change);
                                        oc.Calculators.Add(change);
                                        ocStock.Stocks.Remove(change);
                                        //leave the loops
                                        bBreak = true;
                                        break;
                                    }
                                }
                                if (bBreak)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddCalculatorsToTPs(MN1Stock biStock,
            IOrderedEnumerable<TimePeriod> tps)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
                //&& this.GCCalculatorParams.AnalyzerParms.AnalyzerType != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                if (biStock.Stocks != null)
                {
                    foreach (var tp in tps)
                    {
                        if (tp.Calculators != null)
                        {
                            foreach (var calc in tp.Calculators)
                            {
                                bool bBreak = false;
                                foreach (var change in biStock.Stocks)
                                {
                                    if (calc.Id == change.Id)
                                    {
                                        //replace the stock calculations
                                        //don't need siblings, so new list
                                        tp.Calculators = new List<Calculator1>();
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            SetCalcId(tp, change.Progress1);
                                        }
                                        else
                                        {
                                            SetCalcId(tp, change.Change1);
                                        }
                                        SetCalcId(tp, change);
                                        tp.Calculators.Add(change);
                                        biStock.Stocks.Remove(change);
                                        //leave the loops
                                        bBreak = true;
                                        break;
                                    }
                                }
                                if (bBreak)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddCalculatorsToBIs(MN1Stock biGroupStock,
            IOrderedEnumerable<BudgetInvestment> bis)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
               //&& this.GCCalculatorParams.AnalyzerParms.AnalyzerType != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                if (biGroupStock.Stocks != null)
                {
                    foreach (var bi in bis)
                    {
                        if (bi.Calculators != null)
                        {
                            foreach (var calc in bi.Calculators)
                            {
                                bool bBreak = false;
                                foreach (var change in biGroupStock.Stocks)
                                {
                                    if (calc.Id == change.Id)
                                    {
                                        //replace the stock calculations
                                        //don't need siblings, so new list
                                        bi.Calculators = new List<Calculator1>();
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            SetCalcId(bi, change.Progress1);
                                        }
                                        else
                                        {
                                            SetCalcId(bi, change.Change1);
                                        }
                                        SetCalcId(bi, change);
                                        bi.Calculators.Add(change);
                                        biGroupStock.Stocks.Remove(change);
                                        //leave the loops
                                        bBreak = true;
                                        break;
                                    }
                                }
                                if (bBreak)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        //outputs
        public bool SetOutputAggregation(List<Output> outputs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry1 = outputs
                        .GroupBy(m => m.TypeId.ToString());
                    OutputGroup outputGroup1 = GetOutputMNsAggregation(qry1);
                    if (outputGroup1 != null)
                    {
                        bHasGroups = SetOutputMN1StockTotals(outputGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry2 = outputs
                        .GroupBy(m => m.GroupId.ToString());
                    OutputGroup outputGroup2 = GetOutputMNsAggregation(qry2);
                    if (outputGroup2 != null)
                    {
                        bHasGroups = SetOutputMN1StockTotals(outputGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry3 = outputs
                        .GroupBy(m => m.Label);
                    OutputGroup outputGroup3 = GetOutputMNsAggregation(qry3);
                    if (outputGroup3 != null)
                    {
                        bHasGroups = SetOutputMN1StockTotals(outputGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        OutputGroup outputGroup4 = GetOutputMNsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = SetOutputMN1StockTotals(outputGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        OutputGroup outputGroup4 = GetOutputMNsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = SetOutputMN1StockTotals(outputGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .GroupBy(m => m.Id.ToString());
                        OutputGroup outputGroup4 = GetOutputMNsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = SetOutputMN1StockTotals(outputGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private OutputGroup GetOutputMNsAggregation(
            IEnumerable<System.Linq.IGrouping<string, Output>> qry)
        {
            //use new, not byref, objects
            if (this.OutputGroup == null)
                this.OutputGroup = new OutputGroup();
            OutputGroup outputGroup = new OutputGroup(this.GCCalculatorParams, this.OutputGroup);
            AddStockCalculators(this.OutputGroup.Calculators, outputGroup.Calculators);
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                //calculators start with inputs and outputs
                outputGroup.Outputs = new List<Output>();
                //altern
                int a = 0;
                foreach (var g in qry)
                {
                    int i = 0;
                    //the full collection gets aggregated into output
                    Output outputNew = new Output();
                    foreach (Output output in g)
                    {
                        if (i == 0)
                        {
                            outputNew = new Output(this.GCCalculatorParams, output);
                            outputNew.Outputs = new List<Extensions.Output>();
                            outputNew.Calculators = new List<Calculator1>();
                            outputNew.Alternative2 = a;
                            outputNew.Observations = g.Count();
                        }
                        if (output.Calculators != null)
                        {
                            MN1AnalyzerHelper.AddOutputCalculators(output.Calculators, outputNew.Calculators);
                        }
                        if (output.Outputs != null)
                        {
                            foreach (var output2 in output.Outputs)
                            {
                                Output output1 = new Output(this.GCCalculatorParams, output2);
                                output1.Calculators = new List<Calculator1>();
                                //this must be i
                                output1.Alternative2 = i;
                                if (output2.Calculators != null)
                                {
                                    MN1AnalyzerHelper.AddOutputCalculators(output2.Calculators, output1.Calculators);
                                }
                                outputNew.Outputs.Add(output1);
                            }
                        }
                        i += 1;
                    }
                    outputGroup.Outputs.Add(outputNew);
                    a++;
                }
            }
            return outputGroup;
        }
        private bool SetOutputMN1StockTotals(OutputGroup outputGroup)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (outputGroup.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock outGroupStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                outGroupStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(outputGroup, outGroupStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var Outputs = outputGroup.Outputs.OrderBy(oc => oc.Date);
                //run the oc analysis
                if (Outputs != null)
                {
                    //note that outputs don't run independent output calcs
                    //they only serve as output series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var output in Outputs)
                    {
                        //add the descendent calcs
                        bHasAnalysis = SetOutputMN1StockTotals(output);
                        MN1AnalyzerHelper.AddCalculators(output.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = outGroupStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutputs(outGroupStock, Outputs);
                    outputGroup.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(outputGroup, outGroupStock);
                    outputGroup.Calculators.Add(outGroupStock);
                    //reset this.OutputGroup
                    this.OutputGroup = outputGroup;
                }
            }
            return bHasTotals;
        }
        private bool SetOutputMN1StockTotals(Output output)
        {
            bool bHasAnalysis = true;
            //run the output analysis
            if (output != null)
            {
                if ((this.GCCalculatorParams.StartingDocToCalcNodeName
                    == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString()))
                {
                    //run the base calcs, but don't run analyses
                    bHasAnalysis = SetOutputSeriesMN1BaseStockTotals(output);
                }
                else
                {
                    //ok to run analyses
                    if (output.Outputs != null)
                    {
                        //add analyses to output
                        bHasAnalysis = SetOutputSeriesAggregation(output);
                    }
                }
            }
            return bHasAnalysis;
        }
        //can be run at both output and output series levels
        private bool SetOutputSeriesMN1BaseStockTotals(Output outputserie)
        {
            bool bHasAnalysis = true;
            bool bHasTotals = false;
            if (outputserie.Calculators != null)
            {
                //each mnc calculator is considered a unique output observation needing a unique stock total
                //i.e. if this output is an aggregation of five outputs with mnccalcs, that's five observations
                List<Calculator1> stocks = new List<Calculator1>();
                foreach (Calculator1 calc in outputserie.Calculators)
                {
                    //add indicator1stocks to corresponding MandE element
                    MN1Stock mn1StockMember = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    mn1StockMember.CopyCalculatorProperties(this.MN1DescendentStock);
                    CopyBaseElementProperties(outputserie, calc);
                    //but keep unique id
                    mn1StockMember.Id = calc.Id;
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        //but still needs ocamount, aohamount, and capamount
                        mn1StockMember.Multiplier = outputserie.Multiplier;
                    }
                    else
                    {
                        mn1StockMember.Multiplier = outputserie.Multiplier;
                    }
                    mn1StockMember.MNSR2Stock.AddOutputCalcToStock(calc);
                    //MN1AnalyzerHelper.AddOutputCalculator(calc, mn1StockMember.MNSR2Stock.FoodNutritionCalcs);
                    if (mn1StockMember.MNSR2Stock.FoodNutritionCalcs.Count > 0)
                    {
                        bHasAnalysis = mn1StockMember.RunAnalyses();
                        //don't double count the multiplier
                        mn1StockMember.Multiplier = 1;
                        if (bHasAnalysis)
                        {
                            stocks.Add(mn1StockMember);
                            bHasTotals = true;
                        }
                    }
                }
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString()
                    || MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
                {
                    foreach (var stock in stocks)
                    {
                        //don't set calcid for multiple stocks
                        outputserie.Calculators.Add(stock);
                    }
                }
                else
                {
                    //this gives each output and output correct number of mnccalc observations
                    MN1Stock mn1StockMember2 = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //run the series analyses by aggregating the calcs
                    bHasTotals = mn1StockMember2.RunAnalyses(stocks);
                    if (bHasTotals)
                    {
                        outputserie.Calculators = new List<Calculator1>();
                        //each outputserie has the correct observation
                        SetCalculatorId(outputserie, mn1StockMember2);
                        outputserie.Calculators.Add(mn1StockMember2);
                        //to use same number of obs in parent would need to pass back the stocks
                        //and use that collection to run parent
                    }
                }
            }
            return bHasTotals;
        }
        //outputseries
        public bool SetOutputSeriesAggregation(Output baseOutput)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry1 = baseOutput.Outputs
                        .GroupBy(m => m.TypeId.ToString());
                    Output output1 = GetOutputSeriesMNsAggregation(baseOutput, qry1);
                    if (output1 != null)
                    {
                        bHasGroups = SetOutputSeriesMN1StockTotals(output1);
                        if (bHasGroups)
                        {
                            //copy outputseries w analyses to output
                            CopyOutputSeriesToOutput(baseOutput, output1);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry2 = baseOutput.Outputs
                        .GroupBy(m => m.GroupId.ToString());
                    Output output2 = GetOutputSeriesMNsAggregation(baseOutput, qry2);
                    if (output2 != null)
                    {
                        bHasGroups = SetOutputSeriesMN1StockTotals(output2);
                        if (bHasGroups)
                        {
                            //copy outputseries w analyses to output
                            CopyOutputSeriesToOutput(baseOutput, output2);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry3 = baseOutput.Outputs
                        .GroupBy(m => m.Label);
                    Output output3 = GetOutputSeriesMNsAggregation(baseOutput, qry3);
                    if (output3 != null)
                    {
                        bHasGroups = SetOutputSeriesMN1StockTotals(output3);
                        if (bHasGroups)
                        {
                            //copy outputseries w analyses to output
                            CopyOutputSeriesToOutput(baseOutput, output3);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = baseOutput.Outputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        Output output4 = GetOutputSeriesMNsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = SetOutputSeriesMN1StockTotals(output4);
                            if (bHasGroups)
                            {
                                //copy outputseries w analyses to output
                                CopyOutputSeriesToOutput(baseOutput, output4);
                            }
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = baseOutput.Outputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        Output output4 = GetOutputSeriesMNsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = SetOutputSeriesMN1StockTotals(output4);
                            if (bHasGroups)
                            {
                                //copy outputseries w analyses to output
                                CopyOutputSeriesToOutput(baseOutput, output4);
                            }
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = baseOutput.Outputs
                            .GroupBy(m => m.Id.ToString());
                        Output output4 = GetOutputSeriesMNsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = SetOutputSeriesMN1StockTotals(output4);
                            if (bHasGroups)
                            {
                                //copy outputseries w analyses to output
                                CopyOutputSeriesToOutput(baseOutput, output4);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private void CopyOutputSeriesToOutput(Output baseOutput, Output newOutput)
        {
            //copy outputseries w analyses to output
            baseOutput.Outputs = new List<Output>();
            foreach (var outputserie in newOutput.Outputs)
            {
                baseOutput.Outputs.Add(outputserie);
            }
            //copy stock calculators
            foreach (var calc in newOutput.Calculators)
            {
                baseOutput.Calculators.Add(calc);
            }
        }
        private Output GetOutputSeriesMNsAggregation(Output baseOutput,
            IEnumerable<System.Linq.IGrouping<string, Output>> qry)
        {
            //use new, not byref, objects
            Output newOutput = new Output(this.GCCalculatorParams, baseOutput);
            AddStockCalculators(baseOutput.Calculators, newOutput.Calculators);
            //create a new tp to hold new collections
            if (qry.Count() > 0)
            {
                //calculators start with outputs and outputs
                newOutput.Outputs = new List<Output>();
                //altern
                int a = 0;
                foreach (var g in qry)
                {
                    int i = 0;
                    //the full collection gets aggregated into opComp
                    Output outputNew = new Output();
                    foreach (Output output in g)
                    {
                        if (i == 0)
                        {
                            outputNew = new Output(this.GCCalculatorParams, output);
                            outputNew.Outputs = new List<Extensions.Output>();
                            outputNew.Calculators = new List<Calculator1>();
                            outputNew.Alternative2 = a;
                            outputNew.Observations = g.Count();
                        }
                        if (output.Calculators != null)
                        {
                            MN1AnalyzerHelper.AddOutputCalculators(output.Calculators, outputNew.Calculators);
                        }
                        i += 1;
                    }
                    newOutput.Outputs.Add(outputNew);
                    a++;
                }
            }
            return newOutput;
        }
        private bool SetOutputSeriesMN1StockTotals(Output output)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (output.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                MN1Stock outputStock = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                outputStock.CopyCalculatorProperties(this.MN1DescendentStock);
                CopyBaseElementProperties(output, outputStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var OutputSeries = output.Outputs.OrderBy(o => o.Date);
                //run the oc analysis
                if (OutputSeries != null)
                {
                    //note that outputs don't run independent output calcs
                    //they only serve as output series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var outputserie in OutputSeries)
                    {
                        //add the descendent calcs
                        bHasAnalysis = SetOutputSeriesMN1BaseStockTotals(outputserie);
                        MN1AnalyzerHelper.AddCalculators(outputserie.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = outputStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutputs(outputStock, OutputSeries);
                    output.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(output, outputStock);
                    output.Calculators.Add(outputStock);
                    //unlike other patterns, this doesn't use this.Output because of byref
                }
            }
            return bHasTotals;
        }

        public static void CopyBaseElementProperties(Calculator1 baseElement, Calculator1 calc)
        {
            //alt2 is stored with the base element for some elements (ocs and outcomes, tps)
            if (baseElement.Alternative2 != 0)
            {
                //each aggregated observation is given a unique alt2
                calc.Alternative2 = baseElement.Alternative2;
            }
            //some analyses are aggregated by element date
            calc.Date = baseElement.Date;
            //some elements have been aggregated, note the number of obs in agg
            calc.Observations = baseElement.Observations;
            if (calc.Observations == 0)
            {
                //those that haven't been aggregated
                calc.Observations = 1;
            }
            //alternatives and progress analysis
            calc.AlternativeType = baseElement.AlternativeType;
            calc.TargetType = baseElement.TargetType;
            calc.ChangeType = baseElement.ChangeType;
            calc.Label = baseElement.Label;
            calc.GroupId = baseElement.GroupId;
            calc.TypeId = baseElement.TypeId;
        }
        public static void CopyBaseElementProperties(Calculator1 baseElement, MN1Stock stock)
        {
            //give the stock a unique name and id
            if (!string.IsNullOrEmpty(baseElement.Name))
            {
                stock.Name = baseElement.Name;
            }
            if (baseElement.Id != 0)
            {
                if (baseElement.CalculatorId == 0)
                {
                    if (stock.CalculatorId != 0)
                    {
                        baseElement.CalculatorId = stock.CalculatorId;
                    }
                    else
                    {
                        baseElement.CalculatorId = stock.Id;
                    }
                }
                stock.Id = baseElement.Id;
            }
            //alt2 is stored with the base element for some elements (ocs and outcomes, tps)
            if (baseElement.Alternative2 != 0)
            {
                //each aggregated observation is given a unique alt2
                stock.Alternative2 = baseElement.Alternative2;
            }
            //some analyses are aggregated by element date
            stock.Date = baseElement.Date;
            //some elements have been aggregated, note the number of obs in agg
            if (baseElement.Observations != 0)
            {
                stock.Observations = baseElement.Observations;
            }
            if (stock.Observations == 0)
            {
                //those that haven't been aggregated
                stock.Observations = 1;
            }
            //alternatives and progress analysis
            if (!string.IsNullOrEmpty(baseElement.AlternativeType))
            {
                stock.AlternativeType = baseElement.AlternativeType;
                if (string.IsNullOrEmpty(stock.AlternativeType))
                {
                    //those that haven't been aggregated
                    stock.AlternativeType = Calculator1.ALTERNATIVE_TYPES.none.ToString();
                }
            }
            if (!string.IsNullOrEmpty(baseElement.TargetType))
            {
                stock.TargetType = baseElement.TargetType;
                if (string.IsNullOrEmpty(stock.TargetType))
                {
                    //those that haven't been aggregated
                    stock.TargetType = Calculator1.ALTERNATIVE_TYPES.none.ToString();
                }
            }
            if (!string.IsNullOrEmpty(baseElement.ChangeType))
            {
                stock.ChangeType = baseElement.ChangeType;
                if (string.IsNullOrEmpty(stock.ChangeType))
                {
                    //those that haven't been aggregated
                    stock.ChangeType = Calculator1.CHANGE_TYPES.none.ToString();
                }
            }
            if (!string.IsNullOrEmpty(baseElement.Label))
            {
                stock.Label = baseElement.Label;
                if (string.IsNullOrEmpty(stock.Label))
                {
                    //those that haven't been aggregated
                    stock.Label = Calculator1.ALTERNATIVE_TYPES.none.ToString();
                }
            }
            if (baseElement.GroupId == 0)
            {
                stock.GroupId = baseElement.GroupId;
            }
            if (baseElement.TypeId == 0)
            {
                stock.TypeId = baseElement.TypeId;
            }
            //set missing params when needed in calling procedure
            stock.CalcParameters = new CalculatorParameters();
        }
        
        private void AddStockCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                MN1Stock teststock = new MN1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        MN1Stock stock = (MN1Stock)calc;
                        if (stock != null)
                        {
                            MN1Stock me = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalMN1StockProperties(stock);
                            newcalcs.Add(me);
                        }
                    }
                }
            }
        }
        private void AddStockInputCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                MN1Stock teststock = new MN1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        MN1Stock stock = (MN1Stock)calc;
                        if (stock != null)
                        {
                            MN1Stock me = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalMN1StockProperties(stock);
                            //some analyzers display the results of the initial cost and benefit calcs
                            if (stock.MNSR1Stock != null)
                            {
                                if (stock.MNSR1Stock.FoodNutritionCalcs != null)
                                {
                                    if (stock.MNSR1Stock.FoodNutritionCalcs.Count > 0)
                                    {
                                        //these are mnc cost calculators not generic subp1s
                                        me.MNSR1Stock = new MNSR01Stock();
                                        me.MNSR1Stock.FoodNutritionCalcs = new List<MNC1Calculator>();
                                        stock.MNSR1Stock.AddFoodCalcsToStock(me.MNSR1Stock.FoodNutritionCalcs);
                                        //MN1AnalyzerHelper.AddInputCalculators(stock.MNSR1Stock.FoodNutritionCalcs, me.MNSR1Stock.FoodNutritionCalcs);
                                    }
                                }
                            }
                            newcalcs.Add(me);
                        }
                    }
                }
            }
        }
        private void AddStockOutputCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                MN1Stock teststock = new MN1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        MN1Stock stock = (MN1Stock)calc;
                        if (stock != null)
                        {
                            MN1Stock me = new MN1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalMN1StockProperties(stock);
                            //some analyzers display the results of the initial cost and benefit calcs
                            if (stock.MNSR2Stock != null)
                            {
                                if (stock.MNSR2Stock.FoodNutritionCalcs != null)
                                {
                                    if (stock.MNSR2Stock.FoodNutritionCalcs.Count > 0)
                                    {
                                        //these are mnc cost calculators not generic subp1s
                                        me.MNSR2Stock = new MNSR02Stock();
                                        me.MNSR2Stock.FoodNutritionCalcs = new List<MNB1Calculator>();
                                        stock.MNSR2Stock.AddFoodCalcsToStock(me.MNSR2Stock.FoodNutritionCalcs);
                                        //MN1AnalyzerHelper.AddOutputCalculators(stock.MNSR2Stock.FoodNutritionCalcs, me.MNSR2Stock.FoodNutritionCalcs);
                                    }

                                }
                            }
                            newcalcs.Add(me);
                        }
                    }
                }
            }
        }
        private void AddAncestorTPCalcs(List<Calculator1> calcs)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                int iNewCalcsCount = calcs.Count;
                //add the base comparator
                AddAncestorBaseTPCalcs(calcs);
                //don't change any calc to actual unless there's a base comparator
                if (iNewCalcsCount < calcs.Count
                    || this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                {
                    //actuals have to be set before base and partials
                    foreach (var calc2 in calcs)
                    {
                        //don't change newly added benchmark calcs
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                        {
                            //must be set correctly in base elements
                            //if (calc2.TargetType
                            //    != Calculator1.TARGET_TYPES.benchmark.ToString())
                            //{
                            //    //ocs compare planned vs actual for each member
                            //    calc2.TargetType
                            //        = Calculator1.TARGET_TYPES.actual.ToString();
                            //}
                        }
                        else
                        {
                            if (calc2.ChangeType
                            != Calculator1.CHANGE_TYPES.baseline.ToString()
                            && calc2.ChangeType
                            != Calculator1.CHANGE_TYPES.xminus1.ToString()
                            && calc2.ChangeType
                            != Calculator1.CHANGE_TYPES.other.ToString())
                            {
                                calc2.ChangeType
                                    = Calculator1.CHANGE_TYPES.current.ToString();
                            }
                        }
                    }
                    BudgetInvestment ancestorBI = GetAntecedentXMinus1BI();
                    if (ancestorBI != null)
                    {
                        if (ancestorBI.TimePeriods != null)
                        {
                            //and the xminus1 comparator
                            foreach (var tp in ancestorBI.TimePeriods)
                            {
                                if (tp.Calculators != null)
                                {
                                    //must be able to distinguish ancestor from current
                                    //not necessary to set existing TargetTypes (they are ignored anyway)
                                    foreach (var calc in tp.Calculators)
                                    {
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            //must be set correctly in base elements
                                            //calc.TargetType
                                            //    = Calculator1.TARGET_TYPES.benchmark.ToString();
                                        }
                                        else
                                        {
                                            calc.ChangeType
                                                = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                        }
                                    }
                                    MN1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddAncestorBaseTPCalcs(List<Calculator1> calcs)
        {
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                //the change analyzers also use a base comparator
                BudgetInvestment ancestorBaseBI = GetAntecedentBaseBI();
                if (ancestorBaseBI != null)
                {
                    if (ancestorBaseBI.TimePeriods != null)
                    {
                        foreach (var tp in ancestorBaseBI.TimePeriods)
                        {
                            if (tp.Calculators != null)
                            {
                                //must be able to distinguish ancestor from current
                                //not necessary to set existing TargetTypes (they are ignored anyway)
                                foreach (var calc in tp.Calculators)
                                {
                                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                    {
                                        //must be set correctly in base elements
                                        ////ocs compare planned vs actual for each member
                                        //calc.TargetType
                                        //    = Calculator1.TARGET_TYPES.benchmark.ToString();
                                    }
                                    else
                                    {
                                        //set baseline comparators
                                        calc.ChangeType
                                            = Calculator1.CHANGE_TYPES.baseline.ToString();
                                    }
                                }
                                MN1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
                            }
                        }
                    }
                }
                else
                {
                    int iBIs = GetBIsCount();
                    if (iBIs > 1)
                    {
                        //first element in bis won't find ancestor but don't want the tps compared
                        //so set them to baseline for comparison to remaining bis
                        foreach (var calc in calcs)
                        {
                            //use SetChanges not SetBudgetChanges
                            calc.ChangeType = Calculator1.CHANGE_TYPES.other.ToString();
                            ////set current comparators (next bi will be changed to baseline, but Change1 checks for current to run totals)
                            //calc.ChangeType
                            //    = Calculator1.CHANGE_TYPES.current.ToString();
                        }
                    }
                    else
                    {
                        foreach (var currentcalc in calcs)
                        {
                            //use SetChanges not SetBudgetChanges
                            currentcalc.ChangeType = Calculator1.CHANGE_TYPES.other.ToString();
                        }
                        //AddTPComparators(calcs);
                    }
                }
            }
        }
        private void AddTPComparators(List<Calculator1> calcs)
        {
            //need to try to compare tps only
            //get added to calcs
            List<Calculator1> compCalcs2 = new List<Calculator1>();
            int i = 1;
            //single budget.tp analysis must have unqiue tp labels, or base and xminus1 get mixed up
            foreach (var currentcalc in calcs)
            {
                currentcalc.Label = string.Concat(currentcalc.Label, i.ToString());
                i++;
            }
            i = 1;
            foreach (var currentcalc in calcs)
            {
                if (i != 1)
                {
                    //copy the base and x-1 comparators
                    List<Calculator1> compCalcs = new List<Calculator1>();
                    MN1AnalyzerHelper.AddCalculators(calcs, compCalcs);
                    if (compCalcs.Count > 1)
                    {
                        int j = 1;
                        foreach (var calc in compCalcs)
                        {
                            if (j == 1)
                            {
                                //baseline
                                calc.ChangeType
                                    = Calculator1.CHANGE_TYPES.baseline.ToString();
                                calc.Label
                                    = currentcalc.Label;
                                //don't aggregate the base with the other calcs in alt2
                                calc.Alternative2 = CalculatorHelpers.GetRandomInteger(this.GCCalculatorParams);
                                compCalcs2.Add(calc);
                            }
                            else if (j == (i - 1))
                            {
                                if (i != 2)
                                {
                                    //x-1
                                    calc.ChangeType
                                        = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                    calc.Label
                                        = currentcalc.Label;
                                    //don't aggregate the base with the other calcs in alt2
                                    calc.Alternative2 = CalculatorHelpers.GetRandomInteger(this.GCCalculatorParams);
                                    compCalcs2.Add(calc);
                                }
                            }
                            j++;
                        }
                    }
                }
                i++;
            }
            if (compCalcs2.Count > 0)
            {
                foreach (var calc2 in compCalcs2)
                {
                    calcs.Add(calc2);
                }
            }
        }
        private BudgetInvestment GetAntecedentXMinus1BI()
        {
            BudgetInvestment ancestorBI = null;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                != null)
            {
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                    != null)
                {
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                    {
                        //don't overwrite base changes
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                            .BudgetInvestments.Count > 1)
                        {
                            //else no ChangeByYear at BI level, and ChangeById is already ordered
                            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments
                                .LastOrDefault() != null)
                            {
                                ancestorBI = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                        .BudgetInvestments
                                        .LastOrDefault();
                            }
                        }
                    }
                    else
                    {
                        //don't change benchmark, but add it's tps to remaining budgets
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                            .BudgetInvestments.Count > 0)
                        {
                            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments
                                .LastOrDefault() != null)
                            {
                                //ocs are compared to benchmark budget; so TPs must do same
                                //each budget is compared to benchmark, not to each other
                                ancestorBI = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                        .BudgetInvestments
                                        .LastOrDefault(tp => tp.TargetType == Calculator1.TARGET_TYPES.benchmark.ToString());
                                ////last budget holds cum progress 
                                //ancestorBI = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                //        .BudgetInvestments
                                //        .LastOrDefault();
                            }
                        }
                    }
                }
            }
            return ancestorBI;
        }
        private BudgetInvestment GetAntecedentBaseBI()
        {
            BudgetInvestment ancestorBI = null;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                != null)
            {
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                    != null)
                {
                    //else no ChangeByYear at BI level, and ChangeById is already ordered
                    if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                        .BudgetInvestments
                        .FirstOrDefault() != null)
                    {
                        ancestorBI = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments
                                .FirstOrDefault();
                    }
                }
            }
            return ancestorBI;
        }
        private void AddAncestorOCCalcs(List<Calculator1> calcs)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets
                    || this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    //never want sibling comparisons in bis
                    foreach (var calc2 in calcs)
                    {
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                        {
                            //must be set in base elements
                            ////ocs compare planned vs actual for each member
                            //calc2.TargetType
                            //    = Calculator1.TARGET_TYPES.actual.ToString();
                        }
                        else
                        {
                            //ocs compare current vs comparators
                            calc2.ChangeType
                                = Calculator1.CHANGE_TYPES.current.ToString();
                        }
                    }
                    //add the base comparator
                    AddAncestorBaseOCCalcs(calcs);
                    //this correctly skips the first tp because it's not added yet
                    //and has no ancestor to run comparisons
                    //always comparing the last ancestor ocss to current ocs
                    TimePeriod ancestorTP = GetAntecedentXMinus1TP();
                    if (ancestorTP != null)
                    {
                        if (ancestorTP.OperationComponents != null)
                        {
                            foreach (var oc in ancestorTP.OperationComponents)
                            {
                                if (oc.Calculators != null)
                                {
                                    //must be able to distinguish ancestor from current
                                    //not necessary to set existing TargetTypes (they are ignored anyway)
                                    foreach (var calc in oc.Calculators)
                                    {
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            //must be set in base elements
                                            //calc.TargetType
                                            //    = Calculator1.TARGET_TYPES.benchmark.ToString();
                                        }
                                        else
                                        {
                                            calc.ChangeType
                                                = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                        }
                                    }
                                    MN1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void AddAncestorBaseOCCalcs(List<Calculator1> calcs)
        {
            //progress1 uses xminus1
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                //the change analyzers also use a base comparator
                TimePeriod ancestorBaseTP = GetAntecedentBaseTP();
                if (ancestorBaseTP != null)
                {
                    if (ancestorBaseTP.OperationComponents != null)
                    {
                        foreach (var oc in ancestorBaseTP.OperationComponents)
                        {
                            if (oc.Calculators != null)
                            {
                                //must be able to distinguish ancestor from current
                                //not necessary to set existing TargetTypes (they are ignored anyway)
                                foreach (var calc in oc.Calculators)
                                {
                                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                    {
                                        //must be set in base elements
                                        //calc.TargetType
                                        //    = Calculator1.TARGET_TYPES.benchmark.ToString();
                                    }
                                    else
                                    {
                                        calc.ChangeType
                                            = Calculator1.CHANGE_TYPES.baseline.ToString();
                                    }
                                }
                                MN1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                            }
                        }
                    }
                }
            }
        }
        private TimePeriod GetAntecedentXMinus1TP()
        {
            bool bHasLastBITP = false;
            TimePeriod ancestorTP = null;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                != null)
            {
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                    != null)
                {
                    //else no ChangeByYear at BI level, and ChangeById is already ordered
                    if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                        .BudgetInvestments
                        .LastOrDefault() != null)
                    {
                        //don't overwrite base changes
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                        {
                            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments.Count > 1)
                            {
                                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments
                                .LastOrDefault()
                                .TimePeriods != null)
                                {
                                    //find the corresponding tp in previous bi
                                    //this.GCCalculatorParams.AnalyzerParms.AggregatingId was set to currenttp.Label
                                    ancestorTP = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                        .BudgetInvestments
                                        .LastOrDefault()
                                        .TimePeriods
                                        .LastOrDefault(tp => tp.Label == this.GCCalculatorParams.AnalyzerParms.AggregatingId);
                                    if (ancestorTP != null)
                                    {
                                        bHasLastBITP = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments
                                .LastOrDefault()
                                .TimePeriods != null)
                            {
                                //find the corresponding tp in previous bi
                                //this.GCCalculatorParams.AnalyzerParms.AggregatingId was set to currenttp.Label
                                ancestorTP = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                    .BudgetInvestments
                                    .LastOrDefault()
                                    .TimePeriods
                                    .LastOrDefault(tp => tp.Label == this.GCCalculatorParams.AnalyzerParms.AggregatingId
                                    && tp.TargetType == Calculator1.TARGET_TYPES.benchmark.ToString());
                                if (ancestorTP != null)
                                {
                                    bHasLastBITP = true;
                                }
                            }
                            if (ancestorTP == null)
                            {
                                //actual has cumulative totals
                                ancestorTP = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                    .BudgetInvestments
                                    .FirstOrDefault()
                                    .TimePeriods
                                    .LastOrDefault(tp => tp.Label == this.GCCalculatorParams.AnalyzerParms.AggregatingId
                                        && tp.TargetType == Calculator1.TARGET_TYPES.benchmark.ToString());
                                if (ancestorTP != null)
                                {
                                    bHasLastBITP = true;
                                }
                            }
                        }
                    }
                }
            }
            if (!bHasLastBITP)
            {
                //the change analyzers use the ocs in the last tp as the comparator
                if (this.GCCalculatorParams.ParentBudgetInvestment != null)
                {
                    if (this.GCCalculatorParams
                        .ParentBudgetInvestment
                        .TimePeriods != null)
                    {
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                        {
                            //if only one tp then only base changes allowed
                            if (this.GCCalculatorParams
                            .ParentBudgetInvestment
                            .TimePeriods.Count > 1)
                            {
                                //find the corresponding tp in previous tp (sequential order is ok)
                                ancestorTP = this.GCCalculatorParams
                                    .ParentBudgetInvestment
                                    .TimePeriods
                                    .LastOrDefault();
                            }
                        }
                        else
                        {
                            //find the corresponding tp in previous tp (sequential order is ok)
                            ancestorTP = this.GCCalculatorParams
                                .ParentBudgetInvestment
                                .TimePeriods
                                .LastOrDefault(t => t.TargetType == Calculator1.TARGET_TYPES.benchmark.ToString());
                        }
                    }
                }
            }
            return ancestorTP;
        }
        private TimePeriod GetAntecedentBaseTP()
        {
            bool bHasLastBITP = false;
            TimePeriod ancestorTP = null;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                != null)
            {
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                    != null)
                {
                    if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                        .BudgetInvestments
                        .FirstOrDefault() != null)
                    {
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                        .BudgetInvestments
                        .FirstOrDefault()
                        .TimePeriods != null)
                        {
                            //find the corresponding tp in first bi
                            //this.GCCalculatorParams.AnalyzerParms.AggregatingId was set to currenttp.Label
                            ancestorTP = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments
                                .FirstOrDefault()
                                .TimePeriods
                                .FirstOrDefault(tp => tp.Label == this.GCCalculatorParams.AnalyzerParms.AggregatingId);
                            if (ancestorTP != null)
                            {
                                bHasLastBITP = true;
                            }
                        }
                    }
                }
            }
            if (!bHasLastBITP)
            {
                //the change analyzers use the ocs in the last tp as the comparator
                if (this.GCCalculatorParams.ParentBudgetInvestment != null)
                {
                    if (this.GCCalculatorParams
                        .ParentBudgetInvestment
                        .TimePeriods != null)
                    {
                        //find the corresponding tp in previous tp (sequential order is ok)
                        ancestorTP = this.GCCalculatorParams
                            .ParentBudgetInvestment
                            .TimePeriods
                            .FirstOrDefault();
                    }
                }
            }
            return ancestorTP;
        }
        private void AddAncestorOutcomeCalcs(List<Calculator1> calcs)
        {
            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets
                    || this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    //ok to change in bis -never need sibling comparisons
                    foreach (var calc2 in calcs)
                    {
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                        {
                            //must be set correctly in base elements
                            ////ocs compare planned vs actual for each member
                            //calc2.TargetType
                            //    = Calculator1.TARGET_TYPES.actual.ToString();
                        }
                        else
                        {
                            //ocs compare current vs comparators
                            calc2.ChangeType
                                = Calculator1.CHANGE_TYPES.current.ToString();
                        }
                    }
                    //add the base comparator
                    AddAncestorBaseOutcomeCalcs(calcs);
                    //this correctly skips the first tp because it's not added yet
                    //and has no ancestor to run comparisons
                    //always comparing the last ancestor ocss to current ocs
                    TimePeriod ancestorTP = GetAntecedentXMinus1TP();
                    if (ancestorTP != null)
                    {
                        if (ancestorTP.Outcomes != null)
                        {
                            foreach (var oc in ancestorTP.Outcomes)
                            {
                                if (oc.Calculators != null)
                                {
                                    //must be able to distinguish ancestor from current
                                    //not necessary to set existing TargetTypes (they are ignored anyway)
                                    foreach (var calc in oc.Calculators)
                                    {
                                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                            == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                        {
                                            //must be set correctly in base elements
                                            //calc.TargetType
                                            //    = Calculator1.TARGET_TYPES.benchmark.ToString();
                                        }
                                        else
                                        {
                                            calc.ChangeType
                                                = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                        }
                                    }
                                    MN1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddAncestorBaseOutcomeCalcs(List<Calculator1> calcs)
        {
            //progress1 uses minus1
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                != MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                //the change analyzers also use a base comparator
                TimePeriod ancestorBaseTP = GetAntecedentBaseTP();
                if (ancestorBaseTP != null)
                {
                    if (ancestorBaseTP.Outcomes != null)
                    {
                        foreach (var oc in ancestorBaseTP.Outcomes)
                        {
                            if (oc.Calculators != null)
                            {
                                //must be able to distinguish ancestor from current
                                //not necessary to set existing TargetTypes (they are ignored anyway)
                                foreach (var calc in oc.Calculators)
                                {
                                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                                    {
                                        //must be set correctly in base elements
                                        //calc.TargetType
                                        //    = Calculator1.TARGET_TYPES.benchmark.ToString();
                                    }
                                    else
                                    {
                                        calc.ChangeType
                                            = Calculator1.CHANGE_TYPES.baseline.ToString();
                                    }
                                }
                                MN1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                            }
                        }
                    }
                }
            }
        }

        private int GetBIsCount()
        {
            int iBIs = 1;
            if (this.BudgetGroup != null)
            {
                //if multiple budgets then need cumulative tp totals
                //i.e. planned = bi1.tp1  + bi1.tp2 ; actual = bi2.tp1 + bi2.tp2
                if (this.BudgetGroup.BudgetInvestments != null)
                {
                    iBIs = this.BudgetGroup.BudgetInvestments.Count;
                    if (iBIs == 0)
                        iBIs = 1;
                }
            }
            return iBIs;
        }
        public bool SaveBIMN1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.BudgetGroup != null)
            {
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString();
                }
                else
                {
                    this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString();
                }
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.BudgetGroup.BudgetInvestments.Count;
                    SetComparativeBaseAttributes(this.BudgetGroup, this.BudgetGroup.BudgetInvestments.Count, ref writer);
                }
                //groups have no extension (one totals only)
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.MN1DescendentStock.Option1);
                this.BudgetGroup.SetNewBIGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //should this be an If (IsSelfOrDescendent) ?
                AddMN1StockTotals(sAttNameExt, this.BudgetGroup.Calculators, ref writer);
                if (this.BudgetGroup.BudgetInvestments != null)
                {
                    bHasCalculations = SetBIStockTotals(writer, this.BudgetGroup.BudgetInvestments);
                }
                writer.WriteEndElement();
            }
            return bHasCalculations;
        }
        private bool SetBIStockTotals(XmlWriter writer, List<BudgetInvestment> bis)
        {
            bool bHasTotals = false;
            if (bis != null)
            {
                //comps go horizontal
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //write each of the obs totals to the element (no linkedviews used)
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budget.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investment.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var bi in bis)
                {
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //write each of the obs totals to the element (no linkedviews used)
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budget.ToString();
                        }
                        else 
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investment.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(bi, bis.Count, ref writer);
                        }
                    } 
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    bi.SetNewBIAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddMN1StockTotals(sAttNameExt, bi.Calculators, ref writer);
                        if (bi.TimePeriods != null)
                        {
                            bHasTotals = SetTimePeriodStockTotals(writer, bi.TimePeriods);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //bi
                        writer.WriteEndElement();
                    }
                }
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    writer.WriteStartElement(Constants.ROOT_PATH);
                    writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                    i = 0;
                    foreach (var bi in bis)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(bi, bis.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                        AddMN1StockCompTotals(sAttNameExt, bi.Calculators, ref writer);
                        i++;
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    if (bis.Count == 1)
                    {
                        foreach (var bi in bis)
                        {
                            //tps set this.ColIndex
                            if (bi.TimePeriods != null)
                            {
                                bHasTotals = SetTimePeriodStockTotals(writer, bi.TimePeriods);
                            }
                            i++;
                        }
                        //bi
                        writer.WriteEndElement();
                    }
                    else
                    {
                        this.ColumnIndex = 0;
                        //bis set this.ColIndex
                        foreach (var bi in bis)
                        {
                            if (bi.TimePeriods != null)
                            {
                                //descendents
                                bHasTotals = SetTPComparisons(writer, bi.TimePeriods);
                            }
                            //comparative analysis line up using either budgetindex (if this.ColIndex >1) or tpindex
                            this.ColumnIndex++;
                        }
                        //bi
                        writer.WriteEndElement();
                    }
                }
            }
            return bHasTotals;
        }
        private bool SetTimePeriodStockTotals(XmlWriter writer, List<TimePeriod> tps)
        {
            bool bHasTotals = false;
            if (tps != null)
            {
                //total number of columns needed in descendents
                this.ColumnCount = tps.Count;
                //comps go horizontal
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //write each of the obs totals to the element (no linkedviews used)
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString();
                    }
                    else 
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var tp in tps)
                {
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //write each of the obs totals to the element (no linkedviews used)
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString();
                        }
                        else
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(tp, tps.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        tp.SetNewTPBudgetAttributes(sAttNameExt, ref writer);
                    }
                    else
                    {
                        tp.SetNewTPInvestmentAttributes(sAttNameExt, ref writer);
                    }
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddMN1StockTotals(sAttNameExt, tp.Calculators, ref writer);
                        SetTPDescendentTotals(writer, tp);
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //tp
                        writer.WriteEndElement();
                    }
                }
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    writer.WriteStartElement(Constants.ROOT_PATH);
                    writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                    i = 0;
                    foreach (var tp in tps)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(tp, tps.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                        AddMN1StockCompTotals(sAttNameExt, tp.Calculators, ref writer);
                        i++;
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    //descendents
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                    this.ColumnIndex = 0;
                    foreach (var tp in tps)
                    {
                        bHasTotals = SetTPOutcomeComparisons(writer, tp.Outcomes);
                        //comparative analysis line up using tpindex (budget 3 might start with a tpcol = 6)
                        this.ColumnIndex++;
                    }
                    //outcomes
                    writer.WriteEndElement();
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                    this.ColumnIndex = 0;
                    foreach (var tp in tps)
                    {
                        bHasTotals = SetTPOCComparisons(writer, tp.OperationComponents);
                        //comparative analysis line up using tpindex (budget 3 might start with a tpcol = 6)
                        this.ColumnIndex++;
                    }
                    //opcomps
                    writer.WriteEndElement();
                    //tp
                    writer.WriteEndElement();
                }
            }
            return bHasTotals;
        }
        private bool SetTPComparisons(XmlWriter writer, List<TimePeriod> tps)
        {
            bool bHasTotals = false;
            if (tps != null)
            {
                //comps go horizontal and line up with bi.colindex
                string sAttNameExt = string.Empty;
                foreach (var tp in tps)
                {
                    //can't set id=0 it's needed to verity descendants
                    if (tp.Label2 != "00")
                    {
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(tp, this.ColumnCount, ref writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.MN1DescendentStock.Option1);
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            tp.SetNewTPBudgetAttributes(sAttNameExt, ref writer);
                        }
                        else
                        {
                            tp.SetNewTPInvestmentAttributes(sAttNameExt, ref writer);
                        }
                        Dictionary<int, int> biTPs = new Dictionary<int, int>();
                        bool bIsCalc = false;
                        //these parenttps are not used
                        biTPs = SetSiblingTPCalculator(writer, tp, bIsCalc);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(tp, this.ColumnCount, ref writer);
                        AddMN1StockCompTotals(sAttNameExt, tp.Calculators, ref writer);
                        //signal not to use again
                        tp.Label2 = "00";
                        //tp.Id = 0;
                        bIsCalc = true;
                        biTPs = new Dictionary<int, int>();
                        //recurse to sibling time periods and add same tp if found with bi colindex
                        biTPs = SetSiblingTPCalculator(writer, tp, bIsCalc);
                        //lv
                        writer.WriteEndElement();
                        //root
                        writer.WriteEndElement();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        //tp
                        //writer.WriteEndElement();

                        //descendents
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString();
                        }
                        else
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                        bHasTotals = SetBITPOutcomeComparisons(writer, tp.Outcomes, biTPs);
                        //outcomes
                        writer.WriteEndElement();
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString();
                        }
                        else
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                        bHasTotals = SetBITPOCComparisons(writer, tp.OperationComponents, biTPs);
                        //opcomps
                        writer.WriteEndElement();
                        //tp
                        writer.WriteEndElement();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private Dictionary<int, int> SetSiblingTPCalculator(XmlWriter writer,
            TimePeriod tp, bool isCalculator)
        {
            Dictionary<int, int> biTPs = new Dictionary<int, int>();
            string sAttNameExt = string.Empty;
            int iColIndex = 0;
            foreach (var bi in this.BudgetGroup.BudgetInvestments)
            {
                //this.ColIndex is already written
                if (iColIndex != this.ColumnIndex)
                {
                    if (bi.TimePeriods != null)
                    {
                        List<string> SibLabels = new List<string>();
                        foreach (var sibTP in bi.TimePeriods)
                        {
                            if (sibTP.Label2 != "00")
                            {
                                bool bNeedsCalc = false;
                                switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
                                {
                                    case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                                        if (sibTP.TypeId == tp.TypeId)
                                        {
                                            bNeedsCalc = true;
                                        }
                                        break;
                                    case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                                        if (sibTP.GroupId == tp.GroupId)
                                        {
                                            bNeedsCalc = true;
                                        }
                                        break;
                                    case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                                        if (sibTP.Label == tp.Label)
                                        {
                                            bNeedsCalc = true;
                                        }
                                        break;
                                    case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                                        //change and progress analysis use labels to make comparisons
                                        if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
                                            && (!SibLabels.Contains(sibTP.Label)))
                                        {
                                            if (sibTP.Label == tp.Label)
                                            {
                                                SibLabels.Add(sibTP.Label);
                                                bNeedsCalc = true;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                if (bNeedsCalc)
                                {
                                    sAttNameExt
                                        = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.MN1DescendentStock.Option1);
                                    if (isCalculator)
                                    {
                                        AddMN1StockCompTotals(sAttNameExt, sibTP.Calculators, ref writer);
                                        KeyValuePair<int, int> bitp = new KeyValuePair<int, int>(iColIndex, sibTP.Id);
                                        if (!biTPs.Contains(bitp))
                                        {
                                            biTPs.Add(iColIndex, sibTP.Id);
                                        }
                                        //signal not to use again
                                        //sibTP.Id = 0;
                                        sibTP.Label2 = "00";
                                    }
                                    else
                                    {
                                        SetSiblingComparativeBaseProperties(sibTP, sAttNameExt, ref writer);
                                    }
                                }
                            }
                        }
                    }
                }
                iColIndex++;
            }
            return biTPs;
        }
        private bool SetTPDescendentTotals(XmlWriter writer, TimePeriod tp)
        {
            bool bHasTotals = false;
            if (tp.Outcomes != null)
            {
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString();
                }
                else
                {
                    this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString();
                }
                writer.WriteStartElement(this.CurrentNodeName);
                bHasTotals = SetOutcomeStockTotals(writer, tp.Outcomes);
                writer.WriteEndElement();
            }
            if (tp.OperationComponents != null)
            {
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString();
                }
                else
                {
                    this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString();
                }
                writer.WriteStartElement(this.CurrentNodeName);
                bHasTotals = SetOCStockTotals(writer, tp.OperationComponents);
                writer.WriteEndElement();
            }
            return bHasTotals;
        }
        public bool SaveOCMN1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.OCGroup != null)
            {
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    this.CurrentNodeName = OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString();
                }
                else
                {
                    this.CurrentNodeName = OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString();
                }
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OCGroup.OperationComponents.Count;
                    SetComparativeBaseAttributes(this.OCGroup, this.OCGroup.OperationComponents.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.MN1DescendentStock.Option1);
                this.OCGroup.SetNewOCGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //should this be an If (IsSelfOrDescendent) ?
                AddMN1StockTotals(sAttNameExt, this.OCGroup.Calculators, ref writer);
                if (this.OCGroup.OperationComponents != null)
                {
                    bHasCalculations = SetOCStockTotals(writer, this.OCGroup.OperationComponents);
                }
                writer.WriteEndElement();
            }
            return bHasCalculations;
        }
        private bool SetOCStockTotals(XmlWriter writer, List<OperationComponent> ocs)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //write each of the obs totals to the element (no linkedviews used)
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.operationprices)
                    {
                        this.CurrentNodeName = OperationComponent.OPERATION_PRICE_TYPES.operation.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.componentprices)
                    {
                        this.CurrentNodeName = OperationComponent.COMPONENT_PRICE_TYPES.component.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //write each of the obs totals to the element (no linkedviews used)
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.operationprices)
                        {
                            this.CurrentNodeName = OperationComponent.OPERATION_PRICE_TYPES.operation.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.componentprices)
                        {
                            this.CurrentNodeName = OperationComponent.COMPONENT_PRICE_TYPES.component.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(oc, ocs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddMN1StockTotals(sAttNameExt, oc.Calculators, ref writer);
                        if (oc.Inputs != null)
                        {
                            bHasTotals = SetInputTechStockTotals(writer, oc.Inputs);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    writer.WriteStartElement(Constants.ROOT_PATH);
                    writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                    i = 0;
                    foreach (var oc in ocs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(oc, ocs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                        AddMN1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
                        i++;
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    //don't include desc inputs in comp analysis
                    //op, comp, budop, investcomp
                    writer.WriteEndElement();
                }
            }
            return bHasTotals;
        }
       
        private bool SetBITPOCComparisons(XmlWriter writer, List<OperationComponent> ocs, 
            Dictionary<int, int> biTps)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal and line up with tp.colindex
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (oc.Id != 0)
                    {
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);

                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.MN1DescendentStock.Option1);
                        oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingTPOCCalculator(writer, oc, bIsCalc, biTps);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);
                        AddMN1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        SetSiblingTPOCCalculator(writer, oc, bIsCalc, biTps);
                        //lv
                        writer.WriteEndElement();
                        //root
                        writer.WriteEndElement();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private bool SetBITPOutcomeComparisons(XmlWriter writer, List<Outcome> ocs,
            Dictionary<int, int> biTps)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal and line up with tp.colindex
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (oc.Id != 0)
                    {
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcome.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.MN1DescendentStock.Option1);
                        oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingTPOutcomeCalculator(writer, oc, bIsCalc, biTps);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);
                        AddMN1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        SetSiblingTPOutcomeCalculator(writer, oc, bIsCalc, biTps);
                        //lv
                        writer.WriteEndElement();
                        //root
                        writer.WriteEndElement();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private void SetSiblingTPOCCalculator(XmlWriter writer,
            OperationComponent oc, bool isCalculator, Dictionary<int, int> biTPs)
        {
            string sAttNameExt = string.Empty;
            int iColIndex = 0;
            foreach (var bi in this.BudgetGroup.BudgetInvestments)
            {
                //multiple bis set this.colindex
                if (iColIndex != this.ColumnIndex)
                {
                    foreach (var sibTp in bi.TimePeriods)
                    {
                        KeyValuePair<int, int> biTP = new KeyValuePair<int, int>(iColIndex, sibTp.Id);
                        if (biTPs.Contains(biTP))
                        {
                            if (sibTp.OperationComponents != null)
                            {
                                List<string> SibLabels = new List<string>();
                                foreach (var sibOC in sibTp.OperationComponents)
                                {
                                    if (sibOC.Id != 0)
                                    {
                                        bool bNeedsCalc = false;
                                        switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
                                        {
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                                                if (sibOC.TypeId == oc.TypeId)
                                                {
                                                    bNeedsCalc = true;
                                                }
                                                break;
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                                                if (sibOC.GroupId == oc.GroupId)
                                                {
                                                    bNeedsCalc = true;
                                                }
                                                break;
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                                                if (sibOC.Label == oc.Label)
                                                {
                                                    bNeedsCalc = true;
                                                }
                                                break;
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                                                //change and progress analysis use labels to make comparisons
                                                if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
                                                    && (!SibLabels.Contains(sibOC.Label)))
                                                {
                                                    if (sibOC.Label == oc.Label)
                                                    {
                                                        SibLabels.Add(sibOC.Label);
                                                        bNeedsCalc = true;
                                                    }
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                        if (bNeedsCalc)
                                        {
                                            sAttNameExt
                                                = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.MN1DescendentStock.Option1);
                                            if (isCalculator)
                                            {
                                                AddMN1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
                                                //signal not to use again
                                                sibOC.Id = 0;
                                            }
                                            else
                                            {
                                                SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, ref writer);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                iColIndex++;
            }
        }
        private void SetSiblingTPOutcomeCalculator(XmlWriter writer,
            Outcome oc, bool isCalculator, Dictionary<int, int> biTPs)
        {
            string sAttNameExt = string.Empty;
            int iColIndex = 0;
            foreach (var bi in this.BudgetGroup.BudgetInvestments)
            {
                //multiple bis set this.colindex
                if (iColIndex != this.ColumnIndex)
                {
                    foreach (var sibTp in bi.TimePeriods)
                    {
                        KeyValuePair<int, int> biTP = new KeyValuePair<int, int>(iColIndex, sibTp.Id);
                        if (biTPs.Contains(biTP))
                        {
                            if (sibTp.Outcomes != null)
                            {
                                List<string> SibLabels = new List<string>();
                                foreach (var sibOC in sibTp.Outcomes)
                                {
                                    if (sibOC.Id != 0)
                                    {
                                        bool bNeedsCalc = false;
                                        switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
                                        {
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                                                if (sibOC.TypeId == oc.TypeId)
                                                {
                                                    bNeedsCalc = true;
                                                }
                                                break;
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                                                if (sibOC.GroupId == oc.GroupId)
                                                {
                                                    bNeedsCalc = true;
                                                }
                                                break;
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                                                if (sibOC.Label == oc.Label)
                                                {
                                                    bNeedsCalc = true;
                                                }
                                                break;
                                            case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                                                //change and progress analysis use labels to make comparisons
                                                if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
                                                    && (!SibLabels.Contains(sibOC.Label)))
                                                {
                                                    if (sibOC.Label == oc.Label)
                                                    {
                                                        SibLabels.Add(sibOC.Label);
                                                        bNeedsCalc = true;
                                                    }
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                        if (bNeedsCalc)
                                        {
                                            sAttNameExt
                                                = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.MN1DescendentStock.Option1);
                                            if (isCalculator)
                                            {
                                                AddMN1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
                                                //signal not to use again
                                                sibOC.Id = 0;
                                            }
                                            else
                                            {
                                                SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, ref writer);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                iColIndex++;
            }
        }
        private bool SetTPOCComparisons(XmlWriter writer, List<OperationComponent> ocs)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal and line up with tp.colindex
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (oc.Id != 0)
                    {
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);

                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.MN1DescendentStock.Option1);
                        oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingOCCalculator(writer, oc, bIsCalc);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);
                        AddMN1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        SetSiblingOCCalculator(writer, oc, bIsCalc);
                        //lv
                        writer.WriteEndElement();
                        //root
                        writer.WriteEndElement();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private bool SetTPOutcomeComparisons(XmlWriter writer, List<Outcome> ocs)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal and line up with tp.colindex
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (oc.Id != 0)
                    {
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcome.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.MN1DescendentStock.Option1);
                        oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingOutcomeCalculator(writer, oc, bIsCalc);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, ref writer);
                        AddMN1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        SetSiblingOutcomeCalculator(writer, oc, bIsCalc);
                        //lv
                        writer.WriteEndElement();
                        //root
                        writer.WriteEndElement();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private void SetSiblingOCCalculator(XmlWriter writer,
            OperationComponent oc, bool isCalculator)
        {
            string sAttNameExt = string.Empty;
            int iColIndex = 0;
            foreach (var bi in this.BudgetGroup.BudgetInvestments)
            {
                foreach (var sibTp in bi.TimePeriods)
                {
                    //this.ColIndex is already written
                    if (iColIndex != this.ColumnIndex)
                    {
                        if (sibTp.OperationComponents != null)
                        {
                            List<string> SibLabels = new List<string>();
                            foreach (var sibOC in sibTp.OperationComponents)
                            {
                                if (sibOC.Id != 0)
                                {
                                    bool bNeedsCalc = false;
                                    switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
                                    {
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                                            if (sibOC.TypeId == oc.TypeId)
                                            {
                                                bNeedsCalc = true;
                                            }
                                            break;
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                                            if (sibOC.GroupId == oc.GroupId)
                                            {
                                                bNeedsCalc = true;
                                            }
                                            break;
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                                            if (sibOC.Label == oc.Label)
                                            {
                                                bNeedsCalc = true;
                                            }
                                            break;
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                                            //change and progress analysis use labels to make comparisons
                                            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
                                                && (!SibLabels.Contains(sibOC.Label)))
                                            {
                                                if (sibOC.Label == oc.Label)
                                                {
                                                    SibLabels.Add(sibOC.Label);
                                                    bNeedsCalc = true;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (bNeedsCalc)
                                    {
                                        sAttNameExt
                                            = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.MN1DescendentStock.Option1);
                                        if (isCalculator)
                                        {
                                            AddMN1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
                                            //signal not to use again
                                            sibOC.Id = 0;
                                        }
                                        else
                                        {
                                            SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, ref writer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    iColIndex++;
                }
            }
        }
        private void SetSiblingOutcomeCalculator(XmlWriter writer,
            Outcome oc, bool isCalculator)
        {
            string sAttNameExt = string.Empty;
            int iColIndex = 0;
            foreach (var bi in this.BudgetGroup.BudgetInvestments)
            {
                foreach (var sibTp in bi.TimePeriods)
                {
                    //this.ColIndex is already written
                    if (iColIndex != this.ColumnIndex)
                    {
                        if (sibTp.Outcomes != null)
                        {
                            List<string> SibLabels = new List<string>();
                            foreach (var sibOC in sibTp.Outcomes)
                            {
                                if (sibOC.Id != 0)
                                {
                                    bool bNeedsCalc = false;
                                    switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
                                    {
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                                            if (sibOC.TypeId == oc.TypeId)
                                            {
                                                bNeedsCalc = true;
                                            }
                                            break;
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                                            if (sibOC.GroupId == oc.GroupId)
                                            {
                                                bNeedsCalc = true;
                                            }
                                            break;
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                                            if (sibOC.Label == oc.Label)
                                            {
                                                bNeedsCalc = true;
                                            }
                                            break;
                                        case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                                            //change and progress analysis use labels to make comparisons
                                            if (MN1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
                                                && (!SibLabels.Contains(sibOC.Label)))
                                            {
                                                if (sibOC.Label == oc.Label)
                                                {
                                                    SibLabels.Add(sibOC.Label);
                                                    bNeedsCalc = true;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (bNeedsCalc)
                                    {
                                        sAttNameExt
                                            = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.MN1DescendentStock.Option1);
                                        if (isCalculator)
                                        {
                                            AddMN1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
                                            //signal not to use again
                                            sibOC.Id = 0;
                                        }
                                        else
                                        {
                                            SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, ref writer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    iColIndex++;
                }
            }
        }
        private bool SetInputTechStockTotals(XmlWriter writer, List<Input> inputs)
        {
            bool bHasTotals = false;
            //inputs are not shown in tech comparisons
            if (inputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var input in inputs)
                {
                    //tech inputs get individual els
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.operationprices)
                    {
                        this.CurrentNodeName = OperationComponent.OPERATION_PRICE_TYPES.operationinput.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.componentprices)
                    {
                        this.CurrentNodeName = OperationComponent.COMPONENT_PRICE_TYPES.componentinput.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetinput.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentinput.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    input.SetNewInputAttributes(sAttNameExt, ref writer);
                    if (bNeedsBaseCalcs)
                    {
                        AddMN1InputStockTotals(sAttNameExt, input.Calculators, ref writer);
                    }
                    else
                    {
                        AddMN1StockTotals(sAttNameExt, input.Calculators, ref writer);
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    //input
                    writer.WriteEndElement();
                }
            }
            return bHasTotals;
        }

        public bool SaveOutcomeMN1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.OutcomeGroup != null)
            {
                this.CurrentNodeName = Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString();
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OutcomeGroup.Outcomes.Count;
                    SetComparativeBaseAttributes(this.OutcomeGroup, this.OutcomeGroup.Outcomes.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.MN1DescendentStock.Option1);
                this.OutcomeGroup.SetNewOutcomeGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                AddMN1StockTotals(sAttNameExt, this.OutcomeGroup.Calculators, ref writer);
                if (this.OutcomeGroup.Outcomes != null)
                {
                    bHasCalculations = SetOutcomeStockTotals(writer, this.OutcomeGroup.Outcomes);
                }
                writer.WriteEndElement();
            }
            return bHasCalculations;
        }
        private bool SetOutcomeStockTotals(XmlWriter writer, List<Outcome> ocs)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //write each of the obs totals to the element (no linkedviews used)
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcome.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = Outcome.OUTCOME_PRICE_TYPES.outcome.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //write each of the obs totals to the element (no linkedviews used)
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString();
                        }
                        else if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutcome.ToString();
                        }
                        else
                        {
                            this.CurrentNodeName = Outcome.OUTCOME_PRICE_TYPES.outcome.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(oc, ocs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddMN1StockTotals(sAttNameExt, oc.Calculators, ref writer);
                        if (oc.Outputs != null)
                        {
                            bHasTotals = SetOutputTechStockTotals(writer, oc.Outputs);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    writer.WriteStartElement(Constants.ROOT_PATH);
                    writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                    i = 0;
                    foreach (var oc in ocs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(oc, ocs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets
                            || this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            //lines up tps side by side
                            sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.MN1DescendentStock.Option1);
                        }
                        AddMN1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
                        i++;
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    //op, comp, budop, investcomp
                    writer.WriteEndElement();
                }
            }
            return bHasTotals;
        }
        private bool SetOutputTechStockTotals(XmlWriter writer, List<Output> outputs)
        {
            bool bHasTotals = false;
            //inputs are not shown in tech comparisons
            if (outputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var output in outputs)
                {
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString();
                    }
                    else if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentoutput.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = Outcome.OUTCOME_PRICE_TYPES.outcomeoutput.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    output.SetNewOutputAttributes(sAttNameExt, ref writer);
                    if (bNeedsBaseCalcs)
                    {
                        AddMN1OutputStockTotals(sAttNameExt, output.Calculators, ref writer);
                    }
                    else
                    {
                        AddMN1StockTotals(sAttNameExt, output.Calculators, ref writer);
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    //input
                    writer.WriteEndElement();
                }
            }
            return bHasTotals;
        }
        public bool SaveInputMN1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.InputGroup != null)
            {
                this.CurrentNodeName = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.InputGroup.Inputs.Count;
                    SetComparativeBaseAttributes(this.InputGroup, this.InputGroup.Inputs.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.MN1DescendentStock.Option1);
                this.InputGroup.SetNewInputGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //1.4.1 removed this -detracts from analyzing inputs and input series
                //AddMN1StockTotals(sAttNameExt, this.InputGroup.Calculators, ref writer);
                if (this.InputGroup.Inputs != null)
                {
                    bool bIsSeries = false;
                    bHasCalculations = SetInputStockTotals(writer, this.InputGroup.Inputs, bIsSeries);
                }
                writer.WriteEndElement();
            }
            return bHasCalculations;
        }
        private bool SetInputStockTotals(XmlWriter writer, List<Input> inputs, bool isSeries)
        {
            bool bHasTotals = false;
            if (inputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                //comps go horizontal
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    if (isSeries)
                    {
                        this.CurrentNodeName = Input.INPUT_PRICE_TYPES.inputseries.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = Input.INPUT_PRICE_TYPES.input.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var input in inputs)
                {
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (isSeries)
                        {
                            this.CurrentNodeName = Input.INPUT_PRICE_TYPES.inputseries.ToString();
                        }
                        else
                        {
                            this.CurrentNodeName = Input.INPUT_PRICE_TYPES.input.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(input, inputs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    input.SetNewInputAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (bNeedsBaseCalcs)
                        {
                            AddMN1InputStockTotals(sAttNameExt, input.Calculators, ref writer);
                        }
                        else
                        {
                            AddMN1StockTotals(sAttNameExt, input.Calculators, ref writer);
                        }
                        //don't process input series when analyses are run from group
                        //potential for too many series
                        if (input.Inputs != null
                            && this.GCCalculatorParams.StartingDocToCalcNodeName
                            != Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                            && isSeries == false)
                        {
                            bool bIsSeries = true;
                            bHasTotals = SetInputStockTotals(writer, input.Inputs, bIsSeries);
                        }
                    }
                    else
                    {
                        if (isSeries == false)
                        {
                            //only show inputs in input group analysis
                            if (this.GCCalculatorParams.StartingDocToCalcNodeName
                                == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                            {
                                //add input comp calcs
                                if (bNeedsBaseCalcs)
                                {
                                    AddMN1InputStockTotals(sAttNameExt, input.Calculators, ref writer);
                                }
                                else
                                {
                                    AddMN1StockTotals(sAttNameExt, input.Calculators, ref writer);
                                }
                            }
                            //don't process input series when analyses are run from group
                            //potential for too many series
                            if (input.Inputs != null
                                && this.GCCalculatorParams.StartingDocToCalcNodeName
                                != Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                            {
                                bool bIsSeries = true;
                                bHasTotals = SetInputStockTotals(writer, input.Inputs, bIsSeries);
                            }
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString()
                    && isSeries)
                {
                    //add the analysis
                    writer.WriteStartElement(Constants.ROOT_PATH);
                    writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                    i = 0;
                    foreach (var input in inputs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(input, inputs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                        AddMN1StockCompTotals(sAttNameExt, input.Calculators, ref writer);
                        i++;
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    //descendents
                    //op, comp, budop, investcomp
                    writer.WriteEndElement();
                }
            }
            return bHasTotals;
        }
        public bool SaveOutputMN1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.OutputGroup != null)
            {
                this.CurrentNodeName = Output.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OutputGroup.Outputs.Count;
                    SetComparativeBaseAttributes(this.OutputGroup, this.OutputGroup.Outputs.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.MN1DescendentStock.Option1);
                this.OutputGroup.SetNewOutputGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //1.4.1 removed this -detracts from analyzing inputs and input series
                //AddMN1StockTotals(sAttNameExt, this.OutputGroup.Calculators, ref writer);
                if (this.OutputGroup.Outputs != null)
                {
                    bool bIsSeries = false;
                    bHasCalculations = SetOutputStockTotals(writer, this.OutputGroup.Outputs, bIsSeries);
                }
                writer.WriteEndElement();
            }
            return bHasCalculations;
        }
        private bool SetOutputStockTotals(XmlWriter writer, List<Output> outputs, bool isSeries)
        {
            bool bHasTotals = false;
            if (outputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                //comps go horizontal
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    if (isSeries)
                    {
                        this.CurrentNodeName = Output.OUTPUT_PRICE_TYPES.outputseries.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = Output.OUTPUT_PRICE_TYPES.output.ToString();
                    }
                    writer.WriteStartElement(this.CurrentNodeName);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var output in outputs)
                {
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (isSeries)
                        {
                            this.CurrentNodeName = Output.OUTPUT_PRICE_TYPES.outputseries.ToString();
                        }
                        else
                        {
                            this.CurrentNodeName = Output.OUTPUT_PRICE_TYPES.output.ToString();
                        }
                        writer.WriteStartElement(this.CurrentNodeName);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(output, outputs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                    output.SetNewOutputAttributes(sAttNameExt, ref writer);
                    //don't display outputs (each comp can has multiple outputs -they would need one output)
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (bNeedsBaseCalcs)
                        {
                            AddMN1OutputStockTotals(sAttNameExt, output.Calculators, ref writer);
                        }
                        else
                        {
                            AddMN1StockTotals(sAttNameExt, output.Calculators, ref writer);
                        }
                        //don't process output series when analyses are run from group
                        //potential for too many series
                        if (output.Outputs != null
                            && this.GCCalculatorParams.StartingDocToCalcNodeName
                            != Output.OUTPUT_PRICE_TYPES.outputgroup.ToString()
                            && isSeries == false)
                        {
                            bool bIsSeries = true;
                            bHasTotals = SetOutputStockTotals(writer, output.Outputs, bIsSeries);
                        }
                    }
                    else
                    {
                        if (isSeries == false)
                        {
                            //only show outputs in output group analysis
                            if (this.GCCalculatorParams.StartingDocToCalcNodeName
                                == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                            {
                                //add output comp calcs
                                if (bNeedsBaseCalcs)
                                {
                                    AddMN1OutputStockTotals(sAttNameExt, output.Calculators, ref writer);
                                }
                                else
                                {
                                    AddMN1StockTotals(sAttNameExt, output.Calculators, ref writer);
                                }
                            }
                            //don't process output series when analyses are run from group
                            //potential for too many series
                            if (output.Outputs != null
                                && this.GCCalculatorParams.StartingDocToCalcNodeName
                                != Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                            {
                                bool bIsSeries = true;
                                bHasTotals = SetOutputStockTotals(writer, output.Outputs, bIsSeries);
                            }
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.MN1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.MN1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString()
                    && isSeries)
                {
                    //add the analysis
                    writer.WriteStartElement(Constants.ROOT_PATH);
                    writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                    i = 0;
                    foreach (var output in outputs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(output, outputs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.MN1DescendentStock.Option1);
                        AddMN1StockCompTotals(sAttNameExt, output.Calculators, ref writer);
                        i++;
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    //descendents
                    //op, comp, budop, investcomp
                    writer.WriteEndElement();
                }
            }
            return bHasTotals;
        }
       
        private void AddMN1StockTotals(string attNameExt, List<Calculator1> stocks, ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //each stock can contain as many ptstocks as totaled
                //so only one MN1Stock should be used (or inserting/updating els becomes difficult)
                MN1Stock stock = new MN1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (MN1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            writer.WriteStartElement(Constants.ROOT_PATH);
                            writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseAttributes(calc, this.ColumnCount, ref writer);
                            }
                            //the stock totals are added to currentelement using attnameext to distinguish observations
                            stock.SetDescendantMN1StockAttributes(attNameExt, ref writer);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }
        private void AddMN1StockCompTotals(string attNameExt, List<Calculator1> stocks,
            ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //each stock can contain as many ptstocks as totaled
                //so only one MN1Stock should be used (or inserting/updating els becomes difficult)
                MN1Stock stock = new MN1Stock();
                if (stocks != null)
                {
                    int i = 0;
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (MN1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            //note: this will fail with more than one calculator so make sure not to use more than one
                            //stock calculator per node
                            //the stock totals are added to currentelement using attnameext to distinguish observations
                            stock.SetDescendantMN1StockAttributes(attNameExt, ref writer);
                        }
                    }
                    i++;
                }
            }
        }
        private void AddMN1InputStockTotals(string attNameExt, List<Calculator1> stocks,
            ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //some stocks use calculator, not stock, results
                //the calcs are stored with the stock
                MN1Stock stock = new MN1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (MN1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            writer.WriteStartElement(Constants.ROOT_PATH);
                            writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseAttributes(calc, this.ColumnCount, ref writer);
                            }
                            stock.SetDescendantMN1StockInputAttributes(attNameExt, this.GCCalculatorParams,
                                ref writer);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }
        private void AddMN1OutputStockTotals(string attNameExt, List<Calculator1> stocks,
            ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //some stocks use calculator, not stock, results
                MN1Stock stock = new MN1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (MN1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            writer.WriteStartElement(Constants.ROOT_PATH);
                            writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseAttributes(calc, this.ColumnCount, ref writer);
                            }
                            stock.SetDescendantMN1StockOutputAttributes(attNameExt, this.GCCalculatorParams,
                                ref writer);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }
        private void SetComparativeBaseAttributes(Calculator1 baseObject, int colCount, 
            ref XmlWriter writer)
        {
            //need some attributes without attname extension
            writer.WriteAttributeString(Calculator1.cId, baseObject.Id.ToString());
            writer.WriteAttributeString(Calculator1.cName, baseObject.Name.ToString());
            //column count convention
            writer.WriteAttributeString("Files", colCount.ToString());
            //analyzer type
            writer.WriteAttributeString(Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //set the fn options 
            SetFNAttributes(ref writer);
        }
        private void SetSiblingComparativeBaseProperties(Calculator1 baseObject, string attNameExt,
            ref XmlWriter writer)
        {
            //sibling names, labels, dates needed
            writer.WriteAttributeString(string.Concat(Calculator1.cId, attNameExt), baseObject.Id.ToString());
            writer.WriteAttributeString(string.Concat(Calculator1.cName, attNameExt), baseObject.Name.ToString());
            writer.WriteAttributeString(string.Concat(Calculator1.cLabel, attNameExt), baseObject.Label.ToString());
            writer.WriteAttributeString(string.Concat(Calculator1.cDate, attNameExt), baseObject.Date.ToString());
        }
        public void SetFNAttributes(ref XmlWriter writer)
        {
            if (this.GCCalculatorParams.UrisToAnalyze != null)
            {
                int iFNCount = 0;
                foreach (var mn in this.GCCalculatorParams.UrisToAnalyze)
                {
                    if (mn == MNSR1.cContainerPrice)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cContainerPrice, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cContainerSizeInSSUnits)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cContainerSizeInSSUnits, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cServingCost)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cServingCost, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cActualServingSize)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cActualServingSize, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cTypicalServingSize)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cTypicalServingSize, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cTypicalServingsPerContainer)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cTypicalServingsPerContainer, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cActualServingsPerContainer)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cActualServingsPerContainer, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cWater_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cWater_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cEnerg_Kcal)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cEnerg_Kcal, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cProtein_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cProtein_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cLipid_Tot_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cLipid_Tot_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cAsh_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cAsh_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cCarbohydrt_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cCarbohydrt_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFiber_TD_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFiber_TD_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cSugar_Tot_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cSugar_Tot_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cCalcium_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cCalcium_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cIron_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cIron_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cMagnesium_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cMagnesium_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cPhosphorus_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cPhosphorus_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cPotassium_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cPotassium_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cSodium_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cSodium_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cZinc_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cZinc_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cCopper_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cCopper_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cManganese_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cManganese_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cSelenium_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cSelenium_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_C_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_C_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cThiamin_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cThiamin_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cRiboflavin_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cRiboflavin_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cNiacin_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cNiacin_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cPanto_Acid_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cPanto_Acid_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_B6_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_B6_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFolate_Tot_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFolate_Tot_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFolic_Acid_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFolic_Acid_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFood_Folate_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFood_Folate_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFolate_DFE_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFolate_DFE_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cCholine_Tot_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cCholine_Tot_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_B12_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_B12_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_A_IU)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_A_IU, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_A_RAE)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_A_RAE, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cRetinol_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cRetinol_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cAlpha_Carot_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cAlpha_Carot_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cBeta_Carot_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cBeta_Carot_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cBeta_Crypt_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cBeta_Crypt_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cLycopene_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cLycopene_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cLut_Zea_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cLut_Zea_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_E_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_E_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_D_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_D_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cViVit_D_IU)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cViVit_D_IU, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cVit_K_pg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cVit_K_pg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFA_Sat_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFA_Sat_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFA_Mono_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFA_Mono_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cFA_Poly_g)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cFA_Poly_g, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cCholestrl_mg)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cCholestrl_mg, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cExtra1)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cExtra1, "true");
                        iFNCount++;
                    }
                    else if (mn == MNSR1.cExtra2)
                    {
                        writer.WriteAttributeString(
                            MNSR1.cExtra2, "true");
                        iFNCount++;
                    }
                }
                //tells ss how many to display
                writer.WriteAttributeString(
                    MNSR1.cFNCount, iFNCount.ToString());
            }
        }
    }
}
