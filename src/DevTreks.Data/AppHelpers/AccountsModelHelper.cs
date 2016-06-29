using DevTreks.Data.DataAccess;
using DevTreks.Data.EditHelpers;
using DevTreks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Entity Framework Account support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class AccountModelHelper
    {
        public AccountModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIAccount(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == Accounts.ACCOUNT_TYPES.accountgroup.ToString())
            {
                var rc = await _dataContext.AccountClass
                    .SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .Account
                        .Where(a => a.AccountClass.PKId == uri.URIId)
                        .OrderBy(rp => rp.AccountName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.Account = await qry.ToAsyncEnumerable().ToList();
                        if (rc.Account != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                                .Account
                                .Where(a => a.AccountClass.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.AccountClass = rc;
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Accounts.ACCOUNT_TYPES.account.ToString())
            {
                var qry = _dataContext
                    .Account
                    .Include(t => t.AccountClass)
                    .Where(r => r.PKId == uri.URIId);
                if (qry != null)
                {
                    uri.URIModels.Account = await qry.FirstOrDefaultAsync();
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

        public void SetURIAccount(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Accounts.ACCOUNT_TYPES.accountgroup.ToString())
            {
                var qryRP
                        = _dataContext
                        .AccountClass
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Accounts.ACCOUNT_TYPES.account.ToString())
            {
                var qryR = _dataContext
                    .Account
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddAccount(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            //store updated accounts ids in lists
            List<AccountClass> addedRPs = new List<AccountClass>();
            List<Account> addedRs = new List<Account>();
            AddAccount(argumentsEdits.SelectionsToAdd, addedRPs, addedRs);
            bool bHasSet = false;
            if (addedRPs.Count > 0 || addedRs.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of accounts
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIAccount(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIAccount(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private void AddAccount(List<ContentURI> addedURIs, List<AccountClass> addedACs, 
            List<Account> addedAs)
        {
            //these are not used; but hold them for customization in the future
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
                if (addedURI.URINodeName == Accounts.ACCOUNT_TYPES.accountgroup.ToString())
                {
                    var newAccountClass = new AccountClass
                    {
                        AccountClassNum = Helpers.GeneralHelpers.NONE,
                        AccountClassName = addedURI.URIName,
                        AccountClassDesc = Helpers.GeneralHelpers.NONE,
                        Account = null
                    };
                    _dataContext.AccountClass.Add(newAccountClass);
                    _dataContext.Entry(newAccountClass).State = EntityState.Added;
                    addedACs.Add(newAccountClass);
                }
                else if (addedURI.URINodeName == Accounts.ACCOUNT_TYPES.account.ToString())
                {
                    var newAccount = new Account
                    {
                        AccountName = addedURI.URIName,
                        AccountDesc = Helpers.GeneralHelpers.NONE,
                        AccountLongDesc = Helpers.GeneralHelpers.NONE,
                        AccountEmail = Helpers.GeneralHelpers.NONE,
                        AccountURI = Helpers.GeneralHelpers.NONE,
                        AccountClassId = iParentId,
                        AccountClass = null,
                        GeoRegionId = 0,
                        GeoRegion = null,
                        AccountToMember = null,
                        AccountToAudit = null,
                        AccountToService = null,
                        AccountToCredit = null,
                        AccountToAddIn = null,
                        AccountToLocal = null,
                        AccountToNetwork = null
                        //AccountToPayment = null
                    };
                    _dataContext.Account.Add(newAccount);
                    _dataContext.Entry(newAccount).State = EntityState.Added;
                    addedAs.Add(newAccount);
                }
            }
        }
        public async Task<bool> DeleteAccount(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            bool bHasSet = false;
            //store updated accounts ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bHasSet = await DeleteAccount(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIAccount(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIAccount(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteAccount(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            string sKeyName = string.Empty;
            bool bHasSet = false;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Accounts.ACCOUNT_TYPES.accountgroup.ToString())
                {
                    var accountClass = await _dataContext.AccountClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (accountClass != null)
                    {
                        _dataContext.Entry(accountClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Accounts.ACCOUNT_TYPES.account.ToString())
                {
                    var account = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (account != null)
                    {
                        _dataContext.Entry(account).State = EntityState.Deleted;
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
        public async Task<bool> UpdateAccount(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = false;
            //store updated accounts ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bHasSet = await UpdateAccount(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    bHasSet = await SetURIAccount(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIAccount(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateAccount(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            string sKeyName = string.Empty;
            bool bHasSet = false;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                bHasSet = await ChangeAccountPropertyValues(edit);
                if (string.IsNullOrEmpty(_dtoContentURI.ErrorMessage))
                {
                    if (edit.URIToAdd.URINodeName == Accounts.ACCOUNT_TYPES.accountgroup.ToString())
                    {
                        var accountClass = await _dataContext.AccountClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (accountClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(accountClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(accountClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Accounts.ACCOUNT_TYPES.account.ToString())
                    {
                        var account = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (account != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(account), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(account).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                }
                else
                {
                    bHasSet = await SetURIAccount(_dtoContentURI, false);
                }
            }
            return bHasSet;
        }
        public async Task<bool> ChangeAccountPropertyValues(
            EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bHasSet = false;
            int iEmailCount = 0;
            if (argumentsEdits.EditAttName.ToLower().Contains(
                AppHelpers.Accounts.ACCOUNT_EMAIL.ToLower()))
            {
                IMemberRepositoryEF rep 
                    = new SqlRepositories.MemberRepository(argumentsEdits.URIToEdit);
                iEmailCount = await rep.GetEmailCountForClubAsync(
                    argumentsEdits.EditAttValue);
                rep = null;
                if (iEmailCount != 0)
                {
                    _dtoContentURI.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "EDIT_EMAILINUSE");
                }
                else
                {
                    bHasSet = true;
                }
            }
            return bHasSet;
        }
        public async Task<bool> SaveURIFirstDocAsync()
        {
            bool bHasSavedDoc = false;
            bool bHasSet = false;
            if (string.IsNullOrEmpty(_dtoContentURI.URIClub.ClubDocFullPath))
            {
                //when the file path is not set, too much data is prevented
                return true;
            }
            bool bSaveInFileSystemContent = true;
            bHasSet = await SetURIAccount(_dtoContentURI, bSaveInFileSystemContent);
            XElement root = XmlLinq.GetRootXmlDoc();
            bool bHasGoodDoc = false;
            if (_dtoContentURI.URINodeName == Accounts.ACCOUNT_TYPES.accountgroup.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == Accounts.ACCOUNT_TYPES.account.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName,
                        root.Descendants(Accounts.ACCOUNT_TYPES.accountgroup.ToString()).Last());
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
                == Accounts.ACCOUNT_TYPES.account.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.AccountClassId,
                        Accounts.ACCOUNT_TYPES.accountgroup.ToString(), root);
                }
            }
            return bHasGoodAncestors;
        }
        private async Task<bool> AddDescendants(int id, string nodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            //deserialize objects
            if (nodeName == Accounts.ACCOUNT_TYPES.accountgroup.ToString())
            {
                XElement currentNode = new XElement(nodeName);
                //iterate properties to generate xml
                var currentObject = await _dataContext.AccountClass.SingleOrDefaultAsync(x => x.PKId == id);
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
                    if (_dtoContentURI.URIModels.AccountClass != null)
                    {
                        if (_dtoContentURI.URIModels.AccountClass.Account != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.AccountClass.Account)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, Accounts.ACCOUNT_TYPES.account.ToString(),
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
            else if (nodeName == Accounts.ACCOUNT_TYPES.account.ToString())
            {
                var childObject = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(Accounts.ACCOUNT_TYPES.account.ToString());
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
