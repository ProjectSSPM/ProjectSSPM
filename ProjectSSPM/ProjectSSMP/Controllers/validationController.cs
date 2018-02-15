using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;

namespace ProjectSSMP.Controllers
{
    public class validationController : BaseController
    {
        public validationController(sspmContext context) //=> this.context = context;
        {
            this.context = context;
            CultureInfo en = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = en;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyDate(DateTime ProjectStart, DateTime ProjectEnd)
        {
            if (ProjectEnd < ProjectStart)
            {
                return Json(data: $"The Estimate End is greater than Start.");
            }
            return Json(data: true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyDateTsak(DateTime TaskStart, DateTime TaskEnd, string ProjectNumber)
        {
            if (TaskEnd < TaskStart)
            {
                return Json(data: $"The Estimate End is greater than Start.");
            }
            var checkdate = (from p in context.Project where p.ProjectNumber.Equals(ProjectNumber) select p).FirstOrDefault();
            if(TaskEnd >= checkdate.ProjectEnd)
            {
                return Json(data: $"Your Task Start is Least than Project Start");
            }
            return Json(data: true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyDateFunction(DateTime FunctionStart, DateTime FunctionEnd, string TaskId)
        {
            if (FunctionEnd < FunctionStart)
            {
                return Json(data: $"The Estimate End is greater than Start.");
            }
            var checkdate = (from t in context.Task where t.TaskId.Equals(TaskId) select t).FirstOrDefault();
            if (FunctionEnd >= checkdate.TaskEnd)
            {
                return Json(data: $"Your Task Start is Least than Project Start");
            }

            return Json(data: true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyCheckdateProjectStart(DateTime TaskStart, string ProjectNumber)
        {
            var checkdate = (from p in context.Project where p.ProjectNumber.Equals(ProjectNumber) select p).FirstOrDefault();
            if(TaskStart <= checkdate.ProjectStart)
            {
                return Json(data: $"Your Task Start is Least than Project Start");
            }

            return Json(data: true);
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyCheckdateFunctionStartFu(DateTime FunctionStart, string TaskId)
        {
            var checkdate = (from t in context.Task where t.TaskId.Equals(TaskId) select t).FirstOrDefault();
            if (FunctionStart <= checkdate.TaskStart)
            {
                return Json(data: $"Your Function Start is Least than Project Start");
            }

            return Json(data: true);
        }


        public IActionResult VarifyDateTimeSheet(DateTime TimeSheetStart, DateTime TimeSheetEnd, string FunctionId)
        {
            if (TimeSheetEnd < TimeSheetStart)
            {
                return Json(data: $"The Estimate End is greater than Start.");
            }
            

            return Json(data: true);
        }


    }
}