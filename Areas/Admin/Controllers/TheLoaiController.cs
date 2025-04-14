using System.Linq;
using System.Web.Mvc;
using Web_truyen.Models;
using PagedList;

namespace Web_truyen.Areas.Admin.Controllers
{
    public class TheLoaiController : Controller
    {
        Web_TruyenEntities2 db = new Web_TruyenEntities2();

        public ActionResult DanhSach(int? page, int? editId = null)
        {
            var list = db.TheLoai.ToList();
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
            }

            return RedirectToAction("DanhSach");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhat(int id, string TenTheLoai)
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
            var theLoai = db.TheLoai.Find(id);
            if (theLoai != null)
            {
                db.TheLoai.Remove(theLoai);
                db.SaveChanges();
            }

            return RedirectToAction("DanhSach");
        }
    }
}
