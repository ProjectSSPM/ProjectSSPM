using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSSMP.Models;
using ProjectSSMP.Models.ProjectManagement;
using ProjectSSMP.Models.UserManagenent;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectSSMP.Controllers
{
    public class ProjectManagementController : BaseController
    {
        // GET: /<controller>/
        public ProjectManagementController(sspmContext context) => this.context = context;

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.userMenu = GetMenu();

            return View(await context.Project.ToListAsync());
        }

        public IActionResult CreateProject()
        {
            ViewBag.userMenu = GetMenu();
            return View();
        }

        public IActionResult CreateAll()
        {
            ViewBag.userMenu = GetMenu();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(CreateProjectInputModel inputModel)
        {
            ViewBag.userMenu = GetMenu();
            try
            {
                var loggedInUser = HttpContext.User;
                var loggedInUserName = loggedInUser.Identity.Name;
                var id = (from u in context.RunningNumber where u.Type.Equals("ProjectNumber") select u).FirstOrDefault();

                int num;
                if (id.Number == null)
                {
                    num = 100001;

                }
                else
                {
                    num = Convert.ToInt32(id.Number);
                    num = num + 1;
                }

                Project ord = new Project
                {
                    ProjectNumber = num.ToString(),
                    ProjectId = inputModel.ProjectId,
                    ProjectName = inputModel.ProjectName,
                    ProjectStart = inputModel.ProjectStart,
                    ProjectEnd = inputModel.ProjectEnd,
                    ProjectCost = inputModel.ProjectCost,
                    ProjectManager = inputModel.ProjectManager,
                    ProjectCreateBy = loggedInUserName,
                    ProjectCreateDate = DateTime.Now,
                    CustomerName = inputModel.CustomerName,
                    CustomerTel = inputModel.CustomerTel,
                    Note = inputModel.Note

                };


                // Add the new object to the Orders collection.
                context.Project.Add(ord);
                await context.SaveChangesAsync();


                var query = from xx in context.RunningNumber
                            where xx.Type.Equals("ProjectNumber")
                            select xx;

                foreach (RunningNumber RunUserID in query)
                {
                    RunUserID.Number = num.ToString();

                }

                // Submit the changes to the database.
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

                return RedirectToAction("Index", "ProjectManagement");



            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAll(CreateProjectInputModel inputModel)
        {
            ViewBag.userMenu = GetMenu();
            try
            {
                var loggedInUser = HttpContext.User;
                var loggedInUserName = loggedInUser.Identity.Name;
                var id = (from u in context.RunningNumber where u.Type.Equals("ProjectNumber") select u).FirstOrDefault();

                int num;
                if (id.Number == null)
                {
                    num = 100001;

                }
                else
                {
                    num = Convert.ToInt32(id.Number);
                    num = num + 1;
                }

                Project ord = new Project
                {
                    ProjectNumber = num.ToString(),
                    ProjectId = inputModel.ProjectId,
                    ProjectName = inputModel.ProjectName,
                    ProjectStart = inputModel.ProjectStart,
                    ProjectEnd = inputModel.ProjectEnd,
                    ProjectCost = inputModel.ProjectCost,
                    ProjectManager = inputModel.ProjectManager,
                    ProjectCreateBy = loggedInUserName,
                    ProjectCreateDate = DateTime.Now,
                    CustomerName = inputModel.CustomerName,
                    CustomerTel = inputModel.CustomerTel,
                    Note = inputModel.Note

                };


                // Add the new object to the Orders collection.
                context.Project.Add(ord);
                await context.SaveChangesAsync();


                var query = from xx in context.RunningNumber
                            where xx.Type.Equals("ProjectNumber")
                            select xx;

                foreach (RunningNumber RunUserID in query)
                {
                    RunUserID.Number = num.ToString();

                }

                // Submit the changes to the database.
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

                return ViewBag.PID = num.ToString();



            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return View();
            }
        }


        public IActionResult TestTable()
        {
            return View();
        }

    }


}

