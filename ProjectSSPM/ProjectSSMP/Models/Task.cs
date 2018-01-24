using System;
using System.Collections.Generic;

namespace ProjectSSMP.Models
{
    public partial class Task
    {
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        public string TaskName { get; set; }
        public DateTime? TaskStart { get; set; }
        public DateTime? TaskEnd { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public int? Variant { get; set; }
    }
}
