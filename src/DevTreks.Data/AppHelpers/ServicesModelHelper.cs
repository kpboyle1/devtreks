using DevTreks.Data.DataAccess;
using DevTreks.Data.EditHelpers;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Entity Framework Service support class
    ///Author:		www.devtreks.org
    ///Date:		2016, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       Base tables are always edited using their corresponding join records.
    /// </summary>
    public class ServiceModelHelper
    {
        public ServiceModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            _dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIService(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            //can navigate through base nodes, but edits never use base nodes directly
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebasegroup.ToString())
            {
                var mc = await _dataContext.ServiceClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {
                    var qry = _dataContext
                        .Service
                        .Where(a => a.ServiceClassId == uri.URIId)
                        .OrderBy(m => m.ServiceName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.Service = await qry.ToAsyncEnumerable().ToList();
                        if (mc.Service != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .Service
                                .Where(a => a.ServiceClassId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.ServiceClass = mc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                //part of navigation, but edits take place using join record
                var qry = _dataContext
                    .Service
                    .Include(t => t.ServiceClass)
                    .Where(r => r.PKId == uri.URIId);
                if (qry != null)
                {
                    uri.URIModels.Service = await qry.FirstOrDefaultAsync();
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                }
            }
            else if (uri.URINodeName == Agreement.AGREEMENT_TYPES.serviceaccount.ToString())
            {
                var mc = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {
                    var qry = _dataContext
                        .AccountToService
                        .Where(a => a.AccountId == uri.URIId)
                        .Include(t => t.Service)
                        .Include(t => t.Service.Network)
                        .Include(t => t.AccountToIncentive)
                        //retain: .Include(t => t.AccountToIncentive.Incentive)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.AccountToService = await qry.ToListAsync();
                        if (mc.AccountToService != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .AccountToService
                                .Where(a => a.AccountId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //also need the AccountToNetwork collection, so that network can be set
                    var qry2 = _dataContext
                       .AccountToNetwork
                       .Where(a => a.AccountId == uri.URIId)
                       .Include(t => t.Network)
                       .OrderBy(m => m.Network.NetworkName);
                    //set the data transfer objects
                    if (qry2 != null)
                    {
                        mc.AccountToNetwork = await qry2.ToListAsync();
                        uri.URIModels.Account = mc;
                        //row count is not based on networks
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                var qry = _dataContext
                    .AccountToService
                    .Include(t => t.Service)
                    .Include(t => t.Service.Network)
                    .Include(t => t.AccountToIncentive)
                    //retain: .Include(t => t.AccountToIncentive.Incentive)
                    .Where(r => r.PKId == uri.URIId);
                
                if (qry != null)
                {
                    uri.URIModels.AccountToService = await qry.FirstOrDefaultAsync();
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                }
            }
            //retain for potential use: run new totals anytime a data set is retrieved
            //DevTreks.Data.RuleHelpers.AgreementRulesDTO agreementRules 
            //    = new Data.RuleHelpers.AgreementRulesDTO();
            //agreementRules.RunTotalsAsync(uri);
            if (string.IsNullOrEmpty(uri.ErrorMessage))
            {
                bHasSet = true;
            }
            return bHasSet;
        }

        public IQueryable<AccountToService> GetURIService(string nodeName, int nodeId)
        {
            var qry = _dataContext
                    .AccountToService
                    .Where(r => r.PKId == nodeId);
            return qry;
        }
        public async Task<bool> AddService(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            //store updated service ids in lists
            List<Service> addedSs = new List<Service>();
            List<AccountToService> addedAToSs = new List<AccountToService>();
            bool bHasSet = await AddService(argumentsEdits.SelectionsToAdd, addedAToSs, addedSs);
            if (addedAToSs.Count > 0 || addedSs.Count > 0)
            {
                try
                {
                    //save selected subscription (atoss) and base services(addess)
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    if (addedSs.Count > 0)
                    {
                        bIsAdded = false;
                        foreach (var service in addedSs)
                        {
                            //have a good base table insertion, now can insert join
                            //only time isowner should be ever set
                            bool bIsOwner = true;
                            var newAccountToService = new AccountToService
                            {
                                Name = service.ServiceName,
                                Amount1 = 1,
                                Status = Agreement.SERVICE_STATUS_TYPES.current.ToString(),
                                StatusDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                AuthorizationLevel = (short) Agreement.PUBLIC_AUTHORIZATION_TYPES.public_not_authorized,
                                StartDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                EndDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                IsOwner = bIsOwner,
                                AccountId = _dtoContentURI.URIId,
                                Account = null,
                                ServiceId = service.PKId,
                                Service = null,
                                AccountToIncentive = null,
                                AccountToPayment = null
                            };
                            _dataContext.AccountToService.Add(newAccountToService);
                            _dataContext.Entry(newAccountToService).State = EntityState.Added;
                            iNotUsed = await _dataContext.SaveChangesAsync();
                            bIsAdded = true;
                        }
                    }
                    //only the edit panel needs an updated collection of services
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIService(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIService(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        //note that get selections is only join table insertions
        private async Task<bool> AddService(List<ContentURI> addedURIs, List<AccountToService> addedAToSs, 
            List<Service> addedSs)
        {
            bool bHasSet = true;
            string sParentNodeName = string.Empty;
            int iParentId = 0;
            foreach (ContentURI addedURI in addedURIs)
            {
                Helpers.GeneralHelpers.GetParentIdAndNodeName(addedURI, out iParentId, out sParentNodeName);
                if (!string.IsNullOrEmpty(addedURI.ErrorMessage))
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "INSERT_NOPARENT");
                    return false;
                }
                if (addedURI.URINodeName == Agreement.AGREEMENT_TYPES.service.ToString())
                {
                    //addedURI.URIId == ServiceClassId == SubAppType; must be below 1000 to add a base service
                    int iMaxSubApp = (int)Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.resources;
                    if (addedURI.URIId <= iMaxSubApp
                        && _dtoContentURI.URINodeName == Agreement.AGREEMENT_TYPES.serviceaccount.ToString())
                    {
                        //base table gets inserted first
                        //2.0.0 refactor: EF no longer allows fks = 0; so changed to new NetworkId = 1
                        //new table row inserted for this purpose in table Network (see Source Code documentation)
                        int iNetworkId = 1;
                        
                        //int iNetworkId = 0;
                        //if (_dtoContentURI.URINetwork != null)
                        //{
                        //    iNetworkId = _dtoContentURI.URINetwork.PKId;
                        //}

                        var newService = new Service
                        {
                            ServiceNum = Helpers.GeneralHelpers.NONE,
                            ServiceName = addedURI.URIName,
                            ServiceDesc = Helpers.GeneralHelpers.NONE,
                            ServicePrice1 = 0,
                            ServiceUnit1 = Agreement.SERVICE_UNIT_TYPES.month.ToString(),
                            ServiceCurrency1 = Agreement.SERVICE_CURRENCY_TYPES.usdollar.ToString(),
                            //chosen after insertion
                            NetworkId = iNetworkId,
                            Network = null,
                            //stored as the uriid
                            ServiceClassId = addedURI.URIId,
                            ServiceClass = null,
                            AccountToService = null,
                            BudgetSystem = null,
                            ComponentClass = null,
                            CostSystem = null,
                            DevPackClass = null,
                            InputClass = null,
                            LinkedViewClass = null,
                            OperationClass = null,
                            OutcomeClass = null,
                            OutputClass = null,
                            ResourceClass = null
                        };
                        _dataContext.Service.Add(newService);
                        _dataContext.Entry(newService).State = EntityState.Added;
                        addedSs.Add(newService);
                    }
                    else
                    {
                        //addedURI.URIId == joinid; need base serviceid
                        var accounttoservice = await _dataContext.AccountToService.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                        if (accounttoservice != null)
                        {
                            var newAccountToService = new AccountToService
                            {
                                Name = accounttoservice.Name,
                                Amount1 = 1,
                                Status = Agreement.SERVICE_STATUS_TYPES.current.ToString(),
                                StatusDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                AuthorizationLevel = (short)Agreement.PUBLIC_AUTHORIZATION_TYPES.public_not_authorized,
                                StartDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                EndDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                                IsOwner = false,
                                AccountId = _dtoContentURI.URIId,
                                Account = null,
                                ServiceId = accounttoservice.ServiceId,
                                Service = null,
                                AccountToIncentive = null,
                                AccountToPayment = null
                            };
                            _dataContext.AccountToService.Add(newAccountToService);
                            _dataContext.Entry(newAccountToService).State = EntityState.Added;
                            addedAToSs.Add(newAccountToService);
                        }
                    }
                }
                else if (addedURI.URINodeName == Agreement.AGREEMENT_TYPES.incentive.ToString())
                {
                    //don't allow incentive insertions until they are needed
                }
            }
            return bHasSet;
        }
        public async Task<bool> DeleteService(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            bool bHasSet = false;
            //stored procedures are used to make cascading deletes easier
            bIsDeleted = await DeleteService(argumentsEdits.SelectionsToAdd);
            if (bIsDeleted)
            {
                //update the dtoContentURI collection
                bHasSet = await SetURIService(_dtoContentURI, false);
            }
            else
            {
                //update the dtoContentURI collection
                bHasSet = await SetURIService(_dtoContentURI, false);
                _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "DELETESERVICE_FAIL");
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteService(List<ContentURI> deletionURIs)
        {
            string sKeyName = string.Empty;
            bool bIsDeleted = false;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                //only join records can be edited
                if (deletionURI.URINodeName == Agreement.AGREEMENT_TYPES.service.ToString()
                    || deletionURI.URINodeName == Agreement.AGREEMENT_TYPES.incentive.ToString())
                {
                    deletionURI.URIDataManager.AppType = Helpers.GeneralHelpers.APPLICATION_TYPES.agreements;
                    //use sqlio to cascade delete base to join or just join (stored procedure is better)
                    bIsDeleted = await DeleteBase(deletionURI);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteBase(ContentURI deletionURI)
        {
            bool bIsOkToSave = false;
            if (_dtoContentURI.URIDataManager.ServerSubActionType
               != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
            {
                EditHelper editHelper = new EditHelper();
                bool bIsDbEdit = true;
                bIsOkToSave = await editHelper.DeleteJoinAndCheckBaseAsync(_dtoContentURI, deletionURI, bIsDbEdit);
            }
            return bIsOkToSave;
        }
        public async Task<bool> UpdateService(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bHasSet = await UpdateService(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURIService(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIService(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateService(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.URIToAdd.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                {
                    var service = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (service != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(service), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(service).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
                else if (edit.URIToAdd.URINodeName == Agreement.AGREEMENT_TYPES.service.ToString())
                {
                    var service = await _dataContext.AccountToService.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (service != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(service), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(service).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
                else if (edit.URIToAdd.URINodeName == Agreement.AGREEMENT_BASE_TYPES.incentivebase.ToString())
                {
                    //hold off until needed
                }
                else if (edit.URIToAdd.URINodeName == Agreement.AGREEMENT_TYPES.incentive.ToString())
                {
                    //hold off until needed
                }
            }
            return bHasSet;
        }

        public async Task<bool> SaveURIFirstDocAsync()
        {
            bool bHasSavedDoc = false;
            if (string.IsNullOrEmpty(_dtoContentURI.URIClub.ClubDocFullPath))
            {
                //when the file path is not set, too much data is prevented
                return true;
            }
            bool bSaveInFileSystemContent = true;
            bool bHasSet = await SetURIService(_dtoContentURI, bSaveInFileSystemContent);
            XElement root = XmlLinq.GetRootXmlDoc();
            bool bHasGoodDoc = false;
            if (_dtoContentURI.URINodeName == Agreement.AGREEMENT_TYPES.serviceaccount.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName,
                        root.Descendants(Agreement.AGREEMENT_TYPES.serviceaccount.ToString()).Last());
                }
            }
            if (bHasGoodDoc)
            {
                bHasSavedDoc = await EditModelHelper.SaveStandardContentXmlDocAsync(_dtoContentURI, root);
            }
            else
            {
                //add an error message
                _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "MODELHELPERS_BADXMLCONTENT");
            }
            return bHasSavedDoc;
        }
        private async Task<bool> AddAncestors(XElement root)
        {
            bool bHasGoodAncestors = false;
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.AccountToService.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.AccountId,
                        Agreement.AGREEMENT_TYPES.serviceaccount.ToString(), root);
                }
            }
            return bHasGoodAncestors;
        }
        private async Task<bool> AddDescendants(int id, string nodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            //deserialize objects
            if (nodeName == Agreement.AGREEMENT_TYPES.serviceaccount.ToString())
            {
                XElement currentNode = new XElement(nodeName);
                //iterate properties to generate xml
                var currentObject = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    var currentObjContext = _dataContext.Entry(currentObject);
                    Dictionary<string, string> propValues = new Dictionary<string, string>();
                    foreach (var property in currentObjContext.Metadata.GetProperties())
                    {
                        if (currentObjContext.Property(property.Name).CurrentValue == null)
                        {
                            propValues.Add(property.Name, string.Empty);
                        }
                        else
                        {
                            var currentValue = currentObjContext
                                .Property(property.Name).CurrentValue.ToString();
                            propValues.Add(property.Name, currentValue);
                        }
                    }
                    EditModelHelper.SetAttributes(propValues, currentNode);
                    //set the children
                    if (_dtoContentURI.URIModels.Account != null)
                    {
                        if (_dtoContentURI.URIModels.Account.AccountToService != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.Account.AccountToService)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, Agreement.AGREEMENT_TYPES.service.ToString(),
                                    currentNode);
                            }
                        }
                    }
                }
                if (root != null
                    && currentNode.HasAttributes)
                {
                    //add the node to the root
                    root.Add(currentNode);
                    bHasGoodDescendants = true;
                }
            }
            else if (nodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                var childObject = await _dataContext.AccountToService.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(Agreement.AGREEMENT_TYPES.service.ToString());
                    Dictionary<string, string> propValues = new Dictionary<string, string>();
                    foreach (var property in childObjContext.Metadata.GetProperties())
                    {
                        if (childObjContext.Property(property.Name) != null)
                        {
                            var currentValue = childObjContext
                                .Property(property.Name).CurrentValue.ToString();
                            propValues.Add(property.Name, currentValue);
                        }
                    }
                    EditModelHelper.SetAttributes(propValues, childNode);
                    if (root != null
                        && childNode.HasAttributes)
                    {
                        //add the descendants to the parent
                        root.Add(childNode);
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
    }
}
