
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SSMP.Models.ProjectManagement
{
    public class CreateTaskInputModel
    {
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Remote(action: "VarifyCheckdateProjectStart", controller: "validation", AdditionalFields = nameof(TaskStart) + "," + nameof(ProjectNumber))]
        [Required]
        public DateTime? TaskStart { get; set; }

        [Remote(action: "VarifyDateTsak", controller: "validation", AdditionalFields = nameof(TaskStart) + "," + nameof(TaskEnd)+","+nameof(ProjectNumber))]
        [Required]
        public DateTime? TaskEnd { get; set; }
        public Double Timespan { get; set; }
        public String percent { get; set; }

    }
}
