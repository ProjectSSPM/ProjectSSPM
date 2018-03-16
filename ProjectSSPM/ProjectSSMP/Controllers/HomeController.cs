using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSMP.Models;
using Newtonsoft.Json;
using SSMP.Models.Timeline;
using SSMP.Models.Timesheet;
using SSMP.Models.Home;
using System.Globalization;
using System.Threading;
using NToastNotify;

using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace SSMP.Controllers
{
    public class HomeController : BaseController         
    {
        private readonly IToastNotification _toastNotification;

        
        public HomeController(sspmContext context ,IToastNotification toastNotification) //=> this.context = context;
        {
            this.context = context;
            CultureInfo en = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = en;
            _toastNotification = toastNotification;

        }
        [Authorize]
        public IActionResult Index()
        {
           

            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();

            var getimg = (from i in context.UserImage where i.UserId.Equals("100028") select i).FirstOrDefault();

          
            ViewData["img"] = getimg.Image;
           




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
                var PJ = (from x2 in context.TeamTask
                          join x3 in context.Project on x2.ProjectNumber equals x3.ProjectNumber
                          where x3.ProjectManager.Equals(checkgroup.UserId) || x2.UserId.Equals(checkgroup.UserId)
                          select new
                          {

                              ProjectNumber = x3.ProjectNumber,
                              ProjectName = x3.ProjectName,
                              ProjectId = x3.ProjectId,
                              Note = x3.Note,
                              ProjectEnd = x3.ProjectEnd,
                              ProjectStart = x3.ProjectStart

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
                var PJ = (from x2 in context.TeamTask
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

            var checkfri = (from ts in context.TimeSheet
                            where ts.ActionId.Equals("Z")
                            group ts by ts.FunctionId into tsgr
                            select new
                            {
                                Key = tsgr.Key
                            }).ToList();


            var checkdate = (from tt in context.TeamTask
                             join f in context.Function on tt.FunctionId equals f.FunctionId

                             where tt.UserId.Equals(checkgroup.UserId) && !(checkfri.Select(p => p.Key).Contains(f.FunctionId))
                             select new
                             {
                                 f.FunctionName,
                                 f.FunctionEnd,
                                 f.ActualStart,
                                 f.TaskId,
                                 f.FunctionId,
                                 f.ProjectNumber


                             });
            foreach (var cdete in checkdate)
            {
                DateTime fend = (DateTime)cdete.FunctionEnd;
                DateTime datenow = DateTime.Now;
                int checkfundae = (int)datenow.Subtract(fend).TotalDays;
                if (checkfundae <= 1)
                {
                    _toastNotification.AddToastMessage("Warning", cdete.FunctionName + "", Enums.ToastType.Error, new ToastOption()
                    {
                        PositionClass = ToastPositions.BottomRight
                    });
                }
            }

            var bulle = (from x in context.Bulletin join x2 in context.UserSspm on x.UserId equals x2.UserId

                         orderby x.Time descending
                         select new{
                            Subject = x.Subject,
                            Bnumber = x.Bnumber,
                            Note = x.Note,
                            Time = x.Time,
                            UserId = x.UserId,
                            Username = x2.Username,
                            
                        }).Take(5).ToList();
            List<CreateBulletinModel> modelx = new List<CreateBulletinModel>();

            foreach (var item in bulle)
            {
                var count = (from x in context.BulletinChat where x.Bnumber.Equals(item.Bnumber) select x).Count();

                modelx.Add(new CreateBulletinModel()
                {
                    Subject = item.Subject,
                    Bnumber = item.Bnumber,
                    Note = item.Note,
                    Time = item.Time,
                    UserId = item.UserId,
                    Username = item.Username,
                    BCount = count,


                });
            }


            ViewData["Bulletin"] = modelx;
            ViewData["Timeline"] = model;
            return View();

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
        public ActionResult AddBulletin()
        {

            return PartialView("AddBulletin");
        }

        [Authorize]
        public ActionResult ChatBulletin(String Id)
        {
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            var b = (from x in context.Bulletin
                     join x2 in context.UserSspm on x.UserId equals x2.UserId
                     where x.Bnumber.Equals(Id)
                     select new CreateBulletinModel
                     {
                         Subject = x.Subject,
                         Bnumber = x.Bnumber,
                         Note = x.Note,
                         Time = x.Time,
                         UserId = x.UserId,
                         Username = x2.Username,
                         Name = x2.Firstname + " " + x2.Lastname
                     }).SingleOrDefault();

            var bc = (from c in context.Bulletin
                      join c1 in context.BulletinChat on c.Bnumber equals c1.Bnumber
                      join u in context.UserSspm on c1.UserId equals u.UserId
                      where c.Bnumber.Equals(Id)
                      select new
                      {
                          CUserId = c1.UserId,
                          Bnumber = c.Bnumber,
                          CUsername = u.Username,
                          BChat = c1.Bchat,
                          CTime = c1.Ctime,
                          Chat = c1.Chat,
                          CFullname = u.Firstname + " " + u.Lastname
                      }).ToList();

            List<ChatBulletinModel> model = new List<ChatBulletinModel>();

            foreach (var item in bc)
            {

                model.Add(new ChatBulletinModel()
                {
                    Bchat = item.BChat,
                    Bnumber = item.Bnumber,
                    Ctime = item.CTime,
                    CUserId = item.CUserId,
                    CUsername = item.CUsername,
                    Chat = item.Chat,
                    CFullname = item.CFullname

                });
            }



            ViewData["Subject"] = b;
            ViewData["Chat"] = model;
            return PartialView("ChatBulletin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChatBulletin(CreateBulletinModel inputModel)
        {
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();

            if (!ModelState.IsValid)
            {
                return View();
            }


            try
            {
                var loggedInUser = HttpContext.User;
                var loggedInUserName = loggedInUser.Identity.Name;

                var user = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();

                var ChatNum = (from x in context.BulletinChat
                               where x.Bnumber.Equals(inputModel.Bnumber)
                               orderby x.Bchat descending
                              select x

                          ).FirstOrDefault();

                Boolean X = Boolean.ReferenceEquals(ChatNum, null);

                int num;
                if (X)
                {
                    num = 100000;

                }
                else
                {
                    num = Convert.ToInt32(ChatNum.Bchat);
                    num = num + 1;
                }

                Models.BulletinChat ord = new Models.BulletinChat
                {
                    UserId = user.UserId,
                    Chat = inputModel.Chat,
                    Ctime = DateTime.Now,
                    Bnumber = inputModel.Bnumber,
                    Bchat = num.ToString(),
                };


                // Add the new object to the Orders collection.
                context.BulletinChat.Add(ord);
                await context.SaveChangesAsync();


                return RedirectToAction("Index", "Home");



            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBulletin(CreateBulletinModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            try
            {
                var loggedInUser = HttpContext.User;
                var loggedInUserName = loggedInUser.Identity.Name;

                var id = (from u in context.RunningNumber where u.Type.Equals("BNumber") select u).FirstOrDefault();
                var user = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();

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

                Models.Bulletin ord = new Models.Bulletin
                {
                    UserId = user.UserId,
                    Subject = inputModel.Subject,
                    Note = inputModel.Note,
                    Time = DateTime.Now,
                    Bnumber = num.ToString(),

                };


                // Add the new object to the Orders collection.
                context.Bulletin.Add(ord);
                await context.SaveChangesAsync();


                var query = from xx in context.RunningNumber
                            where xx.Type.Equals("BNumber")
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

                return RedirectToAction("Index", "Home");



            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return View();
            }

        }



        [Authorize]
        public IActionResult Result(string id)
        {
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();


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
                                Username = USR.Firstname+" "+USR.Lastname,
           

                           }).ToList();

            List<ResultModel> model = new List<ResultModel>();

            foreach (var item in joinyub)
            {
                var statnow = "";
                Boolean X = Boolean.ReferenceEquals(item.FActualEnd, null);
                if (X){
                    Boolean Y = Boolean.ReferenceEquals(item.FActualStart, null);
                    if(Y){
                        statnow = "Not Started";
                    }
                    else{
                        statnow = "Process";
                    }
                }
                else{
                    statnow = "Finished";
                }
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
                    Status = statnow
                });
            }

            return View(model);
        }

        [Authorize]
        public IActionResult CheckTimeline(string id)
        {
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
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
