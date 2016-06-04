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
    ///             The class tracks annual changes in totals.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class MN1Change1 : MN1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public MN1Change1(CalculatorParameters calcs)
            : base()
        {
            //subprice object
            InitTotalMN1Change1Properties(this, calcs);
        }
        //copy constructor
        public MN1Change1(MN1Change1 calculator)
            : base(calculator)
        {
            CopyTotalMN1Change1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent MN1Stock
        //calculator properties
        
        //totals names must be consistent with Total1
        //time period total
        public double TotalMN1Q { get; set; }
        public string TotalMN1Name { get; set; }
        //total change from last time period
        public double TotalMN1AmountChange { get; set; }
        //percent change from last time period
        public double TotalMN1PercentChange { get; set; }
        //total change from base mnc or mnb calculator
        public double TotalMN1BaseChange { get; set; }
        //percent change from base mnc or mnb calculator
        public double TotalMN1BasePercentChange { get; set; }

        public double TotalMN2Q { get; set; }
        public string TotalMN2Name { get; set; }
        public double TotalMN2AmountChange { get; set; }
        public double TotalMN2PercentChange { get; set; }
        public double TotalMN2BaseChange { get; set; }
        public double TotalMN2BasePercentChange { get; set; }

        public double TotalMN3Q { get; set; }
        public string TotalMN3Name { get; set; }
        public double TotalMN3AmountChange { get; set; }
        public double TotalMN3PercentChange { get; set; }
        public double TotalMN3BaseChange { get; set; }
        public double TotalMN3BasePercentChange { get; set; }

        //total mnc cost
        public double TotalMN4Q { get; set; }
        public string TotalMN4Name { get; set; }
        public double TotalMN4AmountChange { get; set; }
        public double TotalMN4PercentChange { get; set; }
        public double TotalMN4BaseChange { get; set; }
        public double TotalMN4BasePercentChange { get; set; }

        //total eaa cost (equiv ann annuity)
        public double TotalMN5Q { get; set; }
        public string TotalMN5Name { get; set; }
        public double TotalMN5AmountChange { get; set; }
        public double TotalMN5PercentChange { get; set; }
        public double TotalMN5BaseChange { get; set; }
        public double TotalMN5BasePercentChange { get; set; }

        //total per unit costs
        public double TotalMN6Q { get; set; }
        public string TotalMN6Name { get; set; }
        public double TotalMN6AmountChange { get; set; }
        public double TotalMN6PercentChange { get; set; }
        public double TotalMN6BaseChange { get; set; }
        public double TotalMN6BasePercentChange { get; set; }

        private const string cTotalMN1Q = "TMN1Q";
        private const string cTotalMN1Name = "TMN1Name";
        private const string cTotalMN1AmountChange = "TMN1AmountChange";
        private const string cTotalMN1PercentChange = "TMN1PercentChange";
        private const string cTotalMN1BaseChange = "TMN1BaseChange";
        private const string cTotalMN1BasePercentChange = "TMN1BasePercentChange";

        private const string cTotalMN2Q = "TMN2Q";
        private const string cTotalMN2Name = "TMN2Name";
        private const string cTotalMN2AmountChange = "TMN2AmountChange";
        private const string cTotalMN2PercentChange = "TMN2PercentChange";
        private const string cTotalMN2BaseChange = "TMN2BaseChange";
        private const string cTotalMN2BasePercentChange = "TMN2BasePercentChange";

        private const string cTotalMN3Q = "TMN3Q";
        private const string cTotalMN3Name = "TMN3Name";
        private const string cTotalMN3AmountChange = "TMN3AmountChange";
        private const string cTotalMN3PercentChange = "TMN3PercentChange";
        private const string cTotalMN3BaseChange = "TMN3BaseChange";
        private const string cTotalMN3BasePercentChange = "TMN3BasePercentChange";

        private const string cTotalMN4Q = "TMN4Q";
        private const string cTotalMN4Name = "TMN4Name";
        private const string cTotalMN4AmountChange = "TMN4AmountChange";
        private const string cTotalMN4PercentChange = "TMN4PercentChange";
        private const string cTotalMN4BaseChange = "TMN4BaseChange";
        private const string cTotalMN4BasePercentChange = "TMN4BasePercentChange";

        private const string cTotalMN5Q = "TMN5Q";
        private const string cTotalMN5Name = "TMN5Name";
        private const string cTotalMN5AmountChange = "TMN5AmountChange";
        private const string cTotalMN5PercentChange = "TMN5PercentChange";
        private const string cTotalMN5BaseChange = "TMN5BaseChange";
        private const string cTotalMN5BasePercentChange = "TMN5BasePercentChange";

        private const string cTotalMN6Q = "TMN6Q";
        private const string cTotalMN6Name = "TMN6Name";
        private const string cTotalMN6AmountChange = "TMN6AmountChange";
        private const string cTotalMN6PercentChange = "TMN6PercentChange";
        private const string cTotalMN6BaseChange = "TMN6BaseChange";
        private const string cTotalMN6BasePercentChange = "TMN6BasePercentChange";

       
        public double TotalMN7Q { get; set; }
        public string TotalMN7Name { get; set; }
        public double TotalMN7AmountChange { get; set; }
        public double TotalMN7PercentChange { get; set; }
        public double TotalMN7BaseChange { get; set; }
        public double TotalMN7BasePercentChange { get; set; }
        
        public double TotalMN8Q { get; set; }
        public string TotalMN8Name { get; set; }
        public double TotalMN8AmountChange { get; set; }
        public double TotalMN8PercentChange { get; set; }
        public double TotalMN8BaseChange { get; set; }
        public double TotalMN8BasePercentChange { get; set; }
       
        public double TotalMN9Q { get; set; }
        public string TotalMN9Name { get; set; }
        public double TotalMN9AmountChange { get; set; }
        public double TotalMN9PercentChange { get; set; }
        public double TotalMN9BaseChange { get; set; }
        public double TotalMN9BasePercentChange { get; set; }
      
        public double TotalMN10Q { get; set; }
        public string TotalMN10Name { get; set; }
        public double TotalMN10AmountChange { get; set; }
        public double TotalMN10PercentChange { get; set; }
        public double TotalMN10BaseChange { get; set; }
        public double TotalMN10BasePercentChange { get; set; }

        //options and salvage value taken from other capital inputs
        private const string cTotalMN7Q = "TMN7Q";
        private const string cTotalMN7Name = "TMN7Name";
        private const string cTotalMN7AmountChange = "TMN7AmountChange";
        private const string cTotalMN7PercentChange = "TMN7PercentChange";
        private const string cTotalMN7BaseChange = "TMN7BaseChange";
        private const string cTotalMN7BasePercentChange = "TMN7BasePercentChange";

        private const string cTotalMN8Q = "TMN8Q";
        private const string cTotalMN8Name = "TMN8Name";
        private const string cTotalMN8AmountChange = "TMN8AmountChange";
        private const string cTotalMN8PercentChange = "TMN8PercentChange";
        private const string cTotalMN8BaseChange = "TMN8BaseChange";
        private const string cTotalMN8BasePercentChange = "TMN8BasePercentChange";

        private const string cTotalMN9Q = "TMN9Q";
        private const string cTotalMN9Name = "TMN9Name";
        private const string cTotalMN9AmountChange = "TMN9AmountChange";
        private const string cTotalMN9PercentChange = "TMN9PercentChange";
        private const string cTotalMN9BaseChange = "TMN9BaseChange";
        private const string cTotalMN9BasePercentChange = "TMN9BasePercentChange";

        private const string cTotalMN10Q = "TMN10Q";
        private const string cTotalMN10Name = "TMN10Name";
        private const string cTotalMN10AmountChange = "TMN10AmountChange";
        private const string cTotalMN10PercentChange = "TMN10PercentChange";
        private const string cTotalMN10BaseChange = "TMN10BaseChange";
        private const string cTotalMN10BasePercentChange = "TMN10BasePercentChange";

        public void InitTotalMN1Change1Properties(MN1Change1 ind, CalculatorParameters calcs)
        {
            ind.ErrorMessage = string.Empty;

            ind.TotalMN1Q = 0;
            ind.TotalMN1Name = string.Empty;
            ind.TotalMN1AmountChange = 0;
            ind.TotalMN1PercentChange = 0;
            ind.TotalMN1BaseChange = 0;
            ind.TotalMN1BasePercentChange = 0;

            ind.TotalMN2Q = 0;
            ind.TotalMN2Name = string.Empty;
            ind.TotalMN2AmountChange = 0;
            ind.TotalMN2PercentChange = 0;
            ind.TotalMN2BaseChange = 0;
            ind.TotalMN2BasePercentChange = 0;

            ind.TotalMN3Q = 0;
            ind.TotalMN3Name = string.Empty;
            ind.TotalMN3AmountChange = 0;
            ind.TotalMN3PercentChange = 0;
            ind.TotalMN3BaseChange = 0;
            ind.TotalMN3BasePercentChange = 0;

            ind.TotalMN4Q = 0;
            ind.TotalMN4Name = string.Empty;
            ind.TotalMN4AmountChange = 0;
            ind.TotalMN4PercentChange = 0;
            ind.TotalMN4BaseChange = 0;
            ind.TotalMN4BasePercentChange = 0;

            ind.TotalMN5Q = 0;
            ind.TotalMN5Name = string.Empty;
            ind.TotalMN5AmountChange = 0;
            ind.TotalMN5PercentChange = 0;
            ind.TotalMN5BaseChange = 0;
            ind.TotalMN5BasePercentChange = 0;

            ind.TotalMN6Q = 0;
            ind.TotalMN6Name = string.Empty;
            ind.TotalMN6AmountChange = 0;
            ind.TotalMN6PercentChange = 0;
            ind.TotalMN6BaseChange = 0;
            ind.TotalMN6BasePercentChange = 0;

            ind.TotalMN7Q = 0;
            ind.TotalMN7Name = string.Empty;
            ind.TotalMN7AmountChange = 0;
            ind.TotalMN7PercentChange = 0;
            ind.TotalMN7BaseChange = 0;
            ind.TotalMN7BasePercentChange = 0;

            ind.TotalMN8Q = 0;
            ind.TotalMN8Name = string.Empty;
            ind.TotalMN8AmountChange = 0;
            ind.TotalMN8PercentChange = 0;
            ind.TotalMN8BaseChange = 0;
            ind.TotalMN8BasePercentChange = 0;

            ind.TotalMN9Q = 0;
            ind.TotalMN9Name = string.Empty;
            ind.TotalMN9AmountChange = 0;
            ind.TotalMN9PercentChange = 0;
            ind.TotalMN9BaseChange = 0;
            ind.TotalMN9BasePercentChange = 0;

            ind.TotalMN10Q = 0;
            ind.TotalMN10Name = string.Empty;
            ind.TotalMN10AmountChange = 0;
            ind.TotalMN10PercentChange = 0;
            ind.TotalMN10BaseChange = 0;
            ind.TotalMN10BasePercentChange = 0;
            ind.CalcParameters = calcs;
            ind.MNSR1Stock = new MNSR01Stock();
            ind.MNSR2Stock = new MNSR02Stock();
        }

        public void CopyTotalMN1Change1Properties(MN1Change1 ind,
            MN1Change1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalMN1Q = calculator.TotalMN1Q;
            ind.TotalMN1Name = calculator.TotalMN1Name;
            ind.TotalMN1AmountChange = calculator.TotalMN1AmountChange;
            ind.TotalMN1PercentChange = calculator.TotalMN1PercentChange;
            ind.TotalMN1BaseChange = calculator.TotalMN1BaseChange;
            ind.TotalMN1BasePercentChange = calculator.TotalMN1BasePercentChange;

            ind.TotalMN2Q = calculator.TotalMN2Q;
            ind.TotalMN2Name = calculator.TotalMN2Name;
            ind.TotalMN2AmountChange = calculator.TotalMN2AmountChange;
            ind.TotalMN2PercentChange = calculator.TotalMN2PercentChange;
            ind.TotalMN2BaseChange = calculator.TotalMN2BaseChange;
            ind.TotalMN2BasePercentChange = calculator.TotalMN2BasePercentChange;

            ind.TotalMN3Q = calculator.TotalMN3Q;
            ind.TotalMN3Name = calculator.TotalMN3Name;
            ind.TotalMN3AmountChange = calculator.TotalMN3AmountChange;
            ind.TotalMN3PercentChange = calculator.TotalMN3PercentChange;
            ind.TotalMN3BaseChange = calculator.TotalMN3BaseChange;
            ind.TotalMN3BasePercentChange = calculator.TotalMN3BasePercentChange;

            ind.TotalMN4Q = calculator.TotalMN4Q;
            ind.TotalMN4Name = calculator.TotalMN4Name;
            ind.TotalMN4AmountChange = calculator.TotalMN4AmountChange;
            ind.TotalMN4PercentChange = calculator.TotalMN4PercentChange;
            ind.TotalMN4BaseChange = calculator.TotalMN4BaseChange;
            ind.TotalMN4BasePercentChange = calculator.TotalMN4BasePercentChange;

            ind.TotalMN5Q = calculator.TotalMN5Q;
            ind.TotalMN5Name = calculator.TotalMN5Name;
            ind.TotalMN5AmountChange = calculator.TotalMN5AmountChange;
            ind.TotalMN5PercentChange = calculator.TotalMN5PercentChange;
            ind.TotalMN5BaseChange = calculator.TotalMN5BaseChange;
            ind.TotalMN5BasePercentChange = calculator.TotalMN5BasePercentChange;

            ind.TotalMN6Q = calculator.TotalMN6Q;
            ind.TotalMN6Name = calculator.TotalMN6Name;
            ind.TotalMN6AmountChange = calculator.TotalMN6AmountChange;
            ind.TotalMN6PercentChange = calculator.TotalMN6PercentChange;
            ind.TotalMN6BaseChange = calculator.TotalMN6BaseChange;
            ind.TotalMN6BasePercentChange = calculator.TotalMN6BasePercentChange;

            ind.TotalMN7Q = calculator.TotalMN7Q;
            ind.TotalMN7Name = calculator.TotalMN7Name;
            ind.TotalMN7AmountChange = calculator.TotalMN7AmountChange;
            ind.TotalMN7PercentChange = calculator.TotalMN7PercentChange;
            ind.TotalMN7BaseChange = calculator.TotalMN7BaseChange;
            ind.TotalMN7BasePercentChange = calculator.TotalMN7BasePercentChange;

            ind.TotalMN8Q = calculator.TotalMN8Q;
            ind.TotalMN8Name = calculator.TotalMN8Name;
            ind.TotalMN8AmountChange = calculator.TotalMN8AmountChange;
            ind.TotalMN8PercentChange = calculator.TotalMN8PercentChange;
            ind.TotalMN8BaseChange = calculator.TotalMN8BaseChange;
            ind.TotalMN8BasePercentChange = calculator.TotalMN8BasePercentChange;

            ind.TotalMN9Q = calculator.TotalMN9Q;
            ind.TotalMN9Name = calculator.TotalMN9Name;
            ind.TotalMN9AmountChange = calculator.TotalMN9AmountChange;
            ind.TotalMN9PercentChange = calculator.TotalMN9PercentChange;
            ind.TotalMN9BaseChange = calculator.TotalMN9BaseChange;
            ind.TotalMN9BasePercentChange = calculator.TotalMN9BasePercentChange;

            ind.TotalMN10Q = calculator.TotalMN10Q;
            ind.TotalMN10Name = calculator.TotalMN10Name;
            ind.TotalMN10AmountChange = calculator.TotalMN10AmountChange;
            ind.TotalMN10PercentChange = calculator.TotalMN10PercentChange;
            ind.TotalMN10BaseChange = calculator.TotalMN10BaseChange;
            ind.TotalMN10BasePercentChange = calculator.TotalMN10BasePercentChange;
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

        public void SetTotalMN1Change1Properties(MN1Change1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalMN1Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1Q, attNameExtension));
            ind.TotalMN1Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN1Name, attNameExtension));
            ind.TotalMN1AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1AmountChange, attNameExtension));
            ind.TotalMN1PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1PercentChange, attNameExtension));
            ind.TotalMN1BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1BaseChange, attNameExtension));
            ind.TotalMN1BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1BasePercentChange, attNameExtension));

            ind.TotalMN2Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2Q, attNameExtension));
            ind.TotalMN2Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN2Name, attNameExtension));
            ind.TotalMN2AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2AmountChange, attNameExtension));
            ind.TotalMN2PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2PercentChange, attNameExtension));
            ind.TotalMN2BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2BaseChange, attNameExtension));
            ind.TotalMN2BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2BasePercentChange, attNameExtension));

            ind.TotalMN3Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3Q, attNameExtension));
            ind.TotalMN3Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN3Name, attNameExtension));
            ind.TotalMN3AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3AmountChange, attNameExtension));
            ind.TotalMN3PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3PercentChange, attNameExtension));
            ind.TotalMN3BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3BaseChange, attNameExtension));
            ind.TotalMN3BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3BasePercentChange, attNameExtension));

            ind.TotalMN4Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4Q, attNameExtension));
            ind.TotalMN4Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN4Name, attNameExtension));
            ind.TotalMN4AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4AmountChange, attNameExtension));
            ind.TotalMN4PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4PercentChange, attNameExtension));
            ind.TotalMN4BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4BaseChange, attNameExtension));
            ind.TotalMN4BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4BasePercentChange, attNameExtension));

            ind.TotalMN5Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5Q, attNameExtension));
            ind.TotalMN5Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN5Name, attNameExtension));
            ind.TotalMN5AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5AmountChange, attNameExtension));
            ind.TotalMN5PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5PercentChange, attNameExtension));
            ind.TotalMN5BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5BaseChange, attNameExtension));
            ind.TotalMN5BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5BasePercentChange, attNameExtension));

            ind.TotalMN6Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6Q, attNameExtension));
            ind.TotalMN6Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN6Name, attNameExtension));
            ind.TotalMN6AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6AmountChange, attNameExtension));
            ind.TotalMN6PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6PercentChange, attNameExtension));
            ind.TotalMN6BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6BaseChange, attNameExtension));
            ind.TotalMN6BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6BasePercentChange, attNameExtension));

            ind.TotalMN7Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7Q, attNameExtension));
            ind.TotalMN7Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN7Name, attNameExtension));
            ind.TotalMN7AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7AmountChange, attNameExtension));
            ind.TotalMN7PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7PercentChange, attNameExtension));
            ind.TotalMN7BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7BaseChange, attNameExtension));
            ind.TotalMN7BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7BasePercentChange, attNameExtension));

            ind.TotalMN8Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8Q, attNameExtension));
            ind.TotalMN8Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN8Name, attNameExtension));
            ind.TotalMN8AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8AmountChange, attNameExtension));
            ind.TotalMN8PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8PercentChange, attNameExtension));
            ind.TotalMN8BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8BaseChange, attNameExtension));
            ind.TotalMN8BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8BasePercentChange, attNameExtension));

            ind.TotalMN9Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9Q, attNameExtension));
            ind.TotalMN9Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN9Name, attNameExtension));
            ind.TotalMN9AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9AmountChange, attNameExtension));
            ind.TotalMN9PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9PercentChange, attNameExtension));
            ind.TotalMN9BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9BaseChange, attNameExtension));
            ind.TotalMN9BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9BasePercentChange, attNameExtension));

            ind.TotalMN10Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10Q, attNameExtension));
            ind.TotalMN10Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalMN10Name, attNameExtension));
            ind.TotalMN10AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10AmountChange, attNameExtension));
            ind.TotalMN10PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10PercentChange, attNameExtension));
            ind.TotalMN10BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10BaseChange, attNameExtension));
            ind.TotalMN10BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10BasePercentChange, attNameExtension));
        }

        public void SetTotalMN1Change1Property(MN1Change1 ind,
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
                case cTotalMN1AmountChange:
                    ind.TotalMN1AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1PercentChange:
                    ind.TotalMN1PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1BaseChange:
                    ind.TotalMN1BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1BasePercentChange:
                    ind.TotalMN1BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Q:
                    ind.TotalMN2Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Name:
                    ind.TotalMN2Name = attValue;
                    break;
                case cTotalMN2AmountChange:
                    ind.TotalMN2AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2PercentChange:
                    ind.TotalMN2PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2BaseChange:
                    ind.TotalMN2BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2BasePercentChange:
                    ind.TotalMN2BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Q:
                    ind.TotalMN3Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Name:
                    ind.TotalMN3Name = attValue;
                    break;
                case cTotalMN3AmountChange:
                    ind.TotalMN3AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3PercentChange:
                    ind.TotalMN3PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3BaseChange:
                    ind.TotalMN3BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3BasePercentChange:
                    ind.TotalMN3BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Q:
                    ind.TotalMN4Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Name:
                    ind.TotalMN4Name = attValue;
                    break;
                case cTotalMN4AmountChange:
                    ind.TotalMN4AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4PercentChange:
                    ind.TotalMN4PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4BaseChange:
                    ind.TotalMN4BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4BasePercentChange:
                    ind.TotalMN4BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Q:
                    ind.TotalMN5Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Name:
                    ind.TotalMN5Name = attValue;
                    break;
                case cTotalMN5AmountChange:
                    ind.TotalMN5AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5PercentChange:
                    ind.TotalMN5PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5BaseChange:
                    ind.TotalMN5BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5BasePercentChange:
                    ind.TotalMN5BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Q:
                    ind.TotalMN6Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Name:
                    ind.TotalMN6Name = attValue;
                    break;
                case cTotalMN6AmountChange:
                    ind.TotalMN6AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6PercentChange:
                    ind.TotalMN6PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6BaseChange:
                    ind.TotalMN6BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6BasePercentChange:
                    ind.TotalMN6BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Q:
                    ind.TotalMN7Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Name:
                    ind.TotalMN7Name = attValue;
                    break;
                case cTotalMN7AmountChange:
                    ind.TotalMN7AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7PercentChange:
                    ind.TotalMN7PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7BaseChange:
                    ind.TotalMN7BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7BasePercentChange:
                    ind.TotalMN7BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Q:
                    ind.TotalMN8Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Name:
                    ind.TotalMN8Name = attValue;
                    break;
                case cTotalMN8AmountChange:
                    ind.TotalMN8AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8PercentChange:
                    ind.TotalMN8PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8BaseChange:
                    ind.TotalMN8BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8BasePercentChange:
                    ind.TotalMN8BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Q:
                    ind.TotalMN9Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Name:
                    ind.TotalMN9Name = attValue;
                    break;
                case cTotalMN9AmountChange:
                    ind.TotalMN9AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9PercentChange:
                    ind.TotalMN9PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9BaseChange:
                    ind.TotalMN9BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9BasePercentChange:
                    ind.TotalMN9BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Q:
                    ind.TotalMN10Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Name:
                    ind.TotalMN10Name = attValue;
                    break;
                case cTotalMN10AmountChange:
                    ind.TotalMN10AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10PercentChange:
                    ind.TotalMN10PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10BaseChange:
                    ind.TotalMN10BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10BasePercentChange:
                    ind.TotalMN10BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalMN1Change1Property(MN1Change1 ind, string attDateame)
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
                case cTotalMN1AmountChange:
                    sPropertyValue = ind.TotalMN1AmountChange.ToString();
                    break;
                case cTotalMN1PercentChange:
                    sPropertyValue = ind.TotalMN1PercentChange.ToString();
                    break;
                case cTotalMN1BaseChange:
                    sPropertyValue = ind.TotalMN1BaseChange.ToString();
                    break;
                case cTotalMN1BasePercentChange:
                    sPropertyValue = ind.TotalMN1BasePercentChange.ToString();
                    break;
                case cTotalMN2Q:
                    sPropertyValue = ind.TotalMN2Q.ToString();
                    break;
                case cTotalMN2Name:
                    sPropertyValue = ind.TotalMN2Name;
                    break;
                case cTotalMN2AmountChange:
                    sPropertyValue = ind.TotalMN2AmountChange.ToString();
                    break;
                case cTotalMN2PercentChange:
                    sPropertyValue = ind.TotalMN2PercentChange.ToString();
                    break;
                case cTotalMN2BaseChange:
                    sPropertyValue = ind.TotalMN2BaseChange.ToString();
                    break;
                case cTotalMN2BasePercentChange:
                    sPropertyValue = ind.TotalMN2BasePercentChange.ToString();
                    break;
                case cTotalMN3Q:
                    sPropertyValue = ind.TotalMN3Q.ToString();
                    break;
                case cTotalMN3Name:
                    sPropertyValue = ind.TotalMN3Name;
                    break;
                case cTotalMN3AmountChange:
                    sPropertyValue = ind.TotalMN3AmountChange.ToString();
                    break;
                case cTotalMN3PercentChange:
                    sPropertyValue = ind.TotalMN3PercentChange.ToString();
                    break;
                case cTotalMN3BaseChange:
                    sPropertyValue = ind.TotalMN3BaseChange.ToString();
                    break;
                case cTotalMN3BasePercentChange:
                    sPropertyValue = ind.TotalMN3BasePercentChange.ToString();
                    break;
                case cTotalMN4Q:
                    sPropertyValue = ind.TotalMN4Q.ToString();
                    break;
                case cTotalMN4Name:
                    sPropertyValue = ind.TotalMN4Name;
                    break;
                case cTotalMN4AmountChange:
                    sPropertyValue = ind.TotalMN4AmountChange.ToString();
                    break;
                case cTotalMN4PercentChange:
                    sPropertyValue = ind.TotalMN4PercentChange.ToString();
                    break;
                case cTotalMN4BaseChange:
                    sPropertyValue = ind.TotalMN4BaseChange.ToString();
                    break;
                case cTotalMN4BasePercentChange:
                    sPropertyValue = ind.TotalMN4BasePercentChange.ToString();
                    break;
                case cTotalMN5Q:
                    sPropertyValue = ind.TotalMN5Q.ToString();
                    break;
                case cTotalMN5Name:
                    sPropertyValue = ind.TotalMN5Name;
                    break;
                case cTotalMN5AmountChange:
                    sPropertyValue = ind.TotalMN5AmountChange.ToString();
                    break;
                case cTotalMN5PercentChange:
                    sPropertyValue = ind.TotalMN5PercentChange.ToString();
                    break;
                case cTotalMN5BaseChange:
                    sPropertyValue = ind.TotalMN5BaseChange.ToString();
                    break;
                case cTotalMN5BasePercentChange:
                    sPropertyValue = ind.TotalMN5BasePercentChange.ToString();
                    break;
                case cTotalMN6Q:
                    sPropertyValue = ind.TotalMN6Q.ToString();
                    break;
                case cTotalMN6Name:
                    sPropertyValue = ind.TotalMN6Name;
                    break;
                case cTotalMN6AmountChange:
                    sPropertyValue = ind.TotalMN6AmountChange.ToString();
                    break;
                case cTotalMN6PercentChange:
                    sPropertyValue = ind.TotalMN6PercentChange.ToString();
                    break;
                case cTotalMN6BaseChange:
                    sPropertyValue = ind.TotalMN6BaseChange.ToString();
                    break;
                case cTotalMN6BasePercentChange:
                    sPropertyValue = ind.TotalMN6BasePercentChange.ToString();
                    break;
                case cTotalMN7Q:
                    sPropertyValue = ind.TotalMN7Q.ToString();
                    break;
                case cTotalMN7Name:
                    sPropertyValue = ind.TotalMN7Name;
                    break;
                case cTotalMN7AmountChange:
                    sPropertyValue = ind.TotalMN7AmountChange.ToString();
                    break;
                case cTotalMN7PercentChange:
                    sPropertyValue = ind.TotalMN7PercentChange.ToString();
                    break;
                case cTotalMN7BaseChange:
                    sPropertyValue = ind.TotalMN7BaseChange.ToString();
                    break;
                case cTotalMN7BasePercentChange:
                    sPropertyValue = ind.TotalMN7BasePercentChange.ToString();
                    break;
                case cTotalMN8Q:
                    sPropertyValue = ind.TotalMN8Q.ToString();
                    break;
                case cTotalMN8Name:
                    sPropertyValue = ind.TotalMN8Name;
                    break;
                case cTotalMN8AmountChange:
                    sPropertyValue = ind.TotalMN8AmountChange.ToString();
                    break;
                case cTotalMN8PercentChange:
                    sPropertyValue = ind.TotalMN8PercentChange.ToString();
                    break;
                case cTotalMN8BaseChange:
                    sPropertyValue = ind.TotalMN8BaseChange.ToString();
                    break;
                case cTotalMN8BasePercentChange:
                    sPropertyValue = ind.TotalMN8BasePercentChange.ToString();
                    break;
                case cTotalMN9Q:
                    sPropertyValue = ind.TotalMN9Q.ToString();
                    break;
                case cTotalMN9Name:
                    sPropertyValue = ind.TotalMN9Name;
                    break;
                case cTotalMN9AmountChange:
                    sPropertyValue = ind.TotalMN9AmountChange.ToString();
                    break;
                case cTotalMN9PercentChange:
                    sPropertyValue = ind.TotalMN9PercentChange.ToString();
                    break;
                case cTotalMN9BaseChange:
                    sPropertyValue = ind.TotalMN9BaseChange.ToString();
                    break;
                case cTotalMN9BasePercentChange:
                    sPropertyValue = ind.TotalMN9BasePercentChange.ToString();
                    break;
                case cTotalMN10Q:
                    sPropertyValue = ind.TotalMN10Q.ToString();
                    break;
                case cTotalMN10Name:
                    sPropertyValue = ind.TotalMN10Name;
                    break;
                case cTotalMN10AmountChange:
                    sPropertyValue = ind.TotalMN10AmountChange.ToString();
                    break;
                case cTotalMN10PercentChange:
                    sPropertyValue = ind.TotalMN10PercentChange.ToString();
                    break;
                case cTotalMN10BaseChange:
                    sPropertyValue = ind.TotalMN10BaseChange.ToString();
                    break;
                case cTotalMN10BasePercentChange:
                    sPropertyValue = ind.TotalMN10BasePercentChange.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalMN1Change1Attributes(MN1Change1 ind,
            string attNameExtension, ref XElement calculator)
        {
            
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1Q, attNameExtension), ind.TotalMN1Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN1Name, attNameExtension), ind.TotalMN1Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1AmountChange, attNameExtension), ind.TotalMN1AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1PercentChange, attNameExtension), ind.TotalMN1PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1BaseChange, attNameExtension), ind.TotalMN1BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1BasePercentChange, attNameExtension), ind.TotalMN1BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2Q, attNameExtension), ind.TotalMN2Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN2Name, attNameExtension), ind.TotalMN2Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2AmountChange, attNameExtension), ind.TotalMN2AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2PercentChange, attNameExtension), ind.TotalMN2PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2BaseChange, attNameExtension), ind.TotalMN2BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2BasePercentChange, attNameExtension), ind.TotalMN2BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3Q, attNameExtension), ind.TotalMN3Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN3Name, attNameExtension), ind.TotalMN3Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3AmountChange, attNameExtension), ind.TotalMN3AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3PercentChange, attNameExtension), ind.TotalMN3PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3BaseChange, attNameExtension), ind.TotalMN3BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3BasePercentChange, attNameExtension), ind.TotalMN3BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4Q, attNameExtension), ind.TotalMN4Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN4Name, attNameExtension), ind.TotalMN4Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4AmountChange, attNameExtension), ind.TotalMN4AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4PercentChange, attNameExtension), ind.TotalMN4PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4BaseChange, attNameExtension), ind.TotalMN4BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4BasePercentChange, attNameExtension), ind.TotalMN4BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5Q, attNameExtension), ind.TotalMN5Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN5Name, attNameExtension), ind.TotalMN5Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5AmountChange, attNameExtension), ind.TotalMN5AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5PercentChange, attNameExtension), ind.TotalMN5PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5BaseChange, attNameExtension), ind.TotalMN5BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5BasePercentChange, attNameExtension), ind.TotalMN5BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6Q, attNameExtension), ind.TotalMN6Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN6Name, attNameExtension), ind.TotalMN6Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6AmountChange, attNameExtension), ind.TotalMN6AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6PercentChange, attNameExtension), ind.TotalMN6PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6BaseChange, attNameExtension), ind.TotalMN6BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6BasePercentChange, attNameExtension), ind.TotalMN6BasePercentChange);
           
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7Q, attNameExtension), ind.TotalMN7Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN7Name, attNameExtension), ind.TotalMN7Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7AmountChange, attNameExtension), ind.TotalMN7AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7PercentChange, attNameExtension), ind.TotalMN7PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7BaseChange, attNameExtension), ind.TotalMN7BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7BasePercentChange, attNameExtension), ind.TotalMN7BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8Q, attNameExtension), ind.TotalMN8Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN8Name, attNameExtension), ind.TotalMN8Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8AmountChange, attNameExtension), ind.TotalMN8AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8PercentChange, attNameExtension), ind.TotalMN8PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8BaseChange, attNameExtension), ind.TotalMN8BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8BasePercentChange, attNameExtension), ind.TotalMN8BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9Q, attNameExtension), ind.TotalMN9Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN9Name, attNameExtension), ind.TotalMN9Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9AmountChange, attNameExtension), ind.TotalMN9AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9PercentChange, attNameExtension), ind.TotalMN9PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9BaseChange, attNameExtension), ind.TotalMN9BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9BasePercentChange, attNameExtension), ind.TotalMN9BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10Q, attNameExtension), ind.TotalMN10Q);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN10Name, attNameExtension), ind.TotalMN10Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10AmountChange, attNameExtension), ind.TotalMN10AmountChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10PercentChange, attNameExtension), ind.TotalMN10PercentChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10BaseChange, attNameExtension), ind.TotalMN10BaseChange);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10BasePercentChange, attNameExtension), ind.TotalMN10BasePercentChange);
            
        }

        public void SetTotalMN1Change1Attributes(MN1Change1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            
            writer.WriteAttributeString(
                string.Concat(cTotalMN1Q, attNameExtension), ind.TotalMN1Q.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalMN1Name, attNameExtension), ind.TotalMN1Name);
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1AmountChange, attNameExtension), ind.TotalMN1AmountChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1PercentChange, attNameExtension), ind.TotalMN1PercentChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1BaseChange, attNameExtension), ind.TotalMN1BaseChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1BasePercentChange, attNameExtension), ind.TotalMN1BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(cTotalMN2Q, attNameExtension), ind.TotalMN2Q.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalMN2Name, attNameExtension), ind.TotalMN2Name);
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2AmountChange, attNameExtension), ind.TotalMN2AmountChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2PercentChange, attNameExtension), ind.TotalMN2PercentChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2BaseChange, attNameExtension), ind.TotalMN2BaseChange.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2BasePercentChange, attNameExtension), ind.TotalMN2BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            if (ind.TotalMN3Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN3Q, attNameExtension), ind.TotalMN3Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN3Name, attNameExtension), ind.TotalMN3Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3AmountChange, attNameExtension), ind.TotalMN3AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3PercentChange, attNameExtension), ind.TotalMN3PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3BaseChange, attNameExtension), ind.TotalMN3BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3BasePercentChange, attNameExtension), ind.TotalMN3BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN4Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN4Q, attNameExtension), ind.TotalMN4Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN4Name, attNameExtension), ind.TotalMN4Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4AmountChange, attNameExtension), ind.TotalMN4AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4PercentChange, attNameExtension), ind.TotalMN4PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4BaseChange, attNameExtension), ind.TotalMN4BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4BasePercentChange, attNameExtension), ind.TotalMN4BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN5Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN5Q, attNameExtension), ind.TotalMN5Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN5Name, attNameExtension), ind.TotalMN5Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5AmountChange, attNameExtension), ind.TotalMN5AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5PercentChange, attNameExtension), ind.TotalMN5PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5BaseChange, attNameExtension), ind.TotalMN5BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5BasePercentChange, attNameExtension), ind.TotalMN5BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN6Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN6Q, attNameExtension), ind.TotalMN6Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN6Name, attNameExtension), ind.TotalMN6Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6AmountChange, attNameExtension), ind.TotalMN6AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6PercentChange, attNameExtension), ind.TotalMN6PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6BaseChange, attNameExtension), ind.TotalMN6BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6BasePercentChange, attNameExtension), ind.TotalMN6BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN7Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN7Q, attNameExtension), ind.TotalMN7Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN7Name, attNameExtension), ind.TotalMN7Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7AmountChange, attNameExtension), ind.TotalMN7AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7PercentChange, attNameExtension), ind.TotalMN7PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7BaseChange, attNameExtension), ind.TotalMN7BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7BasePercentChange, attNameExtension), ind.TotalMN7BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN8Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN8Q, attNameExtension), ind.TotalMN8Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN8Name, attNameExtension), ind.TotalMN8Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8AmountChange, attNameExtension), ind.TotalMN8AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8PercentChange, attNameExtension), ind.TotalMN8PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8BaseChange, attNameExtension), ind.TotalMN8BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8BasePercentChange, attNameExtension), ind.TotalMN8BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN9Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN9Q, attNameExtension), ind.TotalMN9Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN9Name, attNameExtension), ind.TotalMN9Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9AmountChange, attNameExtension), ind.TotalMN9AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9PercentChange, attNameExtension), ind.TotalMN9PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9BaseChange, attNameExtension), ind.TotalMN9BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9BasePercentChange, attNameExtension), ind.TotalMN9BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN10Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN10Q, attNameExtension), ind.TotalMN10Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalMN10Name, attNameExtension), ind.TotalMN10Name);
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10AmountChange, attNameExtension), ind.TotalMN10AmountChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10PercentChange, attNameExtension), ind.TotalMN10PercentChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10BaseChange, attNameExtension), ind.TotalMN10BaseChange.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10BasePercentChange, attNameExtension), ind.TotalMN10BasePercentChange.ToString("N3", CultureInfo.InvariantCulture));
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
                mn1Stock.Change1 = new MN1Change1(this.CalcParameters);
                //need one property set
                mn1Stock.Change1.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
                CopyTotalToChangeStock(mn1Stock.Change1, mn1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing change analysis
        public bool RunAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //set calculated changestocks
            List<MN1Stock> changeStocks = new List<MN1Stock>();
            if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                changeStocks = SetIOAnalyses(mn1Stock, calcs);
            }
            else
            {
                if (mn1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || mn1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no changes
                    changeStocks = SetTotals(mn1Stock, calcs);
                }
                else if (mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //tps with currentnodename set only need nets (inputs minus outputs)
                    //note that only mn1stock is used (not changestocks)
                    changeStocks = SetTotals(mn1Stock, calcs);
                }
                else
                {
                    changeStocks = SetAnalyses(mn1Stock, calcs);
                }
            }
            //add the changestocks to parent stock
            if (changeStocks != null)
            {
                bHasAnalyses = AddChangeStocksToBaseStock(mn1Stock, changeStocks);
                //mn1Stock must still add the members of change1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }
        
        private List<MN1Stock> SetTotals(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            List<MN1Stock> changeStocks = new List<MN1Stock>();
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
                        if (stock.Change1 != null)
                        {
                             //tps start substracting outcomes from op/comps
                            if (mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                || mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                            {
                                //subtract stock2 (outputs) from stock1 (inputs) and don't add outcomes stock to collection
                                stock.Change1.SubApplicationType = stock.SubApplicationType;
                                bHasTotals = AddandSubtractSubTotalToTotalStock(mn1Stock.Change1, mn1Stock.Multiplier, stock.Change1);
                                if (bHasTotals
                                    && stock.SubApplicationType != Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
                                {
                                    changeStocks.Add(mn1Stock);
                                }
                            }
                            else
                            {
                                //calc holds an input or output stock
                                //add that stock to mn1stock (some analyses will need to use subprices too)
                                bHasTotals = AddSubTotalToTotalStock(mn1Stock.Change1, mn1Stock.Multiplier, stock.Change1);
                                if (bHasTotals)
                                {
                                    changeStocks.Add(mn1Stock);
                                }
                            }
                        }
                    }
                }
            }
            return changeStocks;
        }
        private List<MN1Stock> SetAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            List<MN1Stock> changeStocks = new List<MN1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of mn1stocks; stats run on calcs for every node
            //so budget holds tp stats; tp holds op/comp/outcome stats; 
            //but op/comp/outcome holds N number of observations defined by alternative2 property
            //object model is calc.Change1.MNSR01Stocks for costs and 2s for benefits
            //set N
            int iQN = 0;
            //calcs are aggregated by their alternative2 property
            //so calcs with alt2 = 0 are in first observation; alt2 = 2nd observation
            //put the calc totals in each observation and then run stats on observations (not calcs)
            IEnumerable<System.Linq.IGrouping<int, Calculator1>>
                calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            foreach (var calcbyalt in calcsByAlt2)
            {
                //set the calc totals in each observation
                MN1Stock observationStock = new MN1Stock();
                observationStock.Change1 = new MN1Change1(this.CalcParameters);
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(mn1Stock.GetType()))
                    {
                        MN1Stock stock = (MN1Stock)calc;
                        if (stock != null)
                        {
                            if (stock.Change1 != null)
                            {
                                MN1Stock observation2Stock = new MN1Stock();
                                stock.Multiplier = mn1Stock.Multiplier;
                                bHasTotals = SetObservationStock(changeStocks, mn1Stock,
                                    stock, observation2Stock);
                                if (bHasTotals)
                                {
                                    //add to the stats collection
                                    changeStocks.Add(observation2Stock);
                                    //N is determined from the stocks
                                    iQN++;
                                }
                            }
                        }
                    }
                }
            }
            if (iQN > 0)
            {
                bHasTotals = SetChangesAnalysis(changeStocks, mn1Stock, iQN);
            }
            return changeStocks;
        }
        private List<MN1Stock> SetIOAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            List<MN1Stock> changeStocks = new List<MN1Stock>();
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
                        //stock.Change1 holds the initial substock/price totals
                        if (stock.Change1 != null)
                        {
                            MN1Stock observation2Stock = new MN1Stock();
                            stock.Multiplier = mn1Stock.Multiplier;
                            bHasTotals = SetObservationStock(changeStocks, mn1Stock, 
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                changeStocks.Add(observation2Stock);
                                //N is determined from the stocks
                                iQN2++;
                            }
                        }
                    }
                }
            }
            if (iQN2 > 0)
            {
                bHasTotals = SetChangesAnalysis(changeStocks, mn1Stock, iQN2);
            }
            return changeStocks;
        }
        private bool SetObservationStock(List<MN1Stock> changeStocks,
            MN1Stock mn1Stock, MN1Stock stock, MN1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Change1 = new MN1Change1(this.CalcParameters);
            observation2Stock.Id = stock.Id;
            observation2Stock.Change1.Id = stock.Id;
            //copy some stock props to progress1
            BIMN1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock.Change1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BIMN1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Change1.CalcParameters == null)
                stock.Change1.CalcParameters = new CalculatorParameters();
            stock.Change1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            //at oc and outcome level no aggregating by year, id or alt
            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Change1,
                stock.Multiplier, stock.Change1);
            
            return bHasTotals;
        }
        private bool SetChangesAnalysis(List<MN1Stock> changeStocks, MN1Stock mn1Stock,
            int qN)
        {
            bool bHasTotals = false;
            //set the total observations total
            bool bHasCurrents = changeStocks.Any(c => c.ChangeType == Calculator1.CHANGE_TYPES.current.ToString());
            foreach (var stat in changeStocks)
            {
                //only current gets added to parent cumulative totals
                if (stat.ChangeType == Calculator1.CHANGE_TYPES.current.ToString())
                {
                    bHasTotals = AddSubTotalToTotalStock(mn1Stock.Change1, 1, stat.Change1);
                }
                else
                {
                    if (!bHasCurrents)
                    {
                        //no changes?, straight totals needed
                        bHasTotals = AddSubTotalToTotalStock(mn1Stock.Change1, 1, stat.Change1);
                    }
                }
            }
            if (qN > 0)
            {
                //if any changestock has this property, it's trying to compare antecedents, rather than siblings
                if (bHasCurrents)
                {
                    //budgets uses antecendent, rather than sibling, comparators
                    SetMN1BudgetChanges(mn1Stock, changeStocks);
                    SetMN2BudgetChanges(mn1Stock, changeStocks);
                    SetMN3BudgetChanges(mn1Stock, changeStocks);
                    SetMN4BudgetChanges(mn1Stock, changeStocks);
                    SetMN5BudgetChanges(mn1Stock, changeStocks);
                    SetMN6BudgetChanges(mn1Stock, changeStocks);
                    SetMN7BudgetChanges(mn1Stock, changeStocks);
                    SetMN8BudgetChanges(mn1Stock, changeStocks);
                    SetMN9BudgetChanges(mn1Stock, changeStocks);
                    SetMN10BudgetChanges(mn1Stock, changeStocks);
                }
                else
                {
                    //set change numbers
                    SetMN1Changes(mn1Stock, changeStocks);
                    SetMN2Changes(mn1Stock, changeStocks);
                    SetMN3Changes(mn1Stock, changeStocks);
                    SetMN4Changes(mn1Stock, changeStocks);
                    SetMN5Changes(mn1Stock, changeStocks);
                    SetMN6Changes(mn1Stock, changeStocks);
                    SetMN7Changes(mn1Stock, changeStocks);
                    SetMN8Changes(mn1Stock, changeStocks);
                    SetMN9Changes(mn1Stock, changeStocks);
                    SetMN10Changes(mn1Stock, changeStocks);
                }
                bHasTotals = true;
            }
            if (qN > 0)
            {
                //remove the comparators (only display the actual)
                changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.xminus1.ToString());
                changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.baseline.ToString());
            }
            return bHasTotals;
        }

        public bool CopyTotalToChangeStock(MN1Change1 totStock, MN1Total1 subTotal)
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
        private bool AddStock(MN1Change1 totStock,
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
       
        
        public bool AddSubTotalToTotalStock(MN1Change1 totStock, double multiplier,
            MN1Change1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
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
            //multiplier adjusted costs
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
        public bool AddandSubtractSubTotalToTotalStock(MN1Change1 totStock, double multiplier,
           MN1Change1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);

            //tps start using nets not totals
            if (subTotal.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
            {
                totStock.TotalMN1Q -= subTotal.TotalMN1Q;
                totStock.TotalMN2Q -= subTotal.TotalMN2Q;
                totStock.TotalMN3Q -= subTotal.TotalMN3Q;
                totStock.TotalMN4Q -= subTotal.TotalMN4Q;
                totStock.TotalMN5Q -= subTotal.TotalMN5Q;
                totStock.TotalMN6Q -= subTotal.TotalMN6Q;
                totStock.TotalMN7Q -= subTotal.TotalMN7Q;
                totStock.TotalMN8Q -= subTotal.TotalMN8Q;
                totStock.TotalMN9Q -= subTotal.TotalMN9Q;
                totStock.TotalMN10Q -= subTotal.TotalMN10Q;
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
            }
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static void ChangeSubTotalByMultipliers(MN1Change1 subTotal, double multiplier)
        {
            if (multiplier == 0)
                multiplier = 1;
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
        private static void SetMN1BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN1BaseChange = stat.Change1.TotalMN1Q - benchmark.Change1.TotalMN1Q;
                        stat.Change1.TotalMN1BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN1BaseChange, benchmark.Change1.TotalMN1Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN1AmountChange
                            = stat.Change1.TotalMN1Q - xminus1.Change1.TotalMN1Q;
                        stat.Change1.TotalMN1PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN1AmountChange, xminus1.Change1.TotalMN1Q);
                    }
                }
            }
        }
        private static MN1Stock GetChangeStockByLabel(MN1Stock actual, List<int> ids,
            List<MN1Stock> changeStocks, string changeType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            MN1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (changeStocks.Any(p => p.Label == actual.Label
                && p.ChangeType == changeType))
            {
                int iIndex = 1;
                foreach (MN1Stock change in changeStocks)
                {
                    if (change.ChangeType == changeType)
                    {
                        if (actual.Label == change.Label)
                        {
                            //make sure it hasn't already been used (2 or more els with same Labels)
                            if (!ids.Any(i => i == iIndex))
                            {
                                plannedMatch = change;
                                //index based check is ok
                                ids.Add(iIndex);
                                //break the for loop
                                break;
                            }
                            else
                            {
                                //break if no remaining planned has same label
                                bool bHasMatch = HasChangeMatchByLabel(actual.Label, change,
                                    changeStocks, changeType);
                                if (!bHasMatch)
                                {
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
        private static bool HasChangeMatchByLabel(string aggLabel,
            MN1Stock change, List<MN1Stock> changeStocks, string changeType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (MN1Stock rp in changeStocks)
            {
                if (rp.ChangeType == changeType)
                {
                    if (bStart)
                    {
                        if (aggLabel == change.Label)
                        {
                            bHasMatch = true;
                            break;
                        }
                    }
                    if (rp.Id == change.Id)
                    {
                        bStart = true;
                    }
                }
            }
            return bHasMatch;
        }
        private static void SetMN2BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN2BaseChange = stat.Change1.TotalMN2Q - benchmark.Change1.TotalMN2Q;
                        stat.Change1.TotalMN2BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN2BaseChange, benchmark.Change1.TotalMN2Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN2AmountChange
                            = stat.Change1.TotalMN2Q - xminus1.Change1.TotalMN2Q;
                        stat.Change1.TotalMN2PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN2AmountChange, xminus1.Change1.TotalMN2Q);
                    }
                }
            }
        }
        private static void SetMN3BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN3BaseChange = stat.Change1.TotalMN3Q - benchmark.Change1.TotalMN3Q;
                        stat.Change1.TotalMN3BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN3BaseChange, benchmark.Change1.TotalMN3Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN3AmountChange
                            = stat.Change1.TotalMN3Q - xminus1.Change1.TotalMN3Q;
                        stat.Change1.TotalMN3PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN3AmountChange, xminus1.Change1.TotalMN3Q);
                    }
                }
            }
        }
        private static void SetMN4BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN4BaseChange = stat.Change1.TotalMN4Q - benchmark.Change1.TotalMN4Q;
                        stat.Change1.TotalMN4BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN4BaseChange, benchmark.Change1.TotalMN4Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN4AmountChange
                            = stat.Change1.TotalMN4Q - xminus1.Change1.TotalMN4Q;
                        stat.Change1.TotalMN4PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN4AmountChange, xminus1.Change1.TotalMN4Q);
                    }
                }
            }
        }
        private static void SetMN5BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN5BaseChange = stat.Change1.TotalMN5Q - benchmark.Change1.TotalMN5Q;
                        stat.Change1.TotalMN5BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN5BaseChange, benchmark.Change1.TotalMN5Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN5AmountChange
                            = stat.Change1.TotalMN5Q - xminus1.Change1.TotalMN5Q;
                        stat.Change1.TotalMN5PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN5AmountChange, xminus1.Change1.TotalMN5Q);
                    }
                }
            }
        }
        private static void SetMN6BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN6BaseChange = stat.Change1.TotalMN6Q - benchmark.Change1.TotalMN6Q;
                        stat.Change1.TotalMN6BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN6BaseChange, benchmark.Change1.TotalMN6Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN6AmountChange
                            = stat.Change1.TotalMN6Q - xminus1.Change1.TotalMN6Q;
                        stat.Change1.TotalMN6PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN6AmountChange, xminus1.Change1.TotalMN6Q);
                    }
                }
            }
        }
        private static void SetMN7BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN7BaseChange = stat.Change1.TotalMN7Q - benchmark.Change1.TotalMN7Q;
                        stat.Change1.TotalMN7BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN7BaseChange, benchmark.Change1.TotalMN7Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN7AmountChange
                            = stat.Change1.TotalMN7Q - xminus1.Change1.TotalMN7Q;
                        stat.Change1.TotalMN7PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN7AmountChange, xminus1.Change1.TotalMN7Q);
                    }
                }
            }
        }
        private static void SetMN8BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN8BaseChange = stat.Change1.TotalMN8Q - benchmark.Change1.TotalMN8Q;
                        stat.Change1.TotalMN8BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN8BaseChange, benchmark.Change1.TotalMN8Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN8AmountChange
                            = stat.Change1.TotalMN8Q - xminus1.Change1.TotalMN8Q;
                        stat.Change1.TotalMN8PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN8AmountChange, xminus1.Change1.TotalMN8Q);
                    }
                }
            }
        }
        private static void SetMN9BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN9BaseChange = stat.Change1.TotalMN9Q - benchmark.Change1.TotalMN9Q;
                        stat.Change1.TotalMN9BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN9BaseChange, benchmark.Change1.TotalMN9Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN9AmountChange
                            = stat.Change1.TotalMN9Q - xminus1.Change1.TotalMN9Q;
                        stat.Change1.TotalMN9PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN9AmountChange, xminus1.Change1.TotalMN9Q);
                    }
                }
            }
        }
        private static void SetMN10BudgetChanges(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    MN1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalMN10BaseChange = stat.Change1.TotalMN10Q - benchmark.Change1.TotalMN10Q;
                        stat.Change1.TotalMN10BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN10BaseChange, benchmark.Change1.TotalMN10Q);
                    }
                    //set the xminus change using partialtarget tt
                    MN1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalMN10AmountChange
                            = stat.Change1.TotalMN10Q - xminus1.Change1.TotalMN10Q;
                        stat.Change1.TotalMN10PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalMN10AmountChange, xminus1.Change1.TotalMN10Q);
                    }
                }
            }
        }
        private static void SetMN1Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN1Q = 0;
            double dbLastTotalMN1Q = 0;
            int i = 0;
            List<int> ids = new List<int>();
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTotalMN1Q = stat.Change1.TotalMN1Q;
                }
                else
                {
                    if (dbLastTotalMN1Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN1AmountChange = stat.Change1.TotalMN1Q - dbLastTotalMN1Q;
                        stat.Change1.TotalMN1PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN1AmountChange, dbLastTotalMN1Q);
                    }
                    dbLastTotalMN1Q = stat.Change1.TotalMN1Q;

                    stat.Change1.TotalMN1BaseChange = stat.Change1.TotalMN1Q - dbBaseTotalMN1Q;
                    stat.Change1.TotalMN1BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN1BaseChange, dbBaseTotalMN1Q);
                }
                i++;
            }
        }
        private static void SetMN2Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN2Q = 0;
            double dbLastTotalMN2Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN2Q = stat.Change1.TotalMN2Q;
                }
                else
                {
                    if (dbLastTotalMN2Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN2AmountChange = stat.Change1.TotalMN2Q - dbLastTotalMN2Q;
                        stat.Change1.TotalMN2PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN2AmountChange, dbLastTotalMN2Q);
                    }
                    dbLastTotalMN2Q = stat.Change1.TotalMN2Q;
                    stat.Change1.TotalMN2BaseChange = stat.Change1.TotalMN2Q - dbBaseTotalMN2Q;
                    stat.Change1.TotalMN2BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN2BaseChange, dbBaseTotalMN2Q);
                }
                i++;
            }
        }
        private static void SetMN3Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN3Q = 0;
            double dbLastTotalMN3Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN3Q = stat.Change1.TotalMN3Q;
                }
                else
                {
                    if (dbLastTotalMN3Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN3AmountChange = stat.Change1.TotalMN3Q - dbLastTotalMN3Q;
                        stat.Change1.TotalMN3PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN3AmountChange, dbLastTotalMN3Q);
                    }
                    dbLastTotalMN3Q = stat.Change1.TotalMN3Q;
                    stat.Change1.TotalMN3BaseChange = stat.Change1.TotalMN3Q - dbBaseTotalMN3Q;
                    stat.Change1.TotalMN3BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN3BaseChange, dbBaseTotalMN3Q);
                }
                i++;
            }
        }
        private static void SetMN4Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN4Q = 0;
            double dbLastTotalMN4Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN4Q = stat.Change1.TotalMN4Q;
                }
                else
                {
                    //set the annual change numbers
                    if (dbLastTotalMN4Q != 0)
                    {
                        stat.Change1.TotalMN4AmountChange = stat.Change1.TotalMN4Q - dbLastTotalMN4Q;
                        stat.Change1.TotalMN4PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN4AmountChange, dbLastTotalMN4Q);
                    }
                    dbLastTotalMN4Q = stat.Change1.TotalMN4Q;
                    stat.Change1.TotalMN4BaseChange = stat.Change1.TotalMN4Q - dbBaseTotalMN4Q;
                    stat.Change1.TotalMN4BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN4BaseChange, dbBaseTotalMN4Q);
                }
                i++;
            }
        }
        private static void SetMN5Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN5Q = 0;
            double dbLastTotalMN5Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN5Q = stat.Change1.TotalMN5Q;
                }
                else
                {
                    if (dbLastTotalMN5Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN5AmountChange = stat.Change1.TotalMN5Q - dbLastTotalMN5Q;
                        stat.Change1.TotalMN5PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN5AmountChange, dbLastTotalMN5Q);
                    }
                    dbLastTotalMN5Q = stat.Change1.TotalMN5Q;
                    stat.Change1.TotalMN5BaseChange = stat.Change1.TotalMN5Q - dbBaseTotalMN5Q;
                    stat.Change1.TotalMN5BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN5BaseChange, dbBaseTotalMN5Q);
                }
                i++;
            }
        }
        private static void SetMN6Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN6Q = 0;
            double dbLastTotalMN6Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN6Q = stat.Change1.TotalMN6Q;
                }
                else
                {
                    if (dbLastTotalMN6Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN6AmountChange = stat.Change1.TotalMN6Q - dbLastTotalMN6Q;
                        stat.Change1.TotalMN6PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN6AmountChange, dbLastTotalMN6Q);
                    }
                    dbLastTotalMN6Q = stat.Change1.TotalMN6Q;
                    stat.Change1.TotalMN6BaseChange = stat.Change1.TotalMN6Q - dbBaseTotalMN6Q;
                    stat.Change1.TotalMN6BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN6BaseChange, dbBaseTotalMN6Q);
                }
                i++;
            }
        }
        private static void SetMN7Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN7Q = 0;
            double dbLastTotalMN7Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN7Q = stat.Change1.TotalMN7Q;
                }
                else
                {
                    if (dbLastTotalMN7Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN7AmountChange = stat.Change1.TotalMN7Q - dbLastTotalMN7Q;
                        stat.Change1.TotalMN7PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN7AmountChange, dbLastTotalMN7Q);
                    }
                    dbLastTotalMN7Q = stat.Change1.TotalMN7Q;
                    stat.Change1.TotalMN7BaseChange = stat.Change1.TotalMN7Q - dbBaseTotalMN7Q;
                    stat.Change1.TotalMN7BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN7BaseChange, dbBaseTotalMN7Q);
                }
                i++;
            }
        }
        private static void SetMN8Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN8Q = 0;
            double dbLastTotalMN8Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN8Q = stat.Change1.TotalMN8Q;
                }
                else
                {
                    if (dbLastTotalMN8Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN8AmountChange = stat.Change1.TotalMN8Q - dbLastTotalMN8Q;
                        stat.Change1.TotalMN8PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN8AmountChange, dbLastTotalMN8Q);
                    }
                    dbLastTotalMN8Q = stat.Change1.TotalMN8Q;
                    //set the change from first tp numbers
                    stat.Change1.TotalMN8BaseChange = stat.Change1.TotalMN8Q - dbBaseTotalMN8Q;
                    stat.Change1.TotalMN8BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN8BaseChange, dbBaseTotalMN8Q);
                }
                i++;
            }
        }
        private static void SetMN9Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN9Q = 0;
            double dbLastTotalMN9Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN9Q = stat.Change1.TotalMN9Q;
                }
                else
                {
                    if (dbLastTotalMN9Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN9AmountChange = stat.Change1.TotalMN9Q - dbLastTotalMN9Q;
                        stat.Change1.TotalMN9PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN9AmountChange, dbLastTotalMN9Q);
                    }
                    dbLastTotalMN9Q = stat.Change1.TotalMN9Q;
                    stat.Change1.TotalMN9BaseChange = stat.Change1.TotalMN9Q - dbBaseTotalMN9Q;
                    stat.Change1.TotalMN9BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN9BaseChange, dbBaseTotalMN9Q);
                }
                i++;
            }
        }
        private static void SetMN10Changes(MN1Stock mn1Stock, List<MN1Stock> changeStocks)
        {
            double dbBaseTotalMN10Q = 0;
            double dbLastTotalMN10Q = 0;
            int i = 0;
            foreach (MN1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalMN10Q = stat.Change1.TotalMN10Q;
                }
                else
                {
                    if (dbLastTotalMN10Q != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalMN10AmountChange = stat.Change1.TotalMN10Q - dbLastTotalMN10Q;
                        stat.Change1.TotalMN10PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN10AmountChange, dbLastTotalMN10Q);
                    }
                    dbLastTotalMN10Q = stat.Change1.TotalMN10Q;
                    stat.Change1.TotalMN10BaseChange = stat.Change1.TotalMN10Q - dbBaseTotalMN10Q;
                    stat.Change1.TotalMN10BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalMN10BaseChange, dbBaseTotalMN10Q);
                }
                i++;
            }
        }
        private bool AddChangeStocksToBaseStock(MN1Stock mn1Stock,
            List<MN1Stock> changeStocks)
        {
            bool bHasAnalyses = false;
            mn1Stock.Stocks = new List<MN1Stock>();
            foreach (MN1Stock changeStock in changeStocks)
            {
                //add it to the list
                mn1Stock.Stocks.Add(changeStock);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }
    }
    public static class MN1Change1Extensions
    {
        

    }
}