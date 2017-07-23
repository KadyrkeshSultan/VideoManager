using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoManager.Model
{
    class AccountGroup
    {
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ApplicationRights { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
