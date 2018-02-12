using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectSSMP.Models.UserManagement
{
    public class AddUserInputModel
    {

        
        public string UserId { get; set; }
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Username must be 5-30 letters.")]
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string JobResponsible { get; set; }
        public string UserCreateBy { get; set; }
        public DateTime? UserCreateDate { get; set; }
        public string UserEditBy { get; set; }
        public DateTime? UserEditDate { get; set; }
        public string GroupId { get; set; }
        public string UserTel { get; set; }
        public string LineId { get; set; }
    }
}
