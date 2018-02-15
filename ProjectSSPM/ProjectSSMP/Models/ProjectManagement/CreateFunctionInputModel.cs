using Microsoft.AspNetCore.Mvc;
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
        [Required]
        public string FunctionName { get; set; }
        [Remote(action: "VarifyCheckdateFunctionStartFu", controller: "validation", AdditionalFields = nameof(FunctionStart) + "," + nameof(TaskId))]
        [Required]
        public DateTime? FunctionStart { get; set; }

        [Remote(action: "VarifyDateFunction", controller: "validation", AdditionalFields = nameof(FunctionStart) + "," + nameof(FunctionEnd)+","+nameof(TaskId))]
        [Required]
        public DateTime? FunctionEnd { get; set; }        
        public string ProjectNumber { get; set; }
        [Required]
        public string UserId { get; set; }        
        public string ProjectResponsible { get; set; }
        public Double Timespan { get; set; }
    }
}
