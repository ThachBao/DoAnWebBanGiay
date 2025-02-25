using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebBanGiay.Models;

namespace DoAnWebBanGiay.Areas.Admin.Controllers
{
    public class ManagementUserController : Controller
    {
        // GET: Admin/ManagementUser
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            List<User> users = db.Users.Where(u => u.Role == "user").ToList();
            return View(users);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid) // Kiểm tra dữ liệu hợp lệ
            {
                MyDBContext db = new MyDBContext();
                user.Role = "user";                     // Gán role mặc định là "user" nếu không có role được truyền
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index"); // Quay lại danh sách người dùng
            }
            return View(user); // Trả lại view nếu dữ liệu không hợp lệ
        }
        public ActionResult Delete(int id)
        {
            MyDBContext db = new MyDBContext();
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user); // Xóa user
                db.SaveChanges();   
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            using (MyDBContext db = new MyDBContext())
            {
                var User = db.Users.Find(id);
                if (User == null)
                {
                    return HttpNotFound();
                }
                return View(User);
            }
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                using (MyDBContext db = new MyDBContext())
                {
                    var existingUser = db.Users.Find(user.UserId);
                    if (existingUser != null)
                    {
                        // Cập nhật các thông tin khác
                        existingUser.UserName = user.UserName;
                        existingUser.Password = user.Password;
                        existingUser.Email = user.Email;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(user);
        }


    }
}