using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for service agreements
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    /// NOTE:       
	public class Agreement
	{
        public Agreement()
        {
        }
        //switch to apps
        public const string SERVICE_BASE_QRY = "servicebase";
        //base tables inserts/edits schema
        public const string AGREEMENTSBASE_SCHEMA = "ServiceAgreementBaseEdits.xml";
        //namespaces
        public const string AGREEMENTS_NAMESPACE = "'urn:DevTreks-support-schemas:ServiceAgreement'";

        //attribute names for edits
        private const string ISOWNER = "IsOwner";
        public const string SERVICE_ID = "ServiceId";
        public const string SERVICEGROUP_ID = "ServiceGroupId";

        //serviceaccountgroup is accountclass -needed to that can navigate to other networks in selects panel
        //serviceaccount is account
        //service is accounttoservice
        //incentive is accounttoincentive
		public enum AGREEMENT_TYPES 
		{
            serviceaccountgroup     = 1,
            serviceaccount          = 2,
			service				    = 3,
			incentive			    = 4
		}
        //xml elements found in base table schemas (i.e. servicebase in all apps)
        public enum AGREEMENT_BASE_TYPES
        {
            //phase out accountbase
            servicebasegroup        = 1,
            servicebase             = 2,
            incentivebase           = 3
        }
        public enum SERVICE_STATUS_TYPES
        {
            notcurrent  = 0,
            current     = 1
        }
        public enum PUBLIC_AUTHORIZATION_TYPES
        {
            public_not_authorized   = 0,
            public_is_authorized    = 1
        }
        public enum SERVICE_UNIT_TYPES
        {
            day     = 0,
            month   = 1,
            year    = 2
        }
        public enum SERVICE_CURRENCY_TYPES
        {
            usdollar    = 0,
            euro        = 1
        }
        public static Dictionary<string, string> GetAuthorizationLevelDictionary()
        {
            Dictionary<string, string> auths = new Dictionary<string, string>();
            int iValue = (int)PUBLIC_AUTHORIZATION_TYPES.public_not_authorized;
            auths.Add(iValue.ToString(), PUBLIC_AUTHORIZATION_TYPES.public_not_authorized.ToString());
            iValue = (int)PUBLIC_AUTHORIZATION_TYPES.public_is_authorized;
            auths.Add(iValue.ToString(), PUBLIC_AUTHORIZATION_TYPES.public_is_authorized.ToString());
            return auths;
        }
        public static Dictionary<string, string> GetServiceUnitDictionary()
        {
            Dictionary<string, string> units = new Dictionary<string, string>();
            units.Add(SERVICE_UNIT_TYPES.day.ToString(), SERVICE_UNIT_TYPES.day.ToString());
            units.Add(SERVICE_UNIT_TYPES.month.ToString(), SERVICE_UNIT_TYPES.month.ToString());
            units.Add(SERVICE_UNIT_TYPES.year.ToString(), SERVICE_UNIT_TYPES.year.ToString());
            return units;
        }
        public static Dictionary<string, string> GetCurrencyDictionary()
        {
            Dictionary<string, string> curs = new Dictionary<string, string>();
            curs.Add(SERVICE_CURRENCY_TYPES.usdollar.ToString(), SERVICE_CURRENCY_TYPES.usdollar.ToString());
            curs.Add(SERVICE_CURRENCY_TYPES.euro.ToString(), SERVICE_CURRENCY_TYPES.euro.ToString());
            return curs;
        }
        
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            //the current params change depending on the node type
            if (currentNodeName == AGREEMENT_TYPES.serviceaccount.ToString()
                || currentNodeName == AGREEMENT_TYPES.serviceaccountgroup.ToString())
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
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (currentNodeName == AGREEMENT_TYPES.serviceaccountgroup.ToString())
                {
                    uri.URIDataManager.ChildrenNodeName = AGREEMENT_TYPES.serviceaccount.ToString();
                    //no link backwards
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = AGREEMENT_TYPES.service.ToString();
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == AGREEMENT_TYPES.service.ToString())
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
                uri.URIDataManager.ChildrenNodeName = AGREEMENT_BASE_TYPES.servicebase.ToString();
            }
            else if (currentNodeName == AGREEMENT_BASE_TYPES.servicebase.ToString())
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
                //can't navigate to incentives (edits made in service agreements)
                uri.URIDataManager.ChildrenNodeName = string.Empty;
            }
        }
        
        public static void GetChildForeignKeyNames(string parentNodeName,
            out string childForeignKeyName, out string baseForeignKeyName)
        {
            childForeignKeyName = string.Empty;
            baseForeignKeyName = string.Empty;
            if (parentNodeName == AGREEMENT_TYPES.serviceaccountgroup.ToString())
            {
                baseForeignKeyName = "AccountId";
                childForeignKeyName = "AccountId";
            }
            else if (parentNodeName == AGREEMENT_BASE_TYPES.servicebase.ToString()
                || parentNodeName == AGREEMENT_TYPES.service.ToString())
            {
                baseForeignKeyName = Agreement.SERVICE_ID;
                childForeignKeyName = "AccountToServiceId";
            }
            else if (parentNodeName == AGREEMENT_BASE_TYPES.incentivebase.ToString()
                || parentNodeName == AGREEMENT_TYPES.incentive.ToString())
            {
                baseForeignKeyName = "IncentiveId";
                childForeignKeyName = string.Empty;
            }
        }
        public static string GetBaseTableParentForeignKeyName(string currentNodeName)
        {
            string sKeyName = string.Empty;
            if (currentNodeName == AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == AGREEMENT_TYPES.service.ToString())
            {
                sKeyName = "ServiceGroupId";
            }
            else if (currentNodeName == AGREEMENT_BASE_TYPES.incentivebase.ToString()
                || currentNodeName == AGREEMENT_TYPES.incentive.ToString())
            {
                sKeyName = "IncentiveGroupId";
            }
            return sKeyName;
        }
        public async Task<List<ServiceClass>> GetServiceGroupsAsync(ContentURI uri)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader serviceGroups
                = await sqlIO.RunProcAsync("0GetServiceGroups");
            List<ServiceClass> colServiceGroups = new List<ServiceClass>();
            if (serviceGroups != null)
            {
                using(serviceGroups)
                {
                    //build a related service list to return to the client
                    while (await serviceGroups.ReadAsync())
                    {
                        ServiceClass newServiceGroup = new ServiceClass();
                        newServiceGroup.PKId = serviceGroups.GetInt32(0);
                        newServiceGroup.ServiceClassNum = serviceGroups.GetString(1);
                        newServiceGroup.ServiceClassName = serviceGroups.GetString(2);
                        newServiceGroup.ServiceClassDesc = serviceGroups.GetString(3);
                        newServiceGroup.Service = new List<Service>();
                        //nondb
                        newServiceGroup.IsSelected = false;
                        colServiceGroups.Add(newServiceGroup);
                    }
                }
            }
            sqlIO.Dispose();
            return colServiceGroups;
        }
        public async Task<List<AccountToService>> GetServiceAsync(SearchManager searcher)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(searcher.SearchResult);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@AccountId", SqlDbType.Int, 4, searcher.SearchResult.URIMember.ClubInUse.PKId),
                sqlIO.MakeInParam("@NetworkId", SqlDbType.Int, 4, searcher.SearchResult.URINetwork.PKId),
				sqlIO.MakeInParam("@NetworkType", SqlDbType.NVarChar, 25, searcher.NetworkType.ToString()),
                sqlIO.MakeInParam("@ServiceGroupId", SqlDbType.Int, 4, searcher.ServiceGroupSelected.PKId)
			};
            SqlDataReader services 
                = await sqlIO.RunProcAsync("0GetServices", colPrams);
            List<AccountToService> colService = FillClubServiceList(services);
            sqlIO.Dispose();
            return colService;
        }
        public async Task<List<AccountToService>> GetServiceByClubIdAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToService> colClubService = new List<AccountToService>();
            if (accountId == 0)
            {
                //set default objects
                AccountToService atos = new AccountToService(true);
                colClubService.Add(atos);
                return colClubService;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@AccountId", SqlDbType.Int, 4, accountId)
			};
            //this sp must also run a subquery that sets the owning club id
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                 "0GetServicesByClubId", colPrams);
            colClubService = FillClubServiceList(dataReader);
            sqlIO.Dispose();
            return colClubService;
        }
        public async Task<List<AccountToService>> GetServiceByServiceIdAsync(
            ContentURI uri, int serviceId, bool isOwner)
        {
            List<AccountToService> colClubService = new List<AccountToService>();
            if (serviceId == 0)
            {
                //set default objects
                AccountToService atos = new AccountToService(true);
                colClubService.Add(atos);
                return colClubService;
            }
            int iIsOwner = (isOwner) ? 1 : 0;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@ServiceId", SqlDbType.Int, 4, serviceId),
                sqlIO.MakeInParam("@IsOwner", SqlDbType.Bit, 1, iIsOwner)
			};
            //this sp must also run a subquery that sets the owning club id
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                 "0GetServicesByServiceId", colPrams);
            colClubService = FillClubServiceList(dataReader);
            sqlIO.Dispose();
            return colClubService;
        }
        public List<AccountToService> FillClubServiceList(SqlDataReader services)
        {
            List<AccountToService> colService = new List<AccountToService>();
            if (services != null)
            {
                using (services)
                {
                    //build a related service list to return to the client
                    while (services.Read())
                    {
                        AccountToService newService = new AccountToService();
                        newService.Service = new Service();
                       
                        newService.PKId = services.GetInt32(0);
                        newService.Name = services.GetString(1);
                        newService.Amount1 = services.GetInt32(2);
                        newService.Status = services.GetString(3);
                        newService.LastChangedDate = services.GetDateTime(4);
                        newService.AuthorizationLevel = services.GetInt16(5);
                        newService.StartDate = services.GetDateTime(6);
                        newService.EndDate = services.GetDateTime(7);
                        newService.LastChangedDate = services.GetDateTime(8);
                        newService.IsOwner = services.GetBoolean(9);
                        newService.AccountId = services.GetInt32(10);
                        newService.ServiceId = services.GetInt32(11);
                        //base table (service being subscribed to or owned)
                        newService.Service = new Service();
                        newService.Service.ServiceNum = services.GetString(12);
                        newService.Service.ServiceName = services.GetString(13);
                        newService.Service.ServiceDesc = services.GetString(14);
                        newService.Service.ServicePrice1 = services.GetDecimal(15);
                        newService.Service.ServiceUnit1 = services.GetString(16);
                        newService.Service.ServiceClassId = services.GetInt32(17);
                        newService.Service.NetworkId = services.GetInt32(18);
                        newService.OwningClubId = services.GetInt32(19);
                        newService.Service.ServiceCurrency1 = services.GetString(20);
                        newService.Account = new Account();
                        newService.Account.PKId = services.GetInt32(21);
                        newService.Account.AccountName = services.GetString(22);
                        newService.Account.AccountDesc = services.GetString(23);
                        newService.Account.AccountLongDesc = services.GetString(24);
                        newService.Account.AccountEmail = services.GetString(25);
                        newService.Account.AccountURI = services.GetString(26);
                        newService.Account.AccountClassId = services.GetInt32(27);
                        newService.Account.GeoRegionId = services.GetInt32(28);
                        //not in db
                        newService.Account.ClubDocFullPath = string.Empty;
                        newService.Account.PrivateAuthorizationLevel
                            = AccountHelper.AUTHORIZATION_LEVELS.none;
                        newService.Account.NetCost = 0;
                        newService.Account.TotalCost = 0;
                        newService.Account.URIFull = string.Empty;
                        colService.Add(newService);
                    }
                }
            }
            return colService;
        }

        public async Task<SqlDataReader> GetServiceAsync(
            Helpers.SqlIOAsync sqlIO, ContentURI uri, bool isBaseService)
        {
            int iServiceId = (isBaseService == true) ? uri.URIService.ServiceId : uri.URIService.PKId;
            SqlParameter[] colPrams = 
            { 
                sqlIO.MakeInParam("@Id", SqlDbType.Int, 4, iServiceId)
            };
            string sQry = (isBaseService == true) ? "0GetServiceBase" : "0GetService";
            SqlDataReader dataReader = await sqlIO.RunProcAsync(sQry, colPrams);
            return dataReader;
        }
        
        public async Task<bool> ChangeApplicationAndServiceAsync(ContentURI uri, 
            int serviceId, bool needsNewApp)
        {
            bool bIsBaseService = false;
            uri.URIService.PKId = serviceId;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader service = await GetServiceAsync(sqlIO, uri, bIsBaseService);
            if (service != null)
            {
                //set the uri's service object
                uri.URIService = FillServiceObject(service);
            }
            sqlIO.Dispose();
            if (needsNewApp == true)
            {
                if (uri.URIService.Service != null)
                {
                    if (uri.URIService.Service.ServiceClassId != 0)
                    {
                        Helpers.GeneralHelpers.SetAppTypes(uri.URIService.Service.ServiceClassId, uri);
                    }
                }
            }
            return bIsBaseService;
        }
        public async Task<bool> ChangeApplicationAndServiceFromBaseServiceAsync(
            ContentURI uri)
        {
            bool bIsBaseService = true;
            uri.URIService.ServiceId = uri.URIId;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader service = await GetServiceAsync(sqlIO, uri, bIsBaseService);
            if (service != null)
            {
                //set the uri's service object
                uri.URIService = FillServiceObject(service);
            }
            sqlIO.Dispose();
            Helpers.GeneralHelpers.SetAppTypes(uri.URIService.Service.ServiceClassId, uri);
            return bIsBaseService;
        }
        public AccountToService FillServiceObject(SqlDataReader service)
        {
            //this is a one-record reader
            int i = 0;
            //avoid null refs with object props
            AccountToService newService = new AccountToService(true);
            if (service != null)
            {
                using(service)
                {
                    while (service.Read())
                    {
                        if (i == 0)
                        {
                            newService.PKId = service.GetInt32(0);
                            newService.Name = service.GetString(1);
                            newService.Amount1 = service.GetInt32(2);
                            newService.Status = service.GetString(3);
                            newService.StatusDate = service.GetDateTime(4);
                            newService.AuthorizationLevel = service.GetInt16(5);
                            newService.StartDate = service.GetDateTime(6);
                            newService.EndDate = service.GetDateTime(7);
                            newService.LastChangedDate = service.GetDateTime(8);
                            newService.IsOwner = service.GetBoolean(9);
                            newService.AccountId = service.GetInt32(10);
                            newService.ServiceId = service.GetInt32(11);
                            //base table (service being subscribed to or owned)
                            newService.Service = new Service();
                            newService.Service.PKId = newService.ServiceId;
                            newService.Service.ServiceNum = service.GetString(12);
                            newService.Service.ServiceName = service.GetString(13);
                            newService.Service.ServiceDesc = service.GetString(14);
                            newService.Service.ServicePrice1 = service.GetDecimal(15);
                            newService.Service.ServiceUnit1 = service.GetString(16);
                            newService.Service.ServiceClassId = service.GetInt32(17);
                            //must switch uripatterns from now on to this networkid (on service layer)
                            newService.Service.NetworkId = service.GetInt32(18);
                            newService.OwningClubId = service.GetInt32(19);
                        }
                        i++;
                    }
                }
            }
            return newService;
        }
        public static SERVICE_STATUS_TYPES GetStatusType(string statusType)
        {
            //(AppHelpers.SERVICE_STATUS_TYPES)Enum.Parse(typeof(AppHelpers.SERVICE_STATUS_TYPES), status);
            SERVICE_STATUS_TYPES eStatusType = SERVICE_STATUS_TYPES.notcurrent;
            if (statusType == SERVICE_STATUS_TYPES.current.ToString())
            {
                eStatusType = SERVICE_STATUS_TYPES.current;
            }
            return eStatusType;
        }
        public async Task<string> GetAncestorsAndSetServiceAsync(ContentURI uri)
        {
            string ancestorArray = string.Empty;
            int iId = uri.URIId;
            string sNodeName = uri.URINodeName;
            if (uri.URINodeName == AGREEMENT_TYPES.service.ToString())
            {
                //same rule enforced with contenthelper.geturichildren
                //only show the ancestors and children of owned services, not subscribed
                if (uri.URIService != null)
                {
                    if (uri.URIService.ServiceId != 0)
                    {
                        iId = uri.URIService.ServiceId;
                        sNodeName = AGREEMENT_BASE_TYPES.servicebase.ToString();
                    }
                }
            }
            //commontreks 
            string sAdminNetworkPartName
                 = Data.Helpers.GeneralHelpers.GetDefaultNetworkPartName();
            //service object can be filled in from the same stored procedure
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
		    {
			    sqlIO.MakeInParam("@Id",				    SqlDbType.Int, 4, iId),
                sqlIO.MakeInParam("@NodeName",		        SqlDbType.NVarChar, 50, sNodeName),
                sqlIO.MakeInParam("@NetworkPartName",	    SqlDbType.NVarChar, 20, uri.URINetworkPartName),
                sqlIO.MakeInParam("@AdminNetworkPartName",	SqlDbType.NVarChar, 20, sAdminNetworkPartName),
                sqlIO.MakeInParam("@ParamDelimiter",	    SqlDbType.NVarChar, 2,  Helpers.GeneralHelpers.PARAMETER_DELIMITER),
                sqlIO.MakeOutParam("@AncestorNameArray",    SqlDbType.NVarChar, 1000),
                sqlIO.MakeOutParam("@CurrentName",		    SqlDbType.NVarChar, 150),
                sqlIO.MakeOutParam("@ServiceId",		    SqlDbType.Int, 8),
		    };
            int iNotUsed = await sqlIO.RunProcIntAsync( 
                "0GetAncestorNamesAgreement", oPrams);
            if (oPrams[5].Value != System.DBNull.Value)
            {
                ancestorArray = oPrams[5].Value.ToString();
            }
            if (oPrams[6].Value != System.DBNull.Value)
            {
                //always use the name stored in db, not the name passed in the search url
                //if needed, fix the uri's name
                string sCurrentNameFromClient = uri.URIName;
                string sNewName = oPrams[6].Value.ToString();
                RuleHelpers.ResourceRules.ValidateScriptArgument(ref sNewName);
                uri.URIName = sNewName;
                //make sure this is the same name sent by the client (i.e. no edit*)
                if (sCurrentNameFromClient.Equals(uri.URIName) == false)
                {
                    uri.URIPattern = uri.URIPattern.Replace(sCurrentNameFromClient,
                        uri.URIName);
                }
            }
            if (oPrams[7].Value != System.DBNull.Value)
            {
                //only applicable for service nodes 
                //(supports switching between agreements and app groups)
                int iServiceId = (int) oPrams[7].Value;
                if (uri.URIService.ServiceId != iServiceId)
                {
                    uri.URIService.ServiceId = iServiceId;
                    bool bIsBaseService = true;
                    //data readers always close sqlio connections, use a new connection
                    Helpers.SqlIOAsync sqlIO2 = new Helpers.SqlIOAsync(uri);
                    SqlDataReader services = await GetServiceAsync(sqlIO2, uri, bIsBaseService);
                    uri.URIService = FillServiceObject(services);
                    sqlIO2.Dispose();
                }
            }
            sqlIO.Dispose();
            return ancestorArray;
        }
        public async Task<string> GetAncestorsAndAuthorizationsAsync(
            ContentURI uri, int clubOrMemberId)
        {
            Dictionary<string, int> ancestors = new Dictionary<string,int>();
            string ancestorArray = string.Empty;
            int iAuthorizationLevel = 0;
            
            string sAdminNetworkPartName 
                = Data.Helpers.GeneralHelpers.GetDefaultNetworkPartName();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
		    {
			    sqlIO.MakeInParam("@AccountId",		        SqlDbType.Int, 4, clubOrMemberId),
			    sqlIO.MakeInParam("@ServiceId",		        SqlDbType.Int, 4, uri.URIService.ServiceId),
                sqlIO.MakeInParam("@NetworkPartName",	    SqlDbType.NVarChar, 20, uri.URINetworkPartName),
                sqlIO.MakeInParam("@AdminNetworkPartName",	SqlDbType.NVarChar, 20, sAdminNetworkPartName),
                sqlIO.MakeInParam("@ParamDelimiter",	    SqlDbType.NVarChar, 2, Helpers.GeneralHelpers.PARAMETER_DELIMITER),
                sqlIO.MakeOutParam("@AncestorNameArray",    SqlDbType.NVarChar, 1000),
                sqlIO.MakeOutParam("@AuthorizationLevel",   SqlDbType.SmallInt, 2)
		    };
            string sQryName = "0GetAncestorNamesAgreementByServiceId";
            int iNotUsed = await sqlIO.RunProcIntAsync(sQryName, oPrams);
            if (oPrams[5].Value != System.DBNull.Value)
            {
                ancestorArray = oPrams[5].Value.ToString();
            }
            if (oPrams[6].Value != System.DBNull.Value)
            {
                AccountHelper.AUTHORIZATION_LEVELS publicOrPrivateAL
                    = AccountHelper.AUTHORIZATION_LEVELS.viewonly;
                iAuthorizationLevel 
                    = Helpers.GeneralHelpers.ConvertStringToInt(oPrams[6].Value.ToString());
                publicOrPrivateAL = AccountHelper.GetAuthorizationLevel(iAuthorizationLevel);
                //new in 1.5.2
                if (uri.URIMember == null)
                    uri.URIMember = new AccountToMember(true);
                if (uri.URIMember.ClubInUse == null)
                   uri.URIMember.ClubInUse = new Account(true);
                //set what the db says is their authorization level
                uri.URIMember.ClubInUse.PrivateAuthorizationLevel = publicOrPrivateAL;
            }
            sqlIO.Dispose();
            return ancestorArray;
        }
        public async Task<string> GetBaseTableGroupIdFromBaseTableIdAync(
            ContentURI uri, string connect, string baseTableId, string currentNodeName)
        {
            string sBaseTableGroupId = string.Empty;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@Id",                    SqlDbType.Int, 4, baseTableId),
				sqlIO.MakeInParam("@NodeName",              SqlDbType.NVarChar, 25, currentNodeName),
                sqlIO.MakeOutParam("@BaseTableGroupId",     SqlDbType.Int, 4)
			};
            string sQryName = "0GetAgreementBaseTableGroupId";
            int iNotUsed = await sqlIO.RunProcIntAsync(sQryName, colPrams);
            if (colPrams[2].Value != System.DBNull.Value)
            {
                sBaseTableGroupId = colPrams[2].Value.ToString();
            }
            sqlIO.Dispose();
            return sBaseTableGroupId;
        }
   
        public static string GetDeleteBaseandJoinQry()
        {
            return "0DeleteAgreements";
        }
        public static void ChangeAttributesForInsertion(
            EditHelpers.EditHelper.ArgumentsEdits addsArguments,
            ContentURI selectedURI, XElement selectedElement)
        {
            //before inserting service nodes, change app-specfic attributes
            if (selectedURI.URINodeName 
                == AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //accountid is always currently logged-in user (for all db-inserted contracts)
                selectedElement.SetAttributeValue(General.ACCOUNTID,
                    addsArguments.URIToEdit.URIMember.ClubInUse.PKId.ToString());
                if (addsArguments.URIToEdit.URIDataManager.ServerSubActionType
                    != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                {
                    //selections always false - they are not owned
                    selectedElement.SetAttributeValue(ISOWNER, "0");
                    int iAuthorizationLevel
                        = (int)AccountHelper.AUTHORIZATION_LEVELS.viewonly;
                    //selections start as view only
                    selectedElement.SetAttributeValue(Members.AUTHORIZATION_LEVEL,
                        iAuthorizationLevel.ToString());
                }
                else
                {
                    //default insertions are owned
                    selectedElement.SetAttributeValue(ISOWNER, "1");
                    int iAuthorizationLevel
                        = (int)AccountHelper.AUTHORIZATION_LEVELS.viewonly;
                    //public can view but not edit
                    selectedElement.SetAttributeValue(Members.AUTHORIZATION_LEVEL,
                        iAuthorizationLevel.ToString());
                }
            }
        }
        public async Task<bool> UpdateSubscribedMemberCountAsync(ContentURI uri,
            int currentClubMemberCount, int accountId, int joinServiceId, int memberCount)
        {
            bool bHasUpdated = false;
            IMemberRepositoryEF memberReposit 
                = new SqlRepositories.MemberRepository(uri);
            bHasUpdated = await memberReposit.UpdateSubscribedMemberCountAsync(uri, currentClubMemberCount, 
                accountId, joinServiceId, memberCount);
            return bHasUpdated;
        }
        public async Task<List<SearchManager.SearchType>> GetTypesAsync(
           ContentURI uri, int networkId, int serviceGroupId)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@NetworkId", SqlDbType.Int, 4, networkId),
				sqlIO.MakeInParam("@ServiceGroupId", SqlDbType.Int, 4, serviceGroupId)
			};
            SqlDataReader categories = await sqlIO.RunProcAsync(
                "0GetCategories", colPrams);
            List<SearchManager.SearchType> colSearchTypes
                = new List<SearchManager.SearchType>();
            if (categories != null)
            {
                using(categories)
                {
                    //categories return id, label, name, servicegroupid and networkid sorted by label
                    //contenturi not strictly needed
                    while (categories.Read())
                    {
                        SearchManager.SearchType searchType = new SearchManager.SearchType();
                        searchType.Id = categories.GetInt32(0);
                        searchType.Label = categories.GetString(1);
                        searchType.Name = categories.GetString(2);
                        searchType.NetworkId = categories.GetInt32(3);
                        searchType.ServiceClassId = categories.GetInt32(4);
                        colSearchTypes.Add(searchType);
                    }
                }
            }
            sqlIO.Dispose();
            return colSearchTypes;
        }
        public async Task<List<ContentURI>> GetNetworkCategoriesAsync(
            ContentURI serviceURI)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(serviceURI);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@NetworkId", SqlDbType.Int, 4, serviceURI.URIService.Service.NetworkId),
				sqlIO.MakeInParam("@ServiceGroupId", SqlDbType.Int, 4, serviceURI.URIService.Service.ServiceClassId)
			};
            SqlDataReader categories = await sqlIO.RunProcAsync("0GetCategories", 
                colPrams);
            List<ContentURI> colCategories = new List<ContentURI>();
            string sTypeNodeName = Helpers.GeneralHelpers.GetCategoryNodeName(serviceURI.URIService.Service.ServiceClassId);
            if (categories != null)
            {
                //categories return id, label, name, servicegroupid and networkid sorted by label
                //contenturi not strictly needed
                while (categories.Read())
                {
                    ContentURI category = new ContentURI(
                        categories.GetString(2), categories.GetInt32(0),
                        serviceURI.URINetworkPartName, sTypeNodeName, string.Empty);
                    category.URIDataManager.Label = categories.GetString(1);
                    if (category.URIService == null)
                        category.URIService = new AccountToService(true);
                    //networkid and servicegroupid can be 0 if it's a static DevTreks-wide category
                    category.URIService.Service.NetworkId = categories.GetInt32(3);
                    category.URIService.Service.ServiceClassId = categories.GetInt32(4);
                    colCategories.Add(category);
                }
            }
            sqlIO.Dispose();
            return colCategories;
        }
        public async Task<bool> AddNetworkCategoriesAsync(ContentURI uri, 
            int serviceGroupId, int networkId, int numberToAdd)
        {
            bool bIsOkToSave = false;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@NetworkId", SqlDbType.Int, 4, networkId),
				sqlIO.MakeInParam("@ServiceGroupId", SqlDbType.Int, 4, serviceGroupId),
                sqlIO.MakeInParam("@NumberToAdd", SqlDbType.Int, 4, numberToAdd)
			};
            string sQryName = "0InsertCategories";
            int iRowCount = await sqlIO.RunProcIntAsync(sQryName, colPrams);
            //no error means sp ran successfully
            if (iRowCount > 0)
                bIsOkToSave = true;
            sqlIO.Dispose();
            return bIsOkToSave;
        }
        
        
        public static string GetServiceGroupCategoryName(ContentURI uri)
        {
            string sTypeDocFileName = string.Empty;
            string sTypeName = Helpers.GeneralHelpers.GetCategoryNodeName(uri.URIService.Service.ServiceClassId);
            if (!string.IsNullOrEmpty(sTypeName))
            {
                sTypeDocFileName = string.Concat(sTypeName, Helpers.GeneralHelpers.EXTENSION_XML);
            }
            return sTypeDocFileName;
        }
        public static IList<ContentURI> MakeNetworkCategoryListForDisplay(
            ContentURI uri, string typeDocPath)
        {
            IList<ContentURI> categories = new List<ContentURI>();
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, typeDocPath))
            {
                XElement categoryRoot 
                    = Helpers.FileStorageIO.LoadXmlElement(uri, typeDocPath);
                if (categoryRoot != null)
                {
                    if (categoryRoot.HasElements)
                    {
                        foreach (XElement categoryEl in categoryRoot.Elements())
                        {
                            //need a minimal list for display purposes
                            ContentURI category = new ContentURI();
                            string sId = EditHelpers.XmlLinq.GetAttributeValue(
                                categoryEl, AppHelpers.Calculator.cId);
                            category.URIId = Helpers.GeneralHelpers.ConvertStringToInt(sId);
                            category.URIName = EditHelpers.XmlLinq.GetAttributeValue(
                                categoryEl, AppHelpers.Calculator.cName);
                            if (category.URIDataManager == null)
                                category.URIDataManager = new ContentURI.DataManager();
                            category.URIDataManager.Label = EditHelpers.XmlLinq.GetAttributeValue(
                                categoryEl, AppHelpers.Calculator.cLabel);
                            categories.Add(category);
                        }
                    }
                }
            }
            return categories;
        }
	}
}
