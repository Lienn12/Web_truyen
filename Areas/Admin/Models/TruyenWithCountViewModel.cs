using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_truyen.Models;

namespace Web_truyen.Areas.Admin.Models
{
    public class TruyenWithCountViewModel
    {
        public Truyen Truyen { get; set; }
        public int SoChuong { get; set; }
    }
}