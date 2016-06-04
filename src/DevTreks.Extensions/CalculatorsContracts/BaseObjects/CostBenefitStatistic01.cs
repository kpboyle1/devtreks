using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;

using DataHelpers = DevTreks.Data.Helpers;
using DataAppHelpers = DevTreks.Data.AppHelpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The CostBenefitStatistic01 derives from the CostBenefitCalculator class 
    ///             and is a base class used by cost and benefit analyzers 
    ///             that need basic statistics (mean, standard deviation, variance,
    ///             median). The virtual methods are meant 
    ///             to be overridden because some analyses, due to file size 
    ///             and performance issues, need to limit the properties 
    ///             used in an object and subsequently deserialized to 
    ///             an xelement's attributes.
    ///             Helpers.AnalyzerParameters has a member of this type.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. This class is currently limited to four basic statistics: 
    ///             mean, standard deviation, variance, and median.
    ///             2. In general, derived classes such as this one should 
    ///             not make an internal call within their methods to the 
    ///             base CostBenefitCalculator class (i.e. this.SetTotalCostsProperies)
    ///             In order to keep the size of files down, and increase their
    ///             performance, its better to specify the exact properties 
    ///             and attributes that are needed by specific analyzers. 
    ///             That's also the reason for specifying the statistic 
    ///             within the method (SetMean, SetStdDev ...).
    ///             3. Advanced statistical analyses should consider using 
    ///             the same object structure and deriving from classes like 
    ///             this one.
    ///             4. Data definition documents will become available during 
    ///             late stage testing.
    ///</summary>          
    public class CostBenefitStatistic01 : CostBenefitCalculator
    {
        //calls the base-class version, and initializes the base class properties.
        public CostBenefitStatistic01() : base()
        {
            //avoid null references
            InitStatisticGeneralProperties();
            InitMeanBenefitsProperties();
            InitMeanCostsProperties();
            InitStdDevBenefitsProperties();
            InitStdDevCostsProperties();
            InitVarianceBenefitsProperties();
            InitVarianceCostsProperties();
            InitMedianBenefitsProperties();
            InitMedianCostsProperties();
            InitNBenefitsProperties();
            //v145 deprecated N in favor of one property per element
            InitNCostsProperties();
        }
        //copy constructor
        public CostBenefitStatistic01(CostBenefitStatistic01 calculator)
            : base(calculator)

        {
            CopyStatisticGeneralProperties(calculator);
            CopyMeanBenefitsProperties(calculator);
            CopyMeanCostsProperties(calculator);
            CopyStdDevBenefitsProperties(calculator);
            CopyStdDevCostsProperties(calculator);
            CopyVarianceBenefitsProperties(calculator);
            CopyVarianceCostsProperties(calculator);
            CopyMedianBenefitsProperties(calculator);
            CopyMedianCostsProperties(calculator);
            CopyNBenefitsProperties(calculator);
            CopyNCostsProperties(calculator);
        }
        //general
        public double TotalObservations { get; set; }
        public int FilesComparisonsCount { get; set; }
        public int NodeCount { get; set; }
        //original id of node before random id used
        public int OldId { get; set; }
        public const string OBSERVATIONS = "Observations";
        public const string FILES = "Files";
        public const string NODES = "Nodes";
        //summary column name
        public const string ALL = "All";
        //basic stat substring
        public const string TOTAL = "TOTAL";
        public const string MEAN = "MEAN";
        public const string VAR2 = "VAR2";
        public const string SD = "SD";
        public const string MED = "MED";
        //note that "N" alone isn't enough of a suffix
        public const string N = "N1";
        private const string NET = "NET";

        //mean benefits
        public const string TRAmount_MEAN = "TRAmount_MEAN";
        public const string TRPrice_MEAN = "TRPrice_MEAN";
        public const string TRCompositionAmount_MEAN = "TRCompositionAmount_MEAN";
        public const string TR_MEAN = "TR_MEAN";
        public const string TR_INT_MEAN = "TR_INT_MEAN";
        public const string TRINCENT_MEAN = "TRINCENT_MEAN";
        public const string TAMR_MEAN = "TAMR_MEAN";
        public const string TAMRINCENT_MEAN = "TAMRINCENT_MEAN";

        //mean costs
        public const string TOCAmount_MEAN = "TOCAmount_MEAN";
        public const string TOCPrice_MEAN = "TOCPrice_MEAN";
        public const string TOC_MEAN = "TOC_MEAN";
        public const string TOC_INT_MEAN = "TOC_INT_MEAN";
        public const string TAOHAmount_MEAN = "TAOHAmount_MEAN";
        public const string TAOHPrice_MEAN = "TAOHPrice_MEAN";
        public const string TAOH_MEAN = "TAOH_MEAN";
        public const string TAOH_INT_MEAN = "TAOH_INT_MEAN";
        public const string TCAPAmount_MEAN = "TCAPAmount_MEAN";
        public const string TCAPPrice_MEAN = "TCAPPrice_MEAN";
        public const string TCAP_MEAN = "TCAP_MEAN";

        public const string TCAP_INT_MEAN = "TCAP_INT_MEAN";
        //incentives
        public const string TINCENT_MEAN = "TINCENT_MEAN";
        //amortized costs
        public const string TAMOC_MEAN = "TAMOC_MEAN";
        public const string TAMAOH_MEAN = "TAMAOH_MEAN";
        public const string TAMCAP_MEAN = "TAMCAP_MEAN";
        public const string TAMOC_INT_MEAN = "TAMOC_INT_MEAN";
        public const string TAMAOH_INT_MEAN = "TAMAOH_INT_MEAN";
        public const string TAMCAP_INT_MEAN = "TAMCAP_INT_MEAN";
        public const string TAMOC_NET_MEAN = "TAMOC_NET_MEAN";
        public const string TAMAOH_NET_MEAN = "TAMAOH_NET_MEAN";
        public const string TAMAOH_NET2_MEAN = "TAMAOH_NET2_MEAN";
        public const string TAMCAP_NET_MEAN = "TAMCAP_NET_MEAN";
        public const string TAMINCENT_MEAN = "TAMINCENT_MEAN";
        public const string TAMINCENT_NET_MEAN = "TAMINCENT_NET_MEAN";

        //standard deviation benefits
        public const string TRAmount_SD = "TRAmount_SD";
        public const string TRPrice_SD = "TRPrice_SD";
        public const string TRCompositionAmount_SD = "TRCompositionAmount_SD";
        public const string TR_SD = "TR_SD";
        public const string TR_INT_SD = "TR_INT_SD";
        public const string TRINCENT_SD = "TRINCENT_SD";
        public const string TAMR_SD = "TAMR_SD";
        public const string TAMRINCENT_SD = "TAMRINCENT_SD";

        //standard deviaion costs
        public const string TOCAmount_SD = "TOCAmount_SD";
        public const string TOCPrice_SD = "TOCPrice_SD";
        public const string TOC_SD = "TOC_SD";
        public const string TOC_INT_SD = "TOC_INT_SD";
        public const string TAOHAmount_SD = "TAOHAmount_SD";
        public const string TAOHPrice_SD = "TAOHPrice_SD";
        public const string TAOH_SD = "TAOH_SD";
        public const string TAOH_INT_SD = "TAOH_INT_SD";
        public const string TCAPAmount_SD = "TCAPAmount_SD";
        public const string TCAPPrice_SD = "TCAPPrice_SD";
        public const string TCAP_SD = "TCAP_SD";

        public const string TCAP_INT_SD = "TCAP_INT_SD";
        //incentives
        public const string TINCENT_SD = "TINCENT_SD";
        //amortized costs
        public const string TAMOC_SD = "TAMOC_SD";
        public const string TAMAOH_SD = "TAMAOH_SD";
        public const string TAMCAP_SD = "TAMCAP_SD";
        public const string TAMOC_INT_SD = "TAMOC_INT_SD";
        public const string TAMAOH_INT_SD = "TAMAOH_INT_SD";
        public const string TAMCAP_INT_SD = "TAMCAP_INT_SD";
        public const string TAMOC_NET_SD = "TAMOC_NET_SD";
        public const string TAMAOH_NET_SD = "TAMAOH_NET_SD";
        public const string TAMAOH_NET2_SD = "TAMAOH_NET2_SD";
        public const string TAMCAP_NET_SD = "TAMCAP_NET_SD";
        public const string TAMINCENT_SD = "TAMINCENT_SD";
        public const string TAMINCENT_NET_SD = "TAMINCENT_NET_SD";

        //variance benefits
        public const string TRAmount_VAR2 = "TRAmount_VAR2";
        public const string TRPrice_VAR2 = "TRPrice_VAR2";
        public const string TRCompositionAmount_VAR2 = "TRCompositionAmount_VAR2";
        public const string TR_VAR2 = "TR_VAR2";
        public const string TR_INT_VAR2 = "TR_INT_VAR2";
        public const string TRINCENT_VAR2 = "TRINCENT_VAR2";
        public const string TAMR_VAR2 = "TAMR_VAR2";
        public const string TAMRINCENT_VAR2 = "TAMRINCENT_VAR2";

        //variance costs
        public const string TOCAmount_VAR2 = "TOCAmount_VAR2";
        public const string TOCPrice_VAR2 = "TOCPrice_VAR2";
        public const string TOC_VAR2 = "TOC_VAR2";
        public const string TOC_INT_VAR2 = "TOC_INT_VAR2";
        public const string TAOHAmount_VAR2 = "TAOHAmount_VAR2";
        public const string TAOHPrice_VAR2 = "TAOHPrice_VAR2";
        public const string TAOH_VAR2 = "TAOH_VAR2";
        public const string TAOH_INT_VAR2 = "TAOH_INT_VAR2";
        public const string TCAPAmount_VAR2 = "TCAPAmount_VAR2";
        public const string TCAPPrice_VAR2 = "TCAPPrice_VAR2";
        public const string TCAP_VAR2 = "TCAP_VAR2";

        public const string TCAP_INT_VAR2 = "TCAP_INT_VAR2";
        //incentives
        public const string TINCENT_VAR2 = "TINCENT_VAR2";
        //amortized costs
        public const string TAMOC_VAR2 = "TAMOC_VAR2";
        public const string TAMAOH_VAR2 = "TAMAOH_VAR2";
        public const string TAMCAP_VAR2 = "TAMCAP_VAR2";
        public const string TAMOC_INT_VAR2 = "TAMOC_INT_VAR2";
        public const string TAMAOH_INT_VAR2 = "TAMAOH_INT_VAR2";
        public const string TAMCAP_INT_VAR2 = "TAMCAP_INT_VAR2";
        public const string TAMOC_NET_VAR2 = "TAMOC_NET_VAR2";
        public const string TAMAOH_NET_VAR2 = "TAMAOH_NET_VAR2";
        public const string TAMAOH_NET2_VAR2 = "TAMAOH_NET2_VAR2";
        public const string TAMCAP_NET_VAR2 = "TAMCAP_NET_VAR2";
        public const string TAMINCENT_VAR2 = "TAMINCENT_VAR2";
        public const string TAMINCENT_NET_VAR2 = "TAMINCENT_NET_VAR2";

        //median benefits
        public const string TRAmount_MED = "TRAmount_MED";
        public const string TRPrice_MED = "TRPrice_MED";
        public const string TRCompositionAmount_MED = "TRCompositionAmount_MED";
        public const string TR_MED = "TR_MED";
        public const string TR_INT_MED = "TR_INT_MED";
        public const string TRINCENT_MED = "TRINCENT_MED";
        public const string TAMR_MED = "TAMR_MED";
        public const string TAMRINCENT_MED = "TAMRINCENT_MED";

        //median costs
        public const string TOCAmount_MED = "TOCAmount_MED";
        public const string TOCPrice_MED = "TOCPrice_MED";
        public const string TOC_MED = "TOC_MED";
        public const string TOC_INT_MED = "TOC_INT_MED";
        public const string TAOHAmount_MED = "TAOHAmount_MED";
        public const string TAOHPrice_MED = "TAOHPrice_MED";
        public const string TAOH_MED = "TAOH_MED";
        public const string TAOH_INT_MED = "TAOH_INT_MED";
        public const string TCAPAmount_MED = "TCAPAmount_MED";
        public const string TCAPPrice_MED = "TCAPPrice_MED";
        public const string TCAP_MED = "TCAP_MED";

        public const string TCAP_INT_MED = "TCAP_INT_MED";
        //incentives
        public const string TINCENT_MED = "TINCENT_MED";
        //amortized costs
        public const string TAMOC_MED = "TAMOC_MED";
        public const string TAMAOH_MED = "TAMAOH_MED";
        public const string TAMCAP_MED = "TAMCAP_MED";
        public const string TAMOC_INT_MED = "TAMOC_INT_MED";
        public const string TAMAOH_INT_MED = "TAMAOH_INT_MED";
        public const string TAMCAP_INT_MED = "TAMCAP_INT_MED";
        public const string TAMOC_NET_MED = "TAMOC_NET_MED";
        public const string TAMAOH_NET_MED = "TAMAOH_NET_MED";
        public const string TAMAOH_NET2_MED = "TAMAOH_NET2_MED";
        public const string TAMCAP_NET_MED = "TAMCAP_NET_MED";
        public const string TAMINCENT_MED = "TAMINCENT_MED";
        public const string TAMINCENT_NET_MED = "TAMINCENT_NET_MED";

        //N benefits
        public const string TRAmount_N = "TRAmount_N1";
        public const string TRPrice_N = "TRPrice_N1";
        public const string TRCompositionAmount_N = "TRCompositionAmount_N1";
        public const string TR_N = "TR_N1";
        public const string TR_INT_N = "TR_INT_N1";
        public const string TRINCENT_N = "TRINCENT_N1";
        public const string TAMR_N = "TAMR_N1";
        public const string TAMRINCENT_N = "TAMRINCENT_N1";

        //N costs
        public const string TOCAmount_N = "TOCAmount_N1";
        public const string TOCPrice_N = "TOCPrice_N1";
        public const string TOC_N = "TOC_N1";
        public const string TOC_INT_N = "TOC_INT_N1";
        public const string TAOHAmount_N = "TAOHAmount_N1";
        public const string TAOHPrice_N = "TAOHPrice_N1";
        public const string TAOH_N = "TAOH_N1";
        public const string TAOH_INT_N = "TAOH_INT_N1";
        public const string TCAPAmount_N = "TCAPAmount_N1";
        public const string TCAPPrice_N = "TCAPPrice_N1";
        public const string TCAP_N = "TCAP_N1";

        public const string TCAP_INT_N = "TCAP_INT_N1";
        //incentives
        public const string TINCENT_N = "TINCENT_N1";
        //amortized costs
        public const string TAMOC_N = "TAMOC_N1";
        public const string TAMAOH_N = "TAMAOH_N1";
        public const string TAMCAP_N = "TAMCAP_N1";
        public const string TAMOC_INT_N = "TAMOC_INT_N1";
        public const string TAMAOH_INT_N = "TAMAOH_INT_N1";
        public const string TAMCAP_INT_N = "TAMCAP_INT_N1";
        public const string TAMOC_NET_N = "TAMOC_NET_N1";
        public const string TAMAOH_NET_N = "TAMAOH_NET_N1";
        public const string TAMAOH_NET2_N = "TAMAOH_NET2_N1";
        public const string TAMCAP_NET_N = "TAMCAP_NET_N1";
        public const string TAMINCENT_N = "TAMINCENT_N1";
        public const string TAMINCENT_NET_N = "TAMINCENT_NET_N1";


        //mean benefits
        public double TotalRAmount_MEAN { get; set; }
        public double TotalRPrice_MEAN { get; set; }
        public string TotalRUnit_MEAN { get; set; }
        public double TotalRCompositionAmount_MEAN { get; set; }
        public double TotalR_MEAN { get; set; }
        public double TotalR_INT_MEAN { get; set; }
        public double TotalRINCENT_MEAN { get; set; }
        public double TotalAMR_MEAN { get; set; }
        public double TotalAMRINCENT_MEAN { get; set; }
        //mean costs
        public double TotalOCPrice_MEAN { get; set; }
        public double TotalOCAmount_MEAN { get; set; }
        public double TotalOC_MEAN { get; set; }
        public double TotalAOHPrice_MEAN { get; set; }
        public double TotalAOHAmount_MEAN { get; set; }
        public double TotalAOH_MEAN { get; set; }
        public double TotalCAPPrice_MEAN { get; set; }
        public double TotalCAPAmount_MEAN { get; set; }
        public double TotalCAP_MEAN { get; set; }
        public double TotalOC_INT_MEAN { get; set; }
        public double TotalAOH_INT_MEAN { get; set; }
        public double TotalCAP_INT_MEAN { get; set; }

        public double TotalINCENT_MEAN { get; set; }

        public double TotalAMOC_MEAN { get; set; }
        public double TotalAMAOH_MEAN { get; set; }
        public double TotalAMCAP_MEAN { get; set; }
        public double TotalAMOC_INT_MEAN { get; set; }
        public double TotalAMAOH_INT_MEAN { get; set; }
        public double TotalAMCAP_INT_MEAN { get; set; }
        public double TotalAMOC_NET_MEAN { get; set; }
        public double TotalAMAOH_NET_MEAN { get; set; }
        public double TotalAMAOH_NET2_MEAN { get; set; }
        public double TotalAMCAP_NET_MEAN { get; set; }
        public double TotalAMINCENT_MEAN { get; set; }
        public double TotalAMINCENT_NET_MEAN { get; set; }
        public double TotalAnnuities_MEAN { get; set; }

        //standard deviation benefits
        public double TotalRAmount_SD { get; set; }
        public double TotalRPrice_SD { get; set; }
        public double TotalRCompositionAmount_SD { get; set; }
        public double TotalR_SD { get; set; }
        public double TotalR_INT_SD { get; set; }
        public double TotalRINCENT_SD { get; set; }
        public double TotalAMR_SD { get; set; }
        public double TotalAMRINCENT_SD { get; set; }
        //standard deviation costs
        public double TotalOCPrice_SD { get; set; }
        public double TotalOCAmount_SD { get; set; }
        public double TotalOC_SD { get; set; }
        public double TotalAOHPrice_SD { get; set; }
        public double TotalAOHAmount_SD { get; set; }
        public double TotalAOH_SD { get; set; }
        public double TotalCAPPrice_SD { get; set; }
        public double TotalCAPAmount_SD { get; set; }
        public double TotalCAP_SD { get; set; }
        public double TotalOC_INT_SD { get; set; }
        public double TotalAOH_INT_SD { get; set; }
        public double TotalCAP_INT_SD { get; set; }

        public double TotalINCENT_SD { get; set; }

        public double TotalAMOC_SD { get; set; }
        public double TotalAMAOH_SD { get; set; }
        public double TotalAMCAP_SD { get; set; }
        public double TotalAMOC_INT_SD { get; set; }
        public double TotalAMAOH_INT_SD { get; set; }
        public double TotalAMCAP_INT_SD { get; set; }
        public double TotalAMOC_NET_SD { get; set; }
        public double TotalAMAOH_NET_SD { get; set; }
        public double TotalAMAOH_NET2_SD { get; set; }
        public double TotalAMCAP_NET_SD { get; set; }
        public double TotalAMINCENT_SD { get; set; }
        public double TotalAMINCENT_NET_SD { get; set; }
        public double TotalAnnuities_SD { get; set; }

        //variance benefits
        public double TotalRAmount_VAR2 { get; set; }
        public double TotalRPrice_VAR2 { get; set; }
        public string TotalRUnit_VAR2 { get; set; }
        public double TotalRCompositionAmount_VAR2 { get; set; }
        public double TotalR_VAR2 { get; set; }
        public double TotalR_INT_VAR2 { get; set; }
        public double TotalRINCENT_VAR2 { get; set; }
        public double TotalAMR_VAR2 { get; set; }
        public double TotalAMRINCENT_VAR2 { get; set; }
        //variance costs
        public double TotalOCPrice_VAR2 { get; set; }
        public double TotalOCAmount_VAR2 { get; set; }
        public double TotalOC_VAR2 { get; set; }
        public double TotalAOHPrice_VAR2 { get; set; }
        public double TotalAOHAmount_VAR2 { get; set; }
        public double TotalAOH_VAR2 { get; set; }
        public double TotalCAPPrice_VAR2 { get; set; }
        public double TotalCAPAmount_VAR2 { get; set; }
        public double TotalCAP_VAR2 { get; set; }
        public double TotalOC_INT_VAR2 { get; set; }
        public double TotalAOH_INT_VAR2 { get; set; }
        public double TotalCAP_INT_VAR2 { get; set; }

        public double TotalINCENT_VAR2 { get; set; }

        public double TotalAMOC_VAR2 { get; set; }
        public double TotalAMAOH_VAR2 { get; set; }
        public double TotalAMCAP_VAR2 { get; set; }
        public double TotalAMOC_INT_VAR2 { get; set; }
        public double TotalAMAOH_INT_VAR2 { get; set; }
        public double TotalAMCAP_INT_VAR2 { get; set; }
        public double TotalAMOC_NET_VAR2 { get; set; }
        public double TotalAMAOH_NET_VAR2 { get; set; }
        public double TotalAMAOH_NET2_VAR2 { get; set; }
        public double TotalAMCAP_NET_VAR2 { get; set; }
        public double TotalAMINCENT_VAR2 { get; set; }
        public double TotalAMINCENT_NET_VAR2 { get; set; }
        public double TotalAnnuities_VAR2 { get; set; }

        //median benefits
        public double TotalRAmount_MED { get; set; }
        public double TotalRPrice_MED { get; set; }
        public string TotalRUnit_MED { get; set; }
        public double TotalRCompositionAmount_MED { get; set; }
        public double TotalR_MED { get; set; }
        public double TotalR_INT_MED { get; set; }
        public double TotalRINCENT_MED { get; set; }
        public double TotalAMR_MED { get; set; }
        public double TotalAMRINCENT_MED { get; set; }
        //median costs
        public double TotalOCPrice_MED { get; set; }
        public double TotalOCAmount_MED { get; set; }
        public double TotalOC_MED { get; set; }
        public double TotalAOHPrice_MED { get; set; }
        public double TotalAOHAmount_MED { get; set; }
        public double TotalAOH_MED { get; set; }
        public double TotalCAPPrice_MED { get; set; }
        public double TotalCAPAmount_MED { get; set; }
        public double TotalCAP_MED { get; set; }
        public double TotalOC_INT_MED { get; set; }
        public double TotalAOH_INT_MED { get; set; }
        public double TotalCAP_INT_MED { get; set; }

        public double TotalINCENT_MED { get; set; }

        public double TotalAMOC_MED { get; set; }
        public double TotalAMAOH_MED { get; set; }
        public double TotalAMCAP_MED { get; set; }
        public double TotalAMOC_INT_MED { get; set; }
        public double TotalAMAOH_INT_MED { get; set; }
        public double TotalAMCAP_INT_MED { get; set; }
        public double TotalAMOC_NET_MED { get; set; }
        public double TotalAMAOH_NET_MED { get; set; }
        public double TotalAMAOH_NET2_MED { get; set; }
        public double TotalAMCAP_NET_MED { get; set; }
        public double TotalAMINCENT_MED { get; set; }
        public double TotalAMINCENT_NET_MED { get; set; }
        public double TotalAnnuities_MED { get; set; }

        //N benefits
        public double TotalRAmount_N { get; set; }
        public double TotalRPrice_N { get; set; }
        public string TotalRUnit_N { get; set; }
        public double TotalRCompositionAmount_N { get; set; }
        public double TotalR_N { get; set; }
        public double TotalR_INT_N { get; set; }
        public double TotalRINCENT_N { get; set; }
        public double TotalAMR_N { get; set; }
        public double TotalAMRINCENT_N { get; set; }
        //N costs
        public double TotalOCPrice_N { get; set; }
        public double TotalOCAmount_N { get; set; }
        public double TotalOC_N { get; set; }
        public double TotalAOHPrice_N { get; set; }
        public double TotalAOHAmount_N { get; set; }
        public double TotalAOH_N { get; set; }
        public double TotalCAPPrice_N { get; set; }
        public double TotalCAPAmount_N { get; set; }
        public double TotalCAP_N { get; set; }
        public double TotalOC_INT_N { get; set; }
        public double TotalAOH_INT_N { get; set; }
        public double TotalCAP_INT_N { get; set; }

        public double TotalINCENT_N { get; set; }

        public double TotalAMOC_N { get; set; }
        public double TotalAMAOH_N { get; set; }
        public double TotalAMCAP_N { get; set; }
        public double TotalAMOC_INT_N { get; set; }
        public double TotalAMAOH_INT_N { get; set; }
        public double TotalAMCAP_INT_N { get; set; }
        public double TotalAMOC_NET_N { get; set; }
        public double TotalAMAOH_NET_N { get; set; }
        public double TotalAMAOH_NET2_N { get; set; }
        public double TotalAMCAP_NET_N { get; set; }
        public double TotalAMINCENT_N { get; set; }
        public double TotalAMINCENT_NET_N { get; set; }
        public double TotalAnnuities_N { get; set; }

        public double TotalAMTOTAL_MEAN { get; set; }
        public double TotalAMTOTAL_MED { get; set; }
        public double TotalAMTOTAL_VAR2 { get; set; }
        public double TotalAMTOTAL_SD { get; set; }
        public const string TAMTOTAL_MEAN = "TAMTOTAL_MEAN";
        public const string TAMTOTAL_MED = "TAMTOTAL_MED";
        public const string TAMTOTAL_VAR2 = "TAMTOTAL_VAR2";
        public const string TAMTOTAL_SD = "TAMTOTAL_SD";

        public double TotalAMNET_MEAN { get; set; }
        public double TotalAMNET_MED { get; set; }
        public double TotalAMNET_VAR2 { get; set; }
        public double TotalAMNET_SD { get; set; }
        public const string TAMNET_MEAN = "TAMNET_MEAN";
        public const string TAMNET_MED = "TAMNET_MED";
        public const string TAMNET_VAR2 = "TAMNET_VAR2";
        public const string TAMNET_SD = "TAMNET_SD";
        public virtual bool NameIsNotAStatistic(string attName)
        {
            bool bIsNotAStatistic = true;
            if (attName.Contains(MEAN)
                || attName.Contains(SD)
                || attName.Contains(VAR2)
                || attName.Contains(MED))
            {
                bIsNotAStatistic = false;
            }
            else if (attName.Contains(N))
            {
                if (!attName.Contains(NET))
                {
                    bIsNotAStatistic = false;
                }
            }
            return bIsNotAStatistic;
        }
        public void InitStatisticGeneralProperties()
        {
            //avoid null references
            this.TotalObservations = 0;
            this.FilesComparisonsCount = 0;
            this.NodeCount = 0;
            this.OldId = 0;
        }
        //copy method
        public virtual void CopyStatisticGeneralProperties(
            CostBenefitStatistic01 baseStat)
        {
            //copy the base properties
            this.CopyCalculatorProperties(baseStat);
            //copy this object's properties
            this.TotalObservations = baseStat.TotalObservations;
            this.FilesComparisonsCount = baseStat.FilesComparisonsCount;
            this.NodeCount = baseStat.NodeCount;
            this.OldId = baseStat.OldId;
        }
        //no attribute indexing
        public virtual void SetStatisticGeneralProperties(string attName, string attValue)
        {
            switch (attName)
            {
                case OBSERVATIONS:
                    this.TotalObservations 
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case FILES:
                    this.FilesComparisonsCount
                       = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case NODES:
                    this.NodeCount
                       = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case Constants.OLDID:
                    this.OldId
                       = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual void SetStatisticGeneralProperties(XElement statElement)
        {
            this.TotalObservations = CalculatorHelpers.GetAttributeDouble(statElement,
                OBSERVATIONS);
            this.FilesComparisonsCount = CalculatorHelpers.GetAttributeInt(statElement,
                FILES);
            this.NodeCount = CalculatorHelpers.GetAttributeInt(statElement,
                 NODES);
            this.OldId = CalculatorHelpers.GetAttributeInt(statElement,
                 Constants.OLDID);
        }
        //attribute indexing
        public virtual void SetStatisticGeneralAttributes(string attNameExtension,
            ref XElement statElement)
        {
            //copy this object's properties
            CalculatorHelpers.SetAttributeDoubleF3(statElement,
               string.Concat(OBSERVATIONS, attNameExtension),
               this.TotalObservations);
            CalculatorHelpers.SetAttributeInt(statElement,
               string.Concat(FILES, attNameExtension),
               this.FilesComparisonsCount);
            CalculatorHelpers.SetAttributeInt(statElement,
               string.Concat(NODES, attNameExtension),
               this.NodeCount);
            CalculatorHelpers.SetAttributeInt(statElement,
               string.Concat(Constants.OLDID, attNameExtension),
               this.OldId);
        }
        public virtual void SetStatisticGeneralAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(OBSERVATIONS, attNameExtension),
               this.TotalObservations.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(FILES, attNameExtension),
               this.FilesComparisonsCount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(NODES, attNameExtension),
                this.NodeCount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(Constants.OLDID, attNameExtension),
                this.NodeCount.ToString("N2", CultureInfo.InvariantCulture));
        }

        public virtual void CopyMeanCostsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalOCAmount_MEAN = calculator.TotalOCAmount_MEAN;
            this.TotalOCPrice_MEAN = calculator.TotalOCPrice_MEAN;
            this.TotalOC_MEAN = calculator.TotalOC_MEAN;
            this.TotalOC_INT_MEAN = calculator.TotalOC_INT_MEAN;
            this.TotalAOHAmount_MEAN = calculator.TotalAOHAmount_MEAN;
            this.TotalAOHPrice_MEAN = calculator.TotalAOHPrice_MEAN;
            this.TotalAOH_MEAN = calculator.TotalAOH_MEAN;
            this.TotalAOH_INT_MEAN = calculator.TotalAOH_INT_MEAN;
            this.TotalCAPAmount_MEAN = calculator.TotalCAPAmount_MEAN;
            this.TotalCAPPrice_MEAN = calculator.TotalCAPPrice_MEAN;
            this.TotalCAP_MEAN = calculator.TotalCAP_MEAN;
            this.TotalCAP_INT_MEAN = calculator.TotalCAP_INT_MEAN;

            this.TotalINCENT_MEAN = calculator.TotalINCENT_MEAN;

            this.TotalAMOC_MEAN = calculator.TotalAMOC_MEAN;
            this.TotalAMOC_INT_MEAN = calculator.TotalAMOC_INT_MEAN;
            this.TotalAMOC_NET_MEAN = calculator.TotalAMOC_NET_MEAN;
            this.TotalAMAOH_MEAN = calculator.TotalAMAOH_MEAN;
            this.TotalAMAOH_INT_MEAN = calculator.TotalAMAOH_INT_MEAN;
            this.TotalAMAOH_NET_MEAN = calculator.TotalAMAOH_NET_MEAN;
            this.TotalAMAOH_NET2_MEAN = calculator.TotalAMAOH_NET2_MEAN;
            this.TotalAMCAP_MEAN = calculator.TotalAMCAP_MEAN;
            this.TotalAMCAP_INT_MEAN = calculator.TotalAMCAP_INT_MEAN;
            this.TotalAMCAP_NET_MEAN = calculator.TotalAMCAP_NET_MEAN;
            this.TotalAMINCENT_MEAN = calculator.TotalAMINCENT_MEAN;
            this.TotalAMINCENT_NET_MEAN = calculator.TotalAMINCENT_NET_MEAN;
            this.TotalAnnuities_MEAN = calculator.TotalAnnuities_MEAN;
            this.TotalAMTOTAL_MEAN = calculator.TotalAMTOTAL_MEAN;
            this.TotalAMNET_MEAN = calculator.TotalAMNET_MEAN;
        }
        public virtual void InitMeanCostsProperties()
        {
            //init totals to zero prior to running calculations
            this.TotalOCAmount_MEAN = 0;
            this.TotalOCPrice_MEAN = 0;
            this.TotalOC_MEAN = 0;
            this.TotalOC_INT_MEAN = 0;
            this.TotalAOHAmount_MEAN = 0;
            this.TotalAOHPrice_MEAN = 0;
            this.TotalAOH_MEAN = 0;
            this.TotalAOH_INT_MEAN = 0;
            this.TotalCAPAmount_MEAN = 0;
            this.TotalCAPPrice_MEAN = 0;
            this.TotalCAP_MEAN = 0;
            this.TotalCAP_INT_MEAN = 0;
            this.TotalINCENT_MEAN = 0;

            this.TotalAMOC_MEAN = 0;
            this.TotalAMOC_INT_MEAN = 0;
            this.TotalAMOC_NET_MEAN = 0;
            this.TotalAMAOH_MEAN = 0;
            this.TotalAMAOH_INT_MEAN = 0;
            this.TotalAMAOH_NET_MEAN = 0;
            this.TotalAMAOH_NET2_MEAN = 0;
            this.TotalAMCAP_MEAN = 0;
            this.TotalAMCAP_INT_MEAN = 0;
            this.TotalAMCAP_NET_MEAN = 0;
            this.TotalAMINCENT_MEAN = 0;
            this.TotalAMINCENT_NET_MEAN = 0;
            this.TotalAMTOTAL_MEAN = 0;
            this.TotalAMNET_MEAN = 0;
        }
        public virtual void SetMeanCostsProperties(XElement calculator)
        {
            this.TotalOCAmount_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCAmount_MEAN);
            this.TotalOCPrice_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCPrice_MEAN);
            this.TotalOC_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_MEAN);
            this.TotalOC_INT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_INT_MEAN);
            this.TotalAOHAmount_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHAmount_MEAN);
            this.TotalAOHPrice_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHPrice_MEAN);
            this.TotalAOH_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_MEAN);
            this.TotalAOH_INT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_INT_MEAN);
            this.TotalCAPAmount_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPAmount_MEAN);
            this.TotalCAPPrice_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPPrice_MEAN);
            this.TotalCAP_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                 TCAP_MEAN);
            this.TotalCAP_INT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAP_INT_MEAN);

            this.TotalINCENT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TINCENT_MEAN);

            this.TotalAMOC_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_MEAN);
            this.TotalAMOC_INT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_INT_MEAN);
            this.TotalAMOC_NET_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_NET_MEAN);
            this.TotalAMAOH_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_MEAN);
            this.TotalAMAOH_INT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_INT_MEAN);
            this.TotalAMAOH_NET_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET_MEAN);
            this.TotalAMAOH_NET2_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET2_MEAN);
            this.TotalAMCAP_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_MEAN);
            this.TotalAMCAP_INT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_INT_MEAN);
            this.TotalAMCAP_NET_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_NET_MEAN);
            this.TotalAMINCENT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_MEAN);
            this.TotalAMINCENT_NET_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_NET_MEAN);
            this.TotalAMTOTAL_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMTOTAL_MEAN);
            this.TotalAMNET_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMNET_MEAN);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetMeanCostsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TOCAmount_MEAN:
                    this.TotalOCAmount_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOCPrice_MEAN:
                    this.TotalOCPrice_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_MEAN:
                    this.TotalOC_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_INT_MEAN:
                    this.TotalOC_INT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHAmount_MEAN:
                    this.TotalAOHAmount_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHPrice_MEAN:
                    this.TotalAOHPrice_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_MEAN:
                    this.TotalAOH_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_INT_MEAN:
                    this.TotalAOH_INT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPAmount_MEAN:
                    this.TotalCAPAmount_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPPrice_MEAN:
                    this.TotalCAPPrice_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_MEAN:
                    this.TotalCAP_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_INT_MEAN:
                    this.TotalCAP_INT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TINCENT_MEAN:
                    this.TotalINCENT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TAMOC_MEAN:
                    this.TotalAMOC_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_INT_MEAN:
                    this.TotalAMOC_INT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_NET_MEAN:
                    this.TotalAMOC_NET_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_MEAN:
                    this.TotalAMAOH_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_INT_MEAN:
                    this.TotalAMAOH_INT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET_MEAN:
                    this.TotalAMAOH_NET_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET2_MEAN:
                    this.TotalAMAOH_NET2_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_MEAN:
                    this.TotalAMCAP_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_INT_MEAN:
                    this.TotalAMCAP_INT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_NET_MEAN:
                    this.TotalAMCAP_NET_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_MEAN:
                    this.TotalAMINCENT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_NET_MEAN:
                    this.TotalAMINCENT_NET_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMTOTAL:
                    this.TotalAMTOTAL = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNET_MEAN:
                    this.TotalAMNET_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetMeanCostsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TOCAmount_MEAN:
                    sPropertyValue = this.TotalOCAmount_MEAN.ToString();
                    break;
                case TOCPrice_MEAN:
                    sPropertyValue = this.TotalOCPrice_MEAN.ToString();
                    break;
                case TOC_MEAN:
                    sPropertyValue = this.TotalOC_MEAN.ToString();
                    break;
                case TOC_INT_MEAN:
                    sPropertyValue = this.TotalOC_INT_MEAN.ToString();
                    break;
                case TAOHAmount_MEAN:
                    sPropertyValue = this.TotalAOHAmount_MEAN.ToString();
                    break;
                case TAOHPrice_MEAN:
                    sPropertyValue = this.TotalAOHPrice_MEAN.ToString();
                    break;
                case TAOH_MEAN:
                    sPropertyValue = this.TotalAOH_MEAN.ToString();
                    break;
                case TAOH_INT_MEAN:
                    sPropertyValue = this.TotalAOH_INT_MEAN.ToString();
                    break;
                case TCAPAmount_MEAN:
                    sPropertyValue = this.TotalCAPAmount_MEAN.ToString();
                    break;
                case TCAPPrice_MEAN:
                    sPropertyValue = this.TotalCAPPrice_MEAN.ToString();
                    break;
                case TCAP_MEAN:
                    sPropertyValue = this.TotalCAP_MEAN.ToString();
                    break;
                case TCAP_INT_MEAN:
                    sPropertyValue = this.TotalCAP_INT_MEAN.ToString();
                    break;

                case TINCENT_MEAN:
                    sPropertyValue = this.TotalINCENT_MEAN.ToString();
                    break;

                case TAMOC_MEAN:
                    sPropertyValue = this.TotalAMOC_MEAN.ToString();
                    break;
                case TAMOC_INT_MEAN:
                    sPropertyValue = this.TotalAMOC_INT_MEAN.ToString();
                    break;
                case TAMOC_NET_MEAN:
                    sPropertyValue = this.TotalAMOC_NET_MEAN.ToString();
                    break;
                case TAMAOH_MEAN:
                    sPropertyValue = this.TotalAMAOH_MEAN.ToString();
                    break;
                case TAMAOH_INT_MEAN:
                    sPropertyValue = this.TotalAMAOH_INT_MEAN.ToString();
                    break;
                case TAMAOH_NET_MEAN:
                    sPropertyValue = this.TotalAMAOH_NET_MEAN.ToString();
                    break;
                case TAMAOH_NET2_MEAN:
                    sPropertyValue = this.TotalAMAOH_NET2_MEAN.ToString();
                    break;
                case TAMCAP_MEAN:
                    sPropertyValue = this.TotalAMCAP_MEAN.ToString();
                    break;
                case TAMCAP_INT_MEAN:
                    sPropertyValue = this.TotalAMCAP_INT_MEAN.ToString();
                    break;
                case TAMCAP_NET_MEAN:
                    sPropertyValue = this.TotalAMCAP_NET_MEAN.ToString();
                    break;
                case TAMINCENT_MEAN:
                    sPropertyValue = this.TotalAMINCENT_MEAN.ToString();
                    break;
                case TAMINCENT_NET_MEAN:
                    sPropertyValue = this.TotalAMINCENT_NET_MEAN.ToString();
                    break;
                case TAMTOTAL_MEAN:
                    sPropertyValue = this.TotalAMTOTAL_MEAN.ToString();
                    break;
                case TAMNET_MEAN:
                    sPropertyValue = this.TotalAMNET_MEAN.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetMeanCostsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //not absolutely necessary to set the type of the attribute
            //the double was properly set when the property was set
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCAmount_MEAN, attNameExtension),
                this.TotalOCAmount_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCPrice_MEAN, attNameExtension),
                this.TotalOCPrice_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_MEAN, attNameExtension),
                this.TotalOC_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_INT_MEAN, attNameExtension),
                this.TotalOC_INT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHAmount_MEAN, attNameExtension),
                this.TotalAOHAmount_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHPrice_MEAN, attNameExtension),
                this.TotalAOHPrice_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_MEAN, attNameExtension),
                this.TotalAOH_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_INT_MEAN, attNameExtension),
                this.TotalAOH_INT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPAmount_MEAN, attNameExtension),
                this.TotalCAPAmount_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPPrice_MEAN, attNameExtension),
                this.TotalCAPPrice_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_MEAN, attNameExtension),
                this.TotalCAP_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_INT_MEAN, attNameExtension),
                this.TotalCAP_INT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TINCENT_MEAN, attNameExtension),
                this.TotalINCENT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_MEAN, attNameExtension),
                this.TotalAMOC_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_INT_MEAN, attNameExtension),
                this.TotalAMOC_INT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_NET_MEAN, attNameExtension),
                this.TotalAMOC_NET_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_MEAN, attNameExtension),
                this.TotalAMAOH_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_INT_MEAN, attNameExtension),
                this.TotalAMAOH_INT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET_MEAN, attNameExtension),
                this.TotalAMAOH_NET_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET2_MEAN, attNameExtension),
                this.TotalAMAOH_NET2_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_MEAN, attNameExtension),
                this.TotalAMCAP_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_INT_MEAN, attNameExtension),
                this.TotalAMCAP_INT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_NET_MEAN, attNameExtension),
                this.TotalAMCAP_NET_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_MEAN, attNameExtension),
                this.TotalAMINCENT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_NET_MEAN, attNameExtension),
                this.TotalAMINCENT_NET_MEAN);

        }
        public virtual void SetMeanCostsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TOCAmount_MEAN, attNameExtension),
                this.TotalOCAmount_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOCPrice_MEAN, attNameExtension),
                this.TotalOCPrice_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_MEAN, attNameExtension),
                this.TotalOC_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_INT_MEAN, attNameExtension),
                this.TotalOC_INT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHAmount_MEAN, attNameExtension),
                this.TotalAOHAmount_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHPrice_MEAN, attNameExtension),
                this.TotalAOHPrice_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_MEAN, attNameExtension),
                this.TotalAOH_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_INT_MEAN, attNameExtension),
                this.TotalAOH_INT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPAmount_MEAN, attNameExtension),
                this.TotalCAPAmount_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPPrice_MEAN, attNameExtension),
                this.TotalCAPPrice_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_MEAN, attNameExtension),
                this.TotalCAP_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_INT_MEAN, attNameExtension),
                this.TotalCAP_INT_MEAN.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TINCENT_MEAN, attNameExtension),
                this.TotalINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMOC_MEAN, attNameExtension),
                this.TotalAMOC_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_INT_MEAN, attNameExtension),
                this.TotalAMOC_INT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_NET_MEAN, attNameExtension),
                this.TotalAMOC_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_MEAN, attNameExtension),
                this.TotalAMAOH_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_INT_MEAN, attNameExtension),
                this.TotalAMAOH_INT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET_MEAN, attNameExtension),
                this.TotalAMAOH_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET2_MEAN, attNameExtension),
                this.TotalAMAOH_NET2_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_MEAN, attNameExtension),
                this.TotalAMCAP_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_INT_MEAN, attNameExtension),
                this.TotalAMCAP_INT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_NET_MEAN, attNameExtension),
                this.TotalAMCAP_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_MEAN, attNameExtension),
                this.TotalAMINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET_MEAN, attNameExtension),
                this.TotalAMINCENT_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void CopyMeanBenefitsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalRPrice_MEAN = calculator.TotalRPrice_MEAN;
            this.TotalRAmount_MEAN = calculator.TotalRAmount_MEAN;
            this.TotalRCompositionAmount_MEAN = calculator.TotalRCompositionAmount_MEAN;
            this.TotalR_MEAN = calculator.TotalR_MEAN;
            this.TotalR_INT_MEAN = calculator.TotalR_INT_MEAN;
            this.TotalRINCENT_MEAN = calculator.TotalRINCENT_MEAN;
            this.TotalAMR_MEAN = calculator.TotalAMR_MEAN;
            this.TotalAMRINCENT_MEAN = calculator.TotalAMRINCENT_MEAN;
        }
        public virtual void InitMeanBenefitsProperties()
        {
            this.TotalRUnit_MEAN = string.Empty;
            this.TotalRPrice_MEAN = 0;
            this.TotalRAmount_MEAN = 0;
            this.TotalRCompositionAmount_MEAN = 0;
            this.TotalR_MEAN = 0;
            this.TotalR_INT_MEAN = 0;
            this.TotalRINCENT_MEAN = 0;
            this.TotalAMR_MEAN = 0;
            this.TotalAMRINCENT_MEAN = 0;
            this.TotalAnnuities_MEAN = 0;
        }
        public virtual void SetMeanBenefitsProperties(XElement calculator)
        {
            this.TotalRAmount_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TRAmount_MEAN);
            this.TotalRPrice_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TRPrice_MEAN);
            this.TotalRCompositionAmount_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TRCompositionAmount_MEAN);
            this.TotalR_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_MEAN);
            this.TotalR_INT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_INT_MEAN);
            this.TotalRINCENT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TRINCENT_MEAN);
            this.TotalAMR_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR_MEAN);
            this.TotalAMRINCENT_MEAN = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMRINCENT_MEAN);
        }
        public virtual void SetMeanBenefitsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TRAmount_MEAN:
                    this.TotalRAmount_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice_MEAN:
                    this.TotalRPrice_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRCompositionAmount_MEAN:
                    this.TotalRCompositionAmount_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_MEAN:
                    this.TotalR_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_INT_MEAN:
                    this.TotalR_INT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRINCENT_MEAN:
                    this.TotalRINCENT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR_MEAN:
                    this.TotalAMR_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT_MEAN:
                    this.TotalAMRINCENT_MEAN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetMeanBenefitsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TRAmount_MEAN:
                    sPropertyValue = this.TotalRAmount_MEAN.ToString();
                    break;
                case TRPrice_MEAN:
                    sPropertyValue = this.TotalRPrice_MEAN.ToString();
                    break;
                case TRCompositionAmount_MEAN:
                    sPropertyValue = this.TotalRCompositionAmount_MEAN.ToString();
                    break;
                case TR_MEAN:
                    sPropertyValue = this.TotalR_MEAN.ToString();
                    break;
                case TR_INT_MEAN:
                    sPropertyValue = this.TotalR_INT_MEAN.ToString();
                    break;
                case TRINCENT_MEAN:
                    sPropertyValue = this.TotalRINCENT_MEAN.ToString();
                    break;
                case TAMR_MEAN:
                    sPropertyValue = this.TotalAMR_MEAN.ToString();
                    break;
                case TAMRINCENT_MEAN:
                    sPropertyValue = this.TotalAMRINCENT_MEAN.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetMeanBenefitsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRAmount_MEAN, attNameExtension),
                this.TotalRAmount_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRPrice_MEAN, attNameExtension),
                this.TotalRPrice_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRCompositionAmount_MEAN, attNameExtension),
                this.TotalRCompositionAmount_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_MEAN, attNameExtension),
                this.TotalR_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_INT_MEAN, attNameExtension),
                this.TotalR_INT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRINCENT_MEAN, attNameExtension),
                this.TotalRINCENT_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMR_MEAN, attNameExtension),
                this.TotalAMR_MEAN);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMRINCENT_MEAN, attNameExtension),
                this.TotalAMRINCENT_MEAN);
        }
        public virtual void SetMeanBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRAmount_MEAN, attNameExtension),
                this.TotalRAmount_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRPrice_MEAN, attNameExtension),
                this.TotalRPrice_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRCompositionAmount_MEAN, attNameExtension),
                this.TotalRCompositionAmount_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_MEAN, attNameExtension),
                this.TotalR_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_INT_MEAN, attNameExtension),
                this.TotalR_INT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRINCENT_MEAN, attNameExtension),
                this.TotalRINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR_MEAN, attNameExtension),
                this.TotalAMR_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_MEAN, attNameExtension),
                this.TotalAMRINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
        }

        //standard deviation
        public virtual void CopyStdDevCostsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalOCAmount_SD = calculator.TotalOCAmount_SD;
            this.TotalOCPrice_SD = calculator.TotalOCPrice_SD;
            this.TotalOC_SD = calculator.TotalOC_SD;
            this.TotalOC_INT_SD = calculator.TotalOC_INT_SD;
            this.TotalAOHAmount_SD = calculator.TotalAOHAmount_SD;
            this.TotalAOHPrice_SD = calculator.TotalAOHPrice_SD;
            this.TotalAOH_SD = calculator.TotalAOH_SD;
            this.TotalAOH_INT_SD = calculator.TotalAOH_INT_SD;
            this.TotalCAPAmount_SD = calculator.TotalCAPAmount_SD;
            this.TotalCAPPrice_SD = calculator.TotalCAPPrice_SD;
            this.TotalCAP_SD = calculator.TotalCAP_SD;
            this.TotalCAP_INT_SD = calculator.TotalCAP_INT_SD;

            this.TotalINCENT_SD = calculator.TotalINCENT_SD;

            this.TotalAMOC_SD = calculator.TotalAMOC_SD;
            this.TotalAMOC_INT_SD = calculator.TotalAMOC_INT_SD;
            this.TotalAMOC_NET_SD = calculator.TotalAMOC_NET_SD;
            this.TotalAMAOH_SD = calculator.TotalAMAOH_SD;
            this.TotalAMAOH_INT_SD = calculator.TotalAMAOH_INT_SD;
            this.TotalAMAOH_NET_SD = calculator.TotalAMAOH_NET_SD;
            this.TotalAMAOH_NET2_SD = calculator.TotalAMAOH_NET2_SD;
            this.TotalAMCAP_SD = calculator.TotalAMCAP_SD;
            this.TotalAMCAP_INT_SD = calculator.TotalAMCAP_INT_SD;
            this.TotalAMCAP_NET_SD = calculator.TotalAMCAP_NET_SD;
            this.TotalAMINCENT_SD = calculator.TotalAMINCENT_SD;
            this.TotalAMINCENT_NET_SD = calculator.TotalAMINCENT_NET_SD;
            this.TotalAnnuities_SD = calculator.TotalAnnuities_SD;
            this.TotalAMTOTAL_SD = calculator.TotalAMTOTAL_SD;
            this.TotalAMNET_SD = calculator.TotalAMNET_SD;
        }
        public virtual void InitStdDevCostsProperties()
        {
            //init totals to zero prior to running calculations
            this.TotalOCAmount_SD = 0;
            this.TotalOCPrice_SD = 0;
            this.TotalOC_SD = 0;
            this.TotalOC_INT_SD = 0;
            this.TotalAOHAmount_SD = 0;
            this.TotalAOHPrice_SD = 0;
            this.TotalAOH_SD = 0;
            this.TotalAOH_INT_SD = 0;
            this.TotalCAPAmount_SD = 0;
            this.TotalCAPPrice_SD = 0;
            this.TotalCAP_SD = 0;
            this.TotalCAP_INT_SD = 0;
            this.TotalINCENT_SD = 0;

            this.TotalAMOC_SD = 0;
            this.TotalAMOC_INT_SD = 0;
            this.TotalAMOC_NET_SD = 0;
            this.TotalAMAOH_SD = 0;
            this.TotalAMAOH_INT_SD = 0;
            this.TotalAMAOH_NET_SD = 0;
            this.TotalAMAOH_NET2_SD = 0;
            this.TotalAMCAP_SD = 0;
            this.TotalAMCAP_INT_SD = 0;
            this.TotalAMCAP_NET_SD = 0;
            this.TotalAMINCENT_SD = 0;
            this.TotalAMINCENT_NET_SD = 0;
            this.TotalAMTOTAL_SD = 0;
            this.TotalAMNET_SD = 0;
        }
        public virtual void SetStdDevCostsProperties(XElement calculator)
        {
            this.TotalOCAmount_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCAmount_SD);
            this.TotalOCPrice_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCPrice_SD);
            this.TotalOC_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_SD);
            this.TotalOC_INT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_INT_SD);
            this.TotalAOHAmount_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHAmount_SD);
            this.TotalAOHPrice_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHPrice_SD);
            this.TotalAOH_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_SD);
            this.TotalAOH_INT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_INT_SD);
            this.TotalCAPAmount_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPAmount_SD);
            this.TotalCAPPrice_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPPrice_SD);
            this.TotalCAP_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                 TCAP_SD);
            this.TotalCAP_INT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAP_INT_SD);

            this.TotalINCENT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TINCENT_SD);

            this.TotalAMOC_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_SD);
            this.TotalAMOC_INT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_INT_SD);
            this.TotalAMOC_NET_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_NET_SD);
            this.TotalAMAOH_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_SD);
            this.TotalAMAOH_INT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_INT_SD);
            this.TotalAMAOH_NET_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET_SD);
            this.TotalAMAOH_NET2_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET2_SD);
            this.TotalAMCAP_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_SD);
            this.TotalAMCAP_INT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_INT_SD);
            this.TotalAMCAP_NET_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_NET_SD);
            this.TotalAMINCENT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_SD);
            this.TotalAMINCENT_NET_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_NET_SD);
            this.TotalAMTOTAL_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMTOTAL_SD);
            this.TotalAMNET_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMNET_SD);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetStdDevCostsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TOCAmount_SD:
                    this.TotalOCAmount_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOCPrice_SD:
                    this.TotalOCPrice_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_SD:
                    this.TotalOC_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_INT_SD:
                    this.TotalOC_INT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHAmount_SD:
                    this.TotalAOHAmount_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHPrice_SD:
                    this.TotalAOHPrice_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_SD:
                    this.TotalAOH_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_INT_SD:
                    this.TotalAOH_INT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPAmount_SD:
                    this.TotalCAPAmount_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPPrice_SD:
                    this.TotalCAPPrice_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_SD:
                    this.TotalCAP_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_INT_SD:
                    this.TotalCAP_INT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TINCENT_SD:
                    this.TotalINCENT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TAMOC_SD:
                    this.TotalAMOC_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_INT_SD:
                    this.TotalAMOC_INT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_NET_SD:
                    this.TotalAMOC_NET_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_SD:
                    this.TotalAMAOH_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_INT_SD:
                    this.TotalAMAOH_INT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET_SD:
                    this.TotalAMAOH_NET_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET2_SD:
                    this.TotalAMAOH_NET2_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_SD:
                    this.TotalAMCAP_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_INT_SD:
                    this.TotalAMCAP_INT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_NET_SD:
                    this.TotalAMCAP_NET_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_SD:
                    this.TotalAMINCENT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_NET_SD:
                    this.TotalAMINCENT_NET_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMTOTAL_SD:
                    this.TotalAMTOTAL_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNET_SD:
                    this.TotalAMNET_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetStdDevCostsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TOCAmount_SD:
                    sPropertyValue = this.TotalOCAmount_SD.ToString();
                    break;
                case TOCPrice_SD:
                    sPropertyValue = this.TotalOCPrice_SD.ToString();
                    break;
                case TOC_SD:
                    sPropertyValue = this.TotalOC_SD.ToString();
                    break;
                case TOC_INT_SD:
                    sPropertyValue = this.TotalOC_INT_SD.ToString();
                    break;
                case TAOHAmount_SD:
                    sPropertyValue = this.TotalAOHAmount_SD.ToString();
                    break;
                case TAOHPrice_SD:
                    sPropertyValue = this.TotalAOHPrice_SD.ToString();
                    break;
                case TAOH_SD:
                    sPropertyValue = this.TotalAOH_SD.ToString();
                    break;
                case TAOH_INT_SD:
                    sPropertyValue = this.TotalAOH_INT_SD.ToString();
                    break;
                case TCAPAmount_SD:
                    sPropertyValue = this.TotalCAPAmount_SD.ToString();
                    break;
                case TCAPPrice_SD:
                    sPropertyValue = this.TotalCAPPrice_SD.ToString();
                    break;
                case TCAP_SD:
                    sPropertyValue = this.TotalCAP_SD.ToString();
                    break;
                case TCAP_INT_SD:
                    sPropertyValue = this.TotalCAP_INT_SD.ToString();
                    break;

                case TINCENT_SD:
                    sPropertyValue = this.TotalINCENT_SD.ToString();
                    break;

                case TAMOC_SD:
                    sPropertyValue = this.TotalAMOC_SD.ToString();
                    break;
                case TAMOC_INT_SD:
                    sPropertyValue = this.TotalAMOC_INT_SD.ToString();
                    break;
                case TAMOC_NET_SD:
                    sPropertyValue = this.TotalAMOC_NET_SD.ToString();
                    break;
                case TAMAOH_SD:
                    sPropertyValue = this.TotalAMAOH_SD.ToString();
                    break;
                case TAMAOH_INT_SD:
                    sPropertyValue = this.TotalAMAOH_INT_SD.ToString();
                    break;
                case TAMAOH_NET_SD:
                    sPropertyValue = this.TotalAMAOH_NET_SD.ToString();
                    break;
                case TAMAOH_NET2_SD:
                    sPropertyValue = this.TotalAMAOH_NET2_SD.ToString();
                    break;
                case TAMCAP_SD:
                    sPropertyValue = this.TotalAMCAP_SD.ToString();
                    break;
                case TAMCAP_INT_SD:
                    sPropertyValue = this.TotalAMCAP_INT_SD.ToString();
                    break;
                case TAMCAP_NET_SD:
                    sPropertyValue = this.TotalAMCAP_NET_SD.ToString();
                    break;
                case TAMINCENT_SD:
                    sPropertyValue = this.TotalAMINCENT_SD.ToString();
                    break;
                case TAMINCENT_NET_SD:
                    sPropertyValue = this.TotalAMINCENT_NET_SD.ToString();
                    break;
                case TAMTOTAL_SD:
                    sPropertyValue = this.TotalAMTOTAL_SD.ToString();
                    break;
                case TAMNET_SD:
                    sPropertyValue = this.TotalAMNET_SD.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetStdDevCostsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //not absolutely necessary to set the type of the attribute
            //the double was properly set when the property was set
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCAmount_SD, attNameExtension),
                this.TotalOCAmount_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCPrice_SD, attNameExtension),
                this.TotalOCPrice_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_SD, attNameExtension),
                this.TotalOC_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_INT_SD, attNameExtension),
                this.TotalOC_INT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHAmount_SD, attNameExtension),
                this.TotalAOHAmount_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHPrice_SD, attNameExtension),
                this.TotalAOHPrice_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_SD, attNameExtension),
                this.TotalAOH_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_INT_SD, attNameExtension),
                this.TotalAOH_INT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPAmount_SD, attNameExtension),
                this.TotalCAPAmount_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPPrice_SD, attNameExtension),
                this.TotalCAPPrice_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_SD, attNameExtension),
                this.TotalCAP_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_INT_SD, attNameExtension),
                this.TotalCAP_INT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TINCENT_SD, attNameExtension),
                this.TotalINCENT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_SD, attNameExtension),
                this.TotalAMOC_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_INT_SD, attNameExtension),
                this.TotalAMOC_INT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_NET_SD, attNameExtension),
                this.TotalAMOC_NET_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_SD, attNameExtension),
                this.TotalAMAOH_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_INT_SD, attNameExtension),
                this.TotalAMAOH_INT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET_SD, attNameExtension),
                this.TotalAMAOH_NET_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET2_SD, attNameExtension),
                this.TotalAMAOH_NET2_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_SD, attNameExtension),
                this.TotalAMCAP_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_INT_SD, attNameExtension),
                this.TotalAMCAP_INT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_NET_SD, attNameExtension),
                this.TotalAMCAP_NET_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_NET_SD, attNameExtension),
                this.TotalAMINCENT_NET_SD);
        }
        public virtual void SetStdDevCostsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TOCAmount_SD, attNameExtension),
                this.TotalOCAmount_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOCPrice_SD, attNameExtension),
                this.TotalOCPrice_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_SD, attNameExtension),
                this.TotalOC_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_INT_SD, attNameExtension),
                this.TotalOC_INT_SD.ToString("N2", CultureInfo.InvariantCulture));
             writer.WriteAttributeString(
                string.Concat(TAOHAmount_SD, attNameExtension),
                this.TotalAOHAmount_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHPrice_SD, attNameExtension),
                this.TotalAOHPrice_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_SD, attNameExtension),
                this.TotalAOH_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_INT_SD, attNameExtension),
                this.TotalAOH_INT_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPAmount_SD, attNameExtension),
                this.TotalCAPAmount_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPPrice_SD, attNameExtension),
                this.TotalCAPPrice_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_SD, attNameExtension),
                this.TotalCAP_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_INT_SD, attNameExtension),
                this.TotalCAP_INT_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TINCENT_SD, attNameExtension),
                this.TotalINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMOC_SD, attNameExtension),
                this.TotalAMOC_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_INT_SD, attNameExtension),
                this.TotalAMOC_INT_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_NET_SD, attNameExtension),
                this.TotalAMOC_NET_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_SD, attNameExtension),
                this.TotalAMAOH_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_INT_SD, attNameExtension),
                this.TotalAMAOH_INT_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET_SD, attNameExtension),
                this.TotalAMAOH_NET_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET2_SD, attNameExtension),
                this.TotalAMAOH_NET2_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_SD, attNameExtension),
                this.TotalAMCAP_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_INT_SD, attNameExtension),
                this.TotalAMCAP_INT_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_NET_SD, attNameExtension),
                this.TotalAMCAP_NET_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_SD, attNameExtension),
                this.TotalAMINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET_SD, attNameExtension),
                this.TotalAMINCENT_NET_SD.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void CopyStdDevBenefitsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalRPrice_SD = calculator.TotalRPrice_SD;
            this.TotalRAmount_SD = calculator.TotalRAmount_SD;
            this.TotalRCompositionAmount_SD = calculator.TotalRCompositionAmount_SD;
            this.TotalR_SD = calculator.TotalR_SD;
            this.TotalR_INT_SD = calculator.TotalR_INT_SD;
            this.TotalRINCENT_SD = calculator.TotalRINCENT_SD;
            this.TotalAMR_SD = calculator.TotalAMR_SD;
            this.TotalAMRINCENT_SD = calculator.TotalAMRINCENT_SD;
        }
        public virtual void InitStdDevBenefitsProperties()
        {
            this.TotalRPrice_SD = 0;
            this.TotalRAmount_SD = 0;
            this.TotalRCompositionAmount_SD = 0;
            this.TotalR_SD = 0;
            this.TotalR_INT_SD = 0;
            this.TotalRINCENT_SD = 0;
            this.TotalAMR_SD = 0;
            this.TotalAMRINCENT_SD = 0;
            this.TotalAnnuities_SD = 0;
        }
        public virtual void SetStdDevBenefitsProperties(XElement calculator)
        {
            this.TotalRAmount_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TRAmount);
            this.TotalRPrice_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TRPrice);
            this.TotalRCompositionAmount_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TRCompositionAmount);
            this.TotalR_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TR);
            this.TotalR_INT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_INT);
            this.TotalRINCENT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TRINCENT);
            this.TotalAMR_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR);
            this.TotalAMRINCENT_SD = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMRINCENT);
        }
        public virtual void SetStdDevBenefitsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TRAmount_SD:
                    this.TotalRAmount_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice_SD:
                    this.TotalRPrice_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRCompositionAmount_SD:
                    this.TotalRCompositionAmount_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_SD:
                    this.TotalR_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_INT_SD:
                    this.TotalR_INT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRINCENT_SD:
                    this.TotalRINCENT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR_SD:
                    this.TotalAMR_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT_SD:
                    this.TotalAMRINCENT_SD = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetStdDevBenefitsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TRAmount_SD:
                    sPropertyValue = this.TotalRAmount_SD.ToString();
                    break;
                case TRPrice_SD:
                    sPropertyValue = this.TotalRPrice_SD.ToString();
                    break;
                case TRCompositionAmount_SD:
                    sPropertyValue = this.TotalRCompositionAmount_SD.ToString();
                    break;
                case TR_SD:
                    sPropertyValue = this.TotalR_SD.ToString();
                    break;
                case TR_INT_SD:
                    sPropertyValue = this.TotalR_INT_SD.ToString();
                    break;
                case TRINCENT_SD:
                    sPropertyValue = this.TotalRINCENT_SD.ToString();
                    break;
                case TAMR_SD:
                    sPropertyValue = this.TotalAMR_SD.ToString();
                    break;
                case TAMRINCENT_SD:
                    sPropertyValue = this.TotalAMRINCENT_SD.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetStdDevBenefitsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRAmount_SD, attNameExtension),
                this.TotalRAmount_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRPrice_SD, attNameExtension),
                this.TotalRPrice_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRCompositionAmount_SD, attNameExtension),
                this.TotalRCompositionAmount_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_SD, attNameExtension),
                this.TotalR_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_INT_SD, attNameExtension),
                this.TotalR_INT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRINCENT_SD, attNameExtension),
                this.TotalRINCENT_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMR_SD, attNameExtension),
                this.TotalAMR_SD);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMRINCENT_SD, attNameExtension),
                this.TotalAMRINCENT_SD);
        }
        public virtual void SetStdDevBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRAmount_SD, attNameExtension),
                this.TotalRAmount_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRPrice_SD, attNameExtension),
                this.TotalRPrice_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRCompositionAmount_SD, attNameExtension),
                this.TotalRCompositionAmount_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_SD, attNameExtension),
                this.TotalR_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_INT_SD, attNameExtension),
                this.TotalR_INT_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRINCENT_SD, attNameExtension),
                this.TotalRINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR_SD, attNameExtension),
                this.TotalAMR_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_SD, attNameExtension),
                this.TotalAMRINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));
        }

        //variance
        public virtual void CopyVarianceCostsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalOCAmount_VAR2 = calculator.TotalOCAmount_VAR2;
            this.TotalOCPrice_VAR2 = calculator.TotalOCPrice_VAR2;
            this.TotalOC_VAR2 = calculator.TotalOC_VAR2;
            this.TotalOC_INT_VAR2 = calculator.TotalOC_INT_VAR2;
            this.TotalAOHAmount_VAR2 = calculator.TotalAOHAmount_VAR2;
            this.TotalAOHPrice_VAR2 = calculator.TotalAOHPrice_VAR2;
            this.TotalAOH_VAR2 = calculator.TotalAOH_VAR2;
            this.TotalAOH_INT_VAR2 = calculator.TotalAOH_INT_VAR2;
            this.TotalCAPAmount_VAR2 = calculator.TotalCAPAmount_VAR2;
            this.TotalCAPPrice_VAR2 = calculator.TotalCAPPrice_VAR2;
            this.TotalCAP_VAR2 = calculator.TotalCAP_VAR2;
            this.TotalCAP_INT_VAR2 = calculator.TotalCAP_INT_VAR2;

            this.TotalINCENT_VAR2 = calculator.TotalINCENT_VAR2;

            this.TotalAMOC_VAR2 = calculator.TotalAMOC_VAR2;
            this.TotalAMOC_INT_VAR2 = calculator.TotalAMOC_INT_VAR2;
            this.TotalAMOC_NET_VAR2 = calculator.TotalAMOC_NET_VAR2;
            this.TotalAMAOH_VAR2 = calculator.TotalAMAOH_VAR2;
            this.TotalAMAOH_INT_VAR2 = calculator.TotalAMAOH_INT_VAR2;
            this.TotalAMAOH_NET_VAR2 = calculator.TotalAMAOH_NET_VAR2;
            this.TotalAMAOH_NET2_VAR2 = calculator.TotalAMAOH_NET2_VAR2;
            this.TotalAMCAP_VAR2 = calculator.TotalAMCAP_VAR2;
            this.TotalAMCAP_INT_VAR2 = calculator.TotalAMCAP_INT_VAR2;
            this.TotalAMCAP_NET_VAR2 = calculator.TotalAMCAP_NET_VAR2;
            this.TotalAMINCENT_VAR2 = calculator.TotalAMINCENT_VAR2;
            this.TotalAMINCENT_NET_VAR2 = calculator.TotalAMINCENT_NET_VAR2;
            this.TotalAnnuities_VAR2 = calculator.TotalAnnuities_VAR2;
            this.TotalAMTOTAL_VAR2 = calculator.TotalAMTOTAL_VAR2;
            this.TotalAMNET_VAR2 = calculator.TotalAMNET_VAR2;
        }
        public virtual void InitVarianceCostsProperties()
        {
            //init totals to zero prior to running calculations
            this.TotalOCAmount_VAR2 = 0;
            this.TotalOCPrice_VAR2 = 0;
            this.TotalOC_VAR2 = 0;
            this.TotalOC_INT_VAR2 = 0;
            this.TotalAOHAmount_VAR2 = 0;
            this.TotalAOHPrice_VAR2 = 0;
            this.TotalAOH_VAR2 = 0;
            this.TotalAOH_INT_VAR2 = 0;
            this.TotalCAPAmount_VAR2 = 0;
            this.TotalCAPPrice_VAR2 = 0;
            this.TotalCAP_VAR2 = 0;
            this.TotalCAP_INT_VAR2 = 0;
            this.TotalINCENT_VAR2 = 0;

            this.TotalAMOC_VAR2 = 0;
            this.TotalAMOC_INT_VAR2 = 0;
            this.TotalAMOC_NET_VAR2 = 0;
            this.TotalAMAOH_VAR2 = 0;
            this.TotalAMAOH_INT_VAR2 = 0;
            this.TotalAMAOH_NET_VAR2 = 0;
            this.TotalAMAOH_NET2_VAR2 = 0;
            this.TotalAMCAP_VAR2 = 0;
            this.TotalAMCAP_INT_VAR2 = 0;
            this.TotalAMCAP_NET_VAR2 = 0;
            this.TotalAMINCENT_VAR2 = 0;
            this.TotalAMINCENT_NET_VAR2 = 0;
            this.TotalAMTOTAL_VAR2 = 0;
            this.TotalAMNET_VAR2 = 0;
        }
        public virtual void SetVarianceCostsProperties(XElement calculator)
        {
            this.TotalOCAmount_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCAmount_VAR2);
            this.TotalOCPrice_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCPrice_VAR2);
            this.TotalOC_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_VAR2);
            this.TotalOC_INT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_INT_VAR2);
            this.TotalAOHAmount_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHAmount_VAR2);
            this.TotalAOHPrice_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHPrice_VAR2);
            this.TotalAOH_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_VAR2);
            this.TotalAOH_INT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_INT_VAR2);
            this.TotalCAPAmount_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPAmount_VAR2);
            this.TotalCAPPrice_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPPrice_VAR2);
            this.TotalCAP_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                 TCAP_VAR2);
            this.TotalCAP_INT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAP_INT_VAR2);

            this.TotalINCENT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TINCENT_VAR2);

            this.TotalAMOC_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_VAR2);
            this.TotalAMOC_INT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_INT_VAR2);
            this.TotalAMOC_NET_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_NET_VAR2);
            this.TotalAMAOH_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_VAR2);
            this.TotalAMAOH_INT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_INT_VAR2);
            this.TotalAMAOH_NET_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET_VAR2);
            this.TotalAMAOH_NET2_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET2_VAR2);
            this.TotalAMCAP_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_VAR2);
            this.TotalAMCAP_INT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_INT_VAR2);
            this.TotalAMCAP_NET_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_NET_VAR2);
            this.TotalAMINCENT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_VAR2);
            this.TotalAMINCENT_NET_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_NET_VAR2);
            this.TotalAMTOTAL_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMTOTAL_VAR2);
            this.TotalAMNET_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMNET_VAR2);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetVarianceCostsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TOCAmount_VAR2:
                    this.TotalOCAmount_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOCPrice_VAR2:
                    this.TotalOCPrice_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_VAR2:
                    this.TotalOC_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_INT_VAR2:
                    this.TotalOC_INT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHAmount_VAR2:
                    this.TotalAOHAmount_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHPrice_VAR2:
                    this.TotalAOHPrice_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_VAR2:
                    this.TotalAOH_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_INT_VAR2:
                    this.TotalAOH_INT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPAmount_VAR2:
                    this.TotalCAPAmount_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPPrice_VAR2:
                    this.TotalCAPPrice_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_VAR2:
                    this.TotalCAP_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_INT_VAR2:
                    this.TotalCAP_INT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TINCENT_VAR2:
                    this.TotalINCENT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TAMOC_VAR2:
                    this.TotalAMOC_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_INT_VAR2:
                    this.TotalAMOC_INT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_NET_VAR2:
                    this.TotalAMOC_NET_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_VAR2:
                    this.TotalAMAOH_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_INT_VAR2:
                    this.TotalAMAOH_INT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET_VAR2:
                    this.TotalAMAOH_NET_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET2_VAR2:
                    this.TotalAMAOH_NET2_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_VAR2:
                    this.TotalAMCAP_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_INT_VAR2:
                    this.TotalAMCAP_INT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_NET_VAR2:
                    this.TotalAMCAP_NET_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_VAR2:
                    this.TotalAMINCENT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_NET_VAR2:
                    this.TotalAMINCENT_NET_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMTOTAL_VAR2:
                    this.TotalAMTOTAL_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNET_VAR2:
                    this.TotalAMNET_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetVarianceCostsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TOCAmount_VAR2:
                    sPropertyValue = this.TotalOCAmount_VAR2.ToString();
                    break;
                case TOCPrice_VAR2:
                    sPropertyValue = this.TotalOCPrice_VAR2.ToString();
                    break;
                case TOC_VAR2:
                    sPropertyValue = this.TotalOC_VAR2.ToString();
                    break;
                case TOC_INT_VAR2:
                    sPropertyValue = this.TotalOC_INT_VAR2.ToString();
                    break;
                case TAOHAmount_VAR2:
                    sPropertyValue = this.TotalAOHAmount_VAR2.ToString();
                    break;
                case TAOHPrice_VAR2:
                    sPropertyValue = this.TotalAOHPrice_VAR2.ToString();
                    break;
                case TAOH_VAR2:
                    sPropertyValue = this.TotalAOH_VAR2.ToString();
                    break;
                case TAOH_INT_VAR2:
                    sPropertyValue = this.TotalAOH_INT_VAR2.ToString();
                    break;
                case TCAPAmount_VAR2:
                    sPropertyValue = this.TotalCAPAmount_VAR2.ToString();
                    break;
                case TCAPPrice_VAR2:
                    sPropertyValue = this.TotalCAPPrice_VAR2.ToString();
                    break;
                case TCAP_VAR2:
                    sPropertyValue = this.TotalCAP_VAR2.ToString();
                    break;
                case TCAP_INT_VAR2:
                    sPropertyValue = this.TotalCAP_INT_VAR2.ToString();
                    break;

                case TINCENT_VAR2:
                    sPropertyValue = this.TotalINCENT_VAR2.ToString();
                    break;

                case TAMOC_VAR2:
                    sPropertyValue = this.TotalAMOC_VAR2.ToString();
                    break;
                case TAMOC_INT_VAR2:
                    sPropertyValue = this.TotalAMOC_INT_VAR2.ToString();
                    break;
                case TAMOC_NET_VAR2:
                    sPropertyValue = this.TotalAMOC_NET_VAR2.ToString();
                    break;
                case TAMAOH_VAR2:
                    sPropertyValue = this.TotalAMAOH_VAR2.ToString();
                    break;
                case TAMAOH_INT_VAR2:
                    sPropertyValue = this.TotalAMAOH_INT_VAR2.ToString();
                    break;
                case TAMAOH_NET_VAR2:
                    sPropertyValue = this.TotalAMAOH_NET_VAR2.ToString();
                    break;
                case TAMAOH_NET2_VAR2:
                    sPropertyValue = this.TotalAMAOH_NET2_VAR2.ToString();
                    break;
                case TAMCAP_VAR2:
                    sPropertyValue = this.TotalAMCAP_VAR2.ToString();
                    break;
                case TAMCAP_INT_VAR2:
                    sPropertyValue = this.TotalAMCAP_INT_VAR2.ToString();
                    break;
                case TAMCAP_NET_VAR2:
                    sPropertyValue = this.TotalAMCAP_NET_VAR2.ToString();
                    break;
                case TAMINCENT_VAR2:
                    sPropertyValue = this.TotalAMINCENT_VAR2.ToString();
                    break;
                case TAMINCENT_NET_VAR2:
                    sPropertyValue = this.TotalAMINCENT_NET_VAR2.ToString();
                    break;
                case TAMTOTAL_VAR2:
                    sPropertyValue = this.TotalAMTOTAL_VAR2.ToString();
                    break;
                case TAMNET_VAR2:
                    sPropertyValue = this.TotalAMNET_VAR2.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetVarianceCostsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //not absolutely necessary to set the type of the attribute
            //the double was properly set when the property was set
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCAmount_VAR2, attNameExtension),
                this.TotalOCAmount_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCPrice_VAR2, attNameExtension),
                this.TotalOCPrice_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_VAR2, attNameExtension),
                this.TotalOC_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_INT_VAR2, attNameExtension),
                this.TotalOC_INT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHAmount_VAR2, attNameExtension),
                this.TotalAOHAmount_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHPrice_VAR2, attNameExtension),
                this.TotalAOHPrice_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_VAR2, attNameExtension),
                this.TotalAOH_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_INT_VAR2, attNameExtension),
                this.TotalAOH_INT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPAmount_VAR2, attNameExtension),
                this.TotalCAPAmount_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPPrice_VAR2, attNameExtension),
                this.TotalCAPPrice_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_VAR2, attNameExtension),
                this.TotalCAP_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_INT_VAR2, attNameExtension),
                this.TotalCAP_INT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TINCENT_VAR2, attNameExtension),
                this.TotalINCENT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_VAR2, attNameExtension),
                this.TotalAMOC_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_INT_VAR2, attNameExtension),
                this.TotalAMOC_INT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_NET_VAR2, attNameExtension),
                this.TotalAMOC_NET_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_VAR2, attNameExtension),
                this.TotalAMAOH_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_INT_VAR2, attNameExtension),
                this.TotalAMAOH_INT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET_VAR2, attNameExtension),
                this.TotalAMAOH_NET_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET2_VAR2, attNameExtension),
                this.TotalAMAOH_NET2_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_VAR2, attNameExtension),
                this.TotalAMCAP_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_INT_VAR2, attNameExtension),
                this.TotalAMCAP_INT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_NET_VAR2, attNameExtension),
                this.TotalAMCAP_NET_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_VAR2, attNameExtension),
                this.TotalAMINCENT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_NET_VAR2, attNameExtension),
                this.TotalAMINCENT_NET_VAR2);
        }
        public virtual void SetVarianceCostsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TOCAmount_VAR2, attNameExtension),
                this.TotalOCAmount_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOCPrice_VAR2, attNameExtension),
                this.TotalOCPrice_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_VAR2, attNameExtension),
                this.TotalOC_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_INT_VAR2, attNameExtension),
                this.TotalOC_INT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHAmount_VAR2, attNameExtension),
                this.TotalAOHAmount_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHPrice_VAR2, attNameExtension),
                this.TotalAOHPrice_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_VAR2, attNameExtension),
                this.TotalAOH_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_INT_VAR2, attNameExtension),
                this.TotalAOH_INT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPAmount_VAR2, attNameExtension),
                this.TotalCAPAmount_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPPrice_VAR2, attNameExtension),
                this.TotalCAPPrice_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_VAR2, attNameExtension),
                this.TotalCAP_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_INT_VAR2, attNameExtension),
                this.TotalCAP_INT_VAR2.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TINCENT_VAR2, attNameExtension),
                this.TotalINCENT_VAR2.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMOC_VAR2, attNameExtension),
                this.TotalAMOC_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_INT_VAR2, attNameExtension),
                this.TotalAMOC_INT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_NET_VAR2, attNameExtension),
                this.TotalAMOC_NET_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_VAR2, attNameExtension),
                this.TotalAMAOH_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_INT_VAR2, attNameExtension),
                this.TotalAMAOH_INT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET_VAR2, attNameExtension),
                this.TotalAMAOH_NET_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET2_VAR2, attNameExtension),
                this.TotalAMAOH_NET2_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_VAR2, attNameExtension),
                this.TotalAMCAP_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_INT_VAR2, attNameExtension),
                this.TotalAMCAP_INT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_NET_VAR2, attNameExtension),
                this.TotalAMCAP_NET_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_VAR2, attNameExtension),
                this.TotalAMINCENT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET_VAR2, attNameExtension),
                this.TotalAMINCENT_NET_VAR2.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void CopyVarianceBenefitsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalRPrice_VAR2 = calculator.TotalRPrice_VAR2;
            this.TotalRAmount_VAR2 = calculator.TotalRAmount_VAR2;
            this.TotalRCompositionAmount_VAR2 = calculator.TotalRCompositionAmount_VAR2;
            this.TotalR_VAR2 = calculator.TotalR_VAR2;
            this.TotalR_INT_VAR2 = calculator.TotalR_INT_VAR2;
            this.TotalRINCENT_VAR2 = calculator.TotalRINCENT_VAR2;
            this.TotalAMR_VAR2 = calculator.TotalAMR_VAR2;
            this.TotalAMRINCENT_VAR2 = calculator.TotalAMRINCENT_VAR2;
        }
        public virtual void InitVarianceBenefitsProperties()
        {
            this.TotalRUnit_VAR2 = string.Empty;
            this.TotalRPrice_VAR2 = 0;
            this.TotalRAmount_VAR2 = 0;
            this.TotalRCompositionAmount_VAR2 = 0;
            this.TotalR_VAR2 = 0;
            this.TotalR_INT_VAR2 = 0;
            this.TotalRINCENT_VAR2 = 0;
            this.TotalAMR_VAR2 = 0;
            this.TotalAMRINCENT_VAR2 = 0;
            this.TotalAnnuities_VAR2 = 0;
        }
        public virtual void SetVarianceBenefitsProperties(XElement calculator)
        {
            this.TotalRAmount_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TRAmount_VAR2);
            this.TotalRPrice_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TRPrice_VAR2);
            this.TotalRCompositionAmount_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TRCompositionAmount_VAR2);
            this.TotalR_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_VAR2);
            this.TotalR_INT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_INT_VAR2);
            this.TotalRINCENT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TRINCENT_VAR2);
            this.TotalAMR_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR_VAR2);
            this.TotalAMRINCENT_VAR2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMRINCENT_VAR2);
        }
        public virtual void SetVarianceBenefitsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TRAmount_VAR2:
                    this.TotalRAmount_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice_VAR2:
                    this.TotalRPrice_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRCompositionAmount_VAR2:
                    this.TotalRCompositionAmount_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_VAR2:
                    this.TotalR_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_INT_VAR2:
                    this.TotalR_INT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRINCENT_VAR2:
                    this.TotalRINCENT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR_VAR2:
                    this.TotalAMR_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT_VAR2:
                    this.TotalAMRINCENT_VAR2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetVarianceBenefitsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TRAmount_VAR2:
                    sPropertyValue = this.TotalRAmount_VAR2.ToString();
                    break;
                case TRPrice_VAR2:
                    sPropertyValue = this.TotalRPrice_VAR2.ToString();
                    break;
                case TRCompositionAmount_VAR2:
                    sPropertyValue = this.TotalRCompositionAmount_VAR2.ToString();
                    break;
                case TR_VAR2:
                    sPropertyValue = this.TotalR_VAR2.ToString();
                    break;
                case TR_INT_VAR2:
                    sPropertyValue = this.TotalR_INT_VAR2.ToString();
                    break;
                case TRINCENT_VAR2:
                    sPropertyValue = this.TotalRINCENT_VAR2.ToString();
                    break;
                case TAMR_VAR2:
                    sPropertyValue = this.TotalAMR_VAR2.ToString();
                    break;
                case TAMRINCENT_VAR2:
                    sPropertyValue = this.TotalAMRINCENT_VAR2.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetVarianceBenefitsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRAmount_VAR2, attNameExtension),
                this.TotalRAmount_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRPrice_VAR2, attNameExtension),
                this.TotalRPrice_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRCompositionAmount_VAR2, attNameExtension),
                this.TotalRCompositionAmount_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_VAR2, attNameExtension),
                this.TotalR_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_INT_VAR2, attNameExtension),
                this.TotalR_INT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRINCENT_VAR2, attNameExtension),
                this.TotalRINCENT_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMR_VAR2, attNameExtension),
                this.TotalAMR_VAR2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMRINCENT_VAR2, attNameExtension),
                this.TotalAMRINCENT_VAR2);
        }
        public virtual void SetVarianceBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRAmount_VAR2, attNameExtension),
                this.TotalRAmount_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRPrice_VAR2, attNameExtension),
                this.TotalRPrice_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRCompositionAmount_VAR2, attNameExtension),
                this.TotalRCompositionAmount_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_VAR2, attNameExtension),
                this.TotalR_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_INT_VAR2, attNameExtension),
                this.TotalR_INT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRINCENT_VAR2, attNameExtension),
                this.TotalRINCENT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR_VAR2, attNameExtension),
                this.TotalAMR_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_VAR2, attNameExtension),
                this.TotalAMRINCENT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
        }

        public virtual void CopyMedianCostsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalOCAmount_MED = calculator.TotalOCAmount_MED;
            this.TotalOCPrice_MED = calculator.TotalOCPrice_MED;
            this.TotalOC_MED = calculator.TotalOC_MED;
            this.TotalOC_INT_MED = calculator.TotalOC_INT_MED;
            this.TotalAOHAmount_MED = calculator.TotalAOHAmount_MED;
            this.TotalAOHPrice_MED = calculator.TotalAOHPrice_MED;
            this.TotalAOH_MED = calculator.TotalAOH_MED;
            this.TotalAOH_INT_MED = calculator.TotalAOH_INT_MED;
            this.TotalCAPAmount_MED = calculator.TotalCAPAmount_MED;
            this.TotalCAPPrice_MED = calculator.TotalCAPPrice_MED;
            this.TotalCAP_MED = calculator.TotalCAP_MED;
            this.TotalCAP_INT_MED = calculator.TotalCAP_INT_MED;

            this.TotalINCENT_MED = calculator.TotalINCENT_MED;

            this.TotalAMOC_MED = calculator.TotalAMOC_MED;
            this.TotalAMOC_INT_MED = calculator.TotalAMOC_INT_MED;
            this.TotalAMOC_NET_MED = calculator.TotalAMOC_NET_MED;
            this.TotalAMAOH_MED = calculator.TotalAMAOH_MED;
            this.TotalAMAOH_INT_MED = calculator.TotalAMAOH_INT_MED;
            this.TotalAMAOH_NET_MED = calculator.TotalAMAOH_NET_MED;
            this.TotalAMAOH_NET2_MED = calculator.TotalAMAOH_NET2_MED;
            this.TotalAMCAP_MED = calculator.TotalAMCAP_MED;
            this.TotalAMCAP_INT_MED = calculator.TotalAMCAP_INT_MED;
            this.TotalAMCAP_NET_MED = calculator.TotalAMCAP_NET_MED;
            this.TotalAMINCENT_MED = calculator.TotalAMINCENT_MED;
            this.TotalAMINCENT_NET_MED = calculator.TotalAMINCENT_NET_MED;
            this.TotalAnnuities_MED = calculator.TotalAnnuities_MED;
            this.TotalAMTOTAL_MED = calculator.TotalAMTOTAL_MED;
            this.TotalAMNET_MED = calculator.TotalAMNET_MED;
        }
        public virtual void InitMedianCostsProperties()
        {
            //init totals to zero prior to running calculations
            this.TotalOCAmount_MED = 0;
            this.TotalOCPrice_MED = 0;
            this.TotalOC_MED = 0;
            this.TotalOC_INT_MED = 0;
            this.TotalAOHAmount_MED = 0;
            this.TotalAOHPrice_MED = 0;
            this.TotalAOH_MED = 0;
            this.TotalAOH_INT_MED = 0;
            this.TotalCAPAmount_MED = 0;
            this.TotalCAPPrice_MED = 0;
            this.TotalCAP_MED = 0;
            this.TotalCAP_INT_MED = 0;
            this.TotalINCENT_MED = 0;

            this.TotalAMOC_MED = 0;
            this.TotalAMOC_INT_MED = 0;
            this.TotalAMOC_NET_MED = 0;
            this.TotalAMAOH_MED = 0;
            this.TotalAMAOH_INT_MED = 0;
            this.TotalAMAOH_NET_MED = 0;
            this.TotalAMAOH_NET2_MED = 0;
            this.TotalAMCAP_MED = 0;
            this.TotalAMCAP_INT_MED = 0;
            this.TotalAMCAP_NET_MED = 0;
            this.TotalAMINCENT_MED = 0;
            this.TotalAMINCENT_NET_MED = 0;
            this.TotalAMTOTAL_MED = 0;
            this.TotalAMNET_MED = 0;
        }
        public virtual void SetMedianCostsProperties(XElement calculator)
        {
            this.TotalOCAmount_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCAmount_MED);
            this.TotalOCPrice_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCPrice_MED);
            this.TotalOC_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_MED);
            this.TotalOC_INT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_INT_MED);
            this.TotalAOHAmount_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHAmount_MED);
            this.TotalAOHPrice_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHPrice_MED);
            this.TotalAOH_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_MED);
            this.TotalAOH_INT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_INT_MED);
            this.TotalCAPAmount_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPAmount_MED);
            this.TotalCAPPrice_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPPrice_MED);
            this.TotalCAP_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                 TCAP_MED);
            this.TotalCAP_INT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAP_INT_MED);

            this.TotalINCENT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TINCENT_MED);

            this.TotalAMOC_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_MED);
            this.TotalAMOC_INT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_INT_MED);
            this.TotalAMOC_NET_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_NET_MED);
            this.TotalAMAOH_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_MED);
            this.TotalAMAOH_INT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_INT_MED);
            this.TotalAMAOH_NET_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET_MED);
            this.TotalAMAOH_NET2_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET2_MED);
            this.TotalAMCAP_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_MED);
            this.TotalAMCAP_INT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_INT_MED);
            this.TotalAMCAP_NET_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_NET_MED);
            this.TotalAMINCENT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_MED);
            this.TotalAMINCENT_NET_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_NET_MED);
            this.TotalAMTOTAL_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMTOTAL_MED);
            this.TotalAMNET_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMNET_MED);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetMedianCostsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TOCAmount_MED:
                    this.TotalOCAmount_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOCPrice_MED:
                    this.TotalOCPrice_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_MED:
                    this.TotalOC_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_INT_MED:
                    this.TotalOC_INT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHAmount_MED:
                    this.TotalAOHAmount_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHPrice_MED:
                    this.TotalAOHPrice_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_MED:
                    this.TotalAOH_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_INT_MED:
                    this.TotalAOH_INT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPAmount_MED:
                    this.TotalCAPAmount_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPPrice_MED:
                    this.TotalCAPPrice_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_MED:
                    this.TotalCAP_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_INT_MED:
                    this.TotalCAP_INT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TINCENT_MED:
                    this.TotalINCENT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TAMOC_MED:
                    this.TotalAMOC_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_INT_MED:
                    this.TotalAMOC_INT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_NET_MED:
                    this.TotalAMOC_NET_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_MED:
                    this.TotalAMAOH_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_INT_MED:
                    this.TotalAMAOH_INT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET_MED:
                    this.TotalAMAOH_NET_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET2_MED:
                    this.TotalAMAOH_NET2_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_MED:
                    this.TotalAMCAP_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_INT_MED:
                    this.TotalAMCAP_INT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_NET_MED:
                    this.TotalAMCAP_NET_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_MED:
                    this.TotalAMINCENT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_NET_MED:
                    this.TotalAMINCENT_NET_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMTOTAL_MED:
                    this.TotalAMTOTAL_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNET_MED:
                    this.TotalAMNET_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetMedianCostsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TOCAmount_MED:
                    sPropertyValue = this.TotalOCAmount_MED.ToString();
                    break;
                case TOCPrice_MED:
                    sPropertyValue = this.TotalOCPrice_MED.ToString();
                    break;
                case TOC_MED:
                    sPropertyValue = this.TotalOC_MED.ToString();
                    break;
                case TOC_INT_MED:
                    sPropertyValue = this.TotalOC_INT_MED.ToString();
                    break;
                case TAOHAmount_MED:
                    sPropertyValue = this.TotalAOHAmount_MED.ToString();
                    break;
                case TAOHPrice_MED:
                    sPropertyValue = this.TotalAOHPrice_MED.ToString();
                    break;
                case TAOH_MED:
                    sPropertyValue = this.TotalAOH_MED.ToString();
                    break;
                case TAOH_INT_MED:
                    sPropertyValue = this.TotalAOH_INT_MED.ToString();
                    break;
                case TCAPAmount_MED:
                    sPropertyValue = this.TotalCAPAmount_MED.ToString();
                    break;
                case TCAPPrice_MED:
                    sPropertyValue = this.TotalCAPPrice_MED.ToString();
                    break;
                case TCAP_MED:
                    sPropertyValue = this.TotalCAP_MED.ToString();
                    break;
                case TCAP_INT_MED:
                    sPropertyValue = this.TotalCAP_INT_MED.ToString();
                    break;

                case TINCENT_MED:
                    sPropertyValue = this.TotalINCENT_MED.ToString();
                    break;

                case TAMOC_MED:
                    sPropertyValue = this.TotalAMOC_MED.ToString();
                    break;
                case TAMOC_INT_MED:
                    sPropertyValue = this.TotalAMOC_INT_MED.ToString();
                    break;
                case TAMOC_NET_MED:
                    sPropertyValue = this.TotalAMOC_NET_MED.ToString();
                    break;
                case TAMAOH_MED:
                    sPropertyValue = this.TotalAMAOH_MED.ToString();
                    break;
                case TAMAOH_INT_MED:
                    sPropertyValue = this.TotalAMAOH_INT_MED.ToString();
                    break;
                case TAMAOH_NET_MED:
                    sPropertyValue = this.TotalAMAOH_NET_MED.ToString();
                    break;
                case TAMAOH_NET2_MED:
                    sPropertyValue = this.TotalAMAOH_NET2_MED.ToString();
                    break;
                case TAMCAP_MED:
                    sPropertyValue = this.TotalAMCAP_MED.ToString();
                    break;
                case TAMCAP_INT_MED:
                    sPropertyValue = this.TotalAMCAP_INT_MED.ToString();
                    break;
                case TAMCAP_NET_MED:
                    sPropertyValue = this.TotalAMCAP_NET_MED.ToString();
                    break;
                case TAMINCENT_MED:
                    sPropertyValue = this.TotalAMINCENT_MED.ToString();
                    break;
                case TAMINCENT_NET_MED:
                    sPropertyValue = this.TotalAMINCENT_NET_MED.ToString();
                    break;
                case TAMTOTAL_MED:
                    sPropertyValue = this.TotalAMTOTAL_MED.ToString();
                    break;
                case TAMNET_MED:
                    sPropertyValue = this.TotalAMNET_MED.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetMedianCostsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //not absolutely necessary to set the type of the attribute
            //the double was properly set when the property was set
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCAmount_MED, attNameExtension),
                this.TotalOCAmount_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCPrice_MED, attNameExtension),
                this.TotalOCPrice_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_MED, attNameExtension),
                this.TotalOC_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_INT_MED, attNameExtension),
                this.TotalOC_INT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHAmount_MED, attNameExtension),
                this.TotalAOHAmount_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHPrice_MED, attNameExtension),
                this.TotalAOHPrice_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_MED, attNameExtension),
                this.TotalAOH_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_INT_MED, attNameExtension),
                this.TotalAOH_INT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPAmount_MED, attNameExtension),
                this.TotalCAPAmount_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPPrice_MED, attNameExtension),
                this.TotalCAPPrice_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_MED, attNameExtension),
                this.TotalCAP_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_INT_MED, attNameExtension),
                this.TotalCAP_INT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TINCENT_MED, attNameExtension),
                this.TotalINCENT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_MED, attNameExtension),
                this.TotalAMOC_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_INT_MED, attNameExtension),
                this.TotalAMOC_INT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_NET_MED, attNameExtension),
                this.TotalAMOC_NET_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_MED, attNameExtension),
                this.TotalAMAOH_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_INT_MED, attNameExtension),
                this.TotalAMAOH_INT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET_MED, attNameExtension),
                this.TotalAMAOH_NET_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET2_MED, attNameExtension),
                this.TotalAMAOH_NET2_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_MED, attNameExtension),
                this.TotalAMCAP_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_INT_MED, attNameExtension),
                this.TotalAMCAP_INT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_NET_MED, attNameExtension),
                this.TotalAMCAP_NET_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_MED, attNameExtension),
                this.TotalAMINCENT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_NET_MED, attNameExtension),
                this.TotalAMINCENT_NET_MED);
        }
        public virtual void SetMedianCostsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TOCAmount_MED, attNameExtension),
                this.TotalOCAmount_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOCPrice_MED, attNameExtension),
                this.TotalOCPrice_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_MED, attNameExtension),
                this.TotalOC_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_INT_MED, attNameExtension),
                this.TotalOC_INT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHAmount_MED, attNameExtension),
                this.TotalAOHAmount_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHPrice_MED, attNameExtension),
                this.TotalAOHPrice_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_MED, attNameExtension),
                this.TotalAOH_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_INT_MED, attNameExtension),
                this.TotalAOH_INT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPAmount_MED, attNameExtension),
                this.TotalCAPAmount_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPPrice_MED, attNameExtension),
                this.TotalCAPPrice_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_MED, attNameExtension),
                this.TotalCAP_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_INT_MED, attNameExtension),
                this.TotalCAP_INT_MED.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TINCENT_MED, attNameExtension),
                this.TotalINCENT_MED.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMOC_MED, attNameExtension),
                this.TotalAMOC_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_INT_MED, attNameExtension),
                this.TotalAMOC_INT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_NET_MED, attNameExtension),
                this.TotalAMOC_NET_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_MED, attNameExtension),
                this.TotalAMAOH_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_INT_MED, attNameExtension),
                this.TotalAMAOH_INT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET_MED, attNameExtension),
                this.TotalAMAOH_NET_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET2_MED, attNameExtension),
                this.TotalAMAOH_NET2_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_MED, attNameExtension),
                this.TotalAMCAP_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_INT_MED, attNameExtension),
                this.TotalAMCAP_INT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_NET_MED, attNameExtension),
                this.TotalAMCAP_NET_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_MED, attNameExtension),
                this.TotalAMINCENT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET_MED, attNameExtension),
                this.TotalAMINCENT_NET_MED.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void CopyMedianBenefitsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalRPrice_MED = calculator.TotalRPrice_MED;
            this.TotalRAmount_MED = calculator.TotalRAmount_MED;
            this.TotalRCompositionAmount_MED = calculator.TotalRCompositionAmount_MED;
            this.TotalR_MED = calculator.TotalR_MED;
            this.TotalR_INT_MED = calculator.TotalR_INT_MED;
            this.TotalRINCENT_MED = calculator.TotalRINCENT_MED;
            this.TotalAMR_MED = calculator.TotalAMR_MED;
            this.TotalAMRINCENT_MED = calculator.TotalAMRINCENT_MED;
        }
        public virtual void InitMedianBenefitsProperties()
        {
            this.TotalRUnit_MED = string.Empty;
            this.TotalRPrice_MED = 0;
            this.TotalRAmount_MED = 0;
            this.TotalRCompositionAmount_MED = 0;
            this.TotalR_MED = 0;
            this.TotalR_INT_MED = 0;
            this.TotalRINCENT_MED = 0;
            this.TotalAMR_MED = 0;
            this.TotalAMRINCENT_MED = 0;
            this.TotalAnnuities_MED = 0;
        }
        public virtual void SetMedianBenefitsProperties(XElement calculator)
        {
            this.TotalRAmount_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TRAmount_MED);
            this.TotalRPrice_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TRPrice_MED);
            this.TotalRCompositionAmount_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TRCompositionAmount_MED);
            this.TotalR_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_MED);
            this.TotalR_INT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_INT_MED);
            this.TotalRINCENT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TRINCENT_MED);
            this.TotalAMR_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR_MED);
            this.TotalAMRINCENT_MED = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMRINCENT_MED);
        }
        public virtual void SetMedianBenefitsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TRAmount_MED:
                    this.TotalRAmount_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice_MED:
                    this.TotalRPrice_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRCompositionAmount_MED:
                    this.TotalRCompositionAmount_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_MED:
                    this.TotalR_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_INT_MED:
                    this.TotalR_INT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRINCENT_MED:
                    this.TotalRINCENT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR_MED:
                    this.TotalAMR_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT_MED:
                    this.TotalAMRINCENT_MED = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetMedianBenefitsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TRAmount_MED:
                    sPropertyValue = this.TotalRAmount_MED.ToString();
                    break;
                case TRPrice_MED:
                    sPropertyValue = this.TotalRPrice_MED.ToString();
                    break;
                case TRCompositionAmount_MED:
                    sPropertyValue = this.TotalRCompositionAmount_MED.ToString();
                    break;
                case TR_MED:
                    sPropertyValue = this.TotalR_MED.ToString();
                    break;
                case TR_INT_MED:
                    sPropertyValue = this.TotalR_INT_MED.ToString();
                    break;
                case TRINCENT_MED:
                    sPropertyValue = this.TotalRINCENT_MED.ToString();
                    break;
                case TAMR_MED:
                    sPropertyValue = this.TotalAMR_MED.ToString();
                    break;
                case TAMRINCENT_MED:
                    sPropertyValue = this.TotalAMRINCENT_MED.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetMedianBenefitsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRAmount_MED, attNameExtension),
                this.TotalRAmount_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRPrice_MED, attNameExtension),
                this.TotalRPrice_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRCompositionAmount_MED, attNameExtension),
                this.TotalRCompositionAmount_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_MED, attNameExtension),
                this.TotalR_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_INT_MED, attNameExtension),
                this.TotalR_INT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRINCENT_MED, attNameExtension),
                this.TotalRINCENT_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMR_MED, attNameExtension),
                this.TotalAMR_MED);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMRINCENT_MED, attNameExtension),
                this.TotalAMRINCENT_MED);
        }
        public virtual void SetMedianBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRAmount_MED, attNameExtension),
                this.TotalRAmount_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRPrice_MED, attNameExtension),
                this.TotalRPrice_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRCompositionAmount_MED, attNameExtension),
                this.TotalRCompositionAmount_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_MED, attNameExtension),
                this.TotalR_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_INT_MED, attNameExtension),
                this.TotalR_INT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRINCENT_MED, attNameExtension),
                this.TotalRINCENT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR_MED, attNameExtension),
                this.TotalAMR_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_MED, attNameExtension),
                this.TotalAMRINCENT_MED.ToString("N2", CultureInfo.InvariantCulture));
        }
        public void SetAmortBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRName, attNameExtension),
               this.TotalRName);
            writer.WriteAttributeString(
                 string.Concat(TRUnit, attNameExtension),
                this.TotalRUnit);

            writer.WriteAttributeString(
                string.Concat(TAMR_N, attNameExtension),
               this.TotalAMR_N.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TRAmount, attNameExtension),
               this.TotalRAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                 string.Concat(TRAmount_MEAN, attNameExtension),
                this.TotalRAmount_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRAmount_SD, attNameExtension),
               this.TotalRAmount_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                 string.Concat(TAMR, attNameExtension),
                this.TotalAMR.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                 string.Concat(TAMR_MEAN, attNameExtension),
                this.TotalAMR_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR_SD, attNameExtension),
               this.TotalAMR_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMRINCENT, attNameExtension),
               this.TotalRINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_MEAN, attNameExtension),
               this.TotalRINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_SD, attNameExtension),
               this.TotalRINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));
        }
        public void SetAmortCostsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
               string.Concat(TAMOC, attNameExtension),
               this.TotalAMOC.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMOC_MEAN, attNameExtension),
               this.TotalAMOC_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMOC_SD, attNameExtension),
               this.TotalAMOC_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMOC_N, attNameExtension),
               this.TotalAMOC_N.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMAOH, attNameExtension),
                this.TotalAMAOH.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMAOH_MEAN, attNameExtension),
               this.TotalAMAOH_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMAOH_SD, attNameExtension),
               this.TotalAMAOH_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMCAP, attNameExtension),
               this.TotalAMCAP.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMCAP_MEAN, attNameExtension),
               this.TotalAMCAP_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMCAP_SD, attNameExtension),
               this.TotalAMCAP_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMINCENT, attNameExtension),
               this.TotalAMINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMINCENT_MEAN, attNameExtension),
               this.TotalAMINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMINCENT_SD, attNameExtension),
               this.TotalAMINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMOC_NET, attNameExtension),
               this.TotalAMOC_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMOC_NET_MEAN, attNameExtension),
               this.TotalAMOC_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMOC_NET_SD, attNameExtension),
               this.TotalAMOC_NET_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMAOH_NET, attNameExtension),
               this.TotalAMAOH_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMAOH_NET_MEAN, attNameExtension),
               this.TotalAMAOH_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMAOH_NET_SD, attNameExtension),
               this.TotalAMAOH_NET_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMAOH_NET2, attNameExtension),
               this.TotalAMAOH_NET2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMAOH_NET2_MEAN, attNameExtension),
               this.TotalAMAOH_NET2_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMAOH_NET2_SD, attNameExtension),
               this.TotalAMAOH_NET2_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMCAP_NET, attNameExtension),
               this.TotalAMCAP_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMCAP_NET_MEAN, attNameExtension),
               this.TotalAMCAP_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMCAP_NET_SD, attNameExtension),
               this.TotalAMCAP_NET_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMINCENT_NET, attNameExtension),
               this.TotalAMINCENT_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMINCENT_NET_MEAN, attNameExtension),
               this.TotalAMINCENT_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMINCENT_NET_SD, attNameExtension),
               this.TotalAMINCENT_NET_SD.ToString("N2", CultureInfo.InvariantCulture));
        }
        public void SetAmortCostBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            //names already written
            writer.WriteAttributeString(
                string.Concat(TAMR_N, attNameExtension),
               this.TotalAMR_N.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TRAmount, attNameExtension),
               this.TotalRAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                 string.Concat(TRAmount_MEAN, attNameExtension),
                this.TotalRAmount_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRAmount_SD, attNameExtension),
               this.TotalRAmount_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                 string.Concat(TAMR, attNameExtension),
                this.TotalAMR.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                 string.Concat(TAMR_MEAN, attNameExtension),
                this.TotalAMR_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR_SD, attNameExtension),
               this.TotalAMR_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMRINCENT, attNameExtension),
               this.TotalRINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_MEAN, attNameExtension),
               this.TotalRINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_SD, attNameExtension),
               this.TotalRINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));
        }
        //N deprecated in favor of v145 single Observations property approach
        public virtual void CopyNCostsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalOCAmount_N = calculator.TotalOCAmount_N;
            this.TotalOCPrice_N = calculator.TotalOCPrice_N;
            this.TotalOC_N = calculator.TotalOC_N;
            this.TotalOC_INT_N = calculator.TotalOC_INT_N;
            this.TotalAOHAmount_N = calculator.TotalAOHAmount_N;
            this.TotalAOHPrice_N = calculator.TotalAOHPrice_N;
            this.TotalAOH_N = calculator.TotalAOH_N;
            this.TotalAOH_INT_N = calculator.TotalAOH_INT_N;
            this.TotalCAPAmount_N = calculator.TotalCAPAmount_N;
            this.TotalCAPPrice_N = calculator.TotalCAPPrice_N;
            this.TotalCAP_N = calculator.TotalCAP_N;
            this.TotalCAP_INT_N = calculator.TotalCAP_INT_N;

            this.TotalINCENT_N = calculator.TotalINCENT_N;

            this.TotalAMOC_N = calculator.TotalAMOC_N;
            this.TotalAMOC_INT_N = calculator.TotalAMOC_INT_N;
            this.TotalAMOC_NET_N = calculator.TotalAMOC_NET_N;
            this.TotalAMAOH_N = calculator.TotalAMAOH_N;
            this.TotalAMAOH_INT_N = calculator.TotalAMAOH_INT_N;
            this.TotalAMAOH_NET_N = calculator.TotalAMAOH_NET_N;
            this.TotalAMAOH_NET2_N = calculator.TotalAMAOH_NET2_N;
            this.TotalAMCAP_N = calculator.TotalAMCAP_N;
            this.TotalAMCAP_INT_N = calculator.TotalAMCAP_INT_N;
            this.TotalAMCAP_NET_N = calculator.TotalAMCAP_NET_N;
            this.TotalAMINCENT_N = calculator.TotalAMINCENT_N;
            this.TotalAMINCENT_NET_N = calculator.TotalAMINCENT_NET_N;
            this.TotalAnnuities_N = calculator.TotalAnnuities_N;
            //this.TotalAMTOTAL_N = calculator.TotalAMINCENT_N;
            //this.TotalAMNET_N = calculator.TotalAMINET_N;
        }
        public virtual void InitNCostsProperties()
        {
            //init totals to zero prior to running calculations
            this.TotalOCAmount_N = 0;
            this.TotalOCPrice_N = 0;
            this.TotalOC_N = 0;
            this.TotalOC_INT_N = 0;
            this.TotalAOHAmount_N = 0;
            this.TotalAOHPrice_N = 0;
            this.TotalAOH_N = 0;
            this.TotalAOH_INT_N = 0;
            this.TotalCAPAmount_N = 0;
            this.TotalCAPPrice_N = 0;
            this.TotalCAP_N = 0;
            this.TotalCAP_INT_N = 0;
            this.TotalINCENT_N = 0;

            this.TotalAMOC_N = 0;
            this.TotalAMOC_INT_N = 0;
            this.TotalAMOC_NET_N = 0;
            this.TotalAMAOH_N = 0;
            this.TotalAMAOH_INT_N = 0;
            this.TotalAMAOH_NET_N = 0;
            this.TotalAMAOH_NET2_N = 0;
            this.TotalAMCAP_N = 0;
            this.TotalAMCAP_INT_N = 0;
            this.TotalAMCAP_NET_N = 0;
            this.TotalAMINCENT_N = 0;
            this.TotalAMINCENT_NET_N = 0;
        }
        public virtual void SetNCostsProperties(XElement calculator)
        {
            this.TotalOCAmount_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCAmount_N);
            this.TotalOCPrice_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCPrice_N);
            this.TotalOC_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_N);
            this.TotalOC_INT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_INT_N);
            this.TotalAOHAmount_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHAmount_N);
            this.TotalAOHPrice_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHPrice_N);
            this.TotalAOH_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_N);
            this.TotalAOH_INT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_INT_N);
            this.TotalCAPAmount_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPAmount_N);
            this.TotalCAPPrice_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPPrice_N);
            this.TotalCAP_N = CalculatorHelpers.GetAttributeDouble(calculator,
                 TCAP_N);
            this.TotalCAP_INT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAP_INT_N);

            this.TotalINCENT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TINCENT_N);

            this.TotalAMOC_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_N);
            this.TotalAMOC_INT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_INT_N);
            this.TotalAMOC_NET_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_NET_N);
            this.TotalAMAOH_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_N);
            this.TotalAMAOH_INT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_INT_N);
            this.TotalAMAOH_NET_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET_N);
            this.TotalAMAOH_NET2_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET2_N);
            this.TotalAMCAP_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_N);
            this.TotalAMCAP_INT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_INT_N);
            this.TotalAMCAP_NET_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_NET_N);
            this.TotalAMINCENT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_N);
            this.TotalAMINCENT_NET_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_NET_N);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetNCostsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TOCAmount_N:
                    this.TotalOCAmount_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOCPrice_N:
                    this.TotalOCPrice_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_N:
                    this.TotalOC_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_INT_N:
                    this.TotalOC_INT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHAmount_N:
                    this.TotalAOHAmount_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHPrice_N:
                    this.TotalAOHPrice_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_N:
                    this.TotalAOH_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_INT_N:
                    this.TotalAOH_INT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPAmount_N:
                    this.TotalCAPAmount_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPPrice_N:
                    this.TotalCAPPrice_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_N:
                    this.TotalCAP_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_INT_N:
                    this.TotalCAP_INT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TINCENT_N:
                    this.TotalINCENT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TAMOC_N:
                    this.TotalAMOC_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_INT_N:
                    this.TotalAMOC_INT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_NET_N:
                    this.TotalAMOC_NET_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_N:
                    this.TotalAMAOH_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_INT_N:
                    this.TotalAMAOH_INT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET_N:
                    this.TotalAMAOH_NET_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET2_N:
                    this.TotalAMAOH_NET2_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_N:
                    this.TotalAMCAP_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_INT_N:
                    this.TotalAMCAP_INT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_NET_N:
                    this.TotalAMCAP_NET_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_N:
                    this.TotalAMINCENT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_NET_N:
                    this.TotalAMINCENT_NET_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetNCostsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TOCAmount_N:
                    sPropertyValue = this.TotalOCAmount_N.ToString();
                    break;
                case TOCPrice_N:
                    sPropertyValue = this.TotalOCPrice_N.ToString();
                    break;
                case TOC_N:
                    sPropertyValue = this.TotalOC_N.ToString();
                    break;
                case TOC_INT_N:
                    sPropertyValue = this.TotalOC_INT_N.ToString();
                    break;
                case TAOHAmount_N:
                    sPropertyValue = this.TotalAOHAmount_N.ToString();
                    break;
                case TAOHPrice_N:
                    sPropertyValue = this.TotalAOHPrice_N.ToString();
                    break;
                case TAOH_N:
                    sPropertyValue = this.TotalAOH_N.ToString();
                    break;
                case TAOH_INT_N:
                    sPropertyValue = this.TotalAOH_INT_N.ToString();
                    break;
                case TCAPAmount_N:
                    sPropertyValue = this.TotalCAPAmount_N.ToString();
                    break;
                case TCAPPrice_N:
                    sPropertyValue = this.TotalCAPPrice_N.ToString();
                    break;
                case TCAP_N:
                    sPropertyValue = this.TotalCAP_N.ToString();
                    break;
                case TCAP_INT_N:
                    sPropertyValue = this.TotalCAP_INT_N.ToString();
                    break;

                case TINCENT_N:
                    sPropertyValue = this.TotalINCENT_N.ToString();
                    break;

                case TAMOC_N:
                    sPropertyValue = this.TotalAMOC_N.ToString();
                    break;
                case TAMOC_INT_N:
                    sPropertyValue = this.TotalAMOC_INT_N.ToString();
                    break;
                case TAMOC_NET_N:
                    sPropertyValue = this.TotalAMOC_NET_N.ToString();
                    break;
                case TAMAOH_N:
                    sPropertyValue = this.TotalAMAOH_N.ToString();
                    break;
                case TAMAOH_INT_N:
                    sPropertyValue = this.TotalAMAOH_INT_N.ToString();
                    break;
                case TAMAOH_NET_N:
                    sPropertyValue = this.TotalAMAOH_NET_N.ToString();
                    break;
                case TAMAOH_NET2_N:
                    sPropertyValue = this.TotalAMAOH_NET2_N.ToString();
                    break;
                case TAMCAP_N:
                    sPropertyValue = this.TotalAMCAP_N.ToString();
                    break;
                case TAMCAP_INT_N:
                    sPropertyValue = this.TotalAMCAP_INT_N.ToString();
                    break;
                case TAMCAP_NET_N:
                    sPropertyValue = this.TotalAMCAP_NET_N.ToString();
                    break;
                case TAMINCENT_N:
                    sPropertyValue = this.TotalAMINCENT_N.ToString();
                    break;
                case TAMINCENT_NET_N:
                    sPropertyValue = this.TotalAMINCENT_NET_N.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetNCostsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //not absolutely necessary to set the type of the attribute
            //the double was properly set when the property was set
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCAmount_N, attNameExtension),
                this.TotalOCAmount_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOCPrice_N, attNameExtension),
                this.TotalOCPrice_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_N, attNameExtension),
                this.TotalOC_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOC_INT_N, attNameExtension),
                this.TotalOC_INT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHAmount_N, attNameExtension),
                this.TotalAOHAmount_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOHPrice_N, attNameExtension),
                this.TotalAOHPrice_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_N, attNameExtension),
                this.TotalAOH_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAOH_INT_N, attNameExtension),
                this.TotalAOH_INT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPAmount_N, attNameExtension),
                this.TotalCAPAmount_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAPPrice_N, attNameExtension),
                this.TotalCAPPrice_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_N, attNameExtension),
                this.TotalCAP_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCAP_INT_N, attNameExtension),
                this.TotalCAP_INT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TINCENT_N, attNameExtension),
                this.TotalINCENT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_N, attNameExtension),
                this.TotalAMOC_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_INT_N, attNameExtension),
                this.TotalAMOC_INT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMOC_NET_N, attNameExtension),
                this.TotalAMOC_NET_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_N, attNameExtension),
                this.TotalAMAOH_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_INT_N, attNameExtension),
                this.TotalAMAOH_INT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET_N, attNameExtension),
                this.TotalAMAOH_NET_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMAOH_NET2_N, attNameExtension),
                this.TotalAMAOH_NET2_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_N, attNameExtension),
                this.TotalAMCAP_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_INT_N, attNameExtension),
                this.TotalAMCAP_INT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMCAP_NET_N, attNameExtension),
                this.TotalAMCAP_NET_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_N, attNameExtension),
                this.TotalAMINCENT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMINCENT_NET_N, attNameExtension),
                this.TotalAMINCENT_NET_N);
        }
        public virtual void SetNCostsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TOCAmount_N, attNameExtension),
                this.TotalOCAmount_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOCPrice_N, attNameExtension),
                this.TotalOCPrice_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_N, attNameExtension),
                this.TotalOC_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_INT_N, attNameExtension),
                this.TotalOC_INT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHAmount_N, attNameExtension),
                this.TotalAOHAmount_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHPrice_N, attNameExtension),
                this.TotalAOHPrice_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_N, attNameExtension),
                this.TotalAOH_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_INT_N, attNameExtension),
                this.TotalAOH_INT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPAmount_N, attNameExtension),
                this.TotalCAPAmount_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPPrice_N, attNameExtension),
                this.TotalCAPPrice_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_N, attNameExtension),
                this.TotalCAP_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_INT_N, attNameExtension),
                this.TotalCAP_INT_N.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TINCENT_N, attNameExtension),
                this.TotalINCENT_N.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMOC_N, attNameExtension),
                this.TotalAMOC_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_INT_N, attNameExtension),
                this.TotalAMOC_INT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_NET_N, attNameExtension),
                this.TotalAMOC_NET_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_N, attNameExtension),
                this.TotalAMAOH_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_INT_N, attNameExtension),
                this.TotalAMAOH_INT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET_N, attNameExtension),
                this.TotalAMAOH_NET_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET2_N, attNameExtension),
                this.TotalAMAOH_NET2_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_N, attNameExtension),
                this.TotalAMCAP_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_INT_N, attNameExtension),
                this.TotalAMCAP_INT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_NET_N, attNameExtension),
                this.TotalAMCAP_NET_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_N, attNameExtension),
                this.TotalAMINCENT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET_N, attNameExtension),
                this.TotalAMINCENT_NET_N.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void CopyNBenefitsProperties(
            CostBenefitStatistic01 calculator)
        {
            this.TotalRPrice_N = calculator.TotalRPrice_N;
            this.TotalRAmount_N = calculator.TotalRAmount_N;
            this.TotalRCompositionAmount_N = calculator.TotalRCompositionAmount_N;
            this.TotalR_N = calculator.TotalR_N;
            this.TotalR_INT_N = calculator.TotalR_INT_N;
            this.TotalRINCENT_N = calculator.TotalRINCENT_N;
            this.TotalAMR_N = calculator.TotalAMR_N;
            this.TotalAMRINCENT_N = calculator.TotalAMRINCENT_N;
        }
        public virtual void InitNBenefitsProperties()
        {
            this.TotalRUnit_N = string.Empty;
            this.TotalRPrice_N = 0;
            this.TotalRAmount_N = 0;
            this.TotalRCompositionAmount_N = 0;
            this.TotalR_N = 0;
            this.TotalR_INT_N = 0;
            this.TotalRINCENT_N = 0;
            this.TotalAMR_N = 0;
            this.TotalAMRINCENT_N = 0;
            this.TotalAnnuities_N = 0;
        }
        public virtual void SetNBenefitsProperties(XElement calculator)
        {
            this.TotalRAmount_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TRAmount_N);
            this.TotalRPrice_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TRPrice_N);
            this.TotalRCompositionAmount_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TRCompositionAmount_N);
            this.TotalR_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_N);
            this.TotalR_INT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_INT_N);
            this.TotalRINCENT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TRINCENT_N);
            this.TotalAMR_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR_N);
            this.TotalAMRINCENT_N = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMRINCENT_N);
        }
        public virtual void SetNBenefitsProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TRAmount_N:
                    this.TotalRAmount_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice_N:
                    this.TotalRPrice_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRCompositionAmount_N:
                    this.TotalRCompositionAmount_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_N:
                    this.TotalR_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_INT_N:
                    this.TotalR_INT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRINCENT_N:
                    this.TotalRINCENT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR_N:
                    this.TotalAMR_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT_N:
                    this.TotalAMRINCENT_N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetNBenefitsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TRAmount_N:
                    sPropertyValue = this.TotalRAmount_N.ToString();
                    break;
                case TRPrice_N:
                    sPropertyValue = this.TotalRPrice_N.ToString();
                    break;
                case TRCompositionAmount_N:
                    sPropertyValue = this.TotalRCompositionAmount_N.ToString();
                    break;
                case TR_N:
                    sPropertyValue = this.TotalR_N.ToString();
                    break;
                case TR_INT_N:
                    sPropertyValue = this.TotalR_INT_N.ToString();
                    break;
                case TRINCENT_N:
                    sPropertyValue = this.TotalRINCENT_N.ToString();
                    break;
                case TAMR_N:
                    sPropertyValue = this.TotalAMR_N.ToString();
                    break;
                case TAMRINCENT_N:
                    sPropertyValue = this.TotalAMRINCENT_N.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetNBenefitsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRAmount_N, attNameExtension),
                this.TotalRAmount_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRPrice_N, attNameExtension),
                this.TotalRPrice_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRCompositionAmount_N, attNameExtension),
                this.TotalRCompositionAmount_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_N, attNameExtension),
                this.TotalR_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TR_INT_N, attNameExtension),
                this.TotalR_INT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRINCENT_N, attNameExtension),
                this.TotalRINCENT_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMR_N, attNameExtension),
                this.TotalAMR_N);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAMRINCENT_N, attNameExtension),
                this.TotalAMRINCENT_N);
        }
        public virtual void SetNBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRAmount_N, attNameExtension),
                this.TotalRAmount_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRPrice_N, attNameExtension),
                this.TotalRPrice_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRCompositionAmount_N, attNameExtension),
                this.TotalRCompositionAmount_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_N, attNameExtension),
                this.TotalR_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_INT_N, attNameExtension),
                this.TotalR_INT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRINCENT_N, attNameExtension),
                this.TotalRINCENT_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR_N, attNameExtension),
                this.TotalAMR_N.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT_N, attNameExtension),
                this.TotalAMRINCENT_N.ToString("N2", CultureInfo.InvariantCulture));
        }
        //version 1.4.5
        public void SetAmortBenefits2Attributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                       string.Concat(TAMR, attNameExtension), this.TotalAMR.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMR_MEAN, attNameExtension), this.TotalAMR_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMR_MED, attNameExtension), this.TotalAMR_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMR_VAR2, attNameExtension), this.TotalAMR_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMR_SD, attNameExtension), this.TotalAMR_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMRINCENT, attNameExtension), this.TotalAMRINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMRINCENT_MEAN, attNameExtension), this.TotalAMRINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMRINCENT_MED, attNameExtension), this.TotalAMRINCENT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMRINCENT_VAR2, attNameExtension), this.TotalAMRINCENT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMRINCENT_SD, attNameExtension), this.TotalAMRINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void SetAmortBenefits2PsandQsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRName, attNameExtension),
                this.TotalRName);
            writer.WriteAttributeString(
               string.Concat(TRUnit, attNameExtension),
               this.TotalRUnit);
            writer.WriteAttributeString(
                string.Concat(TRCompositionUnit, attNameExtension),
                this.TotalRCompositionUnit);
            writer.WriteAttributeString(
                string.Concat(TRAmount, attNameExtension),
                this.TotalRAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRAmount_MEAN, attNameExtension), this.TotalRAmount_MEAN.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRAmount_MED, attNameExtension), this.TotalRAmount_MED.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRAmount_VAR2, attNameExtension), this.TotalRAmount_VAR2.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRAmount_SD, attNameExtension), this.TotalRAmount_SD.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRPrice, attNameExtension),
                this.TotalRPrice.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRPrice_MEAN, attNameExtension), this.TotalRPrice_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRPrice_MED, attNameExtension), this.TotalRPrice_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRPrice_VAR2, attNameExtension), this.TotalRPrice_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRPrice_SD, attNameExtension), this.TotalRPrice_SD.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRCompositionAmount, attNameExtension),
                this.TotalRCompositionAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRCompositionAmount_MEAN, attNameExtension), this.TotalRCompositionAmount_MEAN.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRCompositionAmount_MED, attNameExtension), this.TotalRCompositionAmount_MED.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRCompositionAmount_VAR2, attNameExtension), this.TotalRCompositionAmount_VAR2.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TRCompositionAmount_SD, attNameExtension), this.TotalRCompositionAmount_SD.ToString("N3", CultureInfo.InvariantCulture));
        }
        public virtual void CopyBenefits2PsandQsAttributes(CostBenefitStatistic01 stat)
        {
            this.TotalRName = stat.TotalRName;
            this.TotalRUnit = stat.TotalRUnit;
            this.TotalRCompositionUnit = stat.TotalRCompositionUnit;
            this.TotalRAmount = stat.TotalRAmount;
            this.TotalRAmount_MEAN = stat.TotalRAmount_MEAN;
            this.TotalRAmount_MED = stat.TotalRAmount_MED;
            this.TotalRAmount_VAR2 = stat.TotalRAmount_VAR2;
            this.TotalRAmount_SD = stat.TotalRAmount_SD;
            this.TotalRPrice = stat.TotalRPrice;
            this.TotalRPrice_MEAN = stat.TotalRPrice_MEAN;
            this.TotalRPrice_MED = stat.TotalRPrice_MED;
            this.TotalRPrice_VAR2 = stat.TotalRPrice_VAR2;
            this.TotalRPrice_SD = stat.TotalRPrice_SD;
            this.TotalRCompositionAmount = stat.TotalRCompositionAmount;
            this.TotalRCompositionAmount_MEAN = stat.TotalRCompositionAmount_MEAN;
            this.TotalRCompositionAmount_MED = stat.TotalRCompositionAmount_MED;
            this.TotalRCompositionAmount_VAR2 = stat.TotalRCompositionAmount_VAR2;
            this.TotalRCompositionAmount_SD = stat.TotalRCompositionAmount_SD;
        }
        public void SetAmortCosts2Attributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TAMOC, attNameExtension), this.TotalAMOC.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMOC_MEAN, attNameExtension), this.TotalAMOC_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMOC_MED, attNameExtension), this.TotalAMOC_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMOC_VAR2, attNameExtension), this.TotalAMOC_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMOC_SD, attNameExtension), this.TotalAMOC_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMAOH, attNameExtension), this.TotalAMAOH.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMAOH_MEAN, attNameExtension), this.TotalAMAOH_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMAOH_MED, attNameExtension), this.TotalAMAOH_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMAOH_VAR2, attNameExtension), this.TotalAMAOH_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMAOH_SD, attNameExtension), this.TotalAMAOH_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMCAP, attNameExtension), this.TotalAMCAP.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMCAP_MEAN, attNameExtension), this.TotalAMCAP_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMCAP_MED, attNameExtension), this.TotalAMCAP_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMCAP_VAR2, attNameExtension), this.TotalAMCAP_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMCAP_SD, attNameExtension), this.TotalAMCAP_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMTOTAL, attNameExtension), this.TotalAMTOTAL.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMTOTAL_MEAN, attNameExtension), this.TotalAMTOTAL_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMTOTAL_MED, attNameExtension), this.TotalAMTOTAL_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMTOTAL_VAR2, attNameExtension), this.TotalAMTOTAL_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMTOTAL_SD, attNameExtension), this.TotalAMTOTAL_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMINCENT, attNameExtension), this.TotalAMINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMINCENT_MEAN, attNameExtension), this.TotalAMINCENT_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMINCENT_MED, attNameExtension), this.TotalAMINCENT_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMINCENT_VAR2, attNameExtension), this.TotalAMINCENT_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(TAMINCENT_SD, attNameExtension), this.TotalAMINCENT_SD.ToString("N2", CultureInfo.InvariantCulture));
        }
        public void SetAmortNets2Attributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TAMNET, attNameExtension), 
                this.TotalAMNET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMNET_MEAN, attNameExtension), 
                this.TotalAMNET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMNET_MED, attNameExtension), 
                this.TotalAMNET_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMNET_VAR2, attNameExtension), 
                this.TotalAMNET_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMNET_SD, attNameExtension), 
                this.TotalAMNET_SD.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
               string.Concat(TAMINCENT_NET, attNameExtension),
               this.TotalAMINCENT_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMINCENT_NET_MEAN, attNameExtension),
               this.TotalAMINCENT_NET_MEAN.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET_MED, attNameExtension), 
                this.TotalAMINCENT_NET_MED.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET_VAR2, attNameExtension), 
                this.TotalAMINCENT_NET_VAR2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMINCENT_NET_SD, attNameExtension),
               this.TotalAMINCENT_NET_SD.ToString("N2", CultureInfo.InvariantCulture));
        }
    }
}
