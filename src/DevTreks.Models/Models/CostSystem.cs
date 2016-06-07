using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CostSystem
    {
        public CostSystem()
        {
            CostSystemToPractice = new HashSet<CostSystemToPractice>();
            LinkedViewToCostSystem = new HashSet<LinkedViewToCostSystem>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int TypeId { get; set; }
        public short DocStatus { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<CostSystemToPractice> CostSystemToPractice { get; set; }
        public virtual ICollection<LinkedViewToCostSystem> LinkedViewToCostSystem { get; set; }
        public virtual Service Service { get; set; }
        public virtual CostSystemType CostSystemType { get; set; }
        //public virtual CostSystemType Type { get; set; }
    }
}
