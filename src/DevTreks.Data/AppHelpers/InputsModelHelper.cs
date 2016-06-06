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
    ///Purpose:		Entity Framework Input support class
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       Aasync Task<bool> using ancestor objects in queries or the ancestors return 
    ///             collections of descendants which probably means twice the db work and
    ///             twice the data load.
    /// </summary>
    public class InputModelHelper
    {
        public InputModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIInput(ContentURI uri, bool saveInFileSystemContent)
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
                        .InputClass
                        .Include(t => t.LinkedViewToInputClass)
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.InputClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.InputClass != null)
                        {
                            uri.URIDataManager.RowCount =
                            _dataContext
                             .InputClass
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
                    bHasSet = await SetURIInputType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                var rc = await _dataContext.InputClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .Input
                        .Include(t => t.LinkedViewToInput)
                        .Where(a => a.InputClass.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.Input = await qry.ToAsyncEnumerable().ToList();
                        if (rc.Input != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .Input
                                .Where(a => a.InputClass.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.InputClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.INPUT_PRICE_TYPES.input.ToString())
            {
                var rp = await _dataContext.Input.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    // count the input packs without loading them
                    var qry = _dataContext
                        .InputSeries
                        .Include(t => t.LinkedViewToInputSeries)
                        .Where(a => a.Input.PKId == uri.URIId)
                        .OrderBy(m => m.InputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.InputSeries = await qry.ToAsyncEnumerable().ToList();
                        if (rp.InputSeries != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .InputSeries
                                .Where(a => a.Input.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.Input = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                var qry = _dataContext
                    .InputSeries
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.InputSeries = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIInputType(Service service, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.InputType = new List<InputType>();
                //var s = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (service != null)
                {
                    var qry = _dataContext
                        .InputType
                        .Where(rt => rt.NetworkId == service.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.InputType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.InputType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    InputType rt = await _dataContext.InputType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.InputType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void SetURIInput(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                var qryRC = _dataContext
                    .InputClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.INPUT_PRICE_TYPES.input.ToString())
            {
                var qryRP
                        = _dataContext
                        .Input
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                var qryR = _dataContext
                    .InputSeries
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddInput(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = true;
            //store updated inputs ids in lists
            bool bHasAdded = await AddInput(argumentsEdits.SelectionsToAdd);
            //int iNewId = 0;
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of inputs
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIInput(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIInput(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddInput(List<ContentURI> addedURIs)
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
                if (addedURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                {
                    var newInputClass = new InputClass
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        DocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        InputType = null,
                        Input = null,
                        LinkedViewToInputClass = null
                    };
                    _dataContext.InputClass.Add(newInputClass);
                    _dataContext.Entry(newInputClass).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    AccountToLocal local = await Locals.GetDefaultLocal(_dtoContentURI, _dataContext);
                    var newInput = new Input
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        InputUnit1 = Helpers.GeneralHelpers.NONE,
                        InputPrice1 = 0,
                        InputPrice1Amount = 1,
                        InputUnit2 = Helpers.GeneralHelpers.NONE,
                        InputPrice2 = 0,
                        InputUnit3 = Helpers.GeneralHelpers.NONE,
                        InputPrice3 = 1,

                        InputDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        InputLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        RatingClassId = (local != null) ? local.RatingGroupId : 0,
                        RealRateId = (local != null) ? local.RealRateId : 0,
                        NominalRateId = (local != null) ? local.NominalRateId : 0,
                        DataSourceId = (local != null) ? local.DataSourcePriceId : 0,
                        GeoCodeId = (local != null) ? local.GeoCodePriceId : 0,
                        CurrencyClassId = (local != null) ? local.CurrencyGroupId : 0,
                        UnitClassId = (local != null) ? local.UnitGroupId : 0,
                        InputClassId = iParentId,
                        InputClass = null,
                        LinkedViewToInput = null,
                        InputSeries = null
                    };
                    _dataContext.Input.Add(newInput);
                    _dataContext.Entry(newInput).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                {
                    var input = await _dataContext.Input.SingleOrDefaultAsync(x => x.PKId == iParentId);
                    if (input != null)
                    {
                        var newInputSeries = new InputSeries
                        {
                            Num = input.Num,
                            Name = input.Name,
                            Description = input.Description,
                            InputUnit1 = input.InputUnit1,
                            InputPrice1 = input.InputPrice1,
                            InputPrice1Amount = input.InputPrice1Amount,
                            InputUnit2 = input.InputUnit2,
                            InputPrice2 = input.InputPrice2,
                            InputUnit3 = input.InputUnit3,
                            InputPrice3 = input.InputPrice3,

                            InputDate = input.InputDate,
                            InputLastChangedDate = input.InputLastChangedDate,
                            RatingClassId = input.RatingClassId,
                            RealRateId = input.RealRateId,
                            NominalRateId = input.NominalRateId,
                            DataSourceId = input.DataSourceId,
                            GeoCodeId = input.GeoCodeId,
                            CurrencyClassId = input.CurrencyClassId,
                            UnitClassId = input.UnitClassId,

                            InputId = iParentId,
                            Input = null,
                            ComponentToInput = null,
                            CostSystemToInput = null,
                            OperationToInput = null,
                            BudgetSystemToInput = null,
                            LinkedViewToInputSeries = null
                        };
                        _dataContext.InputSeries.Add(newInputSeries);
                        _dataContext.Entry(newInputSeries).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddInputLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddInputLinkedView(ContentURI addedURI,
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
                    if (inputNodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        var newLinkedView = new LinkedViewToInputClass
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            InputClass = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToInputClass.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
                    {
                        var newLinkedView = new LinkedViewToInput
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            Input = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToInput.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                    {
                        var newLinkedView = new LinkedViewToInputSeries
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            InputSeries = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToInputSeries.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                {
                    var newLinkedView = new LinkedViewToInputClass
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        InputClass = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToInputClass.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    var newLinkedView = new LinkedViewToInput
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        Input = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToInput.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                {
                    var newLinkedView = new LinkedViewToInputSeries
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        InputSeries = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToInputSeries.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteInput(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated inputs ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteInput(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIInput(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIInput(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteInput(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            string sKeyName = string.Empty;
            bool bHasSet = true;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputtype.ToString())
                {
                    var inputType = await _dataContext.InputType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (inputType != null)
                    {
                        _dataContext.Entry(inputType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                {
                    var inputClass = await _dataContext.InputClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (inputClass != null)
                    {
                        _dataContext.Entry(inputClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    var input = await _dataContext.Input.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (input != null)
                    {
                        _dataContext.Entry(input).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                {
                    var inputserie = await _dataContext.InputSeries.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (inputserie != null)
                    {
                        _dataContext.Entry(inputserie).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToInputClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToInput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToInputSeries.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
        public async Task<bool> UpdateInput(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = true;
            //store updated inputs ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewInput = await UpdateInput(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewInput)
                    {
                        bHasSet = await SetURIInput(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIInput(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateInput(List<EditHelper.ArgumentsEdits> edits,
             Dictionary<string, int> updatedIds)
        {
            bool bNeedsNewInput = true;
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (EditHelper.ArgumentsEdits edit in edits)
            {
                if (edit.EditAttName == LinkedViews.LINKINGXMLDOC)
                {
                    //uritoadd has parent node name
                    bHasSet = await UpdateInputLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewInput = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Prices.INPUT_PRICE_TYPES.inputtype.ToString())
                    {
                        var inputType = await _dataContext.InputType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (inputType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(inputType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(inputType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        var inputClass = await _dataContext.InputClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (inputClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(inputClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(inputClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
                    {
                        var input = await _dataContext.Input.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (input != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(input), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(input).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                    {
                        var inputserie = await _dataContext.InputSeries.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (inputserie != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(inputserie), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(inputserie).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateInputLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewInput;
        }
        private async Task<bool> UpdateInputLinkedView(EditHelper.ArgumentsEdits edit, 
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            if (inputNodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToInputClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToInput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToInputSeries.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIInput(_dtoContentURI, bSaveInFileSystemContent);
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
            else if (nodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                var currentObject = await GetInputClass(id);
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
                        if (_dtoContentURI.URINodeName != Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                        {
                            XElement el = MakeInputClassXml(currentObject);
                            bHasGoodAncestors = Prices.AddInputElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, currentObject);
                        }
                    }

                }
            }
            else if (nodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
            {
                var currentObject = await GetInput(id);
                if (currentObject != null)
                {
                    id = currentObject.InputClassId;
                    nodeName = Prices.INPUT_PRICE_TYPES.inputgroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Prices.INPUT_PRICE_TYPES.input.ToString())
                        {
                            XElement el = MakeInputXml(currentObject);
                            bHasGoodAncestors = Prices.AddInputElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                var currentObject = await GetInputSeries(id);
                if (currentObject != null)
                {
                    id = currentObject.InputId;
                    nodeName = Prices.INPUT_PRICE_TYPES.input.ToString();
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
                    bool bHasSet = await SetTempURIInput(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.InputClass != null)
                    {
                        if (tempURI.URIModels.Service.InputClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.InputClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Prices.INPUT_PRICE_TYPES.inputgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                var obj2 = await GetInputClass(childId);
                if (obj2 != null)
                {
                    XElement el = MakeInputClassXml(obj2);
                    bHasBeenAdded = Prices.AddInputElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, obj2);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURIInput(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.InputClass.Input != null)
                            {
                                if (tempURI.URIModels.InputClass.Input.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.InputClass.Input)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Prices.INPUT_PRICE_TYPES.input.ToString(),
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
            else if (childNodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
            {
                var obj3 = await GetInput(childId);
                if (obj3 != null)
                {
                    XElement el = MakeInputXml(obj3);
                    bHasBeenAdded = Prices.AddInputElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIInput(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.Input.InputSeries != null)
                        {
                            if (tempURI.URIModels.Input.InputSeries.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.Input.InputSeries)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Prices.INPUT_PRICE_TYPES.inputseries.ToString(),
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
            else if (childNodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                var obj4 = await GetInputSeries(childId);
                if (obj4 != null)
                {
                    XElement el = MakeInputSeriesXml(obj4);
                    bHasBeenAdded = Prices.AddInputElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURIInput(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIInput(tempURI, true);
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
      
        private async Task<InputClass> GetInputClass(int id)
        {
            InputClass bs = await _dataContext
                    .InputClass
                    .Include(t => t.InputType)
                    .Include(t => t.LinkedViewToInputClass)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        
        private XElement MakeInputClassXml(InputClass obj)
        {
            XElement currentNode = new XElement(Prices.INPUT_PRICE_TYPES.inputgroup.ToString());
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
            if (obj.LinkedViewToInputClass != null)
            {
                foreach (var lv in obj.LinkedViewToInputClass)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<Input> GetInput(int id)
        {
            Input bs = await _dataContext
                    .Input
                    .Include(t => t.LinkedViewToInput)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeInputXml(Input obj)
        {
            XElement currentNode = new XElement(Prices.INPUT_PRICE_TYPES.input.ToString());
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
            if (obj.LinkedViewToInput != null)
            {
                foreach (var lv in obj.LinkedViewToInput)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<InputSeries> GetInputSeries(int id)
        {
            InputSeries bs = await _dataContext
                    .InputSeries
                    .Include(t => t.LinkedViewToInputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeInputSeriesXml(InputSeries obj)
        {
            XElement currentNode = new XElement(Prices.INPUT_PRICE_TYPES.inputseries.ToString());
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
            if (obj.LinkedViewToInputSeries != null)
            {
                foreach (var lv in obj.LinkedViewToInputSeries)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
    }
}
