using DoAnWebBanGiay.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanGiay.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var authCookie = Request.Cookies["auth"];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                // Nếu đã đăng nhập, gửi thông tin tên người dùng vào ViewBag
                ViewBag.USERNAME = authCookie.Value;
                return View();
            }

            // Nếu chưa đăng nhập, trả về giao diện đăng nhập
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user != null)
            {
                using (MyDBContext db = new MyDBContext())
                {
                    User myUser = db.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
                    if (myUser != null)
                    {
                        // So sánh mật khẩu nhập vào với mật khẩu trong cơ sở dữ liệu
                        if (user.Password == myUser.Password)
                        {
                            // Đăng nhập thành công, tạo cookie
                            HttpCookie authCookie = new HttpCookie("auth", myUser.UserName);
                            HttpCookie authCookie2 = new HttpCookie("auth", myUser.UserId.ToString());
                            HttpCookie roleCookie = new HttpCookie("role", myUser.Role);
                            Response.Cookies.Add(authCookie);
                            Response.Cookies.Add(roleCookie);
                            // Lưu thông báo vào TempData
                            TempData["SuccessMessage"] = "Đăng nhập thành công!";
                            // Chuyển hướng theo vai trò
                            if (myUser.Role == "admin")
                            {
                                return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                            }
                            else if (myUser.Role == "user")
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }

            // Nếu không thành công, hiển thị lỗi
            ModelState.AddModelError("", "Đăng nhập không thành công !");
            return View(user);
        }



        public ActionResult Logout()
        {
            // Xóa cookie khi đăng xuất
            HttpCookie authCookie = new HttpCookie("auth");
            authCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(authCookie);

            HttpCookie roleCookie = new HttpCookie("role");
            roleCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(roleCookie);

            return Redirect("/");
        }

        // Mã hóa mật khẩu bằng SHA256
        //private string HashPassword(string password)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        StringBuilder builder = new StringBuilder();
        //        foreach (byte b in bytes)
        //        {
        //            builder.Append(b.ToString("x2")); // Chuyển đổi byte thành chuỗi hex
        //        }
        //        return builder.ToString();
        //    }
        //}
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user, string retypePassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (user.Password != retypePassword)
            {
                ModelState.AddModelError("retypePassword", "Passwords do not match.");
                return View();
            }

            // Kiểm tra đuôi email
            if (!System.Text.RegularExpressions.Regex.IsMatch(user.Email, @"^[a-zA-Z0-9._%+-]+@(gmail\.com|email\.com)$"))
            {
                ModelState.AddModelError("Email", "Email must end with '@gmail.com' or '@email.com'.");
                return View();
            }

            using (MyDBContext db = new MyDBContext())
            {
                // Kiểm tra Username đã tồn tại
                User existingUser = db.Users.FirstOrDefault(u => u.UserName == user.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                    return View();
                }

                // Kiểm tra Email đã tồn tại
                existingUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View();
                }

                // Thêm người dùng mới
                user.Role = "user"; // Gán vai trò mặc định
                db.Users.Add(user);
                db.SaveChanges();
            }

            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login");
        }

    }
}
