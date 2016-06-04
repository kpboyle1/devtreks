using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants used by all extensions
    ///             Support classes, such as DisplayHelpers.StylesheetHelper also use these
    ///Author:		www.devtreks.org
    ///Date:		2013, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        These are xml attribute names used by all apps
    public class General
    {
        public static XNamespace y0 = "https://www.devtreks.org";

        public const string NAME2 = "Name2";
        public const string LAST_CHANGED_cDate = "LastChangedDate";
        public const string UNIT = "Unit";
        public const string AMOUNT = "Amount";
        public const string PRICE = "Price";
        public const string INCENTIVE_RATE = "IncentiveRate";
        public const string INCENTIVE_AMOUNT = "IncentiveAmount";
        public const string DOC_STATUS = "DocStatus";
        public const string OLDID = "OldId";
        public const string ACCOUNTID = "AccountId";
        public const string LABEL3 = "Label";
        public const string FILE_NAME = "FileName";
        public const string RPATTNAME = "ResourcePackMetaDataXml";

        //this db field is used to determine an addin or extension's host
        public const string HOST_ATTNAME = "HostName";
        //this db field determines which addin or extension (hosted by host) to run
        public const string ADD_IN_ATTNAME = "AddInName";
        //db edits saved in temp xmldoc between calculator steps
        public const string UPDATE_NODE_LIST_NAME = "update";
        //request.qry.params
        public const string SAVE_METHOD = "savemethod";
        public const string DEFAULT_DEVDOCID = "defaultlinkedviewid";
        //analyzers find their corresponding calculated docs using this attribute 
        //(also used in ContentURI as the ContentURI.URIFileExtensionType)
        //public const string cFileExtensionType = "FileExtensionType";
        //public const string cFilesToAnalyzeExtensionType = "FilesToAnalyzeExtensionType";
        //db linkedview property name for db updates
        public const string LINKEDVIEW_FILE_EXTENSION_TYPE = "LinkedViewFileExtensionType";
        //used when making csv or tsv files (abbreviated filename needed because of 
        //potential to exceed windows filepath chars limit)
        public const string OBSERVATIONS_OB = "OB";
        public const string DATA = "data";
        //step initiation form elements
        public const string STEPZERO = "stepzero";
        public const string STEP = "step";
        public const string STEPLAST = "lastStepNumber";


    }
}
