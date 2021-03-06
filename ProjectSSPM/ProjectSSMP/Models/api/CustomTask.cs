﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomTask
    {
        public string TaskId { get; set; }
        public string ProjectNumber { get; set; }
        public string TaskName { get; set; }
        public DateTime? TaskStart { get; set; }
        public DateTime? TaskEnd { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public int? Variant { get; set; }
        public string bgColor { get; set; }
        public string deltaForce { get; set; }
    }
}
