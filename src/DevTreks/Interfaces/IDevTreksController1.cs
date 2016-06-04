using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevTreks
{
    /// <summary>
    ///Purpose:		DevTreks controller interface for handling client actions 
    ///             and generating views.
    ///Author:		www.devtreks.org
    ///Date:		2012, August
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    /// NOTES   1. Refer to DevTreks.DataAccess.ContentURI for further explanation
    ///             of the contenturipattern used below.
    ///         2. If contenturipattern is a real uri.uripattern, the page reloads 
    ///             and the address changes to the restful uri.urifull. If
    ///             contenturipattern is an 'aspx' page name, an ajax request 
    ///             has been made. The page does not reload and the address does
    ///             not change. Instead an html fragment is returned to the client,
    ///             refreshing a part of the existing html page. The full version 
    ///             of the page (uri.urifull) is still available and can be loaded
    ///             by clicking on the 'Load URI' links found at the bottom of all
    ///             pages.
    public interface IDevTreksController1
    {
        //returns a search view (i.e. google-style search list), contenturipattern is a uri pattern
        Task<ActionResult> Search(string contenturipattern);
        //returns a member management view
        Task<ActionResult> Member(string contenturipattern);
        //used by the previous two actionresults
        Task<ActionResult> SearchAction(string contenturipattern);
        //returns a preview view of the contenturipattern (i.e. usually, a list of linked views)
        Task<ActionResult> Preview(string contenturipattern);
        //returns a select view of the contenturipattern (a list of selectable children of the uri)
        Task<ActionResult> Select(string contenturipattern);
        //returns an edit view of the contenturipattern 
        Task<ActionResult> Edit(string contenturipattern);
        //returns a package view
        Task<ActionResult> Pack(string contenturipattern);
        //returns additional views of the contenturipattern (i.e. calculators and analyzers)
        Task<ActionResult> LinkedViews(string contenturipattern);
        //used by all of the previous five actionresults
        Task<ActionResult> ContentAction(string contenturipattern);
    }
}