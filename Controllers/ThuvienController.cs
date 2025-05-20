using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Models;

namespace Web_truyen.Controllers
{
    [RoleUser]
    public class ThuvienController : Controller
    {
        private Web_TruyenEntities db = new Web_TruyenEntities();

        // GET: Thuvien
        public ActionResult Thuvien(string tab = "theodoi")
        {
            var user = SessionConfig.GetUser();

            if (user == null)
            {
                // Nếu chưa đăng nhập, redirect hoặc xử lý phù hợp
                return RedirectToAction("Login", "Account");
            }

            int userId = user.userId;

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

            // Dictionary lưu chương đọc gần nhất của mỗi truyện
            var chuongGanNhatDict = new Dictionary<int, int?>();

            foreach (var truyen in danhSach)
            {
                int truyenId = truyen.truyenId;
                int? chuongDocGanNhatId = null;

                var thuVien = db.ThuVien.FirstOrDefault(tv => tv.UserID == userId && tv.TruyenID == truyenId);
                if (thuVien != null)
                {
                    chuongDocGanNhatId = thuVien.ChuongDocGanNhat;
                }

                if (chuongDocGanNhatId == null)
                {
                    var chuongDauTien = db.Chuong
                        .Where(c => c.truyenId == truyenId && c.DaDang == true)
                        .OrderBy(c => c.ThuTu)
                        .FirstOrDefault();

                    chuongDocGanNhatId = chuongDauTien?.ChuongId;
                }

                chuongGanNhatDict[truyenId] = chuongDocGanNhatId;
            }

            ViewBag.CurrentTab = tab;
            ViewBag.ChuongGanNhatDict = chuongGanNhatDict;

            return View(danhSach);
        }
    }
}
