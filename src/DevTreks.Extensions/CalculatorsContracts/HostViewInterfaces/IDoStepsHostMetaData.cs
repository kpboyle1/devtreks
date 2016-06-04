using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace DevTreks.Extensions
{
    //enum for exportmetadata value
    public enum CALULATOR_CONTRACT_TYPES
    {
        none = 0,
        defaultcalculatormanager = 1
    }
    /// <summary>
    ///Purpose:		Host's metadata from imported calculators, 
    ///             analyzers, and storytellers. 
    ///Author:		www.devtreks.org
    ///Date:		2010, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES
    ///             1. These specific attributes are not really needed, but exported 
    ///             attributes will be needed in future upgrades, so learn now.
    /// </summary>
    public interface IDoStepsHostMetaData
    {
        //metadata provided by the imported parts
        CALULATOR_CONTRACT_TYPES CONTRACT_TYPE { get; }
        string CalculatorsExtensionName { get; }
    }

    //custom attribute used for exports
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class DoStepsAttribute : ExportAttribute
    {
        public DoStepsAttribute() : base(typeof(DoStepsHostView)) { }
        //note: this information is already available, but keep these attributes here
        //because export metadata will eventually be needed
        public CALULATOR_CONTRACT_TYPES CONTRACT_TYPE {get; set;}
        public string CalculatorsExtensionName { get; set; }
    }

    
}
