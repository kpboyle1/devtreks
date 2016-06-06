using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The SubPriceStock2 class indirectly extends the SubPrices() class
    ///Author:		www.devtreks.org
    ///Date:		2013, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. Stock1 is for Costs and Stock2 is for Benefits, aggregation at time
    ///             period and above has to distinguish the two
    public class SubPriceStock2
    {
        //calls the base-class version, and initializes the base class properties.
        public SubPriceStock2()
            : base()
        {
            //subprice object
            InitTotalSubPriceStock2Properties();
        }
        //copy constructor
        public SubPriceStock2(SubPriceStock2 calculator)
        {
            CopyTotalSubPriceStock2Properties(calculator);
        }

        //calculator properties
        //subcosts collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<SubPrices>> SubPrice1s = null;
        public string ErrorMessage { get; set; }
        //power
        public string TotalSubP2Name1 { get; set; }
        //aggregator
        public string TotalSubP2Label1 { get; set; }
        //total discounted cost
        public double TotalSubP2Total1 { get; set; }
        //total discounted cost per unit
        public double TotalSubP2TotalPerUnit1 { get; set; }
        //total price
        public double TotalSubP2Price1 { get; set; }
        //total quantity
        public double TotalSubP2Amount1 { get; set; }


        public string TotalSubP2Name2 { get; set; }
        public string TotalSubP2Label2 { get; set; }
        public double TotalSubP2Total2 { get; set; }
        public double TotalSubP2TotalPerUnit2 { get; set; }
        public double TotalSubP2Price2 { get; set; }
        public double TotalSubP2Amount2 { get; set; }

        public string TotalSubP2Name3 { get; set; }
        public string TotalSubP2Label3 { get; set; }
        public double TotalSubP2Total3 { get; set; }
        public double TotalSubP2TotalPerUnit3 { get; set; }
        public double TotalSubP2Price3 { get; set; }
        public double TotalSubP2Amount3 { get; set; }

        public string TotalSubP2Name4 { get; set; }
        public string TotalSubP2Label4 { get; set; }
        public double TotalSubP2Total4 { get; set; }
        public double TotalSubP2TotalPerUnit4 { get; set; }
        public double TotalSubP2Price4 { get; set; }
        public double TotalSubP2Amount4 { get; set; }

        public string TotalSubP2Name5 { get; set; }
        public string TotalSubP2Label5 { get; set; }
        public double TotalSubP2Total5 { get; set; }
        public double TotalSubP2TotalPerUnit5 { get; set; }
        public double TotalSubP2Price5 { get; set; }
        public double TotalSubP2Amount5 { get; set; }

        public string TotalSubP2Name6 { get; set; }
        public string TotalSubP2Label6 { get; set; }
        public double TotalSubP2Total6 { get; set; }
        public double TotalSubP2TotalPerUnit6 { get; set; }
        public double TotalSubP2Price6 { get; set; }
        public double TotalSubP2Amount6 { get; set; }

        public string TotalSubP2Name7 { get; set; }
        public string TotalSubP2Label7 { get; set; }
        public double TotalSubP2Total7 { get; set; }
        public double TotalSubP2TotalPerUnit7 { get; set; }
        public double TotalSubP2Price7 { get; set; }
        public double TotalSubP2Amount7 { get; set; }

        public string TotalSubP2Name8 { get; set; }
        public string TotalSubP2Label8 { get; set; }
        public double TotalSubP2Total8 { get; set; }
        public double TotalSubP2TotalPerUnit8 { get; set; }
        public double TotalSubP2Price8 { get; set; }
        public double TotalSubP2Amount8 { get; set; }

        public string TotalSubP2Name9 { get; set; }
        public string TotalSubP2Label9 { get; set; }
        public double TotalSubP2Total9 { get; set; }
        public double TotalSubP2TotalPerUnit9 { get; set; }
        public double TotalSubP2Price9 { get; set; }
        public double TotalSubP2Amount9 { get; set; }

        public string TotalSubP2Name10 { get; set; }
        public string TotalSubP2Label10 { get; set; }
        public double TotalSubP2Total10 { get; set; }
        public double TotalSubP2TotalPerUnit10 { get; set; }
        public double TotalSubP2Price10 { get; set; }
        public double TotalSubP2Amount10 { get; set; }

        private const string cTotalSubP2Name1 = "TSubP2Name1";
        private const string cTotalSubP2Label1 = "TSubP2Label1";
        private const string cTotalSubP2Total1 = "TSubP2Total1";
        private const string cTotalSubP2TotalPerUnit1 = "TSubP2TotalPerUnit1";
        private const string cTotalSubP2Price1 = "TSubP2Price1";
        private const string cTotalSubP2Amount1 = "TSubP2Amount1";

        private const string cTotalSubP2Name2 = "TSubP2Name2";
        private const string cTotalSubP2Label2 = "TSubP2Label2";
        private const string cTotalSubP2Total2 = "TSubP2Total2";
        private const string cTotalSubP2TotalPerUnit2 = "TSubP2TotalPerUnit2";
        private const string cTotalSubP2Price2 = "TSubP2Price2";
        private const string cTotalSubP2Amount2 = "TSubP2Amount2";

        private const string cTotalSubP2Name3 = "TSubP2Name3";
        private const string cTotalSubP2Label3 = "TSubP2Label3";
        private const string cTotalSubP2Total3 = "TSubP2Total3";
        private const string cTotalSubP2TotalPerUnit3 = "TSubP2TotalPerUnit3";
        private const string cTotalSubP2Price3 = "TSubP2Price3";
        private const string cTotalSubP2Amount3 = "TSubP2Amount3";

        private const string cTotalSubP2Name4 = "TSubP2Name4";
        private const string cTotalSubP2Label4 = "TSubP2Label4";
        private const string cTotalSubP2Total4 = "TSubP2Total4";
        private const string cTotalSubP2TotalPerUnit4 = "TSubP2TotalPerUnit4";
        private const string cTotalSubP2Price4 = "TSubP2Price4";
        private const string cTotalSubP2Amount4 = "TSubP2Amount4";

        private const string cTotalSubP2Name5 = "TSubP2Name5";
        private const string cTotalSubP2Label5 = "TSubP2Label5";
        private const string cTotalSubP2Total5 = "TSubP2Total5";
        private const string cTotalSubP2TotalPerUnit5 = "TSubP2TotalPerUnit5";
        private const string cTotalSubP2Price5 = "TSubP2Price5";
        private const string cTotalSubP2Amount5 = "TSubP2Amount5";

        private const string cTotalSubP2Name6 = "TSubP2Name6";
        private const string cTotalSubP2Label6 = "TSubP2Label6";
        private const string cTotalSubP2Total6 = "TSubP2Total6";
        private const string cTotalSubP2TotalPerUnit6 = "TSubP2TotalPerUnit6";
        private const string cTotalSubP2Price6 = "TSubP2Price6";
        private const string cTotalSubP2Amount6 = "TSubP2Amount6";

        private const string cTotalSubP2Name7 = "TSubP2Name7";
        private const string cTotalSubP2Label7 = "TSubP2Label7";
        private const string cTotalSubP2Total7 = "TSubP2Total7";
        private const string cTotalSubP2TotalPerUnit7 = "TSubP2TotalPerUnit7";
        private const string cTotalSubP2Price7 = "TSubP2Price7";
        private const string cTotalSubP2Amount7 = "TSubP2Amount7";

        private const string cTotalSubP2Name8 = "TSubP2Name8";
        private const string cTotalSubP2Label8 = "TSubP2Label8";
        private const string cTotalSubP2Total8 = "TSubP2Total8";
        private const string cTotalSubP2TotalPerUnit8 = "TSubP2TotalPerUnit8";
        private const string cTotalSubP2Price8 = "TSubP2Price8";
        private const string cTotalSubP2Amount8 = "TSubP2Amount8";

        private const string cTotalSubP2Name9 = "TSubP2Name9";
        private const string cTotalSubP2Label9 = "TSubP2Label9";
        private const string cTotalSubP2Total9 = "TSubP2Total9";
        private const string cTotalSubP2TotalPerUnit9 = "TSubP2TotalPerUnit9";
        private const string cTotalSubP2Price9 = "TSubP2Price9";
        private const string cTotalSubP2Amount9 = "TSubP2Amount9";

        private const string cTotalSubP2Name10 = "TSubP2Name10";
        private const string cTotalSubP2Label10 = "TSubP2Label10";
        private const string cTotalSubP2Total10 = "TSubP2Total10";
        private const string cTotalSubP2TotalPerUnit10 = "TSubP2TotalPerUnit10";
        private const string cTotalSubP2Price10 = "TSubP2Price10";
        private const string cTotalSubP2Amount10 = "TSubP2Amount10";

        public virtual void InitTotalSubPriceStock2Properties()
        {
            this.ErrorMessage = string.Empty;
            this.TotalSubP2Name1 = string.Empty;
            this.TotalSubP2Label1 = string.Empty;
            this.TotalSubP2Total1 = 0;
            this.TotalSubP2TotalPerUnit1 = 0;
            this.TotalSubP2Price1 = 0;
            this.TotalSubP2Amount1 = 0;

            this.TotalSubP2Name2 = string.Empty;
            this.TotalSubP2Label2 = string.Empty;
            this.TotalSubP2Total2 = 0;
            this.TotalSubP2TotalPerUnit2 = 0;
            this.TotalSubP2Price2 = 0;
            this.TotalSubP2Amount2 = 0;

            this.TotalSubP2Name3 = string.Empty;
            this.TotalSubP2Label3 = string.Empty;
            this.TotalSubP2Total3 = 0;
            this.TotalSubP2TotalPerUnit3 = 0;
            this.TotalSubP2Price3 = 0;
            this.TotalSubP2Amount3 = 0;

            this.TotalSubP2Name4 = string.Empty;
            this.TotalSubP2Label4 = string.Empty;
            this.TotalSubP2Total4 = 0;
            this.TotalSubP2TotalPerUnit4 = 0;
            this.TotalSubP2Price4 = 0;
            this.TotalSubP2Amount4 = 0;

            this.TotalSubP2Name5 = string.Empty;
            this.TotalSubP2Label5 = string.Empty;
            this.TotalSubP2Total5 = 0;
            this.TotalSubP2TotalPerUnit5 = 0;
            this.TotalSubP2Price5 = 0;
            this.TotalSubP2Amount5 = 0;

            this.TotalSubP2Name6 = string.Empty;
            this.TotalSubP2Label6 = string.Empty;
            this.TotalSubP2Total6 = 0;
            this.TotalSubP2TotalPerUnit6 = 0;
            this.TotalSubP2Price6 = 0;
            this.TotalSubP2Amount6 = 0;

            this.TotalSubP2Name7 = string.Empty;
            this.TotalSubP2Label7 = string.Empty;
            this.TotalSubP2Total7 = 0;
            this.TotalSubP2TotalPerUnit7 = 0;
            this.TotalSubP2Price7 = 0;
            this.TotalSubP2Amount7 = 0;

            this.TotalSubP2Name8 = string.Empty;
            this.TotalSubP2Label8 = string.Empty;
            this.TotalSubP2Total8 = 0;
            this.TotalSubP2TotalPerUnit8 = 0;
            this.TotalSubP2Price8 = 0;
            this.TotalSubP2Amount8 = 0;

            this.TotalSubP2Name9 = string.Empty;
            this.TotalSubP2Label9 = string.Empty;
            this.TotalSubP2Total9 = 0;
            this.TotalSubP2TotalPerUnit9 = 0;
            this.TotalSubP2Price9 = 0;
            this.TotalSubP2Amount9 = 0;

            this.TotalSubP2Name10 = string.Empty;
            this.TotalSubP2Label10 = string.Empty;
            this.TotalSubP2Total10 = 0;
            this.TotalSubP2TotalPerUnit10 = 0;
            this.TotalSubP2Price10 = 0;
            this.TotalSubP2Amount10 = 0;
        }
        public virtual void CopyTotalSubPriceStock2Properties(
            SubPriceStock2 calculator)
        {
            this.ErrorMessage = calculator.ErrorMessage;
            this.TotalSubP2Name1 = calculator.TotalSubP2Name1;
            this.TotalSubP2Label1 = calculator.TotalSubP2Label1;
            this.TotalSubP2Total1 = calculator.TotalSubP2Total1;
            this.TotalSubP2TotalPerUnit1 = calculator.TotalSubP2TotalPerUnit1;
            this.TotalSubP2Price1 = calculator.TotalSubP2Price1;
            this.TotalSubP2Amount1 = calculator.TotalSubP2Amount1;
            this.TotalSubP2Name2 = calculator.TotalSubP2Name2;
            this.TotalSubP2Label2 = calculator.TotalSubP2Label2;
            this.TotalSubP2Total2 = calculator.TotalSubP2Total2;
            this.TotalSubP2TotalPerUnit2 = calculator.TotalSubP2TotalPerUnit2;
            this.TotalSubP2Price2 = calculator.TotalSubP2Price2;
            this.TotalSubP2Amount2 = calculator.TotalSubP2Amount2;
            this.TotalSubP2Name3 = calculator.TotalSubP2Name3;
            this.TotalSubP2Label3 = calculator.TotalSubP2Label3;
            this.TotalSubP2Total3 = calculator.TotalSubP2Total3;
            this.TotalSubP2TotalPerUnit3 = calculator.TotalSubP2TotalPerUnit3;
            this.TotalSubP2Price3 = calculator.TotalSubP2Price3;
            this.TotalSubP2Amount3 = calculator.TotalSubP2Amount3;
            this.TotalSubP2Name4 = calculator.TotalSubP2Name4;
            this.TotalSubP2Label4 = calculator.TotalSubP2Label4;
            this.TotalSubP2Total4 = calculator.TotalSubP2Total4;
            this.TotalSubP2TotalPerUnit4 = calculator.TotalSubP2TotalPerUnit4;
            this.TotalSubP2Price4 = calculator.TotalSubP2Price4;
            this.TotalSubP2Amount4 = calculator.TotalSubP2Amount4;
            this.TotalSubP2Name5 = calculator.TotalSubP2Name5;
            this.TotalSubP2Label5 = calculator.TotalSubP2Label5;
            this.TotalSubP2Total5 = calculator.TotalSubP2Total5;
            this.TotalSubP2TotalPerUnit5 = calculator.TotalSubP2TotalPerUnit5;
            this.TotalSubP2Price5 = calculator.TotalSubP2Price5;
            this.TotalSubP2Amount5 = calculator.TotalSubP2Amount5;

            this.TotalSubP2Name6 = calculator.TotalSubP2Name6;
            this.TotalSubP2Label6 = calculator.TotalSubP2Label6;
            this.TotalSubP2Total6 = calculator.TotalSubP2Total6;
            this.TotalSubP2TotalPerUnit6 = calculator.TotalSubP2TotalPerUnit6;
            this.TotalSubP2Price6 = calculator.TotalSubP2Price6;
            this.TotalSubP2Amount6 = calculator.TotalSubP2Amount6;

            this.TotalSubP2Name7 = calculator.TotalSubP2Name7;
            this.TotalSubP2Label7 = calculator.TotalSubP2Label7;
            this.TotalSubP2Total7 = calculator.TotalSubP2Total7;
            this.TotalSubP2TotalPerUnit7 = calculator.TotalSubP2TotalPerUnit7;
            this.TotalSubP2Price7 = calculator.TotalSubP2Price7;
            this.TotalSubP2Amount7 = calculator.TotalSubP2Amount7;

            this.TotalSubP2Name8 = calculator.TotalSubP2Name8;
            this.TotalSubP2Label8 = calculator.TotalSubP2Label8;
            this.TotalSubP2Total8 = calculator.TotalSubP2Total8;
            this.TotalSubP2TotalPerUnit8 = calculator.TotalSubP2TotalPerUnit8;
            this.TotalSubP2Price8 = calculator.TotalSubP2Price8;
            this.TotalSubP2Amount8 = calculator.TotalSubP2Amount8;

            this.TotalSubP2Name9 = calculator.TotalSubP2Name9;
            this.TotalSubP2Label9 = calculator.TotalSubP2Label9;
            this.TotalSubP2Total9 = calculator.TotalSubP2Total9;
            this.TotalSubP2TotalPerUnit9 = calculator.TotalSubP2TotalPerUnit9;
            this.TotalSubP2Price9 = calculator.TotalSubP2Price9;
            this.TotalSubP2Amount9 = calculator.TotalSubP2Amount9;

            this.TotalSubP2Name10 = calculator.TotalSubP2Name10;
            this.TotalSubP2Label10 = calculator.TotalSubP2Label10;
            this.TotalSubP2Total10 = calculator.TotalSubP2Total10;
            this.TotalSubP2TotalPerUnit10 = calculator.TotalSubP2TotalPerUnit10;
            this.TotalSubP2Price10 = calculator.TotalSubP2Price10;
            this.TotalSubP2Amount10 = calculator.TotalSubP2Amount10;
        }
        public virtual void SetTotalSubPriceStock2Properties(XElement calculator)
        {
            //set this object's properties
            this.TotalSubP2Name1 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name1);
            this.TotalSubP2Label1 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label1);
            this.TotalSubP2Total1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total1);
            this.TotalSubP2TotalPerUnit1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit1);
            this.TotalSubP2Price1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Price1);
            this.TotalSubP2Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount1);
            this.TotalSubP2Name2 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name2);
            this.TotalSubP2Label2 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label2);
            this.TotalSubP2Total2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total2);
            this.TotalSubP2TotalPerUnit2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit2);
            this.TotalSubP2Price2 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price2);
            this.TotalSubP2Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount2);
            this.TotalSubP2Name3 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name3);
            this.TotalSubP2Label3 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label3);
            this.TotalSubP2Total3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total3);
            this.TotalSubP2TotalPerUnit3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit3);
            this.TotalSubP2Price3 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price3);
            this.TotalSubP2Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount3);
            this.TotalSubP2Name4 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name4);
            this.TotalSubP2Label4 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label4);
            this.TotalSubP2Total4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total4);
            this.TotalSubP2TotalPerUnit4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit4);
            this.TotalSubP2Price4 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price4);
            this.TotalSubP2Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount4);
            this.TotalSubP2Name5 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name5);
            this.TotalSubP2Label5 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label5);
            this.TotalSubP2Total5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total5);
            this.TotalSubP2TotalPerUnit5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit5);
            this.TotalSubP2Price5 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price5);
            this.TotalSubP2Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount5);

            this.TotalSubP2Name6 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name6);
            this.TotalSubP2Label6 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label6);
            this.TotalSubP2Total6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total6);
            this.TotalSubP2TotalPerUnit6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit6);
            this.TotalSubP2Price6 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price6);
            this.TotalSubP2Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount6);

            this.TotalSubP2Name7 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name7);
            this.TotalSubP2Label7 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label7);
            this.TotalSubP2Total7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total7);
            this.TotalSubP2TotalPerUnit7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit7);
            this.TotalSubP2Price7 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price7);
            this.TotalSubP2Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount7);

            this.TotalSubP2Name8 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name8);
            this.TotalSubP2Label8 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label8);
            this.TotalSubP2Total8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total8);
            this.TotalSubP2TotalPerUnit8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit8);
            this.TotalSubP2Price8 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price8);
            this.TotalSubP2Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount8);

            this.TotalSubP2Name9 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name9);
            this.TotalSubP2Label9 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label9);
            this.TotalSubP2Total9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total9);
            this.TotalSubP2TotalPerUnit9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit9);
            this.TotalSubP2Price9 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price9);
            this.TotalSubP2Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount9);

            this.TotalSubP2Name10 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Name10);
            this.TotalSubP2Label10 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP2Label10);
            this.TotalSubP2Total10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Total10);
            this.TotalSubP2TotalPerUnit10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2TotalPerUnit10);
            this.TotalSubP2Price10 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP2Price10);
            this.TotalSubP2Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP2Amount10);
        }
        public virtual void SetTotalSubPriceStock2Property(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cTotalSubP2Name1:
                    this.TotalSubP2Name1 = attValue;
                    break;
                case cTotalSubP2Label1:
                    this.TotalSubP2Label1 = attValue;
                    break;
                case cTotalSubP2Total1:
                    this.TotalSubP2Total1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit1:
                    this.TotalSubP2TotalPerUnit1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price1:
                    this.TotalSubP2Price1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount1:
                    this.TotalSubP2Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Name2:
                    this.TotalSubP2Name2 = attValue;
                    break;
                case cTotalSubP2Label2:
                    this.TotalSubP2Label2 = attValue;
                    break;
                case cTotalSubP2Total2:
                    this.TotalSubP2Total2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit2:
                    this.TotalSubP2TotalPerUnit2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price2:
                    this.TotalSubP2Price2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount2:
                    this.TotalSubP2Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Name3:
                    this.TotalSubP2Name3 = attValue;
                    break;
                case cTotalSubP2Label3:
                    this.TotalSubP2Label3 = attValue;
                    break;
                case cTotalSubP2Total3:
                    this.TotalSubP2Total3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit3:
                    this.TotalSubP2TotalPerUnit3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price3:
                    this.TotalSubP2Price3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount3:
                    this.TotalSubP2Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Name4:
                    this.TotalSubP2Name4 = attValue;
                    break;
                case cTotalSubP2Label4:
                    this.TotalSubP2Label4 = attValue;
                    break;
                case cTotalSubP2Total4:
                    this.TotalSubP2Total4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit4:
                    this.TotalSubP2TotalPerUnit4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price4:
                    this.TotalSubP2Price4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount4:
                    this.TotalSubP2Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Name5:
                    this.TotalSubP2Name5 = attValue;
                    break;
                case cTotalSubP2Label5:
                    this.TotalSubP2Label5 = attValue;
                    break;
                case cTotalSubP2Total5:
                    this.TotalSubP2Total5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit5:
                    this.TotalSubP2TotalPerUnit5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price5:
                    this.TotalSubP2Price5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount5:
                    this.TotalSubP2Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP2Name6:
                    this.TotalSubP2Name6 = attValue;
                    break;
                case cTotalSubP2Label6:
                    this.TotalSubP2Label6 = attValue;
                    break;
                case cTotalSubP2Total6:
                    this.TotalSubP2Total6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit6:
                    this.TotalSubP2TotalPerUnit6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price6:
                    this.TotalSubP2Price6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount6:
                    this.TotalSubP2Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP2Name7:
                    this.TotalSubP2Name7 = attValue;
                    break;
                case cTotalSubP2Label7:
                    this.TotalSubP2Label7 = attValue;
                    break;
                case cTotalSubP2Total7:
                    this.TotalSubP2Total7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit7:
                    this.TotalSubP2TotalPerUnit7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price7:
                    this.TotalSubP2Price7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount7:
                    this.TotalSubP2Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP2Name8:
                    this.TotalSubP2Name8 = attValue;
                    break;
                case cTotalSubP2Label8:
                    this.TotalSubP2Label8 = attValue;
                    break;
                case cTotalSubP2Total8:
                    this.TotalSubP2Total8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit8:
                    this.TotalSubP2TotalPerUnit8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price8:
                    this.TotalSubP2Price8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount8:
                    this.TotalSubP2Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP2Name9:
                    this.TotalSubP2Name9 = attValue;
                    break;
                case cTotalSubP2Label9:
                    this.TotalSubP2Label9 = attValue;
                    break;
                case cTotalSubP2Total9:
                    this.TotalSubP2Total9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit9:
                    this.TotalSubP2TotalPerUnit9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price9:
                    this.TotalSubP2Price9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount9:
                    this.TotalSubP2Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP2Name10:
                    this.TotalSubP2Name10 = attValue;
                    break;
                case cTotalSubP2Label10:
                    this.TotalSubP2Label10 = attValue;
                    break;
                case cTotalSubP2Total10:
                    this.TotalSubP2Total10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit10:
                    this.TotalSubP2TotalPerUnit10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price10:
                    this.TotalSubP2Price10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Amount10:
                    this.TotalSubP2Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalSubPriceStock2Property(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSubP2Name1:
                    sPropertyValue = this.TotalSubP2Name1.ToString();
                    break;
                case cTotalSubP2Label1:
                    sPropertyValue = this.TotalSubP2Label1.ToString();
                    break;
                case cTotalSubP2Total1:
                    sPropertyValue = this.TotalSubP2Total1.ToString();
                    break;
                case cTotalSubP2TotalPerUnit1:
                    sPropertyValue = this.TotalSubP2TotalPerUnit1.ToString();
                    break;
                case cTotalSubP2Price1:
                    sPropertyValue = this.TotalSubP2Price1.ToString();
                    break;
                case cTotalSubP2Amount1:
                    sPropertyValue = this.TotalSubP2Amount1.ToString();
                    break;
                case cTotalSubP2Name2:
                    sPropertyValue = this.TotalSubP2Name2.ToString();
                    break;
                case cTotalSubP2Label2:
                    sPropertyValue = this.TotalSubP2Label2.ToString();
                    break;
                case cTotalSubP2Total2:
                    sPropertyValue = this.TotalSubP2Total2.ToString();
                    break;
                case cTotalSubP2TotalPerUnit2:
                    sPropertyValue = this.TotalSubP2TotalPerUnit2.ToString();
                    break;
                case cTotalSubP2Price2:
                    sPropertyValue = this.TotalSubP2Price2.ToString();
                    break;
                case cTotalSubP2Amount2:
                    sPropertyValue = this.TotalSubP2Amount2.ToString();
                    break;
                case cTotalSubP2Name3:
                    sPropertyValue = this.TotalSubP2Name3.ToString();
                    break;
                case cTotalSubP2Label3:
                    sPropertyValue = this.TotalSubP2Label3.ToString();
                    break;
                case cTotalSubP2Total3:
                    sPropertyValue = this.TotalSubP2Total3.ToString();
                    break;
                case cTotalSubP2TotalPerUnit3:
                    sPropertyValue = this.TotalSubP2TotalPerUnit3.ToString();
                    break;
                case cTotalSubP2Price3:
                    sPropertyValue = this.TotalSubP2Price3.ToString();
                    break;
                case cTotalSubP2Amount3:
                    sPropertyValue = this.TotalSubP2Amount3.ToString();
                    break;
                case cTotalSubP2Name4:
                    sPropertyValue = this.TotalSubP2Name4.ToString();
                    break;
                case cTotalSubP2Label4:
                    sPropertyValue = this.TotalSubP2Label4.ToString();
                    break;
                case cTotalSubP2Total4:
                    sPropertyValue = this.TotalSubP2Total4.ToString();
                    break;
                case cTotalSubP2TotalPerUnit4:
                    sPropertyValue = this.TotalSubP2TotalPerUnit4.ToString();
                    break;
                case cTotalSubP2Price4:
                    sPropertyValue = this.TotalSubP2Price4.ToString();
                    break;
                case cTotalSubP2Amount4:
                    sPropertyValue = this.TotalSubP2Amount4.ToString();
                    break;
                case cTotalSubP2Name5:
                    sPropertyValue = this.TotalSubP2Name5.ToString();
                    break;
                case cTotalSubP2Label5:
                    sPropertyValue = this.TotalSubP2Label5.ToString();
                    break;
                case cTotalSubP2Total5:
                    sPropertyValue = this.TotalSubP2Total5.ToString();
                    break;
                case cTotalSubP2TotalPerUnit5:
                    sPropertyValue = this.TotalSubP2TotalPerUnit5.ToString();
                    break;
                case cTotalSubP2Price5:
                    sPropertyValue = this.TotalSubP2Price5.ToString();
                    break;
                case cTotalSubP2Amount5:
                    sPropertyValue = this.TotalSubP2Amount5.ToString();
                    break;

                case cTotalSubP2Name6:
                    sPropertyValue = this.TotalSubP2Name6.ToString();
                    break;
                case cTotalSubP2Label6:
                    sPropertyValue = this.TotalSubP2Label6.ToString();
                    break;
                case cTotalSubP2Total6:
                    sPropertyValue = this.TotalSubP2Total6.ToString();
                    break;
                case cTotalSubP2TotalPerUnit6:
                    sPropertyValue = this.TotalSubP2TotalPerUnit6.ToString();
                    break;
                case cTotalSubP2Price6:
                    sPropertyValue = this.TotalSubP2Price6.ToString();
                    break;
                case cTotalSubP2Amount6:
                    sPropertyValue = this.TotalSubP2Amount6.ToString();
                    break;

                case cTotalSubP2Name7:
                    sPropertyValue = this.TotalSubP2Name7.ToString();
                    break;
                case cTotalSubP2Label7:
                    sPropertyValue = this.TotalSubP2Label7.ToString();
                    break;
                case cTotalSubP2Total7:
                    sPropertyValue = this.TotalSubP2Total7.ToString();
                    break;
                case cTotalSubP2TotalPerUnit7:
                    sPropertyValue = this.TotalSubP2TotalPerUnit7.ToString();
                    break;
                case cTotalSubP2Price7:
                    sPropertyValue = this.TotalSubP2Price7.ToString();
                    break;
                case cTotalSubP2Amount7:
                    sPropertyValue = this.TotalSubP2Amount7.ToString();
                    break;

                case cTotalSubP2Name8:
                    sPropertyValue = this.TotalSubP2Name8.ToString();
                    break;
                case cTotalSubP2Label8:
                    sPropertyValue = this.TotalSubP2Label8.ToString();
                    break;
                case cTotalSubP2Total8:
                    sPropertyValue = this.TotalSubP2Total8.ToString();
                    break;
                case cTotalSubP2TotalPerUnit8:
                    sPropertyValue = this.TotalSubP2TotalPerUnit8.ToString();
                    break;
                case cTotalSubP2Price8:
                    sPropertyValue = this.TotalSubP2Price8.ToString();
                    break;
                case cTotalSubP2Amount8:
                    sPropertyValue = this.TotalSubP2Amount8.ToString();
                    break;

                case cTotalSubP2Name9:
                    sPropertyValue = this.TotalSubP2Name9.ToString();
                    break;
                case cTotalSubP2Label9:
                    sPropertyValue = this.TotalSubP2Label9.ToString();
                    break;
                case cTotalSubP2Total9:
                    sPropertyValue = this.TotalSubP2Total9.ToString();
                    break;
                case cTotalSubP2TotalPerUnit9:
                    sPropertyValue = this.TotalSubP2TotalPerUnit9.ToString();
                    break;
                case cTotalSubP2Price9:
                    sPropertyValue = this.TotalSubP2Price9.ToString();
                    break;
                case cTotalSubP2Amount9:
                    sPropertyValue = this.TotalSubP2Amount9.ToString();
                    break;

                case cTotalSubP2Name10:
                    sPropertyValue = this.TotalSubP2Name10.ToString();
                    break;
                case cTotalSubP2Label10:
                    sPropertyValue = this.TotalSubP2Label10.ToString();
                    break;
                case cTotalSubP2Total10:
                    sPropertyValue = this.TotalSubP2Total10.ToString();
                    break;
                case cTotalSubP2TotalPerUnit10:
                    sPropertyValue = this.TotalSubP2TotalPerUnit10.ToString();
                    break;
                case cTotalSubP2Price10:
                    sPropertyValue = this.TotalSubP2Price10.ToString();
                    break;
                case cTotalSubP2Amount10:
                    sPropertyValue = this.TotalSubP2Amount10.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalSubPriceStock2Attributes(string attNameExtension,
            ref XElement calculator)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (this.TotalSubP2Name1 != string.Empty && this.TotalSubP2Name1 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name1, attNameExtension), this.TotalSubP2Name1);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label1, attNameExtension), this.TotalSubP2Label1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total1, attNameExtension), this.TotalSubP2Total1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit1, attNameExtension), this.TotalSubP2TotalPerUnit1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price1, attNameExtension), this.TotalSubP2Price1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount1, attNameExtension), this.TotalSubP2Amount1);
            }
            if (this.TotalSubP2Name2 != string.Empty && this.TotalSubP2Name2 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name2, attNameExtension), this.TotalSubP2Name2);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label2, attNameExtension), this.TotalSubP2Label2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total2, attNameExtension), this.TotalSubP2Total2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit2, attNameExtension), this.TotalSubP2TotalPerUnit2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price2, attNameExtension), this.TotalSubP2Price2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount2, attNameExtension), this.TotalSubP2Amount2);
            }
            if (this.TotalSubP2Name3 != string.Empty && this.TotalSubP2Name3 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name3, attNameExtension), this.TotalSubP2Name3);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label3, attNameExtension), this.TotalSubP2Label3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total3, attNameExtension), this.TotalSubP2Total3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit3, attNameExtension), this.TotalSubP2TotalPerUnit3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price3, attNameExtension), this.TotalSubP2Price3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount3, attNameExtension), this.TotalSubP2Amount3);
            }
            if (this.TotalSubP2Name4 != string.Empty && this.TotalSubP2Name4 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name4, attNameExtension), this.TotalSubP2Name4);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label4, attNameExtension), this.TotalSubP2Label4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total4, attNameExtension), this.TotalSubP2Total4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit4, attNameExtension), this.TotalSubP2TotalPerUnit4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price4, attNameExtension), this.TotalSubP2Price4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount4, attNameExtension), this.TotalSubP2Amount4);
            }
            if (this.TotalSubP2Name5 != string.Empty && this.TotalSubP2Name5 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name5, attNameExtension), this.TotalSubP2Name5);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label5, attNameExtension), this.TotalSubP2Label5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total5, attNameExtension), this.TotalSubP2Total5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit5, attNameExtension), this.TotalSubP2TotalPerUnit5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price5, attNameExtension), this.TotalSubP2Price5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount5, attNameExtension), this.TotalSubP2Amount5);
            }

            if (this.TotalSubP2Name6 != string.Empty && this.TotalSubP2Name6 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name6, attNameExtension), this.TotalSubP2Name6);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label6, attNameExtension), this.TotalSubP2Label6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total6, attNameExtension), this.TotalSubP2Total6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit6, attNameExtension), this.TotalSubP2TotalPerUnit6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price6, attNameExtension), this.TotalSubP2Price6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount6, attNameExtension), this.TotalSubP2Amount6);
            }

            if (this.TotalSubP2Name7 != string.Empty && this.TotalSubP2Name7 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name7, attNameExtension), this.TotalSubP2Name7);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label7, attNameExtension), this.TotalSubP2Label7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total7, attNameExtension), this.TotalSubP2Total7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit7, attNameExtension), this.TotalSubP2TotalPerUnit7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price7, attNameExtension), this.TotalSubP2Price7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount7, attNameExtension), this.TotalSubP2Amount7);
            }

            if (this.TotalSubP2Name8 != string.Empty && this.TotalSubP2Name8 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name8, attNameExtension), this.TotalSubP2Name8);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label8, attNameExtension), this.TotalSubP2Label8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total8, attNameExtension), this.TotalSubP2Total8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit8, attNameExtension), this.TotalSubP2TotalPerUnit8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price8, attNameExtension), this.TotalSubP2Price8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount8, attNameExtension), this.TotalSubP2Amount8);
            }

            if (this.TotalSubP2Name9 != string.Empty && this.TotalSubP2Name9 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name9, attNameExtension), this.TotalSubP2Name9);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label9, attNameExtension), this.TotalSubP2Label9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total9, attNameExtension), this.TotalSubP2Total9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit9, attNameExtension), this.TotalSubP2TotalPerUnit9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price9, attNameExtension), this.TotalSubP2Price9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount9, attNameExtension), this.TotalSubP2Amount9);
            }

            if (this.TotalSubP2Name10 != string.Empty && this.TotalSubP2Name10 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Name10, attNameExtension), this.TotalSubP2Name10);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP2Label10, attNameExtension), this.TotalSubP2Label10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Total10, attNameExtension), this.TotalSubP2Total10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2TotalPerUnit10, attNameExtension), this.TotalSubP2TotalPerUnit10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Price10, attNameExtension), this.TotalSubP2Price10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP2Amount10, attNameExtension), this.TotalSubP2Amount10);
            }
        }
        public virtual void SetTotalSubPriceStock2Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (this.TotalSubP2Name1 != string.Empty && this.TotalSubP2Name1 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name1, attNameExtension), this.TotalSubP2Name1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label1, attNameExtension), this.TotalSubP2Label1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total1, attNameExtension), this.TotalSubP2Total1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit1, attNameExtension), this.TotalSubP2TotalPerUnit1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Price1, attNameExtension), this.TotalSubP2Price1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount1, attNameExtension), this.TotalSubP2Amount1.ToString());
            }
            if (this.TotalSubP2Name2 != string.Empty && this.TotalSubP2Name2 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name2, attNameExtension), this.TotalSubP2Name2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label2, attNameExtension), this.TotalSubP2Label2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total2, attNameExtension), this.TotalSubP2Total2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit2, attNameExtension), this.TotalSubP2TotalPerUnit2.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price2, attNameExtension), this.TotalSubP2Price2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount2, attNameExtension), this.TotalSubP2Amount2.ToString());
            }
            if (this.TotalSubP2Name3 != string.Empty && this.TotalSubP2Name3 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name3, attNameExtension), this.TotalSubP2Name3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label3, attNameExtension), this.TotalSubP2Label3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total3, attNameExtension), this.TotalSubP2Total3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit3, attNameExtension), this.TotalSubP2TotalPerUnit3.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price3, attNameExtension), this.TotalSubP2Price3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount3, attNameExtension), this.TotalSubP2Amount3.ToString());
            }
            if (this.TotalSubP2Name4 != string.Empty && this.TotalSubP2Name4 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name4, attNameExtension), this.TotalSubP2Name4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label4, attNameExtension), this.TotalSubP2Label4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total4, attNameExtension), this.TotalSubP2Total4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit4, attNameExtension), this.TotalSubP2TotalPerUnit4.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price4, attNameExtension), this.TotalSubP2Price4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount4, attNameExtension), this.TotalSubP2Amount4.ToString());
            }
            if (this.TotalSubP2Name5 != string.Empty && this.TotalSubP2Name5 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name5, attNameExtension), this.TotalSubP2Name5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label5, attNameExtension), this.TotalSubP2Label5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total5, attNameExtension), this.TotalSubP2Total5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit5, attNameExtension), this.TotalSubP2TotalPerUnit5.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price5, attNameExtension), this.TotalSubP2Price5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount5, attNameExtension), this.TotalSubP2Amount5.ToString());
            }
            if (this.TotalSubP2Name6 != string.Empty && this.TotalSubP2Name6 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name6, attNameExtension), this.TotalSubP2Name6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label6, attNameExtension), this.TotalSubP2Label6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total6, attNameExtension), this.TotalSubP2Total6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit6, attNameExtension), this.TotalSubP2TotalPerUnit6.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price6, attNameExtension), this.TotalSubP2Price6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount6, attNameExtension), this.TotalSubP2Amount6.ToString());
            }
            if (this.TotalSubP2Name7 != string.Empty && this.TotalSubP2Name7 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name7, attNameExtension), this.TotalSubP2Name7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label7, attNameExtension), this.TotalSubP2Label7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total7, attNameExtension), this.TotalSubP2Total7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit7, attNameExtension), this.TotalSubP2TotalPerUnit7.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price7, attNameExtension), this.TotalSubP2Price7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount7, attNameExtension), this.TotalSubP2Amount7.ToString());
            }
            if (this.TotalSubP2Name8 != string.Empty && this.TotalSubP2Name8 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name8, attNameExtension), this.TotalSubP2Name8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label8, attNameExtension), this.TotalSubP2Label8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total8, attNameExtension), this.TotalSubP2Total8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit8, attNameExtension), this.TotalSubP2TotalPerUnit8.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price8, attNameExtension), this.TotalSubP2Price8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount8, attNameExtension), this.TotalSubP2Amount8.ToString());
            }
            if (this.TotalSubP2Name9 != string.Empty && this.TotalSubP2Name9 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name9, attNameExtension), this.TotalSubP2Name9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label9, attNameExtension), this.TotalSubP2Label9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total9, attNameExtension), this.TotalSubP2Total9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit9, attNameExtension), this.TotalSubP2TotalPerUnit9.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price9, attNameExtension), this.TotalSubP2Price9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount9, attNameExtension), this.TotalSubP2Amount9.ToString());
            }
            if (this.TotalSubP2Name10 != string.Empty && this.TotalSubP2Name10 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Name10, attNameExtension), this.TotalSubP2Name10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Label10, attNameExtension), this.TotalSubP2Label10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2Total10, attNameExtension), this.TotalSubP2Total10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP2TotalPerUnit10, attNameExtension), this.TotalSubP2TotalPerUnit10.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Price10, attNameExtension), this.TotalSubP2Price10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount10, attNameExtension), this.TotalSubP2Amount10.ToString());
            }
        }
    }
    public static class SubPrice2Extensions
    {
        //add a base health input stock to the baseStat.BuildCost1s dictionary
        public static bool AddSubPriceStock2sToDictionary(this SubPriceStock2 baseStat,
            int filePosition, int nodePosition, SubPrices subPrice)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.SubPrice1s == null)
                baseStat.SubPrice1s
                = new Dictionary<int, List<SubPrices>>();
            if (baseStat.SubPrice1s.ContainsKey(filePosition))
            {
                if (baseStat.SubPrice1s[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.SubPrice1s[filePosition].Count <= i)
                        {
                            baseStat.SubPrice1s[filePosition]
                                .Add(new SubPrices());
                        }
                    }
                    baseStat.SubPrice1s[filePosition][nodePosition]
                        = subPrice;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<SubPrices> baseStats
                    = new List<SubPrices>();
                KeyValuePair<int, List<SubPrices>> newStat
                    = new KeyValuePair<int, List<SubPrices>>(
                        filePosition, baseStats);
                baseStat.SubPrice1s.Add(newStat);
                bIsAdded = AddSubPriceStock2sToDictionary(baseStat,
                    filePosition, nodePosition, subPrice);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this SubPriceStock2 baseStat,
            int filePosition, SubPrices subPrice)
        {
            int iNodeCount = 0;
            if (baseStat.SubPrice1s == null)
                return iNodeCount;
            if (baseStat.SubPrice1s.ContainsKey(filePosition))
            {
                if (baseStat.SubPrice1s[filePosition] != null)
                {
                    iNodeCount = baseStat.SubPrice1s[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}