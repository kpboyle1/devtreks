using DevTreks.Data.AppHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities used to support DevTreks 
    ///             standard addins and extensions.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        
    /// </summary>
    public class AddInHelper
    {
        public AddInHelper() { }
        //current DevTreks addin/extension hosts
        //(allows more fine-tuned initiation of host's addins and extensions)
        public enum HOSTS
        {
            none                        = 0,
            //extension calculator host
            extensioncalculatorsteps    = 1,
            //extension analyzer host
            extensionanalyzersteps      = 2
        }
        public enum SAVECALCS_METHOD
        {
            none        = 0,
            calcs       = 1,
            analyses    = 2,
            saveastext  = 3
        }
        

    #region "addin/extension state management"
        public static string GetStepNumber(ContentURI docToCalcURI)
        {
            string sLastStepNumber
                = GeneralHelpers.GetFormValue(docToCalcURI, General.STEP);
            if (string.IsNullOrEmpty(sLastStepNumber))
                sLastStepNumber = General.STEPZERO;
            return sLastStepNumber;
        }
        public static bool IsOkToRunExtension(ContentURI docToCalcURI)
        {
            bool bIsOkToRunExtension = true;
            string sStepNumber = GetStepNumber(docToCalcURI);
            if (sStepNumber == General.STEPZERO
                || string.IsNullOrEmpty(sStepNumber))
            {
                //stepzero does nothing, so don't needlessly run the addin
                //display whatever was saved last
                bIsOkToRunExtension = false;
            }
            return bIsOkToRunExtension;
        }
        public static string GetDevTrekPath(ContentURI uri,
            GeneralHelpers.DOC_STATE_NUMBER displayDocType)
        {
            //displayhelper rules for getting the right file system document 
            //to display with uri
            string sDevTrekPath = string.Empty;
            //160 deprecated separate file storage for guests
            uri.URIMember.MemberDocFullPath = uri.URIClub.ClubDocFullPath;
            if (uri.URIDataManager.ServerActionType
                == GeneralHelpers.SERVER_ACTION_TYPES.select
                || uri.URIDataManager.ServerActionType
                == GeneralHelpers.SERVER_ACTION_TYPES.preview)
            {
                sDevTrekPath = uri.URIClub.ClubDocFullPath;
            }
            else
            {
                ContentURI addInURI = new ContentURI();
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                    == DevTreks.Models.AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    if (displayDocType
                        == GeneralHelpers.DOC_STATE_NUMBER.firstdoc)
                    {
                        sDevTrekPath = uri.URIClub.ClubDocFullPath;
                    }
                    else if (displayDocType
                        == GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                    {
                        addInURI =
                                LinqHelpers.GetLinkedViewIsSelectedAddIn(uri);
                        if (addInURI == null)
                        {
                            //init with the default 
                            addInURI =
                                LinqHelpers.GetLinkedViewIsDefaultAddIn(uri.URIDataManager.LinkedView);
                        }
                        if (addInURI != null)
                        {
                            if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                                addInURI.URIDataManager.TempDocPath) == true)
                            {
                                sDevTrekPath = addInURI.URIDataManager.TempDocPath;
                            }
                            else
                            {
                                sDevTrekPath = addInURI.URIClub.ClubDocFullPath;
                            }
                        }
                        else
                        {
                            if (uri.URINodeName 
                                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            {
                                //must be a real calcdocuri when useselectedview = true
                                if (uri.URIDataManager.UseSelectedLinkedView == false)
                                {
                                    //uri is the addin
                                    sDevTrekPath = uri.URIClub.ClubDocFullPath;
                                }
                            }
                        }
                    }
                    else if (displayDocType
                        == GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                    {
                        sDevTrekPath = GetDevTrekThirdDoc(uri, displayDocType);
                    }
                }
                else
                {
                    if (displayDocType
                        == GeneralHelpers.DOC_STATE_NUMBER.firstdoc)
                    {
                        sDevTrekPath = uri.URIClub.ClubDocFullPath;
                    }
                    else if (displayDocType
                        == GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                    {
                        addInURI =
                            LinqHelpers.GetLinkedViewIsSelectedAddIn(uri);
                        if (addInURI == null)
                        {
                            //init with the default 
                            addInURI =
                                LinqHelpers.GetLinkedViewIsDefaultAddIn(uri.URIDataManager.LinkedView);
                        }
                        if (addInURI != null)
                        {
                            if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                                addInURI.URIDataManager.TempDocPath) == true)
                            {
                                sDevTrekPath = addInURI.URIDataManager.TempDocPath;
                            }
                            else
                            {
                                sDevTrekPath = addInURI.URIMember.MemberDocFullPath;
                            }
                        }
                        else
                        {
                            if (uri.URINodeName
                                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            {
                                //must be a real calcdocuri when useselectedview = true
                                if (uri.URIDataManager.UseSelectedLinkedView == false)
                                {
                                    //uri is the addin
                                    sDevTrekPath = uri.URIClub.ClubDocFullPath;
                                }
                            }
                        }
                    }
                    else if (displayDocType
                        == GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                    {
                        sDevTrekPath = GetDevTrekThirdDoc(uri, displayDocType);
                    }
                }
            }
            return sDevTrekPath;
        }
        public static string GetDevTrekThirdDoc(ContentURI uri,
            GeneralHelpers.DOC_STATE_NUMBER displayDocType)
        {
            string sDevTrekPath = string.Empty;
            //160 deprecated separate file storage for guests
            uri.URIClub.ClubDocFullPath = uri.URIClub.ClubDocFullPath;
            if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                    == DevTreks.Models.AccountHelper.AUTHORIZATION_LEVELS.fulledits)
            {
                if (!uri.URIDataManager.UseSelectedLinkedView)
                {
                    if (Helpers.FileStorageIO.URIAbsoluteExists(uri, uri.URIDataManager.TempDocPath) == true)
                    {
                        sDevTrekPath = uri.URIDataManager.TempDocPath;
                    }
                    else
                    {
                        sDevTrekPath = uri.URIClub.ClubDocFullPath;
                    }
                }
                else
                {
                    ContentURI selectedViewURI =
                        LinqHelpers.GetLinkedViewIsSelectedView(uri);
                    if (selectedViewURI != null)
                    {
                        if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                            selectedViewURI.URIDataManager.TempDocPath) == true)
                        {
                            sDevTrekPath = selectedViewURI.URIDataManager.TempDocPath;
                        }
                        else
                        {
                            sDevTrekPath = selectedViewURI.URIClub.ClubDocFullPath;
                        }
                    }
                    else
                    {
                        if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                            uri.URIDataManager.TempDocPath) == true)
                        {
                            sDevTrekPath = uri.URIDataManager.TempDocPath;
                        }
                        else
                        {
                            sDevTrekPath = uri.URIClub.ClubDocFullPath;
                        }
                    }
                }
            }
            else
            {
                if (!uri.URIDataManager.UseSelectedLinkedView)
                {
                    if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                        uri.URIDataManager.TempDocPath) == true)
                    {
                        sDevTrekPath = uri.URIDataManager.TempDocPath;
                    }
                    else
                    {
                        sDevTrekPath = uri.URIClub.ClubDocFullPath;
                    }
                }
                else
                {
                    ContentURI selectedViewURI =
                        LinqHelpers.GetLinkedViewIsSelectedView(uri);
                    if (selectedViewURI != null)
                    {
                        if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                            selectedViewURI.URIDataManager.TempDocPath) == true)
                        {
                            sDevTrekPath = selectedViewURI.URIDataManager.TempDocPath;
                        }
                        else
                        {
                            sDevTrekPath = selectedViewURI.URIMember.MemberDocFullPath;
                        }
                    }
                    else
                    {
                        if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                            uri.URIDataManager.TempDocPath) == true)
                        {
                            sDevTrekPath = uri.URIDataManager.TempDocPath;
                        }
                        else
                        {
                            sDevTrekPath = uri.URIClub.ClubDocFullPath;
                        }
                    }
                }
            }
            return sDevTrekPath;
        }
        
        public static string GetAddInNodeQryFromParentNode(XPathNavigator navParentNode,
            string devDocAttName, string devDocAttValue)
        {
            string sDevDocQry = string.Empty;
            //input[@Id='123']/root/linkedview[@Id='123']
            sDevDocQry = EditHelpers.XmlIO.MakeLinkedViewQry(navParentNode.LocalName,
                Helpers.GeneralHelpers.AT_ID,
                navParentNode.GetAttribute(Calculator.cId, string.Empty),
                devDocAttValue);
            return sDevDocQry;
        }
        public static XPathNavigator GetNode(XmlDocument doc, string uriPattern)
        {
            string sQry = GetNodeQry(uriPattern);
            XPathNavigator navDocToCalcNode = DevTreks.Data.EditHelpers.XPathIO.GetElement(string.Empty,
                string.Empty, doc.CreateNavigator(), sQry);
            return navDocToCalcNode;
        }
    
        public static string GetNodeQry(string uriPattern)
        {
            string sId = ContentURI.GetURIPatternPart(uriPattern,
                    ContentURI.URIPATTERNPART.id);
            string sNodeName = ContentURI.GetURIPatternPart(uriPattern,
                    ContentURI.URIPATTERNPART.node);
            string sCalcNodeQry = string.Empty;
            if (string.IsNullOrEmpty(sId) == false)
            {
                sCalcNodeQry = DevTreks.Data.EditHelpers.XmlIO.MakeXPathAbbreviatedQry(
                    sNodeName, Helpers.GeneralHelpers.AT_ID, sId);
            }
            return sCalcNodeQry;
        }
    #endregion

    #region "analyzers management"
        public IList<string> FillURIsToAnalyzeList(ContentURI docToCalcURI,
            ContentURI calcDocURI, ref IList<string> urisToAnalyze)
        {
            string sErrorMsg = string.Empty;
            //multiple document analyses must verify file system doctocalcs
            //against db records for accuracy
            //the verification is made against subfolder paths, 
            //such as budgettimeperiod_5360,
            //which corresponds to a unique uri.urinodename_uri.uriid combo
            if (docToCalcURI.URIDataManager.AppType
                == GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                //may need new db query to get all descendent devpackparts
                FillDevPackURIsToAnalyzeList(docToCalcURI, calcDocURI, urisToAnalyze);
            }
            else
            {
                //descendent db uripatterns are found in summary docs which
                //are the default documents produced from running any base calculation
                //summary docs use calcdocuri.uripattern in filename
                //fileextensiontype is used to find base calcor and base summary doc
                string sFileExtensionType = GetFileExtensionType(docToCalcURI, 
                    calcDocURI, ref sErrorMsg);
                if ((string.IsNullOrEmpty(sErrorMsg)))
                {
                    if (docToCalcURI.URIFileExtensionType
                        != Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        FillDbURIsToAnalyzeList(docToCalcURI, calcDocURI,
                            sFileExtensionType, ref urisToAnalyze, ref sErrorMsg);
                    }
                    else
                    {
                        //temp docs don't have subfolders or summary docs
                        //only the doctocalc full doc can be used in analysis
                        urisToAnalyze.Add(docToCalcURI.URIClub.ClubDocFullPath);
                    }
                }
                //else no error message; setting default anor attributes
            }
            if (!string.IsNullOrEmpty(sErrorMsg))
            {
                docToCalcURI.ErrorMessage = sErrorMsg;
            }
            return urisToAnalyze;
        }
        public static string GetFileExtensionType(ContentURI docToCalcURI,
            ContentURI calcDocURI, ref string errorMsg)
        {
            string sFileExtensionType = string.Empty;
            XmlDocument oCalcDoc = new XmlDocument();
            XmlReader reader = Helpers.FileStorageIO.GetXmlReader(calcDocURI,
                calcDocURI.URIDataManager.TempDocPath);
            if (reader != null)
            {
                using (reader)
                {
                    oCalcDoc.Load(reader);
                }
            }
            XPathNavigator navCalcDocNode = GetNode(
                oCalcDoc, calcDocURI.URIPattern);
            sFileExtensionType
                = GetFilesToAnalyzeExtensionType(docToCalcURI, 
                navCalcDocNode, ref errorMsg);
            if (string.IsNullOrEmpty(sFileExtensionType))
            {
                sFileExtensionType
                    = navCalcDocNode.GetAttribute(Calculator.cFileExtensionType, string.Empty);
                if (string.IsNullOrEmpty(sFileExtensionType))
                {
                    sFileExtensionType = calcDocURI.URIFileExtensionType;
                }
                if (sFileExtensionType == Helpers.GeneralHelpers.NONE
                    || string.IsNullOrEmpty(sFileExtensionType))
                {
                    errorMsg = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "ADDINHELPER_NOBASECALCS");
                }
            }
            return sFileExtensionType;
        }
        public static string GetFileExtensionType(ContentURI calcDocURI,
            ref string errorMsg)
        {
            string sFileExtensionType = string.Empty;
            if (Helpers.FileStorageIO.URIAbsoluteExists(calcDocURI,
                calcDocURI.URIDataManager.TempDocPath))
            {
                XmlDocument oCalcDoc = new XmlDocument();
                XmlReader reader = Helpers.FileStorageIO.GetXmlReader(calcDocURI,
                    calcDocURI.URIDataManager.TempDocPath);
                if (reader != null)
                {
                    using (reader)
                    {
                        oCalcDoc.Load(reader);
                    }
                }
                XPathNavigator navCalcDocNode = GetNode(
                    oCalcDoc, calcDocURI.URIPattern);
                sFileExtensionType
                    = GetFilesToAnalyzeExtensionType(calcDocURI,
                        navCalcDocNode, ref errorMsg);
                if (string.IsNullOrEmpty(sFileExtensionType))
                {
                    //the form els may not have been processed yet; 
                    sFileExtensionType = Helpers.GeneralHelpers.GetFormElementParam(
                        calcDocURI.URIDataManager.FormInput.ToString(), Calculator.cFilesToAnalyzeExtensionType,
                        string.Empty);
                    if (!string.IsNullOrEmpty(sFileExtensionType))
                    {
                        sFileExtensionType = sFileExtensionType.Replace("*", string.Empty);
                    }
                }
                if (string.IsNullOrEmpty(sFileExtensionType))
                {
                    sFileExtensionType
                        = navCalcDocNode.GetAttribute(Calculator.cFileExtensionType, string.Empty);
                    if (string.IsNullOrEmpty(sFileExtensionType))
                    {
                        sFileExtensionType = calcDocURI.URIFileExtensionType;
                    }
                    if (sFileExtensionType == Helpers.GeneralHelpers.NONE
                        || string.IsNullOrEmpty(sFileExtensionType))
                    {
                        errorMsg = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "ADDINHELPER_NOBASECALCS");
                    }
                }
            }
            return sFileExtensionType;
        }
        private static void FillDbURIsToAnalyzeList(ContentURI docToCalcURI,
            ContentURI calcDocURI, string fileExtensionType,
            ref IList<string> urisToAnalyze, ref string errorMsg)
        {
            string sBaseCalculatorDocURIPattern = GetBaseCalculatorDocURIPattern(docToCalcURI,
                fileExtensionType);
            FillSummaryDocURIs(docToCalcURI, calcDocURI,
                sBaseCalculatorDocURIPattern, ref urisToAnalyze, ref errorMsg);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                //some analyzers use the base db doc rather than a base calculated doc
                //i.e. no reason to run npv calcs on inputs or outputs
                AddBaseDBURIToAnalyzeList(docToCalcURI, fileExtensionType, 
                    ref urisToAnalyze, ref errorMsg);
            }
        }
        public static string GetBaseCalculatorDocURIPattern(ContentURI docToCalcURI,
            string fileExtensionType)
        {
            string sBaseCalculatorDocURIPattern = string.Empty;
            if (docToCalcURI.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in docToCalcURI.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (fileExtensionType == linkedview.URIFileExtensionType)
                        {
                            //this is a potential base calculator
                            sBaseCalculatorDocURIPattern = linkedview.URIPattern;
                            //only one base calculator is allowed per fileexttype
                            break;
                        }
                    }
                }
            }
            //version 165 -bug found when lv comes from different network
            //always use doctocalc network for file names
            if (!string.IsNullOrEmpty(sBaseCalculatorDocURIPattern))
            {
                string sCalcDocNetwork = ContentURI.GetURIPatternPart(sBaseCalculatorDocURIPattern, ContentURI.URIPATTERNPART.network);
                if (docToCalcURI.URINetworkPartName != sCalcDocNetwork)
                {
                    sBaseCalculatorDocURIPattern 
                        = ContentURI.ChangeURIPatternPart(sBaseCalculatorDocURIPattern, 
                        ContentURI.URIPATTERNPART.network, docToCalcURI.URINetworkPartName);
                }
            }
            return sBaseCalculatorDocURIPattern;
        }
        public static void FillSummaryDocURIs(
            ContentURI docToCalcURI, ContentURI calcDocURI, string summaryDocURIPattern, 
            ref IList<string> urisToAnalyze, ref string errorMsg)
        {
            if (!string.IsNullOrEmpty(summaryDocURIPattern))
            {
                string sCalcDocFileName = Path.GetFileNameWithoutExtension(calcDocURI.URIClub.ClubDocFullPath);
                string sSummaryDocPath = calcDocURI.URIClub.ClubDocFullPath.Replace(sCalcDocFileName,
                        Helpers.ContentHelper.MakeStandardFileNameFromURIPattern(summaryDocURIPattern));
                if (Helpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, 
                    sSummaryDocPath))
                {
                    //temp docs can only analyze one calculated fileim
                    urisToAnalyze.Add(sSummaryDocPath);
                    //implement when devpacks are used but not until then
                    //if (docToCalcURI.URIFileExtensionType
                    //    != Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    //{
                    //    string sSubFolderName = string.Empty;
                    //    string sCurrentNodeName = string.Empty;
                    //    string sAttName = string.Empty;
                    //    string sId = string.Empty;
                    //    XmlReader oSummaryDocReader = Helpers.FileStorageIO.GetXmlReader(sSummaryDocPath);
                    //    if (oSummaryDocReader != null)
                    //    {
                    //        using (oSummaryDocReader)
                    //        {
                    //            //read all of the nodes
                    //            while (oSummaryDocReader.Read())
                    //            {
                    //                if (oSummaryDocReader.HasAttributes)
                    //                {
                    //                    sCurrentNodeName = oSummaryDocReader.LocalName;
                    //                    while (oSummaryDocReader.MoveToNextAttribute())
                    //                    {
                    //                        sAttName = oSummaryDocReader.Name;
                    //                        if (sAttName.Equals(Calculator.cId))
                    //                        {
                    //                            sId = oSummaryDocReader.Value;
                    //                            break;
                    //                        }
                    //                    }
                    //                    if (!string.IsNullOrEmpty(sId)
                    //                        && (sCurrentNodeName
                    //                        != AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
                    //                    {
                    //                        sSubFolderName = GeneralHelpers.GetStandardFileDirectoryName(
                    //                            sCurrentNodeName, sId);
                    //                        if (urisToAnalyze.Contains(sSubFolderName)
                    //                            == false)
                    //                        {
                    //                            urisToAnalyze.Add(sSubFolderName);
                    //                        }
                    //                    }
                    //                    sId = string.Empty;
                    //                    sSubFolderName = string.Empty;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    //163 addition
                    //if the analyzer must use a basecalc, this may not be that base (i.e. was the LV.FileExt set correctly in LV table?)
                    if (Helpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, 
                        docToCalcURI.URIDataManager.TempDocPath))
                    {
                        urisToAnalyze.Add(docToCalcURI.URIDataManager.TempDocPath);
                    }
                    else
                    {
                        errorMsg = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "ADDIN_NOSUMMARYDOC");
                    }
                }
            }
            else
            {
                //163 addition
                if (Helpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                    docToCalcURI.URIDataManager.TempDocPath))
                {
                    urisToAnalyze.Add(docToCalcURI.URIDataManager.TempDocPath);
                }
                else
                {
                    errorMsg = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "ADDIN_NOSUMMARYDOC");
                }
            }
        }
        private static void AddBaseDBURIToAnalyzeList(ContentURI docToCalcURI,
            string fileExtensionType, ref IList<string> urisToAnalyze, ref string errorMsg)
        {
            //some analyzers use the base db doc rather than a base calculated doc
            //i.e. no reason to run npv calcs on inputs or outputs
            if (docToCalcURI.URIDataManager.SubAppType
                == GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
            {
                if (fileExtensionType == AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    //input and output analyzers don't require base npv calculations
                    //they can use the base db doc 
                    urisToAnalyze.Add(docToCalcURI.URIDataManager.TempDocPath);
                    errorMsg = string.Empty;
                }
            }
            else if (docToCalcURI.URIDataManager.SubAppType
                == GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
            {
                if (fileExtensionType == AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    //input and output analyzers don't require base npv calculations
                    //they can use the base db doc 
                    urisToAnalyze.Add(docToCalcURI.URIDataManager.TempDocPath);
                    errorMsg = string.Empty;
                }
            }
        }
        private static async Task FillDevPackURIsToAnalyzeList(ContentURI docToCalcURI,
            ContentURI calcDocURI, IList<string> urisToAnalyze)
        {
            string sSubFolderName = string.Empty;
            if (docToCalcURI.URINodeName == AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                //doctocalcuri is the only uritoanalyze
                sSubFolderName = string.Concat(docToCalcURI.URINodeName,
                    GeneralHelpers.FILENAME_DELIMITER, docToCalcURI.URIId);
                if (urisToAnalyze.Contains(sSubFolderName)
                    == false)
                {
                    urisToAnalyze.Add(sSubFolderName);
                }
            }
            else
            {
                bool bHasDevPackPart = false;
                if (docToCalcURI.URIDataManager.Children != null)
                {
                    //add children (i.e. subfolder analyses)
                    foreach (ContentURI child in docToCalcURI.URIDataManager.Children)
                    {
                        if (child.URIDataManager.AppType
                               == GeneralHelpers.APPLICATION_TYPES.devpacks)
                        {
                            sSubFolderName = string.Concat(child.URINodeName,
                                GeneralHelpers.FILENAME_DELIMITER, child.URIId);
                            if (!bHasDevPackPart)
                                bHasDevPackPart = (child.URINodeName 
                                    == AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                                    ? true : false;
                            if (urisToAnalyze.Contains(sSubFolderName)
                                == false)
                            {
                                urisToAnalyze.Add(sSubFolderName);
                            }
                            sSubFolderName = string.Empty;
                        }
                    }
                }
                //add linked views (terminal nodes)
                string sNodeName = string.Empty;
                string sId = string.Empty;
                if (docToCalcURI.URIDataManager.LinkedView != null)
                {
                    foreach (var linkedviewparent in docToCalcURI.URIDataManager.LinkedView)
                    {
                        foreach (ContentURI linkedview in linkedviewparent)
                        {
                            //only analyzes devpacks, not linkedviews
                            if (linkedview.URIDataManager.AppType
                                == GeneralHelpers.APPLICATION_TYPES.devpacks)
                            {
                                if (!bHasDevPackPart)
                                    bHasDevPackPart = (linkedview.URINodeName
                                        == AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                                        ? true : false;
                                //add the parent folder as well 
                                if (!string.IsNullOrEmpty(linkedview.URIDataManager.ParentURIPattern))
                                {
                                    sNodeName = ContentURI.GetURIPatternPart(
                                        linkedview.URIDataManager.ParentURIPattern, ContentURI.URIPATTERNPART.node);
                                    sId = ContentURI.GetURIPatternPart(
                                        linkedview.URIDataManager.ParentURIPattern, ContentURI.URIPATTERNPART.id);
                                    sSubFolderName = string.Concat(sNodeName,
                                        GeneralHelpers.FILENAME_DELIMITER, sId);
                                    if (urisToAnalyze.Contains(sSubFolderName)
                                        == false)
                                    {
                                        urisToAnalyze.Add(sSubFolderName);
                                    }
                                }
                                //add the children
                                sSubFolderName = string.Concat(linkedview.URINodeName,
                                    GeneralHelpers.FILENAME_DELIMITER, linkedview.URIId);
                                if (urisToAnalyze.Contains(sSubFolderName)
                                    == false)
                                {
                                    urisToAnalyze.Add(sSubFolderName);
                                }
                                sSubFolderName = string.Empty;
                            }
                        }
                    }
                }
                if (bHasDevPackPart == false)
                {
                    //set properties specific to this query
                    bool bUseDefaultAddIn = docToCalcURI.URIDataManager.UseDefaultAddIn;
                    int iPageSize = docToCalcURI.URIDataManager.PageSize;
                    docToCalcURI.URIDataManager.UseDefaultAddIn = false;
                    SqlRepositories.ContentRepository rep = new SqlRepositories.ContentRepository(docToCalcURI);
                    //recursive or group node requires a db hit to get all of the descendent devpackparts
                    IEnumerable<System.Linq.IGrouping<int, ContentURI>> groupedURIs =
                        await rep.GetLinkedViewForAnalysesAsync(docToCalcURI);
                    //put properties back in
                    docToCalcURI.URIDataManager.UseDefaultAddIn = bUseDefaultAddIn;
                    docToCalcURI.URIDataManager.PageSize = iPageSize;
                    foreach (var linkedviewparent in groupedURIs)
                    {
                        foreach (ContentURI linkedview in linkedviewparent)
                        {
                            //only analyzes devpacks, not linkedviews
                            if (linkedview.URIDataManager.AppType
                                == GeneralHelpers.APPLICATION_TYPES.devpacks)
                            {
                                sSubFolderName = string.Concat(linkedview.URINodeName,
                                    GeneralHelpers.FILENAME_DELIMITER, linkedview.URIId);
                                if (urisToAnalyze.Contains(sSubFolderName)
                                    == false)
                                {
                                    urisToAnalyze.Add(sSubFolderName);
                                }
                                sSubFolderName = string.Empty;
                            }
                        }
                    }
                    
                }
            }
            if (urisToAnalyze.Count == 0)
            {
                docToCalcURI.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "ADDIN_NOANALYSISLIST");
            }
        }
        public static string GetFilesToAnalyzeExtensionType(ContentURI uri,
            XPathNavigator navCalcDocNode, ref string errorMsg)
        {
            string sFilesToAnalyzeExtensionType = string.Empty;
            sFilesToAnalyzeExtensionType
                = navCalcDocNode.GetAttribute(Calculator.cFilesToAnalyzeExtensionType, string.Empty);
            if (string.IsNullOrEmpty(sFilesToAnalyzeExtensionType))
            {
                errorMsg = DevTreks.Exceptions.DevTreksErrors.GetMessage("EDIT_CHOOSEFILE");
            }
            return sFilesToAnalyzeExtensionType;
        }
        public static void GetFirstSubFolderFiles(ContentURI uri,
            IList<string> urisToAnalyze,
            string docToCalcPath, string fileExtension,
            ref IDictionary<string, string> fileOrFolderPaths)
        {
            FileStorageIO.PLATFORM_TYPES platform
                = FileStorageIO.GetPlatformType(uri);
            if (platform == FileStorageIO.PLATFORM_TYPES.webserver)
            {
                GetFirstSubFolderWebServerFiles(uri, urisToAnalyze,
                    docToCalcPath, fileExtension, ref fileOrFolderPaths);
            }
            else if (platform == FileStorageIO.PLATFORM_TYPES.azure)
            {
                AzureIOAsync azureIO = new AzureIOAsync(uri);
                azureIO.GetFirstSubFolderAzureFiles(
                    uri, urisToAnalyze,
                    docToCalcPath, fileExtension, ref fileOrFolderPaths);
            }
        }
        public static void GetFirstSubFolderWebServerFiles(ContentURI uri,
            IList<string> urisToAnalyze, string docToCalcPath, string fileExtension, 
            ref IDictionary<string, string> fileOrFolderPaths)
        {
            DirectoryInfo dir = null;
            if (Helpers.FileStorageIO.DirectoryExists(uri, docToCalcPath))
            {
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
                DirectoryInfo[] dirs = dir.GetDirectories();
                int i = 0;
                foreach (DirectoryInfo folder in dirs)
                {
                    //verify this folder exists in db
                    if (urisToAnalyze.Contains(folder.Name))
                    {
                        //188 doesn't require new calcs for inputs or outputs
                        if (fileExtension.Contains(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                            || fileExtension.Contains(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
                        {
                            Helpers.XmlFileIO.AddNewestIOFile(
                                uri, dir, ref i, ref fileOrFolderPaths);
                        }
                        else
                        {
                            XmlFileIO.AddNewestFileWithFileExtension(
                                uri, folder, fileExtension,
                                ref i, ref fileOrFolderPaths);
                        }
                    }
                }
            }
        }
        public static void DeleteOldAddInFiles(ContentURI uri,
            string docToCalcPath, StringDictionary colDeletes)
        {
            if (colDeletes != null)
            {
                //delete the corresponding filesystem files to prevent 
                //them from being orphans
                //orphans have the potential to interfere with filesystem-based analyses
                string sAction = string.Empty;
                string sURIdeletedURIPattern = string.Empty;
                foreach (string sKeyName in colDeletes.Keys)
                {
                    //1. sKeyName is a 'searchname;delete' delimited string
                    String[] arrDeleteParams = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                    if (arrDeleteParams.Length > 1)
                    {
                        sAction = string.Empty;
                        sAction = arrDeleteParams[1];
                        if (sAction.ToLower().Equals(EditHelpers.EditHelper.DELETE))
                        {
                            sURIdeletedURIPattern = string.Empty;
                            sURIdeletedURIPattern = arrDeleteParams[0];
                            FileStorageIO.PLATFORM_TYPES platform
                                = FileStorageIO.GetPlatformType(uri);
                            if (platform == FileStorageIO.PLATFORM_TYPES.webserver)
                            {
                                DeleteOldAddInWebServerFiles(uri, 
                                    docToCalcPath, sURIdeletedURIPattern);
                            }
                            else if (platform ==FileStorageIO.PLATFORM_TYPES.azure)
                            {
                                AzureIOAsync azureIO = new AzureIOAsync(uri);
                                azureIO.DeleteOldAddInAzureFiles(uri,
                                    docToCalcPath, sURIdeletedURIPattern);
                            }
                        }
                    }
                }
            }
        }
        public static void DeleteOldAddInWebServerFiles(ContentURI uri,
            string docToCalcPath, string URIdeletedURIPattern)
        {
            string sURIdeletedId = ContentURI.GetURIPatternPart(
                URIdeletedURIPattern, ContentURI.URIPATTERNPART.id);
            string sFileToDeleteId
                = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
                sURIdeletedId, Helpers.GeneralHelpers.FILENAME_DELIMITER);
            DirectoryInfo dir = null;
            if (Helpers.FileStorageIO.DirectoryExists(uri, docToCalcPath))
            {
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
                FileInfo[] files = dir.GetFiles();
                List<string> filesToDelete = new List<string>();
                foreach (FileInfo file in files)
                {
                    if (!file.FullName.Equals(docToCalcPath))
                    {
                        if (file.Name.Contains(sFileToDeleteId))
                        {
                            filesToDelete.Add(file.FullName);
                        }
                    }
                }
                foreach (string filetodelete in filesToDelete)
                {
                    FileStorageIO.DeleteURI(uri, filetodelete);
                }
            }
        }
        public static void DeleteOldAddInHtmlFiles(ContentURI uri, 
            string docToCalcPath, string URIdeletedURIPattern, 
            bool includeCalcDocs)
        {
            FileStorageIO.PLATFORM_TYPES platform
                = FileStorageIO.GetPlatformType(uri);
            if (platform == FileStorageIO.PLATFORM_TYPES.webserver)
            {
                DeleteOldAddInHtmlWebServerFiles(uri, docToCalcPath, URIdeletedURIPattern, includeCalcDocs);
            }
            else if (platform == FileStorageIO.PLATFORM_TYPES.azure)
            {
                AzureIOAsync azureIO = new AzureIOAsync(uri);
                azureIO.DeleteOldAddInHtmlAzureFiles(uri, docToCalcPath, 
                    URIdeletedURIPattern, includeCalcDocs);
            }
        }
        public static void DeleteOldAddInHtmlWebServerFiles(ContentURI uri,
            string docToCalcPath, string URIdeletedURIPattern, bool includeCalcDocs)
        {
            string sURIdeletedId = ContentURI.GetURIPatternPart(
                URIdeletedURIPattern, ContentURI.URIPATTERNPART.id);
            string sFileToDeleteId
                = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
                sURIdeletedId, Helpers.GeneralHelpers.FILENAME_DELIMITER);
            DirectoryInfo dir = null;
            if (Helpers.FileStorageIO.DirectoryExists(uri, docToCalcPath))
            {
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
                FileInfo[] files = dir.GetFiles();
                List<string> filesToDelete = new List<string>();
                foreach (FileInfo file in files)
                {
                    if (!file.FullName.Equals(docToCalcPath))
                    {
                        if (file.Name.Contains(sFileToDeleteId))
                        {
                            if (file.Name.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.frag.ToString())
                                || file.Name.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.html.ToString()))
                            {
                                if (includeCalcDocs == false)
                                {
                                    //addins have small cap addin word
                                    string sAddIn = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
                                        Data.AppHelpers.AddIns.ADDIN_TYPES.addin.ToString());
                                    if (!file.Name.Contains(sAddIn))
                                    {
                                        filesToDelete.Add(file.FullName);
                                    }
                                }
                                else
                                {
                                    filesToDelete.Add(file.FullName);
                                }
                            }
                        }
                    }
                }
                foreach (string filetodelete in filesToDelete)
                {
                    FileStorageIO.DeleteURI(uri, filetodelete);
                }
            }
        }
    #endregion
    #region "set calculator updates state"
        public static void AddToDbList(string uriPattern,
            string attName, string attValue, string dataType,
            string stepNumber, IDictionary<string, string> updates)
        {
            if (attValue != string.Empty
                && uriPattern != string.Empty
                && updates != null)
            {
                string sDbKeyName = EditHelpers.EditHelper.GetDbKeyNameForAddIn(uriPattern,
                    attName, dataType, stepNumber);
                if (updates.ContainsKey(sDbKeyName) == false)
                {
                    updates.Add(sDbKeyName, attValue);
                }
                else
                {
                    //overwrite the value of the existing list item
                    updates[sDbKeyName] = attValue;
                }
            }
        }
        public static void RemoveFromDbList(string uriPattern,
            string attName, string attValue, string dataType,
            string stepNumber, IDictionary<string, string> updates)
        {
            //note that addtodblist also overwrites existing keys
            if (attName != string.Empty
                && updates != null)
            {
                string sDbKeyName = EditHelpers.EditHelper.GetDbKeyNameForAddIn(uriPattern,
                    attName, dataType, stepNumber);
                if (updates.ContainsKey(sDbKeyName) == true)
                {
                    updates.Remove(sDbKeyName);
                }
            }
        }
        public static void AddXmlDocAttributeForDbOnly(bool needsUpdateList,
            string docToCalcURIPattern, string stepNumber,
            IDictionary<string, string> updates, ref string errorMsg)
        {
            string sAttValue = Helpers.GeneralHelpers.ROOT_PATH;
            if (needsUpdateList) AddToDbList(docToCalcURIPattern,
                Helpers.GeneralHelpers.ROOT_PATH,
                sAttValue, RuleHelpers.GeneralRules.XML, 
                stepNumber, updates);
        }
        
        private static void SetDescendentXmlDocAttribute(string docToCalcURIPattern,
            XPathNavigator navCalcNode, XPathNavigator navDocToCalcNode,
            ref string errorMsg)
        {
            if (navDocToCalcNode.CanEdit)
            {
                //replace the current xmldoc in navCalcNode into navDocToCalcNode
                bool bIsReplaced = EditHelpers.XPathIO.ReplaceXmlDocElement(
                    navDocToCalcNode, string.Empty, navCalcNode);
            }
        }
  
        public static string NoFormattingOnly(string newAttValue, string oldValue,
            string dataType)
        {
            string sAttValue = newAttValue;
            string sNew = string.Empty;
            string sOld = string.Empty;
            if (dataType == RuleHelpers.GeneralRules.FLOAT)
            {
                sNew = Helpers.GeneralHelpers.ConvertStringToFloat(newAttValue).ToString("f3");
                sOld = Helpers.GeneralHelpers.ConvertStringToFloat(oldValue).ToString("f3");
                if (sNew == sOld)
                {
                    sAttValue = string.Empty;
                }
                else
                {
                    sAttValue = sNew;
                }
            }
            else if (dataType == RuleHelpers.GeneralRules.DOUBLE)
            {
                //number, N, can not be used as formatter due to comma in 1,000
                //fixed-point is used instead
                sNew = Helpers.GeneralHelpers.ConvertStringToDouble(newAttValue).ToString("f3");
                sOld = Helpers.GeneralHelpers.ConvertStringToDouble(oldValue).ToString("f3");
                if (sNew == sOld)
                {
                    sAttValue = string.Empty;
                }
                else
                {
                    sAttValue = sNew;
                }
            } 
            else if (dataType == RuleHelpers.GeneralRules.DOUBLE2)
            {
                //number, N, can not be used as formatter due to comma in 1,000
                //fixed-point is used instead
                sNew = Helpers.GeneralHelpers.ConvertStringToDouble(newAttValue).ToString("f2");
                sOld = Helpers.GeneralHelpers.ConvertStringToDouble(oldValue).ToString("f2");
                if (sNew == sOld)
                {
                    sAttValue = string.Empty;
                }
                else
                {
                    sAttValue = sNew;
                }
            }
            else if (dataType == RuleHelpers.GeneralRules.DOUBLE4)
            {
                //number, N, can not be used as formatter due to comma in 1,000
                //fixed-point is used instead
                sNew = Helpers.GeneralHelpers.ConvertStringToDouble(newAttValue).ToString("f4");
                sOld = Helpers.GeneralHelpers.ConvertStringToDouble(oldValue).ToString("f4");
                if (sNew == sOld)
                {
                    sAttValue = string.Empty;
                }
                else
                {
                    sAttValue = sNew;
                }
            }
            else if (dataType == RuleHelpers.GeneralRules.DECIMAL)
            {
                //number, N, or currency, C, can not be used as formatter due to comma in 1,000
                //fixed-point is used instead
                sNew = Helpers.GeneralHelpers.ConvertStringToDecimal(newAttValue).ToString("f2");
                sOld = Helpers.GeneralHelpers.ConvertStringToDecimal(oldValue).ToString("f2");
                if (sNew == sOld)
                {
                    sAttValue = string.Empty;
                }
                else
                {
                    sAttValue = sNew;
                }
            }
            else if (dataType == RuleHelpers.GeneralRules.INTEGER)
            {
                int iNew = Helpers.GeneralHelpers.ConvertStringToInt(newAttValue);
                int iOld = Helpers.GeneralHelpers.ConvertStringToInt(oldValue);
                if (iNew == iOld) sAttValue = string.Empty;
            }
            else if (dataType == RuleHelpers.GeneralRules.DATE)
            {
                //use iso860 sortable dates
                sNew = Helpers.GeneralHelpers.ConvertStringToDate(newAttValue).ToString("s");
                sOld = Helpers.GeneralHelpers.ConvertStringToDate(oldValue).ToString("s");
                if (sNew == sOld)
                {
                    sAttValue = string.Empty;
                }
                else
                {
                    sAttValue = sNew;
                }
            }
            else if (dataType == RuleHelpers.GeneralRules.BOOLEAN)
            {
                //sqlserver uses 1 = true and 0 = false (don't change it)
                sNew = Helpers.GeneralHelpers.ConvertStringToBool(newAttValue).ToString();
                sOld = Helpers.GeneralHelpers.ConvertStringToBool(oldValue).ToString();
                if (sNew == sOld)
                {
                    sAttValue = string.Empty;
                }
                else
                {
                    sAttValue = sNew;
                }
            }
            return sAttValue;
        }
        public static bool IsAddInOrExtensionName(string attName)
        {
            bool bIsAddInOrExtensionName = false;
            int iIndex = attName.ToLower().IndexOf(General.HOST_ATTNAME.ToLower());
            if (iIndex != -1)
            {
                bIsAddInOrExtensionName = true;
            }
            else
            {
                iIndex = attName.ToLower().IndexOf(General.ADD_IN_ATTNAME.ToLower());
                if (iIndex != -1)
                {
                    bIsAddInOrExtensionName = true;
                }
            }
            return bIsAddInOrExtensionName;
        }
        public static bool IsAddIn(ContentURI linkedView)
        {
            bool bIsAddIn = false;
            bIsAddIn = IsAddIn(linkedView.URIDataManager.AddInName);
            if (bIsAddIn)
            {
                bIsAddIn = IsAddIn(linkedView.URIDataManager.HostName);
            }
            return bIsAddIn;
        }
        public static bool IsAddIn(string hostOrAddInName)
        {
            bool bIsAddIn = false;
            if ((!string.IsNullOrEmpty(hostOrAddInName))
                && (!hostOrAddInName.ToLower().EndsWith(GeneralHelpers.NONE.ToLower())))
            {
                bIsAddIn = true;
            }
            return bIsAddIn;
        }
        public static bool IsCalculatorAddIn(string hostOrAddInName)
        {
            bool bIsAddIn = false;
            if ((!string.IsNullOrEmpty(hostOrAddInName))
                && (!hostOrAddInName.ToLower().EndsWith(GeneralHelpers.NONE.ToLower())))
            {
                if (hostOrAddInName == AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
                {
                    bIsAddIn = true;
                }
            }
            return bIsAddIn;
        }
        public static bool CanRunBaseDoc(ContentURI docToCalcURI, string hostorAddinName)
        {
            bool bCanRun = false;
            if (IsCalculatorAddIn(hostorAddinName))
            {
                bCanRun = true;
            }
            else
            {
                if (docToCalcURI.URIDataManager.SubAppType 
                    == GeneralHelpers.SUBAPPLICATION_TYPES.inputprices
                    || docToCalcURI.URIDataManager.SubAppType 
                    == GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
                {
                    //input and output analyzers don't always need base calcs (i.e. npv calcs)
                    bCanRun = true;
                }
            }
            return bCanRun;
        }
    #endregion
        
    }
}
