//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NkjSoft.Models.T4
{
    #pragma warning disable 1573
    using System;
    using System.Collections.Generic;
    
    public partial class Applications
    {
        public Applications()
        {
            this.Memberships = new HashSet<Memberships>();
            this.Roles = new HashSet<Roles>();
            this.Users = new HashSet<Users>();
        }
    
        public string ApplicationName { get; set; }
        public System.Guid ApplicationId { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Memberships> Memberships { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
