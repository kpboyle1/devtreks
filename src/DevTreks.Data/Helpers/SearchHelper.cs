using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities supporting search models
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:
    /// </summary>
    public class SearchHelper
    {
        public SearchHelper()
        {
        }
        /// <summary>
        /// Retrieve searcher results
        /// </summary>
        public async Task<SqlDataReader> GetSearchRecordsAsync(SqlIOAsync sqlIO, 
            SearchManager searcher)
        {
            SqlDataReader dataReader = null;
            if (searcher.SearchResult.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.networks
                || searcher.SearchResult.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.locals
                || searcher.SearchResult.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.addins)
            {
                dataReader = await GetAdminPageSearch(sqlIO, searcher);
            }
            else
            {
                if (searcher.SearchResult.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.searchbyservice)
                {
                    dataReader = await GetPageSearchByService(sqlIO, searcher);
                }
                else
                {
                    dataReader = await GetPageSearch(sqlIO, searcher);
                }
            }
            return dataReader;
        }
        
        private async Task<SqlDataReader> GetAdminPageSearch(SqlIOAsync sqlIO, SearchManager searcher)
        {
            SqlDataReader dataReader = null;
            //initialize parameters for general query
            //uint iIsForward = 1;
            //if (searcher.IsForward == "0") iIsForward = 0;
            //returns pkid, name record list (children of the parent displayed at top of list)
            string sQry = string.Empty;
            bool bIsPageSearch = true;
            sQry = GetQry(searcher.SearchResult, bIsPageSearch);
            if (sQry != string.Empty && sQry != null)
            {
                SqlParameter[] oPrams = GetPageSearchParams(searcher, sqlIO);
                dataReader = await sqlIO.RunProcAsync(sQry, oPrams);
                if (dataReader != null)
                {
                    searcher.RowCount = dataReader.RecordsAffected;
                }
            }
            return dataReader;
        }
        private async Task<SqlDataReader> GetPageSearch(SqlIOAsync sqlIO, SearchManager searcher)
        {
            SqlDataReader dataReader = null;
            //initialize parameters for general query
            //returns Web.ViewDataHelper records at a time
            bool bIsPageSearch = true;
            string sQry = GetQry(searcher.SearchResult, bIsPageSearch);
            if (sQry != string.Empty && sQry != null)
            {
                SqlParameter[] oPrams = GetPageSearchParams(searcher, sqlIO);
                dataReader = await sqlIO.RunProcAsync(sQry, oPrams);
                if (dataReader != null)
                {
                    searcher.RowCount = dataReader.RecordsAffected;
                }
            }
            return dataReader;
        }
        private async Task<SqlDataReader> GetPageSearchByService(SqlIOAsync sqlIO, SearchManager searcher)
        {
            SqlDataReader dataReader = null;
            //initialize parameters for general query
            string sQry = GetSearchByServiceQry(searcher.SearchResult);
            if (sQry != string.Empty && sQry != null)
            {
                SqlParameter[] oPrams = GetPageSearchByServiceParams(searcher, sqlIO);
                dataReader = await sqlIO.RunProcAsync(sQry, oPrams);
                if (dataReader != null)
                {
                    searcher.RowCount = dataReader.RecordsAffected;
                }
            }
            return dataReader;
        }
        private static SqlParameter[] GetPageSearchParams(SearchManager searcher,
            SqlIOAsync data)
        {
            int iIsForward = GeneralHelpers.ConvertStringToInt(searcher.IsForward);
            if (iIsForward != 1 || iIsForward != 0) iIsForward = 1;
            bool bNeedsTypeId = NeedsTypeIdSearchParam(searcher.SearchResult.URIDataManager.AppType);
            if (bNeedsTypeId)
            {
                SqlParameter[] oPrams2 =
                {
                    data.MakeInParam("@AccountId",			SqlDbType.Int, 4, searcher.SearchResult.URIMember.ClubInUse.PKId),
                    data.MakeInParam("@NetworkId",			SqlDbType.Int, 4, searcher.NetworkSelected.PKId),
                    data.MakeInParam("@NetworkType",	    SqlDbType.NVarChar,25, searcher.NetworkType.ToString()),
                    data.MakeInParam("@Keywords",		    SqlDbType.NVarChar, 255, searcher.Keywords),
                    data.MakeInParam("@IsForward",			SqlDbType.Bit, 1, iIsForward),
                    data.MakeInParam("@StartRow",			SqlDbType.Int, 4, searcher.StartRow),
                    data.MakeInParam("@PageSize",			SqlDbType.Int, 4, searcher.PageSize),
                    //filter for types
                    data.MakeInParam("@TypeId",	            SqlDbType.Int, 4, searcher.TypeId)
                };
                return oPrams2;
            }
            else
            {
                SqlParameter[] oPrams3 =
                {
                    data.MakeInParam("@AccountId",			SqlDbType.Int, 4, searcher.SearchResult.URIMember.ClubInUse.PKId),
                    data.MakeInParam("@NetworkId",			SqlDbType.Int, 4, searcher.NetworkSelected.PKId),
                    data.MakeInParam("@NetworkType",	    SqlDbType.NVarChar,25, searcher.NetworkType.ToString()),
                    data.MakeInParam("@Keywords",		    SqlDbType.NVarChar, 255, searcher.Keywords),
                    data.MakeInParam("@IsForward",			SqlDbType.Bit, 1, iIsForward),
                    data.MakeInParam("@StartRow",			SqlDbType.Int, 4, searcher.StartRow),
                    data.MakeInParam("@PageSize",			SqlDbType.Int, 4, searcher.PageSize)
                };
                return oPrams3;
            }
        }
        private static SqlParameter[] GetPageSearchByServiceParams(SearchManager searcher,
            SqlIOAsync data)
        {
            int iIsForward = GeneralHelpers.ConvertStringToInt(searcher.IsForward);
            if (iIsForward != 1 || iIsForward != 0) iIsForward = 1;
            bool bNeedsTypeId = NeedsTypeIdSearchParam(searcher.SearchResult.URIDataManager.AppType);
            if (bNeedsTypeId)
            {
                SqlParameter[] oPrams2 =
                {
                    data.MakeInParam("@AccountId",			SqlDbType.Int, 4, searcher.SearchResult.URIMember.ClubInUse.PKId),
                    data.MakeInParam("@NetworkId",			SqlDbType.Int, 4, searcher.NetworkSelected.PKId),
                    data.MakeInParam("@NetworkType",	    SqlDbType.NVarChar,25, searcher.NetworkType.ToString()),
                    data.MakeInParam("@Keywords",		    SqlDbType.NVarChar, 255, searcher.Keywords),
                    data.MakeInParam("@IsForward",			SqlDbType.Bit, 1, iIsForward),
                    data.MakeInParam("@StartRow",			SqlDbType.Int, 4, searcher.StartRow),
                    data.MakeInParam("@PageSize",			SqlDbType.Int, 4, searcher.PageSize),
                    //filter for types
                    data.MakeInParam("@TypeId",	             SqlDbType.Int, 4, searcher.TypeId),
                    data.MakeInParam("@ServiceId",	        SqlDbType.Int, 4, searcher.ServiceSelected.ServiceId)
                };
                return oPrams2;
            }
            else
            {
                SqlParameter[] oPrams3 =
                {
                    data.MakeInParam("@AccountId",			SqlDbType.Int, 4, searcher.SearchResult.URIMember.ClubInUse.PKId),
                    data.MakeInParam("@NetworkId",			SqlDbType.Int, 4, searcher.NetworkSelected.PKId),
                    data.MakeInParam("@NetworkType",	    SqlDbType.NVarChar,25, searcher.NetworkType.ToString()),
                    data.MakeInParam("@Keywords",		    SqlDbType.NVarChar, 255, searcher.Keywords),
                    data.MakeInParam("@IsForward",			SqlDbType.Bit, 1, iIsForward),
                    data.MakeInParam("@StartRow",			SqlDbType.Int, 4, searcher.StartRow),
                    data.MakeInParam("@PageSize",			SqlDbType.Int, 4, searcher.PageSize),
                    data.MakeInParam("@ServiceId",	        SqlDbType.Int, 4, searcher.ServiceSelected.ServiceId)
                };
                return oPrams3;
            }
        }
        public static string GetQry(ContentURI uri,
            bool isPageSearch)
        {
            string sQry = string.Empty;
            switch (uri.URIDataManager.AppType)
            {
                case GeneralHelpers.APPLICATION_TYPES.accounts:
                    if (isPageSearch == true)
                    {
                        sQry = "0GetSearchAccountsByPage";
                    }
                    else
                    {
                        sQry = "0GetSearchAccountChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.members:
                    if (isPageSearch == true)
                    {
                        sQry = "0GetSearchMembersByPage";
                    }
                    else
                    {
                        sQry = "0GetSearchMemberChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.networks:
                    if (isPageSearch == true)
                    {
                        sQry = "0GetSearchNetworksByPage";
                    }
                    else
                    {
                        sQry = "0GetSearchNetworkChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.locals:
                    //no page search
                    if (isPageSearch == false)
                    {
                        sQry = "0GetSearchLocalChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.addins:
                    //no page search
                    if (isPageSearch == false)
                    {
                        sQry = "0GetSearchAddInChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.agreements:
                    if (isPageSearch == true)
                    {
                        sQry = "0GetSearchServicesByPage";
                    }
                    else
                    {
                        sQry = "0GetSearchServiceChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.prices:
                    switch (uri.URIDataManager.SubAppType)
                    {
                        case GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                            if (isPageSearch == true)
                            {
                                sQry = "0GetSearchInputsByPage";
                            }
                            else
                            {
                                sQry = "0GetSearchInputChildren";
                            }
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                            if (isPageSearch == true)
                            {
                                sQry = "0GetSearchOutputsByPage";
                            }
                            else
                            {
                                sQry = "0GetSearchOutputChildren";
                            }
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                            if (isPageSearch == true)
                            {
                                sQry = "0GetSearchOutcomesByPage";
                            }
                            else
                            {
                                sQry = "0GetSearchOutcomeChildren";
                            }
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                            if (isPageSearch == true)
                            {
                                sQry = "0GetSearchOperationsByPage";
                            }
                            else
                            {
                                sQry = "0GetSearchOperationChildren";
                            }
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                            if (isPageSearch == true)
                            {
                                sQry = "0GetSearchComponentsByPage";
                            }
                            else
                            {
                                sQry = "0GetSearchComponentChildren";
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.economics1:
                    switch (uri.URIDataManager.SubAppType)
                    {
                        case GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                            if (isPageSearch == true)
                            {
                                sQry = "0GetSearchBudgetsByPage";
                            }
                            else
                            {
                                sQry = "0GetSearchBudgetChildren";
                            }
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                            if (isPageSearch == true)
                            {
                                sQry = "0GetSearchInvestmentsByPage";
                            }
                            else
                            {
                                sQry = "0GetSearchInvestmentChildren";
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.devpacks:
                    if (isPageSearch == true)
                    {
                        sQry = "0GetSearchDevPacksByPage";
                    }
                    else
                    {
                        sQry = "0GetSearchDevPackChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.linkedviews:
                    if (isPageSearch == true)
                    {
                        sQry = "0GetSearchLinkedViewsByPage";
                    }
                    else
                    {
                        sQry = "0GetSearchLinkedViewChildren";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.resources:
                    if (isPageSearch == true)
                    {
                        sQry = "0GetSearchResourcesByPage";
                    }
                    else
                    {
                        sQry = "0GetSearchResourceChildren";
                    }
                    break;
                default:
                    //don't render any ui
                    break;
            }
            return sQry;
        }
        private static string GetSearchByServiceQry(ContentURI uri)
        {
            string sQry = string.Empty;
            switch (uri.URIDataManager.AppType)
            {
                case GeneralHelpers.APPLICATION_TYPES.accounts:
                    sQry = "0GetSearchAccounts";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.members:
                    sQry = "0GetSearchMembersByService";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.networks:
                    sQry = "0GetSearchNetworksByService";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.locals:
                    sQry = "0GetSearchLinkedViewsByService";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.addins:
                    sQry = "0GetSearchLinkedViewsByService";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.agreements:
                    sQry = "0GetSearchServicesByService";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.prices:
                    switch (uri.URIDataManager.SubAppType)
                    {
                        case GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                            sQry = "0GetSearchInputsByService";
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                            sQry = "0GetSearchOutputsByService";
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                            sQry = "0GetSearchOutcomesByService";
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                            sQry = "0GetSearchOperationsByService";
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                            sQry = "0GetSearchComponentsByService";
                            break;
                        default:
                            break;
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.economics1:
                    switch (uri.URIDataManager.SubAppType)
                    {
                        case GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                            sQry = "0GetSearchBudgetsByService";
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                            sQry = "0GetSearchInvestmentsByService";
                            break;
                        default:
                            break;
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.devpacks:
                    sQry = "0GetSearchDevPacksByService";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.linkedviews:
                    sQry = "0GetSearchLinkedViewsByService";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.resources:
                    sQry = "0GetSearchResourcesByService";
                    break;
                default:
                    //don't render any ui
                    break;
            }
            return sQry;
        }
        //public static string GetQry(ContentURI uri,
        //    bool isPageSearch)
        //{
        //    string sQry = string.Empty;
        //    switch (uri.URIDataManager.AppType)
        //    {
        //        case GeneralHelpers.APPLICATION_TYPES.accounts:
        //            if (isPageSearch == true)
        //            {
        //                sQry = "0GetSearchAccountsByPage";
        //            }
        //            else
        //            {
        //                sQry = "0GetSearchAccountChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.members:
        //            if (isPageSearch == true)
        //            {
        //                sQry = "0GetSearchToResourceByPage";
        //            }
        //            else
        //            {
        //                sQry = "0GetSearchMemberChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.networks:
        //            if (isPageSearch == true)
        //            {
        //                sQry = "0GetSearchNetworkByPage";
        //            }
        //            else
        //            {
        //                sQry = "0GetSearchNetworkChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.locals:
        //            //no page search
        //            if (isPageSearch == false)
        //            {
        //                sQry = "0GetSearchLocalChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.addins:
        //            //no page search
        //            if (isPageSearch == false)
        //            {
        //                sQry = "0GetSearchAddInChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.agreements:
        //            if (isPageSearch == true)
        //            {
        //                sQry = "0GetSearchServiceByPage";
        //            }
        //            else
        //            {
        //                sQry = "0GetSearchServiceChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.prices:
        //            switch (uri.URIDataManager.SubAppType)
        //            {
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
        //                    if (isPageSearch == true)
        //                    {
        //                        sQry = "0GetSearchInputByPage";
        //                    }
        //                    else
        //                    {
        //                        sQry = "0GetSearchInputChildren";
        //                    }
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
        //                    if (isPageSearch == true)
        //                    {
        //                        sQry = "0GetSearchOutputByPage";
        //                    }
        //                    else
        //                    {
        //                        sQry = "0GetSearchOutputChildren";
        //                    }
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
        //                    if (isPageSearch == true)
        //                    {
        //                        sQry = "0GetSearchOutcomeByPage";
        //                    }
        //                    else
        //                    {
        //                        sQry = "0GetSearchOutcomeChildren";
        //                    }
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
        //                    if (isPageSearch == true)
        //                    {
        //                        sQry = "0GetSearchOperationByPage";
        //                    }
        //                    else
        //                    {
        //                        sQry = "0GetSearchOperationChildren";
        //                    }
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
        //                    if (isPageSearch == true)
        //                    {
        //                        sQry = "0GetSearchComponentByPage";
        //                    }
        //                    else
        //                    {
        //                        sQry = "0GetSearchComponentChildren";
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.economics1:
        //            switch (uri.URIDataManager.SubAppType)
        //            {
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
        //                    if (isPageSearch == true)
        //                    {
        //                        sQry = "0GetSearchBudgetsByPage";
        //                    }
        //                    else
        //                    {
        //                        sQry = "0GetSearchBudgetChildren";
        //                    }
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.investments:
        //                    if (isPageSearch == true)
        //                    {
        //                        sQry = "0GetSearchInvestmentsByPage";
        //                    }
        //                    else
        //                    {
        //                        sQry = "0GetSearchInvestmentChildren";
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.devpacks:
        //            if (isPageSearch == true)
        //            {
        //                sQry = "0GetSearchDevPackByPage";
        //            }
        //            else
        //            {
        //                sQry = "0GetSearchDevPackChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.linkedviews:
        //            if (isPageSearch == true)
        //            {
        //                sQry = "0GetSearchLinkedViewByPage";
        //            }
        //            else
        //            {
        //                sQry = "0GetSearchLinkedViewChildren";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.resources:
        //            if (isPageSearch == true)
        //            {
        //                sQry = "0GetSearchResourceByPage";
        //            }
        //            else
        //            {
        //                sQry = "0GetSearchResourceChildren";
        //            }
        //            break;
        //        default:
        //            //don't render any ui
        //            break;
        //    }
        //    return sQry;
        //}
        //private static string GetSearchByServiceQry(ContentURI uri)
        //{
        //    string sQry = string.Empty;
        //    switch (uri.URIDataManager.AppType)
        //    {
        //        case GeneralHelpers.APPLICATION_TYPES.accounts:
        //            sQry = "0GetSearchAccount";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.members:
        //            sQry = "0GetSearchToResourceByService";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.networks:
        //            sQry = "0GetSearchNetworkByService";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.locals:
        //            sQry = "0GetSearchLinkedViewByService";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.addins:
        //            sQry = "0GetSearchLinkedViewByService";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.agreements:
        //            sQry = "0GetSearchServiceByService";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.prices:
        //            switch (uri.URIDataManager.SubAppType)
        //            {
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
        //                    sQry = "0GetSearchInputByService";
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
        //                    sQry = "0GetSearchOutputByService";
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
        //                    sQry = "0GetSearchOutcomeByService";
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
        //                    sQry = "0GetSearchOperationByService";
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
        //                    sQry = "0GetSearchComponentByService";
        //                    break;
        //                default:
        //                    break;
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.economics1:
        //            switch (uri.URIDataManager.SubAppType)
        //            {
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
        //                    sQry = "0GetSearchBudgetsByService";
        //                    break;
        //                case GeneralHelpers.SUBAPPLICATION_TYPES.investments:
        //                    sQry = "0GetSearchInvestmentsByService";
        //                    break;
        //                default:
        //                    break;
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.devpacks:
        //            sQry = "0GetSearchDevPackByService";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.linkedviews:
        //            sQry = "0GetSearchLinkedViewByService";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.resources:
        //            sQry = "0GetSearchResourceByService";
        //            break;
        //        default:
        //            //don't render any ui
        //            break;
        //    }
        //    return sQry;
        //}

        private static bool NeedsTypeIdSearchParam(Helpers.GeneralHelpers.APPLICATION_TYPES appType)
        {
            bool bNeedsTypeId = false;
            if (appType
                == GeneralHelpers.APPLICATION_TYPES.linkedviews
                || appType
                == GeneralHelpers.APPLICATION_TYPES.devpacks
                || appType
                == GeneralHelpers.APPLICATION_TYPES.resources
                || appType
                == GeneralHelpers.APPLICATION_TYPES.prices
                || appType
                == GeneralHelpers.APPLICATION_TYPES.economics1)
            {
                bNeedsTypeId = true;
            }
            return bNeedsTypeId;
        }
        public static void GetSearchResultNodeName(ContentURI uri,
            out string searcherResultNodeName, out string parentNodeName)
        {
            searcherResultNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
            parentNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString();
            switch (uri.URIDataManager.AppType)
            {
                case GeneralHelpers.APPLICATION_TYPES.accounts:
                    searcherResultNodeName = AppHelpers.Accounts.ACCOUNT_TYPES.account.ToString();
                    parentNodeName = AppHelpers.Accounts.ACCOUNT_TYPES.accountgroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.members:
                    searcherResultNodeName = AppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString();
                    parentNodeName = AppHelpers.Members.MEMBER_BASE_TYPES.memberbasegroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.networks:
                    searcherResultNodeName = AppHelpers.Networks.NETWORK_BASE_TYPES.networkbase.ToString();
                    parentNodeName = AppHelpers.Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.agreements:
                    searcherResultNodeName = AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString();
                    parentNodeName = AppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.locals:
                    searcherResultNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    parentNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.addins:
                    searcherResultNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    parentNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.devpacks:
                    searcherResultNodeName = AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString();
                    parentNodeName = AppHelpers.DevPacks.DEVPACKS_TYPES.devpackgroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.linkedviews:
                    searcherResultNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    parentNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewgroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.resources:
                    searcherResultNodeName = AppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString();
                    parentNodeName = AppHelpers.Resources.RESOURCES_TYPES.resourcegroup.ToString();
                    break;
                case GeneralHelpers.APPLICATION_TYPES.prices:
                    switch (uri.URIDataManager.SubAppType)
                    {
                        case GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                            searcherResultNodeName = AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString();
                            parentNodeName = AppHelpers.Prices.INPUT_PRICE_TYPES.inputgroup.ToString();
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                            searcherResultNodeName = AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString();
                            parentNodeName = AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                            searcherResultNodeName = AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString();
                            parentNodeName = AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString();
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                            searcherResultNodeName = AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString();
                            parentNodeName = AppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString();
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                            searcherResultNodeName = AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString();
                            parentNodeName = AppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString();
                            break;
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.economics1:
                    switch (uri.URIDataManager.SubAppType)
                    {
                        case GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                            searcherResultNodeName = AppHelpers.Economics1.BUDGET_TYPES.budget.ToString();
                            parentNodeName = AppHelpers.Economics1.BUDGET_TYPES.budgetgroup.ToString();
                            break;
                        case GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                            searcherResultNodeName = AppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString();
                            parentNodeName = AppHelpers.Economics1.INVESTMENT_TYPES.investmentgroup.ToString();
                            break;
                    }
                    break;
                default:
                    //don't render any ui
                    break;
            }
        }

        
        public static void AddServiceGroupToList(IList<ServiceClass> colServiceGroups,
            GeneralHelpers.SUBAPPLICATION_TYPES subAppType)
        {
            //these are not economics services (they aren't linked to table Service)
            //so add them manually
            ServiceClass serviceGroup = new ServiceClass();
            switch (subAppType)
            {
                case GeneralHelpers.SUBAPPLICATION_TYPES.clubs:
                    serviceGroup.PKId = (int)GeneralHelpers.SUBAPPLICATION_TYPES.clubs;
                    serviceGroup.ServiceClassName = "Clubs";
                    serviceGroup.ServiceClassNum = GeneralHelpers.SUBAPPLICATION_TYPES.clubs.ToString();
                        // AppHelpers.Accounts.ACCOUNT_TYPES.account.ToString();
                    serviceGroup.IsSelected = false;
                    break;
                case GeneralHelpers.SUBAPPLICATION_TYPES.members:
                    serviceGroup.PKId = (int)GeneralHelpers.SUBAPPLICATION_TYPES.members;
                    serviceGroup.ServiceClassName = "Members";
                    serviceGroup.ServiceClassNum = GeneralHelpers.SUBAPPLICATION_TYPES.members.ToString();
                        //AppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString();
                    serviceGroup.IsSelected = false;
                    break;
                case GeneralHelpers.SUBAPPLICATION_TYPES.agreements:
                    serviceGroup.PKId = (int)GeneralHelpers.SUBAPPLICATION_TYPES.agreements;
                    serviceGroup.ServiceClassName = "Services";
                    serviceGroup.ServiceClassNum = GeneralHelpers.SUBAPPLICATION_TYPES.agreements.ToString();
                        //AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString();
                    serviceGroup.IsSelected = false;
                    break;
                case GeneralHelpers.SUBAPPLICATION_TYPES.networks:
                    serviceGroup.PKId = (int)GeneralHelpers.SUBAPPLICATION_TYPES.networks;
                    serviceGroup.ServiceClassName = "Networks";
                    serviceGroup.ServiceClassNum = GeneralHelpers.SUBAPPLICATION_TYPES.networks.ToString();
                        AppHelpers.Networks.NETWORK_BASE_TYPES.networkbase.ToString();
                    serviceGroup.IsSelected = false;
                    break;
                case GeneralHelpers.SUBAPPLICATION_TYPES.locals:
                    serviceGroup.PKId = (int)GeneralHelpers.SUBAPPLICATION_TYPES.locals;
                    serviceGroup.ServiceClassName = "Locals";
                    serviceGroup.ServiceClassNum = GeneralHelpers.SUBAPPLICATION_TYPES.locals.ToString();
                        //AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    serviceGroup.IsSelected = false;
                    break;
                case GeneralHelpers.SUBAPPLICATION_TYPES.addins:
                    serviceGroup.PKId = (int)GeneralHelpers.SUBAPPLICATION_TYPES.addins;
                    serviceGroup.ServiceClassName = "AddIns";
                    serviceGroup.ServiceClassNum = GeneralHelpers.SUBAPPLICATION_TYPES.addins.ToString();
                        //AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    serviceGroup.IsSelected = false;
                    break;
                default:
                    break;
            }
            colServiceGroups.Add(serviceGroup);
        }
        public static List<ContentURI> FillSearchList(
            ContentURI searchURI, SqlDataReader searchReaderResults)
        {
            List<ContentURI> colSearchResults = new List<ContentURI>();
            if (searchReaderResults != null)
            {
                //build a contenturi list to return to the client
                string sSearchResultNodeName = string.Empty;
                string sParentNodeName = string.Empty;
                GetSearchResultNodeName(searchURI,
                    out sSearchResultNodeName, out sParentNodeName);
                bool bIsAdminApp
                    = Helpers.GeneralHelpers.IsAdminApp(searchURI.URIDataManager.AppType);
                using (searchReaderResults)
                {
                    while (searchReaderResults.Read())
                    {
                        if (bIsAdminApp)
                        {
                            colSearchResults.Add(ContentURI.GetContentURI
                            (
                                searchURI
                                //id
                                ,searchReaderResults.GetInt32(0)
                                //label
                                , ""
                                //name
                                , searchReaderResults.GetString(1)
                                //description
                                , searchReaderResults.GetString(2)
                                //file extension type
                                , string.Empty
                                //connection
                                , searchURI.URINetwork.WebConnection
                                //resources
                                , ""
                                //resource alt
                                , ""
                                //parentURIPattern
                                , searchReaderResults.GetString(3)
                                //docstatus
                                , Helpers.GeneralHelpers.DOCS_STATUS.approved
                                , searchURI.URINetworkPartName
                                , sSearchResultNodeName
                                , searchURI.URIDataManager.DefaultRootWebStoragePath
                             ));
                        }
                        else
                        {
                            colSearchResults.Add(ContentURI.GetContentURI
                            (
                                searchURI
                                //id
                                , searchReaderResults.GetInt32(0)
                                //label
                                , searchReaderResults.GetString(2)
                                //name
                                , searchReaderResults.GetString(1)
                                //description
                                , searchReaderResults.GetString(3)
                                //file extension type
                                , string.Empty
                                //connection
                                , searchURI.URINetwork.WebConnection
                                //media resource uri to display
                                , searchReaderResults.GetString(7)
                                //media resource alt to display
                                , searchReaderResults.GetString(8)
                                //parentURIPattern
                                , searchReaderResults.GetString(4)
                                //docStatus
                                , Helpers.GeneralHelpers.GetDocStatus(searchReaderResults.GetInt16(5))
                                //networkId
                                , searchReaderResults.GetString(9).ToString()
                                , sSearchResultNodeName
                                , searchURI.URIDataManager.DefaultRootWebStoragePath
                            ));
                        }
                    }
                }
            }
            return colSearchResults;
        }
        
    }
}

