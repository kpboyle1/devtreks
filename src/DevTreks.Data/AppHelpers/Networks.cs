using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for networks 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    public class Networks
    {
        public Networks()
        {
        }
        //when a network can't be located
        private const string ALL_NETWORKS = "allnetworks";
        public const string NETWORK_NAMESPACE_NODE_QRY = "urn:DevTreks-support-schemas:Network";
        public const string NETWORK_ID = "NetworkId";
        public enum NETWORK_TYPES
        {
            //account to accounttonetwork view 
            //the networks belonging to a specific club
            networkaccountgroup         = 1,
            network                     = 2
        }
        public enum NETWORK_BASE_TYPES
        {
            //networkclass to network
            networkbasegroup    = 1,
            networkbase         = 2
        }
        public enum NETWORK_FILTER_TYPES
        {
            none                    = 0,
            //only search:
            //through services found in uri.URIDataManager.Service
            myagreement             = 1,
            //through individual networks in uri.URIDataManager.Network
            mynetworks              = 2,
            //through individual networks in current networkgroup
            allnetworksbygroup      = 3,
            //through all networks in all network groups
            allnetworks             = 4,
            //consider for possible future use
            //through commontreks network group only
            commontreksonly         = 5,
            //through all networks for a given culture 
            allnetworksbyculture    = 6
        }
        public enum NETWORK_STOREDATA_TYPES
        {
            web     = 0,
            network = 1,
            local   = 2
        }
        public static NETWORK_FILTER_TYPES GetNetworkFilter(string networkFilter)
        {
            NETWORK_FILTER_TYPES eNETWORKFILTER = NETWORK_FILTER_TYPES.none;
            if (networkFilter == NETWORK_FILTER_TYPES.allnetworks.ToString())
            {
                eNETWORKFILTER = NETWORK_FILTER_TYPES.allnetworks;
            }
            else if (networkFilter == NETWORK_FILTER_TYPES.allnetworksbyculture.ToString())
            {
                eNETWORKFILTER = NETWORK_FILTER_TYPES.allnetworksbyculture;
            }
            else if (networkFilter == NETWORK_FILTER_TYPES.allnetworksbygroup.ToString())
            {
                eNETWORKFILTER = NETWORK_FILTER_TYPES.allnetworksbygroup;
            }
            else if (networkFilter == NETWORK_FILTER_TYPES.commontreksonly.ToString())
            {
                eNETWORKFILTER = NETWORK_FILTER_TYPES.commontreksonly;
            }
            else if (networkFilter == NETWORK_FILTER_TYPES.myagreement.ToString())
            {
                eNETWORKFILTER = NETWORK_FILTER_TYPES.myagreement;
            }
            else if (networkFilter == NETWORK_FILTER_TYPES.mynetworks.ToString())
            {
                eNETWORKFILTER = NETWORK_FILTER_TYPES.mynetworks;
            }
            else 
            {
                eNETWORKFILTER = NETWORK_FILTER_TYPES.none;
            }
            return eNETWORKFILTER;
        }
        public static Dictionary<string, string> GetNetworkDictionary(List<AccountToNetwork> networks)
        {
            
            Dictionary<string, string> nets = new Dictionary<string, string>();
            //make them choose something
            nets.Add("0", Helpers.GeneralHelpers.NONE);
            if (networks != null)
            {
                foreach (var network in networks)
                {
                    if (network.Network != null)
                    {
                        //not the key becomes the options value on client
                        nets.Add(network.Network.PKId.ToString(), network.Network.NetworkName);
                    }
                }
            }
            return nets;
        }
        public static Dictionary<string, string> GetNetworkStoreDataDictionary()
        {
            Dictionary<string, string> roles = new Dictionary<string, string>();
            roles.Add(NETWORK_STOREDATA_TYPES.web.ToString(), NETWORK_STOREDATA_TYPES.web.ToString());
            roles.Add(NETWORK_STOREDATA_TYPES.network.ToString(), NETWORK_STOREDATA_TYPES.network.ToString());
            roles.Add(NETWORK_STOREDATA_TYPES.local.ToString(), NETWORK_STOREDATA_TYPES.local.ToString());
            return roles;
        }
        
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == NETWORK_BASE_TYPES.networkbasegroup.ToString())
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
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (uri.URIId == 0)
                {
                    //no link backwards (showing groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                    uri.URIDataManager.ChildrenNodeName = NETWORK_BASE_TYPES.networkbasegroup.ToString();
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = NETWORK_BASE_TYPES.networkbase.ToString();
                    //link backwards (to groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == NETWORK_TYPES.networkaccountgroup.ToString())
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
                    uri.URIDataManager.ChildrenNodeName = NETWORK_TYPES.networkaccountgroup.ToString();
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = NETWORK_TYPES.network.ToString();
                    //link backwards (to groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == NETWORK_TYPES.network.ToString()
                || currentNodeName == NETWORK_BASE_TYPES.networkbase.ToString())
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
                if (currentNodeName == NETWORK_TYPES.network.ToString())
                {
                    //no empty tocs
                    uri.URIDataManager.ChildrenNodeName = NETWORK_TYPES.network.ToString();
                }
                else
                {
                    //no empty tocs
                    uri.URIDataManager.ChildrenNodeName = NETWORK_BASE_TYPES.networkbase.ToString();
                }

            }
        }
        public static void GetChildForeignKeyName(string parentNodeName,
             out string parentForeignKeyName, out string baseForeignKeyName)
        {
            parentForeignKeyName = string.Empty;
            baseForeignKeyName = string.Empty;
            if (parentNodeName
                == NETWORK_TYPES.networkaccountgroup.ToString())
            {
                parentForeignKeyName = "AccountId";
                baseForeignKeyName = "NetworkId";
            }
        }
        public async Task<List<NetworkClass>> GetNetworkGroupsAsync(ContentURI uri)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader networkGroups
                = await sqlIO.RunProcAsync("0GetNetworkGroups");
            List<NetworkClass> colNetworkGroups = new List<NetworkClass>();
            if (networkGroups != null)
            {
                //build a related service list to return to the client
                while (networkGroups.Read())
                {
                    NetworkClass newNetworkGroup = new NetworkClass();
                    newNetworkGroup.PKId = networkGroups.GetInt32(0);
                    newNetworkGroup.NetworkClassLabel = networkGroups.GetString(1);
                    newNetworkGroup.NetworkClassName = networkGroups.GetString(2);
                    newNetworkGroup.NetworkClassDesc = networkGroups.GetString(3);
                    newNetworkGroup.NetworkClassControllerName = networkGroups.GetString(4);
                    newNetworkGroup.NetworkClassUserLanguage = networkGroups.GetString(5);
                    newNetworkGroup.Network = new List<Network>();
                    newNetworkGroup.IsSelected = false;
                    colNetworkGroups.Add(newNetworkGroup);
                }
            }
            sqlIO.Dispose();
            return colNetworkGroups;
        }
        public async Task<List<Network>> GetNetworkAsync(ContentURI uri)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader networks
                = await sqlIO.RunProcAsync("0GetNetworksBase");
            List<Network> colNetwork = new List<Network>();
            if (networks != null)
            {
                //build a related service list to return to the client
                //small set so not async
                while (networks.Read())
                {
                    Network newNetwork = new Network(true);
                    newNetwork.PKId = networks.GetInt32(0);
                    newNetwork.NetworkName = networks.GetString(1);
                    newNetwork.NetworkURIPartName = networks.GetString(2);
                    newNetwork.NetworkDesc = networks.GetString(3);
                    newNetwork.AdminConnection = networks.GetString(4);
                    newNetwork.WebFileSystemPath = networks.GetString(5);
                    newNetwork.WebConnection = networks.GetString(6);
                    newNetwork.WebDbPath = networks.GetString(7);
                    newNetwork.NetworkClassId = networks.GetInt32(8);
                    newNetwork.NetworkClass = new NetworkClass();
                    newNetwork.GeoRegionId = networks.GetInt32(9);
                    //rest are not stored in db
                    newNetwork.IsDefault = false;
                    newNetwork.URIFull = string.Empty;
                    newNetwork.XmlConnection = string.Empty;
                    newNetwork.AccountToNetwork = new List<AccountToNetwork>();
                    newNetwork.Service = new List<Service>();
                    colNetwork.Add(newNetwork);
                }
            }
            sqlIO.Dispose();
            return colNetwork;
        }
       
        public Network GetDefaultNetwork(string networkIdOrPartName)
        {
            int networkId = 0;
            string networkName = string.Empty;
            string networkPartName = string.Empty;
            string networkDescription = "allnetworks network";
            //version 2.0.0 standardized connections using config settings from ui to uri.uridatamngr.appsettings
            string adminConnection = string.Empty;
            string webFileSystemPath = Helpers.GeneralHelpers.GetDefaultNetworkGroupName();
            //most connections are made using this property
            string webConnection = string.Empty;
            string webDbPath = string.Empty;
            int networkGroupId = 0;
            string networkGroupName = string.Empty;
            int iNetworkId = 0;
            bool bIsNetworkId = int.TryParse(networkIdOrPartName, out iNetworkId);
            Helpers.GeneralHelpers.GetDefaultNetworkSettings(out networkId,
                out networkName, out networkPartName, out networkGroupId,
                out networkGroupName);
            Network network = new Network();
            network.PKId = iNetworkId;
            network.NetworkName = networkName;
            if (iNetworkId == 0)
            {
                network.NetworkURIPartName = networkIdOrPartName;
            }
            else
            {
                network.NetworkURIPartName = networkPartName;
            }
            network.NetworkDesc = networkDescription;
            network.AdminConnection = adminConnection;
            network.WebFileSystemPath = webFileSystemPath;
            network.WebConnection = webConnection;
            network.WebDbPath = webDbPath;
            network.NetworkClassId = networkGroupId;
            network.NetworkClass = new NetworkClass();
            network.NetworkClass.PKId = networkGroupId;
            network.NetworkClass.NetworkClassName = networkGroupName;
            network.GeoRegionId = 0;
            network.IsDefault = false;
            network.URIFull = string.Empty;
            return network;
        }
        public async Task<Network> GetNetworkAsync(
            ContentURI uri, string networkIdOrPartName)
        {
            Network network = new Network();
            network = await GetNetworkConnectionsAsync(uri, networkIdOrPartName);
            return network;
        }
        public async Task<Network> GetNetworkConnectionsAsync( 
            ContentURI uri, string networkIdOrPartName)
        {
            //default is 'all networks' network
            int networkId = 0;
            string networkName = string.Empty;
            string networkPartName = string.Empty;
            string networkDescription = string.Empty;
            string adminConnection = Helpers.AppSettings.GetConnection(uri);
            string webFileSystemPath = string.Empty;
            //most connections are made using this property
            string webConnection = string.Empty;
            string webDbPath = string.Empty;
            int networkGroupId = 0;
            string networkGroupName = string.Empty;
            int iNetworkId = 0;
            bool bIsNetworkId = int.TryParse(networkIdOrPartName, out iNetworkId);
            if (networkIdOrPartName == string.Empty
                || networkIdOrPartName == Helpers.GeneralHelpers.NONE)
            {
                //use default network settings
                Helpers.GeneralHelpers.GetDefaultNetworkSettings(out networkId,
                    out networkName, out networkPartName, out networkGroupId,
                    out networkGroupName);
                networkDescription = "allnetworks network";
                webFileSystemPath = Helpers.GeneralHelpers.GetDefaultNetworkGroupName(uri);
                webConnection = adminConnection;
            }
            else
            {
                int iIsNetwork = 0;
                if (bIsNetworkId) iIsNetwork = 1;
                Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                // params to stored proc
                SqlParameter[] oParams =
                {
                    sqlIO.MakeInParam("@NetworkParam",              SqlDbType.NVarChar, 20, networkIdOrPartName),
                    sqlIO.MakeInParam("@IsId",                      SqlDbType.Bit, 1, iIsNetwork),
                    sqlIO.MakeOutParam("@NetworkId",	            SqlDbType.Int, 4),
                    sqlIO.MakeOutParam("@NetworkName",	            SqlDbType.NVarChar, 150),
                    sqlIO.MakeOutParam("@NetworkURIPartName",	    SqlDbType.NVarChar, 20),
                    sqlIO.MakeOutParam("@NetworkDescription",       SqlDbType.NVarChar, 255),
                    sqlIO.MakeOutParam("@AdminConnection",	        SqlDbType.NVarChar, 255),
                    sqlIO.MakeOutParam("@WebFileSystemPath",        SqlDbType.NVarChar, 255),
                    sqlIO.MakeOutParam("@WebConnection",	        SqlDbType.NVarChar, 255),
                    sqlIO.MakeOutParam("@WebDbPath",		        SqlDbType.NVarChar, 255),
                    sqlIO.MakeOutParam("@NetworkClassId",	        SqlDbType.Int, 4),
                    sqlIO.MakeOutParam("@NetworkControllerName",    SqlDbType.NVarChar, 25)
                };
                // run the stored procedure
                await sqlIO.RunProcIntAsync("0GetNetworkConnections", oParams);
                //[1] is join field PKId and not used
                if (oParams[2].Value != System.DBNull.Value)
                {
                    networkId = (int)oParams[2].Value;
                }
                if (oParams[3].Value != System.DBNull.Value)
                {
                    networkName = oParams[3].Value.ToString();
                }
                if (oParams[4].Value != System.DBNull.Value)
                {
                    networkPartName = oParams[4].Value.ToString();
                }
                if (oParams[5].Value != System.DBNull.Value)
                {
                    networkDescription = oParams[5].Value.ToString();
                }
                if (oParams[6].Value != System.DBNull.Value)
                {
                    adminConnection = oParams[6].Value.ToString();
                }
                if (oParams[7].Value != System.DBNull.Value)
                {
                    webFileSystemPath = oParams[7].Value.ToString();
                }
                if (oParams[8].Value != System.DBNull.Value)
                {
                    webConnection = oParams[8].Value.ToString();
                }
                if (oParams[9].Value != System.DBNull.Value)
                {
                    webDbPath = oParams[9].Value.ToString();
                }
                if (oParams[10].Value != System.DBNull.Value)
                {
                    networkGroupId = (int)oParams[10].Value;
                }
                if (oParams[11].Value != System.DBNull.Value)
                {
                    networkGroupName = oParams[11].Value.ToString();
                }
                sqlIO.Dispose();
            }
            //version 2.0.0 standardized on this approach
            adminConnection = Helpers.AppSettings.GetConnection(uri);
            webConnection = Helpers.AppSettings.GetConnection(uri);
            Network network = new Network();
            network.PKId = iNetworkId;
            network.NetworkName = networkName;
            network.NetworkURIPartName = networkPartName;
            network.NetworkDesc = networkDescription;
            network.AdminConnection = adminConnection;
            network.WebFileSystemPath = webFileSystemPath;
            network.WebConnection = webConnection;
            network.WebDbPath = webDbPath;
            network.NetworkClassId = networkGroupId;
            network.NetworkClass = new NetworkClass();
            network.NetworkClass.PKId = networkGroupId;
            network.NetworkClass.NetworkClassName = networkGroupName;
            network.GeoRegionId = 0;
            network.IsDefault = false;
            network.URIFull = string.Empty;
            return network;
        }

        public async Task<SqlDataReader> GetNetworkForLoggedinMemberAsync(Helpers.SqlIOAsync sqlIO,
            SearchManager searcher)
        {
            SqlParameter[] colPrams = 
            { 
                sqlIO.MakeInParam("@AccountId",             SqlDbType.Int, 4, searcher.SearchResult.URIMember.ClubInUse.PKId),
                sqlIO.MakeInParam("@NetworkType",           SqlDbType.NVarChar, 25, searcher.NetworkType.ToString()),
                sqlIO.MakeInParam("@ServiceGroupId",        SqlDbType.Int, 4, searcher.ServiceGroupSelected.PKId),
                sqlIO.MakeInParam("@NetworkGroupId",        SqlDbType.Int, 4, searcher.NetworkGroupSelected.PKId),
                sqlIO.MakeInParam("@Language",              SqlDbType.NVarChar, 5, searcher.UserLanguage)
            };
            SqlDataReader dataReader 
                = await sqlIO.RunProcAsync("0GetNetworks", colPrams);
            return dataReader;
        }
        public async Task<bool> UpdateDefaultNetworkForLoggedinMemberAsync(
            ContentURI uri, int accountId, int newIsDefaultAccountToNetworkId)
        {
            bool bHasUpdated = false;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
            { 
                sqlIO.MakeInParam("@AccountId",                         SqlDbType.Int, 4, accountId),
                sqlIO.MakeInParam("@NewIsDefaultAccountToNetworkId",    SqlDbType.Int, 4, newIsDefaultAccountToNetworkId)
            };
            int iResult = await sqlIO.RunProcIntAsync("0UpdateNetworkDefaultId", colPrams);
            if (iResult == 0) bHasUpdated = true;
            sqlIO.Dispose();
            return bHasUpdated;
        }
    }
}
