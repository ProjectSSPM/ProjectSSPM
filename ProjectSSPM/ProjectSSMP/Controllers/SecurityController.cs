using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSMP.Models;
using SSMP.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSMP.Controllers
{
    public class SecurityController : BaseController
    {
        public string IPAddress { get; private set; }

        public SecurityController(sspmContext context )
        {
            this.context = context;
           
        }
        
        public IActionResult Login()
        {
            string IPAddress = GetIPAddress();
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LgoinInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!validateuser(inputModel.Username, inputModel.Password))
            {
                ModelState.AddModelError("ErrorLogin", "Username หรือ Password ผิด");
                return View();
            }
            if (!checkstatususer(inputModel.Username))
            {
                ModelState.AddModelError("ErrorLogin", "รหัสของคุณถูกระงับการใช้งาน");
                return View();
            }
          
            // create claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, inputModel.Username), new Claim("LastChange", DateTime.Now.ToString())
            };

            // create identity
            var identity = new ClaimsIdentity(claims, "cookie");

            // create principal
            var principal = new ClaimsPrincipal(identity);

            // sign-in
            await HttpContext.SignInAsync(
                    scheme: "FiverSecurityScheme",
                    principal: principal);

            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout(string requestPath)
        {
            await HttpContext.SignOutAsync(
                    scheme: "FiverSecurityScheme");

            return RedirectToAction("Login");
        }
        private bool validateuser(string user , string pass)
        {
            var userid = (from u in context.UserSspm where u.Username.Equals(user) select u).FirstOrDefault();
            if (userid == null)
            {
                return false;
            }
                
            if(userid.Password != pass)
            {
                return false;
            }
            return true;
        }
        private bool checkstatususer(string user)
        {
            var userid = (from u in context.UserSspm where u.Username.Equals(user) select u).FirstOrDefault();
            if(userid.Status == "D")
            {
                return false;
            }

            return true;
        }
        

        public string GetIPAddress()
        {
           
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
        


    }
}