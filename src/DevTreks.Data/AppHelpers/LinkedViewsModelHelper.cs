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
    ///Purpose:		Entity Framework LinkedView support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class LinkedViewModelHelper
    {
        public LinkedViewModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            //has to reference dtouri, not a new one
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURILinkedView(ContentURI uri,
            bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var s = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (s != null)
                {
                    var qry = _dataContext
                        .LinkedViewClass
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.LinkedViewClassName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.LinkedViewClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.LinkedViewClass != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .CostSystemToOutput
                                .Where(a => a.CostSystemToOutcome.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.Service = s;
                    //need the budgettype collection set too
                    bHasSet = await SetURILinkedViewType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
            {
                var rc = await _dataContext.LinkedViewClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .LinkedViewPack
                        .Where(a => a.LinkedViewClassId == uri.URIId)
                        .OrderBy(m => m.LinkedViewPackName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.LinkedViewPack = await qry.ToAsyncEnumerable().ToList();
                        if (rc.LinkedViewPack != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .LinkedViewPack
                                .Where(a => a.LinkedViewClassId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.LinkedViewClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                var rp = await _dataContext.LinkedViewPack.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .LinkedView
                        .Where(a => a.LinkedViewPackId == uri.URIId)
                        .OrderBy(m => m.LinkedViewName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.LinkedView = await qry.ToAsyncEnumerable().ToList();
                        if (rp.LinkedView != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .LinkedView
                                .Where(a => a.LinkedViewPackId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.LinkedViewPack = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                var r = await _dataContext.LinkedView.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (r != null)
                {
                    var qry = _dataContext
                        .LinkedViewToResourcePack
                        .Include(t => t.ResourcePack)
                        .Where(a => a.LinkedViewId == uri.URIId)
                        .OrderBy(m => m.SortLabel)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        r.LinkedViewToResourcePack = await qry.ToAsyncEnumerable().ToList();
                        if (r.LinkedViewToResourcePack != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .LinkedViewToResourcePack
                                .Where(a => a.LinkedViewId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.LinkedView = r;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
            {
                var qry = _dataContext
                    .LinkedViewToResourcePack
                    .Include(t => t.ResourcePack)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.LinkedViewToResourcePack = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURILinkedViewType(Service s, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.LinkedViewType = new List<LinkedViewType>();
                //filter the linkedview types by this service's network
                if (s != null)
                {
                    //the rt.PKId <= 2 condition picks up calculator and analyzer categories
                    var qry = _dataContext
                        .LinkedViewType
                        .Where(rt => (rt.NetworkId == s.NetworkId)
                            || (rt.PKId <= 2))
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.LinkedViewType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.LinkedViewType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    LinkedViewType rt = await _dataContext.LinkedViewType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.LinkedViewType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void SetURILinkedView(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
            {
                var qryRC = _dataContext
                    .LinkedViewClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                var qryRP
                        = _dataContext
                        .LinkedViewPack
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                var qryR = _dataContext
                    .LinkedView
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddLinkedView(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            //store updated linkedviews ids in lists
            List<LinkedViewClass> addedRCs = new List<LinkedViewClass>();
            List<LinkedViewPack> addedRPs = new List<LinkedViewPack>();
            List<LinkedView> addedRs = new List<LinkedView>();
            List<LinkedViewToResourcePack> addedLVRPs = new List<LinkedViewToResourcePack>();
            bool bHasSet = await AddLinkedView(argumentsEdits.SelectionsToAdd, addedRCs, addedRPs, addedRs, addedLVRPs);
            //int iNewId = 0;
            if (addedRCs.Count > 0 || addedRPs.Count > 0 || addedRs.Count > 0 || addedLVRPs.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of linkedviews
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURILinkedView(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURILinkedView(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddLinkedView(List<ContentURI> addedURIs, List<LinkedViewClass> addedRCs,
            List<LinkedViewPack> addedRPs, List<LinkedView> addedRs, List<LinkedViewToResourcePack> addedLVRPs)
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
                if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
                {
                    var newLinkedViewClass = new LinkedViewClass
                    {
                        LinkedViewClassNum = Helpers.GeneralHelpers.NONE,
                        LinkedViewClassName = addedURI.URIName,
                        LinkedViewClassDesc = Helpers.GeneralHelpers.NONE,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        LinkedViewType = null,
                        LinkedViewPack = null
                    };
                    _dataContext.LinkedViewClass.Add(newLinkedViewClass);
                    _dataContext.Entry(newLinkedViewClass).State = EntityState.Added;
                    addedRCs.Add(newLinkedViewClass);
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                {
                    var newLinkedViewPack = new LinkedViewPack
                    {
                        LinkedViewPackNum = Helpers.GeneralHelpers.NONE,
                        LinkedViewPackName = addedURI.URIName,
                        LinkedViewPackDesc = Helpers.GeneralHelpers.NONE,
                        LinkedViewPackKeywords = Helpers.GeneralHelpers.NONE,
                        LinkedViewPackDocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        LinkedViewPackLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        LinkedViewPackMetaDataXml = string.Empty,
                        LinkedViewClassId = iParentId,
                        LinkedViewClass = null,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewPack.Add(newLinkedViewPack);
                    _dataContext.Entry(newLinkedViewPack).State = EntityState.Added;
                    addedRPs.Add(newLinkedViewPack);
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    var newLinkedView = new LinkedView
                    {
                        LinkedViewNum = Helpers.GeneralHelpers.NONE,
                        LinkedViewName = addedURI.URIName,
                        LinkedViewDesc = Helpers.GeneralHelpers.NONE,
                        LinkedViewFileExtensionType = Helpers.GeneralHelpers.NONE,
                        LinkedViewLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        LinkedViewFileName = Helpers.GeneralHelpers.NONE,
                        LinkedViewXml = Helpers.GeneralHelpers.NONE,
                        LinkedViewAddInName = Helpers.GeneralHelpers.NONE,
                        LinkedViewAddInHostName = Helpers.GeneralHelpers.NONE,
                        LinkedViewPackId = iParentId,
                        LinkedViewPack = null,

                        AccountToAddIn = null,
                        LinkedViewToBudgetSystem = null,
                        LinkedViewToBudgetSystemToEnterprise = null,
                        LinkedViewToBudgetSystemToTime = null,
                        LinkedViewToComponent = null,
                        LinkedViewToComponentClass = null,
                        LinkedViewToCostSystem = null,
                        LinkedViewToCostSystemToPractice = null,
                        LinkedViewToCostSystemToTime = null,
                        LinkedViewToDevPackJoin = null,
                        LinkedViewToDevPackPartJoin = null,
                        LinkedViewToInput = null,
                        LinkedViewToInputClass = null,
                        LinkedViewToInputSeries = null,
                        LinkedViewToResourcePack = null,
                        LinkedViewToOperation = null,
                        LinkedViewToOperationClass = null,
                        LinkedViewToOutput = null,
                        LinkedViewToOutputClass = null,
                        LinkedViewToOutputSeries = null,
                        LinkedViewToResource = null
                    };
                    _dataContext.LinkedView.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    addedRs.Add(newLinkedView);
                }
                else if (addedURI.URINodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
                {
                    var newLinkedViewResourcePack = new LinkedViewToResourcePack
                    {
                        SortLabel = Helpers.GeneralHelpers.NONE,
                        LinkedViewId = iParentId,
                        LinkedView = null,
                        ResourcePackId = addedURI.URIId,
                        ResourcePack = null
                    };
                    _dataContext.LinkedViewToResourcePack.Add(newLinkedViewResourcePack);
                    _dataContext.Entry(newLinkedViewResourcePack).State = EntityState.Added;
                    addedLVRPs.Add(newLinkedViewResourcePack);
                }
            }
            return bHasSet;
        }
        public async Task<bool> DeleteLinkedView(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated linkedviews ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteLinkedView(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURILinkedView(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURILinkedView(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteLinkedView(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewtype.ToString())
                {
                    var linkedviewType = await _dataContext.LinkedViewType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (linkedviewType != null)
                    {
                        _dataContext.Entry(linkedviewType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
                {
                    var linkedviewClass = await _dataContext.LinkedViewClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (linkedviewClass != null)
                    {
                        _dataContext.Entry(linkedviewClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                {
                    var linkedviewPack = await _dataContext.LinkedViewPack.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (linkedviewPack != null)
                    {
                        _dataContext.Entry(linkedviewPack).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    var linkedview = await _dataContext.LinkedView.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (linkedview != null)
                    {
                        _dataContext.Entry(linkedview).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
                {
                    var linkedviewresourcepack = await _dataContext.LinkedViewToResourcePack.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (linkedviewresourcepack != null)
                    {
                        _dataContext.Entry(linkedviewresourcepack).State = EntityState.Deleted;
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
        public async Task<bool> UpdateLinkedView(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            //store updated linkedviews ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bHasSet = await UpdateLinkedView(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    //linkedviews can update the linkedviewfileextensiontype when an extension is saved
                    //but don't want _dtoContentURI collection
                    if (_dtoContentURI.URIDataManager.ServerSubActionType
                        != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                    {
                        bHasSet = await SetURILinkedView(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURILinkedView(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateLinkedView(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewtype.ToString())
                {
                    var linkedviewType = await _dataContext.LinkedViewType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (linkedviewType != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(linkedviewType), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(linkedviewType).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
                else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
                {
                    var linkedviewClass = await _dataContext.LinkedViewClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (linkedviewClass != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(linkedviewClass), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(linkedviewClass).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
                else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                {
                    var linkedviewPack = await _dataContext.LinkedViewPack.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (linkedviewPack != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(linkedviewPack), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(linkedviewPack).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
                else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    var linkedview = await _dataContext.LinkedView.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (linkedview != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(linkedview), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(linkedview).State = EntityState.Modified;
                        sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                        if (updatedIds.ContainsKey(sKeyName) == false)
                        {
                            updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                        }
                    }
                }
                else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
                {
                    var linkedviewResourcePack = await _dataContext.LinkedViewToResourcePack.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                    if (linkedviewResourcePack != null)
                    {
                        RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                        //update the property to the new value
                        string sErroMsg = string.Empty;
                        EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(linkedviewResourcePack), edit.EditAttName,
                            edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                        _dtoContentURI.ErrorMessage = sErroMsg;
                        _dataContext.Entry(linkedviewResourcePack).State = EntityState.Modified;
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
            //make sure to return a regular collection
            bool bSaveInFileSystemContent = false;
            bool bHasSet = await SetURILinkedView(_dtoContentURI, bSaveInFileSystemContent);
            XElement root = XmlLinq.GetRootXmlDoc();
            bool bHasGoodDoc = false;
            _parentName = string.Empty;
            //add any missing ancestor up to servicebase root.firstchild
            bool bHasGoodAncestors = await AddAncestors(_dtoContentURI.URIId, _dtoContentURI.URINodeName,
                root);
            if (bHasGoodAncestors
                && !string.IsNullOrEmpty(_parentName))
            {
                //add all descendants below _dtoContentURI (but use a new ContentURI
                ContentURI tempURI = new ContentURI();
                bHasGoodDoc = await AddDescendants(tempURI, _parentId, _parentName, _dtoContentURI.URIId,
                    _dtoContentURI.URINodeName, root);
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

        private async Task<bool> AddAncestors(int id, string nodeName, XElement root)
        {
            bool bHasGoodAncestors = false;
            if (root.HasElements)
            {
                if (_dtoContentURI.URINodeName == nodeName)
                {
                    //don't insert self
                    _parentName = nodeName;
                    _parentId = id;
                    return true;
                }
            }
            //deserialize objects
            if (nodeName == LinkedViews.LINKEDVIEWS_TYPES.servicebase.ToString())
            {
                var currentObject = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    _parentName = nodeName;
                    _parentId = id;
                    if (_dtoContentURI.URINodeName != LinkedViews.LINKEDVIEWS_TYPES.servicebase.ToString())
                    {
                        XElement el = MakeServiceBaseXml(currentObject);
                        if (el != null)
                        {
                            root.AddFirst(el);
                            bHasGoodAncestors = true;
                        }
                    }
                    else
                    {
                        bHasGoodAncestors = true;
                    }
                }
            }
            else if (nodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
            {
                var currentObject = await _dataContext.LinkedViewClass.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    id = currentObject.ServiceId;
                    nodeName = LinkedViews.LINKEDVIEWS_TYPES.servicebase.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
                        {
                            XElement el = MakeLinkedViewClassXml(currentObject);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName);
                        }
                    }

                }
            }
            else if (nodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                var currentObject = await _dataContext.LinkedViewPack.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    id = currentObject.LinkedViewClassId;
                    nodeName = LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                        {
                            XElement el = MakeLinkedViewPackXml(currentObject);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName);
                        }
                    }

                }
            }
            else if (nodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                var currentObject = await _dataContext.LinkedView.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    id = currentObject.LinkedViewPackId;
                    nodeName = LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            XElement el = MakeLinkedViewXml(currentObject);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName);
                        }
                    }

                }
            }
            else if (nodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
            {
                var currentObject = await GetLinkedViewToResourcePack(id);
                if (currentObject != null)
                {
                    id = currentObject.LinkedViewId;
                    nodeName = LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        //return bhasgoodancestors
                    }

                }
            }
            return bHasGoodAncestors;
        }
        private async Task<bool> AddDescendants(ContentURI tempURI, int parentId, string parentNodeName,
            int childId, string childNodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            bool bHasBeenAdded = false;
            //deserialize objects
            if (childNodeName == LinkedViews.LINKEDVIEWS_TYPES.servicebase.ToString())
            {
                var obj1 = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj1 != null)
                {
                    XElement el = MakeServiceBaseXml(obj1);
                    if (el != null)
                    {
                        root.AddFirst(el);
                    }
                    bool bHasSet = await SetTempURILinkedView(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.LinkedViewClass != null)
                    {
                        if (tempURI.URIModels.Service.LinkedViewClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.LinkedViewClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
            {
                var obj2 = await _dataContext.LinkedViewClass.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj2 != null)
                {
                    XElement el = MakeLinkedViewClassXml(obj2);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != LinkedViews.LINKEDVIEWS_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURILinkedView(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.LinkedViewClass.LinkedViewPack != null)
                            {
                                if (tempURI.URIModels.LinkedViewClass.LinkedViewPack.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.LinkedViewClass.LinkedViewPack)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString(),
                                        root);
                                }
                            }
                            else
                            {
                                bHasGoodDescendants = true;
                            }
                        }
                        else
                        {
                            bHasGoodDescendants = true;
                        }
                    }
                }
            }
            else if (childNodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                var obj3 = await _dataContext.LinkedViewPack.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj3 != null)
                {
                    XElement el = MakeLinkedViewPackXml(obj3);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURILinkedView(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.LinkedViewPack.LinkedView != null)
                        {
                            if (tempURI.URIModels.LinkedViewPack.LinkedView.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.LinkedViewPack.LinkedView)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                                    root);
                            }
                        }
                        else
                        {
                            bHasGoodDescendants = true;
                        }
                    }
                }
            }
            else if (childNodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                var obj4 = await _dataContext.LinkedView.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj4 != null)
                {
                    XElement el = MakeLinkedViewXml(obj4);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURILinkedView(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.LinkedView.LinkedViewToResourcePack != null)
                        {
                            if (tempURI.URIModels.LinkedView.LinkedViewToResourcePack.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.LinkedView.LinkedViewToResourcePack)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString(),
                                    root);
                            }
                        }
                        else
                        {
                            bHasGoodDescendants = true;
                        }
                    }
                }
            }
            else if (childNodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
            {
                var obj4 = await GetLinkedViewToResourcePack(childId);
                if (obj4 != null)
                {
                    XElement el = MakeLinkedViewToResourcePackXml(obj4);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURILinkedView(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURILinkedView(tempURI, true);
            return bHasSet;
        }

        public XElement MakeServiceBaseXml(Service sb)
        {
            XElement currentNode = null;
            if (sb != null)
            {
                currentNode = new XElement(Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString());
                var currentObjContext = _dataContext.Entry(sb);
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
            }
            return currentNode;
        }
        private XElement MakeLinkedViewClassXml(LinkedViewClass obj)
        {
            XElement currentNode = new XElement(LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString());
            var currentObjContext = _dataContext.Entry(obj);
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
            return currentNode;
        }
        private XElement MakeLinkedViewPackXml(LinkedViewPack obj)
        {
            XElement currentNode = new XElement(LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString());
            var currentObjContext = _dataContext.Entry(obj);
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
            return currentNode;
        }
        
        private XElement MakeLinkedViewXml(LinkedView obj)
        {
            XElement currentNode = new XElement(LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
            var currentObjContext = _dataContext.Entry(obj);
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
            return currentNode;
        }
        private async Task<LinkedViewToResourcePack> GetLinkedViewToResourcePack(int id)
        {
            LinkedViewToResourcePack bs = await _dataContext
                    .LinkedViewToResourcePack
                    .Include(t => t.ResourcePack)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeLinkedViewToResourcePackXml(LinkedViewToResourcePack obj)
        {
            XElement currentNode = new XElement(LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString());
            var currentObjContext = _dataContext.Entry(obj);
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
            //add any linked view child elements
            if (obj.ResourcePack != null)
            {
                LinkedViews.AddBaseResourcePackToXml(currentNode, obj);
            }
            return currentNode;
        }
    }
}
