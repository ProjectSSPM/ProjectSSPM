using System;
namespace ProjectSSMP.Models.Timesheet
{
    public class TimeSheetInputModel
    {
        public DateTime TimeSheetId { get; set; }
        public string ActionId { get; set; }
        public DateTime? TimeSheetStart { get; set; }
        public DateTime? TimeSheetEnd { get; set; }
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Note { get; set; } 
        public DateTime? ProjectEnd { get; set; }
        public string TimeSheetNumber { get; set; }
        public string ActionName { get; set; }

        

    }
}
