using QuanLyBanGaRan_64131011.App_Start;
using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Controllers
{
    public class AdminHome_64131011Controller : Controller
    {
        private QuanLyBanGaRan_64131011Entities db = new QuanLyBanGaRan_64131011Entities();
        // GET: Admin/AdminHome_64131011
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult Index()
        {
            ViewBag.header = "Tổng quan";

            var orders = db.CustomerOrders.Where(o => o.OrderDate.Value.Day == DateTime.Now.Day 
            && o.OrderDate.Value.Month == DateTime.Now.Month
            && o.OrderDate.Value.Year == DateTime.Now.Year);

            ViewBag.OrderAmount = orders.Count();
            ViewBag.AcceptOrderAmount = orders.Where(o => o.Status == 1).Count();
            ViewBag.ShipOrderAmount = orders.Where(o => o.Status == 2).Count();
            ViewBag.OrderTotals = orders.Where(o => o.Status == 3).Sum(o => o.TotalPrice) ?? 0;

            return View();
        }
        [RoleAuthorize_64131011(RoleID = "")]
        public ActionResult GetStatisticsData()
        {
            int today = DateTime.Now.Day;

            List<string> dailyDay = new List<string>();

            for (int i = 1; i <= today; i++)
            {
                string day;
                if (i < 10)
                    day = "0" + i.ToString();
                else
                    day = i.ToString();

                dailyDay.Add(day);
            }

            var dailyTotals = Enumerable.Range(1, today) // Tạo danh sách từ ngày 1 đến ngày hiện tại
                .Select(day => db.CustomerOrders
                    .Where(o => o.OrderDate.Value.Day == day && o.OrderDate.Value.Month == DateTime.Now.Month && o.OrderDate.Value.Year == DateTime.Now.Year)
                    .Sum(o => (decimal?)o.TotalPrice) ?? 0) // Nếu không có dữ liệu, trả về 0
                .ToList();

            int thisMonth = DateTime.Now.Month;

            // Tạo danh sách từ 1 đến ngày hiện tại
            List<string> dailyMonth = new List<string>();

            for (int i = 1; i <= thisMonth; i++)
            {
                string month = "Tháng " + i.ToString();
                dailyMonth.Add(month);
            }

            var monthlyTotals = Enumerable.Range(1, thisMonth) // Tạo danh sách từ ngày 1 đến ngày hiện tại
                .Select(month => db.CustomerOrders
                    .Where(o => o.OrderDate.Value.Month == month && o.OrderDate.Value.Year == DateTime.Now.Year)
                    .Sum(o => (decimal?)o.TotalPrice) ?? 0) // Nếu không có dữ liệu, trả về 0
                .ToList();

            var startYear = db.CustomerOrders.Min(o => o.OrderDate).Value.Year;
            var thisYear = DateTime.Now.Year;

            List<string> dailyYear = new List<string>();

            for (int i = startYear; i <= thisYear; i++)
            {
                string year = i.ToString();
                dailyYear.Add(year);
            }

            var yearlyTotals = Enumerable.Range(startYear, thisYear) // Tạo danh sách từ ngày 1 đến ngày hiện tại
                .Select(year => db.CustomerOrders
                    .Where(o => o.OrderDate.Value.Year == year)
                    .Sum(o => (decimal?)o.TotalPrice) ?? 0) // Nếu không có dữ liệu, trả về 0
                .ToList();

            var data = new
            {
                daily = new
                {
                    dailyDay,
                    dailyTotals
                },
                monthly = new
                {
                    dailyMonth,
                    monthlyTotals
                },
                yearly = new
                {
                    dailyYear,
                    yearlyTotals
                }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}