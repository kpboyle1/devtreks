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
    ///Purpose:		Entity Framework AddIns support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class AddInsModelHelper
    {
        public AddInsModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIAddIns(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == AddIns.ADDIN_TYPES.addinaccountgroup.ToString())
            {
                var mc = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {
                    var qry = _dataContext
                        .AccountToAddIn
                        .Include(t => t.LinkedView)
                        .Where(a => a.Account.PKId == uri.URIId)
                        .OrderBy(m => m.LinkedViewName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.AccountToAddIn = await qry.ToAsyncEnumerable().ToList();
                        if (mc.AccountToAddIn != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                                .AccountToAddIn
                                .Where(a => a.Account.PKId == uri.URIId)
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
                == AddIns.ADDIN_TYPES.addin.ToString())
            {
                var qry = _dataContext
                    .AccountToAddIn
                    .Include(t => t.LinkedView)
                    .Where(r => r.PKId == uri.URIId);
                if (qry != null)
                {
                    uri.URIModels.AccountToAddIn = await qry.FirstOrDefaultAsync();
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

        public void SetURIAddIn(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == AddIns.ADDIN_TYPES.addin.ToString())
            {
                var qryR = _dataContext
                    .AccountToAddIn
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddAddIns(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = false;
            //store updated resources ids in lists
            List<AccountToAddIn> addedMs = new List<AccountToAddIn>();
            AddAddIns(argumentsEdits.SelectionsToAdd, addedMs);
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
                        bHasSet = await SetURIAddIns(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIAddIns(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private void AddAddIns(List<ContentURI> addedURIs, List<AccountToAddIn> addedMs)
        {
            string sParentNodeName = string.Empty;
            int iParentId = 0;
            foreach (ContentURI addedURI in addedURIs)
            {
                Helpers.GeneralHelpers.GetParentIdAndNodeName(addedURI, out iParentId, out sParentNodeName);
                if (!string.IsNullOrEmpty(addedURI.ErrorMessage))
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "INSERT_NOPARENT");
                    return;
                }
                //selections are made use base addins
                if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                    && _dtoContentURI.URINodeName == AddIns.ADDIN_TYPES.addinaccountgroup.ToString())
                {
                    var newAccountToAddIn = new AccountToAddIn
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingNodeId = _dtoContentURI.URIId,
                        Account = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.AccountToAddIn.Add(newAccountToAddIn);
                    _dataContext.Entry(newAccountToAddIn).State = EntityState.Added;
                    addedMs.Add(newAccountToAddIn);
                }
            }
        }
        public async Task<bool> DeleteAddIns(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteAddIns(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURIAddIns(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIAddIns(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteAddIns(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = false;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                //note that addinbases are not deleted -they'll be archived and used for other purposes
                if (deletionURI.URINodeName == AddIns.ADDIN_TYPES.addin.ToString()
                    || deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    var addin = await _dataContext.AccountToAddIn.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (addin != null)
                    {
                        _dataContext.Entry(addin).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
            }
            bHasSet = true;
            return bHasSet;
        }
        public async Task<bool> UpdateAddIns(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bHasSet = await UpdateAddIns(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURIAddIns(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIAddIns(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateAddIns(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bHasSet = false;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.URIToAdd.URINodeName == AddIns.ADDIN_TYPES.addin.ToString()
                    || edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    var addin = await _dataContext.AccountToAddIn.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (addin != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErrorMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(addin), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErrorMsg);
                        _dtoContentURI.ErrorMessage = sErrorMsg;
                        _dataContext.Entry(addin).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
            }
            bHasSet = true;
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
            bool bHasSet = await SetURIAddIns(_dtoContentURI, bSaveInFileSystemContent);
            XElement root = XmlLinq.GetRootXmlDoc();
            bool bHasGoodDoc = false;
            if (_dtoContentURI.URINodeName == AddIns.ADDIN_TYPES.addinaccountgroup.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == AddIns.ADDIN_TYPES.addin.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName, 
                        root.Descendants(AddIns.ADDIN_TYPES.addinaccountgroup.ToString()).Last());
                }
            }
            if (bHasGoodDoc)
            {
                bHasSavedDoc = await EditModelHelper.SaveStandardContentXmlDocAsync(_dtoContentURI, root);
            }
            return bHasSavedDoc;
        }
        private async Task<bool> AddAncestors(XElement root)
        {
            bool bHasGoodAncestors = false;
            if (_dtoContentURI.URINodeName
                == AddIns.ADDIN_TYPES.addin.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.AccountToAddIn.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.LinkingNodeId,
                        AddIns.ADDIN_TYPES.addinaccountgroup.ToString(), root);
                }
            }
            return bHasGoodAncestors;
        }
        private async Task<bool> AddDescendants(int id, string nodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            //deserialize objects
            if (nodeName == AddIns.ADDIN_TYPES.addinaccountgroup.ToString())
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
                        if (_dtoContentURI.URIModels.Account.AccountToAddIn != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.Account.AccountToAddIn)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, AddIns.ADDIN_TYPES.addin.ToString(),
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
            else if (nodeName == AddIns.ADDIN_TYPES.addin.ToString())
            {
                var childObject = await _dataContext.AccountToAddIn.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(AddIns.ADDIN_TYPES.addin.ToString());
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
