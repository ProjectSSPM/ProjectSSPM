﻿using System;
using System.Collections.Generic;

namespace SSMP.Models
{
    public partial class Project
    {
        public string ProjectNumber { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectManager { get; set; }
        public DateTime? ProjectStart { get; set; }
        public DateTime? ProjectEnd { get; set; }
        public long? ProjectCost { get; set; }
        public string ProjectCreateBy { get; set; }
        public DateTime? ProjectCreateDate { get; set; }
        public string ProjectEditBy { get; set; }
        public DateTime? ProjectEditDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTel { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public string Note { get; set; }
        public int? Variant { get; set; }
        public string ProjectStatus { get; set; }
    }
}
