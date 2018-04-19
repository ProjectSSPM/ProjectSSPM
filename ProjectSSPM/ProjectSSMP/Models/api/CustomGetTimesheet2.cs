using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSMP.Models.api.NewModel
{
    public class CustomGetTimesheet2
    {
        public string userId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string token { get; set; }
        public string projectNumber { get; set; }
    }
}
