namespace DevTreks.Data.RuleHelpers
{
    // <summary>
    ///Purpose:		Resource rule-enforcing class
    ///Author:		www.devtreks.org
    ///Date:		2010, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class ResourceRules
    {
        public static bool VerifyFileLength(ContentURI uri, int contentLength)
        {
            bool bIsValid = false;
            //1024 bytes = 1KB
            //HttpPostFile.ContentLength is bytes, convert to KB (the 1024 should not be 100)
            int iContentLengthKB = contentLength / 1024;
            var sFileSize = uri.URIDataManager.FileSizeValidation;
            if (!string.IsNullOrEmpty(sFileSize))
            {
                int iMaxFileSize = Helpers.GeneralHelpers.ConvertStringToInt(sFileSize) * 1024;
                if (iContentLengthKB <= iMaxFileSize) bIsValid = true;
            }
            return bIsValid;
        }
        public static bool VerifyFileLengthForDBStorage(ContentURI uri, int contentLength)
        {
            bool bIsValid = false;
            //1024 bytes = 1KB
            //HttpPostFile.ContentLength is bytes, convert to KB (the 1024 should not be 100)
            double dbContentLengthKB = contentLength / 1024;
            var sFileSize = uri.URIDataManager.FileSizeDBStorageValidation;
            if (!string.IsNullOrEmpty(sFileSize))
            {
                double dbMaxFileSize = Helpers.GeneralHelpers.ConvertStringToDouble(sFileSize) * 1024;
                if (dbContentLengthKB <= dbMaxFileSize) bIsValid = true;
            }
            return bIsValid;
        }
        public static void ValidateURIPatternScriptArgument(ref string uripattern)
        {
            //the names in uripatterns can cause js errors unless validated
            ContentURI cleanURI = new ContentURI();
            cleanURI.URIPattern = uripattern;
            Helpers.GeneralHelpers.SetURIParams(cleanURI);
            string sCleanName = cleanURI.URIName;
            ValidateScriptArgument(ref sCleanName);
            if (!string.IsNullOrEmpty(cleanURI.URIName))
            {
                uripattern = uripattern.Replace(cleanURI.URIName, sCleanName);
            }
        }
        //don't come in here directly, only come in from ValidateURIPatternScriptArgument
        //and only use this when the name is actually being used in a script argument
        //otherwise run into db filepath name vs. scriptargument name incompatibilities
        public static void ValidateScriptArgument(ref string attValue)
        {
            if (attValue == Helpers.GeneralHelpers.NONE
                || string.IsNullOrEmpty(attValue))
                return;
            //javascript argument exceptions
            attValue = attValue.Replace("@", "-");
            attValue = attValue.Replace("/", "-");
            attValue = attValue.Replace(@"\", "-");
            attValue = attValue.Replace("'", "-");
            attValue = attValue.Replace(GeneralRules.DOUBLEQUOTE, "-");
            attValue = attValue.Replace("(", "-");
            attValue = attValue.Replace(")", "-");
            attValue = attValue.Replace("{", "-");
            attValue = attValue.Replace("}", "-");
            attValue = attValue.Replace("[", "-");
            attValue = attValue.Replace("]", "-");
            attValue = attValue.Replace("|", "-");
            attValue = attValue.Replace("=", "-");
            attValue = attValue.Replace("+", "-");
            attValue = attValue.Replace("^", "-");
        }
    }
}
