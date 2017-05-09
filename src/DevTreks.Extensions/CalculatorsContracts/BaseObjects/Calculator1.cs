using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using DevTreksAppHelpers = DevTreks.Data.AppHelpers;
using DevTreksHelpers = DevTreks.Data.Helpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The BaseCalculator class is a base class used 
    ///             by most standard DevTreks calculators/analyzers to hold 
    ///             base properties, such as ids and names.
    ///Author:		www.devtreks.org
    ///Date:		2017, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. All properties are stored in Data.Calculator class. 
    ///             Inheritance to Data.Calculator is undesirable because extension
    ///             would need to reference Data project.
    ///             2. Version 1.4.5 enforced clear separation between base element and 
    ///             children calculators. Previously, base element could store calc props.
    ///             Now base element stores calcs in baseElement.Calculators[0].
    public class Calculator1 
    {
        public Calculator1()
        {
            InitCalculatorProperties();
        }
        //copy constructor
        public Calculator1(Calculator1 calculator)
        {
            CopyCalculatorProperties(calculator);
        }
        //these properties must also be defined in Data.Calculator class

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
        //display summary only option
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
        //media view
        public string MediaURL { get; set; }
        //1.7.6: calculators hold metadata; this file holds the observations associated with the metadata
        public string DataURL { get; set; }
        //1.8.8: algos can be run from analyzers to compare means with cis
        public string MathType { get; set; }
        public string MathSubType { get; set; }
        public string MathExpression { get; set; }
        public string MathResult { get; set; }
        public int MathCILevel { get; set; }
        //besides data, must store the names of the columns
        public string DataColNames { get; set; }
        //188 allowed analyzers to statistically analyze the data in indiv calcors
        public IDictionary<string, List<List<double>>> DataToAnalyze { get; set; }
        public IDictionary<int, List<List<double>>> Data2ToAnalyze { get; set; }
        //calculator can share with baseelement for some types of aggregations (where calc lists used)
        public string Label { get; set; }
        public string Label2 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }

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
        //comparison option
        public const string cOption1 = "Option1";
        //aggregation option (types, groups, labels)
        public const string cOption2 = "Option2";
        //this option comes into play when cost effectiveness analyses are built
        public const string cOption3 = "Option3";
        //subfolder option (cumulative vs. individual observations)
        public const string cOption4 = "Option4";
        //display summary option
        public const string cOption5 = "Option5";
        public const string cAnalyzerType = "AnalyzerType";
        public const string cFilesToAnalyzeExtensionType = "FilesToAnalyzeExtensionType";
        public const string cDocToCalcReplacementFile = "DocToCalcReplacementFile";
        //nonstateful properties (don't store in xml)
        //easier to pass tech multipliers (Component.Amount) in calc object
        public const string cMultiplier = "cMultiplier";
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
        public const string cMathType = "MathType";
        public const string cMathSubType = "MathSubType";
        public const string cMathExpression = "MathExpression";
        public const string cMathResult = "MathResult";
        public const string cMathCILevel = "MathCILevel";
        public const string cDataColNames = "DataColNames";
        //calculator can share with base element (some aggregations are easier)
        //but not saved with attributes
        public const string cPKId = "PKId";
        public const string cLabel = "Num";
        public const string cLabel2 = "Num2";
        public const string cName = "Name";
        public const string cDescription = "Description";
        public const string cDate = "Date";
        public const string cTypeId = "TypeId";
        public const string cTypeName = "TypeName";
        public const string cGroupId = "GroupId";
        public const string cGroupName = "GroupName";
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
            current = 3,
            other = 4
        }
        
        //cta support
        //distribution types (R&U calculations)
        public enum RUC_TYPES
        {
            none = 0,
            normal = 1,
            triangle = 2,
            bernoulli = 3,
            beta = 4,
            lognormal = 5,
            weibull = 6,
            poisson = 7,
            uniform = 8,
            binomial = 9,
            pareto = 10,
            gamma = 11,
            specific = 12
        }
        public enum MATH_OPERATOR_TYPES
        {
            none = 0,
            equalto = 1,
            lessthan = 2,
            greaterthan = 3,
            lessthanorequalto = 4,
            greaterthanorequalto = 5,
            specific = 6
        }
        public enum MATH_TYPES
        {
            none = 0,
            algorithm1 = 1,
            algorithm2 = 2,
            algorithm3 = 3,
            algorithm4 = 4,
            algorithm5 = 5,
            algorithm6 = 6,
            algorithm7 = 7,
            algorithm8 = 8,
            algorithm9 = 9,
            algorithm10 = 10
        }
        public enum MATH_SUBTYPES
        {
            none = 0,
            subalgorithm1 = 1,
            subalgorithm2 = 2,
            subalgorithm3 = 3,
            subalgorithm4 = 4,
            subalgorithm5 = 5,
            subalgorithm6 = 6,
            subalgorithm7 = 7,
            subalgorithm8 = 8,
            subalgorithm9 = 9,
            subalgorithm10 = 10,
            subalgorithm11 = 11,
            subalgorithm12 = 12,
            subalgorithm13 = 13,
            subalgorithm14 = 14,
            subalgorithm15 = 15,
            subalgorithm16 = 16,
            subalgorithm17 = 17,
            subalgorithm18 = 18,
            subalgorithm19 = 19,
            subalgorithm20 = 20
        }
        public enum MATH_RESULT_TYPES
        {
            none = 0,
            result1 = 1,
            result2 = 2,
            result3 = 3,
            result4 = 4,
            result5 = 5,
            result6 = 6,
            result7 = 7,
            result8 = 8,
            result9 = 9,
            result10 = 10
        }
        //indicators can be prices, demogs, nature, or economic (performance)
        public enum INDIC_TYPES
        {
            none = 0,
            rev = 1,
            oc = 2,
            aoh = 3,
            cap = 4,
            demog1 = 5,
            nature1 = 6,
            econ1 = 7
        }
        //deprecated: mathematical property to do with the 2 Quantities
        //i.e. many indicators are rates or ratios, requiring division
        //note the math takes place after any statistics are derived
        public enum QMATH_TYPE
        {
            none = 0,
            Q1_add_Q2 = 1,
            Q1_subtract_Q2 = 2,
            Q1_divide_Q2 = 3,
            Q1_multiply_Q2 = 4,
            //q1 + q2 / 2
            Q1_average_Q2 = 5,
            //q1 * q2
            Q1_probability_Q2 = 6,
            //square root of Q2var
            Q1mean_stddev_Q2var = 7,
            Q1_log_Q2 = 8,
            Q1_power_Q2 = 9,
            Q1_3point_Q2 = 10
        }
        
        public static double GetTotalMathType(string mathType,
            double Q1Amount, double Q2Amount, double weight)
        {
            double dbTotal = 0;
            Q1Amount = Math.Round(Q1Amount, 4);
            Q2Amount = Math.Round(Q2Amount, 4);
            if (mathType == QMATH_TYPE.Q1_divide_Q2.ToString())
            {
                if (Q2Amount == 0)
                {
                    return 0;
                }
                dbTotal = Q1Amount / Q2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1_multiply_Q2.ToString())
            {
                dbTotal = Q1Amount * Q2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1_add_Q2.ToString())
            {
                dbTotal = Q1Amount + Q2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1_subtract_Q2.ToString())
            {
                dbTotal = Q1Amount - Q2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1_average_Q2.ToString())
            {
                dbTotal = (Q1Amount + Q2Amount) / 2;
            }
            else if (mathType == QMATH_TYPE.Q1_probability_Q2.ToString())
            {
                dbTotal = Q1Amount * Q2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1mean_stddev_Q2var.ToString())
            {
                //sample standard deviation
                dbTotal = Math.Sqrt(Q2Amount);
            }
            else if (mathType == QMATH_TYPE.Q1_log_Q2.ToString())
            {
                //sample standard deviation
                dbTotal = Math.Log(Q1Amount, Q2Amount);
            }
            else if (mathType == QMATH_TYPE.Q1_power_Q2.ToString())
            {
                //sample standard deviation
                dbTotal = Math.Pow(Q1Amount, Q2Amount);
            }
            else if (mathType == QMATH_TYPE.Q1_3point_Q2.ToString())
            {
                //optimistic, most likely, pessimistic
                dbTotal = weight;
            }
            else
            {
                //default is q1
                dbTotal = Q1Amount;
            }
            if (mathType != QMATH_TYPE.Q1_3point_Q2.ToString())
            {
                dbTotal = dbTotal * weight;
            }
            return dbTotal;
        }
        public enum QMATHSTOCK_TYPE
        {
            none = 0,
            add = 1,
            subtract = 2,
            divide = 3,
            multiply = 4,
            log = 6,
            power = 7,
            equalto = 8,
            lessthan = 9,
            greaterthan = 10,
            Q1_3point_Q3 = 11,
            Q1total_Q2mean_Q3med_Q4var_Q5sd = 12
        }
        
        public static double GetTotalMathTypeStocks(string mathType,
            double QAAmount, double QBAmount)
        {
            double dbTotal = 0;
            QAAmount = Math.Round(QAAmount, 4);
            QBAmount = Math.Round(QBAmount, 4);
            if (mathType == QMATHSTOCK_TYPE.divide.ToString())
            {
                if (QBAmount == 0)
                {
                    return 0;
                }
                dbTotal = QAAmount / QBAmount;
            }
            else if (mathType == QMATHSTOCK_TYPE.multiply.ToString())
            {
                dbTotal = QAAmount * QBAmount;
            }
            else if (mathType == QMATHSTOCK_TYPE.add.ToString())
            {
                dbTotal = QAAmount + QBAmount;
            }
            else if (mathType == QMATHSTOCK_TYPE.subtract.ToString())
            {
                dbTotal = QAAmount - QBAmount;
            }
            else if (mathType == QMATHSTOCK_TYPE.log.ToString())
            {
                dbTotal = Math.Log(QAAmount, QBAmount);
            }
            else if (mathType == QMATHSTOCK_TYPE.power.ToString())
            {
                dbTotal = Math.Pow(QAAmount, QBAmount);
            }
            else if (mathType == QMATHSTOCK_TYPE.Q1total_Q2mean_Q3med_Q4var_Q5sd.ToString())
            { 
            }
            else if (mathType == QMATHSTOCK_TYPE.Q1_3point_Q3.ToString())
            {
            }
            else
            {
            }
            return dbTotal;
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
        public virtual void InitCalculatorProperties()
        {
            //general calculator properties
            this.Id = 0;
            this.PKId = 0;
            this.CalculatorId = 0;
            this.Version = string.Empty;
            this.CalculatorType = string.Empty;
            this.WhatIfTagName = string.Empty;
            this.FileExtensionType = string.Empty;
            this.CalculatorName = string.Empty;
            this.StylesheetResourceFileName = string.Empty;
            this.StylesheetObjectNS = string.Empty;
            this.Stylesheet2ResourceFileName = string.Empty;
            this.Stylesheet2ObjectNS = string.Empty;
            this.CalculatorDescription = string.Empty;
            this.CalculatorDate = CalculatorHelpers.GetDateShortNow();
            this.UseSameCalculator = false;
            this.Type = string.Empty;
            this.RelatedCalculatorType = string.Empty;
            this.RelatedCalculatorsType = string.Empty;
            this.Overwrite = string.Empty;
            this.SubApplicationType = string.Empty;
            this.ErrorMessage = string.Empty;
            //these are standard aggregators
            this.Label = string.Empty;
            this.Label2 = string.Empty;
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Date = CalculatorHelpers.GetDateShortNow();
            this.TypeId = 0;
            this.TypeName = string.Empty;
            this.GroupId = 0;
            this.GroupName = string.Empty;
            this.DocToCalcReplacementFile = string.Empty;
            //standard analyzer properties
            //comparison option
            this.Option1 = string.Empty;
            //aggregation option (types, groups, labels)
            this.Option2 = string.Empty;
            //this option comes into play when cost effectiveness analyses are built
            this.Option3 = string.Empty;
            //subfolder option (cumulative vs. individual observations)
            this.Option4 = string.Empty;
            this.Option5 = string.Empty;
            this.AnalyzerType = string.Empty;
            this.FilesToAnalyzeExtensionType = string.Empty;
            //basic alternatives (one, two, three)
            this.AlternativeType = string.Empty;
            //targettype (benchmark, actual, partial target)
            this.TargetType = string.Empty;
            //current compared first to baseline, second to xminus1 (if on hand and not baseline)
            this.ChangeType = string.Empty;
            //which element should the calc observation be aggregated into
            this.Alternative2 = 0;
            //how many observations have been aggregated 
            //(user feedback only, use stat objects for more details)
            this.Observations = 0;
            this.Multiplier = 1;
            this.MediaURL = string.Empty;
            this.DataURL = string.Empty;
            this.MathType = string.Empty;
            this.MathSubType = string.Empty;
            this.MathExpression = string.Empty;
            this.MathResult = string.Empty;
            this.MathCILevel = 0;
            this.DataColNames = string.Empty;
            this.DataToAnalyze = new Dictionary<string, List<List<double>>>();
            this.Data2ToAnalyze = new Dictionary<int, List<List<double>>>();
        }
        public void InitSharedObjectProperties()
        {
            //avoid null references to properties
            this.Id = 0;
            this.PKId = 0;
            this.ErrorMessage = string.Empty;
            //inheritor objects
            this.Label = string.Empty;
            this.Label2 = string.Empty;
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Date = CalculatorHelpers.GetDateShortNow();
            this.TypeId = 0;
            this.TypeName = string.Empty;
            this.GroupId = 0;
            this.GroupName = string.Empty;
        }
        public void CopyCalculatorProperties(
            Calculator1 calculator)
        {
            this.Id = calculator.Id;
            this.PKId = calculator.PKId;
            this.CalculatorId = calculator.CalculatorId;
            this.Version = calculator.Version;
            this.CalculatorType = calculator.CalculatorType;
            this.WhatIfTagName = calculator.WhatIfTagName;
            this.FileExtensionType = calculator.FileExtensionType;
            this.CalculatorName = calculator.CalculatorName;
            this.StylesheetResourceFileName = calculator.StylesheetResourceFileName;
            this.StylesheetObjectNS = calculator.StylesheetObjectNS;
            this.Stylesheet2ResourceFileName = calculator.Stylesheet2ResourceFileName;
            this.Stylesheet2ObjectNS = calculator.Stylesheet2ObjectNS;
            this.CalculatorDescription = calculator.CalculatorDescription;
            this.CalculatorDate = calculator.CalculatorDate;
            this.UseSameCalculator = calculator.UseSameCalculator;
            this.Type = calculator.Type;
            this.RelatedCalculatorType = calculator.RelatedCalculatorType;
            this.RelatedCalculatorsType = calculator.RelatedCalculatorsType;
            this.Overwrite = calculator.Overwrite;
            this.SubApplicationType = calculator.SubApplicationType;
            this.ErrorMessage = calculator.ErrorMessage;
            //these are standard aggregators
            this.Label = calculator.Label;
            this.Label2 = calculator.Label2;
            this.Name = calculator.Name;
            this.Description = calculator.Description;
            this.Date = calculator.Date;
            this.TypeId = calculator.TypeId;
            this.TypeName = calculator.TypeName;
            this.GroupId = calculator.GroupId;
            this.GroupName = calculator.GroupName;
            this.DocToCalcReplacementFile = calculator.DocToCalcReplacementFile;
            this.Option1 = calculator.Option1;
            this.Option2 = calculator.Option2;
            this.Option3 = calculator.Option3;
            this.Option4 = calculator.Option4;
            this.Option5 = calculator.Option5;
            this.AnalyzerType = calculator.AnalyzerType;
            this.FilesToAnalyzeExtensionType = calculator.FilesToAnalyzeExtensionType;
            this.AlternativeType = calculator.AlternativeType;
            this.TargetType = calculator.TargetType;
            this.ChangeType = calculator.ChangeType;
            this.Alternative2 = calculator.Alternative2;
            this.Observations = calculator.Observations;
            this.Multiplier = calculator.Multiplier;
            this.MediaURL = calculator.MediaURL;
            this.DataURL = calculator.DataURL;
            //188 run algorithms
            this.MathType = calculator.MathType;
            this.MathSubType = calculator.MathSubType;
            this.MathExpression = calculator.MathExpression;
            this.MathResult = calculator.MathResult;
            this.MathCILevel = calculator.MathCILevel;
            this.DataColNames = calculator.DataColNames;
            //calc1 should not manipulate any objects -its a base class used extensively and should only use simple props
            //copydata when an algo actually needs the data copied
            //this.CopyData(calculator);
        }
        public void CopyData(Calculator1 calculator)
        {
            //dataToAnalyze holds QT vectors from each each ind.dataToAnalyze
            if (this.DataToAnalyze == null)
            {
                this.DataToAnalyze = new Dictionary<string, List<List<double>>>();
            }
            //TSB11Amount1 to TSB15Amount1 store up to five quantitative props for each algo run on inds
            if (calculator.DataToAnalyze != null)
            {
                this.CopyData(calculator.DataToAnalyze);
            }
        }
        public void CopyData(IDictionary<string, List<List<double>>> data)
        {
            //dataToAnalyze holds QT vectors from each each ind.dataToAnalyze
            if (this.DataToAnalyze == null)
            {
                this.DataToAnalyze = new Dictionary<string, List<List<double>>>();
            }
            if (data != null)
            {
                foreach (var d in data)
                {
                    if (this.DataToAnalyze.ContainsKey(d.Key))
                    {
                        foreach (var db in this.DataToAnalyze)
                        {
                            if (db.Key == d.Key)
                            {
                                foreach (var v in d.Value)
                                {
                                    db.Value.Add(v);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.DataToAnalyze.Add(d);
                    }

                }
            }
        }
        public void CopyData(IDictionary<int, List<List<double>>> data)
        {
            //dataToAnalyze holds QT vectors from each each ind.dataToAnalyze
            if (this.Data2ToAnalyze == null)
            {
                this.Data2ToAnalyze = new Dictionary<int, List<List<double>>>();
            }
            if (data != null)
            {
                foreach (var d in data)
                {
                    if (this.Data2ToAnalyze.ContainsKey(d.Key))
                    {
                        foreach (var db in this.Data2ToAnalyze)
                        {
                            if (db.Key == d.Key)
                            {
                                foreach (var v in d.Value)
                                {
                                    db.Value.Add(v);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Data2ToAnalyze.Add(d);
                    }

                }
            }
        }
        public void CopySharedObjectProperties(Calculator1 calculator)
        {
            //avoid null references to properties
            this.Id = calculator.Id;
            this.PKId = calculator.Id;
            this.ErrorMessage = calculator.ErrorMessage;
            //inheritor objects
            this.Label = calculator.Label;
            this.Label2 = calculator.Label2;
            this.Name = calculator.Name;
            this.Description = calculator.Description;
            this.Date = calculator.Date;
            this.TypeId = calculator.TypeId;
            this.TypeName = calculator.TypeName;
            this.GroupId = calculator.GroupId;
            this.GroupName = calculator.GroupName;
        }
        public virtual void AddandSetCalculatorProperties(List<Calculator1> calculators, XElement calculator)
        {
            if (calculators == null)
            {
                calculators = new List<Calculator1>();
            }
            if (calculator != null)
            {
                //must pass in a legit calculator
                if (calculator.Name.LocalName 
                    == Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    //base element stores calcs in last el of baseElement.Calculators
                    Calculator1 newCalc1 = new Calculator1();
                    newCalc1.SetCalculatorProperties(calculator);
                    calculators.Add(newCalc1);
                }
            }
        }
        //set the class properties using the XElement
        public virtual void SetCalculatorProperties(XElement calculator)
        {
            if (calculator != null)
            {
                //some patterns use SetSharedObjectProperties after this method
                //that will overwrite this.Id, so this.CalculatorId has to be used in set attributes
                this.Id = CalculatorHelpers.GetAttributeInt(calculator,
                    cId);
                //second id storage property
                this.PKId = CalculatorHelpers.GetAttributeInt(calculator,
                    cId);
                this.CalculatorId = CalculatorHelpers.GetAttributeInt(calculator,
                    cCalculatorId);
                if (this.CalculatorId == 0)
                {
                    this.CalculatorId = this.Id;
                }
                this.Version = CalculatorHelpers.GetAttribute(calculator,
                    cVersion);
                this.CalculatorType = CalculatorHelpers.GetAttribute(calculator,
                    cCalculatorType);
                this.WhatIfTagName = CalculatorHelpers.GetAttribute(calculator,
                    cWhatIfTagName);
                this.FileExtensionType = CalculatorHelpers.GetAttribute(calculator,
                    cFileExtensionType);
                this.CalculatorName = CalculatorHelpers.GetAttribute(calculator,
                    cCalculatorName);
                this.StylesheetResourceFileName = CalculatorHelpers.GetAttribute(calculator,
                    cStylesheetResourceFileName);
                this.StylesheetObjectNS = CalculatorHelpers.GetAttribute(calculator,
                    cStylesheetObjectNS);
                this.Stylesheet2ResourceFileName = CalculatorHelpers.GetAttribute(calculator,
                    cStylesheet2ResourceFileName);
                this.Stylesheet2ObjectNS = CalculatorHelpers.GetAttribute(calculator,
                    cStylesheet2ObjectNS);
                this.CalculatorDescription = CalculatorHelpers.GetAttribute(calculator,
                    cCalculatorDescription);
                this.CalculatorDate = CalculatorHelpers.GetAttributeDate(calculator,
                    cCalculatorDate);
                this.UseSameCalculator = CalculatorHelpers.GetAttributeBool(calculator,
                    cUseSameCalculator);
                this.Type = CalculatorHelpers.GetAttribute(calculator,
                    cType);
                this.RelatedCalculatorType = CalculatorHelpers.GetAttribute(calculator,
                    cRelatedCalculatorType);
                this.RelatedCalculatorsType = CalculatorHelpers.GetAttribute(calculator,
                    cRelatedCalculatorsType);
                this.Overwrite = CalculatorHelpers.GetAttribute(calculator,
                    cOverwrite);
                this.SubApplicationType = CalculatorHelpers.GetAttribute(calculator,
                    cSubApplicationType);
                this.ErrorMessage = CalculatorHelpers.GetAttribute(calculator,
                    cErrorMessage);
                this.Label = CalculatorHelpers.GetAttribute(calculator,
                    cLabel);
                this.Label2 = CalculatorHelpers.GetAttribute(calculator,
                    cLabel2);
                this.Name = CalculatorHelpers.GetAttribute(calculator,
                    cName);
                this.Description = CalculatorHelpers.GetAttribute(calculator,
                    cDescription);
                this.Date = CalculatorHelpers.GetAttributeDate(calculator,
                    cDate);
                this.TypeId = CalculatorHelpers.GetAttributeInt(calculator,
                    cTypeId);
                this.TypeName = CalculatorHelpers.GetAttribute(calculator,
                    cTypeName);
                this.GroupId = CalculatorHelpers.GetAttributeInt(calculator,
                    cGroupId);
                this.GroupName = CalculatorHelpers.GetAttribute(calculator,
                    cGroupName);
                this.DocToCalcReplacementFile = CalculatorHelpers.GetAttribute(calculator,
                    cDocToCalcReplacementFile);
                this.Option1 = CalculatorHelpers.GetAttribute(calculator,
                    cOption1);
                this.Option2 = CalculatorHelpers.GetAttribute(calculator,
                    cOption2);
                this.Option3 = CalculatorHelpers.GetAttribute(calculator,
                    cOption3);
                this.Option4 = CalculatorHelpers.GetAttribute(calculator,
                    cOption4);
                this.Option5 = CalculatorHelpers.GetAttribute(calculator,
                    cOption5);
                this.AnalyzerType = CalculatorHelpers.GetAttribute(calculator,
                    cAnalyzerType);
                this.FilesToAnalyzeExtensionType = CalculatorHelpers.GetAttribute(calculator,
                    cFilesToAnalyzeExtensionType);
                this.AlternativeType = CalculatorHelpers.GetAttribute(calculator,
                    cAlternativeType);
                this.TargetType = CalculatorHelpers.GetAttribute(calculator,
                    cTargetType);
                this.ChangeType = CalculatorHelpers.GetAttribute(calculator,
                    cChangeType);
                this.Alternative2 = CalculatorHelpers.GetAttributeInt(calculator,
                    cAlternative2);
                this.Observations = CalculatorHelpers.GetAttributeInt(calculator,
                    cObservations);
                this.Multiplier = CalculatorHelpers.GetAttributeDouble(calculator,
                    cMultiplier);
                if (this.Multiplier == 0)
                    this.Multiplier = 1;
                this.MediaURL = CalculatorHelpers.GetAttribute(calculator,
                    cMediaURL);
                this.DataURL = CalculatorHelpers.GetAttribute(calculator,
                    cDataURL);
                this.MathType = CalculatorHelpers.GetAttribute(calculator,
                    cMathType);
                this.MathSubType = CalculatorHelpers.GetAttribute(calculator,
                    cMathSubType);
                this.MathExpression = CalculatorHelpers.GetAttribute(calculator,
                    cMathExpression);
                this.MathResult = CalculatorHelpers.GetAttribute(calculator,
                    cMathResult);
                this.MathCILevel = CalculatorHelpers.GetAttributeInt(calculator,
                    cMathCILevel);
                this.DataColNames = CalculatorHelpers.GetAttribute(calculator,
                    cDataColNames);
                //DataToAnalyze is copied after calcs are run
            }
        }
        //note that these can come from currentel not calculator
        public void SetSharedObjectProperties(XElement currentElement)
        {
            if (currentElement != null)
            {
                this.Id = CalculatorHelpers.GetAttributeInt(currentElement,
                    cId);
                this.PKId = CalculatorHelpers.GetAttributeInt(currentElement,
                    cPKId);
                this.Label = CalculatorHelpers.GetAttribute(currentElement,
                    cLabel);
                this.Label2 = CalculatorHelpers.GetAttribute(currentElement,
                    cLabel2);
                this.Name = CalculatorHelpers.GetAttribute(currentElement,
                    cName);
                this.Description = CalculatorHelpers.GetAttribute(currentElement,
                    cDescription);
                //172: make sure to handle unique inputdate and outputdate names
                if (currentElement.Name.LocalName.Contains(DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
                {
                    this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
                        Input.INPUT_DATE);
                }
                else if (currentElement.Name.LocalName.Contains(DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
                        Output.OUTPUT_DATE);
                }
                else
                {
                    this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
                        cDate);
                }
                this.TypeId = CalculatorHelpers.GetAttributeInt(currentElement,
                    cTypeId);
                this.TypeName = CalculatorHelpers.GetAttribute(currentElement,
                    cTypeName);
                this.GroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    cGroupId);
                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                    cGroupName);
                SetGroupId(currentElement);
            }
        }
        private void SetGroupId(XElement currentElement)
        {
            int iGroupId = 0;
            if (currentElement != null)
            {
                if (currentElement.Name.LocalName.StartsWith(
                    BudgetInvestment.BUDGET_TYPES.budget.ToString()))
                {
                    //if it has a groupid, store it in GroupId field for analyses
                    iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                        BudgetInvestment.BUDGETSYSTEM_ID);
                    if (iGroupId != 0)
                    {
                        this.GroupId = iGroupId;
                        //budget group name (budgetsystem) is not saved
                        this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                            Calculator1.cName);
                        return;
                    }
                    //check opcomp and outcome
                    SetGroupId2(currentElement);
                }
                else if (currentElement.Name.LocalName.StartsWith(
                    BudgetInvestment.INVESTMENT_TYPES.investment.ToString()))
                {
                    iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    BudgetInvestment.INVESTMENTSYSTEM_ID);
                    if (iGroupId != 0)
                    {
                        this.GroupId = iGroupId;
                        //budget group name (budgetsystem) is not saved
                        this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                            Calculator1.cName);
                        return;
                    }
                    //check opcomp and outcome
                    SetGroupId2(currentElement);
                }
                else if (currentElement.Name.LocalName.StartsWith(
                    OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
                {
                    iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    OperationComponent.OPERATION_GROUP_ID);
                    if (iGroupId != 0)
                    {
                        this.GroupId = iGroupId;
                        this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                            OperationComponent.OPERATION_GROUP_NAME);
                        return;
                    }
                }
                else if (currentElement.Name.LocalName.StartsWith(
                    OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
                {
                    iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    OperationComponent.COMPONENT_GROUP_ID);
                    if (iGroupId != 0)
                    {
                        this.GroupId = iGroupId;
                        this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                            OperationComponent.COMPONENT_GROUP_NAME);
                        return;
                    }
                }
                else if (currentElement.Name.LocalName.StartsWith(
                    Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
                {
                    iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    Outcome.OUTCOME_GROUP_ID);
                    if (iGroupId != 0)
                    {
                        this.GroupId = iGroupId;
                        this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                            Outcome.OUTCOME_GROUP_NAME);
                        return;
                    }
                }
                else if (currentElement.Name.LocalName.StartsWith(
                    Input.INPUT_PRICE_TYPES.input.ToString()))
                {
                    iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    Input.INPUT_GROUP_ID);
                    if (iGroupId != 0)
                    {
                        this.GroupId = iGroupId;
                        this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                            Input.INPUT_GROUP_NAME);
                        return;
                    }
                }
                else if (currentElement.Name.LocalName.StartsWith(
                    Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    Output.OUTPUT_GROUP_ID);
                    if (iGroupId != 0)
                    {
                        this.GroupId = iGroupId;
                        this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                            Output.OUTPUT_GROUP_NAME);
                        return;
                    }
                }
            }
        }
        private void SetGroupId2(XElement currentElement)
        {
            int iGroupId = 0;
            //bud and invest opcomp and outcome have to dig deeper
            if (currentElement.Name.LocalName.EndsWith(
                    OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                OperationComponent.OPERATION_GROUP_ID);
                if (iGroupId != 0)
                {
                    this.GroupId = iGroupId;
                    this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                        OperationComponent.OPERATION_GROUP_NAME);
                    return;
                }
            }
            else if (currentElement.Name.LocalName.EndsWith(
                OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                OperationComponent.COMPONENT_GROUP_ID);
                if (iGroupId != 0)
                {
                    this.GroupId = iGroupId;
                    this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                        OperationComponent.COMPONENT_GROUP_NAME);
                    return;
                }
            }
            else if (currentElement.Name.LocalName.EndsWith(
                Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                Outcome.OUTCOME_GROUP_ID);
                if (iGroupId != 0)
                {
                    this.GroupId = iGroupId;
                    this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
                        Outcome.OUTCOME_GROUP_NAME);
                    return;
                }
            }
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetCalculatorProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cId:
                    this.Id = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cPKId:
                    this.PKId = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cVersion:
                    this.Version = attValue;
                    break;
                case cCalculatorType:
                    this.CalculatorType = attValue;
                    break;
                case cWhatIfTagName:
                    this.WhatIfTagName = attValue;
                    break;
                case cFileExtensionType:
                    this.FileExtensionType = attValue;
                    break;
                case cCalculatorName:
                    this.CalculatorName = attValue;
                    break;
                case cStylesheetResourceFileName:
                    this.StylesheetResourceFileName = attValue;
                    break;
                case cStylesheetObjectNS:
                    this.StylesheetObjectNS = attValue;
                    break;
                case cStylesheet2ResourceFileName:
                    this.Stylesheet2ResourceFileName = attValue;
                    break;
                case cStylesheet2ObjectNS:
                    this.Stylesheet2ObjectNS = attValue;
                    break;
                case cCalculatorDescription:
                    this.CalculatorDescription = attValue;
                    break;
                case cCalculatorDate:
                    this.CalculatorDate = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cUseSameCalculator:
                    this.UseSameCalculator = CalculatorHelpers.ConvertStringToBool(attValue);
                    break;
                case cType:
                    this.Type = attValue;
                    break;
                case cRelatedCalculatorType:
                    this.RelatedCalculatorType = attValue;
                    break;
                case cRelatedCalculatorsType:
                    this.RelatedCalculatorsType = attValue;
                    break;
                case cOverwrite:
                    this.Overwrite = attValue;
                    break;
                case cSubApplicationType:
                    this.SubApplicationType = attValue;
                    break;
                case cErrorMessage:
                    this.ErrorMessage = attValue;
                    break;
                case cLabel:
                    this.Label = attValue;
                    break;
                case cLabel2:
                    this.Label2 = attValue;
                    break;
                case cName:
                    this.Name = attValue;
                    break;
                case cDescription:
                    this.Description = attValue;
                    break;
                case cDate:
                    this.Date = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTypeId:
                    this.TypeId = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cTypeName:
                    this.TypeName = attValue;
                    break;
                case cGroupId:
                    this.GroupId = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cGroupName:
                    this.GroupName = attValue;
                    break;
                case cDocToCalcReplacementFile:
                    this.DocToCalcReplacementFile = attValue;
                    break;
                case cOption1:
                    this.Option1 = attValue;
                    break;
                case cOption2:
                    this.Option2 = attValue;
                    break;
                case cOption3:
                    this.Option3 = attValue;
                    break;
                case cOption4:
                    this.Option4 = attValue;
                    break;
                case cOption5:
                    this.Option5 = attValue;
                    break;
                case cAnalyzerType:
                    this.AnalyzerType = attValue;
                    break;
                case cFilesToAnalyzeExtensionType:
                    this.FilesToAnalyzeExtensionType = attValue;
                    break;
                case cAlternativeType:
                    this.AlternativeType = attValue;
                    break;
                case cTargetType:
                    this.TargetType = attValue;
                    break;
                case cChangeType:
                    this.ChangeType = attValue;
                    break;
                case cAlternative2:
                    this.Alternative2 = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cObservations:
                    this.Observations = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cMultiplier:
                    this.Multiplier = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cMediaURL:
                    this.MediaURL = attValue;
                    break;
                case cDataURL:
                    this.DataURL = attValue;
                    break;
                case cMathType:
                    this.MathType = attValue;
                    break;
                case cMathSubType:
                    this.MathSubType = attValue;
                    break;
                case cMathExpression:
                    this.MathExpression = attValue;
                    break;
                case cMathResult:
                    this.MathResult = attValue;
                    break;
                case cMathCILevel:
                    this.MathCILevel = CalculatorHelpers.ConvertStringToInt(attValue);;
                    break;
                case cDataColNames:
                    this.DataColNames = attValue;
                    break;
                default:
                    break;
            }
        }
        public virtual void SetLastCalculatorAttributes(List<Calculator1> calculators,
            string attNameExtension, ref XElement calculator)
        {
            if (calculators != null && calculator != null)
            {
                //must pass in a legit calculator
                if (calculator.Name.LocalName
                    == Data.AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    //base element stores calcs in last el of baseElement.Calculators
                    Calculator1 newCalc1 = calculators.LastOrDefault();
                    if (newCalc1 != null)
                    {
                        newCalc1.SetAndRemoveCalculatorAttributes(attNameExtension, ref calculator);
                    }
                }
            }
        }
        //the attNameExtension is used with attribute indexing _0_1
        public virtual void SetAndRemoveCalculatorAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //don't mix up base objects with calcors - need calcid with calculators
            if (calculator != null)
            {
                //rule: ALWAYS get rid of unwanted old attributes (all calcs refactored for this)
                //note that locals have been lost and must still be set
                RemoveCalculatorAttributes(ref calculator);
                //CalculatorId is used to set Id because SetSharedObjectProps is sometimes used for aggregation and it changes id
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cId, attNameExtension), this.CalculatorId);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cPKId, attNameExtension), this.PKId);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cCalculatorId, attNameExtension), this.CalculatorId);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cVersion, attNameExtension), this.Version);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cCalculatorType, attNameExtension), this.CalculatorType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cWhatIfTagName, attNameExtension), this.WhatIfTagName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cFileExtensionType, attNameExtension), this.FileExtensionType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cCalculatorName, attNameExtension), this.CalculatorName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheetResourceFileName, attNameExtension), this.StylesheetResourceFileName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheetObjectNS, attNameExtension), this.StylesheetObjectNS);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheet2ResourceFileName, attNameExtension), this.Stylesheet2ResourceFileName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheet2ObjectNS, attNameExtension), this.Stylesheet2ObjectNS);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cCalculatorDescription, attNameExtension), this.CalculatorDescription);
                CalculatorHelpers.SetAttributeDateS(calculator,
                    string.Concat(cCalculatorDate, attNameExtension), this.CalculatorDate);
                CalculatorHelpers.SetAttributeBool(calculator,
                    string.Concat(cUseSameCalculator, attNameExtension), this.UseSameCalculator);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cType, attNameExtension), this.Type);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cRelatedCalculatorType, attNameExtension), this.RelatedCalculatorType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cRelatedCalculatorsType, attNameExtension), this.RelatedCalculatorsType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOverwrite, attNameExtension), this.Overwrite);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cSubApplicationType, attNameExtension), this.SubApplicationType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cDocToCalcReplacementFile, attNameExtension), this.DocToCalcReplacementFile);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption1, attNameExtension), this.Option1);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption2, attNameExtension), this.Option2);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption3, attNameExtension), this.Option3);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption4, attNameExtension), this.Option4);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption5, attNameExtension), this.Option5);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cAnalyzerType, attNameExtension), this.AnalyzerType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cFilesToAnalyzeExtensionType, attNameExtension), this.FilesToAnalyzeExtensionType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cAlternativeType, attNameExtension), this.AlternativeType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTargetType, attNameExtension), this.TargetType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cChangeType, attNameExtension), this.ChangeType);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cAlternative2, attNameExtension), this.Alternative2);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cObservations, attNameExtension), this.Observations);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMediaURL, attNameExtension), this.MediaURL);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cDataURL, attNameExtension), this.DataURL);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathType, attNameExtension), this.MathType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathSubType, attNameExtension), this.MathSubType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathExpression, attNameExtension), this.MathExpression);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathResult, attNameExtension), this.MathResult);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cMathCILevel, attNameExtension), this.MathCILevel);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cDataColNames, attNameExtension), this.DataColNames);
                //only concession -Date is always set in base element and often used with calcs
                CalculatorHelpers.SetAttributeDateS(calculator,
                    string.Concat(cDate, attNameExtension), this.Date);
            }
        }
        public void RemoveCalculatorAttributes(ref XElement calculator)
        {
            //old atts should be removed 99% of time
            if (calculator != null)
            {
                calculator.RemoveAttributes();
            }
        }
        public virtual void SetAndRemoveCalculatorAttributes(string attNameExtension,
           XElement calculator)
        {
            //don't mix up base objects with calcors - need calcid with calculators
            if (calculator != null)
            {
                //rule: ALWAYS get rid of unwanted old attributes (all calcs refactored for this)
                //note that locals have been lost and must still be set
                RemoveCalculatorAttributes(calculator);
                //CalculatorId is used to set Id because SetSharedObjectProps is sometimes used for aggregation and it changes id
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cId, attNameExtension), this.CalculatorId);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cPKId, attNameExtension), this.PKId);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cCalculatorId, attNameExtension), this.CalculatorId);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cVersion, attNameExtension), this.Version);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cCalculatorType, attNameExtension), this.CalculatorType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cWhatIfTagName, attNameExtension), this.WhatIfTagName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cFileExtensionType, attNameExtension), this.FileExtensionType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cCalculatorName, attNameExtension), this.CalculatorName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheetResourceFileName, attNameExtension), this.StylesheetResourceFileName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheetObjectNS, attNameExtension), this.StylesheetObjectNS);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheet2ResourceFileName, attNameExtension), this.Stylesheet2ResourceFileName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cStylesheet2ObjectNS, attNameExtension), this.Stylesheet2ObjectNS);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cCalculatorDescription, attNameExtension), this.CalculatorDescription);
                CalculatorHelpers.SetAttributeDateS(calculator,
                    string.Concat(cCalculatorDate, attNameExtension), this.CalculatorDate);
                CalculatorHelpers.SetAttributeBool(calculator,
                    string.Concat(cUseSameCalculator, attNameExtension), this.UseSameCalculator);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cType, attNameExtension), this.Type);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cRelatedCalculatorType, attNameExtension), this.RelatedCalculatorType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cRelatedCalculatorsType, attNameExtension), this.RelatedCalculatorsType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOverwrite, attNameExtension), this.Overwrite);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cSubApplicationType, attNameExtension), this.SubApplicationType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cDocToCalcReplacementFile, attNameExtension), this.DocToCalcReplacementFile);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption1, attNameExtension), this.Option1);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption2, attNameExtension), this.Option2);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption3, attNameExtension), this.Option3);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption4, attNameExtension), this.Option4);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cOption5, attNameExtension), this.Option5);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cAnalyzerType, attNameExtension), this.AnalyzerType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cFilesToAnalyzeExtensionType, attNameExtension), this.FilesToAnalyzeExtensionType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cAlternativeType, attNameExtension), this.AlternativeType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTargetType, attNameExtension), this.TargetType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cChangeType, attNameExtension), this.ChangeType);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cAlternative2, attNameExtension), this.Alternative2);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cObservations, attNameExtension), this.Observations);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMediaURL, attNameExtension), this.MediaURL);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cDataURL, attNameExtension), this.DataURL);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathType, attNameExtension), this.MathType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathSubType, attNameExtension), this.MathSubType);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathExpression, attNameExtension), this.MathExpression);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cMathResult, attNameExtension), this.MathResult);
                CalculatorHelpers.SetAttributeInt(calculator,
                    string.Concat(cMathCILevel, attNameExtension), this.MathCILevel);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cDataColNames, attNameExtension), this.DataColNames);
                //only concession -Date is always set in base element and often used with calcs
                CalculatorHelpers.SetAttributeDateS(calculator,
                    string.Concat(cDate, attNameExtension), this.Date);
            }
        }
        public void RemoveCalculatorAttributes(XElement calculator)
        {
            //old atts should be removed 99% of time
            if (calculator != null)
            {
                calculator.RemoveAttributes();
            }
        }
        
        public virtual void SetSharedObjectAttributes(string attNameExtension,
            ref XElement currentElement)
        {
            if (currentElement != null)
            {
                CalculatorHelpers.SetAttributeInt(currentElement, string.Concat(cId, attNameExtension),
                   this.Id);
                CalculatorHelpers.SetAttribute(currentElement, 
                   string.Concat(Calculator1.cLabel, attNameExtension),
                  this.Label);
                CalculatorHelpers.SetAttribute(currentElement,
                   string.Concat(Calculator1.cLabel2, attNameExtension),
                  this.Label2);
                CalculatorHelpers.SetAttribute(currentElement,
                   string.Concat(Calculator1.cName, attNameExtension),
                  this.Name);
                CalculatorHelpers.SetAttribute(currentElement,
                   string.Concat(Calculator1.cDescription, attNameExtension),
                  this.Description);
                //172: date names differ for inputs and outputs, but handled inside Input and Output object
                CalculatorHelpers.SetAttribute(currentElement,
                    string.Concat(Calculator1.cDate, attNameExtension),
                    this.Date.ToString("d", DateTimeFormatInfo.InvariantInfo));
                CalculatorHelpers.SetAttributeInt(currentElement,
                   string.Concat(Calculator1.cTypeId, attNameExtension),
                  this.TypeId);
                CalculatorHelpers.SetAttribute(currentElement,
                   string.Concat(Calculator1.cTypeName, attNameExtension),
                  this.TypeName);
                CalculatorHelpers.SetAttributeInt(currentElement,
                  string.Concat(Calculator1.cGroupId, attNameExtension),
                 this.GroupId);
                CalculatorHelpers.SetAttribute(currentElement,
                   string.Concat(Calculator1.cGroupName, attNameExtension),
                  this.GroupName);
            }
        }
        public virtual void SetIdAndNameAttributes(ref XElement currentElement)
        {
            if (currentElement != null)
            {
                CalculatorHelpers.SetAttributeInt(currentElement, cId,
                    this.Id);
                CalculatorHelpers.SetAttribute(currentElement, Calculator1.cName,
                  this.Name);
            }
        }
        public virtual void SetLastCalculatorAttributes(List<Calculator1> calculators,
            string attNameExtension, ref XmlWriter writer)
        {
            if (calculators != null && writer != null)
            {
                //base element stores calcs in last el of baseElement.Calculators
                Calculator1 newCalc1 = calculators.LastOrDefault();
                if (newCalc1 != null)
                {
                    newCalc1.SetCalculatorAttributes(attNameExtension, ref writer);
                }
            }
        }
        public virtual void SetCalculatorAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            if (writer != null)
            {
                //CalculatorId is used to set Id because SetSharedObjectProps is sometimes used for aggregation and it changes id
                writer.WriteAttributeString(
                  string.Concat(cId, attNameExtension), this.CalculatorId.ToString());
                writer.WriteAttributeString(
                    string.Concat(cPKId, attNameExtension), this.PKId.ToString());
                writer.WriteAttributeString(
                    string.Concat(cCalculatorId, attNameExtension), this.CalculatorId.ToString());
                writer.WriteAttributeString(
                    string.Concat(cVersion, attNameExtension), this.Version);
                writer.WriteAttributeString(
                    string.Concat(cCalculatorType, attNameExtension), this.CalculatorType);
                writer.WriteAttributeString(
                    string.Concat(cWhatIfTagName, attNameExtension), this.WhatIfTagName);
                writer.WriteAttributeString(
                    string.Concat(cFileExtensionType, attNameExtension), this.FileExtensionType);
                writer.WriteAttributeString(
                    string.Concat(cCalculatorName, attNameExtension), this.CalculatorName);
                writer.WriteAttributeString(
                    string.Concat(cStylesheetResourceFileName, attNameExtension), this.StylesheetResourceFileName);
                writer.WriteAttributeString(
                    string.Concat(cStylesheetObjectNS, attNameExtension), this.StylesheetObjectNS);
                writer.WriteAttributeString(
                    string.Concat(cStylesheet2ResourceFileName, attNameExtension), this.Stylesheet2ResourceFileName);
                writer.WriteAttributeString(
                    string.Concat(cStylesheet2ObjectNS, attNameExtension), this.Stylesheet2ObjectNS);
                writer.WriteAttributeString(
                    string.Concat(cCalculatorDescription, attNameExtension), this.CalculatorDescription);
                writer.WriteAttributeString(
                    string.Concat(cCalculatorDate, attNameExtension), this.CalculatorDate.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                    string.Concat(cUseSameCalculator, attNameExtension), this.UseSameCalculator.ToString());
                writer.WriteAttributeString(
                    string.Concat(cType, attNameExtension), this.Type);
                writer.WriteAttributeString(
                    string.Concat(cRelatedCalculatorType, attNameExtension), this.RelatedCalculatorType);
                writer.WriteAttributeString(
                    string.Concat(cRelatedCalculatorsType, attNameExtension), this.RelatedCalculatorsType);
                writer.WriteAttributeString(
                    string.Concat(cOverwrite, attNameExtension), this.Overwrite);
                writer.WriteAttributeString(
                    string.Concat(cSubApplicationType, attNameExtension), this.SubApplicationType);
                writer.WriteAttributeString(
                    string.Concat(cDocToCalcReplacementFile, attNameExtension), this.DocToCalcReplacementFile);
                writer.WriteAttributeString(
                    string.Concat(cOption1, attNameExtension), this.Option1);
                writer.WriteAttributeString(
                    string.Concat(cOption2, attNameExtension), this.Option2);
                writer.WriteAttributeString(
                    string.Concat(cOption3, attNameExtension), this.Option3);
                writer.WriteAttributeString(
                    string.Concat(cOption4, attNameExtension), this.Option4);
                writer.WriteAttributeString(
                    string.Concat(cOption5, attNameExtension), this.Option5);
                writer.WriteAttributeString(
                    string.Concat(cAnalyzerType, attNameExtension), this.AnalyzerType);
                writer.WriteAttributeString(
                    string.Concat(cFilesToAnalyzeExtensionType, attNameExtension), this.FilesToAnalyzeExtensionType);
                writer.WriteAttributeString(
                    string.Concat(cAlternativeType, attNameExtension), this.AlternativeType);
                writer.WriteAttributeString(
                    string.Concat(cTargetType, attNameExtension), this.TargetType);
                writer.WriteAttributeString(
                    string.Concat(cChangeType, attNameExtension), this.ChangeType);
                writer.WriteAttributeString(
                    string.Concat(cAlternative2, attNameExtension), this.Alternative2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cObservations, attNameExtension), this.Observations.ToString());
                writer.WriteAttributeString(
                    string.Concat(cMediaURL, attNameExtension), this.MediaURL);
                writer.WriteAttributeString(
                    string.Concat(cDataURL, attNameExtension), this.DataURL);
                if (string.IsNullOrEmpty(attNameExtension))
                {
                    writer.WriteAttributeString(
                        string.Concat(cMathType, attNameExtension), this.MathType);
                    writer.WriteAttributeString(
                        string.Concat(cMathSubType, attNameExtension), this.MathSubType);
                    writer.WriteAttributeString(
                        string.Concat(cMathExpression, attNameExtension), this.MathExpression);
                    writer.WriteAttributeString(
                        string.Concat(cMathResult, attNameExtension), this.MathResult);
                    writer.WriteAttributeString(
                        string.Concat(cMathCILevel, attNameExtension), this.MathCILevel.ToString());
                    writer.WriteAttributeString(
                        string.Concat(cDataColNames, attNameExtension), this.DataColNames);
                }
                //only concession -Date is always set in base element and often used with calcs
                writer.WriteAttributeString(
                    string.Concat(cDate, attNameExtension), this.Date.ToString("d", DateTimeFormatInfo.InvariantInfo));
            }
        }
        public virtual async Task SetCalculatorAttributesAsync(string attNameExtension,
            XmlWriter writer)
        {
            if (writer != null)
            {
                //CalculatorId is used to set Id because SetSharedObjectProps is sometimes used for aggregation and it changes id
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cId, attNameExtension), string.Empty, this.CalculatorId.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cPKId, attNameExtension), string.Empty, this.PKId.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cCalculatorId, attNameExtension), string.Empty, this.CalculatorId.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cVersion, attNameExtension), string.Empty, this.Version);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cCalculatorType, attNameExtension), string.Empty, this.CalculatorType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cWhatIfTagName, attNameExtension), string.Empty, this.WhatIfTagName);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cFileExtensionType, attNameExtension), string.Empty, this.FileExtensionType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cCalculatorName, attNameExtension), string.Empty, this.CalculatorName);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cStylesheetResourceFileName, attNameExtension), string.Empty, this.StylesheetResourceFileName);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cStylesheetObjectNS, attNameExtension), string.Empty, this.StylesheetObjectNS);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cStylesheet2ResourceFileName, attNameExtension), string.Empty, this.Stylesheet2ResourceFileName);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cStylesheet2ObjectNS, attNameExtension), string.Empty, this.Stylesheet2ObjectNS);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cCalculatorDescription, attNameExtension), string.Empty, this.CalculatorDescription);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cCalculatorDate, attNameExtension), string.Empty, this.CalculatorDate.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cUseSameCalculator, attNameExtension), string.Empty, this.UseSameCalculator.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cType, attNameExtension), string.Empty, this.Type);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cRelatedCalculatorType, attNameExtension), string.Empty, this.RelatedCalculatorType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cRelatedCalculatorsType, attNameExtension), string.Empty, this.RelatedCalculatorsType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cOverwrite, attNameExtension), string.Empty, this.Overwrite);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cSubApplicationType, attNameExtension), string.Empty, this.SubApplicationType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cDocToCalcReplacementFile, attNameExtension), string.Empty, this.DocToCalcReplacementFile);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cOption1, attNameExtension), string.Empty, this.Option1);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cOption2, attNameExtension), string.Empty, this.Option2);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cOption3, attNameExtension), string.Empty, this.Option3);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cOption4, attNameExtension), string.Empty, this.Option4);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cOption5, attNameExtension), string.Empty, this.Option5);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cAnalyzerType, attNameExtension), string.Empty, this.AnalyzerType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cFilesToAnalyzeExtensionType, attNameExtension), string.Empty, this.FilesToAnalyzeExtensionType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cAlternativeType, attNameExtension), string.Empty, this.AlternativeType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTargetType, attNameExtension), string.Empty, this.TargetType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cChangeType, attNameExtension), string.Empty, this.ChangeType);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cAlternative2, attNameExtension), string.Empty, this.Alternative2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cObservations, attNameExtension), string.Empty, this.Observations.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cMediaURL, attNameExtension), string.Empty, this.MediaURL);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cDataURL, attNameExtension), string.Empty, this.DataURL);
                if (string.IsNullOrEmpty(attNameExtension))
                {
                    await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cMathType, attNameExtension), string.Empty, this.MathType);
                    await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cMathSubType, attNameExtension), string.Empty, this.MathSubType);
                    await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cMathExpression, attNameExtension), string.Empty, this.MathExpression);
                    await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cMathResult, attNameExtension), string.Empty, this.MathResult);
                    await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cMathCILevel, attNameExtension), string.Empty, this.MathCILevel.ToString());
                    await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cDataColNames, attNameExtension), string.Empty, this.DataColNames);
                }
                //only concession -Date is always set in base element and often used with calcs
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cDate, attNameExtension), string.Empty, this.Date.ToString("d", DateTimeFormatInfo.InvariantInfo));
            }
        }
        public virtual void SetSharedObjectAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            if (writer != null)
            {
                writer.WriteAttributeString(
                    string.Concat(cId, attNameExtension), this.Id.ToString());
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cLabel, attNameExtension),
                  this.Label);
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cLabel2, attNameExtension),
                  this.Label2);
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cName, attNameExtension),
                  this.Name);
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cDescription, attNameExtension),
                  this.Description);
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cDate, attNameExtension),
                  this.Date.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cTypeId, attNameExtension),
                  this.TypeId.ToString());
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cTypeName, attNameExtension),
                  this.TypeName);
                writer.WriteAttributeString(
                  string.Concat(Calculator1.cGroupId, attNameExtension),
                 this.GroupId.ToString());
                writer.WriteAttributeString(
                   string.Concat(Calculator1.cGroupName, attNameExtension),
                  this.GroupName);
            }
        }
    }
    public static class Calculator1Extensions
    {
        
    }
}