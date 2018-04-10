using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSSMP.Models.api;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using ProjectSSMP.Models.api.NewTimesheet;
using SSMP.Models;
using SSMP.Models.api;

namespace ProjectSSMP.Controllers
{
    [Produces("application/json")]
    //[Route("api/APICall3")]
    public class APICall3Controller : Controller
    {
        private sspmContext context;

        private static string readTokenkey = "rDQcbjacQ6tqcDosmB9Y8-QY-2H7CkDlwlwcK6KczIo";
        private static string writeTokenkey = "qa_Bjn69l-8MQcxwN767W_MxMifsUgwTAYSsI4Dc35k";
        //Setup JWT Token
        private static string tokenKey = string.Empty;
        public APICall3Controller(sspmContext context) => this.context = context;



        ///--------------------------------- Timesheet Services ---------------------------///
        [HttpGet]
        public IEnumerable<TeamTask> FilterTeam()
        {

            var _filterdata = (from fdata in context.TeamTask select fdata).ToList();
            return _filterdata;

        }
        [HttpPost]
        public async Task<IActionResult> FilterTimesheetProject(string id)
        {
            List<CustomProject> FilProject = new List<CustomProject>();
            var result = (from data in context.TeamTask
                          where data.UserId == id
                          group data by data.ProjectNumber into g
                          select new
                          {
                              ProjectNumber = g.Key,
                              Count = g.Count()
                          }).ToList();

            foreach (var item in result)
            {
                var items = (from data in context.Project where data.ProjectNumber == item.ProjectNumber && data.ActualEnd == null select data).SingleOrDefault();
                var user = (from data2 in context.UserSspm where data2.UserId == items.ProjectManager select data2).SingleOrDefault();

                FilProject.Add(new CustomProject
                {
                    ProjectNumber = items.ProjectNumber,
                    ProjectName = items.ProjectName,
                    ProjectStart = items.ProjectStart,
                    ProjectEnd = items.ProjectEnd,
                    ProjectManagerId = items.ProjectManager,
                    ProjectManagerName = user.Firstname + " " + user.Lastname,
                    ProjectCost = items.ProjectCost,
                    CustomerName = items.CustomerName,
                    ProjectStatus = items.ProjectStatus

                });
            }
            return Ok(Json(new { FilProject }));

        }

        public IEnumerable<CustomApproveTimesheet> ListApprovTimesheet(string uid)
        {
            List<CustomApproveTimesheet> lister = new List<CustomApproveTimesheet>();
            TimeSpan? temp1;
            TimeSpan? temp2;
            TimeSpan? temp3;
            int hr = 0;
            int mn = 0;
            int totalHr = 0;
            int totalMn = 0;

            var result = (from data in context.TimeSheet
                          join data2 in context.Project on data.ProjectNumber equals data2.ProjectNumber
                          join data3 in context.Task on data.TaskId equals data3.TaskId
                          join data4 in context.Function on data.FunctionId equals data4.FunctionId
                          join data5 in context.Action on data.ActionId equals data5.ActionId
                          join data6 in context.UserSspm on data.UserId equals data6.UserId
                          join data7 in context.UserImage on data.UserId equals data7.UserId
                          where data2.ProjectManager.Equals(uid) && data.ActionId.Equals("Z") && data.Approve1.Equals(null) && data.Approve2.Equals(null)
                          orderby data.TimeSheetId descending
                          select new
                          {
                              sub = data.TimeSheetId,
                              proId = data.ProjectNumber,
                              proName = data2.ProjectName,
                              taskId = data.TaskId,
                              taskName = data3.TaskName,
                              funcId = data.FunctionId,
                              funcName = data4.FunctionName,
                              actId = data.ActionId,
                              actName = data5.ActionName,
                              uid = data.UserId,
                              fname = data6.Firstname,
                              lname = data6.Lastname,
                              str = data.TimeSheetStart,
                              ste = data.TimeSheetEnd,
                              image = data7.Image
                          }).ToList();



            foreach (var item in result)
            {

                // temp1 = item.str?.TimeOfDay;
                ///temp2 = item.ste?.TimeOfDay;
                //temp3 = temp2 - temp1;
                //hr = temp3.Value.Hours;
                //mn = temp3.Value.Minutes;

                var result2 = (from tmp in context.TimeSheet where tmp.UserId.Equals(item.uid) && tmp.ProjectNumber.Equals(item.proId) && tmp.TaskId.Equals(item.taskId) && tmp.FunctionId.Equals(item.funcId) select tmp).ToList();
                foreach (var items in result2)
                {
                    temp1 = items.TimeSheetStart?.TimeOfDay;
                    temp2 = items.TimeSheetEnd?.TimeOfDay;
                    temp3 = temp2 - temp1;
                    hr += temp3.Value.Hours;
                    mn += temp3.Value.Minutes;
                }

                lister.Add(new CustomApproveTimesheet()
                {
                    submitDate = item.sub,
                    projectId = item.proId,
                    projectName = item.proName,
                    taskId = item.taskId,
                    tackName = item.taskName,
                    functionId = item.funcId,
                    functionName = item.funcName,
                    actionId = item.actId,
                    actionName = item.actName,
                    userId = item.uid,
                    fullName = item.fname + " " + item.lname,
                    // timeStart = item.str,
                    // timeEnd = temp2,
                    durationHrs = hr,
                    durationMns = mn,
                    base64Image = item.image
                });
                hr = 0;
                mn = 0;
            }
            return lister;

        }

        public async Task<IActionResult> approveAction([FromBody]CustomNewApprove itemUpdate)
        {


            if (itemUpdate == null)
            {
                return Json(new { success = false });
            }

            else
            {
                var todo = context.TimeSheet.FirstOrDefault(t =>
                              t.TimeSheetId == itemUpdate.timesheetId);
                try
                {
                    if (todo.Approve1 == null && todo.Approve2 == null)
                        todo.Approve1 = itemUpdate.userId;
                    else if (todo.Approve1 != null && todo.Approve2 == null)
                        todo.Approve2 = itemUpdate.userId;

                    context.TimeSheet.Update(todo);
                    context.SaveChanges();
                }
                catch { return Json(new { success = false }); }

                try
                {
                    var todoFuncLog = context.FunctionLog.FirstOrDefault(l => l.FunctionId == itemUpdate.functionId);

                    if (todoFuncLog != null)
                    {
                        var checkerFunc = context.Function.FirstOrDefault(f => f.FunctionId == itemUpdate.functionId);

                        DateTime actstat = (DateTime)todoFuncLog.ActualStart;
                        DateTime actend = (DateTime)todo.TimeSheetEnd;

                        todoFuncLog.StatusId = "F";
                        todoFuncLog.ActualEnd = todo.TimeSheetEnd;
                        todoFuncLog.Approve1 = itemUpdate.userId;
                        todoFuncLog.Variant = (int)actend.Subtract(actstat).TotalDays;
                        context.FunctionLog.Update(todoFuncLog);
                        context.SaveChanges();

                        checkerFunc.ActualStart = todoFuncLog.ActualStart;
                        checkerFunc.ActualEnd = todo.TimeSheetEnd;
                        checkerFunc.Variant = (int)actend.Subtract(actstat).TotalDays;

                        context.Function.Update(checkerFunc);
                        context.SaveChanges();

                    }
                }
                catch { return Json(new { success = false }); }

                try
                {
                    var checkFunctionFinished = (from data in context.Function where data.FunctionId == itemUpdate.functionId && data.ActualEnd == null select data).Count();
                    var todoTask = (from data2 in context.Task where data2.TaskId == itemUpdate.taskId select data2).FirstOrDefault();
                    var todoProject = (from data3 in context.Project where data3.ProjectNumber == itemUpdate.projectId select data3).FirstOrDefault();
                    var rangerTask = (from data in context.FunctionLog where data.ProjectNumber == itemUpdate.projectId && data.TaskId == itemUpdate.taskId orderby data.ActualEnd descending select data.ActualEnd).FirstOrDefault();

                    if (checkFunctionFinished == 0)
                    {
                        todoTask.ActualEnd = todo.TimeSheetEnd;
                        DateTime Tactstat = (DateTime)todoTask.ActualStart;
                        DateTime Tactend = (DateTime)todoTask.ActualEnd;
                        todoTask.Variant = (int)Tactstat.Subtract(Tactend).TotalDays;
                        context.Task.Update(todoTask);
                        context.SaveChanges();
                    }
                    var checkTaskFinished = (from data2 in context.Task where data2.ProjectNumber == itemUpdate.projectId && data2.ActualEnd == null select data2).Count();
                    if (checkTaskFinished == 0)
                    {
                        todoProject.ActualEnd = todo.TimeSheetEnd;
                        DateTime Pactstat = (DateTime)todoProject.ActualStart;
                        DateTime Pactend = (DateTime)todoProject.ActualEnd;
                        todoProject.Variant = (int)Pactstat.Subtract(Pactend).TotalDays;
                        context.Project.Update(todoProject);
                        context.SaveChanges();
                    }
                }
                catch { return Json(new { success = false }); }

                return Json(new { success = true });
            }

        }


        [HttpPost]
        public async Task<IActionResult> listTimesheet([FromBody]CustomGetTimesheet itemGet)
        {
            List<CustomListTimesheet> timesheetResult = new List<CustomListTimesheet>();

            if (string.IsNullOrEmpty(itemGet.token) || itemGet.token != readTokenkey)
            {
                return Json(timesheetResult);
            }
            else
            {
               
                var result =
                           (from data in context.TimeSheet
                            where data.UserId.Equals(itemGet.userId)
                            join data2 in context.Project on data.ProjectNumber equals data2.ProjectNumber
                            join data3 in context.Task on data.TaskId equals data3.TaskId
                            join data4 in context.Function on data.FunctionId equals data4.FunctionId
                            join data5 in context.Action on data.ActionId equals data5.ActionId
                            orderby data.TimeSheetId descending
                            select new
                            {
                                timeId = data.TimeSheetId,
                                proName = data2.ProjectName,
                                taskName = data3.TaskName,
                                funcName = data4.FunctionName,
                                actName = data5.ActionName,
                                approve1 = data.Approve1,
                                approve2 = data.Approve2,
                                inTime = data.TimeSheetStart,
                                outTime = data.TimeSheetEnd,

                            }).Skip(itemGet.pageIndex * itemGet.pageSize).Take(itemGet.pageSize).ToList();
                TimeSpan? calculated;
                string temp;

                foreach (var item in result)
                {
                    var temp1 = (from appr1 in context.UserSspm where item.approve1.Equals(appr1.UserId) select appr1).SingleOrDefault();
                    var temp2 = (from appr2 in context.UserSspm where item.approve2.Equals(appr2.UserId) select appr2).SingleOrDefault();


                    if (temp1 == null && temp2 == null)
                    {
                        calculated = item.outTime - item.inTime;
                        temp = calculated.ToString();
                        timesheetResult.Add(new CustomListTimesheet
                        {

                            timesheetId = item.timeId,
                            projectName = item.proName,
                            taskName = item.taskName,
                            functionName = item.funcName,
                            actionName = item.actName,
                            approve1 = "-/-",
                            approve2 = "-/-",
                            timeSheetStart = item.inTime,
                            timeSheetEnd = item.outTime,
                            duration = string.Concat(temp.Reverse().Skip(3).Reverse()),

                        });

                    }
                    else if (temp2 == null)
                    {
                        calculated = item.outTime - item.inTime;
                        temp = calculated.ToString();
                        timesheetResult.Add(new CustomListTimesheet
                        {
                            timesheetId = item.timeId,
                            projectName = item.proName,
                            taskName = item.taskName,
                            functionName = item.funcName,
                            actionName = item.actName,
                            approve1 = temp1.Firstname + " " + temp1.Lastname,
                            approve2 = "-/-",
                            timeSheetStart = item.inTime,
                            timeSheetEnd = item.outTime,
                            duration = string.Concat(temp.Reverse().Skip(3).Reverse()),

                        });
                    }
                    else
                    {
                        calculated = item.outTime - item.inTime;
                        temp = calculated.ToString();
                        timesheetResult.Add(new CustomListTimesheet
                        {
                            timesheetId = item.timeId,
                            projectName = item.proName,
                            taskName = item.taskName,
                            functionName = item.funcName,
                            actionName = item.actName,
                            approve1 = temp1.Firstname + " " + temp1.Lastname,
                            approve2 = temp2.Firstname + " " + temp2.Lastname,
                            timeSheetStart = item.inTime,
                            timeSheetEnd = item.outTime,
                            duration = string.Concat(temp.Reverse().Skip(3).Reverse()),
                        });
                    }
                }
                return Json(timesheetResult);
            }
             
        }

        //MaxLoadTimesheet
        public async Task<IActionResult> maxLoadTimesheet(string userId)
        {
            var maxLoad = (from data in context.TimeSheet where data.UserId == userId && data.ActionId != null select data).Count();
            return Json(new { maxLoad});
        }

        public async Task<IActionResult> FilterTimesheetTasktById(string id, string userId)
        {
            List<SSMP.Models.Task> FilTask = new List<SSMP.Models.Task>();
            var result = (from data in context.TeamTask
                          where data.UserId == userId && data.ProjectNumber == id
                          group data by data.TaskId into g
                          select new
                          {
                              TaskId = g.Key,
                              Count = g.Count()
                          }).ToList();

            foreach (var item in result)
            {
                var items = (from data in context.Task where data.TaskId == item.TaskId && data.ActualEnd == null select data).SingleOrDefault();

                FilTask.Add(new SSMP.Models.Task
                {
                    ProjectNumber = items.ProjectNumber,
                    TaskId = items.TaskId,
                    TaskName = items.TaskName,
                    TaskStart = items.TaskStart,
                    TaskEnd = items.TaskEnd,
                    ActualStart = items.ActualStart,
                    ActualEnd = items.ActualEnd,
                    Variant = items.Variant


                });
            }
            return Ok(Json(new { FilTask }));

        }

        public async Task<IActionResult> FilterFunctionById(string id, string userId)
        {
            List<Function> FilFunction = new List<Function>();
            var result = (from data in context.TeamTask where data.UserId == userId && data.TaskId == id select data).ToList();
            foreach (var item in result)
            {
                var items = (from data in context.Function where data.FunctionId == item.FunctionId && data.ActualEnd == null select data).SingleOrDefault();

                FilFunction.Add(new Function
                {
                    ProjectNumber = items.ProjectNumber,
                    TaskId = items.TaskId,
                    FunctionId = items.FunctionId,
                    FunctionName = items.FunctionName,
                    FunctionStart = items.FunctionStart,
                    FunctionEnd = items.FunctionEnd,
                    ActualStart = items.ActualStart,
                    ActualEnd = items.ActualEnd,
                    Variant = items.Variant,
                    Age = items.Age
                });
            }
            return Ok(Json(new { FilFunction }));
        }

        [HttpGet]
        public IEnumerable<SSMP.Models.Action> GetAction()
        {
            var act = (from acResult in context.Action select acResult).ToList();
            return act;
        }

        //---------------------------------- Record Timesheet -------------------------------//
        [HttpPost]
        public async Task<IActionResult> FilterMyTimesheet(string itemId)
        {
            List<SelectProject> itemProject = new List<SelectProject>();
            List<SelectTask> itemTask = new List<SelectTask>();
            List<SelectFunction> itemFunction = new List<SelectFunction>();

            var result = (from data in context.TeamTask
                          where data.UserId == itemId
                          group data by data.ProjectNumber into g
                          select new
                          {
                              ProjectNumber = g.Key,
                              Count = g.Count()
                          }).ToList();
            foreach (var item in result)
            {
                var items = (from data in context.Project where data.ProjectNumber == item.ProjectNumber select data).SingleOrDefault();
                if (items != null)
                {
                    itemProject.Add(new SelectProject
                    {
                        projectId = item.ProjectNumber,
                        projectName = items.ProjectName

                    });
                }

            }

            var result2 = (from data in context.TeamTask
                           where data.UserId == itemId
                           group data by data.TaskId into g
                           select new
                           {
                               TaskId = g.Key,
                               Count = g.Count()
                           }).ToList();

            foreach (var item in result2)
            {
                var items = (from data in context.Task where data.TaskId == item.TaskId select data).SingleOrDefault();

                if (items != null)
                {
                    itemTask.Add(new SelectTask
                    {
                        projectId = items.ProjectNumber,
                        taskId = item.TaskId,
                        taskName = items.TaskName,

                    });
                }
            }


            var result3 = (from data in context.TeamTask
                           where data.UserId == itemId
                           group data by data.FunctionId into g
                           select new
                           {
                               FunctionId = g.Key,
                               Count = g.Count()
                           }).ToList();

            foreach (var item in result3)
            {
                var items = (from data in context.Function where data.FunctionId == item.FunctionId && data.ActualEnd == null select data).SingleOrDefault();

                if (items != null)
                {
                    itemFunction.Add(new SelectFunction
                    {
                        projectId = items.ProjectNumber,
                        taskId = items.TaskId,
                        functionStart = items.FunctionStart,
                        functionId = item.FunctionId,
                        functionName = items.FunctionName,

                    });
                }
            }


            return Json(new { projectResp = itemProject, taskResp = itemTask, functionResp = itemFunction });

        }

        public async Task<IActionResult> RecordTimesheet([FromBody]RecordTimesheet itemTimesheet)
        {
            DateTime strDate = DateTime.ParseExact(itemTimesheet.timeSheetStart, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(itemTimesheet.TimeSheetEnd, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            try
            {
                var FilterId = (from data in context.TimeSheet
                                where (data.UserId.Equals(itemTimesheet.userId) && data.ProjectNumber.Equals(itemTimesheet.projectNumber) && data.TaskId.Equals(itemTimesheet.taskId)
                                       && data.FunctionId.Equals(itemTimesheet.functionId))
                                orderby data.TimeSheetNumber descending
                                select data.TimeSheetNumber).FirstOrDefault();
                int tempid;
                int realId;
                if (FilterId != null)
                {
                    tempid = Convert.ToInt32(FilterId);
                    realId = tempid + 1;
                }
                else
                {
                    realId = 100000;
                }
                TimeSheet dataContext = new TimeSheet
                {
                    TimeSheetId = DateTime.Now,
                    ActionId = itemTimesheet.actionId,
                    TimeSheetStart = strDate,
                    TimeSheetEnd = endDate,
                    UserId = itemTimesheet.userId,
                    FunctionId = itemTimesheet.functionId,
                    TaskId = itemTimesheet.taskId,
                    ProjectNumber = itemTimesheet.projectNumber,
                    TimeSheetNumber = realId.ToString()
                };
                context.TimeSheet.Add(dataContext);
                await context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { success = false }));
            }


        }

        public IEnumerable<CustomConfirmTimesheet> ListMyConfirmTimesheet(string itemId)
        {
            List<CustomConfirmTimesheet> lister = new List<CustomConfirmTimesheet>();
            TimeSpan? temp1;
            TimeSpan? temp2;
            TimeSpan? temp3;
            TimeSpan? temp4;
            TimeSpan? temp5;
            int hr = 0;
            int mn = 0;
            string shr;
            string smn;
            string strTS;
            string strTE;
            int totalHr = 0;
            int totalMn = 0;

            var result = (from data in context.TimeSheet
                          where data.UserId == itemId && data.ActionId == null
                          join data2 in context.Project on data.ProjectNumber equals data2.ProjectNumber
                          join data3 in context.Task on data.TaskId equals data3.TaskId
                          join data4 in context.Function on data.FunctionId equals data4.FunctionId
                          join data6 in context.UserSspm on data.UserId equals data6.UserId
                          orderby data.TimeSheetId descending
                          select new
                          {
                              sub = data.TimeSheetId,
                              proId = data.ProjectNumber,
                              proName = data2.ProjectName,
                              taskId = data.TaskId,
                              taskName = data3.TaskName,
                              funcId = data.FunctionId,
                              funcName = data4.FunctionName,
                              uid = data.UserId,
                              fname = data6.Firstname,
                              lname = data6.Lastname,
                              timeId = data.TimeSheetId,
                              timN = data.TimeSheetNumber,
                              DSelect = data.TimeSheetStart,
                              str = data.TimeSheetStart,
                              ste = data.TimeSheetEnd
                          }).ToList();



            foreach (var item in result)
            {

                temp4 = item.str?.TimeOfDay;
                temp5 = item.ste?.TimeOfDay;
                temp1 = item.str?.TimeOfDay;
                temp2 = item.ste?.TimeOfDay;
                temp3 = temp2 - temp1;
                hr = temp3.Value.Hours;
                mn = temp3.Value.Minutes;

                var result2 = (from tmp in context.TimeSheet where tmp.UserId.Equals(item.uid) && tmp.ProjectNumber.Equals(item.proId) && tmp.TaskId.Equals(item.taskId) && tmp.FunctionId.Equals(item.funcId) select tmp).ToList();
                foreach (var items in result2)
                {
                    temp1 = items.TimeSheetStart?.TimeOfDay;
                    temp2 = items.TimeSheetEnd?.TimeOfDay;
                   // temp3 = temp2 - temp1;
                   // hr = temp3.Value.Hours;
                   // mn = temp3.Value.Minutes;
                }
                
                
                shr = hr.ToString();
                smn = mn.ToString();
                strTS = temp4.ToString();
                strTE = temp5.ToString();

                lister.Add(new CustomConfirmTimesheet()
                {
                    submitDate = item.sub,
                    projectNumber = item.proId,
                    projectName = item.proName,
                    taskId = item.taskId,
                    tackName = item.taskName,
                    functionId = item.funcId,
                    functionName = item.funcName,
                    userId = item.uid,
                    fullName = item.fname + " " + item.lname,
                    timesheetId = item.timeId,
                    timeNumber = item.timN,
                    DateSelect = item.DSelect,
                    timeStart = temp4,
                    timeEnd = temp5,
                    strTimeStart = strTS,
                    strTimeEnd = strTE,
                    duration = string.Concat(shr, ".", smn, " ", "Hrs")
                });
                hr = 0;
                mn = 0;
            }
            return lister;
        }

        public async Task<IActionResult> ConfirmAction([FromBody]RecordTimesheet itemTodo)
        {
            FunctionLog newOrder = new FunctionLog();


            if (itemTodo.timesheetId == null || itemTodo.actionId == null)
            {
                return Json(new { success = false });
            }

            try
            {
                var todo = context.TimeSheet.FirstOrDefault(t => t.TimeSheetId == itemTodo.timesheetId);
                if (todo == null)
                {
                    return Json(new { success = false });
                }
                else if (todo != null && itemTodo.actionId != null)
                {
                    var counterFirst = (from counter in context.TimeSheet where counter.TimeSheetId == itemTodo.timesheetId select counter).Count();
                    if (counterFirst == 1)
                    {
                        var todoFuncLog = context.FunctionLog.FirstOrDefault(l => l.FunctionId == itemTodo.functionId
                           && l.TaskId == itemTodo.taskId && l.ProjectNumber == itemTodo.projectNumber);

                        var checkerFunc = context.Function.FirstOrDefault(f => f.TaskId == itemTodo.taskId && f.FunctionId == itemTodo.functionId);

                        var todoTask = context.Task.FirstOrDefault(t => t.TaskId == itemTodo.taskId);

                        var todoProject = context.Project.FirstOrDefault(p => p.ProjectNumber == itemTodo.projectNumber);

                        if (todoFuncLog == null)
                        {


                            newOrder.FunctionId = todo.FunctionId;
                            newOrder.FunctionLogId = todo.TimeSheetId;
                            newOrder.ProjectNumber = todo.ProjectNumber;
                            newOrder.FunctionStart = checkerFunc.FunctionStart;
                            newOrder.FunctionEnd = checkerFunc.FunctionEnd;
                            newOrder.FunctionNumber = todo.TimeSheetNumber;
                            newOrder.ActualStart = todo.TimeSheetStart;
                            newOrder.StatusId = "P";
                            newOrder.TaskId = todo.TaskId;


                            todo.ActionId = itemTodo.actionId;
                            context.TimeSheet.Update(todo);
                            context.SaveChanges();

                            context.FunctionLog.Add(newOrder);
                            context.SaveChanges();

                            checkerFunc.ActualStart = todo.TimeSheetStart;
                            context.Function.Update(checkerFunc);
                            context.SaveChanges();

                            todoTask.ActualStart = todo.TimeSheetStart;
                            context.Task.Update(todoTask);
                            context.SaveChanges();

                            todoProject.ActualStart = todo.TimeSheetStart;
                            context.Project.Update(todoProject);
                            context.SaveChanges();

                            return Json(new { success = true });
                        }
                        else
                        {
                            todo.ActionId = itemTodo.actionId;
                            context.TimeSheet.Update(todo);
                            context.SaveChanges();

                            return Json(new { success = true });
                        }
                    }
                    else
                    {
                        todo.ActionId = itemTodo.actionId;
                        context.TimeSheet.Update(todo);
                        context.SaveChanges();
                        return Json(new { success = true });
                    }
                }

                else
                {
                    todo.ActionId = itemTodo.actionId;
                    context.TimeSheet.Update(todo);
                    context.SaveChanges();

                    return Json(new { success = true });

                }
            }

            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
    }
}