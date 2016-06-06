using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Base calculator class used to raise calculator and 
    ///             analyzer events in one consistent manner. Developers 
    ///             can override the protected members in base classes.
    ///Author:		www.devtreks.org
    ///Date:		2015, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Runcs calculators syncronously
    ///           
    ///             
    /// </summary>
    public class GeneralCalculator
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected GeneralCalculator() { }
        protected GeneralCalculator(CalculatorParameters calcParameters) 
        {
            this.GCCalculatorParams = new CalculatorParameters(calcParameters);
            this.GCArguments = new CustomEventArgs();
        }
        //standard calculator parameters
        public CalculatorParameters GCCalculatorParams { get; set; }

        //declare the events that will be raised
        public event EventHandler<CustomEventArgs> SetAncestorObjects;
        public event EventHandler<CustomEventArgs> RunCalculation;
        public event EventHandler<CustomEventArgs> RunDevPackCalculation;
        public CustomEventArgs GCArguments { get; set; }

        public bool RunCalculationsAndSetUpdates(ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.RunCalculatorType
                == CalculatorHelpers.RUN_CALCULATOR_TYPES.basic)
            {
                bHasCalculations
                    = RunBasicCalculationsAndSetUpdates(ref currentElement);
            }
            else if (this.GCCalculatorParams.RunCalculatorType
                == CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology)
            {
                bHasCalculations
                    = RunIOTechCalculationsAndSetUpdates(ref currentElement);
            }
            else if (this.GCCalculatorParams.RunCalculatorType
                == CalculatorHelpers.RUN_CALCULATOR_TYPES.calcdocuri)
            {
                bHasCalculations
                    = RunIOTechCalculationsAndSetUpdates(ref currentElement);
            }
            else if (this.GCCalculatorParams.RunCalculatorType
                == CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzer)
            {
                bHasCalculations
                    = RunStatisticalCalculation(ref currentElement);
            }
            else if (this.GCCalculatorParams.RunCalculatorType
                == CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects)
            {
                //1.3.6
                bHasCalculations
                    = RunBasicAnalysisAndSetUpdates(ref currentElement);
            }
            return bHasCalculations;
        }
        private bool RunBasicCalculationsAndSetUpdates(ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (!currentElement.HasAttributes)
                return true;
            //1. set parameters needed by updates collection
            this.GCCalculatorParams.CurrentElementNodeName
                = currentElement.Name.LocalName;
            this.GCCalculatorParams.CurrentElementURIPattern
                = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                currentElement);
            //2. don't run calcs on ancestors
            bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
               this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
            if (bIsSelfOrDescendentNode)
            {
                //3. get the calculator to use 
                //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
                XElement linkedViewElement = null;
                linkedViewElement = CalculatorHelpers.GetCalculator(
                    this.GCCalculatorParams, currentElement);
                //some apps, such as locals, work differently 
                AdjustSpecialtyLinkedViewElements(currentElement, ref linkedViewElement);
                //4. Set bool to update base node attributes in db
                this.GCCalculatorParams.AttributeNeedsDbUpdate
                    = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
                //5. raise event to carry out calculations
                GCArguments.CurrentElement = currentElement;
                GCArguments.LinkedViewElement = linkedViewElement;
                OnRunCalculation(GCArguments);
                currentElement = GCArguments.CurrentElement;
                linkedViewElement = GCArguments.LinkedViewElement;
                bHasCalculations = GCArguments.HasCalculations;
                if (bHasCalculations)
                {
                    //6. 100% Rules: don't allow analyzers to db update descendent calculators
                    ChangeLinkedViewCalculator(currentElement, ref linkedViewElement);
                    //7. replace the this.GCCalculatorParams.LinkedViewElement when 
                    //the originating doctocalcuri node is processed
                    bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(this.GCCalculatorParams,
                        currentElement, linkedViewElement);
                    //8. SetXmlDocAttributes
                    CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
                        linkedViewElement, ref currentElement,
                        this.GCCalculatorParams.Updates);
                }
            }
            else
            {
                //basic calculators don't need full collections that include ancestors
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        private bool RunBasicAnalysisAndSetUpdates(ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (!currentElement.HasAttributes)
                return true;
            //1. set parameters needed by updates collection
            this.GCCalculatorParams.CurrentElementNodeName
                = currentElement.Name.LocalName;
            this.GCCalculatorParams.CurrentElementURIPattern
                = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                currentElement);
            //2. don't run calcs on ancestors
            bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
               this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
            if (bIsSelfOrDescendentNode)
            {
                //3. get the calculator to use 
                //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
                XElement linkedViewElement = null;
                linkedViewElement = CalculatorHelpers.GetCalculator(
                    this.GCCalculatorParams, currentElement);
                //4. Set bool to update base node attributes in db
                this.GCCalculatorParams.AttributeNeedsDbUpdate
                    = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
                //5. raise event to carry out calculations
                GCArguments.CurrentElement = currentElement;
                GCArguments.LinkedViewElement = linkedViewElement;
                OnRunCalculation(GCArguments);
                currentElement = GCArguments.CurrentElement;
                //subscriber sets the lv to update 
                linkedViewElement = GCArguments.LinkedViewElement;
                bHasCalculations = GCArguments.HasCalculations;
                if (bHasCalculations)
                {
                    //6. allow analyzers to db update descendent analyzers
                    CalculatorHelpers.ChangeLinkedViewCalculatorForAnalysis(this.GCCalculatorParams, currentElement, 
                        ref linkedViewElement);
                    //7. replace the this.GCCalculatorParams.LinkedViewElement when 
                    //the originating doctocalcuri node is processed
                    bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(this.GCCalculatorParams,
                        currentElement, linkedViewElement);
                    //8. SetXmlDocAttributes
                    CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
                        linkedViewElement, ref currentElement,
                        this.GCCalculatorParams.Updates);
                }
            }
            else
            {
                //version 1.3.6 added this so that ancestors are always added to collections
                //to make analyses consistent (i.e. always start with group)
                GCArguments.CurrentElement = currentElement;
                //ancestors can never update or use a linkedview element
                GCArguments.LinkedViewElement = null;
                //but we want the ancestor current element added to the underlying collections
                OnRunCalculation(GCArguments);
                //always return true, so no error msg is generated
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        private bool RunIOTechCalculationsAndSetUpdates(ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (!currentElement.HasAttributes)
                return true;
            //1. set parameters needed by updates collection
            this.GCCalculatorParams.CurrentElementNodeName
                = currentElement.Name.LocalName;
            this.GCCalculatorParams.CurrentElementURIPattern
                = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                currentElement);
            //2. don't run calcs on ancestors
            bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
               this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
            if (bIsSelfOrDescendentNode)
            {
                //3. get the calculator to use 
                //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
                XElement linkedViewElement = null;
                bool bNeedsRelatedCalculator = NeedsRelatedCalculator();
                if (bNeedsRelatedCalculator)
                {
                    //no db updates -potential to continually have to update in/outs in db
                    linkedViewElement
                        = CalculatorHelpers.GetRelatedCalculator(this.GCCalculatorParams,
                             currentElement);
                }
                else
                {
                    //don't use calctype = input; prefer using stronger relatedcalctype=agmachinery
                    string sCalculatorType = this.GCCalculatorParams.CalculatorType;
                    this.GCCalculatorParams.CalculatorType = string.Empty;
                    linkedViewElement
                       = CalculatorHelpers.GetCalculator(this.GCCalculatorParams, 
                            currentElement);
                    this.GCCalculatorParams.CalculatorType = sCalculatorType;
                }
                //4. Set bool to update base node attributes in db
                this.GCCalculatorParams.AttributeNeedsDbUpdate
                    = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
                //5. raise event to carry out calculations
                GCArguments.CurrentElement = currentElement;
                GCArguments.LinkedViewElement = linkedViewElement;
                OnRunCalculation(GCArguments);
                currentElement = GCArguments.CurrentElement;
                linkedViewElement = GCArguments.LinkedViewElement;
                bHasCalculations = GCArguments.HasCalculations;
                //6. 100% Rules: don't allow analyzers to db update descendent calculators
                ChangeLinkedViewCalculator(currentElement, ref linkedViewElement);
                //inputs have a join-side xmldoc that may need to be updated 
                //to current calculator results (i.e. Input.Times can change
                //the amount of fuel, labor, and other resources for this input)
                //note that the base npvcalculator does not always adjust  
                //resource calculator amounts, such as fuel and labor, when 
                //Input.Times change (if it did, the following would not be needed).
                if (CalculatorHelpers.IsLinkedViewXmlDoc(
                    this.GCCalculatorParams.CurrentElementURIPattern,
                    this.GCCalculatorParams.ExtensionDocToCalcURI))
                {
                    //7. replace the this.GCCalculatorParams.CalculationsElement when 
                    //the originating doctocalcuri node is processed
                    bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(
                        this.GCCalculatorParams, currentElement,
                        linkedViewElement);
                    //8. this pattern combines analyzer and calculator patterns
                    CalculatorHelpers.SetAnalyzerParameters(
                       this.GCCalculatorParams, linkedViewElement);
                }
                //9. SetXmlDocAttributes
                CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
                    linkedViewElement, ref currentElement,
                    this.GCCalculatorParams.Updates);
            }
            else
            {
                //version 1.3.0 added this so that ancestors are always added to collections
                //to make analyses consistent (i.e. always start with group)
                GCArguments.CurrentElement = currentElement;
                //ancestors can never update or use a linkedview element
                GCArguments.LinkedViewElement = null;
                //but we want the ancestor current element added to the underlying collections
                OnRunCalculation(GCArguments);
                //always return true, so no error msg is generated
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        private void AdjustSpecialtyLinkedViewElements(XElement currentElement,
            ref XElement linkedViewElement)
        {
            if (this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                == Constants.SUBAPPLICATION_TYPES.locals.ToString())
            {
                if (currentElement.Name.LocalName
                    == Local.LOCAL_TYPES.localaccountgroup.ToString())
                {
                    linkedViewElement = null;
                }
                else if (currentElement.Name.LocalName
                    == Local.LOCAL_TYPES.local.ToString())
                {
                    //local nodes are treated the same as children of starting group node
                    this.GCCalculatorParams.NeedsCalculators = true;
                    //locals have already been inserted
                    this.GCCalculatorParams.NeedsXmlDocOnly = true;
                    linkedViewElement = new XElement(this.GCCalculatorParams.LinkedViewElement);
                }
            }
        }
        private bool RunStatisticalCalculation(ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //1. set parameters needed by updates collection
            this.GCCalculatorParams.CurrentElementNodeName
                = currentElement.Name.LocalName;
            this.GCCalculatorParams.CurrentElementURIPattern
                = CalculatorHelpers.MakeNewURIPatternFromElement(
                this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                currentElement);
            GCArguments.CurrentElement = currentElement;
            OnRunCalculation(GCArguments);
            currentElement = GCArguments.CurrentElement;
            bHasCalculations = GCArguments.HasCalculations;
            if (!bHasCalculations
               && string.IsNullOrEmpty(this.GCCalculatorParams.ErrorMessage))
                this.GCCalculatorParams.ErrorMessage
                    += Errors.MakeStandardErrorMsg("ANALYSES_CANTRUN");
            return bHasCalculations;
        }
        private bool NeedsRelatedCalculator()
        {
            bool bNeedsRelatedCalculator = false;
            if ((this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                != Constants.SUBAPPLICATION_TYPES.inputprices.ToString()
                && this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                != Constants.SUBAPPLICATION_TYPES.outputprices.ToString())
                && (this.GCCalculatorParams.CurrentElementNodeName.EndsWith(
                Input.INPUT_PRICE_TYPES.input.ToString())
                || this.GCCalculatorParams.CurrentElementNodeName.EndsWith(
                Output.OUTPUT_PRICE_TYPES.output.ToString())))
            {
                bNeedsRelatedCalculator = true;
            }
            return bNeedsRelatedCalculator;
        }
        private void ChangeLinkedViewCalculator(XElement currentElement, ref XElement linkedViewElement)
        {
            //v137 pattern allows analyzers to update descendents using dbupdates
            //i.e. need i/o calculators to get totals, but don't want to 
            //overwrite those calculations in db
            if (this.GCCalculatorParams.ExtensionCalcDocURI.URIDataManager.HostName
                == DevTreks.Data.Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()
                && this.GCCalculatorParams.NeedsCalculators
                && CalculatorHelpers.IsSelfOrChildNode(this.GCCalculatorParams, currentElement.Name.LocalName))
            {
                //100% Rule 1: Analyzers never, ever, update calculators
                string sCalculatorType = CalculatorHelpers.GetAttribute(linkedViewElement, 
                    Calculator1.cCalculatorType);
                //pure calculators never have an analysis type
                string sAnalysisType = CalculatorHelpers.GetAttribute(linkedViewElement,
                    Calculator1.cAnalyzerType);
                if (!string.IsNullOrEmpty(sCalculatorType)
                    && string.IsNullOrEmpty(sAnalysisType))
                {
                    //order of lv retrieval gets calulators before analyzers
                    XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                        currentElement, Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    if (analyzerLV != null)
                    {
                        if (this.GCCalculatorParams.LinkedViewElement != null)
                        {
                            //keep the id and calculatorid, but update the rest of the atts with new lv
                            string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
                            string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
                            analyzerLV = new XElement(this.GCCalculatorParams.LinkedViewElement);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
                            CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
                        }
                        //use it to update db (not the calculator)
                        linkedViewElement = new XElement(analyzerLV); 
                    }
                    else
                    {
                        //use the base linked view standard pattern
                        //avoids updating the wrong lvs
                        linkedViewElement = CalculatorHelpers.GetNewCalculator(this.GCCalculatorParams, currentElement);
                    }
                }
            }
            //100% Rule 2: Analyzers and Calculators never, ever, allow descendent lvs 
            //to have parent Overwrite or UseSameCalc properties
            if (this.GCCalculatorParams.StartingDocToCalcNodeName
                != currentElement.Name.LocalName)
            {
                if (linkedViewElement != null)
                {
                    string sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement, 
                        Calculator1.cUseSameCalculator);
                    if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                    {
                        CalculatorHelpers.SetAttribute(linkedViewElement,
                            Calculator1.cUseSameCalculator, string.Empty);
                    }
                    sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement, 
                        Calculator1.cOverwrite);
                    if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                    {
                        CalculatorHelpers.SetAttribute(linkedViewElement,
                            Calculator1.cOverwrite, string.Empty);
                    }
                }
            }
        }
        
        //allows derived classes to override the event invocation behavior
        protected virtual void OnSetAncestorObjects(CustomEventArgs e)
        {
            //make a temporary copy of the event to avoid possibility of
            //a race condition if the last subscriber unsubscribes
            //immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = SetAncestorObjects;

            //event will be null if there are no subscribers
            if (handler != null)
            {
                //prepare the arguments to send inside the CustomEventArgs parameter
                e.CalculatorParams = this.GCCalculatorParams;
                //use the () operator to raise the event.
                handler(this, e);
            }
        }
        //allows derived classes to override the event invocation behavior
        protected virtual void OnRunCalculation(CustomEventArgs e)
        {
            //make a temporary copy of the event to avoid possibility of
            //a race condition if the last subscriber unsubscribes
            //immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = RunCalculation;

            //event will be null if there are no subscribers
            if (handler != null)
            {
                //prepare the arguments to send inside the CustomEventArgs parameter
                e.CalculatorParams = this.GCCalculatorParams;
                //use the () operator to raise the event.
                handler(this, e);
            }
        }
        
        //allows derived classes to override the default streaming 
        //and save method
        protected virtual bool RunDevPackCalculations()
        {
            bool bHasCalculation = false;
            IDictionary<string, string> fileOrFolderPaths =
                CalculatorHelpers.GetDevPackState(this.GCCalculatorParams);
            if (string.IsNullOrEmpty(this.GCCalculatorParams.ErrorMessage))
            {
                if (fileOrFolderPaths != null)
                {
                    if (fileOrFolderPaths.Count > 0)
                    {
                        string sId = string.Empty;
                        string sDocToCalcFilePath = string.Empty;
                        string sTempDocToCalcPath = string.Empty;
                        int i = 0;
                        foreach (KeyValuePair<string, string> kvp
                            in fileOrFolderPaths)
                        {
                            bHasCalculation = false;
                            sId = kvp.Key;
                            sDocToCalcFilePath = kvp.Value;
                            if (CalculatorHelpers.URIAbsoluteExists(
                                this.GCCalculatorParams.ExtensionDocToCalcURI, 
                                sDocToCalcFilePath))
                            {
                                //make new calcparams using sDocToCalcFilePath
                                ExtensionContentURI devPackCalcURI
                                    = CalculatorHelpers.GetDevPackCalcURI(this.GCCalculatorParams.ExtensionDocToCalcURI,
                                    this.GCCalculatorParams.ExtensionCalcDocURI, sDocToCalcFilePath,
                                    ref sTempDocToCalcPath);
                                //copy the doctocalc to tempdoctocalcpath
                                CalculatorHelpers.CopyFiles(this.GCCalculatorParams.ExtensionDocToCalcURI,
                                    sDocToCalcFilePath, sTempDocToCalcPath);
                                //reset calculatorparams to reflect devPackCalcURI
                                CalculatorParameters devPackCalcParameters
                                    = new CalculatorParameters(this.GCCalculatorParams);
                                devPackCalcParameters.CurrentElementNodeName
                                    = devPackCalcURI.URINodeName;
                                devPackCalcParameters.CurrentElementURIPattern
                                    = devPackCalcURI.URIPattern;
                                //reset the calcparams.extdoctocalcuri
                                devPackCalcParameters.ExtensionDocToCalcURI
                                    = devPackCalcURI;

                                //currentDevPackElement has current db-generated linked views
                                //and is more reliable for updating linked views in db
                                XElement currentDevPackElement = null;
                                XElement linkedViewElement = null;
                                CalculatorHelpers.GetDevPackCalculator(this.GCCalculatorParams,
                                    devPackCalcParameters, out currentDevPackElement, 
                                    out linkedViewElement);
                                //4. raise event to carry out calculations
                                GCArguments.CurrentElement = currentDevPackElement;
                                GCArguments.LinkedViewElement = linkedViewElement;
                                GCArguments.CalculatorParams = devPackCalcParameters;
                                GCArguments.CalculatorParams.CurrentElement = currentDevPackElement;
                                GCArguments.CalculatorParams.LinkedViewElement = linkedViewElement;
                                OnRunDevPackCalculation(GCArguments);
                                bHasCalculation = GCArguments.HasCalculations;
                                currentDevPackElement = GCArguments.CurrentElement;
                                linkedViewElement = GCArguments.LinkedViewElement;
                                //5. SetXmlDocAttributes (using devPackElement)
                                if (bHasCalculation)
                                {
                                    //updates for children
                                    CalculatorHelpers.SetXmlDocUpdates(devPackCalcParameters,
                                        linkedViewElement, ref currentDevPackElement,
                                        this.GCCalculatorParams.Updates);
                                    //add the tempdoctocalcpath to updates for potential saving later
                                    string sErrorMsg = string.Empty;
                                    CalculatorHelpers.SetDevPackUpdatesState(
                                        devPackCalcURI, this.GCCalculatorParams.ExtensionCalcDocURI,
                                        this.GCCalculatorParams.Updates, ref sErrorMsg);
                                    if (i == 0)
                                    {
                                        //set the parent devpack
                                        this.GCCalculatorParams.CurrentElementNodeName
                                            = this.GCCalculatorParams.ExtensionDocToCalcURI.URINodeName;
                                        this.GCCalculatorParams.CurrentElementURIPattern
                                            = this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern;
                                        //currentDevPackElement has current db-generated linked views
                                        //and is reliable for updating linked views in db
                                        CalculatorHelpers.GetDevPackCalculator(this.GCCalculatorParams,
                                            this.GCCalculatorParams, out currentDevPackElement, 
                                            out linkedViewElement);
                                        //update the parent
                                        this.GCCalculatorParams.NeedsXmlDocOnly = true;
                                        CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
                                            linkedViewElement, ref currentDevPackElement,
                                            this.GCCalculatorParams.Updates);
                                        //show them the first calculated document as feedback
                                        //refactor for future to show a summary of all calculated custom docs
                                        //move the doctocalc to tempdoctocalcpath
                                        CalculatorHelpers.CopyFiles(
                                            this.GCCalculatorParams.ExtensionDocToCalcURI, sTempDocToCalcPath,
                                            this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
                                    }
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            return bHasCalculation;
        }
        
        //allows derived classes to override the event invocation behavior
        protected virtual void OnRunDevPackCalculation(CustomEventArgs e)
        {
            //make a temporary copy of the event to avoid possibility of
            //a race condition if the last subscriber unsubscribes
            //immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = RunDevPackCalculation;

            //event will be null if there are no subscribers
            if (handler != null)
            {
                //prepare the arguments to send inside the CustomEventArgs parameter
                //use the () operator to raise the event.
                handler(this, e);
            }
        }
    }
}
