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
    ///Purpose:		The SB101Stock class extends the SB1BaseStock() 
    ///             class and is used by Stock calculators and analyzers 
    ///             to set Input totals.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. 
    /// </summary>
    public class SB101Stock : TSB1BaseStock
    {
        //calls the base-class version, and initializes the base class properties.
        public SB101Stock()
            : base()
        {
            //Stock stock
            InitTotalSB101StockProperties();
        }
        //copy constructor
        public SB101Stock(SB101Stock calculator)
            : base(calculator)
        {
            CopyTotalSB101StockProperties(calculator);
        }

        //simple lists holding calculated results (after being run at io level)
        public List<SBC1Calculator> SB1Calcs = new List<SBC1Calculator>();
     
        public virtual void InitTotalSB101StockProperties()
        {
            //avoid null references to properties
            InitTSB1BaseStockProperties();
            this.SB1Calcs = new List<SBC1Calculator>();
        }
        public virtual void CopyTotalSB101StockProperties(
            SB101Stock calculator)
        {
            CopyTSB1BaseStockProperties(calculator);
            this.SB1Calcs = new List<SBC1Calculator>();
            if (calculator.SB1Calcs != null)
            {
                //use the extension function to copy
                this.AddSB1CalcsToStock(calculator.SB1Calcs);
            }
        }
        //set the class properties using the XElement
        public virtual void SetTotalSB101StockProperties(XElement currentCalculationsElement)
        {
            //set the calculator properties
            string sAttNameExt = string.Empty;
            SetTSB1BaseStockProperties(sAttNameExt, currentCalculationsElement);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalSB101StockProperty(string attName,
            string attValue)
        {
            SetTSB1BaseStockProperty(attName, attValue);
        }
        public void SetTotalSB101StockAttributes(string attNameExtension,
            XElement currentCalculationsElement)
        {
            SetTSB1BaseStockAttributes(attNameExtension, currentCalculationsElement);
        }
        public virtual async Task SetTotalSB101StockAttributesAsync(string attNameExtension,
           XmlWriter writer)
        {
            await SetTSB1BaseStockAttributesAsync(attNameExtension, writer);
        }
        //public virtual void SetTotalSB101StockAttributes(string attNameExtension,
        //   XmlWriter writer)
        //{
        //    SetTSB1BaseStockAttributes(attNameExtension, ref writer);
        //}
    }
    public static class SB101Extensions
    {
        public static void AddSB1CalcsToStock(this SB101Stock baseStat,
            List<SBC1Calculator> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.SB1Calcs == null)
                    baseStat.SB1Calcs = new List<SBC1Calculator>();
                foreach (SBC1Calculator calc in calcs)
                {
                    if (calc.CalculatorType
                        == SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString())
                    {
                        SBC1Calculator sbc = new SBC1Calculator();
                        if (calc.GetType().Equals(sbc.GetType()))
                        {
                            SBC1Calculator sbcInput = (SBC1Calculator)calc;
                            sbc.CopySB1C1Properties(sbcInput);
                            baseStat.SB1Calcs.Add(sbc);
                        }
                    }
                }
            }
        }
        public static void AddInputCalcsToStock(this SB101Stock baseStat, List<Calculator1> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.SB1Calcs == null)
                    baseStat.SB1Calcs = new List<SBC1Calculator>();
                foreach (Calculator1 calc in calcs)
                {
                    AddInputCalcToStock(baseStat, calc);
                }
            }
        }
        public static void AddInputCalcToStock(this SB101Stock baseStat, Calculator1 calc)
        {
            if (calc.CalculatorType
               == SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString())
            {
                SBC1Calculator sbc = new SBC1Calculator();
                if (calc.GetType().Equals(sbc.GetType()))
                {
                    SBC1Calculator sbcInput = (SBC1Calculator)calc;
                    sbc.CopySB1C1Properties(sbcInput);
                    baseStat.SB1Calcs.Add(sbc);
                }
            }
        }
        public static void AddInputToTotalStock(this SB101Stock baseStat, SBC1Calculator sb1Input)
        {
            //add the combined scores
            baseStat.AddSubStockScoreToTotalStock(1, sb1Input.SB1Score, sb1Input.SB1ScoreUnit,
               sb1Input.SB1ScoreD1Amount, sb1Input.SB1ScoreD1Unit, sb1Input.SB1ScoreD2Amount, sb1Input.SB1ScoreD2Unit,
               sb1Input.SB1ScoreM, sb1Input.SB1ScoreMUnit, sb1Input.SB1ScoreLAmount,
               sb1Input.SB1ScoreLUnit, sb1Input.SB1ScoreUAmount, sb1Input.SB1ScoreUUnit, 
               sb1Input.SB1ScoreMathExpression, sb1Input.SB1ScoreDistType, sb1Input.SB1ScoreMathType,
               sb1Input.SB1ScoreMathSubType, sb1Input.SB1ScoreMathResult, sb1Input.SB1Iterations);
            //add the individual indicators
            int i = baseStat.GetStockIndex(sb1Input.SB1Label1);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name1, sb1Input.SB1Label1, sb1Input.SB1RelLabel1,
                        sb1Input.SB1Description1, sb1Input.SB1MathExpression1, sb1Input.SB1Date1,
                        sb1Input.SB1Type1, sb1Input.SB1MathType1, sb1Input.SB1BaseIO1, sb1Input.SB1MathOperator1, sb1Input.SB1MathSubType1,
                        sb1Input.SB11Unit1, sb1Input.SB12Unit1, sb1Input.SB13Unit1, sb1Input.SB14Unit1, sb1Input.SB15Unit1, sb1Input.SB1TUnit1,
                        sb1Input.SB1TD1Unit1, sb1Input.SB1TD2Unit1, sb1Input.SB1TMUnit1, sb1Input.SB1TLUnit1, sb1Input.SB1TUUnit1);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount1, sb1Input.SB12Amount1, sb1Input.SB13Amount1, sb1Input.SB14Amount1, sb1Input.SB15Amount1, sb1Input.SB1TAmount1,
                    sb1Input.SB1TD1Amount1, sb1Input.SB1TD2Amount1, sb1Input.SB1TMAmount1, sb1Input.SB1TLAmount1, sb1Input.SB1TUAmount1);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label2);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name2, sb1Input.SB1Label2, sb1Input.SB1RelLabel2,
                        sb1Input.SB1Description2, sb1Input.SB1MathExpression2, sb1Input.SB1Date2,
                        sb1Input.SB1Type2, sb1Input.SB1MathType2, sb1Input.SB1BaseIO2, sb1Input.SB1MathOperator2, sb1Input.SB1MathSubType2,
                        sb1Input.SB11Unit2, sb1Input.SB12Unit2, sb1Input.SB13Unit2, sb1Input.SB14Unit2, sb1Input.SB15Unit2, sb1Input.SB1TUnit2,
                        sb1Input.SB1TD1Unit2, sb1Input.SB1TD2Unit2, sb1Input.SB1TMUnit2, sb1Input.SB1TLUnit2, sb1Input.SB1TUUnit2);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount2, sb1Input.SB12Amount2, sb1Input.SB13Amount2, sb1Input.SB14Amount2, sb1Input.SB15Amount2, sb1Input.SB1TAmount2,
                    sb1Input.SB1TD1Amount2, sb1Input.SB1TD2Amount2, sb1Input.SB1TMAmount2, sb1Input.SB1TLAmount2, sb1Input.SB1TUAmount2);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label3);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name3, sb1Input.SB1Label3, sb1Input.SB1RelLabel3,
                        sb1Input.SB1Description3, sb1Input.SB1MathExpression3, sb1Input.SB1Date3,
                        sb1Input.SB1Type3, sb1Input.SB1MathType3, sb1Input.SB1BaseIO3, sb1Input.SB1MathOperator3, sb1Input.SB1MathSubType3,
                        sb1Input.SB11Unit3, sb1Input.SB12Unit3, sb1Input.SB13Unit3, sb1Input.SB14Unit3, sb1Input.SB15Unit3, sb1Input.SB1TUnit3,
                        sb1Input.SB1TD1Unit3, sb1Input.SB1TD2Unit3, sb1Input.SB1TMUnit3, sb1Input.SB1TLUnit3, sb1Input.SB1TUUnit3);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount3, sb1Input.SB12Amount3, sb1Input.SB13Amount3, sb1Input.SB14Amount3, sb1Input.SB15Amount3, sb1Input.SB1TAmount3,
                    sb1Input.SB1TD1Amount3, sb1Input.SB1TD2Amount3, sb1Input.SB1TMAmount3, sb1Input.SB1TLAmount3, sb1Input.SB1TUAmount3);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label4);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name4, sb1Input.SB1Label4, sb1Input.SB1RelLabel4,
                       sb1Input.SB1Description4, sb1Input.SB1MathExpression4, sb1Input.SB1Date4,
                        sb1Input.SB1Type4, sb1Input.SB1MathType4, sb1Input.SB1BaseIO4, sb1Input.SB1MathOperator4, sb1Input.SB1MathSubType4,
                        sb1Input.SB11Unit4, sb1Input.SB12Unit4, sb1Input.SB13Unit4, sb1Input.SB14Unit4, sb1Input.SB15Unit4, sb1Input.SB1TUnit4,
                        sb1Input.SB1TD1Unit4, sb1Input.SB1TD2Unit4, sb1Input.SB1TMUnit4, sb1Input.SB1TLUnit4, sb1Input.SB1TUUnit4);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount4, sb1Input.SB12Amount4, sb1Input.SB13Amount4, sb1Input.SB14Amount4, sb1Input.SB15Amount4, sb1Input.SB1TAmount4,
                    sb1Input.SB1TD1Amount4, sb1Input.SB1TD2Amount4, sb1Input.SB1TMAmount4, sb1Input.SB1TLAmount4, sb1Input.SB1TUAmount4);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label5);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name5, sb1Input.SB1Label5, sb1Input.SB1RelLabel5,
                        sb1Input.SB1Description5, sb1Input.SB1MathExpression5, sb1Input.SB1Date5,
                        sb1Input.SB1Type5, sb1Input.SB1MathType5, sb1Input.SB1BaseIO5, sb1Input.SB1MathOperator5, sb1Input.SB1MathSubType5,
                        sb1Input.SB11Unit5, sb1Input.SB12Unit5, sb1Input.SB13Unit5, sb1Input.SB14Unit5, sb1Input.SB15Unit5, sb1Input.SB1TUnit5,
                        sb1Input.SB1TD1Unit5, sb1Input.SB1TD2Unit5, sb1Input.SB1TMUnit5, sb1Input.SB1TLUnit5, sb1Input.SB1TUUnit5);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                sb1Input.SB11Amount5, sb1Input.SB12Amount5, sb1Input.SB13Amount5, sb1Input.SB14Amount5, sb1Input.SB15Amount5, sb1Input.SB1TAmount5,
                sb1Input.SB1TD1Amount5, sb1Input.SB1TD2Amount5, sb1Input.SB1TMAmount5, sb1Input.SB1TLAmount5, sb1Input.SB1TUAmount5);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label6);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name6, sb1Input.SB1Label6, sb1Input.SB1RelLabel6,
                        sb1Input.SB1Description6, sb1Input.SB1MathExpression6, sb1Input.SB1Date6,
                        sb1Input.SB1Type6, sb1Input.SB1MathType6, sb1Input.SB1BaseIO6, sb1Input.SB1MathOperator6, sb1Input.SB1MathSubType6,
                        sb1Input.SB11Unit6, sb1Input.SB12Unit6, sb1Input.SB13Unit6, sb1Input.SB14Unit6, sb1Input.SB15Unit6, sb1Input.SB1TUnit6,
                        sb1Input.SB1TD1Unit6, sb1Input.SB1TD2Unit6, sb1Input.SB1TMUnit6, sb1Input.SB1TLUnit6, sb1Input.SB1TUUnit6);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount6, sb1Input.SB12Amount6, sb1Input.SB13Amount6, sb1Input.SB14Amount6, sb1Input.SB15Amount6, sb1Input.SB1TAmount6,
                    sb1Input.SB1TD1Amount6, sb1Input.SB1TD2Amount6, sb1Input.SB1TMAmount6, sb1Input.SB1TLAmount6, sb1Input.SB1TUAmount6);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label7);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name7, sb1Input.SB1Label7, sb1Input.SB1RelLabel7,
                    sb1Input.SB1Description7, sb1Input.SB1MathExpression7, sb1Input.SB1Date7,
                        sb1Input.SB1Type7, sb1Input.SB1MathType7, sb1Input.SB1BaseIO7, sb1Input.SB1MathOperator7, sb1Input.SB1MathSubType7,
                        sb1Input.SB11Unit7, sb1Input.SB12Unit7, sb1Input.SB13Unit7, sb1Input.SB14Unit7, sb1Input.SB15Unit7, sb1Input.SB1TUnit7,
                        sb1Input.SB1TD1Unit7, sb1Input.SB1TD2Unit7, sb1Input.SB1TMUnit7, sb1Input.SB1TLUnit7, sb1Input.SB1TUUnit7);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount7, sb1Input.SB12Amount7, sb1Input.SB13Amount7, sb1Input.SB14Amount7, sb1Input.SB15Amount7, sb1Input.SB1TAmount7,
                    sb1Input.SB1TD1Amount7, sb1Input.SB1TD2Amount7, sb1Input.SB1TMAmount7, sb1Input.SB1TLAmount7, sb1Input.SB1TUAmount7);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label8);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name8, sb1Input.SB1Label8, sb1Input.SB1RelLabel8,
                    sb1Input.SB1Description8, sb1Input.SB1MathExpression8, sb1Input.SB1Date8,
                        sb1Input.SB1Type8, sb1Input.SB1MathType8, sb1Input.SB1BaseIO8, sb1Input.SB1MathOperator8, sb1Input.SB1MathSubType8,
                        sb1Input.SB11Unit8, sb1Input.SB12Unit8, sb1Input.SB13Unit8, sb1Input.SB14Unit8, sb1Input.SB15Unit8, sb1Input.SB1TUnit8,
                        sb1Input.SB1TD1Unit8, sb1Input.SB1TD2Unit8, sb1Input.SB1TMUnit8, sb1Input.SB1TLUnit8, sb1Input.SB1TUUnit8);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                sb1Input.SB11Amount8, sb1Input.SB12Amount8, sb1Input.SB13Amount8, sb1Input.SB14Amount8, sb1Input.SB15Amount8, sb1Input.SB1TAmount8,
                sb1Input.SB1TD1Amount8, sb1Input.SB1TD2Amount8, sb1Input.SB1TMAmount8, sb1Input.SB1TLAmount8, sb1Input.SB1TUAmount8);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label9);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name9, sb1Input.SB1Label9, sb1Input.SB1RelLabel9,
                    sb1Input.SB1Description9, sb1Input.SB1MathExpression9, sb1Input.SB1Date9,
                        sb1Input.SB1Type9, sb1Input.SB1MathType9, sb1Input.SB1BaseIO9, sb1Input.SB1MathOperator9, sb1Input.SB1MathSubType9,
                        sb1Input.SB11Unit9, sb1Input.SB12Unit9, sb1Input.SB13Unit9, sb1Input.SB14Unit9, sb1Input.SB15Unit9, sb1Input.SB1TUnit9,
                        sb1Input.SB1TD1Unit9, sb1Input.SB1TD2Unit9, sb1Input.SB1TMUnit9, sb1Input.SB1TLUnit9, sb1Input.SB1TUUnit9);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount9, sb1Input.SB12Amount9, sb1Input.SB13Amount9, sb1Input.SB14Amount9, sb1Input.SB15Amount9, sb1Input.SB1TAmount9,
                    sb1Input.SB1TD1Amount9, sb1Input.SB1TD2Amount9, sb1Input.SB1TMAmount9, sb1Input.SB1TLAmount9, sb1Input.SB1TUAmount9);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label10);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name10, sb1Input.SB1Label10, sb1Input.SB1RelLabel10,
                       sb1Input.SB1Description10, sb1Input.SB1MathExpression10, sb1Input.SB1Date10,
                        sb1Input.SB1Type10, sb1Input.SB1MathType10, sb1Input.SB1BaseIO10, sb1Input.SB1MathOperator10, sb1Input.SB1MathSubType10,
                        sb1Input.SB11Unit10, sb1Input.SB12Unit10, sb1Input.SB13Unit10, sb1Input.SB14Unit10, sb1Input.SB15Unit10, sb1Input.SB1TUnit10,
                        sb1Input.SB1TD1Unit10, sb1Input.SB1TD2Unit10, sb1Input.SB1TMUnit10, sb1Input.SB1TLUnit10, sb1Input.SB1TUUnit10);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount10, sb1Input.SB12Amount10, sb1Input.SB13Amount10, sb1Input.SB14Amount10, sb1Input.SB15Amount10, sb1Input.SB1TAmount10,
                    sb1Input.SB1TD1Amount10, sb1Input.SB1TD2Amount10, sb1Input.SB1TMAmount10, sb1Input.SB1TLAmount10, sb1Input.SB1TUAmount10);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label11);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name11, sb1Input.SB1Label11, sb1Input.SB1RelLabel11,
                        sb1Input.SB1Description11, sb1Input.SB1MathExpression11, sb1Input.SB1Date11,
                        sb1Input.SB1Type11, sb1Input.SB1MathType11, sb1Input.SB1BaseIO11, sb1Input.SB1MathOperator11, sb1Input.SB1MathSubType11,
                        sb1Input.SB11Unit11, sb1Input.SB12Unit11, sb1Input.SB13Unit11, sb1Input.SB14Unit11, sb1Input.SB15Unit11, sb1Input.SB1TUnit11,
                        sb1Input.SB1TD1Unit11, sb1Input.SB1TD2Unit11, sb1Input.SB1TMUnit11, sb1Input.SB1TLUnit11, sb1Input.SB1TUUnit11);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount11, sb1Input.SB12Amount11, sb1Input.SB13Amount11, sb1Input.SB14Amount11, sb1Input.SB15Amount11, sb1Input.SB1TAmount11,
                    sb1Input.SB1TD1Amount11, sb1Input.SB1TD2Amount11, sb1Input.SB1TMAmount11, sb1Input.SB1TLAmount11, sb1Input.SB1TUAmount11);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label12);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name12, sb1Input.SB1Label12, sb1Input.SB1RelLabel12,
                        sb1Input.SB1Description12, sb1Input.SB1MathExpression12, sb1Input.SB1Date12,
                        sb1Input.SB1Type12, sb1Input.SB1MathType12, sb1Input.SB1BaseIO12, sb1Input.SB1MathOperator12, sb1Input.SB1MathSubType12,
                        sb1Input.SB11Unit12, sb1Input.SB12Unit12, sb1Input.SB13Unit12, sb1Input.SB14Unit12, sb1Input.SB15Unit12, sb1Input.SB1TUnit12,
                        sb1Input.SB1TD1Unit12, sb1Input.SB1TD2Unit12, sb1Input.SB1TMUnit12, sb1Input.SB1TLUnit12, sb1Input.SB1TUUnit12);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount12, sb1Input.SB12Amount12, sb1Input.SB13Amount12, sb1Input.SB14Amount12, sb1Input.SB15Amount12, sb1Input.SB1TAmount12,
                    sb1Input.SB1TD1Amount12, sb1Input.SB1TD2Amount12, sb1Input.SB1TMAmount12, sb1Input.SB1TLAmount12, sb1Input.SB1TUAmount12);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label13);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name13, sb1Input.SB1Label13, sb1Input.SB1RelLabel13,
                        sb1Input.SB1Description13, sb1Input.SB1MathExpression13, sb1Input.SB1Date13,
                        sb1Input.SB1Type13, sb1Input.SB1MathType13, sb1Input.SB1BaseIO13, sb1Input.SB1MathOperator13, sb1Input.SB1MathSubType13,
                        sb1Input.SB11Unit13, sb1Input.SB12Unit13, sb1Input.SB13Unit13, sb1Input.SB14Unit13, sb1Input.SB15Unit13, sb1Input.SB1TUnit13,
                        sb1Input.SB1TD1Unit13, sb1Input.SB1TD2Unit13, sb1Input.SB1TMUnit13, sb1Input.SB1TLUnit13, sb1Input.SB1TUUnit13);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount13, sb1Input.SB12Amount13, sb1Input.SB13Amount13, sb1Input.SB14Amount13, sb1Input.SB15Amount13, sb1Input.SB1TAmount13,
                    sb1Input.SB1TD1Amount13, sb1Input.SB1TD2Amount13, sb1Input.SB1TMAmount13, sb1Input.SB1TLAmount13, sb1Input.SB1TUAmount13);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label14);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name14, sb1Input.SB1Label14, sb1Input.SB1RelLabel14,
                        sb1Input.SB1Description14, sb1Input.SB1MathExpression14, sb1Input.SB1Date14,
                        sb1Input.SB1Type14, sb1Input.SB1MathType14, sb1Input.SB1BaseIO14, sb1Input.SB1MathOperator14, sb1Input.SB1MathSubType14,
                        sb1Input.SB11Unit14, sb1Input.SB12Unit14, sb1Input.SB13Unit14, sb1Input.SB14Unit14, sb1Input.SB15Unit14, sb1Input.SB1TUnit14,
                        sb1Input.SB1TD1Unit14, sb1Input.SB1TD2Unit14, sb1Input.SB1TMUnit14, sb1Input.SB1TLUnit14, sb1Input.SB1TUUnit14);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount14, sb1Input.SB12Amount14, sb1Input.SB13Amount14, sb1Input.SB14Amount14, sb1Input.SB15Amount14, sb1Input.SB1TAmount14,
                    sb1Input.SB1TD1Amount14, sb1Input.SB1TD2Amount14, sb1Input.SB1TMAmount14, sb1Input.SB1TLAmount14, sb1Input.SB1TUAmount14);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label15);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name15, sb1Input.SB1Label15, sb1Input.SB1RelLabel15,
                        sb1Input.SB1Description15, sb1Input.SB1MathExpression15, sb1Input.SB1Date15,
                        sb1Input.SB1Type15, sb1Input.SB1MathType15, sb1Input.SB1BaseIO15, sb1Input.SB1MathOperator15, sb1Input.SB1MathSubType15,
                        sb1Input.SB11Unit15, sb1Input.SB12Unit15, sb1Input.SB13Unit15, sb1Input.SB14Unit15, sb1Input.SB15Unit15, sb1Input.SB1TUnit15,
                        sb1Input.SB1TD1Unit15, sb1Input.SB1TD2Unit15, sb1Input.SB1TMUnit15, sb1Input.SB1TLUnit15, sb1Input.SB1TUUnit15);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount15, sb1Input.SB12Amount15, sb1Input.SB13Amount15, sb1Input.SB14Amount15, sb1Input.SB15Amount15, sb1Input.SB1TAmount15,
                    sb1Input.SB1TD1Amount15, sb1Input.SB1TD2Amount15, sb1Input.SB1TMAmount15, sb1Input.SB1TLAmount15, sb1Input.SB1TUAmount15);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label16);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name16, sb1Input.SB1Label16, sb1Input.SB1RelLabel16,
                        sb1Input.SB1Description16, sb1Input.SB1MathExpression16, sb1Input.SB1Date16,
                        sb1Input.SB1Type16, sb1Input.SB1MathType16, sb1Input.SB1BaseIO16, sb1Input.SB1MathOperator16, sb1Input.SB1MathSubType16,
                        sb1Input.SB11Unit16, sb1Input.SB12Unit16, sb1Input.SB13Unit16, sb1Input.SB14Unit16, sb1Input.SB15Unit16, sb1Input.SB1TUnit16,
                        sb1Input.SB1TD1Unit16, sb1Input.SB1TD2Unit16, sb1Input.SB1TMUnit16, sb1Input.SB1TLUnit16, sb1Input.SB1TUUnit16);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount16, sb1Input.SB12Amount16, sb1Input.SB13Amount16, sb1Input.SB14Amount16, sb1Input.SB15Amount16, sb1Input.SB1TAmount16,
                    sb1Input.SB1TD1Amount16, sb1Input.SB1TD2Amount16, sb1Input.SB1TMAmount16, sb1Input.SB1TLAmount16, sb1Input.SB1TUAmount16);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label17);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name17, sb1Input.SB1Label17, sb1Input.SB1RelLabel17,
                        sb1Input.SB1Description17, sb1Input.SB1MathExpression17, sb1Input.SB1Date17,
                        sb1Input.SB1Type17, sb1Input.SB1MathType17, sb1Input.SB1BaseIO17, sb1Input.SB1MathOperator17, sb1Input.SB1MathSubType17,
                        sb1Input.SB11Unit17, sb1Input.SB12Unit17, sb1Input.SB13Unit17, sb1Input.SB14Unit17, sb1Input.SB15Unit17, sb1Input.SB1TUnit17,
                        sb1Input.SB1TD1Unit17, sb1Input.SB1TD2Unit17, sb1Input.SB1TMUnit17, sb1Input.SB1TLUnit17, sb1Input.SB1TUUnit17);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount17, sb1Input.SB12Amount17, sb1Input.SB13Amount17, sb1Input.SB14Amount17, sb1Input.SB15Amount17, sb1Input.SB1TAmount17,
                    sb1Input.SB1TD1Amount17, sb1Input.SB1TD2Amount17, sb1Input.SB1TMAmount17, sb1Input.SB1TLAmount17, sb1Input.SB1TUAmount17);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label18);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name18, sb1Input.SB1Label18, sb1Input.SB1RelLabel18,
                        sb1Input.SB1Description18, sb1Input.SB1MathExpression18, sb1Input.SB1Date18,
                        sb1Input.SB1Type18, sb1Input.SB1MathType18, sb1Input.SB1BaseIO18, sb1Input.SB1MathOperator18, sb1Input.SB1MathSubType18,
                        sb1Input.SB11Unit18, sb1Input.SB12Unit18, sb1Input.SB13Unit18, sb1Input.SB14Unit18, sb1Input.SB15Unit18, sb1Input.SB1TUnit18,
                        sb1Input.SB1TD1Unit18, sb1Input.SB1TD2Unit18, sb1Input.SB1TMUnit18, sb1Input.SB1TLUnit18, sb1Input.SB1TUUnit18);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount18, sb1Input.SB12Amount18, sb1Input.SB13Amount18, sb1Input.SB14Amount18, sb1Input.SB15Amount18, sb1Input.SB1TAmount18,
                    sb1Input.SB1TD1Amount18, sb1Input.SB1TD2Amount18, sb1Input.SB1TMAmount18, sb1Input.SB1TLAmount18, sb1Input.SB1TUAmount18);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label19);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name19, sb1Input.SB1Label19, sb1Input.SB1RelLabel19,
                        sb1Input.SB1Description19, sb1Input.SB1MathExpression19, sb1Input.SB1Date19,
                        sb1Input.SB1Type19, sb1Input.SB1MathType19, sb1Input.SB1BaseIO19, sb1Input.SB1MathOperator19, sb1Input.SB1MathSubType19,
                        sb1Input.SB11Unit19, sb1Input.SB12Unit19, sb1Input.SB13Unit19, sb1Input.SB14Unit19, sb1Input.SB15Unit19, sb1Input.SB1TUnit19,
                        sb1Input.SB1TD1Unit19, sb1Input.SB1TD2Unit19, sb1Input.SB1TMUnit19, sb1Input.SB1TLUnit19, sb1Input.SB1TUUnit19);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount19, sb1Input.SB12Amount19, sb1Input.SB13Amount19, sb1Input.SB14Amount19, sb1Input.SB15Amount19, sb1Input.SB1TAmount19,
                    sb1Input.SB1TD1Amount19, sb1Input.SB1TD2Amount19, sb1Input.SB1TMAmount19, sb1Input.SB1TLAmount19, sb1Input.SB1TUAmount19);
            }
            i = baseStat.GetStockIndex(sb1Input.SB1Label20);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    baseStat.AddSubStockToTotalStock(i, sb1Input.SB1Name20, sb1Input.SB1Label20, sb1Input.SB1RelLabel20,
                        sb1Input.SB1Description20, sb1Input.SB1MathExpression20, sb1Input.SB1Date20,
                        sb1Input.SB1Type20, sb1Input.SB1MathType20, sb1Input.SB1BaseIO20, sb1Input.SB1MathOperator20, sb1Input.SB1MathSubType20,
                        sb1Input.SB11Unit20, sb1Input.SB12Unit20, sb1Input.SB13Unit20, sb1Input.SB14Unit20, sb1Input.SB15Unit20, sb1Input.SB1TUnit20,
                        sb1Input.SB1TD1Unit20, sb1Input.SB1TD2Unit20, sb1Input.SB1TMUnit20, sb1Input.SB1TLUnit20, sb1Input.SB1TUUnit20);
                }
                baseStat.AddSubStockToTotalStock(i, 1,
                    sb1Input.SB11Amount20, sb1Input.SB12Amount20, sb1Input.SB13Amount20, sb1Input.SB14Amount20, sb1Input.SB15Amount20, sb1Input.SB1TAmount20,
                    sb1Input.SB1TD1Amount20, sb1Input.SB1TD2Amount20, sb1Input.SB1TMAmount20, sb1Input.SB1TLAmount20, sb1Input.SB1TUAmount20);
            }
        }
        
    }
}

