using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class MoodBoard
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string Subject { get; set; }
        public string Note { get; set; }
        public DateTime? Time { get; set; }
        public string Bnumber { get; set; }
        public string counter { get; set; }
    }
}
