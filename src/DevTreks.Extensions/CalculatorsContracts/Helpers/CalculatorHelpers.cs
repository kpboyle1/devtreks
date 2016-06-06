using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using Errors = DevTreks.Exceptions.DevTreksErrors;
using DevTreksHelpers = DevTreks.Data.Helpers;
using DevTreksAppHelpers = DevTreks.Data.AppHelpers;
using DevTreksEditHelpers = DevTreks.Data.EditHelpers;
using DevTreksRulesHelpers = DevTreks.Data.RuleHelpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Helper utilities used by the DoStepsAddInView's extension calculators.
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:
    ///             1. This class is an extension's only communication link 
    ///             to the utilities available in DevTreks.Data. 
    ///             Extensions do not directly reference Devtreks, instead 
    ///             they go through this module for utilities found in DevTreks.
    /// </summary>
    public class CalculatorHelpers
    {
        public enum RUN_CALCULATOR_TYPES
        {
            none            = 0,
            //run basic calculation
            basic           = 1,
            //run io calculations for io docs
            io              = 2,
            //run io calculations for technology docs
            iotechnology    = 3,
            //run calcdocuri calculations (i.e. locals)
            calcdocuri      = 4,
            //run analyzers
            analyzer        = 5,
            //run analyzers using full object collections (1.3.6)
            analyzeobjects  = 6
        }
        //base npv calculators
        public enum CALCULATOR_TYPES
        {
            none        = 0,
            operation   = 1,
            component   = 2,
            budget      = 3,
            investment  = 4,
            input       = 5,
            output      = 6,
            locals      = 7,
            operation2  = 8,
            component2  = 9,
            outcome     = 10,
            npv         = 11
        }
        public static CALCULATOR_TYPES GetCalculatorType(string calcType)
        {
            CALCULATOR_TYPES eCalculatorType = CALCULATOR_TYPES.none;
            if (calcType == CALCULATOR_TYPES.input.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.input;
            }
            else if (calcType == CALCULATOR_TYPES.output.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.output;
            }
            else if (calcType == CALCULATOR_TYPES.operation.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.operation;
            }
            else if (calcType == CALCULATOR_TYPES.operation2.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.operation2;
            }
            else if (calcType == CALCULATOR_TYPES.component.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.component;
            }
            else if (calcType == CALCULATOR_TYPES.component2.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.component2;
            }
            else if (calcType == CALCULATOR_TYPES.budget.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.budget;
            }
            else if (calcType == CALCULATOR_TYPES.investment.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.investment;
            }
            else if (calcType == CALCULATOR_TYPES.locals.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.locals;
            }
            else if (calcType == CALCULATOR_TYPES.outcome.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.outcome;
            }
            else if (calcType == CALCULATOR_TYPES.npv.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.npv;
            }
            return eCalculatorType;
        }
        public enum PLATFORM_TYPES
        {
            webserver = 0,
            azure = 1,
            amazon = 2
        }
        public enum NORMALIZATION_TYPES
        {
            none = 0,
            zscore = 1,
            minmax = 2,
            logistic = 3,
            logit = 4,
            pnorm = 5,
            tanh = 6
        }
        #region "set linked lists state"
        public static bool SetLinkedLocalsListsState(CalculatorParameters calcParameters)
        {
            bool bIsDone = false;
            //step two passes in constants and other lists of data
            if (calcParameters.LinkedViewElement != null)
            {
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.datasource,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.geocode,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.realrate,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.nominalrate,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.unitgroup,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.currencygroup,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.ratinggroup,
                    calcParameters);
                //locals have greater detail than most calculators
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.datasourcetech,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.geocodetech,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.datasourceprice,
                    calcParameters);
                AddLocalConstants(Constants.LOCALCONSTANTS_TYPES.geocodeprice,
                    calcParameters);
                //save the tempdoc with changes and bIsDone = true
                bIsDone = SaveNewCalculationsDocument(calcParameters);
            }
            else
            {
                calcParameters.ErrorMessage 
                    = Errors.MakeStandardErrorMsg("CALCSHELPER_FILE_NOTFOUND");
            }
            return bIsDone;
        }
        public static bool SaveNewCalculationsDocument(CalculatorParameters calcParameters)
        {
            bool bHasSaved = false;
            XElement linkedViewsDocument = LoadXElement(
                calcParameters.ExtensionCalcDocURI,
                calcParameters.ExtensionCalcDocURI.URIDataManager.TempDocPath);
            bool bHasReplacedCalcDoc
                = CalculatorHelpers.ReplaceElementInDocument(
                calcParameters.LinkedViewElement, ref linkedViewsDocument);
            if (bHasReplacedCalcDoc)
            {
                //save tempdocs with changes and bIsDone = true
                string sErrorMsg = string.Empty;
                CalculatorHelpers.SaveXmlInURI(
                    calcParameters.ExtensionCalcDocURI,
                    linkedViewsDocument.CreateReader(),
                   calcParameters.ExtensionCalcDocURI.URIDataManager.TempDocPath, out sErrorMsg);
                calcParameters.ErrorMessage = sErrorMsg;
                bHasSaved = true;
            }
            else
            {
                calcParameters.ErrorMessage 
                    = Errors.MakeStandardErrorMsg("CALCULATORS_ID_MISMATCH");
            }
            return bHasSaved;
        }
        public static string GetMessage(string resourceName)
        {
            string sMessage = string.Empty;
            sMessage = DevTreks.Exceptions.DevTreksErrors.GetMessage(resourceName);
            return sMessage;
        }
        public static void AddLocalConstants(Constants.LOCALCONSTANTS_TYPES constantType,
            CalculatorParameters calcParameters)
        {
            string sConstantsIdAttName = string.Empty;
            string sConstantsFullDocPath = string.Empty;
            string sConstantsId = string.Empty;
            string sConstantsNodeQry = string.Empty;
            switch (constantType)
            {
                case Constants.LOCALCONSTANTS_TYPES.realrate:
                    sConstantsIdAttName = Local.REAL_RATE_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.nominalrate:
                    sConstantsIdAttName = Local.NOMINAL_RATE_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.unitgroup:
                    sConstantsIdAttName = Local.UNITGROUP_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.currencygroup:
                    sConstantsIdAttName = Local.CURRENCYGROUP_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.geocode:
                    sConstantsIdAttName = Local.GEOCODE_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.geocodetech:
                    sConstantsIdAttName = Local.GEOCODETECH_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.geocodeprice:
                    sConstantsIdAttName = Local.GEOCODEPRICE_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.ratinggroup:
                    sConstantsIdAttName = Local.RATINGGROUP_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.datasource:
                    sConstantsIdAttName = Local.DATASOURCE_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.datasourcetech:
                    sConstantsIdAttName = Local.DATASOURCETECH_ID;
                    break;
                case Constants.LOCALCONSTANTS_TYPES.datasourceprice:
                    sConstantsIdAttName = Local.DATASOURCEPRICE_ID;
                    break;
                default:
                    break;
            }
            sConstantsFullDocPath = GetLinkedListPath(
                calcParameters, constantType.ToString());
            //use the calculator node constant id fields to find the correct node
            sConstantsId
                = Data.EditHelpers.XmlLinq.GetElementAttributeValue(
                    calcParameters.LinkedViewElement, sConstantsIdAttName);
            if (!string.IsNullOrEmpty(sConstantsId))
            {
                string sErrorMsg = string.Empty;
                GetResourceURIPath(calcParameters.ExtensionDocToCalcURI,
                    sConstantsFullDocPath, sErrorMsg);
                calcParameters.ErrorMessage = sErrorMsg;
                if (string.IsNullOrEmpty(sErrorMsg))
                {
                    XElement rootConstants = LoadXElement(
                        calcParameters.ExtensionCalcDocURI,
                        sConstantsFullDocPath);
                    if (rootConstants != null)
                    {
                        //add the remaining constants attributes
                        Data.EditHelpers.XmlLinq.AddAttributesWithoutIdNameDesc(
                            constantType.ToString(), sConstantsId,
                            rootConstants, calcParameters.LinkedViewElement);
                    }
                }
            }
        }
        public static string GetLinkedListPath(
            CalculatorParameters calcParameters, string constantType)
        {
            string sFullLinkedListPath = string.Empty;
            string sResourceArray = Data.AppHelpers.Resources.GetResourceArrayFromResourceArraysByTagName(
                calcParameters.ExtensionCalcDocURI.URIDataManager.LinkedLists, constantType);
            if (!string.IsNullOrEmpty(sResourceArray))
            {
                string sAltDesc = string.Empty;
                string sResourceURIPattern = string.Empty;
                Data.AppHelpers.Resources.GetResourceIdsForResourceFilePaths(sResourceArray, 0,
                    out sFullLinkedListPath, out sAltDesc, out sResourceURIPattern);
            }
            return sFullLinkedListPath;
        }
        public static CalculatorParameters SetCalculatorParameters(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze, 
            IDictionary<string, string> updates)
        {
            //set the state needed to run calculations
            XElement linkedViewsDocument = null;
            XElement linkedViewsElement = null;
            bool bHasState = SetCalculatorsState(
                extDocToCalcURI, extCalcDocURI,
                ref linkedViewsDocument, ref linkedViewsElement);
            CalculatorParameters eCalculatorParameters
               = new CalculatorParameters(extDocToCalcURI, extCalcDocURI,
               stepNumber, urisToAnalyze, updates, linkedViewsElement);
            if (eCalculatorParameters.CalculatorType == Constants.NONE)
            {
                extDocToCalcURI.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_MISSING");
                return null;
            }
            return eCalculatorParameters;
        }
        public static bool AddAttributesWithoutIdNameDesc(string constantType, 
            string nodeId, XElement fromRoot, XElement toElement)
        {
            bool bHasAddedAtts = false;
            Data.EditHelpers.XmlLinq.AddAttributesWithoutIdNameDesc(
                constantType.ToString(), nodeId,
                fromRoot, toElement);
            return bHasAddedAtts;
        }
        //set basic calculator state
        public static bool SetCalculatorsState(ExtensionContentURI extDocToCalcURI,
            ExtensionContentURI extCalcDocURI,
            ref XElement linkedViewsDocument, ref XElement linkedViewsElement)
        {
            bool bHasState = false;
            //set calcdoc
            if (URIAbsoluteExists(
                extCalcDocURI, extCalcDocURI.URIDataManager.TempDocPath))
            {
                //load the calculator doc 
                linkedViewsDocument = LoadXElement(
                    extCalcDocURI, extCalcDocURI.URIDataManager.TempDocPath);
                linkedViewsElement =
                    Data.EditHelpers.XmlLinq.GetDescendantUsingURIPattern(
                        linkedViewsDocument, extCalcDocURI.URIPattern);
                if (linkedViewsElement == null)
                {
                    if (extCalcDocURI.URIDataManager.AppType
                        == DevTreksHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews.ToString())
                    {
                        linkedViewsElement = GetElement(linkedViewsDocument,
                            DevTreksAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(), 
                            "1");
                    }
                }
                if (linkedViewsElement != null)
                {
                    bHasState = true;
                }
            }
            else
            {
                extDocToCalcURI.ErrorMessage = Errors.MakeStandardErrorMsg("ADDINHELPER_NOCALCULATOR");
            }
            return bHasState;
        }
        public static bool URIAbsoluteExists(ExtensionContentURI uri, 
            string fullFilePath)
        {
            bool bURIExists = false;
            bURIExists = DevTreksHelpers.FileStorageIO.URIAbsoluteExists(
                uri.URIDataManager.InitialDocToCalcURI, fullFilePath);
            return bURIExists;
        }
        //the collection index is appended to attribute name:
        public static string GetColIndexAttrExtension(int indexPosition)
        {
            return string.Concat(Constants.FILENAME_DELIMITER, indexPosition.ToString());
        }
        public static string GetResourceURIPath(ExtensionContentURI uri,
            string existingURIPath, string errorMsg)
        {
            string sURIPath = string.Empty;
            sURIPath = DevTreksHelpers.FileStorageIO.GetResourceURIPath(
                uri.URIDataManager.InitialDocToCalcURI, 
                existingURIPath, ref errorMsg);
            return sURIPath;
        }
        public static string GetTempDocsPath(ExtensionContentURI extURI,
            bool isLocalCacheDirectory, string uriPattern,
            string tempURIPattern)
        {
            string sURIPath = string.Empty;
            DevTreks.Data.ContentURI uri = new Data.ContentURI(uriPattern);
            sURIPath = DevTreksHelpers.AppSettings.GetTempDocPath(
                uri, isLocalCacheDirectory, 
                uriPattern, tempURIPattern);
            DevTreksHelpers.FileStorageIO.DirectoryCreate(
                extURI.URIDataManager.InitialDocToCalcURI, sURIPath);
            return sURIPath;
        }
        public static string GetAppSettingString(ExtensionContentURI extURI, string appSettingName)
        {
            string sAppSettingValue = string.Empty;
            sAppSettingValue = DevTreksHelpers.AppSettings.GetAppSettingString(
                extURI.URIDataManager.InitialDocToCalcURI, appSettingName);
            return sAppSettingValue;
        }
        public static string GetTempDocsPath(ExtensionContentURI extURI, bool isLocalCacheDirectory, 
            string fileName)
        {
            string sTempDocPath = string.Empty;
            string sDirectoryPath = DevTreksHelpers.AppSettings.GetTempWebDirectory(extURI.URIDataManager.InitialDocToCalcURI,
                isLocalCacheDirectory, DevTreksHelpers.GeneralHelpers.Get2RandomInteger());
            string sDelimiter = DevTreksHelpers.FileStorageIO.GetDelimiterForFileStorage(sDirectoryPath);
            //make the tempdocpath
            sTempDocPath = string.Concat(sDirectoryPath, sDelimiter, fileName);
            //return sTempDocPath;
            bool bHasDirectory = DevTreksHelpers.FileStorageIO.DirectoryCreate(
                extURI.URIDataManager.InitialDocToCalcURI, sTempDocPath);
            return sTempDocPath;
        }
        public static string ConvertFullURIToFilePath(ExtensionContentURI extURI, string url)
        {
            string sFilePath = string.Empty;
            sFilePath = DevTreksHelpers.AppSettings.ConvertPathFileandWeb(
                extURI.URIDataManager.InitialDocToCalcURI, url);
            return sFilePath;
        }
        public static string GetTempContainerPath(string fileName)
        {
            //web service calls to azure web services expect containername/filename
            string sTempCPath = string.Empty;
            string sDelimiter = DevTreksHelpers.GeneralHelpers.WEBFILE_PATH_DELIMITER;
            //make the tempdocpath
            sTempCPath = string.Concat("temp", sDelimiter,
                DevTreksHelpers.GeneralHelpers.Get2RandomInteger(), sDelimiter, fileName);
            return sTempCPath;
        }
        public static string GetContainerPathFromFullURIPath(string containerName, string fullURI)
        {
            //web service calls to azure web services expect containername/filename
            int iIndex = fullURI.IndexOf(string.Concat(containerName, DevTreksHelpers.GeneralHelpers.WEBFILE_PATH_DELIMITER));
            string sTempCPath = string.Empty;
            if (iIndex >= 0)
            {
                sTempCPath = fullURI.Substring(iIndex);
            }

            return sTempCPath;
        }
        public static bool SaveXmlInURI(
            ExtensionContentURI extURI, XmlReader reader,
            string fullURIPath, out string errorMsg)
        {
            bool bFileHasSaved = false;
            errorMsg = string.Empty;
            DevTreksHelpers.FileStorageIO fileStorageIO = new DevTreksHelpers.FileStorageIO();
            bFileHasSaved = fileStorageIO.SaveXmlInURI(
                extURI.URIDataManager.InitialDocToCalcURI, 
                reader, fullURIPath, out errorMsg);
            return bFileHasSaved;
        }
        public static bool SaveTextInURI(
            ExtensionContentURI extURI, string text,
            string fullURIPath, out string errorMsg)
        {
            bool bFileHasSaved = false;
            errorMsg = string.Empty;
            DevTreksHelpers.FileStorageIO fileStorageIO = new DevTreksHelpers.FileStorageIO();
            bFileHasSaved = fileStorageIO.SaveTextURI(extURI.URIDataManager.InitialDocToCalcURI,
                fullURIPath, text, out errorMsg);
            return bFileHasSaved;
        }
        public static bool SaveTextInURI(DevTreks.Data.ContentURI uri, string text,
           string fullURIPath, out string errorMsg)
        {
            bool bFileHasSaved = false;
            errorMsg = string.Empty;
            DevTreksHelpers.FileStorageIO fileStorageIO = new DevTreksHelpers.FileStorageIO();
            bFileHasSaved = fileStorageIO.SaveTextURI(uri, fullURIPath, text, out errorMsg);
            return bFileHasSaved;
        }
        #endregion

        #region "set calculator state"

        //set basic calculator state
        public static bool SetCalculatorsState(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            ref XElement linkedViewsDocument,
            ref XElement linkedViewsElement, ref string tempCalcPath,
            ref string tempDocToCalcPath, ref string docToCalcURIPattern,
            ref string docToCalcNodeName)
        {
            bool bHasState = false;
            //set calcdoc state
            SetCalculatorsState(extDocToCalcURI, extCalcDocURI,
                ref linkedViewsDocument, ref linkedViewsElement);
            tempCalcPath = extCalcDocURI.URIDataManager.TempDocPath;
            tempDocToCalcPath = extDocToCalcURI.URIDataManager.TempDocPath;
            docToCalcURIPattern = extDocToCalcURI.URIPattern;
            docToCalcNodeName = DevTreks.Data.ContentURI.GetURIPatternPart(docToCalcURIPattern,
                Data.ContentURI.URIPATTERNPART.node);
            if (linkedViewsElement != null)
            {
                bHasState = true;
            }
            return bHasState;
        }
        public static void RefreshUpdatesUsingDocToCalc(CalculatorParameters calcParams,
            string stepNumber, IDictionary<string, string> updates)
        {
            string sDoctoCalcPath = GetFullCalculatorResultsPath(
                calcParams);
            //0.8.7 moved this to CalculatorHelpers.GetFullCalculatorResultsPath for consistency
            //CopyFiles(sDoctoCalcPath, 
            //    calcParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            RefreshUpdates(calcParams, stepNumber, updates);
        }
        public static void RefreshUpdates(CalculatorParameters calcParams, 
            string stepNumber, IDictionary<string, string> updates)
        {
            //the doctocalc must be refreshed when edits are removed
            bool bNeedsNewReplacementDocToCalc = true;
            if (updates != null)
            {
                if (updates.Count > 0)
                {
                    //get rid of any updates member that came from running
                    //the same calculator step more than once
                    //the updates key contains a delimited string
                    //containing the step number for the update member
                    //get rid of any member with a step number == this step number
                    //use a list so that loop can be used
                    List<string> removalKeys = new List<string>();
                    foreach (KeyValuePair<string, string> kvp in updates)
                    {
                        //suffices for current calculators and analyzers
                        if (kvp.Key.EndsWith(stepNumber))
                        {
                            removalKeys.Add(kvp.Key);
                        }
                    }
                    if (removalKeys.Count > 0)
                    {
                        //needs restoration, not storage
                        bNeedsNewReplacementDocToCalc = false;
                        foreach (string key in removalKeys)
                        {
                            updates.Remove(key);
                        }
                        //restore doctocalc to nonedited state
                        ReStoreDocToCalcReplacementFile(calcParams);
                    }
                }
            }
            if (bNeedsNewReplacementDocToCalc)
            {
                //store a second copy of doctocalc in case the edits
                //have to be removed after the same step is run more than 1x
                StoreDocToCalcReplacementFile(calcParams);
            }
        }
        public static void StoreDocToCalcReplacementFile(
            CalculatorParameters calcParams)
        {
            //if xmlnodes are inserted for the first time, they'll
            //interfere when the same step is run 2x
            string sNewFilePath = GetReplacementDocToCalcPath(calcParams);
            CopyFiles(calcParams.ExtensionDocToCalcURI,
                calcParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath,
                sNewFilePath);
        }
        public static void ReStoreDocToCalcReplacementFile(
            CalculatorParameters calcParams)
        {
            string sNewFilePath = GetReplacementDocToCalcPath(calcParams);
            CopyFiles(calcParams.ExtensionDocToCalcURI, sNewFilePath, 
                calcParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
        }
        private static string GetReplacementDocToCalcPath(CalculatorParameters calcParams)
        {
            string sNewFilePath = string.Empty;
            string sOldFileName = Path.GetFileNameWithoutExtension(calcParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            sNewFilePath = calcParams.ExtensionDocToCalcURI.URIDataManager
                .TempDocPath.Replace(sOldFileName, Calculator1.cDocToCalcReplacementFile);
            return sNewFilePath;
        }
        public static void SetCalculatorId(XElement currentLinkedViewElement,
            XElement currentElement)
        {
            //resource stock analysis finds the stock data by inserting a 
            //calculatorid attribute in the parent element 
            if (currentElement != null
                && currentLinkedViewElement != null)
            {
                //all stylesheets use the Id attribute, not CALCULATOR_ID
                string sCalculatorId = GetAttribute(currentLinkedViewElement,
                    Calculator1.cId);
                //string sCalculatorId = GetAttribute(currentLinkedViewElement,
                //    Calculator1.cCalculatorId);
                if (!string.IsNullOrEmpty(sCalculatorId))
                {
                    if (sCalculatorId != "0")
                    {
                        SetAttribute(currentElement, Calculator1.cCalculatorId, 
                            sCalculatorId);
                    }
                }
            }
        }
        public static void GetPowerCalculatorInputVars(CalculatorParameters calcParameters, 
            XElement currentElement, ref DateTime inputDate, out double inputOCAmount)
        {
            inputOCAmount = 0;
            XElement rootInput = calcParameters.GetInputForOpOrCompNode(currentElement);
            if (rootInput != null)
            {
                if (rootInput.HasElements)
                {
                    double dbOCAmount = 0;
                    bool bNeedsBreak = false;
                    foreach (XElement input in rootInput.Elements())
                    {
                        inputDate = GetAttributeDate(input, Input.INPUT_DATE);
                        IEnumerable<XElement> linkedViews = GetChildrenLinkedView(input);
                        if (linkedViews != null)
                        {
                            foreach (XElement linkedview in linkedViews)
                            {
                                decimal dFuelCost = GetAttributeDecimal(linkedview, ConstantsUseful.cFuelCost);
                                if (dFuelCost > 0)
                                {
                                    //parent input amount is correct
                                    dbOCAmount = GetAttributeDouble(input, Input.OC_AMOUNT);
                                    if (dbOCAmount > 0)
                                    {
                                        inputOCAmount = dbOCAmount;
                                        bNeedsBreak = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (bNeedsBreak)
                        {
                            break;
                        }
                    }
                }
            }
        }
        public static XElement GetCalculator(CalculatorParameters calculatorParams,
            XElement currentElement)
        {
            XElement linkedViewsElement = null;
            bool bIsNodeStartingCalculations = false;
            bool bIsNewCalculator = false;
            bIsNodeStartingCalculations = IsNodeStartingCalculations(calculatorParams,
                currentElement);
            calculatorParams.NeedsXmlDocOnly = true;
            bool bIsSameSubApp
                = IsSameSubApp(calculatorParams, currentElement.Name.LocalName);
            //v1.3.0 started to run calcs on ancestors because collections must be Group based
            //but this is ok not to manipulate ancestor nodes
            //calculators can only be inserted into self or children of self
            bool bIsSelfOrChildNode
                = IsSelfOrChildNode(calculatorParams, currentElement.Name.LocalName);
            if (bIsNodeStartingCalculations
                && calculatorParams.LinkedViewElement != null)
            {
                //rule 1
                linkedViewsElement 
                    = new XElement(calculatorParams.LinkedViewElement);
                bIsNewCalculator = true;
            }
            else
            {
                if (calculatorParams.NeedsCalculators
                    && bIsSameSubApp
                    && bIsSelfOrChildNode)
                {
                    //rule 2: try to get an existing calculator using sCalcType, 
                    //sRelatedCalcType, sWhatIfScenario, or bHasNewEdits attribute
                    linkedViewsElement = GetAllyCalculator(calculatorParams,
                        currentElement, calculatorParams.CalculatorType);
                    if (linkedViewsElement == null)
                    {
                        linkedViewsElement = GetNewCalculator(calculatorParams,
                            currentElement);
                        if (linkedViewsElement != null)
                        {
                            bIsNewCalculator = true;
                        }
                    }
                }
                else
                {
                    //rule 5: try to get a calculator using sWhatIfScenario, 
                    //sRelatedCalcType, or bHasNewEdits attribute
                    linkedViewsElement = GetAllyCalculator(calculatorParams,
                        currentElement);
                }
            }
            if (calculatorParams.NeedsCalculators
                && bIsSameSubApp
                && bIsSelfOrChildNode
                && bIsNewCalculator == false)
            {
                //updates children linked views 
                linkedViewsElement = UpdateExistingCalculator(calculatorParams, 
                    linkedViewsElement);
            }
            //ok to return null calculations element (not all calculations need
            //a calculator to run, some can set parent (i.e. input/output) properties 
            //without a calculator)
            return linkedViewsElement;
        }
        public static XElement GetNewCalculator(CalculatorParameters calculatorParams,
           XElement currentElement)
        {
            XElement linkedViewsElement = null;
            if (calculatorParams.IsCustomDoc == false
                && calculatorParams.LinkedViewElement != null)
            {
                //rule 3: 0.8.8: don't insert new calulators when the whatifs, relatedcalctype 
                //are all string.empty or none (danger of too many inadvertent insertions, especially large datasets)
                if (NeedsNewAllyCalculator(calculatorParams))
                {
                    //rule 4: insert a new calculator
                    linkedViewsElement = new XElement(calculatorParams.LinkedViewElement);
                    //bIsNewCalculator = true;
                    //tell SetXmlDocIds to insert a new xmldoc in descendant
                    calculatorParams.NeedsXmlDocOnly = false;
                }
                else
                {
                    //0.8.8: don't allow any changes in descendents when a linkedview can't be found, 
                    //otherwise they'll end up with incorrect attributes and calculations (i.e. 0)
                    calculatorParams.NeedsCalculators = false;
                }
            }
            else
            {
                //0.8.8: ditto
                calculatorParams.NeedsCalculators = false;
            }
            return linkedViewsElement;
        }
        private static bool NeedsNewAllyCalculator(CalculatorParameters calculatorParams)
        {
            bool bNeedsNewAllyCalculator = false;
            //don't automatically insert allied (children) calculators unless they have signalled 
            //they want a new one by having changed one of these properties
            //lessens the danger of inserting a large number of unwanted calculators in descendants
            if ((calculatorParams.WhatIfScenario != string.Empty && calculatorParams.WhatIfScenario != Constants.NONE)
               || (calculatorParams.RelatedCalculatorType != string.Empty && calculatorParams.RelatedCalculatorType != Constants.NONE)
               || (calculatorParams.RelatedCalculatorsType != string.Empty && calculatorParams.RelatedCalculatorsType != Constants.NONE))
            {
                bNeedsNewAllyCalculator = true;
            }
            return bNeedsNewAllyCalculator;
        }
        public static void GetDevPackCalculator(CalculatorParameters subscriberCalcParams,
            CalculatorParameters publisherCalcParams, 
            out XElement currentDevPackElement, out XElement currentLinkedViewElement)
        {
            currentDevPackElement = null;
            currentLinkedViewElement = null;
            //gets a calculator from a customdoc
            //(if the calculator exists, children don't need a new linked view, 
            //if it doesn't exist, children linked views can be inserted)
            //subscriberParams.extensiondoctocalcuri.uripattern combined with
            //subscriberParams.club.docpath can be used to retrieve the full
            //devpack document holding all current linked views for all 
            //nodes currently being processed

            string sTempMiscDoc = GetMiscDocURI(subscriberCalcParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (URIAbsoluteExists(subscriberCalcParams.ExtensionDocToCalcURI,
                sTempMiscDoc))
            {
                //get current devpack element, with children linkedviews
                currentDevPackElement = DevTreksEditHelpers.XmlLinq
                    .GetCurrentElementWithLinkedView(
                    publisherCalcParams.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI, 
                    sTempMiscDoc, publisherCalcParams.ExtensionDocToCalcURI.URIPattern);
                if (currentDevPackElement != null)
                {
                    currentLinkedViewElement = GetCalculator(publisherCalcParams,
                        currentDevPackElement);
                    if (currentLinkedViewElement == null)
                    {
                        currentLinkedViewElement = new XElement(
                            publisherCalcParams.LinkedViewElement);
                        publisherCalcParams.NeedsXmlDocOnly = false;
                    }
                    else
                    {
                        publisherCalcParams.NeedsXmlDocOnly = true;
                    }
                }
            }
        }
        public static string GetMiscDocURI(string existingDocURI)
        {
            string sMiscDocURI = DevTreksHelpers.AppSettings.GetMiscDocURI(existingDocURI);
            return sMiscDocURI;
        }
        private static XElement UpdateExistingCalculator(CalculatorParameters calcParameters,
            XElement linkedViewsElement)
        {
            XElement updatedCalcs = linkedViewsElement;
            if (linkedViewsElement != null
                && calcParameters.LinkedViewElement != null)
            {
                //update the existing calculator, linkedViewsElement, with attributes 
                //from the parent linked view
                string sToId = GetAttribute(linkedViewsElement, Calculator1.cId);
                if (sToId != "0" && (!string.IsNullOrEmpty(sToId)))
                {
                    //must return a new Xelement (no byref or updates won't update)
                    updatedCalcs = new XElement(linkedViewsElement);
                    //use the same method as when extensions update old calculators
                    //i.e. don't change parameters that have been fine-tuned
                    bool bHasUpdates = DevTreksEditHelpers.XmlLinq.AddLinkedViewAttributesThatAreMissing(
                        calcParameters.LinkedViewElement, updatedCalcs);
                    if (!bHasUpdates)
                    {
                        updatedCalcs = new XElement(calcParameters.LinkedViewElement);
                    }
                    //doesn't hurt to double check
                    SetAttribute(updatedCalcs, Calculator1.cId, sToId);
                    SetAttribute(updatedCalcs, Calculator1.cCalculatorId, sToId);
                }
            }
            return updatedCalcs;
        }
        public static IDictionary<string, string> GetDevPackState(
            CalculatorParameters calcParameters)
        {
            IDictionary<string, string> fileOrFolderPaths = null;
            string sErrorMsg = string.Empty;
            fileOrFolderPaths = GetDevPackState(calcParameters.ExtensionDocToCalcURI,
                calcParameters.ExtensionCalcDocURI, calcParameters.LinkedViewElement,
                calcParameters.UrisToAnalyze, ref sErrorMsg);
            calcParameters.ExtensionDocToCalcURI.ErrorMessage += sErrorMsg;
            calcParameters.ExtensionCalcDocURI.ErrorMessage += sErrorMsg;
            return fileOrFolderPaths;
        }
        public static IDictionary<string, string> GetDevPackState(
            ExtensionContentURI docToCalcURI, ExtensionContentURI calcDocURI,
            XElement linkedViewsElement, IList<string> urisToAnalyze,
            ref string errorMsg)
        {
            IDictionary<string, string> fileOrFolderPaths = null;
            //calculations can be run on current devpack's children devpackparts
            //(doctocalc points to folder holding subfolder files to calculate)
            int i = 0;
            //the devpack parts need to exist (have been clicked on and 'selected' file extension type added to filename)
            string sFileExtensionType
                = DevTreks.Data.Helpers.GeneralHelpers.FILENAME_EXTENSIONS.selected.ToString();
            string sCalculatedFileExtensionType
                = DevTreksEditHelpers.XmlLinq.GetAttributeValue(linkedViewsElement,
                    Calculator1.cFileExtensionType);
            if (string.IsNullOrEmpty(sCalculatedFileExtensionType))
            {
                sCalculatedFileExtensionType 
                    = GetURIPatternFileExtensionType(calcDocURI.URIPattern);
            }
            //can't figure out which calc to use without fileexttype
            if (!string.IsNullOrEmpty(sCalculatedFileExtensionType))
            {
                fileOrFolderPaths = new Dictionary<string, string>();
                //calculate all terminal node files devpackparts (custom docs)
                GetTerminalNodeFiles(docToCalcURI, calcDocURI,
                    urisToAnalyze, docToCalcURI.URIClub.ClubDocFullPath,
                    sFileExtensionType, ref i, ref fileOrFolderPaths);
                if (fileOrFolderPaths.Count <= 0)
                    errorMsg = Errors.MakeStandardErrorMsg("ADDINHELPER_NODEVPACKPARTS");
            }
            else
            {
                errorMsg = Errors.MakeStandardErrorMsg("ADDINHELPER_NOFILEEXTENSIONTYPE");
            }
            if (string.IsNullOrEmpty(errorMsg))
            {
                //verify that no files are missing from the analysis
                VerifyFilesInAnalysis(urisToAnalyze,
                     fileOrFolderPaths, errorMsg);
            }
            return fileOrFolderPaths;
        }
        public static void GetTerminalNodeFiles(
            ExtensionContentURI docToCalcURI, ExtensionContentURI calcDocURI,
            IList<string> urisToAnalyze, string docToCalcPath, string fileExtension,
            ref int i, ref IDictionary<string, string> fileOrFolderPaths)
        {
            DevTreksHelpers.FileStorageIO.PLATFORM_TYPES platform 
                = DevTreksHelpers.FileStorageIO.GetPlatformType(docToCalcPath);
            if (platform == DevTreksHelpers.FileStorageIO.PLATFORM_TYPES.webserver)
            {
                GetTerminalNodeWebServerFiles(docToCalcURI, calcDocURI,
                    urisToAnalyze, docToCalcPath, fileExtension,
                    ref i, ref fileOrFolderPaths);
            }
            else if (platform == DevTreksHelpers.FileStorageIO.PLATFORM_TYPES.azure)
            {
                DevTreksHelpers.AzureIOAsync azureIO = new DevTreksHelpers.AzureIOAsync(
                    docToCalcURI.URIDataManager.InitialDocToCalcURI);
                azureIO.GetTerminalNodeAzureFiles(
                    docToCalcURI.URIDataManager.InitialDocToCalcURI, urisToAnalyze,
                    docToCalcURI.URIPattern, docToCalcPath, fileExtension,
                    ref i, ref fileOrFolderPaths);
            }
        }
        
        public static void GetTerminalNodeWebServerFiles(
            ExtensionContentURI docToCalcURI, ExtensionContentURI calcDocURI,
            IList<string> urisToAnalyze, string docToCalcPath, string fileExtension,
            ref int i, ref IDictionary<string, string> fileOrFolderPaths)
        {
            DirectoryInfo dir = null;
            if (Path.HasExtension(docToCalcPath))
            {
                //if it has an extension it needs the parent directory
                dir = new DirectoryInfo(
                    Path.GetDirectoryName(docToCalcPath));
            }
            else
            {
                dir = new DirectoryInfo(docToCalcPath);
            }
            if (DevTreksHelpers.GeneralHelpers.IsTerminalFolder(dir.Name))
            {
                //verify this folder exists in db
                if (urisToAnalyze.Contains(dir.Name))
                {
                    //188 doesn't require new calcs for inputs or outputs
                    if (fileExtension.StartsWith(Input.INPUT_PRICE_TYPES.input.ToString())
                        || fileExtension.StartsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        DevTreks.Data.Helpers.XmlFileIO.AddNewestIOFile(
                            docToCalcURI.URIDataManager.InitialDocToCalcURI,
                            dir, ref i, ref fileOrFolderPaths);
                    }
                    else
                    {
                        int z = fileOrFolderPaths.Count;
                        DevTreks.Data.Helpers.XmlFileIO.AddNewestFileWithFileExtension(
                            docToCalcURI.URIDataManager.InitialDocToCalcURI,
                            dir, fileExtension, ref i, ref fileOrFolderPaths);
                        if (z == fileOrFolderPaths.Count)
                        {
                            //188 allows default uploaded files to be used directly w/o running NPV calculation
                            DevTreks.Data.Helpers.XmlFileIO.AddNewestFileWithFileExtension(
                                docToCalcURI.URIDataManager.InitialDocToCalcURI, dir,
                                DevTreks.Data.Helpers.GeneralHelpers.FILENAME_EXTENSIONS.selected.ToString(),
                                ref i, ref fileOrFolderPaths);
                        }
                    }
                }
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo folder in dirs)
                {
                    if (DevTreksHelpers.GeneralHelpers.IsTerminalFolder(folder.Name))
                    {
                        //recursion will catch the files in this folder
                        //devpacks are recursive
                        if (folder.Name.StartsWith(
                            Constants.DEVPACKS_TYPES.devpack.ToString()))
                        {
                            //recurse
                            GetTerminalNodeFiles(docToCalcURI, calcDocURI, urisToAnalyze,
                                folder.FullName, fileExtension,
                                ref i, ref fileOrFolderPaths);
                        }
                    }
                }
            }
            else
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo folder in dirs)
                {
                    if (DevTreksHelpers.GeneralHelpers.IsTerminalFolder(folder.Name))
                    {
                        string sNodeName
                            = GetURIPatternNodeName(docToCalcURI.URIPattern);
                        if (!sNodeName.StartsWith(Constants.DEVPACKS_TYPES.devpack.ToString()))
                        {
                            //verify this folder exists in db
                            if (urisToAnalyze.Contains(folder.Name))
                            {
                                //188 doesn't require new calcs for inputs or outputs
                                if (fileExtension.StartsWith(Input.INPUT_PRICE_TYPES.input.ToString())
                                    || fileExtension.StartsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                                {
                                    DevTreks.Data.Helpers.XmlFileIO.AddNewestIOFile(
                                        docToCalcURI.URIDataManager.InitialDocToCalcURI,
                                        folder, ref i, ref fileOrFolderPaths);
                                }
                                else
                                {
                                    int z = fileOrFolderPaths.Count;
                                    DevTreks.Data.Helpers.XmlFileIO.AddNewestFileWithFileExtension(
                                        docToCalcURI.URIDataManager.InitialDocToCalcURI,
                                        folder, fileExtension, ref i, ref fileOrFolderPaths);
                                    if (z == fileOrFolderPaths.Count)
                                    {
                                        //188 allows default uploaded files to be used directly w/o running NPV calculation
                                        DevTreks.Data.Helpers.XmlFileIO.AddNewestFileWithFileExtension(
                                            docToCalcURI.URIDataManager.InitialDocToCalcURI,
                                            folder, DevTreks.Data.Helpers.GeneralHelpers.FILENAME_EXTENSIONS.selected.ToString(),
                                            ref i, ref fileOrFolderPaths);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //devpacks are recursive
                            GetTerminalNodeWebServerFiles(docToCalcURI, calcDocURI, urisToAnalyze,
                                folder.FullName, fileExtension,
                                ref i, ref fileOrFolderPaths);
                        }
                    }
                    else
                    {
                        //recurse
                        GetTerminalNodeWebServerFiles(docToCalcURI, calcDocURI, urisToAnalyze,
                            folder.FullName, fileExtension,
                            ref i, ref fileOrFolderPaths);
                    }
                }
            }
        }
        public static bool IsTerminalNode(string nodeName)
        {
            bool bIsTerminalNode = false;
            bIsTerminalNode = DevTreksHelpers.GeneralHelpers
                .IsTerminalFolder(nodeName);
            return bIsTerminalNode;
        }

        //consider making these extensions to AnalyzerParameters 
        public static void SetFileOrFoldersToAnalyze(string fileExtension,
            CalculatorParameters calcParams, IList<string> urisToAnalyze)
        {
            //get the files needing analysis
            string sTempFileExtensionCheck
                = CalculatorHelpers.GetURIPatternFileExtensionType(
                calcParams.ExtensionDocToCalcURI.URIPattern);
            IDictionary<string, string> lstFileOrFolderPaths
                = new Dictionary<string, string>();
            //urisToAnalyze verifies that filesystem files match db descendants 
            if (sTempFileExtensionCheck != "temp")
            {
                if (calcParams.AnalyzerParms.SubFolderType
                    == AnalyzerHelper.SUBFOLDER_OPTIONS.yes)
                {
                    //analyze only analysis results found in the first subfolders
                    GetFirstSubFolderFiles(calcParams,
                        urisToAnalyze, fileExtension, ref lstFileOrFolderPaths);
                }
                else
                {
                    int i = 0;
                    //analyze all terminal node files (i.e. budgettimeperiods, investmenttimeperiods, 
                    //operations, components, inputseriess, outputseriess, devpackparts (custom docs)
                    GetTerminalNodeFiles(calcParams.ExtensionDocToCalcURI,
                        calcParams.ExtensionCalcDocURI, urisToAnalyze,
                        calcParams.ExtensionDocToCalcURI.URIClub.ClubDocFullPath,
                        fileExtension, ref i, ref lstFileOrFolderPaths);
                    ////0.9.0 simplification: use the base calculator doc -it contains all that needs to be analyzed
                    //GetTempDocFiles(urisToAnalyze,
                    //    ref lstFileOrFolderPaths);
                }
            }
            else
            {
                GetTempDocFiles(urisToAnalyze,
                    ref lstFileOrFolderPaths);
            }
            calcParams.AnalyzerParms.FileOrFolderPaths = lstFileOrFolderPaths;
            //verify file for devpacks
            if (calcParams.DocToCalcNodeName.StartsWith(
                Constants.DEVPACKS_TYPES.devpack.ToString()))
            {
                //verify that no custom files are missing from the analysis
                string sErrorMsg = string.Empty;
                CalculatorHelpers.VerifyFilesInAnalysis(
                    urisToAnalyze, calcParams.AnalyzerParms.FileOrFolderPaths, sErrorMsg);
                calcParams.ErrorMessage = sErrorMsg;
            }
        }
        public static void VerifyFilesInAnalysis(IList<string> urisToAnalyze,
            IDictionary<string, string> fileOrFolderPaths, string errorMsg)
        {
            //verify that fileOrFolderPaths match urisToAnalyze
            //i.e. if individual calculations were run (rather than group calculations)
            //one of the calculations could have been inadvertantly missed
            string sCurrentId = string.Empty;
            int iIndex = 0;
            int iCurrentId = 0;
            //foreach (string uriToAnalyze in urisToAnalyze.AsParallel())
            Parallel.ForEach(urisToAnalyze, (uriToAnalyze, loopState) =>
            {
                bool bHasURI = false;
                //verify that uriToAnalyze is a standard subfolder
                iIndex = 1 + uriToAnalyze.LastIndexOf(DevTreksHelpers.GeneralHelpers.FILENAME_DELIMITER);
                sCurrentId = uriToAnalyze.Substring(iIndex);
                if (!string.IsNullOrEmpty(sCurrentId))
                {
                    iCurrentId = DevTreksHelpers.GeneralHelpers.ConvertStringToInt(sCurrentId);
                    if (iCurrentId != 0)
                    {
                        string sFileToAnalyze = string.Empty;
                        foreach (KeyValuePair<string, string> kvp
                            in fileOrFolderPaths.AsParallel())
                        {
                            sFileToAnalyze = kvp.Value;
                            if (sFileToAnalyze.Contains(uriToAnalyze))
                            {
                                bHasURI = true;
                                loopState.Break();
                            }
                        }
                        if (bHasURI == false)
                        {
                            errorMsg = Errors.MakeStandardErrorMsg("ADDIN_MISSINGOBSERVATION");
                            loopState.Break();
                        }
                    }
                }
            });
        }
        public static void ValidateSerializationAttributes(ref XElement linkedViewsElement)
        {
            //an attribute with an empty value in linkedViewsElement can't be parsed 
            //(i.e. to double) during deserialization (only solution is to delete and start over)
            DevTreksHelpers.AddInHelperLinq.ValidateSerializationAttributes(
                ref linkedViewsElement);
        }
        public static string GetFullCalculatorResultsPath(CalculatorParameters calcParams)
        {
            string sFullCalculatorDocPath = string.Empty;
            if (calcParams.UrisToAnalyze != null)
            {
                if (IsTerminalNode(calcParams.ExtensionDocToCalcURI.URINodeName)
                    && calcParams.UrisToAnalyze.Count > 0)
                {
                    //first member of uristoanalyze is the full calculated results
                    sFullCalculatorDocPath = calcParams.UrisToAnalyze[0];
                }
                else
                {
                    if (calcParams.UrisToAnalyze.Count > 0)
                    {
                        //first member of uristoanalyze is the summary calculated results
                        string sSummaryDocPath = calcParams.UrisToAnalyze[0];
                        sFullCalculatorDocPath
                            = DevTreksHelpers.ContentHelper.GetFullCalculatorResultsPath(
                            sSummaryDocPath);
                        if (!URIAbsoluteExists(calcParams.ExtensionDocToCalcURI,
                            sFullCalculatorDocPath))
                        {
                            sFullCalculatorDocPath = sSummaryDocPath;
                        }
                    }
                }
            }
            //calcs always must be run from tempdoc paths
            CopyFiles(calcParams.ExtensionDocToCalcURI, sFullCalculatorDocPath,
                calcParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            sFullCalculatorDocPath = calcParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            return sFullCalculatorDocPath;
        }
        public static bool AddFilesToBaseDocument(CalculatorParameters calcs)
        {
            bool bHasObservation = false;
            //observations will be held in a new editable XElement
            XElement obs = new XElement(Constants.ROOT_PATH);
            string sId = string.Empty;
            string sFileToAnalyze = string.Empty;
            string sErrorMsg = string.Empty;
            if (calcs.AnalyzerParms.FileOrFolderPaths == null)
            {
                calcs.ErrorMessage = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            if (calcs.AnalyzerParms.FileOrFolderPaths.Count == 0)
            {
                calcs.ErrorMessage = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            Constants.SUBAPPLICATION_TYPES subapp = Constants.SUBAPPLICATION_TYPES.none;
            int i = 0;
            foreach (KeyValuePair<string, string> kvp in calcs.AnalyzerParms.FileOrFolderPaths)
            {
                sId = kvp.Key;
                sFileToAnalyze = kvp.Value;
                //stream and add the calculation (observation)
                //to the observationsRoot
                if (CalculatorHelpers.URIAbsoluteExists(calcs.ExtensionDocToCalcURI,
                    sFileToAnalyze))
                {
                    XmlReader xmlRdr = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                        calcs.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI, sFileToAnalyze);
                    if (xmlRdr != null)
                    {
                        using (xmlRdr)
                        {
                            xmlRdr.MoveToContent();
                            while (xmlRdr.Read())
                            {
                                if (xmlRdr.NodeType
                                    == XmlNodeType.Element)
                                {
                                    if (xmlRdr.LocalName == Constants.ROOT_PATH
                                        || xmlRdr.LocalName
                                            == DevTreksAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                                        || xmlRdr.LocalName == string.Empty)
                                    {
                                        //skip the node using the while statement
                                    }
                                    else
                                    {
                                        if (xmlRdr.LocalName.EndsWith("group"))
                                        {
                                            //set the subapptype using first file
                                            subapp = GetSubAppTypeFromNode(xmlRdr.LocalName);
                                            if (i == 0)
                                            {
                                                //subsequent analysis need to know what to analyze
                                                calcs.SubApplicationType = subapp;
                                            }
                                            //keep the data consistent
                                            if (calcs.SubApplicationType == subapp)
                                            {
                                                //add to the root
                                                obs.Add(XElement.Load(xmlRdr.ReadSubtree()));
                                                i++;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (calcs.ErrorMessage == string.Empty)
            {
                //save the observations file
                CalculatorHelpers.SaveXmlInURI(
                    calcs.ExtensionDocToCalcURI, obs.CreateReader(),
                   calcs.AnalyzerParms.ObservationsPath, out sErrorMsg);
                calcs.ErrorMessage = sErrorMsg;
                if (string.IsNullOrEmpty(sErrorMsg))
                {
                    bHasObservation = true;
                }
            }
            return bHasObservation;
        }
        public static bool ValidateIsNumber(string possibleNumber)
        {
            bool bIsNumber
                = DevTreksRulesHelpers.GeneralRules.ValidateIsNumber(possibleNumber);
            return bIsNumber;
        }
        public static XElement GetCurrentElementWithAttributes(XmlReader reader)
        {
            XElement currentElement = null;
            currentElement 
                = DevTreksEditHelpers.XmlLinq.GetCurrentElementWithAttributes(reader);
            return currentElement;
        }
        public static void AddLinkedViewsToCurrentElementWithReader(
            ExtensionContentURI extURI, string xmlDocPath, ref XElement currentElement)
        {
            DevTreksEditHelpers.XmlLinq
                .AddLinkedViewToCurrentElementWithReader(extURI.URIDataManager.InitialDocToCalcURI,
                xmlDocPath, ref currentElement);
        }
        public static Constants.SUBAPPLICATION_TYPES GetSubAppTypeFromNodeName2(
            string nodeName)
        {
            Constants.SUBAPPLICATION_TYPES eSubAppType = Constants.SUBAPPLICATION_TYPES.none;
            string subAppType = DevTreksHelpers.GeneralHelpers.GetSubAppTypeFromNodeName(nodeName);
            eSubAppType = ConvertSubAppType(subAppType);
            return eSubAppType;
        }
        public static void AddChildElementsToCurrentElement(XElement currentElement,
            XStreamingElement childElements)
        {
            currentElement.Add(childElements);
        }
        public static bool AppendElement(string parentElementURIPattern,
           string groupingElementName, XElement newElement, ref XElement root)
        {
            bool bIsAdded = DevTreksEditHelpers.XmlLinq.AppendElementUsingParentURIPattern(
                parentElementURIPattern, groupingElementName, newElement, ref root);
            return bIsAdded;
        }
       
        #endregion
        #region "set calculator updates state"
        public static void UpdateNewValue(bool needsDbUpdate,
            string uriPattern, string attName,
            string attValue, string dataType, 
            string stepNumber, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            DevTreksHelpers.AddInHelperLinq.UpdateNewValue(
                needsDbUpdate, uriPattern, attName, attValue, 
                dataType, stepNumber, ref currentElement, updates);
        }
        
        public static void AddXmlDocToUpdates(CalculatorParameters calcParameters,
            string attName, string attValue)
        {
            if (!string.IsNullOrEmpty(attValue))
            {
                //do not use the tempdoc -it doesn't have the latest changes made during analysis
                //XElement linkedViewsDocument = LoadXElement(
                //        calcParameters.ExtensionCalcDocURI.URIDataManager.TempDocPath);
                if (calcParameters.LinkedViewElement != null)
                {
                    CalculatorHelpers.SetAttribute(calcParameters.LinkedViewElement,
                                attName, attValue);
                    //add the calcsdocs collection of calculators and analyzers
                    AddXmlDocToUpdates(calcParameters.LinkedViewElement, calcParameters);
                }
            }
        }
        public static void AddXmlDocToUpdates(
            CalculatorParameters calcParameters)
        {
            string sErrorMsg = string.Empty;
            XElement linkedViewsDocument = LoadXElement(
                calcParameters.ExtensionCalcDocURI,
                calcParameters.ExtensionCalcDocURI.URIDataManager.TempDocPath);
            //add the calcsdocs collection of calculators and analyzers
            AddXmlDocToUpdates(linkedViewsDocument, calcParameters);
        }
        public static void AddXmlDocToUpdates(XElement linkedViewsDocument,
            CalculatorParameters calcParameters)
        {
            string sErrorMsg = string.Empty;
            //add the calcsdocs collection of calculators and analyzers
            XElement newCalcDoc = null;
            if (linkedViewsDocument.Name.LocalName
                == DevTreksAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                newCalcDoc = new XElement(DevTreksHelpers.GeneralHelpers.ROOT_PATH,
                            new XElement(linkedViewsDocument)
                    );
            }
            else if (linkedViewsDocument.Name.LocalName
                == DevTreksHelpers.GeneralHelpers.ROOT_PATH)
            {
                newCalcDoc = linkedViewsDocument;
            }
            string sXml = XElement.Parse(newCalcDoc.ToString(), LoadOptions.None).ToString();
            DevTreksHelpers.AddInHelper.AddToDbList(calcParameters.ExtensionDocToCalcURI.URIPattern,
                DevTreksHelpers.GeneralHelpers.ROOT_PATH,
                sXml, DevTreks.Data.RuleHelpers.GeneralRules.XML,
                calcParameters.StepNumber, calcParameters.Updates);
        }
        public static async Task<XmlReader> GetXmlReaderAsync(ExtensionContentURI extURI,
            string path)
        {
            XmlReader reader = await DevTreks.Data.Helpers.FileStorageIO.GetXmlReaderAsync(
                extURI.URIDataManager.InitialDocToCalcURI, path);
            return reader;
        }
        public static XElement LoadXElement(ExtensionContentURI extURI,
            string path)
        {
            XElement el = DevTreks.Data.Helpers.FileStorageIO.LoadXmlElement(
                extURI.URIDataManager.InitialDocToCalcURI, path);
            return el;
        }
        public static void AddXmlDocAndXmlDocIdsToUpdates(
            CalculatorParameters calcParameters)
        {
            string sErrorMsg = string.Empty;
            XElement linkedViewsDocument = LoadXElement(
                calcParameters.ExtensionCalcDocURI,
                calcParameters.ExtensionCalcDocURI.URIDataManager.TempDocPath);
            //analyzers usually use this pattern (no descendants insertions to deal with)
            //add the new LinkedViewPackId to the db update list
            DevTreksHelpers.AddInHelper.AddToDbList(calcParameters.ExtensionDocToCalcURI.URIPattern,
                DevTreksAppHelpers.LinkedViews.LINKEDVIEWBASEID,
                calcParameters.ExtensionCalcDocURI.URIDataManager.BaseId.ToString(), 
                DevTreks.Data.RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, calcParameters.Updates);
            //add the calcsdocs collection of calculators and analyzers
            XElement newCalcDoc = null;
            if (linkedViewsDocument.Name.LocalName
                == DevTreksAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                newCalcDoc = new XElement(DevTreksHelpers.GeneralHelpers.ROOT_PATH,
                            new XElement(linkedViewsDocument)
                    );
            }
            else if (linkedViewsDocument.Name.LocalName
                == DevTreksHelpers.GeneralHelpers.ROOT_PATH)
            {
                newCalcDoc = linkedViewsDocument;
            }
            string sXml = XElement.Parse(newCalcDoc.ToString(), LoadOptions.None).ToString();
            DevTreksHelpers.AddInHelper.AddToDbList(calcParameters.ExtensionDocToCalcURI.URIPattern,
                DevTreksHelpers.GeneralHelpers.ROOT_PATH,
                sXml, DevTreks.Data.RuleHelpers.GeneralRules.XML,
                calcParameters.StepNumber, calcParameters.Updates);
        }
        
        private static bool AnalysisNeedsDevPackPart(IList<string> urisToAnalyze)
        {
            bool bAnalysisNeedsDevPackPart = true;
            string sCurrentNodeName = string.Empty;
            int i = 0;
            foreach (string uriToAnalyze in urisToAnalyze)
            {
                //uriToAnalyze is a delimited nodeName_nodeId string
                sCurrentNodeName = DevTreksHelpers.GeneralHelpers.GetSubString(0, uriToAnalyze,
                    DevTreksHelpers.GeneralHelpers.FILENAME_DELIMITER);
                //if the list contains at least two devpacks, then it's a devpack-devpack parent-child update
                //don't update the devpackparts in the list as well (number could be 100s or 1000s, 
                //which, at this stage of developmen,t is too server intensive)
                if (sCurrentNodeName 
                    == DevTreksAppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                {
                    i++;
                    if (i == 2)
                    {
                        bAnalysisNeedsDevPackPart = false;
                        break;
                    }
                }
            }
            return bAnalysisNeedsDevPackPart;
        }
        //v180 pattern 
        public static void SetXmlDocUpdates(CalculatorParameters calculatorParams,
            XElement currentLinkedViewElement, XElement currentElement,
            IDictionary<string, string> updates)
        {
            if (currentLinkedViewElement != null)
            {
                string sErrorMessage = calculatorParams.ErrorMessage;
                string sDocToCalcURIPattern
                    = calculatorParams.CurrentElementURIPattern;
                bool bNeedsDBUpdate = NeedsUpdateList(calculatorParams);
                string sLinkedViewId = Data.EditHelpers.XmlLinq.GetAttributeValue(
                    currentLinkedViewElement, Calculator1.cId);
                
                ////v180: dbupdates for self and children only when Overwrite = true and UseSameCalc = true
                if (bNeedsDBUpdate)
                {
                    if ((calculatorParams.ExtensionDocToCalcURI.URIPattern
                        != sDocToCalcURIPattern)
                        || calculatorParams.IsCustomDoc)
                    {
                        //this will only insert linked views for descendant nodes; note this must come before
                        //SetXmlDocAttributes in order for descendant insertions to work reasonably
                        sErrorMessage = AddXmlDocIds(bNeedsDBUpdate, calculatorParams.ExtensionDocToCalcURI,
                            calculatorParams.ExtensionCalcDocURI,
                            currentLinkedViewElement, currentElement,
                            sDocToCalcURIPattern, calculatorParams.LinkedViewElement,
                            calculatorParams.StepNumber, sLinkedViewId,
                            updates);
                    }
                }
                //v180: no children xml doc changes when Overwrite = false and UseSameCalc = false
                bool bNeedsLVUpdate = NeedsLVUpdate(calculatorParams);
                if (bNeedsLVUpdate)
                {
                    //replace the linkedview (this is the only place where the linkedview is replaced)
                    sErrorMessage = SetXmlDocAttributes(bNeedsDBUpdate, calculatorParams.ExtensionDocToCalcURI,
                        calculatorParams.ExtensionCalcDocURI,
                        currentLinkedViewElement, currentElement,
                        sDocToCalcURIPattern, calculatorParams.LinkedViewElement,
                        calculatorParams.StepNumber, sLinkedViewId,
                        updates);
                }
                calculatorParams.ErrorMessage = sErrorMessage;
            }
        }
        public static void SetXmlDocUpdates(CalculatorParameters calculatorParams,
            XElement currentLinkedViewElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            if (currentLinkedViewElement != null)
            {
                string sErrorMessage = calculatorParams.ErrorMessage;
                string sDocToCalcURIPattern
                    = calculatorParams.CurrentElementURIPattern;
                bool bNeedsDBUpdate = NeedsUpdateList(calculatorParams);
                SetCustomDocXmlDocUpdates(calculatorParams,
                    ref bNeedsDBUpdate, ref sDocToCalcURIPattern);
                string sLinkedViewId = Data.EditHelpers.XmlLinq.GetAttributeValue(
                    currentLinkedViewElement, Calculator1.cId);
                if (calculatorParams.NeedsCalculators
                    || calculatorParams.ExtensionDocToCalcURI.URIPattern
                    == sDocToCalcURIPattern)
                {
                    //can be set for both starting doctocalc and descendants
                    //needing calculators
                    SetFileExtensionType(calculatorParams,
                        calculatorParams.FileExtensionType.ToString(),
                        ref currentLinkedViewElement);
                }
                ////.NeedsXmlDocOnly is set with .GetCalculator()
                if (!calculatorParams.NeedsXmlDocOnly)
                {
                    if ((calculatorParams.ExtensionDocToCalcURI.URIPattern
                        != sDocToCalcURIPattern)
                        || calculatorParams.IsCustomDoc)
                    {
                        //this will only insert linked views for descendant nodes; note this must come before
                        //SetXmlDocAttributes in order for descendant insertions to work reasonably
                        AddXmlDocIds(bNeedsDBUpdate, calculatorParams.ExtensionDocToCalcURI,
                            calculatorParams.ExtensionCalcDocURI,
                            currentLinkedViewElement, ref currentElement,
                            sDocToCalcURIPattern, calculatorParams.LinkedViewElement,
                            calculatorParams.StepNumber, sLinkedViewId,
                            updates, ref sErrorMessage);
                    }
                }
                //replace the linkedview (this is the only place where the linkedview is replaced)
                SetXmlDocAttributes(bNeedsDBUpdate, calculatorParams.ExtensionDocToCalcURI,
                    calculatorParams.ExtensionCalcDocURI,
                    currentLinkedViewElement, ref currentElement,
                    sDocToCalcURIPattern, calculatorParams.LinkedViewElement,
                    calculatorParams.StepNumber, sLinkedViewId,
                    updates, ref sErrorMessage);
                calculatorParams.ErrorMessage = sErrorMessage;
            }
        }
        
        public static bool NeedsUpdateAttribute(CalculatorParameters calculatorParams)
        {
            bool bNeedsUpdateAttribute = false;
            if (calculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                != Constants.SUBAPPLICATION_TYPES.devpacks.ToString())
            {
                bNeedsUpdateAttribute = NeedsUpdateList(calculatorParams);
            }
            return bNeedsUpdateAttribute;
        }
        //this is used to determine self/child dbupdates
        public static bool NeedsUpdateList(CalculatorParameters calculatorParams)
        {
            bool bNeedsUpdateList = false;
            if (calculatorParams.ExtensionDocToCalcURI.URIPattern
                == calculatorParams.CurrentElementURIPattern)
            {
                //self node
                bNeedsUpdateList = true;
            }
            else
            {
                //children nodes
                if (calculatorParams.NeedsCalculators)
                {
                    bool bIsSelfOrChildNode
                        = CalculatorHelpers.IsSelfOrChildNode(calculatorParams,
                        calculatorParams.CurrentElementNodeName);
                    if (bIsSelfOrChildNode)
                    {
                        //bNeedsUpdateList = true;
                        //v180
                        if (calculatorParams.Overwrite == true
                            && calculatorParams.UseSameCalculator == true)
                        {
                            //insert or update child in db (and parent)
                            bNeedsUpdateList = true;
                        }
                    }
                }
            }
            return bNeedsUpdateList;
        }
        //this is used for self/children linkedview updates
        public static bool NeedsLVUpdate(CalculatorParameters calculatorParams)
        {
            bool bNeedsLVUpdate = false;
            if (calculatorParams.ExtensionDocToCalcURI.URIPattern
                == calculatorParams.CurrentElementURIPattern)
            {
                //self node
                bNeedsLVUpdate = true;
            }
            else
            {
                //children nodes
                if (calculatorParams.NeedsCalculators)
                {
                    bool bIsSelfOrChildNode
                        = CalculatorHelpers.IsSelfOrChildNode(calculatorParams,
                        calculatorParams.CurrentElementNodeName);
                    if (bIsSelfOrChildNode)
                    {
                        if (calculatorParams.Overwrite == false
                            && calculatorParams.UseSameCalculator == false)
                        {
                            //one and only one condition for not updating children lv in xmldoc
                            bNeedsLVUpdate = false;
                        }
                        else
                        {
                            bNeedsLVUpdate = true;
                        }
                    }
                }
            }
            return bNeedsLVUpdate;
        }
        public static void SetCustomDocXmlDocUpdates(CalculatorParameters calculatorParams,
            ref bool needsUpdateList, ref string docToCalcURIPattern)
        {
            //devpack updates are handled in the CalcContracts.GetCalculator methods
            //they are never handled when the custom doc itself is being processed
            if (calculatorParams.IsCustomDoc)
            {
                string sCurrentNodeName = CalculatorHelpers.GetURIPatternNodeName(
                    docToCalcURIPattern);
                if (sCurrentNodeName
                    .StartsWith(Constants.DEVPACKS_TYPES.devpack.ToString()))
                {
                    //custom docs can manipulate descendant calculations if it finds any
                    //but can't insert anything into db except for descendant linked views
                    needsUpdateList = true;
                    //needs to insert a linkedview into a descendant
                    docToCalcURIPattern = calculatorParams.ExtensionDocToCalcURI.URIPattern;
                }
                else
                {
                    needsUpdateList = false;
                }
            }
        }
        
        public static bool ReplaceCalculations(CalculatorParameters calculatorParams,
            XElement currentElement, XElement currentLinkedViewElement)
        {
            bool bHasReplacedLinkedView = false;
            bool bIsNodeStartingCalculations
                = IsNodeStartingCalculations(calculatorParams,
                    currentElement);
            //the node inititating calculations is the node needing the new calculations
            if (bIsNodeStartingCalculations)
            {
                calculatorParams.LinkedViewElement = currentLinkedViewElement;
                bHasReplacedLinkedView = true;
            }
            return bHasReplacedLinkedView;
        }
        public static bool IsBenefitNode(string currentNodeName)
        {
            bool bIsBenefitNode = false;
            if (currentNodeName.Contains(Output.OUTPUT_PRICE_TYPES.output.ToString())
                || currentNodeName.Contains(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                bIsBenefitNode = true;
            }
            return bIsBenefitNode;
        }
        public static bool IsCostNode(string currentNodeName)
        {
            bool bIsCostNode = false;
            if (currentNodeName.Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                || currentNodeName.Contains(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentNodeName.Contains(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bIsCostNode = true;
            }
            return bIsCostNode;
        }
        public static double GetConfidenceIntervalProb(int percent)
        {
            //default ci is 90
            if (percent > 100 || percent <= 0) percent = 90;
            double dbCI = 1 - (CalculatorHelpers.ConvertStringToDouble(percent.ToString()) / 100);
            return Math.Round(dbCI, 4);
        }
        public static double GetConfidenceInterval(int percent, double n, double standdev)
        {
            double dbCI = 0;
            n = Math.Round(n, 0);
            standdev = Math.Round(standdev, 4);
            if (n == 0)
            {
                return dbCI;
            }
            //95% CI = Mean +-(1.96 * Standard Error (Standard deviation / N^0.5
            double dbZ = GetZ(percent);
            dbCI = dbZ * (standdev / (Math.Pow(n, 0.5)));
            double dbCIR = Math.Round(dbCI, 4);
            return dbCIR;
        }
        public static double GetSDFromConfidenceInterval(int percent, 
            double n, double ci)
        {
            //ci == QTM - QTL
            double dbSD = 0;
            n = Math.Round(n, 0);
            ci = Math.Round(ci, 4);
            if (n == 0)
            {
                return dbSD;
            }
            //95% CI = Mean +-(1.96 * Standard Error (Standard deviation / N^0.5
            double dbZ = GetZ(percent);
            dbSD = (ci / dbZ) * (Math.Pow(n, 0.5));
            double dbSDR = Math.Round(dbSD, 4);
            return dbSDR;
        }
        public static double GetZ(int percent)
        {
            double dbZ = 0;
            //don't use integers because they return wrong level of rounding
            double dbPCT = CalculatorHelpers.ConvertStringToDouble(percent.ToString());
            double dbArea = (dbPCT / 2) * .01;
            //round to the same level as the normal table
            double z = Math.Round(dbArea, 4);
            double lastI = 0;
            int row = 0;
            int col = 0;
            foreach (double i in ZTABLE)
            {
                //passing 10 columns next row will be col 11
                if (col == 11)
                {
                    //new row
                    row++;
                    //reset colIndex
                    col = 0;
                }
                if (row != 0 && col != 0)
                {
                    if (i != 0 && lastI != 0)
                    {
                        if (i == z
                            || (z > lastI && z <= i))
                        {
                            double z1 = ZTABLE[row, 0];
                            double z2 = ZTABLE[0, col];
                            dbZ = z1 + z2;
                            return dbZ;
                        }
                    }
                }
                col++;
                lastI = i;
            }
            return dbZ;
        }
        //[32 rows,10cols]
        public static double[,] ZTABLE = {
           {0,0,0.01,0.02,0.03,0.04,0.05,0.06,0.07,0.08,0.09},
            {0,0,0.00399,0.00798,0.01197,0.01595,0.01994,0.02392,0.0279,0.03188,0.03586},
            {0.1,0.0398,0.0438,0.04776,0.05172,0.05567,0.05966,0.0636,0.06749,0.07142,0.07535},
            {0.2,0.0793,0.08317,0.08706,0.09095,0.09483,0.09871,0.10257,0.10642,0.11026,0.11409},
            {0.3,0.11791,0.12172,0.12552,0.1293,0.13307,0.13683,0.14058,0.14431,0.14803,0.15173},
            {0.4,0.15542,0.1591,0.16276,0.1664,0.17003,0.17364,0.17724,0.18082,0.18439,0.18793},
            {0.5,0.19146,0.19497,0.19847,0.20194,0.2054,0.20884,0.21226,0.21566,0.21904,0.2224},
            {0.6,0.22575,0.22907,0.23237,0.23565,0.23891,0.24215,0.24537,0.24857,0.25175,0.2549},
            {0.7,0.25804,0.26115,0.26424,0.2673,0.27035,0.27337,0.27637,0.27935,0.2823,0.28524},
            {0.8,0.28814,0.29103,0.29389,0.29673,0.29955,0.30234,0.30511,0.30785,0.31057,0.31327},
            {0.9,0.31594,0.31859,0.32121,0.32381,0.32639,0.32894,0.33147,0.33398,0.33646,0.33891},
            {1,0.34134,0.34375,0.34614,0.34849,0.35083,0.35314,0.35543,0.35769,0.35993,0.36214},
            {1.1,0.36433,0.3665,0.36864,0.37076,0.37286,0.37493,0.37698,0.379,0.381,0.38298},
            {1.2,0.38493,0.38686,0.38877,0.39065,0.39251,0.39435,0.39617,0.39796,0.39973,0.40147},
            {1.3,0.4032,0.4049,0.40658,0.40824,0.40988,0.41149,0.41308,0.41466,0.41621,0.41774},
            {1.4,0.41924,0.42073,0.4222,0.42364,0.42507,0.42647,0.42785,0.42922,0.43056,0.43189},
            {1.5,0.43319,0.43448,0.43574,0.43699,0.43822,0.43943,0.44062,0.44179,0.44295,0.44408},
            {1.6,0.4452,0.4463,0.44738,0.44845,0.4495,0.45053,0.45154,0.45254,0.45352,0.45449},
            {1.7,0.45543,0.45637,0.45728,0.45818,0.45907,0.45994,0.4608,0.46164,0.46246,0.46327},
            {1.8,0.46407,0.46485,0.46562,0.46638,0.46712,0.46784,0.46856,0.46926,0.46995,0.47062},
            {1.9,0.47128,0.47193,0.47257,0.4732,0.47381,0.47441,0.475,0.47558,0.47615,0.4767},
            {2,0.47725,0.47778,0.47831,0.47882,0.47932,0.47982,0.4803,0.48077,0.48124,0.48169},
            {2.1,0.48214,0.48257,0.483,0.48341,0.48382,0.48422,0.48461,0.485,0.48537,0.48574},
            {2.2,0.4861,0.48645,0.48679,0.48713,0.48745,0.48778,0.48809,0.4884,0.4887,0.48899},
            {2.3,0.48928,0.48956,0.48983,0.4901,0.49036,0.49061,0.49086,0.49111,0.49134,0.49158},
            {2.4,0.4918,0.49202,0.49224,0.49245,0.49266,0.49286,0.49305,0.49324,0.49343,0.49361},
            {2.5,0.49379,0.49396,0.49413,0.4943,0.49446,0.49461,0.49477,0.49492,0.49506,0.4952},
            {2.6,0.49534,0.49547,0.4956,0.49573,0.49585,0.49598,0.49609,0.49621,0.49632,0.49643},
            {2.7,0.49653,0.49664,0.49674,0.49683,0.49693,0.49702,0.49711,0.4972,0.49728,0.49736},
            {2.8,0.49744,0.49752,0.4976,0.49767,0.49774,0.49781,0.49788,0.49795,0.49801,0.49807},
            {2.9,0.49813,0.49819,0.49825,0.49831,0.49836,0.49841,0.49846,0.49851,0.49856,0.49861},
            {3,0.49865,0.49869,0.49874,0.49878,0.49882,0.49886,0.49889,0.49893,0.49896,0.499}
        };
        //public static double Z(double x)
        //{
        //    //MathNet.Numerics.Distributions.Normal result = new MathNet.Numerics.Distributions.Normal();
        //    //return result.CumulativeDistribution(x);
        //}

        //public static double GetF(int percent)
        //{
        //    double dbF = 0;
        //    //don't use integers because they return wrong level of rounding
        //    double dbPCT = CalculatorHelpers.ConvertStringToDouble(percent.ToString());
        //    double dbArea = (dbPCT / 2) * .01;
        //    //round to the same level as the normal table
        //    double f = Math.Round(dbArea, 4);
        //    double lastI = 0;
        //    int row = 0;
        //    int col = 0;
        //    foreach (double i in FTABLE)
        //    {
        //        //passing 10 columns next row will be col 11
        //        if (col == 11)
        //        {
        //            //new row
        //            row++;
        //            //reset colIndex
        //            col = 0;
        //        }
        //        if (row != 0 && col != 0)
        //        {
        //            if (i != 0 && lastI != 0)
        //            {
        //                if (i == f
        //                    || (f > lastI && f <= i))
        //                {
        //                    double f1 = FTABLE[row, 0];
        //                    double f2 = FTABLE[0, col];
        //                    dbF = f1 + f2;
        //                    return dbF;
        //                }
        //            }
        //        }
        //        col++;
        //        lastI = i;
        //    }
        //    return dbF;
        //}
        ////[32 rows,10cols]
        //public static double[,] FTABLE = {
        //   {0,0,0.01,0.02,0.03,0.04,0.05,0.06,0.07,0.08,0.09},
        //    {0,0,0.00399,0.00798,0.01197,0.01595,0.01994,0.02392,0.0279,0.03188,0.03586},
        //    {0.1,0.0398,0.0438,0.04776,0.05172,0.05567,0.05966,0.0636,0.06749,0.07142,0.07535},
        //    {0.2,0.0793,0.08317,0.08706,0.09095,0.09483,0.09871,0.10257,0.10642,0.11026,0.11409},
        //    {0.3,0.11791,0.12172,0.12552,0.1293,0.13307,0.13683,0.14058,0.14431,0.14803,0.15173},
        //    {0.4,0.15542,0.1591,0.16276,0.1664,0.17003,0.17364,0.17724,0.18082,0.18439,0.18793},
        //    {0.5,0.19146,0.19497,0.19847,0.20194,0.2054,0.20884,0.21226,0.21566,0.21904,0.2224},
        //    {0.6,0.22575,0.22907,0.23237,0.23565,0.23891,0.24215,0.24537,0.24857,0.25175,0.2549},
        //    {0.7,0.25804,0.26115,0.26424,0.2673,0.27035,0.27337,0.27637,0.27935,0.2823,0.28524},
        //    {0.8,0.28814,0.29103,0.29389,0.29673,0.29955,0.30234,0.30511,0.30785,0.31057,0.31327},
        //    {0.9,0.31594,0.31859,0.32121,0.32381,0.32639,0.32894,0.33147,0.33398,0.33646,0.33891},
        //    {1,0.34134,0.34375,0.34614,0.34849,0.35083,0.35314,0.35543,0.35769,0.35993,0.36214},
        //    {1.1,0.36433,0.3665,0.36864,0.37076,0.37286,0.37493,0.37698,0.379,0.381,0.38298},
        //    {1.2,0.38493,0.38686,0.38877,0.39065,0.39251,0.39435,0.39617,0.39796,0.39973,0.40147},
        //    {1.3,0.4032,0.4049,0.40658,0.40824,0.40988,0.41149,0.41308,0.41466,0.41621,0.41774},
        //    {1.4,0.41924,0.42073,0.4222,0.42364,0.42507,0.42647,0.42785,0.42922,0.43056,0.43189},
        //    {1.5,0.43319,0.43448,0.43574,0.43699,0.43822,0.43943,0.44062,0.44179,0.44295,0.44408},
        //    {1.6,0.4452,0.4463,0.44738,0.44845,0.4495,0.45053,0.45154,0.45254,0.45352,0.45449},
        //    {1.7,0.45543,0.45637,0.45728,0.45818,0.45907,0.45994,0.4608,0.46164,0.46246,0.46327},
        //    {1.8,0.46407,0.46485,0.46562,0.46638,0.46712,0.46784,0.46856,0.46926,0.46995,0.47062},
        //    {1.9,0.47128,0.47193,0.47257,0.4732,0.47381,0.47441,0.475,0.47558,0.47615,0.4767},
        //    {2,0.47725,0.47778,0.47831,0.47882,0.47932,0.47982,0.4803,0.48077,0.48124,0.48169},
        //    {2.1,0.48214,0.48257,0.483,0.48341,0.48382,0.48422,0.48461,0.485,0.48537,0.48574},
        //    {2.2,0.4861,0.48645,0.48679,0.48713,0.48745,0.48778,0.48809,0.4884,0.4887,0.48899},
        //    {2.3,0.48928,0.48956,0.48983,0.4901,0.49036,0.49061,0.49086,0.49111,0.49134,0.49158},
        //    {2.4,0.4918,0.49202,0.49224,0.49245,0.49266,0.49286,0.49305,0.49324,0.49343,0.49361},
        //    {2.5,0.49379,0.49396,0.49413,0.4943,0.49446,0.49461,0.49477,0.49492,0.49506,0.4952},
        //    {2.6,0.49534,0.49547,0.4956,0.49573,0.49585,0.49598,0.49609,0.49621,0.49632,0.49643},
        //    {2.7,0.49653,0.49664,0.49674,0.49683,0.49693,0.49702,0.49711,0.4972,0.49728,0.49736},
        //    {2.8,0.49744,0.49752,0.4976,0.49767,0.49774,0.49781,0.49788,0.49795,0.49801,0.49807},
        //    {2.9,0.49813,0.49819,0.49825,0.49831,0.49836,0.49841,0.49846,0.49851,0.49856,0.49861},
        //    {3,0.49865,0.49869,0.49874,0.49878,0.49882,0.49886,0.49889,0.49893,0.49896,0.499}
        //};
        
        public static double GetPercent(double numerator, double divisor)
        {
            double dbPercent = 0;
            numerator = Math.Round(numerator, 4);
            divisor = Math.Round(divisor, 4);
            if (divisor == 0)
            {
                dbPercent = 0;
            }
            else
            {
                if (divisor < 0 && numerator > 0)
                {
                    dbPercent = ((numerator / divisor) * 100) * -1;
                }
                else if (divisor < 0 && numerator < 0)
                {

                    dbPercent = ((numerator / divisor) * 100) * -1;
                }
                else
                {
                    dbPercent = (numerator / divisor) * 100;
                }
            }
            return dbPercent;
        }
        public static double GetPercentUseIndex(int i, double numerator, double divisor)
        {
            double dbPercent = 0;
            numerator = Math.Round(numerator, 4);
            divisor = Math.Round(divisor, 4);
            if (divisor == 0
                || i == 0)
            {
                dbPercent = 0;
            }
            else
            {
                if (divisor < 0 && numerator > 0)
                {
                    dbPercent = ((numerator / divisor) * 100) * -1;
                }
                else if (divisor < 0 && numerator < 0)
                {

                    dbPercent = ((numerator / divisor) * 100) * -1;
                }
                else
                {
                    dbPercent = (numerator / divisor) * 100;
                }
            }
            return dbPercent;
        }
        public static double GetPercentUseIndex(int i, double numerator, double divisor, double firstNumber, double secondNumber)
        {
            double dbPercent = 0;
            numerator = Math.Round(numerator, 4);
            divisor = Math.Round(divisor, 4);
            if (divisor == 0
                || i == 0)
            {
                dbPercent = 0;
            }
            else
            {
                //avoid this complication
                if (firstNumber > 0 && secondNumber > 0)
                {
                    dbPercent = (numerator / divisor) * 100;
                }
                else if (firstNumber < 0 && secondNumber < 0)
                {
                    dbPercent = (numerator / divisor) * 100;
                }
                else if (firstNumber > 0 && secondNumber < 0)
                {

                    dbPercent = (numerator / divisor) * 100;

                }
                else if (firstNumber < 0 && secondNumber > 0)
                {
                    dbPercent = ((numerator / divisor) * 100) * -1;
                }
                else
                {
                    dbPercent = (numerator / divisor) * 100;
                }
            }
            return dbPercent;
        }
        public static bool IsSelfOrDescendentNode(Constants.SUBAPPLICATION_TYPES subappType,
            string docToCalcNodeName, string currentNodeName)
        {
            CalculatorParameters calcPs = new CalculatorParameters();
            calcPs.SubApplicationType = subappType;
            calcPs.StartingDocToCalcNodeName = docToCalcNodeName;
            bool bIsSelfOrDescendentNode = IsSelfOrDescendentNode(calcPs,
                currentNodeName);
            return bIsSelfOrDescendentNode;
        }
        public static bool IsSelfOrDescendentNode(CalculatorParameters calculatorParams,
            string currentNodeName)
        {
            bool bIsSelfOrDescendentNode = false;
            if (calculatorParams.StartingDocToCalcNodeName
                == currentNodeName)
            {
                return true;
            }
            //don't run calcs on ancestor nodes
            if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    if (currentNodeName == DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    if (currentNodeName == DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    if (currentNodeName == DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.operationprices)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
                {
                    if (currentNodeName == DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    if (currentNodeName == DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budgetgroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budget.ToString())
                {
                    if (currentNodeName != DevTreksAppHelpers.Economics1.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                {
                    if (currentNodeName != DevTreksAppHelpers.Economics1.BUDGET_TYPES.budgetgroup.ToString()
                        && currentNodeName != DevTreksAppHelpers.Economics1.BUDGET_TYPES.budget.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.investments)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString())
                {
                    if (currentNodeName != DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    if (currentNodeName != DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investmentgroup.ToString()
                        && currentNodeName != DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.locals)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Locals.LOCAL_TYPES.local.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString())
                {
                    bIsSelfOrDescendentNode = true;
                }
            }
            else if (calculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.resources)
            {
                if (!currentNodeName.StartsWith(DevTreksAppHelpers.Resources.RESOURCES_TYPES.resource.ToString()))
                {
                    return false;
                }
                if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Resources.RESOURCES_TYPES.resourcegroup.ToString())
                {
                    if (currentNodeName
                        == DevTreksAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString()
                        || currentNodeName
                        == DevTreksAppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
                else if (calculatorParams.StartingDocToCalcNodeName
                    == DevTreksAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString())
                {
                    if (currentNodeName
                        == DevTreksAppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                    {
                        bIsSelfOrDescendentNode = true;
                    }
                }
            }
            return bIsSelfOrDescendentNode;
        }
        public static bool IsSelfOrChildNode(CalculatorParameters calculatorParams,
            string currentNodeName)
        {
            bool bIsSelfOrChildNode = false;
            if (calculatorParams.StartingDocToCalcNodeName
                == currentNodeName)
            {
                return true;
            }
            //limit insertion of descendant calcs to children only
            if (currentNodeName.StartsWith(Constants.DEVPACKS_TYPES.devpack.ToString()))
            {
                //devpack nodes can only be here if self or child
                bIsSelfOrChildNode = true;
            }
            else
            {
                if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        if (currentNodeName
                            == DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                    {
                        if (currentNodeName
                            == DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        if (currentNodeName
                            == DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        if (currentNodeName
                            == DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
                        {
                            //don't insert op calcors into outputs
                            bIsSelfOrChildNode = false;
                        }
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
                        {
                            //don't insert op calcors into inputs
                            bIsSelfOrChildNode = false;
                        }
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
                        {
                            //don't insert comp calcors into inputs
                            bIsSelfOrChildNode = false;
                        }
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budget.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budget.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                    {
                        //don't insert tp calcors into descendants
                        bIsSelfOrChildNode = false;
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                    else if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        //don't insert tp calcors into descendants
                        bIsSelfOrChildNode = false;
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.locals)
                {
                    if (calculatorParams.StartingDocToCalcNodeName
                        == DevTreksAppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString())
                    {
                        if (currentNodeName
                             == DevTreksAppHelpers.Locals.LOCAL_TYPES.local.ToString())
                        {
                            bIsSelfOrChildNode = true;
                        }
                    }
                }
                else if (calculatorParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.resources)
                {
                    //not sure about resources until some calcors get built and tested
                    bIsSelfOrChildNode = false;
                }
            }
            return bIsSelfOrChildNode;
        }
        public static bool IsNodeStartingCalculations(CalculatorParameters calculatorParams,
            XElement currentElement)
        {
            bool bIsNodeStartingCalculations = false;
            string sDocToCalcId
                = CalculatorHelpers.GetURIPatternId(calculatorParams.ExtensionDocToCalcURI.URIPattern);
            string sDocToCalcNodeName = calculatorParams.DocToCalcNodeName;
            string sCurrentId = DevTreksEditHelpers.XmlLinq.GetAttributeValue(
                currentElement, Calculator1.cId);
            if (calculatorParams.IsCustomDoc)
            {
                //startingdoctocalc has the node initiating calculations
                sDocToCalcId
                    = CalculatorHelpers.GetURIPatternId(
                    calculatorParams.ExtensionDocToCalcURI.URIDataManager.StartingDocToCalcURIPattern);
                sDocToCalcNodeName = calculatorParams.StartingDocToCalcNodeName;
            }
            //the node inititating calculations is the node needing the new calculations
            if (sDocToCalcNodeName.Equals(currentElement.Name.LocalName)
                && sDocToCalcId.Equals(sCurrentId))
            {
                bIsNodeStartingCalculations = true;
            }
            return bIsNodeStartingCalculations;
        }
        
        public static bool SetDevPackUpdatesState(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            IDictionary<string, string> updates, ref string errorMsg)
        {
            bool bHasState = false;
            //add to updates for saving, during next step, in sCalculatedDocToCalcPaths
            if (updates.ContainsKey(extDocToCalcURI.URIClub.ClubDocFullPath) == false)
            {
                updates.Add(extDocToCalcURI.URIClub.ClubDocFullPath,
                    extDocToCalcURI.URIDataManager.TempDocPath);
            }
            else
            {
                //overwrite the value of the existing list item
                updates[extDocToCalcURI.URIClub.ClubDocFullPath]
                    = extDocToCalcURI.URIDataManager.TempDocPath;
            }
            return bHasState;
        }
        public static void UpdateCalculatorParams(CalculatorParameters initCalculatorParams,
            CalculatorParameters postCalculatorParams)
        {
            //copying all calcparams not needed in this extension
            if (postCalculatorParams.LinkedViewElement != null)
            {
                initCalculatorParams.LinkedViewElement
                    = new XElement(postCalculatorParams.LinkedViewElement);
            }
            if (postCalculatorParams.Updates != null)
            {
                initCalculatorParams.Updates = postCalculatorParams.Updates;
            }
            initCalculatorParams.ErrorMessage = postCalculatorParams.ErrorMessage;
            //custom docs need some calcparams updated (startingdoctocalc)
            UpdateCalcParams(initCalculatorParams, postCalculatorParams);
        }
        public static void UpdateCalcParams(CalculatorParameters initCalculatorParams,
            CalculatorParameters postCalculatorParams)
        {
            if (initCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType 
                == Constants.SUBAPPLICATION_TYPES.devpacks.ToString())
            {
                //overwrite the value of the existing list item
                initCalculatorParams.ExtensionDocToCalcURI.URIDataManager.StartingDocToCalcURIPattern
                    = postCalculatorParams.ExtensionDocToCalcURI.URIDataManager.StartingDocToCalcURIPattern;
                //v170 simplified pattern handles updating lvs here rather than during document builds
                UpdateDevPackAnalyzerParams(initCalculatorParams);
            }
        }
        public static void UpdateDevPackAnalyzerParams(CalculatorParameters calcs)
        {
            if (calcs.ExtensionDocToCalcURI.URIDataManager.SubAppType
                == Constants.SUBAPPLICATION_TYPES.devpacks.ToString())
            {
                //188 remove any update member that is not related to a devpack (i.e. fileexttype could end up updating a base el)
                if (calcs.Updates != null)
                {
                    List<string> keysToDelete = new List<string>();
                    foreach (var k in calcs.Updates)
                    {
                        if (!k.Key.Contains(Constants.DEVPACKS_TYPES.devpack.ToString()))
                        {
                            keysToDelete.Add(k.Key);
                        }
                    }
                    foreach (var k in keysToDelete)
                    {
                        calcs.Updates.Remove(k);
                    }
                }
                //v170 simplified pattern handles updating lvs here rather than during document builds
                if (calcs.LinkedViewElement != null)
                {
                    AddXmlDocToUpdates(calcs.LinkedViewElement, calcs);
                    //children can be updated using .Children or the subfiles collection
                }
            }
        }
        //v180
        public static string SetXmlDocAttributes(bool needsUpdateList,
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            XElement linkedViewsElement, XElement docToCalcElement,
            string docToCalcURIPattern, XElement ancestorCalcDocNode,
            string stepNumber, string linkedViewId,
            IDictionary<string, string> updates)
        {
            string sErrorMsg = string.Empty;
            if (docToCalcElement == null || linkedViewsElement == null)
            {
                sErrorMsg = Errors.MakeStandardErrorMsg("ADDINHELPER_MISSINGDOC");
                return sErrorMsg;
            }
            string sLinkedViewIds = (docToCalcURIPattern == extDocToCalcURI.URIPattern)
                ? GetLinkedViewIds(extDocToCalcURI) : string.Empty;
            if (docToCalcElement.Name.LocalName !=
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                && linkedViewsElement.Name.LocalName ==
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                sErrorMsg = SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, linkedViewsElement,
                    docToCalcElement, docToCalcURIPattern,
                    sLinkedViewIds, stepNumber, linkedViewId, updates);
            }
            else if (docToCalcElement.Name.LocalName ==
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                && linkedViewsElement.Name.LocalName !=
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                sErrorMsg = SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, docToCalcElement,
                    linkedViewsElement, docToCalcURIPattern,
                    sLinkedViewIds, stepNumber, linkedViewId, updates);
            }
            else
            {
                string sLinkedViewId = Data.ContentURI.GetURIPatternPart(
                    extCalcDocURI.URIPattern, Data.ContentURI.URIPATTERNPART.id);
                XElement newCalcElement
                    = Data.EditHelpers.XmlLinq.GetChildLinkedView(linkedViewsElement, sLinkedViewId);
                if (newCalcElement != null)
                {
                    sErrorMsg = SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, newCalcElement,
                        docToCalcElement, docToCalcURIPattern,
                        sLinkedViewIds, stepNumber, linkedViewId, updates);
                }
                else
                {
                    //replace all nodes
                    sErrorMsg = SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, linkedViewsElement,
                        docToCalcElement, docToCalcURIPattern,
                        sLinkedViewIds, stepNumber, linkedViewId, updates);
                }
            }
            return sErrorMsg;
        }
        public static void SetXmlDocAttributes(bool needsUpdateList,
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            XElement linkedViewsElement, ref XElement docToCalcElement,
            string docToCalcURIPattern, XElement ancestorCalcDocNode,
            string stepNumber, string linkedViewId, 
            IDictionary<string, string> updates, ref string errorMsg)
        {
            if (docToCalcElement == null || linkedViewsElement == null)
            {
                errorMsg = Errors.MakeStandardErrorMsg("ADDINHELPER_MISSINGDOC");
                return;
            }
            string sLinkedViewIds = (docToCalcURIPattern == extDocToCalcURI.URIPattern)
                ? GetLinkedViewIds(extDocToCalcURI) : string.Empty;
            if (docToCalcElement.Name.LocalName !=
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                && linkedViewsElement.Name.LocalName ==
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, linkedViewsElement,
                    ref docToCalcElement, docToCalcURIPattern,
                    sLinkedViewIds, stepNumber, linkedViewId, updates, ref errorMsg);
            }
            else if (docToCalcElement.Name.LocalName ==
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                && linkedViewsElement.Name.LocalName !=
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, docToCalcElement,
                    ref linkedViewsElement, docToCalcURIPattern,
                    sLinkedViewIds, stepNumber, linkedViewId, updates, ref errorMsg);
            }
            else
            {
                string sLinkedViewId = Data.ContentURI.GetURIPatternPart(
                    extCalcDocURI.URIPattern, Data.ContentURI.URIPATTERNPART.id);
                XElement newCalcElement
                    = Data.EditHelpers.XmlLinq.GetChildLinkedView(linkedViewsElement, sLinkedViewId);
                if (newCalcElement != null)
                {
                    SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, newCalcElement,
                        ref docToCalcElement, docToCalcURIPattern,
                        sLinkedViewIds, stepNumber, linkedViewId, updates, ref errorMsg);
                }
                else
                {
                    //replace all nodes
                    SetXmlDocAttribute(needsUpdateList, extDocToCalcURI, extCalcDocURI, linkedViewsElement,
                        ref docToCalcElement, docToCalcURIPattern,
                        sLinkedViewIds, stepNumber, linkedViewId, updates, ref errorMsg);
                }
            }
        }
        public static string GetLinkedViewIds(ExtensionContentURI extDocToCalcURI)
        {
            string sArrayOfLinkedViewIds = string.Empty;
            if (extDocToCalcURI.URIDataManager.LinkedView != null)
            {
                StringBuilder oStrBldr = new StringBuilder();
                //used to delete old, unused linkedviews during new linkedview insertions
                foreach (var linkedviewparent in extDocToCalcURI.URIDataManager.LinkedView)
                {
                    foreach (ExtensionContentURI linkedview in linkedviewparent)
                    {
                        if (oStrBldr.Length == 0)
                        {
                            oStrBldr.Append(linkedview.URIId.ToString());
                        }
                        else
                        {
                            oStrBldr.Append(Data.Helpers.GeneralHelpers.PARAMETER_DELIMITER);
                            oStrBldr.Append(linkedview.URIId.ToString());
                        }
                    }
                    sArrayOfLinkedViewIds = oStrBldr.ToString();
                }
            }
            return sArrayOfLinkedViewIds;
        }
        //v180 pattern 
        public static string SetXmlDocAttribute(bool needsUpdateList,
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            XElement linkedViewsElement, XElement docToCalcElement,
            string docToCalcURIPattern, string linkedViewsIds,
            string stepNumber, string linkedViewId,
            IDictionary<string, string> updates)
        {
            string sErrorMsg = string.Empty;
            //updates need a non-empty attvalue (value will be taken directly from doctocalc)
            string sAttValue = Data.Helpers.GeneralHelpers.ROOT_PATH;
            //check to see if the xmldoc is stored in a separate linkedview db table
            bool bIsLinkedViewXmlDoc = IsLinkedViewXmlDoc(docToCalcURIPattern,
                extDocToCalcURI);
            //inserts individual linked views into individual table rows
            XElement replacedLinkedView = null;
            bool bIsReplaced = SetLinkedViewAttribute(linkedViewsElement,
                ref docToCalcElement, linkedViewsIds, linkedViewId, bIsLinkedViewXmlDoc,
                ref sErrorMsg, out replacedLinkedView);
            if (needsUpdateList && bIsReplaced)
            {
                if (replacedLinkedView != null)
                    sAttValue = replacedLinkedView.ToString();
                //only the db att needs to be set
                DevTreksHelpers.AddInHelper.AddToDbList(docToCalcURIPattern,
                    Data.Helpers.GeneralHelpers.ROOT_PATH,
                    sAttValue, Data.RuleHelpers.GeneralRules.XML,
                    stepNumber, updates);
            }
            return sErrorMsg;
        }
        public static void SetXmlDocAttribute(bool needsUpdateList,
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            XElement linkedViewsElement, ref XElement docToCalcElement,
            string docToCalcURIPattern, string linkedViewsIds,
            string stepNumber, string linkedViewId,
            IDictionary<string, string> updates, ref string errorMsg)
        {
            //updates need a non-empty attvalue (value will be taken directly from doctocalc)
            string sAttValue = Data.Helpers.GeneralHelpers.ROOT_PATH;
            //check to see if the xmldoc is stored in a separate linkedview db table
            bool bIsLinkedViewXmlDoc = IsLinkedViewXmlDoc(docToCalcURIPattern, 
                extDocToCalcURI);
            //inserts individual linked views into individual table rows
            XElement replacedLinkedView = null;
            bool bIsReplaced = SetLinkedViewAttribute(linkedViewsElement,
                ref docToCalcElement, linkedViewsIds, linkedViewId, bIsLinkedViewXmlDoc,
                ref errorMsg, out replacedLinkedView);
            if (needsUpdateList && bIsReplaced)
            {
                if (replacedLinkedView != null)
                    sAttValue = replacedLinkedView.ToString();
                //only the db att needs to be set
                DevTreksHelpers.AddInHelper.AddToDbList(docToCalcURIPattern,
                    Data.Helpers.GeneralHelpers.ROOT_PATH,
                    sAttValue, Data.RuleHelpers.GeneralRules.XML,
                    stepNumber, updates);
            }
        }
        
        public static bool IsLinkedViewXmlDoc(string currentNodeURIPattern,
            ExtensionContentURI extDocToCalcURI)
        {
            Data.Helpers.GeneralHelpers.SUBAPPLICATION_TYPES eSubAppType
                   = Data.Helpers.GeneralHelpers.GetSubApplicationType(extDocToCalcURI.URIDataManager.SubAppType);
            bool bIsLinkedViewXmlDoc = Data.AppHelpers.LinkedViews.IsLinkedViewXmlDoc(
                   currentNodeURIPattern, eSubAppType);
            return bIsLinkedViewXmlDoc;
        }
        public static bool SetLinkedViewAttribute(XElement linkedViewsElement,
            ref XElement docToCalcElement, string linkedViewsIds,
            string linkedViewId, bool isLinkedViewXmlDoc, 
            ref string errorMsg, out XElement replacedLinkedView)
        {
            replacedLinkedView = null;
            //insert the current linkedview in linkedViewsElement into docToCalcElement
            bool bIsReplaced = Data.EditHelpers.XmlLinq.ReplaceOrInsertLinkedViewElement(
                ref docToCalcElement, string.Empty, linkedViewsElement,
                linkedViewsIds, linkedViewId, isLinkedViewXmlDoc, 
                out replacedLinkedView);
            return bIsReplaced;
        }
        //v180
        public static string AddXmlDocIds(bool needsUpdateList,
           ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
           XElement linkedViewsElement, XElement docToCalcElement,
           string docToCalcURIPattern, XElement ancestorCalcDocNode,
           string stepNumber, string linkedViewId,
           IDictionary<string, string> updates)
        {
            string sErrorMsg = string.Empty;
            if (docToCalcElement == null || linkedViewsElement == null)
                return sErrorMsg;
            //add the new LinkedViewId
            if (!string.IsNullOrEmpty(extCalcDocURI.URIDataManager.BaseId.ToString()))
            {
                if (needsUpdateList)
                {
                    bool bLinkedViewExists = Data.EditHelpers.XmlLinq.LinkedViewChildExists(
                        docToCalcElement, Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                        Calculator1.cId, linkedViewId);
                    if (bLinkedViewExists == false)
                    {
                        //add the new LinkedViewId to the db update list
                        DevTreksHelpers.AddInHelper.AddToDbList(docToCalcURIPattern,
                            Data.AppHelpers.LinkedViews.LINKEDVIEWBASEID,
                            extCalcDocURI.URIDataManager.BaseId.ToString(),
                            DevTreks.Data.RuleHelpers.GeneralRules.INTEGER,
                            stepNumber, updates);
                    }
                }
            }
            return sErrorMsg;
        }
        
        public static void AddXmlDocIds(bool needsUpdateList,
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            XElement linkedViewsElement, ref XElement docToCalcElement,
            string docToCalcURIPattern, XElement ancestorCalcDocNode,
            string stepNumber, string linkedViewId,
            IDictionary<string, string> updates, ref string errorMsg)
        {
            if (docToCalcElement == null || linkedViewsElement == null)
                return;
            //add the new LinkedViewId
            if (!string.IsNullOrEmpty(extCalcDocURI.URIDataManager.BaseId.ToString()))
            {
                if (needsUpdateList)
                {
                    bool bLinkedViewExists = Data.EditHelpers.XmlLinq.LinkedViewChildExists(
                        docToCalcElement, Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                        Calculator1.cId, linkedViewId);
                    if (bLinkedViewExists == false)
                    {
                        //add the new LinkedViewId to the db update list
                        DevTreksHelpers.AddInHelper.AddToDbList(docToCalcURIPattern,
                            Data.AppHelpers.LinkedViews.LINKEDVIEWBASEID,
                            extCalcDocURI.URIDataManager.BaseId.ToString(),
                            DevTreks.Data.RuleHelpers.GeneralRules.INTEGER,
                            stepNumber, updates);
                    }
                }
            }
        }
        public static bool IsSaveAction(ExtensionContentURI extDocToCalcURI)
        {
            bool bIsSave = (extDocToCalcURI.URIDataManager.TempDocSaveMethod
                == Data.Helpers.AddInHelper.SAVECALCS_METHOD.none.ToString()
                || extDocToCalcURI.URIDataManager.TempDocSaveMethod == string.Empty)
                ? false : true;
            return bIsSave;
        }
        public static void SetTempDocSaveNoneProperty(ExtensionContentURI extDocToCalcURI)
        {
            if (extDocToCalcURI.URIDataManager.TempDocSaveMethod
                == Data.Helpers.AddInHelper.SAVECALCS_METHOD.none.ToString()
                || extDocToCalcURI.URIDataManager.TempDocSaveMethod == string.Empty)
            {
                extDocToCalcURI.URIDataManager.TempDocSaveMethod
                    = Data.Helpers.AddInHelper.SAVECALCS_METHOD.none.ToString();
            }
        }
        public static void SetTempDocSaveCalcsProperty(ExtensionContentURI extDocToCalcURI)
        {
            if (extDocToCalcURI.URIDataManager.TempDocSaveMethod
                == Data.Helpers.AddInHelper.SAVECALCS_METHOD.none.ToString()
                || extDocToCalcURI.URIDataManager.TempDocSaveMethod == string.Empty)
            {
                extDocToCalcURI.URIDataManager.TempDocSaveMethod
                    = Data.Helpers.AddInHelper.SAVECALCS_METHOD.calcs.ToString();
            }
        }
        public static void SetTempDocSaveAnalysesProperty(ExtensionContentURI extDocToCalcURI)
        {
            if (extDocToCalcURI.URIDataManager.TempDocSaveMethod
                == Data.Helpers.AddInHelper.SAVECALCS_METHOD.none.ToString()
                || extDocToCalcURI.URIDataManager.TempDocSaveMethod == string.Empty)
            {
                extDocToCalcURI.URIDataManager.TempDocSaveMethod
                    = Data.Helpers.AddInHelper.SAVECALCS_METHOD.analyses.ToString();
            }
        }
        public static void SetFileExtensionType(
            CalculatorParameters calcParams, string fileExtensionType,
            ref XElement linkedViewsElement)
        {
            if (linkedViewsElement == null)
                return;
            bool bNeedsDbUpdate = false;
            //calcdocuri should have the file extension
            string sCalcDocURIFileExtensionType
                = DevTreks.Data.ContentURI.GetURIPatternPart(
                calcParams.ExtensionCalcDocURI.URIPattern,
                Data.ContentURI.URIPATTERNPART.fileExtension);
            if (!sCalcDocURIFileExtensionType.Equals(fileExtensionType))
                bNeedsDbUpdate = true;
            if (bNeedsDbUpdate == false)
            {
                //analyzers find these uris using this attribute
                string sFileExtensionType
                    = DevTreksEditHelpers.XmlLinq
                    .GetAttributeValue(linkedViewsElement,
                        Calculator1.cFileExtensionType);
                if (!sFileExtensionType.Equals(fileExtensionType))
                    bNeedsDbUpdate = true;
            }
            if (bNeedsDbUpdate)
            {
                DevTreksEditHelpers.XmlLinq
                    .SetAttributeValue(linkedViewsElement,
                    Calculator1.cFileExtensionType, fileExtensionType);
                //add to list of db updates
                string sStepNumber = string.Empty;
                SetFileExtensionTypeToUpdate(calcParams, fileExtensionType);
            }
        }
        public static void SetFileExtensionType(
            CalculatorParameters calcParams, string fileExtensionType)
        {
            if (calcParams.LinkedViewElement == null)
                return;
            bool bNeedsDbUpdate = false;
            //calcdocuri should have the file extension
            string sCalcDocURIFileExtensionType
                = DevTreks.Data.ContentURI.GetURIPatternPart(
                calcParams.ExtensionCalcDocURI.URIPattern,
                Data.ContentURI.URIPATTERNPART.fileExtension);
            if (!sCalcDocURIFileExtensionType.Equals(fileExtensionType))
                bNeedsDbUpdate = true;
            if (bNeedsDbUpdate == false)
            {
                //analyzers find these uris using this attribute
                string sFileExtensionType
                    = DevTreksEditHelpers.XmlLinq
                    .GetAttributeValue(calcParams.LinkedViewElement,
                        Calculator1.cFileExtensionType);
                if (!sFileExtensionType.Equals(fileExtensionType))
                    bNeedsDbUpdate = true;
            }
            if (bNeedsDbUpdate)
            {
                DevTreksEditHelpers.XmlLinq
                    .SetAttributeValue(calcParams.LinkedViewElement,
                    Calculator1.cFileExtensionType, fileExtensionType);
                //add to list of db updates
                string sStepNumber = string.Empty;
                SetFileExtensionTypeToUpdate(calcParams, fileExtensionType);
            }
        }
        public static void SetFileExtensionTypeToUpdate(
            CalculatorParameters calcParams, string fileExtensionType)
        {
            if (fileExtensionType
                != DevTreksHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                && fileExtensionType != DevTreksHelpers.GeneralHelpers.NONE)
            {
                //add to list of db updates
                DevTreksHelpers.AddInHelper.AddToDbList(calcParams.ExtensionCalcDocURI.URIPattern,
                    Calculator1.cFileExtensionType, fileExtensionType,
                    DevTreks.Data.RuleHelpers.GeneralRules.STRING,
                    calcParams.StepNumber, calcParams.Updates);
            }
        }
        public static bool ReplaceOrInsertChildLinkedViewElement(XElement linkedViewsElement,
            ref XElement docToCalcElement)
        {
            bool bHasReplacedElement = DevTreksEditHelpers.XmlLinq
                .ReplaceOrInsertChildLinkedViewElement(
                linkedViewsElement, ref docToCalcElement);
            return bHasReplacedElement;
        }
        public static bool ReplaceElementInDocument(XElement linkedViewsElement,
            ref XElement linkedViewsDocument)
        {
            bool bHasReplacedElement = DevTreksEditHelpers.XmlLinq.ReplaceElementInRootDoc(
                linkedViewsElement, ref linkedViewsDocument);
            return bHasReplacedElement;
        }
        public static bool ReplaceChildElementInDocument(XElement linkedViewsElement,
            string parentId, string parentNodeName, ref XElement linkedViewsDocument)
        {
            bool bHasReplacedElement = DevTreksEditHelpers.XmlLinq.ReplaceChildElement(
                linkedViewsElement, parentId, parentNodeName, ref linkedViewsDocument);
            return bHasReplacedElement;
        }
        public static bool ReplaceOrInsertDescendantElement(XElement replacementElement,
            string parentURIPattern, IDictionary<string, string> ancestors,
            ref XElement root)
        {
            bool bHasReplacedElement = DevTreksEditHelpers.XmlLinq.ReplaceOrInsertDescendantElement(
                replacementElement, parentURIPattern, ancestors, ref root);
            return bHasReplacedElement;
        }
        #endregion
        #region "run calculations"
        public static void GetLinkedViewType(ExtensionContentURI extDocToCalcURI, 
            ExtensionContentURI extCalcDocURI, XElement linkedViewsElement,
            out string devDocType, out string whatIfTagName, ref string errorMsg)
        {
            devDocType = string.Empty;
            whatIfTagName = string.Empty;
            if (linkedViewsElement.HasAttributes)
            {
                GetLinkedViewType(linkedViewsElement, Calculator1.cCalculatorType,
                    out devDocType, out whatIfTagName, ref errorMsg);
            }
        }
        public static void GetLinkedViewType(XElement linkedViewsElement, 
            string calcAttName, out string calculatorType, 
            out string whatIfTagName, ref string errorMsg)
        {
            calculatorType = string.Empty;
            whatIfTagName = string.Empty;
            if (linkedViewsElement.HasAttributes)
            {
                //calctype determines which calculator or analyzer to run
                calculatorType = Data.EditHelpers.XmlLinq.GetAttributeValue(linkedViewsElement,
                    calcAttName);
                string sId = Data.EditHelpers.XmlLinq.GetAttributeValue(linkedViewsElement,
                    Calculator1.cId);
                //whatIfTagName determines which scenario to run in descendants
                whatIfTagName = Data.EditHelpers.XmlLinq.GetAttributeValue(linkedViewsElement,
                    Data.AppHelpers.LinkedViews.WHATIF_TAG_NAME);
                //if no whatiftagname, use default params in descendants
                if (whatIfTagName == null || whatIfTagName == Data.Helpers.GeneralHelpers.NONE)
                    whatIfTagName = string.Empty;
            }
            if (calculatorType == Data.Helpers.GeneralHelpers.NONE
                || calculatorType == string.Empty)
            {
                errorMsg = Errors.MakeStandardErrorMsg(
                    "ADDINHELPER_NOCALCTYPE");
            }
        }
        #endregion

        #region "misc utilities"
        public static string GetAttribute(XElement el, string attName)
        {
            string sAttValue 
                = DevTreks.Data.EditHelpers.XmlLinq.GetElementAttributeValue(
                    el, attName);
            return sAttValue;
        }
        public static void SetAttribute(XElement el, string attName,
            string attValue)
        {
            if (el != null)
            el.SetAttributeValue(attName, attValue);
        }
        public static string GetAttributeValue(string nodeName, string nodeId,
            string attName, XElement root)
        {
            string sValue = DevTreksEditHelpers.XmlLinq.GetAttributeValue(root,
                nodeName, nodeId, attName);
            return sValue;
        }
        public static string GetAttributeNameExtension(int i, 
           string compOption3)
        {
            string sAttNameExt = string.Empty;
            if (compOption3 == AnalyzerHelper.COMPARISON_OPTIONS.compareonly.ToString())
            {
                sAttNameExt = string.Concat(Constants.FILENAME_DELIMITER, i.ToString());
            }
            return sAttNameExt;
        }
        public static bool SetAttributeValue(string nodeName, string nodeId,
            string attName, string attValue, ref XElement root)
        {
            bool bHasSetValue = DevTreksEditHelpers.XmlLinq.SetAttributeValue(root,
                nodeName, nodeId, attName, attValue);
            return bHasSetValue;
        }
        public static bool GetAttributeBool(XElement el, string attName)
        {
            bool bConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToBool(
                DevTreks.Data.EditHelpers.XmlLinq.GetElementAttributeValue(
                    el, attName));
            return bConvertedNumber;
        }
        public static void SetAttributeBool(XElement el, string attName,
            bool attValue)
        {
            //need sqlserver-compatible bools
            string sAttValue = attValue.ToString();
            if (attValue.ToString().ToLower().Equals("true"))
            {
                sAttValue = "1";
            }
            else if (attValue.ToString().ToLower().Equals("false"))
            {
                sAttValue = "0";
            }
            if (el != null)
            el.SetAttributeValue(attName, sAttValue);
        }
        public static DateTime GetAttributeDate(XElement el, string attName)
        {
            DateTime dtConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToDate(
                DevTreks.Data.EditHelpers.XmlLinq.GetElementAttributeValue(
                    el, attName));
            return dtConvertedNumber;
        }
        public static void SetAttributeDateS(XElement el, string attName,
            DateTime attValue)
        {
            //use iso860 sortable dates
            if (el != null)
                el.SetAttributeValue(attName, attValue.ToString("d", DateTimeFormatInfo.InvariantInfo));
        }
        public static decimal GetAttributeDecimal(XElement el, string attName)
        {
            decimal dcConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToDecimal(
                DevTreks.Data.EditHelpers.XmlLinq.GetElementAttributeValue(
                    el, attName));
            return dcConvertedNumber;
        }
        public static void SetAttributeDoubleN2(XElement el, string attName,
            double attValue)
        {
            //unit costs need three digits
            if (el != null)
                el.SetAttributeValue(attName, attValue.ToString("N2", CultureInfo.InvariantCulture));
        }
        public static void SetAttributeDoubleN3(XElement el, string attName,
            double attValue)
        {
            //unit costs need three digits
            if (el != null)
                el.SetAttributeValue(attName, attValue.ToString("N3", CultureInfo.InvariantCulture));
        }
        public static void SetAttributeDoubleN4(XElement el, string attName,
           double attValue)
        {
            //unit costs need three digits
            if (el != null)
                el.SetAttributeValue(attName, attValue.ToString("N4", CultureInfo.InvariantCulture));
        }
        public static void SetAttributeDoubleF3(XElement el, string attName,
            double attValue)
        {
            //unit costs need three digits
            if (el != null)
            el.SetAttributeValue(attName, attValue.ToString("f3"));
        }
        public static void SetAttributeDoubleF2(XElement el, string attName,
            double attValue)
        {
            //unit costs need three digits
            if (el != null)
            el.SetAttributeValue(attName, attValue.ToString("f2"));
        }
        public static decimal ConvertStringToDecimal(string stringToConvert)
        {
            decimal dcConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToDecimal(stringToConvert);
            return dcConvertedNumber;
        }
        public static double CheckForNaNandRound4(double numberToCheck)
        {
            double number = Math.Round(numberToCheck, 4);
            if (numberToCheck.ToString().Contains("NaN"))
            {
                number = 0.0000;
            }
            return number;
        }
        public static bool ConvertStringToBool(string stringToConvert)
        {
            bool bConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToBool(stringToConvert);
            return bConvertedNumber;
        }
        public static DateTime ConvertStringToDate(string stringToConvert)
        {
            DateTime dtConvertedDate
                = DevTreksHelpers.GeneralHelpers.ConvertStringToDate(stringToConvert);
            return dtConvertedDate;
        }
        public static int GetAttributeInt(XElement el, string attName)
        {
            int iConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToInt(
                DevTreks.Data.EditHelpers.XmlLinq.GetElementAttributeValue(
                    el, attName));
            return iConvertedNumber;
        }
        public static DateTime GetDateShortNow()
        {
            return DevTreksHelpers.GeneralHelpers.GetDateShortNow();
        }
        public static void SetAttributeInt(XElement el, string attName,
            int attValue)
        {
            //shouldn't need to format integers
            if (el != null)
            el.SetAttributeValue(attName, attValue);
        }
        
        public static int ConvertStringToInt(string stringToConvert)
        {
            int iConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToInt(stringToConvert);
            return iConvertedNumber;
        }
        public static List<List<string>> GetList(int rowCount, int colCount)
        {
            List<List<string>> DataResults = new List<List<string>>();
            List<string> col = new List<string>();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    col.Add("0");
                }
                DataResults.Add(col);
                col = new List<string>();
            }
            return DataResults;
        }
        public static double GetAttributeDouble(XElement el, string attName)
        {
            double dbConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToDouble(
                DevTreks.Data.EditHelpers.XmlLinq.GetElementAttributeValue(
                    el, attName));
            return dbConvertedNumber;
        }
        public static void SetAttributeDoubleF4(XElement el, string attName,
            double attValue)
        {
            //some constants use up to four digits 
            //(even though cost and benefit data doesn't need that level of precision)
            if (el != null)
            el.SetAttributeValue(attName, attValue.ToString("f4"));
        }
        public static double ConvertStringToDouble(string stringToConvert)
        {
            double dbConvertedNumber
                = DevTreksHelpers.GeneralHelpers.ConvertStringToDouble(stringToConvert);
            return dbConvertedNumber;
        }
        
        public static void ChangeToWhatIfScenario(string whatIfTagName,
            ref XElement linkedViewsElement)
        {
            //if a whatif scenario exists in currentEconNode.xmldoc, 
            //change currentEconNode's atts to the whatif atts
            bool bUseWhatIfOnly = true;
            XElement oCalculationsElement = null;
            DevTreks.Data.AppHelpers.LinkedViews.GetLinkedViewFromXmlDocAttribute(bUseWhatIfOnly,
                whatIfTagName, linkedViewsElement, out oCalculationsElement);
            if (oCalculationsElement != null)
            {
                DevTreks.Data.EditHelpers.XmlLinq.AddNodeAttributes(
                    oCalculationsElement, ref linkedViewsElement);
            }
        }
        public static bool NeedsAncestorCalculator(XElement ancestorCalcElement)
        {
            bool bNeedsCalculators
                = DevTreksHelpers.AddInHelperLinq.NeedsAncestorCalculator(ancestorCalcElement);
            return bNeedsCalculators;
        }
        public static bool HasCalculations(XElement linkedViewsElement)
        {
            bool bHasCalculations = false;
            if (linkedViewsElement.Name.ToString().StartsWith(
                DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
            {
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        //v137 pattern
        public static bool HasRelatedCalculatorsType(XElement linkedViewsElement, string relatedCalcsType)
        {
            bool bHasSameAttribute = false;
            if (linkedViewsElement != null)
            {
                string sRCalcsType = GetRelatedCalculatorsType(linkedViewsElement);
                if (sRCalcsType == relatedCalcsType)
                {
                    bHasSameAttribute = true;
                }
            }
            return bHasSameAttribute;
        }
        public static string GetURIPatternNodeName(
            string uriPattern)
        {
            string sNodeName = Data.ContentURI.GetURIPatternPart(uriPattern, 
                Data.ContentURI.URIPATTERNPART.node);
            return sNodeName;
        }
        
        public static string GetWhatIfScenario(XElement linkedViewsElement)
        {
            //ancestor calculators can optionally specify a whatif scenario
            //to use in descendants
            string sWhatIf = GetAttribute(linkedViewsElement,
                Calculator1.cWhatIfTagName);
            if (sWhatIf == null)
                sWhatIf = string.Empty;
            return sWhatIf;
        }
        public static string GetRelatedCalculatorType(XElement linkedViewsElement)
        {
            //ancestor calculators can optionally specify a relatedcalculatortype
            //to use in descendants
            string sRCalculatorType = GetAttribute(linkedViewsElement,
                Calculator1.cRelatedCalculatorType);
            if (sRCalculatorType == null)
                sRCalculatorType = string.Empty;
            return sRCalculatorType;
        }
        public static string GetRelatedCalculatorsType(XElement linkedViewsElement)
        {
            //ancestor calculators can optionally specify a relatedcalculatortype
            //to use in descendants
            string sRCalculatorType = GetAttribute(linkedViewsElement,
                Calculator1.cRelatedCalculatorsType);
            if (sRCalculatorType == null)
                sRCalculatorType = string.Empty;
            return sRCalculatorType;
        }
        public static string GetCalculatorType(XElement linkedViewsElement)
        {
            string sCalculatorType = GetAttribute(linkedViewsElement,
                Calculator1.cCalculatorType);
            if (sCalculatorType == null)
                sCalculatorType = string.Empty;
            return sCalculatorType;
        }
        public static string GetVersion(XElement linkedViewsElement)
        {
            string sVersion = GetAttribute(linkedViewsElement,
                Calculator1.cVersion);
            return sVersion;
        }
        public static void AddDisplayParameters(CalculatorParameters calcParams,
            ExtensionContentURI extDocToCalcURI)
        {
            //this might be used to display linkedview data
            StringBuilder strB = new StringBuilder();
            strB.Append("&");
            strB.Append(Calculator1.WHATIF_TAGNAME_FORM);
            strB.Append("=");
            strB.Append(calcParams.WhatIfScenario);
            strB.Append("&");
            strB.Append(Calculator1.RELATEDCALCULATORSTYPE_FORM);
            strB.Append("=");
            strB.Append(calcParams.RelatedCalculatorsType);
            strB.Append(extDocToCalcURI.URIDataManager.CalcParams);
        }
        public static string GetStylesheet2Name(XElement linkedViewsElement)
        {
            string sStylesheet2Name = GetAttribute(linkedViewsElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (sStylesheet2Name == null)
                sStylesheet2Name = string.Empty;
            return sStylesheet2Name;
        }
        public static string GetStylesheet2ObjNS(XElement linkedViewsElement)
        {
            string sStylesheet2ObjNS = GetAttribute(linkedViewsElement,
                Calculator1.cStylesheet2ObjectNS);
            if (sStylesheet2ObjNS == null)
                sStylesheet2ObjNS = string.Empty;
            return sStylesheet2ObjNS;
        }
        public static string GetAnalyzerType(XElement linkedViewsElement)
        {
            //ancestor calculators specify the input and output calculator 
            //to use in descendants
            string sAnalyzerType = string.Empty;
            sAnalyzerType = GetAttribute(linkedViewsElement,
                Calculator1.cAnalyzerType);
            return sAnalyzerType;
        }
        public static void SetAnalyzerParameters(CalculatorParameters calcParams,
            XElement currentLinkedViewElement)
        {
            if (currentLinkedViewElement != null)
            {
                //each linked view needs standard analyzer parameters set
                CalculatorHelpers.SetAttribute(currentLinkedViewElement,
                    Calculator1.cFileExtensionType, calcParams.FileExtensionType);
                CalculatorHelpers.SetAttribute(currentLinkedViewElement,
                    Calculator1.cStylesheet2ResourceFileName, calcParams.Stylesheet2Name);
                CalculatorHelpers.SetAttribute(currentLinkedViewElement,
                        Calculator1.cStylesheet2ObjectNS,
                        calcParams.Stylesheet2ObjectNS);
            }
        }
        
        public static XElement GetAllyCalculator(CalculatorParameters calculatorParams,
            XElement currentElement)
        {
            //does not use calctype as filter
            XElement newCalculator = null;
            string sCalcType = calculatorParams.CalculatorType;
            if (string.IsNullOrEmpty(sCalcType))
            {
                sCalcType = GetCalculatorType(calculatorParams.LinkedViewElement);
            }
            string sRelatedCalcType = calculatorParams.RelatedCalculatorType;
            if (string.IsNullOrEmpty(sRelatedCalcType))
            {
                sRelatedCalcType = GetRelatedCalculatorType(calculatorParams.LinkedViewElement);
            }
            string sRelatedCalcsType = calculatorParams.RelatedCalculatorsType;
            if (string.IsNullOrEmpty(sRelatedCalcsType))
            {
                sRelatedCalcsType = GetRelatedCalculatorsType(calculatorParams.LinkedViewElement);
            }
            string sWhatIfScenario = calculatorParams.WhatIfScenario;
            if (string.IsNullOrEmpty(sWhatIfScenario))
            {
                GetWhatIfScenario(calculatorParams.LinkedViewElement);
            }
            //don't filter by calculatortype
            newCalculator = GetAllyCalculator(calculatorParams, 
                currentElement, currentElement.Name.LocalName,
                string.Empty, sRelatedCalcType, sRelatedCalcsType, sWhatIfScenario);
            return newCalculator;
        }
        public static XElement GetAllyCalculator(CalculatorParameters calculatorParams,
            XElement currentElement, string calculatorType)
        {
            //uses calctype as filter
            XElement newCalculator = null;
            string sRelatedCalcType = GetRelatedCalculatorType(calculatorParams.LinkedViewElement);
            string sRelatedCalcsType = GetRelatedCalculatorsType(calculatorParams.LinkedViewElement);
            string sWhatIfScenario = GetWhatIfScenario(calculatorParams.LinkedViewElement);
            newCalculator = GetAllyCalculator(calculatorParams, 
                currentElement, currentElement.Name.LocalName,
                calculatorType, sRelatedCalcType, sRelatedCalcsType, sWhatIfScenario);
            return newCalculator;
        }
        public static XElement GetAllyCalculator(CalculatorParameters calculatorParams,
            XElement currentElement, string currentNodeName,
            string calculatorType, string relatedCalculatorType,
            string relatedCalculatorsType, string whatIfScenario)
        {
            //rules for finding other calculators/analyzers
            //(these rules may evolve further)
            XElement allyCalculator = null;
            IEnumerable<XElement> linkedviews = null;
            bool bHasCalculatorTypeCollection = false;
            //get base calculator or analyzer
            if (calculatorType != string.Empty)
            {
                //Rule 1. start with the calctype 
                linkedviews = GetChildrenLinkedViewUsingAttribute(currentElement,
                   Calculator1.cCalculatorType, calculatorType);
                //linkedviews = GetChildrenLinkedViewUsingAttributes(currentElement,
                //  Calculator1.cCalculatorType, calculatorType, Calculator1.cAnalyzerType, analyzerParameters.AnalyzerType);
                if (linkedviews != null)
                {
                    bHasCalculatorTypeCollection = true;
                    //one calculator means it's the one needed
                    if (linkedviews.Count() == 1)
                    {
                        return linkedviews.FirstOrDefault();
                    }
                }
            }
            if (linkedviews == null)
            {
                //Rule 2. No Calcs? Retrieve all lvs
                linkedviews = GetChildrenLinkedView(currentElement);
            }
            if (linkedviews != null)
            {
                if (linkedviews.Count() == 0)
                {
                    linkedviews = GetChildrenLinkedView(currentElement);
                }
                if (linkedviews.Count() > 0)
                {
                    allyCalculator = GetAllyCalculator(linkedviews,
                        calculatorParams, currentElement, currentNodeName,
                        calculatorType, relatedCalculatorType,
                        relatedCalculatorsType, whatIfScenario);
                }
            }
            if (allyCalculator == null && bHasCalculatorTypeCollection)
            { 
                //refactor potential: may want to eliminate the condition (i.e. allow highest id calulator)
                //same subapps may be doing scenarios and should not use rule 5
                bool bIsSameSubApp = IsSameSubApp(
                    calculatorParams, currentElement.Name.LocalName);
                if (!bIsSameSubApp)
                {
                    //rule 7: ally calculator found? use highest id (latest calculation)
                    //for this type of calculator
                    //i.e. a budget running joint input calcs can use latest agmach calcs
                    allyCalculator = CalculatorHelpers
                        .GetElementUsingHighestId(linkedviews);
                }
            }
            return allyCalculator;
        }

        public static XElement GetRelatedCalculator(CalculatorParameters calculatorParams,
           XElement currentElement)
        {
            //rules for finding related calculators/analyzers
            //(used by resource stock analyzers)
            //this method doesn't insert new calcs or update old ones (use GetCalc for that)
            XElement allyCalculator = null;
            //only xmldocs can be changed
            calculatorParams.NeedsXmlDocOnly = true;
            IEnumerable<XElement> linkedviews = GetChildrenLinkedView(currentElement);
            if (linkedviews != null)
            {
                if (linkedviews.Count() > 0)
                {
                    allyCalculator = GetAllyCalculator(linkedviews,
                        calculatorParams, currentElement, currentElement.Name.LocalName, 
                        calculatorParams.CalculatorType, calculatorParams.RelatedCalculatorType,
                        calculatorParams.RelatedCalculatorsType, calculatorParams.WhatIfScenario);
                }
            }
            return allyCalculator;
        }
        public static XElement GetAllyCalculator(IEnumerable<XElement> linkedviews,
            CalculatorParameters calculatorParams, 
            XElement currentElement, string currentNodeName, string calculatorType, 
            string relatedCalculatorType, string relatedCalculatorsType, string whatIfScenario)
        {
            //rule 3: Get the lv using ancestors
            //that may have been set when ancestor joint calcs run
            XElement allyCalculator = GetCalculatorFromAncestors(
                calculatorParams, currentElement);
            if (allyCalculator == null)
            {
                //Rule 4: Get the lv using the relatedcalculatortype and calc att name
                //This requires using GeneralCalculator.ChangeLinkedViewCalculator before db lv is added to updates collection
                //so that analyzers never, ever, overwrite descendent calculators
                if (relatedCalculatorType != string.Empty && relatedCalculatorType != Constants.NONE)
                    allyCalculator
                        = Data.EditHelpers.XmlLinq.GetElementUsingAttribute(
                            linkedviews, Calculator1.cCalculatorType,
                            relatedCalculatorType);
                if (allyCalculator == null)
                {
                    //rule 5: Get the lv using the analysis type
                    //v137 added this rule to make sure descendent analyzers could be updated
                    if (calculatorParams.AnalyzerParms.AnalyzerType != string.Empty && calculatorParams.AnalyzerParms.AnalyzerType != Constants.NONE)
                        allyCalculator
                            = Data.EditHelpers.XmlLinq.GetElementUsingAttribute(
                                linkedviews, Calculator1.cAnalyzerType,
                                calculatorParams.AnalyzerParms.AnalyzerType);
                    if (allyCalculator == null)
                    {
                        //rule 6: Get the lv using the relatedcalculatorstype
                        //This requires using GeneralCalculator.ChangeLinkedViewCalculator before db lv is added to updates collection
                        //v137 changed this rule to make sure that the lv is a calculator (not an analyzer)
                        if (relatedCalculatorsType != string.Empty && relatedCalculatorsType != Constants.NONE)
                            allyCalculator
                                = Data.EditHelpers.XmlLinq.GetElementUsingAttributeWithBlankAttribute(
                                    linkedviews, Calculator1.cRelatedCalculatorsType,
                                    relatedCalculatorsType, Calculator1.cAnalyzerType);
                    }
                }
            }
            return allyCalculator;
        }
        public static void ReplaceAnalyzerCalculator(CalculatorParameters calcParams,
            XElement currentElement, string analyzerType, ref XElement currentCalculationsElement)
        {
            //standard getally calcs will retrieve calcors for analyzers that are
            //being used by legitimate db children linked views which must not be overwritten
            //this is only used when the Use Same Calculator in Children is used in analyzers
            //note that up to 0.8.8, analyzers never included this property
            //0.8.8 recognized that future analyzers might want to include this feature
            if (calcParams.NeedsCalculators
                && (calcParams.StartingDocToCalcNodeName
                != Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()
                && calcParams.StartingDocToCalcNodeName
                != OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()
                && calcParams.StartingDocToCalcNodeName
                != OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()
                && calcParams.StartingDocToCalcNodeName
                != BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                && calcParams.StartingDocToCalcNodeName
                != BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString()))
            {
                XElement analyzerCalculator = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                    currentElement, Calculator1.cAnalyzerType, analyzerType);
                if (analyzerCalculator == null)
                {
                    //need the attributes from the current calculator
                    analyzerCalculator = new XElement(currentCalculationsElement);
                    //but reset Ids so that currentcalc is not overwritten
                    int iRandomId = GetRandomInteger(new Random());
                    analyzerCalculator.SetAttributeValue(Calculator1.cId, iRandomId.ToString());
                    analyzerCalculator.SetAttributeValue(Calculator1.cAnalyzerType, analyzerType);
                    analyzerCalculator.SetAttributeValue(Calculator1.cCalculatorType, string.Empty);
                    calcParams.NeedsXmlDocOnly = false;
                }
                if (analyzerCalculator != null)
                {
                    currentCalculationsElement = new XElement(analyzerCalculator);
                }
                else
                {
                    //need the calcs, but don't overwrite it or insert it in children
                    calcParams.NeedsXmlDocOnly = true;
                    calcParams.NeedsCalculators = false;
                }
            }
        }
        public static IEnumerable<XElement> GetChildrenLinkedView(
            XElement currentElement)
        {
            IEnumerable<XElement> linkedviews
                = Data.EditHelpers.XmlLinq.GetChildrenLinkedView(currentElement);
            return linkedviews;
        }
        public static XElement GetCalculatorFromAncestors(
            CalculatorParameters calculatorParams, XElement currentElement)
        {
            XElement ancestorEditedCalculator = null;
            int iCurrentId = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            string sCurrentNodeName = currentElement.Name.LocalName;
            if (currentElement != null)
            {
                if (sCurrentNodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                {
                    //inputs are the only calculations changed by ancestors
                    if (calculatorParams.ParentOperationComponent != null)
                    {
                        if (calculatorParams.ParentOperationComponent.Inputs != null)
                        {
                            
                            foreach (Input input in
                                calculatorParams.ParentOperationComponent.Inputs)
                            {
                                if (input.Id == iCurrentId)
                                {
                                    ancestorEditedCalculator
                                        = Data.EditHelpers.XmlLinq.GetChildLinkedViewUsingAttribute(
                                            input.XmlDocElement, CostBenefitCalculator.TYPE_NEWCALCS,
                                            "true");
                                    if (ancestorEditedCalculator != null)
                                    {
                                        Data.EditHelpers.XmlLinq.RemoveAttribute(
                                            ancestorEditedCalculator, CostBenefitCalculator.TYPE_NEWCALCS);
                                        return ancestorEditedCalculator;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (sCurrentNodeName.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //inputs are the only calculations changed by ancestors
                    if (calculatorParams.ParentOutcome != null)
                    {
                        if (calculatorParams.ParentOutcome.Outputs != null)
                        {

                            foreach (Output output in
                                calculatorParams.ParentOutcome.Outputs)
                            {
                                if (output.Id == iCurrentId)
                                {
                                    ancestorEditedCalculator
                                        = Data.EditHelpers.XmlLinq.GetChildLinkedViewUsingAttribute(
                                            output.XmlDocElement, CostBenefitCalculator.TYPE_NEWCALCS,
                                            "true");
                                    if (ancestorEditedCalculator != null)
                                    {
                                        Data.EditHelpers.XmlLinq.RemoveAttribute(
                                            ancestorEditedCalculator, CostBenefitCalculator.TYPE_NEWCALCS);
                                        return ancestorEditedCalculator;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (sCurrentNodeName
                    .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                    || sCurrentNodeName
                    .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
                {
                    //inputs are the only calculations changed by ancestors
                    if (calculatorParams.ParentTimePeriod != null)
                    {
                        if (calculatorParams.ParentTimePeriod.OperationComponents != null)
                        {
                            foreach (OperationComponent opcomp in
                                calculatorParams.ParentTimePeriod.OperationComponents)
                            {
                                if (opcomp.Id == iCurrentId)
                                {
                                    ancestorEditedCalculator
                                        = Data.EditHelpers.XmlLinq.GetChildLinkedViewUsingAttribute(
                                            opcomp.XmlDocElement, CostBenefitCalculator.TYPE_NEWCALCS,
                                            "true");
                                    if (ancestorEditedCalculator != null)
                                    {
                                        Data.EditHelpers.XmlLinq.RemoveAttribute(
                                            ancestorEditedCalculator, CostBenefitCalculator.TYPE_NEWCALCS);
                                        return ancestorEditedCalculator;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (sCurrentNodeName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
                {
                    //outputs are the only calculations changed by ancestors
                    if (calculatorParams.ParentTimePeriod != null)
                    {
                        if (calculatorParams.ParentTimePeriod.Outcomes != null)
                        {
                            foreach (Outcome outcome in
                                calculatorParams.ParentTimePeriod.Outcomes)
                            {
                                if (outcome.Id == iCurrentId)
                                {
                                    ancestorEditedCalculator
                                        = Data.EditHelpers.XmlLinq.GetChildLinkedViewUsingAttribute(
                                            outcome.XmlDocElement, CostBenefitCalculator.TYPE_NEWCALCS,
                                            "true");
                                    if (ancestorEditedCalculator != null)
                                    {
                                        Data.EditHelpers.XmlLinq.RemoveAttribute(
                                            ancestorEditedCalculator, CostBenefitCalculator.TYPE_NEWCALCS);
                                        return ancestorEditedCalculator;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (sCurrentNodeName
                    == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || sCurrentNodeName
                    == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    if (calculatorParams.ParentBudgetInvestment != null)
                    {
                        if (calculatorParams.ParentBudgetInvestment.TimePeriods != null)
                        {
                            foreach (TimePeriod time in
                                calculatorParams.ParentBudgetInvestment.TimePeriods)
                            {
                                if (time.Id == iCurrentId)
                                {
                                    ancestorEditedCalculator
                                        = Data.EditHelpers.XmlLinq.GetChildLinkedViewUsingAttribute(
                                            time.XmlDocElement, CostBenefitCalculator.TYPE_NEWCALCS,
                                            "true");
                                    if (ancestorEditedCalculator != null)
                                    {
                                        Data.EditHelpers.XmlLinq.RemoveAttribute(
                                            ancestorEditedCalculator, CostBenefitCalculator.TYPE_NEWCALCS);
                                        return ancestorEditedCalculator;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ancestorEditedCalculator;
        }
        
        public static bool IsSameSubApp(CalculatorParameters calculatorParams, 
            string currentNodeName)
        {
            bool bIsSameSubApp = false;
            string sCurrentSubAppType
                = Data.Helpers.GeneralHelpers.GetSubAppTypeFromNodeName(
                currentNodeName);
            bIsSameSubApp
                = calculatorParams.ExtensionDocToCalcURI
                .URIDataManager.SubAppType.Equals(sCurrentSubAppType)
                ? true : false;
            return bIsSameSubApp;
        }
        public static string GetURIPatternFileExtensionType(
            string uriPattern)
        {
            string sFileExtensionType = Data.ContentURI.GetURIPatternPart(uriPattern,
                Data.ContentURI.URIPATTERNPART.fileExtension);
            return sFileExtensionType;
        }
        public static string GetURIPatternNetwork(
            string uriPattern)
        {
            string sNetwork = Data.ContentURI.GetURIPatternPart(uriPattern,
                Data.ContentURI.URIPATTERNPART.network);
            return sNetwork;
        }
        public static string GetURIPatternId(
            string uriPattern)
        {
            string sId = Data.ContentURI.GetURIPatternPart(uriPattern,
                Data.ContentURI.URIPATTERNPART.id);
            return sId;
        }
        public static string GetNewURIPattern(string uriPattern,
            string id, string nodeName)
        {
            string sNewURIPattern = string.Empty;
            string sName = string.Empty;
            string sURIId = string.Empty;
            string sNetwork = string.Empty;
            string sNodeName = string.Empty;
            string sFileNameExtension = string.Empty;
            DevTreks.Data.ContentURI.GetURIParams(uriPattern,
                out sName, out sURIId, out sNetwork, out sNodeName, out sFileNameExtension);
            sNewURIPattern = CalculatorHelpers.MakeURIPattern(
                sName, id, sNetwork, nodeName, sFileNameExtension);
            return sNewURIPattern;
        }
        public static string MakeURIPattern(string currentName,
           string currentId, string networkName, string currentNodeName,
           string fileExtension)
        {
            string sURIPattern = DevTreksHelpers.GeneralHelpers.MakeURIPattern(
                currentName, currentId, networkName, currentNodeName, fileExtension);
            return sURIPattern;
        }
        public static string GetFileToCalculate(CalculatorParameters calcParams)
        {
            string sNewDocToCalcTempDocPath = string.Empty;
            //parallel processing needs unique temp file names
            if (calcParams.RndGenerator == null)
                calcParams.RndGenerator = new Random();
            int iRandomNumber = GetRandomInteger(
                calcParams.RndGenerator);
            string sTempExt = string.Concat(iRandomNumber.ToString(), ".xml");

            //1.6.0: local cloud cache no longer used for tempdoc storage (use temp blob container like the rest)
            //this was changed to false
            bool bIsLocalCacheDirectory = false;
            sNewDocToCalcTempDocPath = GetTempDocsPath(calcParams.ExtensionDocToCalcURI,
                bIsLocalCacheDirectory, calcParams.ExtensionDocToCalcURI.URIPattern,
                calcParams.ExtensionDocToCalcURI.URIDataManager.TempDocURIPattern);
            sNewDocToCalcTempDocPath
                = sNewDocToCalcTempDocPath.Replace(".xml", sTempExt);
            return sNewDocToCalcTempDocPath;
        }
        public static void GetTempDocFiles(IList<string> urisToAnalyze,
            ref IDictionary<string, string> lstFileOrFolderPaths)
        {
            int i = 0;
            foreach (string sTempDocToCalcFilePath in urisToAnalyze)
            {
                lstFileOrFolderPaths.Add(i.ToString(), sTempDocToCalcFilePath);
                i++;
            }
        }
        public static async Task<string> MoveURIsAsync(
            ExtensionContentURI extURI, StringWriter fromWriter, string toFile)
        {
            DevTreksHelpers.FileStorageIO fsIO = new DevTreksHelpers.FileStorageIO();
            string sErrorMsg = await fsIO.SaveTextURIAsync(
                extURI.URIDataManager.InitialDocToCalcURI,
                fromWriter, toFile);
            return sErrorMsg;
        }
        public static string MoveURIs(ExtensionContentURI extURI,
            StringWriter fromWriter, string toFile)
        {
            DevTreksHelpers.FileStorageIO fsIO = new DevTreksHelpers.FileStorageIO();
            string sErrorMsg = fsIO.SaveTextURI(extURI.URIDataManager.InitialDocToCalcURI,
                fromWriter, toFile);
            return sErrorMsg;
        }
        public static void CopyFiles(ExtensionContentURI extURI,
            string fromFile, string toFile)
        {
            DevTreksHelpers.FileStorageIO.CopyURIs(extURI.URIDataManager.InitialDocToCalcURI,
                fromFile, toFile);
        }
        public static async Task<List<string>> ReadLinesAsync(
            ExtensionContentURI extURI, string fromFile)
        {
            DevTreksHelpers.FileStorageIO fs = new DevTreksHelpers.FileStorageIO();
            List<string> lines = await fs.ReadLinesAsync(
                extURI.URIDataManager.InitialDocToCalcURI, 
                fromFile);
            return lines;
        }
        public static string GetPlatform(ExtensionContentURI extURI, string fullPath)
        {
            DevTreksHelpers.FileStorageIO.PLATFORM_TYPES ePlatformType 
                = GetPlatformType(extURI, fullPath);
            return ePlatformType.ToString();
        }
        public static DevTreksHelpers.FileStorageIO.PLATFORM_TYPES GetPlatformType(
            ExtensionContentURI extURI, string fullPath)
        {
            DevTreksHelpers.FileStorageIO.PLATFORM_TYPES ePlatformType 
                = DevTreksHelpers.FileStorageIO.GetPlatformType(
                    extURI.URIDataManager.InitialDocToCalcURI);
            return ePlatformType;
        }
        public static async Task<string> InvokeHttpRequestResponseServiceAsync(string fromFile)
        {
            DevTreksHelpers.FileStorageIO fs = new DevTreksHelpers.FileStorageIO();
            //wait until response is actually received before continuing
            return await fs.InvokeHttpRequestResponseServiceAsync(fromFile).ConfigureAwait(false);
        }
        public static async Task<string> InvokeHttpRequestResponseService(
            ExtensionContentURI extURI, string baseURL, string apiKey,
            string inputFileLocation, string outputFileLocation, string script)
        {
            DevTreksHelpers.FileStorageIO fs = new DevTreksHelpers.FileStorageIO();
            //wait until response is actually received before continuing
            return await fs.InvokeHttpRequestResponseService(
                extURI.URIDataManager.InitialDocToCalcURI, baseURL, apiKey,
                inputFileLocation, outputFileLocation, script).ConfigureAwait(false);
        }
        public static async Task<string> InvokeHttpRequestResponseService2(
            ExtensionContentURI extURI, string baseURL, string apiKey,
            string inputBlob1Location, string inputBlob2Location,
            string outputBlob1Location, string outputBlob2Location)
        {
            DevTreksHelpers.FileStorageIO fs = new DevTreksHelpers.FileStorageIO();
            //wait until response is actually received before continuing
            return await fs.InvokeHttpRequestResponseService2(
                extURI.URIDataManager.InitialDocToCalcURI, baseURL, apiKey,
                inputBlob1Location, inputBlob2Location, outputBlob1Location, outputBlob2Location).ConfigureAwait(false);
        }
        public static List<string> ReadLines(ExtensionContentURI extURI,
            string fromFile, out string errorMsg)
        {
            DevTreksHelpers.FileStorageIO fs = new DevTreksHelpers.FileStorageIO();
            List<string> lines = fs.ReadLines(extURI.URIDataManager.InitialDocToCalcURI,
                fromFile, out errorMsg);
            return lines;
        }
        public static List<string> GetLinesandSkip(string delimitedlines, int skipLines)
        {
            List<string> lines1 = DevTreksHelpers.GeneralHelpers.GetLines(delimitedlines);
            List<string> lines2 = lines1.Skip(skipLines).ToList();
            List<string> lines = new List<string>();
            if (lines2 != null)
            {
                //remove any line that is null or string empty; otherwise indexes fail
                foreach (var line in lines2)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }
        public static string ReadText(ExtensionContentURI extURI,
            string fromFile, out string errorMsg)
        {
            DevTreksHelpers.FileStorageIO fs = new DevTreksHelpers.FileStorageIO();
            string sText = fs.ReadText(extURI.URIDataManager.InitialDocToCalcURI,
                fromFile, out errorMsg);
            return sText;
        }
        public static void SetLocalsCalculation(
          CalculatorParameters calculatorParams, string currentNodeName,
          bool needsCalculators)
        {
            calculatorParams.NeedsCurrentCalculation = false;
            calculatorParams.NeedsXmlDocOnly = false;
            bool bIsSelfOrDescendentNode = IsSelfOrDescendentNode(calculatorParams, 
                currentNodeName);
            if (bIsSelfOrDescendentNode)
            {
                if (calculatorParams.StartingDocToCalcNodeName == currentNodeName)
                {
                    calculatorParams.NeedsCurrentCalculation = true;
                }
                //control the size of calcs run on customdocs
                if (calculatorParams.IsCustomDoc)
                {
                    //assume they need at least cumulative totals
                    //and let individual calculators decide what else is needed
                    calculatorParams.NeedsCurrentCalculation = true;
                }
                if (!calculatorParams.NeedsCurrentCalculation)
                {
                    if (currentNodeName
                        == DevTreks.Data.AppHelpers.Locals.LOCAL_TYPES.local.ToString())
                    {
                        calculatorParams.NeedsCurrentCalculation = true;
                    }
                }
            }
        }
        public static Local GetLocal(string currentNodeURIPattern,
            CalculatorParameters calcParameters, XElement currentLinkedViewElement, 
            XElement currentElement)
        {
            bool bHasGoodLocals = false;
            Local newLocal = new Local();
            //note the assumption that custom docs will always try to get their locals from calcor
            if (currentNodeURIPattern
                == calcParameters.ExtensionDocToCalcURI.URIPattern
                || calcParameters.NeedsCalculators
                || calcParameters.DocToCalcNodeName.StartsWith(DevTreksAppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString()))
            {
                bHasGoodLocals = newLocal.ElementHasGoodLocals(currentLinkedViewElement);
                if (bHasGoodLocals)
                {
                    newLocal.SetLocalProperties(calcParameters,
                        currentLinkedViewElement, currentElement);
                }
                else
                {
                    newLocal.SetLocalProperties(calcParameters,
                        calcParameters.LinkedViewElement, currentElement);
                }
            }
            else
            {
                bHasGoodLocals = newLocal.ElementHasGoodLocals(currentElement);
                if (bHasGoodLocals)
                {
                    newLocal.SetLocalProperties(calcParameters,
                        currentElement, currentElement);
                }
                else
                {
                    bHasGoodLocals = newLocal.ElementHasGoodLocals(currentLinkedViewElement);
                    if (bHasGoodLocals)
                    {
                        newLocal.SetLocalProperties(calcParameters,
                            currentLinkedViewElement, currentElement);
                    }
                    else
                    {
                        newLocal.SetLocalProperties(calcParameters,
                            calcParameters.LinkedViewElement, currentElement);
                    }
                }
            }
            return newLocal;
        }
        
        public static Constants.SUBAPPLICATION_TYPES 
            GetSubAppTypeFromNode(string currentNodeName)
        {
            Constants.SUBAPPLICATION_TYPES eSubAppType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(currentNodeName);
            return eSubAppType;
        }
        public static Constants.SUBAPPLICATION_TYPES ConvertSubAppType(
            string subAppType)
        {
            Constants.SUBAPPLICATION_TYPES eSubAppType 
                = Constants.SUBAPPLICATION_TYPES.none;
            if (subAppType == Constants.SUBAPPLICATION_TYPES.addins.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.addins;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.agreements.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.agreements;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.budgets.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.budgets;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.clubs.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.clubs;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.componentprices.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.componentprices;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.devpacks.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.devpacks;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.inputprices.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.inputprices;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.investments.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.investments;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.linkedviews.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.linkedviews;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.locals.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.locals;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.members.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.members;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.networks.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.networks;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.operationprices.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.operationprices;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.outcomeprices;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.outputprices.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.outputprices;
            }
            else if (subAppType == Constants.SUBAPPLICATION_TYPES.resources.ToString())
            {
                eSubAppType = Constants.SUBAPPLICATION_TYPES.resources;
            }
            return eSubAppType;
        }
        public static string GetSubAppTypeFromNodeName(string groupNodeName)
        {
            string sSubAppType 
                = DevTreksHelpers.GeneralHelpers.GetSubAppTypeFromNodeName(groupNodeName);
            return sSubAppType;
        }
        public static string MakeNewURIPatternFromElement(string existingURIPattern,
            XElement el)
        {
            string sNewURIPattern = string.Empty;
            MakeNewURIPatternFromElement(existingURIPattern, el, out sNewURIPattern);
            return sNewURIPattern;
        }
        public static void MakeNewURIPatternFromElement(string existingURIPattern,
            XElement el, out string newURIPattern)
        {
            newURIPattern = string.Empty;
            DevTreksEditHelpers.XmlLinq.MakeNewURIPatternFromElement(existingURIPattern,
                el, out newURIPattern);
        }
        public static int GetRandomInteger(Random rnd)
        {
            if (rnd == null)
            {
                rnd = new Random();
            }
            int iRandomInteger = DevTreksHelpers.GeneralHelpers.GetRandomInteger(rnd);
            return iRandomInteger;
        }
        public static int GetRandomInteger(CalculatorParameters calcParameters)
        {
            if (calcParameters.RndGenerator == null)
            {
                calcParameters.RndGenerator = new Random();
            }
            int iRandomInteger = DevTreksHelpers.GeneralHelpers.GetRandomInteger(calcParameters.RndGenerator);
            return iRandomInteger;
        }
        public static XElement GetElement(IEnumerable<XElement> elements, 
            string attributeName, string attributeValue)
        {
            //check the content node
            XElement element = DevTreksEditHelpers.XmlLinq.GetElementUsingAttribute(
                elements, attributeName, attributeValue);
            return element;
        }
        public static XElement GetElementUsingHighestId(IEnumerable<XElement> elements)
        {
            //check the content node
            XElement element = DevTreksEditHelpers.XmlLinq.GetElementUsingHighestId(
                elements);
            return element;
        }
        public static XElement GetElement(XElement root,
            string nodeName, string id)
        {
            //check the content node
            XElement element = DevTreksEditHelpers.XmlLinq.GetElement(
                root, nodeName, id);
            return element;
        }
        public static XElement GetElement(XElement root, string uriPattern)
        {
            string sNodeId = DevTreks.Data.ContentURI.GetURIPatternPart(uriPattern, 
                Data.ContentURI.URIPATTERNPART.id);
            string sNodeName = DevTreks.Data.ContentURI.GetURIPatternPart(uriPattern,
                Data.ContentURI.URIPATTERNPART.node);
            XElement element = DevTreksEditHelpers.XmlLinq.GetElement(
                root, sNodeName, sNodeId);
            return element;
        }
        public static XElement GetChildElement(XElement root, string childNodeName,
            string childId, string parentNodeName, string parentId)
        {
            XElement element = DevTreksEditHelpers.XmlLinq.GetChildElement(
                root, childNodeName, childId, parentNodeName, parentId);
            return element;
        }
        public static XElement GetChildLinkedViewUsingAttribute(XElement element,
            string attributeName, string attributeValue)
        {
            //check the content node
            XElement linkedview = DevTreks.Data.EditHelpers.XmlLinq.GetFirstChildLinkedViewUsingAttribute(
                element, attributeName, attributeValue);
            return linkedview;
        }
        public static XElement GetElementUsingAttribute(IEnumerable<XElement> elements,
            string attributeName, string attributeValue)
        {
            //check the content node
            XElement linkedview = DevTreks.Data.EditHelpers.XmlLinq.GetElementUsingAttribute(
                elements, attributeName, attributeValue);
            return linkedview;
        }
        public static IEnumerable<XElement> GetChildrenLinkedViewUsingAttribute(
           XElement parent, string attName, string attValue)
        {
            //check the content node
            IEnumerable<XElement> linkedviews
                = DevTreks.Data.EditHelpers.XmlLinq.GetChildrenLinkedViewUsingAttribute(
                parent, attName, attValue);
            return linkedviews;
        }
        public static IEnumerable<XElement> GetChildrenLinkedViewUsingAttributes(
           XElement parent, string att1Name, string att1Value, string att2Name, string att2Value)
        {
            //check the content node
            IEnumerable<XElement> linkedviews
                = DevTreks.Data.EditHelpers.XmlLinq.GetChildrenLinkedViewUsingAttributes(
                parent, att1Name, att1Value, att2Name, att2Value);
            return linkedviews;
        }
        public static IDictionary<string, string> CopyDictionary(
            IDictionary<string, string> oldDictionary)
        {
            IDictionary<string, string> newDict
                = DevTreksHelpers.LinqHelpers.CopyDictionary(oldDictionary);
            return newDict;
        }
       
        public static ExtensionContentURI GetDevPackCalcURI(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string docToCalcFilePath, ref string tempDocToCalcPath)
        {
            //store the parameters for the devpack file in a new ExtContentURI
            ExtensionContentURI devPackCalcURI
                = new ExtensionContentURI(extDocToCalcURI);
            //make new doctocalc params using doctocalcfilepath
            string sDocToCalcFileName
                = Path.GetFileNameWithoutExtension(docToCalcFilePath);
            //convert this filename to a uripattern
            DevTreks.Data.ContentURI newDocToCalcURI
                = new DevTreks.Data.ContentURI();
            DevTreks.Data.Helpers.GeneralHelpers.SetURIParamsFromFileName(
                newDocToCalcURI, sDocToCalcFileName);
            //don't use the file extension parameter with devpacks in updates list
            newDocToCalcURI.URIFileExtensionType = string.Empty;
            newDocToCalcURI.UpdateURIPattern();
            string sCalculatedDocToCalcPath
                = docToCalcFilePath.Replace(sDocToCalcFileName,
                DevTreks.Data.Helpers.ContentHelper.MakeStandardFileNameFromURIPattern(
                extCalcDocURI.URIPattern));
            //this becomes the value for adding to updates
            tempDocToCalcPath = extDocToCalcURI.URIDataManager.TempDocPath.Replace(
                Path.GetFileNameWithoutExtension(extDocToCalcURI.URIDataManager.TempDocPath),
                sDocToCalcFileName);
            //update devPackCalcURI
            devPackCalcURI.URIId = newDocToCalcURI.URIId;
            devPackCalcURI.URIName = newDocToCalcURI.URIName;
            devPackCalcURI.URINetworkPartName = newDocToCalcURI.URINetworkPartName;
            devPackCalcURI.URINodeName = newDocToCalcURI.URINodeName;
            devPackCalcURI.URIFileExtensionType = newDocToCalcURI.URIFileExtensionType;
            devPackCalcURI.URIPattern = newDocToCalcURI.URIPattern;
            devPackCalcURI.URIClub.ClubDocFullPath = sCalculatedDocToCalcPath;
            devPackCalcURI.URIDataManager.TempDocPath = tempDocToCalcPath;
            return devPackCalcURI;
        }
       
        #endregion
        #region "analyzers"
        public static bool DocumentHasSameElement(string nodeId,
            string nodeName, XElement root)
        {
            bool bHasSameElement = false;
            if (!string.IsNullOrEmpty(nodeId))
            {
                bHasSameElement = DevTreksEditHelpers.XmlLinq.DescendantExists(
                    root, nodeName, nodeId);
                bHasSameElement = DevTreksEditHelpers.XmlLinq.DescendantExists(
                    root, nodeName, nodeId);
            }
            return bHasSameElement;
        }
        public static bool ParentOfNodeTypeExists(XElement root, 
            string childNodeName, string parentNodeName, string parentId)
        {
            bool bHasSameParent = false;
            if (!string.IsNullOrEmpty(parentId))
            {
                bHasSameParent = DevTreksEditHelpers.XmlLinq.ParentOfNodeTypeExists(
                    root, childNodeName, parentNodeName, parentId);
            }
            return bHasSameParent;
        }
        public static bool ParentExists(string nodeId,
            string nodeName, string nodeAttributeName, string parentId, 
            string parentNodeName, XElement root)
        {
            bool bHasSameParent = false;
            if (!string.IsNullOrEmpty(nodeId)
                && !string.IsNullOrEmpty(parentId))
            {
                bHasSameParent = DevTreksEditHelpers.XmlLinq.ParentExists(
                    root, nodeName, nodeId, nodeAttributeName, 
                    parentNodeName, parentId);
            }
            return bHasSameParent;
        }
        public static XElement GetDescendant(string nodeId,
            string nodeName, string parentURIPattern,
            IDictionary<string, string> ancestors, XElement root)
        {
            XElement descendant = null;
            if (!string.IsNullOrEmpty(nodeId)
                && ancestors != null)
            {
                descendant = DevTreksEditHelpers.XmlLinq.GetDescendant(
                    nodeId, nodeName, parentURIPattern, ancestors, root);
            }
            return descendant;
        }
        public static string GetElementIdUsingSiblingAttribute(XElement root,
            string nodeName, string attName, string attValue)
        {
            string sId = string.Empty;
            if (attValue != string.Empty)
            {
                sId = DevTreksEditHelpers.XmlLinq.GetElementIdUsingSiblingAttribute(
                    root, nodeName, attName, attValue);
            }
            return sId;
        }
        public static string GetElementIdUsingParentAndSibling(XElement root,
            string parentId, string parentNodeName, string nodeName, string attName, 
            string attValue)
        {
            string sId = string.Empty;
            if (parentId != string.Empty && attValue != string.Empty)
            {
                sId = DevTreksEditHelpers.XmlLinq.GetElementIdUsingParentAndSibling(
                    root, parentId, parentNodeName, nodeName, attName, attValue);
            }
            return sId;
        }
        public static bool IsGroupingNode(string nodeName)
        {
            bool bIsGroupingNode = DevTreksHelpers.GeneralHelpers.IsGroupingNode(nodeName);
            return bIsGroupingNode;
        }
        public static bool SetCurrentElementIds(CalculatorParameters calcParameters, 
            string currentNodeName)
        {
            bool bIsGoodNode = false;
            if (calcParameters.DocToCalcReader == null)
                return false;
            //grouping nodes don't have sufficient ids to locate
            //if a grouping node exists, an element will be appended to it
            //based on it's parent timeperiod node (ParentURIPattern below)
            bool bIsGroupingNode = CalculatorHelpers.IsGroupingNode(currentNodeName);
            if (!bIsGroupingNode)
            {
                //set the currenturipattern
                string sName = calcParameters.DocToCalcReader
                    .GetAttribute(Calculator1.cName);
                string sId = calcParameters.DocToCalcReader
                    .GetAttribute(Calculator1.cId);
                string sNetworkId
                    = CalculatorHelpers.GetURIPatternNetwork(
                    calcParameters.ExtensionDocToCalcURI.URIPattern);
                string sFileExtension
                    = CalculatorHelpers.GetURIPatternFileExtensionType(
                    calcParameters.ExtensionDocToCalcURI.URIPattern);
                if (!string.IsNullOrEmpty(sId) && sId != "0")
                {
                    //set the current element
                    calcParameters.CurrentElementURIPattern
                        = CalculatorHelpers.MakeURIPattern(sName,
                        sId, sNetworkId, currentNodeName,
                        sFileExtension);
                    bIsGoodNode = true;
                }
            }
            return bIsGoodNode;
        }
        public static bool HasSameId(string xPathQry,
            XElement root)
        {
            bool bHasId = false;
            string sNodeId = string.Empty;
            string sNodeName = string.Empty;
            DevTreksEditHelpers.XPathIO.GetLastNodeIdAndNameFromXPathQuery(
                xPathQry, ref sNodeId, ref sNodeName);
            if (!string.IsNullOrEmpty(sNodeId))
            {
                bHasId = DevTreksEditHelpers.XmlLinq.DescendantExists(
                    root, sNodeName, sNodeId);
            }
            else
            {
                bHasId = root.Descendants()
                    .Any(d => d.Name.LocalName == sNodeName);
            }
            return bHasId;
        }

        public static void GetFirstSubFolderFiles(CalculatorParameters calcParameters,
            IList<string> urisToAnalyze, string fileExtension, 
            ref IDictionary<string, string> fileOrFolderPaths)
        {
            DevTreksHelpers.AddInHelper.GetFirstSubFolderFiles(
                calcParameters.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI, 
                urisToAnalyze, calcParameters.ExtensionDocToCalcURI.URIClub.ClubDocFullPath,
                fileExtension, ref fileOrFolderPaths);
        }
        public static string GetParsedString(int i, char[] delimiter,
            string name)
        {
            string sParsedName 
                = DevTreksHelpers.GeneralHelpers.GetParsedString(i, delimiter, name);
            return sParsedName;
        }
        public static void GetSubstringsSeparateLast(string fullString,
            string pathDelimiter, out string newFullString,
            out string lastSubstring)
        {
            newFullString = string.Empty;
            lastSubstring = string.Empty;
            DevTreksHelpers.GeneralHelpers.GetSubstringsSeparateLast(fullString,
                pathDelimiter, out newFullString, out lastSubstring);
        }
        #endregion
        #region LV calcs
        public static void AdjustSpecialtyLinkedViewElements(XElement currentElement,
            XElement linkedViewElement, CalculatorParameters calcParams)
        {
            if (calcParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                == Constants.SUBAPPLICATION_TYPES.locals.ToString())
            {
                if (currentElement.Name.LocalName
                    == Local.LOCAL_TYPES.localaccountgroup.ToString())
                {
                    linkedViewElement = null;
                }
                else if (currentElement.Name.LocalName
                    == Local.LOCAL_TYPES.local.ToString())
                {
                    //local nodes are treated the same as children of starting group node
                    calcParams.NeedsCalculators = true;
                    //locals have already been inserted
                    calcParams.NeedsXmlDocOnly = true;
                    linkedViewElement = new XElement(calcParams.LinkedViewElement);
                }
            }
        }
        public static void ChangeLinkedViewCalculator(XElement currentElement, XElement linkedViewElement, 
            CalculatorParameters calcParams)
        {
            //v137 pattern allows analyzers to update descendents using dbupdates
            //i.e. need i/o calculators to get totals, but don't want to 
            //overwrite those calculations in db
            if (calcParams.ExtensionCalcDocURI.URIDataManager.HostName
                == DevTreks.Data.Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()
                && calcParams.NeedsCalculators
                && CalculatorHelpers.IsSelfOrChildNode(calcParams, currentElement.Name.LocalName))
            {
                //100% Rule 1: Analyzers never, ever, update calculators
                string sCalculatorType = CalculatorHelpers.GetAttribute(linkedViewElement,
                    Calculator1.cCalculatorType);
                //pure calculators never have an analysis type
                string sAnalysisType = CalculatorHelpers.GetAttribute(linkedViewElement,
                    Calculator1.cAnalyzerType);
                if (!string.IsNullOrEmpty(sCalculatorType)
                    && string.IsNullOrEmpty(sAnalysisType))
                {
                    //order of lv retrieval gets calulators before analyzers
                    XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                        currentElement, Calculator1.cAnalyzerType, calcParams.AnalyzerParms.AnalyzerType);
                    if (analyzerLV != null)
                    {
                        if (calcParams.LinkedViewElement != null)
                        {
                            //keep the id and calculatorid, but update the rest of the atts with new lv
                            string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
                            string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
                            analyzerLV = new XElement(calcParams.LinkedViewElement);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
                        }
                        //use it to update db (not the calculator)
                        linkedViewElement = new XElement(analyzerLV);
                    }
                    else
                    {
                        //use the base linked view standard pattern
                        //avoids updating the wrong lvs
                        linkedViewElement = CalculatorHelpers.GetNewCalculator(calcParams, currentElement);
                    }
                }
            }
            //100% Rule 2: Analyzers and Calculators never, ever, allow descendent lvs 
            //to have parent Overwrite or UseSameCalc properties
            if (calcParams.StartingDocToCalcNodeName
                != currentElement.Name.LocalName)
            {
                if (linkedViewElement != null)
                {
                    string sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
                        Calculator1.cUseSameCalculator);
                    if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                    {
                        CalculatorHelpers.SetAttribute(linkedViewElement,
                            Calculator1.cUseSameCalculator, string.Empty);
                    }
                    sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
                        Calculator1.cOverwrite);
                    if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                    {
                        CalculatorHelpers.SetAttribute(linkedViewElement,
                            Calculator1.cOverwrite, string.Empty);
                    }
                }
            }
        }
        public static bool GetOverwrite(XElement linkedViewElement)
        {
            bool bOverwrite = CalculatorHelpers.GetAttributeBool(linkedViewElement,
                    Calculator1.cOverwrite);
            return bOverwrite;
        }
        public static bool GetUseSameCalculator(XElement linkedViewElement)
        {
            bool bUseSameCalculator = CalculatorHelpers.GetAttributeBool(linkedViewElement,
                    Calculator1.cUseSameCalculator);
            return bUseSameCalculator;
        }
        //v180 moved out of GeneralCalulator for more general use
        public static void ChangeLinkedViewCalculatorForAnalysis(CalculatorParameters calcParams, 
            XElement currentElement, ref XElement linkedViewElement)
        {
            //v137 pattern allows analyzers to update descendents using dbupdates
            //i.e. need i/o calculators to get totals, but don't want to 
            //overwrite those calculations in db
            if (calcParams.ExtensionCalcDocURI.URIDataManager.HostName
                == DevTreks.Data.Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()
                && calcParams.NeedsCalculators
                && CalculatorHelpers.IsSelfOrChildNode(calcParams, currentElement.Name.LocalName))
            {
                //100% Rule 1: Analyzers never, ever, update calculators
                string sCalculatorType = CalculatorHelpers.GetAttribute(linkedViewElement,
                    Calculator1.cCalculatorType);
                //pure calculators never have an analysis type
                string sAnalysisType = CalculatorHelpers.GetAttribute(linkedViewElement,
                    Calculator1.cAnalyzerType);
                //some analyzers include calculators (lca)
                if (!string.IsNullOrEmpty(sCalculatorType)
                    && string.IsNullOrEmpty(sAnalysisType))
                {
                    //order of lv retrieval gets calulators before analyzers
                    XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                        currentElement, Calculator1.cAnalyzerType, calcParams.AnalyzerParms.AnalyzerType);
                    if (analyzerLV != null)
                    {
                        if (calcParams.LinkedViewElement != null)
                        {
                            //keep the id and calculatorid, but update the rest of the atts with new lv
                            string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
                            string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
                            analyzerLV = new XElement(calcParams.LinkedViewElement);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
                        }
                        //use it to update db (not the calculator)
                        linkedViewElement = new XElement(analyzerLV);
                    }
                    else
                    {
                        //use the base linked view standard pattern
                        //avoids updating the wrong lvs
                        linkedViewElement = CalculatorHelpers.GetNewCalculator(calcParams, currentElement);
                    }
                }
                else if (string.IsNullOrEmpty(sCalculatorType)
                    && !string.IsNullOrEmpty(sAnalysisType))
                {
                    //analyzers can insert or update children analyzers
                    XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                        currentElement, Calculator1.cAnalyzerType, calcParams.AnalyzerParms.AnalyzerType);
                    if (analyzerLV != null)
                    {
                        if (calcParams.LinkedViewElement != null)
                        {
                            //keep the id and calculatorid, but update the rest of the atts with new lv
                            string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
                            string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
                            analyzerLV = new XElement(calcParams.LinkedViewElement);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
                        }
                        //use it to update db (not the calculator)
                        linkedViewElement = new XElement(analyzerLV);
                    }
                    else
                    {
                        //use the base linked view standard pattern
                        //avoids updating the wrong lvs
                        linkedViewElement = CalculatorHelpers.GetNewCalculator(calcParams, currentElement);
                    }
                }
            }
            //100% Rule 2: Analyzers and Calculators never, ever, allow descendent lvs 
            //to have parent Overwrite or UseSameCalc properties
            if (calcParams.StartingDocToCalcNodeName
                != currentElement.Name.LocalName)
            {
                if (linkedViewElement != null)
                {
                    string sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
                        Calculator1.cUseSameCalculator);
                    if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                    {
                        CalculatorHelpers.SetAttribute(linkedViewElement,
                            Calculator1.cUseSameCalculator, string.Empty);
                    }
                    sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
                        Calculator1.cOverwrite);
                    if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                    {
                        CalculatorHelpers.SetAttribute(linkedViewElement,
                            Calculator1.cOverwrite, string.Empty);
                    }
                }
            }
        }
        #endregion
    }
}
