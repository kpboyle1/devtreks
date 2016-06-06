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
    ///Purpose:		Entity Framework DevPack support class
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/devpacks/help/devpack/HelpFile/148
    /// </summary>
    public class DevPackModelHelper
    {
        public DevPackModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            //has to reference dtouri, not a new one
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIDevPack(ContentURI uri,
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
                        .DevPackClass
                        .Where(a => a.Service.PKId == uri.URIId)
                        .OrderBy(m => m.DevPackClassName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.DevPackClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.DevPackClass != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .DevPackClass
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
                    bHasSet = await SetURIDevPackType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
            {
                var rc = await _dataContext.DevPackClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .DevPackClassToDevPack
                        .Where(a => a.DevPackClassId == uri.URIId)
                        .Include(t => t.DevPack)
                        .Include(t => t.LinkedViewToDevPackJoin)
                        .Where(rp => rp.ParentId == 0 || rp.ParentId == null)
                        .OrderBy(m => m.DevPackClassAndPackName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.DevPackClassToDevPack = await qry.ToAsyncEnumerable().ToList();
                        if (rc.DevPackClassToDevPack != null)
                        {
                            uri.URIDataManager.RowCount =
                              _dataContext
                                .DevPackClassToDevPack
                                .Where(a => a.DevPackClassId == uri.URIId)
                                .Include(t => t.DevPack)
                                .Include(t => t.LinkedViewToDevPackJoin)
                                .Where(rp => rp.ParentId == 0 || rp.ParentId == null)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.DevPackClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                var rp = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    uri.URIModels.DevPackClassToDevPack = rp;
                    //need recursive descendants
                    var qry = _dataContext
                        .DevPackClassToDevPack
                        .Include(t => t.DevPackClassToDevPack1)
                        .Where(p => p.ParentId == uri.URIId)
                        .OrderBy(m => m.DevPackClassAndPackName)
                        .Include(t => t.DevPack)
                        .Include(t => t.LinkedViewToDevPackJoin)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.DevPackClassToDevPack1 = await qry.ToAsyncEnumerable().ToList();
                        if (rp.DevPackClassToDevPack1 != null)
                        {
                            uri.URIDataManager.RowCount =
                             _dataContext
                               .DevPackClassToDevPack
                                .Include(t => t.DevPackClassToDevPack1)
                                .Where(p => p.ParentId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    // need the same page size records as devpack
                    // this has to paginate the same as the devpack collection
                    var qry2 = _dataContext
                        .DevPackToDevPackPart                        
                        .Where(x => x.DevPackClassToDevPackId == uri.URIId)
                        .Include(t => t.DevPackPart)
                        .Include(t => t.LinkedViewToDevPackPartJoin)
                        .OrderBy(r => r.DevPackToDevPackPartName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    //set the data transfer objects
                    if (qry2 != null)
                    {
                        rp.DevPackToDevPackPart = await qry2.ToAsyncEnumerable().ToList();
                        if (rp.DevPackToDevPackPart != null)
                        {
                            //note the plus equal
                            uri.URIDataManager.RowCount +=
                             _dataContext
                               .DevPackToDevPackPart
                               .Where(x => x.DevPackClassToDevPackId == uri.URIId)
                               .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                   
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                var r = await _dataContext.DevPackToDevPackPart.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (r != null)
                {
                    // count the devpack packs without loading them
                    //int total = _dataContext.Entry(r).Entity.DevPackPartToResourcePack.Count;
                    //uri.URIDataManager.RowCount = total;
                    var qry = _dataContext
                        .DevPackPartToResourcePack
                        .Include(t => t.ResourcePack)
                        .Where(rp => rp.DevPackToDevPackPartId == uri.URIId)
                        .OrderBy(m => m.SortLabel)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        r.DevPackPartToResourcePack = await qry.ToAsyncEnumerable().ToList();
                        if (r.DevPackPartToResourcePack != null)
                        {
                            uri.URIDataManager.RowCount =
                            _dataContext
                              .DevPackPartToResourcePack
                              .Where(rp => rp.DevPackToDevPackPartId == uri.URIId)
                              .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.DevPackToDevPackPart = r;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString())
            {
                var qry = _dataContext
                    .DevPackPartToResourcePack
                    .Include(t => t.ResourcePack)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.DevPackPartToResourcePack = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIDevPackType(Service s, ContentURI uri)
        {
            bool bHasSet = true;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.DevPackType = new List<DevPackType>();
                //filter the devpack types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .DevPackType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.DevPackType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.DevPackType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    DevPackType rt = await _dataContext.DevPackType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.DevPackType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void SetURIDevPack(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
            {
                var qryRC = _dataContext
                    .DevPackClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                var qryRP
                        = _dataContext
                        .DevPackClassToDevPack
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                var qryR = _dataContext
                    .DevPack
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddDevPack(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = true;
            int iNotUsed = 0;
            //store updated devpacks ids in lists
            List<DevPack> addedDs = new List<DevPack>();
            List<DevPackPart> addedDPs = new List<DevPackPart>();
            bool bHasAdded = await AddDevPack(argumentsEdits.SelectionsToAdd, addedDs, addedDPs);
            //int iNewId = 0;
            if (bHasAdded)
            {
                try
                {
                    iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    if (addedDs.Count > 0)
                    {
                        bIsAdded = false;
                        foreach (var devpack in addedDs)
                        {
                            if (_dtoContentURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
                            {
                                var newDevPackClassToDevPack = new DevPackClassToDevPack
                                {
                                    DevPackClassAndPackSortLabel = devpack.DevPackNum,
                                    DevPackClassAndPackName = devpack.DevPackName,
                                    DevPackClassAndPackDesc = Helpers.GeneralHelpers.NONE,
                                    DevPackClassAndPackFileExtensionType = Helpers.GeneralHelpers.NONE,
                                    DevPackId = devpack.PKId,
                                    DevPack = null,
                                    DevPackClassId = _dtoContentURI.URIId,
                                    DevPackClass = null,
                                    ParentId = null,
                                    DevPackClassToDevPack2 = null,
                                    DevPackToDevPackPart = null,
                                    DevPackClassToDevPack1 = null,
                                    LinkedViewToDevPackJoin = null
                                };
                                _dataContext.DevPackClassToDevPack.Add(newDevPackClassToDevPack);
                                _dataContext.Entry(newDevPackClassToDevPack).State = EntityState.Added;
                                iNotUsed = await _dataContext.SaveChangesAsync();
                                bIsAdded = true;
                            }
                            else if (_dtoContentURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
                            {
                                var recursivedevpackclasstodevpack = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                                if (recursivedevpackclasstodevpack != null)
                                {
                                    var newDevPackClassToDevPack = new DevPackClassToDevPack
                                    {
                                        DevPackClassAndPackSortLabel = devpack.DevPackNum,
                                        DevPackClassAndPackName = devpack.DevPackName,
                                        DevPackClassAndPackDesc = Helpers.GeneralHelpers.NONE,
                                        DevPackClassAndPackFileExtensionType = Helpers.GeneralHelpers.NONE,
                                        DevPackId = devpack.PKId,
                                        DevPack = null,
                                        DevPackClassId = recursivedevpackclasstodevpack.DevPackClassId,
                                        DevPackClass = null,
                                        ParentId = recursivedevpackclasstodevpack.PKId,
                                        DevPackClassToDevPack2 = null,
                                        DevPackToDevPackPart = null,
                                        DevPackClassToDevPack1 = null,
                                        LinkedViewToDevPackJoin = null
                                    };
                                    _dataContext.DevPackClassToDevPack.Add(newDevPackClassToDevPack);
                                    _dataContext.Entry(newDevPackClassToDevPack).State = EntityState.Added;
                                    iNotUsed = await _dataContext.SaveChangesAsync();
                                    bIsAdded = true;
                                }
                            }
                        }
                    }
                    else if (addedDPs.Count > 0)
                    {
                        bIsAdded = false;
                        foreach (var devpackpart in addedDPs)
                        {
                            //have a good base table insertion, now can insert join
                            var newDevPackToDevPackPart = new DevPackToDevPackPart
                            {
                                DevPackToDevPackPartSortLabel = Helpers.GeneralHelpers.NONE,
                                DevPackToDevPackPartName = devpackpart.DevPackPartName,
                                DevPackToDevPackPartDesc = Helpers.GeneralHelpers.NONE,
                                DevPackToDevPackPartFileExtensionType = Helpers.GeneralHelpers.NONE,
                                DevPackClassToDevPackId = _dtoContentURI.URIId,
                                DevPackClassToDevPack = null,
                                DevPackPartId = devpackpart.PKId,
                                DevPackPart = null,
                                DevPackPartToResourcePack = null,
                                LinkedViewToDevPackPartJoin = null
                            };
                            _dataContext.DevPackToDevPackPart.Add(newDevPackToDevPackPart);
                            _dataContext.Entry(newDevPackToDevPackPart).State = EntityState.Added;
                            iNotUsed = await _dataContext.SaveChangesAsync();
                            bIsAdded = true;
                        }
                    }
                    //only the edit panel needs an updated collection of devpacks
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                         bHasSet = await SetURIDevPack(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                     bHasSet = await SetURIDevPack(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddDevPack(List<ContentURI> addedURIs, List<DevPack> addedDs, 
            List<DevPackPart> addedDPs)
        {
            string sParentNodeName = string.Empty;
            int iParentId = 0;
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
                if (addedURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
                {
                    var newDevPackClass = new DevPackClass
                    {
                        DevPackClassNum = Helpers.GeneralHelpers.NONE,
                        DevPackClassName = addedURI.URIName,
                        DevPackClassDesc = Helpers.GeneralHelpers.NONE,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        DevPackType = null,
                        DevPackClassToDevPack = null
                    };
                    _dataContext.DevPackClass.Add(newDevPackClass);
                    _dataContext.Entry(newDevPackClass).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
                {
                    if (_dtoContentURI.URIDataManager.ServerSubActionType
                        == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        //insert the base first
                        var newDevPack = new DevPack
                        {
                            DevPackNum = Helpers.GeneralHelpers.NONE,
                            DevPackName = addedURI.URIName,
                            DevPackDesc = Helpers.GeneralHelpers.NONE,
                            DevPackDocStatus= 0,
                            DevPackKeywords = Helpers.GeneralHelpers.NONE,
                            DevPackLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                            DevPackMetaDataXml = string.Empty,
                            DevPackClassToDevPack = null
                        };
                        _dataContext.DevPack.Add(newDevPack);
                        _dataContext.Entry(newDevPack).State = EntityState.Added;
                        addedDs.Add(newDevPack);
                        bIsAdded = true;
                    }
                    else
                    {
                        //insert the join
                        var devpackclasstodevpack = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                         if (devpackclasstodevpack != null)
                         {
                             if (_dtoContentURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
                             {
                                 //this is a recursive insert
                                 //_dtoContentURI is parent table
                                 var recursivedevpackclasstodevpack = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == _dtoContentURI.URIId);
                                 if (recursivedevpackclasstodevpack != null)
                                 {
                                     var newDevPackClassToDevPack = new DevPackClassToDevPack
                                     {
                                         DevPackClassAndPackSortLabel = devpackclasstodevpack.DevPackClassAndPackSortLabel,
                                         DevPackClassAndPackName = devpackclasstodevpack.DevPackClassAndPackName,
                                         DevPackClassAndPackDesc = devpackclasstodevpack.DevPackClassAndPackDesc,
                                         DevPackClassAndPackFileExtensionType = devpackclasstodevpack.DevPackClassAndPackFileExtensionType,
                                         DevPackId = devpackclasstodevpack.DevPackId,
                                         DevPack = null,
                                         DevPackClassId = recursivedevpackclasstodevpack.DevPackClassId,
                                         DevPackClass = null,
                                         ParentId = recursivedevpackclasstodevpack.PKId,
                                         DevPackClassToDevPack2 = null,
                                         DevPackToDevPackPart = null,
                                         DevPackClassToDevPack1 = null,
                                         LinkedViewToDevPackJoin = null
                                     };
                                     _dataContext.DevPackClassToDevPack.Add(newDevPackClassToDevPack);
                                     _dataContext.Entry(newDevPackClassToDevPack).State = EntityState.Added;
                                     bIsAdded = true;
                                 }
                             }
                             else
                             {
                                 //this is not a recursive insert
                                 //parentid is null
                                 var newDevPackClassToDevPack = new DevPackClassToDevPack
                                 {
                                     DevPackClassAndPackSortLabel = devpackclasstodevpack.DevPackClassAndPackSortLabel,
                                     DevPackClassAndPackName = devpackclasstodevpack.DevPackClassAndPackName,
                                     DevPackClassAndPackDesc = devpackclasstodevpack.DevPackClassAndPackDesc,
                                     DevPackClassAndPackFileExtensionType = devpackclasstodevpack.DevPackClassAndPackFileExtensionType,
                                     DevPackId = devpackclasstodevpack.DevPackId,
                                     DevPack = null,
                                     DevPackClassId = iParentId,
                                     DevPackClass = null,
                                     ParentId = null,
                                     DevPackClassToDevPack2 = null,
                                     DevPackToDevPackPart = null,
                                     DevPackClassToDevPack1 = null,
                                     LinkedViewToDevPackJoin = null
                                 };
                                 _dataContext.DevPackClassToDevPack.Add(newDevPackClassToDevPack);
                                 _dataContext.Entry(newDevPackClassToDevPack).State = EntityState.Added;
                                 bIsAdded = true;
                             }
                         }
                    }
                }
                else if (addedURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                {
                    if (_dtoContentURI.URIDataManager.ServerSubActionType
                        == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        //insert the base first
                        var newDevPackPart = new DevPackPart
                        {
                            DevPackPartNum = Helpers.GeneralHelpers.NONE,
                            DevPackPartName = addedURI.URIName,
                            DevPackPartDesc = Helpers.GeneralHelpers.NONE,
                            DevPackPartKeywords = Helpers.GeneralHelpers.NONE,
                            DevPackPartLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                            DevPackPartFileName = Helpers.GeneralHelpers.NONE,
                            DevPackPartXmlDoc = string.Empty,
                            DevPackPartVirtualURIPattern = Helpers.GeneralHelpers.NONE,
                            DevPackToDevPackPart = null
                        };
                        _dataContext.DevPackPart.Add(newDevPackPart);
                        _dataContext.Entry(newDevPackPart).State = EntityState.Added;
                        addedDPs.Add(newDevPackPart);
                        bIsAdded = true;
                    }
                    else
                    {
                        //insert the join
                        var devpacktodevpackpart = await _dataContext.DevPackToDevPackPart.SingleOrDefaultAsync(x => x.PKId == addedURI.URIId);
                        if (devpacktodevpackpart != null)
                        {
                            var newDevPackToDevPackPart = new DevPackToDevPackPart
                            {
                                DevPackToDevPackPartSortLabel = devpacktodevpackpart.DevPackToDevPackPartSortLabel,
                                DevPackToDevPackPartName = devpacktodevpackpart.DevPackToDevPackPartName,
                                DevPackToDevPackPartDesc = devpacktodevpackpart.DevPackToDevPackPartDesc,
                                DevPackToDevPackPartFileExtensionType = string.Empty,
                                DevPackClassToDevPackId = iParentId,
                                DevPackClassToDevPack = null,
                                DevPackPartId = devpacktodevpackpart.DevPackPartId,
                                DevPackPart = null,
                                DevPackPartToResourcePack = null,
                                LinkedViewToDevPackPartJoin = null
                            };
                            _dataContext.DevPackToDevPackPart.Add(newDevPackToDevPackPart);
                            _dataContext.Entry(newDevPackToDevPackPart).State = EntityState.Added;
                            bIsAdded = true;
                        }
                    }
                }
                else if (addedURI.URINodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
                {
                    var newDevPackResourcePack = new DevPackPartToResourcePack
                    {
                        SortLabel = Helpers.GeneralHelpers.NONE,
                        DevPackToDevPackPartId = iParentId,
                        DevPackToDevPackPart = null,
                        ResourcePackId = addedURI.URIId,
                        ResourcePack = null
                    };
                    _dataContext.DevPackPartToResourcePack.Add(newDevPackResourcePack);
                    _dataContext.Entry(newDevPackResourcePack).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddDevPackLinkedView(addedURI, _dtoContentURI.URINodeName);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddDevPackLinkedView(ContentURI addedURI,
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
                    if (inputNodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
                    {
                        var newLinkedView = new LinkedViewToDevPackJoin
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            DevPackClassToDevPack = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToDevPackJoin.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                    else if (inputNodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                    {
                        var newLinkedView = new LinkedViewToDevPackPartJoin
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            DevPackToDevPackPart = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToDevPackPartJoin.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
                {
                    var newLinkedView = new LinkedViewToDevPackJoin
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        DevPackClassToDevPack = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToDevPackJoin.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (inputNodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                {
                    var newLinkedView = new LinkedViewToDevPackPartJoin
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        DevPackToDevPackPart = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToDevPackPartJoin.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteDevPack(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            bool bHasSet = true;
            //linked views are deleted using normal pattern but devpacks use stored procedure deletion
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bIsDeleted = await DeleteDevPack(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    bHasSet = await SetURIDevPack(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIDevPack(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteDevPack(List<ContentURI> deletionURIs, Dictionary<string, int> deletedIds)
        {
            //put the deletebase method in here for base nodes
            bool bIsDeleted = false;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName != LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    deletionURI.URIDataManager.AppType = Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks;
                    //use sqlio to cascade delete base to join or just join (stored procedure is better)
                    bIsDeleted = await DeleteBase(deletionURI);
                }
                else
                {
                    if (_dtoContentURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToDevPackJoin.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
                    else if (_dtoContentURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToDevPackPartJoin.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
            return bIsDeleted;
        }
        private async Task<bool> DeleteBase(ContentURI deletionURI)
        {
            bool bIsOkToSave = false;
            if (_dtoContentURI.URIDataManager.ServerSubActionType
                != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
            {
                EditHelper editHelper = new EditHelper();
                bool bIsDbEdit = true;
                bIsOkToSave = await editHelper.DeleteJoinAndCheckBaseAsync(_dtoContentURI, deletionURI, bIsDbEdit);
            }
            return bIsOkToSave;
        }
        public async Task<bool> UpdateDevPack(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = true;
            //store updated devpacks ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateDevPack(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIDevPack(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIDevPack(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateDevPack(List<EditHelper.ArgumentsEdits> edits,
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
                    bHasSet = await UpdateDevPackLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == DevPacks.DEVPACKS_TYPES.devpacktype.ToString())
                    {
                        var devpackType = await _dataContext.DevPackType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (devpackType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(devpackType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(devpackType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
                    {
                        var devpackClass = await _dataContext.DevPackClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (devpackClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(devpackClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(devpackClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
                    {
                        var devpackClassToDevPack = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (devpackClassToDevPack != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(devpackClassToDevPack), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(devpackClassToDevPack).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == DevPacks.DEVPACKS_BASE_TYPES.devpackbase.ToString())
                    {
                        var devpack = await _dataContext.DevPack.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (devpack != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(devpack), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(devpack).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                    {
                        var devpackToDevPackPart = await _dataContext.DevPackToDevPackPart.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (devpackToDevPackPart != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(devpackToDevPackPart), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(devpackToDevPackPart).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == DevPacks.DEVPACKS_BASE_TYPES.devpackpartbase.ToString())
                    {
                        var devpackpart = await _dataContext.DevPackPart.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (devpackpart != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(devpackpart), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(devpackpart).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString())
                    {
                        var devpackResourcePack = await _dataContext.DevPackPartToResourcePack.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (devpackResourcePack != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(devpackResourcePack), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(devpackResourcePack).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateDevPackLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateDevPackLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            string sKeyName = string.Empty;
            bool bHasSet = true;
            if (inputNodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToDevPackJoin.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            else if (inputNodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToDevPackPartJoin.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIDevPack(_dtoContentURI, bSaveInFileSystemContent);
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
            if (nodeName == DevPacks.DEVPACKS_TYPES.servicebase.ToString())
            {
                var currentObject = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    _parentName = nodeName;
                    _parentId = id;
                    if (_dtoContentURI.URINodeName != DevPacks.DEVPACKS_TYPES.servicebase.ToString())
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
            else if (nodeName == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
            {
                var currentObject = await _dataContext.DevPackClass.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    id = currentObject.ServiceId;
                    nodeName = DevPacks.DEVPACKS_TYPES.servicebase.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
                        {
                            XElement el = MakeDevPackClassXml(currentObject);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName);
                        }
                    }

                }
            }
            else if (nodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                var currentObject = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    //first handle recursive ancestors
                    if (currentObject.ParentId != null)
                    {
                        var currentParentObject = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == currentObject.ParentId);
                        if (currentParentObject != null)
                        {
                            int iParentId = currentParentObject.PKId;
                            if (iParentId != 0)
                            {
                                id = iParentId;
                                nodeName = DevPacks.DEVPACKS_TYPES.devpack.ToString();
                                bHasGoodAncestors = await AddAncestors(id,
                                    nodeName, root);
                                if (bHasGoodAncestors)
                                {
                                    _parentName = nodeName;
                                    _parentId = id;
                                    if (currentParentObject.ParentId != null)
                                    {
                                        if (currentParentObject.ParentId != 0)
                                        {
                                            int iParent2Id = (int) currentParentObject.ParentId;
                                            bHasGoodAncestors = AddDevPack(root, currentParentObject,
                                                nodeName, iParent2Id);
                                        }
                                    }
                                    else
                                    {
                                        AddDevPack(root, currentParentObject, 
                                            DevPacks.DEVPACKS_TYPES.devpackgroup.ToString(), currentParentObject.DevPackClassId);
                                    }
                                }
                                //add the self after recursive ancestors added
                                bHasGoodAncestors = AddDevPack(root, currentObject,
                                    nodeName, id);
                            }
                        }
                    }
                    else
                    {
                        //then regular ancestors
                        id = currentObject.DevPackClassId;
                        nodeName = DevPacks.DEVPACKS_TYPES.devpackgroup.ToString();
                        bHasGoodAncestors = await AddAncestors(id,
                            nodeName, root);
                        if (bHasGoodAncestors)
                        {
                            _parentName = nodeName;
                            _parentId = id;
                            if (_dtoContentURI.URINodeName != DevPacks.DEVPACKS_TYPES.devpack.ToString())
                            {
                                XElement el = MakeDevPackClassToDevPackXml(currentObject);
                                bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                    string.Empty, id.ToString(), nodeName);
                            }
                        }
                    }
                }
            }
            else if (nodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                var currentObject = await _dataContext.DevPackToDevPackPart.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    id = currentObject.DevPackClassToDevPackId;
                    nodeName = DevPacks.DEVPACKS_TYPES.devpack.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                        {
                            XElement el = MakeDevPackToDevPackPartXml(currentObject);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName);
                        }
                    }

                }
            }
            else if (nodeName == DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString())
            {
                var currentObject = await GetDevPackPartToResourcePack(id);
                if (currentObject != null)
                {
                    id = currentObject.DevPackToDevPackPartId;
                    nodeName = DevPacks.DEVPACKS_TYPES.devpackpart.ToString();
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
        private bool CanAddDevPack(int id)
        {
            bool bCanAdd = true;
            if (_dtoContentURI.URINodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                if (_dtoContentURI.URIId == id)
                {
                    bCanAdd = false;
                }
            }
            return bCanAdd;
        }
        private bool AddDevPack(XElement root, DevPackClassToDevPack devpackClassToDevPack,
            string parentNodeName, int parentId)
        {
            bool bHasGoodAncestor = false;
            bool bCanAdd = CanAddDevPack(devpackClassToDevPack.PKId);
            if (bCanAdd)
            {
                bool bHasDescendant = EditHelpers.XmlLinq.DescendantExists(root, DevPacks.DEVPACKS_TYPES.devpack.ToString(),
                    devpackClassToDevPack.PKId.ToString());
                if (!bHasDescendant)
                {
                    XElement el = MakeDevPackClassToDevPackXml(devpackClassToDevPack);
                    bHasGoodAncestor = EditHelpers.XmlLinq.AddElementToParent(root, el,
                        string.Empty, parentId.ToString(), parentNodeName);
                }
                else
                {
                    bHasGoodAncestor = true;
                }
            }
            else
            {
                bHasGoodAncestor = true;
            }
            return bHasGoodAncestor;
        }
        private async Task<bool> AddDescendants(ContentURI tempURI, int parentId, string parentNodeName,
            int childId, string childNodeName, XElement root)
        {
            bool bHasGoodDescendants = false;
            bool bHasBeenAdded = false;
            bool bHasSet = true;
            //deserialize objects
            if (childNodeName == DevPacks.DEVPACKS_TYPES.servicebase.ToString())
            {
                var obj1 = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj1 != null)
                {
                    XElement el = MakeServiceBaseXml(obj1);
                    if (el != null)
                    {
                        root.AddFirst(el);
                    }
                    bHasSet = await SetTempURIDevPack(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.DevPackClass != null)
                    {
                        if (tempURI.URIModels.Service.DevPackClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.DevPackClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, DevPacks.DEVPACKS_TYPES.devpackgroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
            {
                var obj2 = await _dataContext.DevPackClass.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj2 != null)
                {
                    XElement el = MakeDevPackClassXml(obj2);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != DevPacks.DEVPACKS_TYPES.servicebase.ToString())
                        {
                            bHasSet = await SetTempURIDevPack(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.DevPackClass.DevPackClassToDevPack != null)
                            {
                                if (tempURI.URIModels.DevPackClass.DevPackClassToDevPack.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.DevPackClass.DevPackClassToDevPack)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, DevPacks.DEVPACKS_TYPES.devpack.ToString(),
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
            else if (childNodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                var obj3 = await _dataContext.DevPackClassToDevPack.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj3 != null)
                {
                    XElement el = MakeDevPackClassToDevPackXml(obj3);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        bHasSet = await SetTempURIDevPack(tempURI, childNodeName, childId);
                        //check for recursive descendants
                        if (tempURI.URIModels.DevPackClassToDevPack.DevPackClassToDevPack1 != null)
                        {
                            if (tempURI.URIModels.DevPackClassToDevPack.DevPackClassToDevPack1.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.DevPackClassToDevPack.DevPackClassToDevPack1)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, DevPacks.DEVPACKS_TYPES.devpack.ToString(),
                                    root);
                            }
                        }
                        //then parts
                        if (tempURI.URIModels.DevPackClassToDevPack.DevPackToDevPackPart != null)
                        {
                            if (tempURI.URIModels.DevPackClassToDevPack.DevPackToDevPackPart.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.DevPackClassToDevPack.DevPackToDevPackPart)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, DevPacks.DEVPACKS_TYPES.devpackpart.ToString(),
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
            else if (childNodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                var obj4 = await _dataContext.DevPackToDevPackPart.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj4 != null)
                {
                    XElement el = MakeDevPackToDevPackPartXml(obj4);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        bHasSet = await SetTempURIDevPack(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.DevPackToDevPackPart.DevPackPartToResourcePack != null)
                        {
                            if (tempURI.URIModels.DevPackToDevPackPart.DevPackPartToResourcePack.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.DevPackToDevPackPart.DevPackPartToResourcePack)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString(),
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
            else if (childNodeName == DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString())
            {
                var obj4 = await GetDevPackPartToResourcePack(childId);
                if (obj4 != null)
                {
                    XElement el = MakeDevPackPartToResourcePackXml(obj4);
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
        private async Task<bool> SetTempURIDevPack(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIDevPack(tempURI, true);
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
        private XElement MakeDevPackClassXml(DevPackClass obj)
        {
            XElement currentNode = new XElement(DevPacks.DEVPACKS_TYPES.devpackgroup.ToString());
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
            return currentNode;
        }
        private XElement MakeDevPackClassToDevPackXml(DevPackClassToDevPack obj)
        {
            XElement currentNode = new XElement(DevPacks.DEVPACKS_TYPES.devpack.ToString());
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
            if (obj.DevPack != null)
            {
                DevPacks.AddBaseDevPackToXml(currentNode, obj.DevPack);
                //must use base input series linked view child elements
                if (obj.LinkedViewToDevPackJoin != null)
                {
                    foreach (var lv in obj.LinkedViewToDevPackJoin)
                    {
                        EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                    }
                }
            }
            return currentNode;
        }
        private XElement MakeDevPackToDevPackPartXml(DevPackToDevPackPart obj)
        {
            XElement currentNode = new XElement(DevPacks.DEVPACKS_TYPES.devpackpart.ToString());
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
            if (obj.DevPackPart != null)
            {
                DevPacks.AddBaseDevPackToXml(currentNode, obj.DevPackPart);
                //must use base input series linked view child elements
                if (obj.LinkedViewToDevPackPartJoin != null)
                {
                    foreach (var lv in obj.LinkedViewToDevPackPartJoin)
                    {
                        EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                    }
                }
            }
            return currentNode;
        }
        private async Task<DevPackPartToResourcePack> GetDevPackPartToResourcePack(int id)
        {
            DevPackPartToResourcePack bs = await _dataContext
                    .DevPackPartToResourcePack
                    .Include(t => t.ResourcePack)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeDevPackPartToResourcePackXml(DevPackPartToResourcePack obj)
        {
            XElement currentNode = new XElement(DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString());
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
                DevPacks.AddBaseDevPackToXml(currentNode, obj.ResourcePack);
            }
            return currentNode;
        }
    }
}
