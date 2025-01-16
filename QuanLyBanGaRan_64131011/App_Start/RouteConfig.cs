using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyBanGaRan_64131011
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //// Đăng ký các route cho Area
            //routes.MapMvcAttributeRoutes(); // Để hỗ trợ các attribute routes nếu có

            //// Các route cho Area cần được đăng ký
            //AreaRegistration.RegisterAllAreas();  // Phải gọi dòng này để đăng ký tất cả các area

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "MenuRoute",
                url: "Menu/{action}/{id}",
                defaults: new { controller = "Menu_6413011", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "UserPanelRoute",
                url: "User/{action}/{id}",
                defaults: new { controller = "UserPanel_64131011", action = "AccountDetails", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CartRoute",
                url: "Cart/{action}/{id}",
                defaults: new { controller = "CartManager_64131011", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "RecoverRoute",
                url: "RecoverPassword",
                defaults: new { controller = "AccountManager_64131011", action = "RecoverPassword" }
            );

            routes.MapRoute(
                name: "LoginRoute",
                url: "login",
                defaults: new { controller = "AccountManager_64131011", action = "Login" }
            );

            routes.MapRoute(
                name: "RegisterRoute",
                url: "register",
                defaults: new { controller = "AccountManager_64131011", action = "Register" }
            );

            routes.MapRoute(
                name: "LogoutRoute",
                url: "logout",
                defaults: new { controller = "AccountManager_64131011", action = "Logout" }
            );

            routes.MapRoute(
                name: "HomeRoute",
                url: "",
                defaults: new { controller = "Home_64131011", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home_64131011", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
