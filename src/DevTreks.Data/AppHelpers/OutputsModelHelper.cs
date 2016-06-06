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
    ///Purpose:		Entity Framework Output support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OutputModelHelper
    {
        public OutputModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIOutput(ContentURI uri, bool saveInFileSystemContent)
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
                        .OutputClass
                        .Include(t => t.LinkedViewToOutputClass)
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.OutputClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.OutputClass != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .OutputClass
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
                    bHasSet = await SetURIOutputType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                var rc = await _dataContext.OutputClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .Output
                        .Include(t => t.LinkedViewToOutput)
                        .Where(a => a.OutputClass.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.Output = await qry.ToAsyncEnumerable().ToList();
                        if (rc.Output != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .Output
                                .Where(a => a.OutputClass.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.OutputClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.OUTPUT_PRICE_TYPES.output.ToString())
            {
                var rp = await _dataContext.Output.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .OutputSeries
                        .Include(t => t.LinkedViewToOutputSeries)
                        .Where(a => a.Output.PKId == uri.URIId)
                        .OrderBy(m => m.OutputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.OutputSeries = await qry.ToAsyncEnumerable().ToList();
                        if (rp.OutputSeries != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                               .OutputSeries
                                .Where(a => a.Output.PKId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.Output = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                var qry = _dataContext
                    .OutputSeries
                    .Include(t => t.Output)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.OutputSeries = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIOutputType(Service s, ContentURI uri)
        {
            bool bHasSet = false;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.OutputType = new List<OutputType>();
                //filter the output types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .OutputType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.OutputType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.OutputType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    OutputType rt = await _dataContext.OutputType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.OutputType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void SetURIOutput(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                var qryRC = _dataContext
                    .OutputClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OUTPUT_PRICE_TYPES.output.ToString())
            {
                var qryRP
                        = _dataContext
                        .Output
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                var qryR = _dataContext
                    .OutputSeries
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddOutput(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = false;
            //store updated outputs ids in lists
            bool bHasAdded = await AddOutput(argumentsEdits.SelectionsToAdd);
            //int iNewId = 0;
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of outputs
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIOutput(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOutput(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddOutput(List<ContentURI> addedURIs)
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
                if (addedURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                {
                    var newOutputClass = new OutputClass
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        DocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        OutputType = null,
                        Output = null,
                        LinkedViewToOutputClass = null
                    };
                    _dataContext.OutputClass.Add(newOutputClass);
                    _dataContext.Entry(newOutputClass).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    AccountToLocal local = await Locals.GetDefaultLocal(_dtoContentURI, _dataContext);
                    var newOutput = new Output
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        OutputUnit1 = Helpers.GeneralHelpers.NONE,
                        OutputPrice1 = 0,
                        OutputAmount1 = 1,

                        OutputDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        OutputLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        RatingClassId = (local != null) ? local.RatingGroupId : 0,
                        RealRateId = (local != null) ? local.RealRateId : 0,
                        NominalRateId = (local != null) ? local.NominalRateId : 0,
                        DataSourceId = (local != null) ? local.DataSourcePriceId : 0,
                        GeoCodeId = (local != null) ? local.GeoCodePriceId : 0,
                        CurrencyClassId = (local != null) ? local.CurrencyGroupId : 0,
                        UnitClassId = (local != null) ? local.UnitGroupId : 0,

                        OutputClassId = iParentId,
                        OutputClass = null,
                        LinkedViewToOutput = null,
                        OutputSeries = null
                    };
                    _dataContext.Output.Add(newOutput);
                    _dataContext.Entry(newOutput).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                {
                    var output = await _dataContext.Output.SingleOrDefaultAsync(x => x.PKId == iParentId);
                    if (output != null)
                    {
                        var newOutputSeries = new OutputSeries
                        {
                            Num = output.Num,
                            Name = output.Name,
                            Description = output.Description,
                            OutputUnit1 = output.OutputUnit1,
                            OutputPrice1 = output.OutputPrice1,
                            OutputAmount1 = output.OutputAmount1,

                            OutputDate = output.OutputDate,
                            OutputLastChangedDate = output.OutputLastChangedDate,
                            RatingClassId = output.RatingClassId,
                            RealRateId = output.RealRateId,
                            NominalRateId = output.NominalRateId,
                            DataSourceId = output.DataSourceId,
                            GeoCodeId = output.GeoCodeId,
                            CurrencyClassId = output.CurrencyClassId,
                            UnitClassId = output.UnitClassId,

                            OutputId = iParentId,
                            Output = null,
                            CostSystemToOutput = null,
                            BudgetSystemToOutput = null,
                            LinkedViewToOutputSeries = null
                        };
                        _dataContext.OutputSeries.Add(newOutputSeries);
                        _dataContext.Entry(newOutputSeries).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddOutputLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddOutputLinkedView(ContentURI addedURI,
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
                    if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        var newLinkedView = new LinkedViewToOutputClass
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            OutputClass = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToOutputClass.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        var newLinkedView = new LinkedViewToOutput
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            Output = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToOutput.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                    {
                        var newLinkedView = new LinkedViewToOutputSeries
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            OutputSeries = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToOutputSeries.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                {
                    var newLinkedView = new LinkedViewToOutputClass
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        OutputClass = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToOutputClass.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    var newLinkedView = new LinkedViewToOutput
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        Output = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToOutput.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                {
                    var newLinkedView = new LinkedViewToOutputSeries
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        OutputSeries = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToOutputSeries.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteOutput(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated outputs ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteOutput(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIOutput(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOutput(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteOutput(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputtype.ToString())
                {
                    var outputType = await _dataContext.OutputType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (outputType != null)
                    {
                        _dataContext.Entry(outputType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                {
                    var outputClass = await _dataContext.OutputClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (outputClass != null)
                    {
                        _dataContext.Entry(outputClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    var output = await _dataContext.Output.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (output != null)
                    {
                        _dataContext.Entry(output).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                {
                    var outputserie = await _dataContext.OutputSeries.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (outputserie != null)
                    {
                        _dataContext.Entry(outputserie).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToOutputClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToOutput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToOutputSeries.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
        public async Task<bool> UpdateOutput(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = true;
            //store updated outputs ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateOutput(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIOutput(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIOutput(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateOutput(List<EditHelper.ArgumentsEdits> edits,
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
                    bHasSet = await UpdateOutputLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputtype.ToString())
                    {
                        var outputType = await _dataContext.OutputType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (outputType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(outputType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(outputType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        var outputClass = await _dataContext.OutputClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (outputClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(outputClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(outputClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        var output = await _dataContext.Output.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (output != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(output), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(output).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                    {
                        var outputserie = await _dataContext.OutputSeries.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (outputserie != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(outputserie), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(outputserie).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateOutputLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateOutputLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToOutputClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToOutput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToOutputSeries.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIOutput(_dtoContentURI, bSaveInFileSystemContent);
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
            else if (nodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                var currentObject = await GetOutputClass(id);
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
                        if (_dtoContentURI.URINodeName != Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                        {
                            XElement el = MakeOutputClassXml(currentObject);
                            bHasGoodAncestors = Prices.AddOutputElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, currentObject);
                        }
                    }

                }
            }
            else if (nodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
            {
                var currentObject = await GetOutput(id);
                if (currentObject != null)
                {
                    id = currentObject.OutputClassId;
                    nodeName = Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Prices.OUTPUT_PRICE_TYPES.output.ToString())
                        {
                            XElement el = MakeOutputXml(currentObject);
                            bHasGoodAncestors = Prices.AddOutputElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                var currentObject = await GetOutputSeries(id);
                if (currentObject != null)
                {
                    id = currentObject.OutputId;
                    nodeName = Prices.OUTPUT_PRICE_TYPES.output.ToString();
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
                    bool bHasSet = await SetTempURIOutput(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.OutputClass != null)
                    {
                        if (tempURI.URIModels.Service.OutputClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.OutputClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                var obj2 = await GetOutputClass(childId);
                if (obj2 != null)
                {
                    XElement el = MakeOutputClassXml(obj2);
                    bHasBeenAdded = Prices.AddOutputElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, obj2);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURIOutput(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.OutputClass.Output != null)
                            {
                                if (tempURI.URIModels.OutputClass.Output.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.OutputClass.Output)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Prices.OUTPUT_PRICE_TYPES.output.ToString(),
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
            else if (childNodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
            {
                var obj3 = await GetOutput(childId);
                if (obj3 != null)
                {
                    XElement el = MakeOutputXml(obj3);
                    bHasBeenAdded = Prices.AddOutputElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIOutput(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.Output.OutputSeries != null)
                        {
                            if (tempURI.URIModels.Output.OutputSeries.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.Output.OutputSeries)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Prices.OUTPUT_PRICE_TYPES.outputseries.ToString(),
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
            else if (childNodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                var obj4 = await GetOutputSeries(childId);
                if (obj4 != null)
                {
                    XElement el = MakeOutputSeriesXml(obj4);
                    bHasBeenAdded = Prices.AddOutputElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURIOutput(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIOutput(tempURI, true);
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
                //foreach (var property in currentObjContext.Entity.GetType().GetProperties())
                //{
                //    if (currentObjContext.Property(property.Name) != null)
                //    {
                //        var currentValue = currentObjContext
                //        .Property(property.Name).CurrentValue.ToString();
                //        propValues.Add(property.Name, currentValue);
                //    }
                //}
                EditModelHelper.SetAttributes(propValues, currentNode);
                //EditModelHelper.SetAttributes(currentObjContext.Metadata.GetProperties().ToList(), currentNode);
            }
            return currentNode;
        }
        private async Task<OutputClass> GetOutputClass(int id)
        {
            OutputClass bs = await _dataContext
                    .OutputClass
                    .Include(t => t.OutputType)
                    .Include(t => t.LinkedViewToOutputClass)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOutputClassXml(OutputClass obj)
        {
            XElement currentNode = new XElement(Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString());
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
            if (obj.LinkedViewToOutputClass != null)
            {
                foreach (var lv in obj.LinkedViewToOutputClass)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<Output> GetOutput(int id)
        {
            Output bs = await _dataContext
                    .Output
                    .Include(t => t.LinkedViewToOutput)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOutputXml(Output obj)
        {
            XElement currentNode = new XElement(Prices.OUTPUT_PRICE_TYPES.output.ToString());
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
            if (obj.LinkedViewToOutput != null)
            {
                foreach (var lv in obj.LinkedViewToOutput)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<OutputSeries> GetOutputSeries(int id)
        {
            OutputSeries bs = await _dataContext
                    .OutputSeries
                    .Include(t => t.LinkedViewToOutputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeOutputSeriesXml(OutputSeries obj)
        {
            XElement currentNode = new XElement(Prices.OUTPUT_PRICE_TYPES.outputseries.ToString());
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
            if (obj.LinkedViewToOutputSeries != null)
            {
                foreach (var lv in obj.LinkedViewToOutputSeries)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
    }
}
