using System;
using System.Collections.Generic;

namespace ProjectSSMP.Models
{
    public partial class TimeSheet
    {
        public string TimeSheetId { get; set; }
        public string TeamTaskId { get; set; }
        public string ActionId { get; set; }
        public DateTime? TimeSheetStart { get; set; }
        public DateTime? TimeSheetEnd { get; set; }
        public string TeamId { get; set; }
    }
}
