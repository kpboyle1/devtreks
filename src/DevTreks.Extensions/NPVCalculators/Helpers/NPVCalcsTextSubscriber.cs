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
    public class NPVCalcsTextSubscriber : ObservationTextBuilder
    {
        //constructors
        public NPVCalcsTextSubscriber() { }
        //constructor sets class properties
        public NPVCalcsTextSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //the base class holds the parameters
        }
         //subscribe to events and build observations document
        public bool BuildObservations()
        {
            bool bHasAnalysis = false;
            //build the observations file (raising the two events
            //from the base event publisher for each node)
            bHasAnalysis = this.StreamAndSaveObservation();
            return bHasAnalysis;
        }
    }
}
