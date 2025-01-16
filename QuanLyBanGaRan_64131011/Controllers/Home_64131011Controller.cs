using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QuanLyBanGaRan_64131011.Controllers
{
    public class Home_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        public ActionResult Index()
        {
            try
            {
                AppUser user = Session["user"] as AppUser;
                ViewBag.amount = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0).OrderDetails.Sum(od => od.Amount);
            }
            catch {
                ViewBag.amount = 0;
            }

            var foods = db.Foods.ToList();

            Random random = new Random();

            var shuffleFoods = foods.OrderBy(x => random.Next()).Take(10).ToList();


            return View(shuffleFoods);
        }
    }
}