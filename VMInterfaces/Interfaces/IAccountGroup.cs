using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface IAccountGroup : IDisposable
    {
        List<AccountGroup> GetAccountGroupList();

        AccountGroup GetAccountGroup(Guid Id);

        void InsertUpdate(AccountGroup rec);

        void DeleteGroup(AccountGroup rec);

        void DeleteGroup(Guid Id);

        bool Save();
    }
}
