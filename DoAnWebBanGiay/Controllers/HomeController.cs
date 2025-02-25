using DoAnWebBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCrypt.Net;
namespace DoAnWebBanGiay.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            List<Product> pro = db.Products.ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(pro);
        }
        public ActionResult gioithieu()
        {
            return View();
        }
        public ActionResult lienhe()
        {
            return View();
        }
    }
}