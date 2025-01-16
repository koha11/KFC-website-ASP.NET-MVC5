using System;
using System.Web;

namespace QuanLyBanGaRan_64131011.Models
{
    public class CartViewModel_64131011
    {
        public string FoodImage { get; set; }
        public string FoodID { get; set; }
        public string FoodName { get; set; }
        public byte Amount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
