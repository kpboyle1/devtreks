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
    ///Purpose:		The GeneralCapital1Stock class extends the GeneralCapital1Input 
    ///             class and is used by machinery resource stock calculators 
    ///             and analyzers to set totals. Basic capital stock statistical 
    ///             analyzers derive from this class to generate additional statistics.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class GeneralCapital1Stock : Machinery1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public GeneralCapital1Stock()
            : base()
        {
            //base input
            InitCalculatorProperties();
            InitTotalBenefitsProperties();
            InitTotalCostsProperties();
            //genera capital
            InitTotalGeneralCapital1StockProperties();
        }
        //copy constructor
        public GeneralCapital1Stock(GeneralCapital1Stock calculator)
            : base(calculator)
        {
            CopyTotalGeneralCapital1StockProperties(calculator);
        }

        //calculator properties
        //irrigation power stock collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<GeneralCapital1Input>> GenCapitalStocks = null;
        //totals
        public double TotalEnergyUseHr { get; set; }
        public double TotalEnergyEffTypical { get; set; }
       
        //totals for constants
        public double TotalRandMPercent { get; set; }
        
        //totals
        private const string TEnergyUseHr = "TEnergyUseHr";
        public const string TEnergyEffTypical = "TEnergyEffTypical";
        
        //totals for constants (keep them here because this class will be a base class for stats analysis)
        public const string TRandMPercent = "TRandMPercent";
        
        public virtual void InitTotalGeneralCapital1StockProperties()
        {
            this.TotalEnergyUseHr = 0;
            this.TotalEnergyEffTypical = 0;
            this.TotalRandMPercent = 0;
        }
        public virtual void CopyTotalGeneralCapital1StockProperties(
            GeneralCapital1Stock calculator)
        {
            this.TotalEnergyUseHr = calculator.TotalEnergyUseHr;
            this.TotalEnergyEffTypical = calculator.TotalEnergyEffTypical;
            this.TotalRandMPercent = calculator.TotalRandMPercent;
        }
        public virtual void SetTotalGeneralCapital1StockProperties(XElement calculator)
        {
            this.TotalEnergyEffTypical = CalculatorHelpers.GetAttributeDouble(calculator,
                TEnergyEffTypical);
            this.TotalEnergyUseHr = CalculatorHelpers.GetAttributeDouble(calculator,
                TEnergyUseHr);
            this.TotalRandMPercent = CalculatorHelpers.GetAttributeDouble(calculator,
                TRandMPercent);
        }
        public virtual void SetTotalGeneralCapital1StockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TEnergyEffTypical:
                    this.TotalEnergyEffTypical = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEnergyUseHr:
                    this.TotalEnergyUseHr = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRandMPercent:
                    this.TotalRandMPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalGeneralCapital1StockProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TEnergyEffTypical:
                    sPropertyValue = this.TotalEnergyEffTypical.ToString();
                    break;
                case TEnergyUseHr:
                    sPropertyValue = this.TotalEnergyUseHr.ToString();
                    break;
                case TRandMPercent:
                    sPropertyValue = this.TotalRandMPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalGeneralCapital1StockAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TEnergyEffTypical, attNameExtension),
                this.TotalEnergyEffTypical);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TEnergyUseHr, attNameExtension),
                this.TotalEnergyUseHr);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRandMPercent, attNameExtension),
                this.TotalRandMPercent);
        }
        public virtual void SetTotalGeneralCapital1StockAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TEnergyEffTypical, attNameExtension),
                this.TotalEnergyEffTypical.ToString());
            writer.WriteAttributeString(
                string.Concat(TEnergyUseHr, attNameExtension),
                this.TotalEnergyUseHr.ToString());
            writer.WriteAttributeString(
                string.Concat(TRandMPercent, attNameExtension),
                this.TotalRandMPercent.ToString());
        }
    }
    public static class GenCapitalStocks1Extensions
    {
        //add a GeneralCapital1Input to the baseStat.GenCapitalStocks dictionary
        public static bool AddGeneralCapital1StocksToDictionary(
            this GeneralCapital1Stock baseStat,
            int filePosition, int nodePosition, GeneralCapital1Input calculator)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.GenCapitalStocks == null)
                baseStat.GenCapitalStocks
                = new Dictionary<int, List<GeneralCapital1Input>>();
            if (baseStat.GenCapitalStocks.ContainsKey(filePosition))
            {
                if (baseStat.GenCapitalStocks[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.GenCapitalStocks[filePosition].Count <= i)
                        {
                            baseStat.GenCapitalStocks[filePosition]
                                .Add(new GeneralCapital1Input());
                        }
                    }
                    baseStat.GenCapitalStocks[filePosition][nodePosition]
                        = calculator;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<GeneralCapital1Input> baseStats
                    = new List<GeneralCapital1Input>();
                KeyValuePair<int, List<GeneralCapital1Input>> newStat
                    = new KeyValuePair<int, List<GeneralCapital1Input>>(
                        filePosition, baseStats);
                baseStat.GenCapitalStocks.Add(newStat);
                bIsAdded = AddGeneralCapital1StocksToDictionary(baseStat,
                    filePosition, nodePosition, calculator);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this GeneralCapital1Stock baseStat,
            int filePosition, GeneralCapital1Input calculator)
        {
            int iNodeCount = 0;
            if (baseStat.GenCapitalStocks == null)
                return iNodeCount;
            if (baseStat.GenCapitalStocks.ContainsKey(filePosition))
            {
                if (baseStat.GenCapitalStocks[filePosition] != null)
                {
                    iNodeCount = baseStat.GenCapitalStocks[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}
