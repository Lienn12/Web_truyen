using System;
using System.Collections.Generic;

namespace Web_truyen.Areas.Admin.Models
{
    public class ThongKeTruyenViewModel
    {
        public string TieuDe { get; set; }
        public int TongLuotDoc { get; set; }
        public int TongChuong { get; set; }
        public int TongBinhChon { get; set; }
        public string AnhBia { get; set; }

        public List<ThongKeNgay> DocGiaMoiNgay { get; set; }
        public List<ThongKeNgay> BinhLuanMoiNgay { get; set; }
    }

    public class ThongKeNgay
    {
        public DateTime Ngay { get; set; }
        public int GiaTri { get; set; }
    }
}
