using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SSMP.Models.Home
{
    public class CreateBulletinModel
    {
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string Note { get; set; }
        public DateTime? Time { get; set; }
        public string Bnumber { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int BCount { get; set; }
        public string Chat { get; set; }
        public string Image { get; set; }
    }
}
