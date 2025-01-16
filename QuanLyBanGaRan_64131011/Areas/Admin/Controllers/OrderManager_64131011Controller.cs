using Microsoft.SqlServer.Server;
using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Areas.Admin.Data;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class OrderManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult Index(string ods = "", string ode = "", string ots = "00:00", string ote = "23:59", string cid = "", string aid = "", string sid = "", string stt = "", string id = "")
        {
            ViewBag.header = "Đơn hàng";
            ViewBag.sectionid = "OrderID";

            // Viewbag để lưu giá trị filter trước khi apply filter
            ViewBag.ods = ods;
            ViewBag.ode = ode;
            ViewBag.ots = ots;
            ViewBag.ote = ote;
            ViewBag.cid = cid;
            ViewBag.aid = aid;
            ViewBag.sid = sid;
            ViewBag.status = stt;

            if (ode == "")
                ode = ods;

            DateTime.TryParseExact(ots + ":00 " + ods, "HH:mm:ss dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,out DateTime ODS);
            DateTime.TryParseExact(ote + ":00 " + ode, "HH:mm:ss dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ODE);

            byte.TryParse(stt, out byte STT);

            // Láy ra thông tin về order mong muốn hiển thị ra màn hình kết hợp với các truy vấn tìm kiếm
            var orders = db.CustomerOrders.Select(o => new OrderViewModel_64131011
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                OrderedBy = o.OrderedBy,
                Status = (byte)o.Status,
                AcceptedBy = o.AcceptedBy,
                ShippedBy = o.ShippedBy,
                TotalPrice = o.TotalPrice,
                CountFood = db.OrderDetails.Where(orderFood => orderFood.OrderID == o.OrderID).Sum(orderFood => orderFood.Amount)
            })
                .Where(o => (id == "" || o.OrderID.Contains(id.ToUpper())) &&
                            (ods == "" || (o.OrderDate.HasValue && o.OrderDate >= ODS)) &&
                            (ode == "" || (o.OrderDate.HasValue && o.OrderDate <= ODE)) &&
                            (cid == "" || o.OrderedBy == cid) &&
                            (aid == "" || o.AcceptedBy == aid) &&
                            (sid == "" || o.ShippedBy == sid) &&
                            ((stt == "" && o.Status != 0) || o.Status == STT)                      
                );

            var statusList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Tất cả" },
                new SelectListItem { Value = "0", Text = "Đang đặt" },
                new SelectListItem { Value = "1", Text = "Chờ duyệt" },
                new SelectListItem { Value = "2", Text = "Chờ giao hàng" },
                new SelectListItem { Value = "3", Text = "Đã hoàn thành" }
            }, "Value", "Text", ViewBag.stt);

            ViewBag.CustomerList = db.AppUsers.Where(u => u.RoleID == "CUSTOMER").Select(u => u.UserID).ToList();
            ViewBag.ManagerList = db.AppUsers.Where(u => u.RoleID == "MANAGER").Select(u => u.UserID).ToList();
            ViewBag.ShipperList = db.AppUsers.Where(u => u.RoleID == "SHIPPER").Select(u => u.UserID).ToList();


            ViewBag.stt = statusList;

            return View(orders.OrderByDescending(o => o.OrderDate).ToList());
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Create([Bind(Include = "OrderedBy,OrderDate,FinishDate,AcceptedBy,AcceptDate,Status,ShippedBy,ShipmentDate")] CustomerOrder order)
        {
            if (ModelState.IsValid)
            {
                var countOrder = db.CustomerOrders.Max(o => o.OrderID);
                var countOrderString = (int.Parse(countOrder.Substring(2)) + 1).ToString();
                // Tạo id
                order.OrderID = "OR" + new string('0', 6 - countOrderString.Count()) + countOrderString;
                db.CustomerOrders.Add(order);
                db.SaveChanges();

                return Redirect("/Admin/Order");
            }  
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Delete(string id)
        {
            var order = db.CustomerOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                var orderDetails = db.OrderDetails.Where(od => od.OrderID == id).ToList();
                db.OrderDetails.RemoveRange(orderDetails);
                db.SaveChanges();

                db.CustomerOrders.Remove(order);
                db.SaveChanges();
                return Redirect("/Admin/Order");
            }
        }
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.CustomerOrders.Find(id);
            ViewBag.header = "Đơn hàng";
             
            var ods = new
            OrderDetailViewModel_64131011 {
                OrderID = order.OrderID,
                OrderedBy = order.OrderedBy,
                CName = order.AppUser1.FullName,
                AcceptedBy = order.AcceptedBy ?? "N/a",
                ShippedBy = order.ShippedBy ?? "N/a",
                AName = order.AcceptedBy != null ? order.AppUser.FullName : "N/a",
                SName = order.ShippedBy != null ? order.AppUser2.FullName : "N/a",
                Address = order.AppUser1.Address,
                OrderDate = order.OrderDate.HasValue == true ? order.OrderDate.Value.ToString("HH:mm:ss dd/MM/yyyy") : "N/a",
                AcceptDate = order.AcceptDate.HasValue == true ? order.AcceptDate.Value.ToString("HH:mm:ss dd/MM/yyyy") : "N/a",
                ShipmentDate = order.ShipmentDate.HasValue == true ? order.ShipmentDate.Value.ToString("HH:mm:ss dd/MM/yyyy") : "N/a",
                FinishDate = order.FinishDate.HasValue == true ? order.FinishDate.Value.ToString("HH:mm:ss dd/MM/yyyy") : "N/a",
                Status = order.Status ?? 0,
                TotalPrice = (order.TotalPrice.HasValue == true ? order.TotalPrice.Value : 0),
            };

            ViewBag.Status = new SelectList(new List<SelectListItem> {
                new SelectListItem { Text = "Đang đặt", Value = "0"},
                new SelectListItem { Text = "Chờ duyệt", Value = "1"},
                new SelectListItem { Text = "Chờ giao", Value = "2"},
                new SelectListItem { Text = "Đã hoàn thành", Value = "3"},
            }, "Value", "Text", ods.Status);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(ods);
        }
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult GetOrderFoodList(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.CustomerOrders.SingleOrDefault(u => u.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var orderDetails = db.OrderDetails.Where(od => od.OrderID == id)
                .ToArray()
                .Select(od => new
                {
                    od.FoodID,
                    od.Food.FoodName,
                    FoodPrice = od.Food.FoodPrice.ToString("N0") + " VNĐ",
                    od.Amount,
                    od.Food.FoodCategory.FoodCategoryName,
                    NewPrice = (od.Food.FoodPrice - od.Food.FoodPrice *
                        od.Food.FoodPromotions.Where(fp => fp.FoodID == od.FoodID
                            && fp.DateStart <= DateTime.Now
                            && fp.DateEnd >= DateTime.Now)
                        .Select(fp => fp.Promotion.Discount)
                        .FirstOrDefault()).ToString("N0") + " VNĐ"
                });

            return Json(orderDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Edit([Bind(Include = "OrderID,OrderedBy,AcceptedBy,ShippedBy,Status,OrderDate,AcceptDate,ShipmentDate,FinishDate,TotalPrice")] CustomerOrder order)
        {
            if (order.OrderDate.HasValue == true)
            {
                DateTime temp = order.OrderDate.Value;
                order.OrderDate = new DateTime(temp.Year, temp.Day, temp.Month, temp.Hour, temp.Minute, temp.Second);
            }

            if (order.AcceptDate.HasValue == true)
            {
                DateTime temp = order.AcceptDate.Value;
                order.AcceptDate = new DateTime(temp.Year, temp.Day, temp.Month, temp.Hour, temp.Minute, temp.Second);
            }

            if (order.ShipmentDate.HasValue == true)
            {
                DateTime temp = order.ShipmentDate.Value;
                order.ShipmentDate = new DateTime(temp.Year, temp.Day, temp.Month, temp.Hour, temp.Minute, temp.Second);
            }

            if (order.FinishDate.HasValue == true)
            {
                DateTime temp = order.FinishDate.Value;
                order.FinishDate = new DateTime(temp.Year, temp.Day, temp.Month, temp.Hour, temp.Minute, temp.Second);
            }

            if (order.Status != 3)
            {
                order.TotalPrice = null;
            } 

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { isSuccess = "true" });
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult AddFood([Bind(Include = "OrderID,FoodID,Amount")] OrderDetail orderDetail)
        {
            db.OrderDetails.Add(orderDetail);
            db.SaveChanges();

            return Json(new { isSuccess = "true" });
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult UpdateOrderDetails([Bind(Include = "OrderID,FoodID,Amount")] OrderDetail orderDetail, int NewPrice)
        {
            db.Entry(orderDetail).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { amount = orderDetail.Amount, price = orderDetail.Amount * NewPrice });
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult RemoveFood(string OrderID, string FoodID)
        {
            var od = db.OrderDetails.FirstOrDefault(ods => ods.OrderID == OrderID && ods.FoodID == FoodID);
            db.OrderDetails.Remove(od);
            db.SaveChanges();

            return Json(new { isSuccess = "true" });
        }
        [RoleAuthorize_64131011(RoleID = "MANAGER")]
        public ActionResult AcceptOrder(string id = "", string temp = "") {
            ViewBag.header = "Đơn hàng";
            ViewBag.sectionid = "OrderID";

            var orders = db.CustomerOrders.Select(o => new OrderViewModel_64131011
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                OrderedBy = o.OrderedBy,
                Status = (byte)o.Status,
                AcceptedBy = o.AcceptedBy,
                ShippedBy = o.ShippedBy,
                TotalPrice = o.TotalPrice,
                CountFood = db.OrderDetails.Where(orderFood => orderFood.OrderID == o.OrderID).Sum(orderFood => orderFood.Amount)
            })
                .Where(o => o.Status == 1 && (id == "" || o.OrderID.Contains(id.ToUpper())));


            return View(orders.ToList());
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "MANAGER")]
        public ActionResult AcceptOrder(string OrderID)
        {
            ViewBag.header = "Đơn hàng";
           
            var order = db.CustomerOrders.Find(OrderID);

            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                AppUser user = Session["user"] as AppUser;
                order.Status = 2;
                order.AcceptDate = DateTime.Now;
                order.AcceptedBy = user.UserID;

                // Đánh dấu trạng thái cập nhật
                db.Entry(order).State = EntityState.Modified;

                // Lưu thay đổi vào DB
                db.SaveChanges();

                return Redirect("/Admin/Order/AcceptOrder");
            }
        }
        [RoleAuthorize_64131011(RoleID = "SHIPPER")]
        public ActionResult ShipOrder(string id = "", string temp = "")
        {
            ViewBag.header = "Đơn hàng";
            ViewBag.sectionid = "OrderID";

            var orders = db.CustomerOrders.Select(o => new OrderViewModel_64131011
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                OrderedBy = o.OrderedBy,
                Status = (byte)o.Status,
                AcceptedBy = o.AcceptedBy,
                ShippedBy = o.ShippedBy,
                TotalPrice = o.TotalPrice,
                CountFood = db.OrderDetails.Where(orderFood => orderFood.OrderID == o.OrderID).Sum(orderFood => orderFood.Amount)
            })
                .Where(o => o.Status == 2 && (id == "" || o.OrderID.Contains(id.ToUpper())));


            return View(orders.ToList());
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "SHIPPER")]
        public ActionResult ShipOrder(string OrderID)
        {
            ViewBag.header = "Đơn hàng";

            var order = db.CustomerOrders.Find(OrderID);

            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                AppUser user = Session["user"] as AppUser;
                order.Status = 3;
                order.ShipmentDate = DateTime.Now;
                order.FinishDate = DateTime.Now.AddMinutes(30);             
                order.ShippedBy = user.UserID;

                var orderDetails = db.OrderDetails.Where(od => od.OrderID == OrderID)
                .ToArray()
                .Select(od => new
                {                    
                    NewPrice = (od.Food.FoodPrice - od.Food.FoodPrice *
                        od.Food.FoodPromotions.Where(fp => fp.FoodID == od.FoodID
                            && fp.DateStart <= DateTime.Now
                            && fp.DateEnd >= DateTime.Now)
                        .Select(fp => fp.Promotion.Discount)
                        .FirstOrDefault())
                });

                order.TotalPrice = orderDetails.Sum(ods => ods.NewPrice);

                // Đánh dấu trạng thái cập nhật
                db.Entry(order).State = EntityState.Modified;

                // Lưu thay đổi vào DB
                db.SaveChanges();

                return Redirect("/Admin/Order/ShipOrder");
            }
        }
    }
}