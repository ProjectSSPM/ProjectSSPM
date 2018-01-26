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

        public async Task<IActionResult> EditProject(string id)
        {
            ViewBag.userMenu = GetMenu();

            if (id == null)
            {
                return NotFound();
            }

            var Project = await context.Project.SingleOrDefaultAsync(m => m.ProjectNumber == id);

            var e = new CreateProjectInputModel()
            {
                ProjectNumber = Project.ProjectNumber,
                ProjectId = Project.ProjectId,
                ProjectName = Project.ProjectName,
                ProjectManager = Project.ProjectManager,
                ProjectCost = Project.ProjectCost,
                ProjectStart = Project.ProjectStart,
                ProjectEnd = Project.ProjectEnd,
                CustomerTel = Project.CustomerTel,
                CustomerName = Project.CustomerName

            };

            if (Project == null)
            {
                return NotFound();
            }
            return View(e);
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




        public IActionResult CreateTask(string id)
        {
            ViewBag.userMenu = GetMenu();

            if (id == null)
            {
                return NotFound();
            }


            List<CreateTaskInputModel> model = new List<CreateTaskInputModel>();


            var CreateTaskInputModel = (from x in context.Task where x.ProjectNumber.Equals(id) select x);

            foreach (var itme in CreateTaskInputModel)
            {
                
                model.Add(new CreateTaskInputModel()
                {
                    ProjectNumber = id,
                    TaskId = itme.TaskId,
                    TaskName = itme.TaskName,
                    TaskStart = itme.TaskStart,
                    TaskEnd = itme.TaskEnd



                });
            }
            ViewData["ProjectNuber"] = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(string pnum, CreateTaskInputModel inputModel)
        {
            ViewBag.userMenu = GetMenu();

            try
            {
                
                var id = (from u in context.RunningNumber where u.Type.Equals("TaskID") select u).FirstOrDefault();

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

                Models.Task ord = new Models.Task
                {
                    ProjectNumber = inputModel.ProjectNumber,
                    TaskId = num.ToString(),
                    TaskName = inputModel.TaskName,
                    TaskStart = inputModel.TaskStart,
                    TaskEnd = inputModel.TaskEnd

                };


                // Add the new object to the Orders collection.
                context.Task.Add(ord);
                await context.SaveChangesAsync();


                var query = from xx in context.RunningNumber
                            where xx.Type.Equals("TaskId")
                            select xx;

                foreach (RunningNumber RunTaskID in query)
                {
                    RunTaskID.Number = num.ToString();

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

                return RedirectToAction("CreateTask", "ProjectManagement");



            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return View();
            }
 
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


        public IActionResult CreateFunction(string id)
        {
            ViewBag.userMenu = GetMenu();

            if (id == null)
            {
                return NotFound();
            }
            List<CreateFunctionInputModel> model = new List<CreateFunctionInputModel>();
            var projectnumber = (from t in context.Task where t.TaskId.Equals(id) select t).FirstOrDefault();

            var CreateFunction = (from x in context.Function where x.TaskId.Equals(id) 
                                  select x);

            foreach (var itme in CreateFunction)
            {

                model.Add(new CreateFunctionInputModel()
                {
                   FunctionId = itme.FunctionId,
                   ProjectNumber = projectnumber.ProjectNumber,
                   TaskId = itme.TaskId,
                   FunctionName = itme.FunctionName,
                   FunctionStart = itme.FunctionStart,
                   FunctionEnd = itme.FunctionEnd

                });
            }
            ViewData["ProjectNuber"] = projectnumber.ProjectNumber;
            ViewData["TaskId"] = id;
            return View(model);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFunction(string pnum, CreateFunctionInputModel inputModel)
        {
            ViewBag.userMenu = GetMenu();

            try
            {

                var id = (from u in context.RunningNumber where u.Type.Equals("FunctionID") select u).FirstOrDefault();

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

                Models.Function ord = new Models.Function
                {
                    ProjectNumber = inputModel.ProjectNumber,
                    TaskId = inputModel.TaskId,
                    FunctionName = inputModel.FunctionName,
                    FunctionStart = inputModel.FunctionStart,
                    FunctionEnd = inputModel.FunctionEnd,
                    FunctionId = num.ToString()

                };


                // Add the new object to the Orders collection.
                context.Function.Add(ord);
                await context.SaveChangesAsync();


                var query = from xx in context.RunningNumber
                            where xx.Type.Equals("FunctionID")
                            select xx;

                foreach (RunningNumber RunFunctionID in query)
                {
                    RunFunctionID.Number = num.ToString();

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

                return RedirectToAction("CreateFunction", "ProjectManagement");



            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return View();
            }

        }

    }




}

