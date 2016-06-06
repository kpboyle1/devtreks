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
    ///Purpose:		Entity Framework BudgetsModelHelper support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class BudgetsModelHelper
    {
        public BudgetsModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIBudgets(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var s = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (s != null)
                {
                    // count the budget packs without loading them
                    var qry = _dataContext
                        .BudgetSystem
                        .Include(t => t.LinkedViewToBudgetSystem)
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.BudgetSystem = await qry.ToAsyncEnumerable().ToList();
                        if (s.BudgetSystem != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                               .BudgetSystem
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
                    bHasSet = await SetURIBudgetSystemType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
            {
                var rc = await _dataContext.BudgetSystem.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .BudgetSystemToEnterprise
                        .Include(t => t.LinkedViewToBudgetSystemToEnterprise)
                        .Where(a => a.BudgetSystem.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.BudgetSystemToEnterprise = await qry.ToAsyncEnumerable().ToList();
                        if (rc.BudgetSystemToEnterprise != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                               .BudgetSystemToEnterprise
                               .Where(a => a.BudgetSystem.PKId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.BudgetSystem = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.BUDGET_TYPES.budget.ToString())
            {
                var rp = await _dataContext.BudgetSystemToEnterprise.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .BudgetSystemToTime
                        .Include(t => t.LinkedViewToBudgetSystemToTime)
                        .Where(a => a.BudgetSystemToEnterprise.PKId == uri.URIId)
                        .OrderBy(m => m.Date)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.BudgetSystemToTime = await qry.ToAsyncEnumerable().ToList();
                        if (rp.BudgetSystemToTime != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                               .BudgetSystemToTime
                               .Where(a => a.BudgetSystemToEnterprise.PKId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.BudgetSystemToEnterprise = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
            {
                var rp = await _dataContext.BudgetSystemToTime.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    // count the outcomes first
                    var qry = _dataContext
                        .BudgetSystemToOutcome
                        .Include(t => t.Outcome)
                        .ThenInclude(t => t.LinkedViewToOutcome)
                        .Where(a => a.BudgetSystemToTime.PKId == uri.URIId)
                        .OrderBy(m => m.Date)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.BudgetSystemToOutcome = await qry.ToAsyncEnumerable().ToList();
                        if (rp.BudgetSystemToOutcome != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .BudgetSystemToOutcome
                                .Where(a => a.BudgetSystemToTime.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    // add the operations to the total
                    var qry2 = _dataContext
                        .BudgetSystemToOperation
                        .Include(t => t.Operation)
                        .ThenInclude(t => t.LinkedViewToOperation)
                        .Where(a => a.BudgetSystemToTime.PKId == uri.URIId)
                        .OrderBy(m => m.Date)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry2 != null)
                    {
                        rp.BudgetSystemToOperation = await qry2.ToAsyncEnumerable().ToList();
                        if (rp.BudgetSystemToOperation != null)
                        {
                            uri.URIDataManager.RowCount +=
                              _dataContext
                                .BudgetSystemToOperation
                                .Where(a => a.BudgetSystemToTime.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.BudgetSystemToTime = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.BUDGET_TYPES.budgetoperation.ToString())
            {
                var rp = await _dataContext.BudgetSystemToOperation.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .BudgetSystemToInput
                        .Include(t => t.InputSeries)
                        .ThenInclude(t => t.LinkedViewToInputSeries)
                        .Where(a => a.BudgetSystemToOperation.PKId == uri.URIId)
                        .OrderBy(m => m.InputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.BudgetSystemToInput = await qry.ToAsyncEnumerable().ToList();
                        if (rp.BudgetSystemToInput != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .BudgetSystemToInput
                                .Where(a => a.BudgetSystemToOperation.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.BudgetSystemToOperation = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.BUDGET_TYPES.budgetinput.ToString())
            {
                var qry = _dataContext
                    .BudgetSystemToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(r => r.PKId == uri.URIId);
                if (qry != null)
                {
                    uri.URIModels.BudgetSystemToInput = await qry.FirstOrDefaultAsync();
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                }
            }
            else if (uri.URINodeName
                == Economics1.BUDGET_TYPES.budgetoutcome.ToString())
            {
                var rp = await _dataContext.BudgetSystemToOutcome.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .BudgetSystemToOutput
                        .Include(t => t.OutputSeries)
                        .ThenInclude(t => t.LinkedViewToOutputSeries)
                        .Where(a => a.BudgetSystemToOutcome.PKId == uri.URIId)
                        .OrderBy(m => m.OutputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.BudgetSystemToOutput = await qry.ToAsyncEnumerable().ToList();
                        if (rp.BudgetSystemToOutput != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .BudgetSystemToOutput
                                .Where(a => a.BudgetSystemToOutcome.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.BudgetSystemToOutcome = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.BUDGET_TYPES.budgetoutput.ToString())
            {
                var qry = _dataContext
                    .BudgetSystemToOutput
                    .Include(t => t.OutputSeries)
                    .Include(t => t.OutputSeries.LinkedViewToOutputSeries)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.BudgetSystemToOutput = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIBudgetSystemType(Service s, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.BudgetSystemType = new List<BudgetSystemType>();
                //filter the budget types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .BudgetSystemType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.BudgetSystemType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.BudgetSystemType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    BudgetSystemType rt = await _dataContext.BudgetSystemType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.BudgetSystemType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void GetURIBudgets()
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.BUDGET_TYPES.budgetgroup.ToString())
            {
                var qryRC = _dataContext
                    .BudgetSystem
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.BUDGET_TYPES.budget.ToString())
            {
                var qryRP
                        = _dataContext
                        .BudgetSystemToEnterprise
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
            {
                var qryR = _dataContext
                    .BudgetSystemToTime
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
            == Economics1.BUDGET_TYPES.budgetoutcome.ToString())
            {
                var qryR = _dataContext
                    .BudgetSystemToOutcome
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.BUDGET_TYPES.budgetoutput.ToString())
            {
                var qryR = _dataContext
                    .BudgetSystemToOutput
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.BUDGET_TYPES.budgetoperation.ToString())
            {
                var qryR = _dataContext
                    .BudgetSystemToOperation
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.BUDGET_TYPES.budgetinput.ToString())
            {
                var qryR = _dataContext
                    .BudgetSystemToInput
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddBudgets(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = false;
            bool bHasAdded = await AddBudgets(argumentsEdits.SelectionsToAdd);
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of budgets
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIBudgets(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIBudgets(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddBudgets(List<ContentURI> addedURIs)
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
                if (addedURI.URINodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
                {
                    var newBudgetSystem = new BudgetSystem
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        Date = Helpers.GeneralHelpers.GetDateShortNow(),
                        LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        DocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        BudgetSystemToEnterprise = null,
                        LinkedViewToBudgetSystem = null,
                        TypeId = 0,
                        BudgetSystemType = null,
                        ServiceId = iParentId,
                        Service = null
                    };
                    _dataContext.BudgetSystem.Add(newBudgetSystem);
                    _dataContext.Entry(newBudgetSystem).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Economics1.BUDGET_TYPES.budget.ToString())
                {
                    AccountToLocal local = await Locals.GetDefaultLocal(_dtoContentURI, _dataContext);
                    var newBudgetSystemToEnterprise = new BudgetSystemToEnterprise
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Num2 = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        InitialValue = 0,
                        SalvageValue = 0,
                        LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        RatingClassId = (local != null) ? local.RatingGroupId : 0,
                        RealRateId = (local != null) ? local.RealRateId : 0,
                        NominalRateId = (local != null) ? local.NominalRateId : 0,
                        DataSourceId = (local != null) ? local.DataSourcePriceId : 0,
                        GeoCodeId = (local != null) ? local.GeoCodePriceId : 0,
                        CurrencyClassId = (local != null) ? local.CurrencyGroupId : 0,
                        UnitClassId = (local != null) ? local.UnitGroupId : 0,
                        BudgetSystemId = iParentId,
                        BudgetSystem = null,
                        BudgetSystemToTime = null,
                        LinkedViewToBudgetSystemToEnterprise = null
                    };
                    _dataContext.BudgetSystemToEnterprise.Add(newBudgetSystemToEnterprise);
                    _dataContext.Entry(newBudgetSystemToEnterprise).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                {
                    var budget = await _dataContext.BudgetSystemToEnterprise.SingleOrDefaultAsync(x => x.PKId == iParentId);
                    if (budget != null)
                    {
                        var newBudgetSystemToTime = new BudgetSystemToTime
                        {
                            Num = Helpers.GeneralHelpers.NONE,
                            Name = addedURI.URIName,
                            Description = Helpers.GeneralHelpers.NONE,
                            //to front of list
                            Date = Helpers.GeneralHelpers.GetDateSortOld(),
                            DiscountYorN = true,
                            GrowthPeriods = 0,
                            GrowthTypeId = 0,
                            CommonRefYorN = true,
                            EnterpriseName = Helpers.GeneralHelpers.NONE,
                            EnterpriseUnit = Helpers.GeneralHelpers.NONE,
                            EnterpriseAmount = 1,
                            AOHFactor = 0,
                            IncentiveAmount = 0,
                            IncentiveRate = 0,
                            LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                            BudgetSystemToEnterpriseId = iParentId,
                            BudgetSystemToEnterprise = null,
                            BudgetSystemToOperation = null,
                            BudgetSystemToOutcome = null,
                            LinkedViewToBudgetSystemToTime = null
                        };
                        _dataContext.BudgetSystemToTime.Add(newBudgetSystemToTime);
                        _dataContext.Entry(newBudgetSystemToTime).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    //when the inputs are included in the qry can add them too
                    var outcome = await _dataContext
                    .Outcome
                    .Include(t => t.OutcomeToOutput)
                    .Where(r => r.PKId == addedURI.URIId)
                    .FirstOrDefaultAsync();
                    //var outcome = _dataContext.Outcome.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (outcome != null)
                    {
                        var newBudgetSystemToOutcome = new BudgetSystemToOutcome
                        {
                            Num = outcome.Num,
                            Name = outcome.Name,
                            Description = outcome.Description,
                            ResourceWeight = 0,
                            Amount = outcome.Amount,
                            Unit = outcome.Unit,
                            EffectiveLife = outcome.EffectiveLife,
                            SalvageValue = outcome.SalvageValue,
                            IncentiveAmount = outcome.IncentiveAmount,
                            IncentiveRate = outcome.IncentiveRate,
                            Date = outcome.Date,
                            BudgetSystemToTimeId = iParentId,
                            BudgetSystemToTime = null,
                            OutcomeId = outcome.PKId,
                            Outcome = null,
                            BudgetSystemToOutput = null
                        };
                        _dataContext.BudgetSystemToOutcome.Add(newBudgetSystemToOutcome);
                        _dataContext.Entry(newBudgetSystemToOutcome).State = EntityState.Added;
                        bIsAdded = true;
                        if (outcome.OutcomeToOutput != null)
                        {
                            //save the outcome so the outcome foreign key can be found
                            int iNotUsed = await _dataContext.SaveChangesAsync();
                            foreach (var outputseries in outcome.OutcomeToOutput)
                            {
                                var newBudgetSystemToOutput = new BudgetSystemToOutput
                                {
                                    Num = outputseries.Num,
                                    Name = outputseries.Name,
                                    Description = outputseries.Description,
                                    IncentiveRate = outputseries.IncentiveRate,
                                    IncentiveAmount = outputseries.IncentiveAmount,
                                    OutputAmount1 = outputseries.OutputAmount1,
                                    OutputTimes = outputseries.OutputTimes,
                                    OutputCompositionAmount = outputseries.OutputCompositionAmount,
                                    OutputCompositionUnit = outputseries.OutputCompositionUnit,
                                    OutputDate = outputseries.OutputDate,
                                    RatingClassId = outputseries.RatingClassId,
                                    RealRateId = outputseries.RealRateId,
                                    GeoCodeId = outputseries.GeoCodeId,
                                    NominalRateId = outputseries.NominalRateId,

                                    BudgetSystemToOutcomeId = newBudgetSystemToOutcome.PKId,
                                    BudgetSystemToOutcome = null,
                                    OutputId = outputseries.OutputId,
                                    OutputSeries = null,
                                };
                                _dataContext.BudgetSystemToOutput.Add(newBudgetSystemToOutput);
                                _dataContext.Entry(newBudgetSystemToOutput).State = EntityState.Added;
                                bIsAdded = true;
                            }
                        }
                    }
                }
                else if (addedURI.URINodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                {
                    var outputseries = await _dataContext.OutputSeries.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (outputseries != null)
                    {
                        var newBudgetSystemToOutput = new BudgetSystemToOutput
                        {
                            Num = outputseries.Num,
                            Name = outputseries.Name,
                            Description = outputseries.Description,
                            IncentiveRate = 0,
                            IncentiveAmount = 0,
                            OutputCompositionAmount = 1,
                            OutputCompositionUnit = Helpers.GeneralHelpers.NONE,
                            OutputAmount1 = outputseries.OutputAmount1,
                            OutputTimes = 1,
                            OutputDate = outputseries.OutputDate,
                            RatingClassId = outputseries.RatingClassId,
                            RealRateId = outputseries.RealRateId,
                            NominalRateId = outputseries.NominalRateId,
                            GeoCodeId = outputseries.GeoCodeId,
                            BudgetSystemToOutcomeId = iParentId,
                            BudgetSystemToOutcome = null,
                            OutputId = outputseries.PKId,
                            OutputSeries = null
                        };
                        _dataContext.BudgetSystemToOutput.Add(newBudgetSystemToOutput);
                        _dataContext.Entry(newBudgetSystemToOutput).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                {
                    //when the inputs are included in the qry can add them too
                    var operation = await _dataContext
                    .Operation
                    .Include(t => t.OperationToInput)
                    .Where(r => r.PKId == addedURI.URIId)
                    .FirstOrDefaultAsync();
                    //var operation = _dataContext.Operation.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (operation != null)
                    {
                        var newBudgetSystemToOperation = new BudgetSystemToOperation
                        {
                            Num = operation.Num,
                            Name = operation.Name,
                            Description = operation.Description,
                            ResourceWeight = 0,
                            Amount = operation.Amount,
                            Unit = operation.Unit,
                            EffectiveLife = operation.EffectiveLife,
                            SalvageValue = operation.SalvageValue,
                            IncentiveAmount = operation.IncentiveAmount,
                            IncentiveRate = operation.IncentiveRate,
                            Date = operation.Date,
                            BudgetSystemToTimeId = iParentId,
                            BudgetSystemToTime = null,
                            OperationId = operation.PKId,
                            Operation = null,
                            BudgetSystemToInput = null
                        };
                        _dataContext.BudgetSystemToOperation.Add(newBudgetSystemToOperation);
                        _dataContext.Entry(newBudgetSystemToOperation).State = EntityState.Added;
                        bIsAdded = true;
                        if (operation.OperationToInput != null)
                        {
                            //save the operation so the operation foreign key can be found
                            int iNotUsed = await _dataContext.SaveChangesAsync();
                            foreach (var inputseries in operation.OperationToInput)
                            {
                                var newBudgetSystemToInput = new BudgetSystemToInput
                                {
                                    Num = inputseries.Num,
                                    Name = inputseries.Name,
                                    Description = inputseries.Description,
                                    IncentiveRate = inputseries.IncentiveRate,
                                    IncentiveAmount = inputseries.IncentiveAmount,
                                    InputPrice1Amount = inputseries.InputPrice1Amount,
                                    InputPrice2Amount = inputseries.InputPrice2Amount,
                                    InputPrice3Amount = inputseries.InputPrice3Amount,
                                    InputTimes = inputseries.InputTimes,
                                    InputDate = inputseries.InputDate,
                                    InputUseAOHOnly = false,
                                    RatingClassId = inputseries.RatingClassId,
                                    RealRateId = inputseries.RealRateId,
                                    GeoCodeId = inputseries.GeoCodeId,
                                    NominalRateId = inputseries.NominalRateId,

                                    BudgetSystemToOperationId = newBudgetSystemToOperation.PKId,
                                    BudgetSystemToOperation = null,
                                    InputId = inputseries.InputId,
                                    InputSeries = null,
                                };
                                _dataContext.BudgetSystemToInput.Add(newBudgetSystemToInput);
                                _dataContext.Entry(newBudgetSystemToInput).State = EntityState.Added;
                                bIsAdded = true;
                            }
                        }
                    }
                }
                else if (addedURI.URINodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                {
                    var inputseries = await _dataContext.InputSeries.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (inputseries != null)
                    {
                        var newBudgetSystemToInput = new BudgetSystemToInput
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
                            GeoCodeId = inputseries.GeoCodeId,
                            NominalRateId = inputseries.NominalRateId,
        
                            BudgetSystemToOperationId = iParentId,
                            BudgetSystemToOperation = null,
                            InputId = inputseries.PKId,
                            InputSeries = null,
                        };
                        _dataContext.BudgetSystemToInput.Add(newBudgetSystemToInput);
                        _dataContext.Entry(newBudgetSystemToInput).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddBudgetsLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddBudgetsLinkedView(ContentURI addedURI,
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
                    if (inputNodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        var newLinkedView = new LinkedViewToBudgetSystem
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            BudgetSystem = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToBudgetSystem.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Economics1.BUDGET_TYPES.budget.ToString())
                    {
                        var newLinkedView = new LinkedViewToBudgetSystemToEnterprise
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            BudgetSystemToEnterprise = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToBudgetSystemToEnterprise.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                    {
                        var newLinkedView = new LinkedViewToBudgetSystemToTime
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            BudgetSystemToTime = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToBudgetSystemToTime.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
                {
                    var newLinkedView = new LinkedViewToBudgetSystem
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        BudgetSystem = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToBudgetSystem.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Economics1.BUDGET_TYPES.budget.ToString())
                {
                    var newLinkedView = new LinkedViewToBudgetSystemToEnterprise
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        BudgetSystemToEnterprise = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToBudgetSystemToEnterprise.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                {
                    var newLinkedView = new LinkedViewToBudgetSystemToTime
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        BudgetSystemToTime = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToBudgetSystemToTime.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteBudgets(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            bool bHasSet = true;
            //store updated budgets ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bHasSet = await DeleteBudgets(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIBudgets(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIBudgets(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteBudgets(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budgettype.ToString())
                {
                    var budgetType = await _dataContext.BudgetSystemType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budgetType != null)
                    {
                        _dataContext.Entry(budgetType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
                {
                    var budgetClass = await _dataContext.BudgetSystem.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budgetClass != null)
                    {
                        _dataContext.Entry(budgetClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budget.ToString())
                {
                    var budget = await _dataContext.BudgetSystemToEnterprise.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budget != null)
                    {
                        _dataContext.Entry(budget).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                {
                    var budgettotime = await _dataContext.BudgetSystemToTime.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budgettotime != null)
                    {
                        _dataContext.Entry(budgettotime).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budgetoutcome.ToString())
                {
                    var budgetoutcome = await _dataContext.BudgetSystemToOutcome.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budgetoutcome != null)
                    {
                        _dataContext.Entry(budgetoutcome).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budgetoutput.ToString())
                {
                    var budgettooutput = await _dataContext.BudgetSystemToOutput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budgettooutput != null)
                    {
                        _dataContext.Entry(budgettooutput).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budgetoperation.ToString())
                {
                    var budgettooperation = await _dataContext.BudgetSystemToOperation.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budgettooperation != null)
                    {
                        _dataContext.Entry(budgettooperation).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.BUDGET_TYPES.budgetinput.ToString())
                {
                    var budgettoinput = await _dataContext.BudgetSystemToInput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (budgettoinput != null)
                    {
                        _dataContext.Entry(budgettoinput).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToBudgetSystem.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Economics1.BUDGET_TYPES.budget.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToBudgetSystemToEnterprise.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToBudgetSystemToTime.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
        public async Task<bool> UpdateBudgets(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = true;
            //store updated budgets ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateBudgets(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIBudgets(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIBudgets(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateBudgets(List<EditHelper.ArgumentsEdits> edits,
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
                    bHasSet = await UpdateBudgetsLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budgettype.ToString())
                    {
                        var budgetType = await _dataContext.BudgetSystemType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budgetType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budgetType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budgetType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        var budgetClass = await _dataContext.BudgetSystem.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budgetClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budgetClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budgetClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budget.ToString())
                    {
                        var budget = await _dataContext.BudgetSystemToEnterprise.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budget != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budget), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budget).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                    {
                        var budgettimeperiod = await _dataContext.BudgetSystemToTime.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budgettimeperiod != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budgettimeperiod), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budgettimeperiod).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budgetoutcome.ToString())
                    {
                        var budgetoutcome = await _dataContext.BudgetSystemToOutcome.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budgetoutcome != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budgetoutcome), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budgetoutcome).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budgetoutput.ToString())
                    {
                        var budgetoutput = await _dataContext.BudgetSystemToOutput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budgetoutput != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budgetoutput), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budgetoutput).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budgetoperation.ToString())
                    {
                        var budgetoperation = await _dataContext.BudgetSystemToOperation.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budgetoperation != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budgetoperation), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budgetoperation).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.BUDGET_TYPES.budgetinput.ToString())
                    {
                        var budgetinput = await _dataContext.BudgetSystemToInput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (budgetinput != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(budgetinput), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(budgetinput).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateBudgetsLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateBudgetsLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            string sKeyName = string.Empty;
            bool bHasSet = true;
            if (inputNodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToBudgetSystem.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Economics1.BUDGET_TYPES.budget.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToBudgetSystemToEnterprise.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToBudgetSystemToTime.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIBudgets(_dtoContentURI, bSaveInFileSystemContent);
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
            else if (nodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
            {
                var currentObject = await GetBudgetSystem(id);
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
                        if (_dtoContentURI.URINodeName != Economics1.BUDGET_TYPES.budgetgroup.ToString())
                        {
                            XElement el = MakeBudgetSystemXml(currentObject);
                            bHasGoodAncestors = Economics1.AddBudgetElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, currentObject);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.BUDGET_TYPES.budget.ToString())
            {
                var currentObject = await GetBudget(id);
                if (currentObject != null)
                {
                    id = currentObject.BudgetSystemId;
                    nodeName = Economics1.BUDGET_TYPES.budgetgroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.BUDGET_TYPES.budget.ToString())
                        {
                            XElement el = MakeBudgetXml(currentObject);
                            bHasGoodAncestors = Economics1.AddBudgetElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
            {
                var currentObject = await GetBudgetTime(id);
                if (currentObject != null)
                {
                    id = currentObject.BudgetSystemToEnterpriseId;
                    nodeName = Economics1.BUDGET_TYPES.budget.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                        {
                            XElement el = MakeBudgetSystemToTimeXml(currentObject);
                            bHasGoodAncestors = Economics1.AddBudgetElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                            //add the grouping nodes
                            XElement groupOutcome = XElement.Parse(Economics1.BUDGET_OUTCOMES_NODE);
                            bHasGoodAncestors = Economics1.AddBudgetElementToParent(root, groupOutcome,
                                string.Empty, currentObject.PKId.ToString(), Economics1.BUDGET_TYPES.budgettimeperiod.ToString(), null);
                            //add a grouping node
                            XElement groupOperation = XElement.Parse(Economics1.BUDGET_OPERATIONS_NODE);
                            bHasGoodAncestors = Economics1.AddBudgetElementToParent(root, groupOperation,
                                string.Empty, currentObject.PKId.ToString(), Economics1.BUDGET_TYPES.budgettimeperiod.ToString(), null);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.BUDGET_TYPES.budgetoutcome.ToString())
            {
                var currentObject = await GetBudgetOutcome(id);
                if (currentObject != null)
                {
                    id = currentObject.BudgetSystemToTimeId;
                    nodeName = Economics1.BUDGET_TYPES.budgettimeperiod.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.BUDGET_TYPES.budgetoutcome.ToString())
                        {
                            XElement el = MakeBudgetSystemToOutcomeXml(currentObject);
                            bHasGoodAncestors = Economics1.AddBudgetElementToParent(root, el,
                                Economics1.BUDGET_TYPES.budgetoutcomes.ToString(), id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.BUDGET_TYPES.budgetoutput.ToString())
            {
                var currentObject = await GetBudgetOutput(id);
                if (currentObject != null)
                {
                    id = currentObject.BudgetSystemToOutcomeId;
                    nodeName = Economics1.BUDGET_TYPES.budgetoutcome.ToString();
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
            else if (nodeName == Economics1.BUDGET_TYPES.budgetoperation.ToString())
            {
                var currentObject = await GetBudgetOperation(id);
                if (currentObject != null)
                {
                    id = currentObject.BudgetSystemToTimeId;
                    nodeName = Economics1.BUDGET_TYPES.budgettimeperiod.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.BUDGET_TYPES.budgetoperation.ToString())
                        {
                            XElement el = MakeBudgetSystemToOperationXml(currentObject);
                            bHasGoodAncestors = Economics1.AddBudgetElementToParent(root, el,
                                Economics1.BUDGET_TYPES.budgetoperations.ToString(), id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.BUDGET_TYPES.budgetinput.ToString())
            {
                var currentObject = await GetBudgetInput(id);
                if (currentObject != null)
                {
                    id = currentObject.BudgetSystemToOperationId;
                    nodeName = Economics1.BUDGET_TYPES.budgetoperation.ToString();
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
                    bool bHasSet = await SetTempURIBudgets(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.BudgetSystem != null)
                    {
                        if (tempURI.URIModels.Service.BudgetSystem.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.BudgetSystem)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Economics1.BUDGET_TYPES.budgetgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString())
            {
                var obj2 = await GetBudgetSystem(childId);
                if (obj2 != null)
                {
                    XElement el = MakeBudgetSystemXml(obj2);
                    bHasBeenAdded = Economics1.AddBudgetElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, obj2);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURIBudgets(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.BudgetSystem.BudgetSystemToEnterprise != null)
                            {
                                if (tempURI.URIModels.BudgetSystem.BudgetSystemToEnterprise.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.BudgetSystem.BudgetSystemToEnterprise)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Economics1.BUDGET_TYPES.budget.ToString(),
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
            else if (childNodeName == Economics1.BUDGET_TYPES.budget.ToString())
            {
                var obj3 = await GetBudget(childId);
                if (obj3 != null)
                {
                    XElement el = MakeBudgetXml(obj3);
                    bHasBeenAdded = Economics1.AddBudgetElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIBudgets(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.BudgetSystemToEnterprise.BudgetSystemToTime != null)
                        {
                            if (tempURI.URIModels.BudgetSystemToEnterprise.BudgetSystemToTime.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.BudgetSystemToEnterprise.BudgetSystemToTime)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Economics1.BUDGET_TYPES.budgettimeperiod.ToString(),
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
            else if (childNodeName == Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
            {
                var obj4 = await GetBudgetTime(childId);
                if (obj4 != null)
                {
                    XElement el = MakeBudgetSystemToTimeXml(obj4);
                    bHasBeenAdded = Economics1.AddBudgetElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIBudgets(tempURI, childNodeName, childId);
                        //add the grouping nodes
                        XElement groupOutcome = XElement.Parse(Economics1.BUDGET_OUTCOMES_NODE);
                        bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, groupOutcome,
                            string.Empty, childId.ToString(), childNodeName);
                        //add a grouping node
                        XElement groupOperation = XElement.Parse(Economics1.BUDGET_OPERATIONS_NODE);
                        bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, groupOperation,
                            string.Empty, childId.ToString(), childNodeName);
                        if (bHasBeenAdded)
                        {
                            //first do the outcomes
                            if (tempURI.URIModels.BudgetSystemToTime.BudgetSystemToOutcome != null)
                            {
                                if (tempURI.URIModels.BudgetSystemToTime.BudgetSystemToOutcome.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.BudgetSystemToTime.BudgetSystemToOutcome)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Economics1.BUDGET_TYPES.budgetoutcome.ToString(),
                                        root);
                                }
                            }
                            else
                            {
                                bHasGoodDescendants = true;
                            }
                            //then the operations
                            if (tempURI.URIModels.BudgetSystemToTime.BudgetSystemToOperation != null)
                            {
                                if (tempURI.URIModels.BudgetSystemToTime.BudgetSystemToOperation.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.BudgetSystemToTime.BudgetSystemToOperation)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Economics1.BUDGET_TYPES.budgetoperation.ToString(),
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
            }
            else if (childNodeName == Economics1.BUDGET_TYPES.budgetoutcome.ToString())
            {
                var obj4 = await GetBudgetOutcome(childId);
                if (obj4 != null)
                {
                    XElement el = MakeBudgetSystemToOutcomeXml(obj4);
                    bHasBeenAdded = Economics1.AddBudgetElementToParent(root, el,
                            Economics1.BUDGET_TYPES.budgetoutcomes.ToString(), parentId.ToString(),
                            parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIBudgets(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.BudgetSystemToOutcome.BudgetSystemToOutput != null)
                        {
                            if (tempURI.URIModels.BudgetSystemToOutcome.BudgetSystemToOutput.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.BudgetSystemToOutcome.BudgetSystemToOutput)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Economics1.BUDGET_TYPES.budgetoutput.ToString(),
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
            else if (childNodeName == Economics1.BUDGET_TYPES.budgetoutput.ToString())
            {
                var obj4 = await GetBudgetOutput(childId);
                if (obj4 != null)
                {
                    XElement el = MakeBudgetSystemToOutputXml(obj4);
                    bHasBeenAdded = Economics1.AddBudgetElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Economics1.BUDGET_TYPES.budgetoperation.ToString())
            {
                var obj4 = await GetBudgetOperation(childId);
                if (obj4 != null)
                {
                    XElement el = MakeBudgetSystemToOperationXml(obj4);
                    bHasBeenAdded = Economics1.AddBudgetElementToParent(root, el,
                            Economics1.BUDGET_TYPES.budgetoperations.ToString(), parentId.ToString(), 
                            parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIBudgets(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.BudgetSystemToOperation.BudgetSystemToInput != null)
                        {
                            if (tempURI.URIModels.BudgetSystemToOperation.BudgetSystemToInput.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.BudgetSystemToOperation.BudgetSystemToInput)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Economics1.BUDGET_TYPES.budgetinput.ToString(),
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
            else if (childNodeName == Economics1.BUDGET_TYPES.budgetinput.ToString())
            {
                var obj4 = await GetBudgetInput(childId);
                if (obj4 != null)
                {
                    XElement el = MakeBudgetSystemToInputXml(obj4);
                    bHasBeenAdded = Economics1.AddBudgetElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURIBudgets(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIBudgets(tempURI, true);
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
        
        private async Task<BudgetSystem> GetBudgetSystem(int id)
        {
            BudgetSystem bs = await _dataContext
                    .BudgetSystem
                    .Include(t => t.BudgetSystemType)
                    .Include(t => t.LinkedViewToBudgetSystem)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeBudgetSystemXml(BudgetSystem obj)
        {
            XElement currentNode = new XElement(Economics1.BUDGET_TYPES.budgetgroup.ToString());
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
            if (obj.LinkedViewToBudgetSystem != null)
            {
                foreach (var lv in obj.LinkedViewToBudgetSystem)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<BudgetSystemToEnterprise> GetBudget(int id)
        {
            BudgetSystemToEnterprise bs = await _dataContext
                    .BudgetSystemToEnterprise
                    .Include(t => t.LinkedViewToBudgetSystemToEnterprise)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeBudgetXml(BudgetSystemToEnterprise obj)
        {
            XElement currentNode = new XElement(Economics1.BUDGET_TYPES.budget.ToString());
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
            if (obj.LinkedViewToBudgetSystemToEnterprise != null)
            {
                foreach (var lv in obj.LinkedViewToBudgetSystemToEnterprise)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<BudgetSystemToTime> GetBudgetTime(int id)
        {
            BudgetSystemToTime bs = await _dataContext
                    .BudgetSystemToTime
                    .Include(t => t.LinkedViewToBudgetSystemToTime)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeBudgetSystemToTimeXml(BudgetSystemToTime obj)
        {
            XElement currentNode = new XElement(Economics1.BUDGET_TYPES.budgettimeperiod.ToString());
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
            if (obj.LinkedViewToBudgetSystemToTime != null)
            {
                foreach (var lv in obj.LinkedViewToBudgetSystemToTime)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<BudgetSystemToOutcome> GetBudgetOutcome(int id)
        {
            BudgetSystemToOutcome bs = await _dataContext
                    .BudgetSystemToOutcome
                    .Include(t => t.Outcome)
                    .Include(t => t.Outcome.OutcomeClass)
                    .Include(t => t.Outcome.OutcomeClass.OutcomeType)
                    .Include(t => t.Outcome.LinkedViewToOutcome)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeBudgetSystemToOutcomeXml(BudgetSystemToOutcome obj)
        {
            XElement currentNode = new XElement(Economics1.BUDGET_TYPES.budgetoutcome.ToString());
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
            if (obj.Outcome != null)
            {
                Economics1.AddBaseOutcomeToXml(currentNode, obj);
                if (obj.Outcome.LinkedViewToOutcome != null)
                {
                    foreach (var lv in obj.Outcome.LinkedViewToOutcome)
                    {
                        EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                    }
                }
            }
            return currentNode;
        }
        private async Task<BudgetSystemToOutput> GetBudgetOutput(int id)
        {
            BudgetSystemToOutput bs = await _dataContext
                    .BudgetSystemToOutput
                    .Include(t => t.OutputSeries)
                    .Include(t => t.OutputSeries.Output.OutputClass)
                    .Include(t => t.OutputSeries.Output.OutputClass.OutputType)
                    .Include(t => t.OutputSeries.LinkedViewToOutputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeBudgetSystemToOutputXml(BudgetSystemToOutput obj)
        {
            XElement currentNode = new XElement(Economics1.BUDGET_TYPES.budgetoutput.ToString());
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
            if (obj.OutputSeries != null)
            {
                Economics1.AddBaseOutputSeriesToXml(currentNode, obj);
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
        private async Task<BudgetSystemToOperation> GetBudgetOperation(int id)
        {
            BudgetSystemToOperation bs = await _dataContext
                    .BudgetSystemToOperation
                    .Include(t => t.Operation)
                    .Include(t => t.Operation.OperationClass)
                    .Include(t => t.Operation.OperationClass.OperationType)
                    .Include(t => t.Operation.LinkedViewToOperation)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeBudgetSystemToOperationXml(BudgetSystemToOperation obj)
        {
            XElement currentNode = new XElement(Economics1.BUDGET_TYPES.budgetoperation.ToString());
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
            if (obj.Operation != null)
            {
                Economics1.AddBaseOperationToXml(currentNode, obj);
                if (obj.Operation.LinkedViewToOperation != null)
                {
                    foreach (var lv in obj.Operation.LinkedViewToOperation)
                    {
                        EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                    }
                }
            }
            return currentNode;
        }
        private async Task<BudgetSystemToInput> GetBudgetInput(int id)
        {
            BudgetSystemToInput bs = await _dataContext
                    .BudgetSystemToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.Input.InputClass)
                    .Include(t => t.InputSeries.Input.InputClass.InputType)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeBudgetSystemToInputXml(BudgetSystemToInput obj)
        {
            XElement currentNode = new XElement(Economics1.BUDGET_TYPES.budgetinput.ToString());
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
            if (obj.InputSeries != null)
            {
                Economics1.AddBaseInputSeriesToXml(currentNode, obj);
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
