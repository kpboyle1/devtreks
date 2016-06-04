using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

using DevTreksAppHelpers = DevTreks.Data.AppHelpers;
using DevTreksHelpers = DevTreks.Data.Helpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The BaseCalculator class is a base class used 
    ///             by most standard DevTreks calculators/analyzers to hold 
    ///             base properties, such as ids and names.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    public class BaseCalculator1
    {
        //public BaseCalculator1()
        //{
        //    InitCalculatorProperties();
        //}
        ////copy constructor
        //public BaseCalculator1(BaseCalculator1 calculator)
        //{
        //    CopyCalculatorProperties(calculator);
        //}
        ////general calculator properties
        ////distinguish calculator properties from props for other objects
        //public int Id { get; set; }
        //public int CalculatorId { get; set; }
        //public string Version { get; set; }
        //public string CalculatorType { get; set; }
        //public string WhatIfTagName { get; set; }
        //public string FileExtensionType { get; set; }
        //public string CalculatorName { get; set; }
        //public string StylesheetResourceFileName { get; set; }
        //public string Stylesheet2ResourceFileName { get; set; }
        //public string CalculatorDescription { get; set; }
        //public DateTime CalculatorDate { get; set; }
        //public bool UseSameCalculator { get; set; }
        //public string Type { get; set; }
        //public string RelatedCalculatorType { get; set; }
        //public string RelatedCalculatorsType { get; set; }
        //public string Overwrite { get; set; }
        //public string SubApplicationType { get; set; }
        //public string ErrorMessage { get; set; }
        
        ////properties found in most objects that inherit this as a base class 
        ////(Input.Name, Output.Date). Note that the inheritor has to get and set
        ////these properties (they aren't done in this class because currentEl must be used)
        ////these are standard aggregators
        //public string Label { get; set; }
        //public string Label2 { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public DateTime Date { get; set; }
        //public int TypeId { get; set; }
        //public string TypeName { get; set; }
        //public int GroupId { get; set; }
        //public string GroupName { get; set; }
        ////version 1.1.7: base analyzer props now included in basecalculators
        ////so that they can be serialized and deserialized 
        ////comparison option
        //public string Option1 { get; set; }
        ////aggregation option (types, groups, labels)
        //public string Option2 { get; set; }
        ////this option comes into play when cost effectiveness analyses are built
        //public string Option3 { get; set; }
        ////subfolder option (cumulative vs. individual observations)
        //public string Option4 { get; set; }
        //public string AnalyzerType { get; set; }
        //public string FilesToAnalyzeExtensionType { get; set; }

        ////nonstateful properties (don't store in xml)
        ////easier to pass tech multipliers (Component.Amount) in calc object
        //public double Multiplier { get; set; }

        ////stays consistent with DataAppHelpers.General
        ////basic alternatives (one, two, three)
        //public string AlternativeType { get; set; }
        //public enum ALTERNATIVE_TYPES
        //{
        //    none = 0,
        //    A = 1,
        //    B = 2,
        //    C = 3,
        //    D = 4,
        //    E = 5,
        //    F = 6,
        //    G = 7,
        //    H = 8,
        //    I = 9,
        //    J = 10
        //}
        ////targettype (benchmark, actual, partial target)
        //public string TargetType { get; set; }
        //public enum TARGET_TYPES
        //{
        //    none            = 0,
        //    benchmark       = 1,
        //    partialtarget   = 2,
        //    fulltarget      = 3,
        //    actual          = 4
        //}
        ////current compared first to baseline, second to xminus1 (if on hand and not baseline)
        //public string ChangeType { get; set; }
        //public enum CHANGE_TYPES
        //{
        //    none        = 0,
        //    baseline    = 1,
        //    xminus1     = 2,
        //    current     = 3
        //}
        ////which element should the calc observation be aggregated into
        //public int Alternative2 { get; set; }
        ////how many observations have been aggregated 
        ////(user feedback only, use stat objects for more details)
        //public int Observations { get; set; }

        //public virtual void InitCalculatorProperties()
        //{
        //    //avoid null references to properties
        //    this.Id = 0;
        //    this.CalculatorId = 0;
        //    this.Version = string.Empty;
        //    this.CalculatorType = string.Empty;
        //    this.WhatIfTagName = string.Empty;
        //    this.FileExtensionType = string.Empty;
        //    this.CalculatorName = string.Empty;
        //    this.StylesheetResourceFileName = string.Empty;
        //    this.Stylesheet2ResourceFileName = string.Empty;
        //    this.CalculatorDescription = string.Empty;
        //    this.CalculatorDate = new DateTime();
        //    this.UseSameCalculator = false;
        //    this.Type = string.Empty;
        //    this.RelatedCalculatorType = string.Empty;
        //    this.RelatedCalculatorsType = string.Empty;
        //    this.Overwrite = string.Empty;
        //    this.SubApplicationType = string.Empty;
        //    this.ErrorMessage = string.Empty;
        //    this.Multiplier = 1;
        //    this.AlternativeType = string.Empty;
        //    this.TargetType = string.Empty;
        //    this.ChangeType = string.Empty;
        //    this.Alternative2 = 0;
        //    this.Observations = 0;
        //    //inheritor objects
        //    InitSharedObjectProperties();
        //}
        //public virtual void InitCalculatorProperties()
        //{
        //    //avoid null references to properties
        //    InitCalculatorProperties();
        //    this.Option1 = string.Empty;
        //    this.Option2 = string.Empty;
        //    this.Option3 = string.Empty;
        //    this.Option4 = string.Empty;
        //    this.AnalyzerType = string.Empty;
        //    this.FilesToAnalyzeExtensionType = string.Empty;
        //}
        //public virtual void InitSharedObjectProperties()
        //{
        //    //avoid null references to properties
        //    this.Id = 0;
        //    this.ErrorMessage = string.Empty;
        //    //inheritor objects
        //    this.Label = string.Empty;
        //    this.Label2 = string.Empty;
        //    this.Name = string.Empty;
        //    this.Description = string.Empty;
        //    this.Date = CalculatorHelpers.GetDateShortNow();
        //    this.TypeId = 0;
        //    this.TypeName = string.Empty;
        //    this.GroupId = 0;
        //    this.GroupName = string.Empty;
        //}
        //public virtual void CopyCalculatorProperties(
        //    BaseCalculator1 calculator)
        //{
        //    this.Id = calculator.Id;
        //    this.CalculatorId = calculator.CalculatorId;
        //    this.Version = calculator.Version;
        //    this.CalculatorType = calculator.CalculatorType;
        //    this.WhatIfTagName = calculator.WhatIfTagName;
        //    this.FileExtensionType = calculator.FileExtensionType;
        //    this.CalculatorName = calculator.CalculatorName;
        //    this.StylesheetResourceFileName = calculator.StylesheetResourceFileName;
        //    this.Stylesheet2ResourceFileName = calculator.Stylesheet2ResourceFileName;
        //    this.CalculatorDescription = calculator.CalculatorDescription;
        //    this.CalculatorDate = calculator.CalculatorDate;
        //    this.UseSameCalculator = calculator.UseSameCalculator;
        //    this.Type = calculator.Type;
        //    this.RelatedCalculatorType = calculator.RelatedCalculatorType;
        //    this.RelatedCalculatorsType = calculator.RelatedCalculatorsType;
        //    this.Overwrite = calculator.Overwrite;
        //    this.SubApplicationType = calculator.SubApplicationType;
        //    this.ErrorMessage = calculator.ErrorMessage;
        //    this.Multiplier = calculator.Multiplier;
        //    this.AlternativeType = calculator.AlternativeType;
        //    this.TargetType = calculator.TargetType;
        //    this.ChangeType = calculator.ChangeType;
        //    this.Alternative2 = calculator.Alternative2;
        //    this.Observations = calculator.Observations;
        //    //inheritor objects
        //    CopyCalculatorProperties(calculator);
        //}
        //public virtual void CopyCalculatorProperties(
        //    BaseCalculator1 calculator)
        //{
        //    this.Id = calculator.Id;
        //    this.Label = calculator.Label;
        //    this.Label2 = calculator.Label2;
        //    this.Name = calculator.Name;
        //    this.Description = calculator.Description;
        //    this.Date = calculator.Date;
        //    this.TypeId = calculator.TypeId;
        //    this.TypeName = calculator.TypeName;
        //    this.GroupId = calculator.GroupId;
        //    this.GroupName = calculator.GroupName;
        //}
        //public virtual void CopyCalculatorProperties(
        //    BaseCalculator1 calculator)
        //{
        //    CopyCalculatorProperties(calculator);
        //    this.Option1 = calculator.Option1;
        //    this.Option2 = calculator.Option2;
        //    this.Option3 = calculator.Option3;
        //    this.Option4 = calculator.Option4;
        //    this.AnalyzerType = calculator.AnalyzerType;
        //    this.FilesToAnalyzeExtensionType = calculator.FilesToAnalyzeExtensionType;
        //}
        ////set the class properties using the XElement
        //public virtual void SetCalculatorProperties(XElement calculator)
        //{
        //    if (calculator != null)
        //    {
        //        //distinguish calculator properties and attributes 
        //        //from derived classes (i.e. id is the id of the input, output)
        //        this.Id = CalculatorHelpers.GetAttributeInt(calculator,
        //            Calculator1.cId);
        //        this.CalculatorId = CalculatorHelpers.GetAttributeInt(calculator,
        //            Calculator1.cId);
        //        this.Version = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.VERSION);
        //        this.CalculatorType = CalculatorHelpers.GetAttribute(calculator,
        //            Calculator1.cCalculatorType);
        //        this.WhatIfTagName = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.WHATIF_TAGNAME);
        //        this.FileExtensionType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.FILE_EXTENSION_TYPE);
        //        this.CalculatorName = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.CALCULATOR_NAME);
        //        this.StylesheetResourceFileName = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.STYLESHEET_RESOURCE_FILENAME);
        //        this.Stylesheet2ResourceFileName = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.STYLESHEET2_RESOURCE_FILENAME);
        //        this.CalculatorDescription = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.CALCULATOR_DESCRIPTION);
        //        this.CalculatorDate = CalculatorHelpers.GetAttributeDate(calculator,
        //            DevTreksAppHelpers.General.CALCULATOR_DATE);
        //        this.UseSameCalculator = CalculatorHelpers.GetAttributeBool(calculator,
        //            DevTreksAppHelpers.General.USE_SAME_CALCULATOR);
        //        this.Type = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.TYPE);
        //        this.RelatedCalculatorType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.RELATED_CALCULATOR_TYPE);
        //        this.RelatedCalculatorsType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.RELATED_CALCULATORS_TYPE);
        //        this.Overwrite = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.OVERWRITE);
        //        this.SubApplicationType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.SUBAPPLICATION);
        //        this.ErrorMessage = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.ERROR_MESSAGE);
        //        this.AlternativeType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.ALTERNATIVE_TYPE);
        //        this.TargetType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.TARGET_TYPE);
        //        this.ChangeType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.CHANGE_TYPE);
        //        this.Alternative2 = CalculatorHelpers.GetAttributeInt(calculator,
        //            DevTreksAppHelpers.General.ALTERNATIVE2);
        //        this.Observations = CalculatorHelpers.GetAttributeInt(calculator,
        //            DevTreksAppHelpers.General.OBSERVATIONS);
        //        //note that multiplier is not stateful -just temporarily stores multipliers for stock calcs
        //        //SetSharedObjectProperties using currentel not calculatorel
        //        if (calculator.Name.LocalName != Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
        //        {
        //            //npv stat analyzers pass in a budget element, not a calculator
        //            this.SetSharedObjectProperties(calculator);
        //        }
        //    }
        //}
        ////note these come from currentElement, not calculatorelement
        ////these are needed by the 4 option analyzers to aggregate
        //public virtual void SetSharedObjectProperties(XElement currentElement)
        //{
        //    if (currentElement != null)
        //    {
        //        //set this object's base properties (found in BaseCalculator)
        //        //will use CalculatorId to set the attname so that linkedview has correct lv.dbid
        //        this.Id = CalculatorHelpers.GetAttributeInt(currentElement,
        //            Calculator1.cId);
        //        this.Label = CalculatorHelpers.GetAttribute(currentElement,
        //            Calculator1.cLabel);
        //        this.Label2 = CalculatorHelpers.GetAttribute(currentElement,
        //            Calculator1.cLabel2);
        //        this.Name = CalculatorHelpers.GetAttribute(currentElement,
        //            Calculator1.cName);
        //        this.Description = CalculatorHelpers.GetAttribute(currentElement,
        //           Calculator1.cDescription);
        //        this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
        //            Calculator1.cDate);
        //        this.TypeId = CalculatorHelpers.GetAttributeInt(currentElement,
        //            Calculator1.cTypeId);
        //        this.TypeName = CalculatorHelpers.GetAttribute(currentElement,
        //            Calculator1.cTypeName);
        //        SetGroupId(currentElement);
        //    }
        //}
        //private void SetGroupId(XElement currentElement)
        //{
        //    int iGroupId = 0;
        //    if (currentElement != null)
        //    {
        //        if (currentElement.Name.LocalName.StartsWith(
        //            BudgetInvestment.BUDGET_TYPES.budget.ToString()))
        //        {
        //            //if it has a groupid, store it in GroupId field for analyses
        //            iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //                BudgetInvestment.BUDGETSYSTEM_ID);
        //            if (iGroupId != 0)
        //            {
        //                this.GroupId = iGroupId;
        //                //budget group name (budgetsystem) is not saved
        //                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                    Calculator1.cName);
        //                return;
        //            }
        //            //check opcomp and outcome
        //            SetGroupId2(currentElement);
        //        }
        //        else if (currentElement.Name.LocalName.StartsWith(
        //            BudgetInvestment.INVESTMENT_TYPES.investment.ToString()))
        //        {
        //            iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //            BudgetInvestment.INVESTMENTSYSTEM_ID);
        //            if (iGroupId != 0)
        //            {
        //                this.GroupId = iGroupId;
        //                //budget group name (budgetsystem) is not saved
        //                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                    Calculator1.cName);
        //                return;
        //            }
        //            //check opcomp and outcome
        //            SetGroupId2(currentElement);
        //        }
        //        else if (currentElement.Name.LocalName.StartsWith(
        //            OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
        //        {
        //            iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //            OperationComponent.OPERATION_GROUP_ID);
        //            if (iGroupId != 0)
        //            {
        //                this.GroupId = iGroupId;
        //                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                    OperationComponent.OPERATION_GROUP_NAME);
        //                return;
        //            }
        //        }
        //        else if (currentElement.Name.LocalName.StartsWith(
        //            OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
        //        {
        //            iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //            OperationComponent.COMPONENT_GROUP_ID);
        //            if (iGroupId != 0)
        //            {
        //                this.GroupId = iGroupId;
        //                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                    OperationComponent.COMPONENT_GROUP_NAME);
        //                return;
        //            }
        //        }
        //        else if (currentElement.Name.LocalName.StartsWith(
        //            Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
        //        {
        //            iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //            Outcome.OUTCOME_GROUP_ID);
        //            if (iGroupId != 0)
        //            {
        //                this.GroupId = iGroupId;
        //                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                    Outcome.OUTCOME_GROUP_NAME);
        //                return;
        //            }
        //        }
        //        else if (currentElement.Name.LocalName.StartsWith(
        //            Input.INPUT_PRICE_TYPES.input.ToString()))
        //        {
        //            iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //            Input.INPUT_GROUP_ID);
        //            if (iGroupId != 0)
        //            {
        //                this.GroupId = iGroupId;
        //                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                    Input.INPUT_GROUP_NAME);
        //                return;
        //            }
        //        }
        //        else if (currentElement.Name.LocalName.StartsWith(
        //            Output.OUTPUT_PRICE_TYPES.output.ToString()))
        //        {
        //            iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //            Output.OUTPUT_GROUP_ID);
        //            if (iGroupId != 0)
        //            {
        //                this.GroupId = iGroupId;
        //                this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                    Output.OUTPUT_GROUP_NAME);
        //                return;
        //            }
        //        }
        //    }
        //}
        //private void SetGroupId2(XElement currentElement)
        //{
        //    int iGroupId = 0;
        //    //bud and invest opcomp and outcome have to dig deeper
        //    if (currentElement.Name.LocalName.EndsWith(
        //            OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
        //    {
        //        iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //        OperationComponent.OPERATION_GROUP_ID);
        //        if (iGroupId != 0)
        //        {
        //            this.GroupId = iGroupId;
        //            this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                OperationComponent.OPERATION_GROUP_NAME);
        //            return;
        //        }
        //    }
        //    else if (currentElement.Name.LocalName.EndsWith(
        //        OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
        //    {
        //        iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //        OperationComponent.COMPONENT_GROUP_ID);
        //        if (iGroupId != 0)
        //        {
        //            this.GroupId = iGroupId;
        //            this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                OperationComponent.COMPONENT_GROUP_NAME);
        //            return;
        //        }
        //    }
        //    else if (currentElement.Name.LocalName.EndsWith(
        //        Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
        //    {
        //        iGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
        //        Outcome.OUTCOME_GROUP_ID);
        //        if (iGroupId != 0)
        //        {
        //            this.GroupId = iGroupId;
        //            this.GroupName = CalculatorHelpers.GetAttribute(currentElement,
        //                Outcome.OUTCOME_GROUP_NAME);
        //            return;
        //        }
        //    }
        //}
        //public virtual void SetCalculatorProperties(XElement calculator)
        //{
        //    if (calculator != null)
        //    {
        //        SetCalculatorProperties(calculator);
        //        this.Option1 = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.OPTION1);
        //        this.Option2 = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.OPTION2);
        //        this.Option3 = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.OPTION3);
        //        this.Option4 = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.OPTION4);
        //        this.AnalyzerType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksAppHelpers.General.ANALYZER_TYPE);
        //        this.FilesToAnalyzeExtensionType = CalculatorHelpers.GetAttribute(calculator,
        //            DevTreksHelpers.AddInHelper.FILESTOANALYZE_EXTENSION_TYPE);
        //    }
        //}
        ////attname and attvalue generally passed in from a reader
        //public virtual void SetCalculatorProperties(string attName,
        //    string attValue)
        //{
        //    switch (attName)
        //    {
        //        //distinguish calculator properties and attributes 
        //        //from derived classes (i.e. id is the id of the input, output)
        //        case DevTreksHelpers.GeneralHelpers.ID:
        //            this.Id
        //                = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case Calculator1.cCalculatorId:
        //            this.CalculatorId
        //                = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case DevTreksAppHelpers.General.VERSION:
        //            this.Version = attValue;
        //            break;
        //        case Calculator1.cCalculatorType:
        //            this.CalculatorType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.WHATIF_TAGNAME:
        //            this.WhatIfTagName = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.FILE_EXTENSION_TYPE:
        //            this.FileExtensionType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.CALCULATOR_NAME:
        //            this.CalculatorName = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.STYLESHEET_RESOURCE_FILENAME:
        //            this.StylesheetResourceFileName = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.STYLESHEET2_RESOURCE_FILENAME:
        //            this.Stylesheet2ResourceFileName = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.CALCULATOR_DESCRIPTION:
        //            this.CalculatorDescription = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.CALCULATOR_DATE:
        //            this.CalculatorDate = CalculatorHelpers.ConvertStringToDate(attValue);
        //            break;
        //        case DevTreksAppHelpers.General.USE_SAME_CALCULATOR:
        //            this.UseSameCalculator = CalculatorHelpers.ConvertStringToBool(attValue);
        //            break;
        //        case DevTreksAppHelpers.General.TYPE:
        //            this.Type = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.RELATED_CALCULATOR_TYPE:
        //            this.RelatedCalculatorType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.RELATED_CALCULATORS_TYPE:
        //            this.RelatedCalculatorsType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.OVERWRITE:
        //            this.Overwrite = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.SUBAPPLICATION:
        //            this.SubApplicationType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.ERROR_MESSAGE:
        //            this.ErrorMessage = attValue;
        //            break;
        //        case Calculator1.cLabel:
        //            this.Label = attValue;
        //            break;
        //        case Calculator1.cLabel2:
        //            this.Label2 = attValue;
        //            break;
        //        case Calculator1.cName:
        //            this.Name = attValue;
        //            break;
        //        case Calculator1.cDescription:
        //            this.Description = attValue;
        //            break;
        //        case Calculator1.cDate:
        //            this.Date = CalculatorHelpers.ConvertStringToDate(attValue);
        //            break;
        //        case Calculator1.cTypeId:
        //            this.TypeId = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case Calculator1.cTypeName:
        //            this.TypeName = attValue;
        //            break;
        //        case Calculator1.cGroupId:
        //            this.GroupId = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case Calculator1.cGroupName:
        //            this.GroupName = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.ALTERNATIVE_TYPE:
        //            this.AlternativeType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.TARGET_TYPE:
        //            this.TargetType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.CHANGE_TYPE:
        //            this.ChangeType = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.ALTERNATIVE2:
        //            this.Alternative2 = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case DevTreksAppHelpers.General.OBSERVATIONS:
        //            this.Observations = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        default:
        //            break;
        //    }
        //    SetSharedObjectProperties(attName, attValue);
        //}
        //public virtual void SetSharedObjectProperties(string attName,
        //    string attValue)
        //{
        //    switch (attName)
        //    {
        //        //distinguish calculator properties and attributes 
        //        //from derived classes (i.e. id is the id of the input, output)
        //        case DevTreksHelpers.GeneralHelpers.ID:
        //            this.Id
        //                = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case Calculator1.cLabel:
        //            this.Label = attValue;
        //            break;
        //        case Calculator1.cLabel2:
        //            this.Label2 = attValue;
        //            break;
        //        case Calculator1.cName:
        //            this.Name = attValue;
        //            break;
        //        case Calculator1.cDescription:
        //            this.Description = attValue;
        //            break;
        //        case Calculator1.cDate:
        //            this.Date = CalculatorHelpers.ConvertStringToDate(attValue);
        //            break;
        //        case Calculator1.cTypeId:
        //            this.TypeId = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case Calculator1.cTypeName:
        //            this.TypeName = attValue;
        //            break;
        //        case Calculator1.cGroupId:
        //            this.GroupId = CalculatorHelpers.ConvertStringToInt(attValue);
        //            break;
        //        case Calculator1.cGroupName:
        //            this.GroupName = attValue;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //public virtual void SetCalculatorProperties(string attName,
        //    string attValue)
        //{
        //    SetCalculatorProperties(attName, attValue);
        //    switch (attName)
        //    {
        //        case DevTreksAppHelpers.General.OPTION1:
        //            this.Option1 = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.OPTION2:
        //            this.Option2 = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.OPTION3:
        //            this.Option3 = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.OPTION4:
        //            this.Option4 = attValue;
        //            break;
        //        case DevTreksAppHelpers.General.ANALYZER_TYPE:
        //            this.AnalyzerType = attValue;
        //            break;
        //        case DevTreksHelpers.AddInHelper.FILESTOANALYZE_EXTENSION_TYPE:
        //            this.FilesToAnalyzeExtensionType = attValue;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        
        ////the attNameExtension is used with attribute indexing _0_1
        //public virtual void SetCalculatorAttributes(string attNameExtension,
        //    ref XElement calculator)
        //{
        //    //don't mix up base objects with calcors - need calcid with calculators
        //    if (calculator != null)
        //    {
        //        //note that CalculatorId must be used here (Id is the object that inherits this class)
        //        CalculatorHelpers.SetAttributeInt(calculator,
        //            string.Concat(Calculator1.cId, attNameExtension),
        //            this.CalculatorId);
        //        CalculatorHelpers.SetAttributeInt(calculator,
        //           string.Concat(Calculator1.cCalculatorId, attNameExtension),
        //           this.CalculatorId);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.VERSION, attNameExtension),
        //          this.Version);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(Calculator1.cCalculatorType, attNameExtension),
        //          this.CalculatorType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.WHATIF_TAGNAME, attNameExtension),
        //          this.WhatIfTagName);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.FILE_EXTENSION_TYPE, attNameExtension),
        //          this.FileExtensionType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.CALCULATOR_NAME, attNameExtension),
        //          this.CalculatorName);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.STYLESHEET_RESOURCE_FILENAME, attNameExtension),
        //          this.StylesheetResourceFileName);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.STYLESHEET2_RESOURCE_FILENAME, attNameExtension),
        //          this.Stylesheet2ResourceFileName);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.CALCULATOR_DESCRIPTION, attNameExtension),
        //          this.CalculatorDescription);
        //        CalculatorHelpers.SetAttributeDateS(calculator,
        //          string.Concat(DevTreksAppHelpers.General.CALCULATOR_DATE, attNameExtension),
        //          this.CalculatorDate);
        //        CalculatorHelpers.SetAttributeBool(calculator,
        //          string.Concat(DevTreksAppHelpers.General.USE_SAME_CALCULATOR, attNameExtension),
        //          this.UseSameCalculator);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.TYPE, attNameExtension),
        //          this.Type);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.RELATED_CALCULATOR_TYPE, attNameExtension),
        //          this.RelatedCalculatorType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.RELATED_CALCULATORS_TYPE, attNameExtension),
        //          this.RelatedCalculatorsType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.OVERWRITE, attNameExtension),
        //          this.Overwrite);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.SUBAPPLICATION, attNameExtension),
        //          this.SubApplicationType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.ERROR_MESSAGE, attNameExtension),
        //          this.ErrorMessage);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.ALTERNATIVE_TYPE, attNameExtension),
        //          this.AlternativeType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //         string.Concat(DevTreksAppHelpers.General.TARGET_TYPE, attNameExtension),
        //         this.TargetType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //         string.Concat(DevTreksAppHelpers.General.CHANGE_TYPE, attNameExtension),
        //         this.ChangeType);
        //        CalculatorHelpers.SetAttributeInt(calculator,
        //          string.Concat(DevTreksAppHelpers.General.ALTERNATIVE2, attNameExtension),
        //          this.Alternative2);
        //        CalculatorHelpers.SetAttributeInt(calculator,
        //          string.Concat(DevTreksAppHelpers.General.OBSERVATIONS, attNameExtension),
        //          this.Observations);
        //        //ok to store the sharedobject atts in the calculator (facilitates analysis with labels, groupid, typeid)
        //        this.SetSharedObjectAttributes(attNameExtension, ref calculator);
        //    }
        //}
        //public virtual void SetIdAndNameAttributes(ref XElement currentElement)
        //{
        //    if (currentElement != null)
        //    {
        //        CalculatorHelpers.SetAttributeInt(currentElement, DevTreksHelpers.GeneralHelpers.ID,
        //            this.Id);
        //        CalculatorHelpers.SetAttribute(currentElement, Calculator1.cName,
        //          this.Name);
        //    }
        //}
        ////the props come from currentel but get set in calculatorel
        //public virtual void SetSharedObjectAttributes(string attNameExtension,
        //    ref XElement calculator)
        //{
        //    if (calculator != null)
        //    {
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(Calculator1.cLabel, attNameExtension),
        //          this.Label);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(Calculator1.cLabel2, attNameExtension),
        //          this.Label2);
        //        CalculatorHelpers.SetAttribute(calculator,
        //         string.Concat(Calculator1.cName, attNameExtension),
        //         this.Name);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(Calculator1.cDescription, attNameExtension),
        //          this.Description);
        //        CalculatorHelpers.SetAttributeDateS(calculator,
        //         string.Concat(Calculator1.cDate, attNameExtension),
        //         this.Date);
        //        CalculatorHelpers.SetAttributeInt(calculator,
        //          string.Concat(Calculator1.cTypeId, attNameExtension),
        //          this.TypeId);
        //        CalculatorHelpers.SetAttribute(calculator,
        //         string.Concat(Calculator1.cTypeName, attNameExtension),
        //            this.TypeName);
        //        //use the groupid for aggregation via the calculator
        //        CalculatorHelpers.SetAttributeInt(calculator,
        //          string.Concat(Calculator1.cGroupId, attNameExtension),
        //          this.GroupId);
        //        CalculatorHelpers.SetAttribute(calculator,
        //         string.Concat(Calculator1.cGroupName, attNameExtension),
        //         this.GroupName);
        //    }
        //}
        
        //public virtual void SetAnalyzerAttributes(string attNameExtension,
        //    ref XElement calculator, bool removeAttributes)
        //{
        //    if (calculator != null)
        //    {
        //        if (removeAttributes)
        //        {
        //            RemoveCalculatorAttributes(ref calculator);
        //        }
        //        SetCalculatorAttributes(attNameExtension, ref calculator);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.OPTION1, attNameExtension),
        //          this.Option1);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.OPTION2, attNameExtension),
        //          this.Option2);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.OPTION3, attNameExtension),
        //          this.Option3);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.OPTION4, attNameExtension),
        //          this.Option4);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksAppHelpers.General.ANALYZER_TYPE, attNameExtension),
        //          this.AnalyzerType);
        //        CalculatorHelpers.SetAttribute(calculator,
        //          string.Concat(DevTreksHelpers.AddInHelper.FILESTOANALYZE_EXTENSION_TYPE, attNameExtension),
        //          this.FilesToAnalyzeExtensionType);
        //    }
        //}
        //public void RemoveCalculatorAttributes(ref XElement calculator)
        //{
        //    if (calculator != null)
        //    {
        //        calculator.RemoveAttributes();
        //    }
        //}
        //public virtual void SetCalculatorAttributes(string attNameExtension,
        //    ref XmlWriter writer)
        //{
        //    if (writer != null)
        //    {
        //        //note that CalculatorId must be used here (Id is the object that inherits this class)
        //        writer.WriteAttributeString(
        //            string.Concat(DevTreksHelpers.GeneralHelpers.ID, attNameExtension),
        //           this.CalculatorId.ToString());
        //        writer.WriteAttributeString(
        //            string.Concat(Calculator1.cCalculatorId, attNameExtension),
        //           this.CalculatorId.ToString());
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.VERSION, attNameExtension),
        //          this.Version);
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cCalculatorType, attNameExtension),
        //          this.CalculatorType);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.WHATIF_TAGNAME, attNameExtension),
        //          this.WhatIfTagName);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.FILE_EXTENSION_TYPE, attNameExtension),
        //          this.FileExtensionType);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.CALCULATOR_NAME, attNameExtension),
        //          this.CalculatorName);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.STYLESHEET_RESOURCE_FILENAME, attNameExtension),
        //          this.StylesheetResourceFileName);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.STYLESHEET2_RESOURCE_FILENAME, attNameExtension),
        //          this.Stylesheet2ResourceFileName);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.CALCULATOR_DESCRIPTION, attNameExtension),
        //          this.CalculatorDescription);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.CALCULATOR_DATE, attNameExtension),
        //          this.CalculatorDate.ToString());
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.USE_SAME_CALCULATOR, attNameExtension),
        //          this.UseSameCalculator.ToString());
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.TYPE, attNameExtension),
        //          this.Type);
        //        writer.WriteAttributeString(
        //          string.Concat(DevTreksAppHelpers.General.RELATED_CALCULATOR_TYPE, attNameExtension),
        //         this.RelatedCalculatorType);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.RELATED_CALCULATORS_TYPE, attNameExtension),
        //          this.RelatedCalculatorsType);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.OVERWRITE, attNameExtension),
        //          this.Overwrite);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.SUBAPPLICATION, attNameExtension),
        //          this.SubApplicationType);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.ERROR_MESSAGE, attNameExtension),
        //          this.ErrorMessage);
        //        writer.WriteAttributeString(
        //          string.Concat(DevTreksAppHelpers.General.ALTERNATIVE_TYPE, attNameExtension),
        //         this.AlternativeType);
        //        writer.WriteAttributeString(
        //          string.Concat(DevTreksAppHelpers.General.TARGET_TYPE, attNameExtension),
        //         this.TargetType);
        //        writer.WriteAttributeString(
        //          string.Concat(DevTreksAppHelpers.General.CHANGE_TYPE, attNameExtension),
        //         this.ChangeType);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.ALTERNATIVE2, attNameExtension),
        //          this.Alternative2.ToString());
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.OBSERVATIONS, attNameExtension),
        //          this.Observations.ToString());
        //        //if needed, use a separate SetSharedObjectAtts, but do it separately
        //    }
        //}

        //public virtual void SetSharedObjectAttributes(string attNameExtension,
        //    ref XmlWriter writer)
        //{
        //    if (writer != null)
        //    {
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cLabel, attNameExtension),
        //          this.Label);
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cLabel2, attNameExtension),
        //          this.Label2);
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cName, attNameExtension),
        //          this.Name);
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cDescription, attNameExtension),
        //          this.Description);
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cDate, attNameExtension),
        //          this.Date.ToString());
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cTypeId, attNameExtension),
        //          this.TypeId.ToString());
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cTypeName, attNameExtension),
        //          this.TypeName);
        //        writer.WriteAttributeString(
        //          string.Concat(Calculator1.cGroupId, attNameExtension),
        //         this.GroupId.ToString());
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cGroupName, attNameExtension),
        //          this.GroupName);
        //    }
        //}
        //public virtual void SetBaseObjectAttributes(string attNameExtension,
        //    ref XmlWriter writer)
        //{
        //    //writer is a base element not a calculator
        //    if (writer != null)
        //    {
        //        writer.WriteAttributeString(
        //           string.Concat(Calculator1.cId, attNameExtension),
        //          this.Id.ToString());
        //        SetSharedObjectAttributes(attNameExtension, ref writer);
        //    }
        //}
        //public virtual void SetAnalyzerAttributes(string attNameExtension,
        //    ref XmlWriter writer)
        //{
        //    if (writer != null)
        //    {
        //        SetCalculatorAttributes(attNameExtension, ref writer);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.OPTION1, attNameExtension),
        //          this.Option1);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.OPTION2, attNameExtension),
        //          this.Option2);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.OPTION3, attNameExtension),
        //          this.Option3);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.OPTION4, attNameExtension),
        //          this.Option4);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksAppHelpers.General.ANALYZER_TYPE, attNameExtension),
        //          this.AnalyzerType);
        //        writer.WriteAttributeString(
        //           string.Concat(DevTreksHelpers.AddInHelper.FILESTOANALYZE_EXTENSION_TYPE, attNameExtension),
        //          this.FilesToAnalyzeExtensionType);
        //    }
        //}
    }
}
