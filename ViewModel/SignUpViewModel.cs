using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_truyen.ViewModel
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string Username {  get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage="Email không đúng định dạng")]
        public string Email {  get; set; }
        [Required(ErrorMessage = "Password không được để trống")]
        [StringLength(20,MinimumLength =6, ErrorMessage ="Mật khẩu phải có tối thiểu 6 ký tự và tối đa 20 ký tự")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage ="Mật khẩu không khớp")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password không được để trống")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {  get; set; }

    }
}