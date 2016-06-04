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
    ///             properties. These demographics are for families, rather 
    ///             than individuals
    ///Author:		www.devtreks.org
    ///Date:		2013, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    public class Demog3
    {
        public Demog3()
            : base()
        {
            //health care benefit object
            InitDemog3Properties();
        }
        //copy constructor
        public Demog3(Demog3 demog11)
        {
            CopyDemog3Properties(demog11);
        }
        public enum HOUSING_TYPE
        {
            rent    = 0,
            own     = 1,
            shelter = 2
        }
        //properties
        public string Housing { get; set; }
        public string WorkStatus { get; set; }
        public string MaritalStatus { get; set; }
        public string LocationId { get; set; }
        public string FamilyIncomeCurrency { get; set; }
        public double FamilyIncomePerYear { get; set; }

        //want demogs to each family member
        //familywealth
        //familyincomesourcetype
        //familysettingtype

        //family income from food stamps, disability, or welfare or other govt
        private const string cHousing = "Housing";
        private const string cWorkStatus = "WorkStatus";
        private const string cMaritalStatus = "MaritalStatus";
        private const string cLocationId = "LocationId";
        private const string cFamilyIncomeCurrency = "FamilyIncomeCurrency";
        private const string cFamilyIncomePerYear = "FamilyIncomePerYear";

        public virtual void InitDemog3Properties()
        {
            //avoid null references to properties
            this.Housing = HOUSING_TYPE.rent.ToString();
            this.WorkStatus = Demog1.WORK_TYPE.paidwork.ToString();
            this.MaritalStatus = Demog1.MARITAL_TYPE.nevermarried.ToString();
            this.LocationId = string.Empty;
            this.FamilyIncomeCurrency = "usdollars";
            this.FamilyIncomePerYear = 0;
        }

        public virtual void CopyDemog3Properties(
            Demog3 calculator)
        {
            this.Housing = calculator.Housing;
            this.WorkStatus = calculator.WorkStatus;
            this.MaritalStatus = calculator.MaritalStatus;
            this.LocationId = calculator.LocationId;
            this.FamilyIncomeCurrency = calculator.FamilyIncomeCurrency;
            this.FamilyIncomePerYear = calculator.FamilyIncomePerYear;
        }

        //set the class properties using the XElement
        public virtual void SetDemog3Properties(XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            this.Housing = CalculatorHelpers.GetAttribute(currentCalculationsElement, cHousing);
            this.WorkStatus = CalculatorHelpers.GetAttribute(currentCalculationsElement, cWorkStatus);
            this.MaritalStatus = CalculatorHelpers.GetAttribute(currentCalculationsElement, cMaritalStatus);
            this.LocationId = CalculatorHelpers.GetAttribute(currentCalculationsElement, cLocationId);
            this.FamilyIncomeCurrency = CalculatorHelpers.GetAttribute(currentCalculationsElement, cFamilyIncomeCurrency);
            this.FamilyIncomePerYear = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cFamilyIncomePerYear);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetDemog3Properties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cHousing:
                    this.Housing = attValue;
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
                case cFamilyIncomeCurrency:
                    this.FamilyIncomeCurrency = attValue;
                    break;
                case cFamilyIncomePerYear:
                    this.FamilyIncomePerYear = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetDemog3Attributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            //don't set any input attributes; each calculator should set what's needed separately
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cHousing, attNameExtension), this.Housing);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cWorkStatus, attNameExtension), this.WorkStatus);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cMaritalStatus, attNameExtension), this.MaritalStatus);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cLocationId, attNameExtension), this.LocationId);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cFamilyIncomeCurrency, attNameExtension), this.FamilyIncomeCurrency);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(cFamilyIncomePerYear, attNameExtension), this.FamilyIncomePerYear);
        }
        public virtual void SetDemog3Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(cHousing, attNameExtension), this.Housing);
            writer.WriteAttributeString(string.Concat(cWorkStatus, attNameExtension), this.WorkStatus.ToString());
            writer.WriteAttributeString(string.Concat(cMaritalStatus, attNameExtension), this.MaritalStatus.ToString());
            writer.WriteAttributeString(string.Concat(cLocationId, attNameExtension), this.LocationId.ToString());
            writer.WriteAttributeString(string.Concat(cFamilyIncomeCurrency, attNameExtension), this.FamilyIncomeCurrency.ToString());
            writer.WriteAttributeString(string.Concat(cFamilyIncomePerYear, attNameExtension), this.FamilyIncomePerYear.ToString());
        }
        public static void FixSelections(Demog3 demogs)
        {
            if (demogs.Housing == string.Empty)
            {
                demogs.Housing = HOUSING_TYPE.rent.ToString();
            }
            if (demogs.WorkStatus == string.Empty)
            {
                demogs.WorkStatus = Demog1.WORK_TYPE.paidwork.ToString();
            }
            if (demogs.MaritalStatus == string.Empty)
            {
                demogs.MaritalStatus = Demog1.MARITAL_TYPE.nevermarried.ToString();
            }
            if (demogs.FamilyIncomeCurrency == string.Empty)
            {
                demogs.FamilyIncomeCurrency = "usdollars";
            }
        }
    }
}
