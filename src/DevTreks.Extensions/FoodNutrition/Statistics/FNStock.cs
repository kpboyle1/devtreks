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
    ///Purpose:		The FNStock class extends the FoodNutritionCalculator() 
    ///             class and is used by food nutrition calculators and analyzers 
    ///             to set totals and basic food nutrition statistics. Basic 
    ///             food nutrition statistical objects derive from this class 
    ///             to support further statistical analysis.
    ///Author:		www.devtreks.org
    ///Date:		2011, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    public class FNStock : FoodFactCalculator
    {
        //calls the base-class version, and initializes the base class properties.
        public FNStock()
            : base()
        {
            //base input
            InitCalculatorProperties();
            InitTotalBenefitsProperties();
            InitTotalCostsProperties();
            //food fact
            InitTotalFNStockProperties();
        }
        //copy constructor
        public FNStock(FNStock calculator)
            : base(calculator)
        {
            CopyTotalFNStockProperties(calculator);
        }

        //calculator properties
        //food nutrition collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<FoodFactCalculator>> FoodFacts = null;

        //food fact totals
        public double TotalMarketValue { get; set; }
        public double TotalContainerSize { get; set; }
        public double TotalServingCost { get; set; }
        public double TotalActualCaloriesPerDay { get; set; }
        public double TotalServingsPerContainer { get; set; }
        public double TotalActualServingsPerContainer { get; set; }
        public double TotalGenderOfServingPerson { get; set; }
        public double TotalWeightOfServingPerson { get; set; }
        public double TotalCaloriesPerServing { get; set; }
        public double TotalCaloriesPerActualServing { get; set; }
        public double TotalCaloriesFromFatPerServing { get; set; }
        public double TotalCaloriesFromFatPerActualServing { get; set; }
        public double TotalTotalFatPerServing { get; set; }
        public double TotalTotalFatPerActualServing { get; set; }
        public double TotalTotalFatActualDailyPercent { get; set; }
        public double TotalSaturatedFatPerServing { get; set; }
        public double TotalSaturatedFatPerActualServing { get; set; }
        public double TotalSaturatedFatActualDailyPercent { get; set; }
        public double TotalTransFatPerServing { get; set; }
        public double TotalTransFatPerActualServing { get; set; }
        public double TotalCholesterolPerServing { get; set; }
        public double TotalCholesterolPerActualServing { get; set; }
        public double TotalCholesterolActualDailyPercent { get; set; }
        public double TotalSodiumPerServing { get; set; }
        public double TotalSodiumPerActualServing { get; set; }
        public double TotalSodiumActualDailyPercent { get; set; }
        public double TotalPotassiumPerServing { get; set; }
        public double TotalPotassiumPerActualServing { get; set; }
        public double TotalTotalCarbohydratePerServing { get; set; }
        public double TotalTotalCarbohydratePerActualServing { get; set; }
        public double TotalTotalCarbohydrateActualDailyPercent { get; set; }
        public double TotalOtherCarbohydratePerServing { get; set; }
        public double TotalOtherCarbohydratePerActualServing { get; set; }
        public double TotalOtherCarbohydrateActualDailyPercent { get; set; }
        public double TotalDietaryFiberPerServing { get; set; }
        public double TotalDietaryFiberPerActualServing { get; set; }
        public double TotalDietaryFiberActualDailyPercent { get; set; }
        public double TotalSugarsPerServing { get; set; }
        public double TotalSugarsPerActualServing { get; set; }
        public double TotalProteinPerServing { get; set; }
        public double TotalProteinPerActualServing { get; set; }
        public double TotalProteinActualDailyPercent { get; set; }
        public double TotalVitaminAPercentDailyValue { get; set; }
        public double TotalVitaminAPercentActualDailyValue { get; set; }
        public double TotalVitaminCPercentDailyValue { get; set; }
        public double TotalVitaminCPercentActualDailyValue { get; set; }
        public double TotalVitaminDPercentDailyValue { get; set; }
        public double TotalVitaminDPercentActualDailyValue { get; set; }
        public double TotalCalciumPercentDailyValue { get; set; }
        public double TotalCalciumPercentActualDailyValue { get; set; }
        public double TotalIronPercentDailyValue { get; set; }
        public double TotalIronPercentActualDailyValue { get; set; }
        public double TotalThiaminPercentDailyValue { get; set; }
        public double TotalThiaminPercentActualDailyValue { get; set; }
        public double TotalFolatePercentDailyValue { get; set; }
        public double TotalFolatePercentActualDailyValue { get; set; }
        public double TotalRiboflavinPercentDailyValue { get; set; }
        public double TotalRiboflavinPercentActualDailyValue { get; set; }
        public double TotalNiacinPercentDailyValue { get; set; }
        public double TotalNiacinPercentActualDailyValue { get; set; }
        public double TotalVitaminB6PercentDailyValue { get; set; }
        public double TotalVitaminB6PercentActualDailyValue { get; set; }
        public double TotalVitaminB12PercentDailyValue { get; set; }
        public double TotalVitaminB12PercentActualDailyValue { get; set; }
        public double TotalPhosphorousPercentDailyValue { get; set; }
        public double TotalPhosphorousPercentActualDailyValue { get; set; }
        public double TotalMagnesiumPercentDailyValue { get; set; }
        public double TotalMagnesiumPercentActualDailyValue { get; set; }
        public double TotalZincPercentDailyValue { get; set; }
        public double TotalZincPercentActualDailyValue { get; set; }

        private const string TMarketValue = "TMarketValue";
        private const string TContainerSize = "TContainerSize";
        private const string TServingCost = "TServingCost";
        private const string TActualCaloriesPerDay = "TActualCaloriesPerDay";
        private const string TGenderOfServingPerson = "TGenderOfServingPerson";
        private const string TWeightOfServingPerson = "TWeightOfServingPerson";
        private const string TWeightUnitsOfServingPerson = "TWeightUnitsOfServingPerson";
        private const string TServingsPerContainer = "TServingsPerContainer";
        private const string TActualServingsPerContainer = "TActualServingsPerContainer";
        private const string TCaloriesPerServing = "TCaloriesPerServing";
        private const string TCaloriesPerActualServing = "TCaloriesPerActualServing";
        private const string TCaloriesFromFatPerServing = "TCaloriesFromFatPerServing";
        private const string TCaloriesFromFatPerActualServing = "TCaloriesFromFatPerActualServing";
        private const string TTotalFatPerServing = "TTotalFatPerServing";
        private const string TTotalFatPerActualServing = "TTotalFatPerActualServing";
        private const string TTotalFatActualDailyPercent = "TTotalFatActualDailyPercent";
        private const string TSaturatedFatPerServing = "TSaturatedFatPerServing";
        private const string TSaturatedFatPerActualServing = "TSaturatedFatPerActualServing";
        private const string TSaturatedFatActualDailyPercent = "TSaturatedFatActualDailyPercent";
        private const string TTransFatPerServing = "TTransFatPerServing";
        private const string TTransFatPerActualServing = "TTransFatPerActualServing";
        private const string TCholesterolPerServing = "TCholesterolPerServing";
        private const string TCholesterolPerActualServing = "TCholesterolPerActualServing";
        private const string TCholesterolActualDailyPercent = "TCholesterolActualDailyPercent";
        private const string TSodiumPerServing = "TSodiumPerServing";
        private const string TSodiumPerActualServing = "TSodiumPerActualServing";
        private const string TSodiumActualDailyPercent = "TSodiumActualDailyPercent";
        private const string TPotassiumPerServing = "TPotassiumPerServing";
        private const string TPotassiumPerActualServing = "TPotassiumPerActualServing";
        private const string TTotalCarbohydratePerServing = "TTotalCarbohydratePerServing";
        private const string TTotalCarbohydratePerActualServing = "TTotalCarbohydratePerActualServing";
        private const string TTotalCarbohydrateActualDailyPercent = "TTotalCarbohydrateActualDailyPercent";
        private const string TOtherCarbohydratePerServing = "TOtherCarbohydratePerServing";
        private const string TOtherCarbohydratePerActualServing = "TOtherCarbohydratePerActualServing";
        private const string TOtherCarbohydrateActualDailyPercent = "TOtherCarbohydrateActualDailyPercent";
        private const string TDietaryFiberPerServing = "TDietaryFiberPerServing";
        private const string TDietaryFiberPerActualServing = "TDietaryFiberPerActualServing";
        private const string TDietaryFiberActualDailyPercent = "TDietaryFiberActualDailyPercent";
        private const string TSugarsPerServing = "TSugarsPerServing";
        private const string TSugarsPerActualServing = "TSugarsPerActualServing";
        private const string TProteinPerServing = "TProteinPerServing";
        private const string TProteinPerActualServing = "TProteinPerActualServing";
        private const string TProteinActualDailyPercent = "TProteinActualDailyPercent";
        private const string TVitaminAPercentDailyValue = "TVitaminAPercentDailyValue";
        private const string TVitaminAPercentActualDailyValue = "TVitaminAPercentActualDailyValue";
        private const string TVitaminCPercentDailyValue = "TVitaminCPercentDailyValue";
        private const string TVitaminCPercentActualDailyValue = "TVitaminCPercentActualDailyValue";
        private const string TVitaminDPercentDailyValue = "TVitaminDPercentDailyValue";
        private const string TVitaminDPercentActualDailyValue = "TVitaminDPercentActualDailyValue";
        private const string TCalciumPercentDailyValue = "TCalciumPercentDailyValue";
        private const string TCalciumPercentActualDailyValue = "TCalciumPercentActualDailyValue";
        private const string TIronPercentDailyValue = "TIronPercentDailyValue";
        private const string TIronPercentActualDailyValue = "TIronPercentActualDailyValue";
        private const string TThiaminPercentDailyValue = "TThiaminPercentDailyValue";
        private const string TThiaminPercentActualDailyValue = "TThiaminPercentActualDailyValue";
        private const string TFolatePercentDailyValue = "TFolatePercentDailyValue";
        private const string TFolatePercentActualDailyValue = "TFolatePercentActualDailyValue";
        private const string TRiboflavinPercentDailyValue = "TRiboflavinPercentDailyValue";
        private const string TRiboflavinPercentActualDailyValue = "TRiboflavinPercentActualDailyValue";
        private const string TNiacinPercentDailyValue = "TNiacinPercentDailyValue";
        private const string TNiacinPercentActualDailyValue = "TNiacinPercentActualDailyValue";
        private const string TVitaminB6PercentDailyValue = "TVitaminB6PercentDailyValue";
        private const string TVitaminB6PercentActualDailyValue = "TVitaminB6PercentActualDailyValue";
        private const string TVitaminB12PercentDailyValue = "TVitaminB12PercentDailyValue";
        private const string TVitaminB12PercentActualDailyValue = "TVitaminB12PercentActualDailyValue";
        private const string TPhosphorousPercentDailyValue = "TPhosphorousPercentDailyValue";
        private const string TPhosphorousPercentActualDailyValue = "TPhosphorousPercentActualDailyValue";
        private const string TMagnesiumPercentDailyValue = "TMagnesiumPercentDailyValue";
        private const string TMagnesiumPercentActualDailyValue = "TMagnesiumPercentActualDailyValue";
        private const string TZincPercentDailyValue = "TZincPercentDailyValue";
        private const string TZincPercentActualDailyValue = "TZincPercentActualDailyValue";

        public virtual void InitTotalFNStockProperties()
        {
            //avoid null references to properties
            this.TotalMarketValue = 0;
            this.TotalContainerSize = 0;
            this.TotalServingCost = 0;
            this.TotalActualCaloriesPerDay = 0;
            this.TotalServingsPerContainer = 0;
            this.TotalActualServingsPerContainer = 0;
            this.TotalWeightOfServingPerson = 0;
            this.TotalGenderOfServingPerson = 0;
            this.TotalCaloriesPerServing = 0;
            this.TotalCaloriesPerActualServing = 0;
            this.TotalCaloriesFromFatPerServing = 0;
            this.TotalCaloriesFromFatPerActualServing = 0;
            this.TotalTotalFatPerServing = 0;
            this.TotalTotalFatPerActualServing = 0;
            this.TotalTotalFatActualDailyPercent = 0;
            this.TotalSaturatedFatPerServing = 0;
            this.TotalSaturatedFatPerActualServing = 0;
            this.TotalSaturatedFatActualDailyPercent = 0;
            this.TotalTransFatPerServing = 0;
            this.TotalTransFatPerActualServing = 0;
            this.TotalCholesterolPerServing = 0;
            this.TotalCholesterolPerActualServing = 0;
            this.TotalCholesterolActualDailyPercent = 0;
            this.TotalSodiumPerServing = 0;
            this.TotalSodiumPerActualServing = 0;
            this.TotalSodiumActualDailyPercent = 0;
            this.TotalPotassiumPerServing = 0;
            this.TotalPotassiumPerActualServing = 0;
            this.TotalTotalCarbohydratePerServing = 0;
            this.TotalTotalCarbohydratePerActualServing = 0;
            this.TotalTotalCarbohydrateActualDailyPercent = 0;
            this.TotalOtherCarbohydratePerServing = 0;
            this.TotalOtherCarbohydratePerActualServing = 0;
            this.TotalOtherCarbohydrateActualDailyPercent = 0;
            this.TotalDietaryFiberPerServing = 0;
            this.TotalDietaryFiberPerActualServing = 0;
            this.TotalDietaryFiberActualDailyPercent = 0;
            this.TotalSugarsPerServing = 0;
            this.TotalSugarsPerActualServing = 0;
            this.TotalProteinPerServing = 0;
            this.TotalProteinPerActualServing = 0;
            this.TotalProteinActualDailyPercent = 0;
            this.TotalVitaminAPercentDailyValue = 0;
            this.TotalVitaminAPercentActualDailyValue = 0;
            this.TotalVitaminCPercentDailyValue = 0;
            this.TotalVitaminCPercentActualDailyValue = 0;
            this.TotalVitaminDPercentDailyValue = 0;
            this.TotalVitaminDPercentActualDailyValue = 0;
            this.TotalCalciumPercentDailyValue = 0;
            this.TotalCalciumPercentActualDailyValue = 0;
            this.TotalIronPercentDailyValue = 0;
            this.TotalIronPercentActualDailyValue = 0;
            this.TotalThiaminPercentDailyValue = 0;
            this.TotalThiaminPercentActualDailyValue = 0;
            this.TotalFolatePercentDailyValue = 0;
            this.TotalFolatePercentActualDailyValue = 0;
            this.TotalRiboflavinPercentDailyValue = 0;
            this.TotalRiboflavinPercentActualDailyValue = 0;
            this.TotalNiacinPercentDailyValue = 0;
            this.TotalNiacinPercentActualDailyValue = 0;
            this.TotalVitaminB6PercentDailyValue = 0;
            this.TotalVitaminB6PercentActualDailyValue = 0;
            this.TotalVitaminB12PercentDailyValue = 0;
            this.TotalVitaminB12PercentActualDailyValue = 0;
            this.TotalPhosphorousPercentDailyValue = 0;
            this.TotalPhosphorousPercentActualDailyValue = 0;
            this.TotalMagnesiumPercentDailyValue = 0;
            this.TotalMagnesiumPercentActualDailyValue = 0;
            this.TotalZincPercentDailyValue = 0;
            this.TotalZincPercentActualDailyValue = 0;
        }
        public virtual void CopyTotalFNStockProperties(
            FNStock calculator)
        {
            this.TotalMarketValue = calculator.TotalMarketValue;
            this.TotalContainerSize = calculator.TotalContainerSize;
            this.TotalServingCost = calculator.TotalServingCost;
            this.TotalActualCaloriesPerDay = calculator.TotalActualCaloriesPerDay;
            this.TotalServingsPerContainer = calculator.TotalServingsPerContainer;
            this.TotalActualServingsPerContainer = calculator.TotalActualServingsPerContainer;
            this.TotalGenderOfServingPerson = calculator.TotalGenderOfServingPerson;
            this.TotalWeightOfServingPerson = calculator.TotalWeightOfServingPerson;
            this.TotalCaloriesPerServing = calculator.TotalCaloriesPerServing;
            this.TotalCaloriesPerActualServing = calculator.TotalCaloriesPerActualServing;
            this.TotalCaloriesFromFatPerServing = calculator.TotalCaloriesFromFatPerServing;
            this.TotalCaloriesFromFatPerActualServing = calculator.TotalCaloriesFromFatPerActualServing;
            this.TotalTotalFatPerServing = calculator.TotalTotalFatPerServing;
            this.TotalTotalFatPerActualServing = calculator.TotalTotalFatPerActualServing;
            this.TotalTotalFatActualDailyPercent = calculator.TotalTotalFatActualDailyPercent;
            this.TotalSaturatedFatPerServing = calculator.TotalSaturatedFatPerServing;
            this.TotalSaturatedFatPerActualServing = calculator.TotalSaturatedFatPerActualServing;
            this.TotalSaturatedFatActualDailyPercent = calculator.TotalSaturatedFatActualDailyPercent;
            this.TotalTransFatPerServing = calculator.TotalTransFatPerServing;
            this.TotalTransFatPerActualServing = calculator.TotalTransFatPerActualServing;
            this.TotalCholesterolPerServing = calculator.TotalCholesterolPerServing;
            this.TotalCholesterolPerActualServing = calculator.TotalCholesterolPerActualServing;
            this.TotalCholesterolActualDailyPercent = calculator.TotalCholesterolActualDailyPercent;
            this.TotalSodiumPerServing = calculator.TotalSodiumPerServing;
            this.TotalSodiumPerActualServing = calculator.TotalSodiumPerActualServing;
            this.TotalSodiumActualDailyPercent = calculator.TotalSodiumActualDailyPercent;
            this.TotalPotassiumPerServing = calculator.TotalPotassiumPerServing;
            this.TotalPotassiumPerActualServing = calculator.TotalPotassiumPerActualServing;
            this.TotalTotalCarbohydratePerServing = calculator.TotalTotalCarbohydratePerServing;
            this.TotalTotalCarbohydratePerActualServing = calculator.TotalTotalCarbohydratePerActualServing;
            this.TotalTotalCarbohydrateActualDailyPercent = calculator.TotalTotalCarbohydrateActualDailyPercent;
            this.TotalOtherCarbohydratePerServing = calculator.TotalOtherCarbohydratePerServing;
            this.TotalOtherCarbohydratePerActualServing = calculator.TotalOtherCarbohydratePerActualServing;
            this.TotalOtherCarbohydrateActualDailyPercent = calculator.TotalOtherCarbohydrateActualDailyPercent;
            this.TotalDietaryFiberPerServing = calculator.TotalDietaryFiberPerServing;
            this.TotalDietaryFiberPerActualServing = calculator.TotalDietaryFiberPerActualServing;
            this.TotalDietaryFiberActualDailyPercent = calculator.TotalDietaryFiberActualDailyPercent;
            this.TotalSugarsPerServing = calculator.TotalSugarsPerServing;
            this.TotalSugarsPerActualServing = calculator.TotalSugarsPerActualServing;
            this.TotalProteinPerServing = calculator.TotalProteinPerServing;
            this.TotalProteinPerActualServing = calculator.TotalProteinPerActualServing;
            this.TotalProteinActualDailyPercent = calculator.TotalProteinActualDailyPercent;
            this.TotalVitaminAPercentDailyValue = calculator.TotalVitaminAPercentDailyValue;
            this.TotalVitaminAPercentActualDailyValue = calculator.TotalVitaminAPercentActualDailyValue;
            this.TotalVitaminCPercentDailyValue = calculator.TotalVitaminCPercentDailyValue;
            this.TotalVitaminCPercentActualDailyValue = calculator.TotalVitaminCPercentActualDailyValue;
            this.TotalVitaminDPercentDailyValue = calculator.TotalVitaminDPercentDailyValue;
            this.TotalVitaminDPercentActualDailyValue = calculator.TotalVitaminDPercentActualDailyValue;
            this.TotalCalciumPercentDailyValue = calculator.TotalCalciumPercentDailyValue;
            this.TotalCalciumPercentActualDailyValue = calculator.TotalCalciumPercentActualDailyValue;
            this.TotalIronPercentDailyValue = calculator.TotalIronPercentDailyValue;
            this.TotalIronPercentActualDailyValue = calculator.TotalIronPercentActualDailyValue;
            this.TotalThiaminPercentDailyValue = calculator.TotalThiaminPercentDailyValue;
            this.TotalThiaminPercentActualDailyValue = calculator.TotalThiaminPercentActualDailyValue;
            this.TotalFolatePercentDailyValue = calculator.TotalFolatePercentDailyValue;
            this.TotalFolatePercentActualDailyValue = calculator.TotalFolatePercentActualDailyValue;
            this.TotalRiboflavinPercentDailyValue = calculator.TotalRiboflavinPercentDailyValue;
            this.TotalRiboflavinPercentActualDailyValue = calculator.TotalRiboflavinPercentActualDailyValue;
            this.TotalNiacinPercentDailyValue = calculator.TotalNiacinPercentDailyValue;
            this.TotalNiacinPercentActualDailyValue = calculator.TotalNiacinPercentActualDailyValue;
            this.TotalVitaminB6PercentDailyValue = calculator.TotalVitaminB6PercentDailyValue;
            this.TotalVitaminB6PercentActualDailyValue = calculator.TotalVitaminB6PercentActualDailyValue;
            this.TotalVitaminB12PercentDailyValue = calculator.TotalVitaminB12PercentDailyValue;
            this.TotalVitaminB12PercentActualDailyValue = calculator.TotalVitaminB12PercentActualDailyValue;
            this.TotalPhosphorousPercentDailyValue = calculator.TotalPhosphorousPercentDailyValue;
            this.TotalPhosphorousPercentActualDailyValue = calculator.TotalPhosphorousPercentActualDailyValue;
            this.TotalMagnesiumPercentDailyValue = calculator.TotalMagnesiumPercentDailyValue;
            this.TotalMagnesiumPercentActualDailyValue = calculator.TotalMagnesiumPercentActualDailyValue;
            this.TotalZincPercentDailyValue = calculator.TotalZincPercentDailyValue;
            this.TotalZincPercentActualDailyValue = calculator.TotalZincPercentActualDailyValue;
        }
        //set the class properties using the XElement
        public virtual void SetTotalFNStockProperties(XElement currentCalculationsElement)
        {
            //set the calculator properties
            this.TotalMarketValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TMarketValue);
            this.TotalContainerSize = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TContainerSize);
            this.TotalServingCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TServingCost);
            this.TotalActualCaloriesPerDay = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, TActualCaloriesPerDay);
            this.TotalServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TServingsPerContainer);
            this.TotalActualServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TActualServingsPerContainer);
            this.TotalGenderOfServingPerson = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TGenderOfServingPerson);
            this.TotalWeightOfServingPerson = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TWeightOfServingPerson);
            //this.TotalWeightUnitsOfServingPerson = CalculatorHelpers.GetAttribute(currentCalculationsElement, TWeightUnitsOfServingPerson);
            this.TotalCaloriesPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCaloriesPerServing);
            this.TotalCaloriesPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCaloriesPerActualServing);
            this.TotalCaloriesFromFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCaloriesFromFatPerServing);
            this.TotalCaloriesFromFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCaloriesFromFatPerActualServing);
            this.TotalTotalFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTotalFatPerServing);
            this.TotalTotalFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTotalFatPerActualServing);
            this.TotalTotalFatActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTotalFatActualDailyPercent);
            this.TotalSaturatedFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSaturatedFatPerServing);
            this.TotalSaturatedFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSaturatedFatPerActualServing);
            this.TotalSaturatedFatActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSaturatedFatActualDailyPercent);
            this.TotalTransFatPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTransFatPerServing);
            this.TotalTransFatPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTransFatPerActualServing);
            this.TotalCholesterolPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCholesterolPerServing);
            this.TotalCholesterolPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCholesterolPerActualServing);
            this.TotalCholesterolActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCholesterolActualDailyPercent);
            this.TotalSodiumPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSodiumPerServing);
            this.TotalSodiumPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSodiumPerActualServing);
            this.TotalSodiumActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSodiumActualDailyPercent);
            this.TotalPotassiumPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPotassiumPerServing);
            this.TotalPotassiumPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPotassiumPerActualServing);
            this.TotalTotalCarbohydratePerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTotalCarbohydratePerServing);
            this.TotalTotalCarbohydratePerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTotalCarbohydratePerActualServing);
            this.TotalTotalCarbohydrateActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTotalCarbohydrateActualDailyPercent);
            this.TotalOtherCarbohydratePerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TOtherCarbohydratePerServing);
            this.TotalOtherCarbohydratePerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TOtherCarbohydratePerActualServing);
            this.TotalOtherCarbohydrateActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TOtherCarbohydrateActualDailyPercent);
            this.TotalDietaryFiberPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TDietaryFiberPerServing);
            this.TotalDietaryFiberPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TDietaryFiberPerActualServing);
            this.TotalDietaryFiberActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TDietaryFiberActualDailyPercent);
            this.TotalSugarsPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSugarsPerServing);
            this.TotalSugarsPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSugarsPerActualServing);
            this.TotalProteinPerServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TProteinPerServing);
            this.TotalProteinPerActualServing = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TProteinPerActualServing);
            this.TotalProteinActualDailyPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TProteinActualDailyPercent);
            this.TotalVitaminAPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminAPercentDailyValue);
            this.TotalVitaminAPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminAPercentActualDailyValue);
            this.TotalVitaminCPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminCPercentDailyValue);
            this.TotalVitaminCPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminCPercentActualDailyValue);
            this.TotalVitaminDPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminDPercentDailyValue);
            this.TotalVitaminDPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminDPercentActualDailyValue);
            this.TotalCalciumPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCalciumPercentDailyValue);
            this.TotalCalciumPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCalciumPercentActualDailyValue);
            this.TotalIronPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIronPercentDailyValue);
            this.TotalIronPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIronPercentActualDailyValue);
            this.TotalThiaminPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TThiaminPercentDailyValue);
            this.TotalThiaminPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TThiaminPercentActualDailyValue);
            this.TotalFolatePercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFolatePercentDailyValue);
            this.TotalFolatePercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFolatePercentActualDailyValue);
            this.TotalRiboflavinPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TRiboflavinPercentDailyValue);
            this.TotalRiboflavinPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TRiboflavinPercentActualDailyValue);
            this.TotalNiacinPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TNiacinPercentDailyValue);
            this.TotalNiacinPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TNiacinPercentActualDailyValue);
            this.TotalVitaminB6PercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminB6PercentDailyValue);
            this.TotalVitaminB6PercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminB6PercentActualDailyValue);
            this.TotalVitaminB12PercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminB12PercentDailyValue);
            this.TotalVitaminB12PercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVitaminB12PercentActualDailyValue);
            this.TotalPhosphorousPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPhosphorousPercentDailyValue);
            this.TotalPhosphorousPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPhosphorousPercentActualDailyValue);
            this.TotalMagnesiumPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TMagnesiumPercentDailyValue);
            this.TotalMagnesiumPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TMagnesiumPercentActualDailyValue);
            this.TotalZincPercentDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TZincPercentDailyValue);
            this.TotalZincPercentActualDailyValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TZincPercentActualDailyValue);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalFNStockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TMarketValue:
                    this.TotalMarketValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TContainerSize:
                    this.TotalContainerSize = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TServingCost:
                    this.TotalServingCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TActualCaloriesPerDay:
                    this.TotalActualCaloriesPerDay = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case TServingsPerContainer:
                    this.TotalServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TActualServingsPerContainer:
                    this.TotalActualServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TGenderOfServingPerson:
                    this.TotalGenderOfServingPerson = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TWeightOfServingPerson:
                    this.TotalWeightOfServingPerson = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                //case TWeightUnitsOfServingPerson:
                //    this.TotalWeightUnitsOfServingPerson = attValue;
                //    break;
                case TCaloriesPerServing:
                    this.TotalCaloriesPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCaloriesPerActualServing:
                    this.TotalCaloriesPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCaloriesFromFatPerServing:
                    this.TotalCaloriesFromFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCaloriesFromFatPerActualServing:
                    this.TotalCaloriesFromFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTotalFatPerServing:
                    this.TotalTotalFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTotalFatPerActualServing:
                    this.TotalTotalFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTotalFatActualDailyPercent:
                    this.TotalTotalFatActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSaturatedFatPerServing:
                    this.TotalSaturatedFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSaturatedFatPerActualServing:
                    this.TotalSaturatedFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSaturatedFatActualDailyPercent:
                    this.TotalSaturatedFatActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTransFatPerServing:
                    this.TotalTransFatPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTransFatPerActualServing:
                    this.TotalTransFatPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCholesterolPerServing:
                    this.TotalCholesterolPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCholesterolPerActualServing:
                    this.TotalCholesterolPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCholesterolActualDailyPercent:
                    this.TotalCholesterolActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSodiumPerServing:
                    this.TotalSodiumPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSodiumPerActualServing:
                    this.TotalSodiumPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSodiumActualDailyPercent:
                    this.TotalSodiumActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPotassiumPerServing:
                    this.TotalPotassiumPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPotassiumPerActualServing:
                    this.TotalPotassiumPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTotalCarbohydratePerServing:
                    this.TotalTotalCarbohydratePerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTotalCarbohydratePerActualServing:
                    this.TotalTotalCarbohydratePerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTotalCarbohydrateActualDailyPercent:
                    this.TotalTotalCarbohydrateActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOtherCarbohydratePerServing:
                    this.TotalOtherCarbohydratePerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOtherCarbohydratePerActualServing:
                    this.TotalOtherCarbohydratePerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOtherCarbohydrateActualDailyPercent:
                    this.TotalOtherCarbohydrateActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TDietaryFiberPerServing:
                    this.TotalDietaryFiberPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TDietaryFiberPerActualServing:
                    this.TotalDietaryFiberPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TDietaryFiberActualDailyPercent:
                    this.TotalDietaryFiberActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSugarsPerServing:
                    this.TotalSugarsPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSugarsPerActualServing:
                    this.TotalSugarsPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TProteinPerServing:
                    this.TotalProteinPerServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TProteinPerActualServing:
                    this.TotalProteinPerActualServing = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TProteinActualDailyPercent:
                    this.TotalProteinActualDailyPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminAPercentDailyValue:
                    this.TotalVitaminAPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminAPercentActualDailyValue:
                    this.TotalVitaminAPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminCPercentDailyValue:
                    this.TotalVitaminCPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminCPercentActualDailyValue:
                    this.TotalVitaminCPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminDPercentDailyValue:
                    this.TotalVitaminDPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminDPercentActualDailyValue:
                    this.TotalVitaminDPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCalciumPercentDailyValue:
                    this.TotalCalciumPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCalciumPercentActualDailyValue:
                    this.TotalCalciumPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIronPercentDailyValue:
                    this.TotalIronPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIronPercentActualDailyValue:
                    this.TotalIronPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TThiaminPercentDailyValue:
                    this.TotalThiaminPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TThiaminPercentActualDailyValue:
                    this.TotalThiaminPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFolatePercentDailyValue:
                    this.TotalFolatePercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFolatePercentActualDailyValue:
                    this.TotalFolatePercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRiboflavinPercentDailyValue:
                    this.TotalRiboflavinPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRiboflavinPercentActualDailyValue:
                    this.TotalRiboflavinPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TNiacinPercentDailyValue:
                    this.TotalNiacinPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TNiacinPercentActualDailyValue:
                    this.TotalNiacinPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminB6PercentDailyValue:
                    this.TotalVitaminB6PercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminB6PercentActualDailyValue:
                    this.TotalVitaminB6PercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminB12PercentDailyValue:
                    this.TotalVitaminB12PercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVitaminB12PercentActualDailyValue:
                    this.TotalVitaminB12PercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPhosphorousPercentDailyValue:
                    this.TotalPhosphorousPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPhosphorousPercentActualDailyValue:
                    this.TotalPhosphorousPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TMagnesiumPercentDailyValue:
                    this.TotalMagnesiumPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TMagnesiumPercentActualDailyValue:
                    this.TotalMagnesiumPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TZincPercentDailyValue:
                    this.TotalZincPercentDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TZincPercentActualDailyValue:
                    this.TotalZincPercentActualDailyValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetTotalFNStockAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TMarketValue, attNameExtension), this.TotalMarketValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TContainerSize, attNameExtension), this.TotalContainerSize);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TServingCost, attNameExtension), this.TotalServingCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TGenderOfServingPerson, attNameExtension), this.TotalGenderOfServingPerson);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(TWeightOfServingPerson, attNameExtension), this.TotalWeightOfServingPerson);
            //CalculatorHelpers.SetAttribute(currentCalculationsElement,
            //    string.Concat(TWeightUnitsOfServingPerson, attNameExtension), this.TotalWeightUnitsOfServingPerson);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(TActualCaloriesPerDay, attNameExtension), this.TotalActualCaloriesPerDay);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TServingsPerContainer, attNameExtension), this.TotalServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TActualServingsPerContainer, attNameExtension), this.TotalActualServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCaloriesPerServing, attNameExtension), this.TotalCaloriesPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCaloriesPerActualServing, attNameExtension), this.TotalCaloriesPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCaloriesFromFatPerServing, attNameExtension), this.TotalCaloriesFromFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TCaloriesFromFatPerActualServing, attNameExtension), this.TotalCaloriesFromFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTotalFatPerServing, attNameExtension), this.TotalTotalFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTotalFatPerActualServing, attNameExtension), this.TotalTotalFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTotalFatActualDailyPercent, attNameExtension), this.TotalTotalFatActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSaturatedFatPerServing, attNameExtension), this.TotalSaturatedFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSaturatedFatPerActualServing, attNameExtension), this.TotalSaturatedFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSaturatedFatActualDailyPercent, attNameExtension), this.TotalSaturatedFatActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTransFatPerServing, attNameExtension), this.TotalTransFatPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTransFatPerActualServing, attNameExtension), this.TotalTransFatPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCholesterolPerServing, attNameExtension), this.TotalCholesterolPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCholesterolPerActualServing, attNameExtension), this.TotalCholesterolPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCholesterolActualDailyPercent, attNameExtension), this.TotalCholesterolActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSodiumPerServing, attNameExtension), this.TotalSodiumPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSodiumPerActualServing, attNameExtension), this.TotalSodiumPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSodiumActualDailyPercent, attNameExtension), this.TotalSodiumActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TPotassiumPerServing, attNameExtension), this.TotalPotassiumPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TPotassiumPerActualServing, attNameExtension), this.TotalPotassiumPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTotalCarbohydratePerServing, attNameExtension), this.TotalTotalCarbohydratePerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTotalCarbohydratePerActualServing, attNameExtension), this.TotalTotalCarbohydratePerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTotalCarbohydrateActualDailyPercent, attNameExtension), this.TotalTotalCarbohydrateActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TOtherCarbohydratePerServing, attNameExtension), this.TotalOtherCarbohydratePerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TOtherCarbohydratePerActualServing, attNameExtension), this.TotalOtherCarbohydratePerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TOtherCarbohydrateActualDailyPercent, attNameExtension), this.TotalOtherCarbohydrateActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TDietaryFiberPerServing, attNameExtension), this.TotalDietaryFiberPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TDietaryFiberPerActualServing, attNameExtension), this.TotalDietaryFiberPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TDietaryFiberActualDailyPercent, attNameExtension), this.TotalDietaryFiberActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSugarsPerServing, attNameExtension), this.TotalSugarsPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSugarsPerActualServing, attNameExtension), this.TotalSugarsPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TProteinPerServing, attNameExtension), this.TotalProteinPerServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TProteinPerActualServing, attNameExtension), this.TotalProteinPerActualServing);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TProteinActualDailyPercent, attNameExtension), this.TotalProteinActualDailyPercent);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminAPercentDailyValue, attNameExtension), this.TotalVitaminAPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminAPercentActualDailyValue, attNameExtension), this.TotalVitaminAPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminCPercentDailyValue, attNameExtension), this.TotalVitaminCPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminCPercentActualDailyValue, attNameExtension), this.TotalVitaminCPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminDPercentDailyValue, attNameExtension), this.TotalVitaminDPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminDPercentActualDailyValue, attNameExtension), this.TotalVitaminDPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCalciumPercentDailyValue, attNameExtension), this.TotalCalciumPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCalciumPercentActualDailyValue, attNameExtension), this.TotalCalciumPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TIronPercentDailyValue, attNameExtension), this.TotalIronPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TIronPercentActualDailyValue, attNameExtension), this.TotalIronPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TThiaminPercentDailyValue, attNameExtension), this.TotalThiaminPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TThiaminPercentActualDailyValue, attNameExtension), this.TotalThiaminPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFolatePercentDailyValue, attNameExtension), this.TotalFolatePercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFolatePercentActualDailyValue, attNameExtension), this.TotalFolatePercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TRiboflavinPercentDailyValue, attNameExtension), this.TotalRiboflavinPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TRiboflavinPercentActualDailyValue, attNameExtension), this.TotalRiboflavinPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TNiacinPercentDailyValue, attNameExtension), this.TotalNiacinPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TNiacinPercentActualDailyValue, attNameExtension), this.TotalNiacinPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminB6PercentDailyValue, attNameExtension), this.TotalVitaminB6PercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminB6PercentActualDailyValue, attNameExtension), this.TotalVitaminB6PercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TVitaminB12PercentDailyValue, attNameExtension), this.TotalVitaminB12PercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVitaminB12PercentActualDailyValue, attNameExtension), this.TotalVitaminB12PercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TPhosphorousPercentDailyValue, attNameExtension), this.TotalPhosphorousPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TPhosphorousPercentActualDailyValue, attNameExtension), this.TotalPhosphorousPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TMagnesiumPercentDailyValue, attNameExtension), this.TotalMagnesiumPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TMagnesiumPercentActualDailyValue, attNameExtension), this.TotalMagnesiumPercentActualDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TZincPercentDailyValue, attNameExtension), this.TotalZincPercentDailyValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TZincPercentActualDailyValue, attNameExtension), this.TotalZincPercentActualDailyValue);
        }
        public virtual void SetTotalFNStockAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(TMarketValue, attNameExtension), this.TotalMarketValue.ToString());
            writer.WriteAttributeString(string.Concat(TContainerSize, attNameExtension), this.TotalContainerSize.ToString());
            writer.WriteAttributeString(string.Concat(TServingCost, attNameExtension), this.TotalServingCost.ToString());
            writer.WriteAttributeString(string.Concat(TGenderOfServingPerson, attNameExtension), this.TotalGenderOfServingPerson.ToString());
            writer.WriteAttributeString(string.Concat(TWeightOfServingPerson, attNameExtension), this.TotalWeightOfServingPerson.ToString());
            //writer.WriteAttributeString(string.Concat(TWeightUnitsOfServingPerson, attNameExtension), this.TotalWeightUnitsOfServingPerson.ToString());
            writer.WriteAttributeString(string.Concat(TActualCaloriesPerDay, attNameExtension), this.TotalActualCaloriesPerDay.ToString());
            writer.WriteAttributeString(string.Concat(TServingsPerContainer, attNameExtension), this.TotalServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(TActualServingsPerContainer, attNameExtension), this.TotalActualServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(TCaloriesPerServing, attNameExtension), this.TotalCaloriesPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TCaloriesPerActualServing, attNameExtension), this.TotalCaloriesPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TCaloriesFromFatPerServing, attNameExtension), this.TotalCaloriesFromFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TCaloriesFromFatPerActualServing, attNameExtension), this.TotalCaloriesFromFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TTotalFatPerServing, attNameExtension), this.TotalTotalFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TTotalFatPerActualServing, attNameExtension), this.TotalTotalFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TTotalFatActualDailyPercent, attNameExtension), this.TotalTotalFatActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TSaturatedFatPerServing, attNameExtension), this.TotalSaturatedFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TSaturatedFatPerActualServing, attNameExtension), this.TotalSaturatedFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TSaturatedFatActualDailyPercent, attNameExtension), this.TotalSaturatedFatActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TTransFatPerServing, attNameExtension), this.TotalTransFatPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TTransFatPerActualServing, attNameExtension), this.TotalTransFatPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TCholesterolPerServing, attNameExtension), this.TotalCholesterolPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TCholesterolPerActualServing, attNameExtension), this.TotalCholesterolPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TCholesterolActualDailyPercent, attNameExtension), this.TotalCholesterolActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TSodiumPerServing, attNameExtension), this.TotalSodiumPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TSodiumPerActualServing, attNameExtension), this.TotalSodiumPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TSodiumActualDailyPercent, attNameExtension), this.TotalSodiumActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TPotassiumPerServing, attNameExtension), this.TotalPotassiumPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TPotassiumPerActualServing, attNameExtension), this.TotalPotassiumPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TTotalCarbohydratePerServing, attNameExtension), this.TotalTotalCarbohydratePerServing.ToString());
            writer.WriteAttributeString(string.Concat(TTotalCarbohydratePerActualServing, attNameExtension), this.TotalTotalCarbohydratePerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TTotalCarbohydrateActualDailyPercent, attNameExtension), this.TotalTotalCarbohydrateActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TOtherCarbohydratePerServing, attNameExtension), this.TotalOtherCarbohydratePerServing.ToString());
            writer.WriteAttributeString(string.Concat(TOtherCarbohydratePerActualServing, attNameExtension), this.TotalOtherCarbohydratePerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TOtherCarbohydrateActualDailyPercent, attNameExtension), this.TotalOtherCarbohydrateActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TDietaryFiberPerServing, attNameExtension), this.TotalDietaryFiberPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TDietaryFiberPerActualServing, attNameExtension), this.TotalDietaryFiberPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TDietaryFiberActualDailyPercent, attNameExtension), this.TotalDietaryFiberActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TSugarsPerServing, attNameExtension), this.TotalSugarsPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TSugarsPerActualServing, attNameExtension), this.TotalSugarsPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TProteinPerServing, attNameExtension), this.TotalProteinPerServing.ToString());
            writer.WriteAttributeString(string.Concat(TProteinPerActualServing, attNameExtension), this.TotalProteinPerActualServing.ToString());
            writer.WriteAttributeString(string.Concat(TProteinActualDailyPercent, attNameExtension), this.TotalProteinActualDailyPercent.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminAPercentDailyValue, attNameExtension), this.TotalVitaminAPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminAPercentActualDailyValue, attNameExtension), this.TotalVitaminAPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminCPercentDailyValue, attNameExtension), this.TotalVitaminCPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminCPercentActualDailyValue, attNameExtension), this.TotalVitaminCPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminDPercentDailyValue, attNameExtension), this.TotalVitaminDPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminDPercentActualDailyValue, attNameExtension), this.TotalVitaminDPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TCalciumPercentDailyValue, attNameExtension), this.TotalCalciumPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TCalciumPercentActualDailyValue, attNameExtension), this.TotalCalciumPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TIronPercentDailyValue, attNameExtension), this.TotalIronPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TIronPercentActualDailyValue, attNameExtension), this.TotalIronPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TThiaminPercentDailyValue, attNameExtension), this.TotalThiaminPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TThiaminPercentActualDailyValue, attNameExtension), this.TotalThiaminPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TFolatePercentDailyValue, attNameExtension), this.TotalFolatePercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TFolatePercentActualDailyValue, attNameExtension), this.TotalFolatePercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TRiboflavinPercentDailyValue, attNameExtension), this.TotalRiboflavinPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TRiboflavinPercentActualDailyValue, attNameExtension), this.TotalRiboflavinPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TNiacinPercentDailyValue, attNameExtension), this.TotalNiacinPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TNiacinPercentActualDailyValue, attNameExtension), this.TotalNiacinPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminB6PercentDailyValue, attNameExtension), this.TotalVitaminB6PercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminB6PercentActualDailyValue, attNameExtension), this.TotalVitaminB6PercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminB12PercentDailyValue, attNameExtension), this.TotalVitaminB12PercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TVitaminB12PercentActualDailyValue, attNameExtension), this.TotalVitaminB12PercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TPhosphorousPercentDailyValue, attNameExtension), this.TotalPhosphorousPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TPhosphorousPercentActualDailyValue, attNameExtension), this.TotalPhosphorousPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TMagnesiumPercentDailyValue, attNameExtension), this.TotalMagnesiumPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TMagnesiumPercentActualDailyValue, attNameExtension), this.TotalMagnesiumPercentActualDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TZincPercentDailyValue, attNameExtension), this.TotalZincPercentDailyValue.ToString());
            writer.WriteAttributeString(string.Concat(TZincPercentActualDailyValue, attNameExtension), this.TotalZincPercentActualDailyValue.ToString());
        }
    }
    public static class FNExtensions
    {
        //add a foodfact to the baseStat.FoodFacts dictionary
        public static bool AddFNStocksToDictionary(this FNStock baseStat,
            int filePosition, int nodePosition, FoodFactCalculator foodStock)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.FoodFacts == null)
                baseStat.FoodFacts
                = new Dictionary<int, List<FoodFactCalculator>>();
            if (baseStat.FoodFacts.ContainsKey(filePosition))
            {
                if (baseStat.FoodFacts[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.FoodFacts[filePosition].Count <= i)
                        {
                            baseStat.FoodFacts[filePosition]
                                .Add(new FoodFactCalculator());
                        }
                    }
                    baseStat.FoodFacts[filePosition][nodePosition]
                        = foodStock;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<FoodFactCalculator> baseStats
                    = new List<FoodFactCalculator>();
                KeyValuePair<int, List<FoodFactCalculator>> newStat
                    = new KeyValuePair<int, List<FoodFactCalculator>>(
                        filePosition, baseStats);
                baseStat.FoodFacts.Add(newStat);
                bIsAdded = AddFNStocksToDictionary(baseStat,
                    filePosition, nodePosition, foodStock);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this FNStock baseStat,
            int filePosition, FoodFactCalculator foodStock)
        {
            int iNodeCount = 0;
            if (baseStat.FoodFacts == null)
                return iNodeCount;
            if (baseStat.FoodFacts.ContainsKey(filePosition))
            {
                if (baseStat.FoodFacts[filePosition] != null)
                {
                    iNodeCount = baseStat.FoodFacts[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}
