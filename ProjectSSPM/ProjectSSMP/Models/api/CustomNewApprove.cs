using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomNewApprove
    {
      public  string userId { get; set; }
        public DateTime timesheetId { get; set; }
        public string functionId { get; set; }
        public string taskId { get; set; }
        public string projectId { get; set; }
    }
}
