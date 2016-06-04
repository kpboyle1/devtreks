using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities used to support DevTreks 
    ///             standard addins and extensions.
    ///Author:		www.devtreks.org
    ///Date:		2011, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    /// NOTES:      
    public class AddInHelperLinq
    {
        public AddInHelperLinq() { }
       
        #region "addin/extension state management"
        public static void SetDocToCalcState(ref string errorMsg, 
            ref ContentURI docToCalcURI, ContentURI calcDocURI,
            ref XElement docToCalcDocument, ref XElement docToCalcElement)
        {
            string sDocToCalcNodeName = DevTreks.Data.ContentURI.GetURIPatternPart(
                docToCalcURI.URIPattern,
                DevTreks.Data.ContentURI.URIPATTERNPART.node);
            if (!sDocToCalcNodeName.Equals(
                DevTreks.Data.AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString()))
            {
                if (Helpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                    docToCalcURI.URIDataManager.TempDocPath))
                {
                    docToCalcDocument = Helpers.FileStorageIO.LoadXmlElement(docToCalcURI,
                        docToCalcURI.URIDataManager.TempDocPath);
                    docToCalcElement =
                        EditHelpers.XmlLinq.GetDescendantUsingURIPattern(
                            docToCalcDocument, docToCalcURI.URIPattern);
                    if (docToCalcElement == null
                        && docToCalcDocument != null)
                    {
                        //could be a custom linkedview or devpackpart (with any nodename)
                        if (docToCalcDocument.Elements().FirstOrDefault().HasElements)
                        {
                            docToCalcElement = docToCalcDocument
                                .Elements()
                                .FirstOrDefault()
                                .Elements()
                                .FirstOrDefault();
                        }
                    }
                }
            }
        }
        
        public static void ValidateSerializationAttributes(ref XElement calculationsElement)
        {
            //an attribute with an empty value in calculationsElement can't be parsed 
            //(i.e. to double) during deserialization (only solution is to delete and start over)
            foreach (XAttribute att in calculationsElement.Attributes().AsParallel())
            {
                if (string.IsNullOrEmpty(att.Value))
                {
                    if (att.Name.LocalName.IndexOf(AppHelpers.Calculator.cDate) > 0)
                    {
                        calculationsElement.SetAttributeValue(att.Name, 
                            Helpers.GeneralHelpers.GetDateShortNow().ToString());
                    }
                    else
                    {
                        calculationsElement.SetAttributeValue(att.Name, "0");
                    }
                }
            }
        }
        #endregion

        #region "update calculations"

        
        private static void SetDescendantXmlDocAttribute(string docToCalcURIPattern,
            XElement calculationsElement, ref XElement docToCalcElement,
            ref string errorMsg)
        {
            //replace the current xmldoc in calculationsElement into docToCalcElement
            bool bIsReplaced = EditHelpers.XmlLinq.ReplaceXmlDocElement(
                ref docToCalcElement, calculationsElement);
        }
        
        
        private static bool AnalysisNeedsDevPackPart(IList<string> urisToAnalyze)
        {
            bool bAnalysisNeedsDevPackPart = true;
            string sCurrentNodeName = string.Empty;
            int i = 0;
            foreach (string uriToAnalyze in urisToAnalyze)
            {
                //uriToAnalyze is a delimited nodeName_nodeId string
                sCurrentNodeName = Helpers.GeneralHelpers.GetSubString(0, uriToAnalyze,
                    Helpers.GeneralHelpers.FILENAME_DELIMITER);
                //if the list contains at least two devpacks, then it's a devpack-devpack parent-child update
                //don't update the devpackparts in the list as well (number could be 100s or 1000s, 
                //which, at this stage of developmen,t is too server intensive)
                if (sCurrentNodeName == AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                {
                    i++;
                    if (i == 2)
                    {
                        bAnalysisNeedsDevPackPart = false;
                        break;
                    }
                }
            }
            return bAnalysisNeedsDevPackPart;
        }
        public static void UpdateNewValue(bool needsDbUpdate,
            string uriPattern, string attName, string attValue, 
            string dataType, string stepNumber, ref XElement toElement,
            IDictionary<string, string> updates)
        {
            string sNewAttValue = string.Empty;
            //if the attribute doesn't exist in toElement, don't add it (don't add join fields to base docs)
            //this is safe because the toElement was generated from a db schema
            if (toElement.Attribute(attName) != null)
            {
                sNewAttValue = attValue;
                string sOldAttValue = EditHelpers.XmlLinq.GetAttributeValue(toElement, attName);
                if (string.IsNullOrEmpty(sNewAttValue) == false)
                {
                    if (sNewAttValue.Equals(sOldAttValue) == false)
                    {
                        //don't allow formatting-only changes to numbers (schemas ignore anyway)
                        sNewAttValue = AddInHelper.NoFormattingOnly(
                            sNewAttValue, sOldAttValue, dataType);
                        if (sNewAttValue != string.Empty)
                        {
                            EditHelpers.XmlLinq.SetAttributeValue(toElement,
                                attName, sNewAttValue);
                        }
                    }
                    else
                    {
                        sNewAttValue = string.Empty;
                    }
                }
                else
                {
                    sNewAttValue = string.Empty;
                }
            }
            if (needsDbUpdate)
            {
                AddInHelper.AddToDbList(uriPattern, attName, sNewAttValue,
                    dataType, stepNumber, updates);
            }
        }
        public static string GetNewValue(string fromAttName,
            XElement fromElement, string toAttName, string dataType,
            ref XElement toElement, bool needsUpdateList)
        {
            string sNewAttValue = string.Empty;
            //if the attribute doesn't exist in toElement, don't add it (don't add join fields to base docs)
            if (toElement.Attribute(toAttName) != null)
            {
                sNewAttValue = EditHelpers.XmlLinq.GetAttributeValue(fromElement, fromAttName);
                string sOldAttValue = EditHelpers.XmlLinq.GetAttributeValue(toElement, toAttName);
                if (string.IsNullOrEmpty(sNewAttValue) == false)
                {
                    if (sNewAttValue.Equals(sOldAttValue) == false)
                    {
                        if (!needsUpdateList)
                        {
                            //don't allow formatting-only changes to numbers (schemas ignore anyway)
                            sNewAttValue = AddInHelper.NoFormattingOnly(
                                sNewAttValue, sOldAttValue, dataType);
                            if (sNewAttValue != string.Empty)
                            {
                                EditHelpers.XmlLinq.SetAttributeValue(toElement,
                                    toAttName, sNewAttValue);
                            }
                        }
                        else
                        {
                            sNewAttValue = AddInHelper.NoFormattingOnly(
                                sNewAttValue, sOldAttValue, dataType);
                        }
                    }
                    else
                    {
                        sNewAttValue = string.Empty;
                    }
                }
                else
                {
                    sNewAttValue = string.Empty;
                }
            }
            return sNewAttValue;
        }
        
        #endregion
        #region "run calculations"
        public static bool NeedsAncestorCalculator(ContentURI calcDocURI,
            XElement ancestorXmlDocElement)
        {
            bool bNeedsCalculators = false;
            XElement ancestorCalculatorElement
                = EditHelpers.XmlLinq.GetElement(ancestorXmlDocElement,
                AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                calcDocURI.URIId.ToString());
            string sUseSameCalculator = string.Empty;
            if (ancestorCalculatorElement != null)
            {
                sUseSameCalculator
                    = EditHelpers.XmlLinq.GetAttributeValue(ancestorCalculatorElement,
                            AppHelpers.Calculator.cUseSameCalculator);
            }
            if (!string.IsNullOrEmpty(sUseSameCalculator))
            {
                bNeedsCalculators
                    = GeneralHelpers.ConvertToBoolean(sUseSameCalculator);
            }
            return bNeedsCalculators;
        }
        public static bool NeedsAncestorCalculator(XElement ancestorCalcElement)
        {
            bool bNeedsCalculators = false;
            string sUseSameCalculator
                = EditHelpers.XmlLinq.GetAttributeValue(ancestorCalcElement,
                        AppHelpers.Calculator.cUseSameCalculator);
            if (!string.IsNullOrEmpty(sUseSameCalculator))
            {
                bNeedsCalculators
                    = GeneralHelpers.ConvertToBoolean(sUseSameCalculator);
            }
            return bNeedsCalculators;
        }
        
        #endregion
    }
}
