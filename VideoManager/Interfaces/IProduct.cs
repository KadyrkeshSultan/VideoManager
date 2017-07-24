using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IProduct : IDisposable
    {
        List<Product> GetProductList(Guid Id);

        Product GetProduct(Guid Id);

        void DeleteProduct(Guid Id);

        void DeleteProduct(Product rec);

        void InsertUpdate(Product rec);

        Inventory IsDuplicateAssetTag(string tag);

        Account GetAccount(Guid Id);

        int GetInventoryCount();

        List<Inventory> GetInventoryList(Guid Id);

        List<Inventory> GetInventoryByAccount(Guid Id);

        Inventory GetInventory(Guid Id);

        List<Inventory> GetUnassignedList(Guid Id);

        void DeleteInventory(Guid Id);

        void DeleteInventory(Inventory rec);

        void InsertUpdate(Inventory rec);

        List<InventoryLog> GetLogListByID(Guid id);

        void LogAsset(InventoryLog rec);

        void LogAsset(INV_ACTION action, Inventory rec);

        bool IsAssetTag(string tag);

        Guid IsAssetTag(string tag, Guid RecId);

        bool Save();
    }
}
