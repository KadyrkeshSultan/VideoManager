using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IRightsProfile : IDisposable
    {
        List<RightsProfile> GetProfileList();

        RightsProfile GetProfile(Guid Id);

        int GetRights(Guid Id);

        void DeleteProfile(Guid Id);

        void DeleteProfile(RightsProfile rec);

        void InsertUpdate(RightsProfile rec);

        bool Save();
    }
}
