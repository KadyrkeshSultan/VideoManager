using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IAsset : IDisposable
    {
        List<Manufacturer> GetManufacturerList();

        Manufacturer GetManufacturer(Guid Id);

        void Delete(Guid Id);

        void Delete(Manufacturer rec);

        void InsertUpdate(Manufacturer rec);

        bool Save();
    }
}
