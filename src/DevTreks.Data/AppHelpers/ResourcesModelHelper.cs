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
    ///Purpose:		Entity Framework Resource support class
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class ResourceModelHelper
    {
        public ResourceModelHelper(DevTreksContext dataContext, DevTreks.Data.ContentURI dtoURI)
        {
            this._dataContext = dataContext;
            //has to reference dtouri, not a new one
            _dtoContentURI = dtoURI;
        }
        private DevTreksContext _dataContext { get; set; }
        private DevTreks.Data.ContentURI _dtoContentURI { get; set; }
        private string _parentName { get; set; }
        private int _parentId { get; set; }
        public async Task<bool> SetURIResource(ContentURI uri,
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
                        .ResourceClass
                        .Where(a => a.ServiceId == uri.URIId)
                        .OrderBy(m => m.ResourceClassName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        s.ResourceClass = await qry.ToAsyncEnumerable().ToList();
                        if (s.ResourceClass != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                                .ResourceClass
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
                    //need the resourcetype collection set too
                    bHasSet = await SetURIResourceType(s, uri);
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName == Resources.RESOURCES_TYPES.resourcegroup.ToString())
            {
                var rc = await _dataContext.ResourceClass.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rc != null)
                {
                    var qry = _dataContext
                        .ResourcePack
                        .Where(a => a.ResourceClassId == uri.URIId)
                        .OrderBy(m => m.ResourcePackName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rc.ResourcePack = await qry.ToAsyncEnumerable().ToList();
                        if (rc.ResourcePack != null)
                        {
                            uri.URIDataManager.RowCount =
                               _dataContext
                                .ResourcePack
                                .Where(a => a.ResourceClassId == uri.URIId)
                                .Count();
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.ResourceClass = rc;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Resources.RESOURCES_TYPES.resourcepack.ToString())
            {
                var rp = await _dataContext.ResourcePack.SingleOrDefaultAsync(x => x.PKId == uri.URIId);
                if (rp != null)
                {
                    var qry = _dataContext
                        .Resource
                        .Where(a => a.ResourcePackId == uri.URIId)
                        .Include(t => t.LinkedViewToResource)
                        .OrderBy(m => m.ResourceName)
                        .Skip(iStartRow)
                        .Take(iPageSize);
                    if (qry != null)
                    {
                        rp.Resource = await qry.ToAsyncEnumerable().ToList();
                        uri.URIDataManager.RowCount = 
                           _dataContext
                            .Resource
                            .Where(a => a.ResourcePackId == uri.URIId)
                            .Count();
                        foreach(var res in rp.Resource)
                        {
                            //make urls here or they'll need to be retrieved
                            //from db separately in Resource.cshtml
                            bool bIsFileSystemPath = false;
                            res.ResourcePath = Resources.GetResourceFilePath(uri, bIsFileSystemPath,
                                uri.URIDataManager.SubAppType.ToString(), uri.URINetworkPartName, 
                                rp.PKId.ToString(),
                                res.PKId.ToString(), res.ResourceFileName);

                        }
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MODELHELPERS_CANTMAKEMODEL1");
                    }
                    uri.URIModels.ResourcePack = rp;
                    
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MODELHELPERS_CANTMAKEMODEL");
                }
            }
            else if (uri.URINodeName
                == Resources.RESOURCES_TYPES.resource.ToString())
            {
                var qry = _dataContext
                    .Resource
                    .Include(t => t.ResourcePack)
                    .Where(r => r.PKId == uri.URIId);

                if (qry != null)
                {
                    uri.URIModels.Resource = await qry.FirstOrDefaultAsync();
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
        private async Task<bool> SetURIResourceType(Service s, ContentURI uri)
        {
            bool bHasSet = false;
            if (uri.URINodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIModels.ResourceType = new List<ResourceType>();
                //filter the resource types by this service's network
                if (s != null)
                {
                    var qry = _dataContext
                        .ResourceType
                        .Where(rt => rt.NetworkId == s.NetworkId)
                        .OrderBy(rt => rt.Name);
                    if (qry != null)
                    {
                        uri.URIModels.ResourceType = await qry.ToListAsync();
                    }
                }
                if (uri.URIModels.ResourceType.Count == 0)
                {
                    //first record is an allcategories and ok for default
                    ResourceType rt = await _dataContext.ResourceType.SingleOrDefaultAsync(x => x.PKId == 0);
                    if (rt != null)
                    {
                        uri.URIModels.ResourceType.Add(rt);
                    }
                }
            }
            return bHasSet;
        }
        public void SetURIResource(bool isEdit)
        {
            if (_dtoContentURI.URINodeName
                == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                var qry = _dataContext
                    .Service
                    .Where(s => s.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Resources.RESOURCES_TYPES.resourcegroup.ToString())
            {
                var qryRC = _dataContext
                    .ResourceClass
                    .Where(rc => rc.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Resources.RESOURCES_TYPES.resourcepack.ToString())
            {
                var qryRP
                        = _dataContext
                        .ResourcePack
                        .Where(rp => rp.PKId == _dtoContentURI.URIId);
            }
            else if (_dtoContentURI.URINodeName
                == Resources.RESOURCES_TYPES.resource.ToString())
            {
                var qryR = _dataContext
                    .Resource
                    .Where(r => r.PKId == _dtoContentURI.URIId);
            }
            _dtoContentURI.URIDataManager.RowCount = 1;
        }
        public async Task<bool> AddResource(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsAdded = false;
            bool bHasSet = true;
            //store updated resources ids in lists
            bool bHasAdded = await AddResource(argumentsEdits.SelectionsToAdd);
            //int iNewId = 0;
            if (bHasAdded)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsAdded = true;
                    //only the edit panel needs an updated collection of resources
                    if (_dtoContentURI.URIDataManager.ServerActionType
                        == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                    {
                        bHasSet = await SetURIResource(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIResource(_dtoContentURI, false);
                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddResource(List<ContentURI> addedURIs)
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
                if (addedURI.URINodeName == Resources.RESOURCES_TYPES.resourcegroup.ToString())
                {
                    var newResourceClass = new ResourceClass
                    {
                        ResourceClassNum = Helpers.GeneralHelpers.NONE,
                        ResourceClassName = addedURI.URIName,
                        ResourceClassDesc = Helpers.GeneralHelpers.NONE,
                        ServiceId = iParentId,
                        Service = null,
                        TypeId = 0,
                        ResourceType = null,
                        ResourcePack = null
                    };
                    _dataContext.ResourceClass.Add(newResourceClass);
                    _dataContext.Entry(newResourceClass).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
                {
                    var newResourcePack = new ResourcePack
                    {
                        ResourcePackNum = Helpers.GeneralHelpers.NONE,
                        ResourcePackName = addedURI.URIName,
                        ResourcePackDesc = Helpers.GeneralHelpers.NONE,
                        ResourcePackKeywords = Helpers.GeneralHelpers.NONE,
                        ResourcePackDocStatus = (short)Helpers.GeneralHelpers.DOCS_STATUS.notreviewed,
                        ResourcePackLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        ResourcePackMetaDataXml = string.Empty,
                        ResourceClassId = iParentId,
                        ResourceClass = null,
                        DevPackPartToResourcePack = null,
                        LinkedViewToResourcePack = null,
                        Resource = null
                    };
                    _dataContext.ResourcePack.Add(newResourcePack);
                    _dataContext.Entry(newResourcePack).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == Resources.RESOURCES_TYPES.resource.ToString())
                {
                    var newResource = new DevTreks.Models.Resource
                    {
                        ResourceNum = Helpers.GeneralHelpers.NONE,
                        ResourceTagNameForApps = Helpers.GeneralHelpers.NONE,
                        ResourceName = addedURI.URIName,
                        ResourceFileName = Helpers.GeneralHelpers.NONE,
                        ResourceShortDesc = Helpers.GeneralHelpers.NONE,
                        ResourceLongDesc = Helpers.GeneralHelpers.NONE,
                        ResourceLastChangedDate = Helpers.GeneralHelpers.GetDateShortNow(),
                        ResourceGeneralType = Resources.GENERAL_RESOURCE_TYPES.none.ToString(),
                        ResourceMimeType = Helpers.GeneralHelpers.NONE,
                        ResourcePackId = iParentId,
                        ResourcePack = null,
                        LinkedViewToResource = null
                        //0.9.0 removal -handle this large data using stored procs (not model)
                        //ResourceXml = Helpers.GeneralHelpers.NONE,
                        //ResourceBinary = null,
                    };
                    _dataContext.Resource.Add(newResource);
                    _dataContext.Entry(newResource).State = EntityState.Added;
                    bIsAdded = true;
                }
                else if (addedURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    bIsAdded = await AddResourceLinkedView(addedURI, _dtoContentURI.URINodeName);

                }
            }
            return bIsAdded;
        }
        private async Task<bool> AddResourceLinkedView(ContentURI addedURI,
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
                    if (inputNodeName == Resources.RESOURCES_TYPES.resource.ToString())
                    {
                        var newLinkedView = new LinkedViewToResource
                        {
                            LinkedViewName = sAddInName,
                            IsDefaultLinkedView = false,
                            LinkingXmlDoc = string.Empty,
                            LinkingNodeId = _dtoContentURI.URIId,
                            Resource = null,
                            LinkedViewId = iAddInId,
                            LinkedView = null
                        };
                        _dataContext.LinkedViewToResource.Add(newLinkedView);
                        _dataContext.Entry(newLinkedView).State = EntityState.Added;
                        bIsAdded = true;
                    }
                }
            }
            else
            {
                if (inputNodeName == Resources.RESOURCES_TYPES.resource.ToString())
                {
                    var newLinkedView = new LinkedViewToResource
                    {
                        LinkedViewName = addedURI.URIName,
                        IsDefaultLinkedView = false,
                        LinkingXmlDoc = string.Empty,
                        LinkingNodeId = _dtoContentURI.URIId,
                        Resource = null,
                        LinkedViewId = addedURI.URIId,
                        LinkedView = null
                    };
                    _dataContext.LinkedViewToResource.Add(newLinkedView);
                    _dataContext.Entry(newLinkedView).State = EntityState.Added;
                    bIsAdded = true;
                }
            }
            return bIsAdded;
        }
        public async Task<bool> DeleteResource(EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> deletedIds = new Dictionary<string, int>();
            bool bHasSet = await DeleteResource(argumentsEdits.SelectionsToAdd, deletedIds);
            if (deletedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsDeleted = true;
                    //this gets the edited objects
                    bHasSet = await SetURIResource(_dtoContentURI, false);
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIResource(_dtoContentURI, false);
                }
            }
            return bIsDeleted;
        }
        private async Task<bool> DeleteResource(List<ContentURI> deletionURIs,
             Dictionary<string, int> deletedIds)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            foreach (ContentURI deletionURI in deletionURIs)
            {
                if (deletionURI.URINodeName == Resources.RESOURCES_TYPES.resourcetype.ToString())
                {
                    var resourceType = await _dataContext.ResourceType.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (resourceType != null)
                    {
                        _dataContext.Entry(resourceType).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Resources.RESOURCES_TYPES.resourcegroup.ToString())
                {
                    var resourceClass = await _dataContext.ResourceClass.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (resourceClass != null)
                    {
                        _dataContext.Entry(resourceClass).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
                {
                    var resourcePack = await _dataContext.ResourcePack.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (resourcePack != null)
                    {
                        _dataContext.Entry(resourcePack).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == Resources.RESOURCES_TYPES.resource.ToString())
                {
                    var resource = await _dataContext.Resource.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
                    if (resource != null)
                    {
                        _dataContext.Entry(resource).State = EntityState.Deleted;
                        sKeyName = string.Concat(deletionURI.URINodeName, deletionURI.URIId);
                        if (deletedIds.ContainsKey(sKeyName) == false)
                        {
                            deletedIds.Add(sKeyName, deletionURI.URIId);
                        }
                    }
                }
                else if (deletionURI.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (_dtoContentURI.URINodeName == Resources.RESOURCES_TYPES.resource.ToString())
                    {
                        var linkedview = await _dataContext.LinkedViewToResource.SingleOrDefaultAsync(x => x.PKId == deletionURI.URIId);
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
        public async Task<bool> UpdateResource(List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            bool bHasSet = true;
            //store updated resources ids in node name and id dictionary
            Dictionary<string, int> updatedIds = new Dictionary<string, int>();
            bool bNeedsNewCollections = await UpdateResource(edits, updatedIds);
            if (updatedIds.Count > 0)
            {
                try
                {
                    int iNotUsed = await _dataContext.SaveChangesAsync();
                    bIsUpdated = true;
                    if (bNeedsNewCollections)
                    {
                        bHasSet = await SetURIResource(_dtoContentURI, false);
                    }
                }
                catch (Exception e)
                {
                    _dtoContentURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        e.ToString(), "ERROR_INTRO");
                }
                if (_dtoContentURI.ErrorMessage.Length > 0)
                {
                    bHasSet = await SetURIResource(_dtoContentURI, false);
                }
            }
            return bIsUpdated;
        }
        private async Task<bool> UpdateResource(List<EditHelper.ArgumentsEdits> edits,
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
                    bHasSet = await UpdateResourceLinkedView(edit, updatedIds, edit.URIToAdd.URINodeName);
                    bNeedsNewCollections = false;
                }
                else
                {
                    if (edit.URIToAdd.URINodeName == Resources.RESOURCES_TYPES.resourcetype.ToString())
                    {
                        var resourceType = await _dataContext.ResourceType.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (resourceType != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(resourceType), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(resourceType).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Resources.RESOURCES_TYPES.resourcegroup.ToString())
                    {
                        var resourceClass = await _dataContext.ResourceClass.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (resourceClass != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(resourceClass), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(resourceClass).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
                    {
                        var resourcePack = await _dataContext.ResourcePack.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (resourcePack != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(resourcePack), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(resourcePack).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == Resources.RESOURCES_TYPES.resource.ToString())
                    {
                        var resource = await _dataContext.Resource.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
                        if (resource != null)
                        {
                            RuleHelpers.GeneralRules.ValidateXSDInput(edit);
                            //update the property to the new value
                            string sErroMsg = string.Empty;
                            EditModelHelper.UpdateDbEntityProperty(_dataContext.Entry(resource), edit.EditAttName,
                                edit.EditAttValue, edit.EditDataType, ref sErroMsg);
                            _dtoContentURI.ErrorMessage = sErroMsg;
                            _dataContext.Entry(resource).State = EntityState.Modified;
                            sKeyName = string.Concat(edit.URIToAdd.URINodeName, edit.URIToAdd.URIId);
                            if (updatedIds.ContainsKey(sKeyName) == false)
                            {
                                updatedIds.Add(sKeyName, edit.URIToAdd.URIId);
                            }
                        }
                    }
                    else if (edit.URIToAdd.URINodeName == LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        bHasSet = await UpdateResourceLinkedView(edit, updatedIds, _dtoContentURI.URINodeName);
                    }
                }
            }
            return bNeedsNewCollections;
        }
        private async Task<bool> UpdateResourceLinkedView(EditHelper.ArgumentsEdits edit,
            Dictionary<string, int> updatedIds, string inputNodeName)
        {
            bool bHasSet = true;
            string sKeyName = string.Empty;
            if (inputNodeName == Resources.RESOURCES_TYPES.resource.ToString())
            {
                var linkedview = await _dataContext.LinkedViewToResource.SingleOrDefaultAsync(x => x.PKId == edit.URIToAdd.URIId);
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
            bool bHasSet = await SetURIResource(_dtoContentURI, bSaveInFileSystemContent);
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
            if (nodeName == Resources.RESOURCES_TYPES.servicebase.ToString())
            {
                var currentObject = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    _parentName = nodeName;
                    _parentId = id;
                    if (_dtoContentURI.URINodeName != Resources.RESOURCES_TYPES.servicebase.ToString())
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
            else if (nodeName == Resources.RESOURCES_TYPES.resourcegroup.ToString())
            {
                var currentObject = await _dataContext.ResourceClass.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    id = currentObject.ServiceId;
                    nodeName = Resources.RESOURCES_TYPES.servicebase.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Resources.RESOURCES_TYPES.resourcegroup.ToString())
                        {
                            XElement el = MakeResourceClassXml(currentObject);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName);
                        }
                    }

                }
            }
            else if (nodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
            {
                var currentObject = await _dataContext.ResourcePack.SingleOrDefaultAsync(x => x.PKId == id);
                if (currentObject != null)
                {
                    id = currentObject.ResourceClassId;
                    nodeName = Resources.RESOURCES_TYPES.resourcegroup.ToString();
                    bHasGoodAncestors = await AddAncestors(id,
                        nodeName, root);
                    if (bHasGoodAncestors)
                    {
                        _parentName = nodeName;
                        _parentId = id;
                        if (_dtoContentURI.URINodeName != Resources.RESOURCES_TYPES.resourcepack.ToString())
                        {
                            XElement el = MakeResourcePackXml(currentObject);
                            bHasGoodAncestors = EditHelpers.XmlLinq.AddElementToParent(root, el,
                                string.Empty, id.ToString(), nodeName);
                        }
                    }

                }
            }
            else if (nodeName == Resources.RESOURCES_TYPES.resource.ToString())
            {
                var currentObject = await GetResource(id);
                if (currentObject != null)
                {
                    id = currentObject.ResourcePackId;
                    nodeName = Resources.RESOURCES_TYPES.resourcepack.ToString();
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
            if (childNodeName == Resources.RESOURCES_TYPES.servicebase.ToString())
            {
                var obj1 = await _dataContext.Service.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj1 != null)
                {
                    XElement el = MakeServiceBaseXml(obj1);
                    if (el != null)
                    {
                        root.AddFirst(el);
                    }
                    bool bHasSet = await SetTempURIResource(tempURI, childNodeName, childId);
                    if (tempURI.URIModels.Service.ResourceClass != null)
                    {
                        if (tempURI.URIModels.Service.ResourceClass.Count == 0)
                        {
                            bHasGoodDescendants = true;
                        }
                        foreach (var child in tempURI.URIModels.Service.ResourceClass)
                        {
                            bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                child.PKId, Resources.RESOURCES_TYPES.resourcegroup.ToString(),
                                root);
                        }
                    }
                    else
                    {
                        bHasGoodDescendants = true;
                    }
                }
            }
            else if (childNodeName == Resources.RESOURCES_TYPES.resourcegroup.ToString())
            {
                var obj2 = await _dataContext.ResourceClass.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj2 != null)
                {
                    XElement el = MakeResourceClassXml(obj2);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        //don't allow servicebase docs to go deeper or docs will get too large to handle
                        if (_dtoContentURI.URINodeName != Resources.RESOURCES_TYPES.servicebase.ToString())
                        {
                            bool bHasSet = await SetTempURIResource(tempURI, childNodeName, childId);
                            if (tempURI.URIModels.ResourceClass.ResourcePack != null)
                            {
                                if (tempURI.URIModels.ResourceClass.ResourcePack.Count == 0)
                                {
                                    bHasGoodDescendants = true;
                                }
                                foreach (var child in tempURI.URIModels.ResourceClass.ResourcePack)
                                {
                                    bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                        child.PKId, Resources.RESOURCES_TYPES.resourcepack.ToString(),
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
            else if (childNodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
            {
                var obj3 = await _dataContext.ResourcePack.SingleOrDefaultAsync(x => x.PKId == childId);
                if (obj3 != null)
                {
                    XElement el = MakeResourcePackXml(obj3);
                    bHasBeenAdded = EditHelpers.XmlLinq.AddElementToParent(root, el,
                            string.Empty, parentId.ToString(), parentNodeName);
                    if (bHasBeenAdded)
                    {
                        bool bHasSet = await SetTempURIResource(tempURI, childNodeName, childId);
                        if (tempURI.URIModels.ResourcePack.Resource != null)
                        {
                            if (tempURI.URIModels.ResourcePack.Resource.Count == 0)
                            {
                                bHasGoodDescendants = true;
                            }
                            foreach (var child in tempURI.URIModels.ResourcePack.Resource)
                            {
                                bHasGoodDescendants = await AddDescendants(tempURI, childId, childNodeName,
                                    child.PKId, Resources.RESOURCES_TYPES.resource.ToString(),
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
            else if (childNodeName == Resources.RESOURCES_TYPES.resource.ToString())
            {
                var obj4 = await GetResource(childId);
                if (obj4 != null)
                {
                    XElement el = MakeResourceXml(obj4);
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
        private async Task<bool> SetTempURIResource(ContentURI tempURI, string nodeName, int id)
        {
            tempURI.URINodeName = nodeName;
            tempURI.URIId = id;
            Helpers.AppSettings.CopyURIAppSettings(_dtoContentURI, tempURI);
            bool bHasSet = await SetURIResource(tempURI, true);
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
        private XElement MakeResourceClassXml(ResourceClass obj)
        {
            XElement currentNode = new XElement(Resources.RESOURCES_TYPES.resourcegroup.ToString());
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
        private XElement MakeResourcePackXml(ResourcePack obj)
        {
            XElement currentNode = new XElement(Resources.RESOURCES_TYPES.resourcepack.ToString());
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
        private async Task<DevTreks.Models.Resource> GetResource(int id)
        {
            DevTreks.Models.Resource bs = await _dataContext
                    .Resource
                    .Include(t => t.LinkedViewToResource)
                    .Where(b => b.PKId == id)
                    .FirstOrDefaultAsync();
            return bs;
        }
        private XElement MakeResourceXml(DevTreks.Models.Resource obj)
        {
            XElement currentNode = new XElement(Resources.RESOURCES_TYPES.resource.ToString());
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
            if (obj.LinkedViewToResource != null)
            {
                foreach (var lv in obj.LinkedViewToResource)
                {
                    EditModelHelper.AddXmlAttributeToDoc(lv.LinkingXmlDoc, currentNode);
                }
            }
            return currentNode;
        }
    }
}
