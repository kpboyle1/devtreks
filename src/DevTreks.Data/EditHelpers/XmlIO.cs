using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace DevTreks.Data.EditHelpers
{
    /// <summary>
    ///Purpose:		Abstract xml handling class
    ///Author:		www.devtreks.org
    ///Date:		2013, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class XmlIO
    {
        private XmlIO()
        {
            //static methods used in this class; no constructor 
        }
        
        public static string MakeXPathAbbreviatedQry(string nodeName, string attNameWithAt,
            string value)
        {
            string sQry;
            StringBuilder oStrBldr = new StringBuilder();
            oStrBldr.Append(@"//");
            if (attNameWithAt == string.Empty)
            {
                //rare (i.e. root)
                oStrBldr.Append(nodeName);
            }
            else
            {
                oStrBldr.Append(nodeName);
                oStrBldr.Append("[");
                if (attNameWithAt.StartsWith(Helpers.GeneralHelpers.PARAMETER_DELIMITER))
                {
                    oStrBldr.Append(attNameWithAt);
                }
                else
                {
                    oStrBldr.Append(Helpers.GeneralHelpers.PARAMETER_DELIMITER);
                    oStrBldr.Append(attNameWithAt);
                }
                oStrBldr.Append("='");
                oStrBldr.Append(value);
                oStrBldr.Append("']");
            }
            sQry = oStrBldr.ToString();
            return sQry;
        }
        // i.e. //operation[@Id='1'] + /input[@Id='4']
        public static string MakeXPathExtendedQry(string baseQry, string nodeName,
            string nodeId, string value)
        {
            string sQry;
            StringBuilder oStrBldr = new StringBuilder();
            oStrBldr.Append(baseQry);
            oStrBldr.Append("/");
            if (nodeId == string.Empty)
            {
                //rare
                oStrBldr.Append(nodeName);
            }
            else
            {
                oStrBldr.Append(nodeName);
                oStrBldr.Append("[");
                oStrBldr.Append(nodeId);
                oStrBldr.Append("='");
                oStrBldr.Append(value);
                oStrBldr.Append("']");
            }
            sQry = oStrBldr.ToString();
            return sQry;
        }
        
        public static string MakeXmlDocQry(string nodeName, string attNameWithAt,
            string value)
        {
            string sQry = string.Empty;
            string sBaseQry = MakeXPathAbbreviatedQry(
                nodeName, attNameWithAt, value);
            sQry = string.Concat(sBaseQry,
                Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER,
                Helpers.GeneralHelpers.ROOT_PATH);
            return sQry;
        }
        public static string MakeXmlDocRootQry(string nodeName, string attNameWithAt,
            string value)
        {
            string sQry = string.Empty;
            string sBaseQry = MakeXmlDocQry(
                nodeName, attNameWithAt, value);
            sQry = string.Concat(sBaseQry,
                Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER, 
                Helpers.GeneralHelpers.ROOT_PATH);
            return sQry;
        }
        public static string MakeLinkedViewQry(string nodeName, string attNameWithAt,
            string value, string linkedViewId)
        {
            string sQry = string.Empty;
            string sBaseQry = MakeXmlDocQry(
                nodeName, attNameWithAt, value);
            sQry = string.Concat(sBaseQry,
                Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER,
                Helpers.GeneralHelpers.ROOT_PATH, 
                Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER,
                AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                "[", Helpers.GeneralHelpers.AT_ID, "='", linkedViewId, "']");
            return sQry;
        }
        public static string MakeLinkedViewAttributeQry(string nodeName, string attNameWithAt,
            string value, string atLinkedViewAttributeName, string linkedViewAttributeValue)
        {
            string sQry = string.Empty;
            string sBaseQry = MakeXmlDocQry(
                nodeName, attNameWithAt, value);
            sQry = string.Concat(sBaseQry,
                Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER,
                Helpers.GeneralHelpers.ROOT_PATH,
                Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER,
                AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                "[", atLinkedViewAttributeName, "='", linkedViewAttributeValue, "']");
            return sQry;
        }
        /// <summary>
        /// True if navigator already has the id in the xPathQry
        /// </summary>
        public static bool HasSameId(string xPathQry, string prefix, string urn,
            XPathNavigator navigator)
        {
            bool bHasSameId = false;
            XPathNodeIterator oIterator = null;
            if (xPathQry.IndexOf(prefix) > 0)
            {
                XmlNamespaceManager oNSManager = new XmlNamespaceManager(navigator.NameTable);
                oNSManager.AddNamespace(prefix, urn);
                oIterator = navigator.Select(xPathQry, oNSManager);
            }
            else
            {
                XPathExpression oXPathExp;
                oXPathExp = navigator.Compile(xPathQry);
                oIterator = navigator.Select(oXPathExp);
            }
            if (oIterator != null)
            {
                if (oIterator.Count != 0)
                {
                    bHasSameId = true;
                }
            }
            return bHasSameId;
        }
        
        public static string GetNodeValue(XmlNode node, string attName)
        {
            string sValue = string.Empty;
            XmlElement oElement = (XmlElement)node;
            if (oElement.HasAttribute(attName))
            {
                sValue = oElement.GetAttribute(attName);
            }
            return sValue;
        }
        public static string GetNodeElementValue(XPathNavigator xmlNav, string xPath)
        {
            string sValue = string.Empty;
            XPathExpression oXPathExp;
            oXPathExp = xmlNav.Compile(xPath);
            XPathNodeIterator oIterator = xmlNav.Select(oXPathExp);
            //if more than one count than something is being done wrong 
            if (oIterator.Count >= 1)
            {
                oIterator.MoveNext();
                sValue = oIterator.Current.Value;
            }
            return sValue;
        }
        public static void SetNodeAttribute(string attName, string attValue, ref XmlNode node)
        {
            XmlElement oElement = (XmlElement)node;
            oElement.SetAttribute(attName, attValue);
        }
        public static void SetNodeAttributes(NameValueCollection atts, 
            ref XmlElement element)
        {
            string sAttName = string.Empty;
            string sAttValue = string.Empty;
            for (int i = 0; i < atts.Count; i++)
            {
                sAttName = atts.GetKey(i);
                sAttValue = (string)atts[sAttName];
                element.SetAttribute(sAttName, sAttValue);
            }

        }
        public static void SetNodeAttributes(XmlAttributeCollection atts, ref XmlElement element)
        {
            string sAttName = string.Empty;
            string sAttValue = string.Empty;
            for (int i = 0; i < atts.Count; i++)
            {
                sAttName = atts[i].Name;
                sAttValue = atts[i].Value;
                element.SetAttribute(sAttName, sAttValue);
            }
        }
        public static void SetNodeAttributes(XmlElement fromElement, 
            ref XmlElement toElement)
        {
            string sName = string.Empty;
            string sValue = string.Empty;
            XmlAttributeCollection colAtts = fromElement.Attributes;
            foreach (XmlAttribute oAtt in colAtts)
            {
                sName = oAtt.Name;
                sValue = oAtt.Value;
                toElement.SetAttribute(sName, sValue);
                sName = string.Empty;
                sValue = string.Empty;
            }
            colAtts = null;
        }
        
        public static void GetElementAttributeValue(XmlElement element, 
            string attName, out string attValue)
        {
            attValue = string.Empty;
            if (element.HasAttribute(attName) != false)
            {
                //get the attribute's value
                attValue = element.GetAttribute(attName);
            }
        }
        public static XmlElement GetElement(XmlDocument xmlDoc, string xPathQry)
        {
            XmlElement oElement = null;
            XmlNode oNode = null;
            oNode = xmlDoc.SelectSingleNode(xPathQry);
            if (oNode != null)
            {
                oElement = (XmlElement)oNode;
            }
            return oElement;
        }
        public static XmlElement GetDescendentElement(XmlElement parentEl, string xPathQry)
        {
            XmlElement oElement = null;
            XmlNode oNode = null;
            oNode = parentEl.SelectSingleNode(xPathQry);
            if (oNode != null)
            {
                oElement = (XmlElement)oNode;
            }
            return oElement;
        }
        
        public static void AppendElements(int numberToAppend, int maxTimePeriods, 
            string elementName, string idAttName, string nameAttName, 
            ref XmlElement parentElement, ref XmlDocument xmlDoc)
        {
            XmlElement eNewElement = null;
            for (int t = 1; t <= numberToAppend; t++)
            {
                if (t > maxTimePeriods) break;
                //leave the namespace off the timeperiod for now
                eNewElement = xmlDoc.CreateElement(elementName);
                eNewElement.SetAttribute(idAttName, t.ToString());
                eNewElement.SetAttribute(nameAttName, "Period " + t.ToString());
                parentElement.AppendChild(eNewElement);
            }
        }
        public static XmlNode GetNode(XmlDocument xmlDoc, string xPathQry)
        {
            XmlNode oNode = null;
            oNode = xmlDoc.SelectSingleNode(xPathQry);
            return oNode;
        }
        public static XmlTextReader ConvertStringToReader(string xmlString)
        {
            //create the rest of reader
            NameTable oNameTable = new NameTable();
            XmlNamespaceManager oNameManager = new XmlNamespaceManager(oNameTable);
            XmlParserContext context = new XmlParserContext(null, oNameManager, null, XmlSpace.None);
            //create reader
            XmlTextReader oReader = new XmlTextReader(xmlString, XmlNodeType.Element, context);
            return oReader;
        }
        /// <summary>
        /// Returns just the nodes needed in a full doc which is too large to serve to clients
        /// </summary>
        /// <param name="fullDoc"></param>
        /// <param name="rootQry"></param>
        /// <param name="parentQry"></param>
        /// <param name="partDoc"></param>
        public static void FixFullDoc(XmlDocument fullDoc, string rootQry, string parentQry, string root, out XmlDocument partDoc)
        {
            partDoc = new XmlDocument();
            XmlElement oRootElement = partDoc.CreateElement(root);
            if (fullDoc.DocumentElement.HasChildNodes == true)
            {
                //get the element being appended directly to to the root
                XmlElement oGroupElement = (XmlElement)fullDoc.SelectSingleNode(rootQry).CloneNode(false);
                if (oGroupElement != null)
                {
                    //get the children of this node too
                    AppendChildrenOnly(rootQry, fullDoc, ref oGroupElement);
                    //append the child needed to the parent if it has a good parent query
                    if (parentQry != string.Empty && parentQry != null)
                    {
                        XmlElement oParentElement = (XmlElement)fullDoc.SelectSingleNode(parentQry).CloneNode(false);
                        if (oParentElement != null)
                        {
                            //get the children of this node too
                            AppendChildrenOnly(parentQry, fullDoc, ref oParentElement);
                            oGroupElement.AppendChild(oParentElement);
                        }
                    }
                    //append both to the root element
                    XmlNode oNodeToInsert = partDoc.ImportNode(oGroupElement, true);
                    oRootElement.AppendChild(oNodeToInsert);
                    partDoc.AppendChild(oRootElement);
                }
            }
        }
        /// <summary>
        /// Append children node in an xml document to a referenced element node
        /// </summary>
        /// <param name="rootQry"></param>
        /// <param name="fullDoc"></param>
        /// <param name="element"></param>
        public static void AppendChildrenOnly(string rootQry, XmlDocument fullDoc, ref XmlElement element)
        {
            XPathNodeIterator oGroupIterator = fullDoc.CreateNavigator().Select(rootQry);
            if (oGroupIterator != null)
            {
                while (oGroupIterator.MoveNext())
                {
                    //clone the child
                    XPathNavigator nav2 = oGroupIterator.Current.Clone();
                    if (nav2.HasChildren == true)
                    {
                        XPathNodeIterator oChildrenIterator = nav2.SelectChildren(XPathNodeType.Element);
                        while (oChildrenIterator.MoveNext())
                        {
                            //clone it
                            XmlElement oGroupChild = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(false);
                            if (oGroupChild != null)
                            {
                                //append it to the parent
                                element.AppendChild(oGroupChild);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Remove the first child of the second to last node of the base document submitted
        /// </summary>
        /// <param name="newDocNode">node being appended to lastbaseNode</param>
        /// <param name="lastBaseNode">node where new element is being appended</param>
        /// <param name="baseDoc">document holding lastBaseNode</param>
        public static void RemoveLastNode(XmlNode lastBaseNode, ref XmlDocument baseDoc)
        {
            if (lastBaseNode.HasChildNodes == true)
            {
                if (lastBaseNode.FirstChild.HasChildNodes == true)
                {
                    RemoveLastNode(lastBaseNode.FirstChild, ref baseDoc);
                }
                else
                {
                    lastBaseNode.RemoveChild(lastBaseNode.FirstChild);
                }
            }
            else
            {
                lastBaseNode.RemoveChild(lastBaseNode.FirstChild);
            }
        }
        /// <summary>
        /// Append an element to the first child of the last hierachical nodes contained in basedoc
        /// </summary>
        /// <param name="newDocNode">node being appended to lastbaseNode</param>
        /// <param name="lastBaseNode">node where new element is being appended</param>
        /// <param name="baseDoc">document holding lastBaseNode</param>
        public static void AppendNodeToLastDocNode(XmlElement newDocNode, bool isDeepClone, XmlNode lastBaseNode, ref XmlDocument baseDoc)
        {
            if (lastBaseNode.HasChildNodes == true)
            {
                AppendNodeToLastDocNode(newDocNode, isDeepClone, lastBaseNode.FirstChild, ref baseDoc);
            }
            else
            {
                XmlNode oNodeToInsert = baseDoc.ImportNode(newDocNode, isDeepClone);
                lastBaseNode.AppendChild(oNodeToInsert);
            }
        }
        /// <summary>
        /// Append shallow cloned children of the navigator to the element
        /// If the child is an annuity node, make a deep clone
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="sDeepClone"></param>
        /// <param name="parentElement"></param>
        public static void AppendChildrenOrAnnuitiesOnly(XPathNavigator nav, 
            bool deepClone, string docToCalcXmlPath, XmlDocument baseDoc,
            ref XmlElement parentNode)
        {
            //clone the children
            if (nav.HasChildren == true)
            {
                XPathNodeIterator oChildrenIterator = nav.SelectChildren(XPathNodeType.Element);
                while (oChildrenIterator.MoveNext())
                {
                    XmlElement oChildElement = null;
                    if (oChildrenIterator.Current.LocalName == Helpers.GeneralHelpers.ROOT_PATH)
                    {
                        //xmldoc els are always deeply cloned (or the document model can get out of shape)
                        oChildElement = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(true);
                    }
                    else
                    {
                        oChildElement = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(deepClone);
                        AddLinkedViewToElement(oChildrenIterator, ref oChildElement);
                    }
                    if (oChildElement != null)
                    {
                        //annuity nodes must be deep cloned
                        bool bIsAnnuity = IsAnnuityElement(oChildElement);
                        if (bIsAnnuity == false)
                        {
                            parentNode.AppendChild(oChildElement);
                        }
                        else
                        {
                            //always append deep cloned element
                            XmlElement oAnnuityElement = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(true);
                            parentNode.AppendChild(oAnnuityElement);
                        }
                    }
                }
            }
        }
        public static void AddLinkedViewToElement(
            XPathNodeIterator childrenIterator, ref XmlElement childElement)
        {
            //return the current node along with all childrent linkedviews
            XPathNodeIterator linkedViews 
                = childrenIterator.Current
                .SelectChildren(Helpers.GeneralHelpers.ROOT_PATH, string.Empty);
            if (linkedViews != null)
            {
                while (linkedViews.MoveNext())
                {
                    XmlElement oChildLinkedView = null;
                    if (linkedViews.Current.LocalName
                        == Helpers.GeneralHelpers.ROOT_PATH)
                    {
                        //xmldoc els are always deeply cloned (or the document model can get out of shape)
                        oChildLinkedView = (XmlElement)((IHasXmlNode)linkedViews.Current).GetNode().CloneNode(true);
                        if (oChildLinkedView != null)
                        {
                            childElement.AppendChild(oChildLinkedView);
                        }
                    }
                }
            }
        }
        
                        
        /// <summary>
        /// Append shallow cloned children of the navigator to the element
        /// use descendentDepth to recurse to grandchildren ...
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="sDeepClone"></param>
        /// <param name="parentElement"></param>
        public static void AppendChildren(XPathNavigator nav, bool deepClone,
            ref XmlElement parentNode, int descendentDepth)
        {
            //clone the children
            if (nav.HasChildren == true)
            {
                //decrease descendent depth by one
                descendentDepth--;
                XPathNodeIterator oChildrenIterator = nav.SelectChildren(XPathNodeType.Element);
                while (oChildrenIterator.MoveNext())
                {
                    XmlElement oChildElement = null;
                    if (oChildrenIterator.Current.LocalName == Helpers.GeneralHelpers.ROOT_PATH)
                    {
                        //xmldoc els are always deeply cloned (or the document model can get out of shape)
                        oChildElement = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(true);
                    }
                    else
                    {
                        oChildElement = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(deepClone);
                    }
                    if (oChildElement != null)
                    {
                        //annuity nodes must be deep cloned
                        bool bIsAnnuity = IsAnnuityElement(oChildElement);
                        if (bIsAnnuity == false)
                        {
                            if (descendentDepth == 0
                                || oChildrenIterator.Current.LocalName == Helpers.GeneralHelpers.ROOT_PATH)
                            {
                                parentNode.AppendChild(oChildElement);
                            }
                            else
                            {
                                AppendChildren(oChildrenIterator.Current.CreateNavigator(), 
                                    deepClone, ref oChildElement, descendentDepth);
                                parentNode.AppendChild(oChildElement);
                            }
                        }
                        else
                        {
                            //always append deep cloned element
                            XmlElement oAnnuityElement = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(true);
                            if (descendentDepth == 0)
                            {
                                parentNode.AppendChild(oAnnuityElement);
                            }
                            else
                            {
                                AppendChildren(oChildrenIterator.Current.CreateNavigator(), 
                                    deepClone, ref oChildElement, descendentDepth);
                                parentNode.AppendChild(oChildElement);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// True if this time element is an annuity, or growth series
        /// </summary>
        /// <param name="timeElement"></param>
        /// <returns></returns>
        public static bool IsAnnuityElement(XmlElement timeElement)
        {
            bool bIsAnnuity = false;
            if (timeElement.HasAttribute("Unit"))
            {
                string sAnnuityValue = timeElement.GetAttribute("Unit");
                if (sAnnuityValue.Equals("annuity")
                    || sAnnuityValue.Equals("uniform")
                    || sAnnuityValue.Equals("linear")
                    || sAnnuityValue.Equals("geometric"))
                {
                    bIsAnnuity = true;
                }
            }

            return bIsAnnuity;
        }
        public static void InsertElement(XmlDocument xmlDoc, 
            XmlElement nodeToInsert, XmlNode parentAppendNode)
        {
            XmlNode oImportedNode = xmlDoc.ImportNode(nodeToInsert, false);
            if (oImportedNode != null && parentAppendNode != null)
            {
                parentAppendNode.AppendChild(oImportedNode);
            }
        }
        
    }
}
