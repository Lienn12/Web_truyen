using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_truyen.ViewModel
{
    public class ListUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int SoTacPham { get; set; }
        public string VaiTro { get; set; }
        public bool TrangThai {  get; set; }
        public string Avt { get; set; }
        public int BiBaoCao { get; set; }
    }
}