using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface ICamera : IDisposable
    {
        List<CameraFolder> GetFolderList();

        void DeleteFolder(CameraFolder rec);

        void DeleteFolder(Guid Id);

        CameraFolder Find(Guid Id);

        void SaveUpdate(CameraFolder rec);

        bool Save();
    }
}
