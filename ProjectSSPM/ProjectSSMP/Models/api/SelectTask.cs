using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api.NewTimesheet
{
    public class SelectTask
    {
        public string projectId { get; set; }
        public string taskId { get; set; }
        public string taskName { get; set; }
    }
}
