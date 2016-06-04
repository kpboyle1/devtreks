using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Base class for running calculators and analyzers
    ///Author:		www.devtreks.org
    ///Date:		2014, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        This class defines the properties and methods used by all 
    ///             calculators and analyzers, but does not implement any method.
    ///             It must stay synchronized with the same Extensions.Calculator properties
    ///             and is primarily used to help display parts of calculators and analyzers.
    ///</summary>
    public abstract class Calculator
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        public Calculator()
        {
            InitCalculatorProperties();
        }
        //general calculator properties
        public int Id { get; set; }
        public int PKId { get; set; }
        public int CalculatorId { get; set; }
        public string Version { get; set; }
        public string CalculatorType { get; set; }
        public string WhatIfTagName { get; set; }
        public string FileExtensionType { get; set; }
        public string CalculatorName { get; set; }
        public string StylesheetResourceFileName { get; set; }
        public string StylesheetObjectNS { get; set; }
        public string Stylesheet2ResourceFileName { get; set; }
        public string Stylesheet2ObjectNS { get; set; }
        public string CalculatorDescription { get; set; }
        public DateTime CalculatorDate { get; set; }
        public bool UseSameCalculator { get; set; }
        public string Type { get; set; }
        public string RelatedCalculatorType { get; set; }
        public string RelatedCalculatorsType { get; set; }
        public string Overwrite { get; set; }
        public string SubApplicationType { get; set; }
        public string ErrorMessage { get; set; }
        //these are standard aggregators
        public string Label { get; set; }
        public string Label2 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string DocToCalcReplacementFile { get; set; }
        //standard analyzer properties
        //comparison option
        public string Option1 { get; set; }
        //aggregation option (types, groups, labels)
        public string Option2 { get; set; }
        //this option comes into play when cost effectiveness analyses are built
        public string Option3 { get; set; }
        //subfolder option (cumulative vs. individual observations)
        public string Option4 { get; set; }
        //needs full view option
        public string Option5 { get; set; }
        public string AnalyzerType { get; set; }
        public string FilesToAnalyzeExtensionType { get; set; }
        //basic alternatives (one, two, three)
        public string AlternativeType { get; set; }
        //targettype (benchmark, actual, partial target)
        public string TargetType { get; set; }
        //current compared first to baseline, second to xminus1 (if on hand and not baseline)
        public string ChangeType { get; set; }
        //which element should the calc observation be aggregated into
        public int Alternative2 { get; set; }
        //how many observations have been aggregated 
        //(user feedback only, use stat objects for more details)
        public int Observations { get; set; }
        //media view of calcs
        public string MediaURL { get; set; }
        public string DataURL { get; set; }
        //SHOULD BE ADDED TO HELPERS
        //nonstateful properties (don't store in xml)
        //easier to pass tech multipliers (Component.Amount) in calc object
        public double Multiplier { get; set; }
        //form tags only (not serialized in objects)
        public const string WHATIF_TAGNAME_FORM = "whatiftagname";
        public const string RELATEDCALCULATORTYPE_FORM = "relatedcalculatortype";
        public const string RELATEDCALCULATORSTYPE_FORM = "relatedcalculatorstype";

        //string attribute names
        public const string cId = "Id";
        public const string cPKId = "PKId";
        public const string cCalculatorId = "CalculatorId";
        public const string cVersion = "Version";
        public const string cCalculatorType = "CalculatorType";
        public const string cWhatIfTagName = "WhatIfTagName";
        public const string cFileExtensionType = "FileExtensionType";
        public const string cCalculatorName = "CalculatorName";
        public const string cStylesheetResourceFileName = "StylesheetResourceFileName";
        public const string cStylesheetObjectNS = "StylesheetObjectNS";
        public const string cStylesheet2ResourceFileName = "Stylesheet2ResourceFileName";
        //calculators and analyzer results, and storytellers and any "third doc"
        //use these parameters to find the right stylesheet
        public const string cStylesheet2ObjectNS = "Stylesheet2ObjectNS";
        public const string cCalculatorDescription = "CalculatorDescription";
        public const string cCalculatorDate = "CalculatorDate";
        public const string cUseSameCalculator = "UseSameCalculator";
        public const string cType = "Type";
        public const string cRelatedCalculatorType = "RelatedCalculatorType";
        public const string cRelatedCalculatorsType = "RelatedCalculatorsType";
        public const string cOverwrite = "Overwrite";
        public const string cSubApplicationType = "SubApplication";
        public const string cErrorMessage = "ErrorMessage";
        public const string cLabel = "Num";
        public const string cLabel2 = "Num2";
        public const string cName = "Name";
        public const string cDescription = "Description";
        public const string cDate = "Date";
        public const string cTypeId = "TypeId";
        public const string cTypeName = "TypeName";
        public const string cGroupId = "GroupId";
        public const string cGroupName = "GroupName";
        //comparison option
        public const string cOption1 = "Option1";
        //aggregation option (types, groups, labels)
        public const string cOption2 = "Option2";
        //this option comes into play when cost effectiveness analyses are built
        public const string cOption3 = "Option3";
        //subfolder option (cumulative vs. individual observations)
        public const string cOption4 = "Option4";
        public const string cOption5 = "Option5";
        public const string cAnalyzerType = "AnalyzerType";
        public const string cFilesToAnalyzeExtensionType = "FilesToAnalyzeExtensionType";
        public const string cDocToCalcReplacementFile = "DocToCalcReplacementFile";
        //nonstateful properties (don't store in xml)
        //easier to pass tech multipliers (Component.Amount) in calc object
        public const string cMultiplier = "Id";
        //basic alternatives (one, two, three)
        public const string cAlternativeType = "AlternativeType";
        //targettype (benchmark, actual, partial target)
        public const string cTargetType = "TargetType";
        //current compared first to baseline, second to xminus1 (if on hand and not baseline)
        public const string cChangeType = "ChangeType";
        //which element should the calc observation be aggregated into
        public const string cAlternative2 = "Alternative2";
        //how many observations have been aggregated 
        //(user feedback only, use stat objects for more details)
        public const string cObservations = "Observations";
        //media url
        public const string cMediaURL = "MediaURL";
        public const string cDataURL = "DataURL";
        

        #region "enums"
        public enum COMPARISON_OPTIONS
        {
            none = 0,
            compareonly = 1
        }
        /// <summary>
        /// grouping options for statistical analysis
        /// </summary>
        public enum AGGREGATION_OPTIONS
        {
            none = 0,
            labels = 1,
            types = 2,
            groups = 3
        }
        public enum ALTERNATIVE_TYPES
        {
            none = 0,
            A = 1,
            B = 2,
            C = 3,
            D = 4,
            E = 5,
            F = 6,
            G = 7,
            H = 8,
            I = 9,
            J = 10
        }
        public enum TARGET_TYPES
        {
            none = 0,
            benchmark = 1,
            partialtarget = 2,
            fulltarget = 3,
            actual = 4
        }
        public enum CHANGE_TYPES
        {
            none = 0,
            baseline = 1,
            xminus1 = 2,
            current = 3
        }
        public static List<string> GetTargetTypes()
        {
            List<string> targetTypes = new List<string>();
            targetTypes.Add(TARGET_TYPES.none.ToString());
            targetTypes.Add(TARGET_TYPES.benchmark.ToString());
            targetTypes.Add(TARGET_TYPES.partialtarget.ToString());
            targetTypes.Add(TARGET_TYPES.fulltarget.ToString());
            targetTypes.Add(TARGET_TYPES.actual.ToString());
            return targetTypes;
        }
        public static List<string> GetAlternativeTypes()
        {
            List<string> altTypes = new List<string>();
            altTypes.Add(ALTERNATIVE_TYPES.none.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.A.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.B.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.C.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.D.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.E.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.F.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.G.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.H.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.I.ToString());
            altTypes.Add(ALTERNATIVE_TYPES.J.ToString());
            return altTypes;
        }
        #endregion
        //allows derived classes to override the base methods
        public abstract void InitCalculatorProperties();
        //set the class properties using the XElement
        public abstract void SetCalculatorProperties(XElement calculator);
        //attname and attvalue generally passed in from a reader
        public abstract void SetCalculatorProperties(string attName,
            string attValue);
        //the attNameExtension is used with attribute indexing _0_1
        public abstract void SetAndRemoveCalculatorAttributes(string attNameExtension,
            ref XElement calculator);
        public abstract void SetCalculatorAttributes(string attNameExtension,
            ref XmlWriter writer);
        
    }
}      
