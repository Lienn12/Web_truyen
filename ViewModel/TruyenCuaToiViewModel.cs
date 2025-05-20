using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_truyen.Models;

namespace Web_truyen.ViewModel
{
    public class TruyenCuaToiViewModel
    {
        public int TruyenId { get; set; }
        public string TieuDe { get; set; }
        public int SoChuongDaDang { get; set; }
        public int SoBanThao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string AnhBia { get; set; }
        public List<Chuong> DanhSachChuong { get; set; }
    }
}