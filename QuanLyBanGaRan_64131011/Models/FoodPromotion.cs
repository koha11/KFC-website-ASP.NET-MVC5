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
    
    public partial class FoodPromotion
    {
        public string PromotionID { get; set; }
        public string FoodID { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
    
        public virtual Food Food { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
