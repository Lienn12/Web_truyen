using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Models;

namespace Web_truyen.Areas.Admin.Controllers
{
    public class TheoDoiNguoiDungController : Controller
    {
        private Web_TruyenEntities db = new Web_TruyenEntities();
        // GET: Admin/TheoDoiNguoiDung
        [HttpPost]
        public ActionResult TheoDoiNguoiDung(int idNguoiDuocTheoDoi)
        {
            int idNguoiDangTheoDoi = SessionConfig.GetUser().userId;
            var daTheoDoi = db.TheoDoi_NguoiDung.Any(td =>
                td.Users.userId == idNguoiDangTheoDoi && td.Users1.userId == idNguoiDuocTheoDoi);

            if (!daTheoDoi)
            {
                var theoDoi = new TheoDoi_NguoiDung
                {
                    NguoiTheoDoiId = idNguoiDangTheoDoi,
                    TheoDoiNguoiDungId = idNguoiDuocTheoDoi,
                    NgayTheoDoi = DateTime.Now
                };
                db.TheoDoi_NguoiDung.Add(theoDoi);
                db.SaveChanges();
            }
            return RedirectToAction("HoSoCaNhan", "User", new { area = "Admin", useId = idNguoiDuocTheoDoi });


        }
        [HttpPost]
        public ActionResult HuyTheoDoiNguoiDung(int idNguoiDuocHuy)
        {
            int idNguoiDangTheoDoi = SessionConfig.GetUser().userId;

            var theoDoi = db.TheoDoi_NguoiDung.FirstOrDefault(td =>
                td.NguoiTheoDoiId == idNguoiDangTheoDoi && td.TheoDoiNguoiDungId == idNguoiDuocHuy);

            if (theoDoi != null)
            {
                db.TheoDoi_NguoiDung.Remove(theoDoi);
                db.SaveChanges();
            }

            return RedirectToAction("HoSoCaNhan", "User", new { area = "Admin", useId = idNguoiDuocHuy });
        }

    }
}