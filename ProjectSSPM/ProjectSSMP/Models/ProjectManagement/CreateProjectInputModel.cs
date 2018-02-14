using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectSSMP.Models.ProjectManagement
{
    public class CreateProjectInputModel
    {
        public string ProjectNumber { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Project ID must be 5-50 letters.")]
        [Required]
        public string ProjectId { get; set; }
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Project Name must be 5-100 letters.")]
        [Required]
        public string ProjectName { get; set; }

        public string ProjectManager { get; set; }
        
        [Required]
        public DateTime? ProjectStart { get; set; }
        [Remote(action: "VarifyDate", controller: "validation", AdditionalFields = nameof(ProjectStart) +","+nameof(ProjectEnd))]
        [Required]
        public DateTime? ProjectEnd { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public long? ProjectCost { get; set; }
        public string ProjectCreateBy { get; set; }
        public DateTime? ProjectCreateDate { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Phone]
        [Required]
        public string CustomerTel { get; set; }       
        public string Note { get; set; }
    }
}
