﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class TimesheetRecord
    {
        public string actionId { get; set; }
        public DateTime timeSheetStart { get; set; }
        public DateTime TimeSheetEnd { get; set; }
        public string userId { get; set; }
        public string functionId { get; set; }
        public string taskId { get; set; }
        public string projectNumber { get; set; }
        public string Approve1 { get; set; }
        public string Approve2 { get; set; }
    }
}
