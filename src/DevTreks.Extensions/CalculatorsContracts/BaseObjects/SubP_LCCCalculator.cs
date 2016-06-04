using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The LCCCalculator class extends the SubPrice2() class.
    ///Author:		www.devtreks.org
    ///Date:		2013, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       1. The Econ Eval element that inherits from this class must
    ///             use two properties from the base class: SubPrice2.RiskType and 
    ///             SubPrice2.DecisionType. Because all nodes must be calculated using
    ///             uniform risk and decision criteria (they could have been calculated 
    ///             differently).
    ///             
    ///</summary>
    public class SubP_LCCCalculator : SubPrice3
    {

        
        public string LCCName { get; set; }
        //description
        public string LCCDescription { get; set; }
        //aggregation label
        public string LCCLabel { get; set; }

        //sensitity analysis properties 
        //(vary one cost or benefit to see how overall costs/bens vary)
        //subprice to vary for analysis
        public string LCCSensitivityLabel { get; set; }
        //calculator name holding the parameter to vary
        //agmach could be fuel price, hours use, hp, hrs of use ? but how to be 100% sure of 
        //finding that property? would need a step 1 that lists potential calculators, 
        //step 2 lists all of the numeric calculator properties that can be varied
        public string LCCCalculatorName { get; set; }
        //or should it be?
        public string LCCRelatedCalculatorName { get; set; }
        ////sensitity type: price, quantity, escalationrate to vary
        //public string LCCSensitivityType { get; set; }
        //min value for analysis
        public double LCCSensitivityMin { get; set; }
        //max value for analysis
        public double LCCSensitivityMax { get; set; }
    }
}
