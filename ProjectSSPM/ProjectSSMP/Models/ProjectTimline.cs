using System;
using System.Collections.Generic;

namespace ProjectSSMP.Models
{
    public partial class ProjectTimline
    {
        public string TimelineId { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime? TimelineDate { get; set; }
        public string Hader { get; set; }
        public string Note { get; set; }
    }
}
