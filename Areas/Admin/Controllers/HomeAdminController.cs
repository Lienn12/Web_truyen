using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;

namespace Web_truyen.Areas.Admin.Controllers
{
    [RoleUser]
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.Tile = "Home Admin";
            return View();
        }
    }
}