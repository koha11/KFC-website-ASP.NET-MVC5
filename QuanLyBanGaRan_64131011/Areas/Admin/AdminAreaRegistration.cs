using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyBanGaRan_64131011.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            foreach (var route in RouteTable.Routes)
            {
                Debug.WriteLine(route);
            }

            // admin/customer           
            context.MapRoute(
                "CustomerManager",
                "Admin/Customer/{action}/{id}",
                new { controller = "CustomerManager_64131011", action = "Index", id = UrlParameter.Optional }
            );

            // admin/employee           
            context.MapRoute(
                "EmployeeManager",
                "Admin/Employee/{action}/{id}",
                new { controller = "EmployeeManager_64131011", action = "Index", id = UrlParameter.Optional }
            );

            // admin/food           
            context.MapRoute(
                "FoodManager",
                "Admin/Food/{action}/{id}",
                new { controller = "FoodManager_64131011", action = "Index", id = UrlParameter.Optional }
            );

            // admin/promotion           
            context.MapRoute(
                "PromotionManager",
                "Admin/Promotion/{action}/{id}",
                new { controller = "PromotionManager_64131011", action = "Index", id = UrlParameter.Optional }
            );

            // admin/Voucher           
            //context.MapRoute(
            //    "VoucherManager",
            //    "Admin/Voucher/{action}/{id}",
            //    new { controller = "VoucherManager_64131011", action = "Index", id = UrlParameter.Optional }
            //);

            // admin/Order           
            context.MapRoute(
                "OrderManager",
                "Admin/Order/{action}/{id}",
                new { controller = "OrderManager_64131011", action = "Index", id = UrlParameter.Optional }
            );

            // admin/
            context.MapRoute(
                "AdminGet",
                "Admin/GetStatisticsData",
                new { controller = "AdminHome_64131011", action = "GetStatisticsData" }
            );

            // admin/
            context.MapRoute(
                "AdminHome",
                "Admin/",
                new { controller = "AdminHome_64131011", action = "Index" }
            );

            context.MapRoute(
               name: "AdminNoneAuthRoute",
               url: "Admin/NoneAuth",
               defaults: new { controller = "AdminAccountManager_64131011", action = "NoneAuth" }
            );

            context.MapRoute(
               name: "AdminLogutRoute",
               url: "Admin/Logout",
               defaults: new { controller = "AdminAccountManager_64131011", action = "Logout" }
            );

            context.MapRoute(
               name: "AdminLoginRoute",
               url: "Admin/Login",
               defaults: new { controller = "AdminAccountManager_64131011", action = "Login" }
            );

            // Default
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "AdminHome_64131011", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}