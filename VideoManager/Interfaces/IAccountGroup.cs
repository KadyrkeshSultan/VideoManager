using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IAccountGroup : IDisposable
    {
        List<AccountGroup> GetAccountGroupList();

        AccountGroup GetAccountGroup(Guid Id);

        void InsertUpdate(AccountGroup rec);

        void DeleteGroup(AccountGroup rec);

        void DeleteGroup(Guid Id);

        bool Save();
    }
}
