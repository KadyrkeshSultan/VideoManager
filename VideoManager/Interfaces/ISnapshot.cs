using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface ISnapshot : IDisposable
    {
        List<Snapshot> GetSnapshots(Guid id);

        void Delete(Snapshot rec);

        void Delete(Guid Id);

        Snapshot GetSnapshot(Guid id);

        void DeleteAllSnapshots(Guid Id);

        void SaveUpdate(Snapshot rec);

        bool Save();
    }
}
