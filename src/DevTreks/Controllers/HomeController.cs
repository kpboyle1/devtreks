using DevTreks.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DevTreks.Controllers
{
    /// <summary>
    ///Purpose:		Home page
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	
    /// </summary>
    public class HomeController : Controller
    {
        public HomeController()
        {
            //2.0.0 localization code moved to Startup.cs
        }
        public ActionResult Index()
        {
            ViewData["Title"] = AppHelper.GetResource("DEVTREKS_TITLE");
            return View();
        }

        public ActionResult About()
        {
            ViewData["Title"] = AppHelper.GetResource("DEVTREKS_ABOUT");
            ViewData["Goal"] = AppHelper.GetResource("DEVTREKS_GOAL");
            ViewData["Message"] = AppHelper.GetResource("SOCIALBUDGET_DOES");
            return View();
        }
    }
}