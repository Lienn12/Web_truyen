using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Areas.Admin.Models;
using Web_truyen.Models;
using System.Data.Entity;
using PagedList;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Web_truyen.Areas.Admin.Controllers
{

    public class TruyenController : Controller
    {
        Web_TruyenEntities db = new Web_TruyenEntities();

        // GET: Admin/Truyen
        [RoleUser]
        public ActionResult DanhSach(string filter, string searchTerm, bool? statusFilter, int? page)
        {
            var danhSach = db.Truyen.Select(t => new ListTruyenViewModel
            {
                TruyenId = t.truyenId,
                TieuDe = t.TieuDe,
                AnhBia = t.AnhBia,
                Username = t.Users.Username,
                NgayTao = t.NgayTao,
                TrangThai = t.TrangThai,
                VaiTro= t.Users.VaiTro,
                DaDang = t.DaDang,
                SoChuongChoDuyet = db.Chuong.Count(c => c.truyenId == t.truyenId && c.TrangThaiDuyet == "Chờ duyệt")
            }).AsQueryable();
            danhSach = danhSach.Where(t => db.Chuong.Any(c => c.truyenId == t.TruyenId && c.TrangThaiDuyet != null));
            ViewBag.CurrentFilter = filter;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = statusFilter;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lowerSearchTerm = searchTerm.ToLower();
                danhSach = danhSach.Where(t =>
                    t.TieuDe.ToLower().Contains(lowerSearchTerm) ||
                    t.Username.ToLower().Contains(lowerSearchTerm)
                );
            }
            if (statusFilter.HasValue)
            {
                danhSach = danhSach.Where(t => t.TrangThai == statusFilter.Value);
            }

            int pageNumber = page ?? 1;
            int pageSize = 10;
            switch (filter)
            {
                case "donggop":
                    danhSach = danhSach.Where(t =>
                        t.VaiTro == "author" &&
                        db.Chuong.Any(c => c.truyenId == t.TruyenId && c.TrangThaiDuyet != null)
                    );
                    break;
                case "bibaocao":
                    danhSach = danhSach.Where(t => db.BaoCao.Any(b => b.DoiTuongId == t.TruyenId && b.LoaiBaoCao == "truyen") && t.DaDang == true);
                    break;
                default:
                    break;
            }
            var pagedList = danhSach.OrderByDescending(t => t.NgayTao).ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }


        [RoleUser]
        public ActionResult ThemMoi()
        {
                if (SessionConfig.GetUser() == null)
                {
                    TempData["ReturnUrl"] = Request.Url?.PathAndQuery;
                    return RedirectToAction("Login", "Account",new {area=""});
                }
                var model = new Truyen
                {
                    Users = SessionConfig.GetUser(),
                    XepLoai = false,
                    TrangThai = false,
                    userId = SessionConfig.GetUser().userId,
                    NgayTao = DateTime.Now.Date,
                    NgayCapNhap=DateTime.Now.Date,
                    DaDang = false,
                    LuotDoc = 0,
                    TheLoaiId = 2
                };
                ViewBag.Title = model.TieuDe;
            ViewBag.TheLoaiId = new SelectList(db.TheLoai, "TheLoaiId", "TenTheLoai", model.TheLoaiId);
            return View(model);
        }

        // POST: Admin/Truyen/ThemMoi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoi(Truyen model, HttpPostedFileBase AnhBiaFile, string action)
        {
            if (action == "Cancel")
            {
                return RedirectToAction("DanhSach", "Truyen", new { area = "Admin" });
            }
            var currentUser = SessionConfig.GetUser();
            model.userId = currentUser.userId;
            model.TieuDe = string.IsNullOrWhiteSpace(model.TieuDe) ? "Truyện chưa có tiêu đề" : model.TieuDe;
            model.MoTa = string.IsNullOrWhiteSpace(model.MoTa) ? "Mô tả chưa có" : model.MoTa;
            model.TheLoaiId = model.TheLoaiId == 0 ? 2 : model.TheLoaiId;
            if (AnhBiaFile != null && AnhBiaFile.ContentLength > 0)
            {
                string filePath = Path.Combine(Server.MapPath("~/Assets/img"), Path.GetFileName(AnhBiaFile.FileName));
                AnhBiaFile.SaveAs(filePath);
                model.AnhBia = Path.GetFileName(AnhBiaFile.FileName);
            }
            else
            {
                string tenFileAnhMacDinh = Guid.NewGuid().ToString() + ".png";
                string pathOutput = Server.MapPath("~/Assets/img/" + tenFileAnhMacDinh);
                ImageGenerator.TaoAnhBiaMacDinh(model.TieuDe, pathOutput);
                model.AnhBia = tenFileAnhMacDinh;
            }

            if (ModelState.IsValid)
            {

                db.Truyen.Add(model);
                db.SaveChanges();
                    int truyenIdMoi = model.truyenId;
                    var chuongMoi = new Chuong
                    {
                        truyenId = truyenIdMoi,
                        TieuDe = "Chưa có tiêu đề phần 1",
                        NoiDung = "",
                        NgayTao = DateTime.Now,
                        DaDang = false,
                    };
                    db.Chuong.Add(chuongMoi);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "Chuong", new { area = "Admin", id = chuongMoi.ChuongId });
             }
            ViewBag.Title = model.TieuDe;
             ViewBag.TheLoaiId = new SelectList(db.TheLoai, "TheLoaiId", "TenTheLoai", model.TheLoaiId);
            return View(model);
        }


        // GET: Admin/Truyen/ChiTiet/5
        public ActionResult ChiTiet(int id)
        {
            var truyen = db.Truyen
                .Include(t => t.Users)
                .FirstOrDefault(t => t.truyenId == id);

            if (truyen == null)
            {
                return HttpNotFound();
            }

            var goiY = db.Truyen
                .Where(t => t.truyenId != id && t.DaDang == true)
                .OrderBy(r => r.NgayTao) 
                .Take(6)
                .ToList();
            ViewBag.TruyenGoiY = goiY;

            var currentUser = Web_truyen.App_Start.SessionConfig.GetUser();
            var permission = new UserPermissionViewModel();
            if (currentUser != null)
            {
                permission.VaiTro = currentUser.VaiTro;
                permission.IsAuthorOfTruyen = truyen != null && truyen.userId == currentUser.userId;
            }

            ViewBag.Permission = permission;
            List<Chuong> chapters;

            if (!permission.IsAuthorOfTruyen)
            {
                chapters = db.Chuong
                    .Where(c => c.truyenId == id && c.DaDang == true) 
                    .ToList();
            }
            else
            {
                chapters = db.Chuong
                    .Where(c => c.truyenId == id) 
                    .ToList();
            }

            ViewBag.DanhSachChuong = chapters;
            ViewBag.TongSoChuong = chapters.Count;
            var chuongDauTien = db.Chuong
                .Where(c => c.truyenId == truyen.truyenId)
                .OrderBy(c => c.ThuTu) 
                .FirstOrDefault();
            ViewBag.ChuongDauTienId = chuongDauTien.ChuongId;
            ViewBag.ChuongDangChon = 0;
            ViewBag.TruyenId = id;
            ViewBag.TheLoaiId = new SelectList(db.TheLoai, "TheLoaiId", "TenTheLoai", truyen.TheLoaiId);

            return View(truyen);
        }

        // GET: Admin/Truyen/Edit/5
        [RoleUser]
        public ActionResult Edit(int id)
        {
            var truyen = db.Truyen
                .Include(t => t.Users)
                .FirstOrDefault(t => t.truyenId == id);

            if (truyen == null)
            {
                return HttpNotFound();
            }
            var userId = Web_truyen.App_Start.SessionConfig.GetUser().userId;
            if (truyen != null)
            {
                ViewBag.IsAuthor = truyen.userId == userId;
            }
            var chapters = db.Chuong.Where(c => c.truyenId == id).ToList();
            ViewBag.TruyenId = id;
            ViewBag.TheLoaiId = new SelectList(db.TheLoai, "TheLoaiId", "TenTheLoai", truyen.TheLoaiId);
            return View(truyen);
        }

        // POST: Admin/Truyen/Edit/5
        [HttpPost]
        public ActionResult Edit(Truyen model, HttpPostedFileBase AnhBiaFile)
        {
            if (ModelState.IsValid)
            {
                var truyen = db.Truyen.Find(model.truyenId);
                if (truyen == null) return HttpNotFound();

                if (AnhBiaFile != null && AnhBiaFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(AnhBiaFile.FileName);
                    var folderPath = Server.MapPath("~/Assets/img");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var path = Path.Combine(folderPath, fileName);
                    AnhBiaFile.SaveAs(path);
                    truyen.AnhBia = fileName;
                }
                truyen.TieuDe = model.TieuDe;
                truyen.MoTa = model.MoTa;
                truyen.TheLoaiId = model.TheLoaiId;
                truyen.XepLoai = model.XepLoai;
                truyen.NgayCapNhap = DateTime.Now;
                truyen.TrangThai = model.TrangThai;
                db.Entry(truyen).State = EntityState.Modified;
                db.SaveChanges();
                var nextAction = Request["NextAction"] ?? "Edit";
                if (nextAction == "DocChuong")
                {
                    return RedirectToAction("DocChuong","Chuong", new {area="Admin", id = TempData["ChuongId"] });
                }
                TempData["SuccessMessage"] = "Cập nhật truyện thành công!";
                return RedirectToAction("Edit", new { id = model.truyenId });
            }

            ViewBag.TheLoaiId = new SelectList(db.TheLoai, "TheLoaiId", "TenTheLoai", model.TheLoaiId);
            return View(model);
        }
        public ActionResult MucLucEdit(int truyenId)
        {
            if (truyenId <= 0)
            {
                ViewBag.ErrorMessage = "Truyện không hợp lệ!";
                return View("Error"); 
            }

            var chuongs = db.Chuong
                .Where(c => c.truyenId == truyenId)
                .OrderBy(c => c.NgayTao)
                .ToList();

            var userId = Web_truyen.App_Start.SessionConfig.GetUser().userId;
            var truyen = db.Truyen
               .Include(t => t.Users)
               .FirstOrDefault(t => t.truyenId == truyenId);
            if (truyen != null)
            {
                ViewBag.IsAuthor = truyen.userId == userId;
            }

            if (!chuongs.Any())
            {
                ViewBag.ErrorMessage = "Không tìm thấy chương nào!";
                return View("MucLucEdit", new List<Web_truyen.Models.Chuong>());
            }

            return View("MucLucEdit", chuongs);
        }


        // GET: Admin/Truyen/Delete/5
        [RoleUser]
        public ActionResult Delete(int id)
        {
            var truyen = db.Truyen
                .Include(t => t.Chuong)               
                .Include(t => t.BinhLuan)             
                .Include(t => t.TheoDoi_Truyen)      
                .Include(t => t.ThuVien)             
                .FirstOrDefault(t => t.truyenId == id);  

            if (truyen == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy truyện cần xóa!";
                return RedirectToAction("DanhSach");
            }

            try
            {
                db.BinhLuan.RemoveRange(db.BinhLuan.Where(b => b.truyenId == id));
                db.Chuong.RemoveRange(db.Chuong.Where(c => c.truyenId == id));
                db.TheoDoi_Truyen.RemoveRange(db.TheoDoi_Truyen.Where(td => td.truyenId == id));
                db.ThuVien.RemoveRange(db.ThuVien.Where(tv => tv.TruyenID == id));
                db.BaoCao.RemoveRange(db.BaoCao.Where(bc => bc.LoaiBaoCao == "truyen" && bc.DoiTuongId == id));

                // Cuối cùng xóa truyện
                db.Truyen.Remove(truyen);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Truyện đã được xóa thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa truyện: " + ex.Message;
            }

            return RedirectToAction("DanhSach");
        }

    }
}
