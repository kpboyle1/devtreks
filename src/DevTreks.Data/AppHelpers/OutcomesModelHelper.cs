using DevTreks.Data.DataAccess;
using DevTreks.Data.EditHelpers;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Entity Framework Outcome support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OutcomeModelHelper
    {
        public OutcomeModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIOutcome(ContentURI uri, bool saveInFileSystemContent)
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
                        .OutcomeClass
                        .Include(t => t.LinkedViewToOutcomeClass)
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.OutcomeClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.OutcomeClass != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .OutcomeClass
                                .Where(a => a.ServiceId == uri.URIId)
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
                    bHasSet = await SetURIOutcomeType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                var rc = await _dataContext.OutcomeClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .Outcome
                        .Include(t => t.LinkedViewToOutcome)
                        .Where(a => a.OutcomeClass.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.Outcome = await qry.ToAsyncEnumerable().ToList();
                        if (rc.Outcome != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .Outcome
                                .Where(a => a.OutcomeClass.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.OutcomeClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
            {
                var rp = await _dataContext.Outcome.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .OutcomeToOutput
                        .Include(t => t.OutputSeries)
                        .ThenInclude(t => t.LinkedViewToOutputSeries)
                        .Where(a => a.Outcome.PKId == uri.URIId)
                        .OrderBy(m => m.OutputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.OutcomeToOutput = await qry.ToAsyncEnumerable().ToList();
                        if (rp.OutcomeToOutput != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .OutcomeToOutput
                                .Where(a => a.Outcome.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.Outcome = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
            {
                var qry = _dataContext
                    .OutcomeToOutput
                    .Include(t => t.OutputSeries)
                    .Include(t => t.OutputSeries.LinkedViewToOutputSeries)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.OutcomeToOutput = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIOutcomeType(Service s, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.OutcomeType = new List<OutcomeType>();
                //filter the outcome types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .OutcomeType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.OutcomeType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.OutcomeType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    OutcomeType rt = await _dataContext.OutcomeType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.OutcomeType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void SetURIOutcome(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                var qryRC = _dataContext
                    .OutcomeClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
            {
                var qryRP
                        = _dataContext
                        .Outcome
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
            {
                var qryR = _dataContext
                    .OutcomeToOutput
                    .Include(t => t.OutputSeries)
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddOutcome(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bHasSet = false;
            bool bIsAdded = false;
            bool bHasAdded = await AddOutcome(argumentsEdits.SelectionsToAdd);
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of outcomes
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIOutcome(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOutcome(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddOutcome(List<ContentURI> addedURIs)
        {
            string sParentNodeName = string.Empty;
            int iParentId = 0;
            EditModelHelper editHelper = new EditModelHelper();
            bool bIsAdded = false;
            foreach (ContentURI addedURI in addedURIs)
            {
                Helpers.GeneralHelpers.GetParentIdAndNodeName(addedURI, out iParentId, out sParentNodeName);
                if (!string.IsNullOrEmpty(addedURI.ErrorMessage))
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "INSERT_NOPARENT");
                    return false;
                }
                if (addedURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                {
                    var newOutcomeClass = new OutcomeClass
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        DocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        OutcomeType = null,
                        Outcome = null,
                        LinkedViewToOutcomeClass = null
                    };
                    _dataContext.OutcomeClass.Add(newOutcomeClass);
                    _dataContext.Entry(newOutcomeClass).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    AccountToLocal local = await Locals.GetDefaultLocal(_dtoContentURI, _dataContext);
                    var newOutcome = new Outcome
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Num2 = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        ResourceWeight = 0,
                        Amount = 1,
                        Unit = Helpers.GeneralHelpers.NONE,
                        EffectiveLife = 1,
                        SalvageValue = 0,
                        IncentiveRate = 0,
                        IncentiveAmount = 0,
                        //default insertions go to front of list
                        Date = Helpers.GeneralHelpers.GetDateSortOld(),
                        LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        RatingClassId = (local != null) ? local.RatingGroupId : 0,
                        RealRateId = (local != null) ? local.RealRateId : 0,
                        NominalRateId = (local != null) ? local.NominalRateId : 0,
                        DataSourceId = (local != null) ? local.DataSourcePriceId : 0,
                        GeoCodeId = (local != null) ? local.GeoCodePriceId : 0,
                        CurrencyClassId = (local != null) ? local.CurrencyGroupId : 0,
                        UnitClassId = (local != null) ? local.UnitGroupId : 0,

                        OutcomeClassId = iParentId,
                        OutcomeClass = null,
                        BudgetSystemToOutcome = null,
                        LinkedViewToOutcome = null,
                        OutcomeToOutput = null
                    };
                    _dataContext.Outcome.Add(newOutcome);
                    _dataContext.Entry(newOutcome).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                {
                    var outputseries = await _dataContext.OutputSeries.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (outputseries != null)
                    {
                        var newOutcomeToOutput = new OutcomeToOutput
                        {
                            Num = outputseries.Num,
                            Name = outputseries.Name,
                            Description = outputseries.Description,
                            IncentiveRate = 0,
                            IncentiveAmount = 0,
                            OutputCompositionAmount = 1,
                            OutputCompositionUnit = "each",
                            OutputAmount1 = outputseries.OutputAmount1,
                            OutputTimes = 1,
                            OutputDate = outputseries.OutputDate,

                            RatingClassId = outputseries.RatingClassId,
                            RealRateId = outputseries.RealRateId,
                            NominalRateId = outputseries.NominalRateId,
                            GeoCodeId = outputseries.GeoCodeId,
                            OutputId = outputseries.PKId,
                            OutputSeries = null,
                            OutcomeId = iParentId,
                            Outcome = null

                        };
                        _dataContext.OutcomeToOutput.Add(newOutcomeToOutput);
                        _dataContext.Entry(newOutcomeToOutput).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddOutcomeLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddOutcomeLinkedView(ContentURI addedURI,
            string outputNodeName)
        {
            bool bIsAdded = false;
            if (_dtoContentURI.URIDataManager.ServerSubActionType
                        == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
            {
                int iAddInId = 0;
                string sAddInName = string.Empty;
                Dictionary<int, string> addins = await LinkedViews.GetDefaultAddInOrLocalId(
                    _dtoContentURI, _dataContext);
                if (addins.Count > 0)
                {
                    iAddInId = addins.FirstOrDefault().Key;
                    sAddInName = addins.FirstOrDefault().Value;
                }
                if (iAddInId != 0)
                {
                    if (outputNodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        var newLinkedView = new LinkedViewToOutcomeClass
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            OutcomeClass = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToOutcomeClass.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (outputNodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        var newLinkedView = new LinkedViewToOutcome
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            Outcome = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToOutcome.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (outputNodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                {
                    var newLinkedView = new LinkedViewToOutcomeClass
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        OutcomeClass = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToOutcomeClass.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (outputNodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    var newLinkedView = new LinkedViewToOutcome
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        Outcome = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToOutcome.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteOutcome(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated outcomes ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteOutcome(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIOutcome(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOutcome(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteOutcome(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcometype.ToString())
                {
                    var outcomeType = await _dataContext.OutcomeType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (outcomeType != null)
                    {
                        _dataContext.Entry(outcomeType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                {
                    var outcomeClass = await _dataContext.OutcomeClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (outcomeClass != null)
                    {
                        _dataContext.Entry(outcomeClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    var outcome = await _dataContext.Outcome.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (outcome != null)
                    {
                        _dataContext.Entry(outcome).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
                {
                    var outcometooutcome = await _dataContext.OutcomeToOutput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (outcometooutcome != null)
                    {
                        _dataContext.Entry(outcometooutcome).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToOutcomeClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToOutcome.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                }
            }
            return bHasSet;
        }
        public async Task<bool> UpdateOutcome(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = false;
            //store updated outcomes ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateOutcome(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIOutcome(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOutcome(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateOutcome(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bHasSet = true;
            bool bNeedsNewCollections = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.EditAttName == LinkedViews.LINKINGXMLDOC)
                {
                    //uritoadd has parent node name
                    bHasSet = await UpdateOutcomeLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcometype.ToString())
                    {
                        var outcomeType = await _dataContext.OutcomeType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (outcomeType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(outcomeType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(outcomeType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        var outcomeClass = await _dataContext.OutcomeClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (outcomeClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(outcomeClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(outcomeClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        var outcome = await _dataContext.Outcome.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (outcome != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(outcome), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(outcome).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
                    {
                        var outcometooutcome = await _dataContext.OutcomeToOutput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (outcometooutcome != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(outcometooutcome), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(outcometooutcome).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateOutcomeLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateOutcomeLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string outputNodeName)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            if (outputNodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToOutcomeClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (outputNodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToOutcome.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIOutcome(_dtoContentURI, bSaveInFileSystemContent);
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
            if (nodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var currentObject = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    _parentName = nodeName;
                    _parentId = id;
                    if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
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
            else if (nodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                var currentObject = await GetOutcomeClass(id);
                if (currentObject != null)
                {
                    id = currentObject.ServiceId;
                    nodeName = Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                        {
                            XElement el = MakeOutcomeClassXml(currentObject);
                            bHasGoodAncestors = Prices.AddOutcomeElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, currentObject);
                        }
                    }

                }
            }
            else if (nodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
            {
                var currentObject = await GetOutcome(id);
                if (currentObject != null)
                {
                    id = currentObject.OutcomeClassId;
                    nodeName = Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                        {
                            XElement el = MakeOutcomeXml(currentObject);
                            bHasGoodAncestors = Prices.AddOutcomeElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
            {
                var currentObject = await GetOutcomeOutput(id);
                if (currentObject != null)
                {
                    id = currentObject.OutcomeId;
                    nodeName = Prices.OUTCOME_PRICE_TYPES.outcome.ToString();
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
            if (childNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var obj1 = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj1 != null)
                {
                    XElement el = MakeServiceBaseXml(obj1);
                    if (el != null)
                    {
                        root.AddFirst(el);
                    }
                    bool bHasSet = await SetTempURIOutcome(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.OutcomeClass != null)
                    {
                        if (tempURI.URIModels.Service.OutcomeClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.OutcomeClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                var obj2 = await GetOutcomeClass(childId);
                if (obj2 != null)
                {
                    XElement el = MakeOutcomeClassXml(obj2);
                    bHasBeenAdded = Prices.AddOutcomeElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, obj2);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURIOutcome(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.OutcomeClass.Outcome != null)
                            {
                                if (tempURI.URIModels.OutcomeClass.Outcome.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.OutcomeClass.Outcome)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Prices.OUTCOME_PRICE_TYPES.outcome.ToString(),
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
            else if (childNodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
            {
                var obj3 = await GetOutcome(childId);
                if (obj3 != null)
                {
                    XElement el = MakeOutcomeXml(obj3);
                    bHasBeenAdded = Prices.AddOutcomeElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIOutcome(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.Outcome.OutcomeToOutput != null)
                        {
                            if (tempURI.URIModels.Outcome.OutcomeToOutput.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.Outcome.OutcomeToOutput)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString(),
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
            else if (childNodeName == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString())
            {
                var obj4 = await GetOutcomeOutput(childId);
                if (obj4 != null)
                {
                    XElement el = MakeOutcomeToOutputXml(obj4);
                    bHasBeenAdded = Prices.AddOutcomeElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURIOutcome(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIOutcome(tempURI, true);
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
        private async Task<OutcomeClass> GetOutcomeClass(int id)
        {
            OutcomeClass bs = await _dataContext
                    .OutcomeClass
                    .Include(t => t.OutcomeType)
                    .Include(t => t.LinkedViewToOutcomeClass)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOutcomeClassXml(OutcomeClass obj)
        {
            XElement currentNode = new XElement(Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString());
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
            if (obj.LinkedViewToOutcomeClass != null)
            {
                foreach (var lv in obj.LinkedViewToOutcomeClass)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<Outcome> GetOutcome(int id)
        {
            Outcome bs = await _dataContext
                    .Outcome
                    .Include(t => t.LinkedViewToOutcome)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOutcomeXml(Outcome obj)
        {
            XElement currentNode = new XElement(Prices.OUTCOME_PRICE_TYPES.outcome.ToString());
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
            if (obj.LinkedViewToOutcome != null)
            {
                foreach (var lv in obj.LinkedViewToOutcome)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<OutcomeToOutput> GetOutcomeOutput(int id)
        {
            OutcomeToOutput bs = await _dataContext
                    .OutcomeToOutput
                    .Include(t => t.OutputSeries)
                    .Include(t => t.OutputSeries.Output.OutputClass)
                    .Include(t => t.OutputSeries.Output.OutputClass.OutputType)
                    .Include(t => t.OutputSeries.LinkedViewToOutputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOutcomeToOutputXml(OutcomeToOutput obj)
        {
            XElement currentNode = new XElement(Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString());
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
            if (obj.OutputSeries != null)
            {
                Prices.AddBaseOutputSeriesToXml(currentNode, obj);
                //must use base output series linked view child elements
                if (obj.OutputSeries.LinkedViewToOutputSeries != null)
                {
                    foreach (var lv in obj.OutputSeries.LinkedViewToOutputSeries)
                    {
                        EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                    }
                }
            }
            return currentNode;
        }
    }
}