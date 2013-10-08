
using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Data;
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace NkjSoft.Core.Data.Configurations.Account
{
    public partial class MembershipsConfiguration
    {
        partial void MembershipsConfigurationAppend()
        {
            this.HasKey(t => t.UserId);
            this.ToTable("Memberships");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Password).HasColumnName("Password").IsRequired().HasMaxLength(128);
            this.Property(t => t.PasswordFormat).HasColumnName("PasswordFormat");
            this.Property(t => t.PasswordSalt).HasColumnName("PasswordSalt").IsRequired().HasMaxLength(128);
            this.Property(t => t.Email).HasColumnName("Email").HasMaxLength(256);
            this.Property(t => t.PasswordQuestion).HasColumnName("PasswordQuestion").HasMaxLength(256);
            this.Property(t => t.PasswordAnswer).HasColumnName("PasswordAnswer").HasMaxLength(128);
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.IsLockedOut).HasColumnName("IsLockedOut");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.LastPasswordChangedDate).HasColumnName("LastPasswordChangedDate");
            this.Property(t => t.LastLockoutDate).HasColumnName("LastLockoutDate");
            this.Property(t => t.FailedPasswordAttemptCount).HasColumnName("FailedPasswordAttemptCount");
            this.Property(t => t.FailedPasswordAttemptWindowStart).HasColumnName("FailedPasswordAttemptWindowStart");
            this.Property(t => t.FailedPasswordAnswerAttemptCount).HasColumnName("FailedPasswordAnswerAttemptCount");
            this.Property(t => t.FailedPasswordAnswerAttemptWindowsStart).HasColumnName("FailedPasswordAnswerAttemptWindowsStart");
            this.Property(t => t.Comment).HasColumnName("Comment").HasMaxLength(256);
            this.HasRequired(t => t.Applications).WithMany(t => t.Memberships).HasForeignKey(d => d.ApplicationId);
            this.HasRequired(t => t.Users).WithOptional(t => t.Memberships);
        }
    }
}
