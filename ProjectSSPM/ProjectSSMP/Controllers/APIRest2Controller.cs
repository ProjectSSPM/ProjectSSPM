using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;
using Newtonsoft.Json;

namespace ProjectSSMP.Controllers
{
    //[Produces("application/json")]
    //[Route("api/APIRest2")]
    public class APIRest2Controller : BaseController
    {
        public APIRest2Controller(sspmContext context) => this.context = context;

        [HttpGet]
        public IEnumerable<ProjectTimline> Gettimeline()
        { 
            var t = (from tdata in context.ProjectTimline orderby tdata.TimelineDate descending,tdata.TimelineId descending select tdata).ToList();
            return t;
           // return _context.ProjectTimline.ToList();
        }
        [HttpPost]
        public async Task<JsonResult> GetLastId()
        {
            var t = (from tdata in context.ProjectTimline orderby tdata.TimelineId descending select tdata.TimelineId).FirstOrDefault();
            return Json(new {id = t});
        }

        public async Task<IActionResult> Addtimeline(string pid,DateTime tdate,string header, string note)
        {
            try
            {
                var id = (from u in context.ProjectTimline orderby u.TimelineId descending select u.TimelineId).FirstOrDefault();
                int tid;
                DateTime tnow = DateTime.Now;
                if (id == null)
                {
                    tid = 1;

                }
                else
                {
                    tid = Convert.ToInt32(id);
                    tid = tid + 1;
                }

                ProjectTimline dataContext = new ProjectTimline
                {
                    TimelineId = tid.ToString(),
                    ProjectNumber = pid,
                    TimelineDate = tnow,
                    Hader = header,
                    Note = note
                };
                context.ProjectTimline.Add(dataContext);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Json(new { msg = "false" });
            }
            return Json(new { msg = "success" });
        }

        public IEnumerable<UserSspm> GetAllUser()
        {
            return context.UserSspm.ToList();
        }

    }
}