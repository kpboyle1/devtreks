using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Operation
    {
        public Operation()
        {
            BudgetSystemToOperation = new HashSet<BudgetSystemToOperation>();
            LinkedViewToOperation = new HashSet<LinkedViewToOperation>();
            OperationToInput = new HashSet<OperationToInput>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Num2 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float ResourceWeight { get; set; }
        public float Amount { get; set; }
        public string Unit { get; set; }
        public float EffectiveLife { get; set; }
        public decimal SalvageValue { get; set; }
        public float IncentiveRate { get; set; }
        public decimal IncentiveAmount { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public int NominalRateId { get; set; }
        public int DataSourceId { get; set; }
        public int GeoCodeId { get; set; }
        public int CurrencyClassId { get; set; }
        public int UnitClassId { get; set; }
        public int OperationClassId { get; set; }

        public virtual ICollection<BudgetSystemToOperation> BudgetSystemToOperation { get; set; }
        public virtual ICollection<LinkedViewToOperation> LinkedViewToOperation { get; set; }
        public virtual ICollection<OperationToInput> OperationToInput { get; set; }
        public virtual OperationClass OperationClass { get; set; }
    }
}
