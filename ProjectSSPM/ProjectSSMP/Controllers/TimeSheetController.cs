using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;

namespace ProjectSSMP.Controllers
{
    public class TimeSheetController : BaseController
    {
        public TimeSheetController(sspmContext context) //=> this.context = context;
        {
            this.context = context;
            CultureInfo en = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = en;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.userMenu = GetMenu();
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            var userid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();
            


            return View();
        }
    }
}