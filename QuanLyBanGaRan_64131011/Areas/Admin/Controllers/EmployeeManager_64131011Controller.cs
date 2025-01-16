using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class EmployeeManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Index(string fn = "", string mail = "", string rid = "", string ds = "", string de = "", string phone = "", string id = "")
        {
            ViewBag.fn = fn; ViewBag.mail = mail; ViewBag.ds = ds; ViewBag.de = de; ViewBag.phone = phone;

            ViewBag.header = "Nhân sự";
            ViewBag.sectionid = "UserID (chỉ NS)";

            if (de == "")
                de = ds;

            DateTime.TryParseExact(ds, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime DS);
            DateTime.TryParseExact(de, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime DE);

            var appUsers = db.AppUsers
                .Include(a => a.AppRole)
                .Where(u => u.RoleID != "CUSTOMER" && u.RoleID != "ADMIN" &&
                    (id == "" || u.UserID.Contains(id.ToUpper())) &&
                    (fn == "" || u.FullName.Contains(fn)) &&
                    (mail == "" || u.FullName.Contains(mail)) &&
                    (phone == "" || u.FullName.Contains(phone)) &&
                    (rid == "" || u.RoleID == rid) &&
                    (ds == "" || u.DOB >= DS && u.DOB <= DE)
                );

            ViewBag.rid = new SelectList (new List<SelectListItem> {
                new SelectListItem { Text = "Tất cả", Value = ""},
                new SelectListItem { Text = "Quản lý trang web", Value = "MANAGER"},
                new SelectListItem { Text = "Giao hàng", Value = "SHIPPER"},
            }, "Value", "Text", rid);

            return View(appUsers.ToList());
        }
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppUser emp = db.AppUsers.Find(id);
            ViewBag.header = "Nhân sự";

            if (emp == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoleID = new SelectList(db.AppRoles.Where(r => r.RoleID != "CUSTOMER" && r.RoleID != "ADMIN"), "RoleID", "RoleName", emp.RoleID);

            return View(emp);
        }
        [HttpPost]
        [RoleAuthorize_64131011(RoleID = "ADMIN")]
        public ActionResult Create(HttpPostedFileBase Avatar, [Bind(Include = "FullName,Email,Phone,CCCD,Avatar,Address,DOB,Username,Password,RoleID")] AppUser emp)
        {
            try
            {
                string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                // Tạo đường dẫn tới thư mục lưu trữ hình ảnh
                string path = Server.MapPath("/Images/" + fileName);

                // Lưu file vào thư mục trên server
                Avatar.SaveAs(path);

                // Lưu tên file vào thuộc tính AnhNV của NhanVien
                emp.Avatar = fileName;

                var currId = db.AppUsers.Where(u => u.RoleID == "MANAGER" || u.RoleID == "SHIPPER").Max(u => u.UserID);
                var currIdString = (int.Parse(currId.Substring(2)) + 1).ToString();
                // Tạo id
                emp.UserID = "NV" + new string('0', 6 - currIdString.Count()) + currIdString;
                db.AppUsers.Add(emp);

                db.SaveChanges();
                return Redirect("/Admin/Employee");
            }
            catch (Exception ex) {
                throw ex;
            }

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
        public ActionResult Edit(HttpPostedFileBase Avatar, [Bind(Include = "UserID,FullName,Email,Address,DOB,Phone,CCCD,Username,Password,RoleID")] AppUser emp, string prevAvt)
        {
            if (Avatar != null)
            {
                string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                // Tạo đường dẫn tới thư mục lưu trữ hình ảnh
                string path = Server.MapPath("/Images/" + fileName);

                // Lưu file vào thư mục trên server
                Avatar.SaveAs(path);

                // Lưu tên file vào thuộc tính AnhNV của NhanVien
                emp.Avatar = fileName;
            }
            else
                emp.Avatar = prevAvt;
            db.Entry(emp).State = EntityState.Modified;
            db.SaveChanges();

            return Json(emp);
        }
    }
}