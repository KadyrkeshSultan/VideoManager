using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IUserRank : IDisposable
    {
        List<UserRank> GetRankList();

        void Delete(Guid Id);

        void Delete(UserRank rec);

        UserRank GetRank(Guid Id);

        void InsertUpdate(UserRank rec);

        bool Save();
    }
}
