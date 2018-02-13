using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.ProjectManagement
{
    public class CreateFunctionInputModel
    {
        public string FunctionId { get; set; }
        public string TaskId { get; set; }
        public string FunctionName { get; set; }
        [Required]
        public DateTime? FunctionStart { get; set; }
        [Required]
        public DateTime? FunctionEnd { get; set; }        
        public string ProjectNumber { get; set; }
        public string UserId { get; set; }        
        public string ProjectResponsible { get; set; }
        public Double Timespan { get; set; }
    }
}
