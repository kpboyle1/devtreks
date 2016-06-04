using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize a food nutrition fact object with
    ///             properties derived from the Nutrition Facts found on 
    ///             the back of most USA food packages.
    ///Author:		www.devtreks.org
    ///Date:		2012, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Although this class inherits from Input, it doesn't initiate 
    ///             any Input properties or attributes. This keeps the calculator simple 
    ///             and clean. If Input properties can be changed by this calculator, 
    ///             change the input properties/attributes in the calculator 
    ///             (i.e. foodStock.SetInputProperties).
    public class FoodFactCalculator : Input
    {
        public FoodFactCalculator()
            : base()
        {
            //food fact
            InitFoodFactProperties();
        }
        //copy constructor
        public FoodFactCalculator(FoodFactCalculator foodfact)
            : base(foodfact)
        {
            //food fact
            CopyFoodFactProperties(foodfact);
        }
        //calculator properties
        public double MarketValue { get; set; }
        public double ContainerSize { get; set; }
        public double ServingCost { get; set; }
        public double ActualCaloriesPerDay { get; set; }
        public double ServingsPerContainer { get; set; }
        public double ActualServingsPerContainer { get; set; }
        public string GenderOfServingPerson { get; set; }
        public double WeightOfServingPerson { get; set; }
        public string WeightUnitsOfServingPerson { get; set; }
        public double CaloriesPerServing { get; set; }
        public double CaloriesPerActualServing { get; set; }
        public double CaloriesFromFatPerServing { get; set; }
        public double CaloriesFromFatPerActualServing { get; set; }
        public double TotalFatPerServing { get; set; }
        public double TotalFatPerActualServing { get; set; }
        public double TotalFatActualDailyPercent { get; set; }
        public double SaturatedFatPerServing { get; set; }
        public double SaturatedFatPerActualServing { get; set; }
        public double SaturatedFatActualDailyPercent { get; set; }
        public double TransFatPerServing { get; set; }
        public double TransFatPerActualServing { get; set; }
        public double CholesterolPerServing { get; set; }
        public double CholesterolPerActualServing { get; set; }
        public double CholesterolActualDailyPercent { get; set; }
        public double SodiumPerServing { get; set; }
        public double SodiumPerActualServing { get; set; }
        public double SodiumActualDailyPercent { get; set; }
        public double PotassiumPerServing { get; set; }
        public double PotassiumPerActualServing { get; set; }
        public double TotalCarbohydratePerServing { get; set; }
        public double TotalCarbohydratePerActualServing { get; set; }
        public double TotalCarbohydrateActualDailyPercent { get; set; }
        public double OtherCarbohydratePerServing { get; set; }
        public double OtherCarbohydratePerActualServing { get; set; }
        public double OtherCarbohydrateActualDailyPercent { get; set; }
        public double DietaryFiberPerServing { get; set; }
        public double DietaryFiberPerActualServing { get; set; }
        public double DietaryFiberActualDailyPercent { get; set; }
        public double SugarsPerServing { get; set; }
        public double SugarsPerActualServing { get; set; }
        public double ProteinPerServing { get; set; }
        public double ProteinPerActualServing { get; set; }
        public double ProteinActualDailyPercent { get; set; }
        public double VitaminAPercentDailyValue { get; set; }
        public double VitaminAPercentActualDailyValue { get; set; }
        public double VitaminCPercentDailyValue { get; set; }
        public double VitaminCPercentActualDailyValue { get; set; }
        public double VitaminDPercentDailyValue { get; set; }
        public double VitaminDPercentActualDailyValue { get; set; }
        public double CalciumPercentDailyValue { get; set; }
        public double CalciumPercentActualDailyValue { get; set; }
        public double IronPercentDailyValue { get; set; }
        public double IronPercentActualDailyValue { get; set; }
        public double ThiaminPercentDailyValue { get; set; }
        public double ThiaminPercentActualDailyValue { get; set; }
        public double FolatePercentDailyValue { get; set; }
        public double FolatePercentActualDailyValue { get; set; }
        public double RiboflavinPercentDailyValue { get; set; }
        public double RiboflavinPercentActualDailyValue { get; set; }
        public double NiacinPercentDailyValue { get; set; }
        public double NiacinPercentActualDailyValue { get; set; }
        public double VitaminB6PercentDailyValue { get; set; }
        public double VitaminB6PercentActualDailyValue { get; set; }
        public double VitaminB12PercentDailyValue { get; set; }
        public double VitaminB12PercentActualDailyValue { get; set; }
        public double PhosphorousPercentDailyValue { get; set; }
        public double PhosphorousPercentActualDailyValue { get; set; }
        public double MagnesiumPercentDailyValue { get; set; }
        public double MagnesiumPercentActualDailyValue { get; set; }
        public double ZincPercentDailyValue { get; set; }
        public double ZincPercentActualDailyValue { get; set; }

        private const string cMarketValue = "MarketValue";
        private const string cContainerSize = "ContainerSize";
        private const string cServingCost = "ServingCost";
        private const string cActualCaloriesPerDay = "ActualCaloriesPerDay";
        private const string cGenderOfServingPerson = "GenderOfServingPerson";
        private const string cWeightOfServingPerson = "WeightOfServingPerson";
        private const string cWeightUnitsOfServingPerson = "WeightUnitsOfServingPerson";
        private const string cServingsPerContainer = "ServingsPerContainer";
        private const string cActualServingsPerContainer = "ActualServingsPerContainer";
        private const string cCaloriesPerServing = "CaloriesPerServing";
        private const string cCaloriesPerActualServing = "CaloriesPerActualServing";
        private const string cCaloriesFromFatPerServing = "CaloriesFromFatPerServing";
        private const string cCaloriesFromFatPerActualServing = "CaloriesFromFatPerActualServing";
        private const string cTotalFatPerServing = "TotalFatPerServing";
        private const string cTotalFatPerActualServing = "TotalFatPerActualServing";
        private const string cTotalFatActualDailyPercent = "TotalFatActualDailyPercent";
        private const string cSaturatedFatPerServing = "SaturatedFatPerServing";
        private const string cSaturatedFatPerActualServing = "SaturatedFatPerActualServing";
        private const string cSaturatedFatActualDailyPercent = "SaturatedFatActualDailyPercent";
        private const string cTransFatPerServing = "TransFatPerServing";
        private const string cTransFatPerActualServing = "TransFatPerActualServing";
        private const string cCholesterolPerServing = "CholesterolPerServing";
        private const string cCholesterolPerActualServing = "CholesterolPerActualServing";
        private const string cCholesterolActualDailyPercent = "CholesterolActualDailyPercent";
        private const string cSodiumPerServing = "SodiumPerServing";
        private const string cSodiumPerActualServing = "SodiumPerActualServing";
        private const string cSodiumActualDailyPercent = "SodiumActualDailyPercent";
        private const string cPotassiumPerServing = "PotassiumPerServing";
        private const string cPotassiumPerActualServing = "PotassiumPerActualServing";
        private const string cTotalCarbohydratePerServing = "TotalCarbohydratePerServing";
        private const string cTotalCarbohydratePerActualServing = "TotalCarbohydratePerActualServing";
        private const string cTotalCarbohydrateActualDailyPercent = "TotalCarbohydrateActualDailyPercent";
        private const string cOtherCarbohydratePerServing = "OtherCarbohydratePerServing";
        private const string cOtherCarbohydratePerActualServing = "OtherCarbohydratePerActualServing";
        private const string cOtherCarbohydrateActualDailyPercent = "OtherCarbohydrateActualDailyPercent";
        private const string cDietaryFiberPerServing = "DietaryFiberPerServing";
        private const string cDietaryFiberPerActualServing = "DietaryFiberPerActualServing";
        private const string cDietaryFiberActualDailyPercent = "DietaryFiberActualDailyPercent";
        private const string cSugarsPerServing = "SugarsPerServing";
        private const string cSugarsPerActualServing = "SugarsPerActualServing";
        private const string cProteinPerServing = "ProteinPerServing";
        private const string cProteinPerActualServing = "ProteinPerActualServing";
        private const string cProteinActualDailyPercent = "ProteinActualDailyPercent";
        private const string cVitaminAPercentDailyValue = "VitaminAPercentDailyValue";
        private const string cVitaminAPercentActualDailyValue = "VitaminAPercentActualDailyValue";
        private const string cVitaminCPercentDailyValue = "VitaminCPercentDailyValue";
        private const string cVitaminCPercentActualDailyValue = "VitaminCPercentActualDailyValue";
        private const string cVitaminDPercentDailyValue = "VitaminDPercentDailyValue";
        private const string cVitaminDPercentActualDailyValue = "VitaminDPercentActualDailyValue";
        private const string cCalciumPercentDailyValue = "CalciumPercentDailyValue";
        private const string cCalciumPercentActualDailyValue = "CalciumPercentActualDailyValue";
        private const string cIronPercentDailyValue = "IronPercentDailyValue";
        private const string cIronPercentActualDailyValue = "IronPercentActualDailyValue";
        private const string cThiaminPercentDailyValue = "ThiaminPercentDailyValue";
        private const string cThiaminPercentActualDailyValue = "ThiaminPercentActualDailyValue";
        private const string cFolatePercentDailyValue = "FolatePercentDailyValue";
        private const string cFolatePercentActualDailyValue = "FolatePercentActualDailyValue";
        private const string cRiboflavinPercentDailyValue = "RiboflavinPercentDailyValue";
        private const string cRiboflavinPercentActualDailyValue = "RiboflavinPercentActualDailyValue";
        private const string cNiacinPercentDailyValue = "NiacinPercentDailyValue";
        private const string cNiacinPercentActualDailyValue = "NiacinPercentActualDailyValue";
        private const string cVitaminB6PercentDailyValue = "VitaminB6PercentDailyValue";
        private const string cVitaminB6PercentActualDailyValue = "VitaminB6PercentActualDailyValue";
        private const string cVitaminB12PercentDailyValue = "VitaminB12PercentDailyValue";
        private const string cVitaminB12PercentActualDailyValue = "VitaminB12PercentActualDailyValue";
        private const string cPhosphorousPercentDailyValue = "PhosphorousPercentDailyValue";
        private const string cPhosphorousPercentActualDailyValue = "PhosphorousPercentActualDailyValue";
        private const string cMagnesiumPercentDailyValue = "MagnesiumPercentDailyValue";
        private const string cMagnesiumPercentActualDailyValue = "MagnesiumPercentActualDailyValue";
        private const string cZincPercentDailyValue = "ZincPercentDailyValue";
        private const string cZincPercentActualDailyValue = "ZincPercentActualDailyValue";

        
        public virtual void InitFoodFactProperties()
        {
            //avoid null references to properties
            this.MarketValue = 0;
            this.ContainerSize = 0;
            this.ServingCost = 0;
            this.ActualCaloriesPerDay = 0;
            this.ServingsPerContainer = 0;
            this.ActualServingsPerContainer = 0;
            this.GenderOfServingPerson = string.Empty;
            this.WeightOfServingPerson = 0;
            this.WeightUnitsOfServingPerson = string.Empty;
            this.CaloriesPerServing = 0;
            this.CaloriesPerActualServing = 0;
            this.CaloriesFromFatPerServing = 0;
            this.CaloriesFromFatPerActualServing = 0;
            this.TotalFatPerServing = 0;
            this.TotalFatPerActualServing = 0;
            this.TotalFatActualDailyPercent = 0;
            this.SaturatedFatPerServing = 0;
            this.SaturatedFatPerActualServing = 0;
            this.SaturatedFatActualDailyPercent = 0;
            this.TransFatPerServing = 0;
            this.TransFatPerActualServing = 0;
            this.CholesterolPerServing = 0;
            this.CholesterolPerActualServing = 0;
            this.CholesterolActualDailyPercent = 0;
            this.SodiumPerServing = 0;
            this.SodiumPerActualServing = 0;
            this.SodiumActualDailyPercent = 0;
            this.PotassiumPerServing = 0;
            this.PotassiumPerActualServing = 0;
            this.TotalCarbohydratePerServing = 0;
            this.TotalCarbohydratePerActualServing = 0;
            this.TotalCarbohydrateActualDailyPercent = 0;
            this.OtherCarbohydratePerServing = 0;
            this.OtherCarbohydratePerActualServing = 0;
            this.OtherCarbohydrateActualDailyPercent = 0;
            this.DietaryFiberPerServing = 0;
            this.DietaryFiberPerActualServing = 0;
            this.DietaryFiberActualDailyPercent = 0;
            this.SugarsPerServing = 0;
            this.SugarsPerActualServing = 0;
            this.ProteinPerServing = 0;
            this.ProteinPerActualServing = 0;
            this.ProteinActualDailyPercent = 0;
            this.VitaminAPercentDailyValue = 0;
            this.VitaminAPercentActualDailyValue = 0;
            this.VitaminCPercentDailyValue = 0;
            this.VitaminCPercentActualDailyValue = 0;
            this.VitaminDPercentDailyValue = 0;
            this.VitaminDPercentActualDailyValue = 0;
            this.CalciumPercentDailyValue = 0;
            this.CalciumPercentActualDailyValue = 0;
            this.IronPercentDailyValue = 0;
            this.IronPercentActualDailyValue = 0;
            this.ThiaminPercentDailyValue = 0;
            this.ThiaminPercentActualDailyValue = 0;
            this.FolatePercentDailyValue = 0;
            this.FolatePercentActualDailyValue = 0;
            this.RiboflavinPercentDailyValue = 0;
            this.RiboflavinPercentActualDailyValue = 0;
            this.NiacinPercentDailyValue = 0;
            this.NiacinPercentActualDailyValue = 0;
            this.VitaminB6PercentDailyValue = 0;
            this.VitaminB6PercentActualDailyValue = 0;
            this.VitaminB12PercentDailyValue = 0;
            this.VitaminB12PercentActualDailyValue = 0;
            this.PhosphorousPercentDailyValue = 0;
            this.PhosphorousPercentActualDailyValue = 0;
            this.MagnesiumPercentDailyValue = 0;
            this.MagnesiumPercentActualDailyValue = 0;
            this.ZincPercentDailyValue = 0;
            this.ZincPercentActualDailyValue = 0;
        }

        public virtual void CopyFoodFactProperties(
            FoodFactCalculator calculator)
        {
            this.MarketValue = calculator.MarketValue;
            this.ContainerSize = calculator.ContainerSize;
            this.ServingCost = calculator.ServingCost;
            this.ActualCaloriesPerDay = calculator.ActualCaloriesPerDay;
            this.ServingsPerContainer = calculator.ServingsPerContainer;
            this.ActualServingsPerContainer = calculator.ActualServingsPerContainer;
            this.GenderOfServingPerson = calculator.GenderOfServingPerson;
            this.WeightOfServingPerson = calculator.WeightOfServingPerson;
            this.WeightUnitsOfServingPerson = calculator.WeightUnitsOfServingPerson;
            this.CaloriesPerServing = calculator.CaloriesPerServing;
            this.CaloriesPerActualServing = calculator.CaloriesPerActualServing;
            this.CaloriesFromFatPerServing = calculator.CaloriesFromFatPerServing;
            this.CaloriesFromFatPerActualServing = calculator.CaloriesFromFatPerActualServing;
            this.TotalFatPerServing = calculator.TotalFatPerServing;
            this.TotalFatPerActualServing = calculator.TotalFatPerActualServing;
            this.TotalFatActualDailyPercent = calculator.TotalFatActualDailyPercent;
            this.SaturatedFatPerServing = calculator.SaturatedFatPerServing;
            this.SaturatedFatPerActualServing = calculator.SaturatedFatPerActualServing;
            this.SaturatedFatActualDailyPercent = calculator.SaturatedFatActualDailyPercent;
            this.TransFatPerServing = calculator.TransFatPerServing;
            this.TransFatPerActualServing = calculator.TransFatPerActualServing;
            this.CholesterolPerServing = calculator.CholesterolPerServing;
            this.CholesterolPerActualServing = calculator.CholesterolPerActualServing;
            this.CholesterolActualDailyPercent = calculator.CholesterolActualDailyPercent;
            this.SodiumPerServing = calculator.SodiumPerServing;
            this.SodiumPerActualServing = calculator.SodiumPerActualServing;
            this.SodiumActualDailyPercent = calculator.SodiumActualDailyPercent;
            this.PotassiumPerServing = calculator.PotassiumPerServing;
            this.PotassiumPerActualServing = calculator.PotassiumPerActualServing;
            this.TotalCarbohydratePerServing = calculator.TotalCarbohydratePerServing;
            this.TotalCarbohydratePerActualServing = calculator.TotalCarbohydratePerActualServing;
            this.TotalCarbohydrateActualDailyPercent = calculator.TotalCarbohydrateActualDailyPercent;
            this.OtherCarbohydratePerServing = calculator.OtherCarbohydratePerServing;
            this.OtherCarbohydratePerActualServing = calculator.OtherCarbohydratePerActualServing;
            this.OtherCarbohydrateActualDailyPercent = calculator.OtherCarbohydrateActualDailyPercent;
            this.DietaryFiberPerServing = calculator.DietaryFiberPerServing;
            this.DietaryFiberPerActualServing = calculator.DietaryFiberPerActualServing;
            this.DietaryFiberActualDailyPercent = calculator.DietaryFiberActualDailyPercent;
            this.SugarsPerServing = calculator.SugarsPerServing;
            this.SugarsPerActualServing = calculator.SugarsPerActualServing;
            this.ProteinPerServing = calculator.ProteinPerServing;
            this.ProteinPerActualServing = calculator.ProteinPerActualServing;
            this.ProteinActualDailyPercent = calculator.ProteinActualDailyPercent;
            this.VitaminAPercentDailyValue = calculator.VitaminAPercentDailyValue;
            this.VitaminAPercentActualDailyValue = calculator.VitaminAPercentActualDailyValue;
            this.VitaminCPercentDailyValue = calculator.VitaminCPercentDailyValue;
            this.VitaminCPercentActualDailyValue = calculator.VitaminCPercentActualDailyValue;
            this.VitaminDPercentDailyValue = calculator.VitaminDPercentDailyValue;
            this.VitaminDPercentActualDailyValue = calculator.VitaminDPercentActualDailyValue;
            this.CalciumPercentDailyValue = calculator.CalciumPercentDailyValue;
            this.CalciumPercentActualDailyValue = calculator.CalciumPercentActualDailyValue;
            this.IronPercentDailyValue = calculator.IronPercentDailyValue;
            this.IronPercentActualDailyValue = calculator.IronPercentActualDailyValue;
            this.ThiaminPercentDailyValue = calculator.ThiaminPercentDailyValue;
            this.ThiaminPercentActualDailyValue = calculator.ThiaminPercentActualDailyValue;
            this.FolatePercentDailyValue = calculator.FolatePercentDailyValue;
            this.FolatePercentActualDailyValue = calculator.FolatePercentActualDailyValue;
            this.RiboflavinPercentDailyValue = calculator.RiboflavinPercentDailyValue;
            this.RiboflavinPercentActualDailyValue = calculator.RiboflavinPercentActualDailyValue;
            this.NiacinPercentDailyValue = calculator.NiacinPercentDailyValue;
            this.NiacinPercentActualDailyValue = calculator.NiacinPercentActualDailyValue;
            this.VitaminB6PercentDailyValue = calculator.VitaminB6PercentDailyValue;
            this.VitaminB6PercentActualDailyValue = calculator.VitaminB6PercentActualDailyValue;
            this.VitaminB12PercentDailyValue = calculator.VitaminB12PercentDailyValue;
            this.VitaminB12PercentActualDailyValue = calculator.VitaminB12PercentActualDailyValue;
            this.PhosphorousPercentDailyValue = calculator.PhosphorousPercentDailyValue;
            this.PhosphorousPercentActualDailyValue = calculator.PhosphorousPercentActualDailyValue;
            this.MagnesiumPercentDailyValue = calculator.MagnesiumPercentDailyValue;
            this.MagnesiumPercentActualDailyValue = calculator.MagnesiumPercentActualDailyValue;
            this.ZincPercentDailyValue = calculator.ZincPercentDailyValue;
            this.ZincPercentActualDailyValue = calculator.ZincPercentActualDailyValue;
        }
        public virtual void SetFoodFactProperties(CalculatorParameters calcParameters,
            XElement calculator, XElement currentElement)
        {
            SetInputProperties(calcParameters, calculator,
                currentElement);
            SetCalculatorProperties(calculator);
            SetFoodFactProperties(calculator);
        }
        //set the class properties using the XElement
        public virtual void SetFoodFactProperties(XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            this.MarketValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cMarketValue);
            this.ContainerSize = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cContainerSize);
            this.ServingCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cServingCost);
            this.ActualCaloriesPerDay = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualCaloriesPerDay);
            this.ServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cServingsPerContainer);
            this.ActualServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualServingsPerContainer);
            this.GenderOfServingPerson = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGenderOfServingPerson);
            this.WeightOfServingPerson = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cWeightOfServingPerson);
            this.WeightUnitsOfServingPerson = CalculatorHelpers.GetAttribute(currentCalculationsElement, cWeightUnitsOfServingPerson);
            this.CaloriesPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCaloriesPerServing);
            this.CaloriesPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCaloriesPerActualServing);
            this.CaloriesFromFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCaloriesFromFatPerServing);
            this.CaloriesFromFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCaloriesFromFatPerActualServing);
            this.TotalFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTotalFatPerServing);
            this.TotalFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTotalFatPerActualServing);
            this.TotalFatActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTotalFatActualDailyPercent);
            this.SaturatedFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSaturatedFatPerServing);
            this.SaturatedFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSaturatedFatPerActualServing);
            this.SaturatedFatActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSaturatedFatActualDailyPercent);
            this.TransFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTransFatPerServing);
            this.TransFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTransFatPerActualServing);
            this.CholesterolPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCholesterolPerServing);
            this.CholesterolPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCholesterolPerActualServing);
            this.CholesterolActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCholesterolActualDailyPercent);
            this.SodiumPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSodiumPerServing);
            this.SodiumPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSodiumPerActualServing);
            this.SodiumActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSodiumActualDailyPercent);
            this.PotassiumPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cPotassiumPerServing);
            this.PotassiumPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cPotassiumPerActualServing);
            this.TotalCarbohydratePerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTotalCarbohydratePerServing);
            this.TotalCarbohydratePerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTotalCarbohydratePerActualServing);
            this.TotalCarbohydrateActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTotalCarbohydrateActualDailyPercent);
            this.OtherCarbohydratePerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOtherCarbohydratePerServing);
            this.OtherCarbohydratePerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOtherCarbohydratePerActualServing);
            this.OtherCarbohydrateActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOtherCarbohydrateActualDailyPercent);
            this.DietaryFiberPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cDietaryFiberPerServing);
            this.DietaryFiberPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cDietaryFiberPerActualServing);
            this.DietaryFiberActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cDietaryFiberActualDailyPercent);
            this.SugarsPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSugarsPerServing);
            this.SugarsPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSugarsPerActualServing);
            this.ProteinPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cProteinPerServing);
            this.ProteinPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cProteinPerActualServing);
            this.ProteinActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cProteinActualDailyPercent);
            this.VitaminAPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminAPercentDailyValue);
            this.VitaminAPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminAPercentActualDailyValue);
            this.VitaminCPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminCPercentDailyValue);
            this.VitaminCPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminCPercentActualDailyValue);
            this.VitaminDPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminDPercentDailyValue);
            this.VitaminDPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminDPercentActualDailyValue);
            this.CalciumPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCalciumPercentDailyValue);
            this.CalciumPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCalciumPercentActualDailyValue);
            this.IronPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIronPercentDailyValue);
            this.IronPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIronPercentActualDailyValue);
            this.ThiaminPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cThiaminPercentDailyValue);
            this.ThiaminPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cThiaminPercentActualDailyValue);
            this.FolatePercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFolatePercentDailyValue);
            this.FolatePercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFolatePercentActualDailyValue);
            this.RiboflavinPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cRiboflavinPercentDailyValue);
            this.RiboflavinPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cRiboflavinPercentActualDailyValue);
            this.NiacinPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cNiacinPercentDailyValue);
            this.NiacinPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cNiacinPercentActualDailyValue);
            this.VitaminB6PercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminB6PercentDailyValue);
            this.VitaminB6PercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminB6PercentActualDailyValue);
            this.VitaminB12PercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminB12PercentDailyValue);
            this.VitaminB12PercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVitaminB12PercentActualDailyValue);
            this.PhosphorousPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cPhosphorousPercentDailyValue);
            this.PhosphorousPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cPhosphorousPercentActualDailyValue);
            this.MagnesiumPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cMagnesiumPercentDailyValue);
            this.MagnesiumPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cMagnesiumPercentActualDailyValue);
            this.ZincPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cZincPercentDailyValue);
            this.ZincPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cZincPercentActualDailyValue);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetFoodFactProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cMarketValue:
                    this.MarketValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cContainerSize:
                    this.ContainerSize = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cServingCost:
                    this.ServingCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualCaloriesPerDay:
                    this.ActualCaloriesPerDay = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cServingsPerContainer:
                    this.ServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualServingsPerContainer:
                    this.ActualServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cGenderOfServingPerson:
                    this.GenderOfServingPerson = attValue;
                    break;
                case cWeightOfServingPerson:
                    this.WeightOfServingPerson = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWeightUnitsOfServingPerson:
                    this.WeightUnitsOfServingPerson = attValue;
                    break;
                case cCaloriesPerServing:
                    this.CaloriesPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCaloriesPerActualServing:
                    this.CaloriesPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCaloriesFromFatPerServing:
                    this.CaloriesFromFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCaloriesFromFatPerActualServing:
                    this.CaloriesFromFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalFatPerServing:
                    this.TotalFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalFatPerActualServing:
                    this.TotalFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalFatActualDailyPercent:
                    this.TotalFatActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSaturatedFatPerServing:
                    this.SaturatedFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSaturatedFatPerActualServing:
                    this.SaturatedFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSaturatedFatActualDailyPercent:
                    this.SaturatedFatActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTransFatPerServing:
                    this.TransFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTransFatPerActualServing:
                    this.TransFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCholesterolPerServing:
                    this.CholesterolPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCholesterolPerActualServing:
                    this.CholesterolPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCholesterolActualDailyPercent:
                    this.CholesterolActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSodiumPerServing:
                    this.SodiumPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSodiumPerActualServing:
                    this.SodiumPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSodiumActualDailyPercent:
                    this.SodiumActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPotassiumPerServing:
                    this.PotassiumPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPotassiumPerActualServing:
                    this.PotassiumPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCarbohydratePerServing:
                    this.TotalCarbohydratePerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCarbohydratePerActualServing:
                    this.TotalCarbohydratePerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCarbohydrateActualDailyPercent:
                    this.TotalCarbohydrateActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOtherCarbohydratePerServing:
                    this.OtherCarbohydratePerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOtherCarbohydratePerActualServing:
                    this.OtherCarbohydratePerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOtherCarbohydrateActualDailyPercent:
                    this.OtherCarbohydrateActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cDietaryFiberPerServing:
                    this.DietaryFiberPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cDietaryFiberPerActualServing:
                    this.DietaryFiberPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cDietaryFiberActualDailyPercent:
                    this.DietaryFiberActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSugarsPerServing:
                    this.SugarsPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSugarsPerActualServing:
                    this.SugarsPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cProteinPerServing:
                    this.ProteinPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cProteinPerActualServing:
                    this.ProteinPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cProteinActualDailyPercent:
                    this.ProteinActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminAPercentDailyValue:
                    this.VitaminAPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminAPercentActualDailyValue:
                    this.VitaminAPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminCPercentDailyValue:
                    this.VitaminCPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminCPercentActualDailyValue:
                    this.VitaminCPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminDPercentDailyValue:
                    this.VitaminDPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminDPercentActualDailyValue:
                    this.VitaminDPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalciumPercentDailyValue:
                    this.CalciumPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalciumPercentActualDailyValue:
                    this.CalciumPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIronPercentDailyValue:
                    this.IronPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIronPercentActualDailyValue:
                    this.IronPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cThiaminPercentDailyValue:
                    this.ThiaminPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cThiaminPercentActualDailyValue:
                    this.ThiaminPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFolatePercentDailyValue:
                    this.FolatePercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFolatePercentActualDailyValue:
                    this.FolatePercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRiboflavinPercentDailyValue:
                    this.RiboflavinPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRiboflavinPercentActualDailyValue:
                    this.RiboflavinPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cNiacinPercentDailyValue:
                    this.NiacinPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cNiacinPercentActualDailyValue:
                    this.NiacinPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminB6PercentDailyValue:
                    this.VitaminB6PercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminB6PercentActualDailyValue:
                    this.VitaminB6PercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminB12PercentDailyValue:
                    this.VitaminB12PercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVitaminB12PercentActualDailyValue:
                    this.VitaminB12PercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPhosphorousPercentDailyValue:
                    this.PhosphorousPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPhosphorousPercentActualDailyValue:
                    this.PhosphorousPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cMagnesiumPercentDailyValue:
                    this.MagnesiumPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cMagnesiumPercentActualDailyValue:
                    this.MagnesiumPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cZincPercentDailyValue:
                    this.ZincPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cZincPercentActualDailyValue:
                    this.ZincPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetFoodFactAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            //don't set any input attributes; each calculator should set what's needed separately
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cMarketValue, attNameExtension), this.MarketValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cContainerSize, attNameExtension), this.ContainerSize);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cServingCost, attNameExtension), this.ServingCost);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cGenderOfServingPerson, attNameExtension), this.GenderOfServingPerson);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(cWeightOfServingPerson, attNameExtension), this.WeightOfServingPerson);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cWeightUnitsOfServingPerson, attNameExtension), this.WeightUnitsOfServingPerson);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(cActualCaloriesPerDay, attNameExtension), this.ActualCaloriesPerDay);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cServingsPerContainer, attNameExtension), this.ServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualServingsPerContainer, attNameExtension), this.ActualServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCaloriesPerServing, attNameExtension), this.CaloriesPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCaloriesPerActualServing, attNameExtension), this.CaloriesPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCaloriesFromFatPerServing, attNameExtension), this.CaloriesFromFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCaloriesFromFatPerActualServing, attNameExtension), this.CaloriesFromFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTotalFatPerServing, attNameExtension), this.TotalFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTotalFatPerActualServing, attNameExtension), this.TotalFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTotalFatActualDailyPercent, attNameExtension), this.TotalFatActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSaturatedFatPerServing, attNameExtension), this.SaturatedFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSaturatedFatPerActualServing, attNameExtension), this.SaturatedFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSaturatedFatActualDailyPercent, attNameExtension), this.SaturatedFatActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTransFatPerServing, attNameExtension), this.TransFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTransFatPerActualServing, attNameExtension), this.TransFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCholesterolPerServing, attNameExtension), this.CholesterolPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCholesterolPerActualServing, attNameExtension), this.CholesterolPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCholesterolActualDailyPercent, attNameExtension), this.CholesterolActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSodiumPerServing, attNameExtension), this.SodiumPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSodiumPerActualServing, attNameExtension), this.SodiumPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSodiumActualDailyPercent, attNameExtension), this.SodiumActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cPotassiumPerServing, attNameExtension), this.PotassiumPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cPotassiumPerActualServing, attNameExtension), this.PotassiumPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTotalCarbohydratePerServing, attNameExtension), this.TotalCarbohydratePerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTotalCarbohydratePerActualServing, attNameExtension), this.TotalCarbohydratePerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTotalCarbohydrateActualDailyPercent, attNameExtension), this.TotalCarbohydrateActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOtherCarbohydratePerServing, attNameExtension), this.OtherCarbohydratePerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOtherCarbohydratePerActualServing, attNameExtension), this.OtherCarbohydratePerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOtherCarbohydrateActualDailyPercent, attNameExtension), this.OtherCarbohydrateActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cDietaryFiberPerServing, attNameExtension), this.DietaryFiberPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cDietaryFiberPerActualServing, attNameExtension), this.DietaryFiberPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cDietaryFiberActualDailyPercent, attNameExtension), this.DietaryFiberActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSugarsPerServing, attNameExtension), this.SugarsPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSugarsPerActualServing, attNameExtension), this.SugarsPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cProteinPerServing, attNameExtension), this.ProteinPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cProteinPerActualServing, attNameExtension), this.ProteinPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cProteinActualDailyPercent, attNameExtension), this.ProteinActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminAPercentDailyValue, attNameExtension), this.VitaminAPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminAPercentActualDailyValue, attNameExtension), this.VitaminAPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminCPercentDailyValue, attNameExtension), this.VitaminCPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminCPercentActualDailyValue, attNameExtension), this.VitaminCPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminDPercentDailyValue, attNameExtension), this.VitaminDPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminDPercentActualDailyValue, attNameExtension), this.VitaminDPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCalciumPercentDailyValue, attNameExtension), this.CalciumPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCalciumPercentActualDailyValue, attNameExtension), this.CalciumPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIronPercentDailyValue, attNameExtension), this.IronPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIronPercentActualDailyValue, attNameExtension), this.IronPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cThiaminPercentDailyValue, attNameExtension), this.ThiaminPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cThiaminPercentActualDailyValue, attNameExtension), this.ThiaminPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFolatePercentDailyValue, attNameExtension), this.FolatePercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFolatePercentActualDailyValue, attNameExtension), this.FolatePercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cRiboflavinPercentDailyValue, attNameExtension), this.RiboflavinPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cRiboflavinPercentActualDailyValue, attNameExtension), this.RiboflavinPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cNiacinPercentDailyValue, attNameExtension), this.NiacinPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cNiacinPercentActualDailyValue, attNameExtension), this.NiacinPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminB6PercentDailyValue, attNameExtension), this.VitaminB6PercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminB6PercentActualDailyValue, attNameExtension), this.VitaminB6PercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cVitaminB12PercentDailyValue, attNameExtension), this.VitaminB12PercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVitaminB12PercentActualDailyValue, attNameExtension), this.VitaminB12PercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cPhosphorousPercentDailyValue, attNameExtension), this.PhosphorousPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cPhosphorousPercentActualDailyValue, attNameExtension), this.PhosphorousPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cMagnesiumPercentDailyValue, attNameExtension), this.MagnesiumPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cMagnesiumPercentActualDailyValue, attNameExtension), this.MagnesiumPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cZincPercentDailyValue, attNameExtension), this.ZincPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cZincPercentActualDailyValue, attNameExtension), this.ZincPercentActualDailyValue);
        }
        public virtual void SetFoodFactAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(cMarketValue, attNameExtension), this.MarketValue.ToString());
            writer.WriteAttributeString(string.Concat(cContainerSize, attNameExtension), this.ContainerSize.ToString());
            writer.WriteAttributeString(string.Concat(cServingCost, attNameExtension), this.ServingCost.ToString());
            writer.WriteAttributeString(string.Concat(cGenderOfServingPerson, attNameExtension), this.GenderOfServingPerson.ToString());
            writer.WriteAttributeString(string.Concat(cWeightOfServingPerson, attNameExtension), this.WeightOfServingPerson.ToString());
            writer.WriteAttributeString(string.Concat(cWeightUnitsOfServingPerson, attNameExtension), this.WeightUnitsOfServingPerson.ToString());
            writer.WriteAttributeString(string.Concat(cActualCaloriesPerDay, attNameExtension), this.ActualCaloriesPerDay.ToString());
            writer.WriteAttributeString(string.Concat(cServingsPerContainer, attNameExtension), this.ServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(cActualServingsPerContainer, attNameExtension), this.ActualServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(cCaloriesPerServing, attNameExtension), this.CaloriesPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cCaloriesPerActualServing, attNameExtension), this.CaloriesPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cCaloriesFromFatPerServing, attNameExtension), this.CaloriesFromFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cCaloriesFromFatPerActualServing, attNameExtension), this.CaloriesFromFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cTotalFatPerServing, attNameExtension), this.TotalFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cTotalFatPerActualServing, attNameExtension), this.TotalFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cTotalFatActualDailyPercent, attNameExtension), this.TotalFatActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cSaturatedFatPerServing, attNameExtension), this.SaturatedFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cSaturatedFatPerActualServing, attNameExtension), this.SaturatedFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cSaturatedFatActualDailyPercent, attNameExtension), this.SaturatedFatActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cTransFatPerServing, attNameExtension), this.TransFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cTransFatPerActualServing, attNameExtension), this.TransFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cCholesterolPerServing, attNameExtension), this.CholesterolPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cCholesterolPerActualServing, attNameExtension), this.CholesterolPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cCholesterolActualDailyPercent, attNameExtension), this.CholesterolActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cSodiumPerServing, attNameExtension), this.SodiumPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cSodiumPerActualServing, attNameExtension), this.SodiumPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cSodiumActualDailyPercent, attNameExtension), this.SodiumActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cPotassiumPerServing, attNameExtension), this.PotassiumPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cPotassiumPerActualServing, attNameExtension), this.PotassiumPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cTotalCarbohydratePerServing, attNameExtension), this.TotalCarbohydratePerServing.ToString());
            writer.WriteAttributeString(string.Concat(cTotalCarbohydratePerActualServing, attNameExtension), this.TotalCarbohydratePerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cTotalCarbohydrateActualDailyPercent, attNameExtension), this.TotalCarbohydrateActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cOtherCarbohydratePerServing, attNameExtension), this.OtherCarbohydratePerServing.ToString());
            writer.WriteAttributeString(string.Concat(cOtherCarbohydratePerActualServing, attNameExtension), this.OtherCarbohydratePerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cOtherCarbohydrateActualDailyPercent, attNameExtension), this.OtherCarbohydrateActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cDietaryFiberPerServing, attNameExtension), this.DietaryFiberPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cDietaryFiberPerActualServing, attNameExtension), this.DietaryFiberPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cDietaryFiberActualDailyPercent, attNameExtension), this.DietaryFiberActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cSugarsPerServing, attNameExtension), this.SugarsPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cSugarsPerActualServing, attNameExtension), this.SugarsPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cProteinPerServing, attNameExtension), this.ProteinPerServing.ToString());
            writer.WriteAttributeString(string.Concat(cProteinPerActualServing, attNameExtension), this.ProteinPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(cProteinActualDailyPercent, attNameExtension), this.ProteinActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminAPercentDailyValue, attNameExtension), this.VitaminAPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminAPercentActualDailyValue, attNameExtension), this.VitaminAPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminCPercentDailyValue, attNameExtension), this.VitaminCPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminCPercentActualDailyValue, attNameExtension), this.VitaminCPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminDPercentDailyValue, attNameExtension), this.VitaminDPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminDPercentActualDailyValue, attNameExtension), this.VitaminDPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cCalciumPercentDailyValue, attNameExtension), this.CalciumPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cCalciumPercentActualDailyValue, attNameExtension), this.CalciumPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cIronPercentDailyValue, attNameExtension), this.IronPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cIronPercentActualDailyValue, attNameExtension), this.IronPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cThiaminPercentDailyValue, attNameExtension), this.ThiaminPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cThiaminPercentActualDailyValue, attNameExtension), this.ThiaminPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cFolatePercentDailyValue, attNameExtension), this.FolatePercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cFolatePercentActualDailyValue, attNameExtension), this.FolatePercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cRiboflavinPercentDailyValue, attNameExtension), this.RiboflavinPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cRiboflavinPercentActualDailyValue, attNameExtension), this.RiboflavinPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cNiacinPercentDailyValue, attNameExtension), this.NiacinPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cNiacinPercentActualDailyValue, attNameExtension), this.NiacinPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminB6PercentDailyValue, attNameExtension), this.VitaminB6PercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminB6PercentActualDailyValue, attNameExtension), this.VitaminB6PercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminB12PercentDailyValue, attNameExtension), this.VitaminB12PercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cVitaminB12PercentActualDailyValue, attNameExtension), this.VitaminB12PercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cPhosphorousPercentDailyValue, attNameExtension), this.PhosphorousPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cPhosphorousPercentActualDailyValue, attNameExtension), this.PhosphorousPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cMagnesiumPercentDailyValue, attNameExtension), this.MagnesiumPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cMagnesiumPercentActualDailyValue, attNameExtension), this.MagnesiumPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cZincPercentDailyValue, attNameExtension), this.ZincPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(cZincPercentActualDailyValue, attNameExtension), this.ZincPercentActualDailyValue.ToString());
        }
    }
}
