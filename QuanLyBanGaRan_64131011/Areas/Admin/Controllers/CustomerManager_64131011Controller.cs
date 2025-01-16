using Microsoft.Ajax.Utilities;
using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class CustomerManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Index(string fn = "", string mail = "", string phone = "", string addr = "", string id = "")
        {
            ViewBag.fn = fn; ViewBag.mail = mail; ViewBag.phone = phone; ViewBag.addr = addr;

            ViewBag.header = "Khách hàng";
            ViewBag.sectionid = "UserID (chỉ KH)";

            var customers = db.AppUsers.Where(u =>
                (id == "" || u.UserID.Contains(id.ToUpper())) &&           
                (u.RoleID == "CUSTOMER") &&
                (fn == "" || u.FullName.Contains(fn)) &&
                (mail == "" || u.Email.Contains(mail)) &&
                (phone == "" || u.Phone.Contains(phone)) &&
                (addr == "" || u.Address.Contains(addr)));            

            return View(customers.ToList());
        }
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppUser cus = db.AppUsers.Find(id);
            ViewBag.header = "Khách hàng";

            if (cus == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsActived = new SelectList(new List<SelectListItem> {
                new SelectListItem { Text = "Đã kích hoạt", Value = "true"},
                new SelectListItem { Text = "Chưa kích hoạt", Value = "false"},
            }, "Value", "Text", cus.IsActived);

            return View(cus);
        }        
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,Email,Phone,CCCD,Avatar,Address,Username,Password,RoleID")] AppUser emp)
        {
            if (ModelState.IsValid)
            {
                emp.RoleID = "CUSTOMER";
                var countEmp = db.AppUsers.Where(u => u.RoleID == "CUSTOMER").Max(u => u.UserID);
                var countEmpString = (int.Parse(countEmp.Substring(2)) + 1).ToString();
 
                // Tạo id
                emp.UserID = "KH" + new string('0', 6 - countEmpString.Count()) + countEmpString;
                db.AppUsers.Add(emp);

                db.SaveChanges();
                return Redirect("/Admin/Customer");
            }

            //ViewBag.MaLop = new SelectList(db.LOPs, "MaLop", "TenLop", sINHVIEN.MaLop);
            return View(emp);
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Delete(string id)
        {
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.AppUsers.Remove(appUser);
                db.SaveChanges();
                return Redirect("/Admin/Employee");
            }
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Edit([Bind(Include = "UserID,FullName,Email,Phone,Avatar,Address,Username,Password,IsActived,RoleID")] AppUser cus)
        {
            var customer = db.AppUsers.FirstOrDefault(u => u.UserID == cus.UserID);

            customer.FullName = cus.FullName;
            customer.Phone = cus.Phone;
            customer.Address = cus.Address;
            customer.Username = cus.Username;
            customer.IsActived = cus.IsActived;

            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new {isSuccess = "True"});
        }
    }
}