using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_truyen.Areas.Admin.Models
{
    public class ListUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int SoTacPham { get; set; }
        public string VaiTro { get; set; }
        public bool TrangThai {  get; set; }
    }
}