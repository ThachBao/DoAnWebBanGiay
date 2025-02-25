using DoAnWebBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanGiay.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            List<Product> pro = db.Products.ToList();
            return View(pro);
        }
    }
}