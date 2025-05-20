using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;
using Web_truyen.Models;

namespace Web_truyen.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Web_TruyenEntities db = new Web_TruyenEntities();
        public ActionResult Index()
        {
            var newBooks = db.Truyen
                     .Where(t => t.DaDang == true)
                     .OrderByDescending(t => t.NgayTao)
                     .Take(10)
                     .ToList();
            ViewBag.NewBooks = newBooks;
            ViewBag.Title = "Home";
            var authors = db.Users.Where(nd => nd.VaiTro == "author").Take(5).ToList();
            ViewBag.Authors = authors;

            return View();
        }
    }
}