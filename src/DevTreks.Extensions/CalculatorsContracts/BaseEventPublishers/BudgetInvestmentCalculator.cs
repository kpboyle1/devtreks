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
    ///Purpose:		Streams through operating and capital budget documents 
    ///             and publishes events that subscribers 
    ///             can use to run calculations and analyses on 
    ///             each node in each document. Developers can override the 
    ///             protected members in base classes.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. DevTreks 'app store' api will be continually improved.
    ///             2. Publishers should continue experimenting with 
    ///             XStreamingElements and increasing the performance of 
    ///             streaming and event handling.
    ///             
    /// </summary>
    public class BudgetInvestmentCalculator : GeneralCalculator
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected BudgetInvestmentCalculator() { }
        protected BudgetInvestmentCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
        }

        //allows derived classes to override the default streaming 
        //and save method
        protected virtual bool StreamAndSaveCalculation()
        {
            bool bHasCalculations = false;
            //both calculators and analyzers use observationpath for initial file
            if (!CalculatorHelpers.URIAbsoluteExists(
                this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath
                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            if (!CalculatorHelpers.URIAbsoluteExists(
                this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                return bHasCalculations;
            //new temporary path to store calculator results
            //when writing to file, close it with the using
            StringWriter output = new StringWriter();
            this.GCCalculatorParams.DocToCalcReader 
                = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                    this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                    this.GCCalculatorParams.AnalyzerParms.ObservationsPath);
            if (this.GCCalculatorParams.DocToCalcReader != null)
            {
                using (this.GCCalculatorParams.DocToCalcReader)
                {
                    //string sNewDocToCalcTempDocPath = CalculatorHelpers.GetFileToCalculate(this.GCCalculatorParams);
                    //descendant nodes are also processed using streaming techniques
                    //note that XStreamingElements were tested but had faults
                    IEnumerable<XElement> root =
                        from startingElement in StreamRoot()
                        select startingElement;
                    XmlWriterSettings writerSettings = new XmlWriterSettings();
                    writerSettings.OmitXmlDeclaration = true;
                    writerSettings.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(
                        output, writerSettings))
                    {
                        foreach (XElement el in root)
                        {
                            //stop processing if any error message is found
                            if (this.GCCalculatorParams.ErrorMessage == string.Empty)
                            {
                                el.WriteTo(writer);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            using (output)
            {
                if (this.GCCalculatorParams.ErrorMessage == string.Empty
                        && this.GCArguments.HasCalculations)
                {
                    //move the new calculations to tempDocToCalcPath
                    //this returns an error msg
                    this.GCCalculatorParams.ErrorMessage = CalculatorHelpers.MoveURIs(
                        this.GCCalculatorParams.ExtensionDocToCalcURI, output,
                        this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
                    bHasCalculations = true;
                }
            }
            return bHasCalculations;
        }
        protected virtual IEnumerable<XElement> StreamRoot()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamBudgetInvestmentGroups()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            yield return currentElement;
                        }
                    }
                }
            }
        }
        protected virtual IEnumerable<XElement> StreamBudgetInvestmentGroups()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType 
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        this.GCCalculatorParams.ChangeStartingParams(
                            this.GCCalculatorParams.DocToCalcReader.Name);
                        //use streaming techniques to process descendants
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        //attach linked views separately (or the ancestor.linkedview can't be set)
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                            this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentBudget)
                        GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(GCArguments);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamBudgetInvestments()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamBudgetInvestments()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType 
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        //skip using while 
                    }
                    else if (this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
                    {
                        //use streaming techniques to process descendants
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        //attach linked views separately (or the ancestor.linkedview can't be set)
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                            this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentBudget)
                        //that descendants use in their calculations
                        GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(GCArguments);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamTimePeriods()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType 
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                         || this.GCCalculatorParams.DocToCalcReader.Name
                         == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamTimePeriods()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType 
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        //skip using while 
                    }
                    else if (this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        //read currentElement including xmldocnodes
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        //don't process previously calculated nodes
                        bool bIsAnnuity = TimePeriod.IsAnnuity(currentElement);
                        if (!bIsAnnuity)
                        {
                            //attach linked views separately (put in first position)
                            CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                                this.GCCalculatorParams.ExtensionDocToCalcURI,
                                this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                            //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentBudget)
                            //that descendants use in their calculations
                            GCArguments.CurrentElement = currentElement;
                            OnSetAncestorObjects(GCArguments);
                            //use streaming techniques to process descendants
                            IEnumerable<XElement> childElements =
                                from childElement in StreamTimePeriodChildren()
                                select childElement;
                            if (currentElement != null)
                            {
                                if (childElements != null)
                                {
                                    //add the child elements to the currentElement
                                    currentElement.Add(childElements);
                                }
                                this.RunCalculationsAndSetUpdates(ref currentElement);
                                yield return currentElement;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType 
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamTimePeriodChildren()
        {
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                XElement currentElement = null;
                if (this.GCCalculatorParams.DocToCalcReader.NodeType 
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString())
                    {
                        //use streaming techniques to process descendants
                        currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        IEnumerable<XElement> childElements = null;
                        if (this.GCCalculatorParams.DocToCalcReader.Name
                               == BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString())
                        {
                            childElements =
                                from childElement in StreamBudgetOutcome()
                                select childElement;
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.Name
                            == BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString())
                        {
                            childElements =
                                from childElement in StreamInvestmentOutcome()
                                select childElement;
                        }
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                    else if (this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name 
                        == BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString())
                    {
                        //use streaming techniques to process descendants
                        //note: if no costs, would need the same streaming techniques as outcomes
                        currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamOperationComponent()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType 
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        break;
                    }
                }
            }
        }
        
        private IEnumerable<XElement> StreamOperationComponent()
        {
            //loop through the descendants in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
                    {
                        //use streaming techniques to process descendants
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        //attach linked views separately (or the readdescendant syntax skips)
                        CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                            this.GCCalculatorParams.ExtensionDocToCalcURI,
                            this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                        //don't process previously calculated nodes
                        bool bIsAnnuity = TimePeriod.IsAnnuity(currentElement);
                        if (!bIsAnnuity)
                        {
                            //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentBudget)
                            //that descendants use in their calculations
                            GCArguments.CurrentElement = currentElement;
                            OnSetAncestorObjects(GCArguments);
                            IEnumerable<XElement> childElements = null;
                            if (this.GCCalculatorParams.DocToCalcReader.Name
                               == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString())
                            {
                                childElements =
                                    from childElement in StreamBudgetInput()
                                    select childElement;
                            }
                            else if (this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
                            {
                                childElements =
                                    from childElement in StreamInvestmentInput()
                                    select childElement;
                            }
                            if (currentElement != null)
                            {
                                if (childElements != null)
                                {
                                    currentElement.Add(childElements);
                                }
                                this.RunCalculationsAndSetUpdates(ref currentElement);
                                yield return currentElement;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString())
                    {
                        break;
                    }
                }
            }
        }
        
        private IEnumerable<XElement> StreamBudgetOutcome()
        {
            //not all calculations include outcomes, so use ReadToDescendent syntax
            //this works even without outcomes as long as the parent "outcomes" node
            //is yielded, and that node has no xmldoc calcs
            while (this.GCCalculatorParams.DocToCalcReader.ReadToDescendant(
                BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    //use streaming techniques to process descendants
                    XElement currentElement
                        = CalculatorHelpers.GetCurrentElementWithAttributes(
                        this.GCCalculatorParams.DocToCalcReader);
                    //attach linked views separately (or the readdescendant syntax skips)
                    CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                        this.GCCalculatorParams.ExtensionDocToCalcURI,
                        this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                    //don't process previously calculated nodes
                    bool bIsAnnuity = TimePeriod.IsAnnuity(currentElement);
                    if (!bIsAnnuity)
                    {
                        //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentOutcome)
                        //that descendants use in their calculations
                        GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(GCArguments);
                        IEnumerable<XElement> childElements = null;
                        childElements =
                                from childElement in StreamBudgetOutput()
                                select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                currentElement.Add(childElements);
                            }
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                    //process sibling outcomes
                    while (this.GCCalculatorParams.DocToCalcReader
                        .ReadToNextSibling(BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString()))
                    {
                        if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                            XElement siblingElement
                                = CalculatorHelpers.GetCurrentElementWithAttributes(
                                    this.GCCalculatorParams.DocToCalcReader);
                            //attach linked views separately (or the readdescendant syntax skips)
                            CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                                this.GCCalculatorParams.ExtensionDocToCalcURI,
                                this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref siblingElement);
                            //don't process previously calculated nodes
                            bIsAnnuity = TimePeriod.IsAnnuity(siblingElement);
                            if (!bIsAnnuity)
                            {
                                //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentOutcome)
                                //that descendants use in their calculations
                                GCArguments.CurrentElement = siblingElement;
                                OnSetAncestorObjects(GCArguments);
                                IEnumerable<XElement> childElements = null;
                                childElements =
                                        from childElement in StreamBudgetOutput()
                                        select childElement;
                                if (siblingElement != null)
                                {
                                    if (childElements != null)
                                    {
                                        siblingElement.Add(childElements);
                                    }
                                    this.RunCalculationsAndSetUpdates(ref siblingElement);
                                    yield return siblingElement;
                                }
                            }
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.EndElement)
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString()
                                || this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString())
                            {
                                break;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamInvestmentOutcome()
        {
            //not all calculations include outcomes, so use ReadToDescendent syntax
            //this works even without outcomes as long as the parent "outcomes" node
            //is yielded, and that node has no xmldoc calcs
            while (this.GCCalculatorParams.DocToCalcReader.ReadToDescendant(
                BudgetInvestment.INVESTMENT_TYPES.investmentoutcome.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    //use streaming techniques to process descendants
                    XElement currentElement
                        = CalculatorHelpers.GetCurrentElementWithAttributes(
                        this.GCCalculatorParams.DocToCalcReader);
                    //attach linked views separately (or the readdescendant syntax skips)
                    CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                        this.GCCalculatorParams.ExtensionDocToCalcURI,
                        this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref currentElement);
                    //don't process previously calculated nodes
                    bool bIsAnnuity = TimePeriod.IsAnnuity(currentElement);
                    if (!bIsAnnuity)
                    {
                        //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentOutcome)
                        //that descendants use in their calculations
                        GCArguments.CurrentElement = currentElement;
                        OnSetAncestorObjects(GCArguments);
                        IEnumerable<XElement> childElements = null;
                        childElements =
                                from childElement in StreamInvestmentOutput()
                                select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                currentElement.Add(childElements);
                            }
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                    //process sibling outcomes
                    while (this.GCCalculatorParams.DocToCalcReader
                        .ReadToNextSibling(BudgetInvestment.INVESTMENT_TYPES.investmentoutcome.ToString()))
                    {
                        if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                            XElement siblingElement
                                = CalculatorHelpers.GetCurrentElementWithAttributes(
                                    this.GCCalculatorParams.DocToCalcReader);
                            //attach linked views separately (or the readdescendant syntax skips)
                            CalculatorHelpers.AddLinkedViewsToCurrentElementWithReader(
                                this.GCCalculatorParams.ExtensionDocToCalcURI,
                                this.GCCalculatorParams.AnalyzerParms.ObservationsPath, ref siblingElement);
                            //don't process previously calculated nodes
                            bIsAnnuity = TimePeriod.IsAnnuity(siblingElement);
                            if (!bIsAnnuity)
                            {
                                //set stateful ancestor objects (i.e. this.GCCalculatorParams.ParentOutcome)
                                //that descendants use in their calculations
                                GCArguments.CurrentElement = siblingElement;
                                OnSetAncestorObjects(GCArguments);
                                IEnumerable<XElement> childElements = null;
                                childElements =
                                        from childElement in StreamInvestmentOutput()
                                        select childElement;
                                if (siblingElement != null)
                                {
                                    if (childElements != null)
                                    {
                                        siblingElement.Add(childElements);
                                    }
                                    this.RunCalculationsAndSetUpdates(ref siblingElement);
                                    yield return siblingElement;
                                }
                            }
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.EndElement)
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString()
                                || this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString())
                            {
                                break;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentoutcomes.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamBudgetInput()
        {
            //not all analyses include inputs, so use ReadToDescendent syntax
            while (this.GCCalculatorParams.DocToCalcReader
                .ReadToDescendant(BudgetInvestment.BUDGET_TYPES.budgetinput.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    XElement currentElement
                        = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                    if (currentElement != null)
                    {
                        this.RunCalculationsAndSetUpdates(ref currentElement);
                        yield return currentElement;
                    }
                    //process sibling inputs
                    while (this.GCCalculatorParams.DocToCalcReader
                        .ReadToNextSibling(BudgetInvestment.BUDGET_TYPES.budgetinput.ToString()))
                    {
                        if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                             XElement siblingElement
                                    = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                            if (siblingElement != null)
                            {
                                this.RunCalculationsAndSetUpdates(ref siblingElement);
                                yield return siblingElement;
                            }
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.EndElement)
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString()
                                || this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
                            {
                                break;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamInvestmentInput()
        {
            //not all analyses include inputs, so use ReadToDescendent syntax
            while (this.GCCalculatorParams.DocToCalcReader
                .ReadToDescendant(BudgetInvestment.INVESTMENT_TYPES.investmentinput.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    XElement currentElement
                        = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                    if (currentElement != null)
                    {
                        this.RunCalculationsAndSetUpdates(ref currentElement);
                        yield return currentElement;
                    }
                    //process sibling inputs
                    while (this.GCCalculatorParams.DocToCalcReader
                        .ReadToNextSibling(BudgetInvestment.INVESTMENT_TYPES.investmentinput.ToString()))
                    {
                        if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.Element)
                        {
                            XElement siblingElement
                                   = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                            if (siblingElement != null)
                            {
                                this.RunCalculationsAndSetUpdates(ref siblingElement);
                                yield return siblingElement;
                            }
                        }
                        else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                            == XmlNodeType.EndElement)
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString()
                                || this.GCCalculatorParams.DocToCalcReader.Name
                                == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
                            {
                                break;
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamBudgetOutput()
        {
            //not all calculations include outputs, so use ReadToDescendent syntax
            //this works even without outputs as long as the parent "outputs" node
            //is yielded, and that node has no xmldoc calcs
            while (this.GCCalculatorParams.DocToCalcReader.ReadToDescendant(
                BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString())
                    {
                        //process output and any descendant xmldocs
                        XElement currentElement
                            = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                        if (currentElement != null)
                        {
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            //note: stateful ancestor are not needed because opcomps processed after outputs
                            yield return currentElement;
                        }
                        //process sibling outputs
                        while (this.GCCalculatorParams.DocToCalcReader.ReadToNextSibling(
                            BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString()))
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.NodeType
                                == XmlNodeType.Element)
                            {
                                //process output and any descendant xmldocs
                                XElement siblingElement
                                    = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                                if (siblingElement != null)
                                {
                                    this.RunCalculationsAndSetUpdates(ref siblingElement);
                                    //note: stateful ancestor are not needed because opcomps processed after outputs
                                    yield return siblingElement;
                                }
                            }
                            else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                                == XmlNodeType.EndElement)
                            {
                                if (this.GCCalculatorParams.DocToCalcReader.Name
                                    == BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString())
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString())
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamInvestmentOutput()
        {
            //not all calculations include outputs, so use ReadToDescendent syntax
            //this works even without outputs as long as the parent "outputs" node
            //is yielded, and that node has no xmldoc calcs
            while (this.GCCalculatorParams.DocToCalcReader.ReadToDescendant(
                BudgetInvestment.INVESTMENT_TYPES.investmentoutput.ToString()))
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentoutput.ToString())
                    {
                        //process output and any descendant xmldocs
                        XElement currentElement
                            = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                        if (currentElement != null)
                        {
                            this.RunCalculationsAndSetUpdates(ref currentElement);
                            //note: stateful ancestor are not needed because opcomps processed after outputs
                            yield return currentElement;
                        }
                        //process sibling outputs
                        while (this.GCCalculatorParams.DocToCalcReader.ReadToNextSibling(
                            BudgetInvestment.INVESTMENT_TYPES.investmentoutput.ToString()))
                        {
                            if (this.GCCalculatorParams.DocToCalcReader.NodeType
                                == XmlNodeType.Element)
                            {
                                //process output and any descendant xmldocs
                                XElement siblingElement
                                    = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                                if (siblingElement != null)
                                {
                                    this.RunCalculationsAndSetUpdates(ref siblingElement);
                                    //note: stateful ancestor are not needed because opcomps processed after outputs
                                    yield return siblingElement;
                                }
                            }
                            else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                                == XmlNodeType.EndElement)
                            {
                                if (this.GCCalculatorParams.DocToCalcReader.Name
                                    == BudgetInvestment.INVESTMENT_TYPES.investmentoutput.ToString())
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == BudgetInvestment.INVESTMENT_TYPES.investmentoutput.ToString())
                    {
                        break;
                    }
                }
            }
        }
    }
}
