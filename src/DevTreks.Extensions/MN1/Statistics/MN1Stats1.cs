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
    ///             The class statistically analyzes mns.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class MN1Stat1 : MN1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public MN1Stat1(CalculatorParameters calcs)
            : base()
        {
            InitTotalMN1Stat1Properties(this, calcs);
        }
        //copy constructor
        public MN1Stat1(MN1Stat1 calculator)
            : base(calculator)
        {
            CopyTotalMN1Stat1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent MNSR01Stock
        //calculator properties
        //number of cost observations
        public double TotalQN { get; set; }
        //totals come from Total1
        public string TotalMN1Name { get; set; }
        public double TotalMN1Q { get; set; }
        public double TotalMN1Mean { get; set; }
        public double TotalMN1Median { get; set; }
        public double TotalMN1Variance { get; set; }
        public double TotalMN1StandDev { get; set; }

        public string TotalMN2Name { get; set; }
        public double TotalMN2Q { get; set; }
        public double TotalMN2Mean { get; set; }
        public double TotalMN2Median { get; set; }
        public double TotalMN2Variance { get; set; }
        public double TotalMN2StandDev { get; set; }

        public string TotalMN3Name { get; set; }
        public double TotalMN3Q { get; set; }
        public double TotalMN3Mean { get; set; }
        public double TotalMN3Median { get; set; }
        public double TotalMN3Variance { get; set; }
        public double TotalMN3StandDev { get; set; }

        public string TotalMN4Name { get; set; }
        public double TotalMN4Q { get; set; }
        public double TotalMN4Mean { get; set; }
        public double TotalMN4Median { get; set; }
        public double TotalMN4Variance { get; set; }
        public double TotalMN4StandDev { get; set; }

        public string TotalMN5Name { get; set; }
        public double TotalMN5Q { get; set; }
        public double TotalMN5Mean { get; set; }
        public double TotalMN5Median { get; set; }
        public double TotalMN5Variance { get; set; }
        public double TotalMN5StandDev { get; set; }

        public string TotalMN6Name { get; set; }
        public double TotalMN6Q { get; set; }
        public double TotalMN6Mean { get; set; }
        public double TotalMN6Median { get; set; }
        public double TotalMN6Variance { get; set; }
        public double TotalMN6StandDev { get; set; }

        private const string cTotalQN = "TQN";

        private const string cTotalMN1Name = "TMN1Name";
        private const string cTotalMN1Q = "TMN1Q";
        private const string cTotalMN1Mean = "TMN1Mean";
        private const string cTotalMN1Median = "TMN1Median";
        private const string cTotalMN1Variance = "TMN1Variance";
        private const string cTotalMN1StandDev = "TMN1StandDev";

        private const string cTotalMN2Name = "TMN2Name";
        private const string cTotalMN2Q = "TMN2Q";
        private const string cTotalMN2Mean = "TMN2Mean";
        private const string cTotalMN2Median = "TMN2Median";
        private const string cTotalMN2Variance = "TMN2Variance";
        private const string cTotalMN2StandDev = "TMN2StandDev";

        private const string cTotalMN3Name = "TMN3Name";
        private const string cTotalMN3Q = "TMN3Q";
        private const string cTotalMN3Mean = "TMN3Mean";
        private const string cTotalMN3Median = "TMN3Median";
        private const string cTotalMN3Variance = "TMN3Variance";
        private const string cTotalMN3StandDev = "TMN3StandDev";

        private const string cTotalMN4Name = "TMN4Name";
        private const string cTotalMN4Q = "TMN4Q";
        private const string cTotalMN4Mean = "TMN4Mean";
        private const string cTotalMN4Median = "TMN4Median";
        private const string cTotalMN4Variance = "TMN4Variance";
        private const string cTotalMN4StandDev = "TMN4StandDev";

        private const string cTotalMN5Name = "TMN5Name";
        private const string cTotalMN5Q = "TMN5Q";
        private const string cTotalMN5Mean = "TMN5Mean";
        private const string cTotalMN5Median = "TMN5Median";
        private const string cTotalMN5Variance = "TMN5Variance";
        private const string cTotalMN5StandDev = "TMN5StandDev";

        private const string cTotalMN6Name = "TMN6Name";
        private const string cTotalMN6Q = "TMN6Q";
        private const string cTotalMN6Mean = "TMN6Mean";
        private const string cTotalMN6Median = "TMN6Median";
        private const string cTotalMN6Variance = "TMN6Variance";
        private const string cTotalMN6StandDev = "TMN6StandDev";

        public string TotalMN7Name { get; set; }
        public double TotalMN7Q { get; set; }
        public double TotalMN7Mean { get; set; }
        public double TotalMN7Median { get; set; }
        public double TotalMN7Variance { get; set; }
        public double TotalMN7StandDev { get; set; }

        public string TotalMN8Name { get; set; }
        public double TotalMN8Q { get; set; }
        public double TotalMN8Mean { get; set; }
        public double TotalMN8Median { get; set; }
        public double TotalMN8Variance { get; set; }
        public double TotalMN8StandDev { get; set; }

        public string TotalMN9Name { get; set; }
        public double TotalMN9Q { get; set; }
        public double TotalMN9Mean { get; set; }
        public double TotalMN9Median { get; set; }
        public double TotalMN9Variance { get; set; }
        public double TotalMN9StandDev { get; set; }

        public string TotalMN10Name { get; set; }
        public double TotalMN10Q { get; set; }
        public double TotalMN10Mean { get; set; }
        public double TotalMN10Median { get; set; }
        public double TotalMN10Variance { get; set; }
        public double TotalMN10StandDev { get; set; }

        private const string cTotalMN7Name = "TMN7Name";
        private const string cTotalMN7Q = "TMN7Q";
        private const string cTotalMN7Mean = "TMN7Mean";
        private const string cTotalMN7Median = "TMN7Median";
        private const string cTotalMN7Variance = "TMN7Variance";
        private const string cTotalMN7StandDev = "TMN7StandDev";

        private const string cTotalMN8Name = "TMN8Name";
        private const string cTotalMN8Q = "TMN8Q";
        private const string cTotalMN8Mean = "TMN8Mean";
        private const string cTotalMN8Median = "TMN8Median";
        private const string cTotalMN8Variance = "TMN8Variance";
        private const string cTotalMN8StandDev = "TMN8StandDev";

        private const string cTotalMN9Name = "TMN9Name";
        private const string cTotalMN9Q = "TMN9Q";
        private const string cTotalMN9Mean = "TMN9Mean";
        private const string cTotalMN9Median = "TMN9Median";
        private const string cTotalMN9Variance = "TMN9Variance";
        private const string cTotalMN9StandDev = "TMN9StandDev";

        private const string cTotalMN10Name = "TMN10Name";
        private const string cTotalMN10Q = "TMN10Q";
        private const string cTotalMN10Mean = "TMN10Mean";
        private const string cTotalMN10Median = "TMN10Median";
        private const string cTotalMN10Variance = "TMN10Variance";
        private const string cTotalMN10StandDev = "TMN10StandDev";

        public void InitTotalMN1Stat1Properties(MN1Stat1 ind, 
            CalculatorParameters calcs)
        {
            ind.ErrorMessage = string.Empty;
        
            ind.TotalQN = 0;
            ind.TotalMN1Name = string.Empty;
            ind.TotalMN1Q = 0;
            ind.TotalMN1Mean = 0;
            ind.TotalMN1Median = 0;
            ind.TotalMN1Variance = 0;
            ind.TotalMN1StandDev = 0;

            ind.TotalMN2Name = string.Empty;
            ind.TotalMN2Q = 0;
            ind.TotalMN2Mean = 0;
            ind.TotalMN2Median = 0;
            ind.TotalMN2Variance = 0;
            ind.TotalMN2StandDev = 0;

            ind.TotalMN3Name = string.Empty;
            ind.TotalMN3Q = 0;
            ind.TotalMN3Mean = 0;
            ind.TotalMN3Median = 0;
            ind.TotalMN3Variance = 0;
            ind.TotalMN3StandDev = 0;

            ind.TotalMN4Name = string.Empty;
            ind.TotalMN4Q = 0;
            ind.TotalMN4Mean = 0;
            ind.TotalMN4Median = 0;
            ind.TotalMN4Variance = 0;
            ind.TotalMN4StandDev = 0;

            ind.TotalMN5Name = string.Empty;
            ind.TotalMN5Q = 0;
            ind.TotalMN5Mean = 0;
            ind.TotalMN5Median = 0;
            ind.TotalMN5Variance = 0;
            ind.TotalMN5StandDev = 0;

            ind.TotalMN6Name = string.Empty;
            ind.TotalMN6Q = 0;
            ind.TotalMN6Mean = 0;
            ind.TotalMN6Median = 0;
            ind.TotalMN6Variance = 0;
            ind.TotalMN6StandDev = 0;

            ind.TotalMN7Name = string.Empty;
            ind.TotalMN7Q = 0;
            ind.TotalMN7Mean = 0;
            ind.TotalMN7Median = 0;
            ind.TotalMN7Variance = 0;
            ind.TotalMN7StandDev = 0;

            ind.TotalMN8Name = string.Empty;
            ind.TotalMN8Q = 0;
            ind.TotalMN8Mean = 0;
            ind.TotalMN8Median = 0;
            ind.TotalMN8Variance = 0;
            ind.TotalMN8StandDev = 0;

            ind.TotalMN9Name = string.Empty;
            ind.TotalMN9Q = 0;
            ind.TotalMN9Mean = 0;
            ind.TotalMN9Median = 0;
            ind.TotalMN9Variance = 0;
            ind.TotalMN9StandDev = 0;

            ind.TotalMN10Name = string.Empty;
            ind.TotalMN10Q = 0;
            ind.TotalMN10Mean = 0;
            ind.TotalMN10Median = 0;
            ind.TotalMN10Variance = 0;
            ind.TotalMN10StandDev = 0;
            ind.CalcParameters = calcs;
            ind.MNSR1Stock = new MNSR01Stock();
            ind.MNSR2Stock = new MNSR02Stock();
        }

        public void CopyTotalMN1Stat1Properties(MN1Stat1 ind,
            MN1Stat1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalQN = calculator.TotalQN;

            ind.TotalMN1Name = calculator.TotalMN1Name;
            ind.TotalMN1Q = calculator.TotalMN1Q;
            ind.TotalMN1Mean = calculator.TotalMN1Mean;
            ind.TotalMN1Median = calculator.TotalMN1Median;
            ind.TotalMN1Variance = calculator.TotalMN1Variance;
            ind.TotalMN1StandDev = calculator.TotalMN1StandDev;

            ind.TotalMN2Name = calculator.TotalMN2Name;
            ind.TotalMN2Q = calculator.TotalMN2Q;
            ind.TotalMN2Mean = calculator.TotalMN2Mean;
            ind.TotalMN2Median = calculator.TotalMN2Median;
            ind.TotalMN2Variance = calculator.TotalMN2Variance;
            ind.TotalMN2StandDev = calculator.TotalMN2StandDev;

            ind.TotalMN3Name = calculator.TotalMN3Name;
            ind.TotalMN3Q = calculator.TotalMN3Q;
            ind.TotalMN3Mean = calculator.TotalMN3Mean;
            ind.TotalMN3Median = calculator.TotalMN3Median;
            ind.TotalMN3Variance = calculator.TotalMN3Variance;
            ind.TotalMN3StandDev = calculator.TotalMN3StandDev;

            ind.TotalMN4Name = calculator.TotalMN4Name;
            ind.TotalMN4Q = calculator.TotalMN4Q;
            ind.TotalMN4Mean = calculator.TotalMN4Mean;
            ind.TotalMN4Median = calculator.TotalMN4Median;
            ind.TotalMN4Variance = calculator.TotalMN4Variance;
            ind.TotalMN4StandDev = calculator.TotalMN4StandDev;

            ind.TotalMN5Name = calculator.TotalMN5Name;
            ind.TotalMN5Q = calculator.TotalMN5Q;
            ind.TotalMN5Mean = calculator.TotalMN5Mean;
            ind.TotalMN5Median = calculator.TotalMN5Median;
            ind.TotalMN5Variance = calculator.TotalMN5Variance;
            ind.TotalMN5StandDev = calculator.TotalMN5StandDev;

            ind.TotalMN6Name = calculator.TotalMN6Name;
            ind.TotalMN6Q = calculator.TotalMN6Q;
            ind.TotalMN6Mean = calculator.TotalMN6Mean;
            ind.TotalMN6Median = calculator.TotalMN6Median;
            ind.TotalMN6Variance = calculator.TotalMN6Variance;
            ind.TotalMN6StandDev = calculator.TotalMN6StandDev;

            ind.TotalMN7Name = calculator.TotalMN7Name;
            ind.TotalMN7Q = calculator.TotalMN7Q;
            ind.TotalMN7Mean = calculator.TotalMN7Mean;
            ind.TotalMN7Median = calculator.TotalMN7Median;
            ind.TotalMN7Variance = calculator.TotalMN7Variance;
            ind.TotalMN7StandDev = calculator.TotalMN7StandDev;

            ind.TotalMN8Name = calculator.TotalMN8Name;
            ind.TotalMN8Q = calculator.TotalMN8Q;
            ind.TotalMN8Mean = calculator.TotalMN8Mean;
            ind.TotalMN8Median = calculator.TotalMN8Median;
            ind.TotalMN8Variance = calculator.TotalMN8Variance;
            ind.TotalMN8StandDev = calculator.TotalMN8StandDev;

            ind.TotalMN9Name = calculator.TotalMN9Name;
            ind.TotalMN9Q = calculator.TotalMN9Q;
            ind.TotalMN9Mean = calculator.TotalMN9Mean;
            ind.TotalMN9Median = calculator.TotalMN9Median;
            ind.TotalMN9Variance = calculator.TotalMN9Variance;
            ind.TotalMN9StandDev = calculator.TotalMN9StandDev;

            ind.TotalMN10Name = calculator.TotalMN10Name;
            ind.TotalMN10Q = calculator.TotalMN10Q;
            ind.TotalMN10Mean = calculator.TotalMN10Mean;
            ind.TotalMN10Median = calculator.TotalMN10Median;
            ind.TotalMN10Variance = calculator.TotalMN10Variance;
            ind.TotalMN10StandDev = calculator.TotalMN10StandDev;
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

        public void SetTotalMN1Stat1Properties(MN1Stat1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalQN = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQN, attNameExtension));

            ind.TotalMN1Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN1Name, attNameExtension));
            ind.TotalMN1Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1Q, attNameExtension));
            ind.TotalMN1Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1Mean, attNameExtension));
            ind.TotalMN1Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1Median, attNameExtension));
            ind.TotalMN1Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1Variance, attNameExtension));
            ind.TotalMN1StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN1StandDev, attNameExtension));

            ind.TotalMN2Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN2Name, attNameExtension));
            ind.TotalMN2Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2Q, attNameExtension));
            ind.TotalMN2Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2Mean, attNameExtension));
            ind.TotalMN2Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2Median, attNameExtension));
            ind.TotalMN2Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2Variance, attNameExtension));
            ind.TotalMN2StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN2StandDev, attNameExtension));

            ind.TotalMN3Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN3Name, attNameExtension));
            ind.TotalMN3Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3Q, attNameExtension));
            ind.TotalMN3Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3Mean, attNameExtension));
            ind.TotalMN3Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3Median, attNameExtension));
            ind.TotalMN3Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3Variance, attNameExtension));
            ind.TotalMN3StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN3StandDev, attNameExtension));

            ind.TotalMN4Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN4Name, attNameExtension));
            ind.TotalMN4Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4Q, attNameExtension));
            ind.TotalMN4Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4Mean, attNameExtension));
            ind.TotalMN4Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4Median, attNameExtension));
            ind.TotalMN4Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4Variance, attNameExtension));
            ind.TotalMN4StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN4StandDev, attNameExtension));

            ind.TotalMN5Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN5Name, attNameExtension));
            ind.TotalMN5Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5Q, attNameExtension));
            ind.TotalMN5Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5Mean, attNameExtension));
            ind.TotalMN5Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5Median, attNameExtension));
            ind.TotalMN5Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5Variance, attNameExtension));
            ind.TotalMN5StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN5StandDev, attNameExtension));

            ind.TotalMN7Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN7Name, attNameExtension));
            ind.TotalMN6Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6Q, attNameExtension));
            ind.TotalMN6Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6Mean, attNameExtension));
            ind.TotalMN6Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6Median, attNameExtension));
            ind.TotalMN6Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6Variance, attNameExtension));
            ind.TotalMN6StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN6StandDev, attNameExtension));

            ind.TotalMN7Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN7Name, attNameExtension));
            ind.TotalMN7Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7Q, attNameExtension));
            ind.TotalMN7Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7Mean, attNameExtension));
            ind.TotalMN7Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7Median, attNameExtension));
            ind.TotalMN7Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7Variance, attNameExtension));
            ind.TotalMN7StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN7StandDev, attNameExtension));

            ind.TotalMN8Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN8Name, attNameExtension));
            ind.TotalMN8Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8Q, attNameExtension));
            ind.TotalMN8Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8Mean, attNameExtension));
            ind.TotalMN8Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8Median, attNameExtension));
            ind.TotalMN8Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8Variance, attNameExtension));
            ind.TotalMN8StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN8StandDev, attNameExtension));

            ind.TotalMN9Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN9Name, attNameExtension));
            ind.TotalMN9Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9Q, attNameExtension));
            ind.TotalMN9Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9Mean, attNameExtension));
            ind.TotalMN9Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9Median, attNameExtension));
            ind.TotalMN9Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9Variance, attNameExtension));
            ind.TotalMN9StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN9StandDev, attNameExtension));

            ind.TotalMN10Name = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTotalMN10Name, attNameExtension));
            ind.TotalMN10Q = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10Q, attNameExtension));
            ind.TotalMN10Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10Mean, attNameExtension));
            ind.TotalMN10Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10Median, attNameExtension));
            ind.TotalMN10Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10Variance, attNameExtension));
            ind.TotalMN10StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMN10StandDev, attNameExtension));
        }

        public void SetTotalMN1Stat1Property(MN1Stat1 ind,
            string attName, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalQN:
                    ind.TotalQN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1Name:
                    ind.TotalMN1Name = attValue;
                    break;
                case cTotalMN1Q:
                    ind.TotalMN1Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1Mean:
                    ind.TotalMN1Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1Median:
                    ind.TotalMN1Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1Variance:
                    ind.TotalMN1Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN1StandDev:
                    ind.TotalMN1StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Name:
                    ind.TotalMN2Name = attValue;
                    break;
                case cTotalMN2Q:
                    ind.TotalMN2Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Mean:
                    ind.TotalMN2Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Median:
                    ind.TotalMN2Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2Variance:
                    ind.TotalMN2Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN2StandDev:
                    ind.TotalMN2StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Name:
                    ind.TotalMN3Name = attValue;
                    break;
                case cTotalMN3Q:
                    ind.TotalMN3Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Mean:
                    ind.TotalMN3Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Median:
                    ind.TotalMN3Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3Variance:
                    ind.TotalMN3Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN3StandDev:
                    ind.TotalMN3StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Name:
                    ind.TotalMN4Name = attValue;
                    break;
                case cTotalMN4Q:
                    ind.TotalMN4Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Mean:
                    ind.TotalMN4Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Median:
                    ind.TotalMN4Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4Variance:
                    ind.TotalMN4Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN4StandDev:
                    ind.TotalMN4StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Name:
                    ind.TotalMN5Name = attValue;
                    break;
                case cTotalMN5Q:
                    ind.TotalMN5Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Mean:
                    ind.TotalMN5Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Median:
                    ind.TotalMN5Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5Variance:
                    ind.TotalMN5Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN5StandDev:
                    ind.TotalMN5StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Name:
                    ind.TotalMN6Name = attValue;
                    break;
                case cTotalMN6Q:
                    ind.TotalMN6Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Mean:
                    ind.TotalMN6Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Median:
                    ind.TotalMN6Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6Variance:
                    ind.TotalMN6Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN6StandDev:
                    ind.TotalMN6StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Name:
                    ind.TotalMN7Name = attValue;
                    break;
                case cTotalMN7Q:
                    ind.TotalMN7Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Mean:
                    ind.TotalMN7Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Median:
                    ind.TotalMN7Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7Variance:
                    ind.TotalMN7Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN7StandDev:
                    ind.TotalMN7StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Name:
                    ind.TotalMN8Name = attValue;
                    break;
                case cTotalMN8Q:
                    ind.TotalMN8Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Mean:
                    ind.TotalMN8Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Median:
                    ind.TotalMN8Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8Variance:
                    ind.TotalMN8Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN8StandDev:
                    ind.TotalMN8StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Name:
                    ind.TotalMN9Name = attValue;
                    break;
                case cTotalMN9Q:
                    ind.TotalMN9Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Mean:
                    ind.TotalMN9Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Median:
                    ind.TotalMN9Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9Variance:
                    ind.TotalMN9Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN9StandDev:
                    ind.TotalMN9StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Name:
                    ind.TotalMN10Name = attValue;
                    break;
                case cTotalMN10Q:
                    ind.TotalMN10Q = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Mean:
                    ind.TotalMN10Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Median:
                    ind.TotalMN10Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10Variance:
                    ind.TotalMN10Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMN10StandDev:
                    ind.TotalMN10StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalMN1Stat1Property(MN1Stat1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalQN:
                    sPropertyValue = ind.TotalQN.ToString();
                    break;
                case cTotalMN1Name:
                    sPropertyValue = ind.TotalMN1Name;
                    break;
                case cTotalMN1Q:
                    sPropertyValue = ind.TotalMN1Q.ToString();
                    break;
                case cTotalMN1Mean:
                    sPropertyValue = ind.TotalMN1Mean.ToString();
                    break;
                case cTotalMN1Median:
                    sPropertyValue = ind.TotalMN1Median.ToString();
                    break;
                case cTotalMN1Variance:
                    sPropertyValue = ind.TotalMN1Variance.ToString();
                    break;
                case cTotalMN1StandDev:
                    sPropertyValue = ind.TotalMN1StandDev.ToString();
                    break;
                case cTotalMN2Name:
                    sPropertyValue = ind.TotalMN2Name;
                    break;
                case cTotalMN2Q:
                    sPropertyValue = ind.TotalMN2Q.ToString();
                    break;
                case cTotalMN2Mean:
                    sPropertyValue = ind.TotalMN2Mean.ToString();
                    break;
                case cTotalMN2Median:
                    sPropertyValue = ind.TotalMN2Median.ToString();
                    break;
                case cTotalMN2Variance:
                    sPropertyValue = ind.TotalMN2Variance.ToString();
                    break;
                case cTotalMN2StandDev:
                    sPropertyValue = ind.TotalMN2StandDev.ToString();
                    break;
                case cTotalMN3Name:
                    sPropertyValue = ind.TotalMN3Name;
                    break;
                case cTotalMN3Q:
                    sPropertyValue = ind.TotalMN3Q.ToString();
                    break;
                case cTotalMN3Mean:
                    sPropertyValue = ind.TotalMN3Mean.ToString();
                    break;
                case cTotalMN3Median:
                    sPropertyValue = ind.TotalMN3Median.ToString();
                    break;
                case cTotalMN3Variance:
                    sPropertyValue = ind.TotalMN3Variance.ToString();
                    break;
                case cTotalMN3StandDev:
                    sPropertyValue = ind.TotalMN3StandDev.ToString();
                    break;
                case cTotalMN4Name:
                    sPropertyValue = ind.TotalMN4Name;
                    break;
                case cTotalMN4Q:
                    sPropertyValue = ind.TotalMN4Q.ToString();
                    break;
                case cTotalMN4Mean:
                    sPropertyValue = ind.TotalMN4Mean.ToString();
                    break;
                case cTotalMN4Median:
                    sPropertyValue = ind.TotalMN4Median.ToString();
                    break;
                case cTotalMN4Variance:
                    sPropertyValue = ind.TotalMN4Variance.ToString();
                    break;
                case cTotalMN4StandDev:
                    sPropertyValue = ind.TotalMN4StandDev.ToString();
                    break;
                case cTotalMN5Name:
                    sPropertyValue = ind.TotalMN5Name;
                    break;
                case cTotalMN5Q:
                    sPropertyValue = ind.TotalMN5Q.ToString();
                    break;
                case cTotalMN5Mean:
                    sPropertyValue = ind.TotalMN5Mean.ToString();
                    break;
                case cTotalMN5Median:
                    sPropertyValue = ind.TotalMN5Median.ToString();
                    break;
                case cTotalMN5Variance:
                    sPropertyValue = ind.TotalMN5Variance.ToString();
                    break;
                case cTotalMN5StandDev:
                    sPropertyValue = ind.TotalMN5StandDev.ToString();
                    break;
                case cTotalMN6Name:
                    sPropertyValue = ind.TotalMN6Name;
                    break;
                case cTotalMN6Q:
                    sPropertyValue = ind.TotalMN6Q.ToString();
                    break;
                case cTotalMN6Mean:
                    sPropertyValue = ind.TotalMN6Mean.ToString();
                    break;
                case cTotalMN6Median:
                    sPropertyValue = ind.TotalMN6Median.ToString();
                    break;
                case cTotalMN6Variance:
                    sPropertyValue = ind.TotalMN6Variance.ToString();
                    break;
                case cTotalMN6StandDev:
                    sPropertyValue = ind.TotalMN6StandDev.ToString();
                    break;
                case cTotalMN7Name:
                    sPropertyValue = ind.TotalMN7Name;
                    break;
                case cTotalMN7Q:
                    sPropertyValue = ind.TotalMN7Q.ToString();
                    break;
                case cTotalMN7Mean:
                    sPropertyValue = ind.TotalMN7Mean.ToString();
                    break;
                case cTotalMN7Median:
                    sPropertyValue = ind.TotalMN7Median.ToString();
                    break;
                case cTotalMN7Variance:
                    sPropertyValue = ind.TotalMN7Variance.ToString();
                    break;
                case cTotalMN7StandDev:
                    sPropertyValue = ind.TotalMN7StandDev.ToString();
                    break;
                case cTotalMN8Name:
                    sPropertyValue = ind.TotalMN8Name;
                    break;
                case cTotalMN8Q:
                    sPropertyValue = ind.TotalMN8Q.ToString();
                    break;
                case cTotalMN8Mean:
                    sPropertyValue = ind.TotalMN8Mean.ToString();
                    break;
                case cTotalMN8Median:
                    sPropertyValue = ind.TotalMN8Median.ToString();
                    break;
                case cTotalMN8Variance:
                    sPropertyValue = ind.TotalMN8Variance.ToString();
                    break;
                case cTotalMN8StandDev:
                    sPropertyValue = ind.TotalMN8StandDev.ToString();
                    break;
                case cTotalMN9Name:
                    sPropertyValue = ind.TotalMN9Name;
                    break;
                case cTotalMN9Q:
                    sPropertyValue = ind.TotalMN9Q.ToString();
                    break;
                case cTotalMN9Mean:
                    sPropertyValue = ind.TotalMN9Mean.ToString();
                    break;
                case cTotalMN9Median:
                    sPropertyValue = ind.TotalMN9Median.ToString();
                    break;
                case cTotalMN9Variance:
                    sPropertyValue = ind.TotalMN9Variance.ToString();
                    break;
                case cTotalMN9StandDev:
                    sPropertyValue = ind.TotalMN9StandDev.ToString();
                    break;
                case cTotalMN10Name:
                    sPropertyValue = ind.TotalMN10Name;
                    break;
                case cTotalMN10Q:
                    sPropertyValue = ind.TotalMN10Q.ToString();
                    break;
                case cTotalMN10Mean:
                    sPropertyValue = ind.TotalMN10Mean.ToString();
                    break;
                case cTotalMN10Median:
                    sPropertyValue = ind.TotalMN10Median.ToString();
                    break;
                case cTotalMN10Variance:
                    sPropertyValue = ind.TotalMN10Variance.ToString();
                    break;
                case cTotalMN10StandDev:
                    sPropertyValue = ind.TotalMN10StandDev.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalMN1Stat1Attributes(MN1Stat1 ind,
            string attNameExtension, ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalQN, attNameExtension), ind.TotalQN);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN1Name, attNameExtension), ind.TotalMN1Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1Q, attNameExtension), ind.TotalMN1Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1Mean, attNameExtension), ind.TotalMN1Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1Median, attNameExtension), ind.TotalMN1Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1Variance, attNameExtension), ind.TotalMN1Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN1StandDev, attNameExtension), ind.TotalMN1StandDev);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN2Name, attNameExtension), ind.TotalMN2Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2Q, attNameExtension), ind.TotalMN2Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2Mean, attNameExtension), ind.TotalMN2Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2Median, attNameExtension), ind.TotalMN2Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2Variance, attNameExtension), ind.TotalMN2Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN2StandDev, attNameExtension), ind.TotalMN2StandDev);

            
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN3Name, attNameExtension), ind.TotalMN3Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3Q, attNameExtension), ind.TotalMN3Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3Mean, attNameExtension), ind.TotalMN3Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3Median, attNameExtension), ind.TotalMN3Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3Variance, attNameExtension), ind.TotalMN3Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN3StandDev, attNameExtension), ind.TotalMN3StandDev);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN4Name, attNameExtension), ind.TotalMN4Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4Q, attNameExtension), ind.TotalMN4Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4Mean, attNameExtension), ind.TotalMN4Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4Median, attNameExtension), ind.TotalMN4Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4Variance, attNameExtension), ind.TotalMN4Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN4StandDev, attNameExtension), ind.TotalMN4StandDev);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN5Name, attNameExtension), ind.TotalMN5Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5Q, attNameExtension), ind.TotalMN5Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5Mean, attNameExtension), ind.TotalMN5Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5Median, attNameExtension), ind.TotalMN5Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5Variance, attNameExtension), ind.TotalMN5Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN5StandDev, attNameExtension), ind.TotalMN5StandDev);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN6Name, attNameExtension), ind.TotalMN6Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6Q, attNameExtension), ind.TotalMN6Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6Mean, attNameExtension), ind.TotalMN6Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6Median, attNameExtension), ind.TotalMN6Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6Variance, attNameExtension), ind.TotalMN6Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN6StandDev, attNameExtension), ind.TotalMN6StandDev);
         
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN7Name, attNameExtension), ind.TotalMN7Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7Q, attNameExtension), ind.TotalMN7Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7Mean, attNameExtension), ind.TotalMN7Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7Median, attNameExtension), ind.TotalMN7Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7Variance, attNameExtension), ind.TotalMN7Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN7StandDev, attNameExtension), ind.TotalMN7StandDev);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN8Name, attNameExtension), ind.TotalMN8Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8Q, attNameExtension), ind.TotalMN8Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8Mean, attNameExtension), ind.TotalMN8Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8Median, attNameExtension), ind.TotalMN8Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8Variance, attNameExtension), ind.TotalMN8Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN8StandDev, attNameExtension), ind.TotalMN8StandDev);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN9Name, attNameExtension), ind.TotalMN9Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9Q, attNameExtension), ind.TotalMN9Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9Mean, attNameExtension), ind.TotalMN9Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9Median, attNameExtension), ind.TotalMN9Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9Variance, attNameExtension), ind.TotalMN9Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN9StandDev, attNameExtension), ind.TotalMN9StandDev);

            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalMN10Name, attNameExtension), ind.TotalMN10Name);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10Q, attNameExtension), ind.TotalMN10Q);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10Mean, attNameExtension), ind.TotalMN10Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10Median, attNameExtension), ind.TotalMN10Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10Variance, attNameExtension), ind.TotalMN10Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cTotalMN10StandDev, attNameExtension), ind.TotalMN10StandDev);
            
        }

        public void SetTotalMN1Stat1Attributes(MN1Stat1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(cTotalQN, attNameExtension), ind.TotalQN.ToString());

            writer.WriteAttributeString(
                string.Concat(cTotalMN1Name, attNameExtension), ind.TotalMN1Name);
            writer.WriteAttributeString(
                string.Concat(cTotalMN1Q, attNameExtension), ind.TotalMN1Q.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1Mean, attNameExtension), ind.TotalMN1Mean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1Median, attNameExtension), ind.TotalMN1Median.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1Variance, attNameExtension), ind.TotalMN1Variance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN1StandDev, attNameExtension), ind.TotalMN1StandDev.ToString("N3", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(cTotalMN2Name, attNameExtension), ind.TotalMN2Name);
            writer.WriteAttributeString(
                string.Concat(cTotalMN2Q, attNameExtension), ind.TotalMN2Q.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2Mean, attNameExtension), ind.TotalMN2Mean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2Median, attNameExtension), ind.TotalMN2Median.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2Variance, attNameExtension), ind.TotalMN2Variance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMN2StandDev, attNameExtension), ind.TotalMN2StandDev.ToString("N3", CultureInfo.InvariantCulture));
            if (ind.TotalMN3Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN3Name, attNameExtension), ind.TotalMN3Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN3Q, attNameExtension), ind.TotalMN3Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3Mean, attNameExtension), ind.TotalMN3Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3Median, attNameExtension), ind.TotalMN3Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3Variance, attNameExtension), ind.TotalMN3Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN3StandDev, attNameExtension), ind.TotalMN3StandDev.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN4Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN4Name, attNameExtension), ind.TotalMN4Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN4Q, attNameExtension), ind.TotalMN4Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4Mean, attNameExtension), ind.TotalMN4Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4Median, attNameExtension), ind.TotalMN4Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4Variance, attNameExtension), ind.TotalMN4Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN4StandDev, attNameExtension), ind.TotalMN4StandDev.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN5Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN5Name, attNameExtension), ind.TotalMN5Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN5Q, attNameExtension), ind.TotalMN5Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5Mean, attNameExtension), ind.TotalMN5Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5Median, attNameExtension), ind.TotalMN5Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5Variance, attNameExtension), ind.TotalMN5Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN5StandDev, attNameExtension), ind.TotalMN5StandDev.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN6Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN6Name, attNameExtension), ind.TotalMN6Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN6Q, attNameExtension), ind.TotalMN6Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6Mean, attNameExtension), ind.TotalMN6Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6Median, attNameExtension), ind.TotalMN6Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6Variance, attNameExtension), ind.TotalMN6Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN6StandDev, attNameExtension), ind.TotalMN6StandDev.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN7Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN7Name, attNameExtension), ind.TotalMN7Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN7Q, attNameExtension), ind.TotalMN7Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7Mean, attNameExtension), ind.TotalMN7Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7Median, attNameExtension), ind.TotalMN7Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7Variance, attNameExtension), ind.TotalMN7Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN7StandDev, attNameExtension), ind.TotalMN7StandDev.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN8Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN8Name, attNameExtension), ind.TotalMN8Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN8Q, attNameExtension), ind.TotalMN8Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8Mean, attNameExtension), ind.TotalMN8Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8Median, attNameExtension), ind.TotalMN8Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8Variance, attNameExtension), ind.TotalMN8Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN8StandDev, attNameExtension), ind.TotalMN8StandDev.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN9Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN9Name, attNameExtension), ind.TotalMN9Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN9Q, attNameExtension), ind.TotalMN9Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9Mean, attNameExtension), ind.TotalMN9Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9Median, attNameExtension), ind.TotalMN9Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9Variance, attNameExtension), ind.TotalMN9Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN9StandDev, attNameExtension), ind.TotalMN9StandDev.ToString("N3", CultureInfo.InvariantCulture));
            }
            if (ind.TotalMN10Name.Length > 0)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalMN10Name, attNameExtension), ind.TotalMN10Name);
                writer.WriteAttributeString(
                    string.Concat(cTotalMN10Q, attNameExtension), ind.TotalMN10Q.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10Mean, attNameExtension), ind.TotalMN10Mean.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10Median, attNameExtension), ind.TotalMN10Median.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10Variance, attNameExtension), ind.TotalMN10Variance.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalMN10StandDev, attNameExtension), ind.TotalMN10StandDev.ToString("N3", CultureInfo.InvariantCulture));
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
                mn1Stock.Stat1 = new MN1Stat1(this.CalcParameters);

               CopyTotalToStatStock(mn1Stock.Stat1, mn1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing statistical analysis
        public bool RunAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //add totals to mn1stock.Stat1
            if (mn1Stock.Stat1 == null)
            {
                mn1Stock.Stat1 = new MN1Stat1(this.CalcParameters);
            }

            if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                //inputs and outputs calcs are calculated as separate observations (mean input price for similar inputs is meaningfull)
                bHasAnalyses = SetIOAnalyses(mn1Stock, calcs);
            }
            else
            {
                //inputs and outputs are not calculated as separate observation (mean input price for dissimilar inputs is meaningless)
                bHasAnalyses = SetAnalyses(mn1Stock, calcs);
            }
            return bHasAnalyses;
        }
        
        private bool SetAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of mn1stocks; stats run on calcs for every node
            //so budget holds tp stats; tp holds op/comp/outcome stats; 
            //but op/comp/outcome holds N number of observations defined by alternative2 property
            //object model is calc.Stat1.MNSR01Stocks for costs and 2s for benefits
            //set N
            int iQN = 0;
            //calcs are aggregated by their alternative2 property
            //so calcs with alt2 = 0 are in first observation; alt2 = 2nd observation
            //put the calc totals in each observation and then run stats on observations (not calcs)
            IEnumerable<System.Linq.IGrouping<int, Calculator1>>
                calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            //stats lists holds observation.stat1 collection with totals
            List<MN1Stat1> stats = new List<MN1Stat1>();
            foreach (var calcbyalt in calcsByAlt2)
            {
                //set the calc totals in each observation
                MN1Stock observationStock = new MN1Stock();
                observationStock.Stat1 = new MN1Stat1(this.CalcParameters);
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(mn1Stock.GetType()))
                    {
                        MN1Stock stock = (MN1Stock)calc;
                        if (stock != null)
                        {
                            if (stock.Stat1 != null)
                            {
                                //tps start substracting outcomes from op/comps
                                if (mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                    || mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                                {
                                    //subtract stock2 (outputs) from stock1 (inputs) and display stock1 net only
                                    stock.Stat1.SubApplicationType = stock.SubApplicationType;
                                    bHasTotals = AddandSubtractSubTotalToTotalStock(observationStock.Stat1, mn1Stock.Multiplier,
                                        stock.Stat1);
                                }
                                else
                                {
                                    //set each observation's totals
                                    bHasTotals = AddSubTotalToTotalStock(observationStock.Stat1, mn1Stock.Multiplier,
                                        stock.Stat1);
                                }
                            }
                        }
                    }
                }
                //add to the stats collection
                stats.Add(observationStock.Stat1);
                //N is determined from the cost MN1Stock
                iQN++;
            }
            if (iQN > 0)
            {
                bHasAnalysis = true;
                bHasTotals = SetStatsAnalysis(stats, mn1Stock, iQN);
            }
            return bHasAnalysis;
        }
        private bool SetIOAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iQN2 = 0;
            List<MN1Stat1> stats2 = new List<MN1Stat1>();
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(mn1Stock.GetType()))
                {
                    MN1Stock stock = (MN1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Stat1 != null)
                        {
                            //set the calc totals in each observation
                            MN1Stock observation2Stock = new MN1Stock();
                            observation2Stock.Stat1 = new MN1Stat1(this.CalcParameters);
                            //set each observation's totals
                            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Stat1,
                                mn1Stock.Multiplier, stock.Stat1);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                stats2.Add(observation2Stock.Stat1);
                                iQN2++;
                            }
                        }
                    }
                }
            }
            if (iQN2 > 0)
            {
                bHasAnalysis = true;
                bHasTotals = SetStatsAnalysis(stats2, mn1Stock, iQN2);
            }
            return bHasAnalysis;
        }
        private bool SetStatsAnalysis(List<MN1Stat1> stats2, MN1Stock statStock, 
            int qN)
        {
            bool bHasTotals = false;
            //set the total observations total
            foreach (var stat in stats2)
            {
                bHasTotals = AddSubTotalToTotalStock(statStock.Stat1, 1, stat);
            }
            statStock.Stat1.TotalQN = qN;
            //set the cost means
            statStock.Stat1.TotalMN1Mean = statStock.Stat1.TotalMN1Q / qN;
            statStock.Stat1.TotalMN2Mean = statStock.Stat1.TotalMN2Q / qN;
            statStock.Stat1.TotalMN3Mean = statStock.Stat1.TotalMN3Q / qN;
            statStock.Stat1.TotalMN4Mean = statStock.Stat1.TotalMN4Q / qN;
            statStock.Stat1.TotalMN5Mean = statStock.Stat1.TotalMN5Q / qN;
            statStock.Stat1.TotalMN6Mean = statStock.Stat1.TotalMN6Q / qN;
            //set the median, variance, and standard deviation costs
            SetMN1Statistics(statStock, stats2);
            SetMN2Statistics(statStock, stats2);
            SetMN3Statistics(statStock, stats2);
            SetMN4Statistics(statStock, stats2);
            SetMN5Statistics(statStock, stats2);
            SetMN6Statistics(statStock, stats2);
                
            statStock.Stat1.TotalMN7Mean = statStock.Stat1.TotalMN7Q / qN;
            statStock.Stat1.TotalMN8Mean = statStock.Stat1.TotalMN8Q / qN;
            statStock.Stat1.TotalMN9Mean = statStock.Stat1.TotalMN9Q / qN;
            statStock.Stat1.TotalMN10Mean = statStock.Stat1.TotalMN10Q / qN;
            //benefits
            SetMN7Statistics(statStock, stats2);
            SetMN8Statistics(statStock, stats2);
            SetMN9Statistics(statStock, stats2);
            SetMN10Statistics(statStock, stats2);
            
            return bHasTotals;
        }

        
        
        public bool CopyTotalToStatStock(MN1Stat1 totStock, MN1Total1 subTotal)
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
        private bool AddStock(MN1Stat1 totStock, 
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
        
        public bool AddSubTotalToTotalStock(MN1Stat1 totStock, double multiplier,
            MN1Stat1 subTotal)
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
        public bool AddandSubtractSubTotalToTotalStock(MN1Stat1 totStock, double multiplier,
            MN1Stat1 subTotal)
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
       
        public static void ChangeSubTotalByMultipliers(MN1Stat1 subTotal, double multiplier)
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
        private static void SetMN1Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN1Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN1Q - mn1Stock.Stat1.TotalMN1Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN1Median = (stat.TotalMN1Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN1Median = stat.TotalMN1Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN1Q;
            }

            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN1Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN1Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN1StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN1Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN1Variance = 0;
                mn1Stock.Stat1.TotalMN1StandDev = 0;
            }
        }
        private static void SetMN2Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN2Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN2Q - mn1Stock.Stat1.TotalMN2Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN2Median = (stat.TotalMN2Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN2Median = stat.TotalMN2Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN2Q;
            }

            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN2Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN2Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN2StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN2Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN2Variance = 0;
                mn1Stock.Stat1.TotalMN2StandDev = 0;
            }
        }
        private static void SetMN3Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN3Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN3Q - mn1Stock.Stat1.TotalMN3Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN3Median = (stat.TotalMN3Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN3Median = stat.TotalMN3Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN3Q;
            }

            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN3Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN3Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN3StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN3Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN3Variance = 0;
                mn1Stock.Stat1.TotalMN3StandDev = 0;
            }
        }
        private static void SetMN4Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN4Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN4Q - mn1Stock.Stat1.TotalMN4Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN4Median = (stat.TotalMN4Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN4Median = stat.TotalMN4Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN4Q;
            }

            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN4Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN4Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN4StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN4Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN4Variance = 0;
                mn1Stock.Stat1.TotalMN4StandDev = 0;
            }
        }
        private static void SetMN5Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN5Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN5Q - mn1Stock.Stat1.TotalMN5Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN5Median = (stat.TotalMN5Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN5Median = stat.TotalMN5Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN5Q;
            }

            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN5Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN5Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN5StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN5Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN5Variance = 0;
                mn1Stock.Stat1.TotalMN5StandDev = 0;
            }
        }
        private static void SetMN6Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN6Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN6Q - mn1Stock.Stat1.TotalMN6Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN6Median = (stat.TotalMN6Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN6Median = stat.TotalMN6Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN6Q;
            }

            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN6Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN6Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN6StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN6Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN6Variance = 0;
                mn1Stock.Stat1.TotalMN6StandDev = 0;
            }
        }
        private static void SetMN7Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN7Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN7Q - mn1Stock.Stat1.TotalMN7Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN7Median = (stat.TotalMN7Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN7Median = stat.TotalMN7Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN7Q;
            }
            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN7Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN7Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN7StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN7Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN7Variance = 0;
                mn1Stock.Stat1.TotalMN7StandDev = 0;
            }
        }
        private static void SetMN8Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN8Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN8Q - mn1Stock.Stat1.TotalMN8Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN8Median = (stat.TotalMN8Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN8Median = stat.TotalMN8Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN8Q;
            }
            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN8Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN8Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN8StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN8Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN8Variance = 0;
                mn1Stock.Stat1.TotalMN8StandDev = 0;
            }
        }
        private static void SetMN9Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN9Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN9Q - mn1Stock.Stat1.TotalMN9Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN9Median = (stat.TotalMN9Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN9Median = stat.TotalMN9Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN9Q;
            }
            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN9Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN9Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN9StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN9Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN9Variance = 0;
                mn1Stock.Stat1.TotalMN9StandDev = 0;
            }
        }
        private static void SetMN10Statistics(MN1Stock mn1Stock, List<MN1Stat1> stats)
        {
            //reorder for median
            IEnumerable<MN1Stat1> stat2s = stats.OrderByDescending(s => s.TotalMN10Q);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (MN1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalMN10Q - mn1Stock.Stat1.TotalMN10Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        mn1Stock.Stat1.TotalMN10Median = (stat.TotalMN10Q + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        mn1Stock.Stat1.TotalMN10Median = stat.TotalMN10Q;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalMN10Q;
            }
            //don't divide by zero
            if (mn1Stock.Stat1.TotalQN > 1)
            {
                //sample variance
                double dbCount = (1 / (mn1Stock.Stat1.TotalQN - 1));
                mn1Stock.Stat1.TotalMN10Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (mn1Stock.Stat1.TotalMN10Mean != 0)
                {
                    //sample standard deviation
                    mn1Stock.Stat1.TotalMN10StandDev = Math.Sqrt(mn1Stock.Stat1.TotalMN10Variance);
                }
            }
            else
            {
                mn1Stock.Stat1.TotalMN10Variance = 0;
                mn1Stock.Stat1.TotalMN10StandDev = 0;
            }
        }
    }
    public static class MN1Stat1Extensions
    {
        
    }
}
