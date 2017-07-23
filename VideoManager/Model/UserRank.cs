using System;
using System.Collections.Generic;

namespace VideoManager.Model
{
    class UserRank
    {
        public Guid Id { get; set; }
        public string Rank { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
