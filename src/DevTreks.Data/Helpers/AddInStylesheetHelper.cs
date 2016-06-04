using System.Collections.Generic;
using DataAppHelpers = DevTreks.Data.AppHelpers;


namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		Help manage addin and extension stylesheets.
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    public class AddInStylesheetHelper
    {
        public AddInStylesheetHelper() { }
        
        public static void GetStylesheetParametersForHost(ContentURI docToCalcURI,
            ref IDictionary<string, string> styleParams)
        {
            //this host relies on unique steps
            //what parts of the calculator should be hidden or displayed?
            string sLastStepNumber = GeneralHelpers.GetFormValue(docToCalcURI, AppHelpers.General.STEP);
            if (string.IsNullOrEmpty(sLastStepNumber)) sLastStepNumber = string.Empty;
            styleParams.Add(AppHelpers.General.STEPLAST, sLastStepNumber);
        }
        public static bool NeedsLinkedLists(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            bool bNeedsList = false;
            if (calcDocURI.URIDataManager.HostName.ToLower() ==
                AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
            {
                //most need lists; if they don't there won't be any lists to retrieve anyway
                bNeedsList = true;
            }
            else if (calcDocURI.URIDataManager.HostName.ToLower() ==
                AddInHelper.HOSTS.extensionanalyzersteps.ToString())
            {
                //analyzers don't need linked lists (as of this version)
                bNeedsList = false;
            }
            return bNeedsList;
        }
        public static void SetStylesheetURIParams(ContentURI docToDisplayURI,
            ref ContentURI stylesheetURI, string resourceURLs, string stylesheetLabel)
        {
            string sResourcePath = string.Empty;
            string sResourceAlt = string.Empty;
            string sResourceURIPattern = string.Empty;
            DataAppHelpers.Resources.GetResourceIdsForResourceFilePaths(resourceURLs, 0,
                out sResourcePath, out sResourceAlt, out sResourceURIPattern);
            stylesheetURI = ContentURI.ConvertShortURIPattern(sResourceURIPattern, docToDisplayURI);
            stylesheetURI.URIDataManager.FileSystemPath = sResourcePath;
            stylesheetURI.URIDataManager.Description = sResourceAlt;
            stylesheetURI.URIDataManager.Label = stylesheetLabel;
        }
        private static ContentURI GetCalcDocStylesheetURI(ContentURI docToCalcURI)
        {
            ContentURI stylesheetURI = new ContentURI();
            ContentURI calcDocURI =
                LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI == null)
            {
                calcDocURI =
                    LinqHelpers.GetLinkedViewIsDefaultAddIn(
                    docToCalcURI.URIDataManager.LinkedView);
            }
            if (calcDocURI != null)
            {
                stylesheetURI
                    = LinqHelpers.GetContentURIListIsMainStylesheet(
                    calcDocURI.URIDataManager.Resource);
            }
            return stylesheetURI;
        }

        public static string GetViewsPanelStylesheetName(GeneralHelpers.DOC_STATE_NUMBER docState, ContentURI docToCalcURI,
            string stylesheetName)
        {
            string sStylesheetNameForView = stylesheetName;
            if (docState == GeneralHelpers.DOC_STATE_NUMBER.thirddoc
                    && docToCalcURI.URIDataManager.SubActionView != string.Empty
                    && docToCalcURI.URIDataManager.SubActionView != Helpers.GeneralHelpers.NONE
                    && stylesheetName.IndexOf(GeneralHelpers.EXTENSION_XSLT) != 0)
                {
                    //v1.1.1 uses multiple views 
                    GeneralHelpers.SUBACTION_VIEWS eSubActionView 
                        = GeneralHelpers.GetSubActionView(docToCalcURI.URIDataManager.SubActionView);
                    if (eSubActionView != GeneralHelpers.SUBACTION_VIEWS.none)
                    {
                        //pattern is to change the stylesheet's last filename char using enum
                        //Operation1.xslt (mobile) to Operation3.xslt (full)
                        int iStart = stylesheetName.IndexOf(GeneralHelpers.EXTENSION_XSLT) - 1;
                        string sSubActionViewInt = stylesheetName.Substring(iStart, 1);
                        if (!string.IsNullOrEmpty(sSubActionViewInt))
                        {
                            int iNewSSEnum = (int)eSubActionView;
                            sStylesheetNameForView = stylesheetName.Replace(
                                string.Concat(sSubActionViewInt, GeneralHelpers.EXTENSION_XSLT),
                                string.Concat(iNewSSEnum.ToString(), GeneralHelpers.EXTENSION_XSLT));

                        }
                    }
                }
            return sStylesheetNameForView;
        }
        public static void GetStaticStyleSheetPath(ContentURI uri,
            string languageFolder, out string relativeStyleSheetPath)
        {
            relativeStyleSheetPath = string.Empty;
            string sFile = string.Empty;
            string sFolder = string.Empty;
            uri.URIDataManager.ExtensionObjectNamespace =
                DataAppHelpers.Resources.DISPLAY_NAMESPACE_TYPES.displaydevpacks.ToString();
            switch (uri.URIDataManager.AppType)
            {
                case GeneralHelpers.APPLICATION_TYPES.accounts:
                    sFolder = "Accounts/";
                    sFile = "Account1.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.members:
                    sFolder = "Members/";
                    sFile = "Member1.xslt";
                     break;
                case GeneralHelpers.APPLICATION_TYPES.networks:
                    sFolder = "Networks/";
                    sFile = "Network1.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.locals:
                    sFolder = "Locals/";
                    sFile = "Locals.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.addins:
                    sFolder = "AddIns/";
                    sFile = "AddIns.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.agreements:
                    sFolder = "Contracts/";
                    sFile = "Agreements1.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.devpacks:
                    sFolder = "DevPack/";
                    sFile = "DevPack.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.linkedviews:
                    sFolder = "LinkedViews/";
                    sFile = "LinkedView.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.resources:
                    sFolder = "Resource/";
                    sFile = "Resource.xslt";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.prices:
                    sFolder = "Prices/";
                    if (uri.URIDataManager.SubAppType
                        == GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
                    {
                        sFile = "Component1.xslt";
                    }
                    else if (uri.URIDataManager.SubAppType
                        == GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
                    {
                        sFile = "Input1.xslt";
                    }
                    else if (uri.URIDataManager.SubAppType
                        == GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
                    {
                        sFile = "Operation1.xslt";
                    }
                    else if (uri.URIDataManager.SubAppType
                    == GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        sFile = "Outcome1.xslt";
                    }
                    else if (uri.URIDataManager.SubAppType
                        == GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
                    {
                        sFile = "Output1.xslt";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.economics1:
                    sFolder = "Economics1/";
                    if (uri.URIDataManager.SubAppType
                        == GeneralHelpers.SUBAPPLICATION_TYPES.budgets)
                    {
                        sFile = "Budgets1.xslt";
                    }
                    else if (uri.URIDataManager.SubAppType
                        == GeneralHelpers.SUBAPPLICATION_TYPES.investments)
                    {
                        sFile = "Investments1.xslt";
                    }
                    break;
                default:
                    break;
            }
            relativeStyleSheetPath = string.Concat(sFolder, languageFolder, sFile);
        }
        public static string StylesheetFullPath(ContentURI uri, string endPath)
        {
            string sFullPath = AppSettings.GetStylesheetFullPath(uri, endPath);
            return sFullPath;
        }
        public static void GetNetworkStylesheet(ContentURI uri,
            ref string fullStyleSheetPath)
        {
            //this doesn't do anything right now
            //but networks will be able to use their own, custom stylesheets
            string sNewExtension = string.Concat(GeneralHelpers.FILENAME_DELIMITER,
                uri.URINetwork.PKId, ".xslt");
            string sGuideFile = fullStyleSheetPath.Replace("1.xslt", sNewExtension);
            //this will be false until networks start building custom stylesheets
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, sGuideFile) == true)
            {
                fullStyleSheetPath = sGuideFile;
            }
        }
        
    }
}
