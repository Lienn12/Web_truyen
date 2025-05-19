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
    public class TheoDoiTruyenController : Controller
    {
        Web_TruyenEntities db = new Web_TruyenEntities();
        // GET: TheoDoiTruyen
        [HttpPost]
        public ActionResult AddThuvien(int truyenId)
        {
            var userId = SessionConfig.GetUser().userId;

            var exists = db.TheoDoi_Truyen.FirstOrDefault(t => t.truyenId == truyenId && t.userId == (int)userId);

            string message = "";

            if (exists == null)
            {
                db.TheoDoi_Truyen.Add(new TheoDoi_Truyen
                {
                    truyenId = truyenId,
                    userId = (int)userId,
                    NgayTheoDoi = DateTime.Now
                });
                db.SaveChanges();
                message = "Đã thêm truyện vào thư viện theo dõi.";
            }
            else
            {
                db.TheoDoi_Truyen.Remove(exists);
                db.SaveChanges();
                message = "Đã bỏ theo dõi truyện.";
            }

            TempData["NotifyMessage"] = message;

            return RedirectToAction("ChiTiet", "Truyen", new { area = "Admin", id = truyenId });
        }

    }
}