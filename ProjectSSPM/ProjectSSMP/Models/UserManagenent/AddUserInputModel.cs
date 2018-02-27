using System;
using System.ComponentModel.DataAnnotations;

namespace SSMP.Models.UserManagement
{
    public class AddUserInputModel
    {


        public string UserId { get; set; }

        
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Username must be 5-30 letters.")]        
        [Required]
        public string Username { get; set; }
        

        [StringLength(30, MinimumLength = 5, ErrorMessage = "Password must be 5-30 letters.")]
        [Required]
        public string Password { get; set; }
        
        [StringLength(50, MinimumLength = 5, ErrorMessage = "FirstName must be 5-30 letters.")]
        [Required]
        public string Firstname { get; set; }
        
        [StringLength(50, MinimumLength = 5, ErrorMessage = "LastName must be 5-30 letters.")]
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string JobResponsible { get; set; }
        public string UserCreateBy { get; set; }
        public DateTime? UserCreateDate { get; set; }
        public string UserEditBy { get; set; }
        public DateTime? UserEditDate { get; set; }
        
        [Required]
        public string GroupId { get; set; }
        [StringLength(50, MinimumLength = 9, ErrorMessage = "Tel. must be 9-15 letters.")]
        [Required]
        public string UserTel { get; set; }
        
        [Required]
        public string LineId { get; set; }
    }
}
