using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public ProjectManagementController(sspmContext context) //=> this.context = context;
        {
            this.context = context;
            CultureInfo en = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = en;
        }

        public async Task<IActionResult> Index()
        {
            
            ViewBag.userMenu = GetMenu();

            List<IndexProjectModel> model = new List<IndexProjectModel>();


            var IndexProInputModel = await (from x in context.Project select x).ToListAsync();


            foreach (var item in IndexProInputModel)
            {
                var check = (from x in context.FunctionLog where x.ProjectNumber.Equals(item.ProjectNumber) && x.StatusId.Equals("F") select x).Count();
                var all = (from x in context.Function where x.ProjectNumber.Equals(item.ProjectNumber) select x).Count();
                double percent = ((double)check / (double)all) * 100.0;
                double f = Math.Floor(percent);
                var ans = "";
                if(f == 100){
                    ans = "Finished!";
                }
                else{
                    ans = percent.ToString() + "%";
                }

                model.Add(new IndexProjectModel()
                {
                    ProjectNumber = item.ProjectNumber,
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName,
                    ProjectStart = item.ProjectStart,
                    ProjectEnd = item.ProjectEnd,
                    Percent = ans,

                });
            }

            return View(model);
            //return View(await context.Project.ToListAsync());
        }
        [Authorize]
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
            ViewData["UserSSPM"] = new SelectList(context.UserSspm.Join(context.UserAssignGroup,
                                                u => u.UserId,
                                                ua => ua.UserId,
                                                (u, ua) => new {
                                                    UserId = u.UserId,
                                                    Firstname = u.Firstname,
                                                    GroupId = ua.GroupId,
                                                    Status = u.Status

                                                }).Where(ua => ua.GroupId.Equals("50") && ua.Status.Equals("A"))

                                                , "UserId", "Firstname");
            ViewData["naemproject"] = Project.ProjectName;

            if (Project == null)
            {
                return NotFound();
            }
            return PartialView("EditProject",e);
        }

        [Authorize]
        public async Task<IActionResult> EditFunction(string id)
        {

            ViewBag.userMenu = GetMenu();

            if (id == null)
            {
                return NotFound();
            }

            var Function = await context.Function.SingleOrDefaultAsync(m => m.FunctionId == id);
            var Teamtask = await context.TeamTask.SingleOrDefaultAsync(m => m.FunctionId == id);

            var e = new CreateFunctionInputModel
            {
                ProjectNumber = Function.ProjectNumber,
                TaskId = Function.TaskId,
                FunctionName = Function.FunctionName,
                FunctionStart = Function.FunctionStart,
                FunctionEnd = Function.FunctionEnd,
                FunctionId = Function.FunctionId,
                UserId = Teamtask.UserId,
                ProjectResponsible = Teamtask.ProjectResponsible

            };

            if (Function == null)
            {
                return NotFound();
            }

            ViewData["UserSSPM"] = new SelectList(context.UserSspm.Join(context.UserAssignGroup,
                                                                        u => u.UserId,
                                                                        ua => ua.UserId,
                                                                        (u, ua) => new {
                                                                            UserId = u.UserId,
                                                                            Firstname = u.Firstname,
                                                                            GroupId = ua.GroupId,
                                                                            Status = u.Status
                                                                        }).Where(ua => ua.GroupId.Equals("10") || ua.GroupId.Equals("50") && ua.Status.Equals("A")
                                                                        )

                , "UserId", "Firstname");
            ViewData["naemFunction"] = Function.FunctionName;

            return PartialView("EditFunction", e);
        }

        [Authorize]
        public async Task<IActionResult> EditTask(string id)
        {

            ViewBag.userMenu = GetMenu();

            if (id == null)
            {
                return NotFound();
            }

            var Task = await context.Task.SingleOrDefaultAsync(m => m.TaskId == id);

            var e = new CreateTaskInputModel()
            {
                ProjectNumber = Task.ProjectNumber,
                TaskId = Task.TaskId,
                TaskName = Task.TaskName,
                TaskStart = Task.TaskStart,
                TaskEnd = Task.TaskEnd,

            };
            ViewData["naemtask"] = Task.TaskName;
            if (Task == null)
            {
                return NotFound();
            }
            return PartialView("EditTask", e);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFunction(string id, CreateFunctionInputModel editModel)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();



            var query = (from x in context.Function where x.FunctionId.Equals(editModel.FunctionId) select x).FirstOrDefault();
            if (editModel.FunctionId != query.FunctionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    //context.Update(ord);
                    var addquery = from test in context.Function
                                   where test.FunctionId.Equals(editModel.FunctionId)
                                   select test;
                    foreach (Models.Function UserUpdate in addquery)
                    {
                        UserUpdate.FunctionName = editModel.FunctionName;
                        UserUpdate.FunctionEnd = editModel.FunctionEnd;
                        UserUpdate.FunctionStart = editModel.FunctionStart;

                    }
                    await context.SaveChangesAsync();
                    try
                    {
                        var addquery2 = from test2 in context.TeamTask
                                        where test2.FunctionId.Equals(editModel.FunctionId)
                                        select test2;
                        foreach (TeamTask UserUpdate2 in addquery2)
                        {
                            UserUpdate2.UserId = editModel.UserId;
                            UserUpdate2.ProjectResponsible = editModel.ProjectResponsible;

                        }
                        await context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

 

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }


                var baseFormat = "http://localhost:56087/ProjectManagement/CreateFunction/";
                var segment = query.TaskId.ToString();
                var url = baseFormat + segment;
                return Redirect(url);
            }
            return View(query.FunctionId);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(string id, CreateTaskInputModel editModel)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();


            var query = (from x in context.Task where x.TaskId.Equals(editModel.TaskId) select x).FirstOrDefault();
            if (editModel.TaskId != query.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    //context.Update(ord);
                    var addquery = from test in context.Task
                                                           where test.TaskId.Equals(editModel.TaskId)
                                   select test;
                    foreach (Models.Task UserUpdate in addquery)
                    {
                        UserUpdate.TaskName = editModel.TaskName;
                        UserUpdate.TaskEnd = editModel.TaskEnd;
                        UserUpdate.TaskStart = editModel.TaskStart;

                    }
                    await context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                var baseFormat = "http://localhost:56087/ProjectManagement/CreateTask/";
                var segment = query.ProjectNumber.ToString();
                var url = baseFormat + segment;
                return Redirect(url);
            }
            return View(query.TaskId);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(string id, CreateProjectInputModel editModel)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();


            var query = (from x in context.Project where x.ProjectNumber.Equals(editModel.ProjectNumber) select x).FirstOrDefault();
            if (editModel.ProjectNumber != query.ProjectNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    //context.Update(ord);
                    var addquery = from test in context.Project
                                                           where test.ProjectNumber.Equals(editModel.ProjectNumber)
                                                            select test;
                    foreach (Project UserUpdate in addquery)
                    {
                        UserUpdate.ProjectId = editModel.ProjectId;
                        UserUpdate.ProjectName = editModel.ProjectName;
                        UserUpdate.ProjectStart = editModel.ProjectStart;
                        UserUpdate.ProjectEnd = editModel.ProjectEnd;
                        UserUpdate.ProjectManager = editModel.ProjectManager;
                        UserUpdate.ProjectEditBy = loggedInUserName;
                        UserUpdate.ProjectEditDate = DateTime.Now;
                        UserUpdate.ProjectCost = editModel.ProjectCost;
                        UserUpdate.CustomerName = editModel.CustomerName;
                        UserUpdate.CustomerTel = editModel.CustomerTel;

                    }
                    await context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(query.ProjectNumber);
        }




        [Authorize]
        public IActionResult CreateProject()
        {
            
            ViewBag.userMenu = GetMenu();
            ViewData["UserSSPM"] = new SelectList(context.UserSspm.Join(context.UserAssignGroup,
                                                u => u.UserId,
                                                ua => ua.UserId,
                                                (u, ua) => new {
                                                    UserId = u.UserId,
                                                    Firstname = u.Firstname,
                                                    GroupId = ua.GroupId,
                                                    Status = u.Status

                                                }).Where(ua => ua.GroupId.Equals("50") && ua.Status.Equals("A"))

                                                , "UserId", "Firstname");
            return View();
        }




        public Double DT(DateTime D1 , DateTime D2)
        {
            DateTime d1 = D1;
            DateTime d2 = D2;
            Double d3 = d1.Subtract(d2).TotalDays;
            return d3;

        }

        [Authorize]
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
                    TaskEnd = itme.TaskEnd,
                    Timespan = DT((System.DateTime)itme.TaskEnd, (System.DateTime)itme.TaskStart)

                });
            }
            var Proname = (from p in context.Project where p.ProjectNumber.Equals(id) select p).FirstOrDefault();
            ViewData["ProjectName"] = "  "+Proname.ProjectId;
            ViewData["ProjectNuber"] = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(string pnum, CreateTaskInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ViewBag.userMenu = GetMenu();
            if (inputModel.TaskEnd < inputModel.TaskStart)
            {
                ModelState.AddModelError("ErrorCreateTask", "กรุณากรอกเวลาใหม่");
                return View();
            }



            

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
                    TaskName = inputModel.TaskName ,
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
            if (!ModelState.IsValid)
            {
                return View();
            }

            ViewBag.userMenu = GetMenu();
            if (inputModel.ProjectEnd < inputModel.ProjectStart)
            {
                ModelState.AddModelError("ErrorCreatePtoject", "Estimate Date is incorrect. Please Re Input");
                return View();
            }
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





        [Authorize]
        public IActionResult CreateFunction(string id)
        {
            
            ViewBag.userMenu = GetMenu();

            if (id == null)
            {
                return NotFound();
            }
            var user = (from u in context.UserSspm
                        join ua in context.UserAssignGroup on u.UserId equals ua.UserId
                        where ua.GroupId.Equals("10")
                        select new
                        {
                            UserId = u.UserId,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname
                        }).FirstOrDefault();
            
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
                   FunctionEnd = itme.FunctionEnd,
                    Timespan = DT((System.DateTime)itme.FunctionEnd, (System.DateTime)itme.FunctionStart)
                   

                });
            }
            ViewData["UserSSPM"] = new SelectList(context.UserSspm.Join(context.UserAssignGroup,
                                                                        u => u.UserId,
                                                                        ua => ua.UserId,
                                                                        (u,ua) => new {
                                                                            UserId = u.UserId,
                                                                            Firstname =  u.Firstname,
                                                                            GroupId = ua.GroupId,
                                                                            Status = u.Status
                                                                        }).Where(ua => ua.GroupId.Equals("10") || ua.GroupId.Equals("50") && ua.Status.Equals("A")
                                                                        )
                
                , "UserId", "Firstname");
            ViewData["ProjectNuber"] = projectnumber.ProjectNumber;
            ViewData["TaskId"] = id;

            var Proname = (from p in context.Project where p.ProjectNumber.Equals(projectnumber.ProjectNumber) select p).FirstOrDefault();
            var taskname = (from t in context.Task where t.TaskId.Equals(id) select t).FirstOrDefault();

            ViewData["ProjectName"] = Proname.ProjectId;
            ViewData["Taskname"] = taskname.TaskName;
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
                    try
                    {
                        Models.TeamTask ord2 = new Models.TeamTask
                        {
                            FunctionId = num.ToString(),
                            UserId = inputModel.UserId,
                            ProjectResponsible = inputModel.ProjectResponsible,
                            TaskId = inputModel.TaskId,
                            ProjectNumber = inputModel.ProjectNumber

                        };

                        context.TeamTask.Add(ord2);
                        await context.SaveChangesAsync();
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine(x);
                    }
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


        public IActionResult Detail(string id)
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyDate(DateTime ProjectStart, DateTime ProjectEnd)
        {
            if (ProjectEnd < ProjectStart)
            {
                return Json(data: $"The Estimate End is greater than Start.");
            }
            return Json(data: true);
        }

    }




}

