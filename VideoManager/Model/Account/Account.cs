using System;
using System.Collections;
using System.Collections.Generic;
using VideoManager.Enums;

namespace VideoManager.Model
{
    class Account
    {
        public Guid Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string BadgeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Rank { get; set; }
        public string LogonID { get; set; }
        public string Password { get; set; }
        public bool? IsExpires { get; set; }
        public DateTime? Expiration { get; set; }
        public bool? IsLocked { get; set; }
        public DateTime? LockedTimestamp { get; set; }
        public bool? IsPwdReset { get; set; }
        public byte[] Photo { get; set; }
        public SECURITY Security { get; set; }
        public SYSTEM_ROLE SystemRole { get; set; }
        public DateTime RecordCreated { get; set; }
        public string Memo { get; set; }
        public string PIN { get; set; }
        public int ApplicationRights { get; set; }
        public bool IsGroupOverride { get; set; }
        public string Email { get; set; }
        public bool IsAlertEmail { get; set; }
        public Guid Dept_RecId { get; set; }
        public Guid SubStation_RecId { get; set; }
        public virtual ICollection<AccountGroup> AccountGroups { get; set; }
        public virtual UserRank AccountRank_Id { get; set; }
    }
}
