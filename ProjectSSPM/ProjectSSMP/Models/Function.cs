using System;
using System.Collections.Generic;

namespace SSMP.Models
{
    public partial class Function
    {
        public string FunctionId { get; set; }
        public string TaskId { get; set; }
        public string FunctionName { get; set; }
        public DateTime? FunctionStart { get; set; }
        public DateTime? FunctionEnd { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public int? Variant { get; set; }
        public string ProjectNumber { get; set; }
        public int? Age { get; set; }
    }
}
