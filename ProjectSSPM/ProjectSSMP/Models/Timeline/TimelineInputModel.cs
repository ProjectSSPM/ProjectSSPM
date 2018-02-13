using System;
namespace ProjectSSMP.Models.Timeline
{
    public class TimelineInputModel
    {
        public string FunctionId { get; set; }
        public DateTime FunctionLogId { get; set; }
        public DateTime? FunctionStart { get; set; }
        public DateTime? FunctionEnd { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public string StatusId { get; set; }
        public int? Variant { get; set; }
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        public string FunctionNumber { get; set; }
        public string TaskName { get; set; }
        public string FunctionName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

    }
}