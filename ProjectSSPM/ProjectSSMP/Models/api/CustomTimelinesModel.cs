using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomTimelinesModel
    {
        public DateTime? functionLogId { get; set; }
        public DateTime? functionStart { get; set; }
        public DateTime? functionEnd { get; set; }
        public DateTime? actualStart { get; set; }
        public DateTime? actualEnd { get; set; }
        public string userCommit { get; set; }
        public string taskName { get; set; }
        public string functionName { get; set; }
    }
}
