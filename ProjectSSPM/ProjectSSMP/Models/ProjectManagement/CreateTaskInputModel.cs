using System;
namespace ProjectSSMP.Models.ProjectManagement
{
    public class CreateTaskInputModel
    {
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        public string TaskName { get; set; }
        public DateTime? TaskStart { get; set; }
        public DateTime? TaskEnd { get; set; }
        public Double Timespan { get; set; }

    }
}
