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
        public IActionResult VarifyDateTsak(DateTime TaskStart, DateTime TaskEnd)
        {
            if (TaskEnd < TaskStart)
            {
                return Json(data: $"The Estimate End is greater than Start.");
            }
            return Json(data: true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyDateFunction(DateTime FunctionStart, DateTime FunctionEnd)
        {
            if (FunctionEnd < FunctionStart)
            {
                return Json(data: $"The Estimate End is greater than Start.");
            }
            return Json(data: true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VarifyCheckdateProject(DateTime TaskStart, string ProjectNumber)
        {
            var checkdate = (from p in context.Project where p.ProjectNumber.Equals(ProjectNumber) select p).FirstOrDefault();
            if(TaskStart <= checkdate.ProjectStart)
            {
                return Json(data: $"Your Task Start is Least than Project Start");
            }

            return Json(data: true);
        }
    }
}