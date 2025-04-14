using System;
using System.ComponentModel.DataAnnotations;

namespace Web_truyen.ViewModel
{
    public class ChangePasswordViewModel
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có tối thiểu 6 ký tự và tối đa 20 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Mật khẩu không khớp")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
