using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class VoucherManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Index(decimal cs = 0, decimal ce = 0, string ia = "", string id = "")
        {
            ViewBag.header = "Voucher";
            ViewBag.sectionid = "VoucherID";

            ViewBag.maxC = db.Vouchers.Max(v => v.Condition);

            if (ce == 0)
                ce = ViewBag.maxC;

            ViewBag.cs = cs;
            ViewBag.ce = ce;
            ViewBag.ia = ia;

            Boolean.TryParse(ia, out bool IA);

            var vouchers = db.Vouchers.Where(v => 
                (id == "" || v.VoucherID.Contains(id.ToUpper())) &&
                v.Condition >= cs && v.Condition <= ce && 
                (ia == "" || v.IsActive == IA));

            ViewBag.ia = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Tất cả" },
                new SelectListItem { Value = "true", Text = "Đang kích hoạt" },
                new SelectListItem { Value = "false", Text = "Chưa kích hoạt" },
            }, "Value", "Text", ViewBag.ia);

            return View(vouchers.ToList());
        }
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var voucher = db.Vouchers.Find(id);
            ViewBag.IsActive = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Đang kích hoạt" },
                new SelectListItem { Value = "false", Text = "Chưa kích hoạt" },
            }, "Value", "Text", voucher.IsActive);
            ViewBag.header = "Voucher";
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Create([Bind(Include = "VoucherID,Discount,Condition,IsActive")] Voucher voucher)
        {
            db.Vouchers.Add(voucher);
            db.SaveChanges();

            return Redirect("/Admin/Voucher");
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
                return Redirect("/Admin/Voucher");
            }
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Edit([Bind(Include = "VoucherID,Discount,Condition,IsActive")] Voucher voucher)
        {
            db.Entry(voucher).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new {isSuccess = "true"});
        }
    }
}