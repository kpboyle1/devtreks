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
        public OutputSeries(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.OutputUnit1 = General.NONE;
            this.OutputPrice1 = 0;
            this.OutputAmount1 = 1;

            this.OutputDate = General.GetDateShortNow();
            this.OutputLastChangedDate = General.GetDateShortNow();
            this.RatingClassId = 0;
            this.RealRateId = 0;
            this.NominalRateId = 0;
            this.DataSourceId = 0;
            this.GeoCodeId = 0;
            this.CurrencyClassId = 0;
            this.UnitClassId = 0;

            this.OutputId = 0;
            this.Output = new Output();
            this.CostSystemToOutput = new List<CostSystemToOutput>();
            this.BudgetSystemToOutput = new List<BudgetSystemToOutput>();
            this.LinkedViewToOutputSeries = new List<LinkedViewToOutputSeries>();
        }
        public OutputSeries(OutputSeries rp)
        {
            this.PKId = rp.PKId;
            this.Num = rp.Num;
            this.Name = rp.Name;
            this.Description = rp.Description;
            this.OutputUnit1 = rp.OutputUnit1;
            this.OutputPrice1 = rp.OutputPrice1;
            this.OutputAmount1 = rp.OutputAmount1;

            this.OutputDate = rp.OutputDate;
            this.OutputLastChangedDate = rp.OutputLastChangedDate;
            this.RatingClassId = rp.RatingClassId;
            this.RealRateId = rp.RealRateId;
            this.NominalRateId = rp.NominalRateId;
            this.DataSourceId = rp.DataSourceId;
            this.GeoCodeId = rp.GeoCodeId;
            this.CurrencyClassId = rp.CurrencyClassId;
            this.UnitClassId = rp.UnitClassId;

            this.OutputId = rp.OutputId;
            this.Output = new Output();
            this.CostSystemToOutput = new List<CostSystemToOutput>();
            this.BudgetSystemToOutput = new List<BudgetSystemToOutput>();
            this.LinkedViewToOutputSeries = new List<LinkedViewToOutputSeries>();
        }
        public int PKId { get; set; }
        public int CurrencyClassId { get; set; }
        public int DataSourceId { get; set; }
        public string Description { get; set; }
        public int GeoCodeId { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public float OutputAmount1 { get; set; }
        public DateTime OutputDate { get; set; }
        public int OutputId { get; set; }
        public DateTime OutputLastChangedDate { get; set; }
        public decimal OutputPrice1 { get; set; }
        public string OutputUnit1 { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public int UnitClassId { get; set; }

        public virtual ICollection<BudgetSystemToOutput> BudgetSystemToOutput { get; set; }
        public virtual ICollection<CostSystemToOutput> CostSystemToOutput { get; set; }
        public virtual ICollection<LinkedViewToOutputSeries> LinkedViewToOutputSeries { get; set; }
        public virtual ICollection<OutcomeToOutput> OutcomeToOutput { get; set; }
        public virtual Output Output { get; set; }
    }
}
