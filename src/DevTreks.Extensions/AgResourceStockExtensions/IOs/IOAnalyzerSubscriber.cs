using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		This class derives from the CalculatorContracts.InputOutputCalculator 
    ///             class. It subscribes to the RunCalculation event raised by that 
    ///             class. It runs statistical calculations on the nodes returned 
    ///             by that class, and passes back the calculated nodes to the 
    ///             publishing class.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class IOAnalyzerSubscriber : InputOutputCalculator
    {
        //constructors
        public IOAnalyzerSubscriber() { }
        //constructor sets class properties
        public IOAnalyzerSubscriber(CalculatorParameters calcParameters,
            ARSAnalyzerHelper.ANALYZER_TYPES analyzerType)
            : base(calcParameters)
        {
            this.AnalyzerType = analyzerType;
        }

        //properties
        //analyzers specific to this extension
        public ARSAnalyzerHelper.ANALYZER_TYPES AnalyzerType { get; set; }
        //analyzers that need persistent state for a full observation doc
        CostEffectivenessAnalyzer1 ceAnalyzer { get; set; }

        public bool RunAnalyzer()
        {
            bool bHasAnalysis = false;
            //subscribe to the event raised by the publisher delegate
            this.RunCalculation += AddCalculations;
            //run the analyses (raising the RunCalculation event 
            //from the base event publisher for each node)
            bHasAnalysis = this.StreamAndSaveCalculation();
            return bHasAnalysis;
        }
        public void AddCalculations(object sender, CustomEventArgs e)
        {
            //pass a byref xelement from the publisher's data
            XElement statElement = null;
            if (e.CurrentElement != null)
                statElement = new XElement(e.CurrentElement);
            //run the stats and add them to statelement
            e.HasCalculations = RunInputOutputStatistic(
                ref statElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            if (statElement != null)
                e.CurrentElement = new XElement(statElement);
        }
        
        private bool RunInputOutputStatistic(ref XElement currentElement)
        {
            bool bHasStatistics = false;
            if (this.AnalyzerType
                == ARSAnalyzerHelper.ANALYZER_TYPES.statistics01)
            {
                StatisticalAnalyzer1 statsAnalyzer1
                    = new StatisticalAnalyzer1(this.GCCalculatorParams);
                bHasStatistics = statsAnalyzer1.SetStatisticalCalculation(
                    ref currentElement);
                statsAnalyzer1 = null;
            }
            else if (ARSAnalyzerHelper.IsEffectivenessAnalysis(
                this.AnalyzerType))
            {
                if (this.ceAnalyzer == null)
                {
                    this.ceAnalyzer
                        = new CostEffectivenessAnalyzer1(this.GCCalculatorParams);
                }
                bHasStatistics = SetEffectivenessStatistic(ref currentElement);
            }
            else
            {
                this.GCCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOANALYZER");
            }
            return bHasStatistics;
        }

        public bool SetEffectivenessStatistic(ref XElement currentElement)
        {
            bool bHasStatistics = false;
            if (currentElement.HasAttributes)
            {
                //refactor: is a new stats analyzer needed by every node?
                StatisticalAnalyzer1 statsAnalyzer2
                    = new StatisticalAnalyzer1(this.GCCalculatorParams);
                //run the regular mean and standard deviation statistics
                bHasStatistics = statsAnalyzer2.SetStatisticalCalculation(
                    ref currentElement);
                if (bHasStatistics)
                {
                    //run the cost effectiveness statistic
                    bHasStatistics = this.ceAnalyzer
                        .SetCostEffectivenessCalculation(ref currentElement);
                }
                statsAnalyzer2 = null;
            }
            else
            {
                //grouping nodes sometimes need to manipulate children for display
                bHasStatistics = this.ceAnalyzer.SetCostEffectivenessCalculation(
                       ref currentElement);
            }
            return bHasStatistics;
        }
    }
}
