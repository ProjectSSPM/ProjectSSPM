using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;
using Newtonsoft.Json;
using ProjectSSMP.Models.Timeline;
using ProjectSSMP.Models.Timesheet;
using ProjectSSMP.Models.Home;

namespace ProjectSSMP.Controllers
{
    public class HomeController : BaseController         
    {
        public HomeController(sspmContext context) => this.context = context;

        [Authorize]
        public IActionResult Index()
        {

            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();
            var checkgroup = (from u in context.UserSspm
                              join ua in context.UserAssignGroup on u.UserId equals ua.UserId
                              select new
                              {
                                  ua.GroupId
                              }).FirstOrDefault();
            List<TimeSheetInputModel> model = new List<TimeSheetInputModel>();

            if (checkgroup.GroupId == "50")
            {
                var PJ = (from x in context.UserSspm
                          join x2 in context.TeamTask on x.UserId equals x2.UserId
                          join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                          where x3.ProjectManager.Equals(x.UserId) || x.Username.Equals(loggedInUserName)
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
            else if (checkgroup.GroupId == "10")
            {
                var PJ = (from x in context.UserSspm
                          join x2 in context.TeamTask on x.UserId equals x2.UserId
                          join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                          where x.Username.Equals(loggedInUserName)
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

            }

            return View(model);

        }

        public IActionResult About()
        {
            
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public ActionResult AddTimeline(string id)
        {
            ViewBag.userMenu = GetMenu();



            return View();
        }

        [Authorize]
        public IActionResult Result(string id)
        {
            ViewBag.userMenu = GetMenu();


            var pjd = (from p in context.Project join u in context.UserSspm on p.ProjectManager equals u.UserId
                       where p.ProjectNumber.Equals(id) 
                       select new ResultProjectModel
                       {
                           ProjectNumber = p.ProjectNumber,
                           ProjectId = p.ProjectId,
                           ProjectName= p.ProjectName,
                           ProjectManager = u.Firstname + " "+ u.Lastname,
                           ProjectCost = p.ProjectCost,
                           CustomerName = p.CustomerName,
                           CustomerTel = p.CustomerTel,
                           ProjectStart =p.ProjectStart,
                           ProjectEnd =p.ProjectEnd,
                           ActualStart = p.ActualStart,
                           ActualEnd = p.ActualEnd
                       }
                       ).FirstOrDefault();
            ViewData["projectDetile"] = pjd;



            var joinyub = (from TT in context.TeamTask
                           join FC in context.Function on TT.FunctionId equals FC.FunctionId
                           join TSK in context.Task on FC.TaskId equals TSK.TaskId
                           join PJ in context.Project on TSK.ProjectNumber equals PJ.ProjectNumber
                           join USR in context.UserSspm on TT.UserId equals USR.UserId
                           where PJ.ProjectNumber.Equals(id)
                           select new
                           {
                                TaskName = TSK.TaskName,
                                TaskId = TSK.TaskId,
                                TaskStart = TSK.TaskStart,
                                TaskEnd = TSK.TaskEnd,
                                TActualStart = TSK.ActualStart,
                                TActualEnd = TSK.ActualEnd,

                                FunctionName = FC.FunctionName,
                                FunctionId = FC.FunctionId,
                                FunctionStart = FC.FunctionStart,
                                FunctionEnd = FC.FunctionEnd,
                                FActualStart = FC.ActualStart,
                                FActualEnd = FC.ActualEnd,
                                Username = USR.Username,

                           }).ToList();

            List<ResultModel> model = new List<ResultModel>();

            foreach (var item in joinyub)
            {

                model.Add(new ResultModel()
                {
                    TaskName = item.TaskName,
                    TaskId = item.TaskId,
                    TaskStart = item.TaskStart,
                    TaskEnd = item.TaskEnd,
                    TActualStart = item.TActualStart,
                    TActualEnd = item.TActualEnd,

                    FunctionName = item.FunctionName,
                    FunctionId = item.FunctionId,
                    FunctionStart = item.FunctionStart,
                    FunctionEnd = item.FunctionEnd,
                    FActualStart = item.FActualStart,
                    FActualEnd = item.FActualEnd,
                    Username = item.Username,

                });
            }

            return View(model);
        }

        [Authorize]
        public IActionResult CheckTimeline(string id)
        {
            ViewBag.userMenu = GetMenu();
            if (id == null)
            {
                return NotFound();
            }

            var pname = context.Project.FirstOrDefault(m => m.ProjectNumber == id);

            var check = (from x in context.FunctionLog join x2 in context.Task on x.TaskId equals x2.TaskId
                         join x3 in context.Function on x.FunctionId equals x3.FunctionId
                         join x4 in context.TeamTask on x.FunctionId equals x4.FunctionId
                         join x5 in context.UserSspm on x4.UserId equals x5.UserId
                         where x.ProjectNumber.Equals(id) && x.StatusId.Equals("F") select new{
                                TaskId = x.TaskId,
                                TaskName = x2.TaskName,
                                FunctionName = x3.FunctionName,
                                ActualEnd = x.ActualEnd,
                                FunctionEnd = x3.FunctionEnd,
                                Firstname = x5.Firstname,
                                Lastname = x5.Lastname
                            }).OrderByDescending(x => x.ActualEnd).ToList();


            List<TimelineInputModel> model = new List<TimelineInputModel>();



            foreach (var item in check)
            {

                model.Add(new TimelineInputModel()
                {
                    TaskId = item.TaskId,
                    ActualEnd = item.ActualEnd,
                    TaskName = item.TaskName,
                    FunctionName = item.FunctionName,
                    FunctionEnd = item.FunctionEnd,
                    Firstname = item.Firstname,
                    Lastname = item.Lastname

                });
            }



            ViewData["ProjectName"] = pname.ProjectName.ToString();
            ViewData["ProjectNumber"] = pname.ProjectNumber.ToString();
            return View(model);
        }

    }
}
