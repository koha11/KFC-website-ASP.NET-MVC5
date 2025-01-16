using QuanLyBanGaRan_64131011.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace QuanLyBanGaRan_64131011.Areas.Admin.Data
{
    public class OrderDetailViewModel_64131011
    {
        public string OrderID { get; set; }
        public string OrderedBy { get; set; }
        public string CName { get; set; }
        public string AcceptedBy { get; set; }
        public string AName { get; set; }
        public string ShippedBy { get; set; }
        public string SName { get; set; }
        public string OrderDate { get; set; }
        public string AcceptDate { get; set; }
        public string ShipmentDate { get; set; }
        public string FinishDate { get; set; }
        public string Address { get; set; }
        public byte Status { get; set; }
        public string VoucherID { get; set; }
        public decimal TotalPrice { get; set; }
    }
}