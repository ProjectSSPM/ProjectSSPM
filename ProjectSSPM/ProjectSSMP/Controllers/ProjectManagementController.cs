using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectSSMP.Controllers
{
    public class ProjectManagementController : BaseController
    {
        // GET: /<controller>/
        public ProjectManagementController(sspmContext context) => this.context = context;

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.userMenu = GetMenu();
            return View();
        }
        public IActionResult CreateProject()
        {
            ViewBag.userMenu = GetMenu();
            return View();
        }
    }
}
