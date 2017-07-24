using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface ICameraLog : IDisposable
    {
        List<CameraLog> GetCameraLogBySerialNumber(string sn);

        List<CameraLog> GetCameraLogByAssetTag(string tag);

        List<CameraLog> GetCameraLogByAccountID(Guid Id);

        List<CameraLog> GetCameraLogBySerialNumberDateRange(string sn, DateTime d1, DateTime d2);

        List<CameraLog> GetCameraLogByAssetTagDateRange(string tag, DateTime d1, DateTime d2);

        List<CameraLog> GetCameraLogByAccountIDDateRange(Guid Id, DateTime d1, DateTime d2);

        void SaveUpdate(CameraLog rec);

        bool Save();
    }
}
