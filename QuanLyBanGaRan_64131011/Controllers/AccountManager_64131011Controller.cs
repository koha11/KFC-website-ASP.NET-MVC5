using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace QuanLyBanGaRan_64131011.Controllers
{
    public class AccountManager_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        public ActionResult Login()
        {
            AppUser user = Session["user"] as AppUser;

            if (user != null)
                return Redirect("/");
            else
            {
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
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel_64131011 user)
        {
            HttpCookie cookie = new HttpCookie("User");

            if (user.RememberMe == true)
            {
                cookie["username"] = user.Username;
                cookie["password"] = user.Password;
            }    

            var account = db.AppUsers.SingleOrDefault(u => u.Username.ToLower() == user.Username.ToLower() && u.Password == user.Password);
            
            if(account != null)
            {
                if(account.IsActived == false)
                {
                    // Sinh mã OTP
                    string otpCode = GenerateOTP();

                    // Gửi email
                    SendOtpEmail(account.Email, otpCode);

                    Session["OTP"] = otpCode;

                    return View("ConfirmAccount");
                }
                else
                {
                    Session["user"] = account;
                    return Redirect("/");
                }
            }


            return View();
        }
        
        public ActionResult Logout()
        {
            Session.Remove("user");
            return Redirect("/");
        }
        public ActionResult Register()
        {
            AppUser user = Session["user"] as AppUser;

            if (user != null)
                return Redirect("/");
            else
            {
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
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel_64131011 user)
        {
            var isEmailExisted = db.AppUsers.Count(u => u.Email == user.Email);

            ViewBag.error = new List<object>();

            if (isEmailExisted != 0)
            {
                ViewBag.error.Add(new { name = "Email", msg = "Email này đã được sử dụng" });
                return View(user);
            }

            var countCus = db.AppUsers.Where(u => u.RoleID == "CUSTOMER").Max(u => u.UserID);

            var countCusString = (int.Parse(countCus.Substring(2)) + 1).ToString();
            // Tạo id
            var uid = "KH" + new string('0', 6 - countCusString.Count()) + countCusString;

            AppUser us = new AppUser { UserID = uid, Username = user.Username, Password = user.Password, Email = user.Email, Address = user.Address, Phone = user.Phone, RoleID = "CUSTOMER" };
            db.AppUsers.Add(us);
            db.SaveChanges();

            // Sinh mã OTP
            string otpCode = GenerateOTP();

            // Gửi email
            SendOtpEmail(us.Email, otpCode);

            Session["OTP"] = otpCode;

            ViewBag.uid = uid;

            return View("ConfirmAccount");
        }
        public ActionResult ConfirmAccount()
        {
            if (Session["OTP"] == null)
                return Redirect("/");

            AppUser user = Session["user"] as AppUser;

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
        public ActionResult Otp(string otp, string uid)
        {
            string realOtp =  (string) Session["OTP"];
            var result = otp == realOtp;
            
            if(result)
            {
                AppUser us = db.AppUsers.FirstOrDefault(u => u.UserID == uid);
                us.IsActived = true;

                db.Entry(us).State = EntityState.Modified;
                db.SaveChanges();

                Session["user"] = us;
            }    

            return Json(new { isValid = result, real = realOtp }, JsonRequestBehavior.AllowGet);
        }
        public static string GenerateOTP(int length = 6)
        {
            var random = new Random();
            return new string(Enumerable.Repeat("0123456789", length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void SendOtpEmail(string recipientEmail, string otpCode)
        {
            string senderEmail = "takhoa090604.nvtroi1922@gmail.com"; // Gmail của Oppa
            string senderPassword = "rpog mmsf mloa uvgb"; // Mật khẩu ứng dụng

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = "Xác nhận tài khoản - OTP",
                Body = $"Chào Khách hàng kính mến,\n\nMã OTP của bạn là: {otpCode}\n\nVui lòng nhập mã này để xác nhận tài khoản.\n\nCảm ơn!",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(recipientEmail);

            smtpClient.Send(mailMessage);
        }
        [HttpPost]               
        public ActionResult RecoverPassword(string email)
        {
            AppUser user = db.AppUsers.FirstOrDefault(u => u.Email == email);

            if(user.RoleID != "CUSTOMER")
                return Redirect("/Login");

            var newPwd = GenerateRandomPassword(6);
            user.Password = newPwd;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            SendRecoverPWDEmail(email, newPwd);

            return Redirect("/Login");
        }
        static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            Random random = new Random();

            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }
        public void SendRecoverPWDEmail(string recipientEmail, string pwd)
        {
            string senderEmail = "takhoa090604.nvtroi1922@gmail.com"; // Gmail của Oppa
            string senderPassword = "rpog mmsf mloa uvgb"; // Mật khẩu ứng dụng

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = "Khôi phục mật khẩu",
                Body = $"Chào Khách hàng kính mến,\n\nMật khẩu mới của bạn là: {pwd}\n\nCảm ơn!\n\nSau khi vô tài khoản làm ơn hãy thay đổi mật khẩu!",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(recipientEmail);

            smtpClient.Send(mailMessage);
        }
    }
}