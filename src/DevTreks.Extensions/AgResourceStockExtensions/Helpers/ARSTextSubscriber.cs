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
    ///Purpose:		This class derives from the CalculatorContracts.ObservationTextBuilder 
    ///             class. It builds csv text files from files holding observations.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This version does not subscribe to the AddCurrentColumn event 
    ///             found in the publisher. User feedback is needed to know more 
    ///             about how customers want to filter data.
    /// </summary>
    public class ARSTextSubscriber : ObservationTextBuilder
    {
        //constructors
        public ARSTextSubscriber() { }
        //constructor sets class properties
        public ARSTextSubscriber(CalculatorParameters calcParameters,
            ARSAnalyzerHelper.ANALYZER_TYPES analyzerType)
            : base(calcParameters)
        {
            //the base class holds the parameters
            this.AnalyzerType = analyzerType;
        }

        //properties
        //analyzers specific to this extension
        public ARSAnalyzerHelper.ANALYZER_TYPES AnalyzerType { get; set; }

         //subscribe to events and build observations document
        public bool BuildObservations(IList<string> urisToAnalyze)
        {
            bool bHasAnalysis = false;
            string sFileExtension = ARSAnalyzerHelper.GetFilesToAnalyzeExtension(
                this.ObsCalculatorParams, this.AnalyzerType);
            CalculatorHelpers.SetFileOrFoldersToAnalyze(sFileExtension,
                this.ObsCalculatorParams, urisToAnalyze);
            //build the observations file (raising the two events
            //from the base event publisher for each node)
            bHasAnalysis = this.StreamAndSaveObservation();
            return bHasAnalysis;
        }
    }
}
