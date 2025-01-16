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
    
    public partial class Food
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Food()
        {
            this.FoodPromotions = new HashSet<FoodPromotion>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public string FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodDetails { get; set; }
        public decimal FoodPrice { get; set; }
        public string FoodUnits { get; set; }
        public string FoodImage { get; set; }
        public string FoodCategoryID { get; set; }
    
        public virtual FoodCategory FoodCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FoodPromotion> FoodPromotions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
