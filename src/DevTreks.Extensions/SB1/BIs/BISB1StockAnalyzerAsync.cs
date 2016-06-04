using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		1. Group the MN elements by an AggregationId (TypeId, GroupId, Label)
    ///             2. RunAnalyses
    ///             3. Save the analyses as xml
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1.  
    /// </summary>
    public class BISB1StockAnalyzerAsync : BudgetInvestmentCalculatorAsync
    {
        public BISB1StockAnalyzerAsync(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set Indic1Stock
            Init();
        }
        public BISB1StockAnalyzerAsync()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //set SB1DescendentStock state so that descendant stock totals will have good base properties
            //the base.Analyzer is set when the Save... methods are run
            this.SB1DescendentStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (this.GCCalculatorParams.LinkedViewElement != null)
            {
                this.SB1DescendentStock.SetDescendantSB1StockProperties(
                    this.GCCalculatorParams.LinkedViewElement);
            }
        }
        //stateful Stock costs
        public SBC1Calculator SB1C1 { get; set; }
        //stateful Stock benefits
        public SBB1Calculator SB1B1 { get; set; }
        //stateful analyzer used to set base.Analyzer properties in descendants (name, id)
        //and to hold descendent input and output stocks for analysis
        public SB1Stock SB1DescendentStock { get; set; }
        //stateful currentnodename
        private string CurrentNodeName { get; set; }

        //these objects hold collections of SB1s for running totals.
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
        private double RowCount { get; set; }

        public async Task<bool> SaveSB1StockTotalsAsync()
        {
            bool bHasCalculations = false;
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI, 
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath
                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            if (!CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI, 
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                return bHasCalculations;
            StringWriter output = new StringWriter();
            XmlWriterSettings oXmlWriterSettings = new XmlWriterSettings();
            oXmlWriterSettings.Indent = true;
            oXmlWriterSettings.OmitXmlDeclaration = true;
            oXmlWriterSettings.Async = true;
            //store the results in tempdocpath
            using (XmlWriter writer
                = XmlWriter.Create(output,
                oXmlWriterSettings))
            {
                await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                //one aggregating element for every unique node
                //i.e. five ops with same label = one op element with 5 observations in atts
                if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets
                    || this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    bHasCalculations = await SaveBISB1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.operationprices
                    || this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    bHasCalculations = await SaveOCSB1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    bHasCalculations = await SaveOutcomeSB1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    bHasCalculations = await SaveInputSB1StockTotals(writer);
                }
                else if (this.GCCalculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    bHasCalculations = await SaveOutputSB1StockTotals(writer);
                }
                await writer.WriteEndElementAsync();
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


        private void SetCalculatorId(Calculator1 baseElement, SB1Stock stock)
        {
            //change and progress analyzers change the calcid using Change1.Stocks
            if (!SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                SetCalcId(baseElement, stock);
            }
        }
        private void SetCalcId(Calculator1 baseElement, SB1Stock stock)
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
        public async Task<bool> SetBISB1StockTotals()
        {
            bool bHasCalculations = false;
            if (this.BudgetGroup.BudgetInvestments != null)
            {
                //set the correct aggregation for the analysis
                //this aggregates bis, tps, opcomps, and outcomes
                bHasCalculations = await SetBIAggregation(this.BudgetGroup.BudgetInvestments);
            }
            return bHasCalculations;
        }

        //this is called one time after the stateful collections are built
        public async Task<bool> SetOCSB1StockTotals()
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
                                    bHasCalculations = await SetOCAggregation(tp.OperationComponents);
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
                        bHasCalculations = await SetOCAggregation(this.OCGroup.OperationComponents);
                    }
                }
            }
            return bHasCalculations;
        }
        //this is called one time after the stateful collections are built
        public async Task<bool> SetOutcomeSB1StockTotals()
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
                                    bHasCalculations = await SetOutcomeAggregation(tp.Outcomes);
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
                        bHasCalculations = await SetOutcomeAggregation(this.OutcomeGroup.Outcomes);
                    }
                }
            }
            return bHasCalculations;
        }

        //this is called one time after the stateful collections are built
        public async Task<bool> SetInputSB1StockTotals()
        {
            bool bHasCalculations = false;
            if (this.InputGroup != null)
            {
                if (this.InputGroup.Inputs != null)
                {
                    //set the correct aggregation for the analysis
                    bHasCalculations = await SetInputAggregation(this.InputGroup.Inputs);
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
                SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
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
                        SB1AnalyzerHelper.AddOutputCalculators(outputseries.Calculators, outputseries2.Calculators);
                    }
                    o.Outputs.Add(outputseries2);
                }
            }
            outGroup.Outputs.Add(o);
        }

        //this is called one time after the stateful collections are built
        public async Task<bool> SetOutputSB1StockTotals()
        {
            bool bHasCalculations = false;
            if (this.OutputGroup != null)
            {
                if (this.OutputGroup.Outputs != null)
                {
                    //set the correct aggregation for the analysis
                    bHasCalculations = await SetOutputAggregation(this.OutputGroup.Outputs);
                }
            }
            return bHasCalculations;
        }

        //budgets
        public async Task<bool> SetBIAggregation(List<BudgetInvestment> bis)
        {
            bool bHasGroups = false;
            //change analyses agg by yr, alt, or id
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                     qry4 = bis
                    .OrderBy(m => m.Date.Year.ToString())
                    .GroupBy(m => m.Date.Year.ToString());
                await SetBISBsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated return true
                    bHasGroups = true;
                    await SetBIGroupSB1StockTotals();
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                     qry4 = bis
                    .OrderBy(m => m.AlternativeType)
                    .GroupBy(m => m.AlternativeType);
                await SetBISBsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    await SetBIGroupSB1StockTotals();
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                    qry4 = bis
                    .GroupBy(m => m.Id.ToString());
                await SetBISBsAggregation(qry4);
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                {
                    //as long as the tps are aggregated r4eturn true
                    bHasGroups = true;
                    await SetBIGroupSB1StockTotals();
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
                        await SetBISBsAggregation(qry1);
                        //BudgetInvestmentGroup biGroup1 = GetBISBsAggregation(qry1);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            await SetBIGroupSB1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry2 = bis
                            .GroupBy(m => m.GroupId.ToString());
                        await SetBISBsAggregation(qry2);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            await SetBIGroupSB1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry3 = bis
                            .GroupBy(m => m.Label);
                        await SetBISBsAggregation(qry3);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            await SetBIGroupSB1StockTotals();
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                        IEnumerable<System.Linq.IGrouping<string, BudgetInvestment>>
                            qry4 = bis
                            .GroupBy(m => m.Id.ToString());
                        await SetBISBsAggregation(qry4);
                        if (this.GCCalculatorParams.ParentBudgetInvestmentGroup != null)
                        {
                            //as long as the tps are aggregated r4eturn true
                            bHasGroups = true;
                            await SetBIGroupSB1StockTotals();
                        }
                        break;
                    default:
                        break;
                }
            }
            return bHasGroups;
        }
        private async Task SetBISBsAggregation(
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
                        bool bHasTPTotals = await SetTPAggregation(bi.TimePeriods);
                        i += 1;
                    }
                    //now run calcs for this.GCCalculatorParams.ParentBudgetInvestment.Calculators
                    //add the budget to the stateful bg, but don't make it by ref
                    bool bHasTotals = await SetBISB1StockTotal();
                    if (bHasTotals)
                    {
                        //avoid a byref by building a new bi
                        BudgetInvestment budInvest
                            = new BudgetInvestment(this.GCCalculatorParams, this.GCCalculatorParams.ParentBudgetInvestment);
                        budInvest.TimePeriods = new List<TimePeriod>();
                        budInvest.Calculators = new List<Calculator1>();
                        SB1AnalyzerHelper.AddCalculators(this.GCCalculatorParams.ParentBudgetInvestment.Calculators,
                            budInvest.Calculators);
                        foreach (var tp in this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods)
                        {
                            TimePeriod newTp = new TimePeriod(this.GCCalculatorParams, tp);
                            newTp.Calculators = new List<Calculator1>();
                            SetNewTPFromOldTp(newTp, tp);
                            //watch byref
                            SB1AnalyzerHelper.AddCalculators(tp.Calculators, newTp.Calculators);
                            budInvest.TimePeriods.Add(newTp);
                        }
                        this.GCCalculatorParams.ParentBudgetInvestmentGroup
                            .BudgetInvestments.Add(budInvest);
                        //keep them ordered by the type of analysis
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
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
        private async Task<bool> SetBIGroupSB1StockTotals()
        {
            bool bHasTotals = false;
            if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock biGroupStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                biGroupStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(this.GCCalculatorParams.ParentBudgetInvestmentGroup, biGroupStock);
                //each bi holds its own totals for indicators
                //194: don't reorder anything
                this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments
                    = this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments.ToList();
                //order them by ascending date (totals accrue from earliest to latest)
                //this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments 
                //    = this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments.OrderBy(bi => bi.Id).OrderBy(bi => bi.Date).ToList();
                //run the bi analysis
                if (this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var bi in this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments)
                    {
                        SB1AnalyzerHelper.AddCalculators(bi.Calculators, calcs);
                    }
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biGroupStock.RunAnalyses(calcs);
                    }
                    else
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biGroupStock.RunAnalyses(calcs);
                    }
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(biGroupStock.MathResult)
                        && biGroupStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = biGroupStock.MathResult;
                        }
                    }
                }
                if (bHasTotals && biGroupStock != null)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToBIs(biGroupStock, this.GCCalculatorParams.ParentBudgetInvestmentGroup.BudgetInvestments);
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

        private async Task<bool> SetBISB1StockTotal()
        {
            bool bHasTotals = false;
            if (this.GCCalculatorParams.ParentBudgetInvestment != null)
            {
                SB1Stock biStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                biStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(this.GCCalculatorParams.ParentBudgetInvestment, biStock);
                //only need to aggregate if more than one calculator is on hand (tps already aggregated using tempBI)
                if (this.GCCalculatorParams.ParentBudgetInvestment.Calculators.Count > 1)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    //parentbi holds one or more calculators holding results of agg tps
                    SB1AnalyzerHelper.AddCalculators(this.GCCalculatorParams.ParentBudgetInvestment.Calculators, calcs);
                    SB1AnalyzerHelper.InitCalculatorMath(biStock);
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biStock.RunAnalyses(calcs);
                    }
                    else
                    {
                        //run the analyses by aggregating the input.calcs
                        bHasTotals = biStock.RunAnalyses(calcs);
                    }
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(biStock.MathResult)
                        && biStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = biStock.MathResult;
                        }
                    }
                    else
                    {
                        //188 still need the input.calc.DataToAnalyze copied to sb1StockMember
                        SB1AnalyzerHelper.AddCalculatorDataToStock(calcs, biStock);
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
        public async Task<bool> SetTPAggregation(List<TimePeriod> tps)
        {
            bool bHasGroups = false;
            BudgetInvestment tempBI = new BudgetInvestment();
            //change analyses agg by yr, alt, or id
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .OrderBy(m => m.Date.Year.ToString())
                    .GroupBy(m => m.Date.Year.ToString());
                tempBI = await SetTPSBsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = await SetTPSB1StockTotals(tempBI);
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .OrderBy(m => m.AlternativeType)
                    .GroupBy(m => m.AlternativeType);
                tempBI = await SetTPSBsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = await SetTPSB1StockTotals(tempBI);
                }
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString())
            {
                IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                    qry4 = tps
                    .GroupBy(m => m.Id.ToString());
                tempBI = await SetTPSBsAggregation(qry4);
                if (tempBI != null)
                {
                    bHasGroups = await SetTPSB1StockTotals(tempBI);
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
                        tempBI = await SetTPSBsAggregation(qry1);
                        if (tempBI != null)
                        {
                            bHasGroups = await SetTPSB1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry2 = tps
                            .GroupBy(m => m.GroupId.ToString());
                        tempBI = await SetTPSBsAggregation(qry2);
                        if (tempBI != null)
                        {
                            bHasGroups = await SetTPSB1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry3 = tps
                            .GroupBy(m => m.Label);
                        tempBI = await SetTPSBsAggregation(qry3);
                        if (tempBI != null)
                        {
                            bHasGroups = await SetTPSB1StockTotals(tempBI);
                        }
                        break;
                    case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                        IEnumerable<System.Linq.IGrouping<string, TimePeriod>>
                            qry4 = tps
                            .GroupBy(m => m.Id.ToString());
                        tempBI = await SetTPSBsAggregation(qry4);
                        if (tempBI != null)
                        {
                            bHasGroups = await SetTPSB1StockTotals(tempBI);
                        }
                        break;
                    default:
                        break;
                }
            }
            return bHasGroups;
        }

        private async Task<BudgetInvestment> SetTPSBsAggregation(IEnumerable<System.Linq.IGrouping<string, TimePeriod>> qry)
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
                    bool bHasTotals = await GetTPAggregation(newTP, timeperiodTotals);
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

        private async Task<bool> GetTPAggregation(TimePeriod oldTP, TimePeriod newTP)
        {
            //this will group the ocs, run the analyses, and addthem to this.OCGroup
            bool bHasCalculations = await SetOCAggregation(oldTP.OperationComponents);
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
                                //this is what will change correct tp indicators
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
            bool bHasCalculations2 = await SetOutcomeAggregation(oldTP.Outcomes);
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
            //opComp.multiplier comes from correct preaggregated tp (constructor means oc and opComp are identical)
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
                        SB1AnalyzerHelper.AddInputCalculators(input.Calculators, i.Calculators);
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
            //oc.multiplier comes from correct preaggregated tp, opComp.Multiplier is regular oc.multiplier
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
                        SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
                        //add any stock analyzers
                        AddStockOutputCalculators(output.Calculators, o.Calculators);
                    }
                    oc.Outputs.Add(o);
                }
            }
            ocs.Add(oc);
        }

        private async Task<bool> SetTPSB1StockTotals(BudgetInvestment tempBI)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //tempBI has to be used to run totals, because ParentBI.TPs could have TPs from previous tp aggregations
            if (tempBI.TimePeriods != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock biStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (tempBI.Calculators == null)
                    tempBI.Calculators = new List<Calculator1>();
                //need the options
                biStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(tempBI, biStock);
                //each oc holds its own totals for indicators
                //194 removed orderbyId
                //order them by ascending date (totals accrue from earliest to latest)
                tempBI.TimePeriods = tempBI.TimePeriods.OrderBy(t => t.Date).ToList();
                //run the tp analysis
                if (tempBI.TimePeriods != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var tp in tempBI.TimePeriods)
                    {
                        //tp already has the aggregated descendent calcs, 
                        //but still needs the tpstock.props and multiplier-adjusted totals set
                        bHasAnalysis = await SetTPSB1StockTotals(tp);
                        SB1AnalyzerHelper.AddCalculators(tp.Alternative2, tp.Calculators, calcs);
                    }
                    SB1AnalyzerHelper.InitCalculatorMath(biStock);
                    if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        biStock.CalcParameters.CurrentElementNodeName 
                            = BudgetInvestment.BUDGET_TYPES.budget.ToString();
                    }
                    else
                    {
                        biStock.CalcParameters.CurrentElementNodeName 
                            = BudgetInvestment.INVESTMENT_TYPES.investment.ToString();
                    }
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                    {
                        //progress has to do tp by tp because calcs haven't been all run yet
                        AddAncestorTPCalcs(calcs);
                        //run the analyses by aggregating the calcs
                        bHasTotals = biStock.RunAnalyses(calcs);
                    }
                    else
                    {
                        //add ancestors needed for change analysis
                        AddAncestorTPCalcs(calcs);
                        //run the analyses by aggregating the calcs
                        bHasTotals = biStock.RunAnalyses(calcs);
                    }
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(biStock.MathResult)
                        && biStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = biStock.MathResult;
                        }
                    }
                    else
                    {
                        //188 still need the input.calc.DataToAnalyze copied to sb1StockMember
                        SB1AnalyzerHelper.AddCalculatorDataToStock(calcs, biStock);
                    }
                }
                if (bHasTotals && biStock != null)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToTPs(biStock, tempBI.TimePeriods);
                    //this adds the tps to parentbi
                    AddTPCalcsToParentBI(tempBI.TimePeriods);
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
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.AlternativeType).ToList();
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.Date.Year).ToList();
            }
            else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString())
            {
                bi.TimePeriods
                    = bi.TimePeriods.OrderBy(b => b.Id).ToList();
            }
            else
            {
                //keep the existing order
            }
        }
        private async Task<bool> SetTPSB1StockTotals(TimePeriod tp)
        {
            //tp already has good calcs, but needs tp.calc to change to tp.props and multiplied 
            bool bHasTotals = false;
            //add the oc stock totals using the OCs.Inputs collection that now hold inputstock calcs
            //add indicator1stocks to corresponding MandE element
            SB1Stock tpStockMember = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //need the options
            tpStockMember.CopyCalculatorProperties(this.SB1DescendentStock);
            CopyBaseElementProperties(tp, tpStockMember);
            //multiplier was added to children ocs and their totals multiplied 
            tpStockMember.Multiplier = 1;
            //tps subtract stock02 outputs from stock01 inputs and use stock01 for subsequent analysis and display
            //more elaborate stock balances can follow
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                tpStockMember.CalcParameters.CurrentElementNodeName 
                    = BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString();
            }
            else
            {
                tpStockMember.CalcParameters.CurrentElementNodeName 
                    = BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString();
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
        public bool AddTPCalcsToParentBI(List<TimePeriod> tps)
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
        public async Task<bool> SetOCAggregation(List<OperationComponent> ocs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry1 = ocs
                        .GroupBy(m => m.TypeId.ToString());
                    OperationComponentGroup ocGroup1 = GetOCSBsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = await SetOCSB1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry2 = ocs
                        .GroupBy(m => m.GroupId.ToString());
                    OperationComponentGroup ocGroup2 = GetOCSBsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = await SetOCSB1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                        qry3 = ocs
                        .GroupBy(m => m.Label);
                    OperationComponentGroup ocGroup3 = GetOCSBsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = await SetOCSB1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                        && (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                        || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices))
                    {
                        //id field will result in original structure
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        OperationComponentGroup ocGroup4 = GetOCSBsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = await SetOCSB1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                        && (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                        || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices))
                    {
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        OperationComponentGroup ocGroup4 = GetOCSBsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = await SetOCSB1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        //id field will result in original structure
                        IEnumerable<System.Linq.IGrouping<string, OperationComponent>>
                            qry4 = ocs
                            .GroupBy(m => m.Id.ToString());
                        OperationComponentGroup ocGroup4 = GetOCSBsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = await SetOCSB1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }

        private OperationComponentGroup GetOCSBsAggregation(
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
                                SB1AnalyzerHelper.AddInputCalculators(input.Calculators, input1.Calculators);
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
        private async Task<bool> SetOCSB1StockTotals(OperationComponentGroup ocGroup)
        {
            bool bHasTotals = false;
            bool bHasAnalysis = false;
            if (ocGroup.OperationComponents != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock ocGroupStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                ocGroupStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(ocGroup, ocGroupStock);
                //each oc holds its own totals for indicators
                //order them by ascending date (totals accrue from earliest to latest)
                //194 removed Id order
                ocGroup.OperationComponents = ocGroup.OperationComponents.OrderBy(oc => oc.Date).ToList();
                //ocGroup.OperationComponents = ocGroup.OperationComponents.OrderBy(oc => oc.Id).OrderBy(oc => oc.Date).ToList();
                //run the oc analysis
                if (ocGroup.OperationComponents != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var oc in ocGroup.OperationComponents)
                    {
                        bHasAnalysis = await SetOCSB1StockTotals(oc);
                        SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                    }
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                    {
                        //progress has to do tp by tp because calcs haven't been all run yet
                        AddAncestorOCCalcs(calcs);
                    }
                    else
                    {
                        //add ancestors needed for change analysis
                        AddAncestorOCCalcs(calcs);
                    }
                    //run the analysis
                    bHasTotals = ocGroupStock.RunAnalyses(calcs);
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(ocGroupStock.MathResult)
                        && ocGroupStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = ocGroupStock.MathResult;
                        }
                    }
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOCs(ocGroupStock, ocGroup.OperationComponents);
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
        private async Task<bool> SetOCSB1StockTotals(OperationComponent oc)
        {
            bool bHasTotals = false;
            //add the input stock totals using the oc
            //assume that good inputs means good totals
            bHasTotals = await SetInputSB1StockTotals(oc);
            //add the oc stock totals using the OCs.Inputs collection that now hold inputstock calcs
            //add indicator1stocks to corresponding MandE element
            SB1Stock sb1StockMember = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //need the options
            sb1StockMember.CopyCalculatorProperties(this.SB1DescendentStock);
            CopyBaseElementProperties(oc, sb1StockMember);
            //existing oc.Multiplier comes from correct parent tp or ocgroup before tps were aggregated
            //oc.Multiplier not used because it was correctly added to children inputs and they have mults already
            sb1StockMember.Multiplier = 1;
            if (oc.Inputs != null)
            {
                List<Calculator1> calcs = new List<Calculator1>();
                foreach (var input in oc.Inputs)
                {
                    SB1AnalyzerHelper.AddCalculators(input.Calculators, calcs);
                }
                //some analyzers only sum totals for the calcs (i.e. no changes in ins needed)
                sb1StockMember.CalcParameters.CurrentElementNodeName
                    = Input.INPUT_PRICE_TYPES.input.ToString();
                SB1AnalyzerHelper.InitCalculatorMath(sb1StockMember);
                //run the analyses by aggregating the input.calcs
                bool bHasAnalysis = sb1StockMember.RunAnalyses(calcs);
                //188 pass mathresult back to ui
                if (!string.IsNullOrEmpty(sb1StockMember.MathResult)
                    && sb1StockMember.MathResult != Constants.NONE)
                {
                    //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                    if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                    {
                        this.GCCalculatorParams.MathResult = sb1StockMember.MathResult;
                    }
                }
                else
                {
                    //188 still need the input.calc.DataToAnalyze copied to sb1StockMember
                    SB1AnalyzerHelper.AddCalculatorDataToStock(calcs, sb1StockMember);
                }
            }
            //don't double use the multiplier
            sb1StockMember.Multiplier = 1;
            oc.Calculators = new List<Calculator1>();
            //lcaStockMember.Total, or .Stats holds aggregated numbers
            SetCalculatorId(oc, sb1StockMember);
            oc.Calculators.Add(sb1StockMember);
            return bHasTotals;
        }
        private async Task<bool> SetInputSB1StockTotals(OperationComponent oc)
        {
            bool bHasTotals = false;
            //run the input analysis using this.SB1DescendentStock (so that ancestors can use them)
            if (oc.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock sb1Stock = new SB1Stock(this.GCCalculatorParams,
                    this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                sb1Stock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(oc, sb1Stock);
                foreach (var input in oc.Inputs)
                {
                    //add indicator1stocks to corresponding MandE element
                    SB1Stock sb1StockMember = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    sb1StockMember.CopyCalculatorProperties(this.SB1DescendentStock);
                    CopyBaseElementProperties(input, sb1StockMember);
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        //but still needs input.SB1CInput.ocamount, aohamount, and capamount
                        sb1StockMember.Multiplier = input.Multiplier;
                        sb1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                    }
                    else
                    {
                        //existing input.Multiplier comes from correct parent oc before ocs were aggregated
                        sb1StockMember.Multiplier = input.Multiplier;
                        sb1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                    }
                    if (input.Calculators != null)
                    {
                        sb1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                        //add a cost calculator to subp1s collection
                        //some analyzers use the subp1s (total1), so they must be copied correctly into new stock objects
                        sb1StockMember.SB11Stock.AddInputCalcsToStock(input.Calculators);
                    }
                    //add the stockmember to the base stock
                    bHasTotals = await sb1StockMember.RunAnalyses();
                    //don't double use the multiplier
                    sb1StockMember.Multiplier = 1;
                    SetCalculatorId(input, sb1StockMember);
                    input.Calculators.Add(sb1StockMember);
                }
            }
            return bHasTotals;
        }

        //outcomes
        public async Task<bool> SetOutcomeAggregation(List<Outcome> ocs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry1 = ocs
                        .GroupBy(m => m.TypeId.ToString());
                    OutcomeGroup ocGroup1 = GetOutcomeSBsAggregation(qry1);
                    if (ocGroup1 != null)
                    {
                        bHasGroups = await SetOutcomeSB1StockTotals(ocGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry2 = ocs
                        .GroupBy(m => m.GroupId.ToString());
                    OutcomeGroup ocGroup2 = GetOutcomeSBsAggregation(qry2);
                    if (ocGroup2 != null)
                    {
                        bHasGroups = await SetOutcomeSB1StockTotals(ocGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Outcome>>
                        qry3 = ocs
                        .GroupBy(m => m.Label);
                    OutcomeGroup ocGroup3 = GetOutcomeSBsAggregation(qry3);
                    if (ocGroup3 != null)
                    {
                        bHasGroups = await SetOutcomeSB1StockTotals(ocGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .OrderBy(m => m.Date.Year.ToString())
                                .GroupBy(m => m.Date.Year.ToString());
                        OutcomeGroup ocGroup4 = GetOutcomeSBsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = await SetOutcomeSB1StockTotals(ocGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .OrderBy(m => m.AlternativeType)
                                .GroupBy(m => m.AlternativeType);
                        OutcomeGroup ocGroup4 = GetOutcomeSBsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = await SetOutcomeSB1StockTotals(ocGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Outcome>>
                                qry4 = ocs
                                .GroupBy(m => m.Id.ToString());
                        OutcomeGroup ocGroup4 = GetOutcomeSBsAggregation(qry4);
                        if (ocGroup4 != null)
                        {
                            bHasGroups = await SetOutcomeSB1StockTotals(ocGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }

        private OutcomeGroup GetOutcomeSBsAggregation(
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
                                    SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, output1.Calculators);
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
        private async Task<bool> SetOutcomeSB1StockTotals(OutcomeGroup ocGroup)
        {
            bool bHasTotals = false;
            bool bHasAnalysis = false;
            if (ocGroup.Outcomes != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock ocGroupStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                ocGroupStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(ocGroup, ocGroupStock);
                //each oc holds its own totals for indicators
                //194 removed orderyby id
                //order them by ascending date (totals accrue from earliest to latest)
                ocGroup.Outcomes = ocGroup.Outcomes.OrderBy(oc => oc.Date).ToList();
                //run the oc analysis
                if (ocGroup.Outcomes != null)
                {
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var oc in ocGroup.Outcomes)
                    {
                        bHasAnalysis = await SetOutcomeSB1StockTotals(oc);
                        SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                    }
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                    {
                        //progress has to do tp by tp because calcs haven't been all run yet
                        AddAncestorOutcomeCalcs(calcs);
                    }
                    else
                    {
                        //add ancestors needed for change analysis
                        AddAncestorOutcomeCalcs(calcs);
                    }
                    
                    //run the analysis
                    bHasTotals = ocGroupStock.RunAnalyses(calcs);
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(ocGroupStock.MathResult)
                        && ocGroupStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = ocGroupStock.MathResult;
                        }
                    }
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutcomes(ocGroupStock, ocGroup.Outcomes);
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
        private async Task<bool> SetOutcomeSB1StockTotals(Outcome oc)
        {
            bool bHasTotals = false;
            //add the output stock totals using the oc
            //assume that good outputs means good totals
            bHasTotals = await SetOutputSB1StockTotals(oc);
            //add the oc stock totals using the OCs.Outputs collection that now hold outputstock calcs
            //add indicator1stocks to corresponding MandE element
            SB1Stock sb1StockMember = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //need the options
            sb1StockMember.CopyCalculatorProperties(this.SB1DescendentStock);
            CopyBaseElementProperties(oc, sb1StockMember);
            //children outputs were already multiplied by correct oc.Multiplier 
            sb1StockMember.Multiplier = 1;
            if (oc.Outputs != null)
            {
                List<Calculator1> calcs = new List<Calculator1>();
                foreach (var output in oc.Outputs)
                {
                    SB1AnalyzerHelper.AddCalculators(output.Calculators, calcs);
                }
                //some analyzers only sum totals for the calcs (i.e. no changes in ins needed)
                sb1StockMember.CalcParameters.CurrentElementNodeName
                    = Output.OUTPUT_PRICE_TYPES.output.ToString();
                SB1AnalyzerHelper.InitCalculatorMath(sb1StockMember);
                //run the analyses by aggregating the input.calcs
                bool bHasAnalysis = sb1StockMember.RunAnalyses(calcs);
                //188 pass mathresult back to ui
                if (!string.IsNullOrEmpty(sb1StockMember.MathResult)
                    && sb1StockMember.MathResult != Constants.NONE)
                {
                    //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                    if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                    {
                        this.GCCalculatorParams.MathResult = sb1StockMember.MathResult;
                    }
                }
                else
                {
                    //188 still need the input.calc.DataToAnalyze copied to sb1StockMember
                    SB1AnalyzerHelper.AddCalculatorDataToStock(calcs, sb1StockMember);
                }
            }
            //don't double use the multiplier
            sb1StockMember.Multiplier = 1;
            oc.Calculators = new List<Calculator1>();
            //lcaStockMember.Total, or .Stats holds aggregated numbers
            SetCalculatorId(oc, sb1StockMember);
            oc.Calculators.Add(sb1StockMember);
            return bHasTotals;
        }
        private async Task<bool> SetOutputSB1StockTotals(Outcome oc)
        {
            bool bHasTotals = false;
            //run the output analysis using this.SB1DescendentStock (so that ancestors can use them)
            if (oc.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock sb1Stock = new SB1Stock(this.GCCalculatorParams,
                    this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                sb1Stock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(oc, sb1Stock);
                foreach (var output in oc.Outputs)
                {
                    //add indicator1stocks to corresponding MandE element
                    SB1Stock sb1StockMember = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    sb1StockMember.CopyCalculatorProperties(this.SB1DescendentStock);
                    CopyBaseElementProperties(output, sb1StockMember);
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        //but still needs ocamount, aohamount, and capamount
                        sb1StockMember.Multiplier = output.Multiplier;
                    }
                    else
                    {
                        //existing output.Multiplier comes from correct parent oc before ocs were aggregated
                        sb1StockMember.Multiplier = output.Multiplier;
                    }
                    if (output.Calculators != null)
                    {
                        sb1StockMember.SubApplicationType = this.GCCalculatorParams.SubApplicationType.ToString();
                        //add a benefit calculator to subp1s collection
                        //some analyzers use the subp1s (total1), so they must be copied correctly into new stock objects
                        sb1StockMember.SB12Stock.AddOutputCalcsToStock(output.Calculators);
                        //SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, sb1StockMember.SB12Stock.SB1Calcs);
                    }
                    //add the stockmember to the base stock
                    bHasTotals = await sb1StockMember.RunAnalyses();
                    //don't double count the multiplier
                    sb1StockMember.Multiplier = 1;
                    SetCalculatorId(output, sb1StockMember);
                    output.Calculators.Add(sb1StockMember);
                }
            }
            return bHasTotals;
        }


        //inputs
        public async Task<bool> SetInputAggregation(List<Input> inputs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry1 = inputs
                        .GroupBy(m => m.TypeId.ToString());
                    InputGroup inGroup1 = GetInputSBsAggregation(qry1);
                    if (inGroup1 != null)
                    {
                        bHasGroups = await SetInputSB1StockTotals(inGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry2 = inputs
                        .GroupBy(m => m.GroupId.ToString());
                    InputGroup inGroup2 = GetInputSBsAggregation(qry2);
                    if (inGroup2 != null)
                    {
                        bHasGroups = await SetInputSB1StockTotals(inGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry3 = inputs
                        .GroupBy(m => m.Label);
                    InputGroup inGroup3 = GetInputSBsAggregation(qry3);
                    if (inGroup3 != null)
                    {
                        bHasGroups = await SetInputSB1StockTotals(inGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        InputGroup inGroup4 = GetInputSBsAggregation(qry4);
                        if (inGroup4 != null)
                        {
                            bHasGroups = await SetInputSB1StockTotals(inGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        InputGroup inGroup4 = GetInputSBsAggregation(qry4);
                        if (inGroup4 != null)
                        {
                            bHasGroups = await SetInputSB1StockTotals(inGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = inputs
                            .GroupBy(m => m.Id.ToString());
                        InputGroup inGroup4 = GetInputSBsAggregation(qry4);
                        if (inGroup4 != null)
                        {
                            bHasGroups = await SetInputSB1StockTotals(inGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private InputGroup GetInputSBsAggregation(
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
                            SB1AnalyzerHelper.AddInputCalculators(input.Calculators, inputNew.Calculators);
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
                                    SB1AnalyzerHelper.AddInputCalculators(input2.Calculators, input1.Calculators);
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
        private async Task<bool> SetInputSB1StockTotals(InputGroup inputGroup)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (inputGroup.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock inGroupStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                inGroupStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(inputGroup, inGroupStock);
                //each oc holds its own totals for indicators
                //194 removed orderbyId
                //order them by ascending date (totals accrue from earliest to latest)
                inputGroup.Inputs = inputGroup.Inputs.OrderBy(oc => oc.Date).ToList();
                //run the oc analysis
                if (inputGroup.Inputs != null)
                {
                    //note that inputs don't run independent input calcs
                    //they only serve as input series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var input in inputGroup.Inputs)
                    {
                        //add the descendent calcs
                        bHasAnalysis = await SetInputSB1StockTotals(input);
                        SB1AnalyzerHelper.AddCalculators(input.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = inGroupStock.RunAnalyses(calcs);
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(inGroupStock.MathResult)
                        && inGroupStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = inGroupStock.MathResult;
                        }
                    }
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToInputs(inGroupStock, inputGroup.Inputs);
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
        private async Task<bool> SetInputSB1StockTotals(Input input)
        {
            bool bHasAnalysis = true;
            //run the input analysis
            if (input != null)
            {
                if ((this.GCCalculatorParams.StartingDocToCalcNodeName
                    == Input.INPUT_PRICE_TYPES.inputgroup.ToString()))
                {
                    //run the base calcs, but don't run analyses
                    //188 changed to uniform pattern -it's the series that are important
                    if (input.Inputs != null)
                    {
                        //add analyses to input
                        bHasAnalysis = await SetInputSeriesAggregation(input);
                    }
                    //bHasAnalysis = await SetInputSeriesSB1BaseStockTotals(input);
                }
                else
                {
                    //ok to run analyses
                    if (input.Inputs != null)
                    {
                        //add analyses to input
                        bHasAnalysis = await SetInputSeriesAggregation(input);
                    }
                }
            }
            return bHasAnalysis;
        }
        //can be run at both input and input series levels
        private async Task<bool> SetInputSeriesSB1BaseStockTotals(Input inputserie)
        {
            bool bHasAnalysis = true;
            bool bHasTotals = false;
            if (inputserie.Calculators != null)
            {
                //each sbc calculator is considered a unique input observation needing a unique stock total
                //i.e. if this input is an aggregation of five inputs with sbccalcs, that's five observations
                List<Calculator1> stocks = new List<Calculator1>();
                foreach (Calculator1 calc in inputserie.Calculators)
                {
                    //add indicator1stocks to corresponding MandE element
                    SB1Stock sb1StockMember = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    sb1StockMember.CopyCalculatorProperties(this.SB1DescendentStock);
                    CopyBaseElementProperties(inputserie, calc);
                    //but keep unique id
                    sb1StockMember.Id = calc.Id;
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        //but still needs ocamount, aohamount, and capamount
                        sb1StockMember.Multiplier = inputserie.Multiplier;
                    }
                    else
                    {
                        sb1StockMember.Multiplier = inputserie.Multiplier;
                    }
                    sb1StockMember.SB11Stock.AddInputCalcToStock(calc);
                    if (sb1StockMember.SB11Stock.SB1Calcs.Count > 0)
                    {
                        bHasAnalysis = await sb1StockMember.RunAnalyses();
                        //don't double count the multiplier
                        sb1StockMember.Multiplier = 1;
                        if (bHasAnalysis)
                        {
                            stocks.Add(sb1StockMember);
                            bHasTotals = true;
                        }
                    }
                }
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString()
                    || SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
                {
                    foreach (var stock in stocks)
                    {
                        //don't set calcid for multiple stocks
                        inputserie.Calculators.Add(stock);
                    }
                }
                else
                {
                    //this gives each input and output correct number of sbccalc observations
                    SB1Stock sb1StockMember2 = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //run the series analyses by aggregating the calcs
                    bHasTotals = sb1StockMember2.RunAnalyses(stocks);
                    if (bHasTotals)
                    {
                        inputserie.Calculators = new List<Calculator1>();
                        //each inputserie has the correct observation
                        SetCalculatorId(inputserie, sb1StockMember2);
                        inputserie.Calculators.Add(sb1StockMember2);
                        //to use same number of obs in parent would need to pass back the stocks
                        //and use that collection to run parent
                    }
                }
            }
            return bHasTotals;
        }
        //inputseries
        public async Task<bool> SetInputSeriesAggregation(Input baseInput)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Input>>
                        qry1 = baseInput.Inputs
                        .GroupBy(m => m.TypeId.ToString());
                    Input input1 = GetInputSeriesSBsAggregation(baseInput, qry1);
                    if (input1 != null)
                    {
                        bHasGroups = await SetInputSeriesSB1StockTotals(input1);
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
                    Input input2 = GetInputSeriesSBsAggregation(baseInput, qry2);
                    if (input2 != null)
                    {
                        bHasGroups = await SetInputSeriesSB1StockTotals(input2);
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
                    Input input3 = GetInputSeriesSBsAggregation(baseInput, qry3);
                    if (input3 != null)
                    {
                        bHasGroups = await SetInputSeriesSB1StockTotals(input3);
                        if (bHasGroups)
                        {
                            //copy inputseries w analyses to input
                            CopyInputSeriesToInput(baseInput, input3);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = baseInput.Inputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        Input input4 = GetInputSeriesSBsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = await SetInputSeriesSB1StockTotals(input4);
                            if (bHasGroups)
                            {
                                //copy inputseries w analyses to input
                                CopyInputSeriesToInput(baseInput, input4);
                            }
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Input>>
                            qry4 = baseInput.Inputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        Input input4 = GetInputSeriesSBsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = await SetInputSeriesSB1StockTotals(input4);
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
                        Input input4 = GetInputSeriesSBsAggregation(baseInput, qry4);
                        if (input4 != null)
                        {
                            bHasGroups = await SetInputSeriesSB1StockTotals(input4);
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
        private Input GetInputSeriesSBsAggregation(Input baseInput,
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
                            SB1AnalyzerHelper.AddInputCalculators(input.Calculators, inputNew.Calculators);
                        }
                        i += 1;
                    }
                    newInput.Inputs.Add(inputNew);
                    a++;
                }
            }
            return newInput;
        }
        private async Task<bool> SetInputSeriesSB1StockTotals(Input input)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (input.Inputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock inputStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                inputStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(input, inputStock);
                //each oc holds its own totals for indicators
                //194 removed orderbyId
                //order them by ascending date (totals accrue from earliest to latest)
                input.Inputs = input.Inputs.OrderBy(i => i.Date).ToList();
                //run the oc analysis
                if (input.Inputs != null)
                {
                    //note that inputs don't run independent input calcs
                    //they only serve as input series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var inputserie in input.Inputs)
                    {
                        //add the descendent calcs
                        bHasAnalysis = await SetInputSeriesSB1BaseStockTotals(inputserie);
                        SB1AnalyzerHelper.AddCalculators(inputserie.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = inputStock.RunAnalyses(calcs);
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(inputStock.MathResult)
                        && inputStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = inputStock.MathResult;
                        }
                    }
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToInputs(inputStock, input.Inputs);
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
        
        private void AddCalculatorsToInputs(SB1Stock inputStock,
            List<Input> inputs)
        {
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
        private void AddCalculatorsToOutputs(SB1Stock outputStock,
            List<Output> outputs)
        {
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                            == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
        private void AddCalculatorsToOCs(SB1Stock ocStock,
            List<OperationComponent> ocs)
        {
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                                    {
                                        //the list of calcs were ordered by date to get right progress
                                        if (calc.Id == change.Id
                                            && calc.Date == change.Date
                                            && calc.TargetType == change.TargetType)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            oc.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                    else
                                    {
                                        if (calc.Id == change.Id)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            oc.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
        private void AddCalculatorsToOutcomes(SB1Stock ocStock,
            List<Outcome> ocs)
        {
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                                    {
                                        //the list of calcs were ordered by date to get right progress
                                        if (calc.Id == change.Id
                                            && calc.Date == change.Date
                                            && calc.TargetType == change.TargetType)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            oc.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                    else
                                    {
                                        if (calc.Id == change.Id)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            oc.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
        private void AddCalculatorsToTPs(SB1Stock biStock,
            List<TimePeriod> tps)
        {
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                                    {
                                        //the list of calcs were ordered by date to get right progress
                                        if (calc.Id == change.Id
                                            && calc.Date == change.Date
                                            && calc.TargetType == change.TargetType)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            tp.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                    else
                                    {
                                        if (calc.Id == change.Id)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            tp.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
        private void AddCalculatorsToBIs(SB1Stock biGroupStock,
            List<BudgetInvestment> bis)
        {
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                                    {
                                        //the list of calcs were ordered by date to get right progress
                                        if (calc.Id == change.Id
                                            && calc.Date == change.Date
                                            && calc.TargetType == change.TargetType)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            bi.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                    else
                                    {
                                        if (calc.Id == change.Id)
                                        {
                                            //replace the stock calculations
                                            //don't need siblings, so new list
                                            bi.Calculators = new List<Calculator1>();
                                            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                                                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
        public async Task<bool> SetOutputAggregation(List<Output> outputs)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry1 = outputs
                        .GroupBy(m => m.TypeId.ToString());
                    OutputGroup outputGroup1 = GetOutputSBsAggregation(qry1);
                    if (outputGroup1 != null)
                    {
                        bHasGroups = await SetOutputSB1StockTotals(outputGroup1);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.groups:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry2 = outputs
                        .GroupBy(m => m.GroupId.ToString());
                    OutputGroup outputGroup2 = GetOutputSBsAggregation(qry2);
                    if (outputGroup2 != null)
                    {
                        bHasGroups = await SetOutputSB1StockTotals(outputGroup2);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.labels:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry3 = outputs
                        .GroupBy(m => m.Label);
                    OutputGroup outputGroup3 = GetOutputSBsAggregation(qry3);
                    if (outputGroup3 != null)
                    {
                        bHasGroups = await SetOutputSB1StockTotals(outputGroup3);
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        OutputGroup outputGroup4 = GetOutputSBsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = await SetOutputSB1StockTotals(outputGroup4);
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        OutputGroup outputGroup4 = GetOutputSBsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = await SetOutputSB1StockTotals(outputGroup4);
                        }
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = outputs
                            .GroupBy(m => m.Id.ToString());
                        OutputGroup outputGroup4 = GetOutputSBsAggregation(qry4);
                        if (outputGroup4 != null)
                        {
                            bHasGroups = await SetOutputSB1StockTotals(outputGroup4);
                        }
                    }
                    break;
                default:
                    break;
            }
            return bHasGroups;
        }
        private OutputGroup GetOutputSBsAggregation(
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
                            SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, outputNew.Calculators);
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
                                    SB1AnalyzerHelper.AddOutputCalculators(output2.Calculators, output1.Calculators);
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
        private async Task<bool> SetOutputSB1StockTotals(OutputGroup outputGroup)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (outputGroup.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock outGroupStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                outGroupStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(outputGroup, outGroupStock);
                //each oc holds its own totals for indicators
                //194 removed orderbyId
                //order them by ascending date (totals accrue from earliest to latest)
                outputGroup.Outputs = outputGroup.Outputs.OrderBy(oc => oc.Date).ToList();
                //run the oc analysis
                if (outputGroup.Outputs != null)
                {
                    //note that outputs don't run independent output calcs
                    //they only serve as output series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var output in outputGroup.Outputs)
                    {
                        //add the descendent calcs
                        bHasAnalysis = await SetOutputSB1StockTotals(output);
                        SB1AnalyzerHelper.AddCalculators(output.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = outGroupStock.RunAnalyses(calcs);
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(outGroupStock.MathResult)
                        && outGroupStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = outGroupStock.MathResult;
                        }
                    }
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutputs(outGroupStock, outputGroup.Outputs);
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
        private async Task<bool> SetOutputSB1StockTotals(Output output)
        {
            bool bHasAnalysis = true;
            //run the output analysis
            if (output != null)
            {
                if ((this.GCCalculatorParams.StartingDocToCalcNodeName
                    == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString()))
                {
                    //188: changed to uniform pattern -the series should be the emphasis, not the input or output
                    //ok to run analyses
                    if (output.Outputs != null)
                    {
                        //add analyses to output
                        bHasAnalysis = await SetOutputSeriesAggregation(output);
                    }
                    ////run the base calcs, but don't run analyses
                    //bHasAnalysis = await SetOutputSeriesSB1BaseStockTotals(output);
                }
                else
                {
                    //ok to run analyses
                    if (output.Outputs != null)
                    {
                        //add analyses to output
                        bHasAnalysis = await SetOutputSeriesAggregation(output);
                    }
                }
            }
            return bHasAnalysis;
        }
        //can be run at both output and output series levels
        private async Task<bool> SetOutputSeriesSB1BaseStockTotals(Output outputserie)
        {
            bool bHasAnalysis = true;
            bool bHasTotals = false;
            if (outputserie.Calculators != null)
            {
                //each sbc calculator is considered a unique output observation needing a unique stock total
                //i.e. if this output is an aggregation of five outputs with sbccalcs, that's five observations
                List<Calculator1> stocks = new List<Calculator1>();
                foreach (Calculator1 calc in outputserie.Calculators)
                {
                    //add indicator1stocks to corresponding MandE element
                    SB1Stock sb1StockMember = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //keep a uniform analyzer pattern
                    sb1StockMember.CopyCalculatorProperties(this.SB1DescendentStock);
                    CopyBaseElementProperties(outputserie, calc);
                    //but keep unique id
                    sb1StockMember.Id = calc.Id;
                    //set multiplier used in analysis 
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        //but still needs ocamount, aohamount, and capamount
                        sb1StockMember.Multiplier = outputserie.Multiplier;
                    }
                    else
                    {
                        sb1StockMember.Multiplier = outputserie.Multiplier;
                    }
                    sb1StockMember.SB12Stock.AddOutputCalcToStock(calc);
                    if (sb1StockMember.SB12Stock.SB2Calcs.Count > 0)
                    {
                        bHasAnalysis = await sb1StockMember.RunAnalyses();
                        //don't double count the multiplier
                        sb1StockMember.Multiplier = 1;
                        if (bHasAnalysis)
                        {
                            stocks.Add(sb1StockMember);
                            bHasTotals = true;
                        }
                    }
                }
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString()
                    || SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
                {
                    foreach (var stock in stocks)
                    {
                        //don't set calcid for multiple stocks
                        outputserie.Calculators.Add(stock);
                    }
                }
                else
                {
                    //this gives each output and output correct number of sbccalc observations
                    SB1Stock sb1StockMember2 = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //run the series analyses by aggregating the calcs
                    bHasTotals = sb1StockMember2.RunAnalyses(stocks);
                    if (bHasTotals)
                    {
                        outputserie.Calculators = new List<Calculator1>();
                        //each outputserie has the correct observation
                        SetCalculatorId(outputserie, sb1StockMember2);
                        outputserie.Calculators.Add(sb1StockMember2);
                        //to use same number of obs in parent would need to pass back the stocks
                        //and use that collection to run parent
                    }
                }
            }
            return bHasTotals;
        }
        //outputseries
        public async Task<bool> SetOutputSeriesAggregation(Output baseOutput)
        {
            bool bHasGroups = false;
            switch (this.GCCalculatorParams.AnalyzerParms.AggregationType)
            {
                case AnalyzerHelper.AGGREGATION_OPTIONS.types:
                    IEnumerable<System.Linq.IGrouping<string, Output>>
                        qry1 = baseOutput.Outputs
                        .GroupBy(m => m.TypeId.ToString());
                    Output output1 = GetOutputSeriesSBsAggregation(baseOutput, qry1);
                    if (output1 != null)
                    {
                        bHasGroups = await SetOutputSeriesSB1StockTotals(output1);
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
                    Output output2 = GetOutputSeriesSBsAggregation(baseOutput, qry2);
                    if (output2 != null)
                    {
                        bHasGroups = await SetOutputSeriesSB1StockTotals(output2);
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
                    Output output3 = GetOutputSeriesSBsAggregation(baseOutput, qry3);
                    if (output3 != null)
                    {
                        bHasGroups = await SetOutputSeriesSB1StockTotals(output3);
                        if (bHasGroups)
                        {
                            //copy outputseries w analyses to output
                            CopyOutputSeriesToOutput(baseOutput, output3);
                        }
                    }
                    break;
                case AnalyzerHelper.AGGREGATION_OPTIONS.none:
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = baseOutput.Outputs
                            .OrderBy(m => m.Date.Year.ToString())
                            .GroupBy(m => m.Date.Year.ToString());
                        Output output4 = GetOutputSeriesSBsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = await SetOutputSeriesSB1StockTotals(output4);
                            if (bHasGroups)
                            {
                                //copy outputseries w analyses to output
                                CopyOutputSeriesToOutput(baseOutput, output4);
                            }
                        }
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                        && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IEnumerable<System.Linq.IGrouping<string, Output>>
                            qry4 = baseOutput.Outputs
                            .OrderBy(m => m.AlternativeType)
                            .GroupBy(m => m.AlternativeType);
                        Output output4 = GetOutputSeriesSBsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = await SetOutputSeriesSB1StockTotals(output4);
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
                        Output output4 = GetOutputSeriesSBsAggregation(baseOutput, qry4);
                        if (output4 != null)
                        {
                            bHasGroups = await SetOutputSeriesSB1StockTotals(output4);
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
        private Output GetOutputSeriesSBsAggregation(Output baseOutput,
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
                            SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, outputNew.Calculators);
                        }
                        i += 1;
                    }
                    newOutput.Outputs.Add(outputNew);
                    a++;
                }
            }
            return newOutput;
        }
        private async Task<bool> SetOutputSeriesSB1StockTotals(Output output)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            if (output.Outputs != null)
            {
                //build a stock element that holds ind1stocks that correspond to MandEElements
                SB1Stock outputStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                //need the options
                outputStock.CopyCalculatorProperties(this.SB1DescendentStock);
                CopyBaseElementProperties(output, outputStock);
                //each oc holds its own totals for indicators
                //194 removed orderbyId
                //order them by ascending date (totals accrue from earliest to latest)
                output.Outputs = output.Outputs.OrderBy(o => o.Date).ToList();
                //run the oc analysis
                if (output.Outputs != null)
                {
                    //note that outputs don't run independent output calcs
                    //they only serve as output series analysis holders
                    List<Calculator1> calcs = new List<Calculator1>();
                    foreach (var outputserie in output.Outputs)
                    {
                        //add the descendent calcs
                        bHasAnalysis = await SetOutputSeriesSB1BaseStockTotals(outputserie);
                        SB1AnalyzerHelper.AddCalculators(outputserie.Calculators, calcs);
                    }
                    //run the analyses by aggregating the calcs
                    bHasTotals = outputStock.RunAnalyses(calcs);
                    //188 pass mathresult back to ui
                    if (!string.IsNullOrEmpty(outputStock.MathResult)
                        && outputStock.MathResult != Constants.NONE)
                    {
                        //don't overwrite original analysis -parent can run as well, but children hold the main analysis
                        if (string.IsNullOrEmpty(this.GCCalculatorParams.MathResult))
                        {
                            this.GCCalculatorParams.MathResult = outputStock.MathResult;
                        }
                    }
                }
                if (bHasTotals)
                {
                    //some analyzers must now transfer calcs to series
                    AddCalculatorsToOutputs(outputStock, output.Outputs);
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
        public static void CopyBaseElementProperties(Calculator1 baseElement, SB1Stock stock)
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
                SB1Stock teststock = new SB1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            SB1Stock me = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalSB1StockProperties(stock);
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
                SB1Stock teststock = new SB1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            SB1Stock me = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalSB1StockProperties(stock);
                            //some analyzers display the results of the initial cost and benefit calcs
                            if (stock.SB11Stock != null)
                            {
                                if (stock.SB11Stock.SB1Calcs != null)
                                {
                                    if (stock.SB11Stock.SB1Calcs.Count > 0)
                                    {
                                        //these are sbc cost calculators not generic subp1s
                                        me.SB11Stock = new SB101Stock();
                                        me.SB11Stock.SB1Calcs = new List<SBC1Calculator>();
                                        stock.SB11Stock.AddSB1CalcsToStock(me.SB11Stock.SB1Calcs);
                                        //SB1AnalyzerHelper.AddInputCalculators(stock.SB11Stock.SB1Calcs, me.SB11Stock.SB1Calcs);
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
                SB1Stock teststock = new SB1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            SB1Stock me = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalSB1StockProperties(stock);
                            //some analyzers display the results of the initial cost and benefit calcs
                            if (stock.SB12Stock != null)
                            {
                                if (stock.SB12Stock.SB2Calcs != null)
                                {
                                    if (stock.SB12Stock.SB2Calcs.Count > 0)
                                    {
                                        //these are sbc cost calculators not generic subp1s
                                        me.SB12Stock = new SB102Stock();
                                        me.SB12Stock.SB2Calcs = new List<SBB1Calculator>();
                                        stock.SB12Stock.AddSB2CalcsToStock(me.SB12Stock.SB2Calcs);
                                        //SB1AnalyzerHelper.AddOutputCalculators(stock.SB12Stock.SB1Calcs, me.SB12Stock.SB1Calcs);
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
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
            {
                int iNewCalcsCount = calcs.Count;
                //add the base comparator
                AddAncestorBaseTPCalcs(calcs);
                //don't change any calc to actual unless there's a base comparator
                if (iNewCalcsCount < calcs.Count
                    || this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                {
                    //actuals have to be set before base and partials
                    foreach (var calc2 in calcs)
                    {
                        //don't change newly added benchmark calcs
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                            calc.ChangeType
                                            = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                        }
                                        SB1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
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
                                            SB1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
                                        }
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
                != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                SB1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
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
                    SB1AnalyzerHelper.AddCalculators(calcs, compCalcs);
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
                        != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                            }
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
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                            == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                        != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                            calc.ChangeType
                                                = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                        }
                                        SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
                                            SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
                != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
                            != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                            != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString()))
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
                            == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                    {
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
                                            calc.ChangeType
                                                = Calculator1.CHANGE_TYPES.xminus1.ToString();
                                        }
                                        SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
                            foreach(var tp in ancestorTPs)
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
                                            SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
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
            //progress1 uses minus1
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                != SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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
                                SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                            }
                        }
                    }
                }
            }
        }
        private void AddAllTPCalcs(List<Calculator1> calcs)
        {
            //progress1 uses all
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
             == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                foreach (var bi in this.BudgetGroup.BudgetInvestments)
                {
                    foreach (var tp in bi.TimePeriods)
                    {
                        if (tp.Calculators != null)
                        {
                            //must be able to distinguish ancestor from current
                            //not necessary to set existing TargetTypes (they are ignored anyway)
                            foreach (var calc in tp.Calculators)
                            {
                                SB1AnalyzerHelper.AddCalculators(tp.Calculators, calcs);
                            }
                        }
                    }
                }
            }
        }
        private void AddAllOutcomeCalcs(List<Calculator1> calcs)
        {
            //progress1 uses all
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
             == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                foreach (var bi in this.BudgetGroup.BudgetInvestments)
                {
                    foreach(var tp in bi.TimePeriods)
                    {
                        foreach (var oc in tp.Outcomes)
                        {
                            if (oc.Calculators != null)
                            {
                                //must be able to distinguish ancestor from current
                                //not necessary to set existing TargetTypes (they are ignored anyway)
                                foreach (var calc in oc.Calculators)
                                {
                                    SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddAllOCCalcs(List<Calculator1> calcs)
        {
            //progress1 uses all
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
             == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                foreach (var bi in this.BudgetGroup.BudgetInvestments)
                {
                    foreach (var tp in bi.TimePeriods)
                    {
                        foreach (var oc in tp.OperationComponents)
                        {
                            if (oc.Calculators != null)
                            {
                                //must be able to distinguish ancestor from current
                                //not necessary to set existing TargetTypes (they are ignored anyway)
                                foreach (var calc in oc.Calculators)
                                {
                                    SB1AnalyzerHelper.AddCalculators(oc.Calculators, calcs);
                                }
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
        public async Task<bool> SaveBISB1StockTotals(XmlWriter writer)
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
                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.BudgetGroup.BudgetInvestments.Count;
                    SetComparativeBaseAttributes(this.BudgetGroup, this.BudgetGroup.BudgetInvestments.Count, writer);
                }
                //groups have no extension (one totals only)
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.SB1DescendentStock.Option1);
                this.BudgetGroup.SetNewBIGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //should this be an If (IsSelfOrDescendent) ?
                await AddSB1StockTotals(sAttNameExt, this.BudgetGroup.Calculators, writer);
                if (this.BudgetGroup.BudgetInvestments != null)
                {
                    bHasCalculations = await SetBIStockTotals(writer, this.BudgetGroup.BudgetInvestments);
                }
                await writer.WriteEndElementAsync();
            }
            return bHasCalculations;
        }
        private async Task<bool> SetBIStockTotals(XmlWriter writer, List<BudgetInvestment> bis)
        {
            bool bHasTotals = false;
            if (bis != null)
            {
                //comps go horizontal
                if (this.SB1DescendentStock.Option1
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var bi in bis)
                {
                    if (this.SB1DescendentStock.Option1
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(bi, bis.Count, writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                    bi.SetNewBIAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        await AddSB1StockTotals(sAttNameExt, bi.Calculators, writer);
                        if (bi.TimePeriods != null)
                        {
                            bHasTotals = await SetTimePeriodStockTotals(writer, bi.TimePeriods);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //bi
                        await writer.WriteEndElementAsync();
                    }
                }
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                    await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                    i = 0;
                    foreach (var bi in bis)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(bi, bis.Count, writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, bi.Calculators, writer);
                        if (i == 0)
                        {
                            SetRowCount(writer);
                        }
                        i++;
                    }
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                    if (bis.Count == 1)
                    {
                        foreach (var bi in bis)
                        {
                            //tps set this.ColIndex
                            if (bi.TimePeriods != null)
                            {
                                bHasTotals = await SetTimePeriodStockTotals(writer, bi.TimePeriods);
                            }
                            i++;
                        }
                        //bi
                        await writer.WriteEndElementAsync();
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
                                bHasTotals = await SetTPComparisons(writer, bi.TimePeriods);
                            }
                            //comparative analysis line up using either budgetindex (if this.ColIndex >1) or tpindex
                            this.ColumnIndex++;
                        }
                        //bi
                        await writer.WriteEndElementAsync();
                    }
                }
            }
            return bHasTotals;
        }
        private async Task<bool> SetTimePeriodStockTotals(XmlWriter writer, List<TimePeriod> tps)
        {
            bool bHasTotals = false;
            if (tps != null)
            {
                //total number of columns needed in descendents
                this.ColumnCount = tps.Count;
                //comps go horizontal
                if (this.SB1DescendentStock.Option1
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var tp in tps)
                {
                    if (this.SB1DescendentStock.Option1
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(tp, tps.Count, writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
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
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        await AddSB1StockTotals(sAttNameExt, tp.Calculators, writer);
                        await SetTPDescendentTotals(writer, tp);
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //tp
                        await writer.WriteEndElementAsync();
                    }
                }
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                    await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                    i = 0;
                    foreach (var tp in tps)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(tp, tps.Count, writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, tp.Calculators, writer);
                        if (i == 0)
                        {
                            SetRowCount(writer);
                        }
                        i++;
                    }
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    this.ColumnIndex = 0;
                    foreach (var tp in tps)
                    {
                        bHasTotals = await SetTPOutcomeComparisons(writer, tp.Outcomes);
                        //comparative analysis line up using tpindex (budget 3 might start with a tpcol = 6)
                        this.ColumnIndex++;
                    }
                    //outcomes
                    await writer.WriteEndElementAsync();
                    if (this.GCCalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString();
                    }
                    else
                    {
                        this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString();
                    }
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    this.ColumnIndex = 0;
                    foreach (var tp in tps)
                    {
                        bHasTotals = await SetTPOCComparisons(writer, tp.OperationComponents);
                        //comparative analysis line up using tpindex (budget 3 might start with a tpcol = 6)
                        this.ColumnIndex++;
                    }
                    //opcomps
                    await writer.WriteEndElementAsync();
                    //tp
                    await writer.WriteEndElementAsync();
                }
            }
            return bHasTotals;
        }
        private async Task<bool> SetTPComparisons(XmlWriter writer, List<TimePeriod> tps)
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(tp, this.ColumnCount, writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.SB1DescendentStock.Option1);
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
                        biTPs = await SetSiblingTPCalculator(writer, tp, bIsCalc);
                        //add the analysis
                        await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                        await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(tp, this.ColumnCount, writer);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, tp.Calculators, writer);
                        SetRowCount(writer);
                        //signal not to use again
                        tp.Label2 = "00";
                        //tp.Id = 0;
                        bIsCalc = true;
                        biTPs = new Dictionary<int, int>();
                        //recurse to sibling time periods and add same tp if found with bi colindex
                        biTPs = await SetSiblingTPCalculator(writer, tp, bIsCalc);
                        //lv
                        await writer.WriteEndElementAsync();
                        //root
                        await writer.WriteEndElementAsync();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        //tp
                        //await writer.WriteEndElementAsync();

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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                        bHasTotals = await SetBITPOutcomeComparisons(writer, tp.Outcomes, biTPs);
                        //outcomes
                        await writer.WriteEndElementAsync();
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets)
                        {
                            this.CurrentNodeName = BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString();
                        }
                        else
                        {
                            this.CurrentNodeName = BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString();
                        }
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                        bHasTotals = await SetBITPOCComparisons(writer, tp.OperationComponents, biTPs);
                        //opcomps
                        await writer.WriteEndElementAsync();
                        //tp
                        await writer.WriteEndElementAsync();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private async Task<Dictionary<int, int>> SetSiblingTPCalculator(XmlWriter writer,
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
                                        if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                        = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.SB1DescendentStock.Option1);
                                    if (isCalculator)
                                    {
                                        await AddSB1StockCompTotals(sAttNameExt, sibTP.Calculators, writer);
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
                                        SetSiblingComparativeBaseProperties(sibTP, sAttNameExt, writer);
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
        private async Task<bool> SetTPDescendentTotals(XmlWriter writer, TimePeriod tp)
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
                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                bHasTotals = await SetOutcomeStockTotals(writer, tp.Outcomes);
                await writer.WriteEndElementAsync();
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
                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                bHasTotals = await SetOCStockTotals(writer, tp.OperationComponents);
                await writer.WriteEndElementAsync();
            }
            return bHasTotals;
        }
        public async Task<bool> SaveOCSB1StockTotals(XmlWriter writer)
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
                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OCGroup.OperationComponents.Count;
                    SetComparativeBaseAttributes(this.OCGroup, this.OCGroup.OperationComponents.Count, writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.SB1DescendentStock.Option1);
                this.OCGroup.SetNewOCGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //should this be an If (IsSelfOrDescendent) ?
                await AddSB1StockTotals(sAttNameExt, this.OCGroup.Calculators, writer);
                if (this.OCGroup.OperationComponents != null)
                {
                    bHasCalculations = await SetOCStockTotals(writer, this.OCGroup.OperationComponents);
                }
                await writer.WriteEndElementAsync();
            }
            return bHasCalculations;
        }
        private async Task<bool> SetOCStockTotals(XmlWriter writer, List<OperationComponent> ocs)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal
                if (this.SB1DescendentStock.Option1
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (this.SB1DescendentStock.Option1
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(oc, ocs.Count, writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                    oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        await AddSB1StockTotals(sAttNameExt, oc.Calculators, writer);
                        if (oc.Inputs != null)
                        {
                            bHasTotals = await SetInputTechStockTotals(writer, oc.Inputs);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                }
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                    await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                    i = 0;
                    foreach (var oc in ocs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(oc, ocs.Count, writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, oc.Calculators, writer);
                        if (i == 0)
                        {
                            SetRowCount(writer);
                        }
                        i++;
                    }
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                    //don't include desc inputs in comp analysis
                    //op, comp, budop, investcomp
                    await writer.WriteEndElementAsync();
                }
            }
            return bHasTotals;
        }

        private async Task<bool> SetBITPOCComparisons(XmlWriter writer, List<OperationComponent> ocs,
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);

                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.SB1DescendentStock.Option1);
                        oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        await SetSiblingTPOCCalculator(writer, oc, bIsCalc, biTps);
                        //add the analysis
                        await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                        await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, oc.Calculators, writer);
                        SetRowCount(writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        await SetSiblingTPOCCalculator(writer, oc, bIsCalc, biTps);
                        //lv
                        await writer.WriteEndElementAsync();
                        //root
                        await writer.WriteEndElementAsync();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private async Task<bool> SetBITPOutcomeComparisons(XmlWriter writer, List<Outcome> ocs,
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.SB1DescendentStock.Option1);
                        oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        await SetSiblingTPOutcomeCalculator(writer, oc, bIsCalc, biTps);
                        //add the analysis
                        await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                        await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, oc.Calculators, writer);
                        SetRowCount(writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        await SetSiblingTPOutcomeCalculator(writer, oc, bIsCalc, biTps);
                        //lv
                        await writer.WriteEndElementAsync();
                        //root
                        await writer.WriteEndElementAsync();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private async Task SetSiblingTPOCCalculator(XmlWriter writer,
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
                                                if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                                = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.SB1DescendentStock.Option1);
                                            if (isCalculator)
                                            {
                                                await AddSB1StockCompTotals(sAttNameExt, sibOC.Calculators, writer);
                                                //signal not to use again
                                                sibOC.Id = 0;
                                            }
                                            else
                                            {
                                                SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, writer);
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
        private async Task SetSiblingTPOutcomeCalculator(XmlWriter writer,
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
                                                if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                                = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.SB1DescendentStock.Option1);
                                            if (isCalculator)
                                            {
                                                await AddSB1StockCompTotals(sAttNameExt, sibOC.Calculators, writer);
                                                //signal not to use again
                                                sibOC.Id = 0;
                                            }
                                            else
                                            {
                                                SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, writer);
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
        private async Task<bool> SetTPOCComparisons(XmlWriter writer, List<OperationComponent> ocs)
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);

                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.SB1DescendentStock.Option1);
                        oc.SetNewOperationComponentAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        await SetSiblingOCCalculator(writer, oc, bIsCalc);
                        //add the analysis
                        await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                        await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, oc.Calculators, writer);
                        SetRowCount(writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        await SetSiblingOCCalculator(writer, oc, bIsCalc);
                        //lv
                        await writer.WriteEndElementAsync();
                        //root
                        await writer.WriteEndElementAsync();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private async Task<bool> SetTPOutcomeComparisons(XmlWriter writer, List<Outcome> ocs)
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.SB1DescendentStock.Option1);
                        oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                        bool bIsCalc = false;
                        await SetSiblingOutcomeCalculator(writer, oc, bIsCalc);
                        //add the analysis
                        await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                        await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                        //need some attributes without attname extension
                        SetComparativeBaseAttributes(oc, this.ColumnCount, writer);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, oc.Calculators, writer);
                        SetRowCount(writer);
                        //signal not to use again
                        oc.Id = 0;
                        bIsCalc = true;
                        //recurse to sibling time periods and add same oc if found with tp colindex
                        await SetSiblingOutcomeCalculator(writer, oc, bIsCalc);
                        //lv
                        await writer.WriteEndElementAsync();
                        //root
                        await writer.WriteEndElementAsync();
                        //don't display inputs (each comp can has multiple inputs -they would need one input)
                        bHasTotals = true;
                        sAttNameExt = string.Empty;
                        i++;
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                    else
                    {
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private async Task SetSiblingOCCalculator(XmlWriter writer,
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
                                            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                            = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.SB1DescendentStock.Option1);
                                        if (isCalculator)
                                        {
                                            await AddSB1StockCompTotals(sAttNameExt, sibOC.Calculators, writer);
                                            //signal not to use again
                                            sibOC.Id = 0;
                                        }
                                        else
                                        {
                                            SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, writer);
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
        private async Task SetSiblingOutcomeCalculator(XmlWriter writer,
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
                                            if (SB1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
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
                                            = CalculatorHelpers.GetAttributeNameExtension(iColIndex, this.SB1DescendentStock.Option1);
                                        if (isCalculator)
                                        {
                                            await AddSB1StockCompTotals(sAttNameExt, sibOC.Calculators, writer);
                                            //signal not to use again
                                            sibOC.Id = 0;
                                        }
                                        else
                                        {
                                            SetSiblingComparativeBaseProperties(sibOC, sAttNameExt, writer);
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
        private async Task<bool> SetInputTechStockTotals(XmlWriter writer, List<Input> inputs)
        {
            bool bHasTotals = false;
            //inputs are not shown in tech comparisons
            if (inputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString())
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                    input.SetNewInputAttributes(sAttNameExt, ref writer);
                    if (bNeedsBaseCalcs)
                    {
                        await AddSB1InputStockTotals(sAttNameExt, input.Calculators, writer);
                    }
                    else
                    {
                        await AddSB1StockTotals(sAttNameExt, input.Calculators, writer);
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    //input
                    await writer.WriteEndElementAsync();
                }
            }
            return bHasTotals;
        }

        public async Task<bool> SaveOutcomeSB1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.OutcomeGroup != null)
            {
                this.CurrentNodeName = Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString();
                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OutcomeGroup.Outcomes.Count;
                    SetComparativeBaseAttributes(this.OutcomeGroup, this.OutcomeGroup.Outcomes.Count, writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.SB1DescendentStock.Option1);
                this.OutcomeGroup.SetNewOutcomeGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                await AddSB1StockTotals(sAttNameExt, this.OutcomeGroup.Calculators, writer);
                if (this.OutcomeGroup.Outcomes != null)
                {
                    bHasCalculations = await SetOutcomeStockTotals(writer, this.OutcomeGroup.Outcomes);
                }
                await writer.WriteEndElementAsync();
            }
            return bHasCalculations;
        }
        private async Task<bool> SetOutcomeStockTotals(XmlWriter writer, List<Outcome> ocs)
        {
            bool bHasTotals = false;
            if (ocs != null)
            {
                //comps go horizontal
                if (this.SB1DescendentStock.Option1
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var oc in ocs)
                {
                    if (this.SB1DescendentStock.Option1
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(oc, ocs.Count, writer);
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                    oc.SetNewOutcomeAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        await AddSB1StockTotals(sAttNameExt, oc.Calculators, writer);
                        if (oc.Outputs != null)
                        {
                            bHasTotals = await SetOutputTechStockTotals(writer, oc.Outputs);
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                }
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //add the analysis
                    await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                    await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                    i = 0;
                    foreach (var oc in ocs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(oc, ocs.Count, writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                        if (this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.budgets
                            || this.GCCalculatorParams.SubApplicationType
                            == Constants.SUBAPPLICATION_TYPES.investments)
                        {
                            //lines up tps side by side
                            sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(this.ColumnIndex, this.SB1DescendentStock.Option1);
                        }
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, oc.Calculators, writer);
                        if (i == 0)
                        {
                            SetRowCount(writer);
                        }
                        i++;
                    }
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                    //op, comp, budop, investcomp
                    await writer.WriteEndElementAsync();
                }
            }
            return bHasTotals;
        }
        private async Task<bool> SetOutputTechStockTotals(XmlWriter writer, List<Output> outputs)
        {
            bool bHasTotals = false;
            //inputs are not shown in tech comparisons
            if (outputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString())
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                    output.SetNewOutputAttributes(sAttNameExt, ref writer);
                    if (bNeedsBaseCalcs)
                    {
                        await AddSB1OutputStockTotals(sAttNameExt, output.Calculators, writer);
                    }
                    else
                    {
                        await AddSB1StockTotals(sAttNameExt, output.Calculators, writer);
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    //input
                    await writer.WriteEndElementAsync();
                }
            }
            return bHasTotals;
        }
        public async Task<bool> SaveInputSB1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.InputGroup != null)
            {
                this.CurrentNodeName = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.InputGroup.Inputs.Count;
                    SetComparativeBaseAttributes(this.InputGroup, this.InputGroup.Inputs.Count, writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.SB1DescendentStock.Option1);
                this.InputGroup.SetNewInputGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //1.4.1 removed this -detracts from analyzing inputs and input series
                //AddSB1StockTotals(sAttNameExt, this.InputGroup.Calculators, writer);
                if (this.InputGroup.Inputs != null)
                {
                    bool bIsSeries = false;
                    bHasCalculations = await SetInputStockTotals(writer, this.InputGroup.Inputs, bIsSeries);
                }
                await writer.WriteEndElementAsync();
            }
            return bHasCalculations;
        }
        private async Task<bool> SetInputStockTotals(XmlWriter writer, List<Input> inputs, bool isSeries)
        {
            bool bHasTotals = false;
            if (inputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                //comps go horizontal
                if (this.SB1DescendentStock.Option1
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var input in inputs)
                {
                    if (this.SB1DescendentStock.Option1
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(input, inputs.Count, writer);
                        }
                        else
                        {
                            //194: multiple inputs being analyzed from group level
                            //this analysis is discouraged, but prevents returning bad xml error
                            if (!isSeries)
                            {
                                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                            }
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                    input.SetNewInputAttributes(sAttNameExt, ref writer);
                    //don't display inputs (each comp can has multiple inputs -they would need one input)
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (bNeedsBaseCalcs)
                        {
                            await AddSB1InputStockTotals(sAttNameExt, input.Calculators, writer);
                        }
                        else
                        {
                            await AddSB1StockTotals(sAttNameExt, input.Calculators, writer);
                        }
                        //188 includes series because only the series is actually important
                        //don't process input series when analyses are run from group
                        //potential for too many series
                        if (input.Inputs != null
                            //&& this.GCCalculatorParams.StartingDocToCalcNodeName
                            //!= Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                            && isSeries == false)
                        {
                            bool bIsSeries = true;
                            bHasTotals = await SetInputStockTotals(writer, input.Inputs, bIsSeries);
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
                                    await AddSB1InputStockTotals(sAttNameExt, input.Calculators, writer);
                                }
                                else
                                {
                                    await AddSB1StockTotals(sAttNameExt, input.Calculators, writer);
                                }
                            }
                            //188 includes series because only the series is actually important
                            //don't process input series when analyses are run from group
                            //potential for too many series
                            if (input.Inputs != null)
                                //&& this.GCCalculatorParams.StartingDocToCalcNodeName
                                //!= Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                            {
                                bool bIsSeries = true;
                                bHasTotals = await SetInputStockTotals(writer, input.Inputs, bIsSeries);
                            }
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                }
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString()
                    && isSeries)
                {
                    //add the analysis
                    await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                    await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                    i = 0;
                    foreach (var input in inputs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(input, inputs.Count, writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, input.Calculators, writer);
                        if (i == 0)
                        {
                            SetRowCount(writer);
                        }
                        i++;
                    }
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                    //descendents
                    //op, comp, budop, investcomp
                    await writer.WriteEndElementAsync();
                }
            }
            return bHasTotals;
        }
        public async Task<bool> SaveOutputSB1StockTotals(XmlWriter writer)
        {
            bool bHasCalculations = false;
            if (this.OutputGroup != null)
            {
                this.CurrentNodeName = Output.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                {
                    //need some attributes without attname extension
                    this.ColumnCount = this.OutputGroup.Outputs.Count;
                    SetComparativeBaseAttributes(this.OutputGroup, this.OutputGroup.Outputs.Count, writer);
                }
                string sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(0, this.SB1DescendentStock.Option1);
                this.OutputGroup.SetNewOutputGroupAttributes(sAttNameExt, ref writer);
                //add group stock calculations
                //1.4.1 removed this -detracts from analyzing inputs and input series
                //AddSB1StockTotals(sAttNameExt, this.OutputGroup.Calculators, writer);
                if (this.OutputGroup.Outputs != null)
                {
                    bool bIsSeries = false;
                    bHasCalculations = await SetOutputStockTotals(writer, this.OutputGroup.Outputs, bIsSeries);
                }
                await writer.WriteEndElementAsync();
            }
            return bHasCalculations;
        }
        private async Task<bool> SetOutputStockTotals(XmlWriter writer, List<Output> outputs, bool isSeries)
        {
            bool bHasTotals = false;
            if (outputs != null)
            {
                //some analyses use base calc results, not stock totals
                bool bNeedsBaseCalcs = false;
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString())
                {
                    bNeedsBaseCalcs = true;
                }
                else
                {
                    bNeedsBaseCalcs = false;
                }
                //comps go horizontal
                if (this.SB1DescendentStock.Option1
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
                    await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                }
                string sAttNameExt = string.Empty;
                int i = 0;
                foreach (var output in outputs)
                {
                    if (this.SB1DescendentStock.Option1
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
                        await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SetComparativeBaseAttributes(output, outputs.Count, writer);
                        }
                        else
                        {
                            //194: multiple outputs being analyzed from group level
                            //this analysis is discouraged, but prevents returning bad xml error
                            if (!isSeries)
                            {
                                await writer.WriteStartElementAsync(string.Empty, this.CurrentNodeName, string.Empty);
                            }
                        }
                    }
                    sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                    output.SetNewOutputAttributes(sAttNameExt, ref writer);
                    //don't display outputs (each comp can has multiple outputs -they would need one output)
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        if (bNeedsBaseCalcs)
                        {
                            await AddSB1OutputStockTotals(sAttNameExt, output.Calculators, writer);
                        }
                        else
                        {
                            await AddSB1StockTotals(sAttNameExt, output.Calculators, writer);
                        }
                        //188 includes series because only the series is actually important
                        //don't process output series when analyses are run from group
                        //potential for too many series
                        if (output.Outputs != null
                            //&& this.GCCalculatorParams.StartingDocToCalcNodeName
                            //!= Output.OUTPUT_PRICE_TYPES.outputgroup.ToString()
                            && isSeries == false)
                        {
                            bool bIsSeries = true;
                            bHasTotals = await SetOutputStockTotals(writer, output.Outputs, bIsSeries);
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
                                    await AddSB1OutputStockTotals(sAttNameExt, output.Calculators, writer);
                                }
                                else
                                {
                                    await AddSB1StockTotals(sAttNameExt, output.Calculators, writer);
                                }
                            }
                            //188 includes series because only the series is actually important
                            //don't process output series when analyses are run from group
                            //potential for too many series
                            if (output.Outputs != null)
                                //&& this.GCCalculatorParams.StartingDocToCalcNodeName
                                //!= Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                            {
                                bool bIsSeries = true;
                                bHasTotals = await SetOutputStockTotals(writer, output.Outputs, bIsSeries);
                                //if (outputs.Count > 0
                                //    && this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                                //{
                                //    //194 close out parent output
                                //    await writer.WriteEndElementAsync();
                                //}
                            }
                        }
                    }
                    bHasTotals = true;
                    sAttNameExt = string.Empty;
                    i++;
                    if (this.SB1DescendentStock.Option1
                        != AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
                    {
                        //op, comp, budop, investcomp
                        await writer.WriteEndElementAsync();
                    }
                }
                if (this.SB1DescendentStock.Option1
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString()
                    && isSeries)
                {
                    //add the analysis
                    await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                    await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                    i = 0;
                    foreach (var output in outputs)
                    {
                        if (i == 0)
                        {
                            //need some attributes without attname extension
                            SetComparativeBaseAttributes(output, outputs.Count, writer);
                        }
                        sAttNameExt = CalculatorHelpers.GetAttributeNameExtension(i, this.SB1DescendentStock.Option1);
                        this.RowCount = await AddSB1StockCompTotals(sAttNameExt, output.Calculators, writer);
                        if (i == 0)
                        {
                            SetRowCount(writer);
                        }
                        i++;
                    }
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                    //descendents
                    //op, comp, budop, investcomp

                    await writer.WriteEndElementAsync();
                }
            }
            return bHasTotals;
        }

        private async Task AddSB1StockTotals(string attNameExt, List<Calculator1> stocks, XmlWriter writer)
        {
            if (stocks != null)
            {
                //each stock can contain as many ptstocks as totaled
                //so only one SB1Stock should be used (or inserting/updating els becomes difficult)
                SB1Stock stock = new SB1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (SB1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                            await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseAttributes(calc, this.ColumnCount, writer);
                            }
                            //the stock totals are added to currentelement using attnameext to distinguish observations
                            await stock.SetDescendantSB1StockAttributesAsync(attNameExt, writer);
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetRowCount(writer);
                            }
                            await writer.WriteEndElementAsync();
                            await writer.WriteEndElementAsync();
                        }
                    }
                }
            }
        }
        private async Task<double> AddSB1StockCompTotals(string attNameExt, List<Calculator1> stocks,
            XmlWriter writer)
        {
            double dbRowCount = 0;
            if (stocks != null)
            {
                //each stock can contain as many ptstocks as totaled
                //so only one SB1Stock should be used (or inserting/updating els becomes difficult)
                SB1Stock stock = new SB1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (SB1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            //note: this will fail with more than one calculator so make sure not to use more than one
                            //stock calculator per node
                            //the stock totals are added to currentelement using attnameext to distinguish observations
                            await stock.SetDescendantSB1StockAttributesAsync(attNameExt, writer);
                            if (stock.SBCount > 0)
                            {
                                dbRowCount = stock.SBCount;
                            }
                        }
                    }
                }
            }
            return dbRowCount;
        }
        private async Task AddSB1InputStockTotals(string attNameExt, List<Calculator1> stocks,
            XmlWriter writer)
        {
            if (stocks != null)
            {
                //some stocks use calculator, not stock, results
                //the calcs are stored with the stock
                SB1Stock stock = new SB1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (SB1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                            await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseAttributes(calc, this.ColumnCount, writer);
                            }
                            await stock.SetDescendantSB1StockInputAttributesAsync(attNameExt, this.GCCalculatorParams,
                                writer);
                            await writer.WriteEndElementAsync();
                            await writer.WriteEndElementAsync();
                        }
                    }
                }
            }
        }
        private async Task AddSB1OutputStockTotals(string attNameExt, List<Calculator1> stocks,
            XmlWriter writer)
        {
            if (stocks != null)
            {
                //some stocks use calculator, not stock, results
                SB1Stock stock = new SB1Stock();
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (SB1Stock)calc;
                            stock.CalcParameters = new CalculatorParameters(this.GCCalculatorParams);
                            stock.CalcParameters.CurrentElementNodeName = this.CurrentNodeName;
                            stock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                            await writer.WriteStartElementAsync(string.Empty, Constants.ROOT_PATH, string.Empty);
                            await writer.WriteStartElementAsync(string.Empty, Constants.LINKEDVIEWS_TYPES.linkedview.ToString(), string.Empty);
                            if (!string.IsNullOrEmpty(attNameExt))
                            {
                                SetComparativeBaseAttributes(calc, this.ColumnCount, writer);
                            }
                            await stock.SetDescendantSB1StockOutputAttributesAsync(attNameExt, this.GCCalculatorParams,
                                writer);
                            await writer.WriteEndElementAsync();
                            await writer.WriteEndElementAsync();
                        }
                    }
                }
            }
        }
        private void SetComparativeBaseAttributes(Calculator1 baseObject, int colCount,
            XmlWriter writer)
        {
            //need some attributes without attname extension
            writer.WriteAttributeString(Calculator1.cId, baseObject.Id.ToString());
            writer.WriteAttributeString(Calculator1.cName, baseObject.Name.ToString());
            //column count convention
            writer.WriteAttributeString("Files", colCount.ToString());
            //analyzer type
            writer.WriteAttributeString(Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //set the fn options 
            SetSBAttributes(writer);
        }
        private void SetSiblingComparativeBaseProperties(Calculator1 baseObject, string attNameExt,
            XmlWriter writer)
        {
            //sibling names, labels, dates needed
            writer.WriteAttributeString(string.Concat(Calculator1.cId, attNameExt), baseObject.Id.ToString());
            writer.WriteAttributeString(string.Concat(Calculator1.cName, attNameExt), baseObject.Name.ToString());
            writer.WriteAttributeString(string.Concat(Calculator1.cLabel, attNameExt), baseObject.Label.ToString());
            writer.WriteAttributeString(string.Concat(Calculator1.cDate, attNameExt), baseObject.Date.ToString());
        }
        public void SetSBAttributes(XmlWriter writer)
        {
            if (this.GCCalculatorParams.UrisToAnalyze != null)
            {
                int iSBCount = 0;
                foreach (var sb in this.GCCalculatorParams.UrisToAnalyze)
                {
                    if (sb == SB1Base.cSB1Label1)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label1, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label2)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label2, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label3)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label3, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label4)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label4, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label5)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label5, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label6)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label6, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label7)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label7, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label8)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label8, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label9)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label9, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label10)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label10, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label11)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label11, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label12)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label12, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label13)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label13, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label14)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label14, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label15)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label15, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label16)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label16, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label17)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label17, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label18)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label18, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label19)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label19, "true");
                        iSBCount++;
                    }
                    else if (sb == SB1Base.cSB1Label20)
                    {
                        writer.WriteAttributeString(
                            SB1Base.cSB1Label20, "true");
                        iSBCount++;
                    }
                }
                //tells ss how many to display
                //writer.WriteAttributeString(
                //    SB1Base.cSBCount, iSBCount.ToString());
            }
        }

        public void SetRowCount(XmlWriter writer)
        {
            if (this.RowCount > 0)
            {
                writer.WriteAttributeString(
                  SB1Base.cSBCount, this.RowCount.ToString());
            }
            else
            {
                if (this.GCCalculatorParams.UrisToAnalyze != null)
                {
                    int iSBCount = 0;
                    foreach (var sb in this.GCCalculatorParams.UrisToAnalyze)
                    {
                        if (sb == SB1Base.cSB1Label1)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label2)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label3)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label4)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label5)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label6)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label7)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label8)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label9)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label10)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label11)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label12)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label13)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label14)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label15)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label16)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label17)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label18)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label19)
                        {
                            iSBCount++;
                        }
                        else if (sb == SB1Base.cSB1Label20)
                        {
                            iSBCount++;
                        }
                    }
                    writer.WriteAttributeString(
                            SB1Base.cSBCount, iSBCount.ToString());
                }
            }
            //row count set by actual indicators found
            this.RowCount = 0;
        }

    }
}

