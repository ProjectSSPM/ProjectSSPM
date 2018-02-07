using System;
namespace ProjectSSMP.Models.Timeline
{
    public class TimelineInputModel
    {
        public string TimelineId { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime? TimelineDate { get; set; }
        public string Header { get; set; }
        public string Note { get; set; }

        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime? ProjectEnd { get; set; }
    }
}