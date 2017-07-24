using System;
using System.Collections.Generic;

namespace VMModels
{
    class UserRank
    {
        public Guid Id { get; set; }
        public string Rank { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
