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
    ///Purpose:		Entity Framework Member support class
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class MemberModelHelper
    {
        public MemberModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIMember(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == Members.MEMBER_BASE_TYPES.memberbasegroup.ToString())
            {
                //2.0.0: used same as networkaccountgroup, localaccountgroup, and rest of club panel
                //to change isdefaultclub and adddefaultclub, not to edit any other member info
                //no reason to qry on memberbasegroup -won't scale
                var mc = await _dataContext.Member.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {
                    //get all of the clubs that this member belongs to -they'll be stored as clubdefault
                    //can't bind clubinuse
                    var qry = _dataContext
                        .AccountToMember
                        .Where(a => a.MemberId == uri.URIId)
                        .Include(c => c.ClubDefault)
                        .Include(t => t.Member)
                        .OrderBy(m => m.ClubDefault.AccountName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.AccountToMember = await qry.ToAsyncEnumerable().ToList();
                        if (mc.AccountToMember != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .AccountToMember
                                .Where(a => a.MemberId == uri.URIId)
                               .Count();
                        }
                    }
                    uri.URIModels.Member = mc;
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Members.MEMBER_BASE_TYPES.memberbase.ToString())
            {
                var qry = _dataContext
                    .Member
                    .Include(t => t.MemberClass)
                    .Where(r => r.PKId == uri.URIId);
                if (qry != null)
                {
                    uri.URIModels.Member = await qry.FirstOrDefaultAsync();
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                }
            }
            else if (uri.URINodeName == Members.MEMBER_TYPES.memberaccountgroup.ToString())
            {
                var mc = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (mc != null)
                {
                    var qry = _dataContext
                        .AccountToMember
                        .Where(a => a.AccountId == uri.URIId)
                        .Include(t => t.Member)
                        .OrderBy(m => m.Member.MemberLastName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        mc.AccountToMember = await qry.ToAsyncEnumerable().ToList();
                        if (mc.AccountToMember != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .AccountToMember
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
                == Members.MEMBER_TYPES.member.ToString())
            {
                var qry = _dataContext
                    .AccountToMember
                    .Include(t => t.Member)
                    .Where(r => r.PKId == uri.URIId);
                if (qry != null)
                {
                    uri.URIModels.AccountToMember = await qry.FirstOrDefaultAsync();
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

        public void SetURIMember(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Members.MEMBER_BASE_TYPES.memberbasegroup.ToString())
            {
                var qryRC = _dataContext
                    .MemberClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Members.MEMBER_BASE_TYPES.memberbase.ToString())
            {
                var qryR = _dataContext
                    .Member
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Members.MEMBER_TYPES.memberaccountgroup.ToString())
            {
                var qryRC = _dataContext
                    .MemberClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Members.MEMBER_TYPES.member.ToString())
            {
                var qryR = _dataContext
                    .Member
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddMember(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            //store updated resources ids in lists
            List<AccountToMember> addedMs = new List<AccountToMember>();
            bool bHasSet = AddMember(argumentsEdits.SelectionsToAdd, addedMs);
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
                        bHasSet = await SetURIMember(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIMember(_dtoContentURI, false);
                }
            }
            else
            {
                if (bHasSet)
                {
                    bIsAdded = true;
                    bHasSet = await SetURIMember(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private bool AddMember(List<ContentURI> addedURIs, List<AccountToMember> addedMs)
        {
            bool bHasSet = false;
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
                //2.0.0 adddefault club and selectclub are made use base member groups 
                if (addedURI.URINodeName == Accounts.ACCOUNT_TYPES.account.ToString()
                    && _dtoContentURI.URINodeName == Members.MEMBER_BASE_TYPES.memberbasegroup.ToString())
                {
                    if (_dtoContentURI.URIDataManager.ServerSubActionType 
                        == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        //2.0.0 moved here from insertnewclubforloggedinmember from custom memberrepository action
                        int iAccountId = 0;
                        if (_dtoContentURI.URIMember != null)
                        {
                            if (_dtoContentURI.URIMember.Member != null)
                            {
                                if (_dtoContentURI.URIMember.Member.PKId != 0)
                                {
                                    var newAccount = new Account
                                    {
                                        AccountName = "A. New Club",
                                        AccountEmail = _dtoContentURI.URIMember.Member.MemberEmail,
                                        AccountURI = string.Empty,
                                        AccountDesc = string.Empty,
                                        AccountLongDesc = string.Empty,
                                        AccountClassId = _dtoContentURI.URIMember.Member.MemberClassId,
                                        GeoRegionId = _dtoContentURI.URIMember.Member.GeoRegionId,
                                        GeoRegion = null,
                                        AccountClass = null,
                                        AccountToAddIn = null,
                                        AccountToAudit = null,
                                        AccountToCredit = null,
                                        AccountToLocal = null,
                                        AccountToMember = null,
                                        AccountToNetwork = null,
                                        AccountToPayment = null,
                                        AccountToService = null
                                    };
                                    //join table (each member can join one or more accounts (clubs))
                                    var newAccountToMember = new AccountToMember
                                    {
                                        MemberId = _dtoContentURI.URIMember.Member.PKId,
                                        //each member is the coordinator of their own club
                                        MemberRole = AppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString(),
                                        //next step after insertion switches the default club to this club
                                        IsDefaultClub = false,
                                        ClubDefault = null,
                                        Member = null
                                    };
                                    try
                                    {
                                        _dataContext.Entry(newAccount).State = EntityState.Added;
                                        _dataContext.SaveChanges();
                                        newAccountToMember.AccountId = newAccount.PKId;
                                        _dataContext.Entry(newAccountToMember).State = EntityState.Added;
                                        _dataContext.SaveChanges();
                                        iAccountId = newAccount.PKId;
                                        //2.0.0 deprecated because update default club is manual
                                        //also disrupts regular urimember use (i.e. insert audittrail)
                                        //newAccountToMember.Member = new Member(_dtoContentURI.URIMember.Member);
                                        //_dtoContentURI.URIMember = newAccountToMember;
                                    }
                                    catch
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                        if (iAccountId != 0)
                        {
                            bHasSet = true;
                            //they must now do this by hand, but the new club is at the top of the displayed list
                            ////set their default club to the new club so that it can be edited
                            //IMemberRepositoryEF memberRepository = new MemberR(_dtoContentURI);
                            //bool bHasUpdate = await _repository.UpdateDefaultClubAsync(
                            //    _dtoContentURI, Members.ISDEFAULTCLUB, iAccountId);

                        }
                    }
                }
                else if (addedURI.URINodeName == Members.MEMBER_BASE_TYPES.memberbase.ToString()
                        && _dtoContentURI.URINodeName == Members.MEMBER_TYPES.memberaccountgroup.ToString())
                {
                    //this is the select new member pattern
                    var newAccountToMember = new AccountToMember
                    {
                        MemberRole = Members.MEMBER_ROLE_TYPES.contributor.ToString(),
                        IsDefaultClub = false,
                        AccountId = _dtoContentURI.URIId,
                        ClubDefault = null,
                        MemberId = addedURI.URIId,
                        Member = null
                    };
                    _dataContext.AccountToMember.Add(newAccountToMember);
                    _dataContext.Entry(newAccountToMember).State = EntityState.Added;
                    addedMs.Add(newAccountToMember);
                    bHasSet = true;
                }
            }
            return bHasSet;
        }
        public async Task<bool> DeleteMember(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            bool bHasSet = false;
            //last member deletion will also try to delete club
            int iAccountId = await NeedsClubDeletion(argumentsEdits.SelectionsToAdd);
            if (string.IsNullOrEmpty(_dtoContentURI.ErrorMessage))
            {
                //store updated resources ids in node name and id dictionary
                Dictionary<string, int> deletedIds = new Dictionary<string, int>();
                bHasSet = await DeleteMember(argumentsEdits.SelectionsToAdd, deletedIds);
                if (deletedIds.Count > 0 && string.IsNullOrEmpty(_dtoContentURI.ErrorMessage))
                {
                    try
                    {
                        int iNotUsed = await _dataContext.SaveChangesAsync();
                        bIsDeleted = true;
                        //update the dtoContentURI collection
                        bHasSet = await SetURIMember(_dtoContentURI, false);
                    }
                    catch (Exception e)
                    {
                        _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            e.ToString(), "ERROR_INTRO");
                    }
                    if (_dtoContentURI.ErrorMessage.Length > 0)
                    {
                        bHasSet = await SetURIMember(_dtoContentURI, false);
                    }
                }
            }
            else
            {
                bHasSet = await SetURIMember(_dtoContentURI, false);
            }
            if (iAccountId != 0)
            {
                //db cascading deletes won't work here -the members have to deleted first
                //chance of stranded clubs, but that might be a good security recovery feature
                bHasSet = await DeleteLastMemberClub(iAccountId);
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteMember(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                //note that memberbases are not deleted -they'll be archived and used for other purposes
                if (deletionURI.URINodeName == Members.MEMBER_TYPES.member.ToString())
                {
                    var member = await _dataContext.AccountToMember.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (member != null)
                    {
                        _dataContext.Entry(member).State = EntityState.Deleted;
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
        public async Task<bool> UpdateMember(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bHasSet = await UpdateMember(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    //update the dtoContentURI collection
                    bHasSet = await SetURIMember(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIMember(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateMember(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                bHasSet = await ChangeMemberPropertyValues(edit, updatedIds);
                if (string.IsNullOrEmpty(_dtoContentURI.ErrorMessage))
                {
                    if (edit.URIToAdd.URINodeName == Members.MEMBER_BASE_TYPES.memberbase.ToString())
                    {
                        bHasSet = await UpdateBaseMemberEntity(edit, updatedIds);
                    }
                    else if (edit.URIToAdd.URINodeName == Members.MEMBER_TYPES.member.ToString())
                    {
                        var member = await _dataContext.AccountToMember.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (member != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(member), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(member).State = EntityState.Modified;
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
                    bHasSet = await SetURIMember(_dtoContentURI, false);
                }
            }
            return bHasSet;
        }
        private async Task<bool> UpdateBaseMemberEntity(EditHelper.ArgumentsEdits edit, Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            var member = await _dataContext.Member.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
            if (member != null)
            {
                RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                //update the property to the new value
                string sErroMsg = string.Empty;
                EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(member), edit.EditAttName,
                    edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                _dtoContentURI.ErrorMessage = sErroMsg;
                _dataContext.Entry(member).State = EntityState.Modified;
                sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                if (updatedIds.ContainsKey(sKeyName) == false)
                {
                    updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                }
            }
            return bHasSet;
        }
        private async Task<int> NeedsClubDeletion(List<ContentURI> deletionURIs)
        {
            int iAccountId = 0;
            //workflow deletes the club if the last member is being deleted
            //if the club can't be deleted because it has data, generates an error message
            ContentURI deletionURI = deletionURIs.FirstOrDefault();
            var member = await _dataContext.AccountToMember.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
            if (member != null)
            {
                IMemberRepositoryEF rep = new SqlRepositories.MemberRepository(deletionURI);
                int iClubToMemberCount = await rep.GetClubToMemberCountAsync(member.AccountId);
                //last member can only be deleted if the club can be deleted
                if (iClubToMemberCount == 1 || deletionURIs.Count == iClubToMemberCount)
                {
                    iAccountId = member.AccountId;
                }
            }
            return iAccountId;
        }
        private async Task<bool> DeleteLastMemberClub(int accountId)
        {
            bool bHasSet = true;
            //stored procedure that allows cascading deletes would be better
            //would cascade delete both the members and the audits

            //workflow deletes the club if the last member is being deleted
            //if the club can't be deleted because it has data, generates an error message
            //do it here so that the AccountModelHelper and ContentRepository 
            //don't have to be opened up to public methods with greater security risk
            var account = await _dataContext.Account.SingleOrDefaultAsync(x => x.PKId == accountId);
            if (account != null)
            {
                _dataContext.Entry(account).State = EntityState.Deleted;
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIMember(_dtoContentURI, false);
                }
            }
            return bHasSet;
        }
        private async Task<bool> ChangeMemberPropertyValues(EditHelper.ArgumentsEdits argumentsEdits, Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            if (argumentsEdits.EditAttName.ToLower().Contains(
                Members.MEMBER_USERNAME.ToLower()))
            {
                IMemberRepositoryEF rep 
                    = new SqlRepositories.MemberRepository(argumentsEdits.URIToEdit);
                int iUserNameCount = await rep.GetUserNameCountForMemberAsync(
                    argumentsEdits.EditAttValue);
                if (iUserNameCount != 0)
                {
                    _dtoContentURI.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "EDIT_USERNAMEINUSE");
                }
            }
            else if (argumentsEdits.EditAttName.ToLower().Contains(
                Members.MEMBER_EMAIL.ToLower()))
            {
                IMemberRepositoryEF rep 
                    = new SqlRepositories.MemberRepository(argumentsEdits.URIToEdit);
                int iEmailCount = await rep.GetEmailCountForMemberAsync(
                    argumentsEdits.EditAttValue);
                if (iEmailCount != 0)
                {
                    _dtoContentURI.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "EDIT_EMAILINUSE");
                }
            }
            else if (argumentsEdits.EditAttName.ToLower().Contains(
                AppHelpers.Members.MEMBER_ROLE.ToLower()))
            {
                //the club founder (topjoin) can't change role from coordinator
                //fallback so that all clubs have at least one coordinator who
                //can edit the club's model
                bool bJoinIdsMatch = false;
                EditHelper editHelper = new EditHelper();
                //don't use uri.urinodename here (can be a parent uri)
                bJoinIdsMatch = await editHelper.JoinIdsAreMatches(_dtoContentURI, argumentsEdits,
                    AppHelpers.Members.MEMBER_TYPES.member.ToString(),
                    _dtoContentURI.URIMember.MemberId.ToString());
                if (bJoinIdsMatch)
                {
                    _dtoContentURI.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "EDIT_NOFOUNDERCHANGE");
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
            //file system uses all rows (the webconfig.pagesizeforfiles)
            bool bHasSet = await SetURIMember(_dtoContentURI, bSaveInFileSystemContent);
            XElement root = XmlLinq.GetRootXmlDoc();
            bool bHasGoodDoc = false;
            if (_dtoContentURI.URINodeName == Members.MEMBER_BASE_TYPES.memberbasegroup.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == Members.MEMBER_BASE_TYPES.memberbase.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName,
                        root.Descendants(Members.MEMBER_BASE_TYPES.memberbasegroup.ToString()).Last());
                }
            }
            else if (_dtoContentURI.URINodeName == Members.MEMBER_TYPES.memberaccountgroup.ToString())
            {
                bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
            }
            else if (_dtoContentURI.URINodeName == Members.MEMBER_TYPES.member.ToString())
            {
                //all content starts with group nodes
                bHasGoodDoc = await AddAncestors(root);
                if (bHasGoodDoc)
                {
                    //add the descendants to the last ancestor added
                    bHasGoodDoc = await AddDescendants(_dtoContentURI.URIId,
                        _dtoContentURI.URINodeName,
                        root.Descendants(Members.MEMBER_TYPES.memberaccountgroup.ToString()).Last());
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
                == Members.MEMBER_BASE_TYPES.memberbase.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.Member.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.MemberClassId,
                        Members.MEMBER_BASE_TYPES.memberbasegroup.ToString(), root);
                }
            }
            else if (_dtoContentURI.URINodeName
                == Members.MEMBER_TYPES.member.ToString())
            {
                //only needs parent (others can add additional conditions)
                var currentObject = await _dataContext.AccountToMember.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                if (currentObject != null)
                {
                    bHasGoodAncestors = await AddDescendants(currentObject.AccountId,
                        Members.MEMBER_TYPES.memberaccountgroup.ToString(), root);
                }
            }
            return bHasGoodAncestors;
        }
        private async Task<bool> AddDescendants(int id, string nodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            //deserialize objects
            if (nodeName == Members.MEMBER_BASE_TYPES.memberbasegroup.ToString())
            {
                XElement currentNode = new XElement(nodeName);
                //iterate properties to generate xml
                var currentObject = await _dataContext.MemberClass.SingleOrDefaultAsync(x => x.PKId == id);
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
                    if (_dtoContentURI.URIModels.MemberClass != null)
                    {
                        if (_dtoContentURI.URIModels.MemberClass.Member != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.MemberClass.Member)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, Members.MEMBER_BASE_TYPES.memberbase.ToString(),
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
            else if (nodeName == Members.MEMBER_BASE_TYPES.memberbase.ToString())
            {
                var childObject = await _dataContext.Member.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(Members.MEMBER_BASE_TYPES.memberbase.ToString());
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
            else if (nodeName == Members.MEMBER_TYPES.memberaccountgroup.ToString())
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
                        if (_dtoContentURI.URIModels.Account.AccountToMember != null)
                        {
                            foreach (var child in _dtoContentURI.URIModels.Account.AccountToMember)
                            {
                                bHasGoodDescendants = await AddDescendants(child.PKId, Members.MEMBER_TYPES.member.ToString(),
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
            else if (nodeName == Members.MEMBER_TYPES.member.ToString())
            {
                var childObject = await _dataContext.AccountToMember.SingleOrDefaultAsync(x => x.PKId == id);
                if (childObject != null)
                {
                    var childObjContext = _dataContext.Entry(childObject);
                    XElement childNode = new XElement(Members.MEMBER_TYPES.member.ToString());
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
