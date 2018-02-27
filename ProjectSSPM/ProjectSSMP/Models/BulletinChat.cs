using System;
using System.Collections.Generic;

namespace SSMP.Models
{
    public partial class BulletinChat
    {
        public string Bnumber { get; set; }
        public string Bchat { get; set; }
        public string Chat { get; set; }
        public DateTime? Ctime { get; set; }
        public string UserId { get; set; }
    }
}
