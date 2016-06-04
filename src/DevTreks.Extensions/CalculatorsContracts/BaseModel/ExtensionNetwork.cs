using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevTreks.Models;
using DataGeneralHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The ExtensionNetwork class is adapted from the Network class in DevTreks.
    ///Author:		www.devtreks.org
    ///Date:		2011, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class ExtensionNetwork 
    {
        public ExtensionNetwork()
        {
            int iNetworkId = -1;
            string sNetworkName = string.Empty;
            string sNetworkPartName = string.Empty;
            int iNetworkGroupId = 0;
            string sNetworkGroupName = string.Empty;
            this.Id = 1;
            this.NetworkId = iNetworkId;
            this.NetworkName = sNetworkName;
            this.NetworkURIPartName = sNetworkPartName;
            this.NetworkDescription = "default commons network";
            this.URIFull = string.Empty;
            //version 2.0.0 standardized on uri.uridatamngr.appsettings
            this.AdminConnection = string.Empty;
            this.WebFileSystemPath = DataGeneralHelpers.GetDefaultNetworkGroupName();
            this.WebConnection = string.Empty;
            this.WebDbPath = string.Empty;
            this.XmlConnection = string.Empty;
            this.NetworkGroupId = iNetworkGroupId;
            this.NetworkGroupName = sNetworkGroupName;
        }
        public ExtensionNetwork(ExtensionNetwork copyNetwork)
        {
            this.Id = copyNetwork.Id;
            this.NetworkId = copyNetwork.NetworkId;
            this.NetworkName = copyNetwork.NetworkName;
            this.NetworkURIPartName = copyNetwork.NetworkURIPartName;
            this.NetworkDescription = copyNetwork.NetworkDescription;
            this.URIFull = copyNetwork.URIFull;
            this.AdminConnection = copyNetwork.AdminConnection;
            this.WebFileSystemPath = copyNetwork.WebFileSystemPath;
            this.WebConnection = copyNetwork.WebConnection;
            this.WebDbPath = copyNetwork.WebDbPath;
            this.XmlConnection = copyNetwork.XmlConnection;
            this.NetworkGroupId = copyNetwork.NetworkGroupId;
            this.NetworkGroupName = copyNetwork.NetworkGroupName;
        }
        public ExtensionNetwork(Network copyNetwork)
        {
            this.Id = copyNetwork.PKId;
            //this.NetworkId = copyNetwork.NetworkId;
            this.NetworkName = copyNetwork.NetworkName;
            this.NetworkURIPartName = copyNetwork.NetworkURIPartName;
            this.NetworkDescription = copyNetwork.NetworkDesc;
            this.URIFull = copyNetwork.URIFull;
            this.AdminConnection = copyNetwork.AdminConnection;
            this.WebFileSystemPath = copyNetwork.WebFileSystemPath;
            this.WebConnection = copyNetwork.WebConnection;
            this.WebDbPath = copyNetwork.WebDbPath;
            //this.XmlConnection = copyNetwork.XmlConnection;
            this.NetworkGroupId = copyNetwork.NetworkClassId;
            //this.NetworkGroupName = copyNetwork.NetworkGroupName;
        }
        public static Network GetNetwork(ExtensionNetwork copyNetwork)
        {
            Network newNetwork = new Network();
            newNetwork.PKId = copyNetwork.Id;
            //newNetwork.NetworkId = copyNetwork.NetworkId;
            newNetwork.NetworkName = copyNetwork.NetworkName;
            newNetwork.NetworkURIPartName = copyNetwork.NetworkURIPartName;
            newNetwork.NetworkDesc = copyNetwork.NetworkDescription;
            newNetwork.URIFull = copyNetwork.URIFull;
            newNetwork.AdminConnection = copyNetwork.AdminConnection;
            newNetwork.WebFileSystemPath = copyNetwork.WebFileSystemPath;
            newNetwork.WebConnection = copyNetwork.WebConnection;
            newNetwork.WebDbPath = copyNetwork.WebDbPath;
            //newNetwork.XmlConnection = copyNetwork.XmlConnection;
            newNetwork.NetworkClassId = copyNetwork.NetworkGroupId;
            //newNetwork.NetworkGroupName = copyNetwork.NetworkGroupName;
            return newNetwork;
        }
        //join table
        public int Id { get; set; }
        public bool IsDefault { get; set; }
        //base table
        public int NetworkId { get; set; }
        public string NetworkName { get; set; }
        public string NetworkURIPartName { get; set; }
        public string NetworkDescription { get; set; }
        //web address of this network
        public string URIFull { get; set; }
        public string AdminConnection { get; set; }
        public string WebFileSystemPath { get; set; }
        public string WebConnection { get; set; }
        public string WebDbPath { get; set; }
        public string XmlConnection { get; set; }
        //controller id
        public int NetworkGroupId { get; set; }
        //controller name
        public string NetworkGroupName { get; set; }
        public int GeoRegionId { get; set; }
        public static IList<Network> CopyNetwork(IList<Network> networks)
        {
            IList<Network> copyNetworks = new List<Network>();
            if (networks != null)
            {
                foreach (Network copyNetwork in networks)
                {
                    copyNetworks.Add(new Network(copyNetwork));
                }
            }
            return copyNetworks;
        }
    }
}
