using System;
using System.Collections.Generic;

namespace ProjectSSMP.Models
{
    public partial class ProjectTimeline
    {
        public string TimelineId { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime? TimelineDate { get; set; }
        public string Header { get; set; }
        public string Note { get; set; }
    }
}
