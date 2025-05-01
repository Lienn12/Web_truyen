using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_truyen.Models;

namespace Web_truyen.Areas.Admin.Models
{
    public class ListTruyenViewModel
    {
        public int TruyenId {  get; set; }
        public string AnhBia {  get; set; }
        public string TieuDe {  get; set; }
        public string Username { get; set; }
        public DateTime? NgayTao { get; set; }
        public Nullable<bool> TrangThai {  get; set; }
        public bool LoaiDang { get; set; }  
        public bool BiBaoCao { get; set; }
        public bool DaDang { get; set; }
        public string VaiTro {  get; set; }
        public int SoChuongChoDuyet { get; set; }
    }
}