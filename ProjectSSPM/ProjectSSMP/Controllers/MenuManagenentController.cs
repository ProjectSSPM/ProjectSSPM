using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSSMP.Models;

namespace ProjectSSMP.Controllers
{
    public class MenuManagenentController : BaseController
    {
        public MenuManagenentController(sspmContext context) => this.context = context;

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.userMenu = GetMenu();
            var mm = await (from mg in context.MenuGroup select mg).ToListAsync();
            return View(mm);
        }
        public IActionResult AddMenu()
        {
            ViewBag.userMenu = GetMenu();
            return View();
        }
    }
}