using System.Collections.Generic;
using Web_truyen.Models;

namespace Web_truyen.Areas.Admin.Models
{
    public class ChuongViewModel
    {
        public Chuong Chuong { get; set; }
        public List<Chuong> DanhSachChuong { get; set; }
        public List<BinhLuan> DanhSachBinhLuan { get;set; }
    }
}
