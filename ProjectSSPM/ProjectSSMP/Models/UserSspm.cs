﻿using System;
using System.Collections.Generic;

namespace SSMP.Models
{
    public partial class UserSspm
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserCreateBy { get; set; }
        public DateTime? UserCreateDate { get; set; }
        public string UserEditBy { get; set; }
        public DateTime? UserEditDate { get; set; }
        public string JobResponsible { get; set; }
        public string Status { get; set; }
        public string UserTel { get; set; }
        public string LineId { get; set; }
    }
}
