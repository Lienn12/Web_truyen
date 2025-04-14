using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_truyen.ViewModel
{
    public class loginViewModel
    {
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
        [Display(Name ="Remember me?")]
        public bool RememberMe {  get; set; }
    }
}