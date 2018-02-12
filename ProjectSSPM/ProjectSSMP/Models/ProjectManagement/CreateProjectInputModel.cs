using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectSSMP.Models.ProjectManagement
{
    public class CreateProjectInputModel
    {
        public string ProjectNumber { get; set; }
        public string ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public string ProjectManager { get; set; }
        public DateTime? ProjectStart { get; set; }
        public DateTime? ProjectEnd { get; set; }
        public long? ProjectCost { get; set; }
        public string ProjectCreateBy { get; set; }
        public DateTime? ProjectCreateDate { get; set; }        
        public string CustomerName { get; set; }        
        public string CustomerTel { get; set; }       
        public string Note { get; set; }
    }
}
