using DevTreks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods for price apps
    ///Author:		www.devtreks.org
    ///Date:		2013, August
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES       
    /// </summary>
    public class Prices
    {
        public Prices() { }

        /// <summary>
        /// Type of input price node being used.
        /// </summary>
        public enum INPUT_PRICE_TYPES
        {
            inputtype   = 0,
            inputgroup  = 1,
            input       = 2,
            inputseries = 3
        }

        /// <summary>
        /// Type of output price node being used.
        /// </summary>
        public enum OUTPUT_PRICE_TYPES
        {
            outputtype      = 0,
            outputgroup     = 1,
            output          = 2,
            outputseries    = 3
        }
        public enum OUTCOME_PRICE_TYPES
        {
            outcometype     = 0,
            outcomegroup    = 1,
            outcome         = 2,
            outcomeoutput   = 3
        }
        /// <summary>
        /// Type of operation price node being used.
        /// Note: although arbitrary inputs can be grouped together
        /// for any purpose, they are normally grouped together to define
        /// a technological process that becomes part of a budget. 
        /// The same logic holds for budgets - inputs and outputs 
        /// can be grouped together for more purposes than just 
        /// defining technologies (and government agency databases
        /// are available to prove it)
        /// </summary>
        public enum OPERATION_PRICE_TYPES
        {
            operationtype   = 0,
            operationgroup  = 1,
            operation       = 2,
            operationinput  = 3
        }
        /// <summary>
        /// Type of component price node being used.
        /// see operation technology comment
        /// </summary>
        public enum COMPONENT_PRICE_TYPES
        {
            componenttype       = 0,
            componentgroup      = 1,
            component           = 2,
            componentinput      = 3
        }

        //xml attribute names
        //inputs
        public const string INPUT_DATE = "InputDate";
        public const string OC_AMOUNT = "InputPrice1Amount";
        public const string OC_PRICE = "InputPrice1";
        public const string OC_UNIT = "InputUnit1";
        public const string AOH_AMOUNT = "InputPrice2Amount";
        public const string AOH_PRICE = "InputPrice2";
        public const string AOH_UNIT = "InputUnit2";
        public const string CAP_AMOUNT = "InputPrice3Amount";
        public const string CAP_PRICE = "InputPrice3";
        public const string CAP_UNIT = "InputUnit3";
        public const string USE_AOH = "InputUseAOHOnly";
        public const string INPUT_TIMES = "InputTimes";
        public const string INPUT_ID = "InputId";
        public const string INPUT_GROUP_ID = "InputClassId";
        public const string INPUT_GROUP_NAME = "InputClassName";
        public const string INPUT_GROUP_LABEL = "InputClassLabel";
        //outputs
        public const string OUTPUT_PRICE1 = "OutputPrice1";
        public const string OUTPUT_AMOUNT1 = "OutputAmount1";
        public const string OUTPUT_UNIT1 = "OutputUnit1";
        public const string OUTPUT_DATE = "OutputDate";
        public const string COMPOSITION_AMOUNT = "OutputCompositionAmount";
        public const string COMPOSITION_UNIT = "OutputCompositionUnit";
        public const string OUTPUT_TIMES = "OutputTimes";
        public const string OUTPUT_ID = "OutputId";
        public const string OUTPUT_GROUP_ID = "OutputClassId";
        public const string OUTPUT_GROUP_NAME = "OutputClassName";
        public const string OUTPUT_GROUP_LABEL = "OutputClassLabel";
        //operations, components, and outcomes
        public const string ISPRICELIST = "IsPriceList";
        public const string WEIGHT = "ResourceWeight";
        public const string AMOUNT = "Amount";
        public const string EFFECTIVE_LIFE = "EffectiveLife";
        public const string INITIAL_VALUE = "InitialValue";
        public const string SALVAGE_VALUE = "SalvageValue";
        public const string ENDOFPERIOD_DATE = "Date";
        public const string OPERATION_ID = "OperationId";
        public const string OPERATION_GROUP_ID = "OperationClassId";
        public const string OPERATION_GROUP_NAME = "OperationClassName";
        public const string OPERATION_GROUP_LABEL = "OperationClassLabel";
        public const string COMPONENT_ID = "ComponentId";
        public const string COMPONENT_GROUP_ID = "ComponentClassId";
        public const string COMPONENT_GROUP_NAME = "ComponentClassName";
        public const string COMPONENT_GROUP_LABEL = "ComponentClassLabel";
        public const string OUTCOME_ID = "OutcomeId";
        public const string OUTCOME_GROUP_ID = "OutcomeClassId";
        public const string OUTCOME_GROUP_NAME = "OutcomeClassName";
        public const string OUTCOME_GROUP_LABEL = "OutcomeClassLabel";

        //misc calculators
        public const string cEndOfPeriodDate = "EndOfPeriodDate";
        public static Dictionary<string, string> GetPriceTypes(ContentURI uri)
        {
            Dictionary<string, string> colTypes = new Dictionary<string, string>();
            if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
            {
                if (uri.URIModels.InputType != null)
                {
                    foreach (var type in uri.URIModels.InputType)
                    {
                        //note that on the client the key becomes the option's value
                        colTypes.Add(type.PKId.ToString(), type.Name);
                    }
                }
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
            {
                if (uri.URIModels.OutputType != null)
                {
                    foreach (var type in uri.URIModels.OutputType)
                    {
                        //note that on the client the key becomes the option's value
                        colTypes.Add(type.PKId.ToString(), type.Name);
                    }
                }
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
            {
                if (uri.URIModels.OutcomeType != null)
                {
                    foreach (var type in uri.URIModels.OutcomeType)
                    {
                        //note that on the client the key becomes the option's value
                        colTypes.Add(type.PKId.ToString(), type.Name);
                    }
                }
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
            {
                if (uri.URIModels.OperationType != null)
                {
                    foreach (var type in uri.URIModels.OperationType)
                    {
                        //note that on the client the key becomes the option's value
                        colTypes.Add(type.PKId.ToString(), type.Name);
                    }
                }
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
            {
                if (uri.URIModels.ComponentType != null)
                {
                    foreach (var type in uri.URIModels.ComponentType)
                    {
                        //note that on the client the key becomes the option's value
                        colTypes.Add(type.PKId.ToString(), type.Name);
                    }
                }
            }
            return colTypes;
        }
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
            {
                InitInputNavigation(currentNodeName, currentId, uri);
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
            {
                InitOutputNavigation(currentNodeName, currentId, uri);
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
            {
                InitOutcomeNavigation(currentNodeName, currentId, uri);
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
            {
                InitOperationNavigation(currentNodeName, currentId, uri);
            }
            else if (uri.URIDataManager.SubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
            {
                InitComponentNavigation(currentNodeName, currentId, uri);
            }
        }
        private static void InitInputNavigation(string currentNodeName,
            int currentId, ContentURI uri)
        {
            //the current params change depending on the node type
            if (currentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //show the client's input group/inputs
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = INPUT_PRICE_TYPES.inputgroup.ToString();
            }
            else if (currentNodeName == INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                //show the client's input group/inputs
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = INPUT_PRICE_TYPES.input.ToString();
            }
            else if (currentNodeName == INPUT_PRICE_TYPES.input.ToString())
            {
                //show the client's input/input series
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = INPUT_PRICE_TYPES.inputseries.ToString();
            }
            else if (currentNodeName == INPUT_PRICE_TYPES.inputseries.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel 
                    == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards:none
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                uri.URIDataManager.ChildrenNodeName = string.Empty;
            }
        }
        private static void InitOutputNavigation(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //show the client's input group/inputs
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = OUTPUT_PRICE_TYPES.outputgroup.ToString();
            }
            else if (currentNodeName == OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = OUTPUT_PRICE_TYPES.output.ToString();
            }
            else if (currentNodeName == OUTPUT_PRICE_TYPES.output.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = OUTPUT_PRICE_TYPES.outputseries.ToString();
            }
            else if (currentNodeName == OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                    == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards:none
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                uri.URIDataManager.ChildrenNodeName = string.Empty;
            }
        }
        private static void InitOutcomeNavigation(string currentNodeName,
           int currentId, ContentURI uri)
        {
            if (currentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //show the client's input group/inputs
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = OUTCOME_PRICE_TYPES.outcomegroup.ToString();
            }
            else if (currentNodeName == OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = OUTCOME_PRICE_TYPES.outcome.ToString();
            }
            else if (currentNodeName == OUTCOME_PRICE_TYPES.outcome.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                //ability to use selects panel for edits means that inputseries needed
                uri.URIDataManager.ChildrenNodeName = OUTPUT_PRICE_TYPES.outputseries.ToString();
                //uri.URIDataManager.ChildrenNodeName = OUTCOME_PRICE_TYPES.outcomeinput.ToString();
            }
        }
        private static void InitOperationNavigation(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //show the client's input group/inputs
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = OPERATION_PRICE_TYPES.operationgroup.ToString();
            }
            else if (currentNodeName == OPERATION_PRICE_TYPES.operationgroup.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = OPERATION_PRICE_TYPES.operation.ToString();
            }
            else if (currentNodeName == OPERATION_PRICE_TYPES.operation.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                //ability to use selects panel for edits means that inputseries needed
                uri.URIDataManager.ChildrenNodeName = INPUT_PRICE_TYPES.inputseries.ToString();
                //uri.URIDataManager.ChildrenNodeName = OPERATION_PRICE_TYPES.operationinput.ToString();
            }
        }
        private static void InitComponentNavigation(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //show the client's input group/inputs
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = COMPONENT_PRICE_TYPES.componentgroup.ToString();
            }
            else if (currentNodeName == COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                uri.URIDataManager.ChildrenNodeName = COMPONENT_PRICE_TYPES.component.ToString();
            }
            else if (currentNodeName == COMPONENT_PRICE_TYPES.component.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    //checkboxes for node insertions
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                //ability to use selects panel for edits means that inputseries needed
                uri.URIDataManager.ChildrenNodeName = INPUT_PRICE_TYPES.inputseries.ToString();
            }
        }
        
        public static string GetPricesQueryName(Helpers.GeneralHelpers.SUBAPPLICATION_TYPES
            subAppType, string currentNodeName)
        {
            string sQryName = string.Empty;
            if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
            {
                sQryName = "0GetInputXml";
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
            {
                sQryName = "0GetOutputXml";
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
            {
                sQryName = "0GetOutcomeXml";
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
            {
                sQryName = "0GetOperationXml";
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
            {
                sQryName = "0GetComponentXml";
            }
            return sQryName;
        }
        
        
        public static string GetUpdatePricesQueryName(ContentURI uri)
        {
            string sQryName = string.Empty;
            if (uri.URIDataManager.SubAppType ==
                Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
            {
                sQryName = "0UpdateInputXml";
            }
            else if (uri.URIDataManager.SubAppType ==
                Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
            {
                sQryName = "0UpdateOutputXml";
            }
            else if (uri.URIDataManager.SubAppType ==
                Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
            {
                sQryName = "0UpdateOutcomeXml";
            }
            else if (uri.URIDataManager.SubAppType ==
                Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
            {
                sQryName = "0UpdateOperationXml";
            }
            else if (uri.URIDataManager.SubAppType ==
                Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
            {
                sQryName = "0UpdateComponentXml";
            }
            return sQryName;
        }
        public static void GetChildForeignKeyName(Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subAppType,
            string parentNodeName, out string childForeignKeyName)
        {
            childForeignKeyName = string.Empty;
            switch (subAppType)
            {
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        childForeignKeyName = Agreement.SERVICE_ID;
                    }
                    else if (parentNodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        childForeignKeyName = INPUT_GROUP_ID;
                    }
                    else if (parentNodeName == Prices.INPUT_PRICE_TYPES.input.ToString())
                    {
                        childForeignKeyName = INPUT_ID;
                    }
                    else if (parentNodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                    {
                        childForeignKeyName = string.Empty;
                    }
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        childForeignKeyName = Agreement.SERVICE_ID;
                    }
                    else if (parentNodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        childForeignKeyName = OUTPUT_GROUP_ID;
                    }
                    else if (parentNodeName == Prices.OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        childForeignKeyName = OUTPUT_ID;
                    }
                    else if (parentNodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                    {
                        childForeignKeyName = string.Empty;
                    }
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        childForeignKeyName = Agreement.SERVICE_ID;
                    }
                    else if (parentNodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        childForeignKeyName = OUTCOME_GROUP_ID;
                    }
                    else if (parentNodeName == Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        childForeignKeyName = OUTCOME_ID;
                    }
                    else if (parentNodeName == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString()
                        || parentNodeName == Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                    {
                        childForeignKeyName = string.Empty;
                    }
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        childForeignKeyName = Agreement.SERVICE_ID;
                    }
                    else if (parentNodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        childForeignKeyName = OPERATION_GROUP_ID;
                    }
                    else if (parentNodeName == Prices.OPERATION_PRICE_TYPES.operation.ToString())
                    {
                        childForeignKeyName = OPERATION_ID;
                    }
                    else if (parentNodeName == Prices.OPERATION_PRICE_TYPES.operationinput.ToString()
                        || parentNodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                    {
                        childForeignKeyName = string.Empty;
                    }
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        childForeignKeyName = Agreement.SERVICE_ID;
                    }
                    else if (parentNodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        childForeignKeyName = COMPONENT_GROUP_ID;
                    }
                    else if (parentNodeName == Prices.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        childForeignKeyName = COMPONENT_ID;
                    }
                    else if (parentNodeName == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString()
                        || parentNodeName == Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                    {
                        childForeignKeyName = string.Empty;
                    }
                    break;
                default:
                    break;
            }
        }
        public static void ChangeInputAttributes(EditHelpers.EditHelper.ArgumentsEdits addsArguments,
            XmlReader selectedNodeReader, string parentId, 
            string baseTableId, string currentNodeName,
            bool isRecursiveNode, XmlWriter writer)
        {
            if (addsArguments.URIToEdit.URIDataManager.SubAppType
                    != Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
            {
                ChangeInputSubApplicationAttributes(selectedNodeReader,
                    addsArguments.URIToEdit.URIDataManager.SubAppType, isRecursiveNode,
                    parentId, baseTableId, writer);
            }
            else
            {
                //inputs into inputseries require one more attribute
                ChangeInputSeriesAttribute(currentNodeName, selectedNodeReader, baseTableId, writer);
            }
        }
        /// <summary>
        /// Copy and change input attributes over to input attributes that are specific to a subapplication
        /// </summary>
        private static void ChangeInputSubApplicationAttributes(XmlReader selectedNodeReader,
            Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subAppType, bool isRecursive, string parentId, string baseTableId,
            XmlWriter writer)
        {
            string sAttName = string.Empty;
            string sAttValue = string.Empty;
            //write new attributes
            if (isRecursive == false)
            {
                //copying an input/series from input list
                writer.WriteAttributeString(USE_AOH, "0");
                writer.WriteAttributeString(INPUT_TIMES, "1");
                writer.WriteAttributeString(AOH_AMOUNT, "0");
                writer.WriteAttributeString(CAP_AMOUNT, "0");
                writer.WriteAttributeString(General.INCENTIVE_RATE, "0");
                writer.WriteAttributeString(General.INCENTIVE_AMOUNT, "0");
                writer.WriteAttributeString(INPUT_ID, baseTableId);
            }
            if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets)
            {
                writer.WriteAttributeString(Economics1.BUDGETSYSTEM_TO_OPERATION_ID, parentId);
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments)
            {
                writer.WriteAttributeString(Economics1.COSTSYSTEM_TO_COMPONENT_ID, parentId);
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
            {
                writer.WriteAttributeString(OUTCOME_ID, parentId);
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
            {
                writer.WriteAttributeString(OPERATION_ID, parentId);
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
            {
                writer.WriteAttributeString(COMPONENT_ID, parentId);
            }
        }
        private static void ChangeInputSeriesAttribute(string currentNodeName,
            XmlReader selectedNodeReader, string baseTableId, XmlWriter writer)
        {
            if (currentNodeName == AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                if (selectedNodeReader.GetAttribute(INPUT_ID) == null)
                {
                    //input being inserted into an input series
                    writer.WriteAttributeString(INPUT_ID, baseTableId);
                }
            }
        }
        public static void ChangeOutputAttributes(EditHelpers.EditHelper.ArgumentsEdits addsArguments,
            XmlReader selectedNodeReader, string parentId,
            string baseTableId, string currentNodeName,
            bool isRecursiveNode, XmlWriter writer)
        {
            if (addsArguments.URIToEdit.URIDataManager.SubAppType != Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
            {
                //change the subapplication-specific attributes
                ChangeOutputSubApplicationAttributes(addsArguments.URIToEdit.URIDataManager.SubAppType, parentId,
                    baseTableId, writer);
            }
            else
            {
                //outputs into outputseries require one more attribute
                ChangeOutputSeriesAttribute(currentNodeName, selectedNodeReader,
                    baseTableId, writer);
            }
        }
        private static void ChangeOutputSeriesAttribute(string currentNodeName,
            XmlReader selectedNodeReader, string baseTableId, XmlWriter writer)
        {
            if (currentNodeName == AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                if (selectedNodeReader.GetAttribute(OUTPUT_ID) == null)
                {
                    //input being inserted into an input series
                    writer.WriteAttributeString(OUTPUT_ID, baseTableId);
                }
            }
        }
        /// <summary>
        /// Copy and change output attributes over to output attributes that are specific to a subapplication
        /// </summary>
        private static void ChangeOutputSubApplicationAttributes(Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subAppType,
            string parentId, string baseTableId, XmlWriter writer)
        {
            //write new attributes
            writer.WriteAttributeString(COMPOSITION_AMOUNT, "1");
            writer.WriteAttributeString(COMPOSITION_UNIT, "acre");
            if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets)
            {
                //write subapplication specific attributes
                writer.WriteAttributeString(Economics1.BUDGETSYSTEM_TO_TIME_ID, parentId);
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments)
            {
                writer.WriteAttributeString(Economics1.COSTSYSTEM_TO_TIME_ID, parentId);
            }
            writer.WriteAttributeString(OUTPUT_ID, baseTableId);
        }
        public static void ChangeOperationandComponentAttributes(EditHelpers.EditHelper.ArgumentsEdits addsArguments,
            XmlReader selectedNodeReader, string parentId,
            string baseTableId, string currentNodeName,
            bool isRecursiveNode, XmlWriter writer)
        {
            //change the standard attributes (a lot of the operation's atts are not needed)
            ChangeOperationComponentStandardAttributes(selectedNodeReader, 
                ref baseTableId, writer);
            if (addsArguments.URIToEdit.URIDataManager.SubAppType
                != Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
            {
                //change the subapplication-specific attributes
                ChangeOperationComponentSubApplicationAttributes(
                    addsArguments.URIToEdit.URIDataManager.SubAppType,
                    selectedNodeReader, parentId, baseTableId, writer);
            }
        }
        /// <summary>
        /// Copy and change operation attributes from base schemas to join schemas 
        /// </summary>
        private static void ChangeOperationComponentStandardAttributes(XmlReader selectedNodeReader,
            ref string baseTableId, XmlWriter writer)
        {
            string sAttName = string.Empty;
            string sAttValue = string.Empty;
            //write existing attributes
            //write subapplication specific attributes
            sAttName = Calculator.cId;
            sAttValue = selectedNodeReader.GetAttribute(sAttName);
            writer.WriteAttributeString(sAttName, sAttValue);
            baseTableId = sAttValue;
            //use the existing attvalue
            sAttName = Calculator.cLabel;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = Calculator.cName;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = Calculator.cDescription;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = WEIGHT;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = General.AMOUNT;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = General.UNIT;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = EFFECTIVE_LIFE;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = SALVAGE_VALUE;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = General.INCENTIVE_RATE;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = General.INCENTIVE_AMOUNT;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            sAttName = Calculator.cDate;
            writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
        }

        /// <summary>
        /// Copy and change operation/component attributes that are specific to a subapplication
        /// </summary>
        private static void ChangeOperationComponentSubApplicationAttributes(Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subAppType,
            XmlReader selectedNodeReader, string parentId, string baseTableId, XmlWriter writer)
        {
            string sAttName = string.Empty;
            string sAttValue = string.Empty;
            if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets)
            {
                //write subapplication specific attributes
                writer.WriteAttributeString(Economics1.BUDGETSYSTEM_TO_TIME_ID, parentId);
                //write base table id
                writer.WriteAttributeString(OPERATION_ID, baseTableId);
                sAttName = OPERATION_GROUP_NAME;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = OPERATION_GROUP_ID;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = Calculator.cTypeId;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = Calculator.cTypeName;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments)
            {
                writer.WriteAttributeString(Economics1.COSTSYSTEM_TO_TIME_ID, parentId);
                //write base table id
                writer.WriteAttributeString(COMPONENT_ID, baseTableId);
                sAttName = COMPONENT_GROUP_NAME;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = COMPONENT_GROUP_ID;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = Calculator.cTypeId;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = Calculator.cTypeName;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            }
            else if (subAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
            {
                writer.WriteAttributeString(Economics1.COSTSYSTEM_TO_TIME_ID, parentId);
                //write base table id
                writer.WriteAttributeString(OUTCOME_ID, baseTableId);
                sAttName = OUTCOME_GROUP_NAME;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = OUTCOME_GROUP_ID;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = Calculator.cTypeId;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
                sAttName = Calculator.cTypeName;
                writer.WriteAttributeString(sAttName, selectedNodeReader.GetAttribute(sAttName));
            }
        }
        public async Task SaveSummaryAndFullTotals(ContentURI docToCalcURI,
            ContentURI calcDocURI, IDictionary<string, string> childrenLinkedView)
        {
            //the full totals are saved temporarily in oDocToCalc.URIDataManager.TempDocPath (tempDocToCalcPath)
            //now save them in docToCalcURI paths, including descendants
            //filenames change to include calcDocURIPattern.URIId and childrentLinkedView
            string sTotalsFilePath = docToCalcURI.URIDataManager.TempDocPath;
            if (Helpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                sTotalsFilePath))
            {
                bool bNeedsSummaryPriceDocs
                    = NeedsSummaryPriceDocs(docToCalcURI);
                if (bNeedsSummaryPriceDocs)
                {
                    LinkedViews.SaveTempDocTotalsToLinkedViewPaths(docToCalcURI,
                        calcDocURI);
                    XmlDocument oDocToCalc = new XmlDocument();
                    XmlReader reader = await Helpers.FileStorageIO.GetXmlReaderAsync(docToCalcURI,
                        docToCalcURI.URIDataManager.TempDocPath);
                    if (reader != null)
                    {
                        using (reader)
                        {
                            oDocToCalc.Load(reader);
                        }
                    }
                    if (oDocToCalc.DocumentElement.HasChildNodes == true)
                    {
                        string sQry = EditHelpers.XmlIO.MakeXPathAbbreviatedQry(docToCalcURI.URINodeName,
                            Helpers.GeneralHelpers.AT_ID, docToCalcURI.URIId.ToString());
                        //the iterator is used to shallow clone nodes that make up the summary docs
                        XPathNodeIterator oTotalsGroupIterator = oDocToCalc.CreateNavigator().Select(sQry);
                        if (oTotalsGroupIterator != null)
                        {
                            if (oTotalsGroupIterator.Count > 0)
                            {
                                //move to the iterator's top node 
                                oTotalsGroupIterator.MoveNext();
                                //start building the basic doc that will be used as the basis for all of the subsequent docs to be saved (cloning from the root)
                                XmlDocument oBaseDoc = new XmlDocument();
                                oBaseDoc.LoadXml(Helpers.GeneralHelpers.ROOT_NODE);
                                if (docToCalcURI.URINodeName.EndsWith(OUTPUT_PRICE_TYPES.output.ToString())
                                    || docToCalcURI.URINodeName.EndsWith(INPUT_PRICE_TYPES.input.ToString()))
                                {
                                    //need the group node cloned to the base doc
                                    LinkedViews.CloneGroupNode(oDocToCalc, ref oBaseDoc);
                                }
                                await InitSaveTotals(docToCalcURI, calcDocURI,
                                    oTotalsGroupIterator, oBaseDoc,
                                    docToCalcURI.URIClub.ClubDocFullPath, childrenLinkedView);
                            }
                        }
                    }
                }
            }
            else
            {
                docToCalcURI.ErrorMessage = Exceptions.Errors.MakeStandardErrorMsg(
                   "", "ECONOMICS_NOTEMPFILE");
            }
        }
        
        public static bool NeedsSummaryPriceDocs(ContentURI docToCalcURI)
        {
            bool bNeedsSummaryPriceDocs = false;
            if (docToCalcURI.URINodeName
                != AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                && docToCalcURI.URINodeName
                != AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()
                && docToCalcURI.URINodeName
                != AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()
                && docToCalcURI.URINodeName
                != AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString()
                && docToCalcURI.URINodeName
                != AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                bNeedsSummaryPriceDocs = true;
            }
            return bNeedsSummaryPriceDocs;
        }
        /// <summary>
        /// Build and save sub totals documents based on the group totals document sent in.
        /// </summary>
        private async Task InitSaveTotals(ContentURI docToCalcURI, ContentURI calcDocURI, 
            XPathNodeIterator totalsNodeIterator, XmlDocument baseDoc, 
            string directoryPath, IDictionary<string, string> childrenLinkedView)
        {
            if (totalsNodeIterator != null)
            {
                //create a navigator to navigate through children
                XPathNavigator oNavigator = totalsNodeIterator.Current.Clone();
                if (oNavigator.LocalName == Helpers.GeneralHelpers.ROOT_PATH)
                {
                    totalsNodeIterator.MoveNext();
                }
                //clone just the node (not the children) for the various documents
                XmlElement oDocElement = (XmlElement)((IHasXmlNode)totalsNodeIterator.Current).GetNode().CloneNode(false);
                AppHelpers.LinkedViews oLinkedView = new AppHelpers.LinkedViews();
                //save the summary docs
                oLinkedView.SaveSummaryDoc(docToCalcURI, calcDocURI, oDocElement,
                    baseDoc, oNavigator, directoryPath, childrenLinkedView);
                //the base doc has these shallow cloned elements added
                bool bIsDeepClone = false;
                EditHelpers.XmlIO.AppendNodeToLastDocNode(oDocElement, bIsDeepClone,
                    baseDoc.FirstChild, ref baseDoc);
                //save the full docs
                await oLinkedView.SaveFullDoc(docToCalcURI, calcDocURI, oDocElement,
                    baseDoc, oNavigator, directoryPath, childrenLinkedView);
            }
        }
        private static bool IsStartDocToCalcNode(string navDocToCalcNodeName,
            string startDocToCalcURIPattern)
        {
            bool bIsStartDocToCalcNode = false;
            string sStartNodeName = ContentURI.GetURIPatternPart(
                startDocToCalcURIPattern, ContentURI.URIPATTERNPART.node);
            if (navDocToCalcNodeName
                == sStartNodeName)
            {
                //calculators only change db atts here when they are the start doc uri
                //other changes are possible in the update descendent methods
                bIsStartDocToCalcNode = true;

            }
            return bIsStartDocToCalcNode;
        }
        private static bool NeedsInputDescriptionendantsSeriesUpdate(string navDocToCalcNodeName,
            string startDocToCalcURIPattern)
        {
            bool bNeedsSeriesUpdates = false;
            string sStartNodeName = ContentURI.GetURIPatternPart(
                startDocToCalcURIPattern, ContentURI.URIPATTERNPART.node);
            if (navDocToCalcNodeName
                == INPUT_PRICE_TYPES.input.ToString())
            {
                //only descend to input series from input
                if (sStartNodeName
                    == INPUT_PRICE_TYPES.input.ToString())
                {
                    bNeedsSeriesUpdates = true;
                }

            }
            else if (navDocToCalcNodeName
                == OUTPUT_PRICE_TYPES.output.ToString())
            {
                //only descend to output series from output
                if (sStartNodeName
                    == OUTPUT_PRICE_TYPES.output.ToString())
                {
                    bNeedsSeriesUpdates = true;
                }
            }
            return bNeedsSeriesUpdates;
        }
        public static bool AddInputElementToParent(XElement root, XElement childEl,
            string groupingElementName, string parentId, string parentNodeName,
            InputClass inputClass)
        {
            bool bIsAdded = false;
            if (childEl != null)
            {
                bool bParentExists = EditHelpers.XmlLinq.DescendantExists(root, parentNodeName, parentId);
                if (bParentExists)
                {
                    string sParentId = string.Empty;
                    string sParentName = string.Empty;
                    string sGrandParentId = string.Empty;
                    string sGrandParentName = string.Empty;
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                        && inputClass.InputType != null)
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = inputClass.InputType.PKId.ToString();
                            sParentName = inputClass.InputType.Name;
                            childEl.SetAttributeValue(Calculator.cTypeId, sParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cId);
                            sParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cName);
                            childEl.SetAttributeValue(INPUT_GROUP_ID, sParentId);
                            childEl.SetAttributeValue(INPUT_GROUP_NAME, sParentName);
                            sGrandParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeId);
                            sGrandParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeName);
                            childEl.SetAttributeValue(Calculator.cTypeId, sGrandParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sGrandParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == INPUT_PRICE_TYPES.input.ToString())
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, INPUT_GROUP_ID);
                            sParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, INPUT_GROUP_NAME);
                            childEl.SetAttributeValue(INPUT_GROUP_ID, sParentId);
                            childEl.SetAttributeValue(INPUT_GROUP_NAME, sParentName);
                            sGrandParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeId);
                            sGrandParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeName);
                            childEl.SetAttributeValue(Calculator.cTypeId, sGrandParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sGrandParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                }
            }
            return bIsAdded;
        }
        public static bool AddOutputElementToParent(XElement root, XElement childEl,
            string groupingElementName, string parentId, string parentNodeName,
            OutputClass outputClass)
        {
            bool bIsAdded = false;
            if (childEl != null)
            {
                bool bParentExists = EditHelpers.XmlLinq.DescendantExists(root, parentNodeName, parentId);
                if (bParentExists)
                {
                    string sParentId = string.Empty;
                    string sParentName = string.Empty;
                    string sGrandParentId = string.Empty;
                    string sGrandParentName = string.Empty;
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                        && outputClass.OutputType != null)
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = outputClass.OutputType.PKId.ToString();
                            sParentName = outputClass.OutputType.Name;
                            childEl.SetAttributeValue(Calculator.cTypeId, sParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cId);
                            sParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cName);
                            childEl.SetAttributeValue(OUTPUT_GROUP_ID, sParentId);
                            childEl.SetAttributeValue(OUTPUT_GROUP_NAME, sParentName);
                            sGrandParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeId);
                            sGrandParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeName);
                            childEl.SetAttributeValue(Calculator.cTypeId, sGrandParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sGrandParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == OUTPUT_PRICE_TYPES.output.ToString())
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, OUTPUT_GROUP_ID);
                            sParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, OUTPUT_GROUP_NAME);
                            childEl.SetAttributeValue(OUTPUT_GROUP_ID, sParentId);
                            childEl.SetAttributeValue(OUTPUT_GROUP_NAME, sParentName);
                            sGrandParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeId);
                            sGrandParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeName);
                            childEl.SetAttributeValue(Calculator.cTypeId, sGrandParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sGrandParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                }
            }
            return bIsAdded;
        }
        public static bool AddOutcomeElementToParent(XElement root, XElement childEl,
            string groupingElementName, string parentId, string parentNodeName,
            OutcomeClass outcomeClass)
        {
            bool bIsAdded = false;
            if (childEl != null)
            {
                bool bParentExists = EditHelpers.XmlLinq.DescendantExists(root, parentNodeName, parentId);
                if (bParentExists)
                {
                    string sParentId = string.Empty;
                    string sParentName = string.Empty;
                    string sGrandParentId = string.Empty;
                    string sGrandParentName = string.Empty;
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                        && outcomeClass.OutcomeType != null)
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = outcomeClass.OutcomeType.PKId.ToString();
                            sParentName = outcomeClass.OutcomeType.Name;
                            childEl.SetAttributeValue(Calculator.cTypeId, sParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cId);
                            sParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cName);
                            childEl.SetAttributeValue(OUTCOME_GROUP_ID, sParentId);
                            childEl.SetAttributeValue(OUTCOME_GROUP_NAME, sParentName);
                            sGrandParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeId);
                            sGrandParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeName);
                            childEl.SetAttributeValue(Calculator.cTypeId, sGrandParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sGrandParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == OUTCOME_PRICE_TYPES.outcome.ToString())
                    {
                        bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                    }
                }
            }
            return bIsAdded;
        }
        public static bool AddOperationElementToParent(XElement root, XElement childEl,
            string groupingElementName, string parentId, string parentNodeName,
            OperationClass operationClass)
        {
            bool bIsAdded = false;
            if (childEl != null)
            {
                bool bParentExists = EditHelpers.XmlLinq.DescendantExists(root, parentNodeName, parentId);
                if (bParentExists)
                {
                    string sParentId = string.Empty;
                    string sParentName = string.Empty;
                    string sGrandParentId = string.Empty;
                    string sGrandParentName = string.Empty;
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                        && operationClass.OperationType != null)
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = operationClass.OperationType.PKId.ToString();
                            sParentName = operationClass.OperationType.Name;
                            childEl.SetAttributeValue(Calculator.cTypeId, sParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cId);
                            sParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cName);
                            childEl.SetAttributeValue(OPERATION_GROUP_ID, sParentId);
                            childEl.SetAttributeValue(OPERATION_GROUP_NAME, sParentName);
                            sGrandParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeId);
                            sGrandParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeName);
                            childEl.SetAttributeValue(Calculator.cTypeId, sGrandParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sGrandParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == OPERATION_PRICE_TYPES.operation.ToString())
                    {
                        bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                    }
                }
            }
            return bIsAdded;
        }
        public static void AddBaseInputSeriesToXml(XElement componentinput,
            ComponentToInput componentToInput)
        {
            if (componentToInput.InputSeries != null)
            {
                componentinput.SetAttributeValue("InputPrice1", componentToInput.InputSeries.InputPrice1.ToString());
                componentinput.SetAttributeValue("InputUnit1", componentToInput.InputSeries.InputUnit1);
                componentinput.SetAttributeValue("InputUnit2", componentToInput.InputSeries.InputUnit2);
                componentinput.SetAttributeValue("InputPrice2", componentToInput.InputSeries.InputPrice2.ToString());
                componentinput.SetAttributeValue("InputUnit3", componentToInput.InputSeries.InputUnit3);
                componentinput.SetAttributeValue("InputPrice3", componentToInput.InputSeries.InputPrice3.ToString());
                if (componentToInput.InputSeries.Input != null)
                {
                    if (componentToInput.InputSeries.Input.InputClass != null)
                    {
                        componentinput.SetAttributeValue(INPUT_GROUP_ID, componentToInput.InputSeries.Input.InputClass.PKId.ToString());
                        componentinput.SetAttributeValue(INPUT_GROUP_NAME, componentToInput.InputSeries.Input.InputClass.Name);
                        componentinput.SetAttributeValue(INPUT_GROUP_LABEL, componentToInput.InputSeries.Input.InputClass.Num);
                        if (componentToInput.InputSeries.Input.InputClass.InputType != null)
                        {
                            componentinput.SetAttributeValue(Calculator.cTypeId, componentToInput.InputSeries.Input.InputClass.InputType.PKId.ToString());
                            componentinput.SetAttributeValue(Calculator.cTypeName, componentToInput.InputSeries.Input.InputClass.InputType.Name);
                        }
                    }
                }
            }
        }
        public static void AddBaseInputSeriesToXml(XElement operationinput,
            OperationToInput operationToInput)
        {
            if (operationToInput.InputSeries != null)
            {
                operationinput.SetAttributeValue("InputPrice1", operationToInput.InputSeries.InputPrice1.ToString());
                operationinput.SetAttributeValue("InputUnit1", operationToInput.InputSeries.InputUnit1);
                operationinput.SetAttributeValue("InputUnit2", operationToInput.InputSeries.InputUnit2);
                operationinput.SetAttributeValue("InputPrice2", operationToInput.InputSeries.InputPrice2.ToString());
                operationinput.SetAttributeValue("InputUnit3", operationToInput.InputSeries.InputUnit3);
                operationinput.SetAttributeValue("InputPrice3", operationToInput.InputSeries.InputPrice3.ToString());
                if (operationToInput.InputSeries.Input != null)
                {
                    if (operationToInput.InputSeries.Input.InputClass != null)
                    {
                        operationinput.SetAttributeValue(INPUT_GROUP_ID, operationToInput.InputSeries.Input.InputClass.PKId.ToString());
                        operationinput.SetAttributeValue(INPUT_GROUP_NAME, operationToInput.InputSeries.Input.InputClass.Name);
                        operationinput.SetAttributeValue(INPUT_GROUP_LABEL, operationToInput.InputSeries.Input.InputClass.Num);
                        if (operationToInput.InputSeries.Input.InputClass.InputType != null)
                        {
                            operationinput.SetAttributeValue(Calculator.cTypeId, operationToInput.InputSeries.Input.InputClass.InputType.PKId.ToString());
                            operationinput.SetAttributeValue(Calculator.cTypeName, operationToInput.InputSeries.Input.InputClass.InputType.Name);
                        }
                    }
                }
            }

        }
        public static void AddBaseOutputSeriesToXml(XElement outcomeoutput,
            OutcomeToOutput outcomeToOutput)
        {
            if (outcomeToOutput.OutputSeries != null)
            {
                outcomeoutput.SetAttributeValue("OutputPrice1", outcomeToOutput.OutputSeries.OutputPrice1.ToString());
                outcomeoutput.SetAttributeValue("OutputUnit1", outcomeToOutput.OutputSeries.OutputUnit1);
                if (outcomeToOutput.OutputSeries.Output != null)
                {
                    if (outcomeToOutput.OutputSeries.Output.OutputClass != null)
                    {
                        outcomeoutput.SetAttributeValue(OUTPUT_GROUP_ID, outcomeToOutput.OutputSeries.Output.OutputClass.PKId.ToString());
                        outcomeoutput.SetAttributeValue(OUTPUT_GROUP_NAME, outcomeToOutput.OutputSeries.Output.OutputClass.Name);
                        outcomeoutput.SetAttributeValue(OUTPUT_GROUP_LABEL, outcomeToOutput.OutputSeries.Output.OutputClass.Num);
                        if (outcomeToOutput.OutputSeries.Output.OutputClass.OutputType != null)
                        {
                            outcomeoutput.SetAttributeValue(Calculator.cTypeId, outcomeToOutput.OutputSeries.Output.OutputClass.OutputType.PKId.ToString());
                            outcomeoutput.SetAttributeValue(Calculator.cTypeName, outcomeToOutput.OutputSeries.Output.OutputClass.OutputType.Name);
                        }
                    }
                }
            }

        }
        public static bool AddComponentElementToParent(XElement root, XElement childEl,
            string groupingElementName, string parentId, string parentNodeName,
            ComponentClass componentClass)
        {
            bool bIsAdded = false;
            if (childEl != null)
            {
                bool bParentExists = EditHelpers.XmlLinq.DescendantExists(root, parentNodeName, parentId);
                if (bParentExists)
                {
                    string sParentId = string.Empty;
                    string sParentName = string.Empty;
                    string sGrandParentId = string.Empty;
                    string sGrandParentName = string.Empty;
                    if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                        && componentClass.ComponentType != null)
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = componentClass.ComponentType.PKId.ToString();
                            sParentName = componentClass.ComponentType.Name;
                            childEl.SetAttributeValue(Calculator.cTypeId, sParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        XElement parent = EditHelpers.XmlLinq.GetElement(root, parentNodeName, parentId);
                        if (parent != null)
                        {
                            sParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cId);
                            sParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, AppHelpers.Calculator.cName);
                            childEl.SetAttributeValue(COMPONENT_GROUP_ID, sParentId);
                            childEl.SetAttributeValue(COMPONENT_GROUP_NAME, sParentName);
                            sGrandParentId = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeId);
                            sGrandParentName = EditHelpers.XmlLinq.GetAttributeValue(parent, Calculator.cTypeName);
                            childEl.SetAttributeValue(Calculator.cTypeId, sGrandParentId);
                            childEl.SetAttributeValue(Calculator.cTypeName, sGrandParentName);
                            bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                        }
                    }
                    else if (parentNodeName == COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        bIsAdded = EditHelpers.XmlLinq.AddElementToParent(root, childEl,
                                groupingElementName, parentId, parentNodeName);
                    }
                }
            }
            return bIsAdded;
        }
    }
}
