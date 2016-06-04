using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace DevTreks.Data.EditHelpers
{
    /// <summary>
    ///Purpose:		Abstract xpath handling class
    ///Author:		www.devtreks.org
    ///Date:		2011, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class XPathIO
    {
        private XPathIO()
        {
            //static methods used in this class; no constructor 
        }
        public static XmlReader ConvertNavigatorToReader(XPathNavigator nav)
        {
            //stream the entire XML document to the XmlReader.
            XmlReader xmlReader = null;
            if (nav != null)
            {
                xmlReader = nav.ReadSubtree();
            }
            return xmlReader;
        }
        public static XPathNavigator ConvertReaderToNoEditNavigator(XmlReader reader)
        {
            XPathDocument navDoc = null;
            if (reader != null)
            {
                navDoc = new XPathDocument(reader);
            }
            return navDoc.CreateNavigator();
        }
       
        public static XmlElement ConvertNavigatorToElement(XPathNavigator nav)
        {
            XmlElement oConvertedNavigator = null;
            //stream the entire XML document to the XmlReader.
            XmlReader xmlReader = ConvertNavigatorToReader(nav);
            if (xmlReader != null)
            {
                using (xmlReader)
                {
                    XmlDocument oDoc = new XmlDocument();
                    oDoc.Load(xmlReader);
                    if (oDoc != null)
                    {
                        oConvertedNavigator = oDoc.DocumentElement;
                    }
                }
            }
            return oConvertedNavigator;
        }
        public static string GetNodeValue(XPathNavigator xmlNav, string xPath, 
            string attName)
        {
            string sValue = string.Empty;
            XPathExpression oXPathExp;
            oXPathExp = xmlNav.Compile(xPath);
            XPathNodeIterator oIterator = xmlNav.Select(oXPathExp);
            if (oIterator.Count >= 1)
            {
                oIterator.MoveNext();
                XPathNavigator nav2 = oIterator.Current.Clone();
                if (nav2.HasAttributes == true)
                {
                    sValue = nav2.GetAttribute(attName, string.Empty);
                }
            }
            return sValue;
        }
        public static string GetAttributeValue(XPathNavigator nav, string attributeName)
        {
            string sAttValue = string.Empty;
            bool bIsOnAttribute = nav.MoveToAttribute(attributeName, string.Empty);
            if (bIsOnAttribute == true)
            {
                sAttValue = nav.Value;
                //move back to the element
                nav.MoveToParent();
            }
            return sAttValue;
        }
        public static void SetAttributeValue(XPathNavigator nav, string xPathNodeQry,
            string attributeName, string newAttValue)
        {
            //use select single node to move to this node
            XPathNavigator oCurrentNavNodePosition = nav.SelectSingleNode(xPathNodeQry);
            //use movetoattribute to move to the attribute
            if (oCurrentNavNodePosition != null)
            {
                SetAttributeValue(oCurrentNavNodePosition, attributeName,
                    newAttValue);
            }
        }
        public static bool SetAttributeValueForExisting(XPathNavigator nav,
            string attributeName, string newAttValue)
        {
            bool bIsOnAttribute = false;
            if (nav.HasAttributes)
            {
                bIsOnAttribute = nav.MoveToAttribute(attributeName, string.Empty);
                //use setvalue to set the attribute's value
                if (bIsOnAttribute == true)
                {
                    nav.SetValue(newAttValue);
                    //move back to the element
                    nav.MoveToParent();
                }
                else
                {
                    //don't create a new attribute
                }
            }
            return bIsOnAttribute;
        }
        public static void SetAttributeValue(XPathNavigator nav, 
            string attributeName, string newAttValue)
        {
            if (nav.HasAttributes)
            {
                bool bIsOnAttribute = nav.MoveToAttribute(attributeName, string.Empty);
                //use setvalue to set the attribute's value
                if (bIsOnAttribute == true)
                {
                    nav.SetValue(newAttValue);
                    //move back to the element
                    nav.MoveToParent();
                }
                else
                {
                    //create a new attribute
                    CreateAttribute(nav, attributeName, newAttValue);
                }
            }
        }
        public static void CreateAttribute(XPathNavigator nav, string attributeName, 
            string newAttValue)
        {
            if (nav.HasAttributes)
            {
                if (string.IsNullOrEmpty(nav.GetAttribute(attributeName, string.Empty)))
                {
                    nav.CreateAttribute(string.Empty, attributeName, string.Empty, newAttValue);
                }
                else
                {
                    nav.MoveToAttribute(attributeName, string.Empty);
                    nav.SetValue(newAttValue);
                    //move back to the element
                    nav.MoveToParent();
                }
            }
        }
        /// <summary>
        /// Append a child element to the xpathdocument
        /// </summary>
        /// <param name="insertToNav">navigator holding parent where the child will be appended</param>
        /// <param name="parentQry">qry to parent</param>
        /// <param name="childElementNav">the child element to append</param>
        public static void AppendChildElement(XPathNavigator insertToNav, string parentQry,
            string prefix, string urn, XPathNavigator childElementNav)
        {
            XPathNavigator oParentElementNav = null;
            if (parentQry.IndexOf(prefix) > 0)
            {
                XmlNamespaceManager oNSManager = new XmlNamespaceManager(insertToNav.NameTable);
                oNSManager.AddNamespace(prefix, urn);
                oParentElementNav = insertToNav.SelectSingleNode(parentQry, oNSManager);
            }
            else
            {
                oParentElementNav = insertToNav.SelectSingleNode(parentQry);
            }
            if (oParentElementNav != null)
            {
                oParentElementNav.AppendChild(childElementNav);
            }
        }
        public static bool ReplaceElement(XPathNavigator navDoc, 
            string elementToReplaceQry, XPathNavigator replacementElement, 
            string prefix, string urn)
        {
            bool bIsReplaced = false;
            XPathNavigator navElementToReplace = null;
            if (elementToReplaceQry.IndexOf(prefix) > 0)
            {
                XmlNamespaceManager oNSManager 
                    = new XmlNamespaceManager(navDoc.NameTable);
                oNSManager.AddNamespace(prefix, urn);
                navElementToReplace = navDoc.SelectSingleNode(
                    elementToReplaceQry, oNSManager);
            }
            else
            {
                navElementToReplace = navDoc.SelectSingleNode(elementToReplaceQry);
            }
            if (navElementToReplace != null)
            {
                navElementToReplace.ReplaceSelf(replacementElement);
                bIsReplaced = true;
            }
            return bIsReplaced;
        }

        public static bool ReplaceXmlDocElement(XPathNavigator insertToNav, string parentQry,
            string prefix, string urn, XPathNavigator childElementNav)
        {
            bool bIsReplaced = false;
            XPathNavigator oParentElementNav = null;
            if (parentQry.IndexOf(prefix) > 0)
            {
                XmlNamespaceManager oNSManager = new XmlNamespaceManager(insertToNav.NameTable);
                oNSManager.AddNamespace(prefix, urn);
                oParentElementNav = insertToNav.SelectSingleNode(parentQry, oNSManager);
            }
            else
            {
                oParentElementNav = insertToNav.SelectSingleNode(parentQry);
            }
            if (oParentElementNav != null)
            {
                //by convention first child should be an xmldoc node holding xmldoc attribute field
                bool bIsXmlDocNode = oParentElementNav.MoveToChild(Helpers.GeneralHelpers.ROOT_PATH, string.Empty);
                if (bIsXmlDocNode)
                {
                    //move to the root node
                    oParentElementNav.MoveToFirstChild();
                    oParentElementNav.ReplaceSelf(childElementNav);
                    bIsReplaced = true;
                }
            }
            return bIsReplaced;
        }
        public static string MakeXmlDoc(XPathNavigator devDocNav)
        {
            string sXmlDoc = string.Empty;
            if (devDocNav.LocalName == string.Empty)
            {
                if (devDocNav.HasChildren)
                {
                    //move off of the root node
                    devDocNav.MoveToFirstChild();
                }
            }
            if (devDocNav.LocalName 
                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                sXmlDoc = string.Concat(Helpers.GeneralHelpers.ROOT_START_NODE,
                   devDocNav.OuterXml,
                   Helpers.GeneralHelpers.ROOT_END_NODE);
            }
            else if (devDocNav.LocalName
                == Helpers.GeneralHelpers.ROOT_PATH)
            {
                sXmlDoc = devDocNav.OuterXml;
            }
            return sXmlDoc;
        }
        public static void InsertNewXmlDocDevDocElement(bool hasChildren,
             ref XPathNavigator insertToNav, string nameSpaceURI,
             XPathNavigator replacementDevDoc)
        {
            XmlWriter xmlDocAttributeWriter = null;

            if (hasChildren)
            {
                //xmldocs always are first among siblings
                xmlDocAttributeWriter = insertToNav.InsertBefore();
            }
            else
            {
                //or first child
                xmlDocAttributeWriter = insertToNav.PrependChild();
            }
            //xmlDocAttributeWriter.WriteStartElement(Helpers.GeneralHelpers.XMLDOCS_ELEMENT_NAME);
            if (replacementDevDoc.LocalName != Helpers.GeneralHelpers.ROOT_PATH)
            {
                xmlDocAttributeWriter.WriteStartElement(Helpers.GeneralHelpers.ROOT_PATH);
            }
            replacementDevDoc.WriteSubtree(xmlDocAttributeWriter);
            if (replacementDevDoc.LocalName != Helpers.GeneralHelpers.ROOT_PATH)
            {
                xmlDocAttributeWriter.WriteEndElement();
            }
            xmlDocAttributeWriter.WriteEndElement();
            xmlDocAttributeWriter.Dispose();
        }
        public static bool ReplaceXmlDocElement(XPathNavigator insertToNav,
           string nameSpaceURI, XPathNavigator navCalcNode)
        {
            bool bIsReplaced = false;
            bool bHasMovedInsertToNav = false;
            if (insertToNav.LocalName
                != Helpers.GeneralHelpers.ROOT_PATH)
            {
                bHasMovedInsertToNav = insertToNav.MoveToFirstChild();
            }
            if (insertToNav.LocalName
                == Helpers.GeneralHelpers.ROOT_PATH)
            {
                bool bHasMovedNavCalcNode = false;
                if (navCalcNode.LocalName
                    != Helpers.GeneralHelpers.ROOT_PATH)
                {
                    bHasMovedNavCalcNode = navCalcNode.MoveToFirstChild();
                }
                if (navCalcNode.LocalName
                    == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    insertToNav.ReplaceSelf(navCalcNode);
                    bIsReplaced = true;
                    navCalcNode.MoveToParent();
                    insertToNav.MoveToParent();
                }
                else
                {
                    if (bHasMovedNavCalcNode) navCalcNode.MoveToParent();
                }
            }
            else
            {
                if (bHasMovedInsertToNav) insertToNav.MoveToParent();
            }
            return bIsReplaced;
        }
        public static bool ReplaceXmlDocRootElement(ref XPathNavigator navDocToCalcNode,
          string nameSpaceURI, XPathNavigator navCalcNode)
        {
            bool bIsReplaced = false;
            //move to the root node
            //if (navDocToCalcNode.LocalName
            //    == Helpers.GeneralHelpers.XMLDOCS_ELEMENT_NAME)
            //{
            //    navDocToCalcNode.MoveToChild(Helpers.GeneralHelpers.ROOT_PATH, 
            //        string.Empty);
            //}
            if (navDocToCalcNode.LocalName
                == Helpers.GeneralHelpers.ROOT_PATH
                && navCalcNode.LocalName
                == Helpers.GeneralHelpers.ROOT_PATH)
            {
                navDocToCalcNode.ReplaceSelf(navCalcNode);
                bIsReplaced = true;
            }
            return bIsReplaced;
        }
        public static bool ReplaceOrInsertDevDocElement(XPathNavigator insertToNav, 
            string nameSpaceURI, XPathNavigator replacementDevDoc, 
            string linkedViewsIds, ref IList<string> deletionIds)
        {
            bool bIsReplaced = false;
            bool bHasMoved = false;
            //move to the xmldoc element, if one exists
            if (insertToNav.LocalName
                == Helpers.GeneralHelpers.ROOT_PATH)
            {
                //move to an existing linkedview
                bHasMoved = insertToNav.MoveToFirstChild();
                if (insertToNav.LocalName
                    == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    string sId = replacementDevDoc.GetAttribute(AppHelpers.Calculator.cId, nameSpaceURI);
                    string sDevDocId = insertToNav.GetAttribute(AppHelpers.Calculator.cId, nameSpaceURI);
                    if (string.IsNullOrEmpty(sDevDocId) == false)
                    {
                        GetSiblingDevDocsToDelete(sDevDocId, linkedViewsIds, 
                            ref deletionIds);
                        if (sId.Equals(sDevDocId))
                        {
                            insertToNav.ReplaceSelf(replacementDevDoc);
                            bIsReplaced = true;
                        }
                        else
                        {
                            while (insertToNav.MoveToNext())
                            {
                                sDevDocId = insertToNav.GetAttribute(AppHelpers.Calculator.cId, nameSpaceURI);
                                if (sId.Equals(sDevDocId))
                                {
                                    insertToNav.ReplaceSelf(replacementDevDoc);
                                    bIsReplaced = true;
                                }
                                GetSiblingDevDocsToDelete(sDevDocId, linkedViewsIds, 
                                    ref deletionIds);
                            }
                        }
                    }
                    if (bIsReplaced == false)
                    {
                        //insert a new linkedview
                        insertToNav.InsertBefore(replacementDevDoc);
                        bIsReplaced = true;
                    }
                    insertToNav.MoveToParent();
                    insertToNav.MoveToParent();
                    insertToNav.MoveToParent();
                }
                else
                {
                    if (bHasMoved) insertToNav.MoveToParent();
                    //prepend the linkedview
                    insertToNav.PrependChild(replacementDevDoc);
                    insertToNav.MoveToParent();
                    insertToNav.MoveToParent();
                }
            }
            else
            {
                if (bHasMoved) insertToNav.MoveToParent();
                //insert <xmldoc><root></root></xmldoc>
                insertToNav.PrependChild(Helpers.GeneralHelpers.ROOT_NODE);
                //bHasMoved = insertToNav.MoveToFirstChild();
                bHasMoved = insertToNav.MoveToFirstChild();
                //prepend the linkedview
                insertToNav.PrependChild(replacementDevDoc);
                bIsReplaced = true;
                insertToNav.MoveToParent();
                insertToNav.MoveToParent();
            }
            return bIsReplaced;
        }
        private static void GetSiblingDevDocsToDelete(string siblingDevDocId,
            string linkedViewsIds, ref IList<string> deletionIds)
        {
            //clean up the existing linkedviewpack by removing linkedviews that aren't
            //part of the db linkedviewsids
            if (!string.IsNullOrEmpty(linkedViewsIds))
            {
                string[] arrLinkViewsIds = linkedViewsIds.Split(Helpers.GeneralHelpers.PARAMETER_DELIMITERS);
                int i = 0;
                if (arrLinkViewsIds != null)
                {
                    int iLength = arrLinkViewsIds.Length;
                    string sLinkedViewSiblingDevDocId = string.Empty;
                    string sDeletionDevDocId = string.Empty;
                    for (i = 0; i < iLength; i++)
                    {
                        sLinkedViewSiblingDevDocId = arrLinkViewsIds[i];
                        sDeletionDevDocId = siblingDevDocId;
                        if (sLinkedViewSiblingDevDocId.Equals(siblingDevDocId))
                        {
                            sDeletionDevDocId = string.Empty;
                            break;
                        }
                    }
                    if (sDeletionDevDocId != string.Empty)
                    {
                        deletionIds.Add(sDeletionDevDocId);
                    }
                }
            }
        }
        public static void DeleteSiblingDevDocs(XPathNavigator insertToNav,
            IList<string> deletionIds)
        {
            if (deletionIds != null)
            {
                string sQry = string.Empty;
                foreach (string devDocId in deletionIds)
                {
                    sQry = XmlIO.MakeLinkedViewQry(insertToNav.LocalName, Helpers.GeneralHelpers.AT_ID,
                        insertToNav.GetAttribute(AppHelpers.Calculator.cId, string.Empty), devDocId);
                    XPathNavigator navToDelete = insertToNav.SelectSingleNode(sQry);
                    if (navToDelete != null)
                    {
                        navToDelete.DeleteSelf();
                    }
                }
            }
        }
        public static XPathNavigator GetElement(ContentURI uri,
            string prefix, string urn, string docPath, string xPath)
        {
            XPathNavigator navNodeNeeded = null;
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, docPath))
            {
                XmlDocument oDoc = new XmlDocument();
                XmlReader reader = Helpers.FileStorageIO.GetXmlReader(uri, docPath);
                if (reader != null)
                {
                    using (reader)
                    {
                        oDoc.Load(reader);
                    }
                    navNodeNeeded = GetElement(prefix, urn, oDoc.CreateNavigator(),
                        xPath);
                }
            }
            return navNodeNeeded;
        }
        public static XPathNavigator GetElement(string prefix, string urn,
            XPathNavigator nav, string xPath)
        {
            XPathNavigator oElementNav = null;
            if (xPath.IndexOf(prefix) > 0)
            {
                XmlNamespaceManager oNSManager = new XmlNamespaceManager(nav.NameTable);
                oNSManager.AddNamespace(prefix, urn);
                oElementNav = nav.SelectSingleNode(xPath, oNSManager);
            }
            else
            {
                oElementNav = nav.SelectSingleNode(xPath);
            }
            return oElementNav;
        }
        public static string GetElementAsString(string prefix, string urn,
            XPathNavigator nav, string xPath)
        {
            string sOuterXml = string.Empty;
            XPathNavigator oElementNav = null;
            if (xPath.IndexOf(prefix) > 0)
            {
                XmlNamespaceManager oNSManager = new XmlNamespaceManager(nav.NameTable);
                oNSManager.AddNamespace(prefix, urn);
                oElementNav = nav.SelectSingleNode(xPath, oNSManager);
            }
            else
            {
                oElementNav = nav.SelectSingleNode(xPath);
            }
            if (oElementNav != null)
            {
                sOuterXml = oElementNav.OuterXml;
            }
            return sOuterXml;
        }
        public static void AppendChildElement(string prefix, string urn,
            XPathNavigator fromNav, string newChildQry,
            XPathNavigator toNav, string parentQry)
        {
            XPathNavigator oParentElementNav = GetElement(prefix, urn,
                toNav, parentQry);
            XPathNavigator oNewChildElementNav = GetElement(prefix, urn,
                fromNav, newChildQry);
            if (oParentElementNav != null
                && oNewChildElementNav != null)
            {
                oParentElementNav.AppendChild(oNewChildElementNav);
            }
        }
        public static void AddAttributesThatExist(XPathNavigator fromNav, 
           XPathNavigator toNav)
        {
            if (fromNav != null
                && toNav != null)
            {
                if (fromNav.HasAttributes)
                {
                    fromNav.MoveToFirstAttribute();
                    if (fromNav.Name != AppHelpers.Calculator.cId
                        && fromNav.Name != AppHelpers.Calculator.cName)
                    {
                        SetAttributeValueForExisting(toNav, fromNav.Name,
                            fromNav.Value);
                    }
                    while (fromNav.MoveToNextAttribute())
                    {
                        if (fromNav.Name != AppHelpers.Calculator.cId
                            && fromNav.Name != AppHelpers.Calculator.cName)
                        {
                            SetAttributeValueForExisting(toNav, fromNav.Name,
                                fromNav.Value);
                        }
                    }
                    fromNav.MoveToParent();
                }
            }
        }
        public static void AddAllNewAttributes(
           XPathNavigator fromNav, XPathNavigator toNav)
        {
            if (fromNav != null
                && toNav != null)
            {
                string sFromNodeName = fromNav.Name;
                RemoveAllAttributes(toNav);
                if (fromNav.HasAttributes)
                {
                    fromNav.MoveToFirstAttribute();
                    toNav.CreateAttribute(string.Empty, fromNav.Name, 
                        string.Empty, fromNav.Value);
                    while (fromNav.MoveToNextAttribute())
                    {
                        toNav.CreateAttribute(string.Empty, fromNav.Name,
                            string.Empty, fromNav.Value);
                    }
                    if (fromNav.Name != sFromNodeName)
                    {
                        fromNav.MoveToParent();
                    }
                }
            }
        }
        public static void RemoveAllAttributes(XPathNavigator toNav)
        {
            if (toNav.HasAttributes)
            {
                while (toNav.MoveToFirstAttribute())
                {
                    toNav.DeleteSelf();
                }
                //will move to parent when last att is deleted
            }
        }
        public static void AddAttributes(string prefix, string urn,
           XPathNavigator fromNav, string fromNodeQry,
           XPathNavigator toNav, string toNodeQry)
        {
            XPathNavigator oToElementNav = GetElement(prefix, urn,
                toNav, toNodeQry);
            AddAttributes(prefix, urn, fromNav, fromNodeQry,
                oToElementNav);
        }
        public static void AddAttributes(string prefix, string urn,
           XPathNavigator fromNav, string fromNodeQry,
           XPathNavigator toNav)
        {
            XPathNavigator oFromElementNav = GetElement(prefix, urn,
                fromNav, fromNodeQry);
            if (oFromElementNav != null
                && toNav != null)
            {
                //remove common attributes with agmachinery (Id, Name, Label, Description)
                RemoveStandardAttributes(oFromElementNav, prefix, urn);
                if (oFromElementNav.HasAttributes)
                {
                    oFromElementNav.MoveToFirstAttribute();
                    SetAttributeValue(toNav, oFromElementNav.Name,
                        oFromElementNav.Value);
                    while (oFromElementNav.MoveToNextAttribute())
                    {
                        SetAttributeValue(toNav, oFromElementNav.Name,
                            oFromElementNav.Value);
                    }
                }
            }
        }
        public static void RemoveStandardAttributes(XPathNavigator elementNav,
            string prefix, string urn)
        {
            if (elementNav != null)
            {
                bool bHasMoved = false;
                if (urn != string.Empty)
                {
                    //delete standard atts
                    bHasMoved = elementNav.MoveToAttribute(AppHelpers.Calculator.cId, urn);
                    if (bHasMoved) elementNav.DeleteSelf();
                    bHasMoved = elementNav.MoveToAttribute("Name", urn);
                    if (bHasMoved) elementNav.DeleteSelf();
                    bHasMoved = elementNav.MoveToAttribute("Label", urn);
                    if (bHasMoved) elementNav.DeleteSelf();
                    bHasMoved = elementNav.MoveToAttribute("Description", urn);
                    if (bHasMoved) elementNav.DeleteSelf();
                }
                else
                {
                    //delete id att
                    bHasMoved = elementNav.MoveToAttribute(AppHelpers.Calculator.cId, string.Empty);
                    if (bHasMoved) elementNav.DeleteSelf();
                    //delete name att
                    bHasMoved = elementNav.MoveToAttribute("Name", string.Empty);
                    if (bHasMoved) elementNav.DeleteSelf();
                    //delete label att
                    bHasMoved = elementNav.MoveToAttribute("Label", string.Empty);
                    if (bHasMoved) elementNav.DeleteSelf();
                    //delete description
                    bHasMoved = elementNav.MoveToAttribute("Description", string.Empty);
                    if (bHasMoved) elementNav.DeleteSelf();
                }
            }
        }
        public static bool DeleteChildrenNodes(XPathNavigator deleteFromNav,
            string xPathToParent, string prefix, string urn)
        {
            bool bHasDeleted = false;
            XPathNavigator oElementNav = GetElement(prefix, urn,
                deleteFromNav, xPathToParent); ;
            DeleteChildrenNodes(oElementNav, ref bHasDeleted);
            return bHasDeleted;
        }
        private static void DeleteChildrenNodes(XPathNavigator oElementNav, 
            ref bool hasDeletions)
        {
            if (oElementNav.HasChildren)
            {
                oElementNav.MoveToFirstChild();
                oElementNav.DeleteSelf();
                hasDeletions = true;
                //recurse to delete remaining children
                DeleteChildrenNodes(oElementNav, ref hasDeletions);
            }
        }
        public static bool DeleteNode(XPathNavigator deleteFromNav, string xPath,
            string prefix, string urn)
        {
            bool bSuccess = false;
            XPathNavigator oElementNav = GetElement(prefix, urn,
                deleteFromNav, xPath);
            if (oElementNav != null)
            {
                if (oElementNav.CanEdit)
                {
                    oElementNav.DeleteSelf();
                    bSuccess = true;
                }
            }
            return bSuccess;
        }
        public static bool UpdateNode(XPathNavigator updateNav, string xPath,
            string attName, string attValue, string prefix, string urn)
        {
            bool bSuccess = false;
            XPathNavigator oElementNav = GetElement(prefix, urn,
                updateNav, xPath);
            if (oElementNav != null)
            {
                bool bIsOnAttribute = oElementNav.MoveToAttribute(attName, 
                    string.Empty);
                //use setvalue to set the attribute's value
                if (bIsOnAttribute == true)
                {
                    oElementNav.SetValue(attValue);
                    bSuccess = true;
                }
                else
                {
                    //create a new attribute
                    CreateAttribute(oElementNav, attName, attValue);
                    bSuccess = true;
                }
            }
            return bSuccess;
        }
        public static bool UpdateChildrenIds(XPathNavigator parentElementNav,
            string parentNodeName, string parentId, string childAttName)
        {
            bool bHasUpdatedChildren = false;
            bool bHasMoved = false;
            if (parentElementNav.HasChildren)
            {
                parentElementNav.MoveToFirstChild();
                if (parentElementNav.HasAttributes)
                {
                    bHasMoved = parentElementNav.MoveToAttribute(childAttName, string.Empty);
                    if (bHasMoved)
                    {
                        parentElementNav.SetValue(parentId);
                        parentElementNav.MoveToParent();
                        bHasUpdatedChildren = true;
                    }
                }
                else
                {
                    //check for grouping nodes (i.e. outputs or operations)
                    UpdateChildrenIds(parentElementNav, parentNodeName, parentId, childAttName);
                }
                while (parentElementNav.MoveToNext())
                {
                    if (parentElementNav.HasAttributes)
                    {
                        bHasMoved = parentElementNav.MoveToAttribute(childAttName, string.Empty);
                        if (bHasMoved)
                        {
                            parentElementNav.SetValue(parentId);
                            parentElementNav.MoveToParent();
                            bHasUpdatedChildren = true;
                        }
                    }
                    else
                    {
                        //check for grouping nodes (i.e. outputs or operations)
                        UpdateChildrenIds(parentElementNav, parentNodeName, parentId, childAttName);
                    }
                }
                //move back to the starting element
                parentElementNav.MoveToParent();
            }
            return bHasUpdatedChildren;
        }
        public static bool UpdateChildrenAttributeValue(XPathNavigator updateNav, 
            string parentXPath, string prefix, string urn, 
            string childAttName, string childAttValue)
        {
            bool bIsUpdated = false;
            XPathNavigator oParentElementNav = GetElement(prefix, urn,
                updateNav, parentXPath);
            if (oParentElementNav != null)
            {
                if (oParentElementNav.HasChildren)
                {
                    bool bHasMoved = false;
                    oParentElementNav.MoveToFirstChild();
                    if (oParentElementNav.HasAttributes)
                    {
                        bHasMoved = oParentElementNav.MoveToAttribute(childAttName, string.Empty);
                        if (bHasMoved)
                        {
                            oParentElementNav.SetValue(childAttValue);
                            bIsUpdated = true;
                            oParentElementNav.MoveToParent();
                        }
                    }
                    while (oParentElementNav.MoveToNext())
                    {
                        if (oParentElementNav.HasAttributes)
                        {
                            bHasMoved = oParentElementNav.MoveToAttribute(childAttName, string.Empty);
                            if (bHasMoved)
                            {
                                oParentElementNav.SetValue(childAttValue);
                                bIsUpdated = true;
                                oParentElementNav.MoveToParent();
                            }
                        }
                    }
                }
            }
            return bIsUpdated;
        }
        public static void GetAppNamespaces(XPathNavigator devTrekNav, string standardPrefix,
            out string prefix, out string urn)
        {
            prefix = string.Empty;
            urn = string.Empty;
            //moves to the root
            if (devTrekNav.HasChildren) devTrekNav.MoveToFirstChild();
            //moves to the first node (by DevTreks convention, the only possible node that will have a sqlxml-generated prefix)
            if (devTrekNav.HasChildren)
            {
                devTrekNav.MoveToFirstChild();
                bool bHasMoved = devTrekNav.MoveToNamespace(standardPrefix);
                if (bHasMoved)
                {
                    urn = devTrekNav.GetNamespace(standardPrefix);
                    prefix = standardPrefix;
                }
            }
            //move back to the root node
            devTrekNav.MoveToRoot();
        }
        public static XmlElement GetElement(XPathNavigator nav, string xPathToNode,
            bool cloneDeep)
        {
            XmlElement oElement = null;
            XPathExpression oXPathExp;
            oXPathExp = nav.Compile(xPathToNode);
            XPathNodeIterator oIterator = nav.Select(oXPathExp);
            if (oIterator.Count == 1)
            {
                oIterator.MoveNext();
                oElement = (XmlElement)((IHasXmlNode)oIterator.Current).GetNode().CloneNode(cloneDeep);
            }
            return oElement;
        }
        public static void DeleteAttribute(string attName, ref XPathNavigator selectedElementNav)
        {
            if (selectedElementNav.HasAttributes)
            {
                bool bHasMoved = selectedElementNav.MoveToAttribute(attName, string.Empty);
                if (bHasMoved == true)
                {
                    //if successful, repositions on parent
                    selectedElementNav.DeleteSelf();
                }
            }
        }
        public static XPathNavigator MakeSameNavigator(XPathNavigator oldNavigator)
        {
            XPathNavigator oNewNavigator = null;
            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(oldNavigator.OuterXml);
            oNewNavigator = oDoc.CreateNavigator();
            oDoc = null;
            return oNewNavigator;
        }
        /// <summary>
        /// Get the grandparent id of a node referenced by xPath query
        /// </summary>
        /// <param name="xmlDoc">The document containing parentid</param>
        /// <param name="xPath">The path to the child of the nodes needed</param>
        /// <param name="parentAttName">The local name of the attribute holding the parent id to return</param>
        /// <param name="parentId"></param>The parent id of the xPath qry node</param>
        /// <param name="grandParentAttName">The local name of the attribute holding the grandparent id to return</param>
        /// <param name="grandParentId">The parent id of the xPath qry node</param>
        /// <returns>void with two output parameters</returns>
        public static void GetAncestorId(XPathNavigator xmlNav, string xPath, string parentAttName, string ancestorNodeName, out string parentId)
        {
            parentId = "";
            XPathExpression oXPathExp;
            oXPathExp = xmlNav.Compile(xPath);
            XPathNodeIterator oParentIterator = xmlNav.Select(oXPathExp);
            if (oParentIterator.Count == 1)
            {
                oParentIterator.MoveNext();
                XPathNavigator oNav2 = oParentIterator.Current.Clone();
                XPathNodeIterator oAncestorIterator = oNav2.SelectAncestors(ancestorNodeName, string.Empty, false);
                if (oAncestorIterator.Count == 1)
                {
                    oAncestorIterator.MoveNext();
                    XPathNavigator oNav3 = oAncestorIterator.Current.Clone();
                    parentId = oNav3.GetAttribute(parentAttName, string.Empty);
                }
            }
        }
        public static void GetAncestorIds(XPathNavigator xmlNav, string xPath,
            int numberOfParents, out int ancestor1Id, out int ancestor2Id, 
            out int ancestor3Id)
        {
            ancestor1Id = 0;
            ancestor2Id = 0;
            ancestor3Id = 0;
            string sAncestorId = string.Empty;
            XPathExpression oXPathExp;
            oXPathExp = xmlNav.Compile(xPath);
            XPathNavigator navCurrentNode = xmlNav.SelectSingleNode(oXPathExp);
            if (navCurrentNode != null)
            {
                int i = 1;
                bool bHasMoved = false;
                for (i = 1; i <= numberOfParents; i++)
                {
                    bHasMoved = navCurrentNode.MoveToParent();
                    if (bHasMoved)
                    {
                        if (i == 0)
                        {
                            sAncestorId = navCurrentNode.GetAttribute(AppHelpers.Calculator.cId, string.Empty);
                            ancestor1Id = Helpers.GeneralHelpers.ConvertStringToInt(sAncestorId);
                        }
                        else if (i == 1)
                        {
                            sAncestorId = navCurrentNode.GetAttribute(AppHelpers.Calculator.cId, string.Empty);
                            ancestor2Id = Helpers.GeneralHelpers.ConvertStringToInt(sAncestorId);
                        }
                        else if (i == 2)
                        {
                            sAncestorId = navCurrentNode.GetAttribute(AppHelpers.Calculator.cId, string.Empty);
                            ancestor3Id = Helpers.GeneralHelpers.ConvertStringToInt(sAncestorId);
                        }
                    }
                }
                navCurrentNode = null;
            }
        }
        public static string GetNodeValue(XPathNavigator xmlNav, string xPath, 
            string attName, string nsURI)
        {
            string sValue = string.Empty;
            XPathExpression oXPathExp;
            oXPathExp = xmlNav.Compile(xPath);
            XPathNodeIterator oIterator = xmlNav.Select(oXPathExp);
            if (oIterator.Count >= 1)
            {
                oIterator.MoveNext();
                XPathNavigator nav2 = oIterator.Current.Clone();
                if (nav2.HasAttributes == true)
                {
                    sValue = nav2.GetAttribute(attName, nsURI);
                }
            }
            return sValue;
        }
        public static IDictionary<string, string> GetNameValueList(ContentURI uri,
            string docPath, string nodeName)
        {
            IDictionary<string, string> lstUpdates = new Dictionary<string, string>();
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, docPath))
            {
                //stored as an xml doc <root><nodename Id="Name" Value="Value" ...
                XPathDocument oUpdatesDoc = null;
                XmlReader reader = Helpers.FileStorageIO.GetXmlReader(uri, docPath);
                if (reader != null)
                {
                    using (reader)
                    {
                        oUpdatesDoc = new XPathDocument(reader);
                    }
                }
                if (oUpdatesDoc != null)
                {
                    XPathNavigator navUpdatesDoc = oUpdatesDoc.CreateNavigator();
                    //move to the root node
                    navUpdatesDoc.MoveToFirstChild();
                    //move to the first list node
                    navUpdatesDoc.MoveToFirstChild();
                    string sName = navUpdatesDoc.GetAttribute(AppHelpers.Calculator.cId,
                        string.Empty);
                    string sValue = navUpdatesDoc.GetAttribute(Helpers.GeneralHelpers.VALUE,
                        string.Empty);
                    if (sName != string.Empty
                        && sValue != string.Empty)
                    {
                        lstUpdates.Add(sName, sValue);
                    }
                    while (navUpdatesDoc.MoveToNext(XPathNodeType.Element))
                    {
                        if (navUpdatesDoc.LocalName == nodeName)
                        {
                            sName = navUpdatesDoc.GetAttribute(AppHelpers.Calculator.cId,
                                string.Empty);
                            sValue = navUpdatesDoc.GetAttribute(Helpers.GeneralHelpers.VALUE,
                                string.Empty);
                            if (sName != string.Empty
                                && sValue != string.Empty)
                            {
                                lstUpdates.Add(sName, sValue);
                            }
                            sName = string.Empty;
                            sValue = string.Empty;
                        }
                    }
                }
            }
            return lstUpdates;
        }
        public static void SaveNameValueList(ContentURI uri,
            IDictionary<string, string> lstUpdates,
            string docPath, string nodeName)
        {
            if (lstUpdates.Count > 0)
            {
                string sValue = string.Empty;
                XmlDocument oUpdatesDoc = new XmlDocument();
                oUpdatesDoc.LoadXml(Helpers.GeneralHelpers.ROOT_NODE);
                XPathNavigator navDoc = oUpdatesDoc.CreateNavigator();
                //move to the root node
                navDoc.MoveToFirstChild();
                //use an xmlwriter to write remaining nodes
                XmlWriter oUpdateElWriter = null;
                foreach (KeyValuePair<string, string> kvp in lstUpdates)
                {
                    oUpdateElWriter = navDoc.AppendChild();
                    oUpdateElWriter.WriteStartElement(nodeName);
                    oUpdateElWriter.WriteAttributeString(AppHelpers.Calculator.cId,
                        kvp.Key);
                    oUpdateElWriter.WriteAttributeString(Helpers.GeneralHelpers.VALUE,
                        kvp.Value);
                    oUpdateElWriter.WriteEndElement();
                    oUpdateElWriter.Dispose();
                    string sErrorMsg = string.Empty;
                    Helpers.FileStorageIO fileStorageIO = new Helpers.FileStorageIO();
                    fileStorageIO.SaveXmlInURI(uri, oUpdatesDoc, docPath,
                        out sErrorMsg);
                }
            }
        }
        public static void GetLastNodeIdAndNameFromXPathQuery(string xPathQry,
            ref string nodeId, ref string nodeName)
        {
            string sPathQry = xPathQry;
            if (xPathQry.EndsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER))
            {
                sPathQry = xPathQry.TrimEnd(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITERS);
            }
            string sLastQry = Helpers.GeneralHelpers.GetLastSubString(sPathQry, Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
            if (!string.IsNullOrEmpty(sLastQry))
            {
                if (sLastQry.EndsWith("]"))
                {
                    //is using an Id= in qry
                    nodeName = Helpers.GeneralHelpers.GetSubString(0, sLastQry, "[");
                    nodeId = Helpers.GeneralHelpers.GetLastSubString(sLastQry, "=");
                    if (!string.IsNullOrEmpty(nodeId))
                    {
                        char[] qryDelimiter = new char[] { ']' };
                        nodeId = nodeId.TrimEnd(qryDelimiter);
                        nodeId = nodeId.Replace("'", string.Empty);
                    }
                }
                else
                {
                    nodeName = sLastQry;
                }
            }
        }
    }
}
