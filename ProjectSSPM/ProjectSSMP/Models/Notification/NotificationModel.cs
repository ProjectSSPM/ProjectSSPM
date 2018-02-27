using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.Notification
{
    public class NotificationModel
    {
        public string FunctionId { get; set; }
        public string TaskId { get; set; }
        public string FunctionName { get; set; }
        public DateTime? FunctionStart { get; set; }
        public DateTime? FunctionEnd { get; set; }
        public DateTime? Datenow { get; set; }
        public string ProjectNumber { get; set; }
       
    }
}
