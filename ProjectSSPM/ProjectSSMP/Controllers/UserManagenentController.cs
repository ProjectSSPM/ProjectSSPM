using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSMP.Models;
using SSMP.Models.UserManagement;
using SSMP.Models.UserManagenent;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SSMP.Controllers
{
    [Authorize]
    public class UserManagementController : BaseController
    {
        public UserManagementController(sspmContext context) => this.context = context;

        
        public IActionResult Index()
        {
            if (!checkuser())
            {
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            


            List<IndexUserModel> model = new List<IndexUserModel>();

            var indexUserModel = (from x in context.UserSspm
                                  join x2 in context.UserAssignGroup on x.UserId equals x2.UserId
                                  join x3 in context.UserGroup on x2.GroupId equals x3.GroupId
                                  select new
                                  {
                                      UserId = x.UserId,
                                      Username = x.Username,
                                      Status = x.Status,
                                      UserEditBy = x.UserEditBy,
                                      UserEditDate = x.UserEditDate,
                                      GroupName = x3.GroupName
                                  }).ToList();
            
            foreach(var itme in indexUserModel)
            {
                var check ="";
                if (itme.Status == "A")
                {
                    check = "Active";
                }
                else
                {
                    check = "DeActived";
                }

                model.Add(new IndexUserModel()
                {
                    UserId = itme.UserId,
                    Username = itme.Username,
                    Status = check,
                    UserEditBy= itme.UserEditBy,
                    UserEditDate = itme.UserEditDate,
                    GroupName = itme.GroupName,
                   


                });
            }

            return View(model);

        }

       
        public IActionResult AddUser()
        {
            if (!checkuser())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserInputModel inputModel)
        {
            
            ViewBag.userMenu = GetMenu();
            if (ModelState.IsValid)
            {
                try
                {

                    var loggedInUser = HttpContext.User;
                    var loggedInUserName = loggedInUser.Identity.Name;

                    var id = (from u in context.RunningNumber where u.Type.Equals("UserID") select u).FirstOrDefault();

                    int userid;
                    if (id.Number == null)
                    {
                        userid = 100001;

                    }
                    else
                    {
                        userid = Convert.ToInt32(id.Number);
                        userid = userid + 1;
                    }

                    UserSspm ord = new UserSspm
                    {
                        UserId = userid.ToString(),
                        Username = inputModel.Username,
                        Password = inputModel.Password,
                        Firstname = inputModel.Firstname,
                        Lastname = inputModel.Lastname,
                        JobResponsible = inputModel.JobResponsible,
                        UserCreateBy = loggedInUserName,
                        UserCreateDate = DateTime.Now,
                        Status = "A",
                        UserTel = inputModel.UserTel,
                        LineId = inputModel.LineId,

                    };

                    UserAssignGroup ord2 = new UserAssignGroup
                    {
                        UserId = userid.ToString(),
                        GroupId = inputModel.GroupId,

                    };

                    // Add the new object to the Orders collection.

                    context.UserSspm.Add(ord);
                    await context.SaveChangesAsync();
                    context.UserAssignGroup.Add(ord2);
                    await context.SaveChangesAsync();



                    var query = from num in context.RunningNumber
                                where num.Type.Equals("UserID")
                                select num;

                    foreach (RunningNumber RunUserID in query)
                    {
                        RunUserID.Number = userid.ToString();

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
            return View(inputModel);
            
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            if (!checkuser())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            if (id == null)
            {
                return NotFound();
            }

            var userSspm = await context.UserSspm.SingleOrDefaultAsync(m => m.UserId == id);
            var userAssign = await context.UserAssignGroup.SingleOrDefaultAsync(m => m.UserId == id);

            var e = new ShowUserInputModel()
            {
                UserId = userSspm.UserId,
                Username = userSspm.Username,
                Password = userSspm.Password,
                Firstname = userSspm.Firstname,
                Lastname = userSspm.Lastname,
                JobResponsible = userSspm.JobResponsible,
                Status = userSspm.Status,
                GroupId = userAssign.GroupId,
                UserTel = userSspm.UserTel,
                LineId = userSspm.LineId

            };
            ViewData["Username"] = userSspm.Username;
            if (userSspm == null)
            {
                return NotFound();
            }
            return View(e);
        }

        // POST: UserSspms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(string id, EditUserInputModel editModel)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();


            var query = (from x in context.UserSspm where x.UserId.Equals(id) select x).FirstOrDefault();
            if (id != query.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    //context.Update(ord);
                    var addquery = from test in context.UserSspm
                                                            where test.UserId.Equals(id)
                                                        select test;
                    foreach (UserSspm UserUpdate in addquery)
                    {
                        UserUpdate.Username = editModel.Username;
                        UserUpdate.Password = editModel.Password;
                        UserUpdate.Firstname = editModel.Firstname;
                        UserUpdate.Lastname = editModel.Lastname;
                        UserUpdate.JobResponsible = editModel.JobResponsible;
                        UserUpdate.UserEditBy = loggedInUserName;
                        UserUpdate.UserEditDate = DateTime.Now;
                        UserUpdate.Status = editModel.Status;
                        UserUpdate.LineId = editModel.LineId;
                        UserUpdate.UserTel = editModel.UserTel;

                    }
                    await context.SaveChangesAsync();
                    try
                    {
                        var addquery2 = from test2 in context.UserAssignGroup
                                        where test2.UserId.Equals(id)
                                        select test2;
                        foreach (UserAssignGroup UserUpdate2 in addquery2)
                        {
                            UserUpdate2.GroupId = editModel.GroupId;
                            UserUpdate2.UserId = id;

                        }
                        await context.SaveChangesAsync();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserSspmExists(query.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(query.UserId);
        }

        private bool UserSspmExists(string id)
        {
            return context.UserSspm.Any(e => e.UserId == id);
        }
       
        public async Task<IActionResult> Details(string id)
        {
          
            if (id == null)
            {
                return NotFound();
            }

            var userSspm = await context.UserSspm.SingleOrDefaultAsync(m => m.UserId == id);
            var userAssign = await context.UserAssignGroup.SingleOrDefaultAsync(m => m.UserId == id);

            var groupname = (from u in context.UserGroup where u.GroupId.Equals(userAssign.GroupId) select u).FirstOrDefault();
            var check = "";
            if (userSspm.Status == "A")
            {
                check = "Active";
            }
            else
            {
                check = "DeActived";
            }
            var e = new DetailUserInputModel()
            {
                UserId = userSspm.UserId,
                Username = userSspm.Username,
                Password = userSspm.Password,
                Firstname = userSspm.Firstname,
                Lastname = userSspm.Lastname,
                JobResponsible = userSspm.JobResponsible,
                Status = check,
                GroupId = userAssign.GroupId,
                UserCreateDate = userSspm.UserCreateDate,
                UserEditDate = userSspm.UserEditDate,
                UserCreateBy = userSspm.UserCreateBy,
                UserEditBy = userSspm.UserEditBy,
                GroupName = groupname.GroupName,
                UserTel = userSspm.UserTel,
                LineId = userSspm.LineId,
            };
            ViewData["Username"] = userSspm.Username;
            if (userSspm == null)
            {
                return NotFound();
            }
            return PartialView("Details",e);
        }

        public bool checkuser()
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            var userid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();
            var groub = (from g in context.UserAssignGroup where g.UserId.Equals(userid.UserId) select g).FirstOrDefault();
            if (groub.GroupId != "99")
            {
                return false;
            }

            return true;
        }

        public bool checkuser2(string id)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            var userid = (from u in context.UserSspm where u.Username.Equals(loggedInUserName) select u).FirstOrDefault();

            if (userid.UserId != id)
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> EditUser(string id)
        {
            if (!checkuser2(id))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();
            if (id == null)
            {
                return NotFound();
            }

            var userSspm = await context.UserSspm.SingleOrDefaultAsync(m => m.UserId == id);
            var userAssign = await context.UserAssignGroup.SingleOrDefaultAsync(m => m.UserId == id);

            var e = new ShowUserInputModel()
            {
                UserId = userSspm.UserId,
                Username = userSspm.Username,
                Password = userSspm.Password,
                Firstname = userSspm.Firstname,
                Lastname = userSspm.Lastname,
                JobResponsible = userSspm.JobResponsible,
                Status = userSspm.Status,
                GroupId = userAssign.GroupId,
                UserTel = userSspm.UserTel,
                LineId = userSspm.LineId

            };
            ViewData["Username"] = userSspm.Username;
            if (userSspm == null)
            {
                return NotFound();
            }
            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, EditUserInputModel editModel)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            ViewBag.userMenu = GetMenu();
            ViewBag.nothi = Nothi();


            var query = (from x in context.UserSspm where x.UserId.Equals(id) select x).FirstOrDefault();
            if (id != query.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    //context.Update(ord);
                    var addquery = from test in context.UserSspm
                                   where test.UserId.Equals(id)
                                   select test;
                    foreach (UserSspm UserUpdate in addquery)
                    {
                        UserUpdate.Username = editModel.Username;
                        UserUpdate.Password = editModel.Password;
                        UserUpdate.Firstname = editModel.Firstname;
                        UserUpdate.Lastname = editModel.Lastname;
                        UserUpdate.JobResponsible = editModel.JobResponsible;
                        UserUpdate.UserEditBy = loggedInUserName;
                        UserUpdate.UserEditDate = DateTime.Now;
                        UserUpdate.Status = editModel.Status;
                        UserUpdate.LineId = editModel.LineId;
                        UserUpdate.UserTel = editModel.UserTel;

                    }
                    await context.SaveChangesAsync();
                    try
                    {
                        var addquery2 = from test2 in context.UserAssignGroup
                                        where test2.UserId.Equals(id)
                                        select test2;
                        foreach (UserAssignGroup UserUpdate2 in addquery2)
                        {
                            UserUpdate2.GroupId = editModel.GroupId;
                            UserUpdate2.UserId = id;

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
                    if (!UserSspmExists(query.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(query.UserId);
        }


    }
}
