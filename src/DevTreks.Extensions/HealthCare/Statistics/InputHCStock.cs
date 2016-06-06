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
    ///Purpose:		The InputHCStock class extends the HealthCareCost1Calculator() 
    ///             class and is used by health care cost calculators and analyzers 
    ///             to set totals and basic health care cost statistics. Basic 
    ///             health care cost statistical objects derive from this class 
    ///             to support further statistical analysis.
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    public class InputHCStock : HealthCareCost1Calculator
    {
        //calls the base-class version, and initializes the base class properties.
        public InputHCStock()
            : base()
        {
            //base input
            InitCalculatorProperties();
            InitTotalBenefitsProperties();
            InitTotalCostsProperties();
            //health care cost object
            InitTotalInputHCStockProperties();
        }
        //copy constructor
        public InputHCStock(InputHCStock calculator)
            : base(calculator)
        {
            CopyTotalInputHCStockProperties(calculator);
        }

        //calculator properties
        //healthcare cost collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<HealthCareCost1Calculator>> HealthCareCost1s = null;

        public double TotalBasePrice { get; set; }
        //AdjustedPrice = BasePriceAdjustment * BasePrice
        public double TotalBasePriceAdjustment { get; set; }
        public double TotalAdjustedPrice { get; set; }
        //insurance company negotiated fee
        public double TotalContractedPrice { get; set; }

        //health care provider list price
        public double TotalListPrice { get; set; }
        //health care provider market price
        public double TotalMarketPrice { get; set; }
        //health care cost of production price
        public double TotalProductionCostPrice { get; set; }
        //annual premium insurance coverage paid by recipient
        public double TotalAnnualPremium1 { get; set; }
        //annual premium for insurance coverage paid by other than recipient
        public double TotalAnnualPremium2 { get; set; }
        public double TotalAssignedPremiumCost { get; set; }
        //first additional input cost associated with this input (i.e. recipient time oppportunity cost)
        public double TotalAdditionalPrice1 { get; set; }
        public double TotalAdditionalAmount1 { get; set; }
        public double TotalAdditionalCost1 { get; set; }
        //second additional input cost associated with this input (i.e. recipient clothes, furniture, transportation cost)
        public double TotalAdditionalPrice2 { get; set; }
        public double TotalAdditionalAmount2 { get; set; }
        public double TotalAdditionalCost2 { get; set; }
        //health care provider payment
        public double TotalHealthCareProviderCost { get; set; }
        //additional costs (add1 and add2 totals)
        public double TotalAdditionalCost { get; set; }
        //amount of information received to make informed decision
        public double TotalKnowledgeTransferRating { get; set; }
        //degree to which this input or service would have been chosen if not faced with constraints
        public double TotalConstrainedChoiceRating { get; set; }

        //health care received payments
        public double TotalCoPay1Amount { get; set; }
        public double TotalCoPay1Rate { get; set; }
        public double TotalCoPay2Amount { get; set; }
        public double TotalCoPay2Rate { get; set; }
        public double TotalIncentive1Amount { get; set; }
        public double TotalIncentive1Rate { get; set; }
        public double TotalIncentive2Amount { get; set; }
        public double TotalIncentive2Rate { get; set; }
        public double TotalInsuranceProviderCost { get; set; }
        public double TotalIncentivesCost { get; set; }
        public double TotalReceiverCost { get; set; }
        public double TotalDiagnosisQualityRating { get; set; }
        public double TotalTreatmentQualityRating { get; set; }
        public double TotalTreatmentBenefitRating { get; set; }
        public double TotalTreatmentCostRating { get; set; }
        public double TotalInsuranceCoverageRating { get; set; }
        public double TotalCostRating { get; set; }

        private const string TBasePrice = "TBasePrice";
        private const string TBasePriceAdjustment = "TBasePriceAdjustment";
        private const string TAdjustedPrice = "TAdjustedPrice";
        private const string TContractedPrice = "TContractedPrice";

        private const string TListPrice = "TListPrice";
        private const string TMarketPrice = "TMarketPrice";
        private const string TProductionCostPrice = "TProductionCostPrice";
        private const string TAnnualPremium1 = "TAnnualPremium1";
        private const string TAnnualPremium2 = "TAnnualPremium2";
        private const string TAssignedPremiumCost = "TAssignedPremiumCost";
        private const string TAdditionalPrice1 = "TAdditionalPrice1";
        private const string TAdditionalAmount1 = "TAdditionalAmount1";
        private const string TAdditionalCost1 = "TAdditionalCost1";
        private const string TAdditionalPrice2 = "TAdditionalPrice2";
        private const string TAdditionalAmount2 = "TAdditionalAmount2";
        private const string TAdditionalCost2 = "TAdditionalCost2";
        private const string THealthCareProviderCost = "THealthCareProviderCost";
        private const string TAdditionalCost = "TAdditionalCost";
        private const string TKnowledgeTransferRating = "TKnowledgeTransferRating";
        private const string TConstrainedChoiceRating = "TConstrainedChoiceRating";

        private const string TCoPay1Amount = "TCoPay1Amount";
        private const string TCoPay1Rate = "TCoPay1Rate";
        private const string TCoPay2Amount = "TCoPay2Amount";
        private const string TCoPay2Rate = "TCoPay2Rate";
        private const string TIncentive1Amount = "TIncentive1Amount";
        private const string TIncentive1Rate = "TIncentive1Rate";
        private const string TIncentive2Amount = "TIncentive2Amount";
        private const string TIncentive2Rate = "TIncentive2Rate";
        private const string TInsuranceProviderCost = "TInsuranceProviderCost";
        private const string TIncentivesCost = "TIncentivesCost";
        private const string TReceiverCost = "TReceiverCost";
        private const string TDiagnosisQualityRating = "TDiagnosisQualityRating";
        private const string TTreatmentQualityRating = "TTreatmentQualityRating";
        private const string TTreatmentBenefitRating = "TTreatmentBenefitRating";
        private const string TTreatmentCostRating = "TTreatmentCostRating";
        private const string TInsuranceCoverageRating = "TInsuranceCoverageRating";
        private const string TCostRating = "TCostRating";

        public virtual void InitTotalInputHCStockProperties()
        {
            //avoid null references to properties
            this.TotalBasePrice = 0;
            //AdjustedPrice = BasePriceAdjustment * BasePrice
            this.TotalBasePriceAdjustment = 0;
            this.TotalAdjustedPrice = 0;
            //insurance company negotiated fee
            this.TotalContractedPrice = 0;

            this.TotalListPrice = 0;
            this.TotalMarketPrice = 0;
            this.TotalProductionCostPrice = 0;
            this.TotalAnnualPremium1 = 0;
            this.TotalAnnualPremium2 = 0;
            this.TotalAssignedPremiumCost = 0;
            this.TotalAdditionalPrice1 = 0;
            this.TotalAdditionalAmount1 = 0;
            this.TotalAdditionalCost1 = 0;
            this.TotalAdditionalPrice2 = 0;
            this.TotalAdditionalAmount2 = 0;
            this.TotalAdditionalCost2 = 0;
            this.TotalHealthCareProviderCost = 0;
            this.TotalAdditionalCost = 0;
            this.TotalKnowledgeTransferRating = 0;
            this.TotalConstrainedChoiceRating = 0;

            //health care received payments
            this.TotalCoPay1Amount = 0;
            this.TotalCoPay1Rate = 0;
            this.TotalCoPay2Amount = 0;
            this.TotalCoPay2Rate = 0;
            this.TotalIncentive1Amount = 0;
            this.TotalIncentive1Rate = 0;
            this.TotalIncentive2Amount = 0;
            this.TotalIncentive2Rate = 0;
            this.TotalInsuranceProviderCost = 0;
            this.TotalIncentivesCost = 0;
            this.TotalReceiverCost = 0;
            this.TotalDiagnosisQualityRating = 0;
            this.TotalTreatmentQualityRating = 0;
            this.TotalTreatmentBenefitRating = 0;
            this.TotalTreatmentCostRating = 0;
            this.TotalInsuranceCoverageRating = 0;
            this.TotalCostRating = 0;
        }
        public virtual void CopyTotalInputHCStockProperties(
            InputHCStock calculator)
        {
            this.TotalBasePrice = calculator.TotalBasePrice;
            this.TotalBasePriceAdjustment = calculator.TotalBasePriceAdjustment;
            this.TotalAdjustedPrice = calculator.TotalAdjustedPrice;
            this.TotalContractedPrice = calculator.TotalContractedPrice;

            this.TotalListPrice = calculator.TotalListPrice;
            this.TotalMarketPrice = calculator.TotalMarketPrice;
            this.TotalProductionCostPrice = calculator.TotalProductionCostPrice;
            this.TotalAnnualPremium1 = calculator.TotalAnnualPremium1;
            this.TotalAnnualPremium2 = calculator.TotalAnnualPremium2;
            this.TotalAssignedPremiumCost = calculator.TotalAssignedPremiumCost;
            this.TotalAdditionalPrice1 = calculator.TotalAdditionalPrice1;
            this.TotalAdditionalAmount1 = calculator.TotalAdditionalAmount1;
            this.TotalAdditionalCost1 = calculator.TotalAdditionalCost1;
            this.TotalAdditionalPrice2 = calculator.TotalAdditionalPrice2;
            this.TotalAdditionalAmount2 = calculator.TotalAdditionalAmount2;
            this.TotalAdditionalCost2 = calculator.TotalAdditionalCost2;
            this.TotalHealthCareProviderCost = calculator.TotalHealthCareProviderCost;
            this.TotalAdditionalCost = calculator.TotalAdditionalCost;
            this.TotalKnowledgeTransferRating = calculator.TotalKnowledgeTransferRating;
            this.TotalConstrainedChoiceRating = calculator.TotalConstrainedChoiceRating;

            this.TotalCoPay1Amount = calculator.TotalCoPay1Amount;
            this.TotalCoPay1Rate = calculator.TotalCoPay1Rate;
            this.TotalCoPay2Amount = calculator.TotalCoPay2Amount;
            this.TotalCoPay2Rate = calculator.TotalCoPay2Rate;
            this.TotalIncentive1Amount = calculator.TotalIncentive1Amount;
            this.TotalIncentive1Rate = calculator.TotalIncentive1Rate;
            this.TotalIncentive2Amount = calculator.TotalIncentive2Amount;
            this.TotalIncentive2Rate = calculator.TotalIncentive2Rate;
            this.TotalInsuranceProviderCost = calculator.TotalInsuranceProviderCost;
            this.TotalIncentivesCost = calculator.TotalIncentivesCost;
            this.TotalReceiverCost = calculator.TotalReceiverCost;
            this.TotalDiagnosisQualityRating = calculator.TotalDiagnosisQualityRating;
            this.TotalTreatmentQualityRating = calculator.TotalTreatmentQualityRating;
            this.TotalTreatmentBenefitRating = calculator.TotalTreatmentBenefitRating;
            this.TotalTreatmentCostRating = calculator.TotalTreatmentCostRating;
            this.TotalInsuranceCoverageRating = calculator.TotalInsuranceCoverageRating;
            this.TotalCostRating = calculator.TotalCostRating;
        }
        //set the class properties using the XElement
        public virtual void SetTotalInputHCStockProperties(XElement currentCalculationsElement)
        {
            this.TotalBasePrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TBasePrice);
            this.TotalBasePriceAdjustment = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TBasePriceAdjustment);
            this.TotalAdjustedPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdjustedPrice);
            this.TotalContractedPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TContractedPrice);

            this.TotalListPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TListPrice);
            this.TotalMarketPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TMarketPrice);
            this.TotalProductionCostPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TProductionCostPrice);
            this.TotalAnnualPremium1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAnnualPremium1);
            this.TotalAnnualPremium2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAnnualPremium2);
            this.TotalAssignedPremiumCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAssignedPremiumCost);
            this.TotalAdditionalPrice1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdditionalPrice1);
            this.TotalAdditionalAmount1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdditionalAmount1);
            this.TotalAdditionalCost1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdditionalCost1);
            this.TotalAdditionalPrice2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdditionalPrice2);
            this.TotalAdditionalAmount2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdditionalAmount2);
            this.TotalAdditionalCost2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdditionalCost2);
            this.TotalHealthCareProviderCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, THealthCareProviderCost);
            this.TotalAdditionalCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdditionalCost);
            this.TotalKnowledgeTransferRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TKnowledgeTransferRating);
            this.TotalConstrainedChoiceRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TConstrainedChoiceRating);
            
            this.TotalCoPay1Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCoPay1Amount);
            this.TotalCoPay1Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCoPay1Rate);
            this.TotalCoPay2Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCoPay2Amount);
            this.TotalCoPay2Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCoPay2Rate);
            this.TotalIncentive1Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIncentive1Amount);
            this.TotalIncentive1Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIncentive1Rate);
            this.TotalIncentive2Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIncentive2Amount);
            this.TotalIncentive2Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIncentive2Rate);
            this.TotalInsuranceProviderCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TInsuranceProviderCost);
            this.TotalIncentivesCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIncentivesCost);
            this.TotalReceiverCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TReceiverCost);
            this.TotalDiagnosisQualityRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TDiagnosisQualityRating);
            this.TotalTreatmentQualityRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTreatmentQualityRating);
            this.TotalTreatmentBenefitRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTreatmentBenefitRating);
            this.TotalTreatmentCostRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTreatmentCostRating);
            this.TotalInsuranceCoverageRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TInsuranceCoverageRating);
            this.TotalCostRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCostRating);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalInputHCStockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TBasePrice:
                    this.TotalBasePrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBasePriceAdjustment:
                    this.TotalBasePriceAdjustment = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdjustedPrice:
                    this.TotalAdjustedPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TContractedPrice:
                    this.TotalContractedPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TListPrice:
                    this.TotalListPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TMarketPrice:
                    this.TotalMarketPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TProductionCostPrice:
                    this.TotalProductionCostPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAnnualPremium1:
                    this.TotalAnnualPremium1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAssignedPremiumCost:
                    this.TotalAssignedPremiumCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdditionalPrice1:
                    this.TotalAdditionalPrice1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdditionalAmount1:
                    this.TotalAdditionalAmount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdditionalCost1:
                    this.TotalAdditionalCost1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdditionalPrice2:
                    this.TotalAdditionalPrice2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdditionalAmount2:
                    this.TotalAdditionalAmount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdditionalCost2:
                    this.TotalAdditionalCost2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case THealthCareProviderCost:
                    this.TotalHealthCareProviderCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdditionalCost:
                    this.TotalAdditionalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TKnowledgeTransferRating:
                    this.TotalKnowledgeTransferRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TConstrainedChoiceRating:
                    this.TotalConstrainedChoiceRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCoPay1Amount:
                    this.TotalCoPay1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCoPay1Rate:
                    this.TotalCoPay1Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCoPay2Amount:
                    this.TotalCoPay2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCoPay2Rate:
                    this.TotalCoPay2Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIncentive1Amount:
                    this.TotalIncentive1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIncentive2Amount:
                    this.TotalIncentive2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIncentive1Rate:
                    this.TotalIncentive1Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIncentive2Rate:
                    this.TotalIncentive2Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TInsuranceProviderCost:
                    this.TotalInsuranceProviderCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIncentivesCost:
                    this.TotalIncentivesCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TReceiverCost:
                    this.TotalReceiverCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TDiagnosisQualityRating:
                    this.TotalDiagnosisQualityRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTreatmentQualityRating:
                    this.TotalTreatmentQualityRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTreatmentBenefitRating:
                    this.TotalTreatmentBenefitRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTreatmentCostRating:
                    this.TotalTreatmentCostRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TInsuranceCoverageRating:
                    this.TotalInsuranceCoverageRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCostRating:
                    this.TotalCostRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetTotalInputHCStockAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TBasePrice, attNameExtension), this.TotalBasePrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TBasePriceAdjustment, attNameExtension), this.TotalBasePriceAdjustment);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAdjustedPrice, attNameExtension), this.TotalAdjustedPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TContractedPrice, attNameExtension), this.TotalContractedPrice);
            
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TListPrice, attNameExtension), this.TotalListPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TMarketPrice, attNameExtension), this.TotalMarketPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TProductionCostPrice, attNameExtension), this.TotalProductionCostPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TAnnualPremium1, attNameExtension), this.TotalAnnualPremium1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAnnualPremium2, attNameExtension), this.TotalAnnualPremium2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAssignedPremiumCost, attNameExtension), this.TotalAssignedPremiumCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAdditionalPrice1, attNameExtension), this.TotalAdditionalPrice1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TAdditionalAmount1, attNameExtension), this.TotalAdditionalAmount1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAdditionalCost1, attNameExtension), this.TotalAdditionalCost1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAdditionalPrice2, attNameExtension), this.TotalAdditionalPrice2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TAdditionalAmount2, attNameExtension), this.TotalAdditionalAmount2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAdditionalCost2, attNameExtension), this.TotalAdditionalCost2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(THealthCareProviderCost, attNameExtension), this.TotalHealthCareProviderCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TAdditionalCost, attNameExtension), this.TotalAdditionalCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TKnowledgeTransferRating, attNameExtension), this.TotalKnowledgeTransferRating);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TConstrainedChoiceRating, attNameExtension), this.TotalConstrainedChoiceRating);

            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCoPay1Amount, attNameExtension), this.TotalCoPay1Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCoPay1Rate, attNameExtension), this.TotalCoPay1Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCoPay2Amount, attNameExtension), this.TotalCoPay2Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCoPay2Rate, attNameExtension), this.TotalCoPay2Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TIncentive1Amount, attNameExtension), this.TotalIncentive1Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TIncentive2Amount, attNameExtension), this.TotalIncentive2Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TIncentive1Rate, attNameExtension), this.TotalIncentive1Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TIncentive2Rate, attNameExtension), this.TotalIncentive2Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TInsuranceProviderCost, attNameExtension), this.TotalInsuranceProviderCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TIncentivesCost, attNameExtension), this.TotalIncentivesCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TReceiverCost, attNameExtension), this.TotalReceiverCost);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TDiagnosisQualityRating, attNameExtension), this.TotalDiagnosisQualityRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TTreatmentQualityRating, attNameExtension), this.TotalTreatmentQualityRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TTreatmentBenefitRating, attNameExtension), this.TotalTreatmentBenefitRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TTreatmentCostRating, attNameExtension), this.TotalTreatmentCostRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TInsuranceCoverageRating, attNameExtension), this.TotalInsuranceCoverageRating);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TCostRating, attNameExtension), this.TotalCostRating);
        }
        public virtual void SetTotalInputHCStockAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(TBasePrice, attNameExtension), this.TotalBasePrice.ToString());
            writer.WriteAttributeString(string.Concat(TBasePriceAdjustment, attNameExtension), this.TotalBasePriceAdjustment.ToString());
            writer.WriteAttributeString(string.Concat(TAdjustedPrice, attNameExtension), this.TotalAdjustedPrice.ToString());
            writer.WriteAttributeString(string.Concat(TContractedPrice, attNameExtension), this.TotalContractedPrice.ToString());

            writer.WriteAttributeString(string.Concat(TListPrice, attNameExtension), this.TotalListPrice.ToString());
            writer.WriteAttributeString(string.Concat(TMarketPrice, attNameExtension), this.TotalMarketPrice.ToString());
            writer.WriteAttributeString(string.Concat(TProductionCostPrice, attNameExtension), this.TotalProductionCostPrice.ToString());
            writer.WriteAttributeString(string.Concat(TAnnualPremium1, attNameExtension), this.TotalAnnualPremium1.ToString());
            writer.WriteAttributeString(string.Concat(TAnnualPremium2, attNameExtension), this.TotalAnnualPremium2.ToString());
            writer.WriteAttributeString(string.Concat(TAssignedPremiumCost, attNameExtension), this.TotalAssignedPremiumCost.ToString());
            writer.WriteAttributeString(string.Concat(TAdditionalPrice1, attNameExtension), this.TotalAdditionalPrice1.ToString());
            writer.WriteAttributeString(string.Concat(TAdditionalAmount1, attNameExtension), this.TotalAdditionalAmount1.ToString());
            writer.WriteAttributeString(string.Concat(TAdditionalCost1, attNameExtension), this.TotalAdditionalCost1.ToString());
            writer.WriteAttributeString(string.Concat(TAdditionalPrice2, attNameExtension), this.TotalAdditionalPrice2.ToString());
            writer.WriteAttributeString(string.Concat(TAdditionalAmount2, attNameExtension), this.TotalAdditionalAmount2.ToString());
            writer.WriteAttributeString(string.Concat(TAdditionalCost2, attNameExtension), this.TotalAdditionalCost2.ToString());
            writer.WriteAttributeString(string.Concat(THealthCareProviderCost, attNameExtension), this.TotalHealthCareProviderCost.ToString());
            writer.WriteAttributeString(string.Concat(TAdditionalCost, attNameExtension), this.TotalAdditionalCost.ToString());
            writer.WriteAttributeString(string.Concat(TKnowledgeTransferRating, attNameExtension), this.TotalKnowledgeTransferRating.ToString());
            writer.WriteAttributeString(string.Concat(TConstrainedChoiceRating, attNameExtension), this.TotalConstrainedChoiceRating.ToString());
            
            writer.WriteAttributeString(string.Concat(TCoPay1Amount, attNameExtension), this.TotalCoPay1Amount.ToString());
            writer.WriteAttributeString(string.Concat(TCoPay1Rate, attNameExtension), this.TotalCoPay1Rate.ToString());
            writer.WriteAttributeString(string.Concat(TCoPay2Amount, attNameExtension), this.TotalCoPay2Amount.ToString());
            writer.WriteAttributeString(string.Concat(TCoPay2Rate, attNameExtension), this.TotalCoPay2Rate.ToString());
            writer.WriteAttributeString(string.Concat(TIncentive1Amount, attNameExtension), this.TotalIncentive1Amount.ToString());
            writer.WriteAttributeString(string.Concat(TIncentive2Amount, attNameExtension), this.TotalIncentive2Amount.ToString());
            writer.WriteAttributeString(string.Concat(TIncentive1Rate, attNameExtension), this.TotalIncentive1Rate.ToString());
            writer.WriteAttributeString(string.Concat(TIncentive2Rate, attNameExtension), this.TotalIncentive2Rate.ToString());
            writer.WriteAttributeString(string.Concat(TInsuranceProviderCost, attNameExtension), this.TotalInsuranceProviderCost.ToString());
            writer.WriteAttributeString(string.Concat(TIncentivesCost, attNameExtension), this.TotalIncentivesCost.ToString());
            writer.WriteAttributeString(string.Concat(TReceiverCost, attNameExtension), this.TotalReceiverCost.ToString());
            writer.WriteAttributeString(string.Concat(TDiagnosisQualityRating, attNameExtension), this.TotalDiagnosisQualityRating.ToString());
            writer.WriteAttributeString(string.Concat(TTreatmentQualityRating, attNameExtension), this.TotalTreatmentQualityRating.ToString());
            writer.WriteAttributeString(string.Concat(TTreatmentBenefitRating, attNameExtension), this.TotalTreatmentBenefitRating.ToString());
            writer.WriteAttributeString(string.Concat(TTreatmentCostRating, attNameExtension), this.TotalTreatmentCostRating.ToString());
            writer.WriteAttributeString(string.Concat(TInsuranceCoverageRating, attNameExtension), this.TotalInsuranceCoverageRating.ToString());
            writer.WriteAttributeString(string.Concat(TCostRating, attNameExtension), this.TotalCostRating.ToString());
        }
    }
    public static class InputHCExtensions
    {
        //add a base health input stock to the baseStat.HealthCareCost1s dictionary
        public static bool AddInputHCStocksToDictionary(this InputHCStock baseStat,
            int filePosition, int nodePosition, HealthCareCost1Calculator hcInputStock)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.HealthCareCost1s == null)
                baseStat.HealthCareCost1s
                = new Dictionary<int, List<HealthCareCost1Calculator>>();
            if (baseStat.HealthCareCost1s.ContainsKey(filePosition))
            {
                if (baseStat.HealthCareCost1s[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.HealthCareCost1s[filePosition].Count <= i)
                        {
                            baseStat.HealthCareCost1s[filePosition]
                                .Add(new HealthCareCost1Calculator());
                        }
                    }
                    baseStat.HealthCareCost1s[filePosition][nodePosition]
                        = hcInputStock;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<HealthCareCost1Calculator> baseStats
                    = new List<HealthCareCost1Calculator>();
                KeyValuePair<int, List<HealthCareCost1Calculator>> newStat
                    = new KeyValuePair<int, List<HealthCareCost1Calculator>>(
                        filePosition, baseStats);
                baseStat.HealthCareCost1s.Add(newStat);
                bIsAdded = AddInputHCStocksToDictionary(baseStat,
                    filePosition, nodePosition, hcInputStock);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this InputHCStock baseStat,
            int filePosition, HealthCareCost1Calculator hcInputStock)
        {
            int iNodeCount = 0;
            if (baseStat.HealthCareCost1s == null)
                return iNodeCount;
            if (baseStat.HealthCareCost1s.ContainsKey(filePosition))
            {
                if (baseStat.HealthCareCost1s[filePosition] != null)
                {
                    iNodeCount = baseStat.HealthCareCost1s[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}

