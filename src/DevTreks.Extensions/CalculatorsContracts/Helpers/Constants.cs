using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevTreksHelpers = DevTreks.Data.Helpers;
using DevTreksAppHelpers = DevTreks.Data.AppHelpers;
using DevTreksEditHelpers = DevTreks.Data.EditHelpers;
using DevTreksRulesHelpers = DevTreks.Data.RuleHelpers;

/// <summary>
///Purpose:		Constants used by extensions
///Author:		www.devtreks.org
///Date:		2015, September
///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
///NOTES:
///             1. Useful constants used by extensions
/// </summary>
namespace DevTreks.Extensions
{
    public static class Constants
    {
        #region "constants"

        public const string NAME2 = DevTreksAppHelpers.General.NAME2;
        public const string UNIT = DevTreksAppHelpers.General.UNIT;
        public const string AMOUNT = DevTreksAppHelpers.General.AMOUNT;
        public const string PRICE = DevTreksAppHelpers.General.PRICE;
        public const string INCENTIVE_RATE = DevTreksAppHelpers.General.INCENTIVE_RATE;
        public const string INCENTIVE_AMOUNT = DevTreksAppHelpers.General.INCENTIVE_AMOUNT;
        public const string DOC_STATUS = DevTreksAppHelpers.General.DOC_STATUS;
        public const string MACHINERY_CONSTANTS_ID = "MachConstantId";
        public const string PRICE_CONSTANTS_ID = "PriceConstantId";
        public const string RANDM_ID = "RandMConstantId";
        public const string OLDID = DevTreksAppHelpers.General.OLDID;
        public const string NONE = DevTreksHelpers.GeneralHelpers.NONE;
        public const string EACH = DevTreksHelpers.GeneralHelpers.EACH;
        public const string ROOT_PATH = DevTreksHelpers.GeneralHelpers.ROOT_PATH;
        public const string LONG = DevTreksRulesHelpers.GeneralRules.LONG;
        public const string BOOLEAN = DevTreksRulesHelpers.GeneralRules.BOOLEAN;
        public const string STRING = DevTreksRulesHelpers.GeneralRules.STRING;
        public const string DECIMAL = DevTreksRulesHelpers.GeneralRules.DECIMAL;
        //these are the three main types used for calculations and analyses
        //two decimal places
        public const string DOUBLE2 = DevTreksRulesHelpers.GeneralRules.DOUBLE2;
        public const string DOUBLE = DevTreksRulesHelpers.GeneralRules.DOUBLE;
        //three decimal places
        public const string DOUBLE4 = DevTreksRulesHelpers.GeneralRules.DOUBLE4;
        //four decimal places
        public const string INTEGER = DevTreksRulesHelpers.GeneralRules.INTEGER;
        public const string XML = DevTreksRulesHelpers.GeneralRules.XML;
        public const string EXTENSION_XML = DevTreksHelpers.GeneralHelpers.EXTENSION_XML;
        public const string EXTENSION_CSV = DevTreksHelpers.GeneralHelpers.EXTENSION_CSV;
        public const string OBSERVATIONS = DevTreksAppHelpers.General.OBSERVATIONS_OB;
        public const string STRING_DELIMITER = DevTreksHelpers.GeneralHelpers.STRING_DELIMITER;
        public static string ANALYZER_HOSTNAME = DevTreksHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString();
        //this needs an enum here: public const string ANALYZERPROOF = DevTreksHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString();

        public static char[] FILENAME_DELIMITERS
        {
            get
            {
                return DevTreksHelpers.GeneralHelpers.FILENAME_DELIMITERS;
            }
        }
        public const string FILENAME_DELIMITER = DevTreksHelpers.GeneralHelpers.FILENAME_DELIMITER;
        public static char[] STRING_DELIMITERS
        {
            get
            {
                return DevTreksHelpers.GeneralHelpers.STRING_DELIMITERS;
            }
        }
        public const string CSV_DELIMITER = DevTreksHelpers.GeneralHelpers.CSV_DELIMITER;
        public static char[] CSV_DELIMITERS
        {
            get
            {
                return DevTreksHelpers.GeneralHelpers.CSV_DELIMITERS;
            }
        }
        public const string SPACE_DELIMITER = DevTreksHelpers.GeneralHelpers.SPACE_DELIMITER;
        public static char[] SPACE_DELIMITERS
        {
            get
            {
                return DevTreksHelpers.GeneralHelpers.SPACE_DELIMITERS;
            }
        }
        public const string FILEEXTENSION_DELIMITER = DevTreksHelpers.GeneralHelpers.FILEEXTENSION_DELIMITER;
        public static char[] FILEEXTENSION_DELIMITERS
        {
            get
            {
                //period separator
                return DevTreksHelpers.GeneralHelpers.FILEEXTENSION_DELIMITERS;
            }
        }
        
        #endregion
        #region "node names"
        public enum LINKEDVIEWS_TYPES
        {
            servicebase         = 0,
            linkedviewtype      = 1,
            linkedviewgroup     = 2,
            linkedviewpack      = 3,
            linkedview          = 4,
            linkedviewresourcepack = 5
        }
        public enum DEVPACKS_TYPES
        {
            servicebase     = 0,
            devpacktype     = 1,
            devpackgroup    = 2,
            devpack         = 3,
            devpackpart     = 4,
            devpackresourcepack = 5
        }
        public enum SUBAPPLICATION_TYPES
        {
            none            = 0,
            clubs           = 1,
            members         = 2,
            agreements      = 3,
            addins          = 4,
            networks        = 5,
            locals          = 6,
            inputprices     = 100,
            outputprices    = 200,
            outcomeprices   = 300,
            operationprices = 400,
            componentprices = 500,
            budgets         = 600,
            investments     = 700,
            devpacks        = 800,
            linkedviews     = 900,
            resources       = 1000
        }
        public enum LOCALCONSTANTS_TYPES
        {
            none            = 0,
            nominalrate     = 1,
            realrate        = 2,
            unitgroup       = 3,
            currencygroup   = 4,
            geocode         = 5,
            datasource      = 6,
            geocodetech     = 7,
            datasourcetech  = 8,
            geocodeprice    = 9,
            datasourceprice = 10,
            ratinggroup     = 11
        }
        public enum SAVECALCS_METHOD
        {
            none = 0,
            calcs = 1,
            analyses = 2,
            saveastext = 3
        }
        #endregion
        

        
    }
}
