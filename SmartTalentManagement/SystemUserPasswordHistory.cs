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
    
    public partial class SystemUserPasswordHistory
    {
        public int SystemUserPasswordHistoryId { get; set; }
        public int SystemUserId { get; set; }
        public string PasswordHash { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual SystemUser SystemUser { get; set; }
    }
}
