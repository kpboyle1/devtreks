using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DevTreks.Data.EditHelpers
{
    /// <summary>
    ///Purpose:		linq to xml utility methods
    ///Author:		www.devtreks.org
    ///Date:		2013, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class XmlLinq
    {
        public static XElement GetRootXmlDoc()
        {
            XElement root
                = new XElement(Helpers.GeneralHelpers.ROOT_PATH
                );
            return root;
        }
        public static string GetElementAttributeValue(XElement element, 
            string attName)
        {
            string sAttValue = string.Empty;
            if (element != null && !string.IsNullOrEmpty(attName))
            {
                string sAttName = attName.Trim();
                if (element.HasAttributes)
                {
                    XAttribute att = element.Attribute(sAttName);
                    if (att != null)
                    {
                        sAttValue = att.Value;
                    }
                }
            }
            return sAttValue;
        }
        
        public static XElement GetElementUsingAttribute(IEnumerable<XElement> elements,
            string attName, string attValue)
        {
            XElement el = null;
            if (!string.IsNullOrEmpty(attName)
                && elements != null)
            {
                if (elements
                    .Any(p => (string)p.Attribute(attName)!= null))
                {
                    el = elements
                    .FirstOrDefault(p => (string)p.Attribute(attName) == attValue);
                }
            }
            return el;
        }
        public static XElement GetElementUsingAttributeWithBlankAttribute(IEnumerable<XElement> elements,
            string attName, string attValue, string attBlankName)
        {
            XElement el = null;
            if (!string.IsNullOrEmpty(attName)
                && elements != null)
            {
                if (elements
                    .Any(p => (string)p.Attribute(attName) != null))
                {
                    el = elements
                    .FirstOrDefault(p => (string)p.Attribute(attName) == attValue
                    && ((string)p.Attribute(attBlankName) == string.Empty 
                    || (string)p.Attribute(attBlankName) == Helpers.GeneralHelpers.NONE
                    || (string)p.Attribute(attBlankName) == null));
                }
            }
            return el;
        }
        public static XElement GetElementUsingHighestId(IEnumerable<XElement> elements)
        {
            XElement el = null;
            if (elements != null)
            {
                //order by sorts by ascending, so need last
                el = elements
                        .OrderBy(c => (string)c.Attribute(AppHelpers.Calculator.cId))
                        .LastOrDefault();
            }
            return el;
        }
        public static XElement GetElement(XElement root, string nodeName,
            string id)
        {
            XElement el = null;
            XNamespace y0 = GetNamespaceForNode(root, nodeName);
            bool bDescendantExists = false;
            if (y0 == null)
            {
                bDescendantExists = DescendantExists(root, nodeName, id);
                if (bDescendantExists)
                {
                    el = root
                        .Descendants(
                            nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == id);
                }
            }
            else
            {
                bDescendantExists = DescendantExistsWithNS(y0, root, nodeName, id);
                if (bDescendantExists)
                {
                    el = root
                        .Descendants(
                            y0 + nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == id);
                }
            }
            return el;
        }
        public static XElement GetChildElement(XElement root, string childNodeName,
            string childId, string parentNodeName, string parentId)
        {
            XElement el = null;
            XNamespace y0 = GetNamespaceForNode(root, childNodeName);
            bool bDescendantExists = false;
            if (y0 == null)
            {
                bDescendantExists = ParentExists(root, childNodeName, childId,
                    parentNodeName, parentId);
                if (bDescendantExists)
                {
                    root
                        .Descendants(parentNodeName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Descendants(childNodeName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == childId);
                }
            }
            else
            {
                bDescendantExists = ParentExistsWithNS(y0, root, childNodeName, 
                    childId, AppHelpers.Calculator.cId, parentNodeName, parentId);
                if (bDescendantExists)
                {
                    root
                         .Descendants(y0 + parentNodeName)
                         .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                             == parentId)
                         .Descendants(childNodeName)
                         .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                             == childId);
                }
            }
            return el;
        }
        public static XElement GetChildElement(XElement root, string nodeName,
            string id)
        {
            XElement el = null;
            XNamespace y0 = GetNamespaceForNode(root, nodeName);
            bool bDescendantExists = false;
            if (y0 == null)
            {
                bDescendantExists = DescendantExists(root, nodeName, id);
                if (bDescendantExists)
                {
                    if (id == string.Empty)
                    {
                        el = root
                            .Elements(
                                nodeName).FirstOrDefault(
                                p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == id);
                    }
                    else
                    {
                        el = root
                            .Elements(
                                nodeName).FirstOrDefault();
                    }
                }
            }
            else
            {
                bDescendantExists = DescendantExistsWithNS(y0, root, nodeName, id);
                if (bDescendantExists)
                {
                    if (id == string.Empty)
                    {
                        el = root
                            .Elements(
                                y0 + nodeName).FirstOrDefault(
                                p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == id);
                    }
                    else
                    {
                        el = root
                            .Elements(
                                y0 + nodeName).FirstOrDefault();
                    }
                }
            }
            return el;
        }
        public static string GetRootElementFirstChildNodeName(XElement root)
        {
            string sNodeName = string.Empty;
            if (root.HasElements)
            {
                sNodeName = root.Elements().First().Name.LocalName;
            }
            return sNodeName;
        }
        public static bool UpdateElementUsingURIToAdd(XNamespace y0,
            XElement root, EditHelper.ArgumentsEdits addsArguments)
        {
            bool bSuccess = false;
            if (y0 == null)
            {
                bool bHasElement = DescendantExists(
                    root, addsArguments.URIToAdd);
                if (bHasElement)
                {
                    root
                    .Descendants(
                        addsArguments.URIToAdd.URINodeName).FirstOrDefault(
                        p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == addsArguments.URIToAdd.URIId.ToString())
                    .SetAttributeValue(addsArguments.EditAttName, 
                        addsArguments.EditAttValue);
                    bSuccess = true;
                }
            }
            else
            {
                bool bHasElement = DescendantExistsWithNS(y0,
                    root, addsArguments.URIToAdd);
                if (bHasElement)
                {
                    root
                        .Descendants(
                            y0 + addsArguments.URIToAdd.URINodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == addsArguments.URIToAdd.URIId.ToString())
                        .SetAttributeValue(addsArguments.EditAttName, 
                            addsArguments.EditAttValue);
                    bSuccess = true;
                }
            }
            return bSuccess;
        }
        public static bool DeleteLinkedViewUsingURIToAddandURIToEdit(XNamespace y0,
            XElement root, EditHelper.ArgumentsEdits addsArguments)
        {
            bool bSuccess = false;
            if (y0 == null)
            {
                XElement parentEl = GetElement(root, addsArguments.URIToEdit.URINodeName,
                    addsArguments.URIToEdit.URIId.ToString());
                if (parentEl != null)
                {
                    y0 = GetNamespaceForNode(
                        parentEl, parentEl.Name.LocalName);
                    if (y0 != null)
                    {
                        bSuccess = DeleteLinkedViewUsingURIToAddandURIToEditNS(y0,
                            parentEl, addsArguments);
                    }
                    else
                    {
                        bool bDescendantExists = DescendantExists(parentEl,
                            addsArguments.URIToAdd);
                        if (bDescendantExists)
                        {
                            bool bHasSiblings = HasSiblingLinkedView(parentEl,
                                addsArguments.URIToAdd.URIId.ToString());
                            if (bHasSiblings)
                            {
                                root
                                .Descendants(addsArguments.URIToEdit.URINodeName)
                                .FirstOrDefault(
                                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == addsArguments.URIToEdit.URIId.ToString())
                                .Descendants(addsArguments.URIToAdd.URINodeName)
                                .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == addsArguments.URIToAdd.URIId.ToString())
                                .Remove();
                                bSuccess = true;
                            }
                            else
                            {
                                //get rid of <xmldoc><root> nodes
                                root
                                .Descendants(addsArguments.URIToEdit.URINodeName)
                                .FirstOrDefault(
                                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == addsArguments.URIToEdit.URIId.ToString())
                                .Descendants(addsArguments.URIToAdd.URINodeName)
                                .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == addsArguments.URIToAdd.URIId.ToString())
                                .Parent
                                .Parent
                                .Remove();
                                bSuccess = true;
                            }
                        }
                    }
                }
            }
            else
            {
                XElement parentEl = GetElement(root, addsArguments.URIToEdit.URINodeName,
                    addsArguments.URIToEdit.URIId.ToString());
                bSuccess = DeleteLinkedViewUsingURIToAddandURIToEditNS(y0,
                    parentEl, addsArguments);
            }
            return bSuccess;
        }
        private static bool DeleteLinkedViewUsingURIToAddandURIToEditNS(XNamespace y0,
            XElement parentEl, EditHelper.ArgumentsEdits addsArguments)
        {
            bool bSuccess = false;
            if (parentEl != null)
            {
                //the namespace is not needed here
                bool bDescendantExists = DescendantExists(parentEl,
                    addsArguments.URIToAdd);
                if (bDescendantExists)
                {
                    bool bHasSiblings = HasSiblingLinkedView(parentEl,
                        addsArguments.URIToAdd.URIId.ToString());
                    if (bHasSiblings)
                    {
                        parentEl
                        .Descendants(y0 + addsArguments.URIToEdit.URINodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == addsArguments.URIToEdit.URIId.ToString())
                        .Descendants(addsArguments.URIToAdd.URINodeName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == addsArguments.URIToAdd.URIId.ToString())
                        .Remove();
                        bSuccess = true;
                    }
                    else
                    {
                        //get rid of <xmldoc><root> nodes
                        parentEl
                        .Descendants(y0 + addsArguments.URIToEdit.URINodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == addsArguments.URIToEdit.URIId.ToString())
                        .Descendants(addsArguments.URIToAdd.URINodeName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == addsArguments.URIToAdd.URIId.ToString())
                        .Parent
                        .Parent
                        .Remove();
                        bSuccess = true;
                    }
                }
            }
            return bSuccess;
        }
        public static bool DeleteElementUsingURIToAdd(XNamespace y0,
            XElement root, EditHelper.ArgumentsEdits addsArguments)
        {
            bool bSuccess = false;
            if (y0 == null)
            {
                bool bHasElement = DescendantExists(
                    root, addsArguments.URIToAdd);
                if (bHasElement)
                {
                    root
                    .Descendants(addsArguments.URIToAdd.URINodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == addsArguments.URIToAdd.URIId.ToString())
                    .Remove();
                    bSuccess = true;
                }
            }
            else
            {
                bool bHasElement = DescendantExistsWithNS(y0,
                    root, addsArguments.URIToAdd);
                if (bHasElement)
                {
                    root
                        .Descendants(
                            y0 + addsArguments.URIToAdd.URINodeName)
                            .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == addsArguments.URIToAdd.URIId.ToString())
                        .Remove();
                    bSuccess = true;
                }
            }
            return bSuccess;
        }
        public static bool ReplaceElementInRootDoc(
            XElement replacementElement, ref XElement root)
        {
            bool bSuccess = false;
            string sId = GetAttributeValue(replacementElement, AppHelpers.Calculator.cId);
            XNamespace y0 = GetNamespaceForNode(root, replacementElement.Name.LocalName);
            if (y0 == null)
            {
                bool bHasElement = DescendantExists(root, 
                    replacementElement.Name.LocalName, sId);
                if (bHasElement)
                {
                    root
                    .Descendants(replacementElement.Name.LocalName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == sId)
                    .ReplaceWith(replacementElement);
                    bSuccess = true;
                }
            }
            else
            {
                bool bHasElement = DescendantExistsWithNS(y0,
                    root, replacementElement.Name.LocalName, sId);
                if (bHasElement)
                {
                    root
                        .Descendants(y0 + replacementElement.Name.LocalName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == sId)
                        .ReplaceWith(replacementElement);
                    bSuccess = true;
                }
            }
            return bSuccess;
        }
        public static bool ReplaceElementInRootDoc(
            XElement replacementElement,
            ref XElement root, string idToReplace)
        {
            bool bSuccess = false;
            XNamespace y0 = GetNamespaceForNode(root, replacementElement.Name.LocalName);
            if (y0 == null)
            {
                bool bHasElement = DescendantExists(root,
                    replacementElement.Name.LocalName, idToReplace);
                if (bHasElement)
                {
                    root
                    .Descendants(replacementElement.Name.LocalName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == idToReplace)
                    .ReplaceWith(replacementElement);
                    bSuccess = true;
                }
            }
            else
            {
                bool bHasElement = DescendantExistsWithNS(y0,
                    root, replacementElement.Name.LocalName, idToReplace);
                if (bHasElement)
                {
                    root
                        .Descendants(y0 + replacementElement.Name.LocalName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == idToReplace)
                        .ReplaceWith(replacementElement);
                    bSuccess = true;
                }
            }
            return bSuccess;
        }
        private static bool ReplaceChangedElementInChildLinkedView(
            XElement linkedViewElement, string linkedViewId, ref XElement root, 
            ref bool isSameElement, out XElement replacedLinkedView)
        {
            bool bSuccess = false;
            replacedLinkedView = null;
            if (!string.IsNullOrEmpty(linkedViewId))
            {
                XNamespace y0 = GetNamespaceForNode(root,
                    linkedViewElement.Name.LocalName);
                if (y0 == null)
                {
                    //verify that the child linkedview exists
                    bool bHasElement = LinkedViewChildExists(root,
                        AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                        AppHelpers.Calculator.cId, linkedViewId);
                    if (bHasElement)
                    {
                        //don't replace nodes that have no changes
                        XElement nodeToReplace = root
                            .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                            .Descendants(linkedViewElement.Name.LocalName)
                            .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == linkedViewId);
                        if (nodeToReplace.ToString().Equals(linkedViewElement.ToString())
                            == false)
                        {
                            string sExistingLinkedViewId
                                = GetAttributeValue(linkedViewElement,
                                AppHelpers.Calculator.cId);
                            if (sExistingLinkedViewId == linkedViewId)
                            {
                                root
                                   .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                                   .Descendants(linkedViewElement.Name.LocalName)
                                   .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                       == linkedViewId)
                                   .ReplaceWith(linkedViewElement);
                                AddLinkedViewToNullElement(linkedViewElement,
                                    out replacedLinkedView);
                                bSuccess = true;
                            }
                            else
                            {
                                //use a new replacement el
                                XElement newReplacement
                                    = new XElement(linkedViewElement);
                                newReplacement.SetAttributeValue(
                                    AppHelpers.Calculator.cId, linkedViewId);
                                if (nodeToReplace.ToString().Equals(newReplacement.ToString())
                                    == false)
                                {
                                    //replace
                                    root
                                    .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                                    .Descendants(linkedViewElement.Name.LocalName)
                                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                           == linkedViewId)
                                    .ReplaceWith(newReplacement);
                                    AddLinkedViewToNullElement(newReplacement,
                                        out replacedLinkedView);
                                    bSuccess = true;
                                }
                                else
                                {
                                    isSameElement = true;
                                }
                            }
                        }
                        else
                        {
                            isSameElement = true;
                        }
                    }
                    //calling procedure replaces whole xmldoc
                }
                else
                {
                    //should not be possible with current version
                }
            }
            return bSuccess;
        }
        
        public static void AddLinkedViewToNullElement(XElement linkedViewElement,
            out XElement replacedLinkedView)
        {
            replacedLinkedView = null;
            if (linkedViewElement != null)
            {
                if (replacedLinkedView == null)
                {
                    replacedLinkedView
                        = new XElement(Helpers.GeneralHelpers.ROOT_PATH,
                            new XElement(linkedViewElement)
                        );
                }
            }
        }
        public static bool LinkedViewExists(
            XElement linkedViewElement, string childNodeName,
            string childAttName, XElement root)
        {
            bool bLinkedViewExists = false;
            string sChildAttValue = GetAttributeValue(linkedViewElement, childAttName);
            XNamespace y0 = GetNamespaceForNode(root, linkedViewElement.Name.LocalName);
            if (y0 == null)
            {
                //verify that the child exists
                bool bHasElement = ChildElementExists(root, childNodeName,
                    string.Empty, childAttName);
                if (bHasElement)
                {
                    if (childAttName == AppHelpers.Calculator.cId)
                    {
                        //verify that the child's descendant exists
                        bLinkedViewExists = DescendantExists(root.Element(childNodeName),
                            AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                            childAttName, sChildAttValue);
                    }
                    else
                    {
                        //fileextensiontype must also be checked
                        string sFileExtType = GetAttributeValue(linkedViewElement,
                            AppHelpers.Calculator.cFileExtensionType);
                        bLinkedViewExists = root
                            .Element(childNodeName)
                            .Descendants(linkedViewElement.Name.LocalName)
                            .Any(p => (string)p.Attribute(childAttName)
                                == sChildAttValue 
                                && (string)p.Attribute(AppHelpers.Calculator.cFileExtensionType)
                                == sFileExtType);
                    }
                }
            }
            else
            {
                //should not be possible with current version
            }
            return bLinkedViewExists;
        }
        
        public static XElement GetDescendantUsingURIPattern(
            XElement root, string uriPattern)
        {
            XElement descEl = null;
            string sNodeName = ContentURI.GetURIPatternPart(uriPattern, 
                ContentURI.URIPATTERNPART.node);
            string sId = ContentURI.GetURIPatternPart(uriPattern, 
                ContentURI.URIPATTERNPART.id);
            XNamespace y0 = GetNamespaceForNode(root, sNodeName);
            if (y0 == null)
            {
                if (root
                    .Descendants(sNodeName)
                    .Any(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                        == sId))
                {
                    descEl = root
                    .Descendants(sNodeName)
                    .FirstOrDefault(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                        == sId);
                }
            }
            else
            {
                if (root
                    .Descendants(y0 + sNodeName)
                    .Any(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                        == sId))
                {
                    descEl = root
                    .Descendants(y0 + sNodeName)
                    .FirstOrDefault(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                        == sId);
                }
            }
            return descEl;
        }
        public static XElement GetElementUsingURIToAdd(XNamespace y0,
            XElement root, EditHelper.ArgumentsEdits addsArguments)
        {
            XElement descEl = null;
            if (y0 == null)
            {
                bool bHasElement = DescendantExists(
                    root, addsArguments.URIToAdd);
                if (bHasElement)
                {
                    descEl = root
                    .Descendants(addsArguments.URIToAdd.URINodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == addsArguments.URIToAdd.URIId.ToString());
                }
            }
            else
            {
                bool bHasElement = DescendantExistsWithNS(y0,
                    root, addsArguments.URIToAdd);
                if (bHasElement)
                {
                    descEl = root
                        .Descendants(y0 + addsArguments.URIToAdd.URINodeName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == addsArguments.URIToAdd.URIId.ToString());
                }
            }
            return descEl;
        }
        public static void AppendToLastParentElement(ref XElement elBaseDoc,
            ref string parentNodeName, XElement elToAdd)
        {
            XElement lastParentEl =
                (from el in elBaseDoc.Descendants(parentNodeName)
                 select el).LastOrDefault();
            if (lastParentEl != null)
            {
                lastParentEl.Add(elToAdd);
            }
            else
            {
                elBaseDoc.Add(elToAdd);
            }
            parentNodeName = elToAdd.Name.ToString();
        }
        public static bool ChildExists(XElement parentEl, 
            ContentURI childToCheckURI)
        {
            bool bChildExists = parentEl
                .Elements(childToCheckURI.URINodeName)
                .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                == childToCheckURI.URIId.ToString());
            return bChildExists;
        }
        private static bool ChildExistsWithNS(XNamespace y0,
            EditHelper.ArgumentsEdits addsArguments,
            string nodeName, string nodeId, XElement root)
        {
            bool bChildExists = false;
            //see if the sqlxml root namespace is needed (y0:)
            bChildExists = root
                .Elements(y0 + nodeName)
                .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == nodeId);
            if (bChildExists == false
                && nodeName == AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                y0 = AppHelpers.Agreement.AGREEMENTS_NAMESPACE.Replace("'", string.Empty);
                bChildExists = root
                    .Elements(y0 + nodeName)
                    .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == nodeId);
            }
            return bChildExists;
        }
        public static bool ChildExistsOfParentURI(
            EditHelper.ArgumentsEdits addsArguments, XElement root,
            ContentURI parentURI, string groupingElementName)
        {
            bool bChildExists = false;
            if (groupingElementName == string.Empty)
            {
                //use chaining to prevent memory use
                bChildExists = root
                    .Descendants(parentURI.URINodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentURI.URIId.ToString())
                    .Elements(addsArguments.URIToAdd.URINodeName)
                    .Any(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                        == addsArguments.URIToAdd.URIId.ToString());
            }
            else
            {
                //use chaining to prevent memory use
                bChildExists = root
                    .Descendants(parentURI.URINodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentURI.URIId.ToString())
                    .Elements(groupingElementName)
                    .FirstOrDefault()
                    .Elements(addsArguments.URIToAdd.URINodeName)
                    .Any(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                        == addsArguments.URIToAdd.URIId.ToString());
            }
            return bChildExists;
        }
        public static bool ChildExistsOfParentURIWithNS(XNamespace y0,
            EditHelper.ArgumentsEdits addsArguments, XElement root,
            ContentURI parentURI, string groupingElementName)
        {
            bool bChildExists = false;
            //see if the sqlxml root namespace is needed (y0:)
            if (groupingElementName == string.Empty)
            {
                //use chaining to prevent memory use
                bChildExists = root
                    .Descendants(y0 + parentURI.URINodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentURI.URIId.ToString())
                    .Elements(addsArguments.URIToAdd.URINodeName)
                    .Any(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                        == addsArguments.URIToAdd.URIId.ToString());
            }
            else
            {
                //use chaining to prevent memory use
                bChildExists = root
                    .Descendants(y0 + parentURI.URINodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentURI.URIId.ToString())
                    .Elements(groupingElementName)
                    .FirstOrDefault()
                    .Elements(addsArguments.URIToAdd.URINodeName)
                    .Any(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                        == addsArguments.URIToAdd.URIId.ToString());
            }
            return bChildExists;
        }
        public static bool ChildElementExists(XElement parentEl,
            string childNodeName, string childAttValue, string childAttName)
        {
            bool bChildExists = false;
            XNamespace y0 = GetNamespaceForNode(parentEl, childNodeName);
            if (y0 == null)
            {
                if (childAttValue == string.Empty)
                {
                    bChildExists = parentEl
                        .Elements(childNodeName)
                        .Any();
                }
                else
                {
                    bChildExists = parentEl
                        .Elements(childNodeName)
                        .Any(p => (string)p.Attribute(childAttName)
                            == childAttValue);
                }
            }
            else
            {
                if (childAttValue == string.Empty)
                {
                    bChildExists = parentEl.Elements(
                       y0 + childNodeName).Any(
                       p => (string)p.Attribute(childAttName)
                       == childAttValue);
                }
                else
                {
                    bChildExists = parentEl.Elements(
                       y0 + childNodeName).Any(
                       p => (string)p.Attribute(childAttName)
                       == childAttValue);
                }
            }
            return bChildExists;
        }
        public static string GetElementIdUsingSiblingAttribute(XElement root,
            string nodeName, string attName, string attValue)
        {
            string sId = string.Empty;
            bool bHasElement = root
                .Descendants()
                .Any(d => d.Name.LocalName == nodeName
                    && (string) d.Attribute(attName) == attValue);
            if (bHasElement)
            {
                sId = root
                .Descendants()
                .FirstOrDefault(d => d.Name.LocalName == nodeName
                    && (string)d.Attribute(attName) == attValue)
                .Attribute(AppHelpers.Calculator.cId).Value;
            }
            return sId;
        }
        public static string GetElementIdUsingParentAndSibling(XElement root,
            string parentId, string parentNodeName, string nodeName, 
            string attName, string attValue)
        {
            string sId = string.Empty;
            bool bParentExists = ParentExists(root, nodeName,
                attValue, attName, parentNodeName, parentId);
            if (bParentExists)
            {
                //note that the descendants syntax should take care of grouping nodes
                sId = root
                    .Descendants(parentNodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentId)
                    .Descendants(nodeName)
                    .FirstOrDefault(p => (string)p.Attribute(attName)
                        == attValue)
                    .Attribute(AppHelpers.Calculator.cId)
                    .Value;
            }
            return sId;
        }
        
        public static bool DescendantExists(XElement parentEl,
            ContentURI descendantToCheckURI)
        {
            bool bDescendantExists = 
                parentEl
                .Descendants(descendantToCheckURI.URINodeName)
                .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == descendantToCheckURI.URIId.ToString());
            return bDescendantExists;
        }
        public static bool DescendantExists(XElement parentEl,
            string descendantNodeName, string descendantId)
        {
            bool bDescendantExists = false;
            if (!string.IsNullOrEmpty(descendantNodeName)
                && !string.IsNullOrEmpty(descendantId))
            {
                bDescendantExists = 
                    parentEl
                    .Descendants(descendantNodeName)
                    .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == descendantId);
            }
            return bDescendantExists;
        }
        public static bool DescendantExists(XElement parentEl,
            string descendantNodeName, string descendantAttributeName,
            string descendantAttributeValue)
        {
            bool bDescendantExists = false;
            if (!string.IsNullOrEmpty(descendantAttributeName)
                && !string.IsNullOrEmpty(descendantAttributeValue)
                && parentEl != null)
            {
                bDescendantExists = parentEl.Descendants(
                    descendantNodeName).Any(
                    p => (string)p.Attribute(descendantAttributeName)
                    == descendantAttributeValue);
            }
            return bDescendantExists;
        }
        public static bool LinkedViewChildExists(XElement parentEl,
            string descendantNodeName, string descendantAttributeName,
            string descendantAttributeValue)
        {
            bool bDescendantExists = false;
            if (!string.IsNullOrEmpty(descendantAttributeName)
                && !string.IsNullOrEmpty(descendantAttributeValue)
                && parentEl != null)
            {
                if (parentEl
                .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                .Any())
                {
                    bDescendantExists = 
                        parentEl
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                        .Descendants(descendantNodeName)
                        .Any(
                            p => (string)p.Attribute(descendantAttributeName)
                            == descendantAttributeValue);
                }
            }
            return bDescendantExists;
        }
        public static bool DescendantExistsWithNS(XNamespace y0,
           XElement parentEl, ContentURI descendantToCheckURI)
        {
            bool bDescendantExists = false;
            //see if the sqlxml root namespace is needed (y0:)
            bDescendantExists = parentEl
                .Descendants(y0 + descendantToCheckURI.URINodeName)
                .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == descendantToCheckURI.URIId.ToString());
            //applicable with inserts but not edits
            if (bDescendantExists == false
                && descendantToCheckURI.URINodeName 
                == AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                y0 = AppHelpers.Agreement.AGREEMENTS_NAMESPACE.Replace("'", string.Empty);
                bDescendantExists = parentEl
                    .Descendants(y0 + descendantToCheckURI.URINodeName)
                    .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == descendantToCheckURI.URIId.ToString());
            }
            return bDescendantExists;
        }
        public static bool DescendantExistsWithNS(XNamespace y0,
          XElement parentEl, string descendantNodeName, string descendantId)
        {
            bool bDescendantExists = false;
            //see if the sqlxml root namespace is needed (y0:)
            bDescendantExists = parentEl.Descendants(
                y0 + descendantNodeName).Any(
                p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == descendantId);
            return bDescendantExists;
        }
        public static XElement GetCurrentElementWithLinkedView(
            ContentURI uri, string xmlDocPath,
            string currentURIPattern)
        {
            XElement currentElement = null;
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                xmlDocPath))
            {
                string sCurrentNodeName 
                    = ContentURI.GetURIPatternPart(currentURIPattern, ContentURI.URIPATTERNPART.node);
                string sId
                    = ContentURI.GetURIPatternPart(currentURIPattern, ContentURI.URIPATTERNPART.id);
                //use streaming techniques to reduce memory footprint
                XmlReader reader = Helpers.FileStorageIO.GetXmlReader(uri, xmlDocPath);
                if (reader != null)
                {
                    using (reader)
                    {
                        while (reader.ReadToFollowing(sCurrentNodeName))
                        {
                            string sCurrentId = reader
                                .GetAttribute(AppHelpers.Calculator.cId);
                            if (sCurrentId == sId)
                            {
                                currentElement
                                    = GetCurrentElementWithAttributes(reader);
                                if (currentElement != null)
                                {
                                    AddLinkedViewToCurrentElement(
                                        reader, ref currentElement);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return currentElement;
        }
        public static XElement GetCurrentElementWithAttributes(XmlReader reader)
        {
            XElement currentElement = null;
            if (string.IsNullOrEmpty(reader.Prefix))
            {
                currentElement = new XElement(reader.Name);
            }
            else
            {
                XNamespace y0 = GetDevTreksNamespace();
                currentElement = new XElement(y0 + reader.LocalName,
                     new XAttribute(XNamespace.Xmlns + "y0", y0));
            }
            //this should be in an xmlreader helper class
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    if (string.IsNullOrEmpty(reader.Prefix))
                    {
                        currentElement.SetAttributeValue(reader.Name,
                            reader.Value);
                    }
                    else
                    {
                        //ignore xmlns: in current version
                    }
                }
                //move the reader back to the element node
                reader.MoveToElement();
            }
            return currentElement;
        }
        public static void AddLinkedViewToCurrentElementWithReader(
            ContentURI uri, string xmlDocPath, ref XElement currentElement)
        {
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                xmlDocPath))
            {
                string sId = GetAttributeValue(currentElement, 
                    AppHelpers.Calculator.cId);
                if (!string.IsNullOrEmpty(sId))
                {
                    XmlReader reader = Helpers.FileStorageIO.GetXmlReader(uri, xmlDocPath);
                    if (reader != null)
                    {
                        using (reader)
                        {
                            while (reader.ReadToFollowing(currentElement.Name.LocalName))
                            {
                                string sCurrentId = reader
                                    .GetAttribute(AppHelpers.Calculator.cId);
                                if (sCurrentId == sId)
                                {
                                    AddLinkedViewToCurrentElement(
                                        reader, ref currentElement);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void AddLinkedViewToCurrentElement(XmlReader reader, 
            ref XElement currentElement)
        {
            //now return add any linkedviews
            if (reader.Read())
            {
                if (reader.NodeType
                    == XmlNodeType.Element)
                {
                    if (reader.Name
                        == Helpers.GeneralHelpers.ROOT_PATH)
                    {
                        //attach calculators to parents
                        XElement xmlDocElement = XElement.Load(
                            reader.ReadSubtree());
                        currentElement.AddFirst(xmlDocElement);
                        //check siblings
                        while (reader
                           .ReadToNextSibling(Helpers.GeneralHelpers.ROOT_PATH))
                        {
                            if (reader.NodeType
                                == XmlNodeType.Element)
                            {
                                XElement sibDocElement = XElement.Load(
                                    reader.ReadSubtree());
                                currentElement.Add(sibDocElement);
                            }
                        }
                    }
                    else
                    {
                        ////0.8.7 check siblings (could be on a descendant node out of order)
                        //while (reader
                        //   .ReadToNextSibling(Helpers.GeneralHelpers.ROOT_PATH))
                        //{
                        //    if (reader.NodeType
                        //        == XmlNodeType.Element)
                        //    {
                        //        XElement sibDocElement = XElement.Load(
                        //            reader.ReadSubtree());
                        //        currentElement.Add(sibDocElement);
                        //    }
                        //}
                    }
                }
            }
        }
        public static XNamespace GetDevTreksNamespace()
        {
            XNamespace y0 = DevTreks.Data.AppHelpers.General.y0;
            return y0;
        }
        public static XNamespace GetNamespaceForNode(
            XElement root, string nodeNameContainingNS)
        {
            XNamespace y0 = null;
            //by sqlxml convention, namespaces always are associated 
            //with first node, which can be root.firstnode, or in the case
            //of single node Xlements, root
            y0 = root.Name.Namespace;
            if (string.IsNullOrEmpty(y0.ToString()))
            {
                if (root.HasElements)
                {
                    y0 = root.Elements().FirstOrDefault().Name.Namespace;
                    if (!string.IsNullOrEmpty(y0.ToString()))
                    {
                        if (root.Elements().FirstOrDefault().Name.LocalName 
                            != nodeNameContainingNS)
                        {
                            //has a namespace, but its not associated with nodename
                            return null;
                        }
                    }
                }
            }
            else
            {
                if (root.Name.LocalName != nodeNameContainingNS)
                {
                    //has a namespace, but its not associated with nodename
                    return null;
                }
            }
            if (string.IsNullOrEmpty(y0.ToString()))
            {
                return null;
            }
            return y0;
        }
        public static bool ParentExists(XElement root, string childNodeName,
           string nodeId, string parentNodeName, string parentId)
        {
            bool bParentExists = false;
            bool bHasParent = DescendantExists(root, parentNodeName, parentId);
            bool bHasChild  = DescendantExists(root, childNodeName, nodeId);
            if (bHasParent && bHasChild)
            {
                bParentExists = root
                    .Descendants(parentNodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentId)
                    .Descendants(childNodeName)
                    .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == nodeId);
            }
            return bParentExists;
        }
        public static bool ParentOfNodeTypeExists(XElement root, string childNodeName,
            string parentNodeName, string parentId)
        {
            bool bParentExists = false;
            bool bHasParent = DescendantExists(root, parentNodeName, parentId);
            if (bHasParent)
            {
                bParentExists = root
                    .Descendants(parentNodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentId)
                    .Descendants(childNodeName)
                    .Any();
            }
            return bParentExists;
        }
        public static bool ParentOfNodeTypeExistsWithNS(XNamespace y0,
            XElement root, string childNodeName, string parentNodeName, 
            string parentId)
        {
            bool bParentExists = false;
            bool bHasParent = DescendantExistsWithNS(y0, root, parentNodeName, parentId);
            if (bHasParent)
            {
                bParentExists = root
                    .Descendants(y0 + parentNodeName)
                    .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentId)
                    .Descendants(childNodeName)
                    .Any();
            }
            return bParentExists;
        }
        public static bool ParentExists(XElement root, string childNodeName,
            string childAttValue, string childAttributeName, 
            string parentNodeName, string parentId)
        {
            bool bParentExists = false;
            bool bHasParent = DescendantExists(root, parentNodeName, parentId);
            if (bHasParent)
            {
                bool bHasNodeChildren = ParentOfNodeTypeExists(root, childNodeName,
                    parentNodeName, parentId);
                if (bHasNodeChildren)
                {
                    bParentExists = root
                        .Descendants(parentNodeName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Descendants(childNodeName)
                        .Any(p => (string)p.Attribute(childAttributeName)
                            == childAttValue);
                }
            }
            return bParentExists;
        }
        public static bool ParentExistsWithNS(XNamespace y0,
            XElement root, string childNodeName,
            string childAttValue, string childAttributeName,
            string parentNodeName, string parentId)
        {
            bool bParentExists = false;
            bool bHasParent = DescendantExistsWithNS(y0, 
                root, parentNodeName, parentId);
            if (bHasParent)
            {
                bool bHasNodeChildren = ParentOfNodeTypeExistsWithNS(y0,
                    root, childNodeName, parentNodeName, parentId);
                if (bHasNodeChildren)
                {
                    bParentExists = root
                        .Descendants(y0 + parentNodeName)
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Descendants(childNodeName)
                        .Any(p => (string)p.Attribute(childAttributeName)
                            == childAttValue);
                }
            }
            return bParentExists;
        }
        public static void AddElementToRootIfUnique(
            EditHelper.ArgumentsEdits addsArguments, 
            XElement root, XElement elementToAdd)
        {
            //don't add the same ancestors 2x to root
            //XElement parent
            XNamespace y0 = GetNamespaceForNode(root,
                addsArguments.URIToAdd.URINodeName);
            bool bHasChild = false;
            if (y0 != null)
            {
                //see if the sqlxml root namespace is needed (y0:)
                bHasChild = ChildExistsWithNS(y0, addsArguments,
                    addsArguments.URIToAdd.URINodeName,
                    addsArguments.URIToAdd.URIId.ToString(),
                    root);
            }
            else
            {
                bHasChild = ChildExists(root, addsArguments.URIToAdd);
            }
            if (!bHasChild)
            {
                //root never has namespaces, so no need to check
                AddElementToParent(addsArguments, root, elementToAdd);
            }
        }
        public static bool AddElementToParentUsingURI(
            EditHelper.ArgumentsEdits addsArguments,
            XElement root, XElement elementToAdd, 
            string groupingElementName)
        {
            bool bHasInsertedNode = false;
            if (!string.IsNullOrEmpty(
                addsArguments.URIToAdd.URIDataManager.ParentURIPattern))
            {
                //MUST verify parent exists before trying to add a child
                ContentURI parentURI = new ContentURI(addsArguments.URIToAdd);
                parentURI.ChangeURIPattern(
                    addsArguments.URIToAdd.URIDataManager.ParentURIPattern);
                XNamespace y0 = GetNamespaceForNode(root,
                    parentURI.URINodeName);
                bool bParentExists = false;
                if (y0 != null)
                {
                    bParentExists =
                        DescendantExistsWithNS(y0, root, parentURI);
                }
                else
                {
                    bParentExists = DescendantExists(root, parentURI);
                }
                if (bParentExists)
                {
                    if (groupingElementName != string.Empty)
                    {
                        SetGroupingElement(root, parentURI, groupingElementName);
                    }
                    //reduce memory use by using chaining techniques to add to parent
                    AddElementToParentURIAndChangeSameId(addsArguments,
                        parentURI, y0,
                        root, elementToAdd, groupingElementName);
                    //v 1.3.1: this duplicates AddHelperLinq.SetSelectionsToAdd
                    //but the models need the former so this was removed
                    //AddMoreDefaultElements(addsArguments,
                    //    parentURI, y0,
                    //    root, elementToAdd, groupingElementName);
                    bHasInsertedNode = true;
                }
            }
            if (!bHasInsertedNode
                && addsArguments.ErrorMessage == string.Empty)
            {
                //generate an error msg
                addsArguments.ErrorMessage
                    = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                    "ADDS_NOANCESTORFOUND");
            }
            return bHasInsertedNode;
        }
        public static bool AppendElementUsingParentURIPattern(string parentElementURIPattern,
           string groupingElementName, XElement newElement, ref XElement root)
        {
            bool bHasInsertedNode = false;
            if (root.HasElements == false
                || parentElementURIPattern == string.Empty)
            {
                root.Add(newElement);
                bHasInsertedNode = true;
            }
            else
            {
                //make a parentURI, but don't hit db to set network connections
                string sNetworkName = ContentURI.GetURIPatternPart(parentElementURIPattern,
                    ContentURI.URIPATTERNPART.network);
                Network network = new Network();
                network.NetworkURIPartName = sNetworkName;
                ContentURI uri = new ContentURI();
                ContentURI parentURI = new ContentURI(parentElementURIPattern, network);
                XNamespace y0 = GetNamespaceForNode(root,
                    parentURI.URINodeName);
                //verify parent exists before trying to add a child
                bool bParentExists = false;
                if (y0 != null)
                {
                    bParentExists =
                        DescendantExistsWithNS(y0, root, parentURI);
                }
                else
                {
                    bParentExists = DescendantExists(root, parentURI);
                }
                if (bParentExists)
                {
                    if (groupingElementName != string.Empty)
                    {
                        SetGroupingElement(root, parentURI, groupingElementName);
                    }
                    if (y0 != null)
                    {
                        AddElementToParentURIWithNS(y0, parentURI,
                            root, newElement, groupingElementName);
                    }
                    else
                    {
                        AddElementToParentURI(parentURI,
                            root, newElement, groupingElementName);
                    }
                    bHasInsertedNode = true;
                }
            }
            return bHasInsertedNode;
        }

        private static bool SetGroupingElement(
            XElement root, ContentURI parentURI, 
            string groupingElementName)
        {
            bool bHasGroupingElement
                = root
                    .Descendants(parentURI.URINodeName).FirstOrDefault(
                        p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentURI.URIId.ToString())
                    .Elements(groupingElementName).Any();
            if (!bHasGroupingElement)
            {
                //needs a grouping el
                root
                    .Descendants(parentURI.URINodeName).FirstOrDefault(
                        p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == parentURI.URIId.ToString())
                    .Add(new XElement(groupingElementName));
                bHasGroupingElement = true;
            }
            return bHasGroupingElement;
        }
        public static void AddElementToParentURIAndChangeSameId(
            EditHelper.ArgumentsEdits addsArguments, 
            ContentURI parentURI, XNamespace y0,
            XElement root, XElement childEl, string groupingElementName)
        {
            if (childEl.Name
                == Helpers.GeneralHelpers.ROOT_PATH
                && childEl.HasElements)
            {
                //skip child els that start with <root> els
                XElement grandChildEl =
                    childEl.Element(addsArguments.URIToAdd.URINodeName);
                if (grandChildEl != null)
                {
                    ChangeSameIds(addsArguments, parentURI, y0,
                        root, grandChildEl, groupingElementName);
                    if (y0 == null)
                    {
                        AddElementToParentURIWithNS(y0, addsArguments, parentURI,
                            root, grandChildEl, groupingElementName);
                    }
                    else
                    {
                        AddElementToParentURI(addsArguments, parentURI,
                            root, grandChildEl, groupingElementName);
                    }
                    
                }
                else
                {
                    addsArguments.ErrorMessage
                    = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                        "ADDS_COULDNOTADDELEMENT");
                }
            }
            else
            {
                ChangeSameIds(addsArguments, parentURI, y0,
                    root, childEl, groupingElementName);
                if (y0 != null)
                {
                    AddElementToParentURIWithNS(y0, addsArguments, parentURI,
                        root, childEl, groupingElementName);
                }
                else
                {
                    AddElementToParentURI(addsArguments, parentURI,
                        root, childEl, groupingElementName);
                }
                
            }
        }
        private static void ChangeSameIds(
            EditHelper.ArgumentsEdits addsArguments, 
            ContentURI parentURI, XNamespace y0, 
            XElement root, XElement childEl, string groupingElementName)
        {
            //keep memory use down by not passing the parent el directly here
            //should verify that parentURI exists in root before coming here
            bool bHasChildrenWithSameIds = false;
            if (y0 != null)
            {
                bHasChildrenWithSameIds = ChildExistsOfParentURIWithNS(y0,
                   addsArguments, root, parentURI, groupingElementName);
            }
            else
            {
                 bHasChildrenWithSameIds = ChildExistsOfParentURI(
                    addsArguments, root, parentURI, groupingElementName);
            }
            if (bHasChildrenWithSameIds)
            {
                bool bIsBaseKey = false;
                string sChildIdAttName = string.Empty;
                string sNewId = AddRandomIdToElement(addsArguments, childEl);
                //keep the contenturis in synch with id changes
                string sNewURIPattern = ContentURI.ChangeURIPatternPart(
                    addsArguments.URIToAdd.URIPattern, ContentURI.URIPATTERNPART.id,
                    sNewId);
                addsArguments.URIToAdd.ChangeURIPattern(sNewURIPattern);
                //recurse to make sure this is a unique id
                ChangeSameIds(addsArguments, parentURI, y0, 
                    root, childEl, groupingElementName);
                //change children foreign keys
                if (childEl.HasElements)
                {
                    sChildIdAttName = EditHelper.GetForeignKeyName(
                        addsArguments.URIToEdit.URIDataManager.AppType,
                        addsArguments.URIToEdit.URIDataManager.SubAppType,
                        childEl.Name.LocalName, bIsBaseKey);
                    if (!string.IsNullOrEmpty(sChildIdAttName))
                    {
                        UpdateChildrenIds(childEl, sNewId, sChildIdAttName);
                    }
                }
            }
        }
        public static bool AddElementToParent(XElement root, XElement childEl, 
            string groupingElementName, string parentId, string parentNodeName)
        {
            bool bIsAdded = false;
            if (childEl != null)
            {
                bool bParentExists = DescendantExists(root, parentNodeName, parentId);
                if (bParentExists)
                {
                    if (groupingElementName == string.Empty)
                    {
                        root
                        .Descendants(parentNodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Add(childEl);
                        bIsAdded = true;
                    }
                    else
                    {
                        //could do some additional work to check for grouping node
                        root
                        .Descendants(parentNodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Elements(groupingElementName).FirstOrDefault()
                        .Add(childEl);
                        bIsAdded = true;
                    }
                }
            }
            return bIsAdded;
        }
        private static void AddElementToParentURI(
            EditHelper.ArgumentsEdits addsArguments, ContentURI parentURI,
            XElement root, XElement childEl, string groupingElementName)
        {
            //StartInsertGram(addsArguments, childEl);
            //keep memory use down by not passing the parent el directly here
            //should verify that parentURI exists in root before coming here
            if (groupingElementName == string.Empty)
            {
                root
                .Descendants(parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Add(childEl);
            }
            else
            {
                root
                .Descendants(parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Elements(groupingElementName).FirstOrDefault()
                .Add(childEl);
            }
            //FinishInsertGram(addsArguments, childEl);
        }
        private static void AddElementToParentURI(ContentURI parentURI,
            XElement root, XElement childEl, string groupingElementName)
        {
            if (groupingElementName == string.Empty)
            {
                root
                .Descendants(parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Add(childEl);
            }
            else
            {
                root
                .Descendants(parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Elements(groupingElementName).FirstOrDefault()
                .Add(childEl);
            }
        }
        private static void AddElementToParentURIWithNS(XNamespace y0,
            EditHelper.ArgumentsEdits addsArguments, ContentURI parentURI,
            XElement root, XElement childEl, string groupingElementName)
        {
            //StartInsertGram(addsArguments, childEl);
            //keep memory use down by not passing the parent el directly here
            //should verify that parentURI exists in root before coming here
            if (groupingElementName == string.Empty)
            {
                root
                .Descendants(y0 + parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Add(childEl);
            }
            else
            {
                root
                .Descendants(y0 + parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Elements(groupingElementName).FirstOrDefault()
                .Add(childEl);
            }
            //FinishInsertGram(addsArguments, childEl);
        }
        private static void AddElementToParentURIWithNS(XNamespace y0,
            ContentURI parentURI, XElement root, XElement childEl, 
            string groupingElementName)
        {
            //keep memory use down by not passing the parent el directly here
            //should verify that parentURI exists in root before coming here
            if (groupingElementName == string.Empty)
            {
                root
                .Descendants(y0 + parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Add(childEl);
            }
            else
            {
                root
                .Descendants(y0 + parentURI.URINodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                    == parentURI.URIId.ToString())
                .Elements(groupingElementName).FirstOrDefault()
                .Add(childEl);
            }
        }
        private static void AddMoreDefaultElements(
            EditHelper.ArgumentsEdits addsArguments, 
            ContentURI parentURI, XNamespace y0,
            XElement root, XElement elementToAdd, 
            string groupingElementName)
        {
            int iNumberToAdd 
                = Helpers.GeneralHelpers.ConvertStringToInt(addsArguments.NumberToAdd);
            if (iNumberToAdd > 1)
            {
                //add more default nodes
                //first has already been added
                for (int i = 1; i < iNumberToAdd; i++)
                {
                    //prevent byref complications by using a new element
                    XElement newEl = XElement.Parse(elementToAdd.ToString());
                    string sNewId = AddRandomIdToElement(addsArguments, newEl);
                    //keep the contenturis in synch with id changes
                    string sNewURIPattern = ContentURI.ChangeURIPatternPart(
                        addsArguments.URIToAdd.URIPattern, ContentURI.URIPATTERNPART.id,
                        sNewId);
                    AddElementToParentURIAndChangeSameId(addsArguments,
                        parentURI, y0, root, newEl, groupingElementName);
                }
            }
        }
        public static string AddRandomIdToElement(
            EditHelper.ArgumentsEdits addsArguments, XElement el)
        {
            if (addsArguments.RndGenerator == null)
            {
                //only reliable way to generate sequential random numbers
                //is to use the same number generator
                addsArguments.RndGenerator = new Random();
            }
            //use for default insertions (fast successive inserts break random generator)
            string sNewId
                = Helpers.GeneralHelpers.GetRandomInteger(addsArguments.RndGenerator).ToString();
            el.SetAttributeValue(AppHelpers.Calculator.cId, sNewId);
            return sNewId;
        }
        public static void UpdateChildrenIds(XElement parentEl,
            string parentId, string childAttName)
        {
            if (!string.IsNullOrEmpty(childAttName))
            {
                //skips grouping nodes and goes directly to descendants
                //child att names will be unique with the exception of
                //recursive nodes (refactor for devpack and linkedview)
                IEnumerable<XElement> lstSameChildIds
                    = from c in parentEl.Descendants()
                      where c.Attribute(childAttName) != null
                      select c;
                if (lstSameChildIds != null)
                {
                    if (lstSameChildIds.Count() > 0)
                    {
                        foreach (XElement sameChildIdEl in lstSameChildIds)
                        {
                            sameChildIdEl.SetAttributeValue(childAttName, parentId);
                        }
                    }
                }
            }
        }
        //private static void StartInsertGram(
        //    EditHelper.ArgumentsEdits addsArguments,
        //    XElement insertedEl)
        //{
        //    if (addsArguments.IsDbEdit == true
        //         && (!addsArguments.IsAncestorBeingAdded))
        //    {
        //        //prepare returnids prior to insert in case 
        //        //the ids have to be changed to make them unique
        //        AddHelperDb.InsertGramSetReturnIds(
        //            addsArguments, insertedEl);
        //    }
        //}
        //private static void FinishInsertGram(
        //    EditHelper.ArgumentsEdits addsArguments,
        //    XElement insertedEl)
        //{
        //    if (addsArguments.IsDbEdit
        //        && (!addsArguments.IsAncestorBeingAdded))
        //    {
        //        //write an updategram for the insertion
        //        AddHelperDb.InsertGramWrite(addsArguments,
        //            insertedEl);
        //    }
        //}
        private static void UpdateAddsArgumentsWithNewInsertion(
            EditHelper.ArgumentsEdits addsArguments,
            XElement insertedEl)
        {
            if (!addsArguments.IsAncestorBeingAdded)
            {
                //insertedEl.Atts can change (i.e. from changesameids)
                //update addsArguments.SelectionsToAdd[].URIPattern 
                //and addsArguments.SelectionsToAdd list to 
                //the new attributes so that the inserted element 
                //can be found during later db inserts
                addsArguments.URIToAdd.URIId
                    = Helpers.GeneralHelpers.ConvertStringToInt(
                    GetAttributeValue(insertedEl, AppHelpers.Calculator.cId));
                addsArguments.URIToAdd.URINodeName = insertedEl.Name.LocalName;
                addsArguments.URIToAdd.UpdateURIPattern();
                bool bHasUpdated = Helpers.LinqHelpers.UpdateURIPatternInList(
                    addsArguments.URIToAddOriginalURIPattern,
                    addsArguments.URIToAdd.URIPattern,
                    addsArguments.SelectionsToAdd);
                if (bHasUpdated == false)
                {
                    addsArguments.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                        "ADDS_MISSINGFROMSELECTIONSLIST");
                }
            }
        }
        private static XElement GetParentUsingNamespaces(
            EditHelper.ArgumentsEdits addsArguments,
            string nodeName, string nodeId, XElement root)
        {
            XElement parent = null;
            //see if the sqlxml root namespace is needed (y0:)
            XNamespace y0 = addsArguments.DevTrekNamespace.Replace("'", string.Empty);
            parent = root.Descendants(
                y0 + nodeName).FirstOrDefault(
                p => (string)p.Attribute(AppHelpers.Calculator.cId) == nodeId);
            if (parent == null
                && nodeName == AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                y0 = AppHelpers.Agreement.AGREEMENTS_NAMESPACE.Replace("'", string.Empty);
                parent = root.Descendants(
                    y0 + nodeName).FirstOrDefault(
                    p => (string)p.Attribute(AppHelpers.Calculator.cId) == nodeId);
            }
            return parent;
        }
        public static void AddElementToParent(
            EditHelper.ArgumentsEdits addsArguments,
            XElement parentEl, XElement childEl)
        {
            //root never has namespaces, so no need to check
            //StartInsertGram(addsArguments, childEl);
            //unless parent has small memory footprint
            //try using chaining methods instead
            if (childEl.Name
                == Helpers.GeneralHelpers.ROOT_PATH
                && childEl.HasElements)
            {
                parentEl.Add(childEl.FirstNode);
            }
            else
            {
                parentEl.Add(childEl);
            }
            //FinishInsertGram(addsArguments, childEl);
        }
        public static void AddReaderToElement(XElement el, XmlReader reader)
        {
            if (reader != null)
            {
                el.Add(XElement.Load(reader));
            }
        }
        public static void AddXmlDocReader(XElement el, 
            XmlReader reader)
        {
            if (reader != null)
            {
                if (el.HasElements == false)
                {
                    el.AddFirst(
                        new XElement(Helpers.GeneralHelpers.ROOT_PATH));
                }
                el
                .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                .FirstOrDefault()
                .Add(XElement.Load(reader));
            }
        }
        public static bool AddBaseLinkedViewToExistingXmlDoc(ContentURI calcDocURI,
            XElement baseLinkedViewRoot, XElement baseExistingLinkedView)
        {
            bool bHasAdded = false;
            if (baseLinkedViewRoot.Elements(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()).Any())
            {
                //current versions expect one and only one base calculator
                XElement baseLinkedView = baseLinkedViewRoot
                    .Elements(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    .FirstOrDefault();
                if (baseLinkedView != null)
                {
                    //change the linkedview id= to new id
                    SetAttributeValue(
                        baseLinkedView, AppHelpers.Calculator.cId, calcDocURI.URIId.ToString());
                    //if it exists, replace the existing linkedview
                    bHasAdded
                        = ReplaceElementInRootDoc(baseLinkedView, ref baseExistingLinkedView);
                    if (!bHasAdded)
                    {
                        //if it exists, replace the default (id=1) linkedview
                        bHasAdded
                            = ReplaceElementInRootDoc(baseLinkedView,
                                ref baseExistingLinkedView, "1");
                        if (!bHasAdded)
                        {
                            if (baseExistingLinkedView.Name.LocalName == Helpers.GeneralHelpers.ROOT_PATH)
                            {
                                //no linkedviews in db yet
                                baseExistingLinkedView.Add(baseLinkedView);
                                bHasAdded = true;
                            }
                        }
                    }
                }
            }
            else
            {
                calcDocURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                    "CALCDOC_NOLINKEDVIEWNODES");
            }
            return bHasAdded;
        }
        public static string MakeURIPatternFromElement(ContentURI elURI,
            XElement el)
        {
            string sURIPattern = elURI.URIPattern;
            if (el.HasAttributes)
            {
                sURIPattern = Helpers.GeneralHelpers.MakeURIPattern(
                    GetAttributeValue(el, AppHelpers.Calculator.cName),
                    GetAttributeValue(el, AppHelpers.Calculator.cId),
                    elURI.URINetworkPartName,
                    el.Name.ToString(),
                    elURI.URIFileExtensionType);
            }
            return sURIPattern;
        }
        public static string MakeTempURIPatternFromElement(ContentURI elURI,
            XElement el)
        {
            string sURIPattern = elURI.URIPattern;
            if (el.HasAttributes)
            {
                sURIPattern = Helpers.GeneralHelpers.MakeURIPattern(
                    GetAttributeValue(el, AppHelpers.Calculator.cName),
                    Helpers.GeneralHelpers.GetRandomInteger(0).ToString(),
                    elURI.URINetworkPartName,
                    el.Name.ToString(),
                    Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString());
            }
            return sURIPattern;
        }
        
        public static string GetAttributeValue(XElement el,
            string attName)
        {
            string sAttValue = string.Empty;
            if (el != null)
            {
                XAttribute att =
                    el.Attribute(attName);
                if (att != null)
                {
                    sAttValue = att.Value;
                }
            }
            return sAttValue;
        }
        public static string GetAttributeValue(
            XNamespace y0, XElement el, string attName)
        {
            string sAttValue = string.Empty;
            if (el != null)
            {
                XAttribute att =
                    el.Attribute(y0 + attName);
                if (att != null)
                {
                    sAttValue = att.Value;
                }
            }
            return sAttValue;
        }
        public static bool SetAttributeValue(XElement el,
            string attName, string attValue)
        {
            bool bSuccess = false;
            if (el != null)
            {
                XNamespace y0 = GetNamespaceForNode(el, el.Name.LocalName);
                if (y0 == null)
                {
                    el.SetAttributeValue(attName, attValue);
                }
                else
                {
                    el.SetAttributeValue(y0 + attName, attValue);
                }
            }
            return bSuccess;
        }
        public static string GetAttributeValue(XElement root,
            string nodeName, string nodeId, string attName)
        {
            string sAttValue = string.Empty;
            if (root != null)
            {
                XNamespace y0 = GetNamespaceForNode(root, nodeName);
                if (y0 == null)
                {
                    if (root
                        .Descendants(
                            nodeName).Any(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId))
                    {
                        XAttribute att = root
                        .Descendants(nodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId)
                        .Attribute(attName);
                        if (att != null)
                        {
                            sAttValue = att.Value;
                        }
                    }
                }
                else
                {
                    if (root
                        .Descendants(
                            y0 + nodeName).Any(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId))
                    {
                        XAttribute att = root
                        .Descendants(y0 + nodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId)
                        .Attribute(attName);
                        if (att != null)
                        {
                            sAttValue = att.Value;
                        }
                    }
                }
            }
            return sAttValue;
        }
        public static bool SetAttributeValue(XElement el,
            string nodeName, string nodeId,
            string attName, string attValue)
        {
            bool bSuccess = false;
            if (el != null)
            {
                XNamespace y0 = GetNamespaceForNode(el, nodeName);
                if (y0 == null)
                {
                    if (el
                        .Descendants(
                            nodeName).Any(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId))
                    {
                        el
                        .Descendants(
                            nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId)
                        .SetAttributeValue(attName,
                            attValue);
                        bSuccess = true;
                    }
                }
                else
                {
                    if (el
                        .Descendants(
                            y0 + nodeName).Any(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId))
                    {
                        el
                        .Descendants(
                            y0 + nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId)
                        .SetAttributeValue(attName,
                            attValue);
                        bSuccess = true;
                    }
                }
            }
            return bSuccess;
        }
        public static bool SetExistingAttributeValue(XElement el,
            string nodeName, string nodeId,
            string attName, string attValue)
        {
            bool bSuccess = false;
            if (el != null)
            {
                XNamespace y0 = GetNamespaceForNode(el, nodeName);
                if (y0 == null)
                {
                    if (el
                        .Descendants(
                            nodeName).Any(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId))
                    {
                        //check to see if the attribute already exists
                        if (el
                        .Descendants(
                           nodeName).FirstOrDefault(
                           p => (string)p.Attribute(AppHelpers.Calculator.cId)
                           == nodeId)
                         .Attribute(attName) != null)
                        {
                            el
                            .Descendants(
                                nodeName).FirstOrDefault(
                                p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == nodeId)
                            .SetAttributeValue(attName,
                                attValue);
                            bSuccess = true;
                        }
                    }
                }
                else
                {
                    if (el
                        .Descendants(
                            y0 + nodeName).Any(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId))
                    {
                        //check to see if the attribute already exists
                        if (el
                        .Descendants(
                           y0 + nodeName).FirstOrDefault(
                           p => (string)p.Attribute(AppHelpers.Calculator.cId)
                           == nodeId)
                         .Attribute(attName) != null)
                        {
                            el
                            .Descendants(
                                y0 + nodeName).FirstOrDefault(
                                p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == nodeId)
                            .SetAttributeValue(attName,
                                attValue);
                            bSuccess = true;
                        }
                    }
                }
            }
            return bSuccess;
        }
        public static bool UpdateChildrenIds(XElement el,
            string parentNodeName, string parentNodeId,
            string childAttName, string childAttValue)
        {
            bool bIsUpdated = false;
            if (el != null)
            {
                if (!string.IsNullOrEmpty(childAttName))
                {
                    //skips grouping nodes and goes directly to descendants
                    //child att names will be unique with the exception of
                    //recursive nodes (refactor for devpack and linkedview)
                    XNamespace y0 = GetNamespaceForNode(el, parentNodeName);
                    if (y0 == null)
                    {
                        if (el
                            .Descendants(parentNodeName)
                            .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == parentNodeId))
                        {
                            IEnumerable<XElement> lstSameChildIds = el
                                .Descendants(parentNodeName)
                                .Where(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == parentNodeId)
                                .Descendants()
                                .Where(c => (string)c.Attribute(childAttName)
                                  != null);
                            if (lstSameChildIds != null)
                            {
                                if (lstSameChildIds.Count() > 0)
                                {
                                    foreach (XElement sameChildIdEl in lstSameChildIds)
                                    {
                                        sameChildIdEl.SetAttributeValue(childAttName, childAttValue);
                                        bIsUpdated = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (el
                            .Descendants(y0 + parentNodeName)
                            .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == parentNodeId))
                        {
                            IEnumerable<XElement> lstSameChildIds = el
                                .Descendants(y0 + parentNodeName)
                                .Where(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == parentNodeId)
                                .Descendants()
                                .Where(c => (string)c.Attribute(childAttName)
                                  != null);
                            if (lstSameChildIds != null)
                            {
                                if (lstSameChildIds.Count() > 0)
                                {
                                    foreach (XElement sameChildIdEl in lstSameChildIds)
                                    {
                                        sameChildIdEl.SetAttributeValue(childAttName, childAttValue);
                                        bIsUpdated = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return bIsUpdated;
        }
        public static void RemoveSiblingLinkedView(XElement linkviewsDoc,
            string idToKeep)
        {
            if (linkviewsDoc != null)
            {
                IEnumerable<XElement> lstDescendants
                    = linkviewsDoc.Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                string sDescendantId = string.Empty;
                if (lstDescendants != null)
                {
                    foreach (XElement descendant in lstDescendants)
                    {
                        sDescendantId = GetAttributeValue(descendant, AppHelpers.Calculator.cId);
                        if (string.IsNullOrEmpty(sDescendantId))
                            sDescendantId = string.Empty;
                        if (!sDescendantId.Equals(idToKeep))
                        {
                            descendant.SetAttributeValue("Remove", "true");

                        }
                    }
                    //removed the tagged elements
                    IEnumerable<XElement> removalList =
                        from el in linkviewsDoc.Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .Where(p => (string)p.Attribute("Remove")
                                    == "true")
                        select el;
                    if (removalList != null)
                        removalList.Remove();
                }
            }
        }
        public static void RemoveAttribute(
            XElement currentElement, string attName)
        {
            if (currentElement != null)
            {
                if (currentElement.HasAttributes)
                {
                    currentElement.SetAttributeValue(attName, null);
                }
            }
        }
        public static void RemoveDescendantsNotInURILists(
            XElement root, string descendantNodeName,
            IList<System.Linq.IGrouping<int, ContentURI>> linkedViews)
        {
            if (root != null)
            {
                IEnumerable<XElement> lstDescendants
                    = root.Descendants(descendantNodeName);
                string sDescendantURIPattern = string.Empty;
                if (lstDescendants != null)
                {
                    foreach (XElement descendant in lstDescendants)
                    {
                        bool bElementExists = true;
                        sDescendantURIPattern = GetAttributeValue(descendant, "URIPattern");
                        if (!string.IsNullOrEmpty(sDescendantURIPattern))
                        {
                            foreach (var linkedviewparent in linkedViews)
                            {
                                bElementExists
                                    = linkedviewparent.Any(
                                    c => c.URIPattern == sDescendantURIPattern);
                                //break if the element exists in any list
                                if (bElementExists)
                                    break;
                            }
                            if (!bElementExists)
                            {
                                descendant.SetAttributeValue("Remove", "true");

                            }
                        }
                    }
                    //removed the tagged elements
                    IEnumerable<XElement> removalList =
                        from el in root.Descendants(descendantNodeName)
                        .Where(p => (string)p.Attribute("Remove")
                                    == "true")
                        select el;
                    if (removalList != null)
                        removalList.Remove();
                }
            }
        }
        public static XElement GetFirstChildElement(
           XElement parent, string nodeName)
        {
            XElement childElement = null;
            if (parent != null)
            {
                if (parent.Elements(nodeName).Any())
                {
                    childElement = parent.Elements(nodeName).FirstOrDefault();
                }
            }
            return childElement;
        }
        public static IEnumerable<XElement> GetChildrenLinkedView(
           XElement parent)
        {
            IEnumerable<XElement> linkedViews = null;
            if (parent != null)
            {
                if (parent.Name.LocalName
                    == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    linkedViews = parent
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                }
                else
                {
                    if (parent
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                    {
                        linkedViews = parent
                            .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                    }
                }
            }
            return linkedViews;
        }
        public static IEnumerable<XElement> GetChildrenLinkedViewUsingAttribute(
           XElement parent, string attName, string attValue)
        {
            IEnumerable<XElement> linkedViews = null;
            if (parent != null)
            {
                if (parent.Name.LocalName
                    == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    linkedViews = parent
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .Where(c => (string)c.Attribute(attName) == attValue);
                }
                else
                {
                    if (parent
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                    {
                        linkedViews = parent
                            .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            .Where(c => (string)c.Attribute(attName) == attValue);
                    }
                }
            }
            return linkedViews;
        }
        public static IEnumerable<XElement> GetChildrenLinkedViewUsingAttributes(
           XElement parent, string att1Name, string att1Value, string att2Name, string att2Value)
        {
            IEnumerable<XElement> linkedViews = null;
            if (parent != null)
            {
                if (parent.Name.LocalName
                    == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    linkedViews = parent
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .Where(c => (string)c.Attribute(att1Name) == att1Value || (string)c.Attribute(att2Name) == att2Value);
                }
                else
                {
                    if (parent
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                    {
                        linkedViews = parent
                            .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            .Where(c => (string)c.Attribute(att1Name) == att1Value || (string)c.Attribute(att2Name) == att2Value);
                    }
                }
            }
            return linkedViews;
        }
        public static string GetFirstChildLinkedViewIdUsingAttribute(
           XmlElement parent, string attName, string attValue)
        {
            string sLinkedViewId = string.Empty;
            if (parent != null)
            {
                XElement xParent = XElement.Parse(parent.OuterXml);
                if (xParent != null)
                {
                    XElement lv = GetFirstChildLinkedViewUsingAttribute(
                        xParent, attName, attValue);
                    if (lv != null)
                    {
                        sLinkedViewId = GetAttributeValue(lv, AppHelpers.Calculator.cId);
                    }
                }
            }
            return sLinkedViewId;
        }
       
        public static string GetFirstChildLinkedViewIdUsingAttribute(
           XElement parent, string attName, string attValue)
        {
            string sLinkedViewId = string.Empty;
            XElement lv = GetFirstChildLinkedViewUsingAttribute(
                parent, attName, attValue);
            if (lv != null)
            {
                sLinkedViewId = GetAttributeValue(lv, AppHelpers.Calculator.cId);
            }
            return sLinkedViewId;
        }
        public static XElement GetFirstChildLinkedViewUsingAttribute(
           XElement parent, string attName, string attValue)
        {
            XElement linkedView = null;
            if (parent != null)
            {
                if (parent.Name.LocalName
                    == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    linkedView = parent
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .FirstOrDefault(c => (string)c.Attribute(attName) == attValue);
                }
                else
                {
                    if (parent
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                    {
                        linkedView = parent
                            .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            .FirstOrDefault(c => (string)c.Attribute(attName) == attValue);
                    }
                }
            }
            return linkedView;
        }
        public static XElement GetChildLinkedViewUsingAttribute(
            XElement root, string attName, string attValue)
        {
            XElement linkedView = null;
            if (root != null)
            {
                if (root.Name.LocalName
                    == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    if (root
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()).Any())
                    {
                        linkedView = root
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            .FirstOrDefault(c => (string)c.Attribute(attName)
                                == attValue);
                    }
                }
                else
                {
                    if (root
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                    {
                        if (root
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()).Any())
                        {
                            linkedView = root
                                .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                                .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                .FirstOrDefault(c => (string)c.Attribute(attName)
                                    == attValue);
                        }
                    }
                }
            }
            return linkedView;
        }
        public static XElement GetChildLinkedView(
            XElement parent, string linkedViewId)
        {
            XElement linkedView = null;
            if (parent != null)
            {
                if (parent
                    .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                {
                    if (parent
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()).Any())
                    {
                        linkedView = parent
                            .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            .FirstOrDefault(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                                == linkedViewId);
                    }
                }
                else if (parent.Name.LocalName == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    if (parent
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()).Any())
                    {
                        linkedView = parent
                            .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            .FirstOrDefault(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                                == linkedViewId);
                    }
                }
            }
            return linkedView;
        }
        //public static IEnumerable<XElement> GetChildrenLinkedView(
        //    XElement root)
        //{
        //    IEnumerable<XElement> linkedViews = null;
        //    if (root != null)
        //    {
        //        if (root
        //            .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
        //        {
        //            linkedViews = root
        //                .Elements(Helpers.GeneralHelpers.ROOT_PATH)
        //                .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
        //        }
        //    }
        //    return linkedViews;
        //}
        public static IEnumerable<XElement> GetChildrenLinkedView(
            XElement root, string parentNodeName, string parentId)
        {
            IEnumerable<XElement> linkedViews = null;
            if (root != null)
            {
                if (root
                    .Descendants(parentNodeName).Any())
                {
                    if (root
                        .Descendants(parentNodeName)
                        .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId))
                    {
                        if (root
                        .Descendants(parentNodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                        {
                            linkedViews = root
                                .Descendants(parentNodeName).FirstOrDefault(
                                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == parentId)
                                .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                        }
                    }
                }
            }
            return linkedViews;
        }
        public static string GetFirstChildLinkedViewId(XElement root)
        {
            string sId = string.Empty;
            if (root != null)
            {
                if (root
                   .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                   .Any())
                {
                    sId = root
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .FirstOrDefault()
                        .Attribute(AppHelpers.Calculator.cId)
                        .Value;
                }
            }
            return sId;
        }
        public static XElement GetChildrenLinkedViewXmlDoc(ContentURI uri,
            string xmlDocPath, string parentNodeName, string parentId)
        {
            XElement linkedViewsXmlDoc = null;
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, xmlDocPath))
            {
                XElement root = Helpers.FileStorageIO.LoadXmlElement(uri, xmlDocPath);
                if (root != null)
                {
                    linkedViewsXmlDoc = GetChildrenLinkedViewXmlDoc(
                        root, parentNodeName, parentId);
                    root = null;
                }
            }
            return linkedViewsXmlDoc;
        }
        public static XElement GetChildrenLinkedViewXmlDoc(
            XElement root, string parentNodeName, string parentId)
        {
            XElement linkedViewsXmlDoc = null;
            if (root != null)
            {
                if (root
                    .Descendants(parentNodeName).Any())
                {
                    if (root
                        .Descendants(parentNodeName)
                        .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId))
                    {
                        if (root
                        .Descendants(parentNodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                        {
                            IEnumerable<XElement> xmldocs
                                = root
                                .Descendants(parentNodeName).FirstOrDefault(
                                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == parentId)
                                .Elements(Helpers.GeneralHelpers.ROOT_PATH);
                            if (xmldocs != null)
                            {
                                linkedViewsXmlDoc.Add(xmldocs);
                            }
                        }
                    }
                }
            }
            return linkedViewsXmlDoc;
        }
        public static XElement GetChildrenLinkedViewRoot(
            XElement root, string parentNodeName, string parentId)
        {
            //note that the nodes processed here can have one <xmddoc><root>
            XElement linkedViewsXmlDoc = null;
            if (root != null)
            {
                if (root
                    .Descendants(parentNodeName).Any())
                {
                    if (root
                        .Descendants(parentNodeName)
                        .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId))
                    {
                        if (root
                        .Descendants(parentNodeName)
                        .FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == parentId)
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH).Any())
                        {
                            linkedViewsXmlDoc = root
                                    .Descendants(parentNodeName).FirstOrDefault(
                                        p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                        == parentId)
                                    .Element(Helpers.GeneralHelpers.ROOT_PATH);
                        }
                    }
                }
            }
            return linkedViewsXmlDoc;
        }
        public static bool AddAttributesWithoutIdNameDesc(string nodeName, string nodeId,
            XElement fromRoot, XElement toElement)
        {
            bool bHasAddedAtts = false;
            IEnumerable<XAttribute> fromRootAtts = null;
            XNamespace y0 = GetNamespaceForNode(fromRoot, nodeName);
            if (y0 == null)
            {
                bool bHasDescendant 
                    = DescendantExists(fromRoot, nodeName, nodeId);
                if (bHasDescendant)
                {
                    fromRootAtts = fromRoot
                        .Descendants(nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId)
                        .Attributes();
                }
            }
            else
            {
                bool bHasDescendant 
                    = DescendantExistsWithNS(y0, fromRoot, nodeName, nodeId);
                if (bHasDescendant)
                {
                    fromRootAtts = fromRoot
                        .Descendants(y0 + nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == nodeId)
                        .Attributes();
                }
            }
            if (fromRootAtts != null)
            {
                if (fromRootAtts.Count() > 0)
                {
                    foreach (XAttribute att in fromRootAtts)
                    {
                        //don't add ids or names or descriptions
                        if (att.Name != AppHelpers.Calculator.cId
                            && att.Name != AppHelpers.Calculator.cName
                            && att.Name != AppHelpers.Calculator.cDescription)
                        {
                            toElement.SetAttributeValue(att.Name, att.Value);
                        }
                    }
                    bHasAddedAtts = true;
                }
            }
            return bHasAddedAtts;
        }
        public static bool AddAttributesThatAreMissing(string nodeName, string fromNodeId,
            XElement fromRoot, string toNodeId, XElement toRoot)
        {
            bool bHasAddedAtts = false;
            IEnumerable<XAttribute> fromRootAtts = null;
            XNamespace y0 = GetNamespaceForNode(fromRoot, nodeName);
            if (y0 == null)
            {
                bool bHasDescendant
                    = DescendantExists(fromRoot, nodeName, fromNodeId);
                if (bHasDescendant)
                {
                    fromRootAtts = fromRoot
                        .Descendants(nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == fromNodeId)
                        .Attributes();
                }
            }
            else
            {
                bool bHasDescendant
                    = DescendantExistsWithNS(y0, fromRoot, nodeName, fromNodeId);
                if (bHasDescendant)
                {
                    fromRootAtts = fromRoot
                        .Descendants(y0 + nodeName).FirstOrDefault(
                            p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == fromNodeId)
                        .Attributes();
                }
            }
            if (fromRootAtts != null)
            {
                if (fromRootAtts.Count() > 0)
                {
                    y0 = GetNamespaceForNode(toRoot, nodeName);
                    XElement toElement = null;
                    if (y0 == null)
                    {
                        bool bHasDescendant
                            = DescendantExists(toRoot, nodeName, toNodeId);
                        if (bHasDescendant)
                        {
                            toElement = toRoot
                                .Descendants(nodeName).FirstOrDefault(
                                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == toNodeId);
                        }
                    }
                    else
                    {
                        bool bHasDescendant
                            = DescendantExistsWithNS(y0, toRoot, nodeName, toNodeId);
                        if (bHasDescendant)
                        {
                            toElement = toRoot
                                .Descendants(y0 + nodeName).FirstOrDefault(
                                    p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                    == toNodeId);
                        }
                    }
                    if (toElement != null)
                    {
                        //limit the atts that get updated when version changes (i.e. don't 
                        //overwrite existing atts with default atts in new version)
                        bool bIsLinkedViewCalculationUpdate = false;
                        AddAttributes(fromRootAtts, toElement, bIsLinkedViewCalculationUpdate);
                        bHasAddedAtts = true;
                    }
                }
            }
            return bHasAddedAtts;
        }
        public static void AddAttributes(IEnumerable<XAttribute> fromAtts,
            XElement toElement, bool isLinkedViewCalculationUpdate)
        {
            if (fromAtts != null && toElement != null)
            {
                //0.9.1 added the Overwrite attribute making the overwriting of starting calculator
                //and children calculators more transparent
                string sOverwrite = GetAttributeValue(fromAtts, AppHelpers.Calculator.cOverwrite);
                bool bOverwrite = Helpers.GeneralHelpers.ConvertStringToBool(sOverwrite);
                string sValue = string.Empty;
                foreach (XAttribute att in fromAtts)
                {
                    //add missing attributes
                    if (toElement.Attribute(att.Name) == null)
                    {
                        //V180 bug fix
                        if (att.Name.LocalName == AppHelpers.Calculator.cCalculatorType
                            || att.Name.LocalName == AppHelpers.Calculator.cAnalyzerType)
                        {
                            if (NeedsAttribute(att.Name.LocalName, toElement))
                            {
                                toElement.SetAttributeValue(att.Name, att.Value);
                            }
                        }
                        else
                        {
                            toElement.SetAttributeValue(att.Name, att.Value);
                        }                            
                    }
                    else
                    {
                        //also add missing attribute values
                        sValue = GetAttributeValue(toElement, att.Name.LocalName);
                        if (string.IsNullOrEmpty(sValue))
                        {
                            if (att.Name.LocalName == AppHelpers.Calculator.cCalculatorType
                                || att.Name.LocalName == AppHelpers.Calculator.cAnalyzerType)
                            {
                                if (NeedsAttribute(att.Name.LocalName, toElement))
                                {
                                    toElement.SetAttributeValue(att.Name, att.Value);
                                }
                            }
                            else
                            {
                                toElement.SetAttributeValue(att.Name, att.Value);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(att.Value))
                            {
                                //1.4.1: developer-centric attributes are always updated 
                                //analyzer options added otherwise can't get rid of aggtype if the analyzer no longer uses it (i.e. lcachanges)
                                if (att.Name == AppHelpers.Calculator.cCalculatorName
                                    ||att.Name == AppHelpers.Calculator.cFileExtensionType
                                    || att.Name == AppHelpers.Calculator.cRelatedCalculatorType
                                    || att.Name == AppHelpers.Calculator.cRelatedCalculatorsType
                                    || att.Name == AppHelpers.Calculator.cStylesheetResourceFileName
                                    || att.Name == AppHelpers.Calculator.cStylesheet2ResourceFileName
                                    || att.Name == AppHelpers.Calculator.cVersion
                                    || att.Name == AppHelpers.Calculator.cOption1
                                    || att.Name == AppHelpers.Calculator.cOption2
                                    || att.Name == AppHelpers.Calculator.cOption3
                                    || att.Name == AppHelpers.Calculator.cOption4)
                                {
                                    //developers can change these at any time and all calculators 
                                    //that use the base linked view will be updated next time they are run
                                    toElement.SetAttributeValue(att.Name, att.Value);
                                }
                                else if (att.Name.LocalName == AppHelpers.Calculator.cCalculatorType
                                    || att.Name.LocalName == AppHelpers.Calculator.cAnalyzerType)
                                {
                                    //V180 bug fix
                                    if (NeedsAttribute(att.Name.LocalName, toElement))
                                    {
                                        toElement.SetAttributeValue(att.Name, att.Value);
                                    }
                                }
                                else
                                {
                                    //0.9.1: parent calculator can only change other atts when Overwrite = true
                                    //and parent is updating children (not when calcor is checking base calcor)
                                    if (bOverwrite && isLinkedViewCalculationUpdate)
                                    {
                                        if (att.Name.LocalName == AppHelpers.Calculator.cCalculatorType
                                            || att.Name.LocalName == AppHelpers.Calculator.cAnalyzerType)
                                        {
                                            if (NeedsAttribute(att.Name.LocalName, toElement))
                                            {
                                                toElement.SetAttributeValue(att.Name, att.Value);
                                            }
                                        }
                                        else
                                        {
                                            if (att.Name != AppHelpers.Calculator.cId
                                                && att.Name != AppHelpers.Calculator.cCalculatorId)
                                            {

                                                toElement.SetAttributeValue(att.Name, att.Value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static bool NeedsAttribute(string attName, XElement toElement)
        {
            bool bNeedsTypeName = true;
            string sValue = string.Empty;
            if (attName == AppHelpers.Calculator.cCalculatorType)
            {
                sValue = GetAttributeValue(toElement, AppHelpers.Calculator.cAnalyzerType);
                if (!string.IsNullOrEmpty(sValue))
                {
                    //dont' give analyzers a calculator type
                    bNeedsTypeName = false;
                }
            }
            else if (attName == AppHelpers.Calculator.cAnalyzerType)
            {
                sValue = GetAttributeValue(toElement, AppHelpers.Calculator.cCalculatorType);
                if (!string.IsNullOrEmpty(sValue))
                {
                    //dont' give calculators an analyzers type
                    bNeedsTypeName = false;
                }
            }
            return bNeedsTypeName;
        }
        private static bool IsNewVersion(IEnumerable<XAttribute> fromAtts,
            XElement toElement)
        {
            bool bIsNewVersion = false;
            XAttribute versionAtt = fromAtts.FirstOrDefault(a => a.Name.LocalName == AppHelpers.Calculator.cVersion);
            if (versionAtt != null)
            {
                string sValue = GetAttributeValue(toElement, versionAtt.Name.LocalName);
                if (!string.IsNullOrEmpty(sValue))
                {
                    if (!sValue.Equals(versionAtt.Value))
                    {
                        bIsNewVersion = true;
                    }
                }
            }
            return bIsNewVersion;
        }
        private static string GetAttributeValue(IEnumerable<XAttribute> fromAtts,
            string attName)
        {
            string sValue = string.Empty;
            XAttribute att = fromAtts.FirstOrDefault(a => a.Name.LocalName == attName);
            if (att != null)
            {
                sValue = att.Value;
            }
            return sValue;
        }
        public static bool AddLinkedViewAttributesThatAreMissing(
            XElement fromLinkedView, XElement toLinkedView)
        {
            bool bHasAddedAtts = false;
            if (fromLinkedView != null && toLinkedView != null)
            {
                if (fromLinkedView.Name.LocalName
                    == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()
                    && toLinkedView.Name.LocalName
                    == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (fromLinkedView.HasAttributes)
                    {
                        bool bIsLinkedViewCalculationUpdate = true;
                        AddAttributes(fromLinkedView.Attributes(), toLinkedView, bIsLinkedViewCalculationUpdate);
                        bHasAddedAtts = true;
                    }
                }
            }
            return bHasAddedAtts;
        }
        public static XElement ConvertNavigatorToElement(XPathNavigator nav)
        {
            XElement oConvertedNavigator = null;
            if (nav != null)
            {
                //stream the entire XML document to the XmlReader.
                XmlReader xmlReader = XPathIO.ConvertNavigatorToReader(nav);
                if (xmlReader != null)
                {
                    using (xmlReader)
                    {
                        oConvertedNavigator = XElement.Load(xmlReader);
                    }
                }
            }
            return oConvertedNavigator;
        }
        public static void AddNodeAttributes(XElement fromElement,
            ref XElement toElement)
        {
            if (fromElement != null)
            {
                string sName = string.Empty;
                string sValue = string.Empty;
                IEnumerable<XAttribute> colAtts = fromElement.Attributes();
                foreach (XAttribute oAtt in colAtts)
                {
                    sName = oAtt.Name.ToString();
                    sValue = oAtt.Value;
                    toElement.SetAttributeValue(sName, sValue);
                    sName = string.Empty;
                    sValue = string.Empty;
                }
                colAtts = null;
            }
        }
        public static bool ReplaceOrInsertChildLinkedViewElement(
            XElement linkedViewElement, ref XElement root)
        {
            bool bSuccess = false;
            //use one set of methods to replace or insert linkedViews
            XElement replacedLinkedView = null;
            string sLinkedViewId = GetAttributeValue(linkedViewElement, 
                AppHelpers.Calculator.cId);
            bool bIsLinkedViewXmlDoc = false;
            bSuccess = ReplaceOrInsertLinkedViewElement(ref root,
                string.Empty, linkedViewElement,
                string.Empty, sLinkedViewId, bIsLinkedViewXmlDoc,
                out replacedLinkedView);
            return bSuccess;
        }
        public static bool ReplaceOrInsertLinkedViewElement(ref XElement insertToEl,
            string nameSpaceURI, XElement replacementLinkedView,
            string linkedViewsIds, string linkedViewId, bool isLinkedViewXmlDoc, 
            out XElement replacedLinkedView)
        {
            bool bIsReplaced = false;
            replacedLinkedView = null;
            if (insertToEl != null)
            {
                if (insertToEl
                    .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                    .Any())
                {
                    //try to replace insertToEl.xmldoc.root existing linkedview node
                    bool bIsSameElement = false;
                    bIsReplaced = ReplaceChangedElementInChildLinkedView(
                        replacementLinkedView, linkedViewId, ref insertToEl,
                        ref bIsSameElement, out replacedLinkedView);
                    if (!isLinkedViewXmlDoc)
                    {
                        if (!bIsReplaced && !bIsSameElement)
                        {
                            if (insertToEl
                                .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                                .Any())
                            {
                                if (replacementLinkedView.Name.LocalName
                                    == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                {
                                    //needs new <root><linkedview xml
                                    XElement xmlEl
                                        = new XElement(Helpers.GeneralHelpers.ROOT_PATH,
                                                new XElement(replacementLinkedView)
                                        );
                                    insertToEl
                                        .AddFirst(xmlEl);
                                    AddLinkedViewToNullElement(replacementLinkedView,
                                        out replacedLinkedView);
                                    bIsReplaced = true;
                                    //v136 fixed this
                                    ////add the new linkedview to the first position
                                    //insertToEl
                                    //    .Element(Helpers.GeneralHelpers.ROOT_PATH)
                                    //    .AddFirst(replacementLinkedView);
                                    //AddLinkedViewToNullElement(replacementLinkedView,
                                    //    out replacedLinkedView);
                                    //bIsReplaced = true;
                                }
                                else if (replacementLinkedView.Name.LocalName
                                    == Helpers.GeneralHelpers.ROOT_PATH)
                                {
                                    bIsReplaced = ReplaceXmlDocElement(ref insertToEl,
                                        replacementLinkedView, out replacedLinkedView);
                                }
                            }
                            else
                            {
                                //needs new <root><linkedview xml
                                XElement xmlEl
                                    = new XElement(Helpers.GeneralHelpers.ROOT_PATH,
                                            new XElement(replacementLinkedView)
                                    );
                                insertToEl
                                    .AddFirst(xmlEl);
                                AddLinkedViewToNullElement(replacementLinkedView,
                                    out replacedLinkedView);
                                bIsReplaced = true;
                            }
                        }
                    }
                    else
                    {
                        if (!bIsReplaced && !bIsSameElement)
                        {
                            //store each xmldoc in a separate 
                            //table row and xmldoc node
                            bIsReplaced = AddFullLinkedViewXmlDoc(replacementLinkedView,
                                ref insertToEl, out replacedLinkedView);
                        }
                    }
                }
                else
                {
                    //needs new <xmldoc><root><linkedview xml
                    bIsReplaced = AddFullLinkedViewXmlDoc(replacementLinkedView,
                        ref insertToEl, out replacedLinkedView);
                }
                if (bIsReplaced)
                {
                    //return sibling ids that need to be deleted
                    string sId = GetAttributeValue(replacementLinkedView,
                        AppHelpers.Calculator.cId);
                    DeleteSiblingLinkedView(ref insertToEl, sId,
                        linkedViewsIds);
                }
            }
            return bIsReplaced;
        }
        private static bool AddFullLinkedViewXmlDoc(XElement replacementLinkedView,
            ref XElement root, out XElement replacedLinkedView)
        {
            bool bIsReplaced = false;
            XElement xmlEl
                    = new XElement(Helpers.GeneralHelpers.ROOT_PATH,
                            new XElement(replacementLinkedView)
                    );
            root.AddFirst(xmlEl);
            AddLinkedViewToNullElement(replacementLinkedView,
                out replacedLinkedView);
            bIsReplaced = true;
            return bIsReplaced;
        }
        private static void DeleteSiblingLinkedView(ref XElement newLinkedView,
            string siblingLinkedViewId, string linkedViewsIds)
        {
            //clean up the existing linkedviewpack by removing linkedviews that aren't
            //part of the db linkedviewsids
            if (!string.IsNullOrEmpty(linkedViewsIds))
            {
                string[] arrLinkViewsIds 
                    = linkedViewsIds.Split(Helpers.GeneralHelpers.PARAMETER_DELIMITERS);
                if (arrLinkViewsIds != null)
                {
                    int i = 0;
                    int iLength = arrLinkViewsIds.Length;
                    string sLinkedViewSiblingLinkedViewId = string.Empty;
                    string sLinkedViewId = string.Empty;
                    List<string> linkedViewsToDelete = new List<string>();
                    foreach (XElement linkedview in newLinkedView
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
                    {
                        sLinkedViewId = GetAttributeValue(linkedview, AppHelpers.Calculator.cId);
                        if (sLinkedViewId.Equals(siblingLinkedViewId))
                        {
                            sLinkedViewId = string.Empty;
                        }
                        else
                        {
                            for (i = 0; i < iLength; i++)
                            {
                                sLinkedViewSiblingLinkedViewId = string.Empty;
                                sLinkedViewSiblingLinkedViewId = arrLinkViewsIds[i];
                                if (sLinkedViewSiblingLinkedViewId.Equals(sLinkedViewId))
                                {
                                    //it's a good sibling
                                    sLinkedViewId = string.Empty;
                                }
                            }
                        }
                        if (sLinkedViewId != string.Empty)
                        {
                            linkedViewsToDelete.Add(sLinkedViewId);
                            sLinkedViewId = string.Empty;
                        }
                    }
                    if (linkedViewsToDelete.Count > 0)
                    {
                        foreach (string linkedViewId in linkedViewsToDelete)
                        {
                            if (newLinkedView
                                .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                                .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                .Any(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                                    == linkedViewId))
                            {
                                bool bHasSiblings = HasSiblingLinkedView(newLinkedView,
                                    linkedViewId);
                                //if it has no siblings, delete root and xmld nodes too
                                if (!bHasSiblings)
                                {
                                    newLinkedView
                                        .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                        .FirstOrDefault(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                                            == linkedViewId)
                                        .Parent
                                        .Parent
                                        .Remove();
                                }
                                else
                                {
                                    newLinkedView
                                        .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                        .FirstOrDefault(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                                            == linkedViewId)
                                        .Remove();
                                }
                                

                            }
                        }
                    }
                }
            }
        }
        private static bool HasSiblingLinkedView(XElement root, string linkedViewId)
        {
            bool bHasSiblings = false;
            if (root != null)
            {
                if (root
                .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                .Any(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                    == linkedViewId))
                {
                    bHasSiblings = root
                    .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    .FirstOrDefault(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                        == linkedViewId)
                    .ElementsAfterSelf()
                    .Any();
                    if (!bHasSiblings)
                    {
                        bHasSiblings = root
                       .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                       .FirstOrDefault(d => (string)d.Attribute(AppHelpers.Calculator.cId)
                           == linkedViewId)
                       .ElementsBeforeSelf()
                       .Any();
                    }
                }
            }
            return bHasSiblings;
        }
        
        public static bool ReplaceChildElement(XElement replacementElement,
            string parentId, string parentNodeName, ref XElement root)
        {
            bool bHasReplaced = false;
            if (replacementElement != null && root != null)
            {
                string sId = GetAttributeValue(replacementElement,
                    AppHelpers.Calculator.cId);
                XNamespace y0 = GetNamespaceForNode(root,
                    replacementElement.Name.LocalName);
                if (y0 == null)
                {
                    //verify that the child exists
                    bool bHasElement = DescendantExists(root, parentNodeName, parentId);
                    if (bHasElement)
                    {
                        if (root
                            .Descendants(parentNodeName)
                            .FirstOrDefault(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                                == parentId)
                            .Descendants(replacementElement.Name.LocalName)
                            .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == sId))
                        {
                            root
                            .Descendants(parentNodeName)
                            .FirstOrDefault(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                                == parentId)
                            .Descendants(replacementElement.Name.LocalName)
                            .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == sId)
                            .ReplaceWith(replacementElement);
                            bHasReplaced = true;
                        }
                    }
                }
            }
            return bHasReplaced;
        }
        public static bool ReplaceOrInsertChildElement(XElement replacementElement,
            ref XElement parentElement)
        {
            bool bHasReplaced = false;
            if (replacementElement != null && parentElement != null)
            {
                string sId = GetAttributeValue(replacementElement,
                    AppHelpers.Calculator.cId);
                string sNodeName = replacementElement.Name.LocalName;
                XNamespace y0 = GetNamespaceForNode(parentElement,
                    replacementElement.Name.LocalName);
                if (y0 == null)
                {
                    if (sNodeName != parentElement.Name.LocalName)
                    {
                        //verify that the child exists
                        bool bHasElement = DescendantExists(parentElement,
                            sNodeName, sId);
                        if (bHasElement
                            && replacementElement.HasAttributes)
                        {
                            if (parentElement
                                .Descendants(sNodeName)
                                .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                == sId))
                            {
                                XElement elementToReplace = parentElement
                                   .Descendants(sNodeName)
                                   .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                       == sId);
                                if (elementToReplace != null)
                                {
                                    elementToReplace.RemoveAttributes();
                                    foreach (XAttribute att in replacementElement.Attributes())
                                    {
                                        elementToReplace.SetAttributeValue(att.Name, att.Value);
                                    }
                                }
                                //this syntax does not work:
                                //parentElement
                                //.Descendants(sNodeName)
                                //.FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                                //    == sId)
                                //.ReplaceWith(replacementElement);
                                bHasReplaced = true;
                            }
                        }
                    }
                    if (!bHasReplaced)
                    {
                        if (sNodeName != parentElement.Name.LocalName)
                        {
                            //insert the replacement
                            parentElement.Add(replacementElement);
                            bHasReplaced = true;
                        }
                        else
                        {
                            //check to see if replacementEl == parentEl
                            string sParentId = GetAttributeValue(parentElement,
                                AppHelpers.Calculator.cId);
                            if (sParentId != sId)
                            {
                                //add the sibling
                                parentElement.AddAfterSelf(replacementElement);
                                bHasReplaced = true;
                            }
                            else
                            {
                                //replace self
                                parentElement.RemoveAttributes();
                                foreach (XAttribute att in replacementElement.Attributes())
                                {
                                    parentElement.SetAttributeValue(att.Name, att.Value);
                                }
                                bHasReplaced = true;
                            }
                        }
                    }
                }
            }
            return bHasReplaced;
        }
        
        public static void SetDescendantLinkedViewAttributeValue(string descendantNodeName, 
            string descendantId, string descendantAttName, string descendantValue, 
            string needsToBeReplacedLinkedViewId, ref XElement root)
        {
            XNamespace y0 = GetNamespaceForNode(root, descendantNodeName);
            if (y0 == null)
            {
                //verify that the child exists
                bool bHasElement = DescendantExists(root, descendantNodeName, descendantId);
                if (bHasElement)
                {
                    if (root
                        .Descendants(descendantNodeName)
                        .FirstOrDefault(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                            == descendantId)
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .Any(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                        == needsToBeReplacedLinkedViewId))
                    {
                        //if the needsToBeReplacedLinkedViewId el exists, replace it
                        root
                        .Descendants(descendantNodeName)
                        .FirstOrDefault(c => (string)c.Attribute(AppHelpers.Calculator.cId)
                        == descendantId)
                        .Descendants(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .FirstOrDefault(p => (string)p.Attribute(AppHelpers.Calculator.cId)
                            == needsToBeReplacedLinkedViewId)
                        .SetAttributeValue(descendantAttName,
                            descendantValue);
                    }
                }
            }
        }
        public static bool ReplaceXmlDocElement(ref XElement insertToEl, 
            XElement fullXmlDocElement, out XElement replacedLinkedView)
        {
            bool bIsReplaced = false;
            replacedLinkedView = null;
            bIsReplaced = ReplaceXmlDocElement(ref insertToEl, fullXmlDocElement);
            if (bIsReplaced)
            {
                replacedLinkedView = new XElement(fullXmlDocElement);
            }
            return bIsReplaced;
        }
        public static bool ReplaceXmlDocElement(ref XElement insertToEl,
            XElement fullXmlDocElement)
        {
            bool bIsReplaced = false;
            if (insertToEl != null)
            {
                if (insertToEl
                    .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                    .Any())
                {
                    //try to replace insertToEl.xmldoc.root existing linkedview node
                    insertToEl
                        .Elements(Helpers.GeneralHelpers.ROOT_PATH)
                        .Remove();
                    insertToEl.Add(fullXmlDocElement);

                }
            }
            return bIsReplaced;
        }
        public static void MakeNewURIPatternFromElement(string existingURIPattern,
            XElement el, out string newURIPattern)
        {
            newURIPattern = existingURIPattern;
            string sNetworkId = ContentURI.GetURIPatternPart(existingURIPattern,
                ContentURI.URIPATTERNPART.network);
            //0.8.4: each node has distinct att names (i.e. Name)
            string sName = ContentURI.GetURIPatternPart(existingURIPattern,
                ContentURI.URIPATTERNPART.name);
            //string sName = GetAttributeValue(el, AppHelpers.Calculator.cName);
            string sId = GetAttributeValue(el, AppHelpers.Calculator.cId);
            if (!string.IsNullOrEmpty(sId))
            {
                newURIPattern = Helpers.GeneralHelpers.MakeURIPattern(sName, sId,
                    sNetworkId, el.Name.LocalName, string.Empty);
            }
        }
        public static XElement ConvertXmlDocDbFieldToXmlDocElement(
            XElement xmlDocFieldFromDb)
        {
            //when an xmldoc attribute field is retrieved from db
            //by some other means than schema, it does not have 
            //root of <xmldoc>
            XElement el = null;
            if (xmlDocFieldFromDb != null)
            {
                if (xmlDocFieldFromDb.Name.LocalName
                    == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    el = xmlDocFieldFromDb;
                }
            }
            return el;
        }
        public static XElement ConvertXmlDocDbFieldToXmlDocElement(
            string xmlDocFieldFromDb)
        {
            //when an xmldoc attribute field is retrieved from db
            //by some other means than schema, it does not have 
            //root of <xmldoc>
            XElement el = null;
            if (xmlDocFieldFromDb != null)
            {
                if (xmlDocFieldFromDb.StartsWith(Helpers.GeneralHelpers.ROOT_PATH))
                {
                    el = XElement.Parse(xmlDocFieldFromDb);
                }
            }
            return el;
        }
        public static XElement GetDescendant(string nodeId,
            string nodeName, string parentURIPattern,
            IDictionary<string, string> ancestors, XElement root)
        {
            XElement descendant = null;
            if (ancestors != null
                && root != null)
            {
                string sParentNodeName = ContentURI.GetURIPatternPart(
                    parentURIPattern, ContentURI.URIPATTERNPART.node);
                string sParentId = ContentURI.GetURIPatternPart(
                    parentURIPattern, ContentURI.URIPATTERNPART.id);
                string sAncestorNodeId = string.Empty;
                string sAncestorNodeName = string.Empty;
                XmlReader rootReader = null;
                using (rootReader
                    = root.CreateReader())
                {
                    bool bHasAncestor = true;
                    foreach (KeyValuePair<string, string> kvp in ancestors)
                    {
                        sAncestorNodeId = ContentURI.GetURIPatternPart(kvp.Value,
                            ContentURI.URIPATTERNPART.id);
                        sAncestorNodeName = kvp.Key;
                        //read through root and return the XElement for nodeName and nodeId
                        //if it is a descendant of the nodes in ancestors and parentURIPattern
                        bHasAncestor = GetDescendant(sAncestorNodeId,
                            sAncestorNodeName, nodeId, nodeName, sParentNodeName,
                            sParentId, ref rootReader, ref descendant);
                        if (descendant != null)
                        {
                            return descendant;
                        }
                        //the descendant must have all ancestors
                        if (bHasAncestor == false)
                        {
                            break;
                        }
                        if (sAncestorNodeName == sParentNodeName)
                        {
                            break;
                        }
                    }
                }
            }
            return descendant;
        }
        private static bool GetDescendant(string ancestorNodeId, 
            string ancestorNodeName, string nodeId, string nodeName, 
            string parentNodeName, string parentNodeId, 
            ref XmlReader rootReader, ref XElement descendant)
        {
            bool bHasAncestor = false;
            if (rootReader != null
                && (!string.IsNullOrEmpty(ancestorNodeName)))
            {
                while (rootReader.ReadToFollowing(
                    ancestorNodeName))
                {
                    if (rootReader.NodeType
                        == XmlNodeType.Element)
                    {
                        string sCurrentNodeId = rootReader.GetAttribute(
                            AppHelpers.Calculator.cId);
                        if (sCurrentNodeId == ancestorNodeId)
                        {
                            bHasAncestor = true;
                            if (rootReader.Name == parentNodeName
                                && sCurrentNodeId == parentNodeId)
                            {
                                while (rootReader.ReadToDescendant(nodeName))
                                {
                                    if (rootReader.NodeType
                                        == XmlNodeType.Element)
                                    {
                                        string sNodeId = rootReader.GetAttribute(AppHelpers.Calculator.cId);
                                        if (sNodeId == nodeId)
                                        {
                                            descendant
                                                = GetCurrentElementWithAttributes(rootReader);
                                            return true;
                                        }
                                        while (rootReader.ReadToNextSibling(nodeName))
                                        {
                                            sNodeId = rootReader.GetAttribute(AppHelpers.Calculator.cId);
                                            if (sNodeId == nodeId)
                                            {
                                                descendant
                                                    = GetCurrentElementWithAttributes(rootReader);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (descendant == null)
                                {
                                    //group elements don't have parenturis
                                    descendant = GetDescendantGroupNode(rootReader,
                                        nodeId, nodeName, parentNodeName,
                                        parentNodeId);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return bHasAncestor;
        }
        private static XElement GetDescendantGroupNode(XmlReader rootReader,
           string nodeId, string nodeName, string parentNodeName,
           string parentNodeId)
        {
            XElement descendant = null;
            //group elements don't have parenturis
            if (string.IsNullOrEmpty(parentNodeId))
            {
                if (nodeName.EndsWith("group"))
                {
                    if (rootReader.Name == nodeName)
                    {
                        string sNodeId = rootReader.GetAttribute(AppHelpers.Calculator.cId);
                        if (sNodeId == nodeId)
                        {
                            descendant
                                = GetCurrentElementWithAttributes(rootReader);
                            return descendant;
                        }
                    }
                }
            }
            return descendant;
        }
        public static bool ReplaceOrInsertDescendantElement(XElement replacementElement,
            string parentURIPattern, IDictionary<string, string> ancestors,
            ref XElement root)
        {
            bool bIsReplaced = false;
            if (!root.HasElements)
            {
                if (parentURIPattern == string.Empty)
                {
                    root.Add(replacementElement);
                    return true;
                }
            }
            if (ancestors != null
                && root != null)
            {
                string sParentNodeName = ContentURI.GetURIPatternPart(
                    parentURIPattern, ContentURI.URIPATTERNPART.node);
                foreach (KeyValuePair<string, string> kvp in ancestors)
                {
                    XElement descendant = GetDescendantUsingURIPattern
                        (root, kvp.Value);
                    if (descendant != null)
                    {
                        string sAddedGroupingNode = AddGroupingNodes(descendant,
                            replacementElement.Name.LocalName);
                        if (kvp.Key == sParentNodeName)
                        {
                            if (!string.IsNullOrEmpty(sAddedGroupingNode))
                            {
                                if (descendant.Elements(sAddedGroupingNode)
                                    .Any())
                                {
                                    descendant = descendant.Element(sAddedGroupingNode);
                                    bIsReplaced = ReplaceOrInsertChildElement(
                                        replacementElement, ref descendant);
                                }
                            }
                            else
                            {
                                if (replacementElement.Name.LocalName
                                    == AppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString())
                                {
                                    if (descendant.Name.LocalName
                                        != AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                                    {   
                                        descendant = descendant.Element(
                                            AppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString());
                                    }
                                    bIsReplaced = ReplaceOrInsertChildElement(
                                        replacementElement, ref descendant);
                                }
                                else if (replacementElement.Name.LocalName
                                    == AppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString())
                                {
                                    if (descendant.Name.LocalName
                                        != AppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                                    {
                                        descendant = descendant.Element(
                                            AppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString());
                                    }
                                    bIsReplaced = ReplaceOrInsertChildElement(
                                        replacementElement, ref descendant);
                                }
                                else if (replacementElement.Name.LocalName
                                    == AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
                                {
                                    if (descendant.Name.LocalName
                                        != AppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                                    {
                                        descendant = descendant.Element(
                                            AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString());
                                    }
                                    bIsReplaced = ReplaceOrInsertChildElement(
                                        replacementElement, ref descendant);
                                }
                                else if (replacementElement.Name.LocalName
                                    == AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                                {
                                    if (descendant.Name.LocalName
                                        != AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                                    {
                                        descendant = descendant.Element(
                                            AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString());
                                    }
                                    bIsReplaced = ReplaceOrInsertChildElement(
                                        replacementElement, ref descendant);
                                }
                                else
                                {
                                    bIsReplaced = ReplaceOrInsertChildElement(
                                        replacementElement, ref descendant);
                                }
                            }
                            break;
                        }
                        else
                        {
                            if (sParentNodeName == string.Empty)
                            {
                                //group nodes are root nodes
                                bIsReplaced = ReplaceOrInsertChildElement(
                                        replacementElement, ref descendant);
                            }
                        }
                    }
                    else
                    {
                        if (sParentNodeName == string.Empty
                            && replacementElement.Name.LocalName.EndsWith("group"))
                        {
                            //group nodes won't have a descendant
                            root.Add(replacementElement);
                            return true;
                        }
                        //need to verify all ancestors of replacementel
                        break;
                    }
                }
            }
            return bIsReplaced;
        }
        private static string AddGroupingNodes(XElement descendant,
            string replacementNodeName)
        {
            string sGroupingNodeInserted = string.Empty;
            if (descendant.Name
                == AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
            {
                if (replacementNodeName
                    == AppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString())
                {
                    if (!descendant.Elements(
                        AppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString())
                        .Any())
                    {
                        XElement outcomes = new XElement(
                            AppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString());
                        descendant.AddFirst(outcomes);
                        sGroupingNodeInserted = AppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString();
                    }
                }
                else
                {
                    if (!descendant.Elements(
                        AppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString())
                        .Any())
                    {
                        XElement operations = new XElement(
                            AppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString());
                        descendant.Add(operations);
                        sGroupingNodeInserted = AppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString();
                    }
                }
            }
            else if (descendant.Name
                == AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {

                if (replacementNodeName
                   == AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                {
                    if (!descendant.Elements(
                        AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString())
                        .Any())
                    {
                        XElement outputs = new XElement(
                            AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString());
                        descendant.AddFirst(outputs);
                        sGroupingNodeInserted = AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString();
                    }
                }
                else
                {
                    if (!descendant.Elements(
                        AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString())
                        .Any())
                    {
                        XElement components = new XElement(
                            AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString());
                        descendant.Add(components);
                        sGroupingNodeInserted = AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString();
                    }
                }
            }
            return sGroupingNodeInserted;
        }
    }
}
