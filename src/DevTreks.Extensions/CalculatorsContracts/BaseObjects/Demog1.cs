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
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    public class Demog1 
    {
        public Demog1()
            : base()
        {
            //health care benefit object
            InitDemog1Properties();
        }
        //copy constructor
        public Demog1(Demog1 demog11)
        {
            CopyDemog1Properties(demog11);
        }
        //at time of interview
        public enum HOUSING_TYPE
        {
            independentliving       = 0,
            assistedlivingfacility  = 1,
            hospitalized            = 2
        }
        public enum GENDER_TYPE
        {
            female      = 0,
            male        = 1
        }
        public enum RACE_TYPE
        {
            indigenousornative      = 0,
            asian                   = 1,
            blackorafrican          = 2,
            hispanicorlatino        = 3,
            white                   = 4,
            mixed                   = 5
        }
        public enum WORK_TYPE
        {
            paidwork            = 0,
            selfemployed        = 1,
            nonpaidwork         = 2,
            student             = 3,
            homemaker           = 4,
            retired             = 5,
            unemployedforhealth = 6,
            unemployedforother  = 7,
            other               = 8
        }
        public enum MARITAL_TYPE
        {
            nevermarried        = 0,
            currentlymarried    = 1,
            separated           = 2,
            divorced            = 3,
            widowed             = 4,
            cohabiting          = 5
        }
        //properties
        public double Age { get; set; }
        public double EducationYears { get; set; }
        public string Race { get; set; }
        public string Housing { get; set; }
        public string Gender { get; set; }
        public string WorkStatus { get; set; }
        public string MaritalStatus { get; set; }
        public string LocationId { get; set; }
        private const string cAge = "Age";
        private const string cEducationYears = "EducationYears";
        private const string cRace = "Race";
        private const string cHousing = "Housing";
        private const string cGender = "Gender";
        private const string cWorkStatus = "WorkStatus";
        private const string cMaritalStatus = "MaritalStatus";
        private const string cLocationId = "LocationId";
        
        public virtual void InitDemog1Properties()
        {
            //avoid null references to properties
            this.Age = 0;
            this.EducationYears = 0;
            this.Race = RACE_TYPE.white.ToString();
            this.Housing = HOUSING_TYPE.independentliving.ToString();
            this.Gender = GENDER_TYPE.female.ToString();
            this.WorkStatus = WORK_TYPE.paidwork.ToString();
            this.MaritalStatus = MARITAL_TYPE.nevermarried.ToString();
            this.LocationId = string.Empty;
        }

        public virtual void CopyDemog1Properties(
            Demog1 calculator)
        {
            this.Age = calculator.Age;
            this.EducationYears = calculator.EducationYears;
            this.Race = calculator.Race;
            this.Housing = calculator.Housing;
            this.Gender = calculator.Gender;
            this.WorkStatus = calculator.WorkStatus;
            this.MaritalStatus = calculator.MaritalStatus;
            this.LocationId = calculator.LocationId;
        }

        //set the class properties using the XElement
        public virtual void SetDemog1Properties(XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            this.Age = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAge);
            this.EducationYears = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEducationYears);
            this.Race = CalculatorHelpers.GetAttribute(currentCalculationsElement, cRace);
            this.Housing = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHousing);
            this.Gender = CalculatorHelpers.GetAttribute(currentCalculationsElement, cGender);
            this.WorkStatus = CalculatorHelpers.GetAttribute(currentCalculationsElement, cWorkStatus);
            this.MaritalStatus = CalculatorHelpers.GetAttribute(currentCalculationsElement, cMaritalStatus);
            this.LocationId = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLocationId);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetDemog1Properties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cAge:
                    this.Age = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEducationYears:
                    this.EducationYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRace:
                    this.Race = attValue;
                    break;
                case cHousing:
                    this.Housing = attValue;
                    break;
                case cGender:
                    this.Gender = attValue;
                    break;
                case cWorkStatus:
                    this.WorkStatus = attValue;
                    break;
                case cMaritalStatus:
                    this.MaritalStatus = attValue;
                    break;
                case cLocationId:
                    this.LocationId = attValue;
                    break;
                default:
                    break;
            }
        }
        public void SetDemog1Attributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            //don't set any input attributes; each calculator should set what's needed separately
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAge, attNameExtension), this.Age);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cEducationYears, attNameExtension), this.EducationYears);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cRace, attNameExtension), this.Race);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cHousing, attNameExtension), this.Housing);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cGender, attNameExtension), this.Gender);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cWorkStatus, attNameExtension), this.WorkStatus);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cMaritalStatus, attNameExtension), this.MaritalStatus);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cLocationId, attNameExtension), this.LocationId);
        }
        public virtual void SetDemog1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(cAge, attNameExtension), this.Age.ToString());
            writer.WriteAttributeString(string.Concat(cEducationYears, attNameExtension), this.EducationYears.ToString());
            writer.WriteAttributeString(string.Concat(cRace, attNameExtension), this.Race.ToString());
            writer.WriteAttributeString(string.Concat(cHousing, attNameExtension), this.Housing);
            writer.WriteAttributeString(string.Concat(cGender, attNameExtension), this.Gender);
            writer.WriteAttributeString(string.Concat(cWorkStatus, attNameExtension), this.WorkStatus.ToString());
            writer.WriteAttributeString(string.Concat(cMaritalStatus, attNameExtension), this.MaritalStatus.ToString());
            writer.WriteAttributeString(string.Concat(cLocationId, attNameExtension), this.LocationId.ToString());
        }
        public static void FixSelections(Demog1 demogs)
        {
            if (demogs.Race == string.Empty)
            {
                demogs.Race = RACE_TYPE.white.ToString();
            }
            if (demogs.Housing == string.Empty)
            {
                demogs.Housing = HOUSING_TYPE.independentliving.ToString();
            }
            if (demogs.Gender == string.Empty)
            {
                demogs.Gender = GENDER_TYPE.female.ToString();
            }
            if (demogs.WorkStatus == string.Empty)
            {
                demogs.WorkStatus = WORK_TYPE.paidwork.ToString();
            }
            if (demogs.MaritalStatus == string.Empty)
            {
                demogs.MaritalStatus = MARITAL_TYPE.nevermarried.ToString();
            }
        }
    }
}
