using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_truyen.App_Start;

namespace Web_truyen.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.Tile = "Home Admin";
            //var user=SessionConfig.GetUser();
            ////Kiểm tra trạng thái đăng nhâp (role) của người dùng với chức năng authen 
            //if (user == null )
            //{
            //    return RedirectToAction("Login", "Account",new {area=""});
            //}
            return View();
        }
    }
}