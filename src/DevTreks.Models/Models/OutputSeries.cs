using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutputSeries
    {
        public OutputSeries()
        {
            BudgetSystemToOutput = new HashSet<BudgetSystemToOutput>();
            CostSystemToOutput = new HashSet<CostSystemToOutput>();
            LinkedViewToOutputSeries = new HashSet<LinkedViewToOutputSeries>();
            OutcomeToOutput = new HashSet<OutcomeToOutput>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OutputDate { get; set; }
        public string OutputUnit1 { get; set; }
        public float OutputAmount1 { get; set; }
        public decimal OutputPrice1 { get; set; }
        public DateTime OutputLastChangedDate { get; set; }
        public int RealRateId { get; set; }
        public int NominalRateId { get; set; }
        public int GeoCodeId { get; set; }
        public int DataSourceId { get; set; }
        public int CurrencyClassId { get; set; }
        public int UnitClassId { get; set; }
        public int RatingClassId { get; set; }
        public int OutputId { get; set; }

        public virtual ICollection<BudgetSystemToOutput> BudgetSystemToOutput { get; set; }
        public virtual ICollection<CostSystemToOutput> CostSystemToOutput { get; set; }
        public virtual ICollection<LinkedViewToOutputSeries> LinkedViewToOutputSeries { get; set; }
        public virtual ICollection<OutcomeToOutput> OutcomeToOutput { get; set; }
        public virtual Output Output { get; set; }
    }
}
