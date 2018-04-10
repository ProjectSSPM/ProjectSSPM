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
using SSMP.Models.api;

namespace ProjectSSMP.Controllers
{
    [Produces("application/json")]
    //[Route("api/APICall2")]
    public class APICall2Controller : Controller
    {
        private sspmContext context;


        private static string readTokenkey = "rDQcbjacQ6tqcDosmB9Y8-QY-2H7CkDlwlwcK6KczIo";
        private static string writeTokenkey = "qa_Bjn69l-8MQcxwN767W_MxMifsUgwTAYSsI4Dc35k";
        //Setup JWT Token
        private static string tokenKey = string.Empty;
        public APICall2Controller(sspmContext context) => this.context = context;

        //------------------------------------------ Get Project Service Session --------------------------------------------------//


        //Admin | Pm | Emp Get Project Detail
        //Get Project
        [HttpGet]
        public IEnumerable<CustomProject> GetProject()
        {
            List<CustomProject> projectList = new List<CustomProject>();
            var result = (from data in context.Project
                          join data2 in context.UserSspm on data.ProjectManager equals data2.UserId
                          select new
                          {

                              proId = data.ProjectNumber,
                              proN = data.ProjectId,
                              proName = data.ProjectName,
                              proManagerId = data.ProjectManager,
                              proManagerF = data2.Firstname,
                              proManagerL = data2.Lastname,
                              proStart = data.ProjectStart,
                              proEnd = data.ProjectEnd,
                              proCost = data.ProjectCost,
                              proCreby = data.ProjectCreateBy,
                              proCreDate = data.ProjectCreateDate,
                              proEditby = data.ProjectEditBy,
                              proEditdate = data.ProjectEditDate,
                              proCus = data.CustomerName,
                              proCusTel = data.CustomerTel,
                              proActStart = data.ActualStart,
                              proActEnd = data.ActualEnd,
                              proNote = data.Note,
                              proVar = data.Variant,
                              proStat = data.ProjectStatus
                          }).ToList();
            foreach (var item in result)
            {
                projectList.Add(new CustomProject
                {
                    ProjectNumber = item.proId,
                    ProjectId = item.proN,
                    ProjectName = item.proName,
                    ProjectManagerId = item.proManagerId,
                    ProjectManagerName = item.proManagerF + " " + item.proManagerL,
                    ProjectStart = item.proStart,
                    ProjectEnd = item.proEnd,
                    ProjectCost = item.proCost,
                    ProjectCreateBy = item.proCreby,
                    ProjectCreateDate = item.proCreDate,
                    ProjectEditBy = item.proEditby,
                    ProjectEditDate = item.proEditdate,
                    CustomerName = item.proCus,
                    CustomerTel = item.proCusTel,
                    ActualStart = item.proActStart,
                    ActualEnd = item.proActEnd,
                    Note = item.proNote,
                    Variant = item.proVar,
                    ProjectStatus = item.proStat
                });
            }
            return projectList;
        }
        [HttpPost]
        public async Task<IActionResult> GetProjectForManager(string userId)
        {
            string ProjectBG = "#3DD94A";
            DateTime temp = DateTime.Today;
            DateTime temp2;
            List<CustomProject2> itemer = new List<CustomProject2>();
            var result = (from data in context.Project
                          where data.ProjectManager == userId
                          join data2 in context.UserSspm on data.ProjectManager equals data2.UserId
                          select new
                          {

                              proId = data.ProjectNumber,
                              proN = data.ProjectId,
                              proName = data.ProjectName,
                              proManagerId = data.ProjectManager,
                              proManagerF = data2.Firstname,
                              proManagerL = data2.Lastname,
                              proStart = data.ProjectStart,
                              proEnd = data.ProjectEnd,
                              proCost = data.ProjectCost,
                              proCreby = data.ProjectCreateBy,
                              proCreDate = data.ProjectCreateDate,
                              proEditby = data.ProjectEditBy,
                              proEditdate = data.ProjectEditDate,
                              proCus = data.CustomerName,
                              proCusTel = data.CustomerTel,
                              proActStart = data.ActualStart,
                              proActEnd = data.ActualEnd,
                              proNote = data.Note,
                              proVar = data.Variant,
                              proStat = data.ProjectStatus
                          }).ToList();
            if (result != null)
            {
                foreach (var item in result)
                {
                    var tempQuery = context.Project.SingleOrDefault(tp => tp.ProjectNumber == item.proId && tp.ActualEnd == null);
                    var checkTask = context.Task.Count(tp => tp.ProjectNumber == tempQuery.ProjectNumber );
                    var tempFinish = context.Task.Count(tp => tp.ProjectNumber == tempQuery.ProjectNumber && tp.ActualEnd == null);
                    if (tempQuery != null)
                    {
                        temp2 = (DateTime)tempQuery.ProjectEnd;

                        if (temp2.Subtract(temp).TotalDays <= 1)
                            ProjectBG = "#F74933";
                        else if (temp2.Subtract(temp).TotalDays >= 2 && temp2.Subtract(temp).TotalDays <= 7)
                            ProjectBG = "#FFD500";
                        else if (checkTask == tempFinish)
                            ProjectBG = "#bdbdbd";
                        else
                            ProjectBG = "#3DD94A";
                    }

                    itemer.Add(new CustomProject2
                    {
                        ProjectNumber = item.proId,
                        ProjectId = item.proN,
                        ProjectName = item.proName,
                        ProjectManagerId = item.proManagerId,
                        ProjectManagerName = item.proManagerF + " " + item.proManagerL,
                        ProjectStart = item.proStart,
                        ProjectEnd = item.proEnd,
                        ProjectCost = item.proCost,
                        ProjectCreateBy = item.proCreby,
                        ProjectCreateDate = item.proCreDate,
                        ProjectEditBy = item.proEditby,
                        ProjectEditDate = item.proEditdate,
                        CustomerName = item.proCus,
                        CustomerTel = item.proCusTel,
                        ActualStart = item.proActStart,
                        ActualEnd = item.proActEnd,
                        Note = item.proNote,
                        Variant = item.proVar,
                        ProjectStatus = item.proStat,
                        bgColor = ProjectBG
                    });
                    ProjectBG = "#3DD94A";
                }
                return Json(new { itemer });
            
        }
        else
                return Json(new { itemer });
     }

[HttpGet]
        public async Task<IActionResult> GetProjectDetail(string id)
        {
            string projectInfo = null;

            if (string.IsNullOrEmpty(id))
            {
                return Json(new { isComplete = false, message = "Null Reference ID", projectInfo });
            }

            else
            {
                try
                {
                    var project = await context.Project.SingleOrDefaultAsync(p => p.ProjectNumber == id);

                    if (project != null)
                    {
                        var userSspm = await context.UserSspm.SingleOrDefaultAsync(m => m.UserId == project.ProjectManager);

                        var projectResult = new CustomProject()
                        {
                            ProjectNumber = project.ProjectNumber,
                            ProjectId = project.ProjectId,
                            ProjectName = project.ProjectName,
                            ProjectManagerId = userSspm.UserId,
                            ProjectManagerName = userSspm.Firstname + " " + userSspm.Lastname,
                            ProjectStart = project.ProjectStart,
                            ProjectEnd = project.ProjectEnd,
                            ProjectCost = project.ProjectCost,
                            ProjectCreateBy = project.ProjectCreateBy,
                            ProjectCreateDate = project.ProjectCreateDate,
                            ProjectEditBy = project.ProjectEditBy,
                            ProjectEditDate = project.ProjectEditDate,
                            CustomerName = project.CustomerName,
                            CustomerTel = project.CustomerTel,
                            ActualStart = project.ActualStart,
                            ActualEnd = project.ActualEnd,
                            Note = project.Note,
                            Variant = project.Variant,
                            ProjectStatus = project.ProjectStatus
                        };


                        return Json(new { isComplete = true, message = "Complete", projectInfo = projectResult });
                    }
                    else
                    {
                        return Json(new { isComplete = true, message = "Not Found Project : " + id, projectInfo });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { isComplete = false, message = ex.Message.ToString(), projectInfo });
                }

            }
        }

        //Get Task
        public IEnumerable<SSMP.Models.Task> GetTask()
        {
            return context.Task.ToList();
        }
        public IEnumerable<SSMP.Models.Task> GetTaskByProject(string id)
        {
            var result = (from data in context.Task where data.ProjectNumber == id select data).ToList();
            return result;
        }
        public async Task<IActionResult> GetTaskDetail(string id)
        {
            string taskInfo = null;

            if (string.IsNullOrEmpty(id))
            {
                return Json(new { isComplete = false, message = "Null Reference ID", taskInfo });
            }

            else
            {
                try
                {
                    var task = await context.Task.SingleOrDefaultAsync(p => p.TaskId == id);

                    if (task != null)
                    {
                        var taskResult = new SSMP.Models.Task()
                        {
                            ProjectNumber = task.ProjectNumber,
                            TaskId = task.TaskId,
                            TaskName = task.TaskName,
                            TaskStart = task.TaskStart,
                            TaskEnd = task.TaskEnd,
                            ActualStart = task.ActualStart,
                            ActualEnd = task.ActualEnd,
                            Variant = task.Variant
                        };


                        return Json(new { isComplete = true, message = "Complete", projectInfo = taskResult });
                    }
                    else
                    {
                        return Json(new { isComplete = true, message = "Not Found Project : " + id, taskInfo });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { isComplete = false, message = ex.Message.ToString(), taskInfo });
                }

            }

        }

        //Get Function
        public IEnumerable<Function> GetFunction()
        {
            return context.Function.ToList();
        }

        public IEnumerable<Function> GetFunctionByTask(string id)
        {
            var result = (from data in context.Function where data.TaskId == id select data).ToList();
            return result;
        }
     
        [HttpPost]
        public async Task<IActionResult> GetProjectLog(string itemIndex)
        {
            List<CustomTimelinesModel> TimelineLog = new List<CustomTimelinesModel>();
            var result = (from data in context.FunctionLog
                          where (data.ProjectNumber.Equals(itemIndex) && data.StatusId.Equals("F"))
                          join data2 in context.Task on data.TaskId equals data2.TaskId
                          join data3 in context.Function on data.FunctionId equals data3.FunctionId
                          join data4 in context.TeamTask on data.FunctionId equals data4.FunctionId
                          join data5 in context.UserSspm on data4.UserId equals data5.UserId
                          orderby data.ActualEnd descending
                          select new
                          {
                              logId = data.FunctionLogId,
                              funcStr = data.FunctionStart,
                              funcEnd = data.FunctionEnd,
                              actStr = data.ActualStart,
                              actEnd = data.ActualEnd,
                              commit = data5.Firstname,
                              taskName = data2.TaskName,
                              fucnName = data3.FunctionName
                          }).ToList();
            string tempName;
            foreach (var item in result)
            {
                if (item.actEnd != null)
                {
                    tempName = item.commit;
                }
                else
                {
                    tempName = "-/-";
                }

                TimelineLog.Add(new CustomTimelinesModel()
                {

                    functionLogId = item.logId,
                    functionStart = item.funcStr,
                    functionEnd = item.funcEnd,
                    userCommit = tempName,
                    actualStart = item.actStr,
                    actualEnd = item.actEnd,
                    taskName = item.taskName,
                    functionName = item.fucnName
                });


            }
            return Json(TimelineLog);
        }


       

        public async Task<IActionResult> CountItemForUser(string id)
        {
            int PCount = 0;
            int FCount = 0;
            DateTime? PDay = DateTime.Now;
            DateTime? FDay = DateTime.Now;
            DateTime itemmonth;
            DateTime today = DateTime.Now;
            TimeSpan? temp1;
            int hr = 0;
            int mn = 0;
            string strTS;
            string strTE;
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
                var itemCount = (from data in context.Project where data.ProjectNumber == item.ProjectNumber && data.ActualEnd == null select data).SingleOrDefault();
                if (itemCount != null)
                {
                    PCount += 1;

                }
            }

            var result2 = (from data in context.TeamTask
                           where data.UserId == id
                           group data by data.FunctionId into g
                           select new
                           {
                               FunctionId = g.Key,
                               Count = g.Count()
                           }).ToList();

            foreach (var item in result2)
            {
                var itemCount = (from data in context.Function where data.FunctionId == item.FunctionId && data.ActualEnd == null select data).SingleOrDefault();
                if (itemCount != null)
                {
                    FCount += 1;

                }
            }

            var getmonth = (from data in context.TimeSheet
                          where data.UserId == id
                            select data)
                          .ToList();
            foreach (var item in getmonth)
            {
                itemmonth = (DateTime)item.TimeSheetStart;
                if (itemmonth.Month == today.Month)
                {
                    temp1 = item.TimeSheetEnd?.TimeOfDay - item.TimeSheetStart?.TimeOfDay;
                    hr += temp1.Value.Hours;
                    mn += temp1.Value.Minutes;
                    
                    if(mn >= 60)
                    {
                        hr += 1;
                        mn -= 60;
                    }
                    
                }
            }

            if (mn < 10)
            {
                strTS = hr.ToString();
                strTE = "0" + mn.ToString();
            }
            else
            {
                strTS = hr.ToString();
                strTE = mn.ToString();
            }

            return Json(new { projectCount = PCount.ToString(), functionCount = FCount.ToString() , WorkTime = strTS + "." + strTE  });
        }

        //-------------------------------------------change BackGroud Color ----------------------------------------------------//
        public async Task<IActionResult> GetProjectByIdForManagerBG(string id)
        {
            int delta = 0;
            string tempDelta = string.Empty;
            string ProjectBG = "#3DD94A";

            DateTime temp = DateTime.Today;
            DateTime temp2;
            DateTime temp3;
            int planDay = 0;
            int currentDay = temp.Day;
            int actDay = 0;

            List<CustomProject2> itemer = new List<CustomProject2>();
            var result = (from data in context.Project
                          where data.ProjectManager == id
                          select data).ToList();

            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item != null)
                    {
                        var _user = context.UserSspm.SingleOrDefault(tp => tp.UserId == id);
                        var _checkFinish = context.Project.SingleOrDefault(p => p.ProjectNumber == item.ProjectNumber);
                        var checkTask = context.Task.Count(tp => tp.ProjectNumber == item.ProjectNumber);
                        var tempFinish = context.Task.Count(tp => tp.ProjectNumber == item.ProjectNumber && tp.ActualEnd == null);

             
                            temp2 = (DateTime)item.ProjectEnd;
                            planDay = temp2.Day;
                        if (_checkFinish != null)
                        {
                            temp3 = (DateTime)_checkFinish.ActualEnd;
                            actDay = temp3.Day;
                        }
                        else { actDay = 0; }

                        if (planDay - currentDay == 0 && item.ActualEnd == null)
                                tempDelta = "DL";

                            if (planDay - currentDay > 0 && item.ActualEnd == null)
                            {
                                delta = planDay - currentDay;
                                tempDelta = "HT" + delta.ToString();
                            }

                            if (planDay - currentDay < 0 && item.ActualEnd == null)
                            {
                                delta = planDay - currentDay;
                                tempDelta = "OT" + delta.ToString();
                            }

                            if (temp2.Subtract(temp).TotalDays <= 1)
                                ProjectBG = "#F74933";
                            else if (temp2.Subtract(temp).TotalDays >= 2 && temp2.Subtract(temp).TotalDays <= 7)
                                ProjectBG = "#FFD500";

                            else if (checkTask == tempFinish)
                            {
                                if (planDay - actDay == 0)
                                {
                                    delta = 0;
                                    tempDelta = "SC" + delta.ToString();
                                }
                                else if (planDay - actDay > 0)
                                {
                                    delta = planDay - actDay;
                                    tempDelta = "SF" + delta.ToString();
                                }
                                else
                                {
                                    delta = planDay - actDay;
                                    tempDelta = "SL" + delta.ToString();
                                }
                                ProjectBG = "#bdbdbd";
                            }
                            else
                                ProjectBG = "#3DD94A";


                            itemer.Add(
                            new CustomProject2
                            {
                                ProjectNumber = item.ProjectNumber,
                                ProjectName = item.ProjectName,
                                ProjectStart = item.ProjectStart,
                                ProjectCreateBy = item.ProjectCreateBy,
                                Note = item.Note,
                                ActualStart = item.ActualStart,
                                ActualEnd = item.ActualEnd,
                                Variant = item.Variant,
                                CustomerTel = item.CustomerTel,
                                ProjectEnd = item.ProjectEnd,
                                ProjectManagerId = item.ProjectManager,
                                ProjectManagerName = _user.Firstname + " " + _user.Lastname,
                                ProjectCost = item.ProjectCost,
                                CustomerName = item.CustomerName,
                                ProjectStatus = item.ProjectStatus,
                                bgColor = ProjectBG,
                                deltaForce = tempDelta
                            });
                        ProjectBG = "#3DD94A";
                    }
                }
                return Json(new { itemer });
            }
            else
                return Json(new { itemer });
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskDetailBG(string id)
        {
            int delta = 0;
            string tempDelta = string.Empty;

            string taskBG = "#3DD94A";
            DateTime temp = DateTime.Today;
            DateTime temp2;
            DateTime temp3;
            int planDay = 0;
            int currentDay = temp.Day;
            int actDay = 0;

            List<CustomTask> itemer = new List<CustomTask>();
            var result = (from data in context.Task where data.ProjectNumber == id orderby data.TaskEnd descending select data).ToList();
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item != null)
                    {
                        var _checkFinish = context.Task.SingleOrDefault(t => t.TaskId == item.TaskId && t.ActualEnd != null);

                        var checkTask = context.Function.Count(tp => tp.TaskId == item.TaskId);
                        var tempFinish = context.Function.Count(tp => tp.TaskId == item.TaskId && tp.ActualEnd != null);


                        temp2 = (DateTime)item.TaskEnd;
                        planDay = temp2.Day;

                        if (_checkFinish != null)
                        {
                            temp3 = (DateTime)_checkFinish.ActualEnd;
                            actDay = temp3.Day;
                        }
                        else { actDay = 0; }

                        if (planDay - currentDay == 0 && item.ActualEnd == null)
                            tempDelta = "DL";

                        if (planDay - currentDay > 0 && item.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "HT" + delta.ToString();
                        }

                        if (planDay - currentDay < 0 && item.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "OT" + delta.ToString();
                        }

                        if (temp2.Subtract(temp).TotalDays <= 1)
                            taskBG = "#F74933";
                        else if (temp2.Subtract(temp).TotalDays >= 2 && temp2.Subtract(temp).TotalDays <= 7)
                            taskBG = "#FFD500";

                        else if (checkTask == tempFinish)
                        {
                            if (planDay - actDay == 0)
                            {
                                delta = 0;
                                tempDelta = "SC" + delta.ToString();
                            }
                            else if (planDay - actDay > 0)
                            {
                                delta = planDay - actDay;
                                tempDelta = "SF" + delta.ToString();
                            }
                            else
                            {
                                delta = planDay - actDay;
                                tempDelta = "SL" + delta.ToString();
                            }
                            taskBG = "#bdbdbd";
                        }
                        else
                            taskBG = "#3DD94A";


                        itemer.Add(
                            new CustomTask
                            {
                                ProjectNumber = item.ProjectNumber,
                                TaskId = item.TaskId,
                                TaskName = item.TaskName,
                                TaskStart = item.TaskStart,
                                TaskEnd = item.TaskEnd,
                                ActualStart = item.ActualStart,
                                ActualEnd = item.ActualEnd,
                                Variant = Convert.ToInt16(item.Variant),
                                bgColor = taskBG,
                                deltaForce = tempDelta



                            });
                    }
                    taskBG = "#3DD94A";
                }
                return Json(new { itemer });
            }
            else
                return Json(new { itemer });

        }

      
        public async Task<IActionResult> GetFunctionBG(string id)
        {
            int delta = 0;
            string tempDelta = string.Empty;
            
            string functionBG = "#3DD94A";
            DateTime temp = DateTime.Today;
            int planDay = 0;
            int currentDay = temp.Day;
            int actDay =0;
            DateTime temp2;
            DateTime temp3;
            List<CustomFunction> itemer = new List<CustomFunction>();

            var result = (from data in context.Function where data.TaskId == id orderby data.FunctionEnd descending select data).ToList();
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item != null)
                    {
                        var checkFinish = context.Function.SingleOrDefault(tp => tp.FunctionId == item.FunctionId && tp.ActualEnd != null);


                        temp2 = (DateTime)item.FunctionEnd;
                        planDay = temp2.Day;
                        if (checkFinish != null)
                        {
                            temp3 = (DateTime)checkFinish.ActualEnd;
                            actDay = temp3.Day;
                        }                      

                        if (planDay - currentDay == 0 && item.ActualEnd == null)
                            tempDelta = "DL";

                        if (planDay - currentDay > 0 && item.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "HT" + delta.ToString();
                        }

                        if (planDay - currentDay < 0 && item.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "OT" + delta.ToString();
                        }

                        if (temp2.Subtract(temp).TotalDays <= 1)
                            functionBG = "#F74933";
                        else if (temp2.Subtract(temp).TotalDays >= 2 && temp2.Subtract(temp).TotalDays <= 7)
                            functionBG = "#FFD500";

                        else if (checkFinish != null)
                        {
                            if (planDay - actDay == 0)
                            {
                                delta = 0;
                                tempDelta = "SC" + delta.ToString();
                            }
                            else if (planDay - actDay > 0)
                            {
                                delta = planDay - actDay;
                                tempDelta = "SF" + delta.ToString();
                            }
                            else
                            {
                                delta = planDay - actDay;
                                tempDelta = "SL" + delta.ToString();
                            }
                            functionBG = "#bdbdbd";
                        }
                        else
                            functionBG = "#3DD94A";



                        itemer.Add(
                            new CustomFunction
                            {
                                ProjectNumber = item.ProjectNumber,
                                TaskId = item.TaskId,
                                FunctionId = item.FunctionId,
                                FunctionName = item.FunctionName,
                                FunctionStart = item.FunctionStart,
                                FunctionEnd = item.FunctionEnd,
                                ActualStart = item.ActualStart,
                                ActualEnd = item.ActualEnd,
                                Variant = item.Variant,
                                bgColor = functionBG,
                                Age = null,
                                deltaForce = tempDelta
                            });
                    }
                    functionBG = "#3DD94A";
                }
                return Json(new { itemer });
            }
            else
                return Json(new { itemer });

        }
        public async Task<IActionResult> GetProjectByIdForTeamBG(string id)
        {
            int delta = 0;
            string tempDelta = string.Empty;
            string ProjectBG = "#3DD94A";

            DateTime temp = DateTime.Today;
            DateTime temp2;
            DateTime temp3;
            int planDay = 0;
            int currentDay = temp.Day;
            int actDay = 0;

            List<CustomProject2> itemer = new List<CustomProject2>();
            var result = (from data in context.TeamTask
                          where data.UserId == id
                          group data by data.ProjectNumber into g
                          select new
                          {
                              ProjectNumber = g.Key,
                              Count = g.Count()
                          }).ToList();

            if (result != null)
            {
                foreach (var item in result)
                {
                    var items = context.Project.SingleOrDefault(tp => tp.ProjectNumber == item.ProjectNumber);
                    var _checkFinish = context.Project.SingleOrDefault(p => p.ProjectNumber == item.ProjectNumber && p.ActualEnd != null);
                    var checkTask = context.Task.Count(tp => tp.ProjectNumber == item.ProjectNumber);
                    var tempFinish = context.Task.Count(tp => tp.ProjectNumber == item.ProjectNumber && tp.ActualEnd != null);

                    if (items != null) {
                        var user = context.UserSspm.SingleOrDefault(tp => tp.UserId == items.ProjectManager);

                        temp2 = (DateTime)items.ProjectEnd;
                        planDay = temp2.Day;
                        if (_checkFinish != null)
                        {
                            temp3 = (DateTime)_checkFinish.ActualEnd;
                            actDay = temp3.Day;
                        }
                        else { actDay = 0; }


                        if (planDay - currentDay == 0 && items.ActualEnd == null)
                            tempDelta = "DL";

                        if (planDay - currentDay > 0 && items.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "HT" + delta.ToString();
                        }

                        if (planDay - currentDay < 0 && items.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "OT" + delta.ToString();
                        }

                        if (temp2.Subtract(temp).TotalDays <= 1)
                            ProjectBG = "#F74933";
                        else if (temp2.Subtract(temp).TotalDays >= 2 && temp2.Subtract(temp).TotalDays <= 7)
                            ProjectBG = "#FFD500";

                        else if (checkTask == tempFinish)
                        {
                            if (planDay - actDay == 0)
                            {
                                delta = 0;
                                tempDelta = "SC" + delta.ToString();
                            }
                            else if (planDay - actDay > 0)
                            {
                                delta = planDay - actDay;
                                tempDelta = "SF" + delta.ToString();
                            }
                            else
                            {
                                delta = planDay - actDay;
                                tempDelta = "SL" + delta.ToString();
                            }
                            ProjectBG = "#bdbdbd";
                        }
                        else
                            ProjectBG = "#3DD94A";


                        itemer.Add(
                        new CustomProject2
                        {
                            ProjectNumber = items.ProjectNumber,
                            ProjectName = items.ProjectName,
                            ProjectStart = items.ProjectStart,
                            ProjectCreateBy = items.ProjectCreateBy,
                            Note = items.Note,
                            ActualStart = items.ActualStart,
                            ActualEnd = items.ActualEnd,
                            Variant = items.Variant,
                            CustomerTel = items.CustomerTel,
                            ProjectEnd = items.ProjectEnd,
                            ProjectManagerId = items.ProjectManager,
                            ProjectManagerName = user.Firstname + " " + user.Lastname,
                            ProjectCost = items.ProjectCost,
                            CustomerName = items.CustomerName,
                            ProjectStatus = items.ProjectStatus,
                            bgColor = ProjectBG,
                            deltaForce = tempDelta
                        });
                    ProjectBG = "#3DD94A";
                    }
                }
                return Json(new { itemer });
            }
            else
                return Json(new { itemer });
        }


        public async Task<IActionResult> GetProjectBG()
        {
            int delta = 0;
            string tempDelta = string.Empty;
            string ProjectBG = "#3DD94A";
            DateTime temp = DateTime.Today;
            DateTime temp2;
            DateTime temp3;
            int planDay = 0;
            int currentDay = temp.Day;
            int actDay = 0;
            List<CustomProject2> itemer = new List<CustomProject2>();

            var result = (from data in context.Project orderby data.ProjectCreateDate descending
                          select data).ToList();
            if (result != null)
            {
                foreach (var item in result)
                {
                    var tempFinish = context.Task.Count(tp => tp.ProjectNumber == item.ProjectNumber && tp.ActualEnd != null);                  
                    var checkTask = context.Task.Count(tp => tp.ProjectNumber == item.ProjectNumber);
                    var _user = context.UserSspm.SingleOrDefault(u => u.UserId == item.ProjectManager);
                    var _checkFinish = context.Project.SingleOrDefault(p => p.ProjectNumber == item.ProjectNumber && p.ActualEnd != null);

                    temp2 = (DateTime)item.ProjectEnd;
                        planDay = temp2.Day;

                    if (_checkFinish != null)
                    {
                        temp3 = (DateTime)_checkFinish.ActualEnd;
                        actDay = temp3.Day;
                    }
                    else { actDay = 0; }

                        if (planDay - currentDay == 0 && item.ActualEnd == null)
                            tempDelta = "DL";

                        if (planDay - currentDay > 0 && item.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "HT" + delta.ToString();
                        }

                        if (planDay - currentDay < 0 && item.ActualEnd == null)
                        {
                            delta = planDay - currentDay;
                            tempDelta = "OT" + delta.ToString();
                        }

                        if (temp2.Subtract(temp).TotalDays <= 1)
                            ProjectBG = "#F74933";
                        else if (temp2.Subtract(temp).TotalDays >= 2 && temp2.Subtract(temp).TotalDays <= 7)
                            ProjectBG = "#FFD500";

                        else if (checkTask == tempFinish)
                        {
                            if (planDay - actDay == 0)
                            {
                                delta = 0;
                                tempDelta = "SC" + delta.ToString();
                            }
                            else if (planDay - actDay > 0)
                            {
                                delta = planDay - actDay;
                                tempDelta = "SF" + delta.ToString();
                            }
                            else
                            {
                                delta = planDay - actDay;
                                tempDelta = "SL" + delta.ToString();
                            }
                            ProjectBG = "#bdbdbd";
                        }
                        else
                            ProjectBG = "#3DD94A";
                    

                        itemer.Add(
                        new CustomProject2
                        {
                            ProjectNumber = item.ProjectNumber,
                            ProjectName = item.ProjectName,
                            ProjectStart = item.ProjectStart,
                            ProjectCreateBy = item.ProjectCreateBy,
                            Note = item.Note,
                            ActualStart = item.ActualStart,
                            ActualEnd = item.ActualEnd,
                            Variant = item.Variant,
                            CustomerTel = item.CustomerTel,
                            ProjectEnd = item.ProjectEnd,
                            ProjectManagerId = item.ProjectManager,
                            ProjectManagerName = _user.Firstname + " " + _user.Lastname,
                            ProjectCost = item.ProjectCost,
                            CustomerName = item.CustomerName,
                            ProjectStatus = item.ProjectStatus,
                            bgColor = ProjectBG,
                            deltaForce = tempDelta
                        });
                        ProjectBG = "#3DD94A";
                    }
                    return Json(new { itemer });
                }
            

            else
                return Json(new { itemer });
            }

        
        //------------------------------------------ Fillter Service Session --------------------------------------------------//
        //Get Team
        [HttpGet]
public IEnumerable<TeamTask> FilterTeam()
{

    var _filterdata = (from fdata in context.TeamTask select fdata).ToList();
    return _filterdata;

}
public IEnumerable<TeamTask> FilterByUser(string id)
{

    var _filterdata = (from fdata in context.TeamTask where (fdata.UserId.Equals(id)) select fdata).ToList();
    return _filterdata;

}

public async Task<IActionResult> FilterTeamByProject(string id)
{

    List<CustomTeam> filterTeam = new List<CustomTeam>();
    var manager = context.Project.SingleOrDefault(m => m.ProjectNumber == id);
    var result = (from data in context.TeamTask
                  where data.ProjectNumber == id
                  group data by data.UserId into g
                  select new
                  {
                      UserId = g.Key,
                      Count = g.Count()
                  }).ToList();
    foreach (var item in result)
    {
        var user = (from data2 in context.UserSspm where data2.UserId == item.UserId select data2).SingleOrDefault();
        var image = context.UserImage.SingleOrDefault(im => im.UserId == item.UserId);
                
        if (user != null)
        {
            filterTeam.Add(new CustomTeam
            {
                userId = user.UserId,
                userName = user.Firstname + " " + user.Lastname,
                userImage = image.Image,
                jobRes = user.JobResponsible
            });
        }

    }
    return Json(new { filterTeam });

}

public async Task<IActionResult> FilterProjectById(string id)
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
        var items = (from data in context.Project where data.ProjectNumber == item.ProjectNumber select data).SingleOrDefault();
        var user = (from data2 in context.UserSspm where data2.UserId == items.ProjectManager select data2).SingleOrDefault();

        FilProject.Add(new CustomProject
        {
            ProjectNumber = items.ProjectNumber,
            ProjectName = items.ProjectName,
            ProjectStart = items.ProjectStart,
            ProjectCreateBy = items.ProjectCreateBy,
            Note = items.Note,
            ActualStart = items.ActualStart,
            ActualEnd = items.ActualEnd,
            Variant = items.Variant,
            CustomerTel = items.CustomerTel,
            ProjectEnd = items.ProjectEnd,
            ProjectManagerId = items.ProjectManager,
            ProjectManagerName = user.Firstname + " " + user.Lastname,
            ProjectCost = items.ProjectCost,
            CustomerName = items.CustomerName,
            ProjectStatus = items.ProjectStatus

        });
    }
    return Json(new { FilProject });
}

[HttpPost]
public async Task<IActionResult> FilterTasktById(string id, string userId)
{
    List<SSMP.Models.Task> FilTask = new List<SSMP.Models.Task>();
    var manager = await context.Project.SingleOrDefaultAsync(mg => mg.ProjectNumber == id && mg.ProjectManager == userId);
    if (manager != null)
    {
        FilTask = (from itemM in context.Task where itemM.ProjectNumber == id select itemM).ToList();
        return Ok(Json(new { FilTask }));
    }
    else
    {
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
            var items = (from data in context.Task where data.TaskId == item.TaskId select data).SingleOrDefault();

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
}


public async Task<IActionResult> FilterFunctionById(string id, string userId)
{
    List<Function> FilFunction = new List<Function>();
    var itemMgr = await context.Task.SingleOrDefaultAsync(m => m.TaskId == id);
    var manager = await context.Project.SingleOrDefaultAsync(mg => mg.ProjectNumber == itemMgr.ProjectNumber && mg.ProjectManager == userId);
    if (manager != null)
    {
        FilFunction = (from itemM in context.Function where itemM.ProjectNumber == id select itemM).ToList();
        return Ok(Json(new { FilFunction }));
    }
    else
    {

        var result = (from data in context.TeamTask where data.UserId == userId && data.TaskId == id select data).ToList();
        foreach (var item in result)
        {
            var items = (from data in context.Function where data.FunctionId == item.FunctionId select data).SingleOrDefault();

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
}

[HttpGet]
public async Task<IActionResult> GetProjectByIdForTeam(string id)
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
        var items = (from data in context.Project where data.ProjectNumber == item.ProjectNumber select data).LastOrDefault();
        var user = (from data2 in context.UserSspm where data2.UserId == items.ProjectManager select data2).LastOrDefault();

        FilProject.Add(new CustomProject
        {
            ProjectNumber = items.ProjectNumber,
            ProjectName = items.ProjectName,
            ProjectStart = items.ProjectStart,
            ProjectCreateBy = items.ProjectCreateBy,
            Note = items.Note,
            ActualStart = items.ActualStart,
            ActualEnd = items.ActualEnd,
            Variant = items.Variant,
            CustomerTel = items.CustomerTel,
            ProjectEnd = items.ProjectEnd,
            ProjectManagerId = items.ProjectManager,
            ProjectManagerName = user.Firstname + " " + user.Lastname,
            ProjectCost = items.ProjectCost,
            CustomerName = items.CustomerName,
            ProjectStatus = items.ProjectStatus

        });
    }


    return Json(FilProject);

}

//--------------------------------------------------- Mood board -------------------------------------------//
[HttpGet]
public IEnumerable<MoodBoard> GetMoodBoard()
{
    List<MoodBoard> mood = new List<MoodBoard>();
    var result = (from data in context.Bulletin
                  orderby data.Time descending
                  join data2 in context.UserSspm on data.UserId equals data2.UserId
                  select new
                  {
                      UserId = data.UserId,
                      name = data2.Firstname + " " + data2.Lastname,
                      title = data.Subject,
                      detail = data.Note,
                      time = data.Time,
                      number = data.Bnumber
                  }).ToList();
    foreach (var item in result)
    {
        var c = (from data in context.BulletinChat where data.Bnumber == item.number select data).Count();

        mood.Add(new MoodBoard
        {
            userId = item.UserId,
            name = item.name,
            Subject = item.title,
            Note = item.detail,
            Time = item.time,
            Bnumber = item.number,
            counter = c.ToString() + " " + "ความคิดเห็น"

        });
    }
    return mood;
}


        public IEnumerable<MoodBoard> GetLastMoodBoard()
        {
            List<MoodBoard> mood = new List<MoodBoard>();
            var result = (from data in context.Bulletin
                          orderby data.Time descending
                          join data2 in context.UserSspm on data.UserId equals data2.UserId
                          select new
                          {
                              UserId = data.UserId,
                              name = data2.Firstname + " " + data2.Lastname,
                              title = data.Subject,
                              detail = data.Note,
                              time = data.Time,
                              number = data.Bnumber
                          }).SingleOrDefault();
          
                var c = (from data in context.BulletinChat where data.Bnumber == result.number select data).Count();

                mood.Add(new MoodBoard
                {
                    userId = result.UserId,
                    name = result.name,
                    Subject = result.title,
                    Note = result.detail,
                    Time = result.time,
                    Bnumber = result.number,
                    counter = c.ToString() + " " + "ความคิดเห็น"

                });
            
            return mood;
        }
        [HttpPost]
public async Task<IActionResult> NewItemMoodBoard([FromBody]CustomTodoMoodboard itemNew)
{

    DateTime? strDate = DateTime.Now;
    try
    {
        var filId = (from data in context.Bulletin orderby data.Bnumber descending select data.Bnumber).FirstOrDefault();

        int tempid;
        int realid;
        if (filId != null)
        {
            tempid = Convert.ToInt32(filId);
            realid = tempid + 1;
        }
        else
        {
            realid = 100000;
        }
        Bulletin dataContext = new Bulletin
        {
            UserId = itemNew.userId,
            Subject = itemNew.subject,
            Note = itemNew.note,
            Time = strDate,
            Bnumber = realid.ToString()
        };

                var contextRun = context.RunningNumber.SingleOrDefault(rn => rn.Type == "BNumber");

                contextRun.Number = realid.ToString();
                context.Update(contextRun);

                await context.SaveChangesAsync();

        context.Bulletin.Add(dataContext);
        await context.SaveChangesAsync();

    }
    catch (Exception e)
    {
        return Json(new { success = false });
    }
    return Json(new { success = true });
}

[HttpPut]
public async Task<IActionResult> EditMoodItem(string id, [FromBody]CustomEditMoodBoard Model)
{

    DateTime? timer = DateTime.Now;

    var query = (from x in context.Bulletin where x.Bnumber.Equals(id) select x).FirstOrDefault();
    if (query == null)
    {
        return Json(new { success = false });
    }

    else
    {

        query.Subject = Model.subject;
        query.Note = Model.note;
        query.Time = timer;
        context.Bulletin.Update(query);
        await context.SaveChangesAsync();

        return Json(new { success = true });
    }
}


//NewComment
[HttpPost]
public async Task<IActionResult> NewComment([FromBody]CustomNewComment Model)
{

    DateTime? strDate = DateTime.Now;
    try
    {
        var filId = (from data in context.BulletinChat orderby data.Bchat descending select data.Bchat).FirstOrDefault();


        int tempid;
        int realid;
        if (filId != null)
        {
            tempid = Convert.ToInt32(filId);
            realid = tempid + 1;
        }
        else
        {
            realid = 100000;
        }
        BulletinChat dataComment = new BulletinChat
        {
            Bnumber = Model.bNumber,
            Bchat = realid.ToString(),
            Chat = Model.commentDetail,
            Ctime = strDate,
            UserId = Model.userId

        };
        context.BulletinChat.Add(dataComment);
        await context.SaveChangesAsync();
        return Json(new { success = true });
    }
    catch (Exception e)
    {
        return Json(new { success = false });
    }

}
[HttpPut]
public async Task<IActionResult> EditComment(string id, [FromBody]CustomEditComment Model)
{

    DateTime? timer = DateTime.Now;

    var query = (from x in context.BulletinChat where x.Bchat.Equals(id) select x).FirstOrDefault();
    if (query == null)
    {
        return Json(new { success = false });
    }

    else
    {
        query.Chat = Model.detail;
        query.Ctime = timer;
        context.BulletinChat.Update(query);
        await context.SaveChangesAsync();

        return Json(new { success = true });
    }

}

[HttpPost]
public IEnumerable<CustomCommentServer> getComment(string bid)
{
    List<CustomCommentServer> comment = new List<CustomCommentServer>();
    var result = (from data in context.BulletinChat
                  where data.Bnumber.Equals(bid)
                  join data1 in context.Bulletin on data.Bnumber equals data1.Bnumber
                  join data2 in context.UserSspm on data.UserId equals data2.UserId
                  join data3 in context.UserImage on data.UserId equals data3.UserId
                  select new
                  {
                      uid = data2.UserId,
                      bid = data.Bnumber,
                      fullname = data2.Firstname + " " + data2.Lastname,
                      cid = data.Bchat,
                      time = data.Ctime,
                      commentdetail = data.Chat,
                      img = data3.Image
                  }).ToList();
    foreach (var item in result)
    {
        comment.Add(new CustomCommentServer()
        {
            userId = item.uid,
            bNumber = item.bid,
            fullName = item.fullname,
            time = item.time,
            cid = item.cid,
            commentDetail = item.commentdetail,
            base64Image = item.img
        });
    }
    return comment;
}



    }
}