using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CalculateTime
    {
        public string projectId { get; set; }
        public string userId { get; set; }
        public int durationHrs {get;set;}
        public int durationMns { get; set; }
    }
}
