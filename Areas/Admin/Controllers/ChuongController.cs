using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Services.Description;
using Web_truyen.App_Start;
using Web_truyen.Areas.Admin.Models;
using Web_truyen.Models;
using Ganss.Xss;
using System.Web;
namespace Web_truyen.Areas.Admin.Controllers
{

    public class ChuongController : Controller
    {
        private Web_TruyenEntities db = new Web_TruyenEntities();
        public ActionResult MucLuc(int truyenId, int? chuongId = null)
        {
            try
            {
                var truyen = db.Truyen.Find(truyenId);
                if (truyen == null) return HttpNotFound();
                var user = Web_truyen.App_Start.SessionConfig.GetUser();
                var laTacGia = user != null && truyen.userId == user.userId;
                var chuongList = db.Chuong.Where(c => c.truyenId == truyenId)
                                        .OrderBy(c => c.NgayTao).ToList();
                if (chuongList.Count == 0)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy chương nào!";
                }
                ViewBag.laTacGia = laTacGia;
                ViewBag.IsDocChuong = !laTacGia; 
                ViewBag.HienThiNutThemChuong = laTacGia;
                ViewBag.TruyenId = truyenId;
                ViewBag.ActionName = laTacGia ? "Edit" : "Doc";
                ViewBag.ControllerName = "Chuong";

                int chuongDangDocId = chuongId ?? 0;

                return PartialView("MucLuc", Tuple.Create(chuongList, chuongDangDocId));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return PartialView("MucLuc", Tuple.Create(new List<Chuong>(), 0));
            }
        }

        [RoleUser]
        [HttpPost]
        public ActionResult Create(int truyenId)
        {
            if (truyenId == 0)
            {
                return RedirectToAction("Index", "Truyen");
            }
            var danhSachChuong = db.Chuong
                .Where(c => c.truyenId == truyenId)
                .OrderBy(c => c.ChuongId)
                .ToList();
            
            int soThuTuChuong = danhSachChuong.Count > 0 ? danhSachChuong.Count + 1 : 1;

            var chuongMoi = new Chuong
            {
                truyenId = truyenId,
                TieuDe = "Chưa có tiêu đề phần " + soThuTuChuong,
                DaDang = false,
                NoiDung = "",
                NgayTao = DateTime.Now,

            };
            ViewBag.TruyenId = truyenId;
            db.Chuong.Add(chuongMoi);
            db.SaveChanges();

            return RedirectToAction("Edit", "Chuong", new { area = "Admin", id = chuongMoi.ChuongId });
        }

            [RoleUser]
            public ActionResult Edit(int id)
            {
                var chuong = db.Chuong.Find(id);
                if (chuong == null)
                {
                    return HttpNotFound();
                }
                var truyen = db.Truyen.Find(chuong.truyenId);
                ViewBag.TieuDeTruyen = truyen != null && truyen.TieuDe != null ? truyen.TieuDe : "Chưa có tiêu đề";
                ViewBag.TieuDeChuong = chuong.TieuDe ?? "Không có tiêu đề";
                if (chuong.TrangThaiDuyet == "chờ duyệt")
                {
                    ViewBag.TrangThaiChuong = "Chờ duyệt";
                }
                else if (chuong.TrangThaiDuyet == "Từ chối")
                {
                    ViewBag.TrangThaiChuong = "Từ chối";
                }
                else
                {
                    ViewBag.TrangThaiChuong = chuong.DaDang ? "Đã đăng" : "Bản thảo";
                }

                    ViewBag.ChuongDangChon = chuong.ChuongId;
                var viewModel = new ChuongViewModel
                {
                    Chuong = chuong,
                    DanhSachChuong = db.Chuong
                        .Where(c => c.truyenId == chuong.truyenId)
                        .OrderBy(c => c.ChuongId)
                        .ToList()
                };
                var danhSachTheLoai = db.TheLoai.ToList();

                ViewBag.TheLoai = new SelectList(danhSachTheLoai, "TheLoaiId", "TenTheLoai", truyen.TheLoaiId);

                var currentUser = SessionConfig.GetUser();
                if (currentUser != null)
                {
                    ViewBag.CurrentUserId = currentUser.userId;
                }
                else
                {
                    ViewBag.CurrentUserId = null; 
                }
            TempData["ChuongId"] = chuong.ChuongId;

            ViewBag.TruyenId = chuong.truyenId;

                return View(viewModel);
            }


        [RoleUser]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChuongViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                var chuongCanSua = db.Chuong.Find(viewModel.Chuong.ChuongId);
                if (chuongCanSua == null)
                {
                    TempData["ErrorMessage"] = "Chương không tồn tại.";
                    return RedirectToAction("Index", "Chuong");
                }

                chuongCanSua.TrangThaiDuyet = viewModel.Chuong.TrangThaiDuyet;
                chuongCanSua.TieuDe = viewModel.Chuong.TieuDe;
                chuongCanSua.NoiDung = viewModel.Chuong.NoiDung;
                chuongCanSua.DaDang = viewModel.Chuong.DaDang;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Lỗi khi lưu chương: " + ex.Message;
                }

                if (chuongCanSua.DaDang)
                {
                    var truyen = db.Truyen.Find(chuongCanSua.truyenId);
                    if (truyen != null && !truyen.DaDang)
                    {
                        truyen.DaDang = true;
                    }

                    var userId = SessionConfig.GetUser().userId;
                    var user = db.Users.Find(userId);
                    if (user != null && user.VaiTro == "user" )
                    {
                        user.VaiTro = "author";
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Lỗi khi cập nhật: " + ex.Message;
                    }

                    return RedirectToAction("DocChuong", new { id = viewModel.Chuong.ChuongId });
                }
                else
                {
                    return RedirectToAction("Edit", new { id = viewModel.Chuong.ChuongId });
                }
            }

            return RedirectToAction("Edit", new { id = viewModel.Chuong.ChuongId });
        }

        public ActionResult DocChuong(int id)
        {
            var chuong = db.Chuong
                .Include(c => c.Truyen)
                .Include(c => c.Truyen.Users)
                .FirstOrDefault(c => c.ChuongId == id);
            if (chuong == null)
            {
                return HttpNotFound();
            }
            var currentUser = Web_truyen.App_Start.SessionConfig.GetUser();
            var truyen = db.Truyen.Include(t => t.Users).FirstOrDefault(t => t.truyenId == chuong.truyenId);
            var permission = new UserPermissionViewModel();
            if (currentUser != null)
            {
                permission.VaiTro = currentUser.VaiTro;
                permission.IsAuthorOfTruyen = truyen != null && truyen.userId == currentUser.userId;
            }
            bool daTheoDoi = false;
            if (currentUser != null && chuong.Truyen?.Users != null)
            {
                daTheoDoi = db.TheoDoi_NguoiDung.Any(td =>
                    td.NguoiTheoDoiId == currentUser.userId &&
                    td.TheoDoiNguoiDungId == chuong.Truyen.Users.userId
                );
            }
            ViewBag.DaTheoDoiTacGia = daTheoDoi;
            ViewBag.Permission = permission;
            List<Chuong> danhSachChuong;
            if (permission.IsAdmin)
            {
                danhSachChuong = db.Chuong
                    .Where(c => c.truyenId == chuong.truyenId && c.TrangThaiDuyet != null)
                    .OrderBy(c => c.ChuongId)
                    .ToList();
            }
            else if (!permission.IsAdmin && !permission.IsAuthorOfTruyen)
            {
                danhSachChuong = db.Chuong
                    .Where(c => c.truyenId == chuong.truyenId && c.DaDang)
                    .OrderBy(c => c.ChuongId)
                    .ToList();
            }
            else
            {
                danhSachChuong = db.Chuong
                    .Where(c => c.truyenId == chuong.truyenId)
                    .OrderBy(c => c.ChuongId)
                    .ToList();
            }
            var danhSachBinhLuan = db.BinhLuan
                .Include(b => b.Users)
                .Where(b => b.ChuongId == chuong.ChuongId && b.BinhLuanChaId == null)
                .OrderByDescending(b => b.NgayTao)
                .ToList();

            var viewModel = new ChuongViewModel
            {
                Chuong = chuong,
                DanhSachChuong = danhSachChuong,
                DanhSachBinhLuan = danhSachBinhLuan
            };

            int index = danhSachChuong.FindIndex(c => c.ChuongId == id);
            ViewBag.IsDocChuong = true;
            ViewBag.ChuongTruocId = (index > 0) ? (int?)danhSachChuong[index - 1].ChuongId : null;
            ViewBag.ChuongSauId = (index < danhSachChuong.Count - 1) ? (int?)danhSachChuong[index + 1].ChuongId : null;
            ViewBag.TieuDeTruyen = chuong.Truyen?.TieuDe ?? "Chưa có tiêu đề";
            ViewBag.TruyenId = chuong.Truyen?.truyenId;
            ViewBag.TacGia = chuong.Truyen?.Users?.Username ?? "Không có tiêu đề";
            ViewBag.TrangThaiChuong = chuong.DaDang ? "Đã đăng" : "Bản thảo";
            ViewBag.ChuongDangChon = chuong.ChuongId;
            ViewBag.TacGiaId = chuong.Truyen?.Users?.userId;
            ViewBag.Avatar = chuong.Truyen?.Users?.avt;
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult DangTai(ChuongViewModel model)
        {
            var id = model.Chuong.ChuongId;
            var chuong = db.Chuong.FirstOrDefault(c => c.ChuongId == id);
            if (chuong == null)
            {
                return HttpNotFound();
            }
            chuong.DaDang = true;
            chuong.TrangThaiDuyet = "Đã duyệt";
            var cacChuongDaDang = db.Chuong
                .Where(c => c.truyenId == chuong.truyenId && c.DaDang && c.ChuongId != id)
                .ToList();
            if (cacChuongDaDang.Count == 0)
            {
                var truyen = db.Truyen.FirstOrDefault(t => t.truyenId == chuong.truyenId);
                if (truyen != null)
                {
                    truyen.DaDang = true;
                }
            }
            db.SaveChanges();
            return RedirectToAction("DocChuong", new { id = chuong.ChuongId });
        }
        [HttpPost]
        public ActionResult TuChoi(ChuongViewModel model)
        {
            var id = model.Chuong.ChuongId;
            var chuong = db.Chuong.FirstOrDefault(c => c.ChuongId == id);
            if (chuong == null)
            {
                return HttpNotFound();
            }
            chuong.DaDang = false;
            chuong.TrangThaiDuyet = "Từ chối";
            db.SaveChanges();

            return RedirectToAction("DocChuong", new { id = chuong.ChuongId });
        }



        [RoleUser]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Chuong chuong = db.Chuong.Find(id);
            if (chuong == null)
            {
                TempData["ErrorMessage"] = "Chương không tồn tại.";
                return RedirectToAction("Index", "Chuong"); 
            }

            db.Chuong.Remove(chuong);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
