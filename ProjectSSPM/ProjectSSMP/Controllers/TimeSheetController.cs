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

            var checkID = context.UserSspm.SingleOrDefault(m => m.Username == loggedInUserName);
            var checkTeamTask = context.TeamTask.SingleOrDefault(m => m.UserId == checkID.UserId);
            //var checkProject = context.Project.SingleOrDefault(m => m.ProjectNumber == checkTeamTask.ProjectNumber);

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
            });
            /*
            var e = new TimeSheetInputModel()
            {
                ProjectId = checkProject.ProjectId,
                ProjectName = checkProject.ProjectName,
                ProjectNumber = checkProject.ProjectNumber,
                Note = checkProject.Note

            };
            */

            List<TimeSheetInputModel> model = new List<TimeSheetInputModel>();



            foreach (var item in PJ)
            {

                model.Add(new TimeSheetInputModel()
                {
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName,
                    ProjectNumber = item.ProjectNumber,
                    Note = item.Note,

                });
            }
                return View(model);
        }
    }
}