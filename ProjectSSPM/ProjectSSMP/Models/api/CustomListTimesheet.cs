using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api


{
    public class CustomListTimesheet
    {
        public DateTime timeSheetId { get; set; }
        public string taskName { get; set; }
        public string projectName { get; set; }
        public string functionName { get; set; }
        public string actionName {get;set;}
        public string approve1 { get; set; }
        public string approve2 { get; set; }
        public DateTime? timeSheetStart { get; set; }
        public DateTime? timeSheetEnd { get; set; }
        public string duration { get; set; }
    }
}
