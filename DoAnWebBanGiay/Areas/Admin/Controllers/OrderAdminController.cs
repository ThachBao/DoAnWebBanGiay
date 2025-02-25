using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWebBanGiay.Models;

namespace DoAnWebBanGiay.Areas.Admin.Controllers
{
    public class OrderAdminController : Controller
    {
        MyDBContext db = new MyDBContext();
        // GET: Admin/OrderAdmin
        public ActionResult Index()
        {
            var orders = db.Orders.ToList();
            return View(orders);
        }

        // Chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            // Truy vấn Order, kèm theo thông tin User
            var order = db.Orders
                          .Include("User") // Eager Loading bảng User
                          .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            // Truy vấn chi tiết đơn hàng (OrderDetails)
            var orderDetails = db.OrderDetails
                                 .Where(od => od.OrderId == id)
                                 .Include("Product") // Eager Loading bảng Product
                                 .Include("ProductSize") // Eager Loading bảng ProductSize
                                 .ToList();

            ViewBag.orderDetails = orderDetails;
            return View(order);
        }



        // Xóa đơn hàng và các chi tiết liên quan
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);

            if (order != null)
            {
                // Xóa các chi tiết đơn hàng trước
                var orderDetails = db.OrderDetails.Where(od => od.OrderId == id).ToList();
                db.OrderDetails.RemoveRange(orderDetails);

                // Xóa đơn hàng
                db.Orders.Remove(order);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}