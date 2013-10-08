//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NkjSoft.Core.Models.Account
{
    using NkjSoft.Core.Models.Security;
    using NkjSoft.Framework.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Applications : EntityBase<Guid>
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
