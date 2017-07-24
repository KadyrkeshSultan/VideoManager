using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IAppExt : IDisposable
    {
        List<AppExt> GetList();

        void Delete(AppExt rec);

        void Delete(Guid Id);

        AppExt Find(Guid Id);

        void InsertUpdate(AppExt rec);

        bool Save();
    }
}
