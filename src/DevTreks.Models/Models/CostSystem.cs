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
        public CostSystem(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            Date = General.GetDateShortNow();
            LastChangedDate = General.GetDateShortNow();
            DocStatus = 0;
            CostSystemToPractice = new List<CostSystemToPractice>();
            LinkedViewToCostSystem = new List<LinkedViewToCostSystem>();
            TypeId = 0;
            CostSystemType = new CostSystemType();
            ServiceId = 0;
            Service = new Service();
        }
        public CostSystem(CostSystem rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Name = rt.Name;
            Description = rt.Description;
            Date = rt.Date;
            LastChangedDate = rt.LastChangedDate;
            DocStatus = rt.DocStatus;
            CostSystemToPractice = new List<CostSystemToPractice>();
            LinkedViewToCostSystem = new List<LinkedViewToCostSystem>();
            TypeId = rt.TypeId;
            CostSystemType = new CostSystemType();
            ServiceId = rt.ServiceId;
            Service = new Service();
        }
        public int PKId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public short DocStatus { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<CostSystemToPractice> CostSystemToPractice { get; set; }
        public virtual ICollection<LinkedViewToCostSystem> LinkedViewToCostSystem { get; set; }
        public virtual Service Service { get; set; }
        public virtual CostSystemType CostSystemType { get; set; }
    }
}
