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
    ///Purpose:		Entity Framework Component support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class ComponentModelHelper
    {
        public ComponentModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIComponent(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var s = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (s != null)
                {
                    // count the component packs without loading them
                    var qry = _dataContext
                        .ComponentClass
                        .Include(t => t.LinkedViewToComponentClass)
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.ComponentClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.ComponentClass != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .ComponentClass
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
                    bHasSet = await SetURIComponentType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                var rc = await _dataContext.ComponentClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    // count the component packs without loading them
                    var qry = _dataContext
                        .Component
                        .Include(t => t.LinkedViewToComponent)
                        .Where(a => a.ComponentClass.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize); rc.Component = await qry.ToAsyncEnumerable().ToList();
                    if (qry != null)
                    {
                        rc.Component = await qry.ToAsyncEnumerable().ToList();
                        if (rc.Component != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .Component
                                .Where(a => a.ComponentClass.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.ComponentClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.COMPONENT_PRICE_TYPES.component.ToString())
            {
                var rp = await _dataContext.Component.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .ComponentToInput
                        .Include(t => t.InputSeries)
                        .ThenInclude(t => t.LinkedViewToInputSeries)
                        .Where(a => a.Component.PKId == uri.URIId)
                        .OrderBy(m => m.InputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.ComponentToInput = await qry.ToAsyncEnumerable().ToList();
                        if (rp.ComponentToInput != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .ComponentToInput
                                .Where(a => a.Component.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.Component = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
            {
                var qry = _dataContext
                    .ComponentToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(r => r.PKId == uri.URIId);
                if (qry != null)
                {
                    uri.URIModels.ComponentToInput = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIComponentType(Service s, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.ComponentType = new List<ComponentType>();
                //filter the component types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .ComponentType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.ComponentType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.ComponentType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    ComponentType rt = await _dataContext.ComponentType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.ComponentType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void SetURIComponent(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                var qryRC = _dataContext
                    .ComponentClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.COMPONENT_PRICE_TYPES.component.ToString())
            {
                var qryRP
                        = _dataContext
                        .Component
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
            {
                var qryR = _dataContext
                    .ComponentToInput
                    .Include(t => t.InputSeries)
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddComponent(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = true;
            //store updated components ids in lists
            bool bHasAdded = await AddComponent(argumentsEdits.SelectionsToAdd);
            //int iNewId = 0;
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of components
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIComponent(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIComponent(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddComponent(List<ContentURI> addedURIs)
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
                if (addedURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                {
                    var newComponentClass = new ComponentClass
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        DocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        ComponentType = null,
                        Component = null,
                        LinkedViewToComponentClass = null
                    };
                    _dataContext.ComponentClass.Add(newComponentClass);
                    _dataContext.Entry(newComponentClass).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    AccountToLocal local = await Locals.GetDefaultLocal(_dtoContentURI, _dataContext);
                    var newComponent = new Component
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
                        //insertions to front
                        Date = Helpers.GeneralHelpers.GetDateSortOld(),
                        LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        RatingClassId = (local != null) ? local.RatingGroupId : 0,
                        RealRateId = (local != null) ? local.RealRateId : 0,
                        NominalRateId = (local != null) ? local.NominalRateId : 0,
                        DataSourceId = (local != null) ? local.DataSourcePriceId : 0,
                        GeoCodeId = (local != null) ? local.GeoCodePriceId : 0,
                        CurrencyClassId = (local != null) ? local.CurrencyGroupId : 0,
                        UnitClassId = (local != null) ? local.UnitGroupId : 0,

                        ComponentClassId = iParentId,
                        ComponentClass = null,
                        CostSystemToComponent = null,
                        LinkedViewToComponent = null,
                        ComponentToInput = null
                    };
                    _dataContext.Component.Add(newComponent);
                    _dataContext.Entry(newComponent).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                {
                    var inputseries = await _dataContext.InputSeries.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (inputseries != null)
                    {
                        var newComponentToInput = new ComponentToInput
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
                            ComponentId = iParentId,
                            Component = null

                        };
                        _dataContext.ComponentToInput.Add(newComponentToInput);
                        _dataContext.Entry(newComponentToInput).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddComponentLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddComponentLinkedView(ContentURI addedURI,
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
                    if (inputNodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        var newLinkedView = new LinkedViewToComponentClass
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            ComponentClass = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToComponentClass.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        var newLinkedView = new LinkedViewToComponent
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            Component = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToComponent.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                {
                    var newLinkedView = new LinkedViewToComponentClass
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        ComponentClass = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToComponentClass.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    var newLinkedView = new LinkedViewToComponent
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        Component = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToComponent.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteComponent(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            bool bHasSet = true;
            //store updated components ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bHasSet = await DeleteComponent(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIComponent(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIComponent(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteComponent(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            string sKeyName = string.Empty;
            bool bHasSet = true;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.componenttype.ToString())
                {
                    var componentType = await _dataContext.ComponentType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (componentType != null)
                    {
                        _dataContext.Entry(componentType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                {
                    var componentClass = await _dataContext.ComponentClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (componentClass != null)
                    {
                        _dataContext.Entry(componentClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    var component = await _dataContext.Component.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (component != null)
                    {
                        _dataContext.Entry(component).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
                {
                    var componenttoinput = await _dataContext.ComponentToInput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (componenttoinput != null)
                    {
                        _dataContext.Entry(componenttoinput).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToComponentClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToComponent.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
        public async Task<bool> UpdateComponent(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = true;
            //store updated components ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateComponent(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIComponent(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIComponent(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateComponent(List<EditHelper.ArgumentsEdits> edits,
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
                    bHasSet = await UpdateComponentLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Prices.COMPONENT_PRICE_TYPES.componenttype.ToString())
                    {
                        var componentType = await _dataContext.ComponentType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (componentType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(componentType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(componentType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        var componentClass = await _dataContext.ComponentClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (componentClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(componentClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(componentClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        var component = await _dataContext.Component.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (component != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(component), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(component).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
                    {
                        var componenttoinput = await _dataContext.ComponentToInput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (componenttoinput != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(componenttoinput), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(componenttoinput).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateComponentLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateComponentLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            string sKeyName = string.Empty;
            bool bHasSet = true;
            if (inputNodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToComponentClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToComponent.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIComponent(_dtoContentURI, bSaveInFileSystemContent);
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
            else if (nodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                var currentObject = await GetComponentClass(id);
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
                        if (_dtoContentURI.URINodeName != Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                        {
                            XElement el = MakeComponentClassXml(currentObject);
                            bHasGoodAncestors = Prices.AddComponentElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, currentObject);
                        }
                    }

                }
            }
            else if (nodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
            {
                var currentObject = await GetComponent(id);
                if (currentObject != null)
                {
                    id = currentObject.ComponentClassId;
                    nodeName = Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Prices.COMPONENT_PRICE_TYPES.component.ToString())
                        {
                            XElement el = MakeComponentXml(currentObject);
                            bHasGoodAncestors = Prices.AddComponentElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
            {
                var currentObject = await GetComponentInput(id);
                if (currentObject != null)
                {
                    id = currentObject.ComponentId;
                    nodeName = Prices.COMPONENT_PRICE_TYPES.component.ToString();
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
                    bool bHasSet = await SetTempURIComponent(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.ComponentClass != null)
                    {
                        if (tempURI.URIModels.Service.ComponentClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.ComponentClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                var obj2 = await GetComponentClass(childId);
                if (obj2 != null)
                {
                    XElement el = MakeComponentClassXml(obj2);
                    bHasBeenAdded = Prices.AddComponentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, obj2);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURIComponent(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.ComponentClass.Component != null)
                            {
                                if (tempURI.URIModels.ComponentClass.Component.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.ComponentClass.Component)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Prices.COMPONENT_PRICE_TYPES.component.ToString(),
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
            else if (childNodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
            {
                var obj3 = await GetComponent(childId);
                if (obj3 != null)
                {
                    XElement el = MakeComponentXml(obj3);
                    bHasBeenAdded = Prices.AddComponentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIComponent(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.Component.ComponentToInput != null)
                        {
                            if (tempURI.URIModels.Component.ComponentToInput.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.Component.ComponentToInput)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Prices.COMPONENT_PRICE_TYPES.componentinput.ToString(),
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
            else if (childNodeName == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString())
            {
                var obj4 = await GetComponentInput(childId);
                if (obj4 != null)
                {
                    XElement el = MakeComponentToInputXml(obj4);
                    bHasBeenAdded = Prices.AddComponentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURIComponent(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIComponent(tempURI, true);
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
        private async Task<ComponentClass> GetComponentClass(int id)
        {
            ComponentClass bs = await _dataContext
                    .ComponentClass
                    .Include(t => t.ComponentType)
                    .Include(t => t.LinkedViewToComponentClass)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeComponentClassXml(ComponentClass obj)
        {
            XElement currentNode = new XElement(Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString());
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
            if (obj.LinkedViewToComponentClass != null)
            {
                foreach (var lv in obj.LinkedViewToComponentClass)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<Component> GetComponent(int id)
        {
            Component bs = await _dataContext
                    .Component
                    .Include(t => t.LinkedViewToComponent)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeComponentXml(Component obj)
        {
            XElement currentNode = new XElement(Prices.COMPONENT_PRICE_TYPES.component.ToString());
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
            if (obj.LinkedViewToComponent != null)
            {
                foreach (var lv in obj.LinkedViewToComponent)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<ComponentToInput> GetComponentInput(int id)
        {
            ComponentToInput bs = await _dataContext
                    .ComponentToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.Input.InputClass)
                    .Include(t => t.InputSeries.Input.InputClass.InputType)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeComponentToInputXml(ComponentToInput obj)
        {
            XElement currentNode = new XElement(Prices.COMPONENT_PRICE_TYPES.componentinput.ToString());
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
