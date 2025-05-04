using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_truyen.Areas.Admin.Models
{
    public class UserPermissionViewModel
    {
        public string VaiTro { get; set; }
        public bool IsAdmin => VaiTro == "admin";
        public bool IsAuthor => VaiTro == "author";
        public bool IsAuthorOfTruyen { get; set; }  
        public bool CanEdit => (IsAdmin && IsAuthorOfTruyen) || IsAuthor;
        public bool IsAdminNotAuthor => IsAdmin && !IsAuthorOfTruyen;
    }
}