using System;
namespace ProjectSSMP.Models.Home
{
    public class ChatBulletinModel
    {
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string Note { get; set; }
        public DateTime? Time { get; set; }
        public string Bnumber { get; set; }
        public string Username { get; set; }


        public string Bchat { get; set; }
        public string Chat { get; set; }
        public DateTime? Ctime { get; set; }
        public string CUserId { get; set; }
        public string CUsername { get; set; }
        public string CFullname { get; set; }
    }
}
