using System;
using System.Collections.Generic;

namespace SSMP.Models
{
    public partial class TeamTask
    {
        public string FunctionId { get; set; }
        public string UserId { get; set; }
        public string ProjectResponsible { get; set; }
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
    }
}
