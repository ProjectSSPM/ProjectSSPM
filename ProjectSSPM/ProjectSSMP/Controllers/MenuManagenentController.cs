using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSSMP.Models;
using ProjectSSMP.Models.Menu;

namespace ProjectSSMP.Controllers
{
    public class MenuManagenentController : BaseController
    {
        public MenuManagenentController(sspmContext context) => this.context = context;

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.userMenu = GetMenu();
            List<IndexMenuModel> mode = new List<IndexMenuModel>();
            var indexmenu = (from mg in context.MenuGroup
                             join ma in context.MenuAuthentication on mg.MenuId equals ma.MenuId
                             join ug in context.UserGroup on ma.GroupId equals ug.GroupId
                             select new
                             {
                                 MenuId=mg.MenuId,
                                 MenuName = mg.MenuName,
                                 MenuUrl = mg.MenuUrl,
                                 MenuIcon = mg.MenuIcon,
                                 GroupId = ug.GroupId,
                                 GroupName = ug.GroupName
                             }).ToList();
            foreach (var item in indexmenu)
            {
                mode.Add(new IndexMenuModel()
                {
                    MenuId=item.MenuId,
                    MenuName=item.MenuName,
                    MenuUrl=item.MenuUrl,
                    MenuIcon=item.MenuIcon,
                    GroupId=item.GroupId,
                    GroupName=item.GroupName
                });

            }
             
            
            return View(mode);
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

                var query = from num in context.RunningNumber
                            where num.Type.Equals("MenuGroup")
                            select num;

                foreach (RunningNumber RunUserID in query)
                {
                    RunUserID.Number = menuid;

                }
                await context.SaveChangesAsync();
                return RedirectToAction("Index", "MenuManagenent");

            }
            catch (Exception e)
            {
                var error = e; 
            }

            return RedirectToAction("Index", "MenuManagenent");
        }
        public IActionResult AddMenuAuthen()
        {
            ViewBag.userMenu = GetMenu();
            ViewData["UserGroup"] = new SelectList(context.UserGroup, "GroupId", "GroupName");
            ViewData["MenuGroup"] = new SelectList(context.MenuGroup, "MenuId", "MenuName");
            return View();
        }
        


    }
}