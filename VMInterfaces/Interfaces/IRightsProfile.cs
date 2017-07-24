using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface IRightsProfile : IDisposable
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
