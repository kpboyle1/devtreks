using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The SubPriceStock1 class indirectly extends the SubPrices() class
    ///Author:		www.devtreks.org
    ///Date:		2013, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. Stock1 is for Costs and Stock2 is for Benefits, aggregation at time
    ///             period and above has to distinguish the two
    public class SubPriceStock1
    {
        //calls the base-class version, and initializes the base class properties.
        public SubPriceStock1()
            : base()
        {
            //subprice object
            InitTotalSubPriceStock1Properties();
        }
        //copy constructor
        public SubPriceStock1(SubPriceStock1 calculator)
        {
            CopyTotalSubPriceStock1Properties(calculator);
        }

        //calculator properties
        //subcosts collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<SubPrices>> SubPrice1s = null;
        public string ErrorMessage { get; set; }
        //power
        public string TotalSubP1Name1 { get; set; }
        //aggregator
        public string TotalSubP1Label1 { get; set; }
        //total discounted cost
        public double TotalSubP1Total1 { get; set; }
        //total discounted cost per unit
        public double TotalSubP1TotalPerUnit1 { get; set; }
        //total price
        public double TotalSubP1Price1 { get; set; }
        //total quantity
        public double TotalSubP1Amount1 { get; set; }


        public string TotalSubP1Name2 { get; set; }
        public string TotalSubP1Label2 { get; set; }
        public double TotalSubP1Total2 { get; set; }
        public double TotalSubP1TotalPerUnit2 { get; set; }
        public double TotalSubP1Price2 { get; set; }
        public double TotalSubP1Amount2 { get; set; }

        public string TotalSubP1Name3 { get; set; }
        public string TotalSubP1Label3 { get; set; }
        public double TotalSubP1Total3 { get; set; }
        public double TotalSubP1TotalPerUnit3 { get; set; }
        public double TotalSubP1Price3 { get; set; }
        public double TotalSubP1Amount3 { get; set; }

        public string TotalSubP1Name4 { get; set; }
        public string TotalSubP1Label4 { get; set; }
        public double TotalSubP1Total4 { get; set; }
        public double TotalSubP1TotalPerUnit4 { get; set; }
        public double TotalSubP1Price4 { get; set; }
        public double TotalSubP1Amount4 { get; set; }

        public string TotalSubP1Name5 { get; set; }
        public string TotalSubP1Label5 { get; set; }
        public double TotalSubP1Total5 { get; set; }
        public double TotalSubP1TotalPerUnit5 { get; set; }
        public double TotalSubP1Price5 { get; set; }
        public double TotalSubP1Amount5 { get; set; }

        public string TotalSubP1Name6 { get; set; }
        public string TotalSubP1Label6 { get; set; }
        public double TotalSubP1Total6 { get; set; }
        public double TotalSubP1TotalPerUnit6 { get; set; }
        public double TotalSubP1Price6 { get; set; }
        public double TotalSubP1Amount6 { get; set; }

        public string TotalSubP1Name7 { get; set; }
        public string TotalSubP1Label7 { get; set; }
        public double TotalSubP1Total7 { get; set; }
        public double TotalSubP1TotalPerUnit7 { get; set; }
        public double TotalSubP1Price7 { get; set; }
        public double TotalSubP1Amount7 { get; set; }

        public string TotalSubP1Name8 { get; set; }
        public string TotalSubP1Label8 { get; set; }
        public double TotalSubP1Total8 { get; set; }
        public double TotalSubP1TotalPerUnit8 { get; set; }
        public double TotalSubP1Price8 { get; set; }
        public double TotalSubP1Amount8 { get; set; }

        public string TotalSubP1Name9 { get; set; }
        public string TotalSubP1Label9 { get; set; }
        public double TotalSubP1Total9 { get; set; }
        public double TotalSubP1TotalPerUnit9 { get; set; }
        public double TotalSubP1Price9 { get; set; }
        public double TotalSubP1Amount9 { get; set; }

        public string TotalSubP1Name10 { get; set; }
        public string TotalSubP1Label10 { get; set; }
        public double TotalSubP1Total10 { get; set; }
        public double TotalSubP1TotalPerUnit10 { get; set; }
        public double TotalSubP1Price10 { get; set; }
        public double TotalSubP1Amount10 { get; set; }

        private const string cTotalSubP1Name1 = "TSubP1Name1";
        private const string cTotalSubP1Label1 = "TSubP1Label1";
        private const string cTotalSubP1Total1 = "TSubP1Total1";
        private const string cTotalSubP1TotalPerUnit1 = "TSubP1TotalPerUnit1";
        private const string cTotalSubP1Price1 = "TSubP1Price1";
        private const string cTotalSubP1Amount1 = "TSubP1Amount1";

        private const string cTotalSubP1Name2 = "TSubP1Name2";
        private const string cTotalSubP1Label2 = "TSubP1Label2";
        private const string cTotalSubP1Total2 = "TSubP1Total2";
        private const string cTotalSubP1TotalPerUnit2 = "TSubP1TotalPerUnit2";
        private const string cTotalSubP1Price2 = "TSubP1Price2";
        private const string cTotalSubP1Amount2 = "TSubP1Amount2";

        private const string cTotalSubP1Name3 = "TSubP1Name3";
        private const string cTotalSubP1Label3 = "TSubP1Label3";
        private const string cTotalSubP1Total3 = "TSubP1Total3";
        private const string cTotalSubP1TotalPerUnit3 = "TSubP1TotalPerUnit3";
        private const string cTotalSubP1Price3 = "TSubP1Price3";
        private const string cTotalSubP1Amount3 = "TSubP1Amount3";

        private const string cTotalSubP1Name4 = "TSubP1Name4";
        private const string cTotalSubP1Label4 = "TSubP1Label4";
        private const string cTotalSubP1Total4 = "TSubP1Total4";
        private const string cTotalSubP1TotalPerUnit4 = "TSubP1TotalPerUnit4";
        private const string cTotalSubP1Price4 = "TSubP1Price4";
        private const string cTotalSubP1Amount4 = "TSubP1Amount4";

        private const string cTotalSubP1Name5 = "TSubP1Name5";
        private const string cTotalSubP1Label5 = "TSubP1Label5";
        private const string cTotalSubP1Total5 = "TSubP1Total5";
        private const string cTotalSubP1TotalPerUnit5 = "TSubP1TotalPerUnit5";
        private const string cTotalSubP1Price5 = "TSubP1Price5";
        private const string cTotalSubP1Amount5 = "TSubP1Amount5";

        private const string cTotalSubP1Name6 = "TSubP1Name6";
        private const string cTotalSubP1Label6 = "TSubP1Label6";
        private const string cTotalSubP1Total6 = "TSubP1Total6";
        private const string cTotalSubP1TotalPerUnit6 = "TSubP1TotalPerUnit6";
        private const string cTotalSubP1Price6 = "TSubP1Price6";
        private const string cTotalSubP1Amount6 = "TSubP1Amount6";

        private const string cTotalSubP1Name7 = "TSubP1Name7";
        private const string cTotalSubP1Label7 = "TSubP1Label7";
        private const string cTotalSubP1Total7 = "TSubP1Total7";
        private const string cTotalSubP1TotalPerUnit7 = "TSubP1TotalPerUnit7";
        private const string cTotalSubP1Price7 = "TSubP1Price7";
        private const string cTotalSubP1Amount7 = "TSubP1Amount7";

        private const string cTotalSubP1Name8 = "TSubP1Name8";
        private const string cTotalSubP1Label8 = "TSubP1Label8";
        private const string cTotalSubP1Total8 = "TSubP1Total8";
        private const string cTotalSubP1TotalPerUnit8 = "TSubP1TotalPerUnit8";
        private const string cTotalSubP1Price8 = "TSubP1Price8";
        private const string cTotalSubP1Amount8 = "TSubP1Amount8";

        private const string cTotalSubP1Name9 = "TSubP1Name9";
        private const string cTotalSubP1Label9 = "TSubP1Label9";
        private const string cTotalSubP1Total9 = "TSubP1Total9";
        private const string cTotalSubP1TotalPerUnit9 = "TSubP1TotalPerUnit9";
        private const string cTotalSubP1Price9 = "TSubP1Price9";
        private const string cTotalSubP1Amount9 = "TSubP1Amount9";

        private const string cTotalSubP1Name10 = "TSubP1Name10";
        private const string cTotalSubP1Label10 = "TSubP1Label10";
        private const string cTotalSubP1Total10 = "TSubP1Total10";
        private const string cTotalSubP1TotalPerUnit10 = "TSubP1TotalPerUnit10";
        private const string cTotalSubP1Price10 = "TSubP1Price10";
        private const string cTotalSubP1Amount10 = "TSubP1Amount10";

        public virtual void InitTotalSubPriceStock1Properties()
        {
            this.ErrorMessage = string.Empty;
            this.TotalSubP1Name1 = string.Empty;
            this.TotalSubP1Label1 = string.Empty;
            this.TotalSubP1Total1 = 0;
            this.TotalSubP1TotalPerUnit1 = 0;
            this.TotalSubP1Price1 = 0;
            this.TotalSubP1Amount1 = 0;

            this.TotalSubP1Name2 = string.Empty;
            this.TotalSubP1Label2 = string.Empty;
            this.TotalSubP1Total2 = 0;
            this.TotalSubP1TotalPerUnit2 = 0;
            this.TotalSubP1Price2 = 0;
            this.TotalSubP1Amount2 = 0;

            this.TotalSubP1Name3 = string.Empty;
            this.TotalSubP1Label3 = string.Empty;
            this.TotalSubP1Total3 = 0;
            this.TotalSubP1TotalPerUnit3 = 0;
            this.TotalSubP1Price3 = 0;
            this.TotalSubP1Amount3 = 0;

            this.TotalSubP1Name4 = string.Empty;
            this.TotalSubP1Label4 = string.Empty;
            this.TotalSubP1Total4 = 0;
            this.TotalSubP1TotalPerUnit4 = 0;
            this.TotalSubP1Price4 = 0;
            this.TotalSubP1Amount4 = 0;

            this.TotalSubP1Name5 = string.Empty;
            this.TotalSubP1Label5 = string.Empty;
            this.TotalSubP1Total5 = 0;
            this.TotalSubP1TotalPerUnit5 = 0;
            this.TotalSubP1Price5 = 0;
            this.TotalSubP1Amount5 = 0;

            this.TotalSubP1Name6 = string.Empty;
            this.TotalSubP1Label6 = string.Empty;
            this.TotalSubP1Total6 = 0;
            this.TotalSubP1TotalPerUnit6 = 0;
            this.TotalSubP1Price6 = 0;
            this.TotalSubP1Amount6 = 0;

            this.TotalSubP1Name7 = string.Empty;
            this.TotalSubP1Label7 = string.Empty;
            this.TotalSubP1Total7 = 0;
            this.TotalSubP1TotalPerUnit7 = 0;
            this.TotalSubP1Price7 = 0;
            this.TotalSubP1Amount7 = 0;

            this.TotalSubP1Name8 = string.Empty;
            this.TotalSubP1Label8 = string.Empty;
            this.TotalSubP1Total8 = 0;
            this.TotalSubP1TotalPerUnit8 = 0;
            this.TotalSubP1Price8 = 0;
            this.TotalSubP1Amount8 = 0;

            this.TotalSubP1Name9 = string.Empty;
            this.TotalSubP1Label9 = string.Empty;
            this.TotalSubP1Total9 = 0;
            this.TotalSubP1TotalPerUnit9 = 0;
            this.TotalSubP1Price9 = 0;
            this.TotalSubP1Amount9 = 0;

            this.TotalSubP1Name10 = string.Empty;
            this.TotalSubP1Label10 = string.Empty;
            this.TotalSubP1Total10 = 0;
            this.TotalSubP1TotalPerUnit10 = 0;
            this.TotalSubP1Price10 = 0;
            this.TotalSubP1Amount10 = 0;
        }
        public virtual void CopyTotalSubPriceStock1Properties(
            SubPriceStock1 calculator)
        {
            this.ErrorMessage = calculator.ErrorMessage;
            this.TotalSubP1Name1 = calculator.TotalSubP1Name1;
            this.TotalSubP1Label1 = calculator.TotalSubP1Label1;
            this.TotalSubP1Total1 = calculator.TotalSubP1Total1;
            this.TotalSubP1TotalPerUnit1 = calculator.TotalSubP1TotalPerUnit1;
            this.TotalSubP1Price1 = calculator.TotalSubP1Price1;
            this.TotalSubP1Amount1 = calculator.TotalSubP1Amount1;
            this.TotalSubP1Name2 = calculator.TotalSubP1Name2;
            this.TotalSubP1Label2 = calculator.TotalSubP1Label2;
            this.TotalSubP1Total2 = calculator.TotalSubP1Total2;
            this.TotalSubP1TotalPerUnit2 = calculator.TotalSubP1TotalPerUnit2;
            this.TotalSubP1Price2 = calculator.TotalSubP1Price2;
            this.TotalSubP1Amount2 = calculator.TotalSubP1Amount2;
            this.TotalSubP1Name3 = calculator.TotalSubP1Name3;
            this.TotalSubP1Label3 = calculator.TotalSubP1Label3;
            this.TotalSubP1Total3 = calculator.TotalSubP1Total3;
            this.TotalSubP1TotalPerUnit3 = calculator.TotalSubP1TotalPerUnit3;
            this.TotalSubP1Price3 = calculator.TotalSubP1Price3;
            this.TotalSubP1Amount3 = calculator.TotalSubP1Amount3;
            this.TotalSubP1Name4 = calculator.TotalSubP1Name4;
            this.TotalSubP1Label4 = calculator.TotalSubP1Label4;
            this.TotalSubP1Total4 = calculator.TotalSubP1Total4;
            this.TotalSubP1TotalPerUnit4 = calculator.TotalSubP1TotalPerUnit4;
            this.TotalSubP1Price4 = calculator.TotalSubP1Price4;
            this.TotalSubP1Amount4 = calculator.TotalSubP1Amount4;
            this.TotalSubP1Name5 = calculator.TotalSubP1Name5;
            this.TotalSubP1Label5 = calculator.TotalSubP1Label5;
            this.TotalSubP1Total5 = calculator.TotalSubP1Total5;
            this.TotalSubP1TotalPerUnit5 = calculator.TotalSubP1TotalPerUnit5;
            this.TotalSubP1Price5 = calculator.TotalSubP1Price5;
            this.TotalSubP1Amount5 = calculator.TotalSubP1Amount5;

            this.TotalSubP1Name6 = calculator.TotalSubP1Name6;
            this.TotalSubP1Label6 = calculator.TotalSubP1Label6;
            this.TotalSubP1Total6 = calculator.TotalSubP1Total6;
            this.TotalSubP1TotalPerUnit6 = calculator.TotalSubP1TotalPerUnit6;
            this.TotalSubP1Price6 = calculator.TotalSubP1Price6;
            this.TotalSubP1Amount6 = calculator.TotalSubP1Amount6;

            this.TotalSubP1Name7 = calculator.TotalSubP1Name7;
            this.TotalSubP1Label7 = calculator.TotalSubP1Label7;
            this.TotalSubP1Total7 = calculator.TotalSubP1Total7;
            this.TotalSubP1TotalPerUnit7 = calculator.TotalSubP1TotalPerUnit7;
            this.TotalSubP1Price7 = calculator.TotalSubP1Price7;
            this.TotalSubP1Amount7 = calculator.TotalSubP1Amount7;

            this.TotalSubP1Name8 = calculator.TotalSubP1Name8;
            this.TotalSubP1Label8 = calculator.TotalSubP1Label8;
            this.TotalSubP1Total8 = calculator.TotalSubP1Total8;
            this.TotalSubP1TotalPerUnit8 = calculator.TotalSubP1TotalPerUnit8;
            this.TotalSubP1Price8 = calculator.TotalSubP1Price8;
            this.TotalSubP1Amount8 = calculator.TotalSubP1Amount8;

            this.TotalSubP1Name9 = calculator.TotalSubP1Name9;
            this.TotalSubP1Label9 = calculator.TotalSubP1Label9;
            this.TotalSubP1Total9 = calculator.TotalSubP1Total9;
            this.TotalSubP1TotalPerUnit9 = calculator.TotalSubP1TotalPerUnit9;
            this.TotalSubP1Price9 = calculator.TotalSubP1Price9;
            this.TotalSubP1Amount9 = calculator.TotalSubP1Amount9;

            this.TotalSubP1Name10 = calculator.TotalSubP1Name10;
            this.TotalSubP1Label10 = calculator.TotalSubP1Label10;
            this.TotalSubP1Total10 = calculator.TotalSubP1Total10;
            this.TotalSubP1TotalPerUnit10 = calculator.TotalSubP1TotalPerUnit10;
            this.TotalSubP1Price10 = calculator.TotalSubP1Price10;
            this.TotalSubP1Amount10 = calculator.TotalSubP1Amount10;
        }
        public virtual void SetTotalSubPriceStock1Properties(XElement calculator)
        {
            //set this object's properties
            this.TotalSubP1Name1 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name1);
            this.TotalSubP1Label1 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label1);
            this.TotalSubP1Total1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total1);
            this.TotalSubP1TotalPerUnit1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit1);
            this.TotalSubP1Price1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Price1);
            this.TotalSubP1Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount1);
            this.TotalSubP1Name2 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name2);
            this.TotalSubP1Label2 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label2);
            this.TotalSubP1Total2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total2);
            this.TotalSubP1TotalPerUnit2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit2);
            this.TotalSubP1Price2 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price2);
            this.TotalSubP1Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount2);
            this.TotalSubP1Name3 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name3);
            this.TotalSubP1Label3 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label3);
            this.TotalSubP1Total3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total3);
            this.TotalSubP1TotalPerUnit3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit3);
            this.TotalSubP1Price3 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price3);
            this.TotalSubP1Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount3);
            this.TotalSubP1Name4 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name4);
            this.TotalSubP1Label4 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label4);
            this.TotalSubP1Total4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total4);
            this.TotalSubP1TotalPerUnit4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit4);
            this.TotalSubP1Price4 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price4);
            this.TotalSubP1Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount4);
            this.TotalSubP1Name5 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name5);
            this.TotalSubP1Label5 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label5);
            this.TotalSubP1Total5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total5);
            this.TotalSubP1TotalPerUnit5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit5);
            this.TotalSubP1Price5 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price5);
            this.TotalSubP1Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount5);

            this.TotalSubP1Name6 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name6);
            this.TotalSubP1Label6 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label6);
            this.TotalSubP1Total6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total6);
            this.TotalSubP1TotalPerUnit6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit6);
            this.TotalSubP1Price6 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price6);
            this.TotalSubP1Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount6);

            this.TotalSubP1Name7 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name7);
            this.TotalSubP1Label7 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label7);
            this.TotalSubP1Total7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total7);
            this.TotalSubP1TotalPerUnit7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit7);
            this.TotalSubP1Price7 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price7);
            this.TotalSubP1Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount7);

            this.TotalSubP1Name8 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name8);
            this.TotalSubP1Label8 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label8);
            this.TotalSubP1Total8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total8);
            this.TotalSubP1TotalPerUnit8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit8);
            this.TotalSubP1Price8 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price8);
            this.TotalSubP1Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount8);

            this.TotalSubP1Name9 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name9);
            this.TotalSubP1Label9 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label9);
            this.TotalSubP1Total9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total9);
            this.TotalSubP1TotalPerUnit9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit9);
            this.TotalSubP1Price9 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price9);
            this.TotalSubP1Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount9);

            this.TotalSubP1Name10 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Name10);
            this.TotalSubP1Label10 = CalculatorHelpers.GetAttribute(calculator,
               cTotalSubP1Label10);
            this.TotalSubP1Total10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Total10);
            this.TotalSubP1TotalPerUnit10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1TotalPerUnit10);
            this.TotalSubP1Price10 = CalculatorHelpers.GetAttributeDouble(calculator,
              cTotalSubP1Price10);
            this.TotalSubP1Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cTotalSubP1Amount10);
        }
        public virtual void SetTotalSubPriceStock1Property(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cTotalSubP1Name1:
                    this.TotalSubP1Name1 = attValue;
                    break;
                case cTotalSubP1Label1:
                    this.TotalSubP1Label1 = attValue;
                    break;
                case cTotalSubP1Total1:
                    this.TotalSubP1Total1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit1:
                    this.TotalSubP1TotalPerUnit1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price1:
                    this.TotalSubP1Price1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount1:
                    this.TotalSubP1Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Name2:
                    this.TotalSubP1Name2 = attValue;
                    break;
                case cTotalSubP1Label2:
                    this.TotalSubP1Label2 = attValue;
                    break;
                case cTotalSubP1Total2:
                    this.TotalSubP1Total2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit2:
                    this.TotalSubP1TotalPerUnit2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price2:
                    this.TotalSubP1Price2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount2:
                    this.TotalSubP1Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Name3:
                    this.TotalSubP1Name3 = attValue;
                    break;
                case cTotalSubP1Label3:
                    this.TotalSubP1Label3 = attValue;
                    break;
                case cTotalSubP1Total3:
                    this.TotalSubP1Total3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit3:
                    this.TotalSubP1TotalPerUnit3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price3:
                    this.TotalSubP1Price3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount3:
                    this.TotalSubP1Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Name4:
                    this.TotalSubP1Name4 = attValue;
                    break;
                case cTotalSubP1Label4:
                    this.TotalSubP1Label4 = attValue;
                    break;
                case cTotalSubP1Total4:
                    this.TotalSubP1Total4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit4:
                    this.TotalSubP1TotalPerUnit4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price4:
                    this.TotalSubP1Price4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount4:
                    this.TotalSubP1Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Name5:
                    this.TotalSubP1Name5 = attValue;
                    break;
                case cTotalSubP1Label5:
                    this.TotalSubP1Label5 = attValue;
                    break;
                case cTotalSubP1Total5:
                    this.TotalSubP1Total5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit5:
                    this.TotalSubP1TotalPerUnit5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price5:
                    this.TotalSubP1Price5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount5:
                    this.TotalSubP1Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP1Name6:
                    this.TotalSubP1Name6 = attValue;
                    break;
                case cTotalSubP1Label6:
                    this.TotalSubP1Label6 = attValue;
                    break;
                case cTotalSubP1Total6:
                    this.TotalSubP1Total6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit6:
                    this.TotalSubP1TotalPerUnit6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price6:
                    this.TotalSubP1Price6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount6:
                    this.TotalSubP1Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP1Name7:
                    this.TotalSubP1Name7 = attValue;
                    break;
                case cTotalSubP1Label7:
                    this.TotalSubP1Label7 = attValue;
                    break;
                case cTotalSubP1Total7:
                    this.TotalSubP1Total7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit7:
                    this.TotalSubP1TotalPerUnit7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price7:
                    this.TotalSubP1Price7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount7:
                    this.TotalSubP1Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP1Name8:
                    this.TotalSubP1Name8 = attValue;
                    break;
                case cTotalSubP1Label8:
                    this.TotalSubP1Label8 = attValue;
                    break;
                case cTotalSubP1Total8:
                    this.TotalSubP1Total8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit8:
                    this.TotalSubP1TotalPerUnit8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price8:
                    this.TotalSubP1Price8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount8:
                    this.TotalSubP1Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP1Name9:
                    this.TotalSubP1Name9 = attValue;
                    break;
                case cTotalSubP1Label9:
                    this.TotalSubP1Label9 = attValue;
                    break;
                case cTotalSubP1Total9:
                    this.TotalSubP1Total9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit9:
                    this.TotalSubP1TotalPerUnit9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price9:
                    this.TotalSubP1Price9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount9:
                    this.TotalSubP1Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case cTotalSubP1Name10:
                    this.TotalSubP1Name10 = attValue;
                    break;
                case cTotalSubP1Label10:
                    this.TotalSubP1Label10 = attValue;
                    break;
                case cTotalSubP1Total10:
                    this.TotalSubP1Total10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit10:
                    this.TotalSubP1TotalPerUnit10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price10:
                    this.TotalSubP1Price10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Amount10:
                    this.TotalSubP1Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalSubPriceStock1Property(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSubP1Name1:
                    sPropertyValue = this.TotalSubP1Name1.ToString();
                    break;
                case cTotalSubP1Label1:
                    sPropertyValue = this.TotalSubP1Label1.ToString();
                    break;
                case cTotalSubP1Total1:
                    sPropertyValue = this.TotalSubP1Total1.ToString();
                    break;
                case cTotalSubP1TotalPerUnit1:
                    sPropertyValue = this.TotalSubP1TotalPerUnit1.ToString();
                    break;
                case cTotalSubP1Price1:
                    sPropertyValue = this.TotalSubP1Price1.ToString();
                    break;
                case cTotalSubP1Amount1:
                    sPropertyValue = this.TotalSubP1Amount1.ToString();
                    break;
                case cTotalSubP1Name2:
                    sPropertyValue = this.TotalSubP1Name2.ToString();
                    break;
                case cTotalSubP1Label2:
                    sPropertyValue = this.TotalSubP1Label2.ToString();
                    break;
                case cTotalSubP1Total2:
                    sPropertyValue = this.TotalSubP1Total2.ToString();
                    break;
                case cTotalSubP1TotalPerUnit2:
                    sPropertyValue = this.TotalSubP1TotalPerUnit2.ToString();
                    break;
                case cTotalSubP1Price2:
                    sPropertyValue = this.TotalSubP1Price2.ToString();
                    break;
                case cTotalSubP1Amount2:
                    sPropertyValue = this.TotalSubP1Amount2.ToString();
                    break;
                case cTotalSubP1Name3:
                    sPropertyValue = this.TotalSubP1Name3.ToString();
                    break;
                case cTotalSubP1Label3:
                    sPropertyValue = this.TotalSubP1Label3.ToString();
                    break;
                case cTotalSubP1Total3:
                    sPropertyValue = this.TotalSubP1Total3.ToString();
                    break;
                case cTotalSubP1TotalPerUnit3:
                    sPropertyValue = this.TotalSubP1TotalPerUnit3.ToString();
                    break;
                case cTotalSubP1Price3:
                    sPropertyValue = this.TotalSubP1Price3.ToString();
                    break;
                case cTotalSubP1Amount3:
                    sPropertyValue = this.TotalSubP1Amount3.ToString();
                    break;
                case cTotalSubP1Name4:
                    sPropertyValue = this.TotalSubP1Name4.ToString();
                    break;
                case cTotalSubP1Label4:
                    sPropertyValue = this.TotalSubP1Label4.ToString();
                    break;
                case cTotalSubP1Total4:
                    sPropertyValue = this.TotalSubP1Total4.ToString();
                    break;
                case cTotalSubP1TotalPerUnit4:
                    sPropertyValue = this.TotalSubP1TotalPerUnit4.ToString();
                    break;
                case cTotalSubP1Price4:
                    sPropertyValue = this.TotalSubP1Price4.ToString();
                    break;
                case cTotalSubP1Amount4:
                    sPropertyValue = this.TotalSubP1Amount4.ToString();
                    break;
                case cTotalSubP1Name5:
                    sPropertyValue = this.TotalSubP1Name5.ToString();
                    break;
                case cTotalSubP1Label5:
                    sPropertyValue = this.TotalSubP1Label5.ToString();
                    break;
                case cTotalSubP1Total5:
                    sPropertyValue = this.TotalSubP1Total5.ToString();
                    break;
                case cTotalSubP1TotalPerUnit5:
                    sPropertyValue = this.TotalSubP1TotalPerUnit5.ToString();
                    break;
                case cTotalSubP1Price5:
                    sPropertyValue = this.TotalSubP1Price5.ToString();
                    break;
                case cTotalSubP1Amount5:
                    sPropertyValue = this.TotalSubP1Amount5.ToString();
                    break;

                case cTotalSubP1Name6:
                    sPropertyValue = this.TotalSubP1Name6.ToString();
                    break;
                case cTotalSubP1Label6:
                    sPropertyValue = this.TotalSubP1Label6.ToString();
                    break;
                case cTotalSubP1Total6:
                    sPropertyValue = this.TotalSubP1Total6.ToString();
                    break;
                case cTotalSubP1TotalPerUnit6:
                    sPropertyValue = this.TotalSubP1TotalPerUnit6.ToString();
                    break;
                case cTotalSubP1Price6:
                    sPropertyValue = this.TotalSubP1Price6.ToString();
                    break;
                case cTotalSubP1Amount6:
                    sPropertyValue = this.TotalSubP1Amount6.ToString();
                    break;

                case cTotalSubP1Name7:
                    sPropertyValue = this.TotalSubP1Name7.ToString();
                    break;
                case cTotalSubP1Label7:
                    sPropertyValue = this.TotalSubP1Label7.ToString();
                    break;
                case cTotalSubP1Total7:
                    sPropertyValue = this.TotalSubP1Total7.ToString();
                    break;
                case cTotalSubP1TotalPerUnit7:
                    sPropertyValue = this.TotalSubP1TotalPerUnit7.ToString();
                    break;
                case cTotalSubP1Price7:
                    sPropertyValue = this.TotalSubP1Price7.ToString();
                    break;
                case cTotalSubP1Amount7:
                    sPropertyValue = this.TotalSubP1Amount7.ToString();
                    break;

                case cTotalSubP1Name8:
                    sPropertyValue = this.TotalSubP1Name8.ToString();
                    break;
                case cTotalSubP1Label8:
                    sPropertyValue = this.TotalSubP1Label8.ToString();
                    break;
                case cTotalSubP1Total8:
                    sPropertyValue = this.TotalSubP1Total8.ToString();
                    break;
                case cTotalSubP1TotalPerUnit8:
                    sPropertyValue = this.TotalSubP1TotalPerUnit8.ToString();
                    break;
                case cTotalSubP1Price8:
                    sPropertyValue = this.TotalSubP1Price8.ToString();
                    break;
                case cTotalSubP1Amount8:
                    sPropertyValue = this.TotalSubP1Amount8.ToString();
                    break;

                case cTotalSubP1Name9:
                    sPropertyValue = this.TotalSubP1Name9.ToString();
                    break;
                case cTotalSubP1Label9:
                    sPropertyValue = this.TotalSubP1Label9.ToString();
                    break;
                case cTotalSubP1Total9:
                    sPropertyValue = this.TotalSubP1Total9.ToString();
                    break;
                case cTotalSubP1TotalPerUnit9:
                    sPropertyValue = this.TotalSubP1TotalPerUnit9.ToString();
                    break;
                case cTotalSubP1Price9:
                    sPropertyValue = this.TotalSubP1Price9.ToString();
                    break;
                case cTotalSubP1Amount9:
                    sPropertyValue = this.TotalSubP1Amount9.ToString();
                    break;

                case cTotalSubP1Name10:
                    sPropertyValue = this.TotalSubP1Name10.ToString();
                    break;
                case cTotalSubP1Label10:
                    sPropertyValue = this.TotalSubP1Label10.ToString();
                    break;
                case cTotalSubP1Total10:
                    sPropertyValue = this.TotalSubP1Total10.ToString();
                    break;
                case cTotalSubP1TotalPerUnit10:
                    sPropertyValue = this.TotalSubP1TotalPerUnit10.ToString();
                    break;
                case cTotalSubP1Price10:
                    sPropertyValue = this.TotalSubP1Price10.ToString();
                    break;
                case cTotalSubP1Amount10:
                    sPropertyValue = this.TotalSubP1Amount10.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalSubPriceStock1Attributes(string attNameExtension,
            ref XElement calculator)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (this.TotalSubP1Name1 != string.Empty && this.TotalSubP1Name1 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name1, attNameExtension), this.TotalSubP1Name1);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label1, attNameExtension), this.TotalSubP1Label1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total1, attNameExtension), this.TotalSubP1Total1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit1, attNameExtension), this.TotalSubP1TotalPerUnit1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price1, attNameExtension), this.TotalSubP1Price1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount1, attNameExtension), this.TotalSubP1Amount1);
            }
            if (this.TotalSubP1Name2 != string.Empty && this.TotalSubP1Name2 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name2, attNameExtension), this.TotalSubP1Name2);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label2, attNameExtension), this.TotalSubP1Label2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total2, attNameExtension), this.TotalSubP1Total2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit2, attNameExtension), this.TotalSubP1TotalPerUnit2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price2, attNameExtension), this.TotalSubP1Price2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount2, attNameExtension), this.TotalSubP1Amount2);
            }
            if (this.TotalSubP1Name3 != string.Empty && this.TotalSubP1Name3 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name3, attNameExtension), this.TotalSubP1Name3);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label3, attNameExtension), this.TotalSubP1Label3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total3, attNameExtension), this.TotalSubP1Total3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit3, attNameExtension), this.TotalSubP1TotalPerUnit3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price3, attNameExtension), this.TotalSubP1Price3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount3, attNameExtension), this.TotalSubP1Amount3);
            }
            if (this.TotalSubP1Name4 != string.Empty && this.TotalSubP1Name4 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name4, attNameExtension), this.TotalSubP1Name4);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label4, attNameExtension), this.TotalSubP1Label4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total4, attNameExtension), this.TotalSubP1Total4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit4, attNameExtension), this.TotalSubP1TotalPerUnit4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price4, attNameExtension), this.TotalSubP1Price4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount4, attNameExtension), this.TotalSubP1Amount4);
            }
            if (this.TotalSubP1Name5 != string.Empty && this.TotalSubP1Name5 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name5, attNameExtension), this.TotalSubP1Name5);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label5, attNameExtension), this.TotalSubP1Label5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total5, attNameExtension), this.TotalSubP1Total5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit5, attNameExtension), this.TotalSubP1TotalPerUnit5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price5, attNameExtension), this.TotalSubP1Price5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount5, attNameExtension), this.TotalSubP1Amount5);
            }

            if (this.TotalSubP1Name6 != string.Empty && this.TotalSubP1Name6 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name6, attNameExtension), this.TotalSubP1Name6);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label6, attNameExtension), this.TotalSubP1Label6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total6, attNameExtension), this.TotalSubP1Total6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit6, attNameExtension), this.TotalSubP1TotalPerUnit6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price6, attNameExtension), this.TotalSubP1Price6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount6, attNameExtension), this.TotalSubP1Amount6);
            }

            if (this.TotalSubP1Name7 != string.Empty && this.TotalSubP1Name7 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name7, attNameExtension), this.TotalSubP1Name7);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label7, attNameExtension), this.TotalSubP1Label7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total7, attNameExtension), this.TotalSubP1Total7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit7, attNameExtension), this.TotalSubP1TotalPerUnit7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price7, attNameExtension), this.TotalSubP1Price7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount7, attNameExtension), this.TotalSubP1Amount7);
            }

            if (this.TotalSubP1Name8 != string.Empty && this.TotalSubP1Name8 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name8, attNameExtension), this.TotalSubP1Name8);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label8, attNameExtension), this.TotalSubP1Label8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total8, attNameExtension), this.TotalSubP1Total8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit8, attNameExtension), this.TotalSubP1TotalPerUnit8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price8, attNameExtension), this.TotalSubP1Price8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount8, attNameExtension), this.TotalSubP1Amount8);
            }

            if (this.TotalSubP1Name9 != string.Empty && this.TotalSubP1Name9 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name9, attNameExtension), this.TotalSubP1Name9);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label9, attNameExtension), this.TotalSubP1Label9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total9, attNameExtension), this.TotalSubP1Total9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit9, attNameExtension), this.TotalSubP1TotalPerUnit9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price9, attNameExtension), this.TotalSubP1Price9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount9, attNameExtension), this.TotalSubP1Amount9);
            }

            if (this.TotalSubP1Name10 != string.Empty && this.TotalSubP1Name10 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Name10, attNameExtension), this.TotalSubP1Name10);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cTotalSubP1Label10, attNameExtension), this.TotalSubP1Label10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Total10, attNameExtension), this.TotalSubP1Total10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1TotalPerUnit10, attNameExtension), this.TotalSubP1TotalPerUnit10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Price10, attNameExtension), this.TotalSubP1Price10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalSubP1Amount10, attNameExtension), this.TotalSubP1Amount10);
            }
        }
        public virtual void SetTotalSubPriceStock1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (this.TotalSubP1Name1 != string.Empty && this.TotalSubP1Name1 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name1, attNameExtension), this.TotalSubP1Name1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label1, attNameExtension), this.TotalSubP1Label1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total1, attNameExtension), this.TotalSubP1Total1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit1, attNameExtension), this.TotalSubP1TotalPerUnit1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Price1, attNameExtension), this.TotalSubP1Price1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount1, attNameExtension), this.TotalSubP1Amount1.ToString());
            }
            if (this.TotalSubP1Name2 != string.Empty && this.TotalSubP1Name2 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name2, attNameExtension), this.TotalSubP1Name2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label2, attNameExtension), this.TotalSubP1Label2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total2, attNameExtension), this.TotalSubP1Total2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit2, attNameExtension), this.TotalSubP1TotalPerUnit2.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price2, attNameExtension), this.TotalSubP1Price2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount2, attNameExtension), this.TotalSubP1Amount2.ToString());
            }
            if (this.TotalSubP1Name3 != string.Empty && this.TotalSubP1Name3 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name3, attNameExtension), this.TotalSubP1Name3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label3, attNameExtension), this.TotalSubP1Label3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total3, attNameExtension), this.TotalSubP1Total3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit3, attNameExtension), this.TotalSubP1TotalPerUnit3.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price3, attNameExtension), this.TotalSubP1Price3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount3, attNameExtension), this.TotalSubP1Amount3.ToString());
            }
            if (this.TotalSubP1Name4 != string.Empty && this.TotalSubP1Name4 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name4, attNameExtension), this.TotalSubP1Name4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label4, attNameExtension), this.TotalSubP1Label4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total4, attNameExtension), this.TotalSubP1Total4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit4, attNameExtension), this.TotalSubP1TotalPerUnit4.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price4, attNameExtension), this.TotalSubP1Price4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount4, attNameExtension), this.TotalSubP1Amount4.ToString());
            }
            if (this.TotalSubP1Name5 != string.Empty && this.TotalSubP1Name5 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name5, attNameExtension), this.TotalSubP1Name5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label5, attNameExtension), this.TotalSubP1Label5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total5, attNameExtension), this.TotalSubP1Total5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit5, attNameExtension), this.TotalSubP1TotalPerUnit5.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price5, attNameExtension), this.TotalSubP1Price5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount5, attNameExtension), this.TotalSubP1Amount5.ToString());
            }
            if (this.TotalSubP1Name6 != string.Empty && this.TotalSubP1Name6 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name6, attNameExtension), this.TotalSubP1Name6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label6, attNameExtension), this.TotalSubP1Label6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total6, attNameExtension), this.TotalSubP1Total6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit6, attNameExtension), this.TotalSubP1TotalPerUnit6.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price6, attNameExtension), this.TotalSubP1Price6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount6, attNameExtension), this.TotalSubP1Amount6.ToString());
            }
            if (this.TotalSubP1Name7 != string.Empty && this.TotalSubP1Name7 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name7, attNameExtension), this.TotalSubP1Name7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label7, attNameExtension), this.TotalSubP1Label7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total7, attNameExtension), this.TotalSubP1Total7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit7, attNameExtension), this.TotalSubP1TotalPerUnit7.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price7, attNameExtension), this.TotalSubP1Price7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount7, attNameExtension), this.TotalSubP1Amount7.ToString());
            }
            if (this.TotalSubP1Name8 != string.Empty && this.TotalSubP1Name8 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name8, attNameExtension), this.TotalSubP1Name8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label8, attNameExtension), this.TotalSubP1Label8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total8, attNameExtension), this.TotalSubP1Total8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit8, attNameExtension), this.TotalSubP1TotalPerUnit8.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price8, attNameExtension), this.TotalSubP1Price8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount8, attNameExtension), this.TotalSubP1Amount8.ToString());
            }
            if (this.TotalSubP1Name9 != string.Empty && this.TotalSubP1Name9 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name9, attNameExtension), this.TotalSubP1Name9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label9, attNameExtension), this.TotalSubP1Label9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total9, attNameExtension), this.TotalSubP1Total9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit9, attNameExtension), this.TotalSubP1TotalPerUnit9.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price9, attNameExtension), this.TotalSubP1Price9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount9, attNameExtension), this.TotalSubP1Amount9.ToString());
            }
            if (this.TotalSubP1Name10 != string.Empty && this.TotalSubP1Name10 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Name10, attNameExtension), this.TotalSubP1Name10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Label10, attNameExtension), this.TotalSubP1Label10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1Total10, attNameExtension), this.TotalSubP1Total10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cTotalSubP1TotalPerUnit10, attNameExtension), this.TotalSubP1TotalPerUnit10.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Price10, attNameExtension), this.TotalSubP1Price10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount10, attNameExtension), this.TotalSubP1Amount10.ToString());
            }
        }
    }
    public static class SubPrice1Extensions
    {
        //add a base health input stock to the baseStat.BuildCost1s dictionary
        public static bool AddSubPriceStock1sToDictionary(this SubPriceStock1 baseStat,
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
                bIsAdded = AddSubPriceStock1sToDictionary(baseStat,
                    filePosition, nodePosition, subPrice);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this SubPriceStock1 baseStat,
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