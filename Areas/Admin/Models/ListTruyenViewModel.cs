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
        public string UserId { get; set; }
        public DateTime NgayTao { get; set; }
        public string TrangThai {  get; set; }

    }
}