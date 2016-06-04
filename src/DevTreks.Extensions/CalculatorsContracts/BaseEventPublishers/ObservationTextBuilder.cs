using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

using DevTreksAppHelpers = DevTreks.Data.AppHelpers;
using DevTreksHelpers = DevTreks.Data.Helpers;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Generates delimited text files holding each observation
    ///             on a separate line.
    ///             Streams through standard DevTreks documents and publishes 
    ///             events that subscribers  can use to serialize 
    ///             and deserialize the current node being processed. Developers 
    ///             can override the protected and virtual members in base classes. 
    ///Author:		www.devtreks.org. 
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. The observations file generates standard row column text 
    ///             data that can be imported into other software.
    ///             2. The columns are built on the first pass through all of 
    ///             the files. The second pass builds the full row column file.
    ///             3. Each observation can have a lot of repetitive data. The 
    ///             event that filters the data is not raised yet because events 
    ///             affect performance and its easy to delete columns from text 
    ///             files using other software.
    ///             
    /// </summary>
    public class ObservationTextBuilder
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected ObservationTextBuilder() { }
        public ObservationTextBuilder(CalculatorParameters calcParameters)
        {
            //set the class parameters
            this.ObsCalculatorParams = new CalculatorParameters(calcParameters);
            this.ObsArguments = new CustomEventArgs();
        }
        //calculator parameters
        public CalculatorParameters ObsCalculatorParams { get; set; }
        public bool HasGoodObservationFile { get; set; }
        private StreamWriter TextFile { get; set; }
        //column names (key = attribute name, value = nodeName)
        private IDictionary<string, string> ColumnNodeNames { get; set; }
        //linked view counter
        private int LinkedViewCount { get; set; }

        //declare the events that will be raised
        //keep this for potential future use
        public event EventHandler<CustomEventArgs> AddCurrentColumn;
        public CustomEventArgs ObsArguments { get; set; }

        //allows derived classes to override the event invocation behavior
        protected virtual void OnAddCurrentColumn(CustomEventArgs e)
        {
            //make a temporary copy of the event to avoid possibility of
            //a race condition if the last subscriber unsubscribes
            //immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = AddCurrentColumn;

            //event will be null if there are no subscribers
            if (handler != null)
            {
                //prepare the arguments to send inside the CustomEventArgs parameter
                e.CalculatorParams = this.ObsCalculatorParams;
                //use the () operator to raise the event.
                handler(this, e);
            }
        }
        
        //allows derived classes to override the default streaming 
        //and save method
        protected virtual bool StreamAndSaveObservation()
        {
            HasGoodObservationFile = false;
            if (this.ObsCalculatorParams.AnalyzerParms.FileOrFolderPaths == null)
            {
                this.ObsCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("OBSERVATIONS_NOFILES_SAVETEXT");
                return false;
            }
            else
            {
                if (this.ObsCalculatorParams.AnalyzerParms.FileOrFolderPaths.Count <= 0)
                {
                    this.ObsCalculatorParams.ErrorMessage
                        = Errors.MakeStandardErrorMsg("OBSERVATIONS_NOFILES_SAVETEXT");
                    return false;
                }
            }
            bool bHasInitialColumnDictionary = InitColumnDictionary();
            this.ObsCalculatorParams.AnalyzerParms.ObservationsPath
                = GetObservationFilePath(this.ObsCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!string.IsNullOrEmpty(this.ObsCalculatorParams.AnalyzerParms.ObservationsPath))
            {
                if (bHasInitialColumnDictionary)
                {
                    TextFile = new StreamWriter(
                        this.ObsCalculatorParams.AnalyzerParms.ObservationsPath);
                    //write the column headings
                    WriteColumnNameRow();
                }
                //+1 because the first column holds the stats for all comparisons
                //int iFileComparisonsCount = fileOrFolderPaths.Count + 1;
                //subscribers set a "Files" attribute property using this property
                this.ObsCalculatorParams.AnalyzerParms.FilesComparisonsCount
                    = this.ObsCalculatorParams.AnalyzerParms.FileOrFolderPaths.Count + 1;
                string sId = string.Empty;
                string sFileToAnalyze = string.Empty;
                int i = 0;
                //file order is not relevant in this addin,
                //but parallel won't work (yet) because of the way 
                //the observationRoot gets built
                foreach (KeyValuePair<string, string> kvp
                    in this.ObsCalculatorParams.AnalyzerParms.FileOrFolderPaths)
                {
                    sId = kvp.Key;
                    sFileToAnalyze = kvp.Value;
                    if (CalculatorHelpers.URIAbsoluteExists(
                        this.ObsCalculatorParams.ExtensionDocToCalcURI,
                        sFileToAnalyze))
                    {
                        this.ObsCalculatorParams.AnalyzerParms.ObservationFile = sFileToAnalyze;
                        this.ObsCalculatorParams.AnalyzerParms.FilePositionIndex = i;
                        //stream and add the calculation (observation)
                        //to the observationsRoot
                        if (!CalculatorHelpers.URIAbsoluteExists(
                            this.ObsCalculatorParams.ExtensionDocToCalcURI,
                            this.ObsCalculatorParams.AnalyzerParms.ObservationFile))
                            return HasGoodObservationFile;
                        this.ObsCalculatorParams.DocToCalcReader
                            = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                                this.ObsCalculatorParams.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                                this.ObsCalculatorParams.AnalyzerParms.ObservationFile);
                        if (this.ObsCalculatorParams.DocToCalcReader != null)
                        {
                            using (this.ObsCalculatorParams.DocToCalcReader)
                            {
                                this.ObsCalculatorParams.DocToCalcReader.MoveToContent();
                                this.ObsCalculatorParams.AnalyzerParms.Ancestors.Clear();
                                this.ObsCalculatorParams.CurrentElementNodeName
                                    = string.Empty;
                                this.ObsCalculatorParams.ParentElementNodeName
                                    = string.Empty;
                                LinkedViewCount = 0;
                                AddObservations(bHasInitialColumnDictionary);
                            }
                        }
                        if (this.ObsCalculatorParams.ErrorMessage != string.Empty)
                        {
                            //fix all errors before running an analysis
                            this.ObsCalculatorParams.ErrorMessage
                                = this.ObsCalculatorParams.ErrorMessage;
                            break;
                        }
                        i++;
                    }
                }
                if (this.ObsCalculatorParams.ErrorMessage == string.Empty)
                {
                    if (bHasInitialColumnDictionary)
                    {
                        //save the observations file
                        if (TextFile != null)
                        {
                            //2 passes means using syntax can't be used
                            TextFile.Flush();
                            TextFile.Close();
                            HasGoodObservationFile = true;
                        }
                    }
                    else
                    {
                        //second pass builds the observations
                        StreamAndSaveObservation();
                    }
                }
            }
            return HasGoodObservationFile;
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
                Constants.EXTENSION_XML,
                string.Concat(Constants.OBSERVATIONS, Constants.EXTENSION_CSV));
            return sObservationsDocPath;
        }
        public void AddObservations(bool hasColumnNames)
        {
            string sCurrentNodeName = string.Empty;
            //move forward through all of the document's elements
            while (this.ObsCalculatorParams.DocToCalcReader.Read())
            {
                if (this.ObsCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.ObsCalculatorParams.DocToCalcReader.HasAttributes)
                    {
                        InitCurrentObservationElement();
                        if (hasColumnNames)
                        {
                            AddObservationToAnalysis();
                        }
                        else
                        {
                            AddColumnNamesToDictionary();
                        }
                        //reset currentelement
                        ResetObservation();
                        //stop reading if any errors on hand
                        if (this.ObsCalculatorParams.ErrorMessage
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
            this.ObsCalculatorParams.CurrentElementNodeName 
                = this.ObsCalculatorParams.DocToCalcReader.Name;
            if (this.ObsCalculatorParams.CurrentElementNodeName
                == Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                LinkedViewCount += 1;
            }
        }
        private void ResetObservation()
        {
            if (this.ObsCalculatorParams.CurrentElementNodeName
                != Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                this.ObsCalculatorParams.ParentElementNodeName
                    = this.ObsCalculatorParams.CurrentElementNodeName;
            }
            this.ObsCalculatorParams.CurrentElementNodeName
                = string.Empty;
            LinkedViewCount = 0;
        }
        private void AddObservationToAnalysis()
        {
            AddCurrentElementToAncestors();
            if (this.ObsCalculatorParams.CurrentElementNodeName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString())
                || this.ObsCalculatorParams.CurrentElementNodeName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                WriteRow();
            }
        }
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
                    if (kvp.Value == this.ObsCalculatorParams.DocToCalcReader.Name)
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
                && this.ObsCalculatorParams.DocToCalcReader.HasAttributes)
            {
                string sColumnName = string.Empty;
                string sColumnValue = string.Empty;
                foreach (KeyValuePair<string, string> kvp
                    in ColumnNodeNames)
                {
                    if (kvp.Value == this.ObsCalculatorParams.DocToCalcReader.Name)
                    {
                        if (this.ObsCalculatorParams.CurrentElementNodeName
                            == Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
                        {
                            sColumnName = GetLinkedViewColumnName(kvp.Key, kvp.Value);
                        }
                        else
                        {
                            sColumnName = kvp.Key.Replace(kvp.Value, string.Empty);
                        }
                        sColumnValue = this.ObsCalculatorParams.DocToCalcReader
                            .GetAttribute(sColumnName);
                        if (!string.IsNullOrEmpty(sColumnValue))
                        {
                            AddToAncestorArray(kvp.Key, sColumnValue);
                        }
                    }
                }
                //move the reader back to the parent
                this.ObsCalculatorParams.DocToCalcReader.MoveToElement();
            }
        }
        private void AddColumnNamesToDictionary()
        {
            //all observations use the same column names
            if (this.ObsCalculatorParams.DocToCalcReader.HasAttributes)
            {
                while (this.ObsCalculatorParams.DocToCalcReader.MoveToNextAttribute())
                {
                    if (string.IsNullOrEmpty(this.ObsCalculatorParams.DocToCalcReader.Prefix))
                    {
                        AddToColumnArray(this.ObsCalculatorParams.DocToCalcReader.Name);
                    }
                    else
                    {
                        //ignore xmlns: in current version
                    }
                }
                //move the reader back to the element node
                this.ObsCalculatorParams.DocToCalcReader.MoveToElement();
            }
        }
        private void AddToColumnArray(string columnName)
        {
            //descriptions contain data that can interfere with the row columns
            if (!columnName.Contains(Calculator1.cDescription))
            {
                string sNodeColumnName = string.Empty;
                if (this.ObsCalculatorParams.CurrentElementNodeName
                    == Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    sNodeColumnName = MakeLinkedViewColumnName(columnName);
                }
                else
                {
                    sNodeColumnName = string.Concat(
                        this.ObsCalculatorParams.CurrentElementNodeName, columnName);
                }
                if (!ColumnNodeNames.ContainsKey(
                    sNodeColumnName))
                {
                    //add it (the value will be used later to parse the attName)
                    ColumnNodeNames.Add(sNodeColumnName,
                        this.ObsCalculatorParams.CurrentElementNodeName);
                }
            }
        }
        private string MakeLinkedViewColumnName(string columnName)
        {
            //linkedviews must be associated with parent, count handles
            //when more than one linkedview is linked to parent
            string sLVColName = string.Concat(
                this.ObsCalculatorParams.ParentElementNodeName,
                LinkedViewCount.ToString(),
                this.ObsCalculatorParams.CurrentElementNodeName,
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
        //shared with observationbuilder (move to calculatorhelpers)
        private void AddToAncestorArray(string key, string value)
        {
            if (this.ObsCalculatorParams.AnalyzerParms.Ancestors.ContainsKey(key))
            {
                //replace it 
                this.ObsCalculatorParams.AnalyzerParms.Ancestors[key] = value;
            }
            else
            {
                //add it
                this.ObsCalculatorParams.AnalyzerParms.Ancestors.Add(key, value);
            }
        }
        private string GetAncestorValue(string ancestorKeyName)
        {
            string sAncestorDelimitedString = string.Empty;
            if (this.ObsCalculatorParams.AnalyzerParms.Ancestors.ContainsKey(
                ancestorKeyName))
            {
                this.ObsCalculatorParams.AnalyzerParms.Ancestors.TryGetValue(
                    ancestorKeyName, out sAncestorDelimitedString);
            }
            return sAncestorDelimitedString;
        }
        private void RemoveFromAncestorArray(string currentKeyName)
        {
            if (this.ObsCalculatorParams.AnalyzerParms.Ancestors.ContainsKey(
                currentKeyName))
            {
                this.ObsCalculatorParams.AnalyzerParms.Ancestors
                    .Remove(currentKeyName);
            }
        }
    }
}
