using DoAnWebBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanGiay.Areas.Admin.Controllers
{
    public class ProductsAdminController : Controller
    {
        // GET: Admin/Products
        public ActionResult Index(string category, int page = 1, string search = "", string sortOrder = "")
        {
            MyDBContext db = new MyDBContext();
            List<Product> pro = db.Products.ToList(); // Lấy tất cả sản phẩm
            // Lọc theo danh mục
            switch (category)
            {
                case "nam":
                    pro = db.Products.Where(p => p.Category.CategoryName == "Giày Nam").ToList();
                    ViewBag.category = "Sản Phẩm Nam";
                    break;
                case "nu":
                    pro = db.Products.Where(p => p.Category.CategoryName == "Giày Nữ").ToList();
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

            // Phân trang
            int RowOfPage = 8; // Số sản phẩm trên mỗi trang
            ViewBag.page = page;
            ViewBag.totalPages = (int)Math.Ceiling(pro.Count() / (double)RowOfPage);
            pro = pro.Skip((page - 1) * RowOfPage).Take(RowOfPage).ToList();

            return View(pro);
        }
        public ActionResult Create()
        {
            MyDBContext db = new MyDBContext();
            ViewBag.Categories = db.Categorys.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product, List<ProductSize> Sizes, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                using (MyDBContext db = new MyDBContext())
                {
                    // Xử lý ảnh
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".png" };
                        var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Image", "Chỉ chấp nhận file ảnh PNG hoặc JPG");
                            return View(product);
                        }

                        string fileName = Guid.NewGuid().ToString() + fileExtension;
                        string imagesFolder = Server.MapPath("~/Images");
                        string filePath = Path.Combine(imagesFolder, fileName);
                        imageFile.SaveAs(filePath);

                        product.HinhAnh = Url.Content("~/Images/" + fileName);
                    }

                    // Lưu thông tin sản phẩm
                    db.Products.Add(product);
                    db.SaveChanges();

                    // Lưu thông tin size
                    if (Sizes != null && Sizes.Count > 0)
                    {
                        foreach (var size in Sizes)
                        {
                            size.ProId = product.ProId; // Liên kết với sản phẩm vừa tạo
                            db.productSizes.Add(size);
                        }
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
            }

            return View(product);
        }


        public ActionResult Delete(int id)
        {
            using (MyDBContext db = new MyDBContext())
            {
                var product = db.Products.Find(id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            using (MyDBContext db = new MyDBContext())
            {
                var product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Categories = db.Categorys.ToList();
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                using (MyDBContext db = new MyDBContext())
                {
                    var existingProduct = db.Products.Find(product.ProId);
                    if (existingProduct != null)
                    {
                        // Cập nhật các thông tin khác
                        existingProduct.ProName = product.ProName;
                        existingProduct.MoTa = product.MoTa;
                        existingProduct.Gia = product.Gia;
                        existingProduct.BanChay = product.BanChay;
                        existingProduct.Moi = product.Moi;
                        existingProduct.KhuyenMai = product.KhuyenMai;
                        existingProduct.CategoryId = product.CategoryId;

                        // Xử lý upload ảnh mới
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            var allowedExtensions = new[] { ".jpg", ".png" };
                            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                            if (allowedExtensions.Contains(fileExtension))
                            {
                                // Tạo tên file và lưu ảnh
                                string fileName = Guid.NewGuid().ToString() + fileExtension;
                                string imagesFolder = Server.MapPath("~/Images");
                                string filePath = Path.Combine(imagesFolder, fileName);
                                imageFile.SaveAs(filePath);

                                // Cập nhật đường dẫn ảnh mới
                                existingProduct.HinhAnh = Url.Content("~/Images/" + fileName);
                            }
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(product);
        }

    }

}