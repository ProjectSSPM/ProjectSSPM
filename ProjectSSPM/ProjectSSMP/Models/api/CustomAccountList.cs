using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class CustomAccountList
    {
        public string userId { get; set; }

        public string username { get; set; }

        public object password { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string userCreateBy { get; set; }
        public DateTime? userCreateDate { get; set; }

        public string userEditBy { get; set; }

        public DateTime? userEditDate { get; set; }
        public string groupId { get; set; }
        public string groupName { get; set; }

        public string jobResponsible { get; set; }

        public string status { get; set; }

        public string userTel { get; set; }

        public string lineId { get; set; }
        public string base64Image { get; set; }
        public byte[] decodeImage { get; set; }
    }
}
