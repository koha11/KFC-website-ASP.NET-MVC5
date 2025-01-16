using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Controllers
{
    public class Menu_6413011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        // GET: Menu_6413011
        public ActionResult Index()
        {
            try
            {
                AppUser user = Session["user"] as AppUser;
                ViewBag.amount = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0).OrderDetails.Sum(od => od.Amount);
            }
            catch
            {
                ViewBag.amount = 0;
            }
           
            var foods = db.Foods.ToList();

            return View(foods);
        }
    }
}