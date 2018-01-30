using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.ProjectManagement
{
    public class CreateFunctionInputModel
    {
        public string FunctionId { get; set; }
        public string TaskId { get; set; }
        public string FunctionName { get; set; }
        public DateTime? FunctionStart { get; set; }
        public DateTime? FunctionEnd { get; set; }        
        public string ProjectNumber { get; set; }
        public string UserId { get; set; }        
        public string ProjectResponsible { get; set; }
    }
}
