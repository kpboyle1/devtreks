using DevTreks.Data;
using DevTreks.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataEditHelpers = DevTreks.Data.EditHelpers;
using DataHelpers = DevTreks.Data.Helpers;

namespace DevTreks.Services.Helpers
{
    /// <summary>
    ///Purpose:		assist running DevTreks extensions
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///NOTES:       
    /// </summary>
    public class AddInRunHelper
    {
        public AddInRunHelper()
        {
        }
        
        public static bool IsOkToRunAddIn(ContentURI docToCalcURI,
           string selectedLinkedViewAddInHostName)
        {
            bool bIsOkToRunAddIn = true;
            if (DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString().Equals(
                selectedLinkedViewAddInHostName.ToLower())
                || DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(
                    selectedLinkedViewAddInHostName.ToLower()))
            {
                bIsOkToRunAddIn = DataHelpers.AddInHelper.IsOkToRunExtension(
                   docToCalcURI);
            }
            return bIsOkToRunAddIn;
        }
        public async Task<bool> RunAddInAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates,
            CancellationToken cancellationToken)
        {
            bool bStepIsDone = false;
            bool bHasSet = await contentService.SetAddInNamesAsync(calcDocURI);
            if (calcDocURI.URIDataManager == null)
                calcDocURI.URIDataManager = new ContentURI.DataManager();
            if (DataHelpers.AddInHelper.IsAddIn(calcDocURI.URIDataManager.AddInName) == true
                && DataHelpers.AddInHelper.IsAddIn(calcDocURI.URIDataManager.HostName) == true)
            {
                bStepIsDone = await RunAddInAsync(contentService, docToCalcURI, calcDocURI,
                    calcDocURI.URIDataManager.AddInName, calcDocURI.URIDataManager.HostName, updates, 
                    cancellationToken);
            }
            return bStepIsDone;
        }
        private async Task<bool> RunAddInAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI,
            string addInTypeName, string hostTypeName,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bStepIsDone = false;
            //set any properties needed to run the extension
            await SetCalculatorProperties(contentService, docToCalcURI, calcDocURI);
            IList<string> lstURIsToAnalyze
                = FillInURIsToAnalyzeList(docToCalcURI, calcDocURI, hostTypeName);
            if (docToCalcURI.ErrorMessage != string.Empty)
            {
                docToCalcURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    docToCalcURI.ErrorMessage, "ERROR_INTRO");
                return false;
            }
            if (hostTypeName.ToLower().Equals(DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
                || hostTypeName.ToLower().Equals(DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()))
            {
                
                //can progress be calculated on typical calculation??
                //IProgress<long> progress = new Progress<long>();
                DevTreks.Extensions.DoStepsHost oHost
                    = new DevTreks.Extensions.DoStepsHost();
                //host handles rest of extension
                bStepIsDone = await oHost.DoStepAsync(docToCalcURI, calcDocURI, addInTypeName,
                    lstURIsToAnalyze, updates, cancellationToken);
            }
            if (docToCalcURI.ErrorMessage != string.Empty)
            {
                docToCalcURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    docToCalcURI.ErrorMessage, "ERROR_INTRO");
            }
            return bStepIsDone;
        }
        private async Task SetCalculatorProperties(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI)
        {
            calcDocURI.URIDataManager.LinkedLists
                = await contentService.GetLinkedListsArrayAsync(docToCalcURI, 
                calcDocURI);
            //calculators can change this (i.e. tempdocs)
            docToCalcURI.URIDataManager.StartingDocToCalcURIPattern
                = GetNodeToCalcURIPattern(docToCalcURI);
        }
        private IList<string> FillInURIsToAnalyzeList(ContentURI docToCalcURI,
            ContentURI calcDocURI, string hostTypeName)
        {
            IList<string> lstURIsToAnalyze = new List<string>();
            bool bNeedsURIsToAnalyze = false;
            if (hostTypeName.ToLower().Equals(DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
                || hostTypeName.ToLower().Equals(DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()))
            {
                bNeedsURIsToAnalyze
                   = DevTreks.Extensions.DoStepsHost.NeedsURIsToAnalyze(
                        docToCalcURI, calcDocURI,
                        DevTreks.Data.Helpers.AddInHelper.SAVECALCS_METHOD.none.ToString());
            }
            if (bNeedsURIsToAnalyze)
            {
                GetURIsToAnalyze(docToCalcURI, calcDocURI,
                    ref lstURIsToAnalyze);
            }
            return lstURIsToAnalyze;
        }
        private static string GetNodeToCalcURIPattern(ContentURI docToCalcURI)
        {
            string sNodeToCalcURIPattern = docToCalcURI.URIPattern;
            if (!string.IsNullOrEmpty(
                docToCalcURI.URIDataManager.TempDocNodeToCalcURIPattern))
            {
                sNodeToCalcURIPattern 
                    = docToCalcURI.URIDataManager.TempDocNodeToCalcURIPattern;
            }
            return sNodeToCalcURIPattern;
        }

        public bool SetLinkedViewFileExtensionType(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates)
        {
            bool bHasSet = false;
            string sFileExtensionType = string.Empty;
            string sKeyName 
                = DataEditHelpers.EditHelper.GetDbKeyNameForAddInFromUpdates(calcDocURI.URIPattern,
                DataAppHelpers.Calculator.cFileExtensionType, 
                DevTreks.Data.RuleHelpers.GeneralRules.STRING, updates);
            updates.TryGetValue(sKeyName, out sFileExtensionType);
            if (!string.IsNullOrEmpty(sFileExtensionType))
            {
                bool bNeedsDbUpdate
                    = DataHelpers.GeneralHelpers.IsDbEdit(docToCalcURI);
                if (bNeedsDbUpdate)
                {
                    //this is a calcdocuri update (not the typical doctocalcuri update)
                    bHasSet = UpdateCalcDocURIFileExtension(contentService, docToCalcURI,
                        calcDocURI, sFileExtensionType, sKeyName, updates);
                }
            }
            return bHasSet;
        }
        
        private void GetURIsToAnalyze(ContentURI docToCalcURI,
            ContentURI calcDocURI, ref IList<string> lstURIsToAnalyze)
        {
            DataHelpers.AddInHelper oAddInHelper
                = new DataHelpers.AddInHelper();
            lstURIsToAnalyze
                = oAddInHelper.FillURIsToAnalyzeList(docToCalcURI, calcDocURI,
                    ref lstURIsToAnalyze);
        }
        private bool UpdateCalcDocURIFileExtension(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI, string fileExtensionType, 
            string keyToRemoveName, IDictionary<string, string> updates)
        {
            bool bHasSet = false;
            //160 refactored because this method was updating db unecessarily
            //remove prior to doctocalc db updates
            if (updates.ContainsKey(keyToRemoveName))
            {
                updates.Remove(keyToRemoveName);
            }
            return bHasSet;
        }
        private async Task<bool> UpdateCalcDocURIFileExtensionAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI, string fileExtensionType,
            string keyToRemoveName, IDictionary<string, string> updates)
        {
            bool bHasSet = false;
            string sOldURIPattern = calcDocURI.URIPattern;
            string sNewURIPattern
                = DataHelpers.GeneralHelpers.MakeURIPattern(calcDocURI.URIName,
                calcDocURI.URIDataManager.BaseId.ToString(), calcDocURI.URINetworkPartName,
                calcDocURI.URINodeName, fileExtensionType);
            ContentURI oCalcDocURI = new ContentURI(calcDocURI);
            oCalcDocURI.ChangeURIPattern(sNewURIPattern);
            //has to inherit security from doctocalc in order to update db
            oCalcDocURI.URIClub = docToCalcURI.URIClub;
            oCalcDocURI.URIMember = docToCalcURI.URIMember;
            //update in the database, using an editable doc
            StringDictionary colDeletes = new StringDictionary();
            IDictionary<string, string> lstFileExtUpdate
                = new Dictionary<string, string>();
            string sStepNumber = string.Empty;
            //add an edit parameter to update linkedview base table
            DataHelpers.AddInHelper.AddToDbList(oCalcDocURI.URIPattern,
                DataAppHelpers.General.LINKEDVIEW_FILE_EXTENSION_TYPE, fileExtensionType,
                DevTreks.Data.RuleHelpers.GeneralRules.STRING, sStepNumber,
                lstFileExtUpdate);
            //this is a calcdocuri update (not the typical doctocalcuri update)
            bHasSet = await contentService.UpdateContentAsync(oCalcDocURI,
                colDeletes, lstFileExtUpdate);
            //set the new doc paths
            //refactor: the existing doctocalcs with old fileext should be deleted as well
            DataHelpers.ContentHelper.UpdateDocToCalcPathsUsingNewFileExtensionType(
                docToCalcURI, calcDocURI, fileExtensionType);
            //remove prior to doctocalc db updates
            if (updates.ContainsKey(keyToRemoveName))
            {
                updates.Remove(keyToRemoveName);
            }
            //add the new file extension type to calcdocuri (savesummary uses)
            calcDocURI.URIFileExtensionType = fileExtensionType;
            calcDocURI.UpdateURIPattern();
            return bHasSet;
        }
        public async Task<bool> UpdateAddInStateAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates)
        {
            bool bIsSaved = false;
            bool bStillNeedsSave = false;
            if (docToCalcURI.URIMember.ClubInUse.PrivateAuthorizationLevel
                != AccountHelper.AUTHORIZATION_LEVELS.fulledits)
            {
                //anonymous users automatically save the calcs (tempdocs)
                //every time they are run
                return true;
            }
            if (docToCalcURI.URIFileExtensionType 
                == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                 bStillNeedsSave = true;
            }
            else
            {
                //childrenLinkedView are used to update descendant doctocalcs
                //(so that no further manipulation of those docs are required)
                IDictionary<string, string> childrenLinkedView = null;
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                    docToCalcURI.URIDataManager.TempDocPath))
                {
                    //v170 made devpack updates same as base elements
                    //if (docToCalcURI.URIDataManager.UseSelectedLinkedView == false)
                    //    && docToCalcURI.URIDataManager.AppType
                    //    != DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                    if (docToCalcURI.URIDataManager.UseSelectedLinkedView == false
                        || docToCalcURI.URIDataManager.AppType
                        == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                    {
                        //v172: delete any old xml or html view of the addin results prior to save
                        DeleteOldAddins(docToCalcURI, calcDocURI);
                        //use sps to insert lvs into db and insert new ids into doctocalc tempdoc
                        //generates new lvs for display in drop down lists
                        //updates EF xmldocs 
                        //this must be done before anything else (need the new doctocalc tempdoc)
                        childrenLinkedView
                            = await SaveDocToCalcParamsAsync(contentService,
                                docToCalcURI, calcDocURI, updates);
                        if (string.IsNullOrEmpty(docToCalcURI.ErrorMessage))
                        {
                            if (updates.Count > 0 || childrenLinkedView.Count > 0)
                            {
                                //160 tried running these using Task.WhenAll(tasks) but the files got saved in wrong paths
                                //make sure those updates are correct byref
                                if (updates.Count > 0)
                                {
                                    //updates anything remaining in updates (i.e. geocodes)
                                    //and makes good base doc
                                    await UpdateRemainingContentAsync(contentService, docToCalcURI, updates);
                                }
                                else
                                {
                                    //make base doc with good calcs (runs an EF contentService.SaveURIFirstDocAsync
                                    await SaveDocToCalcBaseDbDocAsync(contentService, docToCalcURI);
                                }
                                //this only uses the full tempdoc saved during SaveDocToCalcParamsAsync()
                                //save both summary and full views of the results
                                //note that this action means that further descendant 
                                //manipulation is often not needed (they already have 
                                //calculated results)
                                await SaveSummaryAndFullViewsAsync(docToCalcURI, calcDocURI,
                                    childrenLinkedView);
                                //save the temp calcdocfiles in calcdoc and doctocalc paths
                                CopyCalcDocs(docToCalcURI, calcDocURI);
                                bStillNeedsSave = SaveSummaryAndFullViews(docToCalcURI);
                            }
                        }
                    }
                    else
                    {
                        bool bSaveBaseDoc = false;
                        bool bHasSet = await SaveLinkedViewAsync(contentService, 
                            docToCalcURI, calcDocURI, updates, bSaveBaseDoc);
                        //save both summary and full views of the results
                        //note that this action means that further descendant 
                        //manipulation is often not needed (they already have 
                        //calculated results)
                        bStillNeedsSave = await SaveSummaryAndFullViewsAsync(docToCalcURI, calcDocURI,
                            childrenLinkedView);
                    }
                }
            }
            if (bStillNeedsSave)
            {
                CopyDocToCalcDocs(docToCalcURI, calcDocURI);
            }
            else
            {
                //doctocalcuri linkedview and childrenlinkedview html docs were 
                //deleted during SaveSummaryandFull to eliminate any old html views still around
                //save new doctocalc (and don't do anything with children -force dynamic creation)
                CopyDocToCalcHtmlDocs(docToCalcURI, calcDocURI);
            }
            //deprecated in 1.6
            //copy analyses intermediate data file here
            //CopyAnalysesObservations(docToCalcURI, calcDocURI);
            if (docToCalcURI.URIFileExtensionType !=
                DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //don't let the views display tempdocs after a save
                DataHelpers.FileStorageIO.DeleteURI(docToCalcURI, 
                    calcDocURI.URIDataManager.TempDocPath);
                calcDocURI.URIDataManager.TempDocPath = string.Empty;
                //delete the tempdoc folder
                DataHelpers.FileStorageIO.DeleteDirectory(docToCalcURI,
                    docToCalcURI.URIDataManager.TempDocPath, false);
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, 
                    docToCalcURI.URIDataManager.TempDocPath))
                    DataHelpers.FileStorageIO.DeleteURI(docToCalcURI, 
                        docToCalcURI.URIDataManager.TempDocPath);
                docToCalcURI.URIDataManager.TempDocPath = string.Empty;
            }
            bIsSaved = true;
            return bIsSaved;
        }
        private static void DeleteOldAddins(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            //i.e. naming conventions might change in future; but keep id (_63_) in lv file names
            //delete all old calculated files (html and xml)
            StringDictionary colDeletes = new StringDictionary();
            string sKeyName = string.Concat(calcDocURI.URIPattern,
                DataHelpers.GeneralHelpers.STRING_DELIMITER, DataEditHelpers.EditHelper.DELETE);
            colDeletes.Add(sKeyName, DataEditHelpers.EditHelper.DELETE);
            //this does not delete docToCalcURI.URIClub.ClubDocFullPath, but it will be subsequently overwritten
            DataHelpers.AddInHelper.DeleteOldAddInFiles(docToCalcURI,
                docToCalcURI.URIClub.ClubDocFullPath, colDeletes);
        }
                        
        private async Task<bool> SaveSummaryAndFullViewsAsync(ContentURI docToCalcURI,
            ContentURI calcDocURI, IDictionary<string, string> childrenLinkedView)
        {
            bool bStillNeedsSave = true;
            if (docToCalcURI.URIDataManager.NeedsSummaryView
                && docToCalcURI.URIDataManager.UseSelectedLinkedView == false)
            {
                //summary files are easier to load and view than full files
                //in addition, new children doctocalc calculations can be updated
                //without the need for updating each child individually
                if (docToCalcURI.URIDataManager.AppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.economics1)
                {
                    if (docToCalcURI.URINodeName != DataAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                        && docToCalcURI.URINodeName != DataAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        DataAppHelpers.Economics1 oEcon = new DataAppHelpers.Economics1();
                        await oEcon.SaveSummaryAndFullTotals(docToCalcURI, calcDocURI,
                            childrenLinkedView);
                        bStillNeedsSave = false;
                    }
                }
                else if (docToCalcURI.URIDataManager.AppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.prices)
                {
                    bool bNeedsSummaryPriceDocs
                        = DataAppHelpers.Prices.NeedsSummaryPriceDocs(docToCalcURI);
                    if (bNeedsSummaryPriceDocs)
                    {
                        DataAppHelpers.Prices oPrices = new DataAppHelpers.Prices();
                        await oPrices.SaveSummaryAndFullTotals(docToCalcURI, calcDocURI,
                            childrenLinkedView);
                        bStillNeedsSave = false;
                    }
                }
            }
            return bStillNeedsSave;
        }
        private bool SaveSummaryAndFullViews(ContentURI docToCalcURI)
        {
            bool bStillNeedsSave = true;
            if (docToCalcURI.URIDataManager.NeedsSummaryView
                && docToCalcURI.URIDataManager.UseSelectedLinkedView == false)
            {
                //summary files are easier to load and view than full files
                //in addition, new children doctocalc calculations can be updated
                //without the need for updating each child individually
                if (docToCalcURI.URIDataManager.AppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.economics1)
                {
                    if (docToCalcURI.URINodeName != DataAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                        && docToCalcURI.URINodeName != DataAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        bStillNeedsSave = false;
                    }
                }
                else if (docToCalcURI.URIDataManager.AppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.prices)
                {
                    bool bNeedsSummaryPriceDocs
                        = DataAppHelpers.Prices.NeedsSummaryPriceDocs(docToCalcURI);
                    if (bNeedsSummaryPriceDocs)
                    {
                        bStillNeedsSave = false;
                    }
                }
            }
            return bStillNeedsSave;
        }
        private static async Task<bool> SaveLinkedViewAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates, 
            bool replaceBaseDevDoc)
        {
            bool bHasSet = false;
            if (replaceBaseDevDoc)
            {
                //save doctocalc in base doctocalc fields
                XmlReader oDocToCalcDocReader
                    = await DataHelpers.FileStorageIO.GetXmlReaderAsync(
                        docToCalcURI, docToCalcURI.URIDataManager.TempDocPath);
                if (oDocToCalcDocReader != null)
                {
                    using (oDocToCalcDocReader)
                    {
                        //save in doctocalc base field
                        bool bIsMetaData = false;
                        string sFileName = Path.GetFileName(docToCalcURI.URIClub.ClubDocFullPath);
                        bHasSet = await contentService.SaveURISecondBaseDocAsync(docToCalcURI,
                            bIsMetaData, sFileName, oDocToCalcDocReader);
                    }
                }
            }
            string sXmlDocDbKey = DataEditHelpers.EditHelper.GetDbKeyNameForAddInFromUpdates(
                docToCalcURI.URIPattern, DataHelpers.GeneralHelpers.ROOT_PATH,
                DevTreks.Data.RuleHelpers.GeneralRules.XML, updates);
            bool bNeedsXmlDocUpdated = updates.ContainsKey(sXmlDocDbKey);
            if (bNeedsXmlDocUpdated)
            {
                //save calcDocURI calcs in doctocalc join fields
                XmlReader oCalcDocReader
                    = await DataHelpers.FileStorageIO.GetXmlReaderAsync(
                        docToCalcURI, calcDocURI.URIDataManager.TempDocPath);
                if (oCalcDocReader != null)
                {
                    using (oCalcDocReader)
                    {
                        //save in doctocalc join field
                        bHasSet = await contentService.SaveURISecondDocAsync(docToCalcURI, oCalcDocReader);
                    }
                    //save it in file system
                    DataHelpers.FileStorageIO.CopyURIs(docToCalcURI,
                        calcDocURI.URIDataManager.TempDocPath,
                        calcDocURI.URIClub.ClubDocFullPath);
                }
            }
            return bHasSet;
        }
        private async Task<bool> SaveCustomDocsAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates)
        {
            bool bHasSet = await SaveDevPackAsync(contentService, docToCalcURI, calcDocURI, updates);
            
            return bHasSet;
        }
        private async Task<bool> SaveDevPackAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates)
        {
            bool bHasSet = false;
            //move newly calculated descendants from updates to new dictionary
            IDictionary<string, string> customDocDescendantCalculations
                = new Dictionary<string, string>();
            customDocDescendantCalculations
                = MoveCustomDocDescendantCalculations(docToCalcURI, updates);
            IDictionary<string, string> childrenLinkedView = null;
            if (updates.Count > 0)
            {
                //fileextensiontype, xmldoc node, linkedviewbaseid,
                //and possibly, some atts in descendants 
                childrenLinkedView
                    = await SaveDocToCalcParamsAsync(contentService, 
                    docToCalcURI, calcDocURI, updates);
                if (updates.Count > 0 || childrenLinkedView.Count > 0)
                {
                    //make base doc with good calcs using async
                    bHasSet = await SaveDocToCalcBaseDbDocAsync(contentService, docToCalcURI);
                }
                //save calcs in file system
                DataHelpers.FileStorageIO.CopyURIs(docToCalcURI,
                    calcDocURI.URIDataManager.TempDocPath,
                    calcDocURI.URIClub.ClubDocFullPath);
            }
            else
            {
                bool bSaveBaseDoc = false;
                bHasSet = await SaveLinkedViewAsync(contentService, docToCalcURI,
                    calcDocURI, updates, bSaveBaseDoc);
            }
            //saved calculated files (devpackpart customdocs) in filesystem
            //and delete old calculated files
            SaveCustomDocDescendantCalculations(docToCalcURI,
                customDocDescendantCalculations, childrenLinkedView);
            return bHasSet;
        }
        private static IDictionary<string, string> MoveCustomDocDescendantCalculations(
            ContentURI uri, IDictionary<string, string> updates)
        {
            IDictionary<string, string> customDocDescendantCalculations
                = new Dictionary<string, string>();
            if (updates.Count > 0)
            {
                //check updates for newly calculated custom docs
                //updates contains no other types of filepaths
                foreach (KeyValuePair<string, string> kvp in updates)
                {
                    //key = calculatedocpath
                    //value = tempcalculatedocpath
                    //value must be rooted (temp calcs paths are rooted)
                    if (!kvp.Key.Contains(string.Concat(DataHelpers.GeneralHelpers.STRING_DELIMITER, 
                        DevTreks.Data.RuleHelpers.GeneralRules.XML)))
                    {
                        if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                            uri, kvp.Value))
                        {
                            if (!customDocDescendantCalculations.ContainsKey(kvp.Key))
                            {
                                customDocDescendantCalculations.Add(kvp);
                            }
                        }
                    }
                }
                if (customDocDescendantCalculations.Count > 0)
                {
                    //remove them from the db list of update
                    foreach (KeyValuePair<string, string> kvp in customDocDescendantCalculations)
                    {
                        updates.Remove(kvp.Key);
                    }
                }
            }
            return customDocDescendantCalculations;
        }
        private static void SaveCustomDocDescendantCalculations(ContentURI docToCalcURI,
            IDictionary<string, string> updates, IDictionary<string, string> childrenLinkedView)
        {
            //check updates for newly calculated custom docs
            //updates contains no other types of filepaths
            string sNewCalculatedDocPath = string.Empty;
            string sChildLinkedViewPattern = string.Empty;
            foreach (KeyValuePair<string, string> kvp in updates)
            {
                //key = calculatedocpath
                //value = tempcalculatedocpath
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, kvp.Value))
                {
                    sNewCalculatedDocPath = kvp.Key;
                    DataAppHelpers.LinkedViews oLinkedView 
                        = new DataAppHelpers.LinkedViews();
                    oLinkedView.GetLinkedViewPath(childrenLinkedView,
                       ref sNewCalculatedDocPath, ref sChildLinkedViewPattern);
                    //save it again in file system
                    DataHelpers.FileStorageIO.CopyURIs(docToCalcURI, kvp.Value,
                        sNewCalculatedDocPath);
                    //delete any old html views of the addin (won't get packaged incorrectly)
                    bool bIncludeCalcDocs = false;
                    DataHelpers.AddInHelper.DeleteOldAddInHtmlFiles(docToCalcURI, 
                        sNewCalculatedDocPath,
                        sChildLinkedViewPattern, bIncludeCalcDocs);
                }
            }
        }
        
        private static IDictionary<string, string> MakeXmlDocOnlyUpdateList(
            IDictionary<string, string> updates)
        {
            IDictionary<string, string> lstXmlDocUpdate = new Dictionary<string, string>();
            int iIndex = 0;
            foreach (KeyValuePair<string, string> kvp in updates)
            {
                iIndex = kvp.Key.IndexOf(DataHelpers.GeneralHelpers.ROOT_PATH);
                if (iIndex != -1)
                {
                    if (!lstXmlDocUpdate.ContainsKey(kvp.Key))
                    {
                        lstXmlDocUpdate.Add(kvp);
                    }
                }
            }
            return lstXmlDocUpdate;
        }
        //insert the lvs into db and update doctocalc tempdoc with new lvids
        //must be done before anything else
        private async Task<IDictionary<string, string>> SaveDocToCalcParamsAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates)
        {
            bool bHasSaved = false;
            IDictionary<string, string> childrenLinkedView
                = new Dictionary<string, string>();
            if (DataHelpers.GeneralHelpers.IsAdminApp(docToCalcURI.URIDataManager.AppType))
            {
                //admin apps don't have descendants needing linked views
                bHasSaved = true;
            }
            else
            {
                //this must be done before anything else -can't be run simultaneously
                bHasSaved = await AddDescendantLinkedViewToDbAsync(contentService,
                    docToCalcURI, calcDocURI, updates);
            }
            if (bHasSaved)
            {
                //xmldocs are updated using a separate update action (this uses EF and can be slow)
                bHasSaved = await UpdateLinkedViewXmlDocsInDbAsync(contentService,
                    docToCalcURI, calcDocURI, updates, childrenLinkedView);
            }
            else
            {
                docToCalcURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "CONTENTSERVICE_NOCALCSRUN2");
            }
            return childrenLinkedView;
        }
        
        private async Task<bool> SaveDocToCalcBaseDbDocAsync(IContentService contentService, ContentURI docToCalcURI)
        {
            bool bHasSet = false;
            //0.8.9: The base doctocalc, or 'first doc', does not have any of the 
            //new or updated linked views, meaning it can't be used for analyses
            //switch back to first doc and save a new first doc with new linked views
            string sAddInResultPath = docToCalcURI.URIClub.ClubDocFullPath;
            string sBaseDocToCalcFileName
                = DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(docToCalcURI);
            //switch from the AddInResultPath now being stored to base doctocalc path
            string sBaseDocToCalcFilePath = sAddInResultPath.Replace(
                Path.GetFileNameWithoutExtension(docToCalcURI.URIClub.ClubDocFullPath),
                sBaseDocToCalcFileName);
            //switch to it temporarily
            docToCalcURI.URIClub.ClubDocFullPath = sBaseDocToCalcFilePath;
            //save the new db generated doc (next run calcs will find all linked views)
            bHasSet = await contentService.SaveURIFirstDocAsync(docToCalcURI);
            //turn off hasnewxml flag so html generated for third doc
            docToCalcURI.URIDataManager.HasNewXml = false;
            //160 deprecated separate file storage for guests
            //switch back to addin result so that it displays properly
            docToCalcURI.URIClub.ClubDocFullPath = sAddInResultPath;
            docToCalcURI.URIMember.MemberDocFullPath = docToCalcURI.URIClub.ClubDocFullPath;
            return bHasSet;
        }
        private async Task<bool> UpdateRemainingContentAsync(IContentService contentService,
            ContentURI docToCalcURI, IDictionary<string, string> updates)
        {
            bool bHasSet = false;
            if (updates.Count > 0)
            {
                //0.8.9: The base doctocalc, or 'first doc', does not have any of the 
                //new or updated linked views, meaning it can't be used for analyses
                //switch back to first doc and save a new first doc with new linked views
                string sAddInResultPath = docToCalcURI.URIClub.ClubDocFullPath;
                string sBaseDocToCalcFileName
                    = DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(docToCalcURI);
                //switch from the AddInResultPath now being stored to base doctocalc path
                string sBaseDocToCalcFilePath = sAddInResultPath.Replace(
                    Path.GetFileNameWithoutExtension(docToCalcURI.URIClub.ClubDocFullPath),
                    sBaseDocToCalcFileName);
                //switch to it temporarily
                docToCalcURI.URIClub.ClubDocFullPath = sBaseDocToCalcFilePath;
                StringDictionary colDeletes = new StringDictionary();
                //updates geocodes ... and generates new base doc
                bHasSet = await contentService.UpdateContentAsync(docToCalcURI, colDeletes,
                    updates);
                //turn off hasnewxml flag so html generated for third doc
                docToCalcURI.URIDataManager.HasNewXml = false;
                //160 deprecated separate file storage for guests
                //switch back to addin result so that it displays properly
                docToCalcURI.URIClub.ClubDocFullPath = sAddInResultPath;
                docToCalcURI.URIMember.MemberDocFullPath = docToCalcURI.URIClub.ClubDocFullPath;
            }
            return bHasSet;
        }
        private async Task<bool> AddDescendantLinkedViewToDbAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates)
        {
            //note: doctocalc manipulation should be minimal - memory management is
            //better using addins/extensions to edit doctocalcs as much as possible
            bool bHasSaved = true;
            IDictionary<string, string> linkedViewsToInsert 
                = new Dictionary<string, string>();
            string sLinkedViewBaseURIPattern = ContentURI.ChangeURIPatternPart(
                calcDocURI.URIPattern, ContentURI.URIPATTERNPART.id, 
                calcDocURI.URIDataManager.BaseId.ToString());
            string sKeyToRemoveName = string.Empty;
            IList<string> keysToRemove = new List<string>();
            string sKeyName = string.Empty;
            string sKeyURIPattern = string.Empty;
            string sAttName = string.Empty;
            string sDataType = string.Empty;
            string sStep = string.Empty;
            IDictionary<string, string> newLinkedViewInserted =
                new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in updates)
            {
                sKeyName = kvp.Key;
                DataEditHelpers.EditHelper.GetStandardEditNameParams(
                    sKeyName.Split(DataHelpers.GeneralHelpers.STRING_DELIMITERS),
                    out sKeyURIPattern, out sAttName, out sDataType, out sStep);
                if (sAttName == DataAppHelpers.LinkedViews.LINKEDVIEWBASEID)
                {
                    if (!sKeyURIPattern.Equals(docToCalcURI.URIPattern))
                    {
                        //descendants and siblings can have calcors inserted
                        if (!keysToRemove.Contains(sKeyName))
                        {
                            keysToRemove.Add(sKeyName);
                        }
                        if (!linkedViewsToInsert.ContainsKey(sKeyURIPattern))
                        {
                            //note that the unique key here is the linkingview node, unlike 
                            //addselectlinkedviews where the unique key is the linkeview node
                            linkedViewsToInsert.Add(sKeyURIPattern, sLinkedViewBaseURIPattern);
                        }
                    }
                    else
                    {
                        if (!keysToRemove.Contains(sKeyName))
                        {
                            //doctocalcuri linkedview is only entered using the usedefaults
                            keysToRemove.Add(sKeyName);
                        }
                    }
                }
            }
            //remove the insertions from updates
            foreach (string keyToRemove in keysToRemove)
            {
                if (updates.ContainsKey(keyToRemove))
                {
                    updates.Remove(keyToRemove);
                }
            }
            if (linkedViewsToInsert.Count > 0)
            {
                bool isSelections = false;
                string sInsertedIdsArray = string.Empty;
                //linkedViewsToInsert will remove keys as they are added (recursive sp)
                IDictionary<string, string> linkedViewsToInsert2
                    = DataHelpers.LinqHelpers.CopyDictionary(linkedViewsToInsert);
                //insert the lvs into the db
                sInsertedIdsArray = await contentService.AddLinkedViewAsync(docToCalcURI,
                    linkedViewsToInsert, isSelections);
                if (bHasSaved && !string.IsNullOrEmpty(sInsertedIdsArray))
                {
                    //note: this also sets a new linkedviews collection with newly inserted ids
                    DataAppHelpers.LinkedViews oLinkedView = new DataAppHelpers.LinkedViews();
                    oLinkedView.GetNewLinkedView(linkedViewsToInsert2, sInsertedIdsArray,
                        ref newLinkedViewInserted);
                    if (newLinkedViewInserted.Count > 0)
                    {
                        //an alternative to using newLinkedViewInserted is to use
                        //the new linkedview collection
                        await UpdateXmlDocsInUpdatesAsync(calcDocURI, docToCalcURI,
                            newLinkedViewInserted, updates);
                    }
                }
            }
            return bHasSaved;
        }
        private static async Task UpdateXmlDocsInUpdatesAsync(ContentURI calcDocURI,
            ContentURI docToCalcURI, IDictionary<string, string> newLinkedViewInserted,
            IDictionary<string, string> updates)
        {
            IDictionary<string, string> keysToUpdate 
                = new Dictionary<string, string>();
            string sNewInsertLinkingURIPattern = string.Empty;
            string sNewInsertLinkedViewURIPattern = string.Empty;
            string sNewInsertId = string.Empty;
            string sKeyName = string.Empty;
            string sKeyURIPattern = string.Empty;
            string sAttName = string.Empty;
            string sDataType = string.Empty;
            string sStep = string.Empty;
            string sDbKeyName = string.Empty;
            foreach (KeyValuePair<string, string> kvp in updates)
            {
                sKeyName = kvp.Key;
                DataEditHelpers.EditHelper.GetStandardEditNameParams(
                    sKeyName.Split(DataHelpers.GeneralHelpers.STRING_DELIMITERS),
                    out sKeyURIPattern, out sAttName, out sDataType, out sStep);
                if (sAttName == DataHelpers.GeneralHelpers.ROOT_PATH)
                {
                    foreach (KeyValuePair<string, string> jvp in newLinkedViewInserted)
                    {
                        sNewInsertLinkingURIPattern = jvp.Key;
                        if (sKeyURIPattern == sNewInsertLinkingURIPattern)
                        {
                            sNewInsertLinkedViewURIPattern = jvp.Value;
                            sNewInsertId = ContentURI.GetURIPatternPart(
                                sNewInsertLinkedViewURIPattern, ContentURI.URIPATTERNPART.id);
                            if (!keysToUpdate.ContainsKey(sKeyName))
                            {
                                keysToUpdate.Add(sKeyName, sNewInsertId);
                            }
                        }
                    }
                }
            }
            //update the xmldocs before inserting them
            foreach (KeyValuePair<string, string> keyToUpdate in keysToUpdate)
            {
                sKeyName = keyToUpdate.Key;
                if (updates.ContainsKey(sKeyName))
                {
                    XElement linkedViewsDoc
                        = XElement.Parse(updates[sKeyName]);
                    XElement linkedViewDoc
                        = DataEditHelpers.XmlLinq.GetChildLinkedView(linkedViewsDoc,
                        calcDocURI.URIId.ToString());
                    if (linkedViewDoc != null)
                    {
                        sNewInsertId = keyToUpdate.Value;
                        //update the xmldoc being inserted into db with correct join id
                        //this byrefs to parent linkedViewsDoc
                        linkedViewDoc.SetAttributeValue(
                             DataAppHelpers.Calculator.cId, sNewInsertId);
                        //set the CalculatorId too (its actually a temp id)
                        linkedViewDoc.SetAttributeValue(
                            DataAppHelpers.Calculator.cCalculatorId, sNewInsertId);
                        //increase reliability of linked views by ensuring that this xmldoc 
                        //has one unique linkedview element with this id (delete siblings)
                        DataEditHelpers.XmlLinq.RemoveSiblingLinkedView(linkedViewsDoc, sNewInsertId);
                        //this means that the SaveFullAndSummaryDocs must insert new doctocalcs 
                        //with these new xmldocs (descendants don't need separate action)
                        updates[sKeyName] = linkedViewsDoc.ToString();
                        //need the new ids in doctocalc
                        await UpdateDocToCalcAsync(calcDocURI, docToCalcURI, keysToUpdate);
                    }
                }
            }
        }
        private static async Task UpdateDocToCalcAsync(ContentURI calcDocURI,
            ContentURI docToCalcURI, IDictionary<string, string> keysToUpdate)
        {
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, 
                docToCalcURI.URIDataManager.TempDocPath))
            {
                //refactor?: do this using less memory
                XElement docToCalc = DevTreks.Data.Helpers.FileStorageIO.LoadXmlElement(
                    docToCalcURI, docToCalcURI.URIDataManager.TempDocPath);
                if (docToCalc != null)
                {
                    string sDescendantURIPattern = string.Empty;
                    string sAttName = string.Empty;
                    string sDataType = string.Empty;
                    string sStep = string.Empty;
                    string sDescendantKeyName = string.Empty;
                    string sNewInsertId = string.Empty;
                    foreach (KeyValuePair<string, string> keyToUpdate in keysToUpdate)
                    {
                        sDescendantKeyName = keyToUpdate.Key;
                        sNewInsertId = keyToUpdate.Value;
                        DataEditHelpers.EditHelper.GetStandardEditNameParams(
                            sDescendantKeyName.Split(DataHelpers.GeneralHelpers.STRING_DELIMITERS),
                            out sDescendantURIPattern, out sAttName, out sDataType, out sStep);
                        string sDescendantNodeName = ContentURI.GetURIPatternPart(
                            sDescendantURIPattern, ContentURI.URIPATTERNPART.node);
                        string sDescendantId = ContentURI.GetURIPatternPart(
                            sDescendantURIPattern, ContentURI.URIPATTERNPART.id);
                        DataEditHelpers.XmlLinq.SetDescendantLinkedViewAttributeValue(sDescendantNodeName,
                            sDescendantId,  DataAppHelpers.Calculator.cId, sNewInsertId,
                            calcDocURI.URIId.ToString(), ref docToCalc);
                        //resource stock calculators/analyzers find resource stock
                        //data, in the specific child xmldoc holding the data, by adding a 
                        //calculatorid attribute to parent element
                        DataEditHelpers.XmlLinq.SetExistingAttributeValue(docToCalc, 
                            sDescendantNodeName, sDescendantId,
                            DataAppHelpers.Calculator.cCalculatorId, sNewInsertId);
                    }
                    //note: no need for further manipulation of doctocalc in 
                    //updates action, but still need to insert children doctocalcs
                    //using the SaveSummaryAndFullAction (so that descendants 
                    //don't require separate action)
                    await SaveUpdatedAddinResultsAsync(docToCalcURI, docToCalc);
                }
            }
        }
        private async Task<bool> UpdateLinkedViewXmlDocsInDbAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates,
            IDictionary<string, string> childrenLinkedView)
        {
            bool bIsUpdated = false;
            IDictionary<string, string> xmlDocsToUpdate
                = new Dictionary<string, string>();
            List<string> keysToRemove = new List<string>();
            string sKeyName = string.Empty;
            string sKeyURIPattern = string.Empty;
            string sAttName = string.Empty;
            string sDataType = string.Empty;
            string sStep = string.Empty;
            string sDbKeyName = string.Empty;
            bool bIsLinkedViewXmlDoc = false;
            string sAppNodeId = string.Empty;
            string sLinkedViewId = string.Empty;
            string sNewKeyURIPattern = string.Empty;
            string sNewKeyName = string.Empty;
            bool bHasUpdatedLinkedViewXmlDoc = false;
            foreach (KeyValuePair<string, string> kvp in updates)
            {
                sKeyName = kvp.Key;
                DataEditHelpers.EditHelper.GetStandardEditNameParams(
                    sKeyName.Split(DataHelpers.GeneralHelpers.STRING_DELIMITERS),
                    out sKeyURIPattern, out sAttName, out sDataType, out sStep);
                if (sAttName == DataHelpers.GeneralHelpers.ROOT_PATH)
                {
                    //stores one unique linkedview element
                    //in linkedview tables which should make this 100% reliable
                    bIsLinkedViewXmlDoc
                        = DataAppHelpers.LinkedViews.IsLinkedViewXmlDoc(sKeyURIPattern, docToCalcURI);
                    if (bIsLinkedViewXmlDoc)
                    {
                        bHasUpdatedLinkedViewXmlDoc = false;
                        //retrieving linkedview collection for sKeyURIPattern could be an improvement
                        XElement linkedViewsDoc
                            = XElement.Parse(kvp.Value);
                        if (linkedViewsDoc != null)
                        {
                            //one unique linkedview per table row allowed
                            sLinkedViewId = DataEditHelpers.XmlLinq
                                .GetFirstChildLinkedViewId(linkedViewsDoc);
                            if (!string.IsNullOrEmpty(sLinkedViewId))
                            {
                                sAppNodeId = ContentURI.GetURIPatternPart(sKeyURIPattern,
                                    ContentURI.URIPATTERNPART.id);
                                if (!string.IsNullOrEmpty(sAppNodeId))
                                {
                                    //this is the pattern for node updates
                                    sNewKeyURIPattern = sKeyURIPattern.Replace(sAppNodeId,
                                        sLinkedViewId);
                                    sNewKeyName = sKeyName.Replace(sKeyURIPattern,
                                        sNewKeyURIPattern);
                                    //tell ef which db property needs to be updated
                                    string sAttributeName = string.Concat(DataHelpers.GeneralHelpers.STRING_DELIMITER,
                                        DataHelpers.GeneralHelpers.ROOT_PATH, DataHelpers.GeneralHelpers.STRING_DELIMITER);
                                    string sDBPropertyName = string.Concat(DataHelpers.GeneralHelpers.STRING_DELIMITER,
                                        DataAppHelpers.LinkedViews.LINKINGXMLDOC, DataHelpers.GeneralHelpers.STRING_DELIMITER);
                                    sNewKeyName = sNewKeyName.Replace(sAttributeName, sDBPropertyName);
                                    if (!xmlDocsToUpdate.ContainsKey(sNewKeyName))
                                    {
                                        xmlDocsToUpdate.Add(sNewKeyName, kvp.Value);
                                        if (!keysToRemove.Contains(sKeyName))
                                        {
                                            keysToRemove.Add(sKeyName);
                                        }
                                        //build a childrenlinkedviews collection for file system use
                                        AddChildrenLinkedView(calcDocURI, childrenLinkedView,
                                            sKeyURIPattern, sNewKeyURIPattern);
                                        bHasUpdatedLinkedViewXmlDoc = true;
                                    }
                                }
                            }
                        }
                        if (!bHasUpdatedLinkedViewXmlDoc)
                        {
                            //don't allow xmldoc insertions in base app nodes
                            if (!keysToRemove.Contains(sKeyName))
                            {
                                keysToRemove.Add(sKeyName);
                            }
                        }
                    }
                }
            }
            //this updates xmldocs only
            bIsUpdated = await SaveUpdatedLinkedViewXmlDocsInDbAsync(contentService, 
                docToCalcURI, calcDocURI, updates, xmlDocsToUpdate, keysToRemove);
            return bIsUpdated;
        }
       
        private void AddChildrenLinkedView(ContentURI calcDocURI, 
            IDictionary<string, string> childrenLinkedView,
            string keyURIPattern, string newKeyURIPattern)
        {
            //save summaryandfull docs need to know 
            //the newkeyruripatterns in order to save calcs
            //in descendant files; note that calcDocURI.URIFileExtensionType
            //should have been updated by the extension (see UpdateCalcDocURIFileExtension)
            if (!string.IsNullOrEmpty(calcDocURI.URIFileExtensionType))
            {
                string sLinkedViewId = ContentURI.GetURIPatternPart(newKeyURIPattern,
                    ContentURI.URIPATTERNPART.id);
                //this is the pattern for saving results in file system (calcdocuri used)
                string sNewKeyURIPattern = ContentURI.ChangeURIPatternPart(
                    calcDocURI.URIPattern, ContentURI.URIPATTERNPART.id,
                    sLinkedViewId);
                //make sure the AddIn filenaming convention of using linkedview
                //as the node name is enforced
                newKeyURIPattern = sNewKeyURIPattern; 
            }
            if (!childrenLinkedView.ContainsKey(keyURIPattern))
            {
                childrenLinkedView.Add(new KeyValuePair<string, string>
                    (keyURIPattern, newKeyURIPattern));
            }
        }
        private async Task<bool> SaveUpdatedLinkedViewXmlDocsInDbAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI, IDictionary<string, string> updates, 
            IDictionary<string, string> xmlDocsToUpdate, List<string> keysToRemove)
        {
            bool bIsUpdated = false;
            if (xmlDocsToUpdate.Count > 0)
            {
                //remove all the xmldocs from updates
                foreach (string keyname in keysToRemove)
                {
                    if (updates.ContainsKey(keyname))
                    {
                        updates.Remove(keyname);
                    }
                }
                StringDictionary colDeletes = new StringDictionary();
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, docToCalcURI.URIDataManager.TempDocPath))
                {
                    //replace subaction view temporarily
                    string sSubActionView = docToCalcURI.URIDataManager.SubActionView;
                    //use the same update pattern as regular linkedviews
                    docToCalcURI.URIDataManager.SubActionView
                        = DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewsxmldocs.ToString();
                    //update all of the linkedview xmldocs
                    bIsUpdated = await contentService.UpdateContentAsync(docToCalcURI,
                        colDeletes, xmlDocsToUpdate);
                    //get rid of dataviewname for remaining updates
                    //use the same update pattern as regular linkedviews
                    docToCalcURI.URIDataManager.SubActionView = sSubActionView;
                }
            }
            return bIsUpdated;
        }
        public static async Task SaveUpdatedAddinResultsAsync(ContentURI uri,
            XElement devTrekLinqRoot)
        {
            //some updates don't pass a full doctocalc to updates
            if (devTrekLinqRoot.HasElements)
            {
                //this is called from contentService.Update(docToCalcURI ...)
                //only want the xelement save in tempdoc path at this stage
                string sErrorMsg = string.Empty;
                DataHelpers.FileStorageIO fileStorageIO = new DataHelpers.FileStorageIO();
                sErrorMsg = await fileStorageIO.SaveXmlInURIAsync(
                    uri, devTrekLinqRoot.CreateReader(),
                    uri.URIDataManager.TempDocPath);
                uri.ErrorMessage += sErrorMsg;
            }
        }
        public static void SetHostInitParams(string hostName, ref string calcParams)
        {
            //add host-specific calcParams needed to init the host (i.e. at stepzero)
            if (DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString().Equals(hostName.ToLower())
                || DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(hostName.ToLower()))
            {
                DevTreks.Extensions.DoStepsHost.SetInitParams(
                    ref calcParams);
            }
            else if (hostName == string.Empty)
            {
                //indeterminate hosts should init with step=stepzero
                DevTreks.Extensions.DoStepsHost.SetInitParams(
                     ref calcParams);
            }
        }
        private static void CopyCalcDocs(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            //save calcs in file system
            DataHelpers.FileStorageIO.CopyURIs(docToCalcURI, calcDocURI.URIDataManager.TempDocPath,
                calcDocURI.URIClub.ClubDocFullPath);
            //and copy its corresponding html files
            CopyFullXhtmlFiles(calcDocURI, calcDocURI.URIClub.ClubDocFullPath, 
                DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc);
            //v170 eliminated -there is not print view of calculators (they just don't show commands)
            ////need a new print view of calculators for anonymous users
            //MakeCalcDocPrintHtml(calcDocURI);
            ////copy from tempdoc paths to regular paths
            //CopyPrintXhtmlFiles(calcDocURI, calcDocURI.URIClub.ClubDocFullPath,
            //    DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc);
        }
        private static void CopyDocToCalcDocs(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            //copy xml docs
            DataHelpers.FileStorageIO.CopyURIs(docToCalcURI, 
                docToCalcURI.URIDataManager.TempDocPath,
                docToCalcURI.URIClub.ClubDocFullPath);
            //copy html docs
            CopyDocToCalcHtmlDocs(docToCalcURI, calcDocURI);
            if (docToCalcURI.URIFileExtensionType
                == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                DataHelpers.FileStorageIO.CopyURIs(docToCalcURI, 
                    calcDocURI.URIDataManager.TempDocPath,
                    calcDocURI.URIClub.ClubDocFullPath);
            }
        }
        private static void CopyDocToCalcHtmlDocs(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            //copy the xml's corresponding html files
            CopyPrintXhtmlFiles(docToCalcURI, docToCalcURI.URIClub.ClubDocFullPath, 
                DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc);
        }
        
        private static void CopyFullXhtmlFiles(ContentURI uri,
            string xmlDocPath, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER docStateNumber)
        {
            //and overwrite its corresponding html frag file
            string sFilePathToXhtmlDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, uri.URIDataManager.TempDocPath,
                DataAppHelpers.Resources.FILEEXTENSION_TYPES.frag,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString());
            string sFilePathToXhtmlDbDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, xmlDocPath, DataAppHelpers.Resources.FILEEXTENSION_TYPES.frag,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString());
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, sFilePathToXhtmlDoc))
            {
                DataHelpers.FileStorageIO.CopyURIs(uri, sFilePathToXhtmlDoc,
                    sFilePathToXhtmlDbDoc);
            }
            //and overwrite its corresponding html full file
            sFilePathToXhtmlDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, uri.URIDataManager.TempDocPath,
                DataAppHelpers.Resources.FILEEXTENSION_TYPES.html,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString());
            sFilePathToXhtmlDbDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, xmlDocPath, DataAppHelpers.Resources.FILEEXTENSION_TYPES.html,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString());
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, sFilePathToXhtmlDoc))
            {
                DataHelpers.FileStorageIO.CopyURIs(uri, sFilePathToXhtmlDoc,
                    sFilePathToXhtmlDbDoc);
            }
        }
        private static void CopyPrintXhtmlFiles(ContentURI uri,
            string xmlDocPath, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER docStateNumber)
        {
            //and overwrite its corresponding html frag file
            string sFilePathToXhtmlDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, uri.URIDataManager.TempDocPath,
                DataAppHelpers.Resources.FILEEXTENSION_TYPES.frag,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.print.ToString());
            string sFilePathToXhtmlDbDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, xmlDocPath, DataAppHelpers.Resources.FILEEXTENSION_TYPES.frag,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.print.ToString());
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, sFilePathToXhtmlDoc))
            {
                DataHelpers.FileStorageIO.CopyURIs(uri, sFilePathToXhtmlDoc,
                    sFilePathToXhtmlDbDoc);
            }
            sFilePathToXhtmlDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, uri.URIDataManager.TempDocPath,
                DataAppHelpers.Resources.FILEEXTENSION_TYPES.html,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.print.ToString());
            sFilePathToXhtmlDbDoc = DataHelpers.AppSettings.GetXhtmlDocPath(
                uri, docStateNumber, xmlDocPath, DataAppHelpers.Resources.FILEEXTENSION_TYPES.html,
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.print.ToString());
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, sFilePathToXhtmlDoc))
            {
                DataHelpers.FileStorageIO.CopyURIs(uri, sFilePathToXhtmlDoc,
                    sFilePathToXhtmlDbDoc);
            }
        }
        private static void CopyAnalysesObservations(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            //analyzers in these hosts generate an intermediate observations file
            //if it exists, copy it from tempdocpath to doctocalcpath
            if (DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(
                calcDocURI.URIDataManager.HostName.ToLower()))
            {
                string sObservationsTempDocPath 
                    = docToCalcURI.URIDataManager.TempDocPath.Replace(
                    DataHelpers.GeneralHelpers.EXTENSION_XML,
                    string.Concat(DataAppHelpers.General.OBSERVATIONS_OB, 
                    DataHelpers.GeneralHelpers.EXTENSION_XML));
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, sObservationsTempDocPath))
                {
                    string sDocFileName
                        = Path.GetFileName(docToCalcURI.URIClub.ClubDocFullPath);
                    string sObsFileName = sDocFileName.Replace(
                        DataHelpers.GeneralHelpers.EXTENSION_XML,
                        string.Concat(DataAppHelpers.General.OBSERVATIONS_OB,
                            DataHelpers.GeneralHelpers.EXTENSION_XML));
                    sObsFileName 
                        = DataHelpers.ContentHelper.FixFilePathLength( 
                        docToCalcURI, sObsFileName);
                    string sObservationsDocPath
                        = docToCalcURI.URIClub.ClubDocFullPath.Replace(sDocFileName,
                            sObsFileName);
                    DataHelpers.FileStorageIO.CopyURIs(docToCalcURI, sObservationsTempDocPath,
                        sObservationsDocPath);
                }
            }
        }
        public bool CopyObservationsTextFile(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            bool bHasCopied = false;
            string sObservationsTempDocPath
                = docToCalcURI.URIDataManager.TempDocPath.Replace(
                DataHelpers.GeneralHelpers.EXTENSION_XML,
                string.Concat(DataAppHelpers.General.OBSERVATIONS_OB,
                DataHelpers.GeneralHelpers.EXTENSION_CSV));
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, 
                sObservationsTempDocPath))
            {
                string sDocFileName
                    = Path.GetFileName(docToCalcURI.URIClub.ClubDocFullPath);
                string sObsFileName = sDocFileName.Replace(
                    DataHelpers.GeneralHelpers.EXTENSION_XML,
                    string.Concat(DataAppHelpers.General.OBSERVATIONS_OB,
                        DataHelpers.GeneralHelpers.EXTENSION_CSV));
                sObsFileName
                    = DataHelpers.ContentHelper.FixFilePathLength(docToCalcURI, sObsFileName);
                string sObservationsDocPath
                    = docToCalcURI.URIClub.ClubDocFullPath.Replace(sDocFileName,
                        sObsFileName);
                DataHelpers.FileStorageIO.CopyURIs(docToCalcURI, 
                    sObservationsTempDocPath, sObservationsDocPath);
                bHasCopied = true;
            }
            return bHasCopied;
        }
    }
}
