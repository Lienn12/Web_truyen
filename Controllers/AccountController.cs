using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Configuration;

namespace Web_truyen.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        Web_TruyenEntities db = new Web_TruyenEntities();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(loginViewModel model)
        {
            // Kiểm tra đăng nhập
            if (ModelState.IsValid)
            {
                model.Password = HashPassword(model.Password);
                // Kiểm tra thông tin đăng nhập
                Users check = db.Users.FirstOrDefault(m => m.Username == model.Username && m.Password == model.Password);

                if (check != null)
                {
                    if (string.IsNullOrEmpty(check.VaiTro))
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa");
                        return View(model);
                    }

                    SessionConfig.SetUser(check);
                    var us = SessionConfig.GetUser();

                    if (TempData["ReturnUrl"] != null)
                    {
                        string returnUrl = TempData["ReturnUrl"].ToString();
                        System.Diagnostics.Debug.WriteLine("ReturnUrl: " + returnUrl);
                        return Redirect(returnUrl);
                    }

                    if (check.VaiTro == "admin")
                    {
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                    }
                    else if (check.VaiTro == "user" || check.VaiTro == "author")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Quyền truy cập không hợp lệ");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản và mật khẩu không đúng");
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

                if (db.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã được sử dụng.");
                    return View(model);
                }

                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                Users user = new Users
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = HashPassword(model.Password),
                    VaiTro = "user",
                    GioiTinh = "Khác", 
                    NgaySinh = DateTime.Now, 
                    avt = "usermacdinh.jpg", 
                    TrangThai = true
                };

                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session["SuccessMessage"] = "Đăng ký thành công!";
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

            Session["ErrorMessage"] = "Đăng ký không thành công!";
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

        public ActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyEmail(VerifyRmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    string code = Guid.NewGuid().ToString().Substring(0, 6); // Ví dụ 6 ký tự
                    Session["ResetCode"] = code;
                    Session["ResetEmail"] = user.Email;
                    Session["Username"] = user.Username; // Lưu Username vào Session

                    SendResetEmail(user.Email, code);

                    return RedirectToAction("EnterResetCode");
                }

                ModelState.AddModelError("", "Email không tồn tại.");
            }
            return View(model);
        }

        public ActionResult EnterResetCode()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnterResetCode(string code)
        {
            if (code == (string)Session["ResetCode"])
            {
                return RedirectToAction("ChangePassword");
            }

            // Nếu mã không đúng, thông báo lỗi và hiển thị lại form
            ViewBag.ErrorMessage = "Mã không đúng";
            return View();
        }

        public ActionResult ChangePassword()
        {
            // Lấy Username từ Session
            var username = Session["Username"] as string;
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
                // Lấy Username từ Session
                var username = Session["Username"] as string;

                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("VerifyEmail", "Account");
                }

                var checkUser = db.Users.FirstOrDefault(u => u.Username == username);
                if (checkUser != null)
                {
                    checkUser.Password = HashPassword(model.NewPassword);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy tài khoản với username này.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi. Vui lòng thử lại.");
                return View(model);
            }
        }

        private void SendResetEmail(string email, string code)
        {
            var fromAddress = new MailAddress("liennguyen4221@gmail.com", "JellyCo");
            var toAddress = new MailAddress(email);
            const string fromPassword = "tauf iyzk qulq rbtw";  // Đây là mật khẩu của bạn
            const string subject = "Reset your password";
            string body = $"Mã xác nhận của bạn là: {code}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,  // Cổng SMTP của Gmail
                EnableSsl = true,  // Bật SSL
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}