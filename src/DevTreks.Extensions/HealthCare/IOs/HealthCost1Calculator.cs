using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize a health care cost object with
    ///             properties derived from typical insurance payments and reimbursements
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    public class HealthCareCost1Calculator : Input
    {
        public HealthCareCost1Calculator()
            : base()
        {
            //health care cost object
            InitHealthCareCost1Properties();
        }
        //copy constructor
        public HealthCareCost1Calculator(HealthCareCost1Calculator healthCost1)
            : base(healthCost1)
        {
            CopyHealthCareCost1Properties(healthCost1);
        }
        public enum HC1PRICE_TYPE
        {
            baseprice           = 0,
            adjustedprice       = 1,
            contractedprice     = 2,
            listprice           = 3,
            marketprice         = 4,
            productioncostprice = 5
        }
        public enum SEVERITY_TYPE
        {
            notsevere           = 0,
            slightlysevere      = 1,
            moderatelysevere    = 2,
            extremelysevere     = 3
        }
        public static HC1PRICE_TYPE GetHC1PriceType(string feeToUse)
        {
            //note that the default in the stylesheet must correspond to this default
            HC1PRICE_TYPE eHC1PRICE_TYPE = HC1PRICE_TYPE.contractedprice;
            if (feeToUse == HC1PRICE_TYPE.baseprice.ToString())
            {
                eHC1PRICE_TYPE = HC1PRICE_TYPE.baseprice;
            }
            else if (feeToUse == HC1PRICE_TYPE.adjustedprice.ToString())
            {
                eHC1PRICE_TYPE = HC1PRICE_TYPE.adjustedprice;
            }
            else if (feeToUse == HC1PRICE_TYPE.listprice.ToString())
            {
                eHC1PRICE_TYPE = HC1PRICE_TYPE.listprice;
            }
            else if (feeToUse == HC1PRICE_TYPE.marketprice.ToString())
            {
                eHC1PRICE_TYPE = HC1PRICE_TYPE.marketprice;
            }
            else if (feeToUse == HC1PRICE_TYPE.productioncostprice.ToString())
            {
                eHC1PRICE_TYPE = HC1PRICE_TYPE.productioncostprice;
            }
            return eHC1PRICE_TYPE;
        }   
        //calculator properties

//up to 10 generic demogs 
        public string HealthCareProvider { get; set; }
        public string InsuranceProvider { get; set; }
        public string PackageType { get; set; }
        public string PostalCode { get; set; }
        public string ConditionSeverity { get; set; }

//keep
        //string version of HC1PRICE_TYPE enum
        public string HC1PriceType { get; set; }
        //input.capprice
        public double BasePrice { get; set; }
        //AdjustedPrice = BasePriceAdjustment * BasePrice
        public double BasePriceAdjustment { get; set; }
        public double AdjustedPrice { get; set; }
        //insurance company negotiated fee
        public double ContractedPrice { get; set; }
        //health care provider list price
        public double ListPrice { get; set; }
        //health care provider market price
        public double MarketPrice { get; set; }
        //health care cost of production price
        public double ProductionCostPrice { get; set; }
//add additional life cycle props

//up to 10 subprice1s
//March, 2013: all of the premiums, incentives, copays and additional costs should be added 
//to either a subcosts array or an indicators array
        //annual premium insurance coverage paid by recipient
        public double AnnualPremium1 { get; set; }
        //annual premium for insurance coverage paid by other than recipient
        public double AnnualPremium2 { get; set; }
        //annual premiums to apply to this specific input
        public double AssignedPremiumCost { get; set; }
        //health care payments by receiver
        public double CoPay1Amount { get; set; }
        public double CoPay1Rate { get; set; }
        public double CoPay2Amount { get; set; }
        public double CoPay2Rate { get; set; }
        //health care incentives that reduce the health care receivers costs
        //low income subsidy, risk-related adjustments
        public double Incentive1Amount { get; set; }
        public double Incentive1Rate { get; set; }
        public double Incentive2Amount { get; set; }
        public double Incentive2Rate { get; set; }
        //first additional input cost associated with this input (i.e. recipient time oppportunity cost)
        public string AdditionalName1 { get; set; }
        public double AdditionalPrice1 { get; set; }
        public double AdditionalAmount1 { get; set; }
        public string AdditionalUnit1 { get; set; }
        public double AdditionalCost1 { get; set; }
        //second additional input cost associated with this input (i.e. recipient clothes, furniture, transportation cost)
        public string AdditionalName2 { get; set; }
        public double AdditionalPrice2 { get; set; }
        public double AdditionalAmount2 { get; set; }
        public string AdditionalUnit2 { get; set; }
        public double AdditionalCost2 { get; set; }
        //true means include the additional costs in the associated input.OCPrice
        public bool UseAddedCostsInInput { get; set; }
        //description of additional costs
        public string AdditionalCostsDescription { get; set; }
        //insurance company payment
        public double InsuranceProviderCost { get; set; }
        //incentive payments (low income subsidy ...)
        public double IncentivesCost { get; set; }
        //health care recipient payment
        public double ReceiverCost { get; set; }
        //health care provider payment
        public double HealthCareProviderCost { get; set; }
        //additional costs (add1 and add2 totals)
        public double AdditionalCost { get; set; }

//better to have generic indicators?
        //quality of care ratingings (scale of 0 (not applicable) to 10)
        //excellent and timely diagnosis = 10
        public double DiagnosisQualityRating { get; set; }
        //excellent and timely treatment = 10
        public double TreatmentQualityRating { get; set; }
        //excellent and timely outcome = 10
        public double TreatmentBenefitRating { get; set; }
        //costs fully worth benefits = 10
        public double TreatmentCostRating { get; set; }
        //amount of information received to make informed decision
        public double KnowledgeTransferRating { get; set; }
        //degree to which this input or service would have been chosen if not faced with constraints
        public double ConstrainedChoiceRating { get; set; }
        //insurance converage covered a great deal of the cost = 10
        public double InsuranceCoverageRating { get; set; }
        //explanation of input rating
        public string InputQualityAssessment { get; set; }
        //sum of (quality ratingings ) / 4
        public double CostRating { get; set; }
        //if offered, will complete a survey going into health benefits and costs in more detail
        public bool WillDoSurvey { get; set; }
//better if generic indicator
        //is health care input delivered in network (false equals out of network)
        public bool IsInNetwork { get; set; }

        private const string cHealthCareProvider = "HealthCareProvider";
        private const string cInsuranceProvider = "InsuranceProvider";
        private const string cPackageType = "PackageType";
        private const string cPostalCode = "PostalCode";
        private const string cConditionSeverity = "ConditionSeverity";
        private const string cHC1PriceType = "HC1PriceType";
        private const string cBasePrice = "BasePrice";
        private const string cBasePriceAdjustment = "BasePriceAdjustment";
        private const string cAdjustedPrice = "AdjustedPrice";
        private const string cContractedPrice = "ContractedPrice";
        private const string cListPrice = "ListPrice";
        private const string cMarketPrice = "MarketPrice";
        private const string cProductionCostPrice = "ProductionCostPrice";
        private const string cAnnualPremium1 = "AnnualPremium1";
        private const string cAnnualPremium2 = "AnnualPremium2";
        private const string cAssignedPremiumCost = "AssignedPremiumCost";
        private const string cCoPay1Amount = "CoPay1Amount";
        private const string cCoPay1Rate = "CoPay1Rate";
        private const string cCoPay2Amount = "CoPay2Amount";
        private const string cCoPay2Rate = "CoPay2Rate";
        private const string cIncentive1Amount = "Incentive1Amount";
        private const string cIncentive1Rate = "Incentive1Rate";
        private const string cIncentive2Amount = "Incentive2Amount";
        private const string cIncentive2Rate = "Incentive2Rate";
        private const string cAdditionalName1 = "AdditionalName1";
        private const string cAdditionalPrice1 = "AdditionalPrice1";
        private const string cAdditionalAmount1 = "AdditionalAmount1";
        private const string cAdditionalUnit1 = "AdditionalUnit1";
        private const string cAdditionalCost1 = "AdditionalCost1";
        private const string cAdditionalName2 = "AdditionalName2";
        private const string cAdditionalPrice2 = "AdditionalPrice2";
        private const string cAdditionalAmount2 = "AdditionalAmount2";
        private const string cAdditionalUnit2 = "AdditionalUnit2";
        private const string cAdditionalCost2 = "AdditionalCost2";
        private const string cUseAddedCostsInInput = "UseAddedCostsInInput";
        private const string cAdditionalCostsDescription = "AdditionalCostsDescription";
        private const string cInsuranceProviderCost = "InsuranceProviderCost";
        private const string cIncentivesCost = "IncentivesCost";
        private const string cReceiverCost = "ReceiverCost";
        private const string cHealthCareProviderCost = "HealthCareProviderCost";
        private const string cAdditionalCost = "AdditionalCost";
        private const string cDiagnosisQualityRating = "DiagnosisQualityRating";
        private const string cTreatmentQualityRating = "TreatmentQualityRating";
        private const string cTreatmentBenefitRating = "TreatmentBenefitRating";
        private const string cTreatmentCostRating = "TreatmentCostRating";
        private const string cKnowledgeTransferRating = "KnowledgeTransferRating";
        private const string cConstrainedChoiceRating = "ConstrainedChoiceRating";
        private const string cInsuranceCoverageRating = "InsuranceCoverageRating";
        private const string cInputQualityAssessment = "InputQualityAssessment";
        private const string cCostRating = "CostRating";
        private const string cWillDoSurvey = "WillDoSurvey";
        private const string cIsInNetwork = "IsInNetwork";

        public virtual void InitHealthCareCost1Properties()
        {
            //avoid null references to properties
            this.HealthCareProvider = string.Empty;
            this.InsuranceProvider = string.Empty;
            this.PackageType = string.Empty;
            this.PostalCode = string.Empty;
            this.ConditionSeverity = SEVERITY_TYPE.notsevere.ToString();
            this.HC1PriceType = HC1PRICE_TYPE.contractedprice.ToString();
            this.BasePrice = 0;
            this.BasePriceAdjustment = 0;
            this.AdjustedPrice = 0;
            this.ContractedPrice = 0;
            this.ListPrice = 0;
            this.MarketPrice = 0;
            this.ProductionCostPrice = 0;
            this.AnnualPremium1 = 0;
            this.AnnualPremium2 = 0;
            this.AssignedPremiumCost = 0;
            this.CoPay1Amount = 0;
            this.CoPay1Rate = 0;
            this.CoPay2Amount = 0;
            this.CoPay2Rate = 0;
            this.Incentive1Amount = 0;
            this.Incentive1Rate = 0;
            this.Incentive2Amount = 0;
            this.Incentive2Rate = 0;
            this.AdditionalName1 = string.Empty;
            this.AdditionalPrice1 = 0;
            this.AdditionalAmount1 = 0;
            this.AdditionalUnit1 = string.Empty;
            this.AdditionalCost1 = 0;
            this.AdditionalName2 = string.Empty;
            this.AdditionalPrice2 = 0;
            this.AdditionalAmount2 = 0;
            this.AdditionalUnit2 = string.Empty;
            this.AdditionalCost2 = 0;
            this.UseAddedCostsInInput = false;
            this.AdditionalCostsDescription = string.Empty;
            this.InsuranceProviderCost = 0;
            this.IncentivesCost = 0;
            this.ReceiverCost = 0;
            this.HealthCareProviderCost = 0;
            this.AdditionalCost = 0;
            this.DiagnosisQualityRating = 0;
            this.TreatmentQualityRating = 0;
            this.TreatmentBenefitRating = 0;
            this.TreatmentCostRating = 0;
            this.KnowledgeTransferRating = 0;
            this.ConstrainedChoiceRating = 0;
            this.InsuranceCoverageRating = 0;
            this.InputQualityAssessment = string.Empty;
            this.CostRating = 0;
            this.WillDoSurvey = false;
            this.IsInNetwork = true;
        }

        public virtual void CopyHealthCareCost1Properties(
            HealthCareCost1Calculator calculator)
        {
            this.HealthCareProvider = calculator.HealthCareProvider;
            this.InsuranceProvider = calculator.InsuranceProvider;
            this.PackageType = calculator.PackageType;
            this.PostalCode = calculator.PostalCode;
            this.ConditionSeverity = calculator.ConditionSeverity;
            this.HC1PriceType = calculator.HC1PriceType;
            this.BasePrice = calculator.BasePrice;
            this.BasePriceAdjustment = calculator.BasePriceAdjustment;
            this.AdjustedPrice = calculator.AdjustedPrice;
            this.ContractedPrice = calculator.ContractedPrice;
            this.ListPrice = calculator.ListPrice;
            this.MarketPrice = calculator.MarketPrice;
            this.ProductionCostPrice = calculator.ProductionCostPrice;
            this.AnnualPremium1 = calculator.AnnualPremium1;
            this.AnnualPremium2 = calculator.AnnualPremium2;
            this.AssignedPremiumCost = calculator.AssignedPremiumCost;
            this.CoPay1Amount = calculator.CoPay1Amount;
            this.CoPay1Rate = calculator.CoPay1Rate;
            this.CoPay2Amount = calculator.CoPay2Amount;
            this.CoPay2Rate = calculator.CoPay2Rate;
            this.Incentive1Amount = calculator.Incentive1Amount;
            this.Incentive1Rate = calculator.Incentive1Rate;
            this.Incentive2Amount = calculator.Incentive2Amount;
            this.Incentive2Rate = calculator.Incentive2Rate;
            this.AdditionalName1 = calculator.AdditionalName1;
            this.AdditionalPrice1 = calculator.AdditionalPrice1;
            this.AdditionalAmount1 = calculator.AdditionalAmount1;
            this.AdditionalUnit1 = calculator.AdditionalUnit1;
            this.AdditionalCost1 = calculator.AdditionalCost1;
            this.AdditionalName2 = calculator.AdditionalName2;
            this.AdditionalPrice2 = calculator.AdditionalPrice2;
            this.AdditionalAmount2 = calculator.AdditionalAmount2;
            this.AdditionalUnit2 = calculator.AdditionalUnit2;
            this.AdditionalCost2 = calculator.AdditionalCost2;
            this.UseAddedCostsInInput = calculator.UseAddedCostsInInput;
            this.AdditionalCostsDescription = calculator.AdditionalCostsDescription;
            this.InsuranceProviderCost = calculator.InsuranceProviderCost;
            this.IncentivesCost = calculator.IncentivesCost;
            this.ReceiverCost = calculator.ReceiverCost;
            this.HealthCareProviderCost = calculator.HealthCareProviderCost;
            this.AdditionalCost = calculator.AdditionalCost;
            this.DiagnosisQualityRating = calculator.DiagnosisQualityRating;
            this.TreatmentQualityRating = calculator.TreatmentQualityRating;
            this.TreatmentBenefitRating = calculator.TreatmentBenefitRating;
            this.TreatmentCostRating = calculator.TreatmentCostRating;
            this.KnowledgeTransferRating = calculator.KnowledgeTransferRating;
            this.ConstrainedChoiceRating = calculator.ConstrainedChoiceRating;
            this.InsuranceCoverageRating = calculator.InsuranceCoverageRating;
            this.InputQualityAssessment = calculator.InputQualityAssessment;
            this.CostRating = calculator.CostRating;
            this.WillDoSurvey = calculator.WillDoSurvey;
            this.IsInNetwork = calculator.IsInNetwork;
        }
        public virtual void SetHealthCareCost1Properties(CalculatorParameters calcParameters,
            XElement calculator, XElement currentElement)
        {
            SetInputProperties(calcParameters, calculator,
                currentElement);
            SetHealthCareCost1Properties(calculator);
        }
        //set the class properties using the XElement
        public virtual void SetHealthCareCost1Properties(XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            this.HealthCareProvider = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthCareProvider);
            this.InsuranceProvider = CalculatorHelpers.GetAttribute(currentCalculationsElement, cInsuranceProvider);
            this.PackageType = CalculatorHelpers.GetAttribute(currentCalculationsElement, cPackageType);
            this.PostalCode = CalculatorHelpers.GetAttribute(currentCalculationsElement, cPostalCode);
            this.ConditionSeverity = CalculatorHelpers.GetAttribute(currentCalculationsElement, cConditionSeverity);
            this.HC1PriceType = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHC1PriceType);
            this.BasePrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBasePrice);
            this.BasePriceAdjustment = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBasePriceAdjustment);
            this.AdjustedPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdjustedPrice);
            this.ProductionCostPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cProductionCostPrice);
            this.ListPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cListPrice);
            this.MarketPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cMarketPrice);
            this.ContractedPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cContractedPrice);
            this.AnnualPremium1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAnnualPremium1);
            this.AnnualPremium2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAnnualPremium2);
            this.AssignedPremiumCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAssignedPremiumCost);
            this.CoPay1Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCoPay1Amount);
            this.CoPay1Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCoPay1Rate);
            this.CoPay2Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCoPay2Amount);
            this.CoPay2Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCoPay2Rate);
            this.Incentive1Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIncentive1Amount);
            this.Incentive1Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIncentive1Rate);
            this.Incentive2Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIncentive2Amount);
            this.Incentive2Rate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIncentive2Rate);
            this.AdditionalName1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cAdditionalName1);
            this.AdditionalPrice1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdditionalPrice1);
            this.AdditionalAmount1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdditionalAmount1);
            this.AdditionalUnit1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cAdditionalUnit1);
            this.AdditionalCost1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdditionalCost1);
            this.AdditionalName2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cAdditionalName2);
            this.AdditionalPrice2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdditionalPrice2);
            this.AdditionalAmount2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdditionalAmount2);
            this.AdditionalUnit2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cAdditionalUnit2);
            this.AdditionalCost2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdditionalCost2);
            this.UseAddedCostsInInput = CalculatorHelpers.GetAttributeBool(currentCalculationsElement, cUseAddedCostsInInput);
            this.AdditionalCostsDescription = CalculatorHelpers.GetAttribute(currentCalculationsElement, cAdditionalCostsDescription);
            this.InsuranceProviderCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cInsuranceProviderCost);
            this.IncentivesCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIncentivesCost);
            this.ReceiverCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cReceiverCost);
            this.HealthCareProviderCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cHealthCareProviderCost);
            this.AdditionalCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdditionalCost);
            this.DiagnosisQualityRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cDiagnosisQualityRating);
            this.TreatmentQualityRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTreatmentQualityRating);
            this.TreatmentBenefitRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTreatmentBenefitRating);
            this.TreatmentCostRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTreatmentCostRating);
            this.KnowledgeTransferRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cKnowledgeTransferRating);
            this.ConstrainedChoiceRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cConstrainedChoiceRating);
            this.InsuranceCoverageRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cInsuranceCoverageRating);
            this.InputQualityAssessment = CalculatorHelpers.GetAttribute(currentCalculationsElement, cInputQualityAssessment);
            this.CostRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCostRating);
            this.WillDoSurvey = CalculatorHelpers.GetAttributeBool(currentCalculationsElement, cWillDoSurvey);
            this.IsInNetwork = CalculatorHelpers.GetAttributeBool(currentCalculationsElement, cIsInNetwork);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetHealthCareCost1Properties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cHealthCareProvider:
                    this.HealthCareProvider = attValue;
                    break;
                case cInsuranceProvider:
                    this.InsuranceProvider = attValue;
                    break;
                case cPackageType:
                    this.PackageType = attValue;
                    break;
                case cPostalCode:
                    this.PostalCode = attValue;
                    break;
                case cConditionSeverity:
                    this.ConditionSeverity = attValue;
                    break;
                case cHC1PriceType:
                    this.HC1PriceType = attValue;
                    break;
                case cBasePrice:
                    this.BasePrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cBasePriceAdjustment:
                    this.BasePriceAdjustment = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdjustedPrice:
                    this.AdjustedPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cContractedPrice:
                    this.ContractedPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cListPrice:
                    this.ListPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cMarketPrice:
                    this.MarketPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cProductionCostPrice:
                    this.ProductionCostPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAnnualPremium1:
                    this.AnnualPremium1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAnnualPremium2:
                    this.AnnualPremium2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAssignedPremiumCost:
                    this.AssignedPremiumCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCoPay1Amount:
                    this.CoPay1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCoPay1Rate:
                    this.CoPay1Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCoPay2Amount:
                    this.CoPay2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCoPay2Rate:
                    this.CoPay2Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIncentive1Amount:
                    this.Incentive1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIncentive2Amount:
                    this.Incentive2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIncentive1Rate:
                    this.Incentive1Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIncentive2Rate:
                    this.Incentive2Rate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdditionalName1:
                    this.AdditionalName1 = attValue;
                    break;
                case cAdditionalPrice1:
                    this.AdditionalPrice1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdditionalAmount1:
                    this.AdditionalAmount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdditionalUnit1:
                    this.AdditionalUnit1 = attValue;
                    break;
                case cAdditionalCost1:
                    this.AdditionalCost1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                     break;
                case cAdditionalName2:
                    this.AdditionalName2 =attValue;
                    break;
                case cAdditionalPrice2:
                    this.AdditionalPrice2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdditionalAmount2:
                    this.AdditionalAmount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdditionalUnit2:
                    this.AdditionalUnit2 = attValue;
                    break;
                case cAdditionalCost2:
                    this.AdditionalCost2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cUseAddedCostsInInput:
                    this.UseAddedCostsInInput = CalculatorHelpers.ConvertStringToBool(attValue);
                    break;
                case cAdditionalCostsDescription:
                    this.AdditionalCostsDescription = attValue;
                    break;
                case cInsuranceProviderCost:
                    this.InsuranceProviderCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIncentivesCost:
                    this.IncentivesCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cReceiverCost:
                    this.ReceiverCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cHealthCareProviderCost:
                    this.HealthCareProviderCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdditionalCost:
                    this.AdditionalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cDiagnosisQualityRating:
                    this.DiagnosisQualityRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTreatmentQualityRating:
                    this.TreatmentQualityRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTreatmentBenefitRating:
                    this.TreatmentBenefitRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTreatmentCostRating:
                    this.TreatmentCostRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cKnowledgeTransferRating:
                    this.KnowledgeTransferRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cConstrainedChoiceRating:
                    this.ConstrainedChoiceRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInsuranceCoverageRating:
                    this.InsuranceCoverageRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInputQualityAssessment:
                    this.InputQualityAssessment = attValue;
                    break;
                case cCostRating:
                    this.CostRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWillDoSurvey:
                    this.WillDoSurvey = CalculatorHelpers.ConvertStringToBool(attValue);
                    break;
                case cIsInNetwork:
                    this.IsInNetwork = CalculatorHelpers.ConvertStringToBool(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetHealthCareCost1Attributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            //don't set any input attributes; each calculator should set what's needed separately
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cHealthCareProvider, attNameExtension), this.HealthCareProvider);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cInsuranceProvider, attNameExtension), this.InsuranceProvider);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cPackageType, attNameExtension), this.PackageType);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cPostalCode, attNameExtension), this.PostalCode);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cConditionSeverity, attNameExtension), this.ConditionSeverity);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cHC1PriceType, attNameExtension), this.HC1PriceType);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cBasePrice, attNameExtension), this.BasePrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cBasePriceAdjustment, attNameExtension), this.BasePriceAdjustment);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdjustedPrice, attNameExtension), this.AdjustedPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cContractedPrice, attNameExtension), this.ContractedPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cListPrice, attNameExtension), this.ListPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cMarketPrice, attNameExtension), this.MarketPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cProductionCostPrice, attNameExtension), this.ProductionCostPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cAnnualPremium1, attNameExtension), this.AnnualPremium1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cAnnualPremium2, attNameExtension), this.AnnualPremium2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cAssignedPremiumCost, attNameExtension), this.AssignedPremiumCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cCoPay1Amount, attNameExtension), this.CoPay1Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCoPay1Rate, attNameExtension), this.CoPay1Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCoPay2Amount, attNameExtension), this.CoPay2Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCoPay2Rate, attNameExtension), this.CoPay2Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIncentive1Amount, attNameExtension), this.Incentive1Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIncentive2Amount, attNameExtension), this.Incentive2Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIncentive1Rate, attNameExtension), this.Incentive1Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIncentive2Rate, attNameExtension), this.Incentive2Rate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIncentive2Rate, attNameExtension), this.Incentive2Rate);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cAdditionalName1, attNameExtension), this.AdditionalName1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdditionalPrice1, attNameExtension), this.AdditionalPrice1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdditionalAmount1, attNameExtension), this.AdditionalAmount1);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cAdditionalUnit1, attNameExtension), this.AdditionalUnit1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdditionalCost1, attNameExtension), this.AdditionalCost1);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cAdditionalName2, attNameExtension), this.AdditionalName2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdditionalPrice2, attNameExtension), this.AdditionalPrice2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdditionalAmount2, attNameExtension), this.AdditionalAmount2);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cAdditionalUnit2, attNameExtension), this.AdditionalUnit2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdditionalCost2, attNameExtension), this.AdditionalCost2);
            CalculatorHelpers.SetAttributeBool(currentCalculationsElement,
                string.Concat(cUseAddedCostsInInput, attNameExtension), this.UseAddedCostsInInput);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cAdditionalCostsDescription, attNameExtension), this.AdditionalCostsDescription);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cInsuranceProviderCost, attNameExtension), this.InsuranceProviderCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cIncentivesCost, attNameExtension), this.IncentivesCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cReceiverCost, attNameExtension), this.ReceiverCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cHealthCareProviderCost, attNameExtension), this.HealthCareProviderCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cAdditionalCost, attNameExtension), this.AdditionalCost);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(cDiagnosisQualityRating, attNameExtension), this.DiagnosisQualityRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(cTreatmentQualityRating, attNameExtension), this.TreatmentQualityRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(cTreatmentBenefitRating, attNameExtension), this.TreatmentBenefitRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(cTreatmentCostRating, attNameExtension), this.TreatmentCostRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(cKnowledgeTransferRating, attNameExtension), this.KnowledgeTransferRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(cConstrainedChoiceRating, attNameExtension), this.ConstrainedChoiceRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(cInsuranceCoverageRating, attNameExtension), this.InsuranceCoverageRating);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cInputQualityAssessment, attNameExtension), this.InputQualityAssessment);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cCostRating, attNameExtension), this.CostRating);
            CalculatorHelpers.SetAttributeBool(currentCalculationsElement,
               string.Concat(cWillDoSurvey, attNameExtension), this.WillDoSurvey);
            CalculatorHelpers.SetAttributeBool(currentCalculationsElement,
               string.Concat(cIsInNetwork, attNameExtension), this.IsInNetwork);
        }
        public virtual void SetHealthCareCost1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(cHealthCareProvider, attNameExtension), this.HealthCareProvider.ToString());
            writer.WriteAttributeString(string.Concat(cInsuranceProvider, attNameExtension), this.InsuranceProvider.ToString());
            writer.WriteAttributeString(string.Concat(cPackageType, attNameExtension), this.PackageType.ToString());
            writer.WriteAttributeString(string.Concat(cPostalCode, attNameExtension), this.PostalCode.ToString());
            writer.WriteAttributeString(string.Concat(cConditionSeverity, attNameExtension), this.ConditionSeverity.ToString());
            writer.WriteAttributeString(string.Concat(cHC1PriceType, attNameExtension), this.HC1PriceType.ToString());
            writer.WriteAttributeString(string.Concat(cBasePrice, attNameExtension), this.BasePrice.ToString());
            writer.WriteAttributeString(string.Concat(cBasePriceAdjustment, attNameExtension), this.BasePriceAdjustment.ToString());
            writer.WriteAttributeString(string.Concat(cAdjustedPrice, attNameExtension), this.AdjustedPrice.ToString());
            writer.WriteAttributeString(string.Concat(cContractedPrice, attNameExtension), this.ContractedPrice.ToString());
            writer.WriteAttributeString(string.Concat(cListPrice, attNameExtension), this.ListPrice.ToString());
            writer.WriteAttributeString(string.Concat(cMarketPrice, attNameExtension), this.MarketPrice.ToString());
            writer.WriteAttributeString(string.Concat(cProductionCostPrice, attNameExtension), this.ProductionCostPrice.ToString());
            writer.WriteAttributeString(string.Concat(cAnnualPremium1, attNameExtension), this.AnnualPremium1.ToString());
            writer.WriteAttributeString(string.Concat(cAnnualPremium2, attNameExtension), this.AnnualPremium2.ToString());
            writer.WriteAttributeString(string.Concat(cAssignedPremiumCost, attNameExtension), this.AssignedPremiumCost.ToString());
            writer.WriteAttributeString(string.Concat(cCoPay1Rate, attNameExtension), this.CoPay1Rate.ToString());
            writer.WriteAttributeString(string.Concat(cCoPay2Amount, attNameExtension), this.CoPay2Amount.ToString());
            writer.WriteAttributeString(string.Concat(cCoPay2Rate, attNameExtension), this.CoPay2Rate.ToString());
            writer.WriteAttributeString(string.Concat(cIncentive1Amount, attNameExtension), this.Incentive1Amount.ToString());
            writer.WriteAttributeString(string.Concat(cIncentive2Amount, attNameExtension), this.Incentive2Amount.ToString());
            writer.WriteAttributeString(string.Concat(cIncentive1Rate, attNameExtension), this.Incentive1Rate.ToString());
            writer.WriteAttributeString(string.Concat(cIncentive2Rate, attNameExtension), this.Incentive2Rate.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalName1, attNameExtension), this.AdditionalName1);
            writer.WriteAttributeString(string.Concat(cAdditionalPrice1, attNameExtension), this.AdditionalPrice1.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalAmount1, attNameExtension), this.AdditionalAmount1.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalUnit1, attNameExtension), this.AdditionalUnit1);
            writer.WriteAttributeString(string.Concat(cAdditionalCost1, attNameExtension), this.AdditionalCost1.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalName2, attNameExtension), this.AdditionalName2);
            writer.WriteAttributeString(string.Concat(cAdditionalPrice2, attNameExtension), this.AdditionalPrice2.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalAmount2, attNameExtension), this.AdditionalAmount2.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalUnit2, attNameExtension), this.AdditionalUnit2);
            writer.WriteAttributeString(string.Concat(cAdditionalCost2, attNameExtension), this.AdditionalCost2.ToString());
            writer.WriteAttributeString(string.Concat(cUseAddedCostsInInput, attNameExtension), this.UseAddedCostsInInput.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalCostsDescription, attNameExtension), this.AdditionalCostsDescription);
            writer.WriteAttributeString(string.Concat(cInsuranceProviderCost, attNameExtension), this.InsuranceProviderCost.ToString());
            writer.WriteAttributeString(string.Concat(cIncentivesCost, attNameExtension), this.IncentivesCost.ToString());
            writer.WriteAttributeString(string.Concat(cReceiverCost, attNameExtension), this.ReceiverCost.ToString());
            writer.WriteAttributeString(string.Concat(cHealthCareProviderCost, attNameExtension), this.HealthCareProviderCost.ToString());
            writer.WriteAttributeString(string.Concat(cAdditionalCost, attNameExtension), this.AdditionalCost.ToString());
            writer.WriteAttributeString(string.Concat(cDiagnosisQualityRating, attNameExtension), this.DiagnosisQualityRating.ToString());
            writer.WriteAttributeString(string.Concat(cTreatmentQualityRating, attNameExtension), this.TreatmentQualityRating.ToString());
            writer.WriteAttributeString(string.Concat(cTreatmentBenefitRating, attNameExtension), this.TreatmentBenefitRating.ToString());
            writer.WriteAttributeString(string.Concat(cTreatmentCostRating, attNameExtension), this.TreatmentCostRating.ToString());
            writer.WriteAttributeString(string.Concat(cKnowledgeTransferRating, attNameExtension), this.KnowledgeTransferRating.ToString());
            writer.WriteAttributeString(string.Concat(cConstrainedChoiceRating, attNameExtension), this.ConstrainedChoiceRating.ToString());
            writer.WriteAttributeString(string.Concat(cInsuranceCoverageRating, attNameExtension), this.InsuranceCoverageRating.ToString());
            writer.WriteAttributeString(string.Concat(cInputQualityAssessment, attNameExtension), this.InputQualityAssessment.ToString());
            writer.WriteAttributeString(string.Concat(cCostRating, attNameExtension), this.CostRating.ToString());
            writer.WriteAttributeString(string.Concat(cWillDoSurvey, attNameExtension), this.WillDoSurvey.ToString());
            writer.WriteAttributeString(string.Concat(cIsInNetwork, attNameExtension), this.IsInNetwork.ToString());
        }
        public bool RunHCCalculations(CalculatorParameters calcParameters, 
            HealthCareCost1Calculator hcCost1)
        {
            bool bHasCalculations = false;
            if (hcCost1 != null)
            {
                hcCost1.AdjustedPrice = hcCost1.BasePrice * (hcCost1.BasePriceAdjustment / 100);
                HealthCareCost1Calculator.HC1PRICE_TYPE eHC1PriceType
                    = HealthCareCost1Calculator.GetHC1PriceType(hcCost1.HC1PriceType);
                hcCost1.HC1PriceType = eHC1PriceType.ToString();
                if (eHC1PriceType == HealthCareCost1Calculator.HC1PRICE_TYPE.adjustedprice)
                {
                    CalculateHealthCareCosts(calcParameters, hcCost1, hcCost1.AdjustedPrice);
                }
                else if (eHC1PriceType == HealthCareCost1Calculator.HC1PRICE_TYPE.baseprice)
                {
                    CalculateHealthCareCosts(calcParameters, hcCost1, hcCost1.BasePrice);
                }
                else if (eHC1PriceType == HealthCareCost1Calculator.HC1PRICE_TYPE.contractedprice)
                {
                    CalculateHealthCareCosts(calcParameters, hcCost1, hcCost1.ContractedPrice);
                }
                else if (eHC1PriceType == HealthCareCost1Calculator.HC1PRICE_TYPE.listprice)
                {
                    CalculateHealthCareCosts(calcParameters, hcCost1, hcCost1.ListPrice);
                }
                else if (eHC1PriceType == HealthCareCost1Calculator.HC1PRICE_TYPE.marketprice)
                {
                    CalculateHealthCareCosts(calcParameters, hcCost1, hcCost1.MarketPrice);
                }
                else if (eHC1PriceType == HealthCareCost1Calculator.HC1PRICE_TYPE.productioncostprice)
                {
                    CalculateHealthCareCosts(calcParameters, hcCost1, hcCost1.ProductionCostPrice);
                }
                CalculateAdditionalHealthCareCosts(calcParameters, hcCost1);
                //set the base input properties
                //remember that the npv calcors rerun calcors when amounts get changed (so don't change amount)
                //note that the ReceiveCost can be manipulated to return cost of production, list price, 
                //market price, base price, contracted price by setting appropriate copays and incentives (to zero)
                hcCost1.OCPrice = hcCost1.ReceiverCost;
                hcCost1.OCUnit = (hcCost1.OCUnit != string.Empty && hcCost1.OCUnit != Constants.NONE)
                    ? hcCost1.OCUnit : Constants.EACH;
                hcCost1.IncentiveAmount = hcCost1.Incentive1Amount + hcCost1.Incentive2Amount;
                hcCost1.IncentiveRate = hcCost1.Incentive1Rate + hcCost1.Incentive2Rate;
                //set the bcrating (equally weighted)
                SetCostRating(hcCost1);
                bHasCalculations = true;
            }
            else
            {
                calcParameters.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE");
            }
            return bHasCalculations;
        }
        
        private void CalculateHealthCareCosts(CalculatorParameters calcParameters,
            HealthCareCost1Calculator hcCost1, double fee)
        {
            FixSelections(hcCost1);
            hcCost1.InsuranceProviderCost = hcCost1.ContractedPrice - hcCost1.CoPay1Amount - hcCost1.CoPay2Amount
                - (hcCost1.ContractedPrice * hcCost1.CoPay1Rate) - (hcCost1.ContractedPrice * hcCost1.CoPay2Rate);
            if (hcCost1.InsuranceProviderCost < 0)
            {
                hcCost1.InsuranceProviderCost = 0;
            }
            hcCost1.IncentivesCost = hcCost1.Incentive1Amount + hcCost1.Incentive2Amount
                + (fee * hcCost1.Incentive1Rate) + (fee * hcCost1.Incentive2Rate);
            if (hcCost1.AssignedPremiumCost > (hcCost1.AnnualPremium1 + hcCost1.AnnualPremium2))
            {
                hcCost1.AssignedPremiumCost = hcCost1.AnnualPremium1 + hcCost1.AnnualPremium2;
            }
            hcCost1.ReceiverCost = fee - hcCost1.InsuranceProviderCost - hcCost1.IncentivesCost + hcCost1.AssignedPremiumCost;
            if (hcCost1.ReceiverCost < 0)
            {
                hcCost1.ReceiverCost = 0;
            }
            hcCost1.HealthCareProviderCost = hcCost1.InsuranceProviderCost + hcCost1.ReceiverCost - hcCost1.AssignedPremiumCost;
            if (hcCost1.HealthCareProviderCost < 0)
            {
                hcCost1.HealthCareProviderCost = 0;
            }
        }
        private void FixSelections(HealthCareCost1Calculator hcCost1)
        {
            if (hcCost1.HC1PriceType == string.Empty)
            {
                hcCost1.HC1PriceType = HC1PRICE_TYPE.contractedprice.ToString();
            }
            if (hcCost1.ConditionSeverity == string.Empty)
            {
                hcCost1.ConditionSeverity = SEVERITY_TYPE.notsevere.ToString();
            }
        }
        private void CalculateAdditionalHealthCareCosts(CalculatorParameters calcParameters,
            HealthCareCost1Calculator hcCost1)
        {
            hcCost1.AdditionalCost1 = hcCost1.AdditionalPrice1 * hcCost1.AdditionalAmount1;
            hcCost1.AdditionalCost2 = hcCost1.AdditionalPrice2 * hcCost1.AdditionalAmount2;
            hcCost1.AdditionalCost = hcCost1.AdditionalCost1 + hcCost1.AdditionalCost2;
            if (hcCost1.UseAddedCostsInInput == true)
            {
                hcCost1.ReceiverCost = hcCost1.ReceiverCost + hcCost1.AdditionalCost;
            }
        }
        private void SetCostRating(HealthCareCost1Calculator hcCost1)
        {
            double dbBCRating = 0;
            int iDivisor = 0;
            if (hcCost1.DiagnosisQualityRating > 0)
            {
                dbBCRating += hcCost1.DiagnosisQualityRating * 10;
                iDivisor += 1;
            }
            if (hcCost1.TreatmentQualityRating > 0)
            {
                dbBCRating += hcCost1.TreatmentQualityRating * 10;
                iDivisor += 1;
            }
            if (hcCost1.TreatmentBenefitRating > 0)
            {
                dbBCRating += hcCost1.TreatmentBenefitRating * 10;
                iDivisor += 1;
            }
            if (hcCost1.TreatmentCostRating > 0)
            {
                dbBCRating += hcCost1.TreatmentCostRating * 10;
                iDivisor += 1;
            }
            if (hcCost1.KnowledgeTransferRating > 0)
            {
                dbBCRating += hcCost1.KnowledgeTransferRating * 10;
                iDivisor += 1;
            }
            if (hcCost1.ConstrainedChoiceRating > 0)
            {
                dbBCRating += hcCost1.ConstrainedChoiceRating * 10;
                iDivisor += 1;
            }
            if (hcCost1.InsuranceCoverageRating > 0)
            {
                dbBCRating += hcCost1.InsuranceCoverageRating * 10;
                iDivisor += 1;
            }
            if (iDivisor == 0)
            {
                hcCost1.CostRating = 0;
            }
            else
            {
                hcCost1.CostRating = dbBCRating / iDivisor;
            }
        }
    }
}

