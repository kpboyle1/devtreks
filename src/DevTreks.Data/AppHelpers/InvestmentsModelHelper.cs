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
    ///Purpose:		Entity Framework InvestmentssModelHelper support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class InvestmentsModelHelper
    {
        public InvestmentsModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIInvestments(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSet = false;
            int iStartRow = uri.URIDataManager.StartRow;
            int iPageSize = uri.URIDataManager.PageSize;
            if (uri.URINodeName 
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var s = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (s != null)
                {
                    var qry = _dataContext
                        .CostSystem
                        .Include(t => t.LinkedViewToCostSystem)
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.CostSystem = await qry.ToAsyncEnumerable().ToList();
                        if (s.CostSystem != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .CostSystem
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
                    bHasSet = await SetURICostSystemType(s, uri);
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName 
                == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                var rc = await _dataContext.CostSystem.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .CostSystemToPractice
                        .Include(t => t.LinkedViewToCostSystemToPractice)
                        .Where(a => a.CostSystem.PKId == uri.URIId)
                        .OrderBy(m => m.Name)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.CostSystemToPractice = await qry.ToAsyncEnumerable().ToList();
                        if (rc.CostSystemToPractice != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .CostSystemToPractice
                                .Where(a => a.CostSystem.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.CostSystem = rc;
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.INVESTMENT_TYPES.investment.ToString())
            {
                var rp = await _dataContext.CostSystemToPractice.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .CostSystemToTime
                        .Include(t => t.LinkedViewToCostSystemToTime)
                        .Where(a => a.CostSystemToPractice.PKId == uri.URIId)
                        .OrderBy(m => m.Date)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.CostSystemToTime = await qry.ToAsyncEnumerable().ToList();
                        if (rp.CostSystemToTime != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .CostSystemToTime
                                .Where(a => a.CostSystemToPractice.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.CostSystemToPractice = rp;
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                var rp = await _dataContext.CostSystemToTime.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .CostSystemToOutcome
                        .Include(t => t.Outcome)
                        .ThenInclude(t => t.LinkedViewToOutcome)
                        .Where(a => a.CostSystemToTime.PKId == uri.URIId)
                        .OrderBy(m => m.Date)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.CostSystemToOutcome = await qry.ToAsyncEnumerable().ToList();
                        if (rp.CostSystemToOutcome != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .CostSystemToOutcome
                                .Where(a => a.CostSystemToTime.PKId == uri.URIId)
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
                        .CostSystemToComponent
                        .Include(t => t.Component)
                        .ThenInclude(t => t.LinkedViewToComponent)
                        .Where(a => a.CostSystemToTime.PKId == uri.URIId)
                        .OrderBy(m => m.Date)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry2 != null)
                    {
                        rp.CostSystemToComponent = await qry2.ToAsyncEnumerable().ToList();
                        if (rp.CostSystemToComponent != null)
                        {
                            uri.URIDataManager.RowCount +=
                                _dataContext
                                .CostSystemToComponent
                                .Where(a => a.CostSystemToTime.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.CostSystemToTime = rp;
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
            {
                var rp = await _dataContext.CostSystemToComponent.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .CostSystemToInput
                        .Include(t => t.InputSeries)
                        .ThenInclude(t => t.LinkedViewToInputSeries)
                        .Where(a => a.CostSystemToComponent.PKId == uri.URIId)
                        .OrderBy(m => m.InputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.CostSystemToInput = await qry.ToAsyncEnumerable().ToList();
                        if (rp.CostSystemToInput != null)
                        {
                            uri.URIDataManager.RowCount =
                                _dataContext
                                .CostSystemToInput
                                .Where(a => a.CostSystemToComponent.PKId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    //set the parent object
                    uri.URIModels.CostSystemToComponent = rp;
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentinput.ToString())
            {
                var qry = _dataContext
                    .CostSystemToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.CostSystemToInput = await qry.FirstOrDefaultAsync();
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                }
            }
            else if (uri.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
            {
                var rp = await _dataContext.CostSystemToOutcome.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .CostSystemToOutput
                        .Include(t => t.OutputSeries)
                        .ThenInclude(t => t.LinkedViewToOutputSeries)
                        .Where(a => a.CostSystemToOutcome.PKId == uri.URIId)
                        .OrderBy(m => m.OutputDate)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.CostSystemToOutput = await qry.ToAsyncEnumerable().ToList();
                        if (rp.CostSystemToOutput != null)
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
                    //set the parent object
                    uri.URIModels.CostSystemToOutcome = rp;
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentoutput.ToString())
            {
                var qry = _dataContext
                    .CostSystemToOutput
                    .Include(t => t.OutputSeries)
                    .Include(t => t.OutputSeries.LinkedViewToOutputSeries)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.CostSystemToOutput = await qry.FirstOrDefaultAsync();
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
        
        private async Task<bool> SetURICostSystemType(Service s, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.CostSystemType = new List<CostSystemType>();
                //filter the investment types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .CostSystemType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.CostSystemType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.CostSystemType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    CostSystemType rt = await _dataContext.CostSystemType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.CostSystemType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void GetURIInvestments()
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                var qryRC = _dataContext
                    .CostSystem
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.INVESTMENT_TYPES.investment.ToString())
            {
                var qryRP
                        = _dataContext
                        .CostSystemToPractice
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                var qryR = _dataContext
                    .CostSystemToTime
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
            == Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
            {
                var qryR = _dataContext
                    .CostSystemToOutcome
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentoutput.ToString())
            {
                var qryR = _dataContext
                    .CostSystemToOutput
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
            {
                var qryR = _dataContext
                    .CostSystemToComponent
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Economics1.INVESTMENT_TYPES.investmentinput.ToString())
            {
                var qryR = _dataContext
                    .CostSystemToInput
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddInvestments(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = false;
            //store updated investments ids in lists
            bool bHasAdded = await AddInvestments(argumentsEdits.SelectionsToAdd);
            //int iNewId = 0;
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of investments
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIInvestments(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIInvestments(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddInvestments(List<ContentURI> addedURIs)
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
                if (addedURI.URINodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                {
                    var newCostSystem = new CostSystem
                    {
                        Num = Helpers.GeneralHelpers.NONE,
                        Name = addedURI.URIName,
                        Description = Helpers.GeneralHelpers.NONE,
                        Date = Helpers.GeneralHelpers.GetDateShortNow(),
                        LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        DocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        CostSystemToPractice = null,
                        LinkedViewToCostSystem = null,
                        TypeId = 0,
                        CostSystemType = null,
                        ServiceId = iParentId,
                        Service = null
                    };
                    _dataContext.CostSystem.Add(newCostSystem);
                    _dataContext.Entry(newCostSystem).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
                {
                    AccountToLocal local = await Locals.GetDefaultLocal(_dtoContentURI, _dataContext);
                    var newCostSystemToPractice = new CostSystemToPractice
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
                        CostSystemId = iParentId,
                        CostSystem = null,
                        CostSystemToTime = null,
                        LinkedViewToCostSystemToPractice = null
                    };
                    _dataContext.CostSystemToPractice.Add(newCostSystemToPractice);
                    _dataContext.Entry(newCostSystemToPractice).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    var investment = await _dataContext.CostSystemToPractice.SingleOrDefaultAsync(x => x.PKId == iParentId);
                    if (investment != null)
                    {
                        var newCostSystemToTime = new CostSystemToTime
                        {
                            Num = Helpers.GeneralHelpers.NONE,
                            Name = addedURI.URIName,
                            Description = Helpers.GeneralHelpers.NONE,
                            //front of list
                            Date = Helpers.GeneralHelpers.GetDateSortOld(),
                            DiscountYorN = true,
                            GrowthPeriods = 0,
                            GrowthTypeId = 0,
                            CommonRefYorN = true,
                            PracticeName = Helpers.GeneralHelpers.NONE,
                            PracticeUnit = Helpers.GeneralHelpers.NONE,
                            PracticeAmount = 1,
                            AOHFactor = 0,
                            IncentiveAmount = 0,
                            IncentiveRate = 0,
                            LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                            CostSystemToPracticeId = iParentId,
                            CostSystemToPractice = null,
                            CostSystemToComponent = null,
                            CostSystemToOutcome = null,
                            LinkedViewToCostSystemToTime = null
                        };
                        _dataContext.CostSystemToTime.Add(newCostSystemToTime);
                        _dataContext.Entry(newCostSystemToTime).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    //when the outputs are included in the qry can add them too
                    var outcome = await _dataContext
                    .Outcome
                    .Include(t => t.OutcomeToOutput)
                    .Where(r => r.PKId == addedURI.URIId)
                    .FirstOrDefaultAsync();
                    //var outcome = await _dataContext.Outcome.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (outcome != null)
                    {
                        var newCostSystemToOutcome = new CostSystemToOutcome
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
                            CostSystemToTimeId = iParentId,
                            CostSystemToTime = null,
                            OutcomeId = outcome.PKId,
                            Outcome = null,
                            CostSystemToOutput = null
                        };
                        _dataContext.CostSystemToOutcome.Add(newCostSystemToOutcome);
                        _dataContext.Entry(newCostSystemToOutcome).State = EntityState.Added;
                        bIsAdded = true;
                        if (outcome.OutcomeToOutput != null)
                        {
                            //save the outcome so the outcome foreign key can be found
                            int iNotUsed = await _dataContext.SaveChangesAsync();
                            foreach (var outputseries in outcome.OutcomeToOutput)
                            {
                                var newCostSystemToOutput = new CostSystemToOutput
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

                                    CostSystemToOutcomeId = newCostSystemToOutcome.PKId,
                                    CostSystemToOutcome = null,
                                    OutputId = outputseries.OutputId,
                                    OutputSeries = null,
                                };
                                _dataContext.CostSystemToOutput.Add(newCostSystemToOutput);
                                _dataContext.Entry(newCostSystemToOutput).State = EntityState.Added;
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
                        var newCostSystemToOutput = new CostSystemToOutput
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
                            CostSystemToOutcomeId = iParentId,
                            CostSystemToOutcome = null,
                            OutputId = outputseries.PKId,
                            OutputSeries = null
                        };
                        _dataContext.CostSystemToOutput.Add(newCostSystemToOutput);
                        _dataContext.Entry(newCostSystemToOutput).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    //when the inputs are included in the qry can add them too
                    var component = await _dataContext
                    .Component
                    .Include(t => t.ComponentToInput)
                    .Where(r => r.PKId == addedURI.URIId)
                    .FirstOrDefaultAsync();
                    //var component =await  _dataContext.Component.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                    if (component != null)
                    {
                        var newCostSystemToComponent = new CostSystemToComponent
                        {
                            Num = component.Num,
                            Name = component.Name,
                            Description = component.Description,
                            ResourceWeight = 0,
                            Amount = component.Amount,
                            Unit = component.Unit,
                            EffectiveLife = component.EffectiveLife,
                            SalvageValue = component.SalvageValue,
                            IncentiveAmount = component.IncentiveAmount,
                            IncentiveRate = component.IncentiveRate,
                            Date = component.Date,
                            CostSystemToTimeId = iParentId,
                            CostSystemToTime = null,
                            ComponentId = component.PKId,
                            Component = null,
                            CostSystemToInput = null
                        };
                        _dataContext.CostSystemToComponent.Add(newCostSystemToComponent);
                        _dataContext.Entry(newCostSystemToComponent).State = EntityState.Added;
                        bIsAdded = true;
                        if (component.ComponentToInput != null)
                        {
                            //save the component so the component foreign key can be found
                            int iNotUsed = await _dataContext.SaveChangesAsync();
                            foreach (var inputseries in component.ComponentToInput)
                            {
                                var newCostSystemToInput = new CostSystemToInput
                                {
                                    Num = inputseries.Num,
                                    Name = inputseries.Name,
                                    Description = inputseries.Description,
                                    IncentiveRate = inputseries.IncentiveRate,
                                    IncentiveAmount = inputseries.IncentiveAmount,
                                    InputPrice1Amount = inputseries.InputPrice1Amount,
                                    InputPrice2Amount = inputseries.InputPrice2Amount,
                                    InputPrice3Amount = inputseries.InputPrice3Amount,
                                    InputTimes = 1,
                                    InputDate = inputseries.InputDate,
                                    InputUseAOHOnly = false,
                                    RatingClassId = inputseries.RatingClassId,
                                    RealRateId = inputseries.RealRateId,
                                    GeoCodeId = inputseries.GeoCodeId,
                                    NominalRateId = inputseries.NominalRateId,

                                    CostSystemToComponentId = newCostSystemToComponent.PKId,
                                    CostSystemToComponent = null,
                                    InputId = inputseries.InputId,
                                    InputSeries = null,
                                };
                                _dataContext.CostSystemToInput.Add(newCostSystemToInput);
                                _dataContext.Entry(newCostSystemToInput).State = EntityState.Added;
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
                        var newCostSystemToInput = new CostSystemToInput
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

                            CostSystemToComponentId = iParentId,
                            CostSystemToComponent = null,
                            InputId = inputseries.PKId,
                            InputSeries = null,
                        };
                        _dataContext.CostSystemToInput.Add(newCostSystemToInput);
                        _dataContext.Entry(newCostSystemToInput).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddInvestmentsLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddInvestmentsLinkedView(ContentURI addedURI,
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
                    if (inputNodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        var newLinkedView = new LinkedViewToCostSystem
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            CostSystem = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToCostSystem.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
                    {
                        var newLinkedView = new LinkedViewToCostSystemToPractice
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            CostSystemToPractice = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToCostSystemToPractice.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        var newLinkedView = new LinkedViewToCostSystemToTime
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            CostSystemToTime = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToCostSystemToTime.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                {
                    var newLinkedView = new LinkedViewToCostSystem
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        CostSystem = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToCostSystem.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
                {
                    var newLinkedView = new LinkedViewToCostSystemToPractice
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        CostSystemToPractice = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToCostSystemToPractice.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    var newLinkedView = new LinkedViewToCostSystemToTime
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        CostSystemToTime = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToCostSystemToTime.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteInvestments(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            bool bHasSet = true;
            //store updated investments ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bIsDeleted = await DeleteInvestments(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIInvestments(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIInvestments(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteInvestments(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investmenttype.ToString())
                {
                    var investmentType = await _dataContext.CostSystemType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investmentType != null)
                    {
                        _dataContext.Entry(investmentType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                {
                    var investmentClass = await _dataContext.CostSystem.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investmentClass != null)
                    {
                        _dataContext.Entry(investmentClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
                {
                    var investment = await _dataContext.CostSystemToPractice.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investment != null)
                    {
                        _dataContext.Entry(investment).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    var investmenttotime = await _dataContext.CostSystemToTime.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investmenttotime != null)
                    {
                        _dataContext.Entry(investmenttotime).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                {
                    var investmenttooutcome = await _dataContext.CostSystemToOutcome.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investmenttooutcome != null)
                    {
                        _dataContext.Entry(investmenttooutcome).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investmentoutput.ToString())
                {
                    var investmenttooutput = await _dataContext.CostSystemToOutput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investmenttooutput != null)
                    {
                        _dataContext.Entry(investmenttooutput).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
                {
                    var investmenttocomponent = await _dataContext.CostSystemToComponent.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investmenttocomponent != null)
                    {
                        _dataContext.Entry(investmenttocomponent).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Economics1.INVESTMENT_TYPES.investmentinput.ToString())
                {
                    var investmenttoinput = await _dataContext.CostSystemToInput.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (investmenttoinput != null)
                    {
                        _dataContext.Entry(investmenttoinput).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToCostSystem.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToCostSystemToPractice.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToCostSystemToTime.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
        public async Task<bool> UpdateInvestments(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = true;
            //store updated investments ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateInvestments(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIInvestments(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIInvestments(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateInvestments(List<EditHelper.ArgumentsEdits> edits,
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
                    bHasSet = await UpdateInvestmentsLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investmenttype.ToString())
                    {
                        var investmentType = await _dataContext.CostSystemType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investmentType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investmentType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investmentType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        var investmentClass = await _dataContext.CostSystem.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investmentClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investmentClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investmentClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
                    {
                        var investment = await _dataContext.CostSystemToPractice.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investment != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investment), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investment).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        var investmenttimeperiod = await _dataContext.CostSystemToTime.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investmenttimeperiod != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investmenttimeperiod), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investmenttimeperiod).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                    {
                        var investmentoutcome = await _dataContext.CostSystemToOutcome.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investmentoutcome != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investmentoutcome), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investmentoutcome).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investmentoutput.ToString())
                    {
                        var investmentoutput = await _dataContext.CostSystemToOutput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investmentoutput != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investmentoutput), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investmentoutput).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
                    {
                        var investmentcomponent = await _dataContext.CostSystemToComponent.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investmentcomponent != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investmentcomponent), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investmentcomponent).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Economics1.INVESTMENT_TYPES.investmentinput.ToString())
                    {
                        var investmentinput = await _dataContext.CostSystemToInput.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (investmentinput != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(investmentinput), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(investmentinput).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateInvestmentsLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateInvestmentsLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            string sKeyName = string.Empty;
            bool bHasSet = true;
            if (inputNodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToCostSystem.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToCostSystemToPractice.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToCostSystemToTime.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIInvestments(_dtoContentURI, bSaveInFileSystemContent);
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
            else if (nodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                var currentObject = await GetCostSystem(id);
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
                        if (_dtoContentURI.URINodeName != Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
                        {
                            XElement el = MakeCostSystemXml(currentObject);
                            bHasGoodAncestors = Economics1.AddInvestmentElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, currentObject);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
            {
                var currentObject = await GetInvestment(id);
                if (currentObject != null)
                {
                    id = currentObject.CostSystemId;
                    nodeName = Economics1.INVESTMENT_TYPES.investmentgroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.INVESTMENT_TYPES.investment.ToString())
                        {
                            XElement el = MakeInvestmentXml(currentObject);
                            bHasGoodAncestors = Economics1.AddInvestmentElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                var currentObject = await GetInvestmentTime(id);
                if (currentObject != null)
                {
                    id = currentObject.CostSystemToPracticeId;
                    nodeName = Economics1.INVESTMENT_TYPES.investment.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                        {
                            XElement el = MakeCostSystemToTimeXml(currentObject);
                            bHasGoodAncestors = Economics1.AddInvestmentElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName, null);
                            //add the grouping nodes
                            XElement groupOutcome = XElement.Parse(Economics1.INVESTMENT_OUTCOMES_NODE);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, groupOutcome,
                                string.Empty, currentObject.PKId.ToString(), Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString());
                            //add a grouping node
                            XElement groupComponent = XElement.Parse(Economics1.INVESTMENT_COMPONENTS_NODE);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, groupComponent,
                                string.Empty, currentObject.PKId.ToString(), Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString());
                        }
                    }

                }
            }
            else if (nodeName == Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
            {
                var currentObject = await GetInvestmentOutcome(id);
                if (currentObject != null)
                {
                    id = currentObject.CostSystemToTimeId;
                    nodeName = Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                        {
                            XElement el = MakeCostSystemToOutcomeXml(currentObject);
                            bHasGoodAncestors = Economics1.AddInvestmentElementToParent(root, el,
                                Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString(),
                                id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.INVESTMENT_TYPES.investmentoutput.ToString())
            {
                var currentObject = await GetInvestmentOutput(id);
                if (currentObject != null)
                {
                    id = currentObject.CostSystemToOutcomeId;
                    nodeName = Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString();
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
            else if (nodeName == Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
            {
                var currentObject = await GetInvestmentComponent(id);
                if (currentObject != null)
                {
                    id = currentObject.CostSystemToTimeId;
                    nodeName = Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
                        {
                            XElement el = MakeCostSystemToComponentXml(currentObject);
                            bHasGoodAncestors = Economics1.AddInvestmentElementToParent(root, el,
                                Economics1.INVESTMENT_TYPES.investmentcomponents.ToString(), 
                                id.ToString(), nodeName, null);
                        }
                    }

                }
            }
            else if (nodeName == Economics1.INVESTMENT_TYPES.investmentinput.ToString())
            {
                var currentObject = await GetInvestmentInput(id);
                if (currentObject != null)
                {
                    id = currentObject.CostSystemToComponentId;
                    nodeName = Economics1.INVESTMENT_TYPES.investmentcomponent.ToString();
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
                    bool bHasSet = await SetTempURIInvestments(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.CostSystem != null)
                    {
                        if (tempURI.URIModels.Service.CostSystem.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.CostSystem)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Economics1.INVESTMENT_TYPES.investmentgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                var obj2 = await GetCostSystem(childId);
                if (obj2 != null)
                {
                    XElement el = MakeCostSystemXml(obj2);
                    bHasBeenAdded = Economics1.AddInvestmentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, obj2);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURIInvestments(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.CostSystem.CostSystemToPractice != null)
                            {
                                if (tempURI.URIModels.CostSystem.CostSystemToPractice.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.CostSystem.CostSystemToPractice)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Economics1.INVESTMENT_TYPES.investment.ToString(),
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
            else if (childNodeName == Economics1.INVESTMENT_TYPES.investment.ToString())
            {
                var obj3 = await GetInvestment(childId);
                if (obj3 != null)
                {
                    XElement el = MakeInvestmentXml(obj3);
                    bHasBeenAdded = Economics1.AddInvestmentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIInvestments(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.CostSystemToPractice.CostSystemToTime != null)
                        {
                            if (tempURI.URIModels.CostSystemToPractice.CostSystemToTime.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.CostSystemToPractice.CostSystemToTime)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString(),
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
            else if (childNodeName == Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                var obj4 = await GetInvestmentTime(childId);
                if (obj4 != null)
                {
                    XElement el = MakeCostSystemToTimeXml(obj4);
                    bHasBeenAdded = Economics1.AddInvestmentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIInvestments(tempURI, childNodeName, childId);
                        //add the grouping nodes
                        XElement groupOutcome = XElement.Parse(Economics1.INVESTMENT_OUTCOMES_NODE);
                        bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, groupOutcome,
                            string.Empty, childId.ToString(), childNodeName);
                        //add a grouping node
                        XElement groupComponent = XElement.Parse(Economics1.INVESTMENT_COMPONENTS_NODE);
                        bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, groupComponent,
                            string.Empty, childId.ToString(), childNodeName);
                        if (bHasBeenAdded)
                        {
                            //first do the outcomes
                            if (tempURI.URIModels.CostSystemToTime.CostSystemToOutcome != null)
                            {
                                if (tempURI.URIModels.CostSystemToTime.CostSystemToOutcome.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.CostSystemToTime.CostSystemToOutcome)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Economics1.INVESTMENT_TYPES.investmentoutcome.ToString(),
                                        root);
                                }
                            }
                            else
                            {
                                bHasGoodDescendants = true;
                            }
                            //then the components
                            if (tempURI.URIModels.CostSystemToTime.CostSystemToComponent != null)
                            {
                                if (tempURI.URIModels.CostSystemToTime.CostSystemToComponent.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.CostSystemToTime.CostSystemToComponent)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Economics1.INVESTMENT_TYPES.investmentcomponent.ToString(),
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
            else if (childNodeName == Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
            {
                var obj4 = await GetInvestmentOutcome(childId);
                if (obj4 != null)
                {
                    XElement el = MakeCostSystemToOutcomeXml(obj4);
                    bHasBeenAdded = Economics1.AddInvestmentElementToParent(root, el,
                            Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString(),
                            parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIInvestments(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.CostSystemToOutcome.CostSystemToOutput != null)
                        {
                            if (tempURI.URIModels.CostSystemToOutcome.CostSystemToOutput.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.CostSystemToOutcome.CostSystemToOutput)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Economics1.INVESTMENT_TYPES.investmentoutput.ToString(),
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
            else if (childNodeName == Economics1.INVESTMENT_TYPES.investmentoutput.ToString())
            {
                var obj4 = await GetInvestmentOutput(childId);
                if (obj4 != null)
                {
                    XElement el = MakeCostSystemToOutputXml(obj4);
                    bHasBeenAdded = Economics1.AddInvestmentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
            {
                var obj4 = await GetInvestmentComponent(childId);
                if (obj4 != null)
                {
                    XElement el = MakeCostSystemToComponentXml(obj4);
                    bHasBeenAdded = Economics1.AddInvestmentElementToParent(root, el,
                            Economics1.INVESTMENT_TYPES.investmentcomponents.ToString(), 
                            parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIInvestments(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.CostSystemToComponent.CostSystemToInput != null)
                        {
                            if (tempURI.URIModels.CostSystemToComponent.CostSystemToInput.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.CostSystemToComponent.CostSystemToInput)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Economics1.INVESTMENT_TYPES.investmentinput.ToString(),
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
            else if (childNodeName == Economics1.INVESTMENT_TYPES.investmentinput.ToString())
            {
                var obj4 = await GetInvestmentInput(childId);
                if (obj4 != null)
                {
                    XElement el = MakeCostSystemToInputXml(obj4);
                    bHasBeenAdded = Economics1.AddInvestmentElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName, null);
                    if (bHasBeenAdded)
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            return bHasGoodDescendants;
        }
        private async Task<bool> SetTempURIInvestments(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIInvestments(tempURI, true);
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
        
        private async Task<CostSystem> GetCostSystem(int id)
        {
            CostSystem bs = await _dataContext
                    .CostSystem
                    .Include(t => t.CostSystemType)
                    .Include(t => t.LinkedViewToCostSystem)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeCostSystemXml(CostSystem obj)
        {
            XElement currentNode = new XElement(Economics1.INVESTMENT_TYPES.investmentgroup.ToString());
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
            if (obj.LinkedViewToCostSystem != null)
            {
                foreach (var lv in obj.LinkedViewToCostSystem)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<CostSystemToPractice> GetInvestment(int id)
        {
            CostSystemToPractice bs = await _dataContext
                    .CostSystemToPractice
                    .Include(t => t.LinkedViewToCostSystemToPractice)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeInvestmentXml(CostSystemToPractice obj)
        {
            XElement currentNode = new XElement(Economics1.INVESTMENT_TYPES.investment.ToString());
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
            if (obj.LinkedViewToCostSystemToPractice != null)
            {
                foreach (var lv in obj.LinkedViewToCostSystemToPractice)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<CostSystemToTime> GetInvestmentTime(int id)
        {
            CostSystemToTime bs = await _dataContext
                    .CostSystemToTime
                    .Include(t => t.LinkedViewToCostSystemToTime)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeCostSystemToTimeXml(CostSystemToTime obj)
        {
            XElement currentNode = new XElement(Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString());
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
            if (obj.LinkedViewToCostSystemToTime != null)
            {
                foreach (var lv in obj.LinkedViewToCostSystemToTime)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
        private async Task<CostSystemToOutcome> GetInvestmentOutcome(int id)
        {
            CostSystemToOutcome bs = await _dataContext
                    .CostSystemToOutcome
                    .Include(t => t.Outcome)
                    .Include(t => t.Outcome.OutcomeClass)
                    .Include(t => t.Outcome.OutcomeClass.OutcomeType)
                    .Include(t => t.Outcome.LinkedViewToOutcome)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeCostSystemToOutcomeXml(CostSystemToOutcome obj)
        {
            XElement currentNode = new XElement(Economics1.INVESTMENT_TYPES.investmentoutcome.ToString());
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
        private async Task<CostSystemToOutput> GetInvestmentOutput(int id)
        {
            CostSystemToOutput bs = await _dataContext
                    .CostSystemToOutput
                    .Include(t => t.OutputSeries)
                    .Include(t => t.OutputSeries.Output.OutputClass)
                    .Include(t => t.OutputSeries.Output.OutputClass.OutputType)
                    .Include(t => t.OutputSeries.LinkedViewToOutputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeCostSystemToOutputXml(CostSystemToOutput obj)
        {
            XElement currentNode = new XElement(Economics1.INVESTMENT_TYPES.investmentoutput.ToString());
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
        private async Task<CostSystemToComponent> GetInvestmentComponent(int id)
        {
            CostSystemToComponent bs = await _dataContext
                    .CostSystemToComponent
                    .Include(t => t.Component)
                    .Include(t => t.Component.ComponentClass)
                    .Include(t => t.Component.ComponentClass.ComponentType)
                    .Include(t => t.Component.LinkedViewToComponent)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeCostSystemToComponentXml(CostSystemToComponent obj)
        {
            XElement currentNode = new XElement(Economics1.INVESTMENT_TYPES.investmentcomponent.ToString());
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
            if (obj.Component != null)
            {
                Economics1.AddBaseComponentToXml(currentNode, obj);
                if (obj.Component.LinkedViewToComponent != null)
                {
                    foreach (var lv in obj.Component.LinkedViewToComponent)
                    {
                        EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                    }
                }
            }
            return currentNode;
        }
        private async Task<CostSystemToInput> GetInvestmentInput(int id)
        {
            CostSystemToInput bs = await _dataContext
                    .CostSystemToInput
                    .Include(t => t.InputSeries)
                    .Include(t => t.InputSeries.Input.InputClass)
                    .Include(t => t.InputSeries.Input.InputClass.InputType)
                    .Include(t => t.InputSeries.LinkedViewToInputSeries)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeCostSystemToInputXml(CostSystemToInput obj)
        {
            XElement currentNode = new XElement(Economics1.INVESTMENT_TYPES.investmentinput.ToString());
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
