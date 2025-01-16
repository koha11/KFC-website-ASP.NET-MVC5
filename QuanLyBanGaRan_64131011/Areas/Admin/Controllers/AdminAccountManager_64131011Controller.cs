using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class AdminAccountManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        public ActionResult Login()
        {
            AppUser account = Session["user"] as AppUser;

            if (account != null)
            {
                if (account.RoleID == "CUSTOMER")
                    Session.Remove("user");
                else
                    return Redirect("/Admin");
            }    

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel_64131011 user)
        {
            var account = db.AppUsers.SingleOrDefault(u => u.Username.ToLower() == user.Username.ToLower() && u.Password == user.Password && u.RoleID != "CUSTOMER");

            if (account != null)
            {
                Session["user"] = account;
 
                return Redirect("/Admin");
            }
            else
            {
                ViewBag.ErrMsg = "Thông tin tài khoản không hợp lệ";
                ViewBag.username = user.Username;
                ViewBag.password = user.Password;

                return View();
            }    
        }
        public ActionResult Logout()
        {
            Session.Remove("user");

            return Redirect("/Admin/Login");
        }
        public ActionResult NoneAuth() { 
            return View();
        }
    }
}