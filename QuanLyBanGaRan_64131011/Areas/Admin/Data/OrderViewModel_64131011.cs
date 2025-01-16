using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanGaRan_64131011.Models
{
    public class OrderViewModel_64131011
    {
        public string OrderID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public byte Status { get; set; }
        public string OrderedBy { get; set; }
        public string AcceptedBy { get; set; }
        public string ShippedBy { get; set; }

        public int? CountFood { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }

    }
}