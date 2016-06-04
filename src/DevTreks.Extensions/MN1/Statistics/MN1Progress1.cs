using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Typical Object model: 
    ///             The class measures planned vs actual progress.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class MN1Progress1 : MN1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public MN1Progress1(CalculatorParameters calcs)
            : base()
        {
            //subprice object
            InitTotalMN1Progress1Properties(this, calcs);
        }
        //copy constructor
        public MN1Progress1(MN1Progress1 calculator)
            : base(calculator)
        {
            CopyTotalMN1Progress1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent MN1Stock
        //calculator properties
        //totals names must be consistent with Total1
        //planned period
        public double TotalMN1Q { get; set; }
        public string TotalMN1Name { get; set; }
        //planned full (sum of all planning periods)
        public double TotalMN1PFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalMN1PCTotal { get; set; }
        //actual period
        public double TotalMN1APTotal { get; set; }
        //actual cumulative 
        public double TotalMN1ACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalMN1APChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalMN1ACChange { get; set; }
        //planned period
        public double TotalMN1PPPercent { get; set; }
        //planned cumulative
        public double TotalMN1PCPercent { get; set; }
        //planned full
        public double TotalMN1PFPercent { get; set; }

        public double TotalMN2Q { get; set; }
        public string TotalMN2Name { get; set; }
        public double TotalMN2PFTotal { get; set; }
        public double TotalMN2PCTotal { get; set; }
        public double TotalMN2APTotal { get; set; }
        public double TotalMN2ACTotal { get; set; }
        public double TotalMN2APChange { get; set; }
        public double TotalMN2ACChange { get; set; }
        public double TotalMN2PPPercent { get; set; }
        public double TotalMN2PCPercent { get; set; }
        public double TotalMN2PFPercent { get; set; }

        public double TotalMN3Q { get; set; }
        public string TotalMN3Name { get; set; }
        public double TotalMN3PFTotal { get; set; }
        public double TotalMN3PCTotal { get; set; }
        public double TotalMN3APTotal { get; set; }
        public double TotalMN3ACTotal { get; set; }
        public double TotalMN3APChange { get; set; }
        public double TotalMN3ACChange { get; set; }
        public double TotalMN3PPPercent { get; set; }
        public double TotalMN3PCPercent { get; set; }
        public double TotalMN3PFPercent { get; set; }

        public double TotalMN4Q { get; set; }
        public string TotalMN4Name { get; set; }
        public double TotalMN4PFTotal { get; set; }
        public double TotalMN4PCTotal { get; set; }
        public double TotalMN4APTotal { get; set; }
        public double TotalMN4ACTotal { get; set; }
        public double TotalMN4APChange { get; set; }
        public double TotalMN4ACChange { get; set; }
        public double TotalMN4PPPercent { get; set; }
        public double TotalMN4PCPercent { get; set; }
        public double TotalMN4PFPercent { get; set; }

        public double TotalMN5Q { get; set; }
        public string TotalMN5Name { get; set; }
        public double TotalMN5PFTotal { get; set; }
        public double TotalMN5PCTotal { get; set; }
        public double TotalMN5APTotal { get; set; }
        public double TotalMN5ACTotal { get; set; }
        public double TotalMN5APChange { get; set; }
        public double TotalMN5ACChange { get; set; }
        public double TotalMN5PPPercent { get; set; }
        public double TotalMN5PCPercent { get; set; }
        public double TotalMN5PFPercent { get; set; }

        public double TotalMN6Q { get; set; }
        public string TotalMN6Name { get; set; }
        public double TotalMN6PFTotal { get; set; }
        public double TotalMN6PCTotal { get; set; }
        public double TotalMN6APTotal { get; set; }
        public double TotalMN6ACTotal { get; set; }
        public double TotalMN6APChange { get; set; }
        public double TotalMN6ACChange { get; set; }
        public double TotalMN6PPPercent { get; set; }
        public double TotalMN6PCPercent { get; set; }
        public double TotalMN6PFPercent { get; set; }

        private const string cTotalMN1Q = "TMN1Q";
        private const string cTotalMN1Name = "TMN1Name";
        private const string cTotalMN1PFTotal = "TMN1PFTotal";
        private const string cTotalMN1PCTotal = "TMN1PCTotal";
        private const string cTotalMN1APTotal = "TMN1APTotal";
        private const string cTotalMN1ACTotal = "TMN1ACTotal";
        private const string cTotalMN1APChange = "TMN1APChange";
        private const string cTotalMN1ACChange = "TMN1ACChange";
        private const string cTotalMN1PPPercent = "TMN1PPPercent";
        private const string cTotalMN1PCPercent = "TMN1PCPercent";
        private const string cTotalMN1PFPercent = "TMN1PFPercent";

        private const string cTotalMN2Q = "TMN2Q";
        private const string cTotalMN2Name = "TMN2Name";
        private const string cTotalMN2PFTotal = "TMN2PFTotal";
        private const string cTotalMN2PCTotal = "TMN2PCTotal";
        private const string cTotalMN2APTotal = "TMN2APTotal";
        private const string cTotalMN2ACTotal = "TMN2ACTotal";
        private const string cTotalMN2APChange = "TMN2APChange";
        private const string cTotalMN2ACChange = "TMN2ACChange";
        private const string cTotalMN2PPPercent = "TMN2PPPercent";
        private const string cTotalMN2PCPercent = "TMN2PCPercent";
        private const string cTotalMN2PFPercent = "TMN2PFPercent";

        private const string cTotalMN3Q = "TMN3Q";
        private const string cTotalMN3Name = "TMN3Name";
        private const string cTotalMN3PFTotal = "TMN3PFTotal";
        private const string cTotalMN3PCTotal = "TMN3PCTotal";
        private const string cTotalMN3APTotal = "TMN3APTotal";
        private const string cTotalMN3ACTotal = "TMN3ACTotal";
        private const string cTotalMN3APChange = "TMN3APChange";
        private const string cTotalMN3ACChange = "TMN3ACChange";
        private const string cTotalMN3PPPercent = "TMN3PPPercent";
        private const string cTotalMN3PCPercent = "TMN3PCPercent";
        private const string cTotalMN3PFPercent = "TMN3PFPercent";

        private const string cTotalMN4Q = "TMN4Q";
        private const string cTotalMN4Name = "TMN4Name";
        private const string cTotalMN4PFTotal = "TMN4PFTotal";
        private const string cTotalMN4PCTotal = "TMN4PCTotal";
        private const string cTotalMN4APTotal = "TMN4APTotal";
        private const string cTotalMN4ACTotal = "TMN4ACTotal";
        private const string cTotalMN4APChange = "TMN4APChange";
        private const string cTotalMN4ACChange = "TMN4ACChange";
        private const string cTotalMN4PPPercent = "TMN4PPPercent";
        private const string cTotalMN4PCPercent = "TMN4PCPercent";
        private const string cTotalMN4PFPercent = "TMN4PFPercent";

        private const string cTotalMN5Q = "TMN5Q";
        private const string cTotalMN5Name = "TMN5Name";
        private const string cTotalMN5PFTotal = "TMN5PFTotal";
        private const string cTotalMN5PCTotal = "TMN5PCTotal";
        private const string cTotalMN5APTotal = "TMN5APTotal";
        private const string cTotalMN5ACTotal = "TMN5ACTotal";
        private const string cTotalMN5APChange = "TMN5APChange";
        private const string cTotalMN5ACChange = "TMN5ACChange";
        private const string cTotalMN5PPPercent = "TMN5PPPercent";
        private const string cTotalMN5PCPercent = "TMN5PCPercent";
        private const string cTotalMN5PFPercent = "TMN5PFPercent";

        private const string cTotalMN6Q = "TMN6Q";
        private const string cTotalMN6Name = "TMN6Name";
        private const string cTotalMN6PFTotal = "TMN6PFTotal";
        private const string cTotalMN6PCTotal = "TMN6PCTotal";
        private const string cTotalMN6APTotal = "TMN6APTotal";
        private const string cTotalMN6ACTotal = "TMN6ACTotal";
        private const string cTotalMN6APChange = "TMN6APChange";
        private const string cTotalMN6ACChange = "TMN6ACChange";
        private const string cTotalMN6PPPercent = "TMN6PPPercent";
        private const string cTotalMN6PCPercent = "TMN6PCPercent";
        private const string cTotalMN6PFPercent = "TMN6PFPercent";
        
        public double TotalMN7Q { get; set; }
        public string TotalMN7Name { get; set; }
        public double TotalMN7PFTotal { get; set; }
        public double TotalMN7PCTotal { get; set; }
        public double TotalMN7APTotal { get; set; }
        public double TotalMN7ACTotal { get; set; }
        public double TotalMN7APChange { get; set; }
        public double TotalMN7ACChange { get; set; }
        public double TotalMN7PPPercent { get; set; }
        public double TotalMN7PCPercent { get; set; }
        public double TotalMN7PFPercent { get; set; }
        
        public double TotalMN8Q { get; set; }
        public string TotalMN8Name { get; set; }
        public double TotalMN8PFTotal { get; set; }
        public double TotalMN8PCTotal { get; set; }
        public double TotalMN8APTotal { get; set; }
        public double TotalMN8ACTotal { get; set; }
        public double TotalMN8APChange { get; set; }
        public double TotalMN8ACChange { get; set; }
        public double TotalMN8PPPercent { get; set; }
        public double TotalMN8PCPercent { get; set; }
        public double TotalMN8PFPercent { get; set; }

        public double TotalMN9Q { get; set; }
        public string TotalMN9Name { get; set; }
        public double TotalMN9PFTotal { get; set; }
        public double TotalMN9PCTotal { get; set; }
        public double TotalMN9APTotal { get; set; }
        public double TotalMN9ACTotal { get; set; }
        public double TotalMN9APChange { get; set; }
        public double TotalMN9ACChange { get; set; }
        public double TotalMN9PPPercent { get; set; }
        public double TotalMN9PCPercent { get; set; }
        public double TotalMN9PFPercent { get; set; }

        public double TotalMN10Q { get; set; }
        public string TotalMN10Name { get; set; }
        public double TotalMN10PFTotal { get; set; }
        public double TotalMN10PCTotal { get; set; }
        public double TotalMN10APTotal { get; set; }
        public double TotalMN10ACTotal { get; set; }
        public double TotalMN10APChange { get; set; }
        public double TotalMN10ACChange { get; set; }
        public double TotalMN10PPPercent { get; set; }
        public double TotalMN10PCPercent { get; set; }
        public double TotalMN10PFPercent { get; set; }

        private const string cTotalMN7Q = "TMN7Q";
        private const string cTotalMN7Name = "TMN7Name";
        private const string cTotalMN7PFTotal = "TMN7PFTotal";
        private const string cTotalMN7PCTotal = "TMN7PCTotal";
        private const string cTotalMN7APTotal = "TMN7APTotal";
        private const string cTotalMN7ACTotal = "TMN7ACTotal";
        private const string cTotalMN7APChange = "TMN7APChange";
        private const string cTotalMN7ACChange = "TMN7ACChange";
        private const string cTotalMN7PPPercent = "TMN7PPPercent";
        private const string cTotalMN7PCPercent = "TMN7PCPercent";
        private const string cTotalMN7PFPercent = "TMN7PFPercent";

        private const string cTotalMN8Q = "TMN8Q";
        private const string cTotalMN8Name = "TMN8Name";
        private const string cTotalMN8PFTotal = "TMN8PFTotal";
        private const string cTotalMN8PCTotal = "TMN8PCTotal";
        private const string cTotalMN8APTotal = "TMN8APTotal";
        private const string cTotalMN8ACTotal = "TMN8ACTotal";
        private const string cTotalMN8APChange = "TMN8APChange";
        private const string cTotalMN8ACChange = "TMN8ACChange";
        private const string cTotalMN8PPPercent = "TMN8PPPercent";
        private const string cTotalMN8PCPercent = "TMN8PCPercent";
        private const string cTotalMN8PFPercent = "TMN8PFPercent";

        private const string cTotalMN9Q = "TMN9Q";
        private const string cTotalMN9Name = "TMN9Name";
        private const string cTotalMN9PFTotal = "TMN9PFTotal";
        private const string cTotalMN9PCTotal = "TMN9PCTotal";
        private const string cTotalMN9APTotal = "TMN9APTotal";
        private const string cTotalMN9ACTotal = "TMN9ACTotal";
        private const string cTotalMN9APChange = "TMN9APChange";
        private const string cTotalMN9ACChange = "TMN9ACChange";
        private const string cTotalMN9PPPercent = "TMN9PPPercent";
        private const string cTotalMN9PCPercent = "TMN9PCPercent";
        private const string cTotalMN9PFPercent = "TMN9PFPercent";

        private const string cTotalMN10Q = "TMN10Q";
        private const string cTotalMN10Name = "TMN10Name";
        private const string cTotalMN10PFTotal = "TMN10PFTotal";
        private const string cTotalMN10PCTotal = "TMN10PCTotal";
        private const string cTotalMN10APTotal = "TMN10APTotal";
        private const string cTotalMN10ACTotal = "TMN10ACTotal";
        private const string cTotalMN10APChange = "TMN10APChange";
        private const string cTotalMN10ACChange = "TMN10ACChange";
        private const string cTotalMN10PPPercent = "TMN10PPPercent";
        private const string cTotalMN10PCPercent = "TMN10PCPercent";
        private const string cTotalMN10PFPercent = "TMN10PFPercent";

        public void InitTotalMN1Progress1Properties(MN1Progress1 ind, CalculatorParameters calcs)
        {
            ind.ErrorMessage = string.Empty;

            ind.TotalMN1Q = 0;
            ind.TotalMN1Name = string.Empty;
            ind.TotalMN1PFTotal = 0;
            ind.TotalMN1PCTotal = 0;
            ind.TotalMN1APTotal = 0;
            ind.TotalMN1ACTotal = 0;
            ind.TotalMN1APChange = 0;
            ind.TotalMN1ACChange = 0;
            ind.TotalMN1PPPercent = 0;
            ind.TotalMN1PCPercent = 0;
            ind.TotalMN1PFPercent = 0;

            ind.TotalMN2Q = 0;
            ind.TotalMN2Name = string.Empty;
            ind.TotalMN2PFTotal = 0;
            ind.TotalMN2PCTotal = 0;
            ind.TotalMN2APTotal = 0;
            ind.TotalMN2ACTotal = 0;
            ind.TotalMN2APChange = 0;
            ind.TotalMN2ACChange = 0;
            ind.TotalMN2PPPercent = 0;
            ind.TotalMN2PCPercent = 0;
            ind.TotalMN2PFPercent = 0;

            ind.TotalMN3Q = 0;
            ind.TotalMN3Name = string.Empty;
            ind.TotalMN3PFTotal = 0;
            ind.TotalMN3PCTotal = 0;
            ind.TotalMN3APTotal = 0;
            ind.TotalMN3ACTotal = 0;
            ind.TotalMN3APChange = 0;
            ind.TotalMN3ACChange = 0;
            ind.TotalMN3PPPercent = 0;
            ind.TotalMN3PCPercent = 0;
            ind.TotalMN3PFPercent = 0;

            ind.TotalMN4Q = 0;
            ind.TotalMN4Name = string.Empty;
            ind.TotalMN4PFTotal = 0;
            ind.TotalMN4PCTotal = 0;
            ind.TotalMN4APTotal = 0;
            ind.TotalMN4ACTotal = 0;
            ind.TotalMN4APChange = 0;
            ind.TotalMN4ACChange = 0;
            ind.TotalMN4PPPercent = 0;
            ind.TotalMN4PCPercent = 0;
            ind.TotalMN4PFPercent = 0;

            ind.TotalMN5Q = 0;
            ind.TotalMN5Name = string.Empty;
            ind.TotalMN5PFTotal = 0;
            ind.TotalMN5PCTotal = 0;
            ind.TotalMN5APTotal = 0;
            ind.TotalMN5ACTotal = 0;
            ind.TotalMN5APChange = 0;
            ind.TotalMN5ACChange = 0;
            ind.TotalMN5PPPercent = 0;
            ind.TotalMN5PCPercent = 0;
            ind.TotalMN5PFPercent = 0;

            ind.TotalMN6Q = 0;
            ind.TotalMN6Name = string.Empty;
            ind.TotalMN6PFTotal = 0;
            ind.TotalMN6PCTotal = 0;
            ind.TotalMN6APTotal = 0;
            ind.TotalMN6ACTotal = 0;
            ind.TotalMN6APChange = 0;
            ind.TotalMN6ACChange = 0;
            ind.TotalMN6PPPercent = 0;
            ind.TotalMN6PCPercent = 0;
            ind.TotalMN6PFPercent = 0;

            ind.TotalMN7Q = 0;
            ind.TotalMN7Name = string.Empty;
            ind.TotalMN7PFTotal = 0;
            ind.TotalMN7PCTotal = 0;
            ind.TotalMN7APTotal = 0;
            ind.TotalMN7ACTotal = 0;
            ind.TotalMN7APChange = 0;
            ind.TotalMN7ACChange = 0;
            ind.TotalMN7PPPercent = 0;
            ind.TotalMN7PCPercent = 0;
            ind.TotalMN7PFPercent = 0;

            ind.TotalMN8Q = 0;
            ind.TotalMN8Name = string.Empty;
            ind.TotalMN8PFTotal = 0;
            ind.TotalMN8PCTotal = 0;
            ind.TotalMN8APTotal = 0;
            ind.TotalMN8ACTotal = 0;
            ind.TotalMN8APChange = 0;
            ind.TotalMN8ACChange = 0;
            ind.TotalMN8PPPercent = 0;
            ind.TotalMN8PCPercent = 0;
            ind.TotalMN8PFPercent = 0;

            ind.TotalMN9Q = 0;
            ind.TotalMN9Name = string.Empty;
            ind.TotalMN9PFTotal = 0;
            ind.TotalMN9PCTotal = 0;
            ind.TotalMN9APTotal = 0;
            ind.TotalMN9ACTotal = 0;
            ind.TotalMN9APChange = 0;
            ind.TotalMN9ACChange = 0;
            ind.TotalMN9PPPercent = 0;
            ind.TotalMN9PCPercent = 0;
            ind.TotalMN9PFPercent = 0;

            ind.TotalMN10Q = 0;
            ind.TotalMN10Name = string.Empty;
            ind.TotalMN10PFTotal = 0;
            ind.TotalMN10PCTotal = 0;
            ind.TotalMN10APTotal = 0;
            ind.TotalMN10ACTotal = 0;
            ind.TotalMN10APChange = 0;
            ind.TotalMN10ACChange = 0;
            ind.TotalMN10PPPercent = 0;
            ind.TotalMN10PCPercent = 0;
            ind.TotalMN10PFPercent = 0;
            ind.CalcParameters = calcs;
            ind.MNSR1Stock = new MNSR01Stock();
            ind.MNSR2Stock = new MNSR02Stock();
        }

        public void CopyTotalMN1Progress1Properties(MN1Progress1 ind,
            MN1Progress1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalMN1Q = calculator.TotalMN1Q;
            ind.TotalMN1Name = calculator.TotalMN1Name;
            ind.TotalMN1PFTotal = calculator.TotalMN1PFTotal;
            ind.TotalMN1PCTotal = calculator.TotalMN1PCTotal;
            ind.TotalMN1APTotal = calculator.TotalMN1APTotal;
            ind.TotalMN1ACTotal = calculator.TotalMN1ACTotal;
            ind.TotalMN1APChange = calculator.TotalMN1APChange;
            ind.TotalMN1ACChange = calculator.TotalMN1ACChange;
            ind.TotalMN1PPPercent = calculator.TotalMN1PPPercent;
            ind.TotalMN1PCPercent = calculator.TotalMN1PCPercent;
            ind.TotalMN1PFPercent = calculator.TotalMN1PFPercent;

            ind.TotalMN2Q = calculator.TotalMN2Q;
            ind.TotalMN2Name = calculator.TotalMN2Name;
            ind.TotalMN2PFTotal = calculator.TotalMN2PFTotal;
            ind.TotalMN2PCTotal = calculator.TotalMN2PCTotal;
            ind.TotalMN2APTotal = calculator.TotalMN2APTotal;
            ind.TotalMN2ACTotal = calculator.TotalMN2ACTotal;
            ind.TotalMN2APChange = calculator.TotalMN2APChange;
            ind.TotalMN2ACChange = calculator.TotalMN2ACChange;
            ind.TotalMN2PPPercent = calculator.TotalMN2PPPercent;
            ind.TotalMN2PCPercent = calculator.TotalMN2PCPercent;
            ind.TotalMN2PFPercent = calculator.TotalMN2PFPercent;

            ind.TotalMN3Q = calculator.TotalMN3Q;
            ind.TotalMN3Name = calculator.TotalMN3Name;
            ind.TotalMN3PFTotal = calculator.TotalMN3PFTotal;
            ind.TotalMN3PCTotal = calculator.TotalMN3PCTotal;
            ind.TotalMN3APTotal = calculator.TotalMN3APTotal;
            ind.TotalMN3ACTotal = calculator.TotalMN3ACTotal;
            ind.TotalMN3APChange = calculator.TotalMN3APChange;
            ind.TotalMN3ACChange = calculator.TotalMN3ACChange;
            ind.TotalMN3PPPercent = calculator.TotalMN3PPPercent;
            ind.TotalMN3PCPercent = calculator.TotalMN3PCPercent;
            ind.TotalMN3PFPercent = calculator.TotalMN3PFPercent;

            ind.TotalMN4Q = calculator.TotalMN4Q;
            ind.TotalMN4Name = calculator.TotalMN4Name;
            ind.TotalMN4PFTotal = calculator.TotalMN4PFTotal;
            ind.TotalMN4PCTotal = calculator.TotalMN4PCTotal;
            ind.TotalMN4APTotal = calculator.TotalMN4APTotal;
            ind.TotalMN4ACTotal = calculator.TotalMN4ACTotal;
            ind.TotalMN4APChange = calculator.TotalMN4APChange;
            ind.TotalMN4ACChange = calculator.TotalMN4ACChange;
            ind.TotalMN4PPPercent = calculator.TotalMN4PPPercent;
            ind.TotalMN4PCPercent = calculator.TotalMN4PCPercent;
            ind.TotalMN4PFPercent = calculator.TotalMN4PFPercent;

            ind.TotalMN5Q = calculator.TotalMN5Q;
            ind.TotalMN5Name = calculator.TotalMN5Name;
            ind.TotalMN5PFTotal = calculator.TotalMN5PFTotal;
            ind.TotalMN5PCTotal = calculator.TotalMN5PCTotal;
            ind.TotalMN5APTotal = calculator.TotalMN5APTotal;
            ind.TotalMN5ACTotal = calculator.TotalMN5ACTotal;
            ind.TotalMN5APChange = calculator.TotalMN5APChange;
            ind.TotalMN5ACChange = calculator.TotalMN5ACChange;
            ind.TotalMN5PPPercent = calculator.TotalMN5PPPercent;
            ind.TotalMN5PCPercent = calculator.TotalMN5PCPercent;
            ind.TotalMN5PFPercent = calculator.TotalMN5PFPercent;

            ind.TotalMN6Q = calculator.TotalMN6Q;
            ind.TotalMN6Name = calculator.TotalMN6Name;
            ind.TotalMN6PFTotal = calculator.TotalMN6PFTotal;
            ind.TotalMN6PCTotal = calculator.TotalMN6PCTotal;
            ind.TotalMN6APTotal = calculator.TotalMN6APTotal;
            ind.TotalMN6ACTotal = calculator.TotalMN6ACTotal;
            ind.TotalMN6APChange = calculator.TotalMN6APChange;
            ind.TotalMN6ACChange = calculator.TotalMN6ACChange;
            ind.TotalMN6PPPercent = calculator.TotalMN6PPPercent;
            ind.TotalMN6PCPercent = calculator.TotalMN6PCPercent;
            ind.TotalMN6PFPercent = calculator.TotalMN6PFPercent;

            ind.TotalMN7Q = calculator.TotalMN7Q;
            ind.TotalMN7Name = calculator.TotalMN7Name;
            ind.TotalMN7PFTotal = calculator.TotalMN7PFTotal;
            ind.TotalMN7PCTotal = calculator.TotalMN7PCTotal;
            ind.TotalMN7APTotal = calculator.TotalMN7APTotal;
            ind.TotalMN7ACTotal = calculator.TotalMN7ACTotal;
            ind.TotalMN7APChange = calculator.TotalMN7APChange;
            ind.TotalMN7ACChange = calculator.TotalMN7ACChange;
            ind.TotalMN7PPPercent = calculator.TotalMN7PPPercent;
            ind.TotalMN7PCPercent = calculator.TotalMN7PCPercent;
            ind.TotalMN7PFPercent = calculator.TotalMN7PFPercent;

            ind.TotalMN8Q = calculator.TotalMN8Q;
            ind.TotalMN8Name = calculator.TotalMN8Name;
            ind.TotalMN8PFTotal = calculator.TotalMN8PFTotal;
            ind.TotalMN8PCTotal = calculator.TotalMN8PCTotal;
            ind.TotalMN8APTotal = calculator.TotalMN8APTotal;
            ind.TotalMN8ACTotal = calculator.TotalMN8ACTotal;
            ind.TotalMN8APChange = calculator.TotalMN8APChange;
            ind.TotalMN8ACChange = calculator.TotalMN8ACChange;
            ind.TotalMN8PPPercent = calculator.TotalMN8PPPercent;
            ind.TotalMN8PCPercent = calculator.TotalMN8PCPercent;
            ind.TotalMN8PFPercent = calculator.TotalMN8PFPercent;

            ind.TotalMN9Q = calculator.TotalMN9Q;
            ind.TotalMN9Name = calculator.TotalMN9Name;
            ind.TotalMN9PFTotal = calculator.TotalMN9PFTotal;
            ind.TotalMN9PCTotal = calculator.TotalMN9PCTotal;
            ind.TotalMN9APTotal = calculator.TotalMN9APTotal;
            ind.TotalMN9ACTotal = calculator.TotalMN9ACTotal;
            ind.TotalMN9APChange = calculator.TotalMN9APChange;
            ind.TotalMN9ACChange = calculator.TotalMN9ACChange;
            ind.TotalMN9PPPercent = calculator.TotalMN9PPPercent;
            ind.TotalMN9PCPercent = calculator.TotalMN9PCPercent;
            ind.TotalMN9PFPercent = calculator.TotalMN9PFPercent;

            ind.TotalMN10Q = calculator.TotalMN10Q;
            ind.TotalMN10Name = calculator.TotalMN10Name;
            ind.TotalMN10PFTotal = calculator.TotalMN10PFTotal;
            ind.TotalMN10PCTotal = calculator.TotalMN10PCTotal;
            ind.TotalMN10APTotal = calculator.TotalMN10APTotal;
            ind.TotalMN10ACTotal = calculator.TotalMN10ACTotal;
            ind.TotalMN10APChange = calculator.TotalMN10APChange;
            ind.TotalMN10ACChange = calculator.TotalMN10ACChange;
            ind.TotalMN10PPPercent = calculator.TotalMN10PPPercent;
            ind.TotalMN10PCPercent = calculator.TotalMN10PCPercent;
            ind.TotalMN10PFPercent = calculator.TotalMN10PFPercent;
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            if (calculator.MNSR1Stock == null)
                calculator.MNSR1Stock = new MNSR01Stock();
            if (ind.MNSR1Stock == null)
                ind.MNSR1Stock = new MNSR01Stock();
            ind.MNSR1Stock.CopyTotalMNSR01StockProperties(calculator.MNSR1Stock);
            if (calculator.MNSR2Stock == null)
                calculator.MNSR2Stock = new MNSR02Stock();
            if (ind.MNSR2Stock == null)
                ind.MNSR2Stock = new MNSR02Stock();
            ind.MNSR2Stock.CopyTotalMNSR02StockProperties(calculator.MNSR2Stock);
        }

        public void SetTotalMN1Progress1Properties(MN1Progress1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalMN1Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1Q, attNameExtension));
            ind.TotalMN1Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN1Name, attNameExtension));
            ind.TotalMN1PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1PFTotal, attNameExtension));
            ind.TotalMN1PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1PCTotal, attNameExtension));
            ind.TotalMN1APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1APTotal, attNameExtension));
            ind.TotalMN1ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1ACTotal, attNameExtension));
            ind.TotalMN1APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1APChange, attNameExtension));
            ind.TotalMN1ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1ACChange, attNameExtension));
            ind.TotalMN1PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1PPPercent, attNameExtension));
            ind.TotalMN1PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1PCPercent, attNameExtension));
            ind.TotalMN1PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1PFPercent, attNameExtension));

            ind.TotalMN2Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2Q, attNameExtension));
            ind.TotalMN2Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN2Name, attNameExtension));
            ind.TotalMN2PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2PFTotal, attNameExtension));
            ind.TotalMN2PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2PCTotal, attNameExtension));
            ind.TotalMN2APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2APTotal, attNameExtension));
            ind.TotalMN2ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2ACTotal, attNameExtension));
            ind.TotalMN2APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2APChange, attNameExtension));
            ind.TotalMN2ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2ACChange, attNameExtension));
            ind.TotalMN2PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2PPPercent, attNameExtension));
            ind.TotalMN2PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2PCPercent, attNameExtension));
            ind.TotalMN2PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2PFPercent, attNameExtension));

            ind.TotalMN3Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3Q, attNameExtension));
            ind.TotalMN3Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN3Name, attNameExtension));
            ind.TotalMN3PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3PFTotal, attNameExtension));
            ind.TotalMN3PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3PCTotal, attNameExtension));
            ind.TotalMN3APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3APTotal, attNameExtension));
            ind.TotalMN3ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3ACTotal, attNameExtension));
            ind.TotalMN3APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3APChange, attNameExtension));
            ind.TotalMN3ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3ACChange, attNameExtension));
            ind.TotalMN3PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3PPPercent, attNameExtension));
            ind.TotalMN3PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3PCPercent, attNameExtension));
            ind.TotalMN3PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3PFPercent, attNameExtension));

            ind.TotalMN4Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4Q, attNameExtension));
            ind.TotalMN4Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN4Name, attNameExtension));
            ind.TotalMN4PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4PFTotal, attNameExtension));
            ind.TotalMN4PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4PCTotal, attNameExtension));
            ind.TotalMN4APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4APTotal, attNameExtension));
            ind.TotalMN4ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4ACTotal, attNameExtension));
            ind.TotalMN4APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4APChange, attNameExtension));
            ind.TotalMN4ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4ACChange, attNameExtension));
            ind.TotalMN4PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4PPPercent, attNameExtension));
            ind.TotalMN4PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4PCPercent, attNameExtension));
            ind.TotalMN4PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4PFPercent, attNameExtension));

            ind.TotalMN5Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5Q, attNameExtension));
            ind.TotalMN5Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN5Name, attNameExtension));
            ind.TotalMN5PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5PFTotal, attNameExtension));
            ind.TotalMN5PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5PCTotal, attNameExtension));
            ind.TotalMN5APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5APTotal, attNameExtension));
            ind.TotalMN5ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5ACTotal, attNameExtension));
            ind.TotalMN5APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5APChange, attNameExtension));
            ind.TotalMN5ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5ACChange, attNameExtension));
            ind.TotalMN5PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5PPPercent, attNameExtension));
            ind.TotalMN5PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5PCPercent, attNameExtension));
            ind.TotalMN5PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5PFPercent, attNameExtension));

            ind.TotalMN6Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6Q, attNameExtension));
            ind.TotalMN6Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN6Name, attNameExtension));
            ind.TotalMN6PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6PFTotal, attNameExtension));
            ind.TotalMN6PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6PCTotal, attNameExtension));
            ind.TotalMN6APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6APTotal, attNameExtension));
            ind.TotalMN6ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6ACTotal, attNameExtension));
            ind.TotalMN6APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6APChange, attNameExtension));
            ind.TotalMN6ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6ACChange, attNameExtension));
            ind.TotalMN6PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6PPPercent, attNameExtension));
            ind.TotalMN6PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6PCPercent, attNameExtension));
            ind.TotalMN6PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6PFPercent, attNameExtension));

            ind.TotalMN7Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7Q, attNameExtension));
            ind.TotalMN7Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN7Name, attNameExtension));
            ind.TotalMN7PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7PFTotal, attNameExtension));
            ind.TotalMN7PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7PCTotal, attNameExtension));
            ind.TotalMN7APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7APTotal, attNameExtension));
            ind.TotalMN7ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7ACTotal, attNameExtension));
            ind.TotalMN7APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7APChange, attNameExtension));
            ind.TotalMN7ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7ACChange, attNameExtension));
            ind.TotalMN7PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7PPPercent, attNameExtension));
            ind.TotalMN7PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7PCPercent, attNameExtension));
            ind.TotalMN7PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7PFPercent, attNameExtension));

            ind.TotalMN8Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8Q, attNameExtension));
            ind.TotalMN8Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN8Name, attNameExtension));
            ind.TotalMN8PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8PFTotal, attNameExtension));
            ind.TotalMN8PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8PCTotal, attNameExtension));
            ind.TotalMN8APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8APTotal, attNameExtension));
            ind.TotalMN8ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8ACTotal, attNameExtension));
            ind.TotalMN8APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8APChange, attNameExtension));
            ind.TotalMN8ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8ACChange, attNameExtension));
            ind.TotalMN8PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8PPPercent, attNameExtension));
            ind.TotalMN8PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8PCPercent, attNameExtension));
            ind.TotalMN8PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8PFPercent, attNameExtension));

            ind.TotalMN9Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9Q, attNameExtension));
            ind.TotalMN9Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN9Name, attNameExtension));
            ind.TotalMN9PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9PFTotal, attNameExtension));
            ind.TotalMN9PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9PCTotal, attNameExtension));
            ind.TotalMN9APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9APTotal, attNameExtension));
            ind.TotalMN9ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9ACTotal, attNameExtension));
            ind.TotalMN9APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9APChange, attNameExtension));
            ind.TotalMN9ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9ACChange, attNameExtension));
            ind.TotalMN9PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9PPPercent, attNameExtension));
            ind.TotalMN9PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9PCPercent, attNameExtension));
            ind.TotalMN9PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9PFPercent, attNameExtension));

            ind.TotalMN10Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10Q, attNameExtension));
            ind.TotalMN10Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN10Name, attNameExtension));
            ind.TotalMN10PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10PFTotal, attNameExtension));
            ind.TotalMN10PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10PCTotal, attNameExtension));
            ind.TotalMN10APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10APTotal, attNameExtension));
            ind.TotalMN10ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10ACTotal, attNameExtension));
            ind.TotalMN10APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10APChange, attNameExtension));
            ind.TotalMN10ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10ACChange, attNameExtension));
            ind.TotalMN10PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10PPPercent, attNameExtension));
            ind.TotalMN10PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10PCPercent, attNameExtension));
            ind.TotalMN10PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10PFPercent, attNameExtension));
        }

        public void SetTotalMN1Progress1Property(MN1Progress1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalMN1Q:
                    ind.TotalMN1Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1Name:
                    ind.TotalMN1Name = attValue;
                    break;
                case cTotalMN1PFTotal:
                    ind.TotalMN1PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1PCTotal:
                    ind.TotalMN1PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1APTotal:
                    ind.TotalMN1APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1ACTotal:
                    ind.TotalMN1ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1APChange:
                    ind.TotalMN1APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1ACChange:
                    ind.TotalMN1ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1PPPercent:
                    ind.TotalMN1PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1PCPercent:
                    ind.TotalMN1PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1PFPercent:
                    ind.TotalMN1PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Q:
                    ind.TotalMN2Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Name:
                    ind.TotalMN2Name = attValue;
                    break;
                case cTotalMN2PFTotal:
                    ind.TotalMN2PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2PCTotal:
                    ind.TotalMN2PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2APTotal:
                    ind.TotalMN2APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2ACTotal:
                    ind.TotalMN2ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2APChange:
                    ind.TotalMN2APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2ACChange:
                    ind.TotalMN2ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2PPPercent:
                    ind.TotalMN2PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2PCPercent:
                    ind.TotalMN2PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2PFPercent:
                    ind.TotalMN2PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Q:
                    ind.TotalMN3Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Name:
                    ind.TotalMN3Name = attValue;
                    break;
                case cTotalMN3PFTotal:
                    ind.TotalMN3PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3PCTotal:
                    ind.TotalMN3PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3APTotal:
                    ind.TotalMN3APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3ACTotal:
                    ind.TotalMN3ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3APChange:
                    ind.TotalMN3APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3ACChange:
                    ind.TotalMN3ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3PPPercent:
                    ind.TotalMN3PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3PCPercent:
                    ind.TotalMN3PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3PFPercent:
                    ind.TotalMN3PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Q:
                    ind.TotalMN4Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Name:
                    ind.TotalMN4Name = attValue;
                    break;
                case cTotalMN4PFTotal:
                    ind.TotalMN4PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4PCTotal:
                    ind.TotalMN4PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4APTotal:
                    ind.TotalMN4APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4ACTotal:
                    ind.TotalMN4ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4APChange:
                    ind.TotalMN4APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4ACChange:
                    ind.TotalMN4ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4PPPercent:
                    ind.TotalMN4PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4PCPercent:
                    ind.TotalMN4PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4PFPercent:
                    ind.TotalMN4PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Q:
                    ind.TotalMN5Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Name:
                    ind.TotalMN5Name = attValue;
                    break;
                case cTotalMN5PFTotal:
                    ind.TotalMN5PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5PCTotal:
                    ind.TotalMN5PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5APTotal:
                    ind.TotalMN5APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5ACTotal:
                    ind.TotalMN5ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5APChange:
                    ind.TotalMN5APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5ACChange:
                    ind.TotalMN5ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5PPPercent:
                    ind.TotalMN5PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5PCPercent:
                    ind.TotalMN5PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5PFPercent:
                    ind.TotalMN5PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Q:
                    ind.TotalMN6Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Name:
                    ind.TotalMN6Name = attValue;
                    break;
                case cTotalMN6PFTotal:
                    ind.TotalMN6PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6PCTotal:
                    ind.TotalMN6PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6APTotal:
                    ind.TotalMN6APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6ACTotal:
                    ind.TotalMN6ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6APChange:
                    ind.TotalMN6APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6ACChange:
                    ind.TotalMN6ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6PPPercent:
                    ind.TotalMN6PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6PCPercent:
                    ind.TotalMN6PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6PFPercent:
                    ind.TotalMN6PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Q:
                    ind.TotalMN7Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Name:
                    ind.TotalMN7Name = attValue;
                    break;
                case cTotalMN7PFTotal:
                    ind.TotalMN7PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7PCTotal:
                    ind.TotalMN7PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7APTotal:
                    ind.TotalMN7APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7ACTotal:
                    ind.TotalMN7ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7APChange:
                    ind.TotalMN7APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7ACChange:
                    ind.TotalMN7ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7PPPercent:
                    ind.TotalMN7PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7PCPercent:
                    ind.TotalMN7PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7PFPercent:
                    ind.TotalMN7PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Q:
                    ind.TotalMN8Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Name:
                    ind.TotalMN8Name = attValue;
                    break;
                case cTotalMN8PFTotal:
                    ind.TotalMN8PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8PCTotal:
                    ind.TotalMN8PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8APTotal:
                    ind.TotalMN8APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8ACTotal:
                    ind.TotalMN8ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8APChange:
                    ind.TotalMN8APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8ACChange:
                    ind.TotalMN8ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8PPPercent:
                    ind.TotalMN8PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8PCPercent:
                    ind.TotalMN8PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8PFPercent:
                    ind.TotalMN8PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Q:
                    ind.TotalMN9Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Name:
                    ind.TotalMN9Name = attValue;
                    break;
                case cTotalMN9PFTotal:
                    ind.TotalMN9PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9PCTotal:
                    ind.TotalMN9PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9APTotal:
                    ind.TotalMN9APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9ACTotal:
                    ind.TotalMN9ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9APChange:
                    ind.TotalMN9APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9ACChange:
                    ind.TotalMN9ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9PPPercent:
                    ind.TotalMN9PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9PCPercent:
                    ind.TotalMN9PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9PFPercent:
                    ind.TotalMN9PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Q:
                    ind.TotalMN10Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Name:
                    ind.TotalMN10Name = attValue;
                    break;
                case cTotalMN10PFTotal:
                    ind.TotalMN10PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10PCTotal:
                    ind.TotalMN10PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10APTotal:
                    ind.TotalMN10APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10ACTotal:
                    ind.TotalMN10ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10APChange:
                    ind.TotalMN10APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10ACChange:
                    ind.TotalMN10ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10PPPercent:
                    ind.TotalMN10PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10PCPercent:
                    ind.TotalMN10PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10PFPercent:
                    ind.TotalMN10PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalMN1Progress1Property(MN1Progress1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalMN1Q:
                    sPropertyValue = ind.TotalMN1Q.ToString();
                    break;
                case cTotalMN1Name:
                    sPropertyValue = ind.TotalMN1Name;
                    break;
                case cTotalMN1PFTotal:
                    sPropertyValue = ind.TotalMN1PFTotal.ToString();
                    break;
                case cTotalMN1PCTotal:
                    sPropertyValue = ind.TotalMN1PCTotal.ToString();
                    break;
                case cTotalMN1APTotal:
                    sPropertyValue = ind.TotalMN1APTotal.ToString();
                    break;
                case cTotalMN1ACTotal:
                    sPropertyValue = ind.TotalMN1ACTotal.ToString();
                    break;
                case cTotalMN1APChange:
                    sPropertyValue = ind.TotalMN1APChange.ToString();
                    break;
                case cTotalMN1ACChange:
                    sPropertyValue = ind.TotalMN1ACChange.ToString();
                    break;
                case cTotalMN1PPPercent:
                    sPropertyValue = ind.TotalMN1PPPercent.ToString();
                    break;
                case cTotalMN1PCPercent:
                    sPropertyValue = ind.TotalMN1PCPercent.ToString();
                    break;
                case cTotalMN1PFPercent:
                    sPropertyValue = ind.TotalMN1PFPercent.ToString();
                    break;
                case cTotalMN2Q:
                    sPropertyValue = ind.TotalMN2Q.ToString();
                    break;
                case cTotalMN2Name:
                    sPropertyValue = ind.TotalMN2Name;
                    break;
                case cTotalMN2PFTotal:
                    sPropertyValue = ind.TotalMN2PFTotal.ToString();
                    break;
                case cTotalMN2PCTotal:
                    sPropertyValue = ind.TotalMN2PCTotal.ToString();
                    break;
                case cTotalMN2APTotal:
                    sPropertyValue = ind.TotalMN2APTotal.ToString();
                    break;
                case cTotalMN2ACTotal:
                    sPropertyValue = ind.TotalMN2ACTotal.ToString();
                    break;
                case cTotalMN2APChange:
                    sPropertyValue = ind.TotalMN2APChange.ToString();
                    break;
                case cTotalMN2ACChange:
                    sPropertyValue = ind.TotalMN2ACChange.ToString();
                    break;
                case cTotalMN2PPPercent:
                    sPropertyValue = ind.TotalMN2PPPercent.ToString();
                    break;
                case cTotalMN2PCPercent:
                    sPropertyValue = ind.TotalMN2PCPercent.ToString();
                    break;
                case cTotalMN2PFPercent:
                    sPropertyValue = ind.TotalMN2PFPercent.ToString();
                    break;
                case cTotalMN3Q:
                    sPropertyValue = ind.TotalMN3Q.ToString();
                    break;
                case cTotalMN3Name:
                    sPropertyValue = ind.TotalMN3Name;
                    break;
                case cTotalMN3PFTotal:
                    sPropertyValue = ind.TotalMN3PFTotal.ToString();
                    break;
                case cTotalMN3PCTotal:
                    sPropertyValue = ind.TotalMN3PCTotal.ToString();
                    break;
                case cTotalMN3APTotal:
                    sPropertyValue = ind.TotalMN3APTotal.ToString();
                    break;
                case cTotalMN3ACTotal:
                    sPropertyValue = ind.TotalMN3ACTotal.ToString();
                    break;
                case cTotalMN3APChange:
                    sPropertyValue = ind.TotalMN3APChange.ToString();
                    break;
                case cTotalMN3ACChange:
                    sPropertyValue = ind.TotalMN3ACChange.ToString();
                    break;
                case cTotalMN3PPPercent:
                    sPropertyValue = ind.TotalMN3PPPercent.ToString();
                    break;
                case cTotalMN3PCPercent:
                    sPropertyValue = ind.TotalMN3PCPercent.ToString();
                    break;
                case cTotalMN3PFPercent:
                    sPropertyValue = ind.TotalMN3PFPercent.ToString();
                    break;
                case cTotalMN4Q:
                    sPropertyValue = ind.TotalMN4Q.ToString();
                    break;
                case cTotalMN4Name:
                    sPropertyValue = ind.TotalMN4Name;
                    break;
                case cTotalMN4PFTotal:
                    sPropertyValue = ind.TotalMN4PFTotal.ToString();
                    break;
                case cTotalMN4PCTotal:
                    sPropertyValue = ind.TotalMN4PCTotal.ToString();
                    break;
                case cTotalMN4APTotal:
                    sPropertyValue = ind.TotalMN4APTotal.ToString();
                    break;
                case cTotalMN4ACTotal:
                    sPropertyValue = ind.TotalMN4ACTotal.ToString();
                    break;
                case cTotalMN4APChange:
                    sPropertyValue = ind.TotalMN4APChange.ToString();
                    break;
                case cTotalMN4ACChange:
                    sPropertyValue = ind.TotalMN4ACChange.ToString();
                    break;
                case cTotalMN4PPPercent:
                    sPropertyValue = ind.TotalMN4PPPercent.ToString();
                    break;
                case cTotalMN4PCPercent:
                    sPropertyValue = ind.TotalMN4PCPercent.ToString();
                    break;
                case cTotalMN4PFPercent:
                    sPropertyValue = ind.TotalMN4PFPercent.ToString();
                    break;
                case cTotalMN5Q:
                    sPropertyValue = ind.TotalMN5Q.ToString();
                    break;
                case cTotalMN5Name:
                    sPropertyValue = ind.TotalMN5Name;
                    break;
                case cTotalMN5PFTotal:
                    sPropertyValue = ind.TotalMN5PFTotal.ToString();
                    break;
                case cTotalMN5PCTotal:
                    sPropertyValue = ind.TotalMN5PCTotal.ToString();
                    break;
                case cTotalMN5APTotal:
                    sPropertyValue = ind.TotalMN5APTotal.ToString();
                    break;
                case cTotalMN5ACTotal:
                    sPropertyValue = ind.TotalMN5ACTotal.ToString();
                    break;
                case cTotalMN5APChange:
                    sPropertyValue = ind.TotalMN5APChange.ToString();
                    break;
                case cTotalMN5ACChange:
                    sPropertyValue = ind.TotalMN5ACChange.ToString();
                    break;
                case cTotalMN5PPPercent:
                    sPropertyValue = ind.TotalMN5PPPercent.ToString();
                    break;
                case cTotalMN5PCPercent:
                    sPropertyValue = ind.TotalMN5PCPercent.ToString();
                    break;
                case cTotalMN5PFPercent:
                    sPropertyValue = ind.TotalMN5PFPercent.ToString();
                    break;
                case cTotalMN6Q:
                    sPropertyValue = ind.TotalMN6Q.ToString();
                    break;
                case cTotalMN6Name:
                    sPropertyValue = ind.TotalMN6Name;
                    break;
                case cTotalMN6PFTotal:
                    sPropertyValue = ind.TotalMN6PFTotal.ToString();
                    break;
                case cTotalMN6PCTotal:
                    sPropertyValue = ind.TotalMN6PCTotal.ToString();
                    break;
                case cTotalMN6APTotal:
                    sPropertyValue = ind.TotalMN6APTotal.ToString();
                    break;
                case cTotalMN6ACTotal:
                    sPropertyValue = ind.TotalMN6ACTotal.ToString();
                    break;
                case cTotalMN6APChange:
                    sPropertyValue = ind.TotalMN6APChange.ToString();
                    break;
                case cTotalMN6ACChange:
                    sPropertyValue = ind.TotalMN6ACChange.ToString();
                    break;
                case cTotalMN6PPPercent:
                    sPropertyValue = ind.TotalMN6PPPercent.ToString();
                    break;
                case cTotalMN6PCPercent:
                    sPropertyValue = ind.TotalMN6PCPercent.ToString();
                    break;
                case cTotalMN6PFPercent:
                    sPropertyValue = ind.TotalMN6PFPercent.ToString();
                    break;
                case cTotalMN7Q:
                    sPropertyValue = ind.TotalMN7Q.ToString();
                    break;
                case cTotalMN7Name:
                    sPropertyValue = ind.TotalMN7Name;
                    break;
                case cTotalMN7PFTotal:
                    sPropertyValue = ind.TotalMN7PFTotal.ToString();
                    break;
                case cTotalMN7PCTotal:
                    sPropertyValue = ind.TotalMN7PCTotal.ToString();
                    break;
                case cTotalMN7APTotal:
                    sPropertyValue = ind.TotalMN7APTotal.ToString();
                    break;
                case cTotalMN7ACTotal:
                    sPropertyValue = ind.TotalMN7ACTotal.ToString();
                    break;
                case cTotalMN7APChange:
                    sPropertyValue = ind.TotalMN7APChange.ToString();
                    break;
                case cTotalMN7ACChange:
                    sPropertyValue = ind.TotalMN7ACChange.ToString();
                    break;
                case cTotalMN7PPPercent:
                    sPropertyValue = ind.TotalMN7PPPercent.ToString();
                    break;
                case cTotalMN7PCPercent:
                    sPropertyValue = ind.TotalMN7PCPercent.ToString();
                    break;
                case cTotalMN7PFPercent:
                    sPropertyValue = ind.TotalMN7PFPercent.ToString();
                    break;
                case cTotalMN8Q:
                    sPropertyValue = ind.TotalMN8Q.ToString();
                    break;
                case cTotalMN8Name:
                    sPropertyValue = ind.TotalMN8Name;
                    break;
                case cTotalMN8PFTotal:
                    sPropertyValue = ind.TotalMN8PFTotal.ToString();
                    break;
                case cTotalMN8PCTotal:
                    sPropertyValue = ind.TotalMN8PCTotal.ToString();
                    break;
                case cTotalMN8APTotal:
                    sPropertyValue = ind.TotalMN8APTotal.ToString();
                    break;
                case cTotalMN8ACTotal:
                    sPropertyValue = ind.TotalMN8ACTotal.ToString();
                    break;
                case cTotalMN8APChange:
                    sPropertyValue = ind.TotalMN8APChange.ToString();
                    break;
                case cTotalMN8ACChange:
                    sPropertyValue = ind.TotalMN8ACChange.ToString();
                    break;
                case cTotalMN8PPPercent:
                    sPropertyValue = ind.TotalMN8PPPercent.ToString();
                    break;
                case cTotalMN8PCPercent:
                    sPropertyValue = ind.TotalMN8PCPercent.ToString();
                    break;
                case cTotalMN8PFPercent:
                    sPropertyValue = ind.TotalMN8PFPercent.ToString();
                    break;
                case cTotalMN9Q:
                    sPropertyValue = ind.TotalMN9Q.ToString();
                    break;
                case cTotalMN9Name:
                    sPropertyValue = ind.TotalMN9Name;
                    break;
                case cTotalMN9PFTotal:
                    sPropertyValue = ind.TotalMN9PFTotal.ToString();
                    break;
                case cTotalMN9PCTotal:
                    sPropertyValue = ind.TotalMN9PCTotal.ToString();
                    break;
                case cTotalMN9APTotal:
                    sPropertyValue = ind.TotalMN9APTotal.ToString();
                    break;
                case cTotalMN9ACTotal:
                    sPropertyValue = ind.TotalMN9ACTotal.ToString();
                    break;
                case cTotalMN9APChange:
                    sPropertyValue = ind.TotalMN9APChange.ToString();
                    break;
                case cTotalMN9ACChange:
                    sPropertyValue = ind.TotalMN9ACChange.ToString();
                    break;
                case cTotalMN9PPPercent:
                    sPropertyValue = ind.TotalMN9PPPercent.ToString();
                    break;
                case cTotalMN9PCPercent:
                    sPropertyValue = ind.TotalMN9PCPercent.ToString();
                    break;
                case cTotalMN9PFPercent:
                    sPropertyValue = ind.TotalMN9PFPercent.ToString();
                    break;
                case cTotalMN10Q:
                    sPropertyValue = ind.TotalMN10Q.ToString();
                    break;
                case cTotalMN10Name:
                    sPropertyValue = ind.TotalMN10Name;
                    break;
                case cTotalMN10PFTotal:
                    sPropertyValue = ind.TotalMN10PFTotal.ToString();
                    break;
                case cTotalMN10PCTotal:
                    sPropertyValue = ind.TotalMN10PCTotal.ToString();
                    break;
                case cTotalMN10APTotal:
                    sPropertyValue = ind.TotalMN10APTotal.ToString();
                    break;
                case cTotalMN10ACTotal:
                    sPropertyValue = ind.TotalMN10ACTotal.ToString();
                    break;
                case cTotalMN10APChange:
                    sPropertyValue = ind.TotalMN10APChange.ToString();
                    break;
                case cTotalMN10ACChange:
                    sPropertyValue = ind.TotalMN10ACChange.ToString();
                    break;
                case cTotalMN10PPPercent:
                    sPropertyValue = ind.TotalMN10PPPercent.ToString();
                    break;
                case cTotalMN10PCPercent:
                    sPropertyValue = ind.TotalMN10PCPercent.ToString();
                    break;
                case cTotalMN10PFPercent:
                    sPropertyValue = ind.TotalMN10PFPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalMN1Progress1Attributes(MN1Progress1 ind,
            string attNameExtension, ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1Q, attNameExtension), ind.TotalMN1Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN1Name, attNameExtension), ind.TotalMN1Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1PFTotal, attNameExtension), ind.TotalMN1PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1PCTotal, attNameExtension), ind.TotalMN1PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1APTotal, attNameExtension), ind.TotalMN1APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1ACTotal, attNameExtension), ind.TotalMN1ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1APChange, attNameExtension), ind.TotalMN1APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1ACChange, attNameExtension), ind.TotalMN1ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1PPPercent, attNameExtension), ind.TotalMN1PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1PCPercent, attNameExtension), ind.TotalMN1PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1PFPercent, attNameExtension), ind.TotalMN1PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2Q, attNameExtension), ind.TotalMN2Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN2Name, attNameExtension), ind.TotalMN2Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2PFTotal, attNameExtension), ind.TotalMN2PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2PCTotal, attNameExtension), ind.TotalMN2PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2APTotal, attNameExtension), ind.TotalMN2APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2ACTotal, attNameExtension), ind.TotalMN2ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2APChange, attNameExtension), ind.TotalMN2APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2ACChange, attNameExtension), ind.TotalMN2ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2PPPercent, attNameExtension), ind.TotalMN2PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2PCPercent, attNameExtension), ind.TotalMN2PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2PFPercent, attNameExtension), ind.TotalMN2PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3Q, attNameExtension), ind.TotalMN3Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN3Name, attNameExtension), ind.TotalMN3Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3PFTotal, attNameExtension), ind.TotalMN3PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3PCTotal, attNameExtension), ind.TotalMN3PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3APTotal, attNameExtension), ind.TotalMN3APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3ACTotal, attNameExtension), ind.TotalMN3ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3APChange, attNameExtension), ind.TotalMN3APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3ACChange, attNameExtension), ind.TotalMN3ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3PPPercent, attNameExtension), ind.TotalMN3PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3PCPercent, attNameExtension), ind.TotalMN3PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3PFPercent, attNameExtension), ind.TotalMN3PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4Q, attNameExtension), ind.TotalMN4Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN4Name, attNameExtension), ind.TotalMN4Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4PFTotal, attNameExtension), ind.TotalMN4PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4PCTotal, attNameExtension), ind.TotalMN4PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4APTotal, attNameExtension), ind.TotalMN4APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4ACTotal, attNameExtension), ind.TotalMN4ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4APChange, attNameExtension), ind.TotalMN4APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4ACChange, attNameExtension), ind.TotalMN4ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4PPPercent, attNameExtension), ind.TotalMN4PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4PCPercent, attNameExtension), ind.TotalMN4PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4PFPercent, attNameExtension), ind.TotalMN4PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5Q, attNameExtension), ind.TotalMN5Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN5Name, attNameExtension), ind.TotalMN5Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5PFTotal, attNameExtension), ind.TotalMN5PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5PCTotal, attNameExtension), ind.TotalMN5PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5APTotal, attNameExtension), ind.TotalMN5APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5ACTotal, attNameExtension), ind.TotalMN5ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5APChange, attNameExtension), ind.TotalMN5APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5ACChange, attNameExtension), ind.TotalMN5ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5PPPercent, attNameExtension), ind.TotalMN5PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5PCPercent, attNameExtension), ind.TotalMN5PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5PFPercent, attNameExtension), ind.TotalMN5PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6Q, attNameExtension), ind.TotalMN6Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN6Name, attNameExtension), ind.TotalMN6Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6PFTotal, attNameExtension), ind.TotalMN6PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6PCTotal, attNameExtension), ind.TotalMN6PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6APTotal, attNameExtension), ind.TotalMN6APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6ACTotal, attNameExtension), ind.TotalMN6ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6APChange, attNameExtension), ind.TotalMN6APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6ACChange, attNameExtension), ind.TotalMN6ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6PPPercent, attNameExtension), ind.TotalMN6PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6PCPercent, attNameExtension), ind.TotalMN6PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6PFPercent, attNameExtension), ind.TotalMN6PFPercent);
           
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7Q, attNameExtension), ind.TotalMN7Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN7Name, attNameExtension), ind.TotalMN7Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7PFTotal, attNameExtension), ind.TotalMN7PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7PCTotal, attNameExtension), ind.TotalMN7PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7APTotal, attNameExtension), ind.TotalMN7APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7ACTotal, attNameExtension), ind.TotalMN7ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7APChange, attNameExtension), ind.TotalMN7APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7ACChange, attNameExtension), ind.TotalMN7ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7PPPercent, attNameExtension), ind.TotalMN7PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7PCPercent, attNameExtension), ind.TotalMN7PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7PFPercent, attNameExtension), ind.TotalMN7PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8Q, attNameExtension), ind.TotalMN8Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN8Name, attNameExtension), ind.TotalMN8Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8PFTotal, attNameExtension), ind.TotalMN8PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8PCTotal, attNameExtension), ind.TotalMN8PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8APTotal, attNameExtension), ind.TotalMN8APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8ACTotal, attNameExtension), ind.TotalMN8ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8APChange, attNameExtension), ind.TotalMN8APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8ACChange, attNameExtension), ind.TotalMN8ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8PPPercent, attNameExtension), ind.TotalMN8PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8PCPercent, attNameExtension), ind.TotalMN8PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8PFPercent, attNameExtension), ind.TotalMN8PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9Q, attNameExtension), ind.TotalMN9Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN9Name, attNameExtension), ind.TotalMN9Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9PFTotal, attNameExtension), ind.TotalMN9PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9PCTotal, attNameExtension), ind.TotalMN9PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9APTotal, attNameExtension), ind.TotalMN9APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9ACTotal, attNameExtension), ind.TotalMN9ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9APChange, attNameExtension), ind.TotalMN9APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9ACChange, attNameExtension), ind.TotalMN9ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9PPPercent, attNameExtension), ind.TotalMN9PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9PCPercent, attNameExtension), ind.TotalMN9PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9PFPercent, attNameExtension), ind.TotalMN9PFPercent);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10Q, attNameExtension), ind.TotalMN10Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN10Name, attNameExtension), ind.TotalMN10Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10PFTotal, attNameExtension), ind.TotalMN10PFTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10PCTotal, attNameExtension), ind.TotalMN10PCTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10APTotal, attNameExtension), ind.TotalMN10APTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10ACTotal, attNameExtension), ind.TotalMN10ACTotal);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10APChange, attNameExtension), ind.TotalMN10APChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10ACChange, attNameExtension), ind.TotalMN10ACChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10PPPercent, attNameExtension), ind.TotalMN10PPPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10PCPercent, attNameExtension), ind.TotalMN10PCPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10PFPercent, attNameExtension), ind.TotalMN10PFPercent);
            
        }

        public void SetTotalMN1Progress1Attributes(MN1Progress1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
           
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1Q, attNameExtension), ind.TotalMN1Q.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1Name, attNameExtension), ind.TotalMN1Name);
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1PFTotal, attNameExtension), ind.TotalMN1PFTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1PCTotal, attNameExtension), ind.TotalMN1PCTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1APTotal, attNameExtension), ind.TotalMN1APTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1ACTotal, attNameExtension), ind.TotalMN1ACTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1APChange, attNameExtension), ind.TotalMN1APChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1ACChange, attNameExtension), ind.TotalMN1ACChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1PPPercent, attNameExtension), ind.TotalMN1PPPercent.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1PCPercent, attNameExtension), ind.TotalMN1PCPercent.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1PFPercent, attNameExtension), ind.TotalMN1PFPercent.ToString("N3", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(cTotalMN2Q, attNameExtension), ind.TotalMN2Q.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2Name, attNameExtension), ind.TotalMN2Name);
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2PFTotal, attNameExtension), ind.TotalMN2PFTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2PCTotal, attNameExtension), ind.TotalMN2PCTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2APTotal, attNameExtension), ind.TotalMN2APTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2ACTotal, attNameExtension), ind.TotalMN2ACTotal.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2APChange, attNameExtension), ind.TotalMN2APChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2ACChange, attNameExtension), ind.TotalMN2ACChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2PPPercent, attNameExtension), ind.TotalMN2PPPercent.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2PCPercent, attNameExtension), ind.TotalMN2PCPercent.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2PFPercent, attNameExtension), ind.TotalMN2PFPercent.ToString("N3", CultureInfo.InvariantCulture));

            if (ind.TotalMN3Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN3Q, attNameExtension), ind.TotalMN3Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3Name, attNameExtension), ind.TotalMN3Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3PFTotal, attNameExtension), ind.TotalMN3PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3PCTotal, attNameExtension), ind.TotalMN3PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3APTotal, attNameExtension), ind.TotalMN3APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3ACTotal, attNameExtension), ind.TotalMN3ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3APChange, attNameExtension), ind.TotalMN3APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3ACChange, attNameExtension), ind.TotalMN3ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3PPPercent, attNameExtension), ind.TotalMN3PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3PCPercent, attNameExtension), ind.TotalMN3PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3PFPercent, attNameExtension), ind.TotalMN3PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN4Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN4Q, attNameExtension), ind.TotalMN4Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4Name, attNameExtension), ind.TotalMN4Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4PFTotal, attNameExtension), ind.TotalMN4PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4PCTotal, attNameExtension), ind.TotalMN4PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4APTotal, attNameExtension), ind.TotalMN4APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4ACTotal, attNameExtension), ind.TotalMN4ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4APChange, attNameExtension), ind.TotalMN4APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4ACChange, attNameExtension), ind.TotalMN4ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4PPPercent, attNameExtension), ind.TotalMN4PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4PCPercent, attNameExtension), ind.TotalMN4PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4PFPercent, attNameExtension), ind.TotalMN4PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN5Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN5Q, attNameExtension), ind.TotalMN5Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5Name, attNameExtension), ind.TotalMN5Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5PFTotal, attNameExtension), ind.TotalMN5PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5PCTotal, attNameExtension), ind.TotalMN5PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5APTotal, attNameExtension), ind.TotalMN5APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5ACTotal, attNameExtension), ind.TotalMN5ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5APChange, attNameExtension), ind.TotalMN5APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5ACChange, attNameExtension), ind.TotalMN5ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5PPPercent, attNameExtension), ind.TotalMN5PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5PCPercent, attNameExtension), ind.TotalMN5PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5PFPercent, attNameExtension), ind.TotalMN5PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN6Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN6Q, attNameExtension), ind.TotalMN6Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6Name, attNameExtension), ind.TotalMN6Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6PFTotal, attNameExtension), ind.TotalMN6PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6PCTotal, attNameExtension), ind.TotalMN6PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6APTotal, attNameExtension), ind.TotalMN6APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6ACTotal, attNameExtension), ind.TotalMN6ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6APChange, attNameExtension), ind.TotalMN6APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6ACChange, attNameExtension), ind.TotalMN6ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6PPPercent, attNameExtension), ind.TotalMN6PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6PCPercent, attNameExtension), ind.TotalMN6PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6PFPercent, attNameExtension), ind.TotalMN6PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN7Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN7Q, attNameExtension), ind.TotalMN7Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7Name, attNameExtension), ind.TotalMN7Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7PFTotal, attNameExtension), ind.TotalMN7PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7PCTotal, attNameExtension), ind.TotalMN7PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7APTotal, attNameExtension), ind.TotalMN7APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7ACTotal, attNameExtension), ind.TotalMN7ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7APChange, attNameExtension), ind.TotalMN7APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7ACChange, attNameExtension), ind.TotalMN7ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7PPPercent, attNameExtension), ind.TotalMN7PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7PCPercent, attNameExtension), ind.TotalMN7PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7PFPercent, attNameExtension), ind.TotalMN7PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN8Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN8Q, attNameExtension), ind.TotalMN8Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8Name, attNameExtension), ind.TotalMN8Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8PFTotal, attNameExtension), ind.TotalMN8PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8PCTotal, attNameExtension), ind.TotalMN8PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8APTotal, attNameExtension), ind.TotalMN8APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8ACTotal, attNameExtension), ind.TotalMN8ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8APChange, attNameExtension), ind.TotalMN8APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8ACChange, attNameExtension), ind.TotalMN8ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8PPPercent, attNameExtension), ind.TotalMN8PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8PCPercent, attNameExtension), ind.TotalMN8PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8PFPercent, attNameExtension), ind.TotalMN8PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN9Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN9Q, attNameExtension), ind.TotalMN9Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9Name, attNameExtension), ind.TotalMN9Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9PFTotal, attNameExtension), ind.TotalMN9PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9PCTotal, attNameExtension), ind.TotalMN9PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9APTotal, attNameExtension), ind.TotalMN9APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9ACTotal, attNameExtension), ind.TotalMN9ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9APChange, attNameExtension), ind.TotalMN9APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9ACChange, attNameExtension), ind.TotalMN9ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9PPPercent, attNameExtension), ind.TotalMN9PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9PCPercent, attNameExtension), ind.TotalMN9PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9PFPercent, attNameExtension), ind.TotalMN9PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN10Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN10Q, attNameExtension), ind.TotalMN10Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10Name, attNameExtension), ind.TotalMN10Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10PFTotal, attNameExtension), ind.TotalMN10PFTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10PCTotal, attNameExtension), ind.TotalMN10PCTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10APTotal, attNameExtension), ind.TotalMN10APTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10ACTotal, attNameExtension), ind.TotalMN10ACTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10APChange, attNameExtension), ind.TotalMN10APChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10ACChange, attNameExtension), ind.TotalMN10ACChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10PPPercent, attNameExtension), ind.TotalMN10PPPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10PCPercent, attNameExtension), ind.TotalMN10PCPercent.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10PFPercent, attNameExtension), ind.TotalMN10PFPercent.ToString("N3", CultureInfo.InvariantCulture));
            }
        }
        //run the analyses for inputs an outputs
        public bool RunAnalyses(MN1Stock mn1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (use Total1 object to avoid duplication)
            MN1Total1 total = new MN1Total1(this.CalcParameters);
            //this adds the totals to mn1stock.total1 (not to total)
            bHasAnalyses = total.RunAnalyses(mn1Stock);
            if (mn1Stock.Total1 != null)
            {
                //copy at least the stock and substock totals from total1 to stat1
                //subprices only if needed in future analyses
                mn1Stock.Progress1 = new MN1Progress1(this.CalcParameters);
                //need one property set
                mn1Stock.Progress1.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
                CopyTotalToProgressStock(mn1Stock.Progress1, mn1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing progress analysis
        public bool RunAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //set calculated changestocks
            List<MN1Stock> progressStocks = new List<MN1Stock>();
            if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                progressStocks = SetIOAnalyses(mn1Stock, calcs);
            }
            else
            {
                //use the stock for the node name
                if (mn1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || mn1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no progress
                    progressStocks = SetTotals(mn1Stock, calcs);
                }
                else if (mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //tps with currentnodename set only need nets (inputs minus outputs)
                    //note that only mn1stock is used (not progressStocks)
                    progressStocks = SetTotals(mn1Stock, calcs);
                }
                else
                {
                    progressStocks = SetAnalyses(mn1Stock, calcs);
                }
            }
            //add the progresstocks to parent stock
            if (progressStocks != null)
            {
                bHasAnalyses = AddProgsToBaseStock(mn1Stock, progressStocks);
                //mn1Stock must still add the members of progress1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }
        
        private List<MN1Stock> SetTotals(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            List<MN1Stock> progressStocks = new List<MN1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of mn1stocks for each input and output
            //object model is calc.Total1.MNSR01Stocks
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(mn1Stock.GetType()))
                {
                    MN1Stock stock = (MN1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Progress1 != null)
                        {
                             //tps start substracting outcomes from op/comps
                            if (mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                || mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                            {
                                //subtract stock2 (outputs) from stock1 (inputs) and don't add outcomes stock to collection
                                stock.Progress1.SubApplicationType = stock.SubApplicationType;
                                mn1Stock.Progress1.TargetType = mn1Stock.TargetType;
                                bHasTotals = AddandSubtractSubTotalToTotalStock(mn1Stock.Progress1, mn1Stock.Multiplier, stock.Progress1);
                                if (bHasTotals
                                    && stock.SubApplicationType != Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
                                {
                                    //this is done for consistency but progStocks are not used (only the totals in mn1Stock)
                                    progressStocks.Add(mn1Stock);
                                }
                            }
                            else
                            {
                                //calc holds an input or output stock
                                //add that stock to mn1stock (some analyses will need to use subprices too)
                                bHasTotals = AddSubTotalToTotalStock(mn1Stock.Progress1, mn1Stock.Multiplier, stock.Progress1);
                                if (bHasTotals)
                                {
                                    progressStocks.Add(mn1Stock);
                                }
                            }
                        }
                    }
                }
            }
            return progressStocks;
        }
        private List<MN1Stock> SetAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            List<MN1Stock> progressStocks = new List<MN1Stock>();
            bool bHasTotals = false;
            //set N
            int iQN = 0;
            //set the calc totals in each observation
            MN1Stock observationStock = new MN1Stock();
            observationStock.Progress1 = new MN1Progress1(this.CalcParameters);
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(mn1Stock.GetType()))
                {
                    MN1Stock stock = (MN1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Progress1 != null)
                        {
                            MN1Stock observation2Stock = new MN1Stock();
                            //stocks need some props set
                            stock.CalcParameters.CurrentElementNodeName 
                                = mn1Stock.CalcParameters.CurrentElementNodeName;
                            stock.Multiplier = mn1Stock.Multiplier;
                            bHasTotals = SetObservationStock(progressStocks, mn1Stock,
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
                bHasTotals = SetProgressAnalysis(progressStocks, mn1Stock, iQN);
            }
            return progressStocks;
        }
        private List<MN1Stock> SetIOAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            List<MN1Stock> progressStocks = new List<MN1Stock>();
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iQN2 = 0;
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(mn1Stock.GetType()))
                {
                    MN1Stock stock = (MN1Stock)calc;
                    if (stock != null)
                    {
                        //stock.Progress1 holds the initial substock/price totals
                        if (stock.Progress1 != null)
                        {
                            MN1Stock observation2Stock = new MN1Stock();
                            stock.Multiplier = mn1Stock.Multiplier;
                            bHasTotals = SetObservationStock(progressStocks, mn1Stock,
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
                bHasTotals = SetProgressAnalysis(progressStocks, mn1Stock, iQN2);
            }
            return progressStocks;
        }
    
        private bool SetObservationStock(List<MN1Stock> progressStocks,
            MN1Stock mn1Stock, MN1Stock stock, MN1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Progress1 = new MN1Progress1(this.CalcParameters);
            observation2Stock.Id = stock.Id;
            observation2Stock.Progress1.Id = stock.Id;
            //copy some stock props to progress1
            BIMN1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock.Progress1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BIMN1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Progress1.CalcParameters == null)
                stock.Progress1.CalcParameters = new CalculatorParameters();
            stock.Progress1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            //at oc and outcome level no aggregating by targettype, need schedule variances)
            bHasTotals = SetSubTotalFromTotalStock(observation2Stock.Progress1,
                stock.Multiplier, stock.Progress1);
            return bHasTotals;
        }
        private bool SetProgressAnalysis(List<MN1Stock> progressStocks, MN1Stock mn1Stock,
            int qN)
        {
            bool bHasTotals = false;
            if (qN > 0)
            {
                //set progress numbers
                SetMN1Progress(mn1Stock, progressStocks);
                SetMN2Progress(mn1Stock, progressStocks);
                SetMN3Progress(mn1Stock, progressStocks);
                SetMN4Progress(mn1Stock, progressStocks);
                SetMN5Progress(mn1Stock, progressStocks);
                SetMN6Progress(mn1Stock, progressStocks);
                SetMN7Progress(mn1Stock, progressStocks);
                SetMN8Progress(mn1Stock, progressStocks);
                SetMN9Progress(mn1Stock, progressStocks);
                SetMN10Progress(mn1Stock, progressStocks);
            }
            //add cumulative totals to parent lcastock1 (ocgroup)
            bHasTotals = AddCumulative1Calcs(progressStocks, mn1Stock);
            return bHasTotals;
        }
    
        private bool AddCumulative1Calcs(List<MN1Stock> progressStocks, MN1Stock mn1Stock)
        {
            bool bHasTotals = false;
            //could be all planned, all actual
            MN1Stock cumChange = progressStocks.LastOrDefault();
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
                        AddCumulativeTotals(mn1Stock, cumChange.Progress1);
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private static void AddCumulativeTotals(MN1Stock mn1Stock,
            MN1Progress1 cumulativeTotal)
        {
            if (mn1Stock.Progress1 == null)
            {
                mn1Stock.Progress1 = new MN1Progress1(cumulativeTotal.CalcParameters);
            }
            mn1Stock.Progress1.TotalMN1Name = cumulativeTotal.TotalMN1Name;
            mn1Stock.Progress1.TotalMN2Name = cumulativeTotal.TotalMN2Name;
            mn1Stock.Progress1.TotalMN3Name = cumulativeTotal.TotalMN3Name;
            mn1Stock.Progress1.TotalMN4Name = cumulativeTotal.TotalMN4Name;
            mn1Stock.Progress1.TotalMN5Name = cumulativeTotal.TotalMN5Name;
            mn1Stock.Progress1.TotalMN6Name = cumulativeTotal.TotalMN6Name;
            mn1Stock.Progress1.TotalMN7Name = cumulativeTotal.TotalMN7Name;
            mn1Stock.Progress1.TotalMN8Name = cumulativeTotal.TotalMN8Name;
            mn1Stock.Progress1.TotalMN9Name = cumulativeTotal.TotalMN9Name;
            mn1Stock.Progress1.TotalMN10Name = cumulativeTotal.TotalMN10Name;
            if (cumulativeTotal.TargetType == TARGET_TYPES.benchmark.ToString())
            {
                //Set MNs regular cumulative totals
                mn1Stock.Progress1.TotalMN1Q = cumulativeTotal.TotalMN1PFTotal;
                mn1Stock.Progress1.TotalMN2Q = cumulativeTotal.TotalMN2PFTotal;
                mn1Stock.Progress1.TotalMN3Q = cumulativeTotal.TotalMN3PFTotal;
                mn1Stock.Progress1.TotalMN4Q = cumulativeTotal.TotalMN4PFTotal;
                mn1Stock.Progress1.TotalMN5Q = cumulativeTotal.TotalMN5PFTotal;
                mn1Stock.Progress1.TotalMN6Q = cumulativeTotal.TotalMN6PFTotal;
                mn1Stock.Progress1.TotalMN7Q = cumulativeTotal.TotalMN7PFTotal;
                mn1Stock.Progress1.TotalMN8Q = cumulativeTotal.TotalMN8PFTotal;
                mn1Stock.Progress1.TotalMN9Q = cumulativeTotal.TotalMN9PFTotal;
                mn1Stock.Progress1.TotalMN10Q = cumulativeTotal.TotalMN10PFTotal;
            }
            else 
            {
                //Set MNs regular cumulative totals
                mn1Stock.Progress1.TotalMN1Q = cumulativeTotal.TotalMN1ACTotal;
                mn1Stock.Progress1.TotalMN2Q = cumulativeTotal.TotalMN2ACTotal;
                mn1Stock.Progress1.TotalMN3Q = cumulativeTotal.TotalMN3ACTotal;
                mn1Stock.Progress1.TotalMN4Q = cumulativeTotal.TotalMN4ACTotal;
                mn1Stock.Progress1.TotalMN5Q = cumulativeTotal.TotalMN5ACTotal;
                mn1Stock.Progress1.TotalMN6Q = cumulativeTotal.TotalMN6ACTotal;
                mn1Stock.Progress1.TotalMN7Q = cumulativeTotal.TotalMN7ACTotal;
                mn1Stock.Progress1.TotalMN8Q = cumulativeTotal.TotalMN8ACTotal;
                mn1Stock.Progress1.TotalMN9Q = cumulativeTotal.TotalMN9ACTotal;
                mn1Stock.Progress1.TotalMN10Q = cumulativeTotal.TotalMN10ACTotal;
            }
        }
        
        
        
        public bool AddSubTotalToTotalStock(MN1Progress1 totStock, double multiplier,
            MN1Progress1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier adjusted costs
            totStock.TotalMN1Name = subTotal.TotalMN1Name;
            totStock.TotalMN2Name = subTotal.TotalMN2Name;
            totStock.TotalMN3Name = subTotal.TotalMN3Name;
            totStock.TotalMN4Name = subTotal.TotalMN4Name;
            totStock.TotalMN5Name = subTotal.TotalMN5Name;
            totStock.TotalMN6Name = subTotal.TotalMN6Name;
            totStock.TotalMN7Name = subTotal.TotalMN7Name;
            totStock.TotalMN8Name = subTotal.TotalMN8Name;
            totStock.TotalMN9Name = subTotal.TotalMN9Name;
            totStock.TotalMN10Name = subTotal.TotalMN10Name;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
            //inputs and outputs need cumulative totals 
            totStock.TotalMN1Q += subTotal.TotalMN1Q;
            totStock.TotalMN2Q += subTotal.TotalMN2Q;
            totStock.TotalMN3Q += subTotal.TotalMN3Q;
            totStock.TotalMN4Q += subTotal.TotalMN4Q;
            totStock.TotalMN5Q += subTotal.TotalMN5Q;
            totStock.TotalMN6Q += subTotal.TotalMN6Q;
            totStock.TotalMN7Q += subTotal.TotalMN7Q;
            totStock.TotalMN8Q += subTotal.TotalMN8Q;
            totStock.TotalMN9Q += subTotal.TotalMN9Q;
            totStock.TotalMN10Q += subTotal.TotalMN10Q;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool AddandSubtractSubTotalToTotalStock(MN1Progress1 totStock, double multiplier,
            MN1Progress1 subTotal)
        {
            bool bHasCalculations = false;
            //tps start using nets not totals
            //the tps will use MNQs to run new progress analysis
            if (subTotal.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
            {
                //Set MNs set actual period using last actual total
                //TotalMN1Q is the planned or benchmark
                //actual.Progress1.TotalMN1PFTotal = actual.Progress1.TotalMN1Q;
                totStock.TotalMN1Q -= subTotal.TotalMN1Q * multiplier;
                totStock.TotalMN2Q -= subTotal.TotalMN2Q * multiplier;
                totStock.TotalMN3Q -= subTotal.TotalMN3Q * multiplier;
                totStock.TotalMN4Q -= subTotal.TotalMN4Q * multiplier;
                totStock.TotalMN5Q -= subTotal.TotalMN5Q * multiplier;
                totStock.TotalMN6Q -= subTotal.TotalMN6Q * multiplier;
                totStock.TotalMN7Q -= subTotal.TotalMN7Q * multiplier;
                totStock.TotalMN8Q -= subTotal.TotalMN8Q * multiplier;
                totStock.TotalMN9Q -= subTotal.TotalMN9Q * multiplier;
                totStock.TotalMN10Q -= subTotal.TotalMN10Q * multiplier;
            }
            else
            {
                //multiplier adjusted costs
                //names are displayed using this subtotal
                totStock.TotalMN1Name = subTotal.TotalMN1Name;
                totStock.TotalMN2Name = subTotal.TotalMN2Name;
                totStock.TotalMN3Name = subTotal.TotalMN3Name;
                totStock.TotalMN4Name = subTotal.TotalMN4Name;
                totStock.TotalMN5Name = subTotal.TotalMN5Name;
                totStock.TotalMN6Name = subTotal.TotalMN6Name;
                totStock.TotalMN7Name = subTotal.TotalMN7Name;
                totStock.TotalMN8Name = subTotal.TotalMN8Name;
                totStock.TotalMN9Name = subTotal.TotalMN9Name;
                totStock.TotalMN10Name = subTotal.TotalMN10Name;
                totStock.TotalMN1Q += subTotal.TotalMN1Q * multiplier;
                totStock.TotalMN2Q += subTotal.TotalMN2Q * multiplier;
                totStock.TotalMN3Q += subTotal.TotalMN3Q * multiplier;
                totStock.TotalMN4Q += subTotal.TotalMN4Q * multiplier;
                totStock.TotalMN5Q += subTotal.TotalMN5Q * multiplier;
                totStock.TotalMN6Q += subTotal.TotalMN6Q * multiplier;
                totStock.TotalMN7Q += subTotal.TotalMN7Q * multiplier;
                totStock.TotalMN8Q += subTotal.TotalMN8Q * multiplier;
                totStock.TotalMN9Q += subTotal.TotalMN9Q * multiplier;
                totStock.TotalMN10Q += subTotal.TotalMN10Q * multiplier;
            }
            bHasCalculations = true;
            return bHasCalculations;
        }
        
        public bool SetSubTotalFromTotalStock(MN1Progress1 totStock, double multiplier,
            MN1Progress1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
            totStock.TotalMN1Name = subTotal.TotalMN1Name;
            totStock.TotalMN2Name = subTotal.TotalMN2Name;
            totStock.TotalMN3Name = subTotal.TotalMN3Name;
            totStock.TotalMN4Name = subTotal.TotalMN4Name;
            totStock.TotalMN5Name = subTotal.TotalMN5Name;
            totStock.TotalMN6Name = subTotal.TotalMN6Name;
            totStock.TotalMN7Name = subTotal.TotalMN7Name;
            totStock.TotalMN8Name = subTotal.TotalMN8Name;
            totStock.TotalMN9Name = subTotal.TotalMN9Name;
            totStock.TotalMN10Name = subTotal.TotalMN10Name;
            //only inputs and outputs need +-
            //the SetMN1Progress set cumulative totals (not here)
            totStock.TotalMN1Q = subTotal.TotalMN1Q;
            totStock.TotalMN2Q = subTotal.TotalMN2Q;
            totStock.TotalMN3Q = subTotal.TotalMN3Q;
            totStock.TotalMN4Q = subTotal.TotalMN4Q;
            totStock.TotalMN5Q = subTotal.TotalMN5Q;
            totStock.TotalMN6Q = subTotal.TotalMN6Q;
            totStock.TotalMN7Q = subTotal.TotalMN7Q;
            totStock.TotalMN8Q = subTotal.TotalMN8Q;
            totStock.TotalMN9Q = subTotal.TotalMN9Q;
            totStock.TotalMN10Q = subTotal.TotalMN10Q;
            bHasCalculations = true;
            return bHasCalculations;
        }
        private static void ChangeSubTotalByMultipliers(MN1Progress1 subTotal, double multiplier)
        {
            if (multiplier == 0)
                multiplier = 1;
            //adjust TotalMN1Q -that's the only number used in SetMN1Progress
            subTotal.TotalMN1Q = subTotal.TotalMN1Q * multiplier;
            subTotal.TotalMN2Q = subTotal.TotalMN2Q * multiplier;
            subTotal.TotalMN3Q = subTotal.TotalMN3Q * multiplier;
            subTotal.TotalMN4Q = subTotal.TotalMN4Q * multiplier;
            subTotal.TotalMN5Q = subTotal.TotalMN5Q * multiplier;
            subTotal.TotalMN6Q = subTotal.TotalMN6Q * multiplier;
            subTotal.TotalMN7Q = subTotal.TotalMN7Q * multiplier;
            subTotal.TotalMN8Q = subTotal.TotalMN8Q * multiplier;
            subTotal.TotalMN9Q = subTotal.TotalMN9Q * multiplier;
            subTotal.TotalMN10Q = subTotal.TotalMN10Q * multiplier;

        }
        
        //private static void ChangeProgressSubTotalByMultipliers(MN1Progress1 subTotal, double multiplier)
        //{
        //    if (multiplier == 0)
        //        multiplier = 1;
        //    //adjust TotalMN1Q -that's the only number used in SetMN1Progress
        //    subTotal.TotalMN1Q = subTotal.TotalMN1APTotal * multiplier;
        //    subTotal.TotalMN2Q = subTotal.TotalMN2APTotal * multiplier;
        //    subTotal.TotalMN3Q = subTotal.TotalMN3APTotal * multiplier;
        //    subTotal.TotalMN4Q = subTotal.TotalMN4APTotal * multiplier;
        //    subTotal.TotalMN5Q = subTotal.TotalMN5APTotal * multiplier;
        //    subTotal.TotalMN6Q = subTotal.TotalMN6APTotal * multiplier;
        //    subTotal.TotalMN7Q = subTotal.TotalMN7APTotal * multiplier;
        //    subTotal.TotalMN8Q = subTotal.TotalMN8APTotal * multiplier;
        //    subTotal.TotalMN9Q = subTotal.TotalMN9APTotal * multiplier;
        //    subTotal.TotalMN10Q = subTotal.TotalMN10APTotal * multiplier;
            
        //}

        public bool CopyTotalToProgressStock(MN1Progress1 totStock, MN1Total1 subTotal)
        {
            bool bHasCalculations = false;
            if (this.CalcParameters.UrisToAnalyze != null)
            {
                double dbNutTotal = 0;
                int iNutrientCount = 0;
                foreach (var mn in this.CalcParameters.UrisToAnalyze)
                {
                    if (mn == MNSR1.cContainerPrice)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalContainerPrice != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalContainerPrice;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalContainerPrice;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cContainerSizeInSSUnits)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalContainerSizeInSSUnits != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalContainerSizeInSSUnits;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalContainerSizeInSSUnits;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cServingCost)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalServingCost != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalServingCost;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalServingCost;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cActualServingSize)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalActualServingSize != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalActualServingSize;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalActualServingSize;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cTypicalServingSize)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalTypicalServingSize != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalTypicalServingSize;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalTypicalServingSize;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cTypicalServingsPerContainer)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalTypicalServingsPerContainer != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalTypicalServingsPerContainer;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalTypicalServingsPerContainer;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cActualServingsPerContainer)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalActualServingsPerContainer != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalActualServingsPerContainer;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalActualServingsPerContainer;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cWater_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalWater_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalWater_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalWater_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cEnerg_Kcal)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalEnerg_Kcal != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalEnerg_Kcal;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalEnerg_Kcal;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cProtein_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalProtein_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalProtein_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalProtein_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cLipid_Tot_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalLipid_Tot_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalLipid_Tot_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalLipid_Tot_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cAsh_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalAsh_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalAsh_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalAsh_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cCarbohydrt_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalCarbohydrt_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalCarbohydrt_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalCarbohydrt_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFiber_TD_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFiber_TD_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFiber_TD_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFiber_TD_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cSugar_Tot_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalSugar_Tot_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalSugar_Tot_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalSugar_Tot_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cCalcium_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalCalcium_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalCalcium_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalCalcium_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cIron_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalIron_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalIron_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalIron_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cMagnesium_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalMagnesium_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalMagnesium_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalMagnesium_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cPhosphorus_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalPhosphorus_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalPhosphorus_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalPhosphorus_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cPotassium_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalPotassium_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalPotassium_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalPotassium_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cSodium_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalSodium_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalSodium_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalSodium_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cZinc_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalZinc_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalZinc_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalZinc_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cCopper_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalCopper_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalCopper_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalCopper_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cManganese_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalManganese_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalManganese_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalManganese_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cSelenium_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalSelenium_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalSelenium_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalSelenium_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_C_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_C_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_C_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_C_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cThiamin_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalThiamin_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalThiamin_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalThiamin_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cRiboflavin_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalRiboflavin_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalRiboflavin_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalRiboflavin_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cNiacin_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalNiacin_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalNiacin_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalNiacin_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cPanto_Acid_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalPanto_Acid_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalPanto_Acid_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalPanto_Acid_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_B6_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_B6_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_B6_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_B6_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFolate_Tot_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFolate_Tot_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFolate_Tot_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFolate_Tot_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFolic_Acid_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFolic_Acid_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFolic_Acid_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFolic_Acid_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFood_Folate_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFood_Folate_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFood_Folate_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFood_Folate_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFolate_DFE_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFolate_DFE_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFolate_DFE_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFolate_DFE_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cCholine_Tot_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalCholine_Tot_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalCholine_Tot_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalCholine_Tot_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_B12_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_B12_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_B12_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_B12_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_A_IU)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_A_IU != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_A_IU;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_A_IU;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_A_RAE)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_A_RAE != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_A_RAE;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_A_RAE;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cRetinol_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalRetinol_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalRetinol_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalRetinol_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cAlpha_Carot_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalAlpha_Carot_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalAlpha_Carot_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalAlpha_Carot_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cBeta_Carot_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalBeta_Carot_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalBeta_Carot_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalBeta_Carot_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cBeta_Crypt_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalBeta_Crypt_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalBeta_Crypt_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalBeta_Crypt_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cLycopene_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalLycopene_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalLycopene_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalLycopene_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cLut_Zea_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalLut_Zea_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalLut_Zea_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalLut_Zea_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_E_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_E_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_E_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_E_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_D_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_D_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_D_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_D_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cViVit_D_IU)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalViVit_D_IU != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalViVit_D_IU;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalViVit_D_IU;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cVit_K_pg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalVit_K_pg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalVit_K_pg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalVit_K_pg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFA_Sat_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFA_Sat_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFA_Sat_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFA_Sat_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFA_Mono_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFA_Mono_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFA_Mono_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFA_Mono_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cFA_Poly_g)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalFA_Poly_g != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalFA_Poly_g;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalFA_Poly_g;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cCholestrl_mg)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalCholestrl_mg != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalCholestrl_mg;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalCholestrl_mg;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cExtra1)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalExtra1 != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalExtra1;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalExtra1;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                    else if (mn == MNSR1.cExtra2)
                    {
                        iNutrientCount++;
                        if (subTotal.MNSR1Stock.TotalExtra2 != 0)
                        {
                            dbNutTotal = subTotal.MNSR1Stock.TotalExtra2;
                        }
                        else
                        {
                            dbNutTotal = subTotal.MNSR2Stock.TotalExtra2;
                        }
                        AddStock(totStock, mn, dbNutTotal, iNutrientCount);
                    }
                }
            }
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool AddStock(MN1Progress1 totStock,
            string nutNeededName, double nutTotal, int iNutrientCount)
        {
            bool bHasStock = false;
            if (iNutrientCount == 1)
            {
                totStock.TotalMN1Name = nutNeededName;
                totStock.TotalMN1Q = nutTotal;
            }
            else if (iNutrientCount == 2)
            {
                totStock.TotalMN2Name = nutNeededName;
                totStock.TotalMN2Q = nutTotal;
            }
            else if (iNutrientCount == 3)
            {
                totStock.TotalMN3Name = nutNeededName;
                totStock.TotalMN3Q = nutTotal;
            }
            else if (iNutrientCount == 4)
            {
                totStock.TotalMN4Name = nutNeededName;
                totStock.TotalMN4Q = nutTotal;
            }
            else if (iNutrientCount == 5)
            {
                totStock.TotalMN5Name = nutNeededName;
                totStock.TotalMN5Q = nutTotal;
            }
            else if (iNutrientCount == 6)
            {
                totStock.TotalMN6Name = nutNeededName;
                totStock.TotalMN6Q = nutTotal;
            }
            else if (iNutrientCount == 7)
            {
                totStock.TotalMN7Name = nutNeededName;
                totStock.TotalMN7Q = nutTotal;
            }
            else if (iNutrientCount == 8)
            {
                totStock.TotalMN8Name = nutNeededName;
                totStock.TotalMN8Q = nutTotal;
            }
            else if (iNutrientCount == 9)
            {
                totStock.TotalMN9Name = nutNeededName;
                totStock.TotalMN9Q = nutTotal;
            }
            else if (iNutrientCount == 10)
            {
                totStock.TotalMN10Name = nutNeededName;
                totStock.TotalMN10Q = nutTotal;
            }
            return bHasStock;
        }
        private static void SetMN1Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN1Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN1Q;
                    //planned cumulative
                    planned.Progress1.TotalMN1PCTotal = dbPlannedTotalQ;
                    
                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN1PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN1Q;
                    actual.Progress1.TotalMN1ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN1APTotal = actual.Progress1.TotalMN1Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN1PCTotal = planned.Progress1.TotalMN1PCTotal;
                        //set actual.planned period
                        //TotalMN1Q is always planned period and TotalMN1APTotal is actual period
                        actual.Progress1.TotalMN1Q = planned.Progress1.TotalMN1Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN1PFTotal = planned.Progress1.TotalMN1PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN1APChange = actual.Progress1.TotalMN1APTotal - actual.Progress1.TotalMN1Q;
                    //cumulative change
                    actual.Progress1.TotalMN1ACChange = actual.Progress1.TotalMN1ACTotal - actual.Progress1.TotalMN1PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN1PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN1APTotal, actual.Progress1.TotalMN1Q);
                    actual.Progress1.TotalMN1PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN1ACTotal, actual.Progress1.TotalMN1PCTotal);
                    actual.Progress1.TotalMN1PFPercent
                            = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN1ACTotal, actual.Progress1.TotalMN1PFTotal);
                }
            }
        }
        private static MN1Stock GetProgressStockByLabel(MN1Stock actual, List<int> ids,
            List<MN1Stock> progressStocks, string targetType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            MN1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (progressStocks.Any(p => p.Label == actual.Label
                && p.TargetType == targetType))
            {
                int iIndex = 1;
                foreach (MN1Stock planned in progressStocks)
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
            MN1Stock planned, List<MN1Stock> progressStocks, string targetType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (MN1Stock rp in progressStocks)
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
        private static void SetMN2Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN1Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN2Q;
                    //planned cumulative
                    planned.Progress1.TotalMN2PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN2PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN2Q;
                    actual.Progress1.TotalMN2ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN2APTotal = actual.Progress1.TotalMN2Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN2PCTotal = planned.Progress1.TotalMN2PCTotal;
                        //set actual.planned period
                        //TotalMN2Q is always planned period and TotalMN2APTotal is actual period
                        actual.Progress1.TotalMN2Q = planned.Progress1.TotalMN2Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN2PFTotal = planned.Progress1.TotalMN2PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN2APChange = actual.Progress1.TotalMN2APTotal - actual.Progress1.TotalMN2Q;
                    //cumulative change
                    actual.Progress1.TotalMN2ACChange = actual.Progress1.TotalMN2ACTotal - actual.Progress1.TotalMN2PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN2PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN2APTotal, actual.Progress1.TotalMN2Q);
                    actual.Progress1.TotalMN2PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN2ACTotal, actual.Progress1.TotalMN2PCTotal);
                    actual.Progress1.TotalMN2PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN2ACTotal, actual.Progress1.TotalMN2PFTotal);
                }
            }
        }
        private static void SetMN3Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN3Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN3Q;
                    //planned cumulative
                    planned.Progress1.TotalMN3PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN3PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN3Q;
                    actual.Progress1.TotalMN3ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN3APTotal = actual.Progress1.TotalMN3Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN3PCTotal = planned.Progress1.TotalMN3PCTotal;
                        //set actual.planned period
                        //TotalMN3Q is always planned period and TotalMN3APTotal is actual period
                        actual.Progress1.TotalMN3Q = planned.Progress1.TotalMN3Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN3PFTotal = planned.Progress1.TotalMN3PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN3APChange = actual.Progress1.TotalMN3APTotal - actual.Progress1.TotalMN3Q;
                    //cumulative change
                    actual.Progress1.TotalMN3ACChange = actual.Progress1.TotalMN3ACTotal - actual.Progress1.TotalMN3PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN3PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN3APTotal, actual.Progress1.TotalMN3Q);
                    actual.Progress1.TotalMN3PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN3ACTotal, actual.Progress1.TotalMN3PCTotal);
                    actual.Progress1.TotalMN3PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN3ACTotal, actual.Progress1.TotalMN3PFTotal);
                }
            }
        }
        private static void SetMN4Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN4Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN4Q;
                    //planned cumulative
                    planned.Progress1.TotalMN4PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN4PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN4Q;
                    actual.Progress1.TotalMN4ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN4APTotal = actual.Progress1.TotalMN4Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN4PCTotal = planned.Progress1.TotalMN4PCTotal;
                        //set actual.planned period
                        //TotalMN4Q is always planned period and TotalMN4APTotal is actual period
                        actual.Progress1.TotalMN4Q = planned.Progress1.TotalMN4Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN4PFTotal = planned.Progress1.TotalMN4PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN4APChange = actual.Progress1.TotalMN4APTotal - actual.Progress1.TotalMN4Q;
                    //cumulative change
                    actual.Progress1.TotalMN4ACChange = actual.Progress1.TotalMN4ACTotal - actual.Progress1.TotalMN4PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN4PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN4APTotal, actual.Progress1.TotalMN4Q);
                    actual.Progress1.TotalMN4PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN4ACTotal, actual.Progress1.TotalMN4PCTotal);
                    actual.Progress1.TotalMN4PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN4ACTotal, actual.Progress1.TotalMN4PFTotal);
                }
            }
        }
        private static void SetMN5Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN4Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN5Q;
                    //planned cumulative
                    planned.Progress1.TotalMN5PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN5PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN5Q;
                    actual.Progress1.TotalMN5ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN5APTotal = actual.Progress1.TotalMN5Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN5PCTotal = planned.Progress1.TotalMN5PCTotal;
                        //set actual.planned period
                        //TotalMN5Q is always planned period and TotalMN5APTotal is actual period
                        actual.Progress1.TotalMN5Q = planned.Progress1.TotalMN5Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN5PFTotal = planned.Progress1.TotalMN5PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN5APChange = actual.Progress1.TotalMN5APTotal - actual.Progress1.TotalMN5Q;
                    //cumulative change
                    actual.Progress1.TotalMN5ACChange = actual.Progress1.TotalMN5ACTotal - actual.Progress1.TotalMN5PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN5PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN5APTotal, actual.Progress1.TotalMN5Q);
                    actual.Progress1.TotalMN5PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN5ACTotal, actual.Progress1.TotalMN5PCTotal);
                    actual.Progress1.TotalMN5PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN5ACTotal, actual.Progress1.TotalMN5PFTotal);
                }
            }
        }
        private static void SetMN6Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN6Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN6Q;
                    //planned cumulative
                    planned.Progress1.TotalMN6PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN6PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN6Q;
                    actual.Progress1.TotalMN6ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN6APTotal = actual.Progress1.TotalMN6Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN6PCTotal = planned.Progress1.TotalMN6PCTotal;
                        //set actual.planned period
                        //TotalMN6Q is always planned period and TotalMN6APTotal is actual period
                        actual.Progress1.TotalMN6Q = planned.Progress1.TotalMN6Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN6PFTotal = planned.Progress1.TotalMN6PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN6APChange = actual.Progress1.TotalMN6APTotal - actual.Progress1.TotalMN6Q;
                    //cumulative change
                    actual.Progress1.TotalMN6ACChange = actual.Progress1.TotalMN6ACTotal - actual.Progress1.TotalMN6PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN6PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN6APTotal, actual.Progress1.TotalMN6Q);
                    actual.Progress1.TotalMN6PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN6ACTotal, actual.Progress1.TotalMN6PCTotal);
                    actual.Progress1.TotalMN6PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN6ACTotal, actual.Progress1.TotalMN6PFTotal);
                }
            }
        }
        private static void SetMN7Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN7Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN7Q;
                    //planned cumulative
                    planned.Progress1.TotalMN7PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN7PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN7Q;
                    actual.Progress1.TotalMN7ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN7APTotal = actual.Progress1.TotalMN7Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN7PCTotal = planned.Progress1.TotalMN7PCTotal;
                        //set actual.planned period
                        //TotalMN7Q is always planned period and TotalMN7APTotal is actual period
                        actual.Progress1.TotalMN7Q = planned.Progress1.TotalMN7Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN7PFTotal = planned.Progress1.TotalMN7PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN7APChange = actual.Progress1.TotalMN7APTotal - actual.Progress1.TotalMN7Q;
                    //cumulative change
                    actual.Progress1.TotalMN7ACChange = actual.Progress1.TotalMN7ACTotal - actual.Progress1.TotalMN7PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN7PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN7APTotal, actual.Progress1.TotalMN7Q);
                    actual.Progress1.TotalMN7PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN7ACTotal, actual.Progress1.TotalMN7PCTotal);
                    actual.Progress1.TotalMN7PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN7ACTotal, actual.Progress1.TotalMN7PFTotal);
                }
            }
        }
        private static void SetMN8Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN8Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN8Q;
                    //planned cumulative
                    planned.Progress1.TotalMN8PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN8PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN8Q;
                    actual.Progress1.TotalMN8ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN8APTotal = actual.Progress1.TotalMN8Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN8PCTotal = planned.Progress1.TotalMN8PCTotal;
                        //set actual.planned period
                        //TotalMN8Q is always planned period and TotalMN8APTotal is actual period
                        actual.Progress1.TotalMN8Q = planned.Progress1.TotalMN8Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN8PFTotal = planned.Progress1.TotalMN8PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN8APChange = actual.Progress1.TotalMN8APTotal - actual.Progress1.TotalMN8Q;
                    //cumulative change
                    actual.Progress1.TotalMN8ACChange = actual.Progress1.TotalMN8ACTotal - actual.Progress1.TotalMN8PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN8PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN8APTotal, actual.Progress1.TotalMN8Q);
                    actual.Progress1.TotalMN8PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN8ACTotal, actual.Progress1.TotalMN8PCTotal);
                    actual.Progress1.TotalMN8PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN8ACTotal, actual.Progress1.TotalMN8PFTotal);
                }
            }
        }
        private static void SetMN9Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN9Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN9Q;
                    //planned cumulative
                    planned.Progress1.TotalMN9PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN9PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN9Q;
                    actual.Progress1.TotalMN9ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN9APTotal = actual.Progress1.TotalMN9Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN9PCTotal = planned.Progress1.TotalMN9PCTotal;
                        //set actual.planned period
                        //TotalMN9Q is always planned period and TotalMN9APTotal is actual period
                        actual.Progress1.TotalMN9Q = planned.Progress1.TotalMN9Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN9PFTotal = planned.Progress1.TotalMN9PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN9APChange = actual.Progress1.TotalMN9APTotal - actual.Progress1.TotalMN9Q;
                    //cumulative change
                    actual.Progress1.TotalMN9ACChange = actual.Progress1.TotalMN9ACTotal - actual.Progress1.TotalMN9PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN9PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN9APTotal, actual.Progress1.TotalMN9Q);
                    actual.Progress1.TotalMN9PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN9ACTotal, actual.Progress1.TotalMN9PCTotal);
                    actual.Progress1.TotalMN9PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN9ACTotal, actual.Progress1.TotalMN9PFTotal);
                }
            }
        }
        private static void SetMN10Progress(MN1Stock mn1Stock, List<MN1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalQ = 0;
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalMN10Q
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalQ += planned.Progress1.TotalMN10Q;
                    //planned cumulative
                    planned.Progress1.TotalMN10PCTotal = dbPlannedTotalQ;

                }
            }
            foreach (MN1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalMN10PFTotal = dbPlannedTotalQ;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalQ = 0;
            foreach (MN1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalQ += actual.Progress1.TotalMN10Q;
                    actual.Progress1.TotalMN10ACTotal = dbActualTotalQ;
                    //set actual period using last actual total
                    actual.Progress1.TotalMN10APTotal = actual.Progress1.TotalMN10Q;
                    //set the corresponding planned totals
                    MN1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalMN10PCTotal = planned.Progress1.TotalMN10PCTotal;
                        //set actual.planned period
                        //TotalMN10Q is always planned period and TotalMN10APTotal is actual period
                        actual.Progress1.TotalMN10Q = planned.Progress1.TotalMN10Q;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalMN10PFTotal = planned.Progress1.TotalMN10PFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalMN10APChange = actual.Progress1.TotalMN10APTotal - actual.Progress1.TotalMN10Q;
                    //cumulative change
                    actual.Progress1.TotalMN10ACChange = actual.Progress1.TotalMN10ACTotal - actual.Progress1.TotalMN10PCTotal;
                    //set planned period percent
                    actual.Progress1.TotalMN10PPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN10APTotal, actual.Progress1.TotalMN10Q);
                    actual.Progress1.TotalMN10PCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN10ACTotal, actual.Progress1.TotalMN10PCTotal);
                    actual.Progress1.TotalMN10PFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalMN10ACTotal, actual.Progress1.TotalMN10PFTotal);
                }
            }
        }
        private bool AddProgsToBaseStock(MN1Stock mn1Stock,
            List<MN1Stock> progressStocks)
        {
            bool bHasAnalyses = false;
            mn1Stock.Stocks = new List<MN1Stock>();
            foreach (MN1Stock actual in progressStocks)
            {
                mn1Stock.Stocks.Add(actual);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }
    }
    public static class MN1Progress1Extensions
    {
        
    }
}
