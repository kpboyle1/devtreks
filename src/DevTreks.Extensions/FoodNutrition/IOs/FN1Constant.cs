using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize food nutrition USDA Appendix 5. Dietary Recommended 
    ///             nutrient amounts. 
    ///Author:		www.devtreks.org
    ///Date:		2013, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. User selects constants of type Vegan, Mediterranean, USDA, and Dash. 
    ///             This object then fills in the object using a linkedlist xml document.
    ///             2. The linked list doc then goes into drop down list which user selects
    ///             agegroup for each indicator and pulls out the Recommended Amount
    ///             Could have the four types in one document or four? Probably one.
    ///             OR
    ///             3. The linkedlist is simply displayed to user who can then 
    ///             fill in a Target property (i.e. if the recommend is 10 to 20, might choose 15)
    public class FN1Constant
    {
        //need 3 linked lists 
        //1. Appendix 6. Estimated calorie needs
        //2. Appendix 5. Nutritional goals
        //3. Appendix 7 to 10. Food Pattern goals
        //the parent calculator pulls out the targets in step 1. fill in demogs and food pattern
        public FN1Constant()
        {
            //init properties
            //InitFN1Constant();
        }
        //another potential property:
        //demog.age and demog.activitylevel can be used to set
        //Appendix 6. EstimatedCaloriesNeeds
        //public double FNC1AgeActivty_CalorieNeeds { get; set; }
        //That in turn can be used to set the Food Group indicators 
        //Fruits, Vegs (Dark green ...), Grains (whole ...), Protein Foods (Seafood ...), Dairy, Oils, Fat Limit, Fat Limit %calories
        //for the USDA App 7-10 Food Patterns (USDA, Vegan, Meditarranean ...
        //which would then give a nice DDI index

        //properties come directly from the USDA 2010 reference
        public int FNC1Id { get; set; }
        public string FNC1Label { get; set; }
        public string FNC1Name { get; set; }
        public string FNC1Child1to3 { get; set; }
        public string FNC1Female4to8 { get; set; }
        public string FNC1Male4to8 { get; set; }
        public string FNC1Female9to13 { get; set; }
        public string FNC1Male9to13 { get; set; }
        public string FNC1Female14to18 { get; set; }
        public string FNC1Male14to18 { get; set; }
        public string FNC1Female19to30 { get; set; }
        public string FNC1Male19to30 { get; set; }
        public string FNC1Female31to50 { get; set; }
        public string FNC1Male31to50 { get; set; }
        public string FNC1Female51Plus { get; set; }
        public string FNC1Male51Plus { get; set; }

        private const string cFNC1Id = "FNC1Id";
        private const string cFNC1Label = "FNC1Label";
        public const string cFNC1Name = "FNC1Name";
        private const string cFNC1Child1to3 = "FNC1Child1to3";
        private const string cFNC1Female4to8 = "FNC1Female4to8";
        private const string cFNC1Male4to8 = "FNC1Male4to8";
        private const string cFNC1Female9to13 = "FNC1Female9to13";
        private const string cFNC1Male9to13 = "FNC1Male9to13";
        public const string cFNC1Female14to18 = "FNC1Female14to18";
        private const string cFNC1Male14to18 = "FNC1Male14to18";
        private const string cFNC1Female19to30 = "FNC1Female19to30";
        private const string cFNC1Male19to30 = "FNC1Male19to30";
        private const string cFNC1Female31to50 = "FNC1Female31to50";
        private const string cFNC1Male31to50 = "FNC1Male31to50";
        private const string cFNC1Female51Plus = "FNC1Female51Plus";
        public const string cFNC1Female51PlusPTOEquiv = "FNC1Female51PlusPTOEquiv";
        public const string cFNC1Female51PlusPTOMax = "FNC1Female51PlusPTOMax";
        private const string cFNC1Male51Plus = "FNC1Male51Plus";

        public void SetFN1ConstantProperties(XElement currentCalculationsElement)
        {
            this.FNC1Id = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cFNC1Id);
            this.FNC1Label = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Label);
            this.FNC1Name = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Name);
            this.FNC1Child1to3 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Child1to3);
            this.FNC1Female4to8 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Female4to8);
            this.FNC1Male4to8 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Male4to8);
            this.FNC1Female9to13 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Female9to13);
            this.FNC1Male9to13 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Male9to13);
            this.FNC1Female14to18 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Female14to18);
            this.FNC1Male14to18 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Male14to18);
            this.FNC1Female19to30 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Female19to30);
            this.FNC1Male19to30 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Male19to30);
            this.FNC1Female31to50 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Female31to50);
            this.FNC1Male31to50 = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Male31to50);
            this.FNC1Female51Plus = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Female51Plus);
            this.FNC1Male51Plus = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFNC1Male51Plus);
        }
        public void CopyFN1ConstantProperties(FN1Constant constant)
        {
            this.FNC1Id = constant.FNC1Id;
            this.FNC1Label = constant.FNC1Label;
            this.FNC1Name = constant.FNC1Name;
            this.FNC1Child1to3 = constant.FNC1Child1to3;
            this.FNC1Female4to8 = constant.FNC1Female4to8;
            this.FNC1Male4to8 = constant.FNC1Male4to8;
            this.FNC1Female9to13 = constant.FNC1Female9to13;
            this.FNC1Male9to13 = constant.FNC1Male9to13;
            this.FNC1Female14to18 = constant.FNC1Female14to18;
            this.FNC1Male14to18 = constant.FNC1Male14to18;
            this.FNC1Female19to30 = constant.FNC1Female19to30;
            this.FNC1Male19to30 = constant.FNC1Male19to30;
            this.FNC1Female31to50 = constant.FNC1Female31to50;
            this.FNC1Male31to50 = constant.FNC1Male31to50;
            this.FNC1Female51Plus = constant.FNC1Female51Plus;
            this.FNC1Male51Plus = constant.FNC1Male51Plus;
        }
        public void SetFN1ConstantAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cFNC1Id, this.FNC1Id);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Label, this.FNC1Label);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Name, this.FNC1Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Child1to3, this.FNC1Child1to3);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Female4to8, this.FNC1Female4to8);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Male4to8, this.FNC1Male4to8);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Female9to13, this.FNC1Female9to13);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Male9to13, this.FNC1Male9to13);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Female14to18, this.FNC1Female14to18);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Male14to18, this.FNC1Male14to18);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Female19to30, this.FNC1Female19to30);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Male19to30, this.FNC1Male19to30);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Female31to50, this.FNC1Female31to50);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Male31to50, this.FNC1Male31to50);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Female51Plus, this.FNC1Female51Plus);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFNC1Male51Plus, this.FNC1Male51Plus);
        }
    }
}
