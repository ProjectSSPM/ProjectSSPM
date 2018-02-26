using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ProjectSSMP.Models;
using ProjectSSMP.Models.Timesheet;

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

            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            var checkgroup = (from u in context.UserSspm
                              join ua in context.UserAssignGroup on u.UserId equals ua.UserId
                              where u.Username.Equals(loggedInUserName)
                              select new
                              {
                                  ua.GroupId,
                                  u.UserId
                              }).FirstOrDefault();
            





            List<TimeSheetInputModel> model = new List<TimeSheetInputModel>();

            if (checkgroup.GroupId == "50")
            {
                var PJ = (from  x2 in context.TeamTask 
                          join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                          where x3.ProjectManager.Equals(checkgroup.UserId) || x2.UserId.Equals(checkgroup.UserId)
                          select new
                          {
                                    ProjectNumber = x3.ProjectNumber,
                                    ProjectName = x3.ProjectName,
                                    ProjectId = x3.ProjectId,
                                    Note = x3.Note,
                                    ProjectEnd = x3.ProjectEnd,
                          });
                foreach (var item in PJ)
                {

                    model.Add(new TimeSheetInputModel()
                    {
                        ProjectId = item.ProjectId,
                        ProjectName = item.ProjectName,
                        ProjectNumber = item.ProjectNumber,
                        Note = item.Note,
                        ProjectEnd = item.ProjectEnd
                    });
                }


            }
            else if(checkgroup.GroupId == "10")
            {
              var   PJ = (from  x2 in context.TeamTask
                          join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                          where x2.UserId.Equals(checkgroup.UserId)
                          select new
                          {
                              ProjectNumber = x3.ProjectNumber,
                              ProjectName = x3.ProjectName,
                              ProjectId = x3.ProjectId,
                              Note = x3.Note,
                              ProjectEnd = x3.ProjectEnd
                          });
                foreach (var item in PJ)
                {

                    model.Add(new TimeSheetInputModel()
                    {
                        ProjectId = item.ProjectId,
                        ProjectName = item.ProjectName,
                        ProjectNumber = item.ProjectNumber,
                        Note = item.Note,
                        ProjectEnd = item.ProjectEnd


                    });
                }

            }
            else
            {
                var PJ = (from x in context.UserSspm
                          join x2 in context.TeamTask on x.UserId equals x2.UserId
                          join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                          //where x3.ProjectManager.Equals(x.UserId) || x.Username.Equals(loggedInUserName)
                          select new
                          {
                              ProjectNumber = x3.ProjectNumber,
                              ProjectName = x3.ProjectName,
                              ProjectId = x3.ProjectId,
                              Note = x3.Note,
                              ProjectEnd = x3.ProjectEnd,
                          });
                foreach (var item in PJ)
                {

                    model.Add(new TimeSheetInputModel()
                    {
                        ProjectId = item.ProjectId,
                        ProjectName = item.ProjectName,
                        ProjectNumber = item.ProjectNumber,
                        Note = item.Note,
                        ProjectEnd = item.ProjectEnd
                    });
                }

            }
            return View(model);

        }


        [Authorize]
        public IActionResult TimeSheet(String id)
        {

            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            

            var uid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();
            var gropid = (from g in context.UserAssignGroup where g.UserId.Equals(uid.UserId)select g).FirstOrDefault();

            
            var checkAppro = from ts in context.TimeSheet select ts;
            ViewData["checkZ"] = from ts in context.TimeSheet select ts;
            ViewData["checkfi"] = checkAppro;

            ViewData["UserId"] = uid.UserId;         
            ViewData["gropid"] = gropid.GroupId;
            var checkAC = (from fc in context.Function
                           join ts in context.TimeSheet on fc.FunctionId equals ts.FunctionId
                           where ts.ActionId.Equals("Z")
                           group fc by fc.FunctionId into f
                           select new {
                               FunctionId =    f.Key
                               
                           }).ToList();
            
            List <TimeSheetInputModel> model = new List<TimeSheetInputModel>();
            if(gropid.GroupId == "50")
            {
                var Test = (from x in context.Function
                            join x2 in context.Task on x.TaskId equals x2.TaskId
                            join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                            join x5 in context.TeamTask on x.FunctionId equals x5.FunctionId
                            where x.ProjectNumber.Equals(id) /*&& x5.UserId.Equals(uid.UserId)*/
                             //&& !(checkAC.Select(p => p.FunctionId).Contains(x.FunctionId))
                            select new
                            {
                                ProjectNumber = x3.ProjectNumber,
                                ProjectName = x3.ProjectName,
                                TaskId = x2.TaskId,
                                TaskName = x2.TaskName,
                                FunctionId = x.FunctionId,
                                FunctionName = x.FunctionName,
                                UserId = x5.UserId
                            }).ToList();

                foreach (var item in Test)
                {

                    model.Add(new TimeSheetInputModel()
                    {

                        ProjectName = item.ProjectName,
                        ProjectNumber = item.ProjectNumber,
                        FunctionId = item.FunctionId,
                        FunctionName = item.FunctionName,
                        TaskId = item.TaskId,
                        TaskName = item.TaskName,
                        UserId = item.UserId



                    });
                }

            }
            else if(gropid.GroupId == "10")
            {
                var Test = (from x in context.Function
                            join x2 in context.Task on x.TaskId equals x2.TaskId
                            join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                            join x5 in context.TeamTask on x.FunctionId equals x5.FunctionId
                            where x.ProjectNumber.Equals(id) && x5.UserId.Equals(uid.UserId)
                             && !(checkAC.Select(p => p.FunctionId).Contains(x.FunctionId))


                            select new
                            {
                                ProjectNumber = x3.ProjectNumber,
                                ProjectName = x3.ProjectName,
                                TaskId = x2.TaskId,
                                TaskName = x2.TaskName,
                                FunctionId = x.FunctionId,
                                FunctionName = x.FunctionName,
                            }).ToList();

                foreach (var item in Test)
                {

                    model.Add(new TimeSheetInputModel()
                    {

                        ProjectName = item.ProjectName,
                        ProjectNumber = item.ProjectNumber,
                        FunctionId = item.FunctionId,
                        FunctionName = item.FunctionName,
                        TaskId = item.TaskId,
                        TaskName = item.TaskName,



                    });
                }
            }
            else
            {
                var Test = (from x in context.Function
                            join x2 in context.Task on x.TaskId equals x2.TaskId
                            join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                            join x5 in context.TeamTask on x.FunctionId equals x5.FunctionId
                            where x.ProjectNumber.Equals(id) 


                            select new
                            {
                                ProjectNumber = x3.ProjectNumber,
                                ProjectName = x3.ProjectName,
                                TaskId = x2.TaskId,
                                TaskName = x2.TaskName,
                                FunctionId = x.FunctionId,
                                FunctionName = x.FunctionName,
                            }).ToList();

                foreach (var item in Test)
                {

                    model.Add(new TimeSheetInputModel()
                    {

                        ProjectName = item.ProjectName,
                        ProjectNumber = item.ProjectNumber,
                        FunctionId = item.FunctionId,
                        FunctionName = item.FunctionName,
                        TaskId = item.TaskId,
                        TaskName = item.TaskName,



                    });
                }

            }
                

            
            

            


            
          
           
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> AddTimeSheet(String id){

            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            if (id == null)
            {
                return NotFound();
            }


            var Func = await context.Function.SingleOrDefaultAsync(m => m.FunctionId == id);
            var Tasx = await context.Task.SingleOrDefaultAsync(m => m.TaskId == Func.TaskId);
            var Proj = await context.Project.SingleOrDefaultAsync(m => m.ProjectNumber == Tasx.ProjectNumber);

            var e = new TimeSheetInputModel()
            {
                ProjectNumber = Proj.ProjectNumber,
                ProjectName = Proj.ProjectName,
                TaskId = Tasx.TaskId,
                TaskName = Tasx.TaskName,
                FunctionId = Func.FunctionId,
                FunctionName = Func.FunctionName,

            };

            ViewData["ProjectName"] = " "+Proj.ProjectName;
            if (e == null)
            {
                return NotFound();
            }
            
           
            return PartialView("AddTimeSheet", e);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTimeSheet(TimeSheetInputModel inputModel)
        {
            ViewBag.userMenu = GetMenu();

            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;



            var tsFunc = (from x in context.TimeSheet where x.FunctionId.Equals(inputModel.FunctionId)
                          orderby x.TimeSheetNumber descending
                          select x 
                          
                          ).FirstOrDefault();

            Boolean X = Boolean.ReferenceEquals(tsFunc, null);

            int num;
            if (X)
            {
                num = 100000;

            }
            else
            {
                num = Convert.ToInt32(tsFunc.TimeSheetNumber);
                num = num + 1;
            }

            var uid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();

            TimeSheet ord = new TimeSheet
            {
                ProjectNumber = inputModel.ProjectNumber,
                TaskId = inputModel.TaskId,
                FunctionId = inputModel.FunctionId,
                TimeSheetId = DateTime.Now,
                TimeSheetStart = inputModel.TimeSheetStart,
                TimeSheetEnd = inputModel.TimeSheetEnd,
                UserId = uid.UserId,
                TimeSheetNumber = num.ToString(),
                ActionId = "N"
            };

            try
            {
                context.TimeSheet.Add(ord);
                await context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }

            return RedirectToAction("TimeSheet", "TimeSheet",new RouteValueDictionary(
                new { Controller= "TimeSheet",Action = "TimeSheet",id = inputModel.ProjectNumber }));

        }

        [Authorize]
        public IActionResult UpdateTimeSheet(String id)
        {

            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;

            var uid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();

            var Test = (from x in context.TimeSheet join x2 in context.Function on x.FunctionId equals x2.FunctionId
                        where x.FunctionId.Equals(id) && x.UserId.Equals(uid.UserId) && x.ActionId.Equals("N")
                        select new
                        {
                            TimeSheetId = x.TimeSheetId,
                            TimeSheetStart = x.TimeSheetStart,
                            TimeSheetEnd = x.TimeSheetEnd,
                            FunctionId = x.FunctionId,
                            FunctionName = x2.FunctionName,
                            TimeSheetNumber = x.TimeSheetNumber
                        });

            List<TimeSheetInputModel> model = new List<TimeSheetInputModel>();


            foreach (var item in Test)
            {

                model.Add(new TimeSheetInputModel()
                {
                    TimeSheetId = item.TimeSheetId,
                    TimeSheetStart = item.TimeSheetStart,
                    TimeSheetEnd = item.TimeSheetEnd,
                    FunctionId = item.FunctionId,
                    FunctionName = item.FunctionName,
                    TimeSheetNumber = item.TimeSheetNumber


                });
            }

            if (Test == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ConfirmTimeSheet(string tid, string fid)
        {


            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;

            var upTimeSheet = (from t in context.TimeSheet
                               where t.TimeSheetNumber.Equals(tid) && t.FunctionId.Equals(fid)
                               select t).FirstOrDefault();


            ViewBag.userMenu = GetMenu();

            var uid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();
            var update = (from x in context.TimeSheet
                          where x.FunctionId.Equals(fid)
                          && x.UserId.Equals(uid.UserId) && x.TimeSheetNumber.Equals(tid)
                          select x).FirstOrDefault();

            var e = new TimeSheetInputModel()
            {
                TimeSheetNumber = tid,
                FunctionId = fid,
                UserId = uid.UserId,
                ActionId = update.ActionId,
                TaskId = update.TaskId,
                ProjectNumber = update.ProjectNumber,

            };
            ViewData["Action"] = new SelectList(context.Action.Where(a => a.ActionId != "N" ), "ActionId", "ActionName");

            return PartialView("ConfirmTimeSheet", e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmTimeSheet(string tid, string fid, TimeSheetInputModel inputModel)
        {

            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;

            ViewBag.userMenu = GetMenu();

            
            try
            {
                var update = (from x in context.TimeSheet
                              where x.FunctionId.Equals(inputModel.FunctionId)
                              && x.UserId.Equals(inputModel.UserId) && x.TimeSheetNumber.Equals(inputModel.TimeSheetNumber)
                              select x);
                var actID = " ";

                if(inputModel.ActionId.Equals("Z")){
                    actID = "Y";
                }
                else{
                    actID = inputModel.ActionId;
                }

                foreach (Models.TimeSheet TUpdate in update)
                {
                    TUpdate.ActionId = actID;
                }
                await context.SaveChangesAsync();


                try
                {
                    var addLog = (from x in context.TimeSheet
                                  where x.FunctionId.Equals(inputModel.FunctionId)
                                  && x.UserId.Equals(inputModel.UserId) && x.TimeSheetNumber.Equals(inputModel.TimeSheetNumber)
                                  select x).FirstOrDefault();
                    var stat = "";
                    var AP1 = "";
                    if (addLog.ActionId.Equals("Y"))
                    {
                        return RedirectToAction("TimeSheet", "TimeSheet", new RouteValueDictionary(
                        new { Controller = "TimeSheet", Action = "TimeSheet", id = inputModel.ProjectNumber }));

                    }
                    else
                    {
                        stat = "P";

                    }    
                    FunctionLog ord = new FunctionLog
                    {
                        ProjectNumber = addLog.ProjectNumber,
                        FunctionLogId = addLog.TimeSheetId,
                        FunctionId = addLog.FunctionId,
                        FunctionStart = addLog.TimeSheetStart,
                        FunctionEnd = addLog.TimeSheetEnd,
                        FunctionNumber = addLog.TimeSheetNumber,
                        StatusId = stat,
                        TaskId = addLog.TaskId,
                        Approve1 = AP1,
                    };

                    context.FunctionLog.Add(ord);
                    await context.SaveChangesAsync();

                    try
                    {
                        var update2 = (from x in context.FunctionLog
                                       where x.FunctionId.Equals(inputModel.FunctionId)
                                       && x.FunctionNumber.Equals(inputModel.TimeSheetNumber)
                                       select x);
                        var ip = (from x in context.TimeSheet
                                  where x.FunctionId.Equals(inputModel.FunctionId)
                                  && x.TimeSheetNumber.Equals(inputModel.TimeSheetNumber)
                                  select x).FirstOrDefault();
                        if (ip.ActionId.Equals("Z"))
                        {
                            var checkss = (from x in context.FunctionLog where x.FunctionNumber.Equals("100000") select x).FirstOrDefault();

                            DateTime alstat = (DateTime)checkss.ActualStart;
                            DateTime alend = (DateTime)addLog.TimeSheetEnd;

                            foreach (Models.FunctionLog FUpdate in update2)
                            {
                                FUpdate.StatusId = "F";
                                FUpdate.ActualStart = checkss.ActualStart;
                                FUpdate.ActualEnd = addLog.TimeSheetEnd;
                                FUpdate.Variant = (int)alend.Subtract(alstat).TotalDays;

                            }
                            await context.SaveChangesAsync();
                            try
                            {
                                var check1 = (from x in context.FunctionLog
                                              where x.FunctionId.Equals(addLog.FunctionId) && x.StatusId.Equals("F")
                                                && x.ProjectNumber.Equals(inputModel.ProjectNumber) && x.TaskId.Equals(inputModel.TaskId)
                                              select x).FirstOrDefault();
                                var update3 = (from x in context.Function
                                               where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) && x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                               select x);
                                DateTime afstat = (DateTime)check1.ActualStart;
                                DateTime afend = (DateTime)check1.ActualEnd;


                                foreach (Models.Function FUpdate in update3)
                                {
                                    FUpdate.ActualStart = check1.ActualStart;
                                    FUpdate.ActualEnd = check1.ActualEnd;
                                    FUpdate.Variant = (int)afend.Subtract(afstat).TotalDays;

                                }
                                await context.SaveChangesAsync();




                                var AllTask = (from x in context.Function where x.TaskId.Equals(check1.TaskId) select x).Count();
                                var CheckTask = (from x in context.FunctionLog where x.TaskId.Equals(check1.TaskId) && x.StatusId.Equals("F") select x).Count();



                                if (AllTask == CheckTask)
                                {
                                    var update4 = (from x in context.Task
                                                   where x.TaskId.Equals(addLog.TaskId)
                                                   select x);
                                    var at = (from x in context.Task
                                              where x.TaskId.Equals(addLog.TaskId)
                                              select x).FirstOrDefault();
                                    DateTime atstat = (DateTime)at.ActualStart;
                                    DateTime atend = (DateTime)check1.ActualEnd;
                                    foreach (Models.Task FUpdate in update4)
                                    {

                                        FUpdate.ActualEnd = check1.ActualEnd;
                                        FUpdate.Variant = (int)atend.Subtract(atstat).TotalDays;

                                    }
                                    await context.SaveChangesAsync();

                                    var AllProject = (from x in context.Task where x.TaskId.Equals(check1.TaskId) select x).Count();
                                    var CheckProject = (from x in context.FunctionLog where x.TaskId.Equals(check1.TaskId) && x.StatusId.Equals("F") && x.ProjectNumber.Equals(check1.ProjectNumber) && x.TaskId.Equals(check1.TaskId) select x).Count();
                                    if (AllProject == CheckProject)
                                    {
                                        var update5 = (from x in context.Project
                                                       where x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                                       select x);
                                        var ap = (from x in context.Project
                                                  where x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                                  select x).FirstOrDefault();
                                        DateTime apstat = (DateTime)ap.ActualStart;
                                        DateTime apend = (DateTime)check1.ActualEnd;
                                        foreach (Models.Project FUpdate in update5)
                                        {

                                            FUpdate.ActualEnd = check1.ActualEnd;
                                            FUpdate.Variant = (int)apend.Subtract(apstat).TotalDays;
                                        }
                                        await context.SaveChangesAsync();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            var c = (from x in context.TimeSheet where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) select x).Count();
                            if (c == 1)
                            {
                                foreach (Models.FunctionLog FUpdate in update2)
                                {
                                    FUpdate.ActualStart = addLog.TimeSheetStart;
                                }
                                await context.SaveChangesAsync();

                                var c4 = (from x in context.Function where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) select x).Count();
                                if (c == 1)
                                {
                                    var update3 = (from x in context.Function
                                                   where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) && x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                                   select x);
                                    foreach (Models.Function FUpdate in update3)
                                    {
                                        FUpdate.ActualStart = addLog.TimeSheetStart;
                                    }
                                    await context.SaveChangesAsync();
                                }
                                var c2 = (from x in context.TimeSheet where x.TaskId.Equals(addLog.TaskId) select x).Count();
                                if (c2 == 1)
                                {
                                    var update4 = (from x in context.Task
                                                   where x.TaskId.Equals(addLog.TaskId) && x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                                   select x);
                                    foreach (Models.Task FUpdate in update4)
                                    {

                                        FUpdate.ActualStart = addLog.TimeSheetStart;
                                    }
                                    await context.SaveChangesAsync();

                                    var c3 = (from x in context.TimeSheet where x.ProjectNumber.Equals(addLog.ProjectNumber) select x).Count();
                                    if (c2 == 1)
                                    {
                                        var update5 = (from x in context.Project
                                                       where x.ProjectNumber.Equals(addLog.ProjectNumber)
                                                       select x);
                                        foreach (Models.Project FUpdate in update5)
                                        {

                                            FUpdate.ActualStart = addLog.TimeSheetStart;
                                        }
                                        await context.SaveChangesAsync();
                                    }
                                }

                            }
                            var checkss = (from x in context.FunctionLog where x.FunctionNumber.Equals("100000") && x.FunctionId.Equals(addLog.FunctionId) && x.ProjectNumber.Equals(inputModel.ProjectNumber) && x.TaskId.Equals(inputModel.TaskId) select x).FirstOrDefault();

                            foreach (Models.FunctionLog FUpdate in update2)
                            {
                                FUpdate.StatusId = "P";
                                FUpdate.ActualStart = checkss.ActualStart;
                            }
                            await context.SaveChangesAsync();
                        }


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index", "TimeSheet");
        }

        public ActionResult ApproveFunction(string Id)
        {
            var id = (from x in context.TimeSheet where x.FunctionId.Equals(Id) && x.ActionId.Equals("Y") select x).FirstOrDefault();


            ViewData["ApproveData"] = id;
            return PartialView("ApproveFunction");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveFunction(TimeSheetInputModel inputModel)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;

            var update = (from x in context.TimeSheet
                          where x.FunctionId.Equals(inputModel.FunctionId)
                          && x.UserId.Equals(inputModel.UserId) && x.TimeSheetNumber.Equals(inputModel.TimeSheetNumber)
                          select x);
            var userApprove = (from x in context.UserSspm where x.Username.Equals(loggedInUserName) select x).SingleOrDefault();

            foreach (Models.TimeSheet TUpdate in update)
            {
                TUpdate.ActionId = "Z";
                TUpdate.Approve1 = userApprove.UserId;
            }
            await context.SaveChangesAsync();

            try
            {
                var addLog = (from x in context.TimeSheet
                              where x.FunctionId.Equals(inputModel.FunctionId)
                              && x.UserId.Equals(inputModel.UserId) && x.TimeSheetNumber.Equals(inputModel.TimeSheetNumber)
                              select x).FirstOrDefault();
                var stat = "";
                var AP1 = "";
                if (addLog.ActionId.Equals("Z"))
                {
                    stat = "F";
                    AP1 = userApprove.UserId;
                }
                else
                {
                    stat = "P";

                }
                FunctionLog ord = new FunctionLog
                {
                    ProjectNumber = addLog.ProjectNumber,
                    FunctionLogId = addLog.TimeSheetId,
                    FunctionId = addLog.FunctionId,
                    FunctionStart = addLog.TimeSheetStart,
                    FunctionEnd = addLog.TimeSheetEnd,
                    FunctionNumber = addLog.TimeSheetNumber,
                    StatusId = stat,
                    TaskId = addLog.TaskId,
                    Approve1 = AP1,
                };

                context.FunctionLog.Add(ord);
                await context.SaveChangesAsync();

                try
                {
                    var update2 = (from x in context.FunctionLog
                                   where x.FunctionId.Equals(inputModel.FunctionId)
                                   && x.FunctionNumber.Equals(inputModel.TimeSheetNumber)
                                   select x);
                    var ip = (from x in context.TimeSheet
                                where x.FunctionId.Equals(inputModel.FunctionId)
                                && x.TimeSheetNumber.Equals(inputModel.TimeSheetNumber)
                                select x).FirstOrDefault();
                    if (ip.ActionId.Equals("Z"))
                    {
                        var checkss = (from x in context.FunctionLog where x.FunctionNumber.Equals("100000") select x).FirstOrDefault();

                        DateTime alstat = (DateTime)checkss.ActualStart;
                        DateTime alend = (DateTime)addLog.TimeSheetEnd;

                        foreach (Models.FunctionLog FUpdate in update2)
                        {
                            FUpdate.StatusId = "F";
                            FUpdate.ActualStart = checkss.ActualStart;
                            FUpdate.ActualEnd = addLog.TimeSheetEnd;
                            FUpdate.Variant = (int)alend.Subtract(alstat).TotalDays;

                        }
                        await context.SaveChangesAsync();
                        try
                        {
                            var check1 = (from x in context.FunctionLog
                                          where x.FunctionId.Equals(addLog.FunctionId) && x.StatusId.Equals("F")
                                            && x.ProjectNumber.Equals(inputModel.ProjectNumber) && x.TaskId.Equals(inputModel.TaskId)
                                          select x).FirstOrDefault();
                            var update3 = (from x in context.Function
                                           where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) && x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                           select x);
                            DateTime afstat = (DateTime)check1.ActualStart;
                            DateTime afend = (DateTime)check1.ActualEnd;


                            foreach (Models.Function FUpdate in update3)
                            {
                                FUpdate.ActualStart = check1.ActualStart;
                                FUpdate.ActualEnd = check1.ActualEnd;
                                FUpdate.Variant = (int)afend.Subtract(afstat).TotalDays;

                            }
                            await context.SaveChangesAsync();




                            var AllTask = (from x in context.Function where x.TaskId.Equals(check1.TaskId) select x).Count();
                            var CheckTask = (from x in context.FunctionLog where x.TaskId.Equals(check1.TaskId) && x.StatusId.Equals("F") select x).Count();



                            if (AllTask == CheckTask)
                            {
                                var update4 = (from x in context.Task
                                               where x.TaskId.Equals(addLog.TaskId)
                                               select x);
                                var at = (from x in context.Task
                                          where x.TaskId.Equals(addLog.TaskId)
                                          select x).FirstOrDefault();
                                DateTime atstat = (DateTime)at.ActualStart;
                                DateTime atend = (DateTime)check1.ActualEnd;
                                foreach (Models.Task FUpdate in update4)
                                {

                                    FUpdate.ActualEnd = check1.ActualEnd;
                                    FUpdate.Variant = (int)atend.Subtract(atstat).TotalDays;

                                }
                                await context.SaveChangesAsync();

                                var AllProject = (from x in context.Task where x.TaskId.Equals(check1.TaskId) select x).Count();
                                var CheckProject = (from x in context.FunctionLog where x.TaskId.Equals(check1.TaskId) && x.StatusId.Equals("F") && x.ProjectNumber.Equals(check1.ProjectNumber) && x.TaskId.Equals(check1.TaskId) select x).Count();
                                if (AllProject == CheckProject)
                                {
                                    var update5 = (from x in context.Project
                                                   where x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                                   select x);
                                    var ap = (from x in context.Project
                                              where x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                              select x).FirstOrDefault();
                                    DateTime apstat = (DateTime)ap.ActualStart;
                                    DateTime apend = (DateTime)check1.ActualEnd;
                                    foreach (Models.Project FUpdate in update5)
                                    {

                                        FUpdate.ActualEnd = check1.ActualEnd;
                                        FUpdate.Variant = (int)apend.Subtract(apstat).TotalDays;
                                    }
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    else
                    {
                        var c = (from x in context.TimeSheet where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) select x).Count();
                        if (c == 1)
                        {
                            foreach (Models.FunctionLog FUpdate in update2)
                            {
                                FUpdate.ActualStart = addLog.TimeSheetStart;
                            }
                            await context.SaveChangesAsync();

                            var c4 = (from x in context.Function where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) select x).Count();
                            if (c == 1)
                            {
                                var update3 = (from x in context.Function
                                               where x.FunctionId.Equals(inputModel.FunctionId) && x.TaskId.Equals(inputModel.TaskId) && x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                               select x);
                                foreach (Models.Function FUpdate in update3)
                                {
                                    FUpdate.ActualStart = addLog.TimeSheetStart;
                                }
                                await context.SaveChangesAsync();
                            }
                            var c2 = (from x in context.TimeSheet where x.TaskId.Equals(addLog.TaskId) select x).Count();
                            if (c2 == 1)
                            {
                                var update4 = (from x in context.Task
                                               where x.TaskId.Equals(addLog.TaskId) && x.ProjectNumber.Equals(inputModel.ProjectNumber)
                                               select x);
                                foreach (Models.Task FUpdate in update4)
                                {

                                    FUpdate.ActualStart = addLog.TimeSheetStart;
                                }
                                await context.SaveChangesAsync();

                                var c3 = (from x in context.TimeSheet where x.ProjectNumber.Equals(addLog.ProjectNumber) select x).Count();
                                if (c2 == 1)
                                {
                                    var update5 = (from x in context.Project
                                                   where x.ProjectNumber.Equals(addLog.ProjectNumber)
                                                   select x);
                                    foreach (Models.Project FUpdate in update5)
                                    {

                                        FUpdate.ActualStart = addLog.TimeSheetStart;
                                    }
                                    await context.SaveChangesAsync();
                                }
                            }

                        }
                        var checkss = (from x in context.FunctionLog where x.FunctionNumber.Equals("100000") && x.FunctionId.Equals(addLog.FunctionId) && x.ProjectNumber.Equals(inputModel.ProjectNumber) && x.TaskId.Equals(inputModel.TaskId) select x).FirstOrDefault();

                        foreach (Models.FunctionLog FUpdate in update2)
                        {
                            FUpdate.StatusId = "P";
                            FUpdate.ActualStart = checkss.ActualStart;
                        }
                        await context.SaveChangesAsync();
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("TimeSheet", "TimeSheet", new RouteValueDictionary(
                new { Controller = "TimeSheet", Action = "TimeSheet", id = inputModel.ProjectNumber }));
        }


    }
   
}


