using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize a demographic object with
    ///             properties. This object is generally inserted into 
    ///             other calculators to provide demographic support. 
    ///             Up to 10 people, or 10 targeted populations, can be recorded per object.
    ///Author:		www.devtreks.org
    ///Date:		2013, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. If standalone operation, complete at operation level
    ///             2. If budget, only complete at TP level 
    public class Demog2
    {
        public Demog2()
            : base()
        {
            //health care benefit object
            InitDemog2Properties();
        }
        //copy constructor
        public Demog2(Demog2 demog11)
        {
            CopyDemog2Properties(demog11);
        }
        //can be coupled with gender to derive calories needed/day (App. 6, USDA)
        public enum ACTIVITY_TYPE
        {
            select              = 0,
            sedentary           = 1,
            moderatelyactive    = 2,
            active              = 3,
            veryactive          = 4
        }
        public enum HEALTHCONDITIONS_TYPE
        {
            none    = 0,
            one     = 1,
            two     = 2,
            twoplus = 3
        }
        public enum GENDER_TYPE
        {
            female = 0,
            male    = 1
        }
        public enum RACE_TYPE
        {
            indigenousornative  = 0,
            asian               = 1,
            blackorafrican      = 2,
            hispanicorlatino    = 3,
            white               = 4,
            mixed               = 5
        }
        public enum MEASUREMENT_UNIT_TYPE
        {
            US_inches_pounds                = 0,
            METRIC_centimeters_kilograms    = 1
        }
        //properties
        public string MeasurementUnits { get; set; }
        //enter
        public string Name1 { get; set; }
        public string Label1 { get; set; }
        public string Description1 { get; set; }
        public double Age1 { get; set; }
        public double EducationYears1 { get; set; }
        public string Race1 { get; set; }
        public string Gender1 { get; set; }
        public int Height1 { get; set; }
        public int Weight1 { get; set; }
        public string ActivityType1 { get; set; }
        public string HealthConditionsType1 { get; set; }
        public int HouseholdYearIncome1 { get; set; }
        //calculated results
        //body mass index
        public double BMI1 { get; set; }
        //calories needed per day (take from USDA, App. 6)
        public int CalNeedPerDay1 { get; set; }

        public string Name2 { get; set; }
        public string Label2 { get; set; }
        public string Description2 { get; set; }
        public double Age2 { get; set; }
        public double EducationYears2 { get; set; }
        public string Race2 { get; set; }
        public string Gender2 { get; set; }
        public int Height2 { get; set; }
        public int Weight2 { get; set; }
        public string ActivityType2 { get; set; }
        public string HealthConditionsType2 { get; set; }
        public int HouseholdYearIncome2 { get; set; }
        public double BMI2 { get; set; }
        public int CalNeedPerDay2 { get; set; }

        public string Name3 { get; set; }
        public string Label3 { get; set; }
        public string Description3 { get; set; }
        public double Age3 { get; set; }
        public double EducationYears3 { get; set; }
        public string Race3 { get; set; }
        public string Gender3 { get; set; }
        public int Height3 { get; set; }
        public int Weight3 { get; set; }
        public string ActivityType3 { get; set; }
        public string HealthConditionsType3 { get; set; }
        public int HouseholdYearIncome3 { get; set; }
        public double BMI3 { get; set; }
        public int CalNeedPerDay3 { get; set; }

        public string Name4 { get; set; }
        public string Label4 { get; set; }
        public string Description4 { get; set; }
        public double Age4 { get; set; }
        public double EducationYears4 { get; set; }
        public string Race4 { get; set; }
        public string Gender4 { get; set; }
        public int Height4 { get; set; }
        public int Weight4 { get; set; }
        public string ActivityType4 { get; set; }
        public string HealthConditionsType4 { get; set; }
        public int HouseholdYearIncome4 { get; set; }
        public double BMI4 { get; set; }
        public int CalNeedPerDay4 { get; set; }

        public string Name5 { get; set; }
        public string Label5 { get; set; }
        public string Description5 { get; set; }
        public double Age5 { get; set; }
        public double EducationYears5 { get; set; }
        public string Race5 { get; set; }
        public string Gender5 { get; set; }
        public int Height5 { get; set; }
        public int Weight5 { get; set; }
        public string ActivityType5 { get; set; }
        public string HealthConditionsType5 { get; set; }
        public int HouseholdYearIncome5 { get; set; }
        public double BMI5 { get; set; }
        public int CalNeedPerDay5 { get; set; }

        public string Name6 { get; set; }
        public string Label6 { get; set; }
        public string Description6 { get; set; }
        public double Age6 { get; set; }
        public double EducationYears6 { get; set; }
        public string Race6 { get; set; }
        public string Gender6 { get; set; }
        public int Height6 { get; set; }
        public int Weight6 { get; set; }
        public string ActivityType6 { get; set; }
        public string HealthConditionsType6 { get; set; }
        public int HouseholdYearIncome6 { get; set; }
        public double BMI6 { get; set; }
        public int CalNeedPerDay6 { get; set; }

        public string Name7 { get; set; }
        public string Label7 { get; set; }
        public string Description7 { get; set; }
        public double Age7 { get; set; }
        public double EducationYears7 { get; set; }
        public string Race7 { get; set; }
        public string Gender7 { get; set; }
        public int Height7 { get; set; }
        public int Weight7 { get; set; }
        public string ActivityType7 { get; set; }
        public string HealthConditionsType7 { get; set; }
        public int HouseholdYearIncome7 { get; set; }
        public double BMI7 { get; set; }
        public int CalNeedPerDay7 { get; set; }

        public string Name8 { get; set; }
        public string Label8 { get; set; }
        public string Description8 { get; set; }
        public double Age8 { get; set; }
        public double EducationYears8 { get; set; }
        public string Race8 { get; set; }
        public string Gender8 { get; set; }
        public int Height8 { get; set; }
        public int Weight8 { get; set; }
        public string ActivityType8 { get; set; }
        public string HealthConditionsType8 { get; set; }
        public int HouseholdYearIncome8 { get; set; }
        public double BMI8 { get; set; }
        public int CalNeedPerDay8 { get; set; }

        public string Name9 { get; set; }
        public string Label9 { get; set; }
        public string Description9 { get; set; }
        public double Age9 { get; set; }
        public double EducationYears9 { get; set; }
        public string Race9 { get; set; }
        public string Gender9 { get; set; }
        public int Height9 { get; set; }
        public int Weight9 { get; set; }
        public string ActivityType9 { get; set; }
        public string HealthConditionsType9 { get; set; }
        public int HouseholdYearIncome9 { get; set; }
        public double BMI9 { get; set; }
        public int CalNeedPerDay9 { get; set; }

        public string Name10 { get; set; }
        public string Label10 { get; set; }
        public string Description10 { get; set; }
        public double Age10 { get; set; }
        public double EducationYears10 { get; set; }
        public string Race10 { get; set; }
        public string Gender10 { get; set; }
        public int Height10 { get; set; }
        public int Weight10 { get; set; }
        public string ActivityType10 { get; set; }
        public string HealthConditionsType10 { get; set; }
        public int HouseholdYearIncome10 { get; set; }
        public double BMI10 { get; set; }
        public int CalNeedPerDay10 { get; set; }

        private const string cMeasurementUnits = "MeasurementUnits";

        private const string cName1 = "Name1";
        private const string cLabel1 = "Label1";
        private const string cDescription1 = "Description1";
        private const string cAge1 = "Age1";
        private const string cEducationYears1 = "EducationYears1";
        private const string cRace1 = "Race1";
        private const string cGender1 = "Gender1";
        private const string cHeight1 = "Height1";
        private const string cWeight1 = "Weight1";
        private const string cActivityType1 = "ActivityType1";
        private const string cHealthConditionsType1 = "HealthConditionsType1";
        private const string cHouseholdYearIncome1 = "HouseholdYearIncome1";
        private const string cBMI1 = "BMI1";
        private const string cCalNeedPerDay1 = "CalNeedPerDay1";

        private const string cName2 = "Name2";
        private const string cLabel2 = "Label2";
        private const string cDescription2 = "Description2";
        private const string cAge2 = "Age2";
        private const string cEducationYears2 = "EducationYears2";
        private const string cRace2 = "Race2";
        private const string cGender2 = "Gender2";
        private const string cHeight2 = "Height2";
        private const string cWeight2 = "Weight2";
        private const string cActivityType2 = "ActivityType2";
        private const string cHealthConditionsType2 = "HealthConditionsType2";
        private const string cHouseholdYearIncome2 = "HouseholdYearIncome2";
        private const string cBMI2 = "BMI2";
        private const string cCalNeedPerDay2 = "CalNeedPerDay2";

        private const string cName3 = "Name3";
        private const string cLabel3 = "Label3";
        private const string cDescription3 = "Description3";
        private const string cAge3 = "Age3";
        private const string cEducationYears3 = "EducationYears3";
        private const string cRace3 = "Race3";
        private const string cGender3 = "Gender3";
        private const string cHeight3 = "Height3";
        private const string cWeight3 = "Weight3";
        private const string cActivityType3 = "ActivityType3";
        private const string cHealthConditionsType3 = "HealthConditionsType3";
        private const string cHouseholdYearIncome3 = "HouseholdYearIncome3";
        private const string cBMI3 = "BMI3";
        private const string cCalNeedPerDay3 = "CalNeedPerDay3";

        private const string cName4 = "Name4";
        private const string cLabel4 = "Label4";
        private const string cDescription4 = "Description4";
        private const string cAge4 = "Age4";
        private const string cEducationYears4 = "EducationYears4";
        private const string cRace4 = "Race4";
        private const string cGender4 = "Gender4";
        private const string cHeight4 = "Height4";
        private const string cWeight4 = "Weight4";
        private const string cActivityType4 = "ActivityType4";
        private const string cHealthConditionsType4 = "HealthConditionsType4";
        private const string cHouseholdYearIncome4 = "HouseholdYearIncome4";
        private const string cBMI4 = "BMI4";
        private const string cCalNeedPerDay4 = "CalNeedPerDay4";

        private const string cName5 = "Name5";
        private const string cLabel5 = "Label5";
        private const string cDescription5 = "Description5";
        private const string cAge5 = "Age5";
        private const string cEducationYears5 = "EducationYears5";
        private const string cRace5 = "Race5";
        private const string cGender5 = "Gender5";
        private const string cHeight5 = "Height5";
        private const string cWeight5 = "Weight5";
        private const string cActivityType5 = "ActivityType5";
        private const string cHealthConditionsType5 = "HealthConditionsType5";
        private const string cHouseholdYearIncome5 = "HouseholdYearIncome5";
        private const string cBMI5 = "BMI5";
        private const string cCalNeedPerDay5 = "CalNeedPerDay5";

        private const string cName6 = "Name6";
        private const string cLabel6 = "Label6";
        private const string cDescription6 = "Description6";
        private const string cAge6 = "Age6";
        private const string cEducationYears6 = "EducationYears6";
        private const string cRace6 = "Race6";
        private const string cGender6 = "Gender6";
        private const string cHeight6 = "Height6";
        private const string cWeight6 = "Weight6";
        private const string cActivityType6 = "ActivityType6";
        private const string cHealthConditionsType6 = "HealthConditionsType6";
        private const string cHouseholdYearIncome6 = "HouseholdYearIncome6";
        private const string cBMI6 = "BMI6";
        private const string cCalNeedPerDay6 = "CalNeedPerDay6";

        private const string cName7 = "Name7";
        private const string cLabel7 = "Label7";
        private const string cDescription7 = "Description7";
        private const string cAge7 = "Age7";
        private const string cEducationYears7 = "EducationYears7";
        private const string cRace7 = "Race7";
        private const string cGender7 = "Gender7";
        private const string cHeight7 = "Height7";
        private const string cWeight7 = "Weight7";
        private const string cActivityType7 = "ActivityType7";
        private const string cHealthConditionsType7 = "HealthConditionsType7";
        private const string cHouseholdYearIncome7 = "HouseholdYearIncome7";
        private const string cBMI7 = "BMI7";
        private const string cCalNeedPerDay7 = "CalNeedPerDay7";

        private const string cName8 = "Name8";
        private const string cLabel8 = "Label8";
        private const string cDescription8 = "Description8";
        private const string cAge8 = "Age8";
        private const string cEducationYears8 = "EducationYears8";
        private const string cRace8 = "Race8";
        private const string cGender8 = "Gender8";
        private const string cHeight8 = "Height8";
        private const string cWeight8 = "Weight8";
        private const string cActivityType8 = "ActivityType8";
        private const string cHealthConditionsType8 = "HealthConditionsType8";
        private const string cHouseholdYearIncome8 = "HouseholdYearIncome8";
        private const string cBMI8 = "BMI8";
        private const string cCalNeedPerDay8 = "CalNeedPerDay8";

        private const string cName9 = "Name9";
        private const string cLabel9 = "Label9";
        private const string cDescription9 = "Description9";
        private const string cAge9 = "Age9";
        private const string cEducationYears9 = "EducationYears9";
        private const string cRace9 = "Race9";
        private const string cGender9 = "Gender9";
        private const string cHeight9 = "Height9";
        private const string cWeight9 = "Weight9";
        private const string cActivityType9 = "ActivityType9";
        private const string cHealthConditionsType9 = "HealthConditionsType9";
        private const string cHouseholdYearIncome9 = "HouseholdYearIncome9";
        private const string cBMI9 = "BMI9";
        private const string cCalNeedPerDay9 = "CalNeedPerDay9";

        private const string cName10 = "Name10";
        private const string cLabel10 = "Label10";
        private const string cDescription10 = "Description10";
        private const string cAge10 = "Age10";
        private const string cEducationYears10 = "EducationYears10";
        private const string cRace10 = "Race10";
        private const string cGender10 = "Gender10";
        private const string cHeight10 = "Height10";
        private const string cWeight10 = "Weight10";
        private const string cActivityType10 = "ActivityType10";
        private const string cHealthConditionsType10 = "HealthConditionsType10";
        private const string cHouseholdYearIncome10 = "HouseholdYearIncome10";
        private const string cBMI10 = "BMI10";
        private const string cCalNeedPerDay10 = "CalNeedPerDay10";

        public virtual void InitDemog2Properties()
        {
            //avoid null references to properties
            this.MeasurementUnits = MEASUREMENT_UNIT_TYPE.US_inches_pounds.ToString();
            this.Name1 = string.Empty;
            this.Label1 = string.Empty;
            this.Description1 = string.Empty;
            this.Age1 = 0;
            this.EducationYears1 = 0;
            this.Race1 = RACE_TYPE.white.ToString();
            this.Gender1 = GENDER_TYPE.female.ToString();
            this.Height1 = 0;
            this.Weight1 = 0;
            this.ActivityType1 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType1 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome1 = 0;
            this.BMI1 = 0;
            this.CalNeedPerDay1 = 0;

            this.Name2 = string.Empty;
            this.Label2 = string.Empty;
            this.Description2 = string.Empty;
            this.Age2 = 0;
            this.EducationYears2 = 0;
            this.Race2 = RACE_TYPE.white.ToString();
            this.Gender2 = GENDER_TYPE.female.ToString();
            this.Height2 = 0;
            this.Weight2 = 0;
            this.ActivityType2 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType2 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome2 = 0;
            this.BMI2 = 0;
            this.CalNeedPerDay2 = 0;

            this.Name3 = string.Empty;
            this.Label3 = string.Empty;
            this.Description3 = string.Empty;
            this.Age3 = 0;
            this.EducationYears3 = 0;
            this.Race3 = RACE_TYPE.white.ToString();
            this.Gender3 = GENDER_TYPE.female.ToString();
            this.Height3 = 0;
            this.Weight3 = 0;
            this.ActivityType3 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType3 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome3 = 0;
            this.BMI3 = 0;
            this.CalNeedPerDay3 = 0;

            this.Name4 = string.Empty;
            this.Label4 = string.Empty;
            this.Description4 = string.Empty;
            this.Age4 = 0;
            this.EducationYears4 = 0;
            this.Race4 = RACE_TYPE.white.ToString();
            this.Gender4 = GENDER_TYPE.female.ToString();
            this.Height4 = 0;
            this.Weight4 = 0;
            this.ActivityType4 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType4 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome4 = 0;
            this.BMI4 = 0;
            this.CalNeedPerDay4 = 0;

            this.Name5 = string.Empty;
            this.Label5 = string.Empty;
            this.Description5 = string.Empty;
            this.Age5 = 0;
            this.EducationYears5 = 0;
            this.Race5 = RACE_TYPE.white.ToString();
            this.Gender5 = GENDER_TYPE.female.ToString();
            this.Height5 = 0;
            this.Weight5 = 0;
            this.ActivityType5 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType5 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome5 = 0;
            this.BMI5 = 0;
            this.CalNeedPerDay5 = 0;

            this.Name6 = string.Empty;
            this.Label6 = string.Empty;
            this.Description6 = string.Empty;
            this.Age6 = 0;
            this.EducationYears6 = 0;
            this.Race6 = RACE_TYPE.white.ToString();
            this.Gender6 = GENDER_TYPE.female.ToString();
            this.Height6 = 0;
            this.Weight6 = 0;
            this.ActivityType6 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType6 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome6 = 0;
            this.BMI6 = 0;
            this.CalNeedPerDay6 = 0;

            this.Name7 = string.Empty;
            this.Label7 = string.Empty;
            this.Description7 = string.Empty;
            this.Age7 = 0;
            this.EducationYears7 = 0;
            this.Race7 = RACE_TYPE.white.ToString();
            this.Gender7 = GENDER_TYPE.female.ToString();
            this.Height7 = 0;
            this.Weight7 = 0;
            this.ActivityType7 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType7 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome7 = 0;
            this.BMI7 = 0;
            this.CalNeedPerDay7 = 0;

            this.Name8 = string.Empty;
            this.Label8 = string.Empty;
            this.Description8 = string.Empty;
            this.Age8 = 0;
            this.EducationYears8 = 0;
            this.Race8 = RACE_TYPE.white.ToString();
            this.Gender8 = GENDER_TYPE.female.ToString();
            this.Height8 = 0;
            this.Weight8 = 0;
            this.ActivityType8 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType8 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome8 = 0;
            this.BMI8 = 0;
            this.CalNeedPerDay8 = 0;

            this.Name9 = string.Empty;
            this.Label9 = string.Empty;
            this.Description9 = string.Empty;
            this.Age9 = 0;
            this.EducationYears9 = 0;
            this.Race9 = RACE_TYPE.white.ToString();
            this.Gender9 = GENDER_TYPE.female.ToString();
            this.Height9 = 0;
            this.Weight9 = 0;
            this.ActivityType9 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType9 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome9 = 0;
            this.BMI9 = 0;
            this.CalNeedPerDay9 = 0;

            this.Name10 = string.Empty;
            this.Label10 = string.Empty;
            this.Description10 = string.Empty;
            this.Age10 = 0;
            this.EducationYears10 = 0;
            this.Race10 = RACE_TYPE.white.ToString();
            this.Gender10 = GENDER_TYPE.female.ToString();
            this.Height10 = 0;
            this.Weight10 = 0;
            this.ActivityType10 = ACTIVITY_TYPE.sedentary.ToString();
            this.HealthConditionsType10 = HEALTHCONDITIONS_TYPE.none.ToString();
            this.HouseholdYearIncome10 = 0;
            this.BMI10 = 0;
            this.CalNeedPerDay10 = 0;
        }

        public virtual void CopyDemog2Properties(
            Demog2 calculator)
        {
            this.MeasurementUnits = calculator.MeasurementUnits;
            this.Name1 = calculator.Name1;
            this.Label1 = calculator.Label1;
            this.Description1 = calculator.Description1;
            this.Age1 = calculator.Age1;
            this.EducationYears1 = calculator.EducationYears1;
            this.Race1 = calculator.Race1;
            this.Gender1 = calculator.Gender1;
            this.Height1 = calculator.Height1;
            this.Weight1 = calculator.Weight1;
            this.ActivityType1 = calculator.ActivityType1;
            this.HealthConditionsType1 = calculator.HealthConditionsType1;
            this.HouseholdYearIncome1 = calculator.HouseholdYearIncome1;
            this.BMI1 = calculator.BMI1;
            this.CalNeedPerDay1 = calculator.CalNeedPerDay1;

            this.Name2 = calculator.Name2;
            this.Label2 = calculator.Label2;
            this.Description2 = calculator.Description2;
            this.Age2 = calculator.Age2;
            this.EducationYears2 = calculator.EducationYears2;
            this.Race2 = calculator.Race2;
            this.Gender2 = calculator.Gender2;
            this.Height2 = calculator.Height2;
            this.Weight2 = calculator.Weight2;
            this.ActivityType2 = calculator.ActivityType2;
            this.HealthConditionsType2 = calculator.HealthConditionsType2;
            this.HouseholdYearIncome2 = calculator.HouseholdYearIncome2;
            this.BMI2 = calculator.BMI2;
            this.CalNeedPerDay2 = calculator.CalNeedPerDay2;

            this.Name3 = calculator.Name3;
            this.Label3 = calculator.Label3;
            this.Description3 = calculator.Description3;
            this.Age3 = calculator.Age3;
            this.EducationYears3 = calculator.EducationYears3;
            this.Race3 = calculator.Race3;
            this.Gender3 = calculator.Gender3;
            this.Height3 = calculator.Height3;
            this.Weight3 = calculator.Weight3;
            this.ActivityType3 = calculator.ActivityType3;
            this.HealthConditionsType3 = calculator.HealthConditionsType3;
            this.HouseholdYearIncome3 = calculator.HouseholdYearIncome3;
            this.BMI3 = calculator.BMI3;
            this.CalNeedPerDay3 = calculator.CalNeedPerDay3;

            this.Name4 = calculator.Name4;
            this.Label4 = calculator.Label4;
            this.Description4 = calculator.Description4;
            this.Age4 = calculator.Age4;
            this.EducationYears4 = calculator.EducationYears4;
            this.Race4 = calculator.Race4;
            this.Gender4 = calculator.Gender4;
            this.Height4 = calculator.Height4;
            this.Weight4 = calculator.Weight4;
            this.ActivityType4 = calculator.ActivityType4;
            this.HealthConditionsType4 = calculator.HealthConditionsType4;
            this.HouseholdYearIncome4 = calculator.HouseholdYearIncome4;
            this.BMI4 = calculator.BMI4;
            this.CalNeedPerDay4 = calculator.CalNeedPerDay4;

            this.Name5 = calculator.Name5;
            this.Label5 = calculator.Label5;
            this.Description5 = calculator.Description5;
            this.Age5 = calculator.Age5;
            this.EducationYears5 = calculator.EducationYears5;
            this.Race5 = calculator.Race5;
            this.Gender5 = calculator.Gender5;
            this.Height5 = calculator.Height5;
            this.Weight5 = calculator.Weight5;
            this.ActivityType5 = calculator.ActivityType5;
            this.HealthConditionsType5 = calculator.HealthConditionsType5;
            this.HouseholdYearIncome5 = calculator.HouseholdYearIncome5;
            this.BMI5 = calculator.BMI5;
            this.CalNeedPerDay5 = calculator.CalNeedPerDay5;

            this.Name6 = calculator.Name6;
            this.Label6 = calculator.Label6;
            this.Description6 = calculator.Description6;
            this.Age6 = calculator.Age6;
            this.EducationYears6 = calculator.EducationYears6;
            this.Race6 = calculator.Race6;
            this.Gender6 = calculator.Gender6;
            this.Height6 = calculator.Height6;
            this.Weight6 = calculator.Weight6;
            this.ActivityType6 = calculator.ActivityType6;
            this.HealthConditionsType6 = calculator.HealthConditionsType6;
            this.HouseholdYearIncome6 = calculator.HouseholdYearIncome6;
            this.BMI6 = calculator.BMI6;
            this.CalNeedPerDay6 = calculator.CalNeedPerDay6;

            this.Name7 = calculator.Name7;
            this.Label7 = calculator.Label7;
            this.Description7 = calculator.Description7;
            this.Age7 = calculator.Age7;
            this.EducationYears7 = calculator.EducationYears7;
            this.Race7 = calculator.Race7;
            this.Gender7 = calculator.Gender7;
            this.Height7 = calculator.Height7;
            this.Weight7 = calculator.Weight7;
            this.ActivityType7 = calculator.ActivityType7;
            this.HealthConditionsType7 = calculator.HealthConditionsType7;
            this.HouseholdYearIncome7 = calculator.HouseholdYearIncome7;
            this.BMI7 = calculator.BMI7;
            this.CalNeedPerDay7 = calculator.CalNeedPerDay7;

            this.Name8 = calculator.Name8;
            this.Label8 = calculator.Label8;
            this.Description8 = calculator.Description8;
            this.Age8 = calculator.Age8;
            this.EducationYears8 = calculator.EducationYears8;
            this.Race8 = calculator.Race8;
            this.Gender8 = calculator.Gender8;
            this.Height8 = calculator.Height8;
            this.Weight8 = calculator.Weight8;
            this.ActivityType8 = calculator.ActivityType8;
            this.HealthConditionsType8 = calculator.HealthConditionsType8;
            this.HouseholdYearIncome8 = calculator.HouseholdYearIncome8;
            this.BMI8 = calculator.BMI8;
            this.CalNeedPerDay8 = calculator.CalNeedPerDay8;

            this.Name9 = calculator.Name9;
            this.Label9 = calculator.Label9;
            this.Description9 = calculator.Description9;
            this.Age9 = calculator.Age9;
            this.EducationYears9 = calculator.EducationYears9;
            this.Race9 = calculator.Race9;
            this.Gender9 = calculator.Gender9;
            this.Height9 = calculator.Height9;
            this.Weight9 = calculator.Weight9;
            this.ActivityType9 = calculator.ActivityType9;
            this.HealthConditionsType9 = calculator.HealthConditionsType9;
            this.HouseholdYearIncome9 = calculator.HouseholdYearIncome9;
            this.BMI9 = calculator.BMI9;
            this.CalNeedPerDay9 = calculator.CalNeedPerDay9;

            this.Name10 = calculator.Name10;
            this.Label10 = calculator.Label10;
            this.Description10 = calculator.Description10;
            this.Age10 = calculator.Age10;
            this.EducationYears10 = calculator.EducationYears10;
            this.Race10 = calculator.Race10;
            this.Gender10 = calculator.Gender10;
            this.Height10 = calculator.Height10;
            this.Weight10 = calculator.Weight10;
            this.ActivityType10 = calculator.ActivityType10;
            this.HealthConditionsType10 = calculator.HealthConditionsType10;
            this.HouseholdYearIncome10 = calculator.HouseholdYearIncome10;
            this.BMI10 = calculator.BMI10;
            this.CalNeedPerDay10 = calculator.CalNeedPerDay10;
        }

        //set the class properties using the XElement
        public virtual void SetDemog2Properties(XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            this.MeasurementUnits = CalculatorHelpers.GetAttribute(currentCalculationsElement, cMeasurementUnits);
            this.Name1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName1);
            this.Label1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel1);
            this.Description1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription1);
            this.Age1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge1);
            this.EducationYears1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears1);
            this.Race1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace1);
            this.Gender1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender1);
            this.Height1 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight1);
            this.Weight1 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight1);
            this.ActivityType1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType1);
            this.HealthConditionsType1 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType1);
            this.HouseholdYearIncome1 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome1);
            this.BMI1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI1);
            this.CalNeedPerDay1 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay1);

            this.Name2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName2);
            this.Label2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel2);
            this.Description2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription2);
            this.Age2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge2);
            this.EducationYears2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears2);
            this.Race2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace2);
            this.Gender2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender2);
            this.Height2 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight2);
            this.Weight2 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight2);
            this.ActivityType2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType2);
            this.HealthConditionsType2 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType2);
            this.HouseholdYearIncome2 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome2);
            this.BMI2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI2);
            this.CalNeedPerDay2 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay2);

            this.Name3 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName3);
            this.Label3 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel3);
            this.Description3 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription3);
            this.Age3 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge3);
            this.EducationYears3 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears3);
            this.Race3 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace3);
            this.Gender3 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender3);
            this.Height3 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight3);
            this.Weight3 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight3);
            this.ActivityType3 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType3);
            this.HealthConditionsType3 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType3);
            this.HouseholdYearIncome3 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome3);
            this.BMI3 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI3);
            this.CalNeedPerDay3 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay3);

            this.Name4 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName4);
            this.Label4 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel4);
            this.Description4 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription4);
            this.Age4 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge4);
            this.EducationYears4 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears4);
            this.Race4 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace4);
            this.Gender4 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender4);
            this.Height4 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight4);
            this.Weight4 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight4);
            this.ActivityType4 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType4);
            this.HealthConditionsType4 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType4);
            this.HouseholdYearIncome4 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome4);
            this.BMI4 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI4);
            this.CalNeedPerDay4 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay4);

            this.Name5 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName5);
            this.Label5 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel5);
            this.Description5 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription5);
            this.Age5 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge5);
            this.EducationYears5 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears5);
            this.Race5 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace5);
            this.Gender5 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender5);
            this.Height5 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight5);
            this.Weight5 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight5);
            this.ActivityType5 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType5);
            this.HealthConditionsType5 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType5);
            this.HouseholdYearIncome5 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome5);
            this.BMI5 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI5);
            this.CalNeedPerDay5 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay5);

            this.Name6 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName6);
            this.Label6 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel6);
            this.Description6 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription6);
            this.Age6 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge6);
            this.EducationYears6 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears6);
            this.Race6 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace6);
            this.Gender6 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender6);
            this.Height6 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight6);
            this.Weight6 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight6);
            this.ActivityType6 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType6);
            this.HealthConditionsType6 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType6);
            this.HouseholdYearIncome6 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome6);
            this.BMI6 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI6);
            this.CalNeedPerDay6 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay6);

            this.Name7 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName7);
            this.Label7 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel7);
            this.Description7 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription7);
            this.Age7 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge7);
            this.EducationYears7 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears7);
            this.Race7 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace7);
            this.Gender7 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender7);
            this.Height7 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight7);
            this.Weight7 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight7);
            this.ActivityType7 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType7);
            this.HealthConditionsType7 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType7);
            this.HouseholdYearIncome7 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome7);
            this.BMI7 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI7);
            this.CalNeedPerDay7 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay7);

            this.Name8 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName8);
            this.Label8 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel8);
            this.Description8 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription8);
            this.Age8 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge8);
            this.EducationYears8 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears8);
            this.Race8 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace8);
            this.Gender8 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender8);
            this.Height8 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight8);
            this.Weight8 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight8);
            this.ActivityType8 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType8);
            this.HealthConditionsType8 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType8);
            this.HouseholdYearIncome8 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome8);
            this.BMI8 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI8);
            this.CalNeedPerDay8 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay8);

            this.Name9 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName9);
            this.Label9 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel9);
            this.Description9 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription9);
            this.Age9 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge9);
            this.EducationYears9 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears9);
            this.Race9 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace9);
            this.Gender9 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender9);
            this.Height9 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight9);
            this.Weight9 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight9);
            this.ActivityType9 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType9);
            this.HealthConditionsType9 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType9);
            this.HouseholdYearIncome9 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome9);
            this.BMI9 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI9);
            this.CalNeedPerDay9 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay9);

            this.Name10 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cName10);
            this.Label10 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLabel10);
            this.Description10 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cDescription10);
            this.Age10 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge10);
            this.EducationYears10 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears10);
            this.Race10 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace10);
            this.Gender10 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender10);
            this.Height10 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHeight10);
            this.Weight10 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cWeight10);
            this.ActivityType10 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cActivityType10);
            this.HealthConditionsType10 = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHealthConditionsType10);
            this.HouseholdYearIncome10 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHouseholdYearIncome10);
            this.BMI10 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBMI10);
            this.CalNeedPerDay10 = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cCalNeedPerDay10);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetDemog2Properties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cMeasurementUnits:
                    this.MeasurementUnits = attValue;
                    break;
                case cName1:
                    this.Name1 = attValue;
                    break;
                case cLabel1:
                    this.Label1 = attValue;
                    break;
                case cDescription1:
                    this.Description1 = attValue;
                    break;
                case cAge1:
                    this.Age1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears1:
                    this.EducationYears1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace1:
                    this.Race1 = attValue;
                    break;
                case cGender1:
                    this.Gender1 = attValue;
                    break;
                case cHeight1:
                    this.Height1 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight1:
                    this.Weight1 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType1:
                    this.ActivityType1 = attValue;
                    break;
                case cHealthConditionsType1:
                    this.HealthConditionsType1 = attValue;
                    break;
                case cHouseholdYearIncome1:
                    this.HouseholdYearIncome1 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI1:
                    this.BMI1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay1:
                    this.CalNeedPerDay1 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName2:
                    this.Name2 = attValue;
                    break;
                case cLabel2:
                    this.Label2 = attValue;
                    break;
                case cDescription2:
                    this.Description2 = attValue;
                    break;
                case cAge2:
                    this.Age2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears2:
                    this.EducationYears2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace2:
                    this.Race2 = attValue;
                    break;
                case cGender2:
                    this.Gender2 = attValue;
                    break;
                case cHeight2:
                    this.Height2 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight2:
                    this.Weight2 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType2:
                    this.ActivityType2 = attValue;
                    break;
                case cHealthConditionsType2:
                    this.HealthConditionsType2 = attValue;
                    break;
                case cHouseholdYearIncome2:
                    this.HouseholdYearIncome2 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI2:
                    this.BMI2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay2:
                    this.CalNeedPerDay2 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName3:
                    this.Name3 = attValue;
                    break;
                case cLabel3:
                    this.Label3 = attValue;
                    break;
                case cDescription3:
                    this.Description3 = attValue;
                    break;
                case cAge3:
                    this.Age3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears3:
                    this.EducationYears3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace3:
                    this.Race3 = attValue;
                    break;
                case cGender3:
                    this.Gender3 = attValue;
                    break;
                case cHeight3:
                    this.Height3 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight3:
                    this.Weight3 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType3:
                    this.ActivityType3 = attValue;
                    break;
                case cHealthConditionsType3:
                    this.HealthConditionsType3 = attValue;
                    break;
                case cHouseholdYearIncome3:
                    this.HouseholdYearIncome3 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI3:
                    this.BMI3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay3:
                    this.CalNeedPerDay3 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName4:
                    this.Name4 = attValue;
                    break;
                case cLabel4:
                    this.Label4 = attValue;
                    break;
                case cDescription4:
                    this.Description4 = attValue;
                    break;
                case cAge4:
                    this.Age4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears4:
                    this.EducationYears4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace4:
                    this.Race4 = attValue;
                    break;
                case cGender4:
                    this.Gender4 = attValue;
                    break;
                case cHeight4:
                    this.Height4 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight4:
                    this.Weight4 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType4:
                    this.ActivityType4 = attValue;
                    break;
                case cHealthConditionsType4:
                    this.HealthConditionsType4 = attValue;
                    break;
                case cHouseholdYearIncome4:
                    this.HouseholdYearIncome4 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI4:
                    this.BMI4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay4:
                    this.CalNeedPerDay4 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName5:
                    this.Name5 = attValue;
                    break;
                case cLabel5:
                    this.Label5 = attValue;
                    break;
                case cDescription5:
                    this.Description5 = attValue;
                    break;
                case cAge5:
                    this.Age5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears5:
                    this.EducationYears5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace5:
                    this.Race5 = attValue;
                    break;
                case cGender5:
                    this.Gender5 = attValue;
                    break;
                case cHeight5:
                    this.Height5 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight5:
                    this.Weight5 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType5:
                    this.ActivityType5 = attValue;
                    break;
                case cHealthConditionsType5:
                    this.HealthConditionsType5 = attValue;
                    break;
                case cHouseholdYearIncome5:
                    this.HouseholdYearIncome5 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI5:
                    this.BMI5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay5:
                    this.CalNeedPerDay5 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName6:
                    this.Name6 = attValue;
                    break;
                case cLabel6:
                    this.Label6 = attValue;
                    break;
                case cDescription6:
                    this.Description6 = attValue;
                    break;
                case cAge6:
                    this.Age6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears6:
                    this.EducationYears6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace6:
                    this.Race6 = attValue;
                    break;
                case cGender6:
                    this.Gender6 = attValue;
                    break;
                case cHeight6:
                    this.Height6 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight6:
                    this.Weight6 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType6:
                    this.ActivityType6 = attValue;
                    break;
                case cHealthConditionsType6:
                    this.HealthConditionsType6 = attValue;
                    break;
                case cHouseholdYearIncome6:
                    this.HouseholdYearIncome6 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI6:
                    this.BMI6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay6:
                    this.CalNeedPerDay6 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName7:
                    this.Name7 = attValue;
                    break;
                case cLabel7:
                    this.Label7 = attValue;
                    break;
                case cDescription7:
                    this.Description7 = attValue;
                    break;
                case cAge7:
                    this.Age7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears7:
                    this.EducationYears7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace7:
                    this.Race7 = attValue;
                    break;
                case cGender7:
                    this.Gender7 = attValue;
                    break;
                case cHeight7:
                    this.Height7 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight7:
                    this.Weight7 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType7:
                    this.ActivityType7 = attValue;
                    break;
                case cHealthConditionsType7:
                    this.HealthConditionsType7 = attValue;
                    break;
                case cHouseholdYearIncome7:
                    this.HouseholdYearIncome7 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI7:
                    this.BMI7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay7:
                    this.CalNeedPerDay7 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName8:
                    this.Name8 = attValue;
                    break;
                case cLabel8:
                    this.Label8 = attValue;
                    break;
                case cDescription8:
                    this.Description8 = attValue;
                    break;
                case cAge8:
                    this.Age8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears8:
                    this.EducationYears8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace8:
                    this.Race8 = attValue;
                    break;
                case cGender8:
                    this.Gender8 = attValue;
                    break;
                case cHeight8:
                    this.Height8 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight8:
                    this.Weight8 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType8:
                    this.ActivityType8 = attValue;
                    break;
                case cHealthConditionsType8:
                    this.HealthConditionsType8 = attValue;
                    break;
                case cHouseholdYearIncome8:
                    this.HouseholdYearIncome8 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI8:
                    this.BMI8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay8:
                    this.CalNeedPerDay8 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName9:
                    this.Name9 = attValue;
                    break;
                case cLabel9:
                    this.Label9 = attValue;
                    break;
                case cDescription9:
                    this.Description9 = attValue;
                    break;
                case cAge9:
                    this.Age9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears9:
                    this.EducationYears9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace9:
                    this.Race9 = attValue;
                    break;
                case cGender9:
                    this.Gender9 = attValue;
                    break;
                case cHeight9:
                    this.Height9 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight9:
                    this.Weight9 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType9:
                    this.ActivityType9 = attValue;
                    break;
                case cHealthConditionsType9:
                    this.HealthConditionsType9 = attValue;
                    break;
                case cHouseholdYearIncome9:
                    this.HouseholdYearIncome9 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI9:
                    this.BMI9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay9:
                    this.CalNeedPerDay9 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cName10:
                    this.Name10 = attValue;
                    break;
                case cLabel10:
                    this.Label10 = attValue;
                    break;
                case cDescription10:
                    this.Description10 = attValue;
                    break;
                case cAge10:
                    this.Age10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears10:
                    this.EducationYears10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace10:
                    this.Race10 = attValue;
                    break;
                case cGender10:
                    this.Gender10 = attValue;
                    break;
                case cHeight10:
                    this.Height10 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cWeight10:
                    this.Weight10 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cActivityType10:
                    this.ActivityType10 = attValue;
                    break;
                case cHealthConditionsType10:
                    this.HealthConditionsType10 = attValue;
                    break;
                case cHouseholdYearIncome10:
                    this.HouseholdYearIncome10 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBMI10:
                    this.BMI10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalNeedPerDay10:
                    this.CalNeedPerDay10 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetDemog2Attributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            if (this.Name1 != string.Empty && this.Name1 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cMeasurementUnits, attNameExtension), this.MeasurementUnits);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName1, attNameExtension), this.Name1);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel1, attNameExtension), this.Label1);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription1, attNameExtension), this.Description1);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge1, attNameExtension), this.Age1);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears1, attNameExtension), this.EducationYears1);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace1, attNameExtension), this.Race1);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender1, attNameExtension), this.Gender1);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight1, attNameExtension), this.Height1);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight1, attNameExtension), this.Weight1);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType1, attNameExtension), this.ActivityType1);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType1, attNameExtension), this.HealthConditionsType1);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome1, attNameExtension), this.HouseholdYearIncome1);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI1, attNameExtension), this.BMI1);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay1, attNameExtension), this.CalNeedPerDay1);
            }
            if (this.Name2 != string.Empty && this.Name2 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName2, attNameExtension), this.Name2);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel2, attNameExtension), this.Label2);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription2, attNameExtension), this.Description2);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge2, attNameExtension), this.Age2);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears2, attNameExtension), this.EducationYears2);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace2, attNameExtension), this.Race2);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender2, attNameExtension), this.Gender2);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight2, attNameExtension), this.Height2);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight2, attNameExtension), this.Weight2);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType2, attNameExtension), this.ActivityType2);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType2, attNameExtension), this.HealthConditionsType2);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome2, attNameExtension), this.HouseholdYearIncome2);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI2, attNameExtension), this.BMI2);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay2, attNameExtension), this.CalNeedPerDay2);
            }
            if (this.Name3 != string.Empty && this.Name3 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName3, attNameExtension), this.Name3);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel3, attNameExtension), this.Label3);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription3, attNameExtension), this.Description3);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge3, attNameExtension), this.Age3);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears3, attNameExtension), this.EducationYears3);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace3, attNameExtension), this.Race3);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender3, attNameExtension), this.Gender3);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight3, attNameExtension), this.Height3);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight3, attNameExtension), this.Weight3);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType3, attNameExtension), this.ActivityType3);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType3, attNameExtension), this.HealthConditionsType3);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome3, attNameExtension), this.HouseholdYearIncome3);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI3, attNameExtension), this.BMI3);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay3, attNameExtension), this.CalNeedPerDay3);
            }
            if (this.Name4 != string.Empty && this.Name4 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName4, attNameExtension), this.Name4);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel4, attNameExtension), this.Label4);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription4, attNameExtension), this.Description4);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge4, attNameExtension), this.Age4);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears4, attNameExtension), this.EducationYears4);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace4, attNameExtension), this.Race4);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender4, attNameExtension), this.Gender4);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight4, attNameExtension), this.Height4);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight4, attNameExtension), this.Weight4);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType4, attNameExtension), this.ActivityType4);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType4, attNameExtension), this.HealthConditionsType4);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome4, attNameExtension), this.HouseholdYearIncome4);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI4, attNameExtension), this.BMI4);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay4, attNameExtension), this.CalNeedPerDay4);
            }
            if (this.Name5 != string.Empty && this.Name5 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName5, attNameExtension), this.Name5);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel5, attNameExtension), this.Label5);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription5, attNameExtension), this.Description5);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge5, attNameExtension), this.Age5);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears5, attNameExtension), this.EducationYears5);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace5, attNameExtension), this.Race5);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender5, attNameExtension), this.Gender5);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight5, attNameExtension), this.Height5);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight5, attNameExtension), this.Weight5);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType5, attNameExtension), this.ActivityType5);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType5, attNameExtension), this.HealthConditionsType5);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome5, attNameExtension), this.HouseholdYearIncome5);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI5, attNameExtension), this.BMI5);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay5, attNameExtension), this.CalNeedPerDay5);
            }
            if (this.Name6 != string.Empty && this.Name6 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName6, attNameExtension), this.Name6);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel6, attNameExtension), this.Label6);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription6, attNameExtension), this.Description6);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge6, attNameExtension), this.Age6);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears6, attNameExtension), this.EducationYears6);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace6, attNameExtension), this.Race6);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender6, attNameExtension), this.Gender6);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight6, attNameExtension), this.Height6);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight6, attNameExtension), this.Weight6);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType6, attNameExtension), this.ActivityType6);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType6, attNameExtension), this.HealthConditionsType6);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome6, attNameExtension), this.HouseholdYearIncome6);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI6, attNameExtension), this.BMI6);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay6, attNameExtension), this.CalNeedPerDay6);
            }
            if (this.Name7 != string.Empty && this.Name7 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName7, attNameExtension), this.Name7);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel7, attNameExtension), this.Label7);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription7, attNameExtension), this.Description7);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge7, attNameExtension), this.Age7);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears7, attNameExtension), this.EducationYears7);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace7, attNameExtension), this.Race7);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender7, attNameExtension), this.Gender7);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight7, attNameExtension), this.Height7);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight7, attNameExtension), this.Weight7);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType7, attNameExtension), this.ActivityType7);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType7, attNameExtension), this.HealthConditionsType7);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome7, attNameExtension), this.HouseholdYearIncome7);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI7, attNameExtension), this.BMI7);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay7, attNameExtension), this.CalNeedPerDay7);
            }
            if (this.Name8 != string.Empty && this.Name8 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName8, attNameExtension), this.Name8);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel8, attNameExtension), this.Label8);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription8, attNameExtension), this.Description8);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge8, attNameExtension), this.Age8);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears8, attNameExtension), this.EducationYears8);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace8, attNameExtension), this.Race8);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender8, attNameExtension), this.Gender8);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight8, attNameExtension), this.Height8);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight8, attNameExtension), this.Weight8);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType8, attNameExtension), this.ActivityType8);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType8, attNameExtension), this.HealthConditionsType8);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome8, attNameExtension), this.HouseholdYearIncome8);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI8, attNameExtension), this.BMI8);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay8, attNameExtension), this.CalNeedPerDay8);
            }
            if (this.Name9 != string.Empty && this.Name9 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName9, attNameExtension), this.Name9);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel9, attNameExtension), this.Label9);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription9, attNameExtension), this.Description9);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge9, attNameExtension), this.Age9);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears9, attNameExtension), this.EducationYears9);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace9, attNameExtension), this.Race9);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender9, attNameExtension), this.Gender9);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight9, attNameExtension), this.Height9);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight9, attNameExtension), this.Weight9);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType9, attNameExtension), this.ActivityType9);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType9, attNameExtension), this.HealthConditionsType9);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome9, attNameExtension), this.HouseholdYearIncome9);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI9, attNameExtension), this.BMI9);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay9, attNameExtension), this.CalNeedPerDay9);
            }
            if (this.Name10 != string.Empty && this.Name10 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cName10, attNameExtension), this.Name10);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cLabel10, attNameExtension), this.Label10);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cDescription10, attNameExtension), this.Description10);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cAge10, attNameExtension), this.Age10);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cEducationYears10, attNameExtension), this.EducationYears10);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cRace10, attNameExtension), this.Race10);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cGender10, attNameExtension), this.Gender10);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHeight10, attNameExtension), this.Height10);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cWeight10, attNameExtension), this.Weight10);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cActivityType10, attNameExtension), this.ActivityType10);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cHealthConditionsType10, attNameExtension), this.HealthConditionsType10);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                    string.Concat(cHouseholdYearIncome10, attNameExtension), this.HouseholdYearIncome10);
                CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                    string.Concat(cBMI10, attNameExtension), this.BMI10);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   string.Concat(cCalNeedPerDay10, attNameExtension), this.CalNeedPerDay10);
            }
        }
        public virtual void SetDemog2Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            if (this.Name1 != string.Empty && this.Name1 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cMeasurementUnits, attNameExtension), this.MeasurementUnits.ToString());
                writer.WriteAttributeString(string.Concat(cName1, attNameExtension), this.Name1.ToString());
                writer.WriteAttributeString(string.Concat(cLabel1, attNameExtension), this.Label1.ToString());
                writer.WriteAttributeString(string.Concat(cDescription1, attNameExtension), this.Description1.ToString());
                writer.WriteAttributeString(string.Concat(cAge1, attNameExtension), this.Age1.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears1, attNameExtension), this.EducationYears1.ToString());
                writer.WriteAttributeString(string.Concat(cRace1, attNameExtension), this.Race1.ToString());
                writer.WriteAttributeString(string.Concat(cGender1, attNameExtension), this.Gender1);
                writer.WriteAttributeString(string.Concat(cHeight1, attNameExtension), this.Height1.ToString());
                writer.WriteAttributeString(string.Concat(cWeight1, attNameExtension), this.Weight1.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType1, attNameExtension), this.ActivityType1);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType1, attNameExtension), this.HealthConditionsType1);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome1, attNameExtension), this.HouseholdYearIncome1.ToString());
                writer.WriteAttributeString(string.Concat(cBMI1, attNameExtension), this.BMI1.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay1, attNameExtension), this.CalNeedPerDay1.ToString());
            }
            if (this.Name2 != string.Empty && this.Name2 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName2, attNameExtension), this.Name2.ToString());
                writer.WriteAttributeString(string.Concat(cLabel2, attNameExtension), this.Label2.ToString());
                writer.WriteAttributeString(string.Concat(cDescription2, attNameExtension), this.Description2.ToString());
                writer.WriteAttributeString(string.Concat(cAge2, attNameExtension), this.Age2.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears2, attNameExtension), this.EducationYears2.ToString());
                writer.WriteAttributeString(string.Concat(cRace2, attNameExtension), this.Race2.ToString());
                writer.WriteAttributeString(string.Concat(cGender2, attNameExtension), this.Gender2);
                writer.WriteAttributeString(string.Concat(cHeight2, attNameExtension), this.Height2.ToString());
                writer.WriteAttributeString(string.Concat(cWeight2, attNameExtension), this.Weight2.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType2, attNameExtension), this.ActivityType2);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType2, attNameExtension), this.HealthConditionsType2);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome2, attNameExtension), this.HouseholdYearIncome2.ToString());
                writer.WriteAttributeString(string.Concat(cBMI2, attNameExtension), this.BMI2.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay2, attNameExtension), this.CalNeedPerDay2.ToString());
            }
            if (this.Name3 != string.Empty && this.Name3 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName3, attNameExtension), this.Name3.ToString());
                writer.WriteAttributeString(string.Concat(cLabel3, attNameExtension), this.Label3.ToString());
                writer.WriteAttributeString(string.Concat(cDescription3, attNameExtension), this.Description3.ToString());
                writer.WriteAttributeString(string.Concat(cAge3, attNameExtension), this.Age3.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears3, attNameExtension), this.EducationYears3.ToString());
                writer.WriteAttributeString(string.Concat(cRace3, attNameExtension), this.Race3.ToString());
                writer.WriteAttributeString(string.Concat(cGender3, attNameExtension), this.Gender3);
                writer.WriteAttributeString(string.Concat(cHeight3, attNameExtension), this.Height3.ToString());
                writer.WriteAttributeString(string.Concat(cWeight3, attNameExtension), this.Weight3.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType3, attNameExtension), this.ActivityType3);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType3, attNameExtension), this.HealthConditionsType3);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome3, attNameExtension), this.HouseholdYearIncome3.ToString());
                writer.WriteAttributeString(string.Concat(cBMI3, attNameExtension), this.BMI3.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay3, attNameExtension), this.CalNeedPerDay3.ToString());
            }
            if (this.Name4 != string.Empty && this.Name4 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName4, attNameExtension), this.Name4.ToString());
                writer.WriteAttributeString(string.Concat(cLabel4, attNameExtension), this.Label4.ToString());
                writer.WriteAttributeString(string.Concat(cDescription4, attNameExtension), this.Description4.ToString());
                writer.WriteAttributeString(string.Concat(cAge4, attNameExtension), this.Age4.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears4, attNameExtension), this.EducationYears4.ToString());
                writer.WriteAttributeString(string.Concat(cRace4, attNameExtension), this.Race4.ToString());
                writer.WriteAttributeString(string.Concat(cGender4, attNameExtension), this.Gender4);
                writer.WriteAttributeString(string.Concat(cHeight4, attNameExtension), this.Height4.ToString());
                writer.WriteAttributeString(string.Concat(cWeight4, attNameExtension), this.Weight4.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType4, attNameExtension), this.ActivityType4);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType4, attNameExtension), this.HealthConditionsType4);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome4, attNameExtension), this.HouseholdYearIncome4.ToString());
                writer.WriteAttributeString(string.Concat(cBMI4, attNameExtension), this.BMI4.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay4, attNameExtension), this.CalNeedPerDay4.ToString());
            }
            if (this.Name5 != string.Empty && this.Name5 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName5, attNameExtension), this.Name5.ToString());
                writer.WriteAttributeString(string.Concat(cLabel5, attNameExtension), this.Label5.ToString());
                writer.WriteAttributeString(string.Concat(cDescription5, attNameExtension), this.Description5.ToString());
                writer.WriteAttributeString(string.Concat(cAge5, attNameExtension), this.Age5.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears5, attNameExtension), this.EducationYears5.ToString());
                writer.WriteAttributeString(string.Concat(cRace5, attNameExtension), this.Race5.ToString());
                writer.WriteAttributeString(string.Concat(cGender5, attNameExtension), this.Gender5);
                writer.WriteAttributeString(string.Concat(cHeight5, attNameExtension), this.Height5.ToString());
                writer.WriteAttributeString(string.Concat(cWeight5, attNameExtension), this.Weight5.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType5, attNameExtension), this.ActivityType5);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType5, attNameExtension), this.HealthConditionsType5);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome5, attNameExtension), this.HouseholdYearIncome5.ToString());
                writer.WriteAttributeString(string.Concat(cBMI5, attNameExtension), this.BMI5.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay5, attNameExtension), this.CalNeedPerDay5.ToString());
            }
            if (this.Name6 != string.Empty && this.Name6 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName6, attNameExtension), this.Name6.ToString());
                writer.WriteAttributeString(string.Concat(cLabel6, attNameExtension), this.Label6.ToString());
                writer.WriteAttributeString(string.Concat(cDescription6, attNameExtension), this.Description6.ToString());
                writer.WriteAttributeString(string.Concat(cAge6, attNameExtension), this.Age6.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears6, attNameExtension), this.EducationYears6.ToString());
                writer.WriteAttributeString(string.Concat(cRace6, attNameExtension), this.Race6.ToString());
                writer.WriteAttributeString(string.Concat(cGender6, attNameExtension), this.Gender6);
                writer.WriteAttributeString(string.Concat(cHeight6, attNameExtension), this.Height6.ToString());
                writer.WriteAttributeString(string.Concat(cWeight6, attNameExtension), this.Weight6.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType6, attNameExtension), this.ActivityType6);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType6, attNameExtension), this.HealthConditionsType6);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome6, attNameExtension), this.HouseholdYearIncome6.ToString());
                writer.WriteAttributeString(string.Concat(cBMI6, attNameExtension), this.BMI6.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay6, attNameExtension), this.CalNeedPerDay6.ToString());
            }
            if (this.Name7 != string.Empty && this.Name7 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName7, attNameExtension), this.Name7.ToString());
                writer.WriteAttributeString(string.Concat(cLabel7, attNameExtension), this.Label7.ToString());
                writer.WriteAttributeString(string.Concat(cDescription7, attNameExtension), this.Description7.ToString());
                writer.WriteAttributeString(string.Concat(cAge7, attNameExtension), this.Age7.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears7, attNameExtension), this.EducationYears7.ToString());
                writer.WriteAttributeString(string.Concat(cRace7, attNameExtension), this.Race7.ToString());
                writer.WriteAttributeString(string.Concat(cGender7, attNameExtension), this.Gender7);
                writer.WriteAttributeString(string.Concat(cHeight7, attNameExtension), this.Height7.ToString());
                writer.WriteAttributeString(string.Concat(cWeight7, attNameExtension), this.Weight7.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType7, attNameExtension), this.ActivityType7);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType7, attNameExtension), this.HealthConditionsType7);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome7, attNameExtension), this.HouseholdYearIncome7.ToString());
                writer.WriteAttributeString(string.Concat(cBMI7, attNameExtension), this.BMI7.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay7, attNameExtension), this.CalNeedPerDay7.ToString());
            }
            if (this.Name8 != string.Empty && this.Name8 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName8, attNameExtension), this.Name8.ToString());
                writer.WriteAttributeString(string.Concat(cLabel8, attNameExtension), this.Label8.ToString());
                writer.WriteAttributeString(string.Concat(cDescription8, attNameExtension), this.Description8.ToString());
                writer.WriteAttributeString(string.Concat(cAge8, attNameExtension), this.Age8.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears8, attNameExtension), this.EducationYears8.ToString());
                writer.WriteAttributeString(string.Concat(cRace8, attNameExtension), this.Race8.ToString());
                writer.WriteAttributeString(string.Concat(cGender8, attNameExtension), this.Gender8);
                writer.WriteAttributeString(string.Concat(cHeight8, attNameExtension), this.Height8.ToString());
                writer.WriteAttributeString(string.Concat(cWeight8, attNameExtension), this.Weight8.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType8, attNameExtension), this.ActivityType8);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType8, attNameExtension), this.HealthConditionsType8);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome8, attNameExtension), this.HouseholdYearIncome8.ToString());
                writer.WriteAttributeString(string.Concat(cBMI8, attNameExtension), this.BMI8.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay8, attNameExtension), this.CalNeedPerDay8.ToString());
            }
            if (this.Name9 != string.Empty && this.Name9 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName9, attNameExtension), this.Name9.ToString());
                writer.WriteAttributeString(string.Concat(cLabel9, attNameExtension), this.Label9.ToString());
                writer.WriteAttributeString(string.Concat(cDescription9, attNameExtension), this.Description9.ToString());
                writer.WriteAttributeString(string.Concat(cAge9, attNameExtension), this.Age9.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears9, attNameExtension), this.EducationYears9.ToString());
                writer.WriteAttributeString(string.Concat(cRace9, attNameExtension), this.Race9.ToString());
                writer.WriteAttributeString(string.Concat(cGender9, attNameExtension), this.Gender9);
                writer.WriteAttributeString(string.Concat(cHeight9, attNameExtension), this.Height9.ToString());
                writer.WriteAttributeString(string.Concat(cWeight9, attNameExtension), this.Weight9.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType9, attNameExtension), this.ActivityType9);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType9, attNameExtension), this.HealthConditionsType9);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome9, attNameExtension), this.HouseholdYearIncome9.ToString());
                writer.WriteAttributeString(string.Concat(cBMI9, attNameExtension), this.BMI9.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay9, attNameExtension), this.CalNeedPerDay9.ToString());
            }
            if (this.Name10 != string.Empty && this.Name10 != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cName10, attNameExtension), this.Name10.ToString());
                writer.WriteAttributeString(string.Concat(cLabel10, attNameExtension), this.Label10.ToString());
                writer.WriteAttributeString(string.Concat(cDescription10, attNameExtension), this.Description10.ToString());
                writer.WriteAttributeString(string.Concat(cAge10, attNameExtension), this.Age10.ToString());
                writer.WriteAttributeString(string.Concat(cEducationYears10, attNameExtension), this.EducationYears10.ToString());
                writer.WriteAttributeString(string.Concat(cRace10, attNameExtension), this.Race10.ToString());
                writer.WriteAttributeString(string.Concat(cGender10, attNameExtension), this.Gender10);
                writer.WriteAttributeString(string.Concat(cHeight10, attNameExtension), this.Height10.ToString());
                writer.WriteAttributeString(string.Concat(cWeight10, attNameExtension), this.Weight10.ToString());
                writer.WriteAttributeString(string.Concat(cActivityType10, attNameExtension), this.ActivityType10);
                writer.WriteAttributeString(string.Concat(cHealthConditionsType10, attNameExtension), this.HealthConditionsType10);
                writer.WriteAttributeString(string.Concat(cHouseholdYearIncome10, attNameExtension), this.HouseholdYearIncome10.ToString());
                writer.WriteAttributeString(string.Concat(cBMI10, attNameExtension), this.BMI10.ToString());
                writer.WriteAttributeString(string.Concat(cCalNeedPerDay10, attNameExtension), this.CalNeedPerDay10.ToString());
            }
        }
        public bool SetCalculations(Demog2 demog2, XDocument caloriesNeededDoc)
        {
            bool bHasCalcs = false;
            //set conditions for not dividing by zero
            //if imperial, this must be converted to metric
            if (demog2.Height1 > 0)
            {
                demog2.BMI1 = demog2.Weight1 / demog2.Height1;
                //might be better to use CalcHelpers GetConstants
                demog2.CalNeedPerDay1 = 0;
            }
            if (demog2.Height2 > 0)
            {
                demog2.BMI2 = demog2.Weight2 / demog2.Height2;
                demog2.CalNeedPerDay2 = 0;
            }
            if (demog2.Height3 > 0)
            {
                demog2.BMI3 = demog2.Weight3 / demog2.Height3;
                demog2.CalNeedPerDay3 = 0;
            }
            if (demog2.Height4 > 0)
            {
                demog2.BMI1 = demog2.Weight4 / demog2.Height4;
                demog2.CalNeedPerDay4 = 0;
            }
            if (demog2.Height5 > 0)
            {
                demog2.BMI1 = demog2.Weight5 / demog2.Height5;
                demog2.CalNeedPerDay5 = 0;
            }
            if (demog2.Height6 > 0)
            {
                demog2.BMI1 = demog2.Weight6 / demog2.Height6;
                demog2.CalNeedPerDay6 = 0;
            }
            if (demog2.Height7 > 0)
            {
                demog2.BMI1 = demog2.Weight7 / demog2.Height7;
                demog2.CalNeedPerDay7 = 0;
            }
            if (demog2.Height8 > 0)
            {
                demog2.BMI1 = demog2.Weight8 / demog2.Height8;
                demog2.CalNeedPerDay8 = 0;
            }
            if (demog2.Height9 > 0)
            {
                demog2.BMI1 = demog2.Weight9 / demog2.Height9;
                demog2.CalNeedPerDay9 = 0;
            }
            if (demog2.Height10 > 0)
            {
                demog2.BMI1 = demog2.Weight10 / demog2.Height10;
                demog2.CalNeedPerDay10 = 0;
            }
            bHasCalcs = true;
            return bHasCalcs;
        }
    }
}
