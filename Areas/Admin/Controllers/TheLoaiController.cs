using System.Linq;
using System.Web.Mvc;
using Web_truyen.Models;
using PagedList;
using Web_truyen.App_Start;
using System;

namespace Web_truyen.Areas.Admin.Controllers
{
    
    public class TheLoaiController : Controller
    {
        Web_TruyenEntities db = new Web_TruyenEntities();
       [RoleUser]
        public ActionResult DanhSach(int? page, int? editId = null)
        {
            var list = db.TheLoai.Where(t => t.TheLoaiId != 2).ToList();
            int pageNumber = page ?? 1;
            int pageSize = 10;

            ViewBag.EditId = editId;

            if (editId != null)
            {
                var theLoai = db.TheLoai.Find(editId);
                ViewBag.EditTenTheLoai = theLoai?.TenTheLoai;
            }

            return View(list.ToPagedList(pageNumber, pageSize));
        } 
        public ActionResult Index()
        {
             var theLoaiList = db.TheLoai.ToList(); 
              ViewData["TheLoaiList"] = theLoaiList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DanhSach(string TenTheLoai)
        {
            if (!string.IsNullOrWhiteSpace(TenTheLoai))
            {
                var theLoai = new TheLoai { TenTheLoai = TenTheLoai };
                db.TheLoai.Add(theLoai);
                db.SaveChanges();
                return RedirectToAction("DanhSach");
            }
            TempData["Error"] = "Tên thể loại không được để trống.";
            return RedirectToAction("DanhSach");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Capnhat(int id, string TenTheLoai)
        {
            if (string.IsNullOrWhiteSpace(TenTheLoai))
            {
                TempData["Error"] = "Tên thể loại không được để trống.";
                return RedirectToAction("DanhSach", new { editId = id });
            }

            var theLoai = db.TheLoai.Find(id);
            if (theLoai != null)
            {
                theLoai.TenTheLoai = TenTheLoai;
                db.SaveChanges();
            }

            return RedirectToAction("DanhSach");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var truyen = db.Truyen.Where(t => t.TheLoaiId == id).ToList();
                foreach (var item in truyen)
                {
                    item.TheLoaiId = 2; 
                }
                db.SaveChanges();
                var theLoai = db.TheLoai.Find(id);
                if (theLoai != null)
                {
                    db.TheLoai.Remove(theLoai);
                    db.SaveChanges();
                }
                return RedirectToAction("DanhSach"); 
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa thể loại: " + ex.Message;
                return RedirectToAction("DanhSach");
            }
        }

    }
}
