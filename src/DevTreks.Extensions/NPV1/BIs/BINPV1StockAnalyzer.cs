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
    ///Purpose:		1. Group the NPV elements by an AggregationId (TypeId, GroupId, Label)
    ///             2. Run analyses using collections of base els converted to calculators
    ///             3. Save the analyses as xml
    ///Author:		www.devtreks.org
    ///Date:		2014, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// NOTES        1. Each base element gets converted to a calculator. 
    ///             The calc is a NPV1Stock which inherits from CostBenefitCalculator.
    ///             The collection of calcs is used to run all analyses.
    /// </summary>
    public class BINPV1StockAnalyzer : BudgetInvestmentCalculator
    {
        public BINPV1StockAnalyzer(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set Indic1Stock
            Init();
        }
        public BINPV1StockAnalyzer()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //set NPV1DescendentStock state so that descendant stock totals will have good base properties
            //the base.Analyzer is set when the Save... methods are run
            this.NPV1DescendentStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (this.GCCalculatorParams.LinkedViewElement != null)
            {
                this.NPV1DescendentStock.SetDescendantNPV1StockProperties(
                    this.GCCalculatorParams.LinkedViewElement);
            }
        }
        //stateful analyzer used to set base.Analyzer properties in descendants (name, id)
        //and to hold descendent input and output stocks for analysis
        public NPV1Stock NPV1DescendentStock { get; set; }
        //stateful currentnodename
        private string CurrentNodeName { get; set; }

        //these objects hold collections of NPV1s for running totals.
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
        public bool SaveNPV1StockTotals()
        {
            bool bHasCalculations = false;
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath
                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                return bHasCalculations;
            //new temporary path to store calculator results
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
                    bHasCalculations = SaveBINPV1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.operationprices
                    || this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    bHasCalculations = SaveOCNPV1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    bHasCalculations = SaveOutcomeNPV1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    bHasCalculations = SaveInputNPV1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    bHasCalculations = SaveOutputNPV1StockTotals(writer);
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
        
        
        private void SetCalculatorId(Calculator1 baseElement, NPV1Stock stock)
        {
            //change and progress analyzers change the calcid using Change1.Stocks
            if (!NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                SetCalcId(baseElement, stock);
            }
        }
        private void SetCalcId(Calculator1 baseElement, NPV1Stock stock)
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
        public bool SetBINPV1StockTotals()
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
        public bool SetOCNPV1StockTotals()
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
        public bool SetOutcomeNPV1StockTotals()
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
        public bool SetInputNPV1StockTotals()
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
                NPV1AnalyzerHelper.CopyStockCalculator(output.Calculators, o.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(outputseries.Calculators, outputseries2.Calculators);
                    }
                    o.Outputs.Add(outputseries2);
                }
            }
            outGroup.Outputs.Add(o);
        }
        
        //this is called one time after the stateful collections are built
        public bool SetOutputNPV1StockTotals()
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
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                     qry4 = bis
                    .OrderBy(m => m.Date.Year.ToString())
                    .GroupBy(m => m.Date.Year.ToString());
                SetBINPVsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    SetBIGroupNPV1StockTotals();
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                     qry4 = bis
                    .OrderBy(m => m.AlternativeType)
                    .GroupBy(m => m.AlternativeType);
                SetBINPVsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    SetBIGroupNPV1StockTotals();
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                    qry4 = bis
                    .GroupBy(m => m.Id.ToString());
                SetBINPVsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    SetBIGroupNPV1StockTotals();
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
                        SetBINPVsAggregation(qry1);
                        //BudgetInvestmentGroup biGroup1 = GetBINPVsAggregation(qry1);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupNPV1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry2 = bis
                            .GroupBy(m => m.GroupId.ToString());
                        SetBINPVsAggregation(qry2);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupNPV1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry3 = bis
                            .GroupBy(m => m.Label);
                        SetBINPVsAggregation(qry3);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupNPV1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry4 = bis
                            .GroupBy(m => m.Id.ToString());
                        SetBINPVsAggregation(qry4);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            SetBIGroupNPV1StockTotals();
                        }
                        break;
                    default:
                        break;
                }
            }
            return bHasGroups;
        }
        private void SetBINPVsAggregation(
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
                    //may want to display the aggregated tp results with change analysis
                    List<Calculator1> biCalcs = new List<Calculator1>();
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
                        NPV1AnalyzerHelper.CopyStockCalculator(bi.Calculators, biCalcs);
                        //calculators start with inputs and outputs
                        //this aggregates the tps, opcomps, and outcomes
                        //and adds stock totals to those collections
                        bool bHasTPTotals = SetTPAggregation(bi.TimePeriods);
                        i += 1;
                    }
                    bool bHasTotals = false;
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                    {
                        //multiple observations mean multiple bis to aggregate using base npv calcs
                        if ((this.GCCalculatorParams.ParentBudgetInvestment.Observations > 1
                            && biCalcs.Count > 0)
                            || qry.Count() > 1)
                        {
                            //now use the base npv calcs to run bistock analyses
                            //this can be byref
                            this.GCCalculatorParams.ParentBudgetInvestment.Calculators = biCalcs;
                            bHasTotals = SetBINPV1StockTotal();
                        }
                        else
                        {
                            //the tp aggregation add the cumulative totals to
                            //this.GCCalculatorParams.ParentBudgetInvestment.Calculator
                            //display it, rather than a bunch of zeroes
                            bHasTotals = true;
                        }
                    }
                    else
                    {
                        //now use the base npv calcs to run bistock analyses
                        //this can be byref
                        this.GCCalculatorParams.ParentBudgetInvestment.Calculators = biCalcs;
                        bHasTotals = SetBINPV1StockTotal();
                    }
                    if (bHasTotals)
                    {
                        //avoid a byref by building a new bi
                        BudgetInvestment budInvest
                            = new BudgetInvestment(this.GCCalculatorParams, this.GCCalculatorParams.ParentBudgetInvestment);
                        budInvest.TimePeriods = new List<TimePeriod>();
                        budInvest.Calculators = new List<Calculator1>();
                        if (this.GCCalculatorParams.ParentBudgetInvestment.Calculators != null)
                        {
                            NPV1AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams.ParentBudgetInvestment.Calculators,
                                budInvest.Calculators);
                        }
                        foreach (var tp in this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods)
                        {
                            TimePeriod newTp = new TimePeriod(this.GCCalculatorParams, tp);
                            newTp.Calculators = new List<Calculator1>();
                            SetNewTPFromOldTp(newTp, tp);
                            if (tp.Calculators != null)
                            {
                                NPV1AnalyzerHelper.CopyStockCalculator(tp.Calculators,
                                    newTp.Calculators);
                            }
                            budInvest.TimePeriods.Add(newTp);
                        }
                        this.GCCalculatorParams.ParentBudgetInvestmentGroup
                            .BudgetInvestments.Add(budInvest);
                        //keep them ordered by the type of analysis
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
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
        private bool SetBIGroupNPV1StockTotals()
        {
            bool bHasTotals = false;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock biGroupStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators == null)
                    this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators = new List<Calculator1>();
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators
                        .FirstOrDefault(), biGroupStock);
                }
                else
                {
                    //need the options
                    biGroupStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
                CopyBaseElementProperties(this.GCCalculatorParams.ParentBudgetInvestmentGroup, biGroupStock);
                //each bi holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                var BIs = this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments.OrderBy(bi => bi.Date);
                //run the bi analysis
                List<Calculator1> calcs = new List<Calculator1>();
                if (BIs != null)
                {
                    foreach (var bi in BIs)
                    {
                        NPV1AnalyzerHelper.AddCalculators(bi.Calculators, calcs);
                    }
                    //change analysis
                    AdjustAncestorBICalcs(calcs);
                    //run the analyses by aggregating the input.calcs
                    bHasTotals = biGroupStock.RunAnalyses(calcs);
                    
                }
                if (bHasTotals && biGroupStock != null)
                {
                    //preferable to display cumulative results from children, rather than a bunch of zeroes
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString()
                        && calcs.Count > 1)
                    {
                        //some analyzers must now transfer calcs to series
                        AddCalculatorsToBIs(biGroupStock, BIs);
                    }
                    else
                    {
                        AddCalculatorsToBIs(biGroupStock, BIs);
                    }
                    this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators = new List<Calculator1>();
                    //npvStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(this.GCCalculatorParams.ParentBudgetInvestmentGroup, biGroupStock);
                    this.GCCalculatorParams.ParentBudgetInvestmentGroup.Calculators.Add(biGroupStock);
                    //replace this.budgetgroup
                    this.BudgetGroup = this.GCCalculatorParams.ParentBudgetInvestmentGroup;
                }
            }
            return bHasTotals;
        }
        private bool SetBINPV1StockTotal()
        {
            bool bHasTotals = false;
            if (this.GCCalculatorParams.ParentBudgetInvestment != null)
            {
                NPV1Stock biStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (this.GCCalculatorParams.ParentBudgetInvestment.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(this.GCCalculatorParams.ParentBudgetInvestment.Calculators
                        .FirstOrDefault(), biStock);
                }
                else
                {
                    //need the options
                    biStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
                CopyBaseElementProperties(this.GCCalculatorParams.ParentBudgetInvestment, biStock);
                //run the analyses by aggregating the calcs
                //note that the change analyses replace these calcs at budgetgroup.runanalyses
                bHasTotals = biStock.RunAnalyses(this.GCCalculatorParams.ParentBudgetInvestment.Calculators);
                if (bHasTotals && biStock != null)
                {
                    //replace the existing calcs with bistock
                    this.GCCalculatorParams.ParentBudgetInvestment.Calculators = new List<Calculator1>();
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(this.GCCalculatorParams.ParentBudgetInvestment, biStock);
                    this.GCCalculatorParams.ParentBudgetInvestment.Calculators.Add(biStock);
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
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .OrderBy(m => m.Date.Year.ToString())
                    .GroupBy(m => m.Date.Year.ToString());
                tempBI = SetTPNPVsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = SetTPNPV1StockTotals(tempBI);
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .OrderBy(m => m.AlternativeType)
                    .GroupBy(m => m.AlternativeType);
                tempBI = SetTPNPVsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = SetTPNPV1StockTotals(tempBI);
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .GroupBy(m => m.Id.ToString());
                tempBI = SetTPNPVsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = SetTPNPV1StockTotals(tempBI);
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
                        tempBI = SetTPNPVsAggregation(qry1);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPNPV1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry2 = tps
                            .GroupBy(m => m.GroupId.ToString());
                        tempBI = SetTPNPVsAggregation(qry2);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPNPV1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry3 = tps
                            .GroupBy(m => m.Label);
                        tempBI = SetTPNPVsAggregation(qry3);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPNPV1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry4 = tps
                            .GroupBy(m => m.Id.ToString());
                        tempBI = SetTPNPVsAggregation(qry4);
                        if (tempBI != null)
                        {
                            bHasGroups = SetTPNPV1StockTotals(tempBI);
                        }
                        break;
                    default:
                        break;
                }
            }
            return bHasGroups;
        }
        private BudgetInvestment SetTPNPVsAggregation(IEnumerable<System.Linq.IGrouping<string, TimePeriod>> qry)
        {
            BudgetInvestment tempBI = new BudgetInvestment(this.GCCalculatorParams, this.GCCalculatorParams.ParentBudgetInvestment);
            tempBI.TimePeriods = new List<TimePeriod>();
            tempBI.Calculators = new List<Calculator1>();
            if (this.GCCalculatorParams.ParentBudgetInvestment.Calculators != null)
            {
                NPV1AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams.ParentBudgetInvestment.Calculators, tempBI.Calculators);
            }
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
                        NPV1AnalyzerHelper.CopyStockCalculator(tp.Calculators, newTP.Calculators);
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
                    //don't run tp totals here; wait for tempBI, when ancestors can be added correctly
                    if (bHasTotals)
                    {
                        //add to temporary aggregator (runs partial bi calcs)
                        tempBI.TimePeriods.Add(timeperiodTotals);
                        if (this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods == null)
                            this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods = new List<TimePeriod>();
                        this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods.Add(timeperiodTotals);
                        //don't add to parent bi; it will happen after tps calcs are run
                        //keep the tps ordered by analysistype, byref to PT.TPs too                       
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
            //unlike lca, each element has good npv totals
            if (newTP.Calculators == null)
            {
                newTP.Calculators = new List<Calculator1>();
            }
            NPV1AnalyzerHelper.CopyStockCalculator(oldTP.Calculators, newTP.Calculators);
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
                            AddNewOCToCollection(oc, newTP.OperationComponents);
                            //calculators start with inputs and outputs
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
                            AddNewOutcomeToCollection(oc, newTP.Outcomes);
                            //calculators start with inputs and outputs
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
            oc.Calculators = new List<Calculator1>();
            if (opComp.Calculators != null)
            {
                NPV1AnalyzerHelper.CopyStockCalculator(opComp.Calculators, oc.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(input.Calculators, i.Calculators);
                    }
                    oc.Inputs.Add(i);
                }
            }
            ocs.Add(oc);
        }
        private void AddNewOutcomeToCollection(Outcome outcome, List<Outcome> ocs)
        {
            Outcome oc = new Outcome(this.GCCalculatorParams, outcome);
            oc.Calculators = new List<Calculator1>();
            if (outcome.Calculators != null)
            {
                NPV1AnalyzerHelper.CopyStockCalculator(outcome.Calculators, oc.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(output.Calculators, o.Calculators);
                    }
                    oc.Outputs.Add(o);
                }
            }
            ocs.Add(oc);
        }
        private bool SetTPNPV1StockTotals(BudgetInvestment tempBI)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (tempBI.TimePeriods != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock biStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (tempBI.Calculators == null)
                    tempBI.Calculators = new List<Calculator1>();
                if (tempBI.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(tempBI.Calculators
                        .FirstOrDefault(), biStock);
                }
                else
                {
                    //need the options
                    biStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
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
                        bHasAnalysis = SetTPNPV1StockTotals(tp);
                        NPV1AnalyzerHelper.AddCalculators(tp.Alternative2, tp.Calculators, calcs);
                    }
                    AddAncestorTPCalcs(calcs);
                    //run the analyses by aggregating the calcs
                    bHasTotals = biStock.RunAnalyses(calcs);
                }
                if (bHasTotals && biStock != null)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToTPs(biStock, TPs);
                    //this adds the tps to parentbi
                    AddTPCalcsToParentBI(TPs);
                    //lcaStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(this.GCCalculatorParams.ParentBudgetInvestment, biStock);
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                       == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                    {
                        if (this.GCCalculatorParams.ParentBudgetInvestment.Observations <= 1)
                        {
                            //want the cumulative results displayed by parent if no new calcs needed by parent
                            this.GCCalculatorParams.ParentBudgetInvestment.Calculators.Add(biStock);
                        }
                    }
                    //use SetBISTockTotals to set biStock calcs
                }
            }
            return bHasTotals;
        }
        private void OrderTPs(BudgetInvestment bi)
        {
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.AlternativeType).ToList();
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.Date.Year).ToList();
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.Id).ToList();
            }
            else
            {
                //keep the existing order
            }
        }
        private bool SetTPNPV1StockTotals(TimePeriod tp)
        {
            //tp already has good calcs, but a needs tp.calc to change to tp.props and multiplied 
            bool bHasTotals = false;
            //add the oc stock totals using the OCs.Inputs collection that now hold inputstock calcs
            //add indicator1stocks to corresponding NPV element
            NPV1Stock tpStockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (tp.Calculators == null)
                tp.Calculators = new List<Calculator1>();
            if (tp.Calculators.Count > 0)
            {
                NPV1AnalyzerHelper.CopyandInitStockCalculator(tp.Calculators
                    .FirstOrDefault(), tpStockMember);
            }
            else
            {
                //need the options
                tpStockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
            }
            CopyBaseElementProperties(tp, tpStockMember);
            //run the analyses by aggregating the calcs
            //keep in mind these are really totals; the full calcs use ancestors at tempBI level
            bHasTotals = tpStockMember.RunAnalyses(tp.Calculators);
            if (tpStockMember != null)
            {
                //reset tp.calcs - already have totals in tpstockmember
                tp.Calculators = new List<Calculator1>();
                //npvStockMember.Total, or .Stats holds aggregated numbers
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
                    OperationComponentGroup ocGroup1 = GetOCNPVsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = SetOCNPV1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry2 = ocs
                        .GroupBy(m => m.GroupId.ToString());
                    OperationComponentGroup ocGroup2 = GetOCNPVsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = SetOCNPV1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry3 = ocs
                        .GroupBy(m => m.Label);
                    OperationComponentGroup ocGroup3 = GetOCNPVsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = SetOCNPV1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                        && (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                        || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices))
                    {
                        //id field will result in original structure
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        OperationComponentGroup ocGroup4 = GetOCNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOCNPV1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                        && (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                        || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices))
                    {
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        OperationComponentGroup ocGroup4 = GetOCNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOCNPV1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        //id field will result in original structure
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .GroupBy(m => m.Id.ToString());
                        OperationComponentGroup ocGroup4 = GetOCNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOCNPV1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        
        private OperationComponentGroup GetOCNPVsAggregation(
            IEnumerable<System.Linq.IGrouping<string, OperationComponent>> qry)
        {
            //use new, not byref, objects
            if (this.OCGroup == null)
                this.OCGroup = new OperationComponentGroup();
            OperationComponentGroup ocGroup2 = new OperationComponentGroup(this.GCCalculatorParams, this.OCGroup);
            NPV1AnalyzerHelper.CopyStockCalculator(this.OCGroup.Calculators, ocGroup2.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(oc.Calculators, opComp.Calculators);
                        //calculators start with inputs and outputs
                        if (oc.Inputs != null)
                        {
                            foreach (var input in oc.Inputs)
                            {
                                Input input1 = new Input(this.GCCalculatorParams, input);
                                //note this has to be i
                                input1.Alternative2 = i;
                                input1.Calculators = new List<Calculator1>();
                                NPV1AnalyzerHelper.CopyStockCalculator(input.Calculators, input1.Calculators);
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
        private bool SetOCNPV1StockTotals(OperationComponentGroup ocGroup)
        {
            bool bHasTotals = false;
            bool bHasAnalysis = false;
            if (ocGroup.OperationComponents != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock ocGroupStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (ocGroup.Calculators == null)
                    ocGroup.Calculators = new List<Calculator1>();
                if (ocGroup.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(ocGroup.Calculators
                        .FirstOrDefault(), ocGroupStock);
                }
                else
                {
                    //need the options
                    ocGroupStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
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
                        bHasAnalysis = SetOCNPV1StockTotals(oc);
                        NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
                    //npvStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(ocGroup, ocGroupStock);
                    ocGroup.Calculators.Add(ocGroupStock);
                    //replace this.ocgroup
                    this.OCGroup = ocGroup;
                }
            }
            return bHasTotals;
        }
        //this statistically analyzes the oc's calculators (which represent individual ocs)
        private bool SetOCNPV1StockTotals(OperationComponent oc)
        {
            bool bHasTotals = false;
            //the inputs already have npv calculations and need no further calculations
            //bHasTotals = SetInputNPV1StockTotals(oc);
            NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (oc.Calculators == null)
                oc.Calculators = new List<Calculator1>();
            if (oc.Calculators.Count > 0)
            {
                NPV1AnalyzerHelper.CopyandInitStockCalculator(oc.Calculators
                    .FirstOrDefault(), npv1StockMember);
            }
            else
            {
                npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
            }
            CopyBaseElementProperties(oc, npv1StockMember);
            //run the analyses by aggregating the outcome.calcs
            bHasTotals = npv1StockMember.RunAnalyses(oc.Calculators);
            oc.Calculators = new List<Calculator1>();
            //npvStockMember.Total, or .Stats holds aggregated numbers
            SetCalculatorId(oc, npv1StockMember);
            oc.Calculators.Add(npv1StockMember);
            return bHasTotals;
        }
        //this will statistically analyze the oc's inputs (not needed yet)
        private bool SetOCNPV1PsandQsTotals(OperationComponent oc)
        {
            bool bHasTotals = false;
            //this method is not needed because each input has good initial npv calculations
            //bHasTotals = SetInputNPV1StockTotals(oc);
            //add the oc stock totals using the OCs.Inputs collection that now hold inputstock calcs
            //add indicator1stocks to corresponding NPV element
            NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (oc.Calculators == null)
                oc.Calculators = new List<Calculator1>();
            if (oc.Calculators.Count > 0)
            {
                NPV1AnalyzerHelper.CopyandInitStockCalculator(oc.Calculators
                    .FirstOrDefault(), npv1StockMember);
            }
            else
            {
                npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
            }
            CopyBaseElementProperties(oc, npv1StockMember);
            //inputs not used because ocs have total oc calcs
            //inputs not used in most analyzers because npv calcs on hand already
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                if (oc.Inputs != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var input in oc.Inputs)
                    {
                        NPV1AnalyzerHelper.AddCalculators(input.Calculators, calcs);
                    }
                    //some analyzers only sum totals for the calcs (i.e. no changes in ins needed)
                    npv1StockMember.CalcParameters.CurrentElementNodeName
                        = Input.INPUT_PRICE_TYPES.input.ToString();
                    //run the analyses by aggregating the input.calcs
                    bHasTotals = npv1StockMember.RunAnalyses(calcs);
                }
            }
            else
            {
                //run the analyses by aggregating the input.calcs
                bHasTotals = npv1StockMember.RunAnalyses(oc.Calculators);
            }
            //copy TOCAmount, ... to oc.Calculators.FirstofDefault (it will be subsequently transferred to remainder)
            //npv1StockMember.CopyTotalNPV1RPropertiesToCalc(oc.Calculators.FirstOrDefault());
            //oc.Calculators = new List<Calculator1>();
            ////npvStockMember.Total, or .Stats holds aggregated numbers
            //SetCalculatorId(oc, npv1StockMember);
            //oc.Calculators.Add(npv1StockMember);
            return bHasTotals;
        }
        //this method is not needed because each input has good initial npv calculations
        //keep it around for potential future use
        private bool SetInputNPV1StockTotals(OperationComponent oc)
        {
            bool bHasTotals = false;
            //run the input analysis using this.NPV1DescendentStock (so that ancestors can use them)
            if (oc.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock npv1Stock = new NPV1Stock(this.GCCalculatorParams,
                    this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (oc.Calculators == null)
                    oc.Calculators = new List<Calculator1>();
                if (oc.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(oc.Calculators
                        .FirstOrDefault(), npv1Stock);
                }
                else
                {
                    npv1Stock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
                CopyBaseElementProperties(oc, npv1Stock);
                foreach (var input in oc.Inputs)
                {
                    //add indicator1stocks to corresponding NPV element
                    NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    if (input.Calculators == null)
                        input.Calculators = new List<Calculator1>();
                    if (input.Calculators.Count > 0)
                    {
                        NPV1AnalyzerHelper.CopyandInitStockCalculator(input.Calculators
                            .FirstOrDefault(), npv1StockMember);
                    }
                    else
                    {
                        npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
                    }
                    CopyBaseElementProperties(input, npv1StockMember);
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        npv1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                    }
                    else
                    {
                        npv1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                    }
                    if (input.Calculators != null)
                    {
                        npv1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                        foreach (var calc in input.Calculators)
                        {
                            //copy the base totals to stockmember; it will aggregate those base totals into AMTotals
                            NPV1AnalyzerHelper.CopyStockCalculator(calc, npv1StockMember);
                        }
                    }
                    //add the stockmember to the base stock
                    bHasTotals = npv1StockMember.RunAnalyses();
                    SetCalculatorId(input, npv1StockMember);
                    input.Calculators.Add(npv1StockMember);
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
                    OutcomeGroup ocGroup1 = GetOutcomeNPVsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = SetOutcomeNPV1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry2 = ocs
                        .GroupBy(m => m.GroupId.ToString());
                    OutcomeGroup ocGroup2 = GetOutcomeNPVsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = SetOutcomeNPV1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry3 = ocs
                        .GroupBy(m => m.Label);
                    OutcomeGroup ocGroup3 = GetOutcomeNPVsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = SetOutcomeNPV1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .OrderBy(m => m.Date.Year.ToString())
                                .GroupBy(m => m.Date.Year.ToString());
                        OutcomeGroup ocGroup4 = GetOutcomeNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOutcomeNPV1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .OrderBy(m => m.AlternativeType)
                                .GroupBy(m => m.AlternativeType);
                        OutcomeGroup ocGroup4 = GetOutcomeNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOutcomeNPV1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .GroupBy(m => m.Id.ToString());
                        OutcomeGroup ocGroup4 = GetOutcomeNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetOutcomeNPV1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }

        private OutcomeGroup GetOutcomeNPVsAggregation(
            IEnumerable<System.Linq.IGrouping<string, Outcome>> qry)
        {
            //use new, not byref, objects
            if (this.OutcomeGroup == null)
                this.OutcomeGroup = new OutcomeGroup();
            OutcomeGroup ocGroup2 = new OutcomeGroup(this.GCCalculatorParams, this.OutcomeGroup);
            NPV1AnalyzerHelper.CopyStockCalculator(this.OutcomeGroup.Calculators, ocGroup2.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(oc.Calculators, outcome.Calculators);
                        //calculators start with inputs and outputs
                        if (oc.Outputs != null)
                        {
                            foreach (var output in oc.Outputs)
                            {
                                Output output1 = new Output(this.GCCalculatorParams, output);
                                output1.Calculators = new List<Calculator1>();
                                //this must be i
                                output1.Alternative2 = i;
                                if (output.Calculators != null)
                                {
                                    NPV1AnalyzerHelper.CopyStockCalculator(output.Calculators, output1.Calculators);
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
        private bool SetOutcomeNPV1StockTotals(OutcomeGroup ocGroup)
        {
            bool bHasTotals = false;
            bool bHasAnalysis = false;
            if (ocGroup.Outcomes != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock ocGroupStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (ocGroup.Calculators == null)
                    ocGroup.Calculators = new List<Calculator1>();
                if (ocGroup.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(ocGroup.Calculators
                        .FirstOrDefault(), ocGroupStock);
                }
                else
                {
                    ocGroupStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
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
                        bHasAnalysis = SetOutcomeNPV1StockTotals(oc);
                        NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
                    //npvStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(ocGroup, ocGroupStock);
                    ocGroup.Calculators.Add(ocGroupStock);
                    //replace this.outcomegroup
                    this.OutcomeGroup = ocGroup;
                }
            }
            return bHasTotals;
        }
        //this statistically analyzes the outcome's calculators (which represent individual outcomes)
        //the children outputs are displayed unchanged from npv calcs
        private bool SetOutcomeNPV1StockTotals(Outcome oc)
        {
            bool bHasTotals = false;
            bHasTotals = SetOutcomeNPV1StockPsandQs(oc);
            NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (oc.Calculators == null)
                oc.Calculators = new List<Calculator1>();
            if (oc.Calculators.Count > 0)
            {
                NPV1AnalyzerHelper.CopyandInitStockCalculator(oc.Calculators
                    .FirstOrDefault(), npv1StockMember);
            }
            else
            {
                npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
            }
            CopyBaseElementProperties(oc, npv1StockMember);
            //run the analyses by aggregating the outcome.calcs
            bHasTotals = npv1StockMember.RunAnalyses(oc.Calculators);
            oc.Calculators = new List<Calculator1>();
            //npvStockMember.Total, or .Stats holds aggregated numbers
            SetCalculatorId(oc, npv1StockMember);
            oc.Calculators.Add(npv1StockMember);
            return bHasTotals;
        }
        //this will aggregate the outputs to pass TRAmount, TRPrice to OC
        private bool SetOutcomeNPV1StockPsandQs(Outcome oc)
        {
            bool bHasTotals = false;
            //this method is not needed because each output has good initial npv calculations
            //bHasTotals = SetOutputNPV1StockTotals(oc);
            //add the oc stock totals using the OCs.Outputs collection that now hold outputstock calcs
            //add indicator1stocks to corresponding NPV element
            NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (oc.Calculators == null)
                oc.Calculators = new List<Calculator1>();
            if (oc.Calculators.Count > 0)
            {
                NPV1AnalyzerHelper.CopyandInitStockCalculator(oc.Calculators
                    .FirstOrDefault(), npv1StockMember);
            }
            else
            {
                npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
            }
            CopyBaseElementProperties(oc, npv1StockMember);
            //outputs are used to support cost effectiveness analysis
            if (oc.Outputs != null)
            {
                List<Calculator1> calcs = new List<Calculator1>();
                foreach (var output in oc.Outputs)
                {
                    NPV1AnalyzerHelper.AddCalculators(output.Calculators, calcs);
                }
                //some analyzers only sum totals for the calcs (i.e. no changes in ins needed)
                npv1StockMember.CalcParameters.CurrentElementNodeName
                    = Output.OUTPUT_PRICE_TYPES.output.ToString();
                //run the analyses by aggregating the output.calcs
                bHasTotals = npv1StockMember.RunAnalyses(calcs);
            }
            //copy TRAmount, ... to oc.Calculators.FirstofDefault (it will be subsequently transferred to remainder)
            npv1StockMember.CopyTotalNPV1RPropertiesToCalc(oc.Calculators.FirstOrDefault());
            //don't need the calculator anymore
            //oc.Calculators = new List<Calculator1>();
            ////npvStockMember.Total, or .Stats holds aggregated numbers
            //SetCalculatorId(oc, npv1StockMember);
            //oc.Calculators.Add(npv1StockMember);
            return bHasTotals;
        }
        //this method is not needed because each output has good initial npv calculations
        //keep it around for future use
        private bool SetOutputNPV1StockTotals(Outcome oc)
        {
            bool bHasTotals = false;
            //run the output analysis using this.NPV1DescendentStock (so that ancestors can use them)
            if (oc.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock npv1Stock = new NPV1Stock(this.GCCalculatorParams,
                    this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (oc.Calculators == null)
                    oc.Calculators = new List<Calculator1>();
                if (oc.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(oc.Calculators
                        .FirstOrDefault(), npv1Stock);
                }
                else
                {
                    npv1Stock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
                CopyBaseElementProperties(oc, npv1Stock);
                foreach (var output in oc.Outputs)
                {
                    //add indicator1stocks to corresponding NPV element
                    NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    if (output.Calculators == null)
                        output.Calculators = new List<Calculator1>();
                    if (output.Calculators.Count > 0)
                    {
                        NPV1AnalyzerHelper.CopyandInitStockCalculator(output.Calculators
                            .FirstOrDefault(), npv1StockMember);
                    }
                    else
                    {
                        npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
                    }
                    CopyBaseElementProperties(output, npv1StockMember);
                    if (output.Calculators != null)
                    {
                        npv1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                        foreach (var calc in output.Calculators)
                        {
                            //copy the base totals to stockmember; it will aggregate those base totals into AMTotals
                            NPV1AnalyzerHelper.CopyStockCalculator(calc, npv1StockMember);
                        }
                    }
                    //add the stockmember to the base stock
                    bHasTotals = npv1StockMember.RunAnalyses();
                    SetCalculatorId(output, npv1StockMember);
                    output.Calculators.Add(npv1StockMember);
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
                    InputGroup ocGroup1 = GetInputNPVsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = SetInputNPV1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry2 = inputs
                        .GroupBy(m => m.GroupId.ToString());
                    InputGroup ocGroup2 = GetInputNPVsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = SetInputNPV1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry3 = inputs
                        .GroupBy(m => m.Label);
                    InputGroup ocGroup3 = GetInputNPVsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = SetInputNPV1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        InputGroup ocGroup4 = GetInputNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetInputNPV1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        InputGroup ocGroup4 = GetInputNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetInputNPV1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .GroupBy(m => m.Id.ToString());
                        InputGroup ocGroup4 = GetInputNPVsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = SetInputNPV1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private InputGroup GetInputNPVsAggregation(
            IEnumerable<System.Linq.IGrouping<string, Input>> qry)
        {
            if (this.InputGroup == null)
                this.InputGroup = new InputGroup();
            //use new, not byref, objects
            InputGroup inputGroup = new InputGroup(this.GCCalculatorParams, this.InputGroup);
            NPV1AnalyzerHelper.CopyStockCalculator(this.InputGroup.Calculators, inputGroup.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(input.Calculators, inputNew.Calculators);
                        if (input.Inputs != null)
                        {
                            foreach (var input2 in input.Inputs)
                            {
                                Input input1 = new Input(this.GCCalculatorParams, input2);
                                input1.Calculators = new List<Calculator1>();
                                //this must be i
                                input1.Alternative2 = i;
                                NPV1AnalyzerHelper.CopyStockCalculator(input2.Calculators, input1.Calculators);
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
        private bool SetInputNPV1StockTotals(InputGroup inputGroup)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (inputGroup.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock inGroupStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (inputGroup.Calculators == null)
                    inputGroup.Calculators = new List<Calculator1>();
                if (inputGroup.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(inputGroup.Calculators
                        .FirstOrDefault(), inGroupStock);
                }
                else
                {
                    //need the options
                    inGroupStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
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
                        bHasAnalysis = SetInputNPV1StockTotals(input);
                        NPV1AnalyzerHelper.AddCalculators(input.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = inGroupStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToInputs(inGroupStock, Inputs);
                    inputGroup.Calculators = new List<Calculator1>();
                    //npvStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(inputGroup, inGroupStock);
                    inputGroup.Calculators.Add(inGroupStock);
                    //reset this.InputGroup
                    this.InputGroup = inputGroup;
                }
            }
            return bHasTotals;
        }
        private bool SetInputNPV1StockTotals(Input input)
        {
            bool bHasAnalysis = true;
            //run the input analysis
            if (input != null)
            {
                if ((this.GCCalculatorParams.StartingDocToCalcNodeName
                    == Input.INPUT_PRICE_TYPES.inputgroup.ToString()))
                {
                    //run the base calcs, but don't run analyses
                    bHasAnalysis = SetInputSeriesNPV1BaseStockTotals(input);
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
        private bool SetInputSeriesNPV1BaseStockTotals(Input inputserie)
        {
            bool bHasTotals = false;
            //this gives each input and output correct number of lcccalc observations
            NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (inputserie.Calculators == null)
                inputserie.Calculators = new List<Calculator1>();
            if (inputserie.Calculators.Count > 0)
            {
                NPV1AnalyzerHelper.CopyandInitStockCalculator(inputserie.Calculators
                    .FirstOrDefault(), npv1StockMember);
            }
            else
            {
                //need the options
                npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
            }
            CopyBaseElementProperties(inputserie, npv1StockMember);
            //run the series analyses by aggregating the calcs
            bHasTotals = npv1StockMember.RunAnalyses(inputserie.Calculators);
            if (bHasTotals)
            {
                inputserie.Calculators = new List<Calculator1>();
                //each inputserie has the correct observation
                SetCalculatorId(inputserie, npv1StockMember);
                inputserie.Calculators.Add(npv1StockMember);
            }
            return bHasTotals;
        }
        //no longer need to run input or output calcs -they already have npv calcs or base prices
        private bool SetInputSeriesNPV1BaseStockTotals(Input inputserie, bool notNeeded)
        {
            bool bHasAnalysis = true;
            bool bHasTotals = false;
            //inputs and outputs don't usually have npv calculations
            //instead, their prices and ocamount were added to the calculators during aggregation 
            //and will be used to run the following price analyses
            if (inputserie.Calculators != null)
            {
                int i = 0;
                //each lcc calculator is considered a unique input observation needing a unique stock total
                //i.e. if this input is an aggregation of five inputs with lcccalcs, that's five observations
                List<Calculator1> stocks = new List<Calculator1>();
                foreach (Calculator1 calc in inputserie.Calculators)
                {
                    //add indicator1stocks to corresponding NPV element
                    NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    NPV1AnalyzerHelper.CopyStockCalculator(calc, npv1StockMember);
                    if (i == 0)
                    {
                        //don't double add npv1stock to calc or or double counting
                        npv1StockMember.InitTotalNPV1StocksProperties();
                    }
                    //npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
                    CopyBaseElementProperties(inputserie, calc);
                    //but keep unique id
                    npv1StockMember.Id = calc.Id;
                    bHasAnalysis = npv1StockMember.RunAnalyses();
                    if (bHasAnalysis)
                    {
                        stocks.Add(npv1StockMember);
                        bHasTotals = true;
                        i++;
                    }
                }
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1.ToString()
                    || NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
                {
                    inputserie.Calculators = new List<Calculator1>();
                    foreach (var stock in stocks)
                    {
                        //don't set calcid for multiple stocks
                        inputserie.Calculators.Add(stock);
                    }
                }
                else
                {
                    //this gives each input and output correct number of lcccalc observations
                    NPV1Stock npv1StockMember2 = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //run the series analyses by aggregating the calcs
                    bHasTotals = npv1StockMember2.RunAnalyses(stocks);
                    if (bHasTotals)
                    {
                        inputserie.Calculators = new List<Calculator1>();
                        //each inputserie has the correct observation
                        SetCalculatorId(inputserie, npv1StockMember2);
                        inputserie.Calculators.Add(npv1StockMember2);
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
                    Input input1 = GetInputSeriesNPVsAggregation(baseInput, qry1);
                    if (input1 != null)
                    {
                        bHasGroups = SetInputSeriesNPV1StockTotals(input1);
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
                    Input input2 = GetInputSeriesNPVsAggregation(baseInput, qry2);
                    if (input2 != null)
                    {
                        bHasGroups = SetInputSeriesNPV1StockTotals(input2);
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
                    Input input3 = GetInputSeriesNPVsAggregation(baseInput, qry3);
                    if (input3 != null)
                    {
                        bHasGroups = SetInputSeriesNPV1StockTotals(input3);
                        if (bHasGroups)
                        {
                            //copy inputseries w analyses to input
                            CopyInputSeriesToInput(baseInput, input3);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = baseInput.Inputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        Input input4 = GetInputSeriesNPVsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = SetInputSeriesNPV1StockTotals(input4);
                            if (bHasGroups)
                            {
                                //copy inputseries w analyses to input
                                CopyInputSeriesToInput(baseInput, input4);
                            }
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = baseInput.Inputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        Input input4 = GetInputSeriesNPVsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = SetInputSeriesNPV1StockTotals(input4);
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
                        Input input4 = GetInputSeriesNPVsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = SetInputSeriesNPV1StockTotals(input4);
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
            //v1.4.5 
            baseInput.Calculators = new List<Calculator1>();
            //copy stock calculators
            foreach (var calc in newInput.Calculators)
            {
                baseInput.Calculators.Add(calc);
            }
        }
        private Input GetInputSeriesNPVsAggregation(Input baseInput,
            IEnumerable<System.Linq.IGrouping<string, Input>> qry)
        {
            //use new, not byref, objects
            Input newInput = new Input(this.GCCalculatorParams, baseInput);
            NPV1AnalyzerHelper.CopyStockCalculator(baseInput.Calculators, newInput.Calculators);
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
                    //the full collection gets aggregated into inputNew
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
                        NPV1AnalyzerHelper.CopyStockCalculator(input.Calculators, inputNew.Calculators);
                        i += 1;
                    }
                    newInput.Inputs.Add(inputNew);
                    a++;
                }
            }
            return newInput;
        }
        private bool SetInputSeriesNPV1StockTotals(Input input)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (input.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPV Elements
                NPV1Stock inputStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (input.Calculators == null)
                    input.Calculators = new List<Calculator1>();
                if (input.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(input.Calculators
                        .FirstOrDefault(), inputStock);
                }
                else
                {
                    //need the options
                    inputStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
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
                        bHasAnalysis = SetInputSeriesNPV1BaseStockTotals(inputserie);
                        NPV1AnalyzerHelper.AddCalculators(inputserie.Calculators, calcs);
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
                    //npvStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(input, inputStock);
                    input.Calculators.Add(inputStock);
                    //unlike other patterns, this doesn't use this.Input because of byref
                }
            }
            return bHasTotals;
        }
        private void AddCalculatorsToInputs(NPV1Stock inputStock, 
            IOrderedEnumerable<Input> inputs)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
        private void AddCalculatorsToOutputs(NPV1Stock outputStock,
            IOrderedEnumerable<Output> outputs)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
        private void AddCalculatorsToOCs(NPV1Stock ocStock,
            IOrderedEnumerable<OperationComponent> ocs)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
        private void AddCalculatorsToOutcomes(NPV1Stock ocStock,
            IOrderedEnumerable<Outcome> ocs)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
        private void AddCalculatorsToTPs(NPV1Stock biStock,
            IOrderedEnumerable<TimePeriod> tps)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
        private void AddCalculatorsToBIs(NPV1Stock biGroupStock,
            IOrderedEnumerable<BudgetInvestment> bis)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                    OutputGroup outputGroup1 = GetOutputNPVsAggregation(qry1);
                    if (outputGroup1 != null)
                    {
                        bHasGroups = SetOutputNPV1StockTotals(outputGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry2 = outputs
                        .GroupBy(m => m.GroupId.ToString());
                    OutputGroup outputGroup2 = GetOutputNPVsAggregation(qry2);
                    if (outputGroup2 != null)
                    {
                        bHasGroups = SetOutputNPV1StockTotals(outputGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry3 = outputs
                        .GroupBy(m => m.Label);
                    OutputGroup outputGroup3 = GetOutputNPVsAggregation(qry3);
                    if (outputGroup3 != null)
                    {
                        bHasGroups = SetOutputNPV1StockTotals(outputGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        OutputGroup outputGroup4 = GetOutputNPVsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = SetOutputNPV1StockTotals(outputGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        OutputGroup outputGroup4 = GetOutputNPVsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = SetOutputNPV1StockTotals(outputGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .GroupBy(m => m.Id.ToString());
                        OutputGroup outputGroup4 = GetOutputNPVsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = SetOutputNPV1StockTotals(outputGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private OutputGroup GetOutputNPVsAggregation(
            IEnumerable<System.Linq.IGrouping<string, Output>> qry)
        {
            //use new, not byref, objects
            if (this.OutputGroup == null)
                this.OutputGroup = new OutputGroup();
            OutputGroup outputGroup = new OutputGroup(this.GCCalculatorParams, this.OutputGroup);
            NPV1AnalyzerHelper.CopyStockCalculator(this.OutputGroup.Calculators, outputGroup.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(output.Calculators, outputNew.Calculators);
                        if (output.Outputs != null)
                        {
                            foreach (var output2 in output.Outputs)
                            {
                                Output output1 = new Output(this.GCCalculatorParams, output2);
                                output1.Calculators = new List<Calculator1>();
                                //this must be i
                                output1.Alternative2 = i;
                                NPV1AnalyzerHelper.CopyStockCalculator(output2.Calculators, output1.Calculators);
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
        private bool SetOutputNPV1StockTotals(OutputGroup outputGroup)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (outputGroup.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock outGroupStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (outputGroup.Calculators == null)
                    outputGroup.Calculators = new List<Calculator1>();
                if (outputGroup.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(outputGroup.Calculators
                        .FirstOrDefault(), outGroupStock);
                }
                else
                {
                    //need the options
                    outGroupStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
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
                        bHasAnalysis = SetOutputNPV1StockTotals(output);
                        NPV1AnalyzerHelper.AddCalculators(output.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = outGroupStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutputs(outGroupStock, Outputs);
                    outputGroup.Calculators = new List<Calculator1>();
                    //npvStockMember.Total, or .Stats holds aggregated numbers
                    SetCalculatorId(outputGroup, outGroupStock);
                    outputGroup.Calculators.Add(outGroupStock);
                    //reset this.OutputGroup
                    this.OutputGroup = outputGroup;
                }
            }
            return bHasTotals;
        }
        private bool SetOutputNPV1StockTotals(Output output)
        {
            bool bHasAnalysis = true;
            //run the output analysis
            if (output != null)
            {
                if ((this.GCCalculatorParams.StartingDocToCalcNodeName
                    == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString()))
                {
                    //run the base calcs, but don't run analyses
                    bHasAnalysis = SetOutputSeriesNPV1BaseStockTotals(output);
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
        private bool SetOutputSeriesNPV1BaseStockTotals(Output outputserie)
        {
            bool bHasTotals = false;
            //this gives each input and output correct number of lcccalc observations
            NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (outputserie.Calculators == null)
                outputserie.Calculators = new List<Calculator1>();
            if (outputserie.Calculators.Count > 0)
            {
                NPV1AnalyzerHelper.CopyandInitStockCalculator(outputserie.Calculators
                    .FirstOrDefault(), npv1StockMember);
            }
            else
            {
                //need the options
                npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
            }
            CopyBaseElementProperties(outputserie, npv1StockMember);
            //run the series analyses by aggregating the calcs
            bHasTotals = npv1StockMember.RunAnalyses(outputserie.Calculators);
            if (bHasTotals)
            {
                outputserie.Calculators = new List<Calculator1>();
                //each inputserie has the correct observation
                SetCalculatorId(outputserie, npv1StockMember);
                outputserie.Calculators.Add(npv1StockMember);
            }
            return bHasTotals;
        }
        //base inputs already have npv totals or prices and don't need new I/O calcs run
        private bool SetOutputSeriesNPV1BaseStockTotals(Output outputserie, bool notNeeds)
        {
            bool bHasAnalysis = true;
            bool bHasTotals = false;
            if (outputserie.Calculators != null)
            {
                int i = 0;
                //each npv calculator is considered a unique output observation needing a unique stock total
                //i.e. if this output is an aggregation of five outputs with lcccalcs, that's five observations
                List<Calculator1> stocks = new List<Calculator1>();
                foreach (Calculator1 calc in outputserie.Calculators)
                {
                    //add indicator1stocks to corresponding NPV element
                    NPV1Stock npv1StockMember = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    NPV1AnalyzerHelper.CopyStockCalculator(calc, npv1StockMember);
                    if (i == 0)
                    {
                        //don't double add npv1stock to calc or or double counting
                        npv1StockMember.InitTotalNPV1StocksProperties();
                    }
                    //npv1StockMember.CopyCalculatorProperties(this.NPV1DescendentStock);
                    CopyBaseElementProperties(outputserie, calc);
                    //debug: v1.5.0 check this removal
                    ////copy the base totals to stockmember; it will aggregate those base totals into AMTotals
                    //NPV1AnalyzerHelper.CopyStockCalculator(calc, npv1StockMember);
                    //but keep unique id
                    npv1StockMember.Id = calc.Id;
                    bHasAnalysis = npv1StockMember.RunAnalyses();
                    if (bHasAnalysis)
                    {
                        stocks.Add(npv1StockMember);
                        bHasTotals = true;
                        i++;
                    }
                }
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1.ToString()
                    || NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
                {
                    outputserie.Calculators = new List<Calculator1>();
                    foreach (var stock in stocks)
                    {
                        //don't set calcid for multiple stocks
                        outputserie.Calculators.Add(stock);
                    }
                }
                else
                {
                    //this gives each output and output correct number of lcccalc observations
                    NPV1Stock npv1StockMember2 = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //run the series analyses by aggregating the calcs
                    bHasTotals = npv1StockMember2.RunAnalyses(stocks);
                    if (bHasTotals)
                    {
                        outputserie.Calculators = new List<Calculator1>();
                        //each outputserie has the correct observation
                        SetCalculatorId(outputserie, npv1StockMember2);
                        outputserie.Calculators.Add(npv1StockMember2);
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
                    Output output1 = GetOutputSeriesNPVsAggregation(baseOutput, qry1);
                    if (output1 != null)
                    {
                        bHasGroups = SetOutputSeriesNPV1StockTotals(output1);
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
                    Output output2 = GetOutputSeriesNPVsAggregation(baseOutput, qry2);
                    if (output2 != null)
                    {
                        bHasGroups = SetOutputSeriesNPV1StockTotals(output2);
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
                    Output output3 = GetOutputSeriesNPVsAggregation(baseOutput, qry3);
                    if (output3 != null)
                    {
                        bHasGroups = SetOutputSeriesNPV1StockTotals(output3);
                        if (bHasGroups)
                        {
                            //copy outputseries w analyses to output
                            CopyOutputSeriesToOutput(baseOutput, output3);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = baseOutput.Outputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        Output output4 = GetOutputSeriesNPVsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = SetOutputSeriesNPV1StockTotals(output4);
                            if (bHasGroups)
                            {
                                //copy outputseries w analyses to output
                                CopyOutputSeriesToOutput(baseOutput, output4);
                            }
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = baseOutput.Outputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        Output output4 = GetOutputSeriesNPVsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = SetOutputSeriesNPV1StockTotals(output4);
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
                        Output output4 = GetOutputSeriesNPVsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = SetOutputSeriesNPV1StockTotals(output4);
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
            //v1.4.5 
            baseOutput.Calculators = new List<Calculator1>();
            //copy stock calculators
            foreach (var calc in newOutput.Calculators)
            {
                baseOutput.Calculators.Add(calc);
            }
        }
        private Output GetOutputSeriesNPVsAggregation(Output baseOutput,
            IEnumerable<System.Linq.IGrouping<string, Output>> qry)
        {
            //use new, not byref, objects
            Output newOutput = new Output(this.GCCalculatorParams, baseOutput);
            NPV1AnalyzerHelper.CopyStockCalculator(baseOutput.Calculators, newOutput.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(output.Calculators, outputNew.Calculators);
                        i += 1;
                    }
                    newOutput.Outputs.Add(outputNew);
                    a++;
                }
            }
            return newOutput;
        }
        private bool SetOutputSeriesNPV1StockTotals(Output output)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (output.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to NPVElements
                NPV1Stock outputStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (output.Calculators == null)
                    output.Calculators = new List<Calculator1>();
                if (output.Calculators.Count > 0)
                {
                    NPV1AnalyzerHelper.CopyandInitStockCalculator(output.Calculators
                        .FirstOrDefault(), outputStock);
                }
                else
                {
                    outputStock.CopyCalculatorProperties(this.NPV1DescendentStock);
                }
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
                        bHasAnalysis = SetOutputSeriesNPV1BaseStockTotals(outputserie);
                        NPV1AnalyzerHelper.AddCalculators(outputserie.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = outputStock.RunAnalyses(calcs);
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutputs(outputStock, OutputSeries);
                    output.Calculators = new List<Calculator1>();
                    //npvStockMember.Total, or .Stats holds aggregated numbers
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
        public static void CopyBaseElementProperties(Calculator1 baseElement, NPV1Stock stock)
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
            if (stock.CalcParameters == null) 
                stock.CalcParameters = new CalculatorParameters();
        }
        private void AdjustAncestorBICalcs(List<Calculator1> calcs)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                //actuals have to be set before base and partials
                int i = 0;
                foreach (var calc in calcs)
                {
                    //don't change newly added benchmark calcs
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                    {
                    }
                    else
                    {
                        if (i == 0)
                        {
                            //first is the base
                            calc.ChangeType
                                = Calculator1.CHANGE_TYPES.current.ToString();
                        }
                        //second and last can't be xminus1, rest can be
                        if (i != calcs.Count - 1
                            && i != 1)
                        {
                            //rest are the xminus1
                            calc.ChangeType
                                = Calculator1.CHANGE_TYPES.xminus1.ToString();
                        }
                    }
                    i++;
                }
            }
        }
        private void AddAncestorBaseBICalcs(List<Calculator1> calcs)
        {
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                //the change analyzers also use a base comparator
                BudgetInvestment ancestorBaseBI = GetAntecedentBaseBI();
                if (ancestorBaseBI != null)
                {
                    if (ancestorBaseBI.Calculators != null)
                    {
                        //must be able to distinguish ancestor from current
                        //not necessary to set existing TargetTypes (they are ignored anyway)
                        foreach (var calc in ancestorBaseBI.Calculators)
                        {
                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                        NPV1AnalyzerHelper.AddCalculators(ancestorBaseBI.Calculators, calcs);
                    }
                }
                else
                {
                    int iBIs = GetBIsCount();
                    if (iBIs > 1)
                    {
                        //first element in bis won't find ancestor but don't want the bis compared
                        //so set them to baseline for comparison to remaining bis
                        foreach (var calc in calcs)
                        {
                            //set current comparators (next bi will be changed to baseline, but Change1 checks for current to run totals)
                            calc.ChangeType
                                = Calculator1.CHANGE_TYPES.current.ToString();
                        }
                    }
                }
            }
        }
        
        private void AddAncestorTPCalcs(List<Calculator1> calcs)
        {
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                int iNewCalcsCount = calcs.Count;
                AddAncestorBaseTPCalcs(calcs);
                //actuals have to be set before base and partials
                foreach (var calc2 in calcs)
                {
                    //don't change newly added benchmark calcs
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                    {
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
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                {
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
                                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                    NPV1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<BudgetInvestment> ancestorBIs = GetAntecedentBIs();
                    if (ancestorBIs != null)
                    {
                        foreach (var bi in ancestorBIs)
                        {
                            if (bi.TimePeriods != null)
                            {
                                //and the xminus1 comparator
                                foreach (var tp in bi.TimePeriods)
                                {
                                    if (tp.Calculators != null)
                                    {
                                        //must be able to distinguish ancestor from current
                                        //not necessary to set existing TargetTypes (they are ignored anyway)
                                        foreach (var calc in tp.Calculators)
                                        {
                                            //flag them so they don't get cumulatively added to 
                                            //parent (need them for PF and PC, but not QTM = PC in AddCumulative
                                            calc.ChangeType
                                                = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                        }
                                        NPV1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
                                    }
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
                != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                NPV1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
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
                    NPV1AnalyzerHelper.CopyStockCalculator(calcs, compCalcs);
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
                        != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                        //else no ChangeByYear at BI level, and ChangeById is already ordered
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                            .BudgetInvestments
                            .LastOrDefault() != null)
                        {
                            //use last benchmark
                            ancestorBI = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                    .BudgetInvestments
                                    .LastOrDefault(bi => bi.TargetType == Calculator1.TARGET_TYPES.benchmark.ToString());
                        }
                    }
                }
            }
            return ancestorBI;
        }
        private List<BudgetInvestment> GetAntecedentBIs()
        {
            List<BudgetInvestment> ancestorBIs = null;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                != null)
            {
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                    != null)
                {
                    //don't change benchmark, but add it's tps to remaining budgets
                    if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                        .BudgetInvestments.Count > 0)
                    {
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                            .BudgetInvestments != null)
                        {
                            //ocs are compared to benchmark budget; so TPs must do same
                            //each budget is compared to benchmark, not to each other
                            ancestorBIs = this.GCCalculatorParams.ParentBudgetInvestmentGroup
                                .BudgetInvestments;
                        }
                    }
                }
            }
            return ancestorBIs;
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
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                    {
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
                                                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                        NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        List<TimePeriod> ancestorTPs = GetAntecedentTPs();
                        if (ancestorTPs != null)
                        {
                            foreach (var tp in ancestorTPs)
                            {
                                if (tp.OperationComponents != null)
                                {
                                    foreach (var oc in tp.OperationComponents)
                                    {
                                        if (oc.Calculators != null)
                                        {
                                            foreach (var calc in oc.Calculators)
                                            {
                                                //flag them so they don't get cumulatively added to 
                                                //parent tp (need them for PF and PC, but not QTM = PC in AddCumulative
                                                calc.ChangeType
                                                    = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                            }
                                            NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                        }
                                    }
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
                != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
                            != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                        }
                    }
                }
            }
            if (!bHasLastBITP)
            {
                ancestorTP = GetAncestorTP();
            }
            return ancestorTP;
        }
        private TimePeriod GetAncestorTP()
        {
            TimePeriod ancestorTP = null;
            //the change analyzers use the ocs in the last tp as the comparator
            if (this.GCCalculatorParams.ParentBudgetInvestment != null)
            {
                if (this.GCCalculatorParams
                    .ParentBudgetInvestment
                    .TimePeriods != null)
                {
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
            return ancestorTP;
        }
        private List<TimePeriod> GetAntecedentTPs()
        {
            bool bHasLastBITP = false;
            List<TimePeriod> ancestorTPs = null;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                != null)
            {
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup
                    .BudgetInvestments != null)
                {
                    if (this.GCCalculatorParams
                        .ParentBudgetInvestmentGroup
                        .BudgetInvestments
                        .FirstOrDefault() != null)
                    {
                        foreach (var bi in this.GCCalculatorParams
                            .ParentBudgetInvestmentGroup
                            .BudgetInvestments)
                        {
                            if (bi.TimePeriods != null)
                            {
                                foreach (var tp in bi.TimePeriods)
                                {
                                    if (ancestorTPs == null)
                                    {
                                        ancestorTPs = new List<TimePeriod>();
                                    }
                                    ancestorTPs.Add(tp);
                                }
                            }
                        }
                    }
                }
            }
            if (!bHasLastBITP)
            {
                //also need sibling tps in progress
                if (this.GCCalculatorParams.ParentBudgetInvestment != null)
                {
                    if (this.GCCalculatorParams.ParentBudgetInvestment
                        .TimePeriods != null)
                    {
                        foreach (var tp in this.GCCalculatorParams.ParentBudgetInvestment
                            .TimePeriods)
                        {
                            if (ancestorTPs == null)
                            {
                                ancestorTPs = new List<TimePeriod>();
                            }
                            ancestorTPs.Add(tp);
                            if (ancestorTPs.Count > 0)
                            {
                                bHasLastBITP = true;
                            }
                        }
                    }
                }
            }
            return ancestorTPs;
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
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                            == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                    //add the base comparator (progress1 does not use)
                    AddAncestorBaseOutcomeCalcs(calcs);
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                    {
                        //this correctly skips the first tp because it's not added yet
                        //and has no ancestor to run comparisons
                        //always comparing the last ancestor ocss to current ocs
                        //progress1 does use
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
                                                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                        NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        List<TimePeriod> ancestorTPs = GetAntecedentTPs();
                        if (ancestorTPs != null)
                        {
                            foreach (var tp in ancestorTPs)
                            {
                                if (tp.Outcomes != null)
                                {
                                    foreach (var oc in tp.Outcomes)
                                    {
                                        if (oc.Calculators != null)
                                        {
                                            foreach (var calc in oc.Calculators)
                                            {
                                                //flag them so they don't get cumulatively added to 
                                                //parent tp (need them for PF and PC, but not QTM = PC in AddCumulative
                                                calc.ChangeType
                                                    = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                            }
                                            //progress has to update base calcs too
                                            //they must be passed byref
                                            NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void AddAncestorBaseOutcomeCalcs(List<Calculator1> calcs)
        {
            //progress1 uses xminus1
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                != NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                                NPV1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
        public bool SaveBINPV1StockTotals(XmlWriter writer)
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
                if (this.NPV1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.BudgetGroup.BudgetInvestments.Count;
                    SetComparativeBaseProperties(this.BudgetGroup, this.BudgetGroup.BudgetInvestments.Count, ref writer);
                }
                //groups have no extension (one totals only)
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.NPV1DescendentStock.Option1);
                this.BudgetGroup.SetNewBIGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //should this be an If (IsSelfOrDescendent) ?
                AddNPV1StockTotals(sAttNameExt, this.BudgetGroup.Calculators, ref writer);
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
                if (this.NPV1DescendentStock.Option1
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
                    if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(bi, bis.Count, ref writer);
                        }
                    } 
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                    bi.SetNewBIAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddNPV1StockTotals(sAttNameExt, bi.Calculators, ref writer);
                        if (bi.TimePeriods != null)
                        {
                            bHasTotals = SetTimePeriodStockTotals(writer, bi.TimePeriods);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //bi
                        writer.WriteEndElement();
                    }
                }
                if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(bi, bis.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                        AddNPV1StockCompTotals(sAttNameExt, bi.Calculators, ref writer);
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
                if (this.NPV1DescendentStock.Option1
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
                    if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(tp, tps.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
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
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddNPV1StockTotals(sAttNameExt, tp.Calculators, ref writer);
                        SetTPDescendentTotals(writer, tp);
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //tp
                        writer.WriteEndElement();
                    }
                }
                if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(tp, tps.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                        AddNPV1StockCompTotals(sAttNameExt, tp.Calculators, ref writer);
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
                        SetComparativeBaseProperties(tp, this.ColumnCount, ref writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.NPV1DescendentStock.Option1);
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
                        SetComparativeBaseProperties(tp, this.ColumnCount, ref writer);
                        AddNPV1StockCompTotals(sAttNameExt, tp.Calculators, ref writer);
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
                                        if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                        = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.NPV1DescendentStock.Option1);
                                    if (isCalculator)
                                    {
                                        AddNPV1StockCompTotals(sAttNameExt, sibTP.Calculators, ref writer);
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
        public bool SaveOCNPV1StockTotals(XmlWriter writer)
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
                if (this.NPV1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OCGroup.OperationComponents.Count;
                    SetComparativeBaseProperties(this.OCGroup, this.OCGroup.OperationComponents.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.NPV1DescendentStock.Option1);
                this.OCGroup.SetNewOCGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //should this be an If (IsSelfOrDescendent) ?
                AddNPV1StockTotals(sAttNameExt, this.OCGroup.Calculators, ref writer);
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
                if (this.NPV1DescendentStock.Option1
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
                    if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(oc, ocs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                    oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddNPV1StockTotals(sAttNameExt, oc.Calculators, ref writer);
                        if (oc.Inputs != null)
                        {
                            bHasTotals = SetInputTechStockTotals(writer, oc.Inputs);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(oc, ocs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                        AddNPV1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
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
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);

                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.NPV1DescendentStock.Option1);
                        oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingTPOCCalculator(writer, oc, bIsCalc, biTps);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);
                        AddNPV1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
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
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.NPV1DescendentStock.Option1);
                        oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingTPOutcomeCalculator(writer, oc, bIsCalc, biTps);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);
                        AddNPV1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
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
                                                if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                                = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.NPV1DescendentStock.Option1);
                                            if (isCalculator)
                                            {
                                                AddNPV1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
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
                                                if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                                = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.NPV1DescendentStock.Option1);
                                            if (isCalculator)
                                            {
                                                AddNPV1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
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
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);

                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.NPV1DescendentStock.Option1);
                        oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingOCCalculator(writer, oc, bIsCalc);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);
                        AddNPV1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
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
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.NPV1DescendentStock.Option1);
                        oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        SetSiblingOutcomeCalculator(writer, oc, bIsCalc);
                        //add the analysis
                        writer.WriteStartElement(Constants.ROOT_PATH);
                        writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                        //need some attributes without attname extension
                        SetComparativeBaseProperties(oc, this.ColumnCount, ref writer);
                        AddNPV1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
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
                                            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                            = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.NPV1DescendentStock.Option1);
                                        if (isCalculator)
                                        {
                                            AddNPV1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
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
                                            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                            = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.NPV1DescendentStock.Option1);
                                        if (isCalculator)
                                        {
                                            AddNPV1StockCompTotals(sAttNameExt, sibOC.Calculators, ref writer);
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
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1.ToString())
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
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                    input.SetNewInputAttributes(sAttNameExt, ref writer);
                    if (bNeedsBaseCalcs)
                    {
                        AddNPV1InputStockTotals(sAttNameExt, input.Calculators, ref writer);
                    }
                    else
                    {
                        AddNPV1StockTotals(sAttNameExt, input.Calculators, ref writer);
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

        public bool SaveOutcomeNPV1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.OutcomeGroup != null)
            {
                this.CurrentNodeName = Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString();
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.NPV1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OutcomeGroup.Outcomes.Count;
                    SetComparativeBaseProperties(this.OutcomeGroup, this.OutcomeGroup.Outcomes.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.NPV1DescendentStock.Option1);
                this.OutcomeGroup.SetNewOutcomeGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                AddNPV1StockTotals(sAttNameExt, this.OutcomeGroup.Calculators, ref writer);
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
                if (this.NPV1DescendentStock.Option1
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
                    if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(oc, ocs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                    oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        AddNPV1StockTotals(sAttNameExt, oc.Calculators, ref writer);
                        if (oc.Outputs != null)
                        {
                            bHasTotals = SetOutputTechStockTotals(writer, oc.Outputs);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(oc, ocs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets
                            || this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            //lines up tps side by side
                            sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.NPV1DescendentStock.Option1);
                        }
                        AddNPV1StockCompTotals(sAttNameExt, oc.Calculators, ref writer);
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
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1.ToString())
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
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                    output.SetNewOutputAttributes(sAttNameExt, ref writer);
                    if (bNeedsBaseCalcs)
                    {
                        AddNPV1OutputStockTotals(sAttNameExt, output.Calculators, ref writer);
                    }
                    else
                    {
                        AddNPV1StockTotals(sAttNameExt, output.Calculators, ref writer);
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
        public bool SaveInputNPV1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.InputGroup != null)
            {
                this.CurrentNodeName = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.NPV1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.InputGroup.Inputs.Count;
                    SetComparativeBaseProperties(this.InputGroup, this.InputGroup.Inputs.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.NPV1DescendentStock.Option1);
                this.InputGroup.SetNewInputGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //1.4.1 removed this -detracts from analyzing inputs and input series
                //AddNPV1StockTotals(sAttNameExt, this.InputGroup.Calculators, ref writer);
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
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                //comps go horizontal
                if (this.NPV1DescendentStock.Option1
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
                    if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(input, inputs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                    input.SetNewInputAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (bNeedsBaseCalcs)
                        {
                            AddNPV1InputStockTotals(sAttNameExt, input.Calculators, ref writer);
                        }
                        else
                        {
                            AddNPV1StockTotals(sAttNameExt, input.Calculators, ref writer);
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
                                    AddNPV1InputStockTotals(sAttNameExt, input.Calculators, ref writer);
                                }
                                else
                                {
                                    AddNPV1StockTotals(sAttNameExt, input.Calculators, ref writer);
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
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(input, inputs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                        AddNPV1StockCompTotals(sAttNameExt, input.Calculators, ref writer);
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
        public bool SaveOutputNPV1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.OutputGroup != null)
            {
                this.CurrentNodeName = Output.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                writer.WriteStartElement(this.CurrentNodeName);
                if (this.NPV1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OutputGroup.Outputs.Count;
                    SetComparativeBaseProperties(this.OutputGroup, this.OutputGroup.Outputs.Count, ref writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.NPV1DescendentStock.Option1);
                this.OutputGroup.SetNewOutputGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //1.4.1 removed this -detracts from analyzing inputs and input series
                //AddNPV1StockTotals(sAttNameExt, this.OutputGroup.Calculators, ref writer);
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
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                //comps go horizontal
                if (this.NPV1DescendentStock.Option1
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
                    if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(output, outputs.Count, ref writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                    output.SetNewOutputAttributes(sAttNameExt, ref writer);
                    //don't display outputs (each comp can has multiple outputs -they would need one output)
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (bNeedsBaseCalcs)
                        {
                            AddNPV1OutputStockTotals(sAttNameExt, output.Calculators, ref writer);
                        }
                        else
                        {
                            AddNPV1StockTotals(sAttNameExt, output.Calculators, ref writer);
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
                                    AddNPV1OutputStockTotals(sAttNameExt, output.Calculators, ref writer);
                                }
                                else
                                {
                                    AddNPV1StockTotals(sAttNameExt, output.Calculators, ref writer);
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
                    if (this.NPV1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        writer.WriteEndElement();
                    }
                }
                if (this.NPV1DescendentStock.Option1
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
                            SetComparativeBaseProperties(output, outputs.Count, ref writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.NPV1DescendentStock.Option1);
                        AddNPV1StockCompTotals(sAttNameExt, output.Calculators, ref writer);
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
       
        private void AddNPV1StockTotals(string attNameExt, List<Calculator1> stocks, ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //each stock can contain as many ptstocks as totaled
                //so only one NPV1Stock should be used (or inserting/updating els becomes difficult)
                NPV1Stock stock = new NPV1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (NPV1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            writer.WriteStartElement(Constants.ROOT_PATH);
                            writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseProperties(calc, this.ColumnCount, ref writer);
                            }
                            //the stock totals are added to currentelement using attnameext to distinguish observations
                            stock.SetDescendantNPV1StockAttributes(attNameExt, ref writer);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }
        private void AddNPV1StockCompTotals(string attNameExt, List<Calculator1> stocks,
            ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //each stock can contain as many ptstocks as totaled
                //so only one NPV1Stock should be used (or inserting/updating els becomes difficult)
                NPV1Stock stock = new NPV1Stock();
                if (stocks != null)
                {
                    int i = 0;
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (NPV1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            //note: this will fail with more than one calculator so make sure not to use more than one
                            //stock calculator per node
                            //the stock totals are added to currentelement using attnameext to distinguish observations
                            stock.SetDescendantNPV1StockAttributes(attNameExt, ref writer);
                        }
                    }
                    i++;
                }
            }
        }
        private void AddNPV1InputStockTotals(string attNameExt, List<Calculator1> stocks,
            ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //some stocks use calculator, not stock, results
                //the calcs are stored with the stock
                NPV1Stock stock = new NPV1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (NPV1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            writer.WriteStartElement(Constants.ROOT_PATH);
                            writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseProperties(calc, this.ColumnCount, ref writer);
                            }
                            stock.SetDescendantNPV1StockInputAttributes(attNameExt, this.GCCalculatorParams,
                                ref writer);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }
        private void AddNPV1OutputStockTotals(string attNameExt, List<Calculator1> stocks,
            ref XmlWriter writer)
        {
            if (stocks != null)
            {
                //some stocks use calculator, not stock, results
                NPV1Stock stock = new NPV1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (NPV1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            writer.WriteStartElement(Constants.ROOT_PATH);
                            writer.WriteStartElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseProperties(calc, this.ColumnCount, ref writer);
                            }
                            stock.SetDescendantNPV1StockOutputAttributes(attNameExt, this.GCCalculatorParams,
                                ref writer);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }
        private void SetComparativeBaseProperties(Calculator1 baseObject, int colCount, 
            ref XmlWriter writer)
        {
            //need some attributes without attname extension
            writer.WriteAttributeString(Calculator1.cId, baseObject.Id.ToString());
            writer.WriteAttributeString(Calculator1.cName, baseObject.Name.ToString());
            //column count convention
            writer.WriteAttributeString("Files", colCount.ToString());
            //analyzer type
            writer.WriteAttributeString(Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
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
    }
}
