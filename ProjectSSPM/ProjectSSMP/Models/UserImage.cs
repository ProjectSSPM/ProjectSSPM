using System;
using System.Collections.Generic;

namespace ProjectSSMP.Models
{
    public partial class UserImage
    {
        public string UserId { get; set; }
        public byte[] Image { get; set; }
        public int? ImageNumber { get; set; }
    }
}
