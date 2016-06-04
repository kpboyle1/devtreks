using DevTreks.Models;
using System.Linq;

namespace DevTreks.Data
{
    /// <summary>
    ///Purpose:		Pipeline pattern for filtering SearchManager data 
    ///Author:		www.devtreks.org
    ///Date:		2008, August
    ///References:	deprecated but keep for reference: called from an extension class to SearchManager
    /// </summary>
    public static class SearchFilters
    {
        /// <summary>
        /// Filters the query (getnetworks()) by NetworkId
        /// </summary>
        /// <param name="networkId">The network ID to filter by</param>
        /// <returns>IQueryable of Network</returns>
        public static IQueryable<Network> WithNetworkId(this IQueryable<Network> qry,
            int networkId)
        {
            return from n in qry
                   where n.PKId == networkId
                   select n;
        }
        public static IQueryable<Network> WithNetworkPartName(this IQueryable<Network> qry,
            string networkPartName)
        {
            return from n in qry
                   where n.NetworkURIPartName == networkPartName
                   select n;
        }
        /// <summary>
        /// Filters the query (getnetworkgrouops()) by NetworkGroupId
        /// </summary>
        /// <param name="networkGroupId">The networkgroup ID to filter by</param>
        /// <returns>IQueryable of Network</returns>
        public static IQueryable<NetworkClass>
            WithNetworkGroupId(this IQueryable<NetworkClass> qry,
            int networkGroupId)
        {
            return from ng in qry
                   where ng.PKId == networkGroupId
                   select ng;
        }
    }
}
