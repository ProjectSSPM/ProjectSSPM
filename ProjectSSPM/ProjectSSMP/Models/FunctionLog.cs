using System;
using System.Collections.Generic;

namespace ProjectSSMP.Models
{
    public partial class FunctionLog
    {
        public string FunctionId { get; set; }
        public DateTime FunctionLogId { get; set; }
        public DateTime? FunctionStart { get; set; }
        public DateTime? FunctionEnd { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public string StatusId { get; set; }
        public int? Variant { get; set; }
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        public string FunctionNumber { get; set; }
        public string Approve1 { get; set; }
        public string Approve2 { get; set; }
    }
}
