using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectSSMP.Models.UserManagement
{
    public class AddUserInputModel
    {


        public string UserId { get; set; }
<<<<<<< HEAD
        [RegularExpression(@"^[a-zA-Z0-9]",ErrorMessage = "กรุณากรอกภาษาอังกฤษ")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Min")]        
        [Required(ErrorMessage ="กรุณาใส่")]
        public string Username { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9]", ErrorMessage = "กรุณากรอกภาษาอังกฤษ")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Min")]
=======
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Username must be 5-30 letters.")]
>>>>>>> 94bc0a21e360fc2dfabd1717a66fbb210181f62b
        [Required]
        public string Password { get; set; }
        [RegularExpression(@"^[a-zA-Z]", ErrorMessage = "กรุณากรอกภาษาอังกฤษ")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Min")]
        [Required]
        public string Firstname { get; set; }
        [RegularExpression(@"^[a-zA-Z]", ErrorMessage = "กรุณากรอกภาษาอังกฤษ")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Min")]
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string JobResponsible { get; set; }
        public string UserCreateBy { get; set; }
        public DateTime? UserCreateDate { get; set; }
        public string UserEditBy { get; set; }
        public DateTime? UserEditDate { get; set; }
        public string GroupId { get; set; }
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Min")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string UserTel { get; set; }
        [Required]
        public string LineId { get; set; }
    }
}
