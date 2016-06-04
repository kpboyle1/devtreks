using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The LCCAnalyzer class extends the SubPrice2Stock() class.
    ///             Provides an array of sensitivity analysis and risk analysis
    ///             parameters to vary.
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
    public class LCCAnalyzer : SubPrice3Stock
    {
        //these properties are part of the main collection for this class
        //see p168 Fig14 as an example of multiple stock params to vary for a sensitivity analysis
        //list of indicators 
        public List<LCCAnalyzer> SubP_LCCAnalyzers = new List<LCCAnalyzer>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfSubPrice2s = 20;

        public string LCCName { get; set; }
        //description
        public string LCCDescription { get; set; }
        //aggregation label
        public string LCCLabel { get; set; }

        
    }
}
