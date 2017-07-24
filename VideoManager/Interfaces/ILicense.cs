using System;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface ILicense : IDisposable
    {
        int GetLicenseCount();

        string GetLicenseData();

        void CreateDefaultLicense();

        bool UpdateLicense(string data);

        string GetRequestKey(int Count);

        void SaveUpdate(License rec);

        bool Save();
    }
}
