using System;
using System.Collections.Generic;

namespace SSMP.Models
{
    public partial class TimeSheet
    {
        public DateTime TimeSheetId { get; set; }
        public string ActionId { get; set; }
        public DateTime? TimeSheetStart { get; set; }
        public DateTime? TimeSheetEnd { get; set; }
        public string UserId { get; set; }
        public string FunctionId { get; set; }
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        public string TimeSheetNumber { get; set; }
        public string Approve1 { get; set; }
        public string Approve2 { get; set; }
    }
}
