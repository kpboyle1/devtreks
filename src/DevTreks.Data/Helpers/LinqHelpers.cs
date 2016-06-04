using DevTreks.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities for working with language integrated queries
    ///             and generics
    ///Author:		www.devtreks.org
    ///Date:		2013, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class LinqHelpers
    {
        public static ContentURI GetContentURIByURIPattern(
            IList<ContentURI> contentURIs, string uriPattern)
        {
            ContentURI contentURI = null;
            if (contentURIs != null && uriPattern != string.Empty)
            {
                contentURI = contentURIs.FirstOrDefault(
                    c => c.URIPattern == uriPattern);
            }
            return contentURI;
        }
        public static ContentURI GetLinkedViewURIByURIPattern(
            ContentURI uri, string uriPattern)
        {
            ContentURI contentURI = null;
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIPattern == uriPattern)
                        {
                            return linkedview;
                        }
                    }
                }
            }
            return contentURI;
        }
        public static ContentURI GetContentURIByIdAndNodeName(
            IList<ContentURI> contentURIs, int id, string nodeName)
        {
            ContentURI contentURI = null;
            if (contentURIs != null && id != 0 && nodeName != string.Empty)
            {
                contentURI = contentURIs.FirstOrDefault(
                    c => c.URINodeName == nodeName && c.URIId == id);
            }
            return contentURI;
        }
        public static ContentURI GetContentURIByNodeName(
            IList<ContentURI> contentURIs, string nodeName)
        {
            ContentURI contentURI = null;
            if (contentURIs != null && nodeName != string.Empty)
            {
                contentURI = contentURIs.FirstOrDefault(
                    c => c.URINodeName == nodeName);
            }
            return contentURI;
        }
        public static int GetFirstAncestorPositionFromEndOfList(ContentURI uri)
        {
            int iAncestorPositionFromEndOfList = 1;
            if (uri.URIDataManager.Ancestors != null)
            {
                int iIndex = uri.URIDataManager.Ancestors.IndexOf(
                    uri.URIDataManager.Ancestors.FirstOrDefault(
                        c => c.URIPattern == uri.URIDataManager.ParentURIPattern));
                if (iIndex == -1)
                {
                    //first ancestor is one more than current node
                    iIndex = uri.URIDataManager.Ancestors.IndexOf(
                        uri.URIDataManager.Ancestors.FirstOrDefault(
                            c => c.URINodeName == uri.URINodeName
                            && c.URIId == uri.URIId));
                    if (iIndex != -1)
                    {
                        iAncestorPositionFromEndOfList
                            = (uri.URIDataManager.Ancestors.Count - iIndex) + 1;
                    }
                }
                else
                {
                    iAncestorPositionFromEndOfList
                        = (uri.URIDataManager.Ancestors.Count - iIndex);
                }
            }
            return iAncestorPositionFromEndOfList;
        }
        public static ContentURI GetParentURIByChildIdAndNodeName(
            IList<ContentURI> contentURIs, int id, string nodeName)
        {
            ContentURI contentURI = null;
            if (contentURIs != null && id != 0 && nodeName != string.Empty)
            {
                ContentURI childContentURI 
                    = GetContentURIByIdAndNodeName(contentURIs, id, nodeName);
                int iChildIndex = contentURIs.IndexOf(childContentURI);
                if (iChildIndex >= 0)
                {
                    int iParentIndex = (iChildIndex != 0) ? iChildIndex - 1 : iChildIndex;
                    contentURI = contentURIs.ElementAtOrDefault(iParentIndex);
                }
            }
            return contentURI;
        }
        public static ContentURI GetParentFromAncestors(ContentURI uri)
        {
            ContentURI parentURI = new ContentURI();
            if (uri.URIDataManager.Ancestors != null)
            {
                parentURI = uri.URIDataManager.Ancestors.FirstOrDefault(
                       c => c.URIPattern 
                           == uri.URIDataManager.ParentURIPattern);
            }
            return parentURI;
        }
        public static ContentURI GetLastAncestorFromAncestors(ContentURI uri)
        {
            ContentURI lastAncestorURI = new ContentURI();
            if (uri.URIDataManager.Ancestors != null)
            {
                lastAncestorURI = uri.URIDataManager.Ancestors.LastOrDefault();
            }
            return lastAncestorURI;
        }
        public static bool UpdateURIPatternInList(string oldURIPattern,
            string newURIPattern, List<ContentURI> uris)
        {
            bool bHasUpdated = false;
            if (uris.Any(c => c.URIPattern == oldURIPattern))
            {
                uris.FirstOrDefault(
                    c => c.URIPattern == oldURIPattern).URIPattern = newURIPattern;
                bHasUpdated = true;
            }
            return bHasUpdated;
        }
        public static ContentURI GetContentURIByIdAndNodeName(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews, 
            int id, string nodeName)
        {
            ContentURI contentURI = null;
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIId == id
                            && linkedview.URINodeName == nodeName)
                        {
                            return linkedview;
                        }
                    }
                }
            }
            return contentURI;
        }
        public static void SetLinkedViewTempDocPaths(
            ContentURI uri)
        {
            //refactor with query syntax
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        //keep all linkedviews in same temp subdirectory
                        linkedview.URIDataManager.TempDocURIPattern = uri.URIDataManager.TempDocURIPattern;
                        //don't change ids or uripatterns or can't find related resources
                        Helpers.AppSettings.SetDocPathandFileNameForTempDocs(
                            linkedview);
                        linkedview.URIDataManager.EditViewEditType
                                = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    }
                }
            }
        }
        /// <summary>
        /// Filters the linkedViews addin list by the isdefault property = true
        /// </summary>
        public static ContentURI GetLinkedViewIsDefaultAddIn(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews)
        {
            ContentURI oLinkedView = null;
            //refactor with query syntax
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        string sNodeName = ContentURI.GetURIPatternPart(linkedview.URIPattern, ContentURI.URIPATTERNPART.node);
                        if (sNodeName == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            if (linkedview.URIDataManager.IsDefault == true
                                && AddInHelper.IsAddIn(linkedview))
                            {
                                return linkedview;
                            }
                        }
                    }
                }
            }
            return oLinkedView;
        }
        public static string GetLinkedViewIsDefaultAddInId(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews)
        {
            string sLinkedViewId = string.Empty;
            //refactor with query syntax
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        string sNodeName = ContentURI.GetURIPatternPart(linkedview.URIPattern, ContentURI.URIPATTERNPART.node);
                        if (sNodeName == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            if (linkedview.URIDataManager.IsDefault == true
                                && AddInHelper.IsAddIn(linkedview))
                            {
                                return linkedview.URIId.ToString();
                            }
                        }
                    }
                }
            }
            return sLinkedViewId;
        }
        public static ContentURI GetLinkedViewIsDefault(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews)
        {
            ContentURI oLinkedView = null;
            //refactor with query syntax
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        string sNodeName = ContentURI.GetURIPatternPart(linkedview.URIPattern, ContentURI.URIPATTERNPART.node);
                        if (sNodeName == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            if (linkedview.URIDataManager.IsDefault == true)
                            {
                                return linkedview;
                            }
                        }
                    }
                }
            }
            return oLinkedView;
        }
        public static string GetLinkedViewURIPatternByFileExtension(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews, string fileExtType)
        {
            string sLVURIPattern = string.Empty;
            //refactor with query syntax
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        string sNodeName = ContentURI.GetURIPatternPart(linkedview.URIPattern, ContentURI.URIPATTERNPART.node);
                        if (sNodeName == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            if (linkedview.URIFileExtensionType == fileExtType
                                && AddInHelper.IsAddIn(linkedview))
                            {
                                //calculator
                                sLVURIPattern = linkedview.URIPattern;
                            }
                            else
                            {
                                if (linkedview.URIFileExtensionType == fileExtType
                                    && fileExtType == GeneralHelpers.NONE)
                                {
                                    //story
                                    sLVURIPattern = linkedview.URIPattern;
                                }
                            }
                        }
                    }
                }
            }
            return sLVURIPattern;
        }
        public static void SetLinkedViewIsDefaultView(
          IList<System.Linq.IGrouping<int, ContentURI>> linkedViews,
          string linkedViewCustomDocURIPattern)
        {
            string sLinkedViewId = ContentURI.GetURIPatternPart(
                linkedViewCustomDocURIPattern, ContentURI.URIPATTERNPART.id);
            int iLinkedViewId = Helpers.GeneralHelpers.ConvertStringToInt(sLinkedViewId);
            if (linkedViews != null && iLinkedViewId != 0)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        //set them all to false
                        linkedview.URIDataManager.IsDefault = false;
                        if (linkedview.URIId == iLinkedViewId)
                        {
                            linkedview.URIDataManager.IsDefault = true;
                        }
                    }
                }
            }
        }
        public static bool SetLinkedViewIsDefaultView(
          IList<ContentURI> linkedViews,
          string linkedViewCustomDocURIPattern)
        {
            bool bHasIsSelectedMember = false;
            ContentURI defaultLinkedViewURI 
                = ContentURI.ConvertShortURIPattern(linkedViewCustomDocURIPattern);
            int iLinkedViewId = defaultLinkedViewURI.URIId;
            if (linkedViews != null && iLinkedViewId != 0)
            {
                foreach (ContentURI linkedview in linkedViews)
                {
                    //set them all to false
                    linkedview.URIDataManager.IsDefault = false;
                    if (linkedview.URIId == iLinkedViewId)
                    {
                        linkedview.URIDataManager.IsDefault = true;
                        bHasIsSelectedMember = true;
                    }
                }
            }
            return bHasIsSelectedMember;
        }
        public static ContentURI GetLinkedView(
           IList<System.Linq.IGrouping<int, ContentURI>> linkedViews,
           int linkedViewId)
        {
            ContentURI oLinkedView = null;
            //refactor with query syntax
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIId == linkedViewId)
                        {
                            return linkedview;
                        }
                    }
                }
            }
            return oLinkedView;
        }
        public static bool HasLinkedView(
           IList<System.Linq.IGrouping<int, ContentURI>> linkedViews,
           string linkedViewId)
        {
            bool bHasLinkedView = false;
            ContentURI oLinkedView = null;
            //refactor with query syntax
            if (linkedViews != null)
            {
                int iLinkedViewId = Helpers.GeneralHelpers.ConvertStringToInt(linkedViewId);
                oLinkedView = GetLinkedView(linkedViews, iLinkedViewId);
                if (oLinkedView != null)
                {
                    bHasLinkedView = true;
                }
            }
            return bHasLinkedView;
        }
        /// <summary>
        /// Sets the linkedViews list  isselectedaddin property = true
        /// </summary>
        public static bool SetLinkedViewIsSelectedAddIn(ContentURI uri,
           int linkedViewId)
        {
            bool bHasIsSelectedMember = false;
            //refactor with query syntax
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIId == linkedViewId)
                        {
                            //linked addins must have hostnames and addinnames
                            if (AddInHelper.IsAddIn(linkedview))
                            {
                                linkedview.URIDataManager.IsSelectedLinkedAddIn = true;
                                bHasIsSelectedMember = true;
                                return bHasIsSelectedMember;
                            }
                        }
                    }
                }
            }
            return bHasIsSelectedMember;
        }
        public static bool SetLinkedViewIsDefaultOrFirstAddIn(ContentURI uri)
        {
            //if uri has an addin, even if not selected yet, set one for a default display
            bool bHasIsSelectedMember = false;
            //set the addin based on a. IsDefault or b.IsFirst
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        string sNodeName = ContentURI.GetURIPatternPart(linkedview.URIPattern, ContentURI.URIPATTERNPART.node);
                        if (sNodeName == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            if (linkedview.URIDataManager.IsDefault == true
                                && AddInHelper.IsAddIn(linkedview))
                            {
                                linkedview.URIDataManager.IsSelectedLinkedAddIn = true;
                                bHasIsSelectedMember = true;
                                return bHasIsSelectedMember;
                            }
                        }
                    }
                }
                if (!bHasIsSelectedMember)
                {
                    foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                    {
                        foreach (ContentURI linkedview in linkedviewparent)
                        {
                            string sNodeName = ContentURI.GetURIPatternPart(linkedview.URIPattern, ContentURI.URIPATTERNPART.node);
                            if (sNodeName == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            {
                                if (AddInHelper.IsAddIn(linkedview))
                                {
                                    linkedview.URIDataManager.IsSelectedLinkedAddIn = true;
                                    bHasIsSelectedMember = true;
                                    return bHasIsSelectedMember;
                                }
                            }
                        }
                    }
                }
            }
            
            return bHasIsSelectedMember;
        }
        
        /// <summary>
        /// Sets the linkedViews list  isselectedview property = true
        /// </summary>
        public static bool SetLinkedViewIsSelectedView(ContentURI uri,
           string linkedViewCustomDocURIPattern)
        {
            bool bHasIsSelectedMember = false;
            string sLinkedViewId = ContentURI.GetURIPatternPart(linkedViewCustomDocURIPattern, 
                ContentURI.URIPATTERNPART.id);
            int iLinkedViewId = Helpers.GeneralHelpers.ConvertStringToInt(sLinkedViewId);
            if (uri.URIDataManager.LinkedView != null && iLinkedViewId != 0)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIId == iLinkedViewId)
                        {
                            //custom doc views can't be addins as well, custom docs need clear separation
                            if (!AddInHelper.IsAddIn(linkedview))
                            {
                                linkedview.URIDataManager.IsSelectedLinkedView = true;
                                bHasIsSelectedMember = true;
                                return bHasIsSelectedMember;
                            }
                        }
                    }
                }
            }
            return bHasIsSelectedMember;
        }
        public static void SetLinkedViewUseSelectedView(ContentURI uri)
        {
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIDataManager.IsSelectedLinkedView)
                        {
                            linkedview.URIDataManager.UseSelectedLinkedView = true;
                        }
                    }
                }
            }
        }
        public static void SetLinkedViewUseFirst(ContentURI uri)
        {
            //used when paginating, but not selecting (to display something instead of nothing)
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    int i = 0;
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (i == 0)
                        {
                            linkedview.URIDataManager.IsSelectedLinkedView = true;
                            linkedview.URIDataManager.UseSelectedLinkedView = true;
                        }
                        else
                        {
                            linkedview.URIDataManager.IsSelectedLinkedView = false;
                            linkedview.URIDataManager.UseSelectedLinkedView = false;
                        }
                        i += 1;
                    }
                }
            }
        }
        /// <summary>
        /// Filters the linkedViews list by the isselectedaddin property = true
        /// </summary>
        public static ContentURI GetLinkedViewIsSelectedAddIn(ContentURI uri)
        {
            ContentURI oLinkedView = null;
            //refactor with query syntax
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIDataManager.IsSelectedLinkedAddIn == true)
                        {
                            return linkedview;
                        }
                    }
                }
            }
            return oLinkedView;
        }
        public static ContentURI GetLinkedViewIsSelectedView(ContentURI uri)
        {
            ContentURI oLinkedView = null;
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        //2.0.0 condition added because calcdocuris can inherit props from parent and 
                        //custom doc views can't be addins as well (see SetLinkedViewIsSelectedView)
                        if (!AddInHelper.IsAddIn(linkedview))
                        {
                            if (linkedview.URIDataManager.IsSelectedLinkedView == true)
                            {
                                return linkedview;
                            }
                        }
                    }
                }
            }
            return oLinkedView;
        }
        public static bool HasLinkedViewIsSelectedView(ContentURI uri)
        {
            bool bHasStoryView = false;
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIDataManager.IsSelectedLinkedView == true
                            || (linkedview.URIDataManager.HostName == GeneralHelpers.NONE
                                || linkedview.URIDataManager.HostName == string.Empty))
                        {
                            bHasStoryView = true;
                            return bHasStoryView;
                        }
                    }
                }
            }
            return bHasStoryView;
        }
        public static void AddAncestorsToLinkedView(ContentURI docToCalcURI,
            ContentURI linkedViewURI)
        {
            if (linkedViewURI != null
                && docToCalcURI != null)
            {
                if (docToCalcURI.URIDataManager.Ancestors != null)
                {
                    linkedViewURI.URIDataManager.Ancestors
                        = new List<ContentURI>();
                    AddList2ToList1(linkedViewURI.URIDataManager.Ancestors, 
                        docToCalcURI.URIDataManager.Ancestors);
                    //and linkedviews are the children of doctocalc
                    linkedViewURI.URIDataManager.Ancestors.Add(docToCalcURI);
                }
            }
        }
        public static void UpdateLinkedViewAddInAndSelectedView(ContentURI uri)
        {
            ContentURI oSelectedLinkedViewURI = null;
            ContentURI oCalcDocURI = null;
            if (uri.URIDataManager.LinkedView != null)
            {
                oSelectedLinkedViewURI = GetLinkedViewIsSelectedView(uri);
                if (oSelectedLinkedViewURI != null)
                {
                    //linkedview was set prior to all of uri's properties being set
                    //the linkedview needs several of those properties 
                    bool bNeedsAncestors = true;
                    Helpers.ContentHelper.UpdateNewURIArgs(uri,
                        oSelectedLinkedViewURI, bNeedsAncestors);
                    oSelectedLinkedViewURI.URIDataManager.UseDefaultAddIn
                        = uri.URIDataManager.UseDefaultAddIn;
                    oSelectedLinkedViewURI.URIClub.PrivateAuthorizationLevel
                        = uri.URIClub.PrivateAuthorizationLevel;
                    oSelectedLinkedViewURI.URIMember.ClubInUse.PrivateAuthorizationLevel
                        = uri.URIMember.ClubInUse.PrivateAuthorizationLevel;
                }
                if (uri.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks
                    || uri.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews)
                {
                    if (oSelectedLinkedViewURI != null)
                    {
                        //calcdocs are associated with selected view
                        oCalcDocURI = GetLinkedViewIsSelectedAddIn(oSelectedLinkedViewURI);
                        if (oCalcDocURI != null)
                        {
                            //linkedview was set prior to all of uri's properties being set
                            //the linkedview needs several of those properties 
                            bool bNeedsAncestors = true;
                            Helpers.ContentHelper.UpdateNewURIArgs(uri,
                                oCalcDocURI, bNeedsAncestors);
                            oCalcDocURI.URIDataManager.UseDefaultAddIn
                                = uri.URIDataManager.UseDefaultAddIn;
                            oCalcDocURI.URIClub.PrivateAuthorizationLevel
                                = uri.URIClub.PrivateAuthorizationLevel;
                            oCalcDocURI.URIMember.ClubInUse.PrivateAuthorizationLevel
                                = uri.URIMember.ClubInUse.PrivateAuthorizationLevel;
                        }
                    }
                }
                else
                {
                    //addins are associated with doctocalcuri
                    oCalcDocURI = GetLinkedViewIsSelectedAddIn(uri);
                    if (oCalcDocURI != null)
                    {
                        //linkedview was set prior to all of uri's properties being set
                        //the linkedview needs several of those properties 
                        bool bNeedsAncestors = true;
                        Helpers.ContentHelper.UpdateNewURIArgs(uri,
                            oCalcDocURI, bNeedsAncestors);
                        oCalcDocURI.URIDataManager.UseDefaultAddIn
                            = uri.URIDataManager.UseDefaultAddIn;
                        oCalcDocURI.URIClub.PrivateAuthorizationLevel
                            = uri.URIClub.PrivateAuthorizationLevel;
                        oCalcDocURI.URIMember.ClubInUse.PrivateAuthorizationLevel
                            = uri.URIMember.ClubInUse.PrivateAuthorizationLevel;
                    }
                }
            }
        }
        public static ContentURI GetLinkedViewIsFirst(ContentURI uri)
        {
            //return the first contenturi in lists
            ContentURI oLinkedView = null;
            //refactor with query syntax
            if (uri.URIDataManager.LinkedView != null)
            {
                foreach (var linkedviewparent in uri.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        return linkedview;
                    }
                }
            }
            return oLinkedView;
        }
        /// <summary>
        /// Filters the contenturi list by the ismainimage property = true
        /// </summary>
        public static ContentURI GetContentURIListIsMainImage(
           IList<ContentURI> contenturis)
        {
            ContentURI oLinkedView = null;
            if (contenturis != null)
            {
                oLinkedView = contenturis.FirstOrDefault(
                    c => c.URIDataManager.IsMainImage == true);
            }
            return oLinkedView;
        }
        /// <summary>
        /// Filters the contenturi list by the ismainstylesheet property = true
        /// </summary>
        public static ContentURI GetContentURIListIsMainStylesheet(
           IList<ContentURI> contenturis)
        {
            ContentURI oLinkedView = null;
            if (contenturis != null)
            {
                oLinkedView = contenturis.FirstOrDefault(
                    c => c.URIDataManager.IsMainStylesheet == true);
            }
            return oLinkedView;
        }
        public static bool HasMainStylesheetWithSameFileName(
           IList<ContentURI> contenturis, string stylesheetFileName)
        {
            bool bHasSameStylesheet = false;
            if (contenturis != null)
            {
                ContentURI oLinkedView = contenturis.FirstOrDefault(
                    c => c.URIDataManager.IsMainStylesheet == true);
                if (oLinkedView != null)
                {
                    if (Path.GetFileName(oLinkedView.URIDataManager.FileSystemPath).ToLower()
                        == stylesheetFileName.ToLower())
                    {
                        bHasSameStylesheet = true;
                    }
                }
            }
            return bHasSameStylesheet;
        }
        public static bool HasMainStylesheet(
           IList<ContentURI> contenturis)
        {
            bool bHasMainStylesheet = false;
            if (contenturis != null)
            {
                ContentURI oLinkedView = contenturis.FirstOrDefault(
                    c => c.URIDataManager.IsMainStylesheet == true);
                if (oLinkedView != null)
                {
                    bHasMainStylesheet = true;
                }
            }
            return bHasMainStylesheet;
        }
        public static void SetContentURIListIsMainStylesheet(
           IList<ContentURI> contenturis, ContentURI stylesheetURI)
        {
            ContentURI oLinkedView = null;
            stylesheetURI.URIDataManager.IsMainStylesheet = true;
            if (contenturis != null)
            {
                foreach (ContentURI resourceURI in contenturis)
                {
                    resourceURI.URIDataManager.IsMainStylesheet = false;
                }
                oLinkedView = contenturis.FirstOrDefault(
                    c => c.URIPattern == stylesheetURI.URIPattern);
                if (oLinkedView == null)
                {
                    contenturis.Add(stylesheetURI);
                }
                else
                {
                    oLinkedView.URIDataManager.IsMainStylesheet = true;
                }

            }
            else
            {
                contenturis = new List<ContentURI>();
                contenturis.Add(stylesheetURI);
            }
        }
        public static bool LinkedViewHaveAddIn(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews)
        {
            bool bSelectedLinkedViewHasAddInHost = false;
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (AddInHelper.IsAddIn(linkedview))
                        {
                            bSelectedLinkedViewHasAddInHost = true;
                            return bSelectedLinkedViewHasAddInHost;
                        }
                    }
                }
            }
            return bSelectedLinkedViewHasAddInHost;
        }
        public static bool LinkedViewHaveAddIn(
            IList<ContentURI> linkedViews)
        {
            bool bSelectedLinkedViewHasAddInHost = false;
            if (linkedViews != null)
            {
                foreach (ContentURI linkedview in linkedViews)
                {
                    if (AddInHelper.IsAddIn(linkedview))
                    {
                        bSelectedLinkedViewHasAddInHost = true;
                        return bSelectedLinkedViewHasAddInHost;
                    }
                }
            }
            return bSelectedLinkedViewHasAddInHost;
        }
        public static string SelectedLinkedViewAddInHostName(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews)
        {
            string sSelectedLinkedViewHasAddInHost = string.Empty;
            if (linkedViews != null)
            {
                foreach (var linkedviewparent in linkedViews)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIDataManager.IsSelectedLinkedAddIn == true)
                        {
                            if (AddInHelper.IsAddIn(linkedview))
                            {
                                sSelectedLinkedViewHasAddInHost = linkedview.URIDataManager.HostName;
                                return sSelectedLinkedViewHasAddInHost;
                            }
                        }
                    }
                }
            }
            return sSelectedLinkedViewHasAddInHost;
        }
        public static string GetChildURIPattern(IList<ContentURI> uris, int index)
        {
            string sSelectedViewURI = string.Empty;
            if (uris.Count > index)
            {
                //selected linked view is child of current ancestor
                ContentURI oChildURI = uris.ElementAt(index);
                if (oChildURI != null)
                {
                    sSelectedViewURI = oChildURI.URIPattern;
                }
            }
            return sSelectedViewURI;
        }
        public static void CopyLinkedView(ContentURI uriCopyFrom,
            ContentURI uriCopyTo)
        {
            if (uriCopyFrom.URIDataManager.LinkedView != null)
            {
                uriCopyTo.URIDataManager.LinkedView 
                    = new List<System.Linq.IGrouping<int, ContentURI>>();
                foreach (var linkedviewparent in uriCopyFrom.URIDataManager.LinkedView)
                {
                    uriCopyTo.URIDataManager.LinkedView.Add(linkedviewparent);
                }
            }
        }
       
        public static void ReplaceAddInViews(ContentURI selectedLinkedView,  
            IList<System.Linq.IGrouping<int, ContentURI>> parentLinkedView)
        {
            //customdoc state management runs parent addins against selectedlinkedviews
            //i.e. it's more convenient to run one set of calcs/analyses against treatments 1 to 35
            //(db state management runs parent addins against parent docs)
            if (selectedLinkedView.URIDataManager.LinkedView != null
                && parentLinkedView != null)
            {
                //replace selectedlinkedview's addins and isselectedlinkedview with parents
                IList<ContentURI> lstLinkedView = new List<ContentURI>();
                foreach (var linkedviewparent in parentLinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIDataManager.IsSelectedLinkedView == true
                            || (string.IsNullOrEmpty(linkedview.URIDataManager.AddInName) == false
                                && (!linkedview.URIDataManager.AddInName.EndsWith(Helpers.GeneralHelpers.NONE))))
                        {
                            lstLinkedView.Add(linkedview);
                        }
                    }
                }
                foreach (var linkedviewparent in selectedLinkedView.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URIDataManager.IsSelectedLinkedView == false
                            && (string.IsNullOrEmpty(linkedview.URIDataManager.AddInName) == true
                                || (linkedview.URIDataManager.AddInName.EndsWith(Helpers.GeneralHelpers.NONE))))
                        {
                            lstLinkedView.Add(linkedview);
                        }
                    }
                }
                IEnumerable<System.Linq.IGrouping<int, ContentURI>> qryGroupURIs = 
                    from parent in lstLinkedView
                    group parent by ContentURI.GetGroupingParentId(parent.URIDataManager.ParentURIPattern)
                    into parents
                    select parents;
                selectedLinkedView.URIDataManager.LinkedView = qryGroupURIs.ToList();
            }
        }
        public static void AddURIToLinkedView(ContentURI uri,
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews)
        {
            IList<ContentURI> lstLinkedView = new List<ContentURI>();
            lstLinkedView.Add(uri);
            IEnumerable<System.Linq.IGrouping<int, ContentURI>> qryGroupURIs =
                    from parent in lstLinkedView
                    group parent by ContentURI.GetGroupingParentId(parent.URIDataManager.ParentURIPattern)
                    into parents
                    select parents;
            linkedViews.Add(qryGroupURIs.ToList().First());
        }
        public static void AddLinkedView2ToLinkedView1(
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews1,
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews2)
        {
            if (linkedViews1 != null && linkedViews2 != null)
            {
                foreach (var linkedparent in linkedViews2)
                {
                    linkedViews1.Add(linkedparent);
                }
            }
        }
        public static void AddDefaultAddIns1ToLinkedView1(IList<System.Linq.IGrouping<int, ContentURI>> linkedViews1,
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews2)
        {
            if (linkedViews1 != null && linkedViews2 != null)
            {
                foreach (var linkedviewparent in linkedViews2)
                {
                    bool bNeedsParent = false;
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URINodeName 
                            == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            bNeedsParent = true;
                            break;
                        }
                    }
                    if (bNeedsParent)
                    {
                        linkedViews1.Add(linkedviewparent);
                    }
                }
            }
        }
        public static void AddList2ToList1(IList<ContentURI> list1, 
            IList<ContentURI> list2)
        {
            if (list2 != null && list1 != null)
            {
                foreach (ContentURI list2Member in list2)
                {
                    list1.Add(list2Member);
                }
            }
        }
        public static List<ContentURI> CopyContentURIs(IList<ContentURI> contentURIs)
        {
            List<ContentURI> copyContentURIs = new List<ContentURI>();
            if (contentURIs != null)
            {
                AddList2ToList1(copyContentURIs, contentURIs);
            }
            return copyContentURIs;
        }

        public static void AddResource2ToResource1(ContentURI uri1,
            ContentURI uri2)
        {
            if (uri1 != null && uri2 != null)
            {
                if (uri2.URIDataManager.Resource != null)
                {
                    if (uri1.URIDataManager.Resource == null)
                        uri1.URIDataManager.Resource = new List<ContentURI>();
                    foreach (ContentURI list2Member in uri2.URIDataManager.Resource)
                    {
                        uri1.URIDataManager.Resource.Add(list2Member);
                    }
                }
            }
        }
        public static IDictionary<string, string> CopyDictionary(
            IDictionary<string, string> list1)
        {
            IDictionary<string, string> list2 
                = new Dictionary<string, string>();
            if (list1 != null)
            {
                foreach (KeyValuePair<string, string> kvp
                    in list1)
                {
                    list2.Add(kvp.Key, kvp.Value);
                }
            }
            return list2;
        }
        public static Network GetSameNetworkFromList(string uriPattern, 
            List<ContentURI> uris)
        {
            Network network = null;
            string sNetworkName = ContentURI.GetURIPatternPart(uriPattern,
                ContentURI.URIPATTERNPART.network);
            //networkname can be either a number or a shortname
            int iNetworkId = Helpers.GeneralHelpers.ConvertStringToInt(sNetworkName);
            ContentURI networkURI = new ContentURI();
            if (iNetworkId == 0)
            {
                //nullable type can be returned
                networkURI = uris.FirstOrDefault(
                    c => c.URINetwork.NetworkURIPartName == sNetworkName);
            }
            else
            {
                networkURI = uris.FirstOrDefault(
                    c => c.URINetwork.PKId == iNetworkId);
            }
            if (networkURI != null)
            {
                network = new Network(networkURI.URINetwork);
            }
            return network;
        }
        public static bool ListContainsAncestors(
            ContentURI uriNeedingAncestors, List<ContentURI> urisWithAncestors)
        {
            //objective is to reduce xml to linq queries (with memory overhead) 
            //arising from processing same ancestors multiple times
            bool bContainsAncestors = false;
            ContentURI uriWithAncestors = urisWithAncestors.FirstOrDefault(
                    c => c.URIDataManager.ParentURIPattern 
                    == uriNeedingAncestors.URIDataManager.ParentURIPattern
                    && c.URIDataManager.Ancestors != null);
            if (uriWithAncestors != null)
            {
                if (uriWithAncestors.URIDataManager.Ancestors.Count > 0)
                {
                    bContainsAncestors = true;
                }
            }
            return bContainsAncestors;
        }
        public static List<ContentURI> GetURIsWithAncestors(
            List<ContentURI> uris)
        {
            List<ContentURI> lstSelectionsWithAncestors
                = uris.FindAll(
                  c => c.URIDataManager.Ancestors != null);
            if (lstSelectionsWithAncestors != null)
            {
                //one more filter
                lstSelectionsWithAncestors
                    = lstSelectionsWithAncestors.FindAll(
                    c => c.URIDataManager.Ancestors.Count > 0);
            }
            return lstSelectionsWithAncestors;
        }
        public static void SetParentURIPatternsForAncestors(
            IList<ContentURI> ancestors)
        {
            string sParentURIPattern = string.Empty;
            //ancestors are in uniform order
            foreach (ContentURI childURI in ancestors)
            {
                if (sParentURIPattern != string.Empty)
                {
                    childURI.URIDataManager.ParentURIPattern = sParentURIPattern;
                }
                sParentURIPattern = childURI.URIPattern;
            }
        }
        public static void SetParentURIPatternFromAncestors(
            ContentURI uri)
        {
            if (uri.URIDataManager.Ancestors != null)
            {
                //parent is set using last ancestor (but double check)
                string sParentURIPattern = string.Empty;
                if (uri.URIDataManager.Ancestors.Count > 0)
                {
                    ContentURI ancestorURI
                        = uri.URIDataManager.Ancestors.ElementAt(
                        uri.URIDataManager.Ancestors.Count() - 1);
                    if (ancestorURI != null)
                    {
                        sParentURIPattern = ancestorURI.URIPattern;
                        if (sParentURIPattern
                            == uri.URIDataManager.ParentURIPattern)
                        {
                            sParentURIPattern = string.Empty;
                        }
                    }
                    if (sParentURIPattern == string.Empty)
                    {
                        if (uri.URIDataManager.Ancestors.Count > 1)
                        {
                            //try the next ancestor
                            ancestorURI
                                = uri.URIDataManager.Ancestors.ElementAt(
                            uri.URIDataManager.Ancestors.Count() - 2);
                            if (ancestorURI != null)
                            {
                                sParentURIPattern = ancestorURI.URIPattern;
                                if (sParentURIPattern
                                    == uri.URIDataManager.ParentURIPattern)
                                {
                                    sParentURIPattern = string.Empty;
                                }
                            }
                        }
                    }
                }
                if (sParentURIPattern != string.Empty)
                {
                    uri.URIDataManager.ParentURIPattern = sParentURIPattern;
                }
            }
        }
        public static void AdjustForDefaultLinkedView(ContentURI uri,
            List<ContentURI> linkedViews)
        {
            if (uri.URIDataManager.UseDefaultAddIn)
            {
                ContentURI defaultLinkedView = linkedViews.FirstOrDefault(
                    c => c.URIDataManager.IsDefault == true);
                linkedViews.Clear();
                linkedViews.Add(defaultLinkedView);
            }
            else if (uri.URIDataManager.UseDefaultLocal)
            {
                ContentURI defaultLinkedView = linkedViews.FirstOrDefault(
                    c => c.URIDataManager.IsDefault == true);
                linkedViews.Clear();
                linkedViews.Add(defaultLinkedView);
            }
        }
        public static void AddLinkedViewStoriesToURI(ContentURI uri, 
            ContentURI selectedLinkedViewURI)
        {
            if (selectedLinkedViewURI.URIDataManager.LinkedView != null)
            {
                if (uri.URIDataManager.LinkedView == null)
                {
                    uri.URIDataManager.LinkedView 
                        = new List<System.Linq.IGrouping<int, ContentURI>>();
                }
                foreach (var linkedviewparent in selectedLinkedViewURI.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URINodeName
                            == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                            && AddInHelper.IsAddIn(linkedview) == false)
                        {
                            AddURIToLinkedView(linkedview, uri.URIDataManager.LinkedView);
                        }
                    }
                }
            }
        }
    }
}