using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            else if(checkgroup.GroupId == "10")
            {
              var   PJ = (from x in context.UserSspm
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



            ////var PJ = (from x in context.UserSspm
            ////          join x2 in context.TeamTask on x.UserId equals x2.UserId
            ////          join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
            ////          where x.Username.Equals(loggedInUserName)
            ////          select new
            ////          {
            ////              ProjectNumber = x3.ProjectNumber,
            ////              ProjectName = x3.ProjectName,
            ////              ProjectId = x3.ProjectId,
            ////              Note = x3.Note,
            ////}); 
            //foreach (var item in PJ)
            //{

            //    model.Add(new TimeSheetInputModel()
            //    {
            //        ProjectId = item.ProjectId,
            //        ProjectName = item.ProjectName,
            //        ProjectNumber = item.ProjectNumber,
            //        Note = item.Note,

            
            return View(model);

        }


        [Authorize]
        public IActionResult TimeSheet(String id)
        {

            ViewBag.userMenu = GetMenu();


            var Test = (from x in context.Function
                        join x2 in context.Task on x.TaskId equals x2.TaskId
                        join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                        where x.ProjectNumber.Equals(id)
                        select new
                        {
                            ProjectNumber = x3.ProjectNumber,
                            ProjectName = x3.ProjectName,
                            TaskId = x2.TaskId,
                            TaskName = x2.TaskName,
                            FunctionId = x.FunctionId,
                            FunctionName = x.FunctionName,
            });

            List<TimeSheetInputModel> model = new List<TimeSheetInputModel>();


            foreach (var item in Test)
            {

                model.Add(new TimeSheetInputModel()
                {

                    ProjectName = item.ProjectName,
                    ProjectNumber = item.ProjectNumber,
                    FunctionId = item.FunctionId,
                    FunctionName = item.FunctionName,
                    TaskId = item.TaskId,
                    TaskName = item.TaskName

                });
            }
          
            if (Test == null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}