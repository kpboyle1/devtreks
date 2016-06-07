using DevTreks.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for 'locals' 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       1. Clubs use this class to hold a collection of locals. 
    ///             uri.urimember.clubinuse.locals[].isdefault is used to insert
    ///             default localids (i.e. gecodeid, datasourceid ...) into newly
    ///             added service app nodes (if the node doesn't already have a linked calculator)
    public class Locals
    {
        public Locals()
        {
        }
        //namespaces
        public const string LOCAL_NAMESPACE_NODE_QRY = @"https://www.devtreks.org";
        public const string LOCALGROUP_NS = "y0:localaccountgroup";
        //db fields for updating
        public const string LOCAL_NAME     = "LocalName";
        public const string LOCAL_DESCRIPTION = "LocalDescription";
        //attribute names
        public const string REAL_RATE_ID = "RealRateId";
        public const string NOMINAL_RATE_ID = "NominalRateId";
        public const string UNITGROUP_ID = "UnitGroupId";
        public const string CURRENCYGROUP_ID = "CurrencyGroupId";
        public const string RATINGGROUP_ID = "RatingGroupId";
        public const string DATASOURCE_ID = "DataSourceId";
        public const string GEOCODE_ID = "GeoCodeId";
        public const string DATASOURCETECH_ID = "DataSourceTechId";
        public const string GEOCODETECH_ID = "GeoCodeTechId";
        public const string DATASOURCEPRICE_ID = "DataSourcePriceId";
        public const string GEOCODEPRICE_ID = "GeoCodePriceId";
        public const string REAL_RATE = "RealRate";
        public const string NOMINAL_RATE = "NominalRate";
        public const string INFLATION_RATE = "InflationRate";
        public const string INTEREST_RATE_GROUP_ID = "InterestRateGroupId";
        public const string GEOCODE_NAME_ID = "GeoCodeNameId";
        public const string GEOCODE_PARENT_NAME_ID = "GeoCodeParentNameId";
        public const string GEOCODE_URI = "GeoCodeURI";
        public const string INTEREST_RATES_DATE = "InterestRatesDate";
        public const string GEORREGION_ID = "GeoRegionId";

        public enum LOCAL_TYPES
        {
            //account to accounttolocal view 
            //the locals belonging to a specific club
            localaccountgroup   = 1,
            local               = 2
        }
        public static bool IsLocalsIdAttribute(string attName)
        {
            bool bIsLocalsIdName = false;
            if (attName == REAL_RATE_ID
                || attName == NOMINAL_RATE_ID
                || attName == UNITGROUP_ID
                || attName == CURRENCYGROUP_ID
                || attName == RATINGGROUP_ID
                || attName == DATASOURCE_ID
                || attName == GEOCODE_ID
                || attName == DATASOURCETECH_ID
                || attName == GEOCODETECH_ID
                || attName == DATASOURCEPRICE_ID
                || attName == GEOCODEPRICE_ID)
            {
                bIsLocalsIdName = true;
            }
            return bIsLocalsIdName;
        }
        
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == LOCAL_TYPES.localaccountgroup.ToString())
            {
                if (uri.URIMember.MemberRole == Members.MEMBER_ROLE_TYPES.coordinator.ToString())
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                }
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (uri.URIId == 0)
                {
                    //no link backwards (showing groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                    uri.URIDataManager.ChildrenNodeName = LOCAL_TYPES.localaccountgroup.ToString();
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = LOCAL_TYPES.local.ToString();
                    //link backwards (to groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == LOCAL_TYPES.local.ToString())
            {
                if (uri.URIMember.MemberRole == Members.MEMBER_ROLE_TYPES.coordinator.ToString())
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards because it still will show group-item in toc
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (currentNodeName == LOCAL_TYPES.local.ToString())
                {
                    //no empty tocs
                    uri.URIDataManager.ChildrenNodeName = LOCAL_TYPES.local.ToString();
                }

            }
        }
        public static void GetChildForeignKeyName(string parentNodeName,
            out string parentForeignKeyName, out string baseForeignKeyName)
        {
            parentForeignKeyName = string.Empty;
            baseForeignKeyName = string.Empty;
            if (parentNodeName
                == LOCAL_TYPES.localaccountgroup.ToString())
            {
                parentForeignKeyName = "AccountId";
                baseForeignKeyName = LinkedViews.LINKEDVIEWBASEID;
            }
            //else can insert members into base table during initial registration
        }
        //2.0.0 deprecated
        //public static string GetLocalsJoinQueryName(ContentURI uri)
        //{
        //    string sQryName = string.Empty;//get local.xmldoc field
        //    //holding localization nodes
        //    sQryName = "0GetLocalXml";
        //    return sQryName;
        //}
        
        public static void ChangeAttributesForInsertion(XElement insertedLocal)
        {
            //set locals-specific db attributes
            insertedLocal.SetAttributeValue(LOCAL_NAME, 
                EditHelpers.XmlLinq.GetAttributeValue(insertedLocal, 
                AppHelpers.Calculator.cName));
            insertedLocal.SetAttributeValue(LOCAL_DESCRIPTION,
                EditHelpers.XmlLinq.GetAttributeValue(insertedLocal,
                Calculator.cDescription));
        }

        public static void AddLocals(ContentURI uri, int defaultAddId,
            XElement selectedElement)
        {
            //if the selected element has xmldocs, don't interfere with them
            if (!selectedElement.HasElements)
            {
                bool bNeedsLocals = NeedsLocals(selectedElement, defaultAddId);
                if (bNeedsLocals)
                {
                    if (uri.URIMember.ClubInUse.AccountToLocal != null)
                    {
                        AccountToLocal defaultLocal = GetIsDefaultLocal(uri);
                        if (defaultLocal != null)
                        {
                            //these assist the ui for newly inserted economics nodes
                            //i.e. by giving them the right units of measurement in the drop down selections
                            selectedElement.SetAttributeValue(
                                UNITGROUP_ID, defaultLocal.UnitGroupId.ToString());
                            selectedElement.SetAttributeValue(
                                CURRENCYGROUP_ID, defaultLocal.CurrencyGroupId.ToString());
                            selectedElement.SetAttributeValue(
                                REAL_RATE_ID, defaultLocal.RealRateId.ToString());
                            selectedElement.SetAttributeValue(
                                NOMINAL_RATE_ID, defaultLocal.NominalRateId.ToString());
                            selectedElement.SetAttributeValue(
                                RATINGGROUP_ID, defaultLocal.RatingGroupId.ToString());
                        }
                    }
                }
            }
        }
        
        private static bool NeedsLocals(XElement selectedElement, 
            int uriToAddId)
        {
            bool bNeedsLocals = false;
            //if it is already linked to a calculator, use the locals associated with 
            //the calculator (and assume the calculator has locals)
            int iNominalRateId
                = Helpers.GeneralHelpers.ConvertStringToInt(
                    EditHelpers.XmlLinq.GetElementAttributeValue(
                        selectedElement, NOMINAL_RATE_ID));
            int iRealRateId
                = Helpers.GeneralHelpers.ConvertStringToInt(
                    EditHelpers.XmlLinq.GetElementAttributeValue(
                        selectedElement, REAL_RATE_ID));
            //a uritoaddid = 1 means that a real default node is being added
            //if not equal to 1 its an input or output being added to a series
            //and generally uses it's parent's locals
            if ((iNominalRateId <= 1
                || iRealRateId <= 1)
                && (uriToAddId == 1))
            {
                string sCurrentNodeName = selectedElement.Name.LocalName;
                if (sCurrentNodeName.EndsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                    || sCurrentNodeName.EndsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                    || sCurrentNodeName.EndsWith(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                    || sCurrentNodeName.EndsWith(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
                    || sCurrentNodeName.EndsWith(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                    || sCurrentNodeName
                        == AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString()
                    || sCurrentNodeName
                        == AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString()
                    || sCurrentNodeName
                        == AppHelpers.Economics1.BUDGET_TYPES.budget.ToString()
                    || sCurrentNodeName
                        == AppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString())
                {
                    //these are the only nodes where default locals are added
                    bNeedsLocals = true;
                }
            }
            return bNeedsLocals;
        }
        public static AccountToLocal GetIsDefaultLocal(ContentURI uri)
        {
            AccountToLocal defaultLocal = null;
            if (uri.URIMember.ClubInUse.AccountToLocal != null)
            {
                defaultLocal = uri.URIMember.ClubInUse.AccountToLocal.FirstOrDefault(
                    l => l.IsDefaultLinkedView == true);
            }
            return defaultLocal;
        }
        public static string GetUpdateLocalQueryName(ContentURI uri)
        {
            string sQry = "0UpdateLocalXml";
            return sQry;
        }
        public static async Task<AccountToLocal> GetDefaultLocal(ContentURI uri,
            DataAccess.DevTreksContext context)
        {
            AccountToLocal local = null;
            if (uri.URIMember != null && context != null)
            {
                local = await context.AccountToLocal
                    .Where(cl => cl.AccountId == uri.URIMember.AccountId
                    && cl.IsDefaultLinkedView == true)
                    .FirstOrDefaultAsync();
            }
            return local;
        }
    }
}
