using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class LinkedView
    {
        public LinkedView()
        {
            AccountToAddIn = new HashSet<AccountToAddIn>();
            LinkedViewToBudgetSystem = new HashSet<LinkedViewToBudgetSystem>();
            LinkedViewToBudgetSystemToEnterprise = new HashSet<LinkedViewToBudgetSystemToEnterprise>();
            LinkedViewToBudgetSystemToTime = new HashSet<LinkedViewToBudgetSystemToTime>();
            LinkedViewToComponent = new HashSet<LinkedViewToComponent>();
            LinkedViewToComponentClass = new HashSet<LinkedViewToComponentClass>();
            LinkedViewToCostSystem = new HashSet<LinkedViewToCostSystem>();
            LinkedViewToCostSystemToPractice = new HashSet<LinkedViewToCostSystemToPractice>();
            LinkedViewToCostSystemToTime = new HashSet<LinkedViewToCostSystemToTime>();
            LinkedViewToDevPackJoin = new HashSet<LinkedViewToDevPackJoin>();
            LinkedViewToDevPackPartJoin = new HashSet<LinkedViewToDevPackPartJoin>();
            LinkedViewToInput = new HashSet<LinkedViewToInput>();
            LinkedViewToInputClass = new HashSet<LinkedViewToInputClass>();
            LinkedViewToInputSeries = new HashSet<LinkedViewToInputSeries>();
            LinkedViewToOperation = new HashSet<LinkedViewToOperation>();
            LinkedViewToOperationClass = new HashSet<LinkedViewToOperationClass>();
            LinkedViewToOutcome = new HashSet<LinkedViewToOutcome>();
            LinkedViewToOutcomeClass = new HashSet<LinkedViewToOutcomeClass>();
            LinkedViewToOutput = new HashSet<LinkedViewToOutput>();
            LinkedViewToOutputClass = new HashSet<LinkedViewToOutputClass>();
            LinkedViewToOutputSeries = new HashSet<LinkedViewToOutputSeries>();
            LinkedViewToResource = new HashSet<LinkedViewToResource>();
            LinkedViewToResourcePack = new HashSet<LinkedViewToResourcePack>();
        }
        public LinkedView(bool init)
        {
            PKId = 0;
            LinkedViewNum = General.NONE;
            LinkedViewName = General.NONE;
            LinkedViewDesc = General.NONE;
            LinkedViewFileExtensionType = General.NONE;
            LinkedViewLastChangedDate = General.GetDateShortNow();
            LinkedViewFileName = General.NONE;
            LinkedViewXml = string.Empty;
            LinkedViewAddInName = General.NONE;
            LinkedViewAddInHostName = General.NONE;
            LinkedViewPackId = 0;
            LinkedViewPack = new LinkedViewPack();

            AccountToAddIn = new List<AccountToAddIn>();
            LinkedViewToBudgetSystem = new List<LinkedViewToBudgetSystem>();
            LinkedViewToBudgetSystemToEnterprise = new List<LinkedViewToBudgetSystemToEnterprise>();
            LinkedViewToBudgetSystemToTime = new List<LinkedViewToBudgetSystemToTime>();
            LinkedViewToComponent = new List<LinkedViewToComponent>();
            LinkedViewToComponentClass = new List<LinkedViewToComponentClass>();
            LinkedViewToCostSystem = new List<LinkedViewToCostSystem>();
            LinkedViewToCostSystemToPractice = new List<LinkedViewToCostSystemToPractice>();
            LinkedViewToCostSystemToTime = new List<LinkedViewToCostSystemToTime>();
            LinkedViewToDevPackJoin = new List<LinkedViewToDevPackJoin>();
            LinkedViewToDevPackPartJoin = new List<LinkedViewToDevPackPartJoin>();
            LinkedViewToInput = new List<LinkedViewToInput>();
            LinkedViewToInputClass = new List<LinkedViewToInputClass>();
            LinkedViewToInputSeries = new List<LinkedViewToInputSeries>();
            LinkedViewToResourcePack = new List<LinkedViewToResourcePack>();
            LinkedViewToOperation = new List<LinkedViewToOperation>();
            LinkedViewToOperationClass = new List<LinkedViewToOperationClass>();
            LinkedViewToOutput = new List<LinkedViewToOutput>();
            LinkedViewToOutputClass = new List<LinkedViewToOutputClass>();
            LinkedViewToOutputSeries = new List<LinkedViewToOutputSeries>();
            LinkedViewToResource = new List<LinkedViewToResource>();
        }
        public int PKId { get; set; }
        public string LinkedViewNum { get; set; }
        public string LinkedViewName { get; set; }
        public string LinkedViewDesc { get; set; }
        public string LinkedViewFileExtensionType { get; set; }
        public DateTime LinkedViewLastChangedDate { get; set; }
        public string LinkedViewFileName { get; set; }
        public string LinkedViewXml { get; set; }
        public string LinkedViewAddInName { get; set; }
        public string LinkedViewAddInHostName { get; set; }
        public int LinkedViewPackId { get; set; }

        public virtual ICollection<AccountToAddIn> AccountToAddIn { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystem> LinkedViewToBudgetSystem { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystemToEnterprise> LinkedViewToBudgetSystemToEnterprise { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystemToTime> LinkedViewToBudgetSystemToTime { get; set; }
        public virtual ICollection<LinkedViewToComponent> LinkedViewToComponent { get; set; }
        public virtual ICollection<LinkedViewToComponentClass> LinkedViewToComponentClass { get; set; }
        public virtual ICollection<LinkedViewToCostSystem> LinkedViewToCostSystem { get; set; }
        public virtual ICollection<LinkedViewToCostSystemToPractice> LinkedViewToCostSystemToPractice { get; set; }
        public virtual ICollection<LinkedViewToCostSystemToTime> LinkedViewToCostSystemToTime { get; set; }
        public virtual ICollection<LinkedViewToDevPackJoin> LinkedViewToDevPackJoin { get; set; }
        public virtual ICollection<LinkedViewToDevPackPartJoin> LinkedViewToDevPackPartJoin { get; set; }
        public virtual ICollection<LinkedViewToInput> LinkedViewToInput { get; set; }
        public virtual ICollection<LinkedViewToInputClass> LinkedViewToInputClass { get; set; }
        public virtual ICollection<LinkedViewToInputSeries> LinkedViewToInputSeries { get; set; }
        public virtual ICollection<LinkedViewToOperation> LinkedViewToOperation { get; set; }
        public virtual ICollection<LinkedViewToOperationClass> LinkedViewToOperationClass { get; set; }
        public virtual ICollection<LinkedViewToOutcome> LinkedViewToOutcome { get; set; }
        public virtual ICollection<LinkedViewToOutcomeClass> LinkedViewToOutcomeClass { get; set; }
        public virtual ICollection<LinkedViewToOutput> LinkedViewToOutput { get; set; }
        public virtual ICollection<LinkedViewToOutputClass> LinkedViewToOutputClass { get; set; }
        public virtual ICollection<LinkedViewToOutputSeries> LinkedViewToOutputSeries { get; set; }
        public virtual ICollection<LinkedViewToResource> LinkedViewToResource { get; set; }
        public virtual ICollection<LinkedViewToResourcePack> LinkedViewToResourcePack { get; set; }
        public virtual LinkedViewPack LinkedViewPack { get; set; }
    }
}
