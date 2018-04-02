using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomFilterProject
    {
        public string projectNumber { get; set; }
        public string projectName { get; set; }
        public string projectManagerId { get; set; }
        public string projectManagerName { get; set; }
        public string customerName { get; set; }
        public DateTime projectStart  { get; set; }
        public DateTime projectEnd { get; set; }


    }
}
