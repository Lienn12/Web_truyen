using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Web_truyen.App_Start;
using Web_truyen.Models;
using Web_truyen.ViewModel;

namespace Web_truyen.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        Web_TruyenEntities2 db = new Web_TruyenEntities2();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(loginViewModel model)
        {
            //Kiểm tra đăng nhập
            if (ModelState.IsValid)
            {
                model.Password = HashPassword(model.Password);
                //Thêm mã hóa bảo mật để kiểm tra 
                Users check = db.Users.FirstOrDefault(m => m.Username == model.Username && m.Password == model.Password);
                // Kiểm tra đăng nhập
                if (check != null)
                {
                    SessionConfig.SetUser(check);
                    var us = SessionConfig.GetUser();
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản và mật khẩu không đúng ");
                }
            }

            return View(model);
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "Password không được để trống.");
                    return View(model);
                }

                Users user = new Users
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = HashPassword(model.Password)
                };

                if (db.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã được sử dụng.");
                    return View(model);
                }

                if (db.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            if (!string.IsNullOrEmpty(validationError.PropertyName) && !string.IsNullOrEmpty(validationError.ErrorMessage))
                            {
                                ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Một lỗi không xác định đã xảy ra.");
                            }
                        }
                    }
                    return View(model);
                }
            }

            return View(model);
        }


        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))  
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public ActionResult VerifyEmail ()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyEmail(VerifyRmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkUser  = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (checkUser == null)
                {
                    ModelState.AddModelError("Email", "Email không tồn tại.");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = checkUser.Username });
                }
            }
            return View(model);
        }
        public ActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Username = username });
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (checkUser != null)
                {
                    checkUser.Password = HashPassword(model.NewPassword);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy tài khoản với email này.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi. Vui lòng thử lại.");
                return View(model);
            }
        }
        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            return RedirectToAction("Index", "Home",new {area=""});
        }

    }
}