using Microsoft.Ajax.Utilities;
using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class PromotionManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult Index(string pid = "", string ds = "", string de = "", string id = "")
        {
            ViewBag.ds = ds; ViewBag.de = "";

            ViewBag.header = "Khuyến mãi";
            ViewBag.sectionid = "PromotionID";

            DateTime.TryParseExact(ds, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime DS);
            DateTime.TryParseExact(de, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime DE);

            var foodPromotions = db.FoodPromotions
                .Where(fp => (id == "" || fp.PromotionID.Contains(id.ToUpper())) &&
                (fp.DateEnd >= DateTime.Now) &&
                (pid == "" || fp.PromotionID == pid) &&
                (ds == "" || fp.DateStart >= DS) &&
                (de == "" || fp.DateEnd <= DE))
                .Include(fp => fp.Promotion)
                .Include(fp => fp.Food);

            var pids = db.Promotions
                .Select(fc => new SelectListItem
                {
                    Value = fc.PromotionID,
                    Text = fc.PromotionName
                })
                .ToList();

            // Thêm giá trị mặc định vào đầu danh sách
            pids.Insert(0, new SelectListItem { Value = "", Text = "Tất cả" });

            // Gán vào ViewBag
            ViewBag.pid = new SelectList(pids, "Value", "Text", pid);

            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "FoodName");
            ViewBag.PromotionID = new SelectList(db.Promotions, "PromotionID", "PromotionName");

            return View(foodPromotions.ToList());
        }
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult OldPromotion(string DS = "", string DE = "", string PID = "")
        {
            DateTime.TryParse(DS, out DateTime ds);
            DateTime.TryParse(DE, out DateTime de);

            ViewBag.ds = DS;
            ViewBag.de = DE;

            ViewBag.header = "Khuyến mãi";            
            var promotions = db.Promotions;
            var foodPromotions = db.FoodPromotions.Where(
                    fp => fp.DateEnd < DateTime.Now &&
                    (DS == "" || fp.DateStart >= ds.Date) &&
                    (DE == "" || fp.DateEnd <= de.Date) &&
                    (PID == "" || fp.PromotionID == PID)
                ).Include(fp => fp.Promotion)
                .Include(fp => fp.Food);

            var PromotionSelectList = db.Promotions.Select(p => 
                new SelectListItem
                {
                    Value = p.PromotionID,
                    Text = p.PromotionName
                }).ToList();

            PromotionSelectList.Insert(0, new SelectListItem { Value = "", Text = "Tất cả" });
            ViewBag.PID = PromotionSelectList;

            return View(foodPromotions.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Create([Bind(Include = "FoodID,PromotionID,DateStart,DateEnd")] FoodPromotion fp)
        {
            if (ModelState.IsValid)
            {
                db.FoodPromotions.Add(fp);

                db.SaveChanges();
                return Redirect("/Admin/Promotion");
            }

            return View(fp);
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Delete(string id)
        {
            FoodPromotion fp = db.FoodPromotions.FirstOrDefault(fps => fps.PromotionID == id && fps.DateEnd >= DateTime.Now);
            if (fp == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.FoodPromotions.Remove(fp);
                db.SaveChanges();
                return Redirect("/Admin/Promotion");
            }
        }
    }
}