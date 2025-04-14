using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Areas.Admin.Models;
using Web_truyen.Models;
namespace Web_truyen.Areas.Admin.Controllers
{
    public class TruyenController : Controller
    {
        Web_TruyenEntities2 db = new Web_TruyenEntities2();

        // GET: Admin/Truyen
        public ActionResult DanhSach()
        {
            ViewBag.Titel = "Danh sách truyện";
            var list = db.Truyen.Select(t => new ListTruyenViewModel
            {
                TruyenId = t.truyenId,
                AnhBia = t.AnhBia,
                TieuDe = t.TieuDe,
                NgayTao = t.NgayTao ?? DateTime.MinValue,
                TrangThai = t.TrangThai
            }).ToList();
            return View(list);
        }

        // GET: Admin/Truyen/ThemMoi
        public ActionResult ThemMoi()
        {
            var model = new Truyen
            {
                XepLoai = false // Đảm bảo giá trị mặc định là false
            };
            ViewBag.TheLoaiId = new SelectList(db.TheLoai, "TheLoaiId", "TenTheLoai"); // Đảm bảo ViewBag có dữ liệu cho dropdown
            return View(model);
        }

        [HttpPost]
        public ActionResult ThemMoi(Truyen model, HttpPostedFileBase AnhBiaFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu người dùng upload ảnh
                if (AnhBiaFile != null && AnhBiaFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(AnhBiaFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Images"), fileName);
                    AnhBiaFile.SaveAs(path);
                    model.AnhBia = fileName; // Lưu tên file ảnh vào model
                }

                db.Truyen.Add(model); // Thêm truyện vào cơ sở dữ liệu
                db.SaveChanges();
                return RedirectToAction("Index"); // Quay lại trang index sau khi lưu
            }

            ViewBag.TheLoaiId = new SelectList(db.TheLoai, "TheLoaiId", "TenTheLoai"); // Đảm bảo dropdown vẫn có dữ liệu nếu model không hợp lệ
            return View(model);
        }



    }
}
