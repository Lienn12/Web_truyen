using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Models;

namespace Web_truyen.Controllers
{
    [RoleUser]        
    public class ThuvienController : Controller
    {
        Web_TruyenEntities db = new Web_TruyenEntities();
        // GET: Thuvien
        
        public ActionResult Thuvien(string tab = "theodoi")
        {
            var user = SessionConfig.GetUser();
            int userId = user.userId;

            ViewBag.CurrentTab = tab;

            List<Truyen> danhSach;
            if (tab == "ganDay")
            {
                danhSach = db.ThuVien
                             .Where(x => x.UserID == userId)
                             .OrderByDescending(x => x.ThoiGianDocGanNhat)
                             .Select(x => x.Truyen)
                             .Distinct()
                             .Take(20)
                             .ToList();
            }
            else
            {
                danhSach = db.TheoDoi_Truyen
                             .Where(x => x.userId == userId)
                             .Select(x => x.Truyen)
                             .ToList();
            }

            return View(danhSach);
        }

    }
}
