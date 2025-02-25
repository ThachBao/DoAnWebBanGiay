using DoAnWebBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanGiay.Controllers
{
    public class UserPFsController : Controller
    {
        // GET: UserPFs
        MyDBContext db = new MyDBContext();

        // GET: UserProFs/Index
        public ActionResult Index()
        {
            // Lấy thông tin từ Cookie (giả định "auth" chứa UserName)
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập.";
                return RedirectToAction("Login", "Users");
            }

            string userName = authCookie.Value;

            // Tìm người dùng theo UserName
            var user = db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Login", "Users");
            }

            // Tìm thông tin cá nhân theo UserId
            var profile = db.UserProFs.FirstOrDefault(p => p.UserId == user.UserId);

            // Nếu không có thông tin, chuyển sang trang chỉnh sửa
            if (profile == null)
            {
                return RedirectToAction("Edit", new { userId = user.UserId });
            }

            // Trả về thông tin cá nhân
            return View(profile);
        }

        // GET: UserProFs/Edit
        public ActionResult Edit(int userId)
        {
            // Lấy thông tin cá nhân hoặc tạo mới nếu không tồn tại
            var profile = db.UserProFs.FirstOrDefault(p => p.UserId == userId) ?? new UserProF { UserId = userId };
            return View(profile);
        }

        // POST: UserProFs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProF model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra thông tin cá nhân có tồn tại không
                    var profile = db.UserProFs.FirstOrDefault(p => p.UserId == model.UserId);

                    if (profile == null)
                    {
                        // Tạo mới nếu chưa có
                        db.UserProFs.Add(model);
                    }
                    else
                    {
                        // Cập nhật thông tin nếu đã có
                        profile.FullName = model.FullName;
                        profile.PhoneNumber = model.PhoneNumber;
                        profile.Address = model.Address;
                        profile.BirthDate = model.BirthDate;
                    }

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
                    return RedirectToAction("Index","UserPFs");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            // Trả về view nếu có lỗi
            return View(model);
        }

    }
}