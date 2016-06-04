using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevTreks
{
    /// <summary>
    ///Purpose:		Member controller interface for handling login/logout views
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org
    /// </summary>
    public interface IMemberController
    {
        //logouts out and redirects to home/index page
        //ActionResult Logout();
        //allows a club to manage their payments (subscription fees collected and paid)
        Task<ActionResult> Payments();
        Task<ActionResult> UpdatePaymentHandling();
    }
}
