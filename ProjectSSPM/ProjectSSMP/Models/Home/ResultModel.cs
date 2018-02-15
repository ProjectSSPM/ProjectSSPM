using System;
namespace ProjectSSMP.Models.Home
{
    public class ResultModel
    {
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public DateTime? FunctionStart { get; set; }
        public DateTime? FunctionEnd { get; set; }
        public DateTime? FActualStart { get; set; }
        public DateTime? FActualEnd { get; set; }
        public string Username { get; set; }

        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime? TaskStart { get; set; }
        public DateTime? TaskEnd { get; set; }
        public DateTime? TActualStart { get; set; }
        public DateTime? TActualEnd { get; set; }

    }
}
