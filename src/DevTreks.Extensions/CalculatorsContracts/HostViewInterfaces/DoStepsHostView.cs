using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;

using DevTreks.Data;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Host's view of imported calculators, 
    ///             analyzers, and storytellers. The 
    ///             host defines its own views so that extenders
    ///             have more flexibility i.e. they may choose to 
    ///             develop their own Calculator Contracts, Analyzer Contracts ...
    ///             rather than use DevTreks defaults.
    ///Author:		www.devtreks.org
    ///Date:		2015, January
    ///References:	none
    ///             1. 
    /// </summary>
    public abstract class DoStepsHostView
    {
        //run calculator extensions
        public abstract Task<bool> RunCalculatorStep(
            ExtensionContentURI docToCalcURI, ExtensionContentURI calcDocURI, 
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken);
        //run analyzer extensions
        public abstract Task<bool> RunAnalyzerStep(
            ExtensionContentURI docToCalcURI, ExtensionContentURI calcDocURI, 
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken);

    }
}
