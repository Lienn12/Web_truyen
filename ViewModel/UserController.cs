using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.Areas.Admin.Models;
using Web_truyen.Models;
using PagedList;
using System.Data.Entity;
using Web_truyen.App_Start;
namespace Web_truyen.Areas.Admin.Controllers
{
    
    public class UserController : Controller
    {
        // GET: Admin/User
        private Web_TruyenEntities db = new Web_TruyenEntities(); // DbContext

        // GET: Admin/User
        [RoleUser]
        public ActionResult DanhSachUser(string filter, string searchTerm, string roleFilter, int? page)
        {
            var danhSach = db.Users
                .Where(t => t.VaiTro == null || t.VaiTro.ToLower() != "admin") // Lọc bỏ admin
                .Select(t => new ListUserViewModel
                {
                    UserId = t.userId,
                    UserName = t.Username,
                    SoTacPham = db.Truyen.Count(tr => tr.userId == t.userId),
                    VaiTro = t.VaiTro
                }).AsQueryable();

            ViewBag.CurrentFilter = filter;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.roleFilter = roleFilter;

            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (roleFilter == "NULL")
                {
                    danhSach = danhSach.Where(t => string.IsNullOrEmpty(t.VaiTro));
                }
                else
                {
                    danhSach = danhSach.Where(t => t.VaiTro != null && t.VaiTro.ToLower() == roleFilter.ToLower());
                }
            }

            // Các bộ lọc khác
            if (filter == "bibaocao")
            {
                danhSach = danhSach.Where(t => db.BaoCao.Any(b =>
                    db.Users.Any(tr => tr.userId == t.UserId && tr.userId == b.DoiTuongId && b.LoaiBaoCao == "user")));
            }

            int pageNumber = page ?? 1;
            int pageSize = 10;

            var pagedList = danhSach.OrderByDescending(t => t.UserId).ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }

        public ActionResult HoSoCaNhan(int useId)
        {
            var user = db.Users.Find(useId);
            if (user == null)
            {
                return HttpNotFound();
            }
            int soTruyen = db.Truyen.Count(t => t.userId == useId);
            int soNguoiTheoDoi = db.TheoDoi_NguoiDung.Count(td => td.TheoDoiNguoiDungId == useId);
            int soTruyenDaDang = db.Truyen.Count(t => t.userId == useId && t.DaDang);
            int soTruyenBanThao = db.Truyen.Count(t => t.userId == useId && !t.DaDang);

            var danhSachTruyen = db.Truyen
                .Where(t => t.userId == useId)
                 .Select(t => new TruyenWithCountViewModel
                 {
                     Truyen = t,
                     SoChuong = db.Chuong.Count(c => c.truyenId == t.truyenId)
                 })
                .OrderByDescending(t => t.Truyen.NgayTao) // sắp xếp mới nhất trước
                .ToList();
            int currentUserId = SessionConfig.GetUser().userId;
            bool daTheoDoi = db.TheoDoi_NguoiDung.Any(td =>
                td.NguoiTheoDoiId == currentUserId && td.TheoDoiNguoiDungId == useId);
            ViewBag.DaTheoDoi = daTheoDoi;

            ViewBag.SoTruyen = soTruyen;
            ViewBag.SoNguoiTheoDoi = soNguoiTheoDoi;
            ViewBag.DanhSachTruyen = danhSachTruyen;
            ViewBag.SoTruyenDaDang = soTruyenDaDang;
            ViewBag.SoTruyenBanThao = soTruyenBanThao;
            return View(user);
        }

        public ActionResult ChinhSuaUser(int useId)
        {
            var user = db.Users.Find(useId);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaUser(Users model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
                return RedirectToAction("HoSoCaNhan", new { useId = model.userId });
            }
            return View(model);
        }
        [RoleUser]
        public ActionResult TruyenCuaToi(int? page)
        {
            var currentUser = SessionConfig.GetUser();

            // Kiểm tra nếu currentUser là null
            if (currentUser == null)
            {
                // Xử lý trường hợp người dùng không đăng nhập hoặc phiên làm việc đã hết hạn
                return RedirectToAction("Login", "Account"); // Ví dụ: chuyển hướng tới trang đăng nhập
            }

            var currentUserId = currentUser.userId;

            var danhSachTruyen = db.Truyen
                                    .Where(t => t.userId == currentUserId)
                                    .AsQueryable();

            // Chuyển đổi các đối tượng Truyen thành ListTruyenViewModel
            var danhSachTruyenViewModel = danhSachTruyen
                                            .Select(t => new Web_truyen.Areas.Admin.Models.ListTruyenViewModel
                                            {
                                                TruyenId = t.truyenId,
                                                AnhBia = t.AnhBia,
                                                TieuDe = t.TieuDe,
                                                Username = t.Users.Username,
                                                NgayTao = t.NgayTao,
                                                TrangThai = t.TrangThai
                                            })
                                            .ToList();

            // Phân trang
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var pagedList = danhSachTruyenViewModel.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleUser] // Hoặc kiểm tra phân quyền phù hợp
        public ActionResult KhoaTaiKhoan(int userId)
        {
            var user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.VaiTro != null && user.VaiTro.ToLower() == "admin")
            {
                TempData["ErrorMessage"] = "Không thể khóa tài khoản Admin.";
                return RedirectToAction("DanhSachUser");
            }

            user.TrangThai = false;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Tài khoản đã bị khóa.";
            return RedirectToAction("DanhSachUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleUser]
        public ActionResult MoKhoaTaiKhoan(int userId)
        {
            var user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.TrangThai = true;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Tài khoản đã được mở khóa.";
            return RedirectToAction("DanhSachUser");
        }

    }
}