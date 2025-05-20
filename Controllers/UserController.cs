using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.Areas.Admin.Models;
using Web_truyen.Models;
using Web_truyen.ViewModel;
using PagedList;
using System.Data.Entity;
using Web_truyen.App_Start;
using System.IO;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
namespace Web_truyen.Areas.Admin.Controllers
{
    
    public class UserController : Controller
    {
        // GET: Admin/User
        private Web_TruyenEntities db = new Web_TruyenEntities(); // DbContext

        [RoleUser]
        public ActionResult TruyenCuaToi(int userId, string filter, int? page = 1)
        {
            var truyens = db.Truyen.Where(t => t.userId == userId);

            if (filter == "dadang")
            {
                truyens = truyens.Where(t => db.Chuong.Any(c => c.truyenId == t.truyenId && c.DaDang == true));
            }

            int? firstTruyenId = truyens.Select(t => t.truyenId).FirstOrDefault();
            ViewBag.DanhSachChuong = new List<Chuong>();

            if (firstTruyenId != null && firstTruyenId != 0)
            {
                ViewBag.DanhSachChuong = db.Chuong.Where(c => c.truyenId == firstTruyenId).ToList();
            }

            ViewBag.ChuongDangChon = 0;

            var model = truyens.Select(t => new TruyenCuaToiViewModel
            {
                TruyenId = t.truyenId,
                TieuDe = t.TieuDe,
                SoChuongDaDang = db.Chuong.Count(c => c.truyenId == t.truyenId && c.DaDang == true),
                SoBanThao = db.Chuong.Count(c => c.truyenId == t.truyenId && c.DaDang == false),
                AnhBia = t.AnhBia,
                NgayCapNhat = t.NgayCapNhap,
                DanhSachChuong = db.Chuong.Where(c => c.truyenId == t.truyenId).ToList()
            }).OrderByDescending(t => t.NgayCapNhat);
            var currentUser = SessionConfig.GetUser();
            var permission = new UserPermissionViewModel();
            if (currentUser != null)
            {
                permission.VaiTro = currentUser.VaiTro;
                permission.IsAuthorOfTruyen = currentUser.userId == userId;
            }

            ViewBag.Permission = permission;
            int pageSize = 10;
            int pageNumber = page ?? 1;

            return View(model.ToPagedList(pageNumber, pageSize));
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
                .OrderByDescending(t => t.Truyen.NgayTao)
                .ToList();

            var currentUser = SessionConfig.GetUser();
            bool daTheoDoi = false;
            bool isOwner = false;
            int currentUserId = -1;

            List<Users> danhSachNguoiDuocTheoDoi = new List<Users>();

            if (currentUser != null)
            {
                currentUserId = currentUser.userId;
                isOwner = currentUserId == useId;
                daTheoDoi = db.TheoDoi_NguoiDung.Any(td =>
                    td.NguoiTheoDoiId == currentUserId && td.TheoDoiNguoiDungId == useId);

                // Lấy danh sách người mà user này đang theo dõi (Users1 là người được theo dõi)
                danhSachNguoiDuocTheoDoi = db.TheoDoi_NguoiDung
                    .Where(td => td.NguoiTheoDoiId == useId)
                    .Select(td => td.Users1)
                    .ToList();
            }

            ViewBag.CurrentUserId = currentUserId;
            ViewBag.IsOwner = isOwner;
            ViewBag.DaTheoDoi = daTheoDoi;
            ViewBag.SoTruyen = soTruyen;
            ViewBag.SoNguoiTheoDoi = soNguoiTheoDoi;
            ViewBag.DanhSachTruyen = danhSachTruyen;
            ViewBag.SoTruyenDaDang = soTruyenDaDang;
            ViewBag.SoTruyenBanThao = soTruyenBanThao;
            ViewBag.dsFollow = danhSachNguoiDuocTheoDoi;

            return View(user);
        }

        [RoleUser]
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
        public ActionResult ChinhSuaUser(Users model, HttpPostedFileBase avatar)
        {
            model.avt = db.Users.FirstOrDefault(u => u.userId == model.userId)?.avt;
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Username))
                {
                    ModelState.AddModelError("Username", "Tên người dùng không được để trống.");
                }
                if (string.IsNullOrEmpty(model.MoTa))
                {
                    ModelState.AddModelError("MoTa", "Mô tả không được để trống.");
                }
                var existingUser = db.Users.FirstOrDefault(u => u.Username == model.Username && u.userId != model.userId);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã tồn tại.");
                }
                var user = db.Users.Find(model.userId); 
                if (user != null)
                {
                    user.Username = model.Username;
                    user.MoTa = model.MoTa;
                    if (avatar != null && avatar.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(avatar.FileName);
                        var path = Path.Combine(Server.MapPath("~/Assets/img/"), fileName);
                        avatar.SaveAs(path);
                        user.avt = fileName;
                    }
                    Debug.WriteLine("Avatar updated: " + user.avt);
                    db.Users.Attach(user);  
                    db.Entry(user).State = EntityState.Modified;  
                    db.SaveChanges();
                    var updatedUser = db.Users.Find(user.userId);
                    SessionConfig.SetUser(updatedUser);
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
                    return RedirectToAction("HoSoCaNhan", new { useId = model.userId });
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy người dùng.");
                }
            }
            return View(model);
        }

        [RoleUser]

        public ActionResult DanhSachUser(string filter, string searchTerm, string roleFilter, int? page = 1)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            ViewBag.CurrentFilter = filter;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.RoleFilter = roleFilter;

            var users = db.Users.Where(u => u.VaiTro.ToLower() != "admin").AsQueryable();

            if (filter == "bibaocao")
            {
                var biBaoCaoUserIds = db.BaoCao
                    .Where(b => b.LoaiBaoCao == "user")
                    .Select(b => b.DoiTuongId)
                    .Distinct()
                    .ToList();
                users = users.Where(u => biBaoCaoUserIds.Contains(u.userId));
            }

            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (roleFilter == "author")
                {
                    users = users.Where(u => u.VaiTro == "author");
                }
                else if (roleFilter == "user")
                {
                    users = users.Where(u => u.VaiTro == "user");
                }
                else if (roleFilter == "block")
                {
                    users = users.Where(u => u.TrangThai == false);
                }
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => u.Username.Contains(searchTerm));
            }

            var model = users
                .OrderByDescending(u => u.userId)
                .Select(u => new ListUserViewModel
                {
                    UserId = u.userId,
                    UserName = u.Username,
                    SoTacPham = db.Truyen.Count(t => t.userId == u.userId),
                    VaiTro = u.VaiTro,
                    TrangThai = u.TrangThai,
                    Avt = u.avt,
                    BiBaoCao = db.BaoCao.Count(b => b.DoiTuongId == u.userId && b.LoaiBaoCao == "user")
                });

            return View(model.ToPagedList(pageNumber, pageSize));
        }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [RoleUser] 
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
        [HttpGet]
        public ActionResult Settings(int? userId)
        {
            if (userId == null)
                return RedirectToAction("Index", "Home");

            var user = db.Users.Find(userId);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }
        [HttpPost]
        [RoleUser]
        public ActionResult Settings(int userId) {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(userId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra trùng username (nếu bạn cho phép đổi username)
                var exists = db.Users.Any(u => u.Username == Username && u.userId != userId);
                if (exists)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
                    return View(user);
                }

                // Cập nhật thông tin được phép thay đổi
                user.Username = model.Username;
                user.Email = model.Email;
                user.NgaySinh = model.NgaySinh;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
                return RedirectToAction("Settings", new { userId = user.userId });
            }

            // Nếu có lỗi validate, trả về view với model để hiển thị lỗi
            return View(userId);
        }
    }
}