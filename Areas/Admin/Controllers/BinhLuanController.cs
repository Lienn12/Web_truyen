using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Models;

namespace Web_truyen.Areas.Admin.Controllers
{
    [RoleUser]
    public class BinhLuanController : Controller
    {
        // GET: Admin/BinhLuan
        private Web_TruyenEntities db = new Web_TruyenEntities();
        // POST: Admin/BinhLuan/Them

        // POST: Lưu bình luận
        [HttpPost]
        public ActionResult Them(int truyenId, int chuongId, string noiDung)
        {
            if (string.IsNullOrWhiteSpace(noiDung))
            {
                TempData["Error"] = "Nội dung không được để trống.";
                return RedirectToAction("DocChuong", "Chuong", new { id = chuongId });
            }
            var user = SessionConfig.GetUser();
            var binhLuan = new BinhLuan
            {
                userId = user.userId,
                truyenId = truyenId,
                ChuongId = chuongId,
                NoiDung = noiDung,
                NgayTao = DateTime.Now,
                BinhLuanChaId = null,
            };

            db.BinhLuan.Add(binhLuan);
            db.SaveChanges();

            TempData["Success"] = "Đã bình luận thành công.";
            return RedirectToAction("DocChuong", "Chuong", new {area="Admin", id = chuongId });
        }
        [HttpPost]
        public ActionResult TraLoi(int commentId, string noiDung)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(noiDung))
                {
                    TempData["Error"] = "Nội dung trả lời không được để trống.";
                    return RedirectToAction("DocChuong", "Chuong", new { id = (int?)null }); 
                }

                var binhLuanCha = db.BinhLuan.Find(commentId);
                var user = SessionConfig.GetUser();

                if (binhLuanCha != null)
                {
                    var traLoi = new BinhLuan
                    {
                        userId = user.userId,
                        truyenId = binhLuanCha.truyenId,
                        ChuongId = binhLuanCha.ChuongId,
                        NoiDung = noiDung,
                        NgayTao = DateTime.Now,
                        BinhLuanChaId = binhLuanCha.BinhLuanId 
                    };

                    db.BinhLuan.Add(traLoi);
                    db.SaveChanges();

                    return RedirectToAction("DocChuong", "Chuong", new { id = binhLuanCha.ChuongId });
                }
                else
                {
                    TempData["Error"] = "Bình luận cha không tồn tại.";
                    return RedirectToAction("DocChuong", "Chuong", new { id = (int?)null }); 
                }
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra, vui lòng thử lại.";
                return RedirectToAction("DocChuong", "Chuong", new { id = (int?)null }); 
            }
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            var binhLuan = db.BinhLuan.Find(id);
            if (binhLuan == null)
            {
                TempData["CommentError"] = "Bình luận không tồn tại.";
                return RedirectToAction("DocChuong", "Chuong", new { id = 1 }); 
            }
            if ((int)Session["UserId"] != binhLuan.userId && Session["IsAdmin"] == null)
            {
                TempData["CommentError"] = "Bạn không có quyền xóa bình luận này.";
                return RedirectToAction("DocChuong", "Chuong", new { id = binhLuan.ChuongId });
            }

            int chuongId = binhLuan.ChuongId; 

            db.BinhLuan.Remove(binhLuan);
            db.SaveChanges();

            TempData["CommentSuccess"] = "Xóa bình luận thành công.";
            return RedirectToAction("DocChuong", "Chuong", new { id = chuongId });
        }

    }
}