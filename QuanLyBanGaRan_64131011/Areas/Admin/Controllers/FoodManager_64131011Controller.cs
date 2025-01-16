using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class FoodManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult Index(string fn = "", string ft = "", int fps = 0, int fpe = 0, string id = "")
        {
            ViewBag.MaxFP = (int)db.Foods.Max(f => f.FoodPrice);

            if (fpe == 0)
                fpe = ViewBag.MaxFP;


            ViewBag.fn = fn; ViewBag.fps = fps; ViewBag.fpe = fpe;

            ViewBag.header = "Thức ăn";
            ViewBag.sectionid = "FoodID";

            var foods = db.Foods
                .Where(f => (id == "" || f.FoodID.Contains(id.ToUpper())) &&
                (fn == "" || f.FoodName.Contains(fn)) &&
                (f.FoodPrice >= (decimal) fps && f.FoodPrice <= (decimal)fpe) &&
                (ft == "" || f.FoodCategoryID == ft))
                .Include(a => a.FoodCategory);

            var categories = db.FoodCategories
                .Select(fc => new SelectListItem
                {
                    Value = fc.FoodCategoryID.ToString(),
                    Text = fc.FoodCategoryName
                })
                .ToList();

            // Thêm giá trị mặc định vào đầu danh sách
            categories.Insert(0, new SelectListItem { Value = "", Text = "Tất cả" });

            // Gán vào ViewBag
            ViewBag.ft = new SelectList(categories, "Value", "Text", ft);

            ViewBag.FoodCategoryID = new SelectList(db.FoodCategories, "FoodCategoryID", "FoodCategoryName");

            return View(foods.ToList());
        }
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var food = db.Foods.Find(id);
            ViewBag.header = "Thức ăn";
            if (food == null)
            {
                return HttpNotFound();
            }

            ViewBag.FoodCategoryID = new SelectList(db.FoodCategories, "FoodCategoryID", "FoodCategoryName", food.FoodCategoryID);

            return View(food);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Create(HttpPostedFileBase FoodImage, [Bind(Include = "FoodName,FoodDetails,FoodPrice,FoodUnits,FoodCategoryID")] Food food)
        {
            if (ModelState.IsValid)
            {
                if(FoodImage != null)
                {
                    string fileName = System.IO.Path.GetFileName(FoodImage.FileName);
                    // Tạo đường dẫn tới thư mục lưu trữ hình ảnh
                    string path = Server.MapPath("/Images/" + fileName);

                    // Lưu file vào thư mục trên server
                    FoodImage.SaveAs(path);

                    // Lưu tên file vào thuộc tính AnhNV của NhanVien
                    food.FoodImage = fileName;
                }

                var countFood = db.Foods.Max(f => f.FoodID);
                var countFoodString = (int.Parse(countFood.Substring(1)) + 1).ToString();

                // Tạo id
                food.FoodID = "F" + new string('0', 7 - countFoodString.Count()) + countFoodString;
                db.Foods.Add(food);

                db.SaveChanges();
                return Redirect("/Admin/Food");
            }

            return View(food);
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Delete(string id)
        {
            var food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Foods.Remove(food);
                db.SaveChanges();
                return Redirect("/Admin/Food");
            }
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Edit(HttpPostedFileBase FoodImage, [Bind(Include = "FoodID,FoodName,FoodDetails,FoodPrice,FoodUnits,FoodCategoryID")] Food food, string prevImg)
        {
            if (FoodImage != null)
            {
                string fileName = System.IO.Path.GetFileName(FoodImage.FileName);
                // Tạo đường dẫn tới thư mục lưu trữ hình ảnh
                string path = Server.MapPath("/Images/" + fileName);

                // Lưu file vào thư mục trên server
                FoodImage.SaveAs(path);

                // Lưu tên file vào thuộc tính AnhNV của NhanVien
                food.FoodImage = fileName;
            }
            else
                food.FoodImage = prevImg;

            db.Entry(food).State = EntityState.Modified;
            db.SaveChanges();

            return Json(food);
        }
    }
}