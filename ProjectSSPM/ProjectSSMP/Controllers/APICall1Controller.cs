using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSSMP.Models;
using ProjectSSMP.Models.api;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using SSMP.Models;
using SSMP.Models.UserManagenent;

namespace ProjectSSMP.Controllers
{
    [Produces("application/json")]
    //[Route("api/APICall1")]
    public class APICall1Controller : Controller
    {
        private sspmContext context;

        private static string readTokenkey = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIwOTg3NjU0MzIxIiwibmFtZSI6InR5Y2hlVG9rZW5SZWFkIiwiYWRtaW4iOnRydWUsImp0aSI6Ijk3Zjk3NzVlLTAyNjYtNDdjNC05ODU0LWZiZDQ5NmJhZDVjZSIsImlhdCI6MTUyMjM3MzM0OCwiZXhwIjoxNTIyMzc3MDM4fQ.rDQcbjacQ6tqcDosmB9Y8-QY-2H7CkDlwlwcK6KczIo";
        private static string writeTokenkey = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIwOTg3NjU0MzIxIiwibmFtZSI6InR5Y2hlVG9rZW5Xcml0ZSIsImFkbWluIjp0cnVlLCJqdGkiOiI5N2Y5Nzc1ZS0wMjY2LTQ3YzQtOTg1NC1mYmQ0OTZiYWQ1Y2UiLCJpYXQiOjE1MjIzNzMzNDgsImV4cCI6MTUyMjM3NzA2M30.qa_Bjn69l-8MQcxwN767W_MxMifsUgwTAYSsI4Dc35k";

        //Setup JWT Token
        private static string tokenKey = string.Empty;
        public APICall1Controller(sspmContext context) => this.context = context;


        //------------------------------------------ Authentication Service Session --------------------------------------------------//
        //Authentication
        public JsonResult Identity([FromBody]CustomLogin itemModel)
        {
            if (validateuser(itemModel.username, itemModel.password) != true)
            {
                return Json(new { success = false, msg = "ไม่สามารถดึงข้อมูลได้ กรุณาทำรายการใหม่" });
            }
            else
            {
                AccountInfo accoutInfo = new AccountInfo();
                var user = (from data in context.UserSspm where data.Username == itemModel.username select data).SingleOrDefault();
                var group = (from data in context.UserAssignGroup where data.UserId == user.UserId select data.GroupId).SingleOrDefault();

                accoutInfo.accountId = user.UserId;
                accoutInfo.accountName = user.Firstname + " " + user.Lastname;
                accoutInfo.accountPosition = user.JobResponsible;
                accoutInfo.accountGroup = group;

                return Json(new { success = true, accoutInfo });
            }
        }
        private bool validateuser(string user, string pass)
        {
            var userid = (from u in context.UserSspm where u.Username.Equals(user) select u).FirstOrDefault();
            if (userid == null)
            {
                return false;
            }

            else if (userid.Password != pass)
            {
                return false;
            }
            else if (checkstatususer(user) != true)
            {
                return false;
            }

            return true;
        }
        //Check StatusUser
        private bool checkstatususer(string user)
        {
            var userid = (from u in context.UserSspm where u.Username.Equals(user) select u).FirstOrDefault();
            if (userid.Status == "D")
            {
                return false;
            }

            return true;
        }


        //------------------------------------------------------- Account Management ---------------------------------------------------------//

        //Get All Account
        [HttpGet]
        public IEnumerable<UserSspm> GetAllEmployee()
        {
            var result = (from data in context.UserSspm orderby data.Status select data).ToList();
            return result;
        }



        public async Task<IActionResult> GetEmployee()
        {
            List<CustomAccountList> account = new List<CustomAccountList>();
            var user = (from data in context.UserSspm orderby data.Status
                       join data2 in context.UserAssignGroup on data.UserId equals data2.UserId
                       join data3 in context.UserGroup on data2.GroupId equals data3.GroupId
                       join data4 in context.UserImage on data.UserId equals data4.UserId
                       select new {
                            id = data.UserId,
                            username = data.Username,
                            password = data.Password,
                            firstname = data.Firstname,
                            lastname = data.Lastname,
                            userCreateBy = data.UserCreateBy,
                            userCreateDate = data.UserCreateDate,
                            userEditBy = data.UserEditBy,
                            userEditDate = data.UserEditDate,
                            groupId = data2.GroupId,
                            groupName = data3.GroupName,
                            jobResponsible = data.JobResponsible,
                            status = data.Status,
                            lineId = data.LineId,
                            userTel = data.UserTel,
                            image = data4.Image
                       }).ToList();

            foreach(var item in user)
            {
                account.Add(
                    new CustomAccountList
                    {
                        userId = item.id,
                        username = item.username,
                        password = item.password,
                        firstname = item.firstname,
                        lastname = item.lastname,
                        userCreateBy = item.userCreateBy,
                        userCreateDate = item.userCreateDate,
                        userEditBy = item.userEditBy,
                        userEditDate = item.userEditDate,
                        groupId = item.groupId,
                        groupName = item.groupName,
                        jobResponsible = item.jobResponsible,
                        status = item.status,
                        lineId = item.lineId,
                        userTel = item.userTel,
                        base64Image = item.image

                    }
                    );

            }
            return Json(account);
        }


        //Get Group

        public IEnumerable<UserGroup> GetAllGroup()
        {
            var result = (from data in context.UserGroup orderby data.GroupId select data).ToList();
            return result;
        }

        //Get Status
        public async Task<IActionResult> GetStatus()
        {
            List<CustomAccountStatus> stat = new List<CustomAccountStatus>();
            var result = (from data in context.UserSspm
                          where data.Status != null
                          group data by data.Status into g
                          select new
                          {
                              Status = g.Key,
                              Count = g.Count()
                          }).ToList();
              foreach(var item in result)
                {
                if (item != null)
                {

                    if (item.Status == "A")
                    {
                        stat.Add(new CustomAccountStatus
                        {
                            statusCode = item.Status,
                            statusName = "Active"
                        });
                    }
                    else
                    {
                        stat.Add(new CustomAccountStatus
                        {
                            statusCode = item.Status,
                            statusName = "DeActive"
                        });
                    }



                }

            }
            return Json( stat );
        }
        //Get user Detail
        public async Task<IActionResult> GetUserDetail(string id)
        {
            string userInfo = null;

            if (string.IsNullOrEmpty(id))
            {
                return Json(userInfo);
            }
            else
            {
                try
                {

                    var userSspm = await context.UserSspm.SingleOrDefaultAsync(m => m.UserId == id);
                    if (userSspm != null)
                    {
                        var userAssign = await context.UserAssignGroup.SingleOrDefaultAsync(m => m.UserId == id);
                        var Image = await context.UserImage.SingleOrDefaultAsync(im => im.UserId == id);

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
                        var userResult = new CustomAccountList()
                        {
                            userId = userSspm.UserId,
                            username = userSspm.Username,
                            password = userSspm.Password,
                            firstname = userSspm.Firstname,
                            lastname = userSspm.Lastname,
                            jobResponsible = userSspm.JobResponsible,
                            status = check,
                            groupName = groupname.GroupName,
                            groupId = userAssign.GroupId,
                            userCreateDate = userSspm.UserCreateDate,
                            userEditDate = userSspm.UserEditDate,
                            userCreateBy = userSspm.UserCreateBy,
                            userEditBy = userSspm.UserEditBy,                      
                            userTel = userSspm.UserTel,
                            lineId = userSspm.LineId,
                            base64Image = Image.Image
                        };                  
                        return Json(userResult);
                    }
                    else
                    {
                        return Json(userInfo);
                    }
                } catch (Exception ex)
                {
                    return Json(userInfo);
                }
                
            }
        }
    

        //Admin Adduser
        [HttpPost]
        public async Task<IActionResult> AddTodoUser([FromBody]CustomTodoAccount itemAccount)
        {
            try
            {
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
                        Username = itemAccount.username,
                        Password = itemAccount.password,
                        Firstname = itemAccount.firstname,
                        Lastname = itemAccount.lastname,
                        JobResponsible = itemAccount.position,
                        UserCreateBy = itemAccount.userLogged,
                        UserCreateDate = DateTime.Now,
                        LineId = itemAccount.lineId,
                        UserTel = itemAccount.Tel,
                        Status = "A"
                    };

                    UserAssignGroup ord2 = new UserAssignGroup
                    {
                        UserId = userid.ToString(),
                        GroupId = itemAccount.groupId,
                    };

                UserImage newImage = new UserImage
                {
                    UserId = userid.ToString(),
                    Image = itemAccount.acImage,
                    ImageNumber = userid
                };
                    // Add the new object to the Orders collection.
                    context.UserSspm.Add(ord);
                    await context.SaveChangesAsync();

                    context.UserAssignGroup.Add(ord2);
                    await context.SaveChangesAsync();

                context.UserImage.Add(newImage);
                context.SaveChanges();

                    var query = from num in context.RunningNumber where num.Type.Equals("UserID") select num;

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

                return Json (new { success = true});
            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { success = false, message = ex.Message }));
            }
        }

        //Admin Edit User
        [HttpPut]
        public async Task<IActionResult> UpdateTodoAccount(string id,[FromBody]CustomUpdateAccount itemUpdate)
        {

            var query = (from x in context.UserSspm where x.UserId.Equals(id) select x).FirstOrDefault();
            if (id != query.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var todoUser  =  context.UserSspm.FirstOrDefault(t => t.UserId == id);
                    var todoGroup =  context.UserAssignGroup.FirstOrDefault(t => t.UserId == id);

                    todoUser.Username = itemUpdate.username;
                    todoUser.Password = itemUpdate.password;
                    todoUser.Firstname = itemUpdate.firstname;
                    todoUser.Lastname = itemUpdate.lastname;
                    todoUser.UserEditBy = itemUpdate.userLogged;
                    todoUser.UserEditDate = DateTime.Now;
                    todoUser.JobResponsible = itemUpdate.position;
                    todoUser.Status = itemUpdate.stat;
                    todoUser.LineId = itemUpdate.lineId;
                    todoUser.UserTel = itemUpdate.Tel;

                    todoGroup.GroupId = itemUpdate.groupId;

                    context.UserSspm.Update(todoUser);
                    context.UserAssignGroup.Update(todoGroup);
                    await context.SaveChangesAsync();

                    return Json(new { success= true });
                }                              
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false });
                }                
            }
            else {
                return Json(new { success = false });
            }
        }


        ///------------------------------------------------- User Control --------------------------------------------------//

        [HttpPut]
        public async Task<IActionResult> UpdateTodoUser(string id, [FromBody]CustomEditAccount itemUpdate)
        {

            var query = (from x in context.UserSspm where x.UserId.Equals(id) select x).FirstOrDefault();
            if (id != query.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var todoUser = context.UserSspm.FirstOrDefault(t => t.UserId == id);

                    todoUser.Password = itemUpdate.password;
                    todoUser.Firstname = itemUpdate.firstname;
                    todoUser.Lastname = itemUpdate.lastname;
                    todoUser.UserEditBy = itemUpdate.userLogged;
                    todoUser.UserEditDate = DateTime.Now;
                    todoUser.LineId = itemUpdate.lineId;
                    todoUser.UserTel = itemUpdate.Tel;

                    context.UserSspm.Update(todoUser);
                    await context.SaveChangesAsync();

                    return Json(new { success = true});
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }





        /// ------------------------------------------------- User Image Control ---------------------------------------///
         [HttpPost]
        public async Task<IActionResult> UpdateUserImage([FromBody]CustomImage item)
        {
            var tblImage = (from data in context.UserImage where data.UserId == item.userId select data).SingleOrDefault();
            try
            {
                if (tblImage != null)
                {
                    tblImage.Image = item.Image;
                    tblImage.ImageNumber = Convert.ToInt32(item.userId);
                    context.Update(tblImage);
                    context.SaveChanges();


                }
                else
                {
                    UserImage newImage = new UserImage();
                    newImage.UserId = item.userId;
                    newImage.Image = item.Image;
                    newImage.ImageNumber = Convert.ToInt32(item.userId);
                    context.UserImage.Add(newImage);
                    context.SaveChanges();
                }
                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false});
            }
        }

        public async Task<IActionResult> GetMyImage(string id)
        {
            CustomImage myImg = new CustomImage();

            var contexData = (from data in context.UserImage where data.UserId == id select data).SingleOrDefault();
            if (contexData != null)
            {
                myImg.userId = contexData.UserId;
                myImg.Image = contexData.Image;
                myImg.imageNumber = contexData.ImageNumber;

                return Json(myImg);
            }
            else
            {
                return Json(myImg);
            }

            
        }
    }
}