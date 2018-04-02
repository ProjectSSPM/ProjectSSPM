using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomEditMoodBoard
    {
        public string userId { get; set; }
        public string subject { get; set; }
        public string note { get; set; }
        public string bnumber { get; set; }
    }
}
