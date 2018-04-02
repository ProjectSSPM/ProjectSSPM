using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomApproveTimesheet
    {
        public DateTime submitDate { get; set; }
        public string projectId { get; set; }
        public string projectName { get; set; }
        public string taskId { get; set; }
        public string tackName { get; set; }
        public string functionId { get; set; }
        public string functionName { get; set; }
        public string actionId { get; set; }
        public string actionName { get; set; }
        public string userId { get; set; }
        public string fullName { get; set; }
       // public TimeSpan? timeStart { get; set; }
      //  public TimeSpan? timeEnd { get; set; }
        public int durationHrs { get; set; }
        public int durationMns { get; set; }


    }
}
