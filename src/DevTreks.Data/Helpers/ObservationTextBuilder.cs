using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DevTreks.Data.Helpers
{
    ///<summary>
    ///Purpose:		Generates delimited text files holding each observation
    ///             on a separate line.
    ///Author:		www.devtreks.org. 
    ///Date:		2015, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. The observations file generates standard row column text 
    ///             data that can be imported into other software.
    ///             2. The columns are built on the first pass through all of 
    ///             the files. The second pass builds the full row column file.
    ///             
    /// </summary>
    public class ObservationTextBuilder
    {
        public ObservationTextBuilder() 
        {
            Ancestors = new Dictionary<string, string>();
        }

        public bool HasGoodObservationFile { get; set; }
        private StreamWriter TextFile { get; set; }
        //column names (key = attribute name, value = nodeName)
        private IDictionary<string, string> ColumnNodeNames { get; set; }
        //dictionary holding current observation
        private IDictionary<string, string> Ancestors { get; set; }
        //linked view counter
        private int LinkedViewCount { get; set; }
        private XmlReader FileToAnalyzeReader { get; set; }
        private string CurrentNodeName { get; set; }
        private string ParentNodeName { get; set; }
        //depth of xml determines when to write a row
        private string LastNodeName { get; set; }
        private int RowDepth = 0;
        //allows derived classes to override the default streaming 
        //and save method
        public bool StreamAndSaveObservation(ContentURI docToCalcURI,
            IDictionary<string, string> fileOrFolderPaths)
        {
            HasGoodObservationFile = false;
            if (fileOrFolderPaths == null)
            {
                docToCalcURI.ErrorMessage
                    = DevTreks.Exceptions.DevTreksErrors.GetMessage("OBSERVATIONS_NOFILES_SAVETEXT");
                return false;
            }
            else
            {
                if (fileOrFolderPaths.Count <= 0)
                {
                    docToCalcURI.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.GetMessage("OBSERVATIONS_NOFILES_SAVETEXT");
                    return false;
                }
            }
            bool bHasInitialColumnDictionary = InitColumnDictionary();
            string sObservationsPath
                = GetObservationFilePath(docToCalcURI.URIDataManager.TempDocPath);
            if (!string.IsNullOrEmpty(sObservationsPath))
            {
                try
                {
                    using (TextFile =
                        new StreamWriter(sObservationsPath))
                    {
                        string sId = string.Empty;
                        string sFileToAnalyze = string.Empty;
                        int i = 0;
                        //file order is not relevant in this addin,
                        //but parallel won't work (yet) because of the way 
                        //the observationRoot gets built
                        foreach (KeyValuePair<string, string> kvp
                            in fileOrFolderPaths)
                        {
                            sId = kvp.Key;
                            sFileToAnalyze = kvp.Value;
                            if (Helpers.FileStorageIO.URIAbsoluteExists(
                                docToCalcURI, sFileToAnalyze))
                            {
                                //stream and add the calculation (observation)
                                //to the observationsRoot
                                if (!Helpers.FileStorageIO.URIAbsoluteExists(
                                    docToCalcURI, sFileToAnalyze))
                                    return HasGoodObservationFile;
                                FileToAnalyzeReader = Helpers.FileStorageIO.GetXmlReader(docToCalcURI, sFileToAnalyze);
                                if (FileToAnalyzeReader != null)
                                {
                                    using (FileToAnalyzeReader)
                                    {
                                        FileToAnalyzeReader.MoveToContent();
                                        Ancestors.Clear();
                                        CurrentNodeName
                                            = string.Empty;
                                        ParentNodeName
                                            = string.Empty;
                                        LinkedViewCount = 0;
                                        AddObservations(docToCalcURI, bHasInitialColumnDictionary);
                                    }
                                }
                                if (docToCalcURI.ErrorMessage != string.Empty)
                                {
                                    //fix all errors before running an analysis
                                    docToCalcURI.ErrorMessage
                                        = docToCalcURI.ErrorMessage;
                                    break;
                                }
                                i++;
                            }
                        }
                        if (docToCalcURI.ErrorMessage == string.Empty)
                        {
                            //subroutine builds the observations
                            bHasInitialColumnDictionary = true;
                            HasGoodObservationFile = StreamAndSaveObservation(docToCalcURI, fileOrFolderPaths,
                                bHasInitialColumnDictionary);
                        }
                    }
                }
                catch (Exception x)
                {
                    docToCalcURI.ErrorMessage = x.ToString();
                    TextFile.Dispose();
                }
            }
            return HasGoodObservationFile;
        }
        private bool StreamAndSaveObservation(ContentURI docToCalcURI,
            IDictionary<string, string> fileOrFolderPaths, bool hasInitialColumnDictionary)
        {
            bool bHasObservation = false;
            string sId = string.Empty;
            string sFileToAnalyze = string.Empty;
            int i = 0;
            WriteColumnNameRow();
            foreach (KeyValuePair<string, string> kvp in fileOrFolderPaths)
            {
                sId = kvp.Key;
                sFileToAnalyze = kvp.Value;
                if (Helpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, sFileToAnalyze))
                {
                    FileToAnalyzeReader 
                        = Helpers.FileStorageIO.GetXmlReader(docToCalcURI, sFileToAnalyze);
                    if (FileToAnalyzeReader != null)
                    {
                        using (FileToAnalyzeReader)
                        {
                            FileToAnalyzeReader.MoveToContent();
                            Ancestors.Clear();
                            AddObservations(docToCalcURI,
                                hasInitialColumnDictionary);
                        }
                    }
                    if (docToCalcURI.ErrorMessage != string.Empty)
                    {
                        //fix all errors before running an analysis
                        break;
                    }
                    i++;
                }
            }
            if (docToCalcURI.ErrorMessage == string.Empty)
            {
                if (hasInitialColumnDictionary)
                {
                    //save the observations file
                    TextFile.Flush();
                    bHasObservation = true;
                }
            }
            return bHasObservation;
        }
        private bool InitColumnDictionary()
        {
            bool bHasInititalDictionary = false;
            if (ColumnNodeNames == null)
            {
                ColumnNodeNames = new Dictionary<string, string>();
            }
            else
            {
                bHasInititalDictionary = true;
            }
            return bHasInititalDictionary;
        }
        public static string GetObservationFilePath(string docToCalcTempDocPath)
        {
            string sObservationsDocPath
                = docToCalcTempDocPath.Replace(
                GeneralHelpers.EXTENSION_XML, GeneralHelpers.EXTENSION_CSV);
            return sObservationsDocPath;
        }
        public void AddObservations(ContentURI docToCalcURI, bool hasColumnNames)
        {
            string sCurrentNodeName = string.Empty;
            bool bNeedsLVs = false;
            //move forward through all of the document's elements
            while (FileToAnalyzeReader.Read())
            {
                if (FileToAnalyzeReader.NodeType
                    == XmlNodeType.Element)
                {
                    //skip <root> and </input>
                    if (FileToAnalyzeReader.HasAttributes)
                    {
                        //set CurrentNodeName
                        InitCurrentObservationElement();
                        if (hasColumnNames)
                        {
                            bool needsLVs = bNeedsLVs;
                            bNeedsLVs = AddObservationToAnalysis(needsLVs);
                        }
                        else
                        {
                            AddColumnNamesToDictionary();
                            //set the depth for writing rows
                            if (FileToAnalyzeReader.Depth > RowDepth)
                            {
                                if (CurrentNodeName
                                    != AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                {
                                    LastNodeName = CurrentNodeName;
                                }
                                else
                                {
                                    LastNodeName = ParentNodeName;
                                }
                                RowDepth = FileToAnalyzeReader.Depth;
                            }
                        }
                        //reset currentelement
                        ResetObservation();
                        //stop reading if any errors on hand
                        if (docToCalcURI.ErrorMessage
                            != string.Empty)
                        {
                            break;
                        }
                    }
                }
            }
        }
        private void InitCurrentObservationElement()
        {
            CurrentNodeName 
                = FileToAnalyzeReader.Name;
            if (CurrentNodeName
                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                LinkedViewCount += 1;
            }
            
        }
        private void ResetObservation()
        {
            if (CurrentNodeName
                != AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                ParentNodeName
                    = CurrentNodeName;
                LinkedViewCount = 0;
            }
            CurrentNodeName
                = string.Empty;
        }
        private bool AddObservationToAnalysis(bool needsLVs)
        {
            bool bNeedsLVs = needsLVs;
            if (FileToAnalyzeReader.Name
                  .StartsWith(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
            {
                AddObservationLinkedViewToAnalysis();
                //always write the row after lvs processed
                WriteRow();
                //turn flag off
                bNeedsLVs = false;
            }
            else
            {
                if (bNeedsLVs)
                {
                    //last node didn't have any lvs, need to write the row
                    WriteRow();
                }
                AddCurrentElementToAncestors();
                bNeedsLVs = true;
            }
            return bNeedsLVs;
        }
        private void AddObservationLinkedViewToAnalysis()
        {
            //set ParentNodeName
            ResetObservation();
            //set CurrentNodeName
            InitCurrentObservationElement();
            AddCurrentElementToAncestors();
            //add siblings
            while (FileToAnalyzeReader.ReadToNextSibling(
                AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
            {
                if (FileToAnalyzeReader.HasAttributes)
                {
                    AddCurrentElementToAncestors();
                }
            }
        }

        //178 deprecated: skipped elements
        //private void AddObservationToAnalysis()
        //{
        //    AddCurrentElementToAncestors();
        //    if (LastNodeName
        //        .EndsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
        //        || LastNodeName
        //        .EndsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
        //    {
        //        if (CurrentNodeName
        //            .EndsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
        //            || CurrentNodeName
        //            .EndsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
        //        {
        //            //first add children linkedviews
        //            AddObservationLinkedViewToAnalysis();
        //            WriteRow();
        //        }
        //    }
        //    else if (LastNodeName
        //        .EndsWith(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
        //        || LastNodeName
        //        .EndsWith(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
        //        || LastNodeName
        //        .EndsWith(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
        //    {
        //        if (CurrentNodeName
        //        .EndsWith(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
        //        || CurrentNodeName
        //        .EndsWith(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
        //        || CurrentNodeName
        //        .EndsWith(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
        //        {
        //            //first add children linkedviews
        //            AddObservationLinkedViewToAnalysis();
        //            WriteRow();
        //        }
        //    }
        //    else if (LastNodeName
        //        .Equals(AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
        //        || LastNodeName
        //        .Equals(AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString()))
        //    {
        //        if (CurrentNodeName
        //            .Equals(AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
        //            || CurrentNodeName
        //            .Equals(AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString()))
        //        {
        //            //first add children linkedviews
        //            AddObservationLinkedViewToAnalysis();
        //            WriteRow();
        //        }
        //    }
        //    else if (LastNodeName
        //        .EndsWith("series"))
        //    {
        //        //first add children linkedviews
        //        AddObservationLinkedViewToAnalysis();
        //        WriteRow();
        //    }
        //}
        
        //private void AddObservationLinkedViewToAnalysis()
        //{
        //    //first move to children linked views
        //    FileToAnalyzeReader.Read();
        //    bool bHasLinkedView  = (FileToAnalyzeReader.Name
        //        .StartsWith(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()));
        //    //if the current node does not have a lv, this skips past childnode to the lv of child
        //    //bool bHasLinkedView = FileToAnalyzeReader.ReadToDescendant(
        //    //    AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
        //    if (bHasLinkedView)
        //    {
        //        //set ParentNodeName
        //        ResetObservation();
        //        //set CurrentNodeName
        //        InitCurrentObservationElement();
        //        AddCurrentElementToAncestors();
        //        //add siblings
        //        while (FileToAnalyzeReader.ReadToNextSibling(
        //            AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
        //        {
        //            if (FileToAnalyzeReader.HasAttributes)
        //            {
        //                AddCurrentElementToAncestors();
        //            }
        //        }
        //    }
        //}
        private void WriteColumnNameRow()
        {
            if (TextFile != null)
            {
                int i = 0;
                foreach (KeyValuePair<string, string> kvp
                    in ColumnNodeNames)
                {
                    if (i != 0)
                    {
                        TextFile.Write(",");
                    }
                    if (!string.IsNullOrEmpty(kvp.Key))
                    {
                        TextFile.Write(kvp.Key);
                    }
                    i++;
                }
                //write a line terminator
                TextFile.WriteLine();
            }
        }
        private void WriteRow()
        {
            if (TextFile != null)
            {
                //each output and input is an observation
                //that gets added as a line in text file
                string sColumnValue = string.Empty;
                int i = 0;
                foreach (KeyValuePair<string, string> kvp
                    in ColumnNodeNames)
                {
                    sColumnValue = GetAncestorValue(kvp.Key);
                    //remove the delimiter
                    sColumnValue = sColumnValue.Replace(",", string.Empty);
                    //remove semicolons as well 
                    sColumnValue = sColumnValue.Replace(";", string.Empty);
                    if (i != 0)
                    {
                        TextFile.Write(",");
                    }
                    if (!string.IsNullOrEmpty(sColumnValue))
                    {
                        TextFile.Write(sColumnValue);
                    }
                    else
                    {
                        TextFile.Write(string.Empty);
                    }
                    if (kvp.Value == FileToAnalyzeReader.Name)
                    {
                        //remove the input or output ancestors for next observation
                        RemoveFromAncestorArray(kvp.Key);
                    }
                    i++;
                }
                //write a line terminator
                TextFile.WriteLine();
            }
        }
        private void AddCurrentElementToAncestors()
        {
            if (ColumnNodeNames != null 
                && FileToAnalyzeReader.HasAttributes)
            {
                string sColumnName = string.Empty;
                string sColumnValue = string.Empty;
                foreach (KeyValuePair<string, string> kvp
                    in ColumnNodeNames)
                {
                    sColumnName = string.Empty;
                    sColumnValue = string.Empty;
                    if (kvp.Value == FileToAnalyzeReader.Name)
                    {
                        if (CurrentNodeName
                            == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            string sParColName = GetLinkedViewParentColumnName(kvp.Key, kvp.Value);
                            if (sParColName == ParentNodeName)
                            {
                                sColumnName = GetLinkedViewColumnName(kvp.Key, kvp.Value);
                            }
                        }
                        else
                        {
                            sColumnName = kvp.Key.Replace(kvp.Value, string.Empty);
                        }
                        if (!string.IsNullOrEmpty(sColumnName))
                        {
                            sColumnValue = FileToAnalyzeReader
                                .GetAttribute(sColumnName);
                            if (!string.IsNullOrEmpty(sColumnValue))
                            {
                                AddToAncestorArray(kvp.Key, sColumnValue);
                            }
                        }
                    }
                }
                //move off the attributes and to the element
                FileToAnalyzeReader.MoveToElement();
            }
        }
        private void AddColumnNamesToDictionary()
        {
            //all observations use the same column names
            if (FileToAnalyzeReader.HasAttributes)
            {
                while (FileToAnalyzeReader.MoveToNextAttribute())
                {
                    if (string.IsNullOrEmpty(FileToAnalyzeReader.Prefix))
                    {
                        AddToColumnArray(FileToAnalyzeReader.Name);
                    }
                    else
                    {
                        //ignore xmlns: in current version
                    }
                }
                //move the reader back to the element node
                FileToAnalyzeReader.MoveToElement();
            }
        }
        private void AddToColumnArray(string columnName)
        {
            //descriptions and MediaURLs contain data that can interfere with the row columns
            if (!columnName.Contains(AppHelpers.Calculator.cDescription)
                && !columnName.Contains(AppHelpers.Calculator.cMediaURL)
                && !columnName.Contains(AppHelpers.Calculator.cDataURL)
                && !columnName.Contains("Joint")
                //keep any math attributes out -they contain csv data
                && !columnName.Contains("Math"))
            {
                string sNodeColumnName = string.Empty;
                if (CurrentNodeName
                    == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    sNodeColumnName = MakeLinkedViewColumnName(columnName);
                }
                else
                {
                    sNodeColumnName = string.Concat(
                        CurrentNodeName, columnName);
                }
                if (!ColumnNodeNames.ContainsKey(
                    sNodeColumnName))
                {
                    //add it (the value will be used later to parse the attName)
                    ColumnNodeNames.Add(sNodeColumnName,
                        CurrentNodeName);
                }
            }
        }
        private string MakeLinkedViewColumnName(string columnName)
        {
            //linkedviews must be associated with parent, count handles
            //when more than one linkedview is linked to parent
            string sLVColName = string.Concat(
                ParentNodeName,
                LinkedViewCount.ToString(),
                CurrentNodeName,
                columnName);
            return sLVColName;
        }
        private string GetLinkedViewColumnName(string key, string value)
        {
            string sAttributeName = string.Empty;
            if ((!string.IsNullOrEmpty(key))
                && (!string.IsNullOrEmpty(value)))
            {
                int iIndex = key.IndexOf(value);
                iIndex += value.Length;
                if (iIndex < key.Length)
                {
                    sAttributeName = key.Remove(0, iIndex);
                }
            }
            return sAttributeName;

        }
        private string GetLinkedViewParentColumnName(string key, string value)
        {
            string sParentColName = string.Empty;
            if ((!string.IsNullOrEmpty(key))
                && (!string.IsNullOrEmpty(value)))
            {
                int iIndex = key.IndexOf(value);
                //LinkedViewCount (assume less than 10 lvs per node)
                iIndex -= 1;
                if (iIndex < key.Length)
                {
                    sParentColName = key.Remove(iIndex);
                }
            }
            return sParentColName;

        }
        //shared with observationbuilder (move to calculatorhelpers)
        private void AddToAncestorArray(string key, string value)
        {
            if (Ancestors.ContainsKey(key))
            {
                //replace it 
                Ancestors[key] = value;
            }
            else
            {
                //add it
                Ancestors.Add(key, value);
            }
        }
        private string GetAncestorValue(string ancestorKeyName)
        {
            string sAncestorDelimitedString = string.Empty;
            if (Ancestors.ContainsKey(
                ancestorKeyName))
            {
                Ancestors.TryGetValue(
                    ancestorKeyName, out sAncestorDelimitedString);
            }
            return sAncestorDelimitedString;
        }
        private void RemoveFromAncestorArray(string currentKeyName)
        {
            if (Ancestors.ContainsKey(
                currentKeyName))
            {
                Ancestors
                    .Remove(currentKeyName);
            }
        }
    }
}
