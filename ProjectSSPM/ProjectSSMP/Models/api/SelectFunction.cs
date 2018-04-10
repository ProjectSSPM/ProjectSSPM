using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api.NewTimesheet
{
    public class SelectFunction
    {
        public string projectId { get; set; }
        public string taskId { get; set; }
        public DateTime? functionStart { get; set; }
        public string functionId { get; set; }
        public string functionName { get; set; }
    }
}
