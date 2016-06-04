﻿using System.Collections.Generic;
using System.Linq;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Use a generic SB1Base indicator object to hold the results of algorithms. 
    ///             The algorithm results are passed back to SB1Base-based objects, 
    ///             who then fill in the original SB1Base with the results. 
    ///Author:		www.devtreks.org
    ///Date:		2015, November
    ///NOTES        1. Most data manipulation takes place using the collection property.
    ///             2. By convention, the first member of a this collection is the Score, and the 
    ///             remaining are the indexed Indicators.
    /// </summary>
    public class IndicatorQT1 : Calculator1
    {
        public IndicatorQT1()
            : base()
        {
            InitCalculatorProperties();
            InitIndicatorQT1sProperties();
        }
        public IndicatorQT1(string label, double qTM, double qTL, double qTU, double qT, double qTD1, double qTD2,
            string qTMUnit, string qTLUnit, string qTUUnit, string qTUnit, string qTD1Unit, string qTD2Unit,
            string qMathType, string qMathSubType, string qDistributionType, 
            string qMathExpression, string qMathResult,
            double q1, double q2, double q3, double q4, double q5,
            string q1Unit, string q2Unit, string q3Unit, string q4Unit, string q5Unit)
            : base()
        {
            this.Label = label;
            this.QTM = CalculatorHelpers.CheckForNaNandRound4(qTM);
            this.QTL = CalculatorHelpers.CheckForNaNandRound4(qTL);
            this.QTU = CalculatorHelpers.CheckForNaNandRound4(qTU);
            this.QT = CalculatorHelpers.CheckForNaNandRound4(qT);
            this.QTD1 = CalculatorHelpers.CheckForNaNandRound4(qTD1);
            this.QTD2 = CalculatorHelpers.CheckForNaNandRound4(qTD2);
            this.QTMUnit = qTMUnit;
            this.QTLUnit = qTLUnit;
            this.QTUUnit = qTUUnit;
            this.QTUnit = qTUnit;
            this.QTD1Unit = qTD1Unit;
            this.QTD2Unit = qTD2Unit;
            this.QMathType = qMathType;
            this.QMathSubType = qMathSubType;
            this.QDistributionType = qDistributionType;
            this.QMathExpression = qMathExpression;
            this.MathResult = qMathResult;
            this.Q1 = CalculatorHelpers.CheckForNaNandRound4(q1);
            this.Q1Unit = q1Unit;
            this.Q2 = CalculatorHelpers.CheckForNaNandRound4(q2);
            this.Q2Unit = q2Unit;
            this.Q3 = CalculatorHelpers.CheckForNaNandRound4(q3);
            this.Q3Unit = q3Unit;
            this.Q4 = CalculatorHelpers.CheckForNaNandRound4(q4);
            this.Q4Unit = q4Unit;
            this.Q5 = CalculatorHelpers.CheckForNaNandRound4(q5);
            this.Q5Unit = q5Unit;
        }
        //copy constructor
        public IndicatorQT1(IndicatorQT1 indQT1)
        {
            CopyIndicatorQT1sProperties(indQT1);
        }
        //properties
        //list of IndicatorQT1s 
        public List<IndicatorQT1> IndicatorQT1s = new List<IndicatorQT1>();
        public double Q1 { get; set; }
        public string Q1Unit { get; set; }
        public double Q2 { get; set; }
        public string Q2Unit { get; set; }
        public double Q3 { get; set; }
        public string Q3Unit { get; set; }
        public double Q4 { get; set; }
        public string Q4Unit { get; set; }
        public double Q5 { get; set; }
        public string Q5Unit { get; set; }
        public double QTM { get; set; }
        public double QTL { get; set; }
        public double QTU { get; set; }
        public double QT { get; set; }
        public double QTD1 { get; set; }
        public double QTD2 { get; set; }
        public string QTMUnit { get; set; }
        public string QTLUnit { get; set; }
        public string QTUUnit { get; set; }
        public string QTUnit { get; set; }
        public string QTD1Unit { get; set; }
        public string QTD2Unit { get; set; }
        public string QMathType { get; set; }
        public string QMathSubType { get; set; }
        public string QDistributionType { get; set; }
        public string QMathExpression { get; set; }
        //specialty use in specific algorithms (don't init or copy)
        public string[] Indicators = new string[] { };
        public virtual void InitIndicatorQT1sProperties()
        {
            if (this.IndicatorQT1s == null)
            {
                this.IndicatorQT1s = new List<IndicatorQT1>();
            }
            if (this.IndicatorQT1s.Count() > 0)
            {
                foreach (IndicatorQT1 indQ in this.IndicatorQT1s)
                {
                    InitIndicatorQT1Properties(indQ);
                }
            }
            //and prevent null refs to parent
            InitIndicatorQT1Properties(this);
        }
        private void InitIndicatorQT1Properties(IndicatorQT1 indQ)
        {
            //avoid null references to properties
            indQ.Q1 = 0;
            indQ.Q1Unit = string.Empty;
            indQ.Q2 = 0;
            indQ.Q2Unit = string.Empty;
            indQ.Q3 = 0;
            indQ.Q3Unit = string.Empty;
            indQ.Q4 = 0;
            indQ.Q4Unit = string.Empty;
            indQ.Q5 = 0;
            indQ.Q5Unit = string.Empty;
            indQ.QTM = 0;
            indQ.QTL = 0;
            indQ.QTU = 0;
            indQ.QT = 0;
            indQ.QTD1 = 0;
            indQ.QTD2 = 0;
            indQ.QTMUnit = string.Empty;
            indQ.QTLUnit = string.Empty;
            indQ.QTUUnit = string.Empty;
            indQ.QTUnit = string.Empty;
            indQ.QTD1Unit = string.Empty;
            indQ.QTD2Unit = string.Empty;
            indQ.QMathType = string.Empty;
            indQ.QMathSubType = string.Empty;
            indQ.QDistributionType = string.Empty;
            indQ.QMathExpression = string.Empty;
            //don't overwrite URLs
            if (!indQ.MathResult.ToLower().StartsWith("http"))
            {
                indQ.MathResult = string.Empty;
            }
        }
        public static void InitIndicatorQT1MathProperties(IndicatorQT1 indQ)
        {
            //avoid null references to properties
            indQ.Q1 = 0;
            indQ.Q2 = 0;
            indQ.Q3 = 0;
            indQ.Q4 = 0;
            indQ.Q5 = 0;
            indQ.QTM = 0;
            indQ.QTL = 0;
            indQ.QTU = 0;
            indQ.QT = 0;
            indQ.QTD1 = 0;
            indQ.QTD2 = 0;
            //units must not be null
            if (indQ.Q1Unit == null)
                indQ.Q1Unit = string.Empty;
            if (indQ.Q2Unit == null)
                indQ.Q2Unit = string.Empty;
            if (indQ.Q3Unit == null)
                indQ.Q3Unit = string.Empty;
            if (indQ.Q4Unit == null)
                indQ.Q4Unit = string.Empty;
            if (indQ.Q5Unit == null)
                indQ.Q5Unit = string.Empty;
            if (indQ.QTMUnit == null)
                indQ.QTMUnit = string.Empty;
            if (indQ.QTLUnit == null)
                indQ.QTLUnit = string.Empty;
            if (indQ.QTUUnit == null)
                indQ.QTUUnit = string.Empty;
            if (indQ.QTUnit == null)
                indQ.QTUnit = string.Empty;
            if (indQ.QTD1Unit == null)
                indQ.QTD1Unit = string.Empty;
            if (indQ.QTD2Unit == null)
                indQ.QTD2Unit = string.Empty;
        }
        public virtual void CopyIndicatorQT1sProperties(
            IndicatorQT1 calculator)
        {
            if (calculator == null)
                calculator = new IndicatorQT1();
            if (calculator.IndicatorQT1s == null)
                calculator.IndicatorQT1s = new List<IndicatorQT1>();
            if (calculator.IndicatorQT1s.Count() > 0)
            {
                foreach (IndicatorQT1 calculatorInd in calculator.IndicatorQT1s)
                {
                    IndicatorQT1 qt = new IndicatorQT1(calculatorInd);
                    this.IndicatorQT1s.Add(qt);
                }
                //need all 21 indicators because methods have conditions that must check all 21 (i.e. if label ==)
                //remember that score is the first indicator plus 20 regular ones
                for (int i = (this.IndicatorQT1s.Count - 1); i < 20; i++)
                {
                    IndicatorQT1 qt = new IndicatorQT1();
                    this.IndicatorQT1s.Add(qt);
                }
            }
            //prevent null refs
            CopyIndicatorQT1Properties(this, calculator);
        }
        public void CopyIndicatorQT1Properties(IndicatorQT1 indQ,
            IndicatorQT1 calculator)
        {
            //should be able to replace this with new c#5 syntax
            if (calculator == null)
                calculator = new IndicatorQT1();
            if (indQ == null)
                indQ = new IndicatorQT1();
            indQ.Label = calculator.Label;
            indQ.Q1 = calculator.Q1;
            indQ.Q1Unit = calculator.Q1Unit;
            indQ.Q2 = calculator.Q2;
            indQ.Q2Unit = calculator.Q2Unit;
            indQ.Q3 = calculator.Q3;
            indQ.Q3Unit = calculator.Q3Unit;
            indQ.Q4 = calculator.Q4;
            indQ.Q4Unit = calculator.Q4Unit;
            indQ.Q5 = calculator.Q5;
            indQ.Q5Unit = calculator.Q5Unit;
            indQ.QTM = calculator.QTM;
            indQ.QTL = calculator.QTL;
            indQ.QTU = calculator.QTU;
            indQ.QT = calculator.QT;
            indQ.QTD1 = calculator.QTD1;
            indQ.QTD2 = calculator.QTD2;
            indQ.QTMUnit = calculator.QTMUnit;
            indQ.QTLUnit = calculator.QTLUnit;
            indQ.QTUUnit = calculator.QTUUnit;
            indQ.QTUnit = calculator.QTUnit;
            indQ.QTD1Unit = calculator.QTD1Unit;
            indQ.QTD2Unit = calculator.QTD2Unit;
            indQ.QMathType = calculator.QMathType;
            indQ.QMathSubType = calculator.QMathSubType;
            indQ.QDistributionType = calculator.QDistributionType;
            indQ.QMathExpression = calculator.QMathExpression;
            indQ.MathResult = calculator.MathResult;
        }
        public void CopyIndicatorQT1MathProperties(IndicatorQT1 indQ,
            IndicatorQT1 calculator)
        {
            //do not overwrite the Q1 to Q5 props entered on ui
            //if they need updating, use CopyIndicatorQT1Properties
            indQ.QTM = calculator.QTM;
            indQ.QTL = calculator.QTL;
            indQ.QTU = calculator.QTU;
            //do not include QT - handled separately using mathexpress
            indQ.QTD1 = calculator.QTD1;
            indQ.QTD2 = calculator.QTD2;
            //conditional overwrite
            if (string.IsNullOrEmpty(indQ.QTMUnit))
                indQ.QTMUnit = calculator.QTMUnit;
            if (string.IsNullOrEmpty(indQ.QTD1Unit))
                indQ.QTD1Unit = calculator.QTD1Unit;
            if (string.IsNullOrEmpty(indQ.QTD2Unit))
                indQ.QTD2Unit = calculator.QTD2Unit;
            //overwrite w confid ints
            indQ.QTLUnit = calculator.QTLUnit;
            indQ.QTUUnit = calculator.QTUUnit;
            indQ.MathResult = calculator.MathResult;
        }
    }
}