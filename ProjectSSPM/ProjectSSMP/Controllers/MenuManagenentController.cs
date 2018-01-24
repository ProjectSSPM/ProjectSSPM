using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSSMP.Models;
using ProjectSSMP.Models.Menu;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMenu(AddMenuModel addMenu)
        {
            ViewBag.userMenu = GetMenu();
            try
            {
                var mid = (from mg in context.RunningNumber where mg.Type.Equals("MenuGroup") select mg).FirstOrDefault();
                string menuid;
                if(mid.Number == null)
                {
                    menuid = "M001";
                }
                else
                {
                    string[] meid = Regex.Split(mid.Number, "M");
                    menuid = "M00" + (Convert.ToInt32(meid[1]) + 1).ToString();


                }
                MenuGroup menu = new MenuGroup
                {
                    MenuId=menuid,
                    MenuName=addMenu.MenuName,
                    MenuUrl=addMenu.MenuUrl,
                    MenuIcon=addMenu.MenuIcon
                };
                context.MenuGroup.Add(menu);
                await context.SaveChangesAsync();

            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Index", "MenuManagenent");
        }
    }
}