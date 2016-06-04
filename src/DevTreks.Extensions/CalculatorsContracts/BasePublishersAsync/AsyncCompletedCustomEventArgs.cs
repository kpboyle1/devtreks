using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		This class implements the c# standard event handler pattern. 
    ///             It holds custom event information passed by publishers 
    ///             to subscribers. The subscribers run calculations on that 
    ///             information (XElement) and return the results back to 
    ///             the publishers.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Future releases will fine tune performance, synchronous 
    ///                 vs. asynchronous processing, ...
    /// </summary>
    public class AsyncCompletedCustomEventArgs : EventArgs
    {
        //constructors
        public AsyncCompletedCustomEventArgs() { }
        public AsyncCompletedCustomEventArgs(CalculatorParameters calcorParams)
        {
            calculatorParams = calcorParams;
        }
        public AsyncCompletedCustomEventArgs(XElement currentElement)
        {
            currentElementToCalc = currentElement;
        }
        //these elements are passed to subscribers
        private XElement currentElementToCalc { get; set; }
        private XElement linkedViewElementToCalc { get; set; }
        private IEnumerable<XElement> linkedViewElements { get; set; }
        //calculator parameters
        private CalculatorParameters calculatorParams { get; set; }
        //true if calcs run successfully
        private bool hasCalculations { get; set; }

        //this element is returned to publisher with
        //calculated results (for building the calculation or analysis doc)
        public XElement CurrentElement
        {
            get { return currentElementToCalc; }
            set { currentElementToCalc = value; }
        }
        //this element is passed from publisher to subscribers
        //containing linked view with instructions for carrying out 
        //a calc/analysis
        public XElement LinkedViewElement
        {
            get { return linkedViewElementToCalc; }
            set { linkedViewElementToCalc = value; }
        }
        //these elements contain all children linkedviews
        //subscriber chooses which one is needed
        public IEnumerable<XElement> LinkedViewElements
        {
            get { return linkedViewElements; }
            set { linkedViewElements = value; }
        }
        public CalculatorParameters CalculatorParams
        {
            get { return calculatorParams; }
            set { calculatorParams = value; }
        }
        public bool HasCalculations
        {
            get { return hasCalculations; }
            set { hasCalculations = value; }
        }
    }
}
