using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyBanGaRan_64131011.App_Start
{
    public class RoleAuthorize_64131011 : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //1. check session => cho thực hiện filter
            // Ngc lại thì redirect trang đăng nhập
            AppUser userSession = (AppUser)HttpContext.Current.Session["user"];

            if (userSession != null)
            {
                QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
                //2. Check quyền: có quyền thì => cho thực hiện filter
                // Ngc lại thì redirect tới trang báo lỗi quyền truy cập

                // Check quyền của toàn bộ Nhân viên
                if((RoleID == "" || RoleID == "CUSTOMER") && userSession.RoleID != "CUSTOMER")
                    return;

                // Check admin có mọi quyền
                if (userSession.RoleID == "ADMIN")
                    return;

                // Check quyền cụ thể
                var isAuthorize = db.AppRoles.Count(r => r.RoleID == userSession.RoleID && userSession.RoleID == RoleID);

                if (isAuthorize != 0)
                    return;

                // Nếu ko đúng quyền thì trả về trang lỗi
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            Controller = "AdminAccountManager_64131011",
                            action = "NoneAuth",
                            area = "Admin",
                            returnUrl = returnUrl.ToString()
                        }));
            }
            else
            {
                if(RoleID != "CUSTOMER")
                {
                    // Nếu chưa đăng nhập thì sẽ đưa về trang đăng nhập của admin
                    var returnUrl = filterContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                Controller = "AdminAccountManager_64131011",
                                action = "Login",
                                area = "Admin",
                                returnUrl = returnUrl.ToString()
                            }));
                }
                else
                {
                    // Nếu chưa đăng nhập thì sẽ đưa về trang đăng nhập của admin
                    var returnUrl = filterContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                Controller = "AccountManager_64131011",
                                action = "Login",
                                area = "",
                                returnUrl = returnUrl.ToString()
                            }));
                } 
                    
            }
        }
    }
}