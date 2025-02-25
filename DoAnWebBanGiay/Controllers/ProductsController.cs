using DoAnWebBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCrypt.Net;

namespace DoAnWebBanGiay.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string category, int page = 1, string search = "", string sortOrder = "")
        {
            MyDBContext db = new MyDBContext();
            List<Product> pro = db.Products.ToList(); // Lấy tất cả sản phẩm

            // Lọc theo danh mục
            switch (category)
            {
                case "nam":
                    pro = db.Products.Where(p => p.Category.CategoryName == "Nam").ToList();
                    ViewBag.category = "Sản Phẩm Nam";
                    break;
                case "nu":
                    pro = db.Products.Where(p => p.Category.CategoryName == "Nu").ToList();
                    ViewBag.category = "Sản Phẩm Nữ";
                    break;
                case "unisex":
                    pro = db.Products.Where(p => p.Category.CategoryName == "Unisex").ToList();
                    ViewBag.category = "Sản Phẩm Unisex";
                    break;
                case "all":
                default:
                    ViewBag.category = "Tất cả";
                    break;
            }

            // Tìm kiếm
            ViewBag.search = search;
            if (!string.IsNullOrEmpty(search))
            {
                pro = pro.Where(row => row.ProName.ToLower().Contains(search.ToLower())).ToList();
            }

            // Kiểm tra nếu không có sản phẩm nào
            if (pro.Count == 0)
            {
                ViewBag.message = "Không tìm thấy sản phẩm nào.";
            }

            // Sắp xếp theo giá
            if (sortOrder == "asc")
            {
                pro = pro.OrderBy(p => p.Gia).ToList(); // Sắp xếp từ thấp đến cao
            }
            else if (sortOrder == "desc")
            {
                pro = pro.OrderByDescending(p => p.Gia).ToList(); // Sắp xếp từ cao đến thấp
            }
            if (!pro.Any())
            {
                ViewBag.Message = "Không tìm thấy sản phẩm nào khớp với từ khóa.";
            }

            // Phân trang
            int RowOfPage = 8; // Số sản phẩm trên mỗi trang
            ViewBag.page = page;
            ViewBag.totalPages = (int)Math.Ceiling(pro.Count() / (double)RowOfPage);
            pro = pro.Skip((page - 1) * RowOfPage).Take(RowOfPage).ToList();

            return View(pro);
        }
        public ActionResult Detail(int id = 0)
        {
            MyDBContext db = new MyDBContext();

            // Lấy thông tin sản phẩm từ bảng Products
            Product product = db.Products.FirstOrDefault(p => p.ProId == id);

            // Kiểm tra nếu sản phẩm không tồn tại
            if (product == null)
            {
                return HttpNotFound("Sản phẩm không tồn tại.");
            }

            // Lấy danh sách kích thước từ bảng ProductSizes dựa trên ProId
            List<ProductSize> sizes = db.productSizes.Where(ps => ps.ProId == id).ToList();
            ViewBag.sizes = sizes; // Lưu danh sách ProductSize vào ViewBag

            // Trả thông tin sản phẩm cho View
            return View(product);
        }
    }
}