using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The CEStatistic class extends the CostBenefitStatistic01() 
    ///             class and is used by cost effectiveness analyzers.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. This class was originally designed to filter the attributes 
    ///             needed by an analyzer. It may be used in future releases to 
    ///             cut down on file sizes (i.e. better performance).
    /// </summary>      
    public class CEStatistic : CostBenefitStatistic01
    {
        //calls the base-class version, and initializes the base class properties.
        public CEStatistic() 
        { 
        }
        //copy constructor
        public CEStatistic(CEStatistic calculator)
            : base(calculator)
        {
            CopyCEStatisticProperties(calculator);
        }
        //additional properties needed by ce stat
        public int Outputs { get; set; }
        public const string OUTPUTS = "Outputs";
        
        public int Inputs { get; set; }
        public const string INPUTS = "Inputs";

        //substrings
        //part of data definition for cost per unit output
        public const string AMOUNT = "AMOUNT";
        //part of data definition for cost per unit revenue
        public const string REVENUE = "REVENUE";
        //part of data definition for cost per unit benefit
        public const string BENEFIT = "BENEFIT";

        public void InitCEStatisticProperties()
        {
            //avoid null references
            this.Outputs = 0;
            this.Inputs = 0;
        }
        public void CopyCEStatisticProperties(
            CEStatistic calculator)
        {
            this.Outputs = calculator.Outputs;
            this.Inputs = calculator.Inputs;
        }
        //set the class properties using the XElement
        public void SetCEGeneralProperties(XElement calculator)
        {
            this.Outputs = CalculatorHelpers.GetAttributeInt(calculator,
                OUTPUTS);
            this.Inputs = CalculatorHelpers.GetAttributeInt(calculator,
                INPUTS);
        }
        //attname and attvalue generally passed in from a reader
        public void SetCEGeneralProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case OUTPUTS:
                    this.Outputs
                        = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case INPUTS:
                    this.Inputs
                        = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                default:
                    break;
            }
        }
        //the attNameExtension is used with attribute indexing _0_1
        public void SetCEGeneralAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeInt(calculator,
                string.Concat(OUTPUTS, attNameExtension),
                this.Outputs);
            CalculatorHelpers.SetAttributeInt(calculator,
               string.Concat(INPUTS, attNameExtension),
               this.Inputs);
        }
        public void SetCEGeneralAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(OUTPUTS, attNameExtension),
               this.Outputs.ToString());
            writer.WriteAttributeString(
                string.Concat(INPUTS, attNameExtension),
               this.Inputs.ToString());
        }
        public static bool NeedsCEStatistic(string attName)
        {
            //filter the cost effectiveness properties 
            //that will be converted to xattributes in the stats document
            bool bNeedsCostStatistics = false;
            switch (attName)
            {
                case TRAmount:
                    bNeedsCostStatistics = true;
                    break;
                case TRAmount_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TRAmount_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMR:
                    bNeedsCostStatistics = true;
                    break;
                case TAMR_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMR_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMR_N:
                    bNeedsCostStatistics = true;
                    break;
                case TAMRINCENT:
                    bNeedsCostStatistics = true;
                    break;
                case TAMRINCENT_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMRINCENT_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMOC:
                    bNeedsCostStatistics = true;
                    break;
                case TAMOC_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMOC_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMOC_N:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMCAP:
                    bNeedsCostStatistics = true;
                    break;
                case TAMCAP_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMCAP_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMINCENT:
                    bNeedsCostStatistics = true;
                    break;
                case TAMINCENT_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMINCENT_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMOC_NET:
                    bNeedsCostStatistics = true;
                    break;
                case TAMOC_NET_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMOC_NET_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_NET:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_NET_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_NET_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_NET2:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_NET2_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMAOH_NET2_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMCAP_NET:
                    bNeedsCostStatistics = true;
                    break;
                case TAMCAP_NET_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMCAP_NET_SD:
                    bNeedsCostStatistics = true;
                    break;
                case TAMINCENT_NET:
                    bNeedsCostStatistics = true;
                    break;
                case TAMINCENT_NET_MEAN:
                    bNeedsCostStatistics = true;
                    break;
                case TAMINCENT_NET_SD:
                    bNeedsCostStatistics = true;
                    break;
                default:
                    break;
            }
            return bNeedsCostStatistics;
        }
        
        
    }
}
