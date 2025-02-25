using DoAnWebBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanGiay.Controllers
{
    public class UserProfilesController : Controller
    {
        MyDBContext db = new MyDBContext();
        // GET: UserProfiles
        public ActionResult Index()
        {
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập.";
                return RedirectToAction("Login", "Users");
            }

            // Tra cứu UserId bằng UserName từ cơ sở dữ liệu
            string userName = authCookie.Value;
            var user = db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Users");
            }

            var profile = db.UserProfiles.FirstOrDefault(p => p.UserId == user.UserId);
            if (profile == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin cá nhân.";
                return RedirectToAction("Edit", new { userId = user.UserId });
            }

            return View(profile);
        }



        public ActionResult Edit(int userId)
        {
            UserProfile profile = db.UserProfiles.FirstOrDefault(p => p.UserId == userId);

            // Nếu chưa có thông tin, tạo một đối tượng mới
            if (profile == null)
            {
                profile = new UserProfile { UserId = userId };
            }

            return View(profile);
        }

        // Xử lý lưu thông tin cá nhân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile model)
        {
            if (ModelState.IsValid)
            {
                var profile = db.UserProfiles.FirstOrDefault(p => p.UserId == model.UserId);

                if (profile == null)
                {
                    // Thêm thông tin mới
                    db.UserProfiles.Add(model);
                }
                else
                {
                    // Cập nhật thông tin hiện có
                    profile.FullName = model.FullName;
                    profile.PhoneNumber = model.PhoneNumber;
                    profile.Address = model.Address;
                    profile.BirthDate = model.BirthDate;
                    db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}