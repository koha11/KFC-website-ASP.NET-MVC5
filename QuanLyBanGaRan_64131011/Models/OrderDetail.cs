//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyBanGaRan_64131011.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public string OrderID { get; set; }
        public string FoodID { get; set; }
        public byte Amount { get; set; }
    
        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Food Food { get; set; }
    }
}
