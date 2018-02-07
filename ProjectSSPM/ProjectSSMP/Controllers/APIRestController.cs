using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;

namespace ProjectSSMP.Controllers
{
    [Produces("application/json")]
    //[Route("api/APIRest")]
    public class APIRestController : Controller
    {
        private readonly sspmContext _context;
        public APIRestController(sspmContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IEnumerable<UserSspm> GetAllEmp()
        {
            return _context.UserSspm.ToList();
        }

    //[Route("api/APIResponseAuthen/{id}")]
        [HttpPost]
        public JsonResult Authen(string eusername, string epassword)
        {
            if (!validateuser(eusername, epassword))
            {
                return Json(new { success = false, msg = "ไม่สามารถดึงข้อมูลได้ กรุณาทำรายการใหม่" });

            }
            return Json(new { success = true, username = eusername, password = epassword });

        }

        [HttpPost]
        private bool validateuser(string user, string pass)
        {
            var userid = (from u in _context.UserSspm where u.Username.Equals(user) select u).FirstOrDefault();
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
        private bool checkstatususer(string user)
        {
            var userid = (from u in _context.UserSspm where u.Username.Equals(user) select u).FirstOrDefault();
            if (userid.Status == "D")
            {   
                return false;
            }

            return true;
        }
    }
}