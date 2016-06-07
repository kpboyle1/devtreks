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
    ///Purpose:		Entity Framework Locals support class
    ///Author:		www.devtreks.org
    ///Date:		2016, June
    ///Notes:	    2.0.0 simplified by removing reliance on LV calculator and 
    ///             including properties in the model formally defined by calculator
    /// </summary>
    public class LocalsModelHelper
    {
        public LocalsModelHelper(DevTreksContext dataContext, ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURILocals(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = true;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == Locals.LOCAL_TYPES.localaccountgroup.ToString())
            {
                var mc = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {

                    var qry = _dataContext
                       .AccountToLocal
                       .Where(a => a.AccountId == uri.URIId)
                       .Skip(iStartRow)
                       .Take(iPageSize);
                    //var qry = _dataContext
                    //    .AccountToLocal
                    //    .Where(a => a.Account.PKId == uri.URIId)
                    //    .Skip(iStartRow)
                    //    .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.AccountToLocal = await qry.ToAsyncEnumerable().ToList();
                        if (mc.AccountToLocal != null)
                        {
                            uri.URIDataManager.RowCount = mc.AccountToLocal.Count;
                            //uri.URIDataManager.RowCount =
                            //   _dataContext
                            //   .AccountToLocal
                            //    .Where(a => a.Account.PKId == uri.URIId)
                            //   .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.Account = mc;


                    //// count the resource packs without loading them
                    //int total = await _dataContext.Entry(mc)
                    //    .Collection(mcs => mcs.AccountToLocal)
                    //    .Query().Cast<AccountToLocal>()
                    //    .CountAsync();
                    //uri.URIDataManager.RowCount = total;
                    ////paginate the collection
                    //var qry = _dataContext.Entry(mc)
                    //    .Collection("AccountToLocal")
                    //    .Query().Cast<AccountToLocal>()
                    //    .Include("LinkedView")
                    //    .OrderBy(m => m.LocalName)
                    //    .Skip(iStartRow)
                    //    .Take(iPageSize);
                    ////set the data transfer objects
                    //if (total > 0)
                    //{
                    //    mc.AccountToLocal = await qry.ToListAsync();
                    //}
                    //uri.URIModels.Account = mc;
                    //2.0.0 rc2 bug : qry stopped working with refactored accounttolocal
                    AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
                    List<AccountToLocal> colClubLocals
                        = await oClubHelper.GetLocalsByClubIdAsync(uri, uri.URIId);
                    mc.AccountToLocal = colClubLocals;
                    if (mc.AccountToLocal != null)
                    {
                        uri.URIDataManager.RowCount = mc.AccountToLocal.Count;
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
                == Locals.LOCAL_TYPES.local.ToString())
            {
                var qry = _dataContext
                    .AccountToLocal
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.AccountToLocal = await qry.FirstOrDefaultAsync();
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

        public void SetURILocal(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Locals.LOCAL_TYPES.local.ToString())
            {
                var qryR = _dataContext
                    .AccountToLocal
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddLocals(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            //store updated resources ids in lists
            List<AccountToLocal> addedMs = new List<AccountToLocal>();
            bool bHasSet = AddLocals(argumentsEdits.SelectionsToAdd, addedMs);
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
                        bHasSet = await SetURILocals(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURILocals(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private bool AddLocals(List<ContentURI> addedURIs, List<AccountToLocal> addedMs)
        {
            bool bHasSet = true;
            string sParentNodeName = string.Empty;
            int iParentId = 0;
            foreach (ContentURI addedURI in addedURIs)
            {
                //not essential with a 2 level hierarchy, but keep the pattern consistent
                Helpers.GeneralHelpers.GetParentIdAndNodeName(addedURI, out iParentId, out sParentNodeName);
                if (!string.IsNullOrEmpty(addedURI.ErrorMessage))
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "INSERT_NOPARENT");
                    return false;
                }
                //2.0.0 -direct edits allowed for newly added default locals
                if (addedURI.URINodeName == Locals.LOCAL_TYPES.local.ToString()
                   && _dtoContentURI.URINodeName == Locals.LOCAL_TYPES.localaccountgroup.ToString())
                {
                    var newAccountToLocal = new AccountToLocal
                    {
                        LocalName = addedURI.URIName,
                        LocalDesc = Helpers.GeneralHelpers.NONE,
                        UnitGroupId = 0,
                        UnitGroup = string.Empty,
                        CurrencyGroupId = 0,
                        CurrencyGroup = string.Empty,
                        RealRateId = 0,
                        RealRate = 0,
                        NominalRateId = 0,
                        NominalRate = 0,
                        DataSourceTechId = 0,
                        DataSourceTech = string.Empty,
                        GeoCodeTechId = 0,
                        GeoCodeTech = string.Empty,
                        DataSourcePriceId = 0,
                        DataSourcePrice = string.Empty,
                        GeoCodePriceId = 0,
                        GeoCodePrice = string.Empty,
                        RatingGroupId = 0,
                        RatingGroup = string.Empty,
                        IsDefaultLinkedView = false,
                        //parentid is the same as dtoURI
                        AccountId = _dtoContentURI.URIId,
                        Account = null,
                    };
                    _dataContext.AccountToLocal.Add(newAccountToLocal);
                    _dataContext.Entry(newAccountToLocal).State = EntityState.Added;
                    addedMs.Add(newAccountToLocal);
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                    && _dtoContentURI.URINodeName == Locals.LOCAL_TYPES.localaccountgroup.ToString())
                {
                    //2.0.0 deprecated local linked view calculators, 
                    //retain for potential uses
                    //var newAccountToLocal = new AccountToLocal
                    //{
                    //    LocalName = addedURI.URIName,
                    //    LocalDesc = Helpers.GeneralHelpers.NONE,
                    //    UnitGroupId = 0,
                    //    UnitGroup = string.Empty,
                    //    CurrencyGroupId = 0,
                    //    CurrencyGroup = string.Empty,
                    //    RealRateId = 0,
                    //    RealRate = 0,
                    //    NominalRateId = 0,
                    //    NominalRate = 0,
                    //    DataSourceTechId = 0,
                    //    DataSourceTech = string.Empty,
                    //    GeoCodeTechId = 0,
                    //    GeoCodeTech = string.Empty,
                    //    DataSourcePriceId = 0,
                    //    DataSourcePrice = string.Empty,
                    //    GeoCodePriceId = 0,
                    //    GeoCodePrice = string.Empty,
                    //    RatingGroupId = 0,
                    //    RatingGroup = string.Empty,
                    //    IsDefaultLinkedView = false,
                    //    LinkingXmlDoc = string.Empty,
                    //    AccountId = _dtoContentURI.URIId,
                    //    Account = null,
                    //    LinkedViewId = addedURI.URIId,
                    //    //LinkedView = null
                    //};
                    //_dataContext.AccountToLocal.Add(newAccountToLocal);
                    //_dataContext.Entry(newAccountToLocal).State = EntityState.Added;
                    //addedMs.Add(newAccountToLocal);
                }
            }
            return bHasSet;
        }
        public async Task<bool> DeleteLocals(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteLocals(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURILocals(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURILocals(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteLocals(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                //note that localbases are not deleted -they'll be archived and used for other purposes
                if (deletionURI.URINodeName == Locals.LOCAL_TYPES.local.ToString()
                    || deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    var local = await _dataContext.AccountToLocal.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (local != null)
                    {
                        _dataContext.Entry(local).State = EntityState.Deleted;
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
        public async Task<bool> UpdateLocals(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bHasSet = await UpdateLocals(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURILocals(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURILocals(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateLocals(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.URIToAdd.URINodeName == Locals.LOCAL_TYPES.local.ToString()
                    || edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    var local = await _dataContext.AccountToLocal.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (local != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErrorMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(local), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErrorMsg);
                        _dtoContentURI.ErrorMessage = sErrorMsg;
                        _dataContext.Entry(local).State = EntityState.Modified;
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
            bool bHasSet = await SetURILocals(_dtoContentURI, bSaveInFileSystemContent);
            XElement root = XmlLinq.GetRootXmlDoc();
            bool bHasGoodDoc = false;
            if (_dtoContentURI.URINodeName == Locals.LOCAL_TYPES.localaccountgroup.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == Locals.LOCAL_TYPES.local.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    //add the descendants to the last ancestor added
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName,
                        root.Descendants(Locals.LOCAL_TYPES.localaccountgroup.ToString()).Last());
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
                == Locals.LOCAL_TYPES.local.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.AccountToLocal.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.AccountId,
                        Locals.LOCAL_TYPES.localaccountgroup.ToString(), root);
                }
            }
            return bHasGoodAncestors;
        }
        private async Task<bool> AddDescendants(int id, string nodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            //deserialize objects
            if (nodeName == Locals.LOCAL_TYPES.localaccountgroup.ToString())
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
                        if (_dtoContentURI.URIModels.Account.AccountToLocal != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.Account.AccountToLocal)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, Locals.LOCAL_TYPES.local.ToString(),
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
            else if (nodeName == Locals.LOCAL_TYPES.local.ToString())
            {
                var childObject = await _dataContext.AccountToLocal.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(Locals.LOCAL_TYPES.local.ToString());
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
