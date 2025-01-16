using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Controllers
{
    public class UserPanel_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();

        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult AccountDetails()
        {
            AppUser user = Session["user"] as AppUser;
            try
            {
                ViewBag.amount = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0).OrderDetails.Sum(od => od.Amount);
            }
            catch
            {
                ViewBag.amount = 0;
            }

            return View(user);
        }
        [HttpPost]
        public ActionResult EditAccount([Bind(Include = "UserID,FullName,Phone,Address,Email")] AppUser data)
        {
            var user = db.AppUsers.FirstOrDefault(u => u.UserID == data.UserID);

            user.FullName = data.FullName;
            user.Phone = data.Phone;
            user.Address = data.Address;
            user.Email = data.Email;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { isSuccess = "true"});
        }
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult Order()
        {
            AppUser user = Session["user"] as AppUser;

            var Order = db.CustomerOrders.Where(o => o.OrderedBy == user.UserID && o.Status != 0).ToList();

            if(Order.Count == 0)
                ViewBag.Empty = "Trống";
            else
                ViewBag.Empty = "";

            try
            {
                ViewBag.amount = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0).OrderDetails.Sum(od => od.Amount);
            }
            catch
            {
                ViewBag.amount = 0;
            }

            return View();
        }
        public ActionResult GetOrderList()
        {
            AppUser user = Session["user"] as AppUser;

            var Order = db.CustomerOrders.Where(o => o.OrderedBy == user.UserID && o.Status != 0).ToList();

            var data = Order.Select(o => new
            {
                o.OrderID,
                OrderDate = o.OrderDate.Value.ToString("HH:mm:ss dd/MM/yyyy"),
                Status = o.Status == 1 ? "Chờ duyệt" : (o.Status == 2 ? "Chờ giao" : "Đã giao"),
                TotalPayment = o.OrderDetails.Sum(od => od.Food.FoodPrice * (1 - (od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now) != null ? od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now).Promotion.Discount : 0)) * od.Amount),
                TotalFood = o.OrderDetails.Sum(od => od.Amount)
            }).ToArray();

            return Json(data,JsonRequestBehavior.AllowGet);
        }
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult OrderDetails(string id)
        {
            AppUser user = Session["user"] as AppUser;

            ViewBag.OrderID = id;

            try
            {
                ViewBag.amount = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0).OrderDetails.Sum(od => od.Amount);
            }
            catch
            {
                ViewBag.amount = 0;
            }

            return View();
        }
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult GetOrderDetails(string id)
        {
            AppUser user = Session["user"] as AppUser;

            var orderDetailsList = db.CustomerOrders.FirstOrDefault(o => o.OrderID == id).OrderDetails
                .Select(od => new { 
                    od.Food.FoodImage,
                    od.Food.FoodName,
                    od.Amount,
                    FoodPrice = od.Food.FoodPrice * (1 - (od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now) != null ? od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now).Promotion.Discount : 0)),
                }).ToArray();

            ViewBag.OrderID = id;

            try
            {
                ViewBag.amount = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0).OrderDetails.Sum(od => od.Amount);
            }
            catch
            {
                ViewBag.amount = 0;
            }

            return Json(orderDetailsList,JsonRequestBehavior.AllowGet);
        }
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult ChangePassword()
        {
            AppUser user = Session["user"] as AppUser;
            try
            {
                ViewBag.amount = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0).OrderDetails.Sum(od => od.Amount);
            }
            catch
            {
                ViewBag.amount = 0;
            }

            return View(user);
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult ChangePassword(string oldPwd, string newPwd, string confirmNewPwd)
        {
            ViewBag.oldPwd = oldPwd;
            ViewBag.newPwd = newPwd;
            ViewBag.confirmNewPwd = confirmNewPwd;

            AppUser user = Session["user"] as AppUser;

            if (user.Password != oldPwd)
            {
                ViewBag.errMsg = "Sai mật khẩu";
                return View(user);
            } 

            if(newPwd != confirmNewPwd)
            {
                ViewBag.errMsg = "Xác nhận mật khẩu không trùng khớp";
                return View(user);
            }
            var appUser = db.AppUsers.FirstOrDefault(u => u.UserID == user.UserID);

            appUser.Password = newPwd;

            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect("/User");
        }
    }
}