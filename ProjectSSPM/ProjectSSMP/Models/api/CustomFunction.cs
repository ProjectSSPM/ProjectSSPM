using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSMP.Models.api
{
    public class CustomFunction
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
        public string bgColor { get; set; }
        public int? Age { get; set; }
        public string deltaForce { get; set; }
    }
}
