using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Views
{
    public class CartManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        // GET: CartManager_64131011
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult Index()
        {
            AppUser user = Session["user"] as AppUser;

            var odList = db.OrderDetails.Where(od => od.CustomerOrder.OrderedBy == user.UserID && od.CustomerOrder.Status == 0);

            var cart = odList.Select(od => new CartViewModel_64131011
            {
                FoodImage = od.Food.FoodImage,
                FoodID = od.Food.FoodID,
                FoodName = od.Food.FoodName,
                Amount = od.Amount,
                TotalPrice = od.Food.FoodPrice * (1 - (od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now) != null ? od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now).Promotion.Discount : 0)) * od.Amount,
            }).ToList();

            if (cart.Count == 0)
                ViewBag.Empty = "Giỏ hàng của bạn đang trống. Hãy đặt món ngay";
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

            ViewBag.VoucherID = new SelectList(db.Vouchers.ToList(),"VoucherID","VoucherID");
                
            return View(cart);
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult UpdateFoodAmount(string FoodID, byte Amount) 
        {
            AppUser user = Session["user"] as AppUser;

            var foodItem = db.OrderDetails.FirstOrDefault(od => od.CustomerOrder.OrderedBy == user.UserID && od.CustomerOrder.Status == 0 && od.FoodID == FoodID);

            foodItem.Amount = Amount;

            db.Entry(foodItem).State = EntityState.Modified;
            db.SaveChanges();

            var cart = db.OrderDetails.Select(od => new
            {
                TotalPrice = od.Food.FoodPrice * (1 - (od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now) != null ? od.Food.FoodPromotions.FirstOrDefault(fp => fp.DateEnd >= DateTime.Now).Promotion.Discount : 0)) * od.Amount,
                od.CustomerOrder,
                od.Amount,
                od.FoodID
            })
            .Where(od => od.CustomerOrder.OrderedBy == user.UserID && od.CustomerOrder.Status == 0);

            var TotalPayment = cart.Sum(c => c.TotalPrice);
            var TotalFood = cart.Sum(c => c.Amount);

            var TotalFoodPrice = cart.FirstOrDefault(c => c.FoodID == FoodID).TotalPrice;
            

            return Json(new { IsSuccess = "true", TotalPayment, TotalFoodPrice, TotalFood });
        }
        [HttpPost]
        public ActionResult AddFood(string FoodID)
        {
            AppUser user = Session["user"] as AppUser;

            var order = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0);

            if (order == null) 
            {
                var countOrder = db.CustomerOrders.Max(o => o.OrderID) ?? "OR000000";
                var countOrderString = (int.Parse(countOrder.Substring(2)) + 1).ToString();

                order = new CustomerOrder
                {
                    OrderedBy = user.UserID,
                    OrderID = "OR" + new string('0', 6 - countOrderString.Count()) + countOrderString,
                    Status = 0
                };

                db.CustomerOrders.Add(order);
                db.SaveChanges();
            }

            var orderID = order.OrderID;

            var isExisted = db.OrderDetails.Count(od => od.OrderID == orderID && od.FoodID == FoodID);

            if (isExisted == 0)
            {
                var od = new OrderDetail
                {
                    OrderID = orderID,
                    FoodID = FoodID,
                    Amount = 1
                };

                db.OrderDetails.Add(od);
            }
            else
            {
                var foodItem = db.OrderDetails.FirstOrDefault(od => od.OrderID == orderID && od.FoodID == FoodID);

                foodItem.Amount = (byte)(foodItem.Amount + 1);

                db.Entry(foodItem).State = EntityState.Modified;
            }
                             
            db.SaveChanges();
            
            return Json(new { isSuccess = "True" });
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult Payment(string Address, string Phone, string FullName)
        {
            AppUser user = Session["user"] as AppUser;

            var us = db.AppUsers.FirstOrDefault(u => u.UserID == user.UserID);

            if (Address != null)
                us.Address = Address;

            if (Phone != null)
                us.Phone = Phone;

            if (FullName != null)
                us.FullName = FullName;

            db.Entry(us).State = EntityState.Modified;
            db.SaveChanges();

            var order = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0);
            order.Status = 1;
            order.OrderDate = DateTime.Now;

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect("/User/Order");
        }
        [RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        public ActionResult RemoveFood(string id)
        {
            AppUser user = Session["user"] as AppUser;

            var order = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0);
            var od = db.OrderDetails.FirstOrDefault(ods => ods.OrderID == order.OrderID && ods.FoodID == id);
            order.OrderDetails.Remove(od);

            db.SaveChanges();

            return Redirect("/Cart");
        }
        //[HttpPost]
        //[RoleAuthorize_64131011(RoleID = "CUSTOMER")]
        //public ActionResult AddVoucher(string VoucherID)
        //{
        //    AppUser user = Session["user"] as AppUser;

        //    var order = db.CustomerOrders.FirstOrDefault(o => o.OrderedBy == user.UserID && o.Status == 0);
        //    order.VoucherID = VoucherID;

        //    db.Entry(order).State = EntityState.Modified;
        //    db.SaveChanges();
        //    var discount = db.Vouchers.FirstOrDefault(v => v.VoucherID == VoucherID).Discount;
        //    return Json(new { isSucces = true, Discount = discount });
        //}
    }
}