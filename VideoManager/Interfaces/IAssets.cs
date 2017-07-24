using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IAssets : IDisposable
    {
        List<Manufacturer> GetManufacturerList();

        Manufacturer GetManufacturer(Guid id);

        void DeleteManufacturer(Guid Id);

        void DeleteManufacturer(Manufacturer rec);

        Guid GetAccountByTrackingID(string Id);

        void InsertUpdateManufacturer(Manufacturer rec);

        bool Save();
    }
}
