using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomCommentServer
    {
        public string userId { get; set; }
        public string bNumber { get; set; }
        public string commentDetail { get; set; }
        public string fullName { get; set; }
        public string cid { get; set; }
        public DateTime? time { get; set; }

        public string base64Image { get; set; }
    }
}
