using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Models;
using Web_truyen.ViewModel;

namespace Web_truyen.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Web_TruyenEntities db = new Web_TruyenEntities();
        public ActionResult Index()
        {
            var user = SessionConfig.GetUser();
            if (user != null)
            { 
                var reading = db.ThuVien
                                .Where(ls => ls.UserID == user.userId)
                                .OrderByDescending(ls => ls.ThoiGianDocGanNhat)
                                .Select(ls => ls.Chuong.Truyen)
                                .Distinct()
                                .Take(5)
                                .ToList();
                ViewBag.ReadingBooks = reading;
            }
            

            // Truyện mới đăng
            var newBooks = db.Truyen
                     .Where(t => t.DaDang == true)
                     .OrderByDescending(t => t.NgayTao)
                     .Take(10)
                     .ToList();
            ViewBag.NewBooks = newBooks;

            // Truyện đề xuất (nhiều lượt xem nhất)
            var recommended = db.Truyen
                                .Where(t => t.DaDang == true)
                                .OrderByDescending(t => t.LuotDoc)
                                .Take(8)
                                .ToList();
            ViewBag.Recommended = recommended;

            // Truyện đã hoàn thành
            var completed = db.Truyen
                              .Where(t => t.TrangThai == true)
                              .OrderByDescending(t => t.NgayTao)
                              .Take(8)
                              .ToList();
            ViewBag.Completed = completed;

            var liked = db.Truyen
                          .Where(t => t.DaDang == true)
                          .ToList()                      
                          .OrderBy(t => Guid.NewGuid()) 
                          .Take(8)
                          .ToList();
            ViewBag.Liked = liked;

            DateTime weekAgo = DateTime.Now.AddDays(-7);
            var hotWeek = db.Truyen
                            .Where(t => t.DaDang == true && t.NgayTao >= weekAgo)
                            .OrderByDescending(t => t.LuotDoc)
                            .Take(5)
                            .ToList();
            ViewBag.HotWeek = hotWeek;

            DateTime monthAgo = DateTime.Now.AddMonths(-1);
            var hotMonth = db.Truyen
                             .Where(t => t.DaDang == true && t.NgayTao >= monthAgo)
                             .OrderByDescending(t => t.LuotDoc)
                             .Take(5)
                             .ToList();
            ViewBag.HotMonth = hotMonth;

            var authors = db.Users
                 .Where(u => u.VaiTro == "author")
                 .Select(u => new AuthorViewModel
                 {
                     User = u,
                     TruyenCount = u.Truyen.Count(),
                     FollowerCount = db.TheoDoi_NguoiDung.Count(td => td.NguoiTheoDoiId == u.userId)
                 })
                 .OrderByDescending(a => a.TruyenCount)
                 .ThenByDescending(a => a.FollowerCount)
                 .Take(5)
                 .ToList();
            ViewBag.Authors = authors;
            ViewBag.Title = "Trang chủ";

            return View();
        }
        public ActionResult AllBooks(string filter = "tatca", int? theLoaiId = null)
        {
            IQueryable<Truyen> query = db.Truyen.Where(t => t.DaDang == true);

            ViewBag.Title = "Tất cả truyện";

            switch (filter.ToLower())
            {
                case "hot":
                    query = query.OrderByDescending(t => t.LuotDoc);
                    ViewBag.Title = "Truyện Hot";
                    break;
                case "hoanthanh":
                    query = query.Where(t => t.TrangThai == true).OrderByDescending(t => t.NgayTao);
                    ViewBag.Title = "Truyện Hoàn Thành";
                    break;
                default:
                    query = query.OrderByDescending(t => t.NgayTao);
                    break;
            }

            if (theLoaiId.HasValue)
            {
                query = query.Where(t => t.TheLoaiId == theLoaiId.Value);
                var theLoai = db.TheLoai.Find(theLoaiId.Value);
                if (theLoai != null)
                {
                    ViewBag.Title = $"Thể loại: {theLoai.TenTheLoai}";
                }
            }

            var allBooks = query.ToList();

            ViewBag.Filter = filter;
            ViewBag.TheLoaiId = theLoaiId;
            ViewBag.AllTheLoai = db.TheLoai.ToList();

            return View(allBooks);
        }


    }
}