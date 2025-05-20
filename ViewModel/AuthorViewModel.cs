using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_truyen.Models;

namespace Web_truyen.ViewModel
{
    public class AuthorViewModel
    {
        public Users User { get; set; }
        public int TruyenCount { get; set; }
        public int FollowerCount { get; set; }
    }
}