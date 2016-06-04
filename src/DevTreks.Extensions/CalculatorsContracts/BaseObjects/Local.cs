using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

using DevTreks.Models;
using DataHelpers = DevTreks.Data.Helpers;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataEditHelpers = DevTreks.Data.EditHelpers;
using RuleHelpers = DevTreks.Data.RuleHelpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The local class is used by many base objects.
    ///Author:		www.devtreks.org
    ///Date:		2013, August
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class Local : CostBenefitCalculator
    {
        public Local() 
        {
            Init();
        }
        //derived constants
        public const string REAL_RATE_ID = DataAppHelpers.Locals.REAL_RATE_ID;
        public const string NOMINAL_RATE_ID = DataAppHelpers.Locals.NOMINAL_RATE_ID;
        public const string UNITGROUP_ID = DataAppHelpers.Locals.UNITGROUP_ID;
        public const string CURRENCYGROUP_ID = DataAppHelpers.Locals.CURRENCYGROUP_ID;
        public const string RATINGGROUP_ID = DataAppHelpers.Locals.RATINGGROUP_ID;
        public const string DATASOURCE_ID = DataAppHelpers.Locals.DATASOURCE_ID;
        public const string GEOCODE_ID = DataAppHelpers.Locals.GEOCODE_ID;
        public const string DATASOURCETECH_ID = DataAppHelpers.Locals.DATASOURCETECH_ID;
        public const string GEOCODETECH_ID = DataAppHelpers.Locals.GEOCODETECH_ID;
        public const string DATASOURCEPRICE_ID = DataAppHelpers.Locals.DATASOURCEPRICE_ID;
        public const string GEOCODEPRICE_ID = DataAppHelpers.Locals.GEOCODEPRICE_ID;
        public const string REAL_RATE = DataAppHelpers.Locals.REAL_RATE;
        public const string NOMINAL_RATE = DataAppHelpers.Locals.NOMINAL_RATE;
        public const string INFLATION_RATE = DataAppHelpers.Locals.INFLATION_RATE;
        public const string INTEREST_RATE_GROUP_ID = DataAppHelpers.Locals.INTEREST_RATE_GROUP_ID;
        public const string GEOCODE_NAME_ID = DataAppHelpers.Locals.GEOCODE_NAME_ID;
        public const string GEOCODE_PARENT_NAME_ID = DataAppHelpers.Locals.GEOCODE_PARENT_NAME_ID;
        public const string GEOCODE_URI = DataAppHelpers.Locals.GEOCODE_URI;
        public const string INTEREST_RATES_DATE = DataAppHelpers.Locals.INTEREST_RATES_DATE;
        public const string GEORREGION_ID = DataAppHelpers.Locals.GEORREGION_ID;
        public enum LOCAL_TYPES
        {
            //the locals belonging to a specific club
            localaccountgroup = 1,
            local = 2
        }
        //properties
        public int UnitGroupId { get; set; }
        public int CurrencyGroupId { get; set; }
        public int RealRateId { get; set; }
        public int NominalRateId { get; set; }
        public int DataSourcePriceId { get; set; }
        public int DataSourceTechId { get; set; }
        public int GeoCodePriceId { get; set; }
        public int GeoCodeTechId { get; set; }
        public int RatingGroupId { get; set; }
        public double RealRate { get; set; }
        public double NominalRate { get; set; }
        public double InflationRate { get; set; }
        //linked calculators/analyzers
        public XElement XmlDocElement { get; set; }
        public void Init()
        {
            this.InitSharedObjectProperties();
            this.UnitGroupId = 0;
            this.CurrencyGroupId = 0;
            this.RealRateId = 0;
            this.NominalRateId = 0;
            this.RatingGroupId = 0;
            this.DataSourcePriceId = 0;
            this.GeoCodePriceId = 0;
            this.DataSourceTechId = 0;
            this.GeoCodeTechId = 0;
            this.RealRate = 0;
            this.NominalRate = 0;
            this.InflationRate = 0;
        }
        public void SetLocalProperties(CalculatorParameters calcParameters,
            XElement elementWithLocals, XElement currentElement)
        {
            //the host base is based on base calculator/analyzer properties (not currentElement)
            this.SetSharedObjectProperties(currentElement);
            this.Id = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            this.UnitGroupId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                DataAppHelpers.Locals.UNITGROUP_ID);
            this.CurrencyGroupId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                DataAppHelpers.Locals.CURRENCYGROUP_ID);
            this.RealRateId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                DataAppHelpers.Locals.REAL_RATE_ID);
            this.NominalRateId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                DataAppHelpers.Locals.NOMINAL_RATE_ID);
            this.RatingGroupId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                   DataAppHelpers.Locals.RATINGGROUP_ID);
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.locals)
            {
                this.DataSourcePriceId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                    DataAppHelpers.Locals.DATASOURCEPRICE_ID);
                this.GeoCodePriceId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                    DataAppHelpers.Locals.GEOCODEPRICE_ID);
                this.DataSourceTechId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                    DataAppHelpers.Locals.DATASOURCETECH_ID);
                this.GeoCodeTechId = CalculatorHelpers.GetAttributeInt(elementWithLocals,
                    DataAppHelpers.Locals.GEOCODETECH_ID);
            }
            this.RealRate = CalculatorHelpers.GetAttributeDouble(elementWithLocals,
                DataAppHelpers.Locals.REAL_RATE);
            this.NominalRate = CalculatorHelpers.GetAttributeDouble(elementWithLocals,
                DataAppHelpers.Locals.NOMINAL_RATE);
            this.InflationRate = CalculatorHelpers.GetAttributeDouble(elementWithLocals,
                DataAppHelpers.Locals.INFLATION_RATE);
            this.XmlDocElement = elementWithLocals;
        }

        //copy constructor
        public Local(Local local)
        {
            this.Id = local.Id;
            this.CalculatorId = local.CalculatorId;
            this.UnitGroupId = local.UnitGroupId;
            this.CurrencyGroupId = local.CurrencyGroupId;
            this.RealRateId = local.RealRateId;
            this.NominalRateId = local.NominalRateId;
            this.RatingGroupId = local.RatingGroupId;
            this.DataSourcePriceId = local.DataSourcePriceId;
            this.GeoCodePriceId = local.GeoCodePriceId;
            this.DataSourceTechId = local.DataSourceTechId;
            this.GeoCodeTechId = local.GeoCodeTechId;
            this.RealRate = local.RealRate;
            this.NominalRate = local.NominalRate;
            this.InflationRate = local.InflationRate;
            if (local.XmlDocElement != null)
            {
                this.XmlDocElement = new XElement(local.XmlDocElement);
            }
        }
        public Local(CalculatorParameters calcParameters, Local local)
        {
            this.Id = local.Id;
            this.CalculatorId = local.CalculatorId;
            this.UnitGroupId = local.UnitGroupId;
            this.CurrencyGroupId = local.CurrencyGroupId;
            this.RealRateId = local.RealRateId;
            this.NominalRateId = local.NominalRateId;
            this.RatingGroupId = local.RatingGroupId;
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.locals)
            {
                this.DataSourcePriceId = local.DataSourcePriceId;
                this.GeoCodePriceId = local.GeoCodePriceId;
                this.DataSourceTechId = local.DataSourceTechId;
                this.GeoCodeTechId = local.GeoCodeTechId;
            }
            this.RealRate = local.RealRate;
            this.NominalRate = local.NominalRate;
            this.InflationRate = local.InflationRate;
            if (local.XmlDocElement != null)
            {
                this.XmlDocElement = new XElement(local.XmlDocElement);
            }
        }
        public void SetLocalAttributes(CalculatorParameters calcParameters,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            this.SetSharedObjectAttributes(string.Empty, ref currentElement);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.UNITGROUP_ID,
                this.UnitGroupId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.CURRENCYGROUP_ID,
                this.CurrencyGroupId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.REAL_RATE_ID,
                this.RealRateId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.NOMINAL_RATE_ID,
                this.NominalRateId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.RATINGGROUP_ID,
                this.RatingGroupId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, ref currentElement, updates);
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.locals)
            {
                //locals app uses xml name and description
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.LOCAL_NAME,
                   this.Name, RuleHelpers.GeneralRules.STRING,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.LOCAL_DESCRIPTION,
                   this.Description, RuleHelpers.GeneralRules.STRING,
                   calcParameters.StepNumber, ref currentElement, updates);
                //note that currentElement uses tech or price in attribute name
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.DATASOURCEPRICE_ID,
                   this.DataSourcePriceId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.GEOCODEPRICE_ID,
                    this.GeoCodePriceId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.DATASOURCETECH_ID,
                   this.DataSourceTechId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Locals.GEOCODETECH_ID,
                    this.GeoCodeTechId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                    calcParameters.StepNumber, ref currentElement, updates);
            }
            //rates don't get added to db, but they are needed for display
            CalculatorHelpers.SetAttributeDoubleF4(currentElement,
                DataAppHelpers.Locals.REAL_RATE, this.RealRate);
            CalculatorHelpers.SetAttributeDoubleF4(currentElement,
                DataAppHelpers.Locals.NOMINAL_RATE, this.NominalRate);
            CalculatorHelpers.SetAttributeDoubleF4(currentElement,
                DataAppHelpers.Locals.INFLATION_RATE, this.InflationRate);
        }
        public void SetLocalAttributesForCalculator(CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement)
        {
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.locals)
            {
                //serialize the calculator by first removing its attributes
                //to get rid of outdated ones and then serializing what's needed
                //to run the calculator and/or display the results
                if (currentCalculationsElement != null)
                {
                    this.SetAndRemoveCalculatorAttributes(string.Empty, ref currentCalculationsElement);
                }
            }
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               DataAppHelpers.Locals.UNITGROUP_ID, this.UnitGroupId);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               DataAppHelpers.Locals.CURRENCYGROUP_ID, this.CurrencyGroupId);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               DataAppHelpers.Locals.REAL_RATE_ID, this.RealRateId);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               DataAppHelpers.Locals.NOMINAL_RATE_ID, this.NominalRateId);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               DataAppHelpers.Locals.RATINGGROUP_ID, this.RatingGroupId);
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.locals)
            {
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                  DataAppHelpers.Locals.DATASOURCEPRICE_ID, this.DataSourcePriceId);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   DataAppHelpers.Locals.GEOCODEPRICE_ID, this.GeoCodePriceId);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   DataAppHelpers.Locals.DATASOURCETECH_ID, this.DataSourceTechId);
                CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                   DataAppHelpers.Locals.GEOCODETECH_ID, this.GeoCodeTechId);
            }
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                DataAppHelpers.Locals.REAL_RATE, this.RealRate);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                DataAppHelpers.Locals.NOMINAL_RATE, this.NominalRate);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                DataAppHelpers.Locals.INFLATION_RATE, this.InflationRate);
        }
        public void SetLocalAttributesForCalculator(string attNameExt, 
            CalculatorParameters calcParameters, ref XmlWriter writer)
        {
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.locals)
            {
                this.SetCalculatorAttributes(attNameExt, ref writer);
            }
            writer.WriteAttributeString(
                string.Concat(DataAppHelpers.Locals.UNITGROUP_ID, attNameExt), this.UnitGroupId.ToString());
            writer.WriteAttributeString(
               string.Concat(DataAppHelpers.Locals.CURRENCYGROUP_ID, attNameExt), this.CurrencyGroupId.ToString());
            writer.WriteAttributeString(
               string.Concat(DataAppHelpers.Locals.REAL_RATE_ID, attNameExt), this.RealRateId.ToString());
            writer.WriteAttributeString(
               string.Concat(DataAppHelpers.Locals.NOMINAL_RATE_ID, attNameExt), this.NominalRateId.ToString());
            writer.WriteAttributeString(
              string.Concat(DataAppHelpers.Locals.RATINGGROUP_ID, attNameExt), this.RatingGroupId.ToString());
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.locals)
            {
                writer.WriteAttributeString(
                    string.Concat(DataAppHelpers.Locals.DATASOURCEPRICE_ID, attNameExt), this.DataSourcePriceId.ToString());
                writer.WriteAttributeString(
                    string.Concat(DataAppHelpers.Locals.GEOCODEPRICE_ID, attNameExt), this.GeoCodePriceId.ToString());
                 writer.WriteAttributeString(
                    string.Concat(DataAppHelpers.Locals.DATASOURCETECH_ID, attNameExt), this.DataSourceTechId.ToString());
                writer.WriteAttributeString(
                    string.Concat(DataAppHelpers.Locals.GEOCODETECH_ID, attNameExt), this.GeoCodeTechId.ToString());
            }
            writer.WriteAttributeString(
               string.Concat(DataAppHelpers.Locals.REAL_RATE, attNameExt), this.RealRate.ToString("f3"));
            writer.WriteAttributeString(
               string.Concat(DataAppHelpers.Locals.NOMINAL_RATE, attNameExt), this.NominalRate.ToString("f3"));
             writer.WriteAttributeString(
               string.Concat(DataAppHelpers.Locals.INFLATION_RATE, attNameExt), this.InflationRate.ToString("f3"));
        }
        public bool ElementHasGoodLocals(XElement elementWithLocals)
        {
            bool bHasGoodLocals = false;
            double RealRate = CalculatorHelpers.GetAttributeDouble(elementWithLocals,
                DataAppHelpers.Locals.REAL_RATE);
            double NominalRate = CalculatorHelpers.GetAttributeDouble(elementWithLocals,
                DataAppHelpers.Locals.NOMINAL_RATE);
            if (RealRate != 0 && NominalRate != 0)
            {
                bHasGoodLocals = true;
            }
            return bHasGoodLocals;
        }
        public static IList<AccountToLocal> CopyLocals(IList<AccountToLocal> locals)
        {
            IList<AccountToLocal> copyLocals = new List<AccountToLocal>();
            if (locals != null)
            {
                foreach (AccountToLocal copyLocal in locals)
                {
                    copyLocals.Add(new AccountToLocal(copyLocal));
                }
            }
            return copyLocals;
        }
    }
}
