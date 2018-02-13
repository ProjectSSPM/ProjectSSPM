
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectSSMP.Models.ProjectManagement
{
    public class CreateTaskInputModel
    {
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        public string TaskName { get; set; }
        
        [Required]
        public DateTime? TaskStart { get; set; }
        
        [Required]

        public DateTime? TaskEnd { get; set; }
        public Double Timespan { get; set; }

    }
}
