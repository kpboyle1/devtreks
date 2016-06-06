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
    ///Purpose:		Entity Framework Network support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class NetworkModelHelper
    {
        public NetworkModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURINetwork(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = true;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString())
            {
                var mc = await _dataContext.NetworkClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {
                    var qry = _dataContext
                        .Network
                        .Where(a => a.NetworkClassId == uri.URIId)
                        .OrderBy(m => m.NetworkName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.Network = await qry.ToAsyncEnumerable().ToList();
                        if (mc.Network != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .Network
                                .Where(a => a.NetworkClassId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.NetworkClass = mc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Networks.NETWORK_BASE_TYPES.networkbase.ToString())
            {
                var qry = _dataContext
                    .Network
                    .Include(t => t.NetworkClass)
                    .Where(r => r.PKId == uri.URIId);
                
                if (qry != null)
                {
                    uri.URIModels.Network = await qry.FirstOrDefaultAsync();
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                }
            }
            else if (uri.URINodeName == Networks.NETWORK_TYPES.networkaccountgroup.ToString())
            {
                var mc = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {
                    var qry = _dataContext
                        .AccountToNetwork
                        .Where(a => a.AccountId == uri.URIId)
                        .Include(t => t.Network)
                        .OrderBy(m => m.Network.NetworkName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.AccountToNetwork = await qry.ToAsyncEnumerable().ToList();
                        if (mc.AccountToNetwork != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .AccountToNetwork
                               .Where(a => a.AccountId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.Account = mc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Networks.NETWORK_TYPES.network.ToString())
            {
                var qry = _dataContext
                    .AccountToNetwork
                    .Include(t => t.Network)
                    .Where(r => r.PKId == uri.URIId);
                
                if (qry != null)
                {
                    uri.URIModels.AccountToNetwork = await qry.FirstOrDefaultAsync();
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                }
            }
            if (string.IsNullOrEmpty(uri.ErrorMessage))
            {
                bHasSet = true;
            }
            return bHasSet;
        }

        public void SetURINetwork(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString())
            {
                var qryRC = _dataContext
                    .NetworkClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Networks.NETWORK_BASE_TYPES.networkbase.ToString())
            {
                var qryR = _dataContext
                    .Network
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Networks.NETWORK_TYPES.networkaccountgroup.ToString())
            {
                var qryRC = _dataContext
                    .NetworkClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Networks.NETWORK_TYPES.network.ToString())
            {
                var qryR = _dataContext
                    .Network
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddNetwork(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            //store updated resources ids in lists
            List<AccountToNetwork> addedMs = new List<AccountToNetwork>();
            bool bHasSet = AddNetwork(argumentsEdits.SelectionsToAdd, addedMs);
            if (addedMs.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of resources
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURINetwork(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURINetwork(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private bool AddNetwork(List<ContentURI> addedURIs, List<AccountToNetwork> addedMs)
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
                //selections are made use base networks
                if (addedURI.URINodeName == Networks.NETWORK_BASE_TYPES.networkbase.ToString()
                    && _dtoContentURI.URINodeName == Networks.NETWORK_TYPES.networkaccountgroup.ToString())
                {
                    var newAccountToNetwork = new AccountToNetwork
                    {
                        IsDefaultNetwork = false,
                        DefaultGetDataFromType = Networks.NETWORK_STOREDATA_TYPES.web.ToString(),
                        DefaultStoreDataAtType = Networks.NETWORK_STOREDATA_TYPES.web.ToString(),
                        NetworkRole = Members.MEMBER_ROLE_TYPES.contributor.ToString(),
                        AccountId = _dtoContentURI.URIId,
                        Account = null,
                        NetworkId = addedURI.URIId,
                        Network = null
                    };
                    _dataContext.AccountToNetwork.Add(newAccountToNetwork);
                    _dataContext.Entry(newAccountToNetwork).State = EntityState.Added;
                    addedMs.Add(newAccountToNetwork);
                }
            }
            return bHasSet;
        }
        public async Task<bool> DeleteNetwork(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteNetwork(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURINetwork(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURINetwork(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteNetwork(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                //note that networkbases are not deleted -they'll be archived and used for other purposes
                if (deletionURI.URINodeName == Networks.NETWORK_TYPES.network.ToString())
                {
                    var network = await _dataContext.AccountToNetwork.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (network != null)
                    {
                        _dataContext.Entry(network).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
            }
            return bHasSet;
        }
        public async Task<bool> UpdateNetwork(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bHasSet = await UpdateNetwork(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURINetwork(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURINetwork(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateNetwork(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.URIToAdd.URINodeName == Networks.NETWORK_BASE_TYPES.networkbase.ToString())
                {
                    var network = await _dataContext.Network.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (network != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(network), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(network).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
                else if (edit.URIToAdd.URINodeName == Networks.NETWORK_TYPES.network.ToString())
                {
                    var network = await _dataContext.AccountToNetwork.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (network != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(network), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(network).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
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
            bool bHasSet = await SetURINetwork(_dtoContentURI, bSaveInFileSystemContent);
            XElement root = XmlLinq.GetRootXmlDoc();
            bool bHasGoodDoc = false;
            if (_dtoContentURI.URINodeName == Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == Networks.NETWORK_BASE_TYPES.networkbase.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName,
                        root.Descendants(Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString()).Last());
                }
            }
            else if (_dtoContentURI.URINodeName == Networks.NETWORK_TYPES.networkaccountgroup.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == Networks.NETWORK_TYPES.network.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    //add the descendants to the last ancestor added
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName,
                        root.Descendants(Networks.NETWORK_TYPES.networkaccountgroup.ToString()).Last());
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
                == Networks.NETWORK_BASE_TYPES.networkbase.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.Network.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.NetworkClassId,
                        Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString(), root);
                }
            }
            else if (_dtoContentURI.URINodeName
                == Networks.NETWORK_TYPES.network.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.AccountToNetwork.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.AccountId,
                        Networks.NETWORK_TYPES.networkaccountgroup.ToString(), root);
                }
            }
            return bHasGoodAncestors;
        }
        private async Task<bool> AddDescendants(int id, string nodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            //deserialize objects
            if (nodeName == Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString())
            {
                XElement currentNode = new XElement(nodeName);
                //iterate properties to generate xml
                var currentObject = await _dataContext.NetworkClass.SingleOrDefaultAsync(x => x.PKId == id);
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
                    if (_dtoContentURI.URIModels.NetworkClass != null)
                    {
                        if (_dtoContentURI.URIModels.NetworkClass.Network != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.NetworkClass.Network)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, Networks.NETWORK_BASE_TYPES.networkbase.ToString(),
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
            else if (nodeName == Networks.NETWORK_BASE_TYPES.networkbase.ToString())
            {
                var childObject = await _dataContext.Network.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(Networks.NETWORK_BASE_TYPES.networkbase.ToString());
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
            else if (nodeName == Networks.NETWORK_TYPES.networkaccountgroup.ToString())
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
                        if (_dtoContentURI.URIModels.Account.AccountToNetwork != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.Account.AccountToNetwork)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, Networks.NETWORK_TYPES.network.ToString(),
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
            else if (nodeName == Networks.NETWORK_TYPES.network.ToString())
            {
                var childObject = await _dataContext.AccountToNetwork.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(Networks.NETWORK_TYPES.network.ToString());
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
