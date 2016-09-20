using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Errors = DevTreks.Exceptions.DevTreksErrors;
using DataHelpers = DevTreks.Data.Helpers;
using DevTreks.Data;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		This host extends (MEF) DevTreks with new calculators, 
    ///             analyzers, and storytellers that use distinct steps.
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	refer to the Calculators and Analyzers tutorial
    ///NOTES        1. 
    ///             See the CalculatorContracts.BaseModel folder.
    /// </summary>
    public class DoStepsHost
    {
        private const string EXTENSIONS_NAMESPACE = "DevTreks.Extensions.";
        #region "run calculator"
        //interface (or abstract class) this host imports
        [ImportMany(RequiredCreationPolicy = CreationPolicy.NonShared)]
        IEnumerable<Lazy<DoStepsHostView, IDoStepsHostMetaData>> calculatorViews { get; set; }
        public async Task<bool> DoStepAsync(ContentURI docToCalcURI, ContentURI calcDocURI,
            string extensionTypeName, IList<string> urisToAnalyze, IDictionary<string, string> updates,
            CancellationToken cancellationToken)
        {
            bool bIsStepDone = false;
            try
            {
                //extensiontypename (a parameter stored in db) determines 
                //extension to run
                DoStepsHostView hostview = GetDoStepsView(calcDocURI, 
                    extensionTypeName);
                if (hostview != null)
                {
                    //these extensions define their own number of steps internally
                    string sStepNumber = DataHelpers.AddInHelper.GetStepNumber(docToCalcURI);
                    //use the view to carry out the current step
                    bIsStepDone = await RunStepAsync(docToCalcURI, calcDocURI, sStepNumber,
                        hostview, urisToAnalyze, updates, cancellationToken);
                }
                else
                {
                    docToCalcURI.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_MISSING_HOST");
                }
            }
            catch (CompositionException x)
            {
                docToCalcURI.ErrorMessage = x.ToString();
            }
            return bIsStepDone;
        }
        private DoStepsHostView GetDoStepsView(ContentURI uri, 
            string extensionTypeName)
        {
            DoStepsHostView hostview = null;
            try
            {
                //directory storing extensions (webproject apppath (c:\Devtreks)
                //extension projects use the project's post build event to copy their dlls there
                string sExtensionRoot = DataHelpers.AppSettings.GetExtensionsRelPath(uri);
                CompositionContainer container
                    = GetCompositionContainerFromDirectory(sExtensionRoot);
                container.ComposeParts(this);
                foreach (var calculatorView in calculatorViews)
                {
                    //the export attributes will change in future upgrades
                    //extensiontypename (a parameter stored in db) determines extension to run
                    if (calculatorView.Metadata.CONTRACT_TYPE
                        == CALULATOR_CONTRACT_TYPES.defaultcalculatormanager
                        && calculatorView.Metadata.CalculatorsExtensionName.ToLower()
                            == extensionTypeName.ToLower())
                    {
                        hostview = calculatorView.Value;
                        container.ReleaseExport(calculatorView);
                        break;
                    }
                }
            }
            catch (CompositionException cex)
            {
                uri.ErrorMessage = cex.ToString();
            }
            return hostview;
        }
        private CompositionContainer GetCompositionContainerFromDirectory(
            string extensionsDirectory)
        {
            var catalog = new AggregateCatalog();
            var thisAssembly = new AssemblyCatalog(
                System.Reflection.Assembly.GetExecutingAssembly());
            catalog.Catalogs.Add(thisAssembly);
            //the post build events of current extensions copy them to this directory
            catalog.Catalogs.Add(new DirectoryCatalog(extensionsDirectory));
            var container = new CompositionContainer(catalog);
            return container;
        }
        private DirectoryCatalog GetDirectoryCatalog(string extensionsDirectory)
        {
            DirectoryCatalog dirCatalog = null;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            string sBaseDirectory = currentDomain.GetData("APPBASE").ToString();
            string sSystemDir = System.Environment.CurrentDirectory;
            dirCatalog = new DirectoryCatalog(extensionsDirectory);
            return dirCatalog;
        }
        public DoStepsHostView GetHostView(
            CALULATOR_CONTRACT_TYPES calcContractType)
        {
            return calculatorViews
                .Where(cv => cv.Metadata.CONTRACT_TYPE.Equals(calcContractType.ToString()))
                .Select(cv => cv.Value)
                .FirstOrDefault();
        }
        public IEnumerable<DoStepsHostView> GetHostViews(
            CALULATOR_CONTRACT_TYPES calcContractType)
        {
            return calculatorViews
                .Where(cv => cv.Metadata.CONTRACT_TYPE.Equals(calcContractType.ToString()))
                .Select(cv => cv.Value);
        }
        private async Task<bool> RunStepAsync(ContentURI docToCalcURI, ContentURI calcDocURI,
            string step, DoStepsHostView hostview, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bIsStepDone = false;
            //convert DevTreks' base model to the Extensions base model
            ExtensionContentURI extDocToCalcURI
                = new ExtensionContentURI(docToCalcURI);
            //extensions sets html view needed for each step
            extDocToCalcURI.URIDataManager.NeedsFullView = false;
            extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
            ExtensionContentURI extCalcDocURI
                = new ExtensionContentURI(calcDocURI);
            //always display calculator
            extCalcDocURI.URIDataManager.NeedsFullView = true;
            //return Task.Run(() =>
            //{
            //    cancellationToken.ThrowIfCancellationRequested();
            //}, cancellationToken);
            
            if (calcDocURI.URIDataManager.HostName.ToLower() ==
                DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
            {
                //compute bound and mostly run synchronously
                bIsStepDone = await hostview.RunCalculatorStep(
                    extDocToCalcURI, extCalcDocURI, step,
                    urisToAnalyze, updates, cancellationToken);
                //set any new general display parameters 
                docToCalcURI.URIDataManager.NeedsFullView
                    = extDocToCalcURI.URIDataManager.NeedsFullView;
                docToCalcURI.URIDataManager.NeedsSummaryView
                    = extDocToCalcURI.URIDataManager.NeedsSummaryView;
                docToCalcURI.URIDataManager.TempDocSaveMethod
                    = extDocToCalcURI.URIDataManager.TempDocSaveMethod;
                docToCalcURI.ErrorMessage += extDocToCalcURI.ErrorMessage;
            }
            else if (calcDocURI.URIDataManager.HostName.ToLower() ==
                DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString())
            {
                //compute bound and mostly run synchronously
                bIsStepDone = await hostview.RunAnalyzerStep(
                    extDocToCalcURI, extCalcDocURI, step,
                    urisToAnalyze, updates, cancellationToken);
                //set any new general display parameters
                docToCalcURI.URIDataManager.NeedsFullView
                    = extDocToCalcURI.URIDataManager.NeedsFullView;
                docToCalcURI.URIDataManager.NeedsSummaryView
                    = extDocToCalcURI.URIDataManager.NeedsSummaryView;
                docToCalcURI.URIDataManager.TempDocSaveMethod
                    = extDocToCalcURI.URIDataManager.TempDocSaveMethod;
                docToCalcURI.ErrorMessage += extDocToCalcURI.ErrorMessage;
            }
            //newly inserted calculators usually set new fileextensiontypes
            if (extCalcDocURI.URIFileExtensionType
                != calcDocURI.URIFileExtensionType
                && (!string.IsNullOrEmpty(extCalcDocURI.URIFileExtensionType))
                && extCalcDocURI.URIFileExtensionType != Constants.NONE)
            {
                //note: nothing more should be done with this parameter
                //linked view authors must manually set this property on
                //the LinkedView html form (previously ran unnecessary db updates)
                calcDocURI.URIFileExtensionType
                    = extCalcDocURI.URIFileExtensionType;
            }
            if (docToCalcURI.ErrorMessage == string.Empty)
            {
                //set new stylesheet parameters
                //note that the sshelper looks for ssname2, which could be passed to it here
                IContentRepositoryEF contentRepository
                    = new DevTreks.Data.SqlRepositories.ContentRepository(docToCalcURI);
                //i/o bound and run async
                await contentRepository.SetLinkedViewStateAsync(docToCalcURI, docToCalcURI);
                contentRepository.Dispose();
            }
            
            return bIsStepDone;
        }
        
        #endregion
        #region "static methods used to initialize calculator"
        public static string GetCalcDocVersion(ContentURI calcDocURI,
            XElement linkedView, bool UseBaseId)
        {
            //extensions update calculators by changing the VERSION
            string sVersion = string.Empty;
            if (linkedView.Name.LocalName
                == DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                sVersion = DevTreks.Data.EditHelpers.XmlLinq
                    .GetAttributeValue(linkedView, 
                    Calculator1.cVersion);
            }
            else
            {
                string sId = calcDocURI.URIId.ToString();
                if (UseBaseId)
                {
                    //base calculators in this host always init with an id = 1
                    sId = "1";
                }
                XElement baseLinkedView = CalculatorHelpers.GetElement(linkedView,
                    DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                    sId);
                if (baseLinkedView == null)
                {
                    //but some ext builders may violate that
                    baseLinkedView = CalculatorHelpers.GetElement(linkedView,
                        DevTreks.Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                        calcDocURI.URIDataManager.BaseId.ToString());
                }
                if (baseLinkedView != null)
                {
                    sVersion = DevTreks.Data.EditHelpers.XmlLinq
                        .GetAttributeValue(baseLinkedView, 
                        Calculator1.cVersion);
                }
            }
            return sVersion;
        }
        public static void GetStylesheetParametersForHost(ContentURI docToCalcURI,
            ref IDictionary<string, string> styleParams)
        {
            //this host relies on unique steps
            //what parts of the calculator should be hidden or displayed?
            string sLastStepNumber = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI, "step");
            if (string.IsNullOrEmpty(sLastStepNumber)) sLastStepNumber = string.Empty;
            styleParams.Add("lastStepNumber", sLastStepNumber);
        }
        public static void SetInitParams(ref string calcParams)
        {
            //all extensions init with stepzero
            if (calcParams.StartsWith("'"))
            {
                calcParams = calcParams.Remove(0, 1);
                calcParams = string.Concat("'&step=",
                    DevTreks.Data.AppHelpers.General.STEPZERO, calcParams);
            }
            else
            {
                calcParams = string.Concat("&step=",
                    DevTreks.Data.AppHelpers.General.STEPZERO, calcParams);
            }
        }
        
        public static bool NeedsNewDocToCalc(ContentURI calcDocURI,
            ContentURI docToCalcURI)
        {
            bool bNeedsNewDocToCalc = false;
            bool bHasTempDocState = HasTempDocToCalcState(calcDocURI, docToCalcURI);
            if (calcDocURI.URIDataManager.HostName.ToLower() ==
                DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
            {
                //if no tempdocuri in form els, needs an initial tempdoctocalc
                if (bHasTempDocState == false)
                {
                    bNeedsNewDocToCalc = true;
                }
            }
            else if (calcDocURI.URIDataManager.HostName.ToLower() ==
                DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString())
            {
                //0.9.1: calculators and analyzers now use same state management
                if (bHasTempDocState == false)
                {
                    bNeedsNewDocToCalc = true;
                }
            }
            return bNeedsNewDocToCalc;
        }
        
        public static bool HasTempDocToCalcState(ContentURI calcDocURI,
            ContentURI docToCalcURI)
        {
            bool bHasTempDocState = false;
            string sTempDocURIPattern = DataHelpers.GeneralHelpers.GetFormValue(calcDocURI,
                DevTreks.Data.AppHelpers.LinkedViews.TEMPDOCURI);
            if (!string.IsNullOrEmpty(sTempDocURIPattern))
            {
                string sNewURIPattern
                    = DataHelpers.AppSettings.GetTempDocToCalcURIPattern(docToCalcURI,
                    sTempDocURIPattern);
                //holds any calculation made to doctocalc
                string sTempDocToCalcPath
                    = DataHelpers.AppSettings.GetTempDocPath(docToCalcURI,
                    sNewURIPattern);
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, sTempDocToCalcPath))
                {
                    bHasTempDocState = true;
                }
            }
            return bHasTempDocState;
        }
        public static bool NeedsInitialDbDoc(ContentURI calcDocURI)
        {
            bool bNeedsInitialDbDoc = false;
            if (calcDocURI.URIDataManager.HostName.ToLower() == 
                DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
            {
                //absolutely
                bNeedsInitialDbDoc = true;
            }
            else if (calcDocURI.URIDataManager.HostName.ToLower() == 
                DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString())
            {
                //analyzers in this host rely on base calculated docs, not initial db docs
                //since base db docs can be quite large, don't load if not needed
                bNeedsInitialDbDoc = false;
            }
            return bNeedsInitialDbDoc;
        }
        public static bool NeedsURIsToAnalyze(ContentURI docToCalcURI,
            ContentURI calcDocURI, string saveNone)
        {
            bool bNeedsURIsToAnalyze = false;
            if (calcDocURI.URIDataManager.HostName.ToLower()
                == DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString())
            {
                if ((string.IsNullOrEmpty(
                    docToCalcURI.URIDataManager.TempDocSaveMethod))
                    || docToCalcURI.URIDataManager.TempDocSaveMethod
                    == saveNone
                    || docToCalcURI.URIDataManager.TempDocSaveMethod
                    == DevTreks.Data.Helpers.AddInHelper.SAVECALCS_METHOD.saveastext.ToString())
                {
                    bNeedsURIsToAnalyze = true;
                }
            }
            else if (calcDocURI.URIDataManager.HostName.ToLower()
                == DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
            {
                if (docToCalcURI.URIDataManager.AppType 
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks) 
                {
                    //custom docs can run analyses for children devpackparts
                    if ((string.IsNullOrEmpty(
                        docToCalcURI.URIDataManager.TempDocSaveMethod))
                        || docToCalcURI.URIDataManager.TempDocSaveMethod
                        == saveNone
                        || docToCalcURI.URIDataManager.TempDocSaveMethod
                        == DevTreks.Data.Helpers.AddInHelper.SAVECALCS_METHOD.saveastext.ToString())
                    {
                        bNeedsURIsToAnalyze = true;
                    }
                }
            }
            return bNeedsURIsToAnalyze;
        }
        
        public static bool NeedsLinkedLists(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            bool bNeedsList = false;
            if (calcDocURI.URIDataManager.HostName.ToLower() ==
                DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
            {
                //most need lists; if they don't there won't be any lists to retrieve anyway
                bNeedsList = true;
            }
            else if (calcDocURI.URIDataManager.HostName.ToLower() ==
                DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString())
            {
                //analyzers don't need linked lists (as of this version)
                bNeedsList = false;
            }
            return bNeedsList;
        }
        #endregion
    }
}
