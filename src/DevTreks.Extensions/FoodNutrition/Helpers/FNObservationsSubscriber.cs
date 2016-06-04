using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		This class derives from the CalculatorContracts.ObservationBuilder 
    ///             class. It subscribes to the GetAggregators and GetObservationElement events 
    ///             raised by that class. It builds delimited strings of observation values 
    ///             and places them in observations documents that have the same structure 
    ///             as the data being analyzed.
    ///Author:		www.devtreks.org
    ///Date:		2011, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. In this version, all analyses use the same pattern to build 
    ///             an observations document. If that needs to change in the future, 
    ///             use a switch statement conditional on the analysis type.
    /// </summary>
    public class FNObservationsSubscriber : ObservationBuilder
    {
        //constructors
        public FNObservationsSubscriber() { }
        //constructor sets class properties
        public FNObservationsSubscriber(CalculatorParameters calcParameters,
            FNAnalyzerHelper.ANALYZER_TYPES analyzerType)
            : base(calcParameters)
        {
            //the base class holds the parameters
        }

        //properties
        //analyzers specific to this extension
        public FNAnalyzerHelper.ANALYZER_TYPES AnalyzerType { get; set; }

        //methods
        //subscribe to events and build observations document
        public bool BuildObservations()
        {
            bool bHasAnalysis = false;
            //subscribe to the events raised by the publisher delegate
            this.AddCurrentObservation += SetCurrentObservation;
            //resource use analyzers add linked view attributes to parent nodes
            this.AddLinkedViewToObservation += SetLinkedView;
            //build the observations file (raising the two events
            //from the base event publisher for each node)
            bHasAnalysis = this.StreamAndSaveObservation();
            return bHasAnalysis;
        }
        //define the actions to take when the event is raised
        public void SetCurrentObservation(object sender, CustomEventArgs e)
        {
            //pass a byref xelement from the publisher's data
            XElement obsElement = new XElement(e.CurrentElement);
            FNAnalyzerHelper analyzerHelper = new FNAnalyzerHelper();
            this.AnalyzerType = analyzerHelper.GetAnalyzerType(e.CalculatorParams.AnalyzerParms.AnalyzerType);
            //if observation documents need to differ depending on the analysis
            //use a switch (this.AnalyzerType) here 
            SetObservationAttributes(e.CalculatorParams,
                ref obsElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            e.CurrentElement = new XElement(obsElement);
        }
        //define the actions to take when the event is raised
        public void SetLinkedView(object sender, CustomEventArgs e)
        {
            //pass a byref xelement from the publisher's data
            XElement obsElement = new XElement(e.CurrentElement);
            FNAnalyzerHelper analyzerHelper = new FNAnalyzerHelper();
            this.AnalyzerType = analyzerHelper.GetAnalyzerType(e.CalculatorParams.AnalyzerParms.AnalyzerType);
            //if observation documents need to differ depending on the analysis
            //use a switch (this.AnalyzerType) here 
            SetLinkedViewAttributes(e.CalculatorParams,
                ref obsElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            e.CurrentElement = new XElement(obsElement);
        }
        private void SetObservationAttributes(CalculatorParameters calcParameters, ref XElement obsElement)
        {
            if (calcParameters.DocToCalcReader != null)
            {
                int iAttCount = calcParameters.DocToCalcReader
                    .AttributeCount - 1;
                string sAttName = string.Empty;
                string sAttValue = string.Empty;
                int i = 0;
                for (i = 0; i <= iAttCount; i++)
                {
                    if (i == 0)
                    {
                        calcParameters.DocToCalcReader.MoveToFirstAttribute();
                    }
                    else
                    {
                        calcParameters.DocToCalcReader.MoveToNextAttribute();
                    }
                    sAttName = calcParameters.DocToCalcReader.LocalName;
                    if (calcParameters.DocToCalcReader
                        .ReadAttributeValue())
                    {
                        sAttValue
                            = calcParameters.DocToCalcReader
                            .GetAttribute(sAttName);
                        //only numbers are needed in the observations lists
                        //(the lists are used to generate statistics)
                        bool bNeedsObservationInList
                            = NeedsObservation(sAttName, sAttValue);
                        if (bNeedsObservationInList)
                        {
                            AddObservation(ref obsElement,
                                ref calcParameters, ref sAttName, ref sAttValue);
                            if (!string.IsNullOrEmpty(sAttValue)
                                && !string.IsNullOrEmpty(sAttName))
                            {
                                obsElement.SetAttributeValue(sAttName, sAttValue);
                            }
                        }
                    }
                }
            }
        }
        public bool NeedsObservation(string attName, string attValue)
        {
            bool bNeedsObservation = false;
            //don't change the aggregators used in DevTreks
            if (!attName.Contains(Calculator1.cId)
                && !attName.Contains(Calculator1.cLabel)
                && !attName.Contains(CostBenefitStatistic01.FILES))
            {
                //real numbers needed
                bool bIsNumber
                    = CalculatorHelpers.ValidateIsNumber(attValue);
                if (bIsNumber)
                    bNeedsObservation = true;
            }
            return bNeedsObservation;
        }
        private void SetLinkedViewAttributes(CalculatorParameters calcParameters, ref XElement obsElement)
        {
            if (calcParameters.LinkedViewElement != null)
            {
                XElement linkedView = GetLinkedView(
                    calcParameters.LinkedViewElement);
                if (linkedView != null)
                {
                    XmlReader calculatorReader
                        = linkedView.CreateReader();
                    calculatorReader.MoveToContent();
                    int iAttCount = calculatorReader
                        .AttributeCount - 1;
                    string sAttName = string.Empty;
                    string sAttValue = string.Empty;
                    int i = 0;
                    for (i = 0; i <= iAttCount; i++)
                    {
                        if (i == 0)
                        {
                            calculatorReader.MoveToFirstAttribute();
                        }
                        else
                        {
                            calculatorReader.MoveToNextAttribute();
                        }
                        sAttName = calculatorReader.LocalName;
                        if (calculatorReader
                            .ReadAttributeValue())
                        {
                            sAttValue
                                = calculatorReader
                                .GetAttribute(sAttName);
                            //only numbers are needed in the observations lists
                            //(the lists are used to generate statistics)
                            bool bNeedsLinkedViewAttribute
                                = NeedsLinkedViewAttribute(sAttName, sAttValue);
                            if (bNeedsLinkedViewAttribute)
                            {
                                AddObservation(ref obsElement,
                                    ref calcParameters, ref sAttName, ref sAttValue);
                                if (!string.IsNullOrEmpty(sAttValue)
                                    && !string.IsNullOrEmpty(sAttName))
                                {
                                    obsElement.SetAttributeValue(sAttName, sAttValue);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public bool NeedsLinkedViewAttribute(string attName, string attValue)
        {
            bool bNeedsLinkedViewAttribute = false;
            //don't change the aggregators used in DevTreks
            if (!attName.Contains(Calculator1.cId)
                && !attName.Contains(Calculator1.cName)
                && !attName.Contains(Calculator1.cDescription)
                && !attName.Contains(Calculator1.cLabel)
                && !attName.Contains(CostBenefitStatistic01.FILES))
            {
                //real numbers needed
                bool bIsNumber
                    = CalculatorHelpers.ValidateIsNumber(attValue);
                if (bIsNumber)
                    bNeedsLinkedViewAttribute = true;
            }
            return bNeedsLinkedViewAttribute;
        }
        private void AddObservation(ref XElement obsElement, ref CalculatorParameters calcParameters,
            ref string attName, ref string attValue)
        {
            string sAttributeValueList = string.Empty;
            //convert the attribute name to the statistical object property names
            //used in further analyses (i.e. output.amount to output.tramount)
            CostBenefitCalculator.ConvertAttributeNameToStatisticName(
                calcParameters.CurrentElementNodeName, ref attName);
            if (!string.IsNullOrEmpty(attName))
            {
                if (obsElement.Attribute(attName)
                    != null)
                {
                    //add to the existing observations
                    string sOldAttValue = CalculatorHelpers.GetAttribute(
                        obsElement, attName);
                    string sTotalAttsValue = string.Concat(sOldAttValue,
                        Constants.STRING_DELIMITER, attValue);
                    sAttributeValueList = string.Concat(sTotalAttsValue,
                        Constants.FILENAME_DELIMITER, calcParameters.AnalyzerParms.FilePositionIndex.ToString());
                    //double delimited string: 2.52_1;3.25_1
                    attValue = sAttributeValueList;
                }
                else
                {
                    //add to a new observation
                    //always use standard delimited string
                    sAttributeValueList = string.Concat(attValue,
                        Constants.FILENAME_DELIMITER, calcParameters.AnalyzerParms.FilePositionIndex.ToString());
                    //double delimited string: 2.52_1;3.25_1
                    attValue = sAttributeValueList;
                }
            }
        }
    }
}
