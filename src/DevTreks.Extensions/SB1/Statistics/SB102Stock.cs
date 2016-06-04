using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The SB102Stock class extends the SB1BaseStock() 
    ///             class and is used by Stock calculators and analyzers 
    ///             to set Output totals.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1.            
    /// </summary> 
    public class SB102Stock : TSB1BaseStock
    {
        //calls the base-class version, and initializes the base class properties.
        public SB102Stock()
            : base()
        {
            //Stock stock
            InitTotalSB102StockProperties();
        }
        //copy constructor
        public SB102Stock(SB102Stock calculator)
            : base(calculator)
        {
            CopyTotalSB102StockProperties(calculator);
        }

        //simple lists holding calculated results (after being run at io level)
        public List<SBB1Calculator> SB2Calcs = new List<SBB1Calculator>();
     
        public virtual void InitTotalSB102StockProperties()
        {
            //avoid null references to properties
            InitTSB1BaseStockProperties();
            this.SB2Calcs = new List<SBB1Calculator>();
        }
        public virtual void CopyTotalSB102StockProperties(
            SB102Stock calculator)
        {
            CopyTSB1BaseStockProperties(calculator);
            this.SB2Calcs = new List<SBB1Calculator>();
            if (calculator.SB2Calcs != null)
            {
                //use the extension function to copy
                this.AddSB2CalcsToStock(calculator.SB2Calcs);
            }
        }
        //set the class properties using the XElement
        public virtual void SetTotalSB102StockProperties(XElement currentCalculationsElement)
        {
            //set the calculator properties
            string sAttNameExt = string.Empty;
            SetTSB1BaseStockProperties(sAttNameExt, currentCalculationsElement);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalSB102StockProperty(string attName,
            string attValue)
        {
            SetTSB1BaseStockProperty(attName, attValue);
        }
        public void SetTotalSB102StockAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            SetTSB1BaseStockAttributes(attNameExtension, currentCalculationsElement);
        }
        public virtual async Task SetTotalSB102StockAttributesAsync(string attNameExtension,
           XmlWriter writer)
        {
            await SetTSB1BaseStockAttributesAsync(attNameExtension, writer);
        }
        //public virtual void SetTotalSB102StockAttributes(string attNameExtension,
        //   ref XmlWriter writer)
        //{
        //    SetTSB1BaseStockAttributes(attNameExtension, ref writer);
        //}
    }
    public static class SB102Extensions
    {
        public static void AddSB2CalcsToStock(this SB102Stock baseStat,
            List<SBB1Calculator> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.SB2Calcs == null)
                    baseStat.SB2Calcs = new List<SBB1Calculator>();
                foreach (SBB1Calculator calc in calcs)
                {
                    if (calc.CalculatorType
                        == SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString())
                    {
                        SBB1Calculator sbc = new SBB1Calculator();
                        if (calc.GetType().Equals(sbc.GetType()))
                        {
                            SBB1Calculator sbcOutput = (SBB1Calculator)calc;
                            sbc.CopySB1B1Properties(sbcOutput);
                            baseStat.SB2Calcs.Add(sbc);
                        }
                    }
                }
            }
        }
        public static void AddOutputCalcsToStock(this SB102Stock baseStat, List<Calculator1> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.SB2Calcs == null)
                    baseStat.SB2Calcs = new List<SBB1Calculator>();
                foreach (Calculator1 calc in calcs)
                {
                    AddOutputCalcToStock(baseStat, calc);
                }
            }
        }
        public static void AddOutputCalcToStock(this SB102Stock baseStat, Calculator1 calc)
        {
            if (calc.CalculatorType
               == SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString())
            {
                SBB1Calculator sbc = new SBB1Calculator();
                if (calc.GetType().Equals(sbc.GetType()))
                {
                    SBB1Calculator sbcOutput = (SBB1Calculator)calc;
                    sbc.CopySB1B1Properties(sbcOutput);
                    baseStat.SB2Calcs.Add(sbc);
                }
            }
        }
        public static void AddOutputToTotalStock(this SB102Stock baseStat, SBB1Calculator sb1Output)
        {
            //add the combined scores
            baseStat.AddSubStockScoreToTotalStock(1, sb1Output.SB1Score, sb1Output.SB1ScoreUnit,
              sb1Output.SB1ScoreD1Amount, sb1Output.SB1ScoreD1Unit, sb1Output.SB1ScoreD2Amount, sb1Output.SB1ScoreD2Unit,
              sb1Output.SB1ScoreM, sb1Output.SB1ScoreMUnit, sb1Output.SB1ScoreLAmount,
              sb1Output.SB1ScoreLUnit, sb1Output.SB1ScoreUAmount, sb1Output.SB1ScoreUUnit,
              sb1Output.SB1ScoreMathExpression, sb1Output.SB1ScoreDistType, sb1Output.SB1ScoreMathType,
              sb1Output.SB1ScoreMathSubType, sb1Output.SB1ScoreMathResult, sb1Output.SB1Iterations);
            //add the individual indicators
            int i = baseStat.GetStockIndex(sb1Output.SB1Label1);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name1, sb1Output.SB1Label1, sb1Output.SB1RelLabel1,
                        sb1Output.SB1Description1, sb1Output.SB1MathExpression1, sb1Output.SB1Date1,
                        sb1Output.SB1Type1, sb1Output.SB1MathType1, sb1Output.SB1BaseIO1, sb1Output.SB1MathOperator1, sb1Output.SB1MathSubType1,
                        sb1Output.SB11Unit1, sb1Output.SB12Unit1, sb1Output.SB13Unit1, sb1Output.SB14Unit1, sb1Output.SB15Unit1, sb1Output.SB1TUnit1,
                        sb1Output.SB1TD1Unit1, sb1Output.SB1TD2Unit1, sb1Output.SB1TMUnit1, sb1Output.SB1TLUnit1, sb1Output.SB1TUUnit1);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount1, sb1Output.SB12Amount1, sb1Output.SB13Amount1, sb1Output.SB14Amount1, sb1Output.SB15Amount1, sb1Output.SB1TAmount1,
                    sb1Output.SB1TD1Amount1, sb1Output.SB1TD2Amount1, sb1Output.SB1TMAmount1, sb1Output.SB1TLAmount1, sb1Output.SB1TUAmount1);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label2);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name2, sb1Output.SB1Label2, sb1Output.SB1RelLabel2,
                        sb1Output.SB1Description2, sb1Output.SB1MathExpression2, sb1Output.SB1Date2,
                        sb1Output.SB1Type2, sb1Output.SB1MathType2, sb1Output.SB1BaseIO2, sb1Output.SB1MathOperator2, sb1Output.SB1MathSubType2,
                        sb1Output.SB11Unit2, sb1Output.SB12Unit2, sb1Output.SB13Unit2, sb1Output.SB14Unit2, sb1Output.SB15Unit2, sb1Output.SB1TUnit2,
                        sb1Output.SB1TD1Unit2, sb1Output.SB1TD2Unit2, sb1Output.SB1TMUnit2, sb1Output.SB1TLUnit2, sb1Output.SB1TUUnit2);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount2, sb1Output.SB12Amount2, sb1Output.SB13Amount2, sb1Output.SB14Amount2, sb1Output.SB15Amount2, sb1Output.SB1TAmount2,
                    sb1Output.SB1TD1Amount2, sb1Output.SB1TD2Amount2, sb1Output.SB1TMAmount2, sb1Output.SB1TLAmount2, sb1Output.SB1TUAmount2);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label3);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name3, sb1Output.SB1Label3, sb1Output.SB1RelLabel3,
                        sb1Output.SB1Description3, sb1Output.SB1MathExpression3, sb1Output.SB1Date3,
                        sb1Output.SB1Type3, sb1Output.SB1MathType3, sb1Output.SB1BaseIO3, sb1Output.SB1MathOperator3, sb1Output.SB1MathSubType3,
                        sb1Output.SB11Unit3, sb1Output.SB12Unit3, sb1Output.SB13Unit3, sb1Output.SB14Unit3, sb1Output.SB15Unit3, sb1Output.SB1TUnit3,
                        sb1Output.SB1TD1Unit3, sb1Output.SB1TD2Unit3, sb1Output.SB1TMUnit3, sb1Output.SB1TLUnit3, sb1Output.SB1TUUnit3);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount3, sb1Output.SB12Amount3, sb1Output.SB13Amount3, sb1Output.SB14Amount3, sb1Output.SB15Amount3, sb1Output.SB1TAmount3,
                    sb1Output.SB1TD1Amount3, sb1Output.SB1TD2Amount3, sb1Output.SB1TMAmount3, sb1Output.SB1TLAmount3, sb1Output.SB1TUAmount3);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label4);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name4, sb1Output.SB1Label4, sb1Output.SB1RelLabel4,
                       sb1Output.SB1Description4, sb1Output.SB1MathExpression4, sb1Output.SB1Date4,
                        sb1Output.SB1Type4, sb1Output.SB1MathType4, sb1Output.SB1BaseIO4, sb1Output.SB1MathOperator4, sb1Output.SB1MathSubType4,
                        sb1Output.SB11Unit4, sb1Output.SB12Unit4, sb1Output.SB13Unit4, sb1Output.SB14Unit4, sb1Output.SB15Unit4, sb1Output.SB1TUnit4,
                        sb1Output.SB1TD1Unit4, sb1Output.SB1TD2Unit4, sb1Output.SB1TMUnit4, sb1Output.SB1TLUnit4, sb1Output.SB1TUUnit4);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount4, sb1Output.SB12Amount4, sb1Output.SB13Amount4, sb1Output.SB14Amount4, sb1Output.SB15Amount4, sb1Output.SB1TAmount4,
                    sb1Output.SB1TD1Amount4, sb1Output.SB1TD2Amount4, sb1Output.SB1TMAmount4, sb1Output.SB1TLAmount4, sb1Output.SB1TUAmount4);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label5);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name5, sb1Output.SB1Label5, sb1Output.SB1RelLabel5,
                        sb1Output.SB1Description5, sb1Output.SB1MathExpression5, sb1Output.SB1Date5,
                        sb1Output.SB1Type5, sb1Output.SB1MathType5, sb1Output.SB1BaseIO5, sb1Output.SB1MathOperator5, sb1Output.SB1MathSubType5,
                        sb1Output.SB11Unit5, sb1Output.SB12Unit5, sb1Output.SB13Unit5, sb1Output.SB14Unit5, sb1Output.SB15Unit5, sb1Output.SB1TUnit5,
                        sb1Output.SB1TD1Unit5, sb1Output.SB1TD2Unit5, sb1Output.SB1TMUnit5, sb1Output.SB1TLUnit5, sb1Output.SB1TUUnit5);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                sb1Output.SB11Amount5, sb1Output.SB12Amount5, sb1Output.SB13Amount5, sb1Output.SB14Amount5, sb1Output.SB15Amount5, sb1Output.SB1TAmount5,
                sb1Output.SB1TD1Amount5, sb1Output.SB1TD2Amount5, sb1Output.SB1TMAmount5, sb1Output.SB1TLAmount5, sb1Output.SB1TUAmount5);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label6);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name6, sb1Output.SB1Label6, sb1Output.SB1RelLabel6,
                        sb1Output.SB1Description6, sb1Output.SB1MathExpression6, sb1Output.SB1Date6,
                        sb1Output.SB1Type6, sb1Output.SB1MathType6, sb1Output.SB1BaseIO6, sb1Output.SB1MathOperator6, sb1Output.SB1MathSubType6,
                        sb1Output.SB11Unit6, sb1Output.SB12Unit6, sb1Output.SB13Unit6, sb1Output.SB14Unit6, sb1Output.SB15Unit6, sb1Output.SB1TUnit6,
                        sb1Output.SB1TD1Unit6, sb1Output.SB1TD2Unit6, sb1Output.SB1TMUnit6, sb1Output.SB1TLUnit6, sb1Output.SB1TUUnit6);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount6, sb1Output.SB12Amount6, sb1Output.SB13Amount6, sb1Output.SB14Amount6, sb1Output.SB15Amount6, sb1Output.SB1TAmount6,
                    sb1Output.SB1TD1Amount6, sb1Output.SB1TD2Amount6, sb1Output.SB1TMAmount6, sb1Output.SB1TLAmount6, sb1Output.SB1TUAmount6);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label7);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name7, sb1Output.SB1Label7, sb1Output.SB1RelLabel7,
                    sb1Output.SB1Description7, sb1Output.SB1MathExpression7, sb1Output.SB1Date7,
                        sb1Output.SB1Type7, sb1Output.SB1MathType7, sb1Output.SB1BaseIO7, sb1Output.SB1MathOperator7, sb1Output.SB1MathSubType7,
                        sb1Output.SB11Unit7, sb1Output.SB12Unit7, sb1Output.SB13Unit7, sb1Output.SB14Unit7, sb1Output.SB15Unit7, sb1Output.SB1TUnit7,
                        sb1Output.SB1TD1Unit7, sb1Output.SB1TD2Unit7, sb1Output.SB1TMUnit7, sb1Output.SB1TLUnit7, sb1Output.SB1TUUnit7);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount7, sb1Output.SB12Amount7, sb1Output.SB13Amount7, sb1Output.SB14Amount7, sb1Output.SB15Amount7, sb1Output.SB1TAmount7,
                    sb1Output.SB1TD1Amount7, sb1Output.SB1TD2Amount7, sb1Output.SB1TMAmount7, sb1Output.SB1TLAmount7, sb1Output.SB1TUAmount7);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label8);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name8, sb1Output.SB1Label8, sb1Output.SB1RelLabel8,
                    sb1Output.SB1Description8, sb1Output.SB1MathExpression8, sb1Output.SB1Date8,
                        sb1Output.SB1Type8, sb1Output.SB1MathType8, sb1Output.SB1BaseIO8, sb1Output.SB1MathOperator8, sb1Output.SB1MathSubType8,
                        sb1Output.SB11Unit8, sb1Output.SB12Unit8, sb1Output.SB13Unit8, sb1Output.SB14Unit8, sb1Output.SB15Unit8, sb1Output.SB1TUnit8,
                        sb1Output.SB1TD1Unit8, sb1Output.SB1TD2Unit8, sb1Output.SB1TMUnit8, sb1Output.SB1TLUnit8, sb1Output.SB1TUUnit8);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                sb1Output.SB11Amount8, sb1Output.SB12Amount8, sb1Output.SB13Amount8, sb1Output.SB14Amount8, sb1Output.SB15Amount8, sb1Output.SB1TAmount8,
                sb1Output.SB1TD1Amount8, sb1Output.SB1TD2Amount8, sb1Output.SB1TMAmount8, sb1Output.SB1TLAmount8, sb1Output.SB1TUAmount8);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label9);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name9, sb1Output.SB1Label9, sb1Output.SB1RelLabel9,
                    sb1Output.SB1Description9, sb1Output.SB1MathExpression9, sb1Output.SB1Date9,
                        sb1Output.SB1Type9, sb1Output.SB1MathType9, sb1Output.SB1BaseIO9, sb1Output.SB1MathOperator9, sb1Output.SB1MathSubType9,
                        sb1Output.SB11Unit9, sb1Output.SB12Unit9, sb1Output.SB13Unit9, sb1Output.SB14Unit9, sb1Output.SB15Unit9, sb1Output.SB1TUnit9,
                        sb1Output.SB1TD1Unit9, sb1Output.SB1TD2Unit9, sb1Output.SB1TMUnit9, sb1Output.SB1TLUnit9, sb1Output.SB1TUUnit9);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount9, sb1Output.SB12Amount9, sb1Output.SB13Amount9, sb1Output.SB14Amount9, sb1Output.SB15Amount9, sb1Output.SB1TAmount9,
                    sb1Output.SB1TD1Amount9, sb1Output.SB1TD2Amount9, sb1Output.SB1TMAmount9, sb1Output.SB1TLAmount9, sb1Output.SB1TUAmount9);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label10);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name10, sb1Output.SB1Label10, sb1Output.SB1RelLabel10,
                       sb1Output.SB1Description10, sb1Output.SB1MathExpression10, sb1Output.SB1Date10,
                        sb1Output.SB1Type10, sb1Output.SB1MathType10, sb1Output.SB1BaseIO10, sb1Output.SB1MathOperator10, sb1Output.SB1MathSubType10,
                        sb1Output.SB11Unit10, sb1Output.SB12Unit10, sb1Output.SB13Unit10, sb1Output.SB14Unit10, sb1Output.SB15Unit10, sb1Output.SB1TUnit10,
                        sb1Output.SB1TD1Unit10, sb1Output.SB1TD2Unit10, sb1Output.SB1TMUnit10, sb1Output.SB1TLUnit10, sb1Output.SB1TUUnit10);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount10, sb1Output.SB12Amount10, sb1Output.SB13Amount10, sb1Output.SB14Amount10, sb1Output.SB15Amount10, sb1Output.SB1TAmount10,
                    sb1Output.SB1TD1Amount10, sb1Output.SB1TD2Amount10, sb1Output.SB1TMAmount10, sb1Output.SB1TLAmount10, sb1Output.SB1TUAmount10);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label11);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name11, sb1Output.SB1Label11, sb1Output.SB1RelLabel11,
                        sb1Output.SB1Description11, sb1Output.SB1MathExpression11, sb1Output.SB1Date11,
                        sb1Output.SB1Type11, sb1Output.SB1MathType11, sb1Output.SB1BaseIO11, sb1Output.SB1MathOperator11, sb1Output.SB1MathSubType11,
                        sb1Output.SB11Unit11, sb1Output.SB12Unit11, sb1Output.SB13Unit11, sb1Output.SB14Unit11, sb1Output.SB15Unit11, sb1Output.SB1TUnit11,
                        sb1Output.SB1TD1Unit11, sb1Output.SB1TD2Unit11, sb1Output.SB1TMUnit11, sb1Output.SB1TLUnit11, sb1Output.SB1TUUnit11);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount11, sb1Output.SB12Amount11, sb1Output.SB13Amount11, sb1Output.SB14Amount11, sb1Output.SB15Amount11, sb1Output.SB1TAmount11,
                    sb1Output.SB1TD1Amount11, sb1Output.SB1TD2Amount11, sb1Output.SB1TMAmount11, sb1Output.SB1TLAmount11, sb1Output.SB1TUAmount11);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label12);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name12, sb1Output.SB1Label12, sb1Output.SB1RelLabel12,
                        sb1Output.SB1Description12, sb1Output.SB1MathExpression12, sb1Output.SB1Date12,
                        sb1Output.SB1Type12, sb1Output.SB1MathType12, sb1Output.SB1BaseIO12, sb1Output.SB1MathOperator12, sb1Output.SB1MathSubType12,
                        sb1Output.SB11Unit12, sb1Output.SB12Unit12, sb1Output.SB13Unit12, sb1Output.SB14Unit12, sb1Output.SB15Unit12, sb1Output.SB1TUnit12,
                        sb1Output.SB1TD1Unit12, sb1Output.SB1TD2Unit12, sb1Output.SB1TMUnit12, sb1Output.SB1TLUnit12, sb1Output.SB1TUUnit12);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount12, sb1Output.SB12Amount12, sb1Output.SB13Amount12, sb1Output.SB14Amount12, sb1Output.SB15Amount12, sb1Output.SB1TAmount12,
                    sb1Output.SB1TD1Amount12, sb1Output.SB1TD2Amount12, sb1Output.SB1TMAmount12, sb1Output.SB1TLAmount12, sb1Output.SB1TUAmount12);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label13);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name13, sb1Output.SB1Label13, sb1Output.SB1RelLabel13,
                        sb1Output.SB1Description13, sb1Output.SB1MathExpression13, sb1Output.SB1Date13,
                        sb1Output.SB1Type13, sb1Output.SB1MathType13, sb1Output.SB1BaseIO13, sb1Output.SB1MathOperator13, sb1Output.SB1MathSubType13,
                        sb1Output.SB11Unit13, sb1Output.SB12Unit13, sb1Output.SB13Unit13, sb1Output.SB14Unit13, sb1Output.SB15Unit13, sb1Output.SB1TUnit13,
                        sb1Output.SB1TD1Unit13, sb1Output.SB1TD2Unit13, sb1Output.SB1TMUnit13, sb1Output.SB1TLUnit13, sb1Output.SB1TUUnit13);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount13, sb1Output.SB12Amount13, sb1Output.SB13Amount13, sb1Output.SB14Amount13, sb1Output.SB15Amount13, sb1Output.SB1TAmount13,
                    sb1Output.SB1TD1Amount13, sb1Output.SB1TD2Amount13, sb1Output.SB1TMAmount13, sb1Output.SB1TLAmount13, sb1Output.SB1TUAmount13);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label14);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name14, sb1Output.SB1Label14, sb1Output.SB1RelLabel14,
                        sb1Output.SB1Description14, sb1Output.SB1MathExpression14, sb1Output.SB1Date14,
                        sb1Output.SB1Type14, sb1Output.SB1MathType14, sb1Output.SB1BaseIO14, sb1Output.SB1MathOperator14, sb1Output.SB1MathSubType14,
                        sb1Output.SB11Unit14, sb1Output.SB12Unit14, sb1Output.SB13Unit14, sb1Output.SB14Unit14, sb1Output.SB15Unit14, sb1Output.SB1TUnit14,
                        sb1Output.SB1TD1Unit14, sb1Output.SB1TD2Unit14, sb1Output.SB1TMUnit14, sb1Output.SB1TLUnit14, sb1Output.SB1TUUnit14);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount14, sb1Output.SB12Amount14, sb1Output.SB13Amount14, sb1Output.SB14Amount14, sb1Output.SB15Amount14, sb1Output.SB1TAmount14,
                    sb1Output.SB1TD1Amount14, sb1Output.SB1TD2Amount14, sb1Output.SB1TMAmount14, sb1Output.SB1TLAmount14, sb1Output.SB1TUAmount14);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label15);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name15, sb1Output.SB1Label15, sb1Output.SB1RelLabel15,
                        sb1Output.SB1Description15, sb1Output.SB1MathExpression15, sb1Output.SB1Date15,
                        sb1Output.SB1Type15, sb1Output.SB1MathType15, sb1Output.SB1BaseIO15, sb1Output.SB1MathOperator15, sb1Output.SB1MathSubType15,
                        sb1Output.SB11Unit15, sb1Output.SB12Unit15, sb1Output.SB13Unit15, sb1Output.SB14Unit15, sb1Output.SB15Unit15, sb1Output.SB1TUnit15,
                        sb1Output.SB1TD1Unit15, sb1Output.SB1TD2Unit15, sb1Output.SB1TMUnit15, sb1Output.SB1TLUnit15, sb1Output.SB1TUUnit15);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount15, sb1Output.SB12Amount15, sb1Output.SB13Amount15, sb1Output.SB14Amount15, sb1Output.SB15Amount15, sb1Output.SB1TAmount15,
                    sb1Output.SB1TD1Amount15, sb1Output.SB1TD2Amount15, sb1Output.SB1TMAmount15, sb1Output.SB1TLAmount15, sb1Output.SB1TUAmount15);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label16);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name16, sb1Output.SB1Label16, sb1Output.SB1RelLabel16,
                        sb1Output.SB1Description16, sb1Output.SB1MathExpression16, sb1Output.SB1Date16,
                        sb1Output.SB1Type16, sb1Output.SB1MathType16, sb1Output.SB1BaseIO16, sb1Output.SB1MathOperator16, sb1Output.SB1MathSubType16,
                        sb1Output.SB11Unit16, sb1Output.SB12Unit16, sb1Output.SB13Unit16, sb1Output.SB14Unit16, sb1Output.SB15Unit16, sb1Output.SB1TUnit16,
                        sb1Output.SB1TD1Unit16, sb1Output.SB1TD2Unit16, sb1Output.SB1TMUnit16, sb1Output.SB1TLUnit16, sb1Output.SB1TUUnit16);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount16, sb1Output.SB12Amount16, sb1Output.SB13Amount16, sb1Output.SB14Amount16, sb1Output.SB15Amount16, sb1Output.SB1TAmount16,
                    sb1Output.SB1TD1Amount16, sb1Output.SB1TD2Amount16, sb1Output.SB1TMAmount16, sb1Output.SB1TLAmount16, sb1Output.SB1TUAmount16);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label17);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name17, sb1Output.SB1Label17, sb1Output.SB1RelLabel17,
                        sb1Output.SB1Description17, sb1Output.SB1MathExpression17, sb1Output.SB1Date17,
                        sb1Output.SB1Type17, sb1Output.SB1MathType17, sb1Output.SB1BaseIO17, sb1Output.SB1MathOperator17, sb1Output.SB1MathSubType17,
                        sb1Output.SB11Unit17, sb1Output.SB12Unit17, sb1Output.SB13Unit17, sb1Output.SB14Unit17, sb1Output.SB15Unit17, sb1Output.SB1TUnit17,
                        sb1Output.SB1TD1Unit17, sb1Output.SB1TD2Unit17, sb1Output.SB1TMUnit17, sb1Output.SB1TLUnit17, sb1Output.SB1TUUnit17);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount17, sb1Output.SB12Amount17, sb1Output.SB13Amount17, sb1Output.SB14Amount17, sb1Output.SB15Amount17, sb1Output.SB1TAmount17,
                    sb1Output.SB1TD1Amount17, sb1Output.SB1TD2Amount17, sb1Output.SB1TMAmount17, sb1Output.SB1TLAmount17, sb1Output.SB1TUAmount17);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label18);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name18, sb1Output.SB1Label18, sb1Output.SB1RelLabel18,
                        sb1Output.SB1Description18, sb1Output.SB1MathExpression18, sb1Output.SB1Date18,
                        sb1Output.SB1Type18, sb1Output.SB1MathType18, sb1Output.SB1BaseIO18, sb1Output.SB1MathOperator18, sb1Output.SB1MathSubType18,
                        sb1Output.SB11Unit18, sb1Output.SB12Unit18, sb1Output.SB13Unit18, sb1Output.SB14Unit18, sb1Output.SB15Unit18, sb1Output.SB1TUnit18,
                        sb1Output.SB1TD1Unit18, sb1Output.SB1TD2Unit18, sb1Output.SB1TMUnit18, sb1Output.SB1TLUnit18, sb1Output.SB1TUUnit18);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount18, sb1Output.SB12Amount18, sb1Output.SB13Amount18, sb1Output.SB14Amount18, sb1Output.SB15Amount18, sb1Output.SB1TAmount18,
                    sb1Output.SB1TD1Amount18, sb1Output.SB1TD2Amount18, sb1Output.SB1TMAmount18, sb1Output.SB1TLAmount18, sb1Output.SB1TUAmount18);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label19);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name19, sb1Output.SB1Label19, sb1Output.SB1RelLabel19,
                        sb1Output.SB1Description19, sb1Output.SB1MathExpression19, sb1Output.SB1Date19,
                        sb1Output.SB1Type19, sb1Output.SB1MathType19, sb1Output.SB1BaseIO19, sb1Output.SB1MathOperator19, sb1Output.SB1MathSubType19,
                        sb1Output.SB11Unit19, sb1Output.SB12Unit19, sb1Output.SB13Unit19, sb1Output.SB14Unit19, sb1Output.SB15Unit19, sb1Output.SB1TUnit19,
                        sb1Output.SB1TD1Unit19, sb1Output.SB1TD2Unit19, sb1Output.SB1TMUnit19, sb1Output.SB1TLUnit19, sb1Output.SB1TUUnit19);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount19, sb1Output.SB12Amount19, sb1Output.SB13Amount19, sb1Output.SB14Amount19, sb1Output.SB15Amount19, sb1Output.SB1TAmount19,
                    sb1Output.SB1TD1Amount19, sb1Output.SB1TD2Amount19, sb1Output.SB1TMAmount19, sb1Output.SB1TLAmount19, sb1Output.SB1TUAmount19);
            }
            i = baseStat.GetStockIndex(sb1Output.SB1Label20);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Output.SB1Name20, sb1Output.SB1Label20, sb1Output.SB1RelLabel20,
                        sb1Output.SB1Description20, sb1Output.SB1MathExpression20, sb1Output.SB1Date20,
                        sb1Output.SB1Type20, sb1Output.SB1MathType20, sb1Output.SB1BaseIO20, sb1Output.SB1MathOperator20, sb1Output.SB1MathSubType20,
                        sb1Output.SB11Unit20, sb1Output.SB12Unit20, sb1Output.SB13Unit20, sb1Output.SB14Unit20, sb1Output.SB15Unit20, sb1Output.SB1TUnit20,
                        sb1Output.SB1TD1Unit20, sb1Output.SB1TD2Unit20, sb1Output.SB1TMUnit20, sb1Output.SB1TLUnit20, sb1Output.SB1TUUnit20);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Output.SB11Amount20, sb1Output.SB12Amount20, sb1Output.SB13Amount20, sb1Output.SB14Amount20, sb1Output.SB15Amount20, sb1Output.SB1TAmount20,
                    sb1Output.SB1TD1Amount20, sb1Output.SB1TD2Amount20, sb1Output.SB1TMAmount20, sb1Output.SB1TLAmount20, sb1Output.SB1TUAmount20);
            }
        }
    }
}

