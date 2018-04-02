using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class AccountInfo
    {
        public string accountId { get; set; }
        public string accountGroup { get; set; }
        public string accountName { get; set; }
        public string accountPosition { get; set; }
        public string accountImage { get; set; }
        public string deviceId { get; set; }
    }
}
