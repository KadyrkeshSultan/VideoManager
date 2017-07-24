using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface IAsset : IDisposable
    {
        List<Manufacturer> GetManufacturerList();

        Manufacturer GetManufacturer(Guid Id);

        void Delete(Guid Id);

        void Delete(Manufacturer rec);

        void InsertUpdate(Manufacturer rec);

        bool Save();
    }
}
