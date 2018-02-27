using System;
using System.Collections.Generic;

namespace SSMP.Models
{
    public partial class Bulletin
    {
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string Note { get; set; }
        public DateTime? Time { get; set; }
        public string Bnumber { get; set; }
    }
}
