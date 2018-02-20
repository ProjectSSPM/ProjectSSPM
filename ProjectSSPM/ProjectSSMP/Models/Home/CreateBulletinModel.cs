using System;
namespace ProjectSSMP.Models.Home
{
    public class CreateBulletinModel
    {
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string Note { get; set; }
        public DateTime? Time { get; set; }
        public string Bnumber { get; set; }
        public string Username { get; set; }
    }
}
