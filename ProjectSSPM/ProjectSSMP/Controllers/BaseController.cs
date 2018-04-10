using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSMP.Models;
using SSMP.Models.Menu;
using SSMP.Models.Notification;

namespace SSMP.Controllers
{
    public class BaseController : Controller
    {
        public sspmContext context;

        
        public List<GetMenuModelcs>  GetMenu()
        {

            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            var userid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();
            

            var userMenu = (from mg in context.MenuGroup
                            join ma in context.MenuAuthentication on mg.MenuId equals ma.MenuId
                            join ua in context.UserAssignGroup on ma.GroupId equals ua.GroupId
                            where ua.UserId.Equals(userid.UserId)
                            select new GetMenuModelcs
                            {
                                Userid =userid.UserId,
                                MenuId =mg.MenuId,
                                MenuName = mg.MenuName,
                                MenuUrl  =  mg.MenuUrl,
                                MenuIcon = mg.MenuIcon,
                Username = loggedInUserName,
                            }).ToList();
            
            
            return userMenu; 
        }

        public List<NotificationModel> Nothi()
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            var userid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();

            var checkfri = (from ts in context.TimeSheet
                            where ts.ActionId.Equals("Z") || ts.ActionId.Equals("Y")
                            group ts by ts.FunctionId into tsgr
                            select new {
                                Key = tsgr.Key
                            }).ToList();


            var checkdate = (from tt in context.TeamTask
                             join f in context.Function on tt.FunctionId equals f.FunctionId
                             where tt.UserId.Equals(userid.UserId) && !(checkfri.Select(p => p.Key).Contains(f.FunctionId))
                             select new
                             {
                                 f.FunctionName,
                                 f.FunctionEnd,
                                 f.ActualStart,
                                 f.TaskId,
                                 f.FunctionId,
                                 f.ProjectNumber


                             });
            List<NotificationModel> model = new List<NotificationModel>();
            foreach (var cdete in checkdate)
            {
                DateTime fend = (DateTime)cdete.FunctionEnd;
                DateTime datenow = DateTime.Now;
                int checkfundae = (int)fend.Subtract(datenow).TotalDays;
                if (checkfundae <= 1)
                {
                    model.Add(new NotificationModel() {
                        TaskId = cdete.TaskId,
                        FunctionName = cdete.FunctionName,
                        FunctionId = cdete.FunctionId,
                        FunctionStart = cdete.ActualStart,
                        FunctionEnd = cdete.FunctionEnd,
                        ProjectNumber = cdete.ProjectNumber,
                        Datenow = DateTime.Now

                    });
                   
                }

            }

            return model;
        }
    }
}