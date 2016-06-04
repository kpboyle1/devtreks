using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Component
    {
        public Component()
        {
            ComponentToInput = new HashSet<ComponentToInput>();
            CostSystemToComponent = new HashSet<CostSystemToComponent>();
            LinkedViewToComponent = new HashSet<LinkedViewToComponent>();
        }
        public Component(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Num2 = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.ResourceWeight = 0;
            this.Amount = 1;
            this.Unit = General.NONE;
            this.EffectiveLife = 1;
            this.SalvageValue = 0;
            this.IncentiveRate = 0;
            this.IncentiveAmount = 0;
            this.Date = General.GetDateShortNow();
            this.LastChangedDate = General.GetDateShortNow();
            this.RatingClassId = 0;
            this.RealRateId = 0;
            this.NominalRateId = 0;
            this.DataSourceId = 0;
            this.GeoCodeId = 0;
            this.CurrencyClassId = 0;
            this.UnitClassId = 0;

            this.ComponentClassId = 0;
            this.ComponentClass = new ComponentClass();
            this.CostSystemToComponent = new List<CostSystemToComponent>();
            this.LinkedViewToComponent = new List<LinkedViewToComponent>();
            this.ComponentToInput = new List<ComponentToInput>();
        }
        public Component(Component rp)
        {
            this.PKId = rp.PKId;
            this.Num = rp.Num;
            this.Num2 = rp.Num2;
            this.Name = rp.Name;
            this.Description = rp.Description;
            this.ResourceWeight = rp.ResourceWeight;
            this.Amount = rp.Amount;
            this.Unit = rp.Unit;
            this.EffectiveLife = rp.EffectiveLife;
            this.SalvageValue = rp.SalvageValue;
            this.IncentiveRate = rp.IncentiveRate;
            this.IncentiveAmount = rp.IncentiveAmount;
            this.Date = rp.Date;
            this.LastChangedDate = rp.LastChangedDate;
            this.RatingClassId = rp.RatingClassId;
            this.RealRateId = rp.RealRateId;
            this.NominalRateId = rp.NominalRateId;
            this.DataSourceId = rp.DataSourceId;
            this.GeoCodeId = rp.GeoCodeId;
            this.CurrencyClassId = rp.CurrencyClassId;
            this.UnitClassId = rp.UnitClassId;

            this.ComponentClassId = rp.ComponentClassId;
            this.ComponentClass = new ComponentClass();
            this.CostSystemToComponent = new List<CostSystemToComponent>();
            this.LinkedViewToComponent = new List<LinkedViewToComponent>();
            this.ComponentToInput = new List<ComponentToInput>();
        }
        public int PKId { get; set; }
        public float Amount { get; set; }
        public int ComponentClassId { get; set; }
        public int CurrencyClassId { get; set; }
        public int DataSourceId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public float EffectiveLife { get; set; }
        public int GeoCodeId { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public string Num2 { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public float ResourceWeight { get; set; }
        public decimal SalvageValue { get; set; }
        public string Unit { get; set; }
        public int UnitClassId { get; set; }

        public virtual ICollection<ComponentToInput> ComponentToInput { get; set; }
        public virtual ICollection<CostSystemToComponent> CostSystemToComponent { get; set; }
        public virtual ICollection<LinkedViewToComponent> LinkedViewToComponent { get; set; }
        public virtual ComponentClass ComponentClass { get; set; }
    }
}
