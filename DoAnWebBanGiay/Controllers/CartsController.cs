using DoAnWebBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCrypt.Net;

namespace DoAnWebBanGiay.Controllers
{
    public class CartsController : Controller
    {
        // GET: Carts4
        public ActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null)
            {
                TempData["LoginMessage"] = "Bạn phải đăng nhập mới vào được giỏ hàng.";
                return RedirectToAction("Login", "Users");
            }

            string userName = authCookie.Value;

            using (MyDBContext db = new MyDBContext())
            {
                // Lấy thông tin người dùng hiện tại
                User currentUser = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Users");
                }

                // Lấy các sản phẩm trong giỏ hàng của người dùng hiện tại
                List<Cart> carts = db.Carts
                                     .Where(cart => cart.UserId == currentUser.UserId)
                                     .Include(cart => cart.Product) // Bao gồm thông tin sản phẩm
                                     .Include(cart => cart.ProductSizes) // Bao gồm thông tin kích thước
                                     .ToList();

                return View(carts);
            }
        }


        public ActionResult Add(int id = 0, int productSizeId = 0, int quantity = 1)
        {
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null)
            {
                TempData["LoginMessage"] = "Bạn cần phải đăng nhập để thêm vào giỏ hàng.";
                return RedirectToAction("Login", "Users");
            }

            string userName = authCookie.Value;

            using (MyDBContext db = new MyDBContext())
            {
                User currentUser = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Users");
                }

                if (id > 0 && productSizeId > 0)
                {
                    Cart cartItem = db.Carts
                                      .FirstOrDefault(cart => cart.ProId == id && cart.ProductSizeId == productSizeId && cart.UserId == currentUser.UserId);

                    if (cartItem != null)
                    {
                        // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng
                        cartItem.SoLuong += quantity;
                    }
                    else
                    {
                        // Nếu sản phẩm chưa có, thêm mới
                        Cart cart = new Cart
                        {
                            ProId = id,
                            ProductSizeId = productSizeId,
                            SoLuong = quantity,
                            UserId = currentUser.UserId
                        };
                        db.Carts.Add(cart);
                    }

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Bạn cần chọn kích thước trước khi thêm vào giỏ hàng.";
                }
            }

            return RedirectToAction("Detail", "Products", new { id });
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int quan = 0, int proid = 0, int productSizeId = 0)
        {
            // Kiểm tra đăng nhập
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null)
            {
                TempData["LoginMessage"] = "Bạn cần phải đăng nhập.";
                return RedirectToAction("Login", "Users");
            }

            string userName = authCookie.Value;

            using (MyDBContext db = new MyDBContext())
            {
                // Tìm người dùng hiện tại
                User currentUser = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Users");
                }

                // Kiểm tra giá trị nhập vào
                if (quan <= 0)
                {
                    TempData["ErrorMessage"] = "Số lượng phải lớn hơn 0.";
                    return RedirectToAction("Index");
                }

                // Tìm sản phẩm trong giỏ hàng của người dùng hiện tại
                Cart cartItem = db.Carts
                                  .Where(cart => cart.ProId == proid &&
                                                 cart.ProductSizeId == productSizeId &&
                                                 cart.UserId == currentUser.UserId)
                                  .FirstOrDefault();

                if (cartItem != null)
                {
                    // Cập nhật số lượng
                    cartItem.SoLuong = quan;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật số lượng thành công!";
                }
                else
                {
                    // Trường hợp sản phẩm không tồn tại trong giỏ hàng
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại trong giỏ hàng.";
                }
            }

            return RedirectToAction("Index");
        }





        public ActionResult Delete(int id = 0, int productSizeId = 0)
        {
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null)
            {
                TempData["LoginMessage"] = "Bạn cần phải đăng nhập.";
                return RedirectToAction("Login", "Users");
            }

            string userName = authCookie.Value;

            using (MyDBContext db = new MyDBContext())
            {
                User currentUser = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Users");
                }

                if (id > 0 && productSizeId > 0)
                {
                    Cart cartItem = db.Carts
                                      .Where(cart => cart.ProId == id && cart.ProductSizeId == productSizeId && cart.UserId == currentUser.UserId)
                                      .FirstOrDefault();

                    if (cartItem != null)
                    {
                        db.Carts.Remove(cartItem);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            using (MyDBContext db = new MyDBContext())
            {
                // Lấy tất cả các sản phẩm trong giỏ hàng
                var cartItems = db.Carts.ToList();

                if (cartItems.Any())
                {
                    // Xóa tất cả sản phẩm trong giỏ hàng
                    db.Carts.RemoveRange(cartItems);
                    db.SaveChanges();
                }

                TempData["SuccessMessage"] = "Đã xóa tất cả sản phẩm trong giỏ hàng.";
            }

            return RedirectToAction("Index");
        }
        public ActionResult Checkout()
        {
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null)
            {
                TempData["LoginMessage"] = "Bạn cần phải đăng nhập để thanh toán.";
                return RedirectToAction("Login", "Users");
            }

            string userName = authCookie.Value;

            using (MyDBContext db = new MyDBContext())
            {
                // Lấy thông tin người dùng
                User currentUser = db.Users.FirstOrDefault(u => u.UserName == userName);


                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Users");
                }

                // Lấy thông tin từ UserProfile
                UserProF userProfile = db.UserProFs.FirstOrDefault(up => up.UserId == currentUser.UserId);
                ViewBag.UserProfile = userProfile;
                if (userProfile == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng trong UserProfile.";
                    return RedirectToAction("Index");
                }

                // Lấy giỏ hàng
                List<Cart> carts = db.Carts
                                     .Where(cart => cart.UserId == currentUser.UserId)
                                     .Include(cart => cart.Product)
                                     .Include(cart => cart.ProductSizes)
                                     .ToList();

                if (carts.Count == 0)
                {
                    TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                    return RedirectToAction("Index");
                }

                // Pass giỏ hàng và thông tin user profile sang View
                ViewBag.UserProfile = userProfile;
                return View(carts);
            }
        }

        // Xử lý thanh toán
        [HttpPost]
        public ActionResult ProcessCheckout(string shippingAddress, string paymentMethod)
        {
            var authCookie = Request.Cookies["auth"];
            if (authCookie == null)
            {
                TempData["LoginMessage"] = "Bạn cần phải đăng nhập để thanh toán.";
                return RedirectToAction("Login", "Users");
            }

            string userName = authCookie.Value;

            using (MyDBContext db = new MyDBContext())
            {
                // Lấy thông tin người dùng
                User currentUser = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Users");
                }

                // Lấy giỏ hàng
                List<Cart> carts = db.Carts
                                     .Where(cart => cart.UserId == currentUser.UserId)
                                     .Include(cart => cart.Product)
                                     .ToList();

                if (carts.Count == 0)
                {
                    TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                    return RedirectToAction("Index");
                }

                // Tạo đơn hàng mới
                Order order = new Order
                {
                    UserId = currentUser.UserId,
                    OrderDate = DateTime.Now,
                    ShippingAddress = shippingAddress,
                    PaymentMethod = paymentMethod,
                    TotalAmount = carts.Sum(c => c.SoLuong * (int)c.Product.Gia)
                };

                db.Orders.Add(order);
                db.SaveChanges();

                // Lưu chi tiết đơn hàng
                foreach (var cart in carts)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProId = cart.ProId,
                        ProductSizeId = cart.ProductSizeId,
                        Quantity = cart.SoLuong,
                        Price = (int)cart.Product.Gia
                    };

                    db.OrderDetails.Add(orderDetail);
                }

                // Xóa giỏ hàng sau khi thanh toán
                db.Carts.RemoveRange(carts);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thanh toán thành công! Đơn hàng của bạn đã được tạo.";
                return RedirectToAction("Index", "Carts"); // Chuyển hướng về trang sản phẩm
            }
        }
    }
}