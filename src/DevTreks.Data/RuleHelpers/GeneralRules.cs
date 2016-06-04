using System;
using System.Text.RegularExpressions;

namespace DevTreks.Data.RuleHelpers
{
    // <summary>
    ///Purpose:		General rule-enforcing class
    ///Author:		www.devtreks.org
    ///Date:		2015, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class GeneralRules
    {
        public const string DOUBLEQUOTE = "\"";
        public const string NONE = "none";
        public const string LONG = "long";
        public const string BOOLEAN = "boolean";
        public const string BINARY = "binary";
        public const string STRING = "string";
        public const string DATE = "date";
        public const string SHORTDATE = "shortdate";
        //these are the three main types used for calculations and analyses
        //two decimal places
        public const string DECIMAL = "decimal";
        public const string DOUBLE2 = "double2";
        //three decimal places
        public const string DOUBLE = "double";
        //equivalent to real in sql server
        public const string FLOAT = "float";
        //four decimal places
        public const string DOUBLE4 = "double4";
        public const string INTEGER = "integer";
        public const string SHORTINTEGER = "shortinteger";
        public const string XML = "xml";
        public const double DOUBLE_YEAR = 365.25;
        //sizes
        public const string NAME_SIZE = "75";
        public const string LABEL_SIZE = "25";
        public const string DESC_SIZE = "255";
        public const string NUMBER_SIZE = "8";
        //special names
        public const string NAME = "Name";
        public const string MATHEXPRESS = "mathexpression";
        public const string JOINTDATA = "jointdataurl";
        public enum XSD_DATA_TYPES
        {
            none            = 0,
            //SqlServer will convert to: bigint
            xsd_long        = 1,
            //bit
            xsd_boolean     = 2,
            //char, nchar, text, nvarchar
            xsd_string      = 3,
            //datetime, smalldatetime
            xsd_datetime    = 4,
            //decimal, money
            xsd_decimal     = 5,
            //float
            xsd_double      = 6,
            //int
            xsd_int         = 7,
            //real
            xsd_float       = 8,
            //smallint
            xsd_short       = 9,
            //tinyint
            xsd_unsignedbyte = 10,
            //xml
            xsd_xml         = 11
        }
        public enum REGEX_PATTERN_TYPES
        {
            number              = 1,
            socialsecurity      = 2,
            phonenumber         = 3,
            postcode            = 4,
            email               = 5,
            url                 = 6,
            passwordsimple      = 7,
            passwordcomplex     = 8,
            filemaskscommon     = 9,
            creditcardmajor     = 10
        }
        
        public static void ValidateXSDInput(
            EditHelpers.EditHelper.ArgumentsEdits editArgs)
        {
            //no leading or trailing white spaces 
            if (editArgs.EditAttValue == null) editArgs.EditAttValue = string.Empty;
            string sAttValue = editArgs.EditAttValue.Trim();
            if (editArgs.EditDataType != string.Empty)
            {
                ValidateXSDInput(editArgs.EditAttName, ref sAttValue, 
                    editArgs.EditDataType, editArgs.EditSize);
                editArgs.EditAttValue = sAttValue;
            }
            else
            {
                editArgs.EditAttValue = sAttValue;
            }
            if (editArgs.EditAttValue == string.Empty)
            {
                editArgs.EditAttValue = "0";
            }
        }
  
        public static void ValidateXSDInput(
            string attName, ref string attValue, string dataType, 
            string size)
        {
            XSD_DATA_TYPES eDataType = GetXSDDataType(dataType.ToLower());
            //prevent empty data submissions
            if (attValue == string.Empty || attValue == null) attValue = "0";
            if (string.IsNullOrEmpty(size))
                size = NAME_SIZE;
            switch (eDataType)
            {
                case XSD_DATA_TYPES.xsd_long:
                    long lResult =  Helpers.GeneralHelpers.ConvertStringToLong(attValue);
                    attValue = lResult.ToString();
                    break;
                case XSD_DATA_TYPES.xsd_boolean:
                    //1==true and 0==false - only values that should be passed to db
                    if (attValue != "1" && attValue != "0")
                    {
                        attValue = "0";
                    }
                    break;
                case XSD_DATA_TYPES.xsd_string:
                    //these rules are ok for mathexpression
                    int iSize = (size == string.Empty || size == null)
                        ? 0 : Helpers.GeneralHelpers.ConvertStringToInt(size);
                    //don't allow database string size limitations to be exceeded
                    if (attValue.Length > iSize)
                    {
                        int iRemove = attValue.Length - iSize;
                        attValue = attValue.Remove(iSize, iRemove);
                    }
                    //validate names and labels
                    ValidateNameStrings(attName, ref attValue);
                    ValidateSpecialAttributes(attName, ref attValue);
                    break;
                case XSD_DATA_TYPES.xsd_datetime:
                    DateTime dtDataType = Helpers.GeneralHelpers.ConvertStringToDate(attValue);
                    attValue = dtDataType.ToString("yyyy-MM-dd");
                    break;
                case XSD_DATA_TYPES.xsd_decimal:
                    //check for illegal price chars
                    //refactor: needs regex that replaces nonnumeric except period
                    attValue = Replace("$", attValue);
                    attValue = Replace(",", attValue);
                    Decimal dDataType = Helpers.GeneralHelpers.ConvertStringToDecimal(attValue);
                    attValue = dDataType.ToString("N4");
                    break;
                case XSD_DATA_TYPES.xsd_double:
                    double dbDataType = Helpers.GeneralHelpers.ConvertStringToDouble(attValue);
                    //maximum reasonable precision for storage of db field data
                    attValue = dbDataType.ToString("N4");
                    break;
                case XSD_DATA_TYPES.xsd_int:
                    int iDataType = Helpers.GeneralHelpers.ConvertStringToInt(attValue);
                    attValue = iDataType.ToString();
                    break;
                case XSD_DATA_TYPES.xsd_float:
                    float fDataType = Helpers.GeneralHelpers.ConvertStringToFloat(attValue);
                    //maximum reasonable precision for storage of db field data
                    attValue = fDataType.ToString("N4");
                    break;
                case XSD_DATA_TYPES.xsd_short:
                    short shDataType = Helpers.GeneralHelpers.ConvertStringToShort(attValue);
                    attValue = shDataType.ToString();
                    break;
                case XSD_DATA_TYPES.xsd_unsignedbyte:
                    try
                    {
                        byte btDataType = Convert.ToByte(attValue);
                    }
                    catch (Exception x)
                    {
                        //won't convert correctly when adding to db; do the conversion manually
                        ConvertToXSDDataType(eDataType, ref attValue);
                    }
                    break;
                default:
                    break;
            }
        }
        private static XSD_DATA_TYPES GetXSDDataType(string dataType)
        {
            XSD_DATA_TYPES eDataType = XSD_DATA_TYPES.none;
            switch (dataType)
            {
                case "long":
                    eDataType = XSD_DATA_TYPES.xsd_long;
                    break;
                case "bool":
                    eDataType = XSD_DATA_TYPES.xsd_boolean;
                    break;
                case "boolean":
                    eDataType = XSD_DATA_TYPES.xsd_boolean;
                    break;
                case "string":
                    eDataType = XSD_DATA_TYPES.xsd_string;
                    break;
                case "datetime":
                    eDataType = XSD_DATA_TYPES.xsd_datetime;
                    break;
                case "date":
                    eDataType = XSD_DATA_TYPES.xsd_datetime;
                    break;
                case "decimal":
                    eDataType = XSD_DATA_TYPES.xsd_decimal;
                    break;
                case "money":
                    eDataType = XSD_DATA_TYPES.xsd_decimal;
                    break;
                case "double":
                    eDataType = XSD_DATA_TYPES.xsd_double;
                    break;
                case "int":
                    eDataType = XSD_DATA_TYPES.xsd_int;
                    break;
                case "integer":
                    eDataType = XSD_DATA_TYPES.xsd_int;
                    break;
                case "float":
                    eDataType = XSD_DATA_TYPES.xsd_float;
                    break;
                case "real":
                    eDataType = XSD_DATA_TYPES.xsd_float;
                    break;
                case "short":
                    eDataType = XSD_DATA_TYPES.xsd_short;
                    break;
                case "unsignedbyte":
                    eDataType = XSD_DATA_TYPES.xsd_unsignedbyte;
                    break;
                case "xml":
                    eDataType = XSD_DATA_TYPES.xsd_xml;
                    break;
                default:
                    break;
            }
            return eDataType;
        }
        private static void ConvertToXSDDataType(XSD_DATA_TYPES dataType, ref string attValue)
        {
            switch (dataType)
            {
                case XSD_DATA_TYPES.xsd_long:
                    attValue = "0";
                    break;
                case XSD_DATA_TYPES.xsd_boolean:
                    attValue = "0";
                    break;
                case XSD_DATA_TYPES.xsd_string:
                    attValue = "0";
                    break;
                case XSD_DATA_TYPES.xsd_datetime:
                    //try to see if it will convert to the corresponding schema datatype
                    DateTime dtToday = new DateTime();
                    dtToday = (DateTime)DateTime.Today;
                    attValue = dtToday.ToString("yyyy-MM-dd");
                    break;
                case XSD_DATA_TYPES.xsd_decimal:
                    attValue = "0";
                    Decimal dDataType = Convert.ToDecimal(attValue);
                    attValue = dDataType.ToString("f2");
                    break;
                case XSD_DATA_TYPES.xsd_double:
                    attValue = "0";
                    double dbDataType = Convert.ToDouble(attValue);
                    //maximum reasonable precision in this network
                    attValue = dbDataType.ToString("N4");
                    break;
                case XSD_DATA_TYPES.xsd_int:
                    attValue = "0";
                    break;
                case XSD_DATA_TYPES.xsd_float:
                    attValue = "0";
                    float fDataType = Convert.ToSingle(attValue);
                    //maximum reasonable precision in this network
                    attValue = fDataType.ToString("N4");
                    break;
                case XSD_DATA_TYPES.xsd_short:
                    attValue = "0";
                    break;
                case XSD_DATA_TYPES.xsd_unsignedbyte:
                    attValue = "0";
                    break;
                default:
                    break;
            }
        }
        private static void ValidateNameStrings(string attName, ref string attValue)
        {
            //don't do anything to mathexpressions
            if (attName.ToLower().Contains(MATHEXPRESS))
                return;
            //or to joint data props (r scripts_
            if (attName.ToLower().Contains(JOINTDATA))
                return;
            int iIndex = -1;
            bool bIsAddInOrExtensionName 
                = DevTreks.Data.Helpers.AddInHelper.IsAddInOrExtensionName(attName);
            iIndex = attName.ToLower().IndexOf(NAME.ToLower());
            if (iIndex != -1 && bIsAddInOrExtensionName == false)
            {
                //file naming mishaps (these are either DevTrek param delimiters 
                //or file path errors)
                if (attName.ToLower() != AppHelpers.General.FILE_NAME.ToLower())
                    attValue = attValue.Replace(".", "-");
                //devtreks string param conventions aren't allowed in names
                attValue = attValue.Replace("_", "-");
                attValue = attValue.Replace(";", "-");
                attValue = attValue.Replace("@", "-");
                attValue = attValue.Replace("/", "-");
                attValue = attValue.Replace(@"\", "-");
                attValue = attValue.Replace("?", "-");
                attValue = attValue.Replace(":", "-");
                attValue = attValue.Replace("=", "-");
                attValue = attValue.Replace("%", "-");
                //get rid of line breaks and tabs (replace them with spaces)
                attValue = Regex.Replace(attValue, "(\\n|\\r|\\t)", " ");
                //javacript incompatabilities
                ResourceRules.ValidateScriptArgument(ref attValue);
                //don't break apart filenames (or may end up with .x ml)
                iIndex = attName.ToLower().IndexOf(AppHelpers.General.FILE_NAME.ToLower());
                if (iIndex == -1)
                {
                    //tocs appear lousy with more than 23 consecutive chars
                    //this should be culture specific
                    MakeTocName(23, ref attValue);
                }
            }
            //xml mishaps
            RemoveXmlChars(attName, ref attValue);
        }
        
        /// <summary>
        /// Removes any chars that would violate a clean xml attribute value
        /// </summary>
        /// <param name="attName"></param>
        /// <param name="attValue"></param>
        private static void RemoveXmlChars(string attName, ref string attValue)
        {
            attValue = attValue.Replace("<", "-");
            attValue = attValue.Replace(">", "-");
            attValue = attValue.Replace("&", "-");
        }
        private static void MakeTocName(int charLength, ref string attValue)
        {
            int iNameLength = attValue.Length;
            int iStartingNameIndex = charLength - 23;
            int iIndex = 0;
            int iSubstringLength = 0;
            if (iNameLength > charLength
                && iStartingNameIndex >= 0)
            {
                iSubstringLength = ((iStartingNameIndex + 23) < iNameLength) ? 23 : (iNameLength - iStartingNameIndex);
                string sNamePart = attValue.Substring(iStartingNameIndex, iSubstringLength);
                iIndex = sNamePart.IndexOf(" ");
                if (iIndex == -1)
                {
                    //needs a space inserted into name
                    string sNewNamePart = sNamePart + " ";
                    attValue = attValue.Replace(sNamePart, sNewNamePart);
                }
                //check next 23 chars
                MakeTocName(charLength + 23, ref attValue);
            }
        }
        
        private static void ValidateSpecialAttributes(string attName, ref string attValue)
        {
            //keep for reference
            bool bIsType = false;
            switch (attName)
            {
                case "Email":
                    ValidateType(REGEX_PATTERN_TYPES.email, ref attValue, out bIsType);
                    break;
                default:
                    break;
            }
        }
        public static void ValidateType(REGEX_PATTERN_TYPES typeName, ref string inputValue, 
            out bool isType)
        {
            isType = true;
            string sNewValue = string.Empty;
            switch (typeName)
            {
                case REGEX_PATTERN_TYPES.number:
                    //easier: bool bIsInteger = int.TryParse(inputValue)


                    //get rid of likely chars that might be added such as dollar signs, commas
                    inputValue = Replace("$", inputValue);
                    inputValue = Replace(",", inputValue);
                    Regex regExpression = new Regex("(?<digit>[0-9])");
                    //if it doesn't match change it to match
                    Match oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "0";
                    }
                    break;
                case REGEX_PATTERN_TYPES.socialsecurity:
                    regExpression = new Regex(@"\d{3}-\d{2}-\d{4}");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "111-111-1111";
                    }
                    break;
                case REGEX_PATTERN_TYPES.phonenumber:
                    regExpression = new Regex(@"\d{3}-\d{3}-\d{4}");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "111-111-1111";
                    }
                    break;
                case REGEX_PATTERN_TYPES.postcode:
                    regExpression = new Regex(@"\d{5}(-\d{4})?");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "0";
                    }
                    break;
                case REGEX_PATTERN_TYPES.email:
                    regExpression = new Regex(@"[\w-]+@([\w-]+\.)+[\w-]+");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "WrongEmailAddress@public.org";
                    }
                    break;
                case REGEX_PATTERN_TYPES.url:
                    regExpression = new Regex(@"http://([\w-]\.)+[\w-](/[\w- ./?%=]*)?");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "0";
                    }
                    break;
                case REGEX_PATTERN_TYPES.passwordsimple:
                    regExpression = new Regex(@"^(?=.*\d).{4,8}$");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "0";
                    }
                    break;
                case REGEX_PATTERN_TYPES.passwordcomplex:
                    regExpression = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "0";
                    }
                    break;
                case REGEX_PATTERN_TYPES.filemaskscommon:
                    regExpression = new Regex(@"^(.+)\\(.+)\.(.+)");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "0";
                    }
                    break;
                case REGEX_PATTERN_TYPES.creditcardmajor:
                    regExpression = new Regex(@"\d{4}-?\d{4}-?\d{4}-?\d{4}");
                    //if it doesn't match change it to match
                    oMatch = regExpression.Match(inputValue);
                    if (oMatch.Success == false)
                    {
                        isType = false;
                        inputValue = "0";
                    }
                    break;
                default:
                    break;
            }
        }
        public static string Replace(string toReplace, string inputValue)
        {
            string sNewValue = inputValue.Replace(toReplace, "");
            return sNewValue;
        }
        public static bool ValidateIsNumber(string possibleNumber)
        {
            bool bIsNumber = false;
            double dbResult = 0;
            //integers, decimals and doubles are the 3 number types used in linkedviews
            //they can all be cast to double
            bIsNumber = double.TryParse(possibleNumber, out dbResult);
            return bIsNumber;
        }
        //use ValidateNonNumber first; retain the regex for future use
        private static bool ValidateNumber(string test)
        {
            bool bIsNumber = false;
            if (!string.IsNullOrEmpty(test))
            {
                bool bIsNotNumber = TestNonNumber(test);
                if (bIsNotNumber == false)
                {
                    bIsNumber = true;
                }
            }
            return bIsNumber;
        }
        private static bool TestNonNumber(string sNumber)
        {
            bool bIsNotNumber = true;
            //has a non-number? (except for decimal, negative sign, and comma)
            string sCleanNumber = sNumber;
            sCleanNumber = Replace(",", sNumber);
            sCleanNumber = Replace(".", sCleanNumber);
            sCleanNumber = Replace("-", sCleanNumber);
            string sExp = @"(\D)";
            Regex regExpression = new Regex(sExp);
            bIsNotNumber = regExpression.IsMatch(sCleanNumber);
            return bIsNotNumber;
        }
    }
}
