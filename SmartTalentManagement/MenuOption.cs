//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartTalentManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class MenuOption
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuOption()
        {
            this.MenuOptionNodes = new HashSet<MenuOptionNode>();
        }
    
        public int MenuId { get; set; }
        public string MenuDescription { get; set; }
        public string ControlName { get; set; }
        public int MenuHierarchy { get; set; }
        public bool ShowMenuItemInApp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuOptionNode> MenuOptionNodes { get; set; }
    }
}
