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
    
    public partial class AppRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppRole()
        {
            this.AppUsers = new HashSet<AppUser>();
        }
    
        public string RoleID { get; set; }
        public string RoleName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}
