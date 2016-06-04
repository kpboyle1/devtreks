using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Typical Object model: 
    ///             The class measures planned vs actual progress.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. 
    ///</summary>
    public class SB1Progress1 : SB1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public SB1Progress1(CalculatorParameters calcs)
            : base()
        {
            //subprice object
            InitTotalSB1Progress1Properties(this, calcs);
        }
        //copy constructor
        public SB1Progress1(SB1Progress1 calculator)
            : base(calculator)
        {
            CopyTotalSB1Progress1Properties(this, calculator);
        }
        #region "properties"
        //planned full (sum of all planning periods)
        public double TotalSBScorePFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSBScorePCTotal { get; set; }
        //actual period
        public double TotalSBScoreAPTotal { get; set; }
        //actual cumulative 
        public double TotalSBScoreACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSBScoreAPChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSBScoreACChange { get; set; }
        //planned period
        public double TotalSBScorePPPercent { get; set; }
        //planned cumulative
        public double TotalSBScorePCPercent { get; set; }
        //planned full
        public double TotalSBScorePFPercent { get; set; }

        private const string cTotalSBScorePFTotal = "TSBScorePFTotal";
        private const string cTotalSBScorePCTotal = "TSBScorePCTotal";
        private const string cTotalSBScoreAPTotal = "TSBScoreAPTotal";
        private const string cTotalSBScoreACTotal = "TSBScoreACTotal";
        private const string cTotalSBScoreAPChange = "TSBScoreAPChange";
        private const string cTotalSBScoreACChange = "TSBScoreACChange";
        private const string cTotalSBScorePPPercent = "TSBScorePPPercent";
        private const string cTotalSBScorePCPercent = "TSBScorePCPercent";
        private const string cTotalSBScorePFPercent = "TSBScorePFPercent";

        //planned full (sum of all planning periods)
        public double TotalSBScoreLPFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSBScoreLPCTotal { get; set; }
        //actual period
        public double TotalSBScoreLAPTotal { get; set; }
        //actual cumulative 
        public double TotalSBScoreLACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSBScoreLAPChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSBScoreLACChange { get; set; }
        //planned period
        public double TotalSBScoreLPPPercent { get; set; }
        //planned cumulative
        public double TotalSBScoreLPCPercent { get; set; }
        //planned full
        public double TotalSBScoreLPFPercent { get; set; }

        private const string cTotalSBScoreLPFTotal = "TSBScoreLPFTotal";
        private const string cTotalSBScoreLPCTotal = "TSBScoreLPCTotal";
        private const string cTotalSBScoreLAPTotal = "TSBScoreLAPTotal";
        private const string cTotalSBScoreLACTotal = "TSBScoreLACTotal";
        private const string cTotalSBScoreLAPChange = "TSBScoreLAPChange";
        private const string cTotalSBScoreLACChange = "TSBScoreLACChange";
        private const string cTotalSBScoreLPPPercent = "TSBScoreLPPPercent";
        private const string cTotalSBScoreLPCPercent = "TSBScoreLPCPercent";
        private const string cTotalSBScoreLPFPercent = "TSBScoreLPFPercent";

        //planned full (sum of all planning periods)
        public double TotalSBScoreUPFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSBScoreUPCTotal { get; set; }
        //actual period
        public double TotalSBScoreUAPTotal { get; set; }
        //actual cumulative 
        public double TotalSBScoreUACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSBScoreUAPChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSBScoreUACChange { get; set; }
        //planned period
        public double TotalSBScoreUPPPercent { get; set; }
        //planned cumulative
        public double TotalSBScoreUPCPercent { get; set; }
        //planned full
        public double TotalSBScoreUPFPercent { get; set; }

        private const string cTotalSBScoreUPFTotal = "TSBScoreUPFTotal";
        private const string cTotalSBScoreUPCTotal = "TSBScoreUPCTotal";
        private const string cTotalSBScoreUAPTotal = "TSBScoreUAPTotal";
        private const string cTotalSBScoreUACTotal = "TSBScoreUACTotal";
        private const string cTotalSBScoreUAPChange = "TSBScoreUAPChange";
        private const string cTotalSBScoreUACChange = "TSBScoreUACChange";
        private const string cTotalSBScoreUPPPercent = "TSBScoreUPPPercent";
        private const string cTotalSBScoreUPCPercent = "TSBScoreUPCPercent";
        private const string cTotalSBScoreUPFPercent = "TSBScoreUPFPercent";


        //planned full (sum of all planning periods)
        public double TotalSB1PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB1PCTotal { get; set; }
        //actual period
        public double TotalSB1APTotal { get; set; }
        //actual cumulative 
        public double TotalSB1ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB1APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB1ACChange { get; set; }
        //planned period
        public double TotalSB1PPPercent { get; set; }
        //planned cumulative
        public double TotalSB1PCPercent { get; set; }
        //planned full
        public double TotalSB1PFPercent { get; set; }

        private const string cTotalSB1PFTotal = "TSB1PFTotal";
        private const string cTotalSB1PCTotal = "TSB1PCTotal";
        private const string cTotalSB1APTotal = "TSB1APTotal";
        private const string cTotalSB1ACTotal = "TSB1ACTotal";
        private const string cTotalSB1APChange = "TSB1APChange";
        private const string cTotalSB1ACChange = "TSB1ACChange";
        private const string cTotalSB1PPPercent = "TSB1PPPercent";
        private const string cTotalSB1PCPercent = "TSB1PCPercent";
        private const string cTotalSB1PFPercent = "TSB1PFPercent";

        //planned full (sum of all planning periods)
        public double TotalSB2PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB2PCTotal { get; set; }
        //actual period
        public double TotalSB2APTotal { get; set; }
        //actual cumulative 
        public double TotalSB2ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB2APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB2ACChange { get; set; }
        //planned period
        public double TotalSB2PPPercent { get; set; }
        //planned cumulative
        public double TotalSB2PCPercent { get; set; }
        //planned full
        public double TotalSB2PFPercent { get; set; }

        private const string cTotalSB2PFTotal = "TSB2PFTotal";
        private const string cTotalSB2PCTotal = "TSB2PCTotal";
        private const string cTotalSB2APTotal = "TSB2APTotal";
        private const string cTotalSB2ACTotal = "TSB2ACTotal";
        private const string cTotalSB2APChange = "TSB2APChange";
        private const string cTotalSB2ACChange = "TSB2ACChange";
        private const string cTotalSB2PPPercent = "TSB2PPPercent";
        private const string cTotalSB2PCPercent = "TSB2PCPercent";
        private const string cTotalSB2PFPercent = "TSB2PFPercent";

        public double TotalSB3PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB3PCTotal { get; set; }
        //actual period
        public double TotalSB3APTotal { get; set; }
        //actual cumulative 
        public double TotalSB3ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB3APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB3ACChange { get; set; }
        //planned period
        public double TotalSB3PPPercent { get; set; }
        //planned cumulative
        public double TotalSB3PCPercent { get; set; }
        //planned full
        public double TotalSB3PFPercent { get; set; }

        private const string cTotalSB3PFTotal = "TSB3PFTotal";
        private const string cTotalSB3PCTotal = "TSB3PCTotal";
        private const string cTotalSB3APTotal = "TSB3APTotal";
        private const string cTotalSB3ACTotal = "TSB3ACTotal";
        private const string cTotalSB3APChange = "TSB3APChange";
        private const string cTotalSB3ACChange = "TSB3ACChange";
        private const string cTotalSB3PPPercent = "TSB3PPPercent";
        private const string cTotalSB3PCPercent = "TSB3PCPercent";
        private const string cTotalSB3PFPercent = "TSB3PFPercent";

        public double TotalSB4PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB4PCTotal { get; set; }
        //actual period
        public double TotalSB4APTotal { get; set; }
        //actual cumulative 
        public double TotalSB4ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB4APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB4ACChange { get; set; }
        //planned period
        public double TotalSB4PPPercent { get; set; }
        //planned cumulative
        public double TotalSB4PCPercent { get; set; }
        //planned full
        public double TotalSB4PFPercent { get; set; }

        private const string cTotalSB4PFTotal = "TSB4PFTotal";
        private const string cTotalSB4PCTotal = "TSB4PCTotal";
        private const string cTotalSB4APTotal = "TSB4APTotal";
        private const string cTotalSB4ACTotal = "TSB4ACTotal";
        private const string cTotalSB4APChange = "TSB4APChange";
        private const string cTotalSB4ACChange = "TSB4ACChange";
        private const string cTotalSB4PPPercent = "TSB4PPPercent";
        private const string cTotalSB4PCPercent = "TSB4PCPercent";
        private const string cTotalSB4PFPercent = "TSB4PFPercent";

        public double TotalSB5PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB5PCTotal { get; set; }
        //actual period
        public double TotalSB5APTotal { get; set; }
        //actual cumulative 
        public double TotalSB5ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB5APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB5ACChange { get; set; }
        //planned period
        public double TotalSB5PPPercent { get; set; }
        //planned cumulative
        public double TotalSB5PCPercent { get; set; }
        //planned full
        public double TotalSB5PFPercent { get; set; }

        private const string cTotalSB5PFTotal = "TSB5PFTotal";
        private const string cTotalSB5PCTotal = "TSB5PCTotal";
        private const string cTotalSB5APTotal = "TSB5APTotal";
        private const string cTotalSB5ACTotal = "TSB5ACTotal";
        private const string cTotalSB5APChange = "TSB5APChange";
        private const string cTotalSB5ACChange = "TSB5ACChange";
        private const string cTotalSB5PPPercent = "TSB5PPPercent";
        private const string cTotalSB5PCPercent = "TSB5PCPercent";
        private const string cTotalSB5PFPercent = "TSB5PFPercent";

        public double TotalSB6PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB6PCTotal { get; set; }
        //actual period
        public double TotalSB6APTotal { get; set; }
        //actual cumulative 
        public double TotalSB6ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB6APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB6ACChange { get; set; }
        //planned period
        public double TotalSB6PPPercent { get; set; }
        //planned cumulative
        public double TotalSB6PCPercent { get; set; }
        //planned full
        public double TotalSB6PFPercent { get; set; }

        private const string cTotalSB6PFTotal = "TSB6PFTotal";
        private const string cTotalSB6PCTotal = "TSB6PCTotal";
        private const string cTotalSB6APTotal = "TSB6APTotal";
        private const string cTotalSB6ACTotal = "TSB6ACTotal";
        private const string cTotalSB6APChange = "TSB6APChange";
        private const string cTotalSB6ACChange = "TSB6ACChange";
        private const string cTotalSB6PPPercent = "TSB6PPPercent";
        private const string cTotalSB6PCPercent = "TSB6PCPercent";
        private const string cTotalSB6PFPercent = "TSB6PFPercent";

        public double TotalSB7PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB7PCTotal { get; set; }
        //actual period
        public double TotalSB7APTotal { get; set; }
        //actual cumulative 
        public double TotalSB7ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB7APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB7ACChange { get; set; }
        //planned period
        public double TotalSB7PPPercent { get; set; }
        //planned cumulative
        public double TotalSB7PCPercent { get; set; }
        //planned full
        public double TotalSB7PFPercent { get; set; }

        private const string cTotalSB7PFTotal = "TSB7PFTotal";
        private const string cTotalSB7PCTotal = "TSB7PCTotal";
        private const string cTotalSB7APTotal = "TSB7APTotal";
        private const string cTotalSB7ACTotal = "TSB7ACTotal";
        private const string cTotalSB7APChange = "TSB7APChange";
        private const string cTotalSB7ACChange = "TSB7ACChange";
        private const string cTotalSB7PPPercent = "TSB7PPPercent";
        private const string cTotalSB7PCPercent = "TSB7PCPercent";
        private const string cTotalSB7PFPercent = "TSB7PFPercent";

        public double TotalSB8PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB8PCTotal { get; set; }
        //actual period
        public double TotalSB8APTotal { get; set; }
        //actual cumulative 
        public double TotalSB8ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB8APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB8ACChange { get; set; }
        //planned period
        public double TotalSB8PPPercent { get; set; }
        //planned cumulative
        public double TotalSB8PCPercent { get; set; }
        //planned full
        public double TotalSB8PFPercent { get; set; }

        private const string cTotalSB8PFTotal = "TSB8PFTotal";
        private const string cTotalSB8PCTotal = "TSB8PCTotal";
        private const string cTotalSB8APTotal = "TSB8APTotal";
        private const string cTotalSB8ACTotal = "TSB8ACTotal";
        private const string cTotalSB8APChange = "TSB8APChange";
        private const string cTotalSB8ACChange = "TSB8ACChange";
        private const string cTotalSB8PPPercent = "TSB8PPPercent";
        private const string cTotalSB8PCPercent = "TSB8PCPercent";
        private const string cTotalSB8PFPercent = "TSB8PFPercent";

        public double TotalSB9PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB9PCTotal { get; set; }
        //actual period
        public double TotalSB9APTotal { get; set; }
        //actual cumulative 
        public double TotalSB9ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB9APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB9ACChange { get; set; }
        //planned period
        public double TotalSB9PPPercent { get; set; }
        //planned cumulative
        public double TotalSB9PCPercent { get; set; }
        //planned full
        public double TotalSB9PFPercent { get; set; }

        private const string cTotalSB9PFTotal = "TSB9PFTotal";
        private const string cTotalSB9PCTotal = "TSB9PCTotal";
        private const string cTotalSB9APTotal = "TSB9APTotal";
        private const string cTotalSB9ACTotal = "TSB9ACTotal";
        private const string cTotalSB9APChange = "TSB9APChange";
        private const string cTotalSB9ACChange = "TSB9ACChange";
        private const string cTotalSB9PPPercent = "TSB9PPPercent";
        private const string cTotalSB9PCPercent = "TSB9PCPercent";
        private const string cTotalSB9PFPercent = "TSB9PFPercent";

        public double TotalSB10PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalSB10PCTotal { get; set; }
        //actual period
        public double TotalSB10APTotal { get; set; }
        //actual cumulative 
        public double TotalSB10ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalSB10APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalSB10ACChange { get; set; }
        //planned period
        public double TotalSB10PPPercent { get; set; }
        //planned cumulative
        public double TotalSB10PCPercent { get; set; }
        //planned full
        public double TotalSB10PFPercent { get; set; }

        private const string cTotalSB10PFTotal = "TSB10PFTotal";
        private const string cTotalSB10PCTotal = "TSB10PCTotal";
        private const string cTotalSB10APTotal = "TSB10APTotal";
        private const string cTotalSB10ACTotal = "TSB10ACTotal";
        private const string cTotalSB10APChange = "TSB10APChange";
        private const string cTotalSB10ACChange = "TSB10ACChange";
        private const string cTotalSB10PPPercent = "TSB10PPPercent";
        private const string cTotalSB10PCPercent = "TSB10PCPercent";
        private const string cTotalSB10PFPercent = "TSB10PFPercent";

        
        public void InitTotalSB1Progress1Properties(SB1Progress1 ind, CalculatorParameters calcs)
        {
            //avoid nulls
            InitSB1AnalysisProperties(ind, calcs);
            ind.ErrorMessage = string.Empty;

            ind.TotalSBScorePFTotal = 0;
            ind.TotalSBScorePCTotal = 0;
            ind.TotalSBScoreAPTotal = 0;
            ind.TotalSBScoreACTotal = 0;
            ind.TotalSBScoreAPChange = 0;
            ind.TotalSBScoreACChange = 0;
            ind.TotalSBScorePPPercent = 0;
            ind.TotalSBScorePCPercent = 0;
            ind.TotalSBScorePFPercent = 0;

            ind.TotalSBScoreLPFTotal = 0;
            ind.TotalSBScoreLPCTotal = 0;
            ind.TotalSBScoreLAPTotal = 0;
            ind.TotalSBScoreLACTotal = 0;
            ind.TotalSBScoreLAPChange = 0;
            ind.TotalSBScoreLACChange = 0;
            ind.TotalSBScoreLPPPercent = 0;
            ind.TotalSBScoreLPCPercent = 0;
            ind.TotalSBScoreLPFPercent = 0;

            ind.TotalSBScoreUPFTotal = 0;
            ind.TotalSBScoreUPCTotal = 0;
            ind.TotalSBScoreUAPTotal = 0;
            ind.TotalSBScoreUACTotal = 0;
            ind.TotalSBScoreUAPChange = 0;
            ind.TotalSBScoreUACChange = 0;
            ind.TotalSBScoreUPPPercent = 0;
            ind.TotalSBScoreUPCPercent = 0;
            ind.TotalSBScoreUPFPercent = 0;

            ind.TotalSB1PFTotal = 0;
            ind.TotalSB1PCTotal = 0;
            ind.TotalSB1APTotal = 0;
            ind.TotalSB1ACTotal = 0;
            ind.TotalSB1APChange = 0;
            ind.TotalSB1ACChange = 0;
            ind.TotalSB1PPPercent = 0;
            ind.TotalSB1PCPercent = 0;
            ind.TotalSB1PFPercent = 0;

            ind.TotalSB2PFTotal = 0;
            ind.TotalSB2PCTotal = 0;
            ind.TotalSB2APTotal = 0;
            ind.TotalSB2ACTotal = 0;
            ind.TotalSB2APChange = 0;
            ind.TotalSB2ACChange = 0;
            ind.TotalSB2PPPercent = 0;
            ind.TotalSB2PCPercent = 0;
            ind.TotalSB2PFPercent = 0;

            ind.TotalSB3PFTotal = 0;
            ind.TotalSB3PCTotal = 0;
            ind.TotalSB3APTotal = 0;
            ind.TotalSB3ACTotal = 0;
            ind.TotalSB3APChange = 0;
            ind.TotalSB3ACChange = 0;
            ind.TotalSB3PPPercent = 0;
            ind.TotalSB3PCPercent = 0;
            ind.TotalSB3PFPercent = 0;

            ind.TotalSB4PFTotal = 0;
            ind.TotalSB4PCTotal = 0;
            ind.TotalSB4APTotal = 0;
            ind.TotalSB4ACTotal = 0;
            ind.TotalSB4APChange = 0;
            ind.TotalSB4ACChange = 0;
            ind.TotalSB4PPPercent = 0;
            ind.TotalSB4PCPercent = 0;
            ind.TotalSB4PFPercent = 0;

            ind.TotalSB5PFTotal = 0;
            ind.TotalSB5PCTotal = 0;
            ind.TotalSB5APTotal = 0;
            ind.TotalSB5ACTotal = 0;
            ind.TotalSB5APChange = 0;
            ind.TotalSB5ACChange = 0;
            ind.TotalSB5PPPercent = 0;
            ind.TotalSB5PCPercent = 0;
            ind.TotalSB5PFPercent = 0;

            ind.TotalSB6PFTotal = 0;
            ind.TotalSB6PCTotal = 0;
            ind.TotalSB6APTotal = 0;
            ind.TotalSB6ACTotal = 0;
            ind.TotalSB6APChange = 0;
            ind.TotalSB6ACChange = 0;
            ind.TotalSB6PPPercent = 0;
            ind.TotalSB6PCPercent = 0;
            ind.TotalSB6PFPercent = 0;

            ind.TotalSB7PFTotal = 0;
            ind.TotalSB7PCTotal = 0;
            ind.TotalSB7APTotal = 0;
            ind.TotalSB7ACTotal = 0;
            ind.TotalSB7APChange = 0;
            ind.TotalSB7ACChange = 0;
            ind.TotalSB7PPPercent = 0;
            ind.TotalSB7PCPercent = 0;
            ind.TotalSB7PFPercent = 0;

            ind.TotalSB8PFTotal = 0;
            ind.TotalSB8PCTotal = 0;
            ind.TotalSB8APTotal = 0;
            ind.TotalSB8ACTotal = 0;
            ind.TotalSB8APChange = 0;
            ind.TotalSB8ACChange = 0;
            ind.TotalSB8PPPercent = 0;
            ind.TotalSB8PCPercent = 0;
            ind.TotalSB8PFPercent = 0;

            ind.TotalSB9PFTotal = 0;
            ind.TotalSB9PCTotal = 0;
            ind.TotalSB9APTotal = 0;
            ind.TotalSB9ACTotal = 0;
            ind.TotalSB9APChange = 0;
            ind.TotalSB9ACChange = 0;
            ind.TotalSB9PPPercent = 0;
            ind.TotalSB9PCPercent = 0;
            ind.TotalSB9PFPercent = 0;

            ind.TotalSB10PFTotal = 0;
            ind.TotalSB10PCTotal = 0;
            ind.TotalSB10APTotal = 0;
            ind.TotalSB10ACTotal = 0;
            ind.TotalSB10APChange = 0;
            ind.TotalSB10ACChange = 0;
            ind.TotalSB10PPPercent = 0;
            ind.TotalSB10PCPercent = 0;
            ind.TotalSB10PFPercent = 0;

            
            ind.CalcParameters = calcs;
            ind.SB11Stock = new SB101Stock();
            ind.SB12Stock = new SB102Stock();
        }

        public void CopyTotalSB1Progress1Properties(SB1Progress1 ind,
            SB1Progress1 calculator)
        {
            //avoid nulls
            CopySB1AnalysisProperties(ind, calculator);
            ind.ErrorMessage = calculator.ErrorMessage;

            ind.TotalSBScorePFTotal = calculator.TotalSBScorePFTotal;
            ind.TotalSBScorePCTotal = calculator.TotalSBScorePCTotal;
            ind.TotalSBScoreAPTotal = calculator.TotalSBScoreAPTotal;
            ind.TotalSBScoreACTotal = calculator.TotalSBScoreACTotal;
            ind.TotalSBScoreAPChange = calculator.TotalSBScoreAPChange;
            ind.TotalSBScoreACChange = calculator.TotalSBScoreACChange;
            ind.TotalSBScorePPPercent = calculator.TotalSBScorePPPercent;
            ind.TotalSBScorePCPercent = calculator.TotalSBScorePCPercent;
            ind.TotalSBScorePFPercent = calculator.TotalSBScorePFPercent;

            ind.TotalSBScoreLPFTotal = calculator.TotalSBScoreLPFTotal;
            ind.TotalSBScoreLPCTotal = calculator.TotalSBScoreLPCTotal;
            ind.TotalSBScoreLAPTotal = calculator.TotalSBScoreLAPTotal;
            ind.TotalSBScoreLACTotal = calculator.TotalSBScoreLACTotal;
            ind.TotalSBScoreLAPChange = calculator.TotalSBScoreLAPChange;
            ind.TotalSBScoreLACChange = calculator.TotalSBScoreLACChange;
            ind.TotalSBScoreLPPPercent = calculator.TotalSBScoreLPPPercent;
            ind.TotalSBScoreLPCPercent = calculator.TotalSBScoreLPCPercent;
            ind.TotalSBScoreLPFPercent = calculator.TotalSBScoreLPFPercent;

            ind.TotalSBScoreUPFTotal = calculator.TotalSBScoreUPFTotal;
            ind.TotalSBScoreUPCTotal = calculator.TotalSBScoreUPCTotal;
            ind.TotalSBScoreUAPTotal = calculator.TotalSBScoreUAPTotal;
            ind.TotalSBScoreUACTotal = calculator.TotalSBScoreUACTotal;
            ind.TotalSBScoreUAPChange = calculator.TotalSBScoreUAPChange;
            ind.TotalSBScoreUACChange = calculator.TotalSBScoreUACChange;
            ind.TotalSBScoreUPPPercent = calculator.TotalSBScoreUPPPercent;
            ind.TotalSBScoreUPCPercent = calculator.TotalSBScoreUPCPercent;
            ind.TotalSBScoreUPFPercent = calculator.TotalSBScoreUPFPercent;

            ind.TotalSB1PFTotal = calculator.TotalSB1PFTotal;
            ind.TotalSB1PCTotal = calculator.TotalSB1PCTotal;
            ind.TotalSB1APTotal = calculator.TotalSB1APTotal;
            ind.TotalSB1ACTotal = calculator.TotalSB1ACTotal;
            ind.TotalSB1APChange = calculator.TotalSB1APChange;
            ind.TotalSB1ACChange = calculator.TotalSB1ACChange;
            ind.TotalSB1PPPercent = calculator.TotalSB1PPPercent;
            ind.TotalSB1PCPercent = calculator.TotalSB1PCPercent;
            ind.TotalSB1PFPercent = calculator.TotalSB1PFPercent;

            ind.TotalSB2PFTotal = calculator.TotalSB2PFTotal;
            ind.TotalSB2PCTotal = calculator.TotalSB2PCTotal;
            ind.TotalSB2APTotal = calculator.TotalSB2APTotal;
            ind.TotalSB2ACTotal = calculator.TotalSB2ACTotal;
            ind.TotalSB2APChange = calculator.TotalSB2APChange;
            ind.TotalSB2ACChange = calculator.TotalSB2ACChange;
            ind.TotalSB2PPPercent = calculator.TotalSB2PPPercent;
            ind.TotalSB2PCPercent = calculator.TotalSB2PCPercent;
            ind.TotalSB2PFPercent = calculator.TotalSB2PFPercent;

            ind.TotalSB3PFTotal = calculator.TotalSB3PFTotal;
            ind.TotalSB3PCTotal = calculator.TotalSB3PCTotal;
            ind.TotalSB3APTotal = calculator.TotalSB3APTotal;
            ind.TotalSB3ACTotal = calculator.TotalSB3ACTotal;
            ind.TotalSB3APChange = calculator.TotalSB3APChange;
            ind.TotalSB3ACChange = calculator.TotalSB3ACChange;
            ind.TotalSB3PPPercent = calculator.TotalSB3PPPercent;
            ind.TotalSB3PCPercent = calculator.TotalSB3PCPercent;
            ind.TotalSB3PFPercent = calculator.TotalSB3PFPercent;

            ind.TotalSB4PFTotal = calculator.TotalSB4PFTotal;
            ind.TotalSB4PCTotal = calculator.TotalSB4PCTotal;
            ind.TotalSB4APTotal = calculator.TotalSB4APTotal;
            ind.TotalSB4ACTotal = calculator.TotalSB4ACTotal;
            ind.TotalSB4APChange = calculator.TotalSB4APChange;
            ind.TotalSB4ACChange = calculator.TotalSB4ACChange;
            ind.TotalSB4PPPercent = calculator.TotalSB4PPPercent;
            ind.TotalSB4PCPercent = calculator.TotalSB4PCPercent;
            ind.TotalSB4PFPercent = calculator.TotalSB4PFPercent;

            ind.TotalSB5PFTotal = calculator.TotalSB5PFTotal;
            ind.TotalSB5PCTotal = calculator.TotalSB5PCTotal;
            ind.TotalSB5APTotal = calculator.TotalSB5APTotal;
            ind.TotalSB5ACTotal = calculator.TotalSB5ACTotal;
            ind.TotalSB5APChange = calculator.TotalSB5APChange;
            ind.TotalSB5ACChange = calculator.TotalSB5ACChange;
            ind.TotalSB5PPPercent = calculator.TotalSB5PPPercent;
            ind.TotalSB5PCPercent = calculator.TotalSB5PCPercent;
            ind.TotalSB5PFPercent = calculator.TotalSB5PFPercent;

            ind.TotalSB6PFTotal = calculator.TotalSB6PFTotal;
            ind.TotalSB6PCTotal = calculator.TotalSB6PCTotal;
            ind.TotalSB6APTotal = calculator.TotalSB6APTotal;
            ind.TotalSB6ACTotal = calculator.TotalSB6ACTotal;
            ind.TotalSB6APChange = calculator.TotalSB6APChange;
            ind.TotalSB6ACChange = calculator.TotalSB6ACChange;
            ind.TotalSB6PPPercent = calculator.TotalSB6PPPercent;
            ind.TotalSB6PCPercent = calculator.TotalSB6PCPercent;
            ind.TotalSB6PFPercent = calculator.TotalSB6PFPercent;

            ind.TotalSB7PFTotal = calculator.TotalSB7PFTotal;
            ind.TotalSB7PCTotal = calculator.TotalSB7PCTotal;
            ind.TotalSB7APTotal = calculator.TotalSB7APTotal;
            ind.TotalSB7ACTotal = calculator.TotalSB7ACTotal;
            ind.TotalSB7APChange = calculator.TotalSB7APChange;
            ind.TotalSB7ACChange = calculator.TotalSB7ACChange;
            ind.TotalSB7PPPercent = calculator.TotalSB7PPPercent;
            ind.TotalSB7PCPercent = calculator.TotalSB7PCPercent;
            ind.TotalSB7PFPercent = calculator.TotalSB7PFPercent;

            ind.TotalSB8PFTotal = calculator.TotalSB8PFTotal;
            ind.TotalSB8PCTotal = calculator.TotalSB8PCTotal;
            ind.TotalSB8APTotal = calculator.TotalSB8APTotal;
            ind.TotalSB8ACTotal = calculator.TotalSB8ACTotal;
            ind.TotalSB8APChange = calculator.TotalSB8APChange;
            ind.TotalSB8ACChange = calculator.TotalSB8ACChange;
            ind.TotalSB8PPPercent = calculator.TotalSB8PPPercent;
            ind.TotalSB8PCPercent = calculator.TotalSB8PCPercent;
            ind.TotalSB8PFPercent = calculator.TotalSB8PFPercent;

            ind.TotalSB9PFTotal = calculator.TotalSB9PFTotal;
            ind.TotalSB9PCTotal = calculator.TotalSB9PCTotal;
            ind.TotalSB9APTotal = calculator.TotalSB9APTotal;
            ind.TotalSB9ACTotal = calculator.TotalSB9ACTotal;
            ind.TotalSB9APChange = calculator.TotalSB9APChange;
            ind.TotalSB9ACChange = calculator.TotalSB9ACChange;
            ind.TotalSB9PPPercent = calculator.TotalSB9PPPercent;
            ind.TotalSB9PCPercent = calculator.TotalSB9PCPercent;
            ind.TotalSB9PFPercent = calculator.TotalSB9PFPercent;

            ind.TotalSB10PFTotal = calculator.TotalSB10PFTotal;
            ind.TotalSB10PCTotal = calculator.TotalSB10PCTotal;
            ind.TotalSB10APTotal = calculator.TotalSB10APTotal;
            ind.TotalSB10ACTotal = calculator.TotalSB10ACTotal;
            ind.TotalSB10APChange = calculator.TotalSB10APChange;
            ind.TotalSB10ACChange = calculator.TotalSB10ACChange;
            ind.TotalSB10PPPercent = calculator.TotalSB10PPPercent;
            ind.TotalSB10PCPercent = calculator.TotalSB10PCPercent;
            ind.TotalSB10PFPercent = calculator.TotalSB10PFPercent;
        }

        public void SetTotalSB1Progress1Properties(SB1Progress1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalSBScorePFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScorePFTotal, attNameExtension));
            ind.TotalSBScorePCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScorePCTotal, attNameExtension));
            ind.TotalSBScoreAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreAPTotal, attNameExtension));
            ind.TotalSBScoreACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreACTotal, attNameExtension));
            ind.TotalSBScoreAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreAPChange, attNameExtension));
            ind.TotalSBScoreACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreACChange, attNameExtension));
            ind.TotalSBScorePPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScorePPPercent, attNameExtension));
            ind.TotalSBScorePCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScorePCPercent, attNameExtension));
            ind.TotalSBScorePFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScorePFPercent, attNameExtension));

            ind.TotalSBScoreLPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLPFTotal, attNameExtension));
            ind.TotalSBScoreLPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLPCTotal, attNameExtension));
            ind.TotalSBScoreLAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLAPTotal, attNameExtension));
            ind.TotalSBScoreLACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLACTotal, attNameExtension));
            ind.TotalSBScoreLAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLAPChange, attNameExtension));
            ind.TotalSBScoreLACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLACChange, attNameExtension));
            ind.TotalSBScoreLPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLPPPercent, attNameExtension));
            ind.TotalSBScoreLPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLPCPercent, attNameExtension));
            ind.TotalSBScoreLPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLPFPercent, attNameExtension));

            ind.TotalSBScoreUPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUPFTotal, attNameExtension));
            ind.TotalSBScoreUPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUPCTotal, attNameExtension));
            ind.TotalSBScoreUAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUAPTotal, attNameExtension));
            ind.TotalSBScoreUACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUACTotal, attNameExtension));
            ind.TotalSBScoreUAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUAPChange, attNameExtension));
            ind.TotalSBScoreUACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUACChange, attNameExtension));
            ind.TotalSBScoreUPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUPPPercent, attNameExtension));
            ind.TotalSBScoreUPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUPCPercent, attNameExtension));
            ind.TotalSBScoreUPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUPFPercent, attNameExtension));


            ind.TotalSB1PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1PFTotal, attNameExtension));
            ind.TotalSB1PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1PCTotal, attNameExtension));
            ind.TotalSB1APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1APTotal, attNameExtension));
            ind.TotalSB1ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1ACTotal, attNameExtension));
            ind.TotalSB1APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1APChange, attNameExtension));
            ind.TotalSB1ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1ACChange, attNameExtension));
            ind.TotalSB1PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1PPPercent, attNameExtension));
            ind.TotalSB1PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1PCPercent, attNameExtension));
            ind.TotalSB1PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1PFPercent, attNameExtension));

            ind.TotalSB2PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2PFTotal, attNameExtension));
            ind.TotalSB2PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2PCTotal, attNameExtension));
            ind.TotalSB2APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2APTotal, attNameExtension));
            ind.TotalSB2ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2ACTotal, attNameExtension));
            ind.TotalSB2APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2APChange, attNameExtension));
            ind.TotalSB2ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2ACChange, attNameExtension));
            ind.TotalSB2PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2PPPercent, attNameExtension));
            ind.TotalSB2PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2PCPercent, attNameExtension));
            ind.TotalSB2PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2PFPercent, attNameExtension));

            ind.TotalSB3PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3PFTotal, attNameExtension));
            ind.TotalSB3PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3PCTotal, attNameExtension));
            ind.TotalSB3APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3APTotal, attNameExtension));
            ind.TotalSB3ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3ACTotal, attNameExtension));
            ind.TotalSB3APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3APChange, attNameExtension));
            ind.TotalSB3ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3ACChange, attNameExtension));
            ind.TotalSB3PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3PPPercent, attNameExtension));
            ind.TotalSB3PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3PCPercent, attNameExtension));
            ind.TotalSB3PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3PFPercent, attNameExtension));

            ind.TotalSB4PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4PFTotal, attNameExtension));
            ind.TotalSB4PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4PCTotal, attNameExtension));
            ind.TotalSB4APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4APTotal, attNameExtension));
            ind.TotalSB4ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4ACTotal, attNameExtension));
            ind.TotalSB4APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4APChange, attNameExtension));
            ind.TotalSB4ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4ACChange, attNameExtension));
            ind.TotalSB4PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4PPPercent, attNameExtension));
            ind.TotalSB4PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4PCPercent, attNameExtension));
            ind.TotalSB4PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4PFPercent, attNameExtension));

            ind.TotalSB5PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5PFTotal, attNameExtension));
            ind.TotalSB5PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5PCTotal, attNameExtension));
            ind.TotalSB5APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5APTotal, attNameExtension));
            ind.TotalSB5ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5ACTotal, attNameExtension));
            ind.TotalSB5APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5APChange, attNameExtension));
            ind.TotalSB5ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5ACChange, attNameExtension));
            ind.TotalSB5PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5PPPercent, attNameExtension));
            ind.TotalSB5PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5PCPercent, attNameExtension));
            ind.TotalSB5PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5PFPercent, attNameExtension));

            ind.TotalSB6PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6PFTotal, attNameExtension));
            ind.TotalSB6PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6PCTotal, attNameExtension));
            ind.TotalSB6APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6APTotal, attNameExtension));
            ind.TotalSB6ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6ACTotal, attNameExtension));
            ind.TotalSB6APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6APChange, attNameExtension));
            ind.TotalSB6ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6ACChange, attNameExtension));
            ind.TotalSB6PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6PPPercent, attNameExtension));
            ind.TotalSB6PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6PCPercent, attNameExtension));
            ind.TotalSB6PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6PFPercent, attNameExtension));

            ind.TotalSB7PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7PFTotal, attNameExtension));
            ind.TotalSB7PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7PCTotal, attNameExtension));
            ind.TotalSB7APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7APTotal, attNameExtension));
            ind.TotalSB7ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7ACTotal, attNameExtension));
            ind.TotalSB7APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7APChange, attNameExtension));
            ind.TotalSB7ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7ACChange, attNameExtension));
            ind.TotalSB7PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7PPPercent, attNameExtension));
            ind.TotalSB7PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7PCPercent, attNameExtension));
            ind.TotalSB7PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7PFPercent, attNameExtension));

            ind.TotalSB8PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8PFTotal, attNameExtension));
            ind.TotalSB8PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8PCTotal, attNameExtension));
            ind.TotalSB8APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8APTotal, attNameExtension));
            ind.TotalSB8ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8ACTotal, attNameExtension));
            ind.TotalSB8APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8APChange, attNameExtension));
            ind.TotalSB8ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8ACChange, attNameExtension));
            ind.TotalSB8PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8PPPercent, attNameExtension));
            ind.TotalSB8PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8PCPercent, attNameExtension));
            ind.TotalSB8PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8PFPercent, attNameExtension));

            ind.TotalSB9PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9PFTotal, attNameExtension));
            ind.TotalSB9PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9PCTotal, attNameExtension));
            ind.TotalSB9APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9APTotal, attNameExtension));
            ind.TotalSB9ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9ACTotal, attNameExtension));
            ind.TotalSB9APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9APChange, attNameExtension));
            ind.TotalSB9ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9ACChange, attNameExtension));
            ind.TotalSB9PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9PPPercent, attNameExtension));
            ind.TotalSB9PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9PCPercent, attNameExtension));
            ind.TotalSB9PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9PFPercent, attNameExtension));

            ind.TotalSB10PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10PFTotal, attNameExtension));
            ind.TotalSB10PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10PCTotal, attNameExtension));
            ind.TotalSB10APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10APTotal, attNameExtension));
            ind.TotalSB10ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10ACTotal, attNameExtension));
            ind.TotalSB10APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10APChange, attNameExtension));
            ind.TotalSB10ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10ACChange, attNameExtension));
            ind.TotalSB10PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10PPPercent, attNameExtension));
            ind.TotalSB10PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10PCPercent, attNameExtension));
            ind.TotalSB10PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10PFPercent, attNameExtension));
            
        }

        public void SetTotalSB1Progress1Property(SB1Progress1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalSBScorePFTotal:
                    ind.TotalSBScorePFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScorePCTotal:
                    ind.TotalSBScorePCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreAPTotal:
                    ind.TotalSBScoreAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreACTotal:
                    ind.TotalSBScoreACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreAPChange:
                    ind.TotalSBScoreAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreACChange:
                    ind.TotalSBScoreACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScorePPPercent:
                    ind.TotalSBScorePPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScorePCPercent:
                    ind.TotalSBScorePCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScorePFPercent:
                    ind.TotalSBScorePFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLPFTotal:
                    ind.TotalSBScoreLPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLPCTotal:
                    ind.TotalSBScoreLPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLAPTotal:
                    ind.TotalSBScoreLAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLACTotal:
                    ind.TotalSBScoreLACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLAPChange:
                    ind.TotalSBScoreLAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLACChange:
                    ind.TotalSBScoreLACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLPPPercent:
                    ind.TotalSBScoreLPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLPCPercent:
                    ind.TotalSBScoreLPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLPFPercent:
                    ind.TotalSBScoreLPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUPFTotal:
                    ind.TotalSBScoreUPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUPCTotal:
                    ind.TotalSBScoreUPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUAPTotal:
                    ind.TotalSBScoreUAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUACTotal:
                    ind.TotalSBScoreUACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUAPChange:
                    ind.TotalSBScoreUAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUACChange:
                    ind.TotalSBScoreUACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUPPPercent:
                    ind.TotalSBScoreUPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUPCPercent:
                    ind.TotalSBScoreUPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUPFPercent:
                    ind.TotalSBScoreUPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1PFTotal:
                    ind.TotalSB1PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1PCTotal:
                    ind.TotalSB1PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1APTotal:
                    ind.TotalSB1APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1ACTotal:
                    ind.TotalSB1ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1APChange:
                    ind.TotalSB1APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1ACChange:
                    ind.TotalSB1ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1PPPercent:
                    ind.TotalSB1PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1PCPercent:
                    ind.TotalSB1PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1PFPercent:
                    ind.TotalSB1PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2PFTotal:
                    ind.TotalSB2PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2PCTotal:
                    ind.TotalSB2PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2APTotal:
                    ind.TotalSB2APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2ACTotal:
                    ind.TotalSB2ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2APChange:
                    ind.TotalSB2APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2ACChange:
                    ind.TotalSB2ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2PPPercent:
                    ind.TotalSB2PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2PCPercent:
                    ind.TotalSB2PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2PFPercent:
                    ind.TotalSB2PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3PFTotal:
                    ind.TotalSB3PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3PCTotal:
                    ind.TotalSB3PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3APTotal:
                    ind.TotalSB3APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3ACTotal:
                    ind.TotalSB3ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3APChange:
                    ind.TotalSB3APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3ACChange:
                    ind.TotalSB3ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3PPPercent:
                    ind.TotalSB3PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3PCPercent:
                    ind.TotalSB3PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3PFPercent:
                    ind.TotalSB3PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4PFTotal:
                    ind.TotalSB4PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4PCTotal:
                    ind.TotalSB4PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4APTotal:
                    ind.TotalSB4APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4ACTotal:
                    ind.TotalSB4ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4APChange:
                    ind.TotalSB4APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4ACChange:
                    ind.TotalSB4ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4PPPercent:
                    ind.TotalSB4PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4PCPercent:
                    ind.TotalSB4PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4PFPercent:
                    ind.TotalSB4PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5PFTotal:
                    ind.TotalSB5PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5PCTotal:
                    ind.TotalSB5PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5APTotal:
                    ind.TotalSB5APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5ACTotal:
                    ind.TotalSB5ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5APChange:
                    ind.TotalSB5APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5ACChange:
                    ind.TotalSB5ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5PPPercent:
                    ind.TotalSB5PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5PCPercent:
                    ind.TotalSB5PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5PFPercent:
                    ind.TotalSB5PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6PFTotal:
                    ind.TotalSB6PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6PCTotal:
                    ind.TotalSB6PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6APTotal:
                    ind.TotalSB6APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6ACTotal:
                    ind.TotalSB6ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6APChange:
                    ind.TotalSB6APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6ACChange:
                    ind.TotalSB6ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6PPPercent:
                    ind.TotalSB6PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6PCPercent:
                    ind.TotalSB6PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6PFPercent:
                    ind.TotalSB6PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7PFTotal:
                    ind.TotalSB7PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7PCTotal:
                    ind.TotalSB7PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7APTotal:
                    ind.TotalSB7APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7ACTotal:
                    ind.TotalSB7ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7APChange:
                    ind.TotalSB7APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7ACChange:
                    ind.TotalSB7ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7PPPercent:
                    ind.TotalSB7PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7PCPercent:
                    ind.TotalSB7PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7PFPercent:
                    ind.TotalSB7PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8PFTotal:
                    ind.TotalSB8PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8PCTotal:
                    ind.TotalSB8PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8APTotal:
                    ind.TotalSB8APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8ACTotal:
                    ind.TotalSB8ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8APChange:
                    ind.TotalSB8APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8ACChange:
                    ind.TotalSB8ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8PPPercent:
                    ind.TotalSB8PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8PCPercent:
                    ind.TotalSB8PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8PFPercent:
                    ind.TotalSB8PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9PFTotal:
                    ind.TotalSB9PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9PCTotal:
                    ind.TotalSB9PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9APTotal:
                    ind.TotalSB9APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9ACTotal:
                    ind.TotalSB9ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9APChange:
                    ind.TotalSB9APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9ACChange:
                    ind.TotalSB9ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9PPPercent:
                    ind.TotalSB9PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9PCPercent:
                    ind.TotalSB9PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9PFPercent:
                    ind.TotalSB9PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10PFTotal:
                    ind.TotalSB10PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10PCTotal:
                    ind.TotalSB10PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10APTotal:
                    ind.TotalSB10APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10ACTotal:
                    ind.TotalSB10ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10APChange:
                    ind.TotalSB10APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10ACChange:
                    ind.TotalSB10ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10PPPercent:
                    ind.TotalSB10PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10PCPercent:
                    ind.TotalSB10PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10PFPercent:
                    ind.TotalSB10PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalSB1Progress1Property(SB1Progress1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalSBScorePFTotal:
                    sPropertyValue = ind.TotalSBScorePFTotal.ToString();
                    break;
                case cTotalSBScorePCTotal:
                    sPropertyValue = ind.TotalSBScorePCTotal.ToString();
                    break;
                case cTotalSBScoreAPTotal:
                    sPropertyValue = ind.TotalSBScoreAPTotal.ToString();
                    break;
                case cTotalSBScoreACTotal:
                    sPropertyValue = ind.TotalSBScoreACTotal.ToString();
                    break;
                case cTotalSBScoreAPChange:
                    sPropertyValue = ind.TotalSBScoreAPChange.ToString();
                    break;
                case cTotalSBScoreACChange:
                    sPropertyValue = ind.TotalSBScoreACChange.ToString();
                    break;
                case cTotalSBScorePPPercent:
                    sPropertyValue = ind.TotalSBScorePPPercent.ToString();
                    break;
                case cTotalSBScorePCPercent:
                    sPropertyValue = ind.TotalSBScorePCPercent.ToString();
                    break;
                case cTotalSBScorePFPercent:
                    sPropertyValue = ind.TotalSBScorePFPercent.ToString();
                    break;
                case cTotalSBScoreLPFTotal:
                    sPropertyValue = ind.TotalSBScoreLPFTotal.ToString();
                    break;
                case cTotalSBScoreLPCTotal:
                    sPropertyValue = ind.TotalSBScoreLPCTotal.ToString();
                    break;
                case cTotalSBScoreLAPTotal:
                    sPropertyValue = ind.TotalSBScoreLAPTotal.ToString();
                    break;
                case cTotalSBScoreLACTotal:
                    sPropertyValue = ind.TotalSBScoreLACTotal.ToString();
                    break;
                case cTotalSBScoreLAPChange:
                    sPropertyValue = ind.TotalSBScoreLAPChange.ToString();
                    break;
                case cTotalSBScoreLACChange:
                    sPropertyValue = ind.TotalSBScoreLACChange.ToString();
                    break;
                case cTotalSBScoreLPPPercent:
                    sPropertyValue = ind.TotalSBScoreLPPPercent.ToString();
                    break;
                case cTotalSBScoreLPCPercent:
                    sPropertyValue = ind.TotalSBScoreLPCPercent.ToString();
                    break;
                case cTotalSBScoreLPFPercent:
                    sPropertyValue = ind.TotalSBScoreLPFPercent.ToString();
                    break;
                case cTotalSBScoreUPFTotal:
                    sPropertyValue = ind.TotalSBScoreUPFTotal.ToString();
                    break;
                case cTotalSBScoreUPCTotal:
                    sPropertyValue = ind.TotalSBScoreUPCTotal.ToString();
                    break;
                case cTotalSBScoreUAPTotal:
                    sPropertyValue = ind.TotalSBScoreUAPTotal.ToString();
                    break;
                case cTotalSBScoreUACTotal:
                    sPropertyValue = ind.TotalSBScoreUACTotal.ToString();
                    break;
                case cTotalSBScoreUAPChange:
                    sPropertyValue = ind.TotalSBScoreUAPChange.ToString();
                    break;
                case cTotalSBScoreUACChange:
                    sPropertyValue = ind.TotalSBScoreUACChange.ToString();
                    break;
                case cTotalSBScoreUPPPercent:
                    sPropertyValue = ind.TotalSBScoreUPPPercent.ToString();
                    break;
                case cTotalSBScoreUPCPercent:
                    sPropertyValue = ind.TotalSBScoreUPCPercent.ToString();
                    break;
                case cTotalSBScoreUPFPercent:
                    sPropertyValue = ind.TotalSBScoreUPFPercent.ToString();
                    break;
                case cTotalSB1PFTotal:
                    sPropertyValue = ind.TotalSB1PFTotal.ToString();
                    break;
                case cTotalSB1PCTotal:
                    sPropertyValue = ind.TotalSB1PCTotal.ToString();
                    break;
                case cTotalSB1APTotal:
                    sPropertyValue = ind.TotalSB1APTotal.ToString();
                    break;
                case cTotalSB1ACTotal:
                    sPropertyValue = ind.TotalSB1ACTotal.ToString();
                    break;
                case cTotalSB1APChange:
                    sPropertyValue = ind.TotalSB1APChange.ToString();
                    break;
                case cTotalSB1ACChange:
                    sPropertyValue = ind.TotalSB1ACChange.ToString();
                    break;
                case cTotalSB1PPPercent:
                    sPropertyValue = ind.TotalSB1PPPercent.ToString();
                    break;
                case cTotalSB1PCPercent:
                    sPropertyValue = ind.TotalSB1PCPercent.ToString();
                    break;
                case cTotalSB1PFPercent:
                    sPropertyValue = ind.TotalSB1PFPercent.ToString();
                    break;
                case cTotalSB2PFTotal:
                    sPropertyValue = ind.TotalSB2PFTotal.ToString();
                    break;
                case cTotalSB2PCTotal:
                    sPropertyValue = ind.TotalSB2PCTotal.ToString();
                    break;
                case cTotalSB2APTotal:
                    sPropertyValue = ind.TotalSB2APTotal.ToString();
                    break;
                case cTotalSB2ACTotal:
                    sPropertyValue = ind.TotalSB2ACTotal.ToString();
                    break;
                case cTotalSB2APChange:
                    sPropertyValue = ind.TotalSB2APChange.ToString();
                    break;
                case cTotalSB2ACChange:
                    sPropertyValue = ind.TotalSB2ACChange.ToString();
                    break;
                case cTotalSB2PPPercent:
                    sPropertyValue = ind.TotalSB2PPPercent.ToString();
                    break;
                case cTotalSB2PCPercent:
                    sPropertyValue = ind.TotalSB2PCPercent.ToString();
                    break;
                case cTotalSB2PFPercent:
                    sPropertyValue = ind.TotalSB2PFPercent.ToString();
                    break;
                case cTotalSB3PFTotal:
                    sPropertyValue = ind.TotalSB3PFTotal.ToString();
                    break;
                case cTotalSB3PCTotal:
                    sPropertyValue = ind.TotalSB3PCTotal.ToString();
                    break;
                case cTotalSB3APTotal:
                    sPropertyValue = ind.TotalSB3APTotal.ToString();
                    break;
                case cTotalSB3ACTotal:
                    sPropertyValue = ind.TotalSB3ACTotal.ToString();
                    break;
                case cTotalSB3APChange:
                    sPropertyValue = ind.TotalSB3APChange.ToString();
                    break;
                case cTotalSB3ACChange:
                    sPropertyValue = ind.TotalSB3ACChange.ToString();
                    break;
                case cTotalSB3PPPercent:
                    sPropertyValue = ind.TotalSB3PPPercent.ToString();
                    break;
                case cTotalSB3PCPercent:
                    sPropertyValue = ind.TotalSB3PCPercent.ToString();
                    break;
                case cTotalSB3PFPercent:
                    sPropertyValue = ind.TotalSB3PFPercent.ToString();
                    break;
                case cTotalSB4PFTotal:
                    sPropertyValue = ind.TotalSB4PFTotal.ToString();
                    break;
                case cTotalSB4PCTotal:
                    sPropertyValue = ind.TotalSB4PCTotal.ToString();
                    break;
                case cTotalSB4APTotal:
                    sPropertyValue = ind.TotalSB4APTotal.ToString();
                    break;
                case cTotalSB4ACTotal:
                    sPropertyValue = ind.TotalSB4ACTotal.ToString();
                    break;
                case cTotalSB4APChange:
                    sPropertyValue = ind.TotalSB4APChange.ToString();
                    break;
                case cTotalSB4ACChange:
                    sPropertyValue = ind.TotalSB4ACChange.ToString();
                    break;
                case cTotalSB4PPPercent:
                    sPropertyValue = ind.TotalSB4PPPercent.ToString();
                    break;
                case cTotalSB4PCPercent:
                    sPropertyValue = ind.TotalSB4PCPercent.ToString();
                    break;
                case cTotalSB4PFPercent:
                    sPropertyValue = ind.TotalSB4PFPercent.ToString();
                    break;
                case cTotalSB5PFTotal:
                    sPropertyValue = ind.TotalSB5PFTotal.ToString();
                    break;
                case cTotalSB5PCTotal:
                    sPropertyValue = ind.TotalSB5PCTotal.ToString();
                    break;
                case cTotalSB5APTotal:
                    sPropertyValue = ind.TotalSB5APTotal.ToString();
                    break;
                case cTotalSB5ACTotal:
                    sPropertyValue = ind.TotalSB5ACTotal.ToString();
                    break;
                case cTotalSB5APChange:
                    sPropertyValue = ind.TotalSB5APChange.ToString();
                    break;
                case cTotalSB5ACChange:
                    sPropertyValue = ind.TotalSB5ACChange.ToString();
                    break;
                case cTotalSB5PPPercent:
                    sPropertyValue = ind.TotalSB5PPPercent.ToString();
                    break;
                case cTotalSB5PCPercent:
                    sPropertyValue = ind.TotalSB5PCPercent.ToString();
                    break;
                case cTotalSB5PFPercent:
                    sPropertyValue = ind.TotalSB5PFPercent.ToString();
                    break;
                case cTotalSB6PFTotal:
                    sPropertyValue = ind.TotalSB6PFTotal.ToString();
                    break;
                case cTotalSB6PCTotal:
                    sPropertyValue = ind.TotalSB6PCTotal.ToString();
                    break;
                case cTotalSB6APTotal:
                    sPropertyValue = ind.TotalSB6APTotal.ToString();
                    break;
                case cTotalSB6ACTotal:
                    sPropertyValue = ind.TotalSB6ACTotal.ToString();
                    break;
                case cTotalSB6APChange:
                    sPropertyValue = ind.TotalSB6APChange.ToString();
                    break;
                case cTotalSB6ACChange:
                    sPropertyValue = ind.TotalSB6ACChange.ToString();
                    break;
                case cTotalSB6PPPercent:
                    sPropertyValue = ind.TotalSB6PPPercent.ToString();
                    break;
                case cTotalSB6PCPercent:
                    sPropertyValue = ind.TotalSB6PCPercent.ToString();
                    break;
                case cTotalSB6PFPercent:
                    sPropertyValue = ind.TotalSB6PFPercent.ToString();
                    break;
                case cTotalSB7PFTotal:
                    sPropertyValue = ind.TotalSB7PFTotal.ToString();
                    break;
                case cTotalSB7PCTotal:
                    sPropertyValue = ind.TotalSB7PCTotal.ToString();
                    break;
                case cTotalSB7APTotal:
                    sPropertyValue = ind.TotalSB7APTotal.ToString();
                    break;
                case cTotalSB7ACTotal:
                    sPropertyValue = ind.TotalSB7ACTotal.ToString();
                    break;
                case cTotalSB7APChange:
                    sPropertyValue = ind.TotalSB7APChange.ToString();
                    break;
                case cTotalSB7ACChange:
                    sPropertyValue = ind.TotalSB7ACChange.ToString();
                    break;
                case cTotalSB7PPPercent:
                    sPropertyValue = ind.TotalSB7PPPercent.ToString();
                    break;
                case cTotalSB7PCPercent:
                    sPropertyValue = ind.TotalSB7PCPercent.ToString();
                    break;
                case cTotalSB7PFPercent:
                    sPropertyValue = ind.TotalSB7PFPercent.ToString();
                    break;
                case cTotalSB8PFTotal:
                    sPropertyValue = ind.TotalSB8PFTotal.ToString();
                    break;
                case cTotalSB8PCTotal:
                    sPropertyValue = ind.TotalSB8PCTotal.ToString();
                    break;
                case cTotalSB8APTotal:
                    sPropertyValue = ind.TotalSB8APTotal.ToString();
                    break;
                case cTotalSB8ACTotal:
                    sPropertyValue = ind.TotalSB8ACTotal.ToString();
                    break;
                case cTotalSB8APChange:
                    sPropertyValue = ind.TotalSB8APChange.ToString();
                    break;
                case cTotalSB8ACChange:
                    sPropertyValue = ind.TotalSB8ACChange.ToString();
                    break;
                case cTotalSB8PPPercent:
                    sPropertyValue = ind.TotalSB8PPPercent.ToString();
                    break;
                case cTotalSB8PCPercent:
                    sPropertyValue = ind.TotalSB8PCPercent.ToString();
                    break;
                case cTotalSB8PFPercent:
                    sPropertyValue = ind.TotalSB8PFPercent.ToString();
                    break;
                case cTotalSB9PFTotal:
                    sPropertyValue = ind.TotalSB9PFTotal.ToString();
                    break;
                case cTotalSB9PCTotal:
                    sPropertyValue = ind.TotalSB9PCTotal.ToString();
                    break;
                case cTotalSB9APTotal:
                    sPropertyValue = ind.TotalSB9APTotal.ToString();
                    break;
                case cTotalSB9ACTotal:
                    sPropertyValue = ind.TotalSB9ACTotal.ToString();
                    break;
                case cTotalSB9APChange:
                    sPropertyValue = ind.TotalSB9APChange.ToString();
                    break;
                case cTotalSB9ACChange:
                    sPropertyValue = ind.TotalSB9ACChange.ToString();
                    break;
                case cTotalSB9PPPercent:
                    sPropertyValue = ind.TotalSB9PPPercent.ToString();
                    break;
                case cTotalSB9PCPercent:
                    sPropertyValue = ind.TotalSB9PCPercent.ToString();
                    break;
                case cTotalSB9PFPercent:
                    sPropertyValue = ind.TotalSB9PFPercent.ToString();
                    break;
                case cTotalSB10PFTotal:
                    sPropertyValue = ind.TotalSB10PFTotal.ToString();
                    break;
                case cTotalSB10PCTotal:
                    sPropertyValue = ind.TotalSB10PCTotal.ToString();
                    break;
                case cTotalSB10APTotal:
                    sPropertyValue = ind.TotalSB10APTotal.ToString();
                    break;
                case cTotalSB10ACTotal:
                    sPropertyValue = ind.TotalSB10ACTotal.ToString();
                    break;
                case cTotalSB10APChange:
                    sPropertyValue = ind.TotalSB10APChange.ToString();
                    break;
                case cTotalSB10ACChange:
                    sPropertyValue = ind.TotalSB10ACChange.ToString();
                    break;
                case cTotalSB10PPPercent:
                    sPropertyValue = ind.TotalSB10PPPercent.ToString();
                    break;
                case cTotalSB10PCPercent:
                    sPropertyValue = ind.TotalSB10PCPercent.ToString();
                    break;
                case cTotalSB10PFPercent:
                    sPropertyValue = ind.TotalSB10PFPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalSB1Progress1Attributes(SB1Progress1 ind,
            string attNameExtension, XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScorePFTotal, attNameExtension), ind.TotalSBScorePFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScorePCTotal, attNameExtension), ind.TotalSBScorePCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreAPTotal, attNameExtension), ind.TotalSBScoreAPTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreACTotal, attNameExtension), ind.TotalSBScoreACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreAPChange, attNameExtension), ind.TotalSBScoreAPChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreACChange, attNameExtension), ind.TotalSBScoreACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScorePPPercent, attNameExtension), ind.TotalSBScorePPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScorePCPercent, attNameExtension), ind.TotalSBScorePCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScorePFPercent, attNameExtension), ind.TotalSBScorePFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLPFTotal, attNameExtension), ind.TotalSBScoreLPFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLPCTotal, attNameExtension), ind.TotalSBScoreLPCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLAPTotal, attNameExtension), ind.TotalSBScoreLAPTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLACTotal, attNameExtension), ind.TotalSBScoreLACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLAPChange, attNameExtension), ind.TotalSBScoreLAPChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLACChange, attNameExtension), ind.TotalSBScoreLACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLPPPercent, attNameExtension), ind.TotalSBScoreLPPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLPCPercent, attNameExtension), ind.TotalSBScoreLPCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLPFPercent, attNameExtension), ind.TotalSBScoreLPFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUPFTotal, attNameExtension), ind.TotalSBScoreUPFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUPCTotal, attNameExtension), ind.TotalSBScoreUPCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUAPTotal, attNameExtension), ind.TotalSBScoreUAPTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUACTotal, attNameExtension), ind.TotalSBScoreUACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUAPChange, attNameExtension), ind.TotalSBScoreUAPChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUACChange, attNameExtension), ind.TotalSBScoreUACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUPPPercent, attNameExtension), ind.TotalSBScoreUPPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUPCPercent, attNameExtension), ind.TotalSBScoreUPCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUPFPercent, attNameExtension), ind.TotalSBScoreUPFPercent);


            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1PFTotal, attNameExtension), ind.TotalSB1PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1PCTotal, attNameExtension), ind.TotalSB1PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1APTotal, attNameExtension), ind.TotalSB1APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1ACTotal, attNameExtension), ind.TotalSB1ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1APChange, attNameExtension), ind.TotalSB1APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1ACChange, attNameExtension), ind.TotalSB1ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1PPPercent, attNameExtension), ind.TotalSB1PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1PCPercent, attNameExtension), ind.TotalSB1PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1PFPercent, attNameExtension), ind.TotalSB1PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2PFTotal, attNameExtension), ind.TotalSB2PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2PCTotal, attNameExtension), ind.TotalSB2PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2APTotal, attNameExtension), ind.TotalSB2APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2ACTotal, attNameExtension), ind.TotalSB2ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2APChange, attNameExtension), ind.TotalSB2APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2ACChange, attNameExtension), ind.TotalSB2ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2PPPercent, attNameExtension), ind.TotalSB2PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2PCPercent, attNameExtension), ind.TotalSB2PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2PFPercent, attNameExtension), ind.TotalSB2PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3PFTotal, attNameExtension), ind.TotalSB3PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3PCTotal, attNameExtension), ind.TotalSB3PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3APTotal, attNameExtension), ind.TotalSB3APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3ACTotal, attNameExtension), ind.TotalSB3ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3APChange, attNameExtension), ind.TotalSB3APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3ACChange, attNameExtension), ind.TotalSB3ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3PPPercent, attNameExtension), ind.TotalSB3PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3PCPercent, attNameExtension), ind.TotalSB3PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3PFPercent, attNameExtension), ind.TotalSB3PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4PFTotal, attNameExtension), ind.TotalSB4PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4PCTotal, attNameExtension), ind.TotalSB4PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4APTotal, attNameExtension), ind.TotalSB4APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4ACTotal, attNameExtension), ind.TotalSB4ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4APChange, attNameExtension), ind.TotalSB4APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4ACChange, attNameExtension), ind.TotalSB4ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4PPPercent, attNameExtension), ind.TotalSB4PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4PCPercent, attNameExtension), ind.TotalSB4PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4PFPercent, attNameExtension), ind.TotalSB4PFPercent);

             CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5PFTotal, attNameExtension), ind.TotalSB5PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5PCTotal, attNameExtension), ind.TotalSB5PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5APTotal, attNameExtension), ind.TotalSB5APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5ACTotal, attNameExtension), ind.TotalSB5ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5APChange, attNameExtension), ind.TotalSB5APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5ACChange, attNameExtension), ind.TotalSB5ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5PPPercent, attNameExtension), ind.TotalSB5PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5PCPercent, attNameExtension), ind.TotalSB5PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5PFPercent, attNameExtension), ind.TotalSB5PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6PFTotal, attNameExtension), ind.TotalSB6PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6PCTotal, attNameExtension), ind.TotalSB6PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6APTotal, attNameExtension), ind.TotalSB6APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6ACTotal, attNameExtension), ind.TotalSB6ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6APChange, attNameExtension), ind.TotalSB6APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6ACChange, attNameExtension), ind.TotalSB6ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6PPPercent, attNameExtension), ind.TotalSB6PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6PCPercent, attNameExtension), ind.TotalSB6PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6PFPercent, attNameExtension), ind.TotalSB6PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7PFTotal, attNameExtension), ind.TotalSB7PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7PCTotal, attNameExtension), ind.TotalSB7PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7APTotal, attNameExtension), ind.TotalSB7APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7ACTotal, attNameExtension), ind.TotalSB7ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7APChange, attNameExtension), ind.TotalSB7APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7ACChange, attNameExtension), ind.TotalSB7ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7PPPercent, attNameExtension), ind.TotalSB7PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7PCPercent, attNameExtension), ind.TotalSB7PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7PFPercent, attNameExtension), ind.TotalSB7PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8PFTotal, attNameExtension), ind.TotalSB8PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8PCTotal, attNameExtension), ind.TotalSB8PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8APTotal, attNameExtension), ind.TotalSB8APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8ACTotal, attNameExtension), ind.TotalSB8ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8APChange, attNameExtension), ind.TotalSB8APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8ACChange, attNameExtension), ind.TotalSB8ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8PPPercent, attNameExtension), ind.TotalSB8PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8PCPercent, attNameExtension), ind.TotalSB8PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8PFPercent, attNameExtension), ind.TotalSB8PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9PFTotal, attNameExtension), ind.TotalSB9PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9PCTotal, attNameExtension), ind.TotalSB9PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9APTotal, attNameExtension), ind.TotalSB9APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9ACTotal, attNameExtension), ind.TotalSB9ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9APChange, attNameExtension), ind.TotalSB9APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9ACChange, attNameExtension), ind.TotalSB9ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9PPPercent, attNameExtension), ind.TotalSB9PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9PCPercent, attNameExtension), ind.TotalSB9PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9PFPercent, attNameExtension), ind.TotalSB9PFPercent);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10PFTotal, attNameExtension), ind.TotalSB10PFTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10PCTotal, attNameExtension), ind.TotalSB10PCTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10APTotal, attNameExtension), ind.TotalSB10APTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10ACTotal, attNameExtension), ind.TotalSB10ACTotal);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10APChange, attNameExtension), ind.TotalSB10APChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10ACChange, attNameExtension), ind.TotalSB10ACChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10PPPercent, attNameExtension), ind.TotalSB10PPPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10PCPercent, attNameExtension), ind.TotalSB10PCPercent);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10PFPercent, attNameExtension), ind.TotalSB10PFPercent);
            
        }

        public async Task SetTotalSB1Progress1AttributesAsync(SB1Progress1 ind,
            string attNameExtension, XmlWriter writer)
        {
            //comparative analyses need to know how many rows to display
            int iSBCount = 0;
            if (ind.TSB1ScoreMUnit.Length > 0)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScorePFTotal, attNameExtension), string.Empty, ind.TotalSBScorePFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScorePCTotal, attNameExtension), string.Empty, ind.TotalSBScorePCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreAPTotal, attNameExtension), string.Empty, ind.TotalSBScoreAPTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreACTotal, attNameExtension), string.Empty, ind.TotalSBScoreACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreAPChange, attNameExtension), string.Empty, ind.TotalSBScoreAPChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreACChange, attNameExtension), string.Empty, ind.TotalSBScoreACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScorePPPercent, attNameExtension), string.Empty, ind.TotalSBScorePPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScorePCPercent, attNameExtension), string.Empty, ind.TotalSBScorePCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScorePFPercent, attNameExtension), string.Empty, ind.TotalSBScorePFPercent.ToString("N4", CultureInfo.InvariantCulture));

                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLPFTotal, attNameExtension), string.Empty, ind.TotalSBScoreLPFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLPCTotal, attNameExtension), string.Empty, ind.TotalSBScoreLPCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLAPTotal, attNameExtension), string.Empty, ind.TotalSBScoreLAPTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLACTotal, attNameExtension), string.Empty, ind.TotalSBScoreLACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLAPChange, attNameExtension), string.Empty, ind.TotalSBScoreLAPChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLACChange, attNameExtension), string.Empty, ind.TotalSBScoreLACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLPPPercent, attNameExtension), string.Empty, ind.TotalSBScoreLPPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLPCPercent, attNameExtension), string.Empty, ind.TotalSBScoreLPCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreLPFPercent, attNameExtension), string.Empty, ind.TotalSBScoreLPFPercent.ToString("N4", CultureInfo.InvariantCulture));

                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUPFTotal, attNameExtension), string.Empty, ind.TotalSBScoreUPFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUPCTotal, attNameExtension), string.Empty, ind.TotalSBScoreUPCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUAPTotal, attNameExtension), string.Empty, ind.TotalSBScoreUAPTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUACTotal, attNameExtension), string.Empty, ind.TotalSBScoreUACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUAPChange, attNameExtension), string.Empty, ind.TotalSBScoreUAPChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUACChange, attNameExtension), string.Empty, ind.TotalSBScoreUACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUPPPercent, attNameExtension), string.Empty, ind.TotalSBScoreUPPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUPCPercent, attNameExtension), string.Empty, ind.TotalSBScoreUPCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSBScoreUPFPercent, attNameExtension), string.Empty, ind.TotalSBScoreUPFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label1.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1PFTotal, attNameExtension), string.Empty, ind.TotalSB1PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1PCTotal, attNameExtension), string.Empty, ind.TotalSB1PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1APTotal, attNameExtension), string.Empty, ind.TotalSB1APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1ACTotal, attNameExtension), string.Empty, ind.TotalSB1ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1APChange, attNameExtension), string.Empty, ind.TotalSB1APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1ACChange, attNameExtension), string.Empty, ind.TotalSB1ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1PPPercent, attNameExtension), string.Empty, ind.TotalSB1PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1PCPercent, attNameExtension), string.Empty, ind.TotalSB1PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB1PFPercent, attNameExtension), string.Empty, ind.TotalSB1PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label2.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2PFTotal, attNameExtension), string.Empty, ind.TotalSB2PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2PCTotal, attNameExtension), string.Empty, ind.TotalSB2PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2APTotal, attNameExtension), string.Empty, ind.TotalSB2APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2ACTotal, attNameExtension), string.Empty, ind.TotalSB2ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2APChange, attNameExtension), string.Empty, ind.TotalSB2APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2ACChange, attNameExtension), string.Empty, ind.TotalSB2ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2PPPercent, attNameExtension), string.Empty, ind.TotalSB2PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2PCPercent, attNameExtension), string.Empty, ind.TotalSB2PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB2PFPercent, attNameExtension), string.Empty, ind.TotalSB2PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label3.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3PFTotal, attNameExtension), string.Empty, ind.TotalSB3PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3PCTotal, attNameExtension), string.Empty, ind.TotalSB3PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3APTotal, attNameExtension), string.Empty, ind.TotalSB3APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3ACTotal, attNameExtension), string.Empty, ind.TotalSB3ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3APChange, attNameExtension), string.Empty, ind.TotalSB3APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3ACChange, attNameExtension), string.Empty, ind.TotalSB3ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3PPPercent, attNameExtension), string.Empty, ind.TotalSB3PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3PCPercent, attNameExtension), string.Empty, ind.TotalSB3PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB3PFPercent, attNameExtension), string.Empty, ind.TotalSB3PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label4.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4PFTotal, attNameExtension), string.Empty, ind.TotalSB4PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4PCTotal, attNameExtension), string.Empty, ind.TotalSB4PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4APTotal, attNameExtension), string.Empty, ind.TotalSB4APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4ACTotal, attNameExtension), string.Empty, ind.TotalSB4ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4APChange, attNameExtension), string.Empty, ind.TotalSB4APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4ACChange, attNameExtension), string.Empty, ind.TotalSB4ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4PPPercent, attNameExtension), string.Empty, ind.TotalSB4PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4PCPercent, attNameExtension), string.Empty, ind.TotalSB4PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB4PFPercent, attNameExtension), string.Empty, ind.TotalSB4PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label5.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5PFTotal, attNameExtension), string.Empty, ind.TotalSB5PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5PCTotal, attNameExtension), string.Empty, ind.TotalSB5PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5APTotal, attNameExtension), string.Empty, ind.TotalSB5APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5ACTotal, attNameExtension), string.Empty, ind.TotalSB5ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5APChange, attNameExtension), string.Empty, ind.TotalSB5APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5ACChange, attNameExtension), string.Empty, ind.TotalSB5ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5PPPercent, attNameExtension), string.Empty, ind.TotalSB5PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5PCPercent, attNameExtension), string.Empty, ind.TotalSB5PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB5PFPercent, attNameExtension), string.Empty, ind.TotalSB5PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label6.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6PFTotal, attNameExtension), string.Empty, ind.TotalSB6PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6PCTotal, attNameExtension), string.Empty, ind.TotalSB6PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6APTotal, attNameExtension), string.Empty, ind.TotalSB6APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6ACTotal, attNameExtension), string.Empty, ind.TotalSB6ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6APChange, attNameExtension), string.Empty, ind.TotalSB6APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6ACChange, attNameExtension), string.Empty, ind.TotalSB6ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6PPPercent, attNameExtension), string.Empty, ind.TotalSB6PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6PCPercent, attNameExtension), string.Empty, ind.TotalSB6PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB6PFPercent, attNameExtension), string.Empty, ind.TotalSB6PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label7.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7PFTotal, attNameExtension), string.Empty, ind.TotalSB7PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7PCTotal, attNameExtension), string.Empty, ind.TotalSB7PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7APTotal, attNameExtension), string.Empty, ind.TotalSB7APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7ACTotal, attNameExtension), string.Empty, ind.TotalSB7ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7APChange, attNameExtension), string.Empty, ind.TotalSB7APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7ACChange, attNameExtension), string.Empty, ind.TotalSB7ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7PPPercent, attNameExtension), string.Empty, ind.TotalSB7PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7PCPercent, attNameExtension), string.Empty, ind.TotalSB7PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB7PFPercent, attNameExtension), string.Empty, ind.TotalSB7PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label8.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8PFTotal, attNameExtension), string.Empty, ind.TotalSB8PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8PCTotal, attNameExtension), string.Empty, ind.TotalSB8PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8APTotal, attNameExtension), string.Empty, ind.TotalSB8APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8ACTotal, attNameExtension), string.Empty, ind.TotalSB8ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8APChange, attNameExtension), string.Empty, ind.TotalSB8APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8ACChange, attNameExtension), string.Empty, ind.TotalSB8ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8PPPercent, attNameExtension), string.Empty, ind.TotalSB8PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8PCPercent, attNameExtension), string.Empty, ind.TotalSB8PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB8PFPercent, attNameExtension), string.Empty, ind.TotalSB8PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label9.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9PFTotal, attNameExtension), string.Empty, ind.TotalSB9PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9PCTotal, attNameExtension), string.Empty, ind.TotalSB9PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9APTotal, attNameExtension), string.Empty, ind.TotalSB9APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9ACTotal, attNameExtension), string.Empty, ind.TotalSB9ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9APChange, attNameExtension), string.Empty, ind.TotalSB9APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9ACChange, attNameExtension), string.Empty, ind.TotalSB9ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9PPPercent, attNameExtension), string.Empty, ind.TotalSB9PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9PCPercent, attNameExtension), string.Empty, ind.TotalSB9PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB9PFPercent, attNameExtension), string.Empty, ind.TotalSB9PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label10.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10PFTotal, attNameExtension), string.Empty, ind.TotalSB10PFTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10PCTotal, attNameExtension), string.Empty, ind.TotalSB10PCTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10APTotal, attNameExtension), string.Empty, ind.TotalSB10APTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10ACTotal, attNameExtension), string.Empty, ind.TotalSB10ACTotal.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10APChange, attNameExtension), string.Empty, ind.TotalSB10APChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10ACChange, attNameExtension), string.Empty, ind.TotalSB10ACChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10PPPercent, attNameExtension), string.Empty, ind.TotalSB10PPPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10PCPercent, attNameExtension), string.Empty, ind.TotalSB10PCPercent.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTotalSB10PFPercent, attNameExtension), string.Empty, ind.TotalSB10PFPercent.ToString("N4", CultureInfo.InvariantCulture));
            }
            //tells ss how many inds to display
            this.SBCount = iSBCount;
        }
        #endregion
        //run the analyses for inputs an outputs
        public async Task<bool> RunAnalyses(SB1Stock sb1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (use Total1 object to avoid duplication)
            SB1Total1 total = new SB1Total1(this.CalcParameters);
            //this adds the totals to sb1stock.total1 (not to total)
            bHasAnalyses = await total.RunAnalyses(sb1Stock);
            if (sb1Stock.Total1 != null)
            {
                //copy at least the stock and substock totals from total1 to stat1
                //subprices only if needed in future analyses
                sb1Stock.Progress1 = new SB1Progress1(this.CalcParameters);
                //need one property set
                sb1Stock.Progress1.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
                bHasAnalyses = CopyTotalIndicatorsToSB1Stock(sb1Stock.Progress1, sb1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing progress analysis
        public bool RunAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //set calculated changestocks
            List<SB1Stock> progressStocks = new List<SB1Stock>();
            if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                progressStocks = SetIOAnalyses(sb1Stock, calcs);
            }
            else
            {
                string sNodeName = sb1Stock.CalcParameters.CurrentElementNodeName;
                //use the stock for the node name
                if (sNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || sNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no progress
                    progressStocks = SetTotals(sb1Stock, calcs);
                }
                else if (sNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || sNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //tps with currentnodename set only need nets (inputs minus outputs)
                    //note that only sb1stock is used (not progressStocks)
                    progressStocks = SetTotals(sb1Stock, calcs);
                }
                else
                {
                    progressStocks = SetAnalyses(sb1Stock, calcs);
                }
            }
            //add the progresstocks to parent stock
            if (progressStocks != null)
            {
                bHasAnalyses = AddProgsToBaseStock(sb1Stock, progressStocks);
                //sb1Stock must still add the members of progress1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }
        
        private List<SB1Stock> SetTotals(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            List<SB1Stock> progressStocks = new List<SB1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of sb1stocks for each input and output
            //object model is calc.Total1.SB101Stocks
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(sb1Stock.GetType()))
                {
                    SB1Stock stock = (SB1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Progress1 != null)
                        {
                            string sNodeName = sb1Stock.CalcParameters.CurrentElementNodeName;
                            //tps start substracting outcomes from op/comps
                            if (sNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                || sNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                            {
                                stock.Progress1.SubApplicationType = stock.SubApplicationType;
                                sb1Stock.Progress1.TargetType = sb1Stock.TargetType;
                                bHasTotals = AddSubTotalToTotalStock(sb1Stock.Progress1, stock.Multiplier,
                                       stock.Progress1);
                                if (bHasTotals
                                    && stock.SubApplicationType != Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
                                {
                                    //this is done for consistency but progStocks are not used (only the totals in sb1Stock)
                                    progressStocks.Add(sb1Stock);
                                }
                            }
                            else
                            {
                                //calc holds an input or output stock
                                //add that stock to sb1stock (some analyses will need to use subprices too)
                                bHasTotals = AddSubTotalToTotalStock(sb1Stock.Progress1, stock.Multiplier, stock.Progress1);
                                if (bHasTotals)
                                {
                                    progressStocks.Add(sb1Stock);
                                }
                            }
                        }
                    }
                }
            }
            return progressStocks;
        }
        private List<SB1Stock> SetAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            List<SB1Stock> progressStocks = new List<SB1Stock>();
            bool bHasTotals = false;
            //set N
            int iQN = 0;
            //set the calc totals in each observation
            SB1Stock observationStock = new SB1Stock();
            observationStock.Progress1 = new SB1Progress1(this.CalcParameters);
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(sb1Stock.GetType()))
                {
                    SB1Stock stock = (SB1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Progress1 != null)
                        {
                            SB1Stock observation2Stock = new SB1Stock();
                            //stocks need some props set
                            stock.CalcParameters.CurrentElementNodeName 
                                = sb1Stock.CalcParameters.CurrentElementNodeName;
                            //168 need calc.Mults not agg.Mults
                            //stock.Multiplier = sb1Stock.Multiplier;
                            bHasTotals = SetObservationStock(progressStocks, sb1Stock,
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                progressStocks.Add(observation2Stock);
                                //N is determined from the stocks
                                iQN++;
                            }
                        }
                    }
                }
            }
            if (iQN > 0)
            {
                bHasTotals = SetProgressAnalysis(progressStocks, sb1Stock, iQN);
            }
            return progressStocks;
        }
        private List<SB1Stock> SetIOAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            List<SB1Stock> progressStocks = new List<SB1Stock>();
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iQN2 = 0;
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(sb1Stock.GetType()))
                {
                    SB1Stock stock = (SB1Stock)calc;
                    if (stock != null)
                    {
                        //stock.Progress1 holds the initial substock/price totals
                        if (stock.Progress1 != null)
                        {
                            SB1Stock observation2Stock = new SB1Stock();
                            stock.Multiplier = sb1Stock.Multiplier;
                            bHasTotals = SetObservationStock(progressStocks, sb1Stock,
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                progressStocks.Add(observation2Stock);
                                //N is determined from the stocks
                                iQN2++;
                            }
                        }
                    }
                }
            }
            if (iQN2 > 0)
            {
                bHasTotals = SetProgressAnalysis(progressStocks, sb1Stock, iQN2);
            }
            return progressStocks;
        }
    
        private bool SetObservationStock(List<SB1Stock> progressStocks,
            SB1Stock sb1Stock, SB1Stock stock, SB1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Progress1 = new SB1Progress1(this.CalcParameters);
            observation2Stock.Id = stock.Id;
            observation2Stock.Progress1.Id = stock.Id;
            //copy some stock props to progress1
            BISB1StockAnalyzerAsync.CopyBaseElementProperties(stock, observation2Stock.Progress1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BISB1StockAnalyzerAsync.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Progress1.CalcParameters == null)
                stock.Progress1.CalcParameters = new CalculatorParameters();
            stock.Progress1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            //194 to deal with multiple tps that have set parentstock.QTM += cumulativestock.PCAmount
            observation2Stock.Progress1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            if (stock.Progress1.CalcParameters.CurrentElementNodeName
               == BudgetInvestment.BUDGET_TYPES.budget.ToString()
               || stock.Progress1.CalcParameters.CurrentElementNodeName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                //at oc and outcome level no aggregating by targettype, need schedule variances)
                bHasTotals = AddSubTotalToTotalStock(observation2Stock.Progress1, stock.Multiplier, stock.Progress1);
            }
            else
            {
                //at oc and outcome level no aggregating by targettype, need schedule variances)
                bHasTotals = AddSubTotalToTotalStock(observation2Stock.Progress1, stock.Multiplier, stock.Progress1);
            }
            return bHasTotals;
        }
        
        private bool SetProgressAnalysis(List<SB1Stock> progressStocks, SB1Stock sb1Stock,
            int qN)
        {
            bool bHasTotals = false;
            if (qN > 0)
            {
                //set progress numbers
                SetSBScoreProgress(sb1Stock, progressStocks);
                SetSBScoreLProgress(sb1Stock, progressStocks);
                SetSBScoreUProgress(sb1Stock, progressStocks);
                SetSB1Progress(sb1Stock, progressStocks);
                SetSB2Progress(sb1Stock, progressStocks);
                SetSB3Progress(sb1Stock, progressStocks);
                SetSB4Progress(sb1Stock, progressStocks);
                SetSB5Progress(sb1Stock, progressStocks);
                SetSB6Progress(sb1Stock, progressStocks);
                SetSB7Progress(sb1Stock, progressStocks);
                SetSB8Progress(sb1Stock, progressStocks);
                SetSB9Progress(sb1Stock, progressStocks);
                SetSB10Progress(sb1Stock, progressStocks);
            }
            //add cumulative totals to parent lcastock1 (ocgroup)
            bHasTotals = AddCumulative1Calcs(progressStocks, sb1Stock);
            return bHasTotals;
        }
        
        private bool AddCumulative1Calcs(List<SB1Stock> progressStocks, 
            SB1Stock sb1Stock)
        {
            bool bHasTotals = false;
            SB1Stock cumChange = new SB1Stock();
            //194 no antecedent tps in QTMs 
            if (progressStocks.Any(p => p.ChangeType 
                == CHANGE_TYPES.xminus1.ToString()))
            {
                //set parent QTM for current period only; no xminus calcs
                //mults already applied
                double dbMultiplier = 1;
                foreach (var prog in progressStocks)
                {
                    if (prog.ChangeType != CHANGE_TYPES.xminus1.ToString())
                    {
                        AddSubTotalToTotalStock(sb1Stock.Progress1, dbMultiplier, prog.Progress1);
                    }
                }
            }
            else
            {
                //could be all planned, all actual
                cumChange = progressStocks.LastOrDefault();
                //or both
                if (progressStocks.Any(p => p.TargetType == TARGET_TYPES.benchmark.ToString()))
                {
                    if (progressStocks.Any(p2 => p2.TargetType == TARGET_TYPES.actual.ToString()))
                    {
                        //the last actual holds the cumulatives and that's all that is needed
                        cumChange = progressStocks.LastOrDefault(p => p.TargetType == TARGET_TYPES.actual.ToString());
                    }
                }
                if (cumChange != null)
                {
                    if (!string.IsNullOrEmpty(cumChange.TargetType))
                    {
                        if (cumChange.Progress1 != null)
                        {
                            AddCumulativeTotals(sb1Stock, cumChange.Progress1);
                            bHasTotals = true;
                        }
                    }
                }
            }
            return bHasTotals;
        }
        private static void AddCumulativeTotals(SB1Stock sb1Stock,
            SB1Progress1 cumulativeTotal)
        {
            if (sb1Stock.Progress1 == null)
            {
                sb1Stock.Progress1 = new SB1Progress1(cumulativeTotal.CalcParameters);
            }
            //mults already applied
            double dbMultiplier = 1;
            //need the labels and names and qunit; will change the qamount next
            AddSubTotalToTotalStock(sb1Stock.Progress1, dbMultiplier, cumulativeTotal);
            if (cumulativeTotal.TargetType == TARGET_TYPES.benchmark.ToString())
            {
                sb1Stock.Progress1.TSB1ScoreM = cumulativeTotal.TotalSBScorePCTotal;
                sb1Stock.Progress1.TSB1ScoreLAmount = cumulativeTotal.TotalSBScoreLPCTotal;
                sb1Stock.Progress1.TSB1ScoreUAmount = cumulativeTotal.TotalSBScoreUPCTotal;
                sb1Stock.Progress1.TSB1TMAmount1 = cumulativeTotal.TotalSB1PCTotal;
                sb1Stock.Progress1.TSB1TMAmount2 = cumulativeTotal.TotalSB2PCTotal;
                sb1Stock.Progress1.TSB1TMAmount3 = cumulativeTotal.TotalSB3PCTotal;
                sb1Stock.Progress1.TSB1TMAmount4 = cumulativeTotal.TotalSB4PCTotal;
                sb1Stock.Progress1.TSB1TMAmount5 = cumulativeTotal.TotalSB5PCTotal;
                sb1Stock.Progress1.TSB1TMAmount6 = cumulativeTotal.TotalSB6PCTotal;
                sb1Stock.Progress1.TSB1TMAmount7 = cumulativeTotal.TotalSB7PCTotal;
                sb1Stock.Progress1.TSB1TMAmount8 = cumulativeTotal.TotalSB8PCTotal;
                sb1Stock.Progress1.TSB1TMAmount9 = cumulativeTotal.TotalSB9PCTotal;
                sb1Stock.Progress1.TSB1TMAmount10 = cumulativeTotal.TotalSB10PCTotal;
            }
            else
            {
                //Set SBs regular cumulative totals
                sb1Stock.Progress1.TSB1ScoreM = cumulativeTotal.TotalSBScoreACTotal;
                sb1Stock.Progress1.TSB1ScoreLAmount = cumulativeTotal.TotalSBScoreLACTotal;
                sb1Stock.Progress1.TSB1ScoreUAmount = cumulativeTotal.TotalSBScoreUACTotal;
                sb1Stock.Progress1.TSB1TMAmount1 = cumulativeTotal.TotalSB1ACTotal;
                sb1Stock.Progress1.TSB1TMAmount2 = cumulativeTotal.TotalSB2ACTotal;
                sb1Stock.Progress1.TSB1TMAmount3 = cumulativeTotal.TotalSB3ACTotal;
                sb1Stock.Progress1.TSB1TMAmount4 = cumulativeTotal.TotalSB4ACTotal;
                sb1Stock.Progress1.TSB1TMAmount5 = cumulativeTotal.TotalSB5ACTotal;
                sb1Stock.Progress1.TSB1TMAmount6 = cumulativeTotal.TotalSB6ACTotal;
                sb1Stock.Progress1.TSB1TMAmount7 = cumulativeTotal.TotalSB7ACTotal;
                sb1Stock.Progress1.TSB1TMAmount8 = cumulativeTotal.TotalSB8ACTotal;
                sb1Stock.Progress1.TSB1TMAmount9 = cumulativeTotal.TotalSB9ACTotal;
                sb1Stock.Progress1.TSB1TMAmount10 = cumulativeTotal.TotalSB10ACTotal;
            }
        }
        private static void SetSBScoreProgress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //194 -TSB1TMAmount1 changed to TSB1ScoreM 
                    //planned period = planned.Progress1.TSB1TMAmount1
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1ScoreM;
                    //planned cumulative
                    planned.Progress1.TotalSBScorePCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSBScorePFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1ScoreM;
                    actual.Progress1.TotalSBScoreACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSBScoreAPTotal = actual.Progress1.TSB1ScoreM;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSBScorePCTotal = planned.Progress1.TotalSBScorePCTotal;
                        actual.Progress1.TotalSBScorePFTotal = planned.Progress1.TotalSBScorePFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSBScoreAPChange = actual.Progress1.TotalSBScoreAPTotal - actual.Progress1.TSB1ScoreM;
                    //cumulative change
                    actual.Progress1.TotalSBScoreACChange = actual.Progress1.TotalSBScoreACTotal - actual.Progress1.TotalSBScorePCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSBScorePPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreAPTotal, actual.Progress1.TSB1ScoreM);
                    actual.Progress1.TotalSBScorePCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreACTotal, actual.Progress1.TotalSBScorePCTotal);
                    actual.Progress1.TotalSBScorePFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreACTotal, actual.Progress1.TotalSBScorePFTotal);
                }
            }
        }
        private static void SetSBScoreLProgress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount1
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1ScoreLAmount;
                    //planned cumulative
                    planned.Progress1.TotalSBScoreLPCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSBScoreLPFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1ScoreLAmount;
                    actual.Progress1.TotalSBScoreLACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSBScoreLAPTotal = actual.Progress1.TSB1ScoreLAmount;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSBScoreLPCTotal = planned.Progress1.TotalSBScoreLPCTotal;
                        actual.Progress1.TotalSBScoreLPFTotal = planned.Progress1.TotalSBScoreLPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSBScoreLAPChange = actual.Progress1.TotalSBScoreLAPTotal - actual.Progress1.TSB1ScoreLAmount;
                    //cumulative change
                    actual.Progress1.TotalSBScoreLACChange = actual.Progress1.TotalSBScoreLACTotal - actual.Progress1.TotalSBScoreLPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSBScoreLPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreLAPTotal, actual.Progress1.TSB1ScoreLAmount);
                    actual.Progress1.TotalSBScoreLPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreLACTotal, actual.Progress1.TotalSBScoreLPCTotal);
                    actual.Progress1.TotalSBScoreLPFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreLACTotal, actual.Progress1.TotalSBScoreLPFTotal);
                }
            }
        }
        private static void SetSBScoreUProgress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount1
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1ScoreUAmount;
                    //planned cumulative
                    planned.Progress1.TotalSBScoreUPCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSBScoreUPFTotal = dbPlannedTotalQ;
                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1ScoreUAmount;
                    actual.Progress1.TotalSBScoreUACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSBScoreUAPTotal = actual.Progress1.TSB1ScoreUAmount;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSBScoreUPCTotal = planned.Progress1.TotalSBScoreUPCTotal;
                        actual.Progress1.TotalSBScoreUPFTotal = planned.Progress1.TotalSBScoreUPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSBScoreUAPChange = actual.Progress1.TotalSBScoreUAPTotal - actual.Progress1.TSB1ScoreUAmount;
                    //cumulative change
                    actual.Progress1.TotalSBScoreUACChange = actual.Progress1.TotalSBScoreUACTotal - actual.Progress1.TotalSBScoreUPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSBScoreUPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreUAPTotal, actual.Progress1.TSB1ScoreUAmount);
                    actual.Progress1.TotalSBScoreUPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreUACTotal, actual.Progress1.TotalSBScoreUPCTotal);
                    actual.Progress1.TotalSBScoreUPFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSBScoreUACTotal, actual.Progress1.TotalSBScoreUPFTotal);
                }
            }
        }
        private static void SetSB1Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount1
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount1;
                    //planned cumulative
                    planned.Progress1.TotalSB1PCTotal = dbPlannedTotalQ;
                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB1PFTotal = dbPlannedTotalQ;
                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount1;
                    actual.Progress1.TotalSB1ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB1APTotal = actual.Progress1.TSB1TMAmount1;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB1PCTotal = planned.Progress1.TotalSB1PCTotal;
                        actual.Progress1.TotalSB1PFTotal = planned.Progress1.TotalSB1PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB1APChange = actual.Progress1.TotalSB1APTotal - actual.Progress1.TSB1TMAmount1;
                    //cumulative change
                    actual.Progress1.TotalSB1ACChange = actual.Progress1.TotalSB1ACTotal - actual.Progress1.TotalSB1PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB1PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB1APTotal, actual.Progress1.TSB1TMAmount1);
                    actual.Progress1.TotalSB1PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB1ACTotal, actual.Progress1.TotalSB1PCTotal);
                    actual.Progress1.TotalSB1PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB1ACTotal, actual.Progress1.TotalSB1PFTotal);
                }
            }
        }
        private static void SetSB2Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount2
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount2;
                    //planned cumulative
                    planned.Progress1.TotalSB2PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB2PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount2;
                    actual.Progress1.TotalSB2ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB2APTotal = actual.Progress1.TSB1TMAmount2;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB2PCTotal = planned.Progress1.TotalSB2PCTotal;
                        actual.Progress1.TotalSB2PFTotal = planned.Progress1.TotalSB2PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB2APChange = actual.Progress1.TotalSB2APTotal - actual.Progress1.TSB1TMAmount2;
                    //cumulative change
                    actual.Progress1.TotalSB2ACChange = actual.Progress1.TotalSB2ACTotal - actual.Progress1.TotalSB2PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB2PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB2APTotal, actual.Progress1.TSB1TMAmount2);
                    actual.Progress1.TotalSB2PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB2ACTotal, actual.Progress1.TotalSB2PCTotal);
                    actual.Progress1.TotalSB2PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB2ACTotal, actual.Progress1.TotalSB2PFTotal);
                }
            }
        }
        private static void SetSB3Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount3
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount3;
                    //planned cumulative
                    planned.Progress1.TotalSB3PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB3PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount3;
                    actual.Progress1.TotalSB3ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB3APTotal = actual.Progress1.TSB1TMAmount3;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB3PCTotal = planned.Progress1.TotalSB3PCTotal;
                        actual.Progress1.TotalSB3PFTotal = planned.Progress1.TotalSB3PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB3APChange = actual.Progress1.TotalSB3APTotal - actual.Progress1.TSB1TMAmount3;
                    //cumulative change
                    actual.Progress1.TotalSB3ACChange = actual.Progress1.TotalSB3ACTotal - actual.Progress1.TotalSB3PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB3PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB3APTotal, actual.Progress1.TSB1TMAmount3);
                    actual.Progress1.TotalSB3PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB3ACTotal, actual.Progress1.TotalSB3PCTotal);
                    actual.Progress1.TotalSB3PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB3ACTotal, actual.Progress1.TotalSB3PFTotal);
                }
            }
        }
        private static void SetSB4Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount4
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount4;
                    //planned cumulative
                    planned.Progress1.TotalSB4PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB4PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount4;
                    actual.Progress1.TotalSB4ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB4APTotal = actual.Progress1.TSB1TMAmount4;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB4PCTotal = planned.Progress1.TotalSB4PCTotal;
                        actual.Progress1.TotalSB4PFTotal = planned.Progress1.TotalSB4PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB4APChange = actual.Progress1.TotalSB4APTotal - actual.Progress1.TSB1TMAmount4;
                    //cumulative change
                    actual.Progress1.TotalSB4ACChange = actual.Progress1.TotalSB4ACTotal - actual.Progress1.TotalSB4PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB4PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB4APTotal, actual.Progress1.TSB1TMAmount4);
                    actual.Progress1.TotalSB4PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB4ACTotal, actual.Progress1.TotalSB4PCTotal);
                    actual.Progress1.TotalSB4PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB4ACTotal, actual.Progress1.TotalSB4PFTotal);
                }
            }
        }
        private static void SetSB5Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount5
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount5;
                    //planned cumulative
                    planned.Progress1.TotalSB5PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB5PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount5;
                    actual.Progress1.TotalSB5ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB5APTotal = actual.Progress1.TSB1TMAmount5;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB5PCTotal = planned.Progress1.TotalSB5PCTotal;
                        actual.Progress1.TotalSB5PFTotal = planned.Progress1.TotalSB5PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB5APChange = actual.Progress1.TotalSB5APTotal - actual.Progress1.TSB1TMAmount5;
                    //cumulative change
                    actual.Progress1.TotalSB5ACChange = actual.Progress1.TotalSB5ACTotal - actual.Progress1.TotalSB5PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB5PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB5APTotal, actual.Progress1.TSB1TMAmount5);
                    actual.Progress1.TotalSB5PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB5ACTotal, actual.Progress1.TotalSB5PCTotal);
                    actual.Progress1.TotalSB5PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB5ACTotal, actual.Progress1.TotalSB5PFTotal);
                }
            }
        }
        private static void SetSB6Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount6
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount6;
                    //planned cumulative
                    planned.Progress1.TotalSB6PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB6PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount6;
                    actual.Progress1.TotalSB6ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB6APTotal = actual.Progress1.TSB1TMAmount6;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB6PCTotal = planned.Progress1.TotalSB6PCTotal;
                        actual.Progress1.TotalSB6PFTotal = planned.Progress1.TotalSB6PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB6APChange = actual.Progress1.TotalSB6APTotal - actual.Progress1.TSB1TMAmount6;
                    //cumulative change
                    actual.Progress1.TotalSB6ACChange = actual.Progress1.TotalSB6ACTotal - actual.Progress1.TotalSB6PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB6PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB6APTotal, actual.Progress1.TSB1TMAmount6);
                    actual.Progress1.TotalSB6PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB6ACTotal, actual.Progress1.TotalSB6PCTotal);
                    actual.Progress1.TotalSB6PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB6ACTotal, actual.Progress1.TotalSB6PFTotal);
                }
            }
        }
        private static void SetSB7Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount7
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount7;
                    //planned cumulative
                    planned.Progress1.TotalSB7PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB7PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount7;
                    actual.Progress1.TotalSB7ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB7APTotal = actual.Progress1.TSB1TMAmount7;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB7PCTotal = planned.Progress1.TotalSB7PCTotal;
                        actual.Progress1.TotalSB7PFTotal = planned.Progress1.TotalSB7PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB7APChange = actual.Progress1.TotalSB7APTotal - actual.Progress1.TSB1TMAmount7;
                    //cumulative change
                    actual.Progress1.TotalSB7ACChange = actual.Progress1.TotalSB7ACTotal - actual.Progress1.TotalSB7PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB7PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB7APTotal, actual.Progress1.TSB1TMAmount7);
                    actual.Progress1.TotalSB7PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB7ACTotal, actual.Progress1.TotalSB7PCTotal);
                    actual.Progress1.TotalSB7PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB7ACTotal, actual.Progress1.TotalSB7PFTotal);
                }
            }
        }
        private static void SetSB8Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount8
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount8;
                    //planned cumulative
                    planned.Progress1.TotalSB8PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB8PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount8;
                    actual.Progress1.TotalSB8ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB8APTotal = actual.Progress1.TSB1TMAmount8;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB8PCTotal = planned.Progress1.TotalSB8PCTotal;
                        actual.Progress1.TotalSB8PFTotal = planned.Progress1.TotalSB8PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB8APChange = actual.Progress1.TotalSB8APTotal - actual.Progress1.TSB1TMAmount8;
                    //cumulative change
                    actual.Progress1.TotalSB8ACChange = actual.Progress1.TotalSB8ACTotal - actual.Progress1.TotalSB8PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB8PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB8APTotal, actual.Progress1.TSB1TMAmount8);
                    actual.Progress1.TotalSB8PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB8ACTotal, actual.Progress1.TotalSB8PCTotal);
                    actual.Progress1.TotalSB8PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB8ACTotal, actual.Progress1.TotalSB8PFTotal);
                }
            }
        }
        private static void SetSB9Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount9
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount9;
                    //planned cumulative
                    planned.Progress1.TotalSB9PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB9PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount9;
                    actual.Progress1.TotalSB9ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB9APTotal = actual.Progress1.TSB1TMAmount9;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB9PCTotal = planned.Progress1.TotalSB9PCTotal;
                        actual.Progress1.TotalSB9PFTotal = planned.Progress1.TotalSB9PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB9APChange = actual.Progress1.TotalSB9APTotal - actual.Progress1.TSB1TMAmount9;
                    //cumulative change
                    actual.Progress1.TotalSB9ACChange = actual.Progress1.TotalSB9ACTotal - actual.Progress1.TotalSB9PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB9PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB9APTotal, actual.Progress1.TSB1TMAmount9);
                    actual.Progress1.TotalSB9PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB9ACTotal, actual.Progress1.TotalSB9PCTotal);
                    actual.Progress1.TotalSB9PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB9ACTotal, actual.Progress1.TotalSB9PFTotal);
                }
            }
        }
        private static void SetSB10Progress(SB1Stock sb1Stock, List<SB1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TSB1TMAmount10
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TSB1TMAmount10;
                    //planned cumulative
                    planned.Progress1.TotalSB10PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (SB1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalSB10PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (SB1Stock actual in progressStocks.OrderBy(c => c.Date))
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TSB1TMAmount10;
                    actual.Progress1.TotalSB10ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalSB10APTotal = actual.Progress1.TSB1TMAmount10;
                    //set the corresponding planned totals
                    SB1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalSB10PCTotal = planned.Progress1.TotalSB10PCTotal;
                        actual.Progress1.TotalSB10PFTotal = planned.Progress1.TotalSB10PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalSB10APChange = actual.Progress1.TotalSB10APTotal - actual.Progress1.TSB1TMAmount10;
                    //cumulative change
                    actual.Progress1.TotalSB10ACChange = actual.Progress1.TotalSB10ACTotal - actual.Progress1.TotalSB10PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalSB10PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB10APTotal, actual.Progress1.TSB1TMAmount10);
                    actual.Progress1.TotalSB10PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB10ACTotal, actual.Progress1.TotalSB10PCTotal);
                    actual.Progress1.TotalSB10PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalSB10ACTotal, actual.Progress1.TotalSB10PFTotal);
                }
            }
        }
        private static SB1Stock GetProgressStockByLabel(SB1Stock actual, List<int> ids,
            List<SB1Stock> progressStocks, string targetType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            SB1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (progressStocks.Any(p => p.Label == actual.Label
                && p.TargetType == targetType))
            {
                int iIndex = 1;
                foreach (SB1Stock planned in progressStocks)
                {
                    if (planned.TargetType == targetType)
                    {
                        if (actual.Label == planned.Label)
                        {
                            //make sure it hasn't already been used (2 or more els with same Labels)
                            if (!ids.Any(i => i == iIndex))
                            {
                                plannedMatch = planned;
                                //index based check is ok
                                ids.Add(iIndex);
                                //break the for loop
                                break;
                            }
                            else
                            {
                                bool bHasMatch = HasProgressMatchByLabel(actual.Label, planned,
                                    progressStocks, targetType);
                                if (!bHasMatch)
                                {
                                    //if no match use the last one (i.e. input series with 1 bm and 20 actuals)
                                   plannedMatch = progressStocks.LastOrDefault(p => p.Label == actual.Label
                                        && p.TargetType == targetType);
                                   break;
                                }
                            }
                        }
                    }
                    iIndex++;
                }
            }
            return plannedMatch;
        }
        private static bool HasProgressMatchByLabel(string aggLabel,
            SB1Stock planned, List<SB1Stock> progressStocks, string targetType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (SB1Stock rp in progressStocks)
            {
                if (rp.TargetType == targetType)
                {
                    if (bStart)
                    {
                        if (aggLabel == planned.Label)
                        {
                            bHasMatch = true;
                            break;
                        }
                    }
                    if (rp.Id == planned.Id)
                    {
                        bStart = true;
                    }
                }
            }
            return bHasMatch;
        }
        
        private bool AddProgsToBaseStock(SB1Stock sb1Stock,
            List<SB1Stock> progressStocks)
        {
            bool bHasAnalyses = false;
            sb1Stock.Stocks = new List<SB1Stock>();
            foreach (SB1Stock actual in progressStocks)
            {
                sb1Stock.Stocks.Add(actual);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }
    }
}
