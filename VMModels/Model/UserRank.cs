using System;
using System.Collections.Generic;

namespace VMModels.Model
{
    public class UserRank
    {
        public Guid Id { get; set; }
        public string Rank { get; set; }
        public ICollection<Account> Accounts { get; set; }

        public UserRank()
        {
            Accounts = new HashSet<Account>();
        }
    }
}
