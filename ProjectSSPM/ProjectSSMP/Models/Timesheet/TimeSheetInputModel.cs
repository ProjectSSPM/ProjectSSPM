using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SSMP.Models.Timesheet
{
    public class TimeSheetInputModel
    {
        public DateTime TimeSheetId { get; set; }
        public string ActionId { get; set; }
        [Required]
        public DateTime? TimeSheetStart { get; set; }
        [Remote(action: "VarifyDateTimeSheet", controller: "validation", AdditionalFields = nameof(TimeSheetStart) + "," + nameof(TimeSheetEnd) + "," + nameof(FunctionId))]
        [Required]
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
        public string Approve1 { get; set; }
        public string Approve2 { get; set; }

    }
}
