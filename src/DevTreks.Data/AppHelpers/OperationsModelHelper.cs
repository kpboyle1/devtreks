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
    ///Purpose:		Entity Framework Operation support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OperationModelHelper
    {
        public OperationModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }

        public async Task<bool> SetURIOperation(ContentURI uri, bool saveInFileSystemContent)
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
                        .OperationClass
                        .Include(t => t.LinkedViewToOperationClass)
                        .Where(a => a.Service.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.OperationClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.OperationClass != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .OperationClass
                                .Where(a => a.Service.PKId == uri.URIId)
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
                    bHasSet = await SetURIOperationType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
            {
                var rc = await _dataContext.OperationClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .Operation
                        .Include(t => t.LinkedViewToOperation)
                        .Where(a => a.OperationClass.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.Operation = await qry.ToAsyncEnumerable().ToList();
                        if (rc.Operation != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .Operation
                                .Where(a => a.OperationClass.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.OperationClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.OPERATION_PRICE_TYPES.operation.ToString())
            {
                var rp = await _dataContext.Operation.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .OperationToInput
                        .Include(t => t.InputSeries)
                        .ThenInclude(t => t.LinkedViewToInputSeries)
                        .Where(a => a.Operation.PKId == uri.URIId)
                        .OrderBy(m => m.InputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.OperationToInput = await qry.ToAsyncEnumerable().ToList();
                        if (rp.OperationToInput != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .OperationToInput
                                .Where(a => a.Operation.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.Operation = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
            {
                var qry = _dataContext
                    .OperationToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.OperationToInput = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIOperationType(Service s, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.OperationType = new List<OperationType>();
                //filter the operation types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .OperationType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.OperationType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.OperationType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    OperationType rt = await _dataContext.OperationType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.OperationType.Add(rt);
                    }
                }
            }
            if (string.IsNullOrEmpty(uri.ErrorMessage))
            {
                bHasSet = true;
            }
            return bHasSet;
        }
        public void SetURIOperation(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
            {
                var qryRC = _dataContext
                    .OperationClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OPERATION_PRICE_TYPES.operation.ToString())
            {
                var qryRP
                        = _dataContext
                        .Operation
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
            {
                var qryR = _dataContext
                    .OperationToInput
                    .Include(t => t.InputSeries)
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddOperation(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasAdded = await AddOperation(argumentsEdits.SelectionsToAdd);
            bool bHasSet = true;
            //int iNewId = 0;
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of operations
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIOperation(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOperation(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddOperation(List<ContentURI> addedURIs)
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
                if (addedURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                {
                    var newOperationClass = new OperationClass
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        DocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        OperationType = null,
                        Operation = null,
                        LinkedViewToOperationClass = null
                    };
                    _dataContext.OperationClass.Add(newOperationClass);
                    _dataContext.Entry(newOperationClass).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                {
                    AccountToLocal local = await Locals.GetDefaultLocal(_dtoContentURI, _dataContext);
                    var newOperation = new Operation
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

                        OperationClassId = iParentId,
                        OperationClass = null,
                        BudgetSystemToOperation = null,
                        LinkedViewToOperation = null,
                        OperationToInput = null
                    };
                    _dataContext.Operation.Add(newOperation);
                    _dataContext.Entry(newOperation).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                {
                    var inputseries = await _dataContext.InputSeries.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (inputseries != null)
                    {
                        var newOperationToInput = new OperationToInput
                        {
                            Num = inputseries.Num,
                            Name = inputseries.Name,
                            Description = inputseries.Description,
                            IncentiveRate = 0,
                            IncentiveAmount = 0,
                            InputPrice1Amount = inputseries.InputPrice1Amount,
                            InputPrice2Amount = 0,
                            InputPrice3Amount = 0,
                            InputTimes = 1,
                            InputDate = inputseries.InputDate,
                            InputUseAOHOnly = false,

                            RatingClassId = inputseries.RatingClassId,
                            RealRateId = inputseries.RealRateId,
                            NominalRateId = inputseries.NominalRateId,
                            GeoCodeId = inputseries.GeoCodeId,
                            InputId = inputseries.PKId,
                            InputSeries = null,
                            OperationId = iParentId,
                            Operation = null

                        };
                        _dataContext.OperationToInput.Add(newOperationToInput);
                        _dataContext.Entry(newOperationToInput).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddOperationLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddOperationLinkedView(ContentURI addedURI,
            string inputNodeName)
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
                    if (inputNodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        var newLinkedView = new LinkedViewToOperationClass
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            OperationClass = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToOperationClass.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                    {
                        var newLinkedView = new LinkedViewToOperation
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            Operation = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToOperation.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                {
                    var newLinkedView = new LinkedViewToOperationClass
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        OperationClass = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToOperationClass.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                {
                    var newLinkedView = new LinkedViewToOperation
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        Operation = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToOperation.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteOperation(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated operations ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteOperation(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIOperation(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOperation(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteOperation(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operationtype.ToString())
                {
                    var operationType = await _dataContext.OperationType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (operationType != null)
                    {
                        _dataContext.Entry(operationType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                {
                    var operationClass = await _dataContext.OperationClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (operationClass != null)
                    {
                        _dataContext.Entry(operationClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                {
                    var operation = await _dataContext.Operation.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (operation != null)
                    {
                        _dataContext.Entry(operation).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
                {
                    var operationtooperation = await _dataContext.OperationToInput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (operationtooperation != null)
                    {
                        _dataContext.Entry(operationtooperation).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToOperationClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToOperation.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
        public async Task<bool> UpdateOperation(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            //store updated operations ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateOperation(edits, updatedIds);
            bool bHasSet = true;
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIOperation(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOperation(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateOperation(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bNeedsNewCollections = true;
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.EditAttName == LinkedViews.LINKINGXMLDOC)
                {
                    //uritoadd has parent node name
                    bHasSet = await UpdateOperationLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Prices.OPERATION_PRICE_TYPES.operationtype.ToString())
                    {
                        var operationType = await _dataContext.OperationType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (operationType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(operationType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(operationType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        var operationClass = await _dataContext.OperationClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (operationClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(operationClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(operationClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                    {
                        var operation = await _dataContext.Operation.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (operation != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(operation), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(operation).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
                    {
                        var operationtooperation = await _dataContext.OperationToInput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (operationtooperation != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(operationtooperation), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(operationtooperation).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateOperationLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateOperationLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            if (inputNodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToOperationClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToOperation.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIOperation(_dtoContentURI, bSaveInFileSystemContent);
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
            else if (nodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
            {
                var currentObject = await GetOperationClass(id);
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
                        if (_dtoContentURI.URINodeName != Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                        {
                            XElement el = MakeOperationClassXml(currentObject);
                            bHasGoodAncestors = Prices.AddOperationElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, currentObject);
                        }
                    }

                }
            }
            else if (nodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
            {
                var currentObject = await GetOperation(id);
                if (currentObject != null)
                {
                    id = currentObject.OperationClassId;
                    nodeName = Prices.OPERATION_PRICE_TYPES.operationgroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Prices.OPERATION_PRICE_TYPES.operation.ToString())
                        {
                            XElement el = MakeOperationXml(currentObject);
                            bHasGoodAncestors = Prices.AddOperationElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
            {
                var currentObject = await GetOperationInput(id);
                if (currentObject != null)
                {
                    id = currentObject.OperationId;
                    nodeName = Prices.OPERATION_PRICE_TYPES.operation.ToString();
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
            bool bHasSet = true;
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
                    bHasSet = await SetTempURIOperation(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.OperationClass != null)
                    {
                        if (tempURI.URIModels.Service.OperationClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.OperationClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Prices.OPERATION_PRICE_TYPES.operationgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
            {
                var obj2 = await GetOperationClass(childId);
                if (obj2 != null)
                {
                    XElement el = MakeOperationClassXml(obj2);
                    bHasBeenAdded = Prices.AddOperationElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, obj2);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            bHasSet = await SetTempURIOperation(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.OperationClass.Operation != null)
                            {
                                if (tempURI.URIModels.OperationClass.Operation.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.OperationClass.Operation)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Prices.OPERATION_PRICE_TYPES.operation.ToString(),
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
            else if (childNodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
            {
                var obj3 = await GetOperation(childId);
                if (obj3 != null)
                {
                    XElement el = MakeOperationXml(obj3);
                    bHasBeenAdded = Prices.AddOperationElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasSet = await SetTempURIOperation(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.Operation.OperationToInput != null)
                        {
                            if (tempURI.URIModels.Operation.OperationToInput.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.Operation.OperationToInput)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Prices.OPERATION_PRICE_TYPES.operationinput.ToString(),
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
            else if (childNodeName == Prices.OPERATION_PRICE_TYPES.operationinput.ToString())
            {
                var obj4 = await GetOperationInput(childId);
                if (obj4 != null)
                {
                    XElement el = MakeOperationToInputXml(obj4);
                    bHasBeenAdded = Prices.AddOperationElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURIOperation(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIOperation(tempURI, true);
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
        private async Task<OperationClass> GetOperationClass(int id)
        {
            OperationClass bs = await _dataContext
                    .OperationClass
                    .Include(t => t.OperationType)
                    .Include(t => t.LinkedViewToOperationClass)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOperationClassXml(OperationClass obj)
        {
            XElement currentNode = new XElement(Prices.OPERATION_PRICE_TYPES.operationgroup.ToString());
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
            if (obj.LinkedViewToOperationClass != null)
            {
                foreach (var lv in obj.LinkedViewToOperationClass)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<Operation> GetOperation(int id)
        {
            Operation bs = await _dataContext
                    .Operation
                    .Include(t => t.LinkedViewToOperation)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOperationXml(Operation obj)
        {
            XElement currentNode = new XElement(Prices.OPERATION_PRICE_TYPES.operation.ToString());
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
            if (obj.LinkedViewToOperation != null)
            {
                foreach (var lv in obj.LinkedViewToOperation)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<OperationToInput> GetOperationInput(int id)
        {
            OperationToInput bs = await _dataContext
                    .OperationToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.Input.InputClass)
                    .Include(t => t.InputSeries.Input.InputClass.InputType)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOperationToInputXml(OperationToInput obj)
        {
            XElement currentNode = new XElement(Prices.OPERATION_PRICE_TYPES.operationinput.ToString());
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
            if (obj.InputSeries != null)
            {
                Prices.AddBaseInputSeriesToXml(currentNode, obj);
                //must use base input series linked view child elements
                if (obj.InputSeries.LinkedViewToInputSeries != null)
                {
                    foreach (var lv in obj.InputSeries.LinkedViewToInputSeries)
                    {
                        EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                    }
                }
            }
            return currentNode;
        }
    }
}
