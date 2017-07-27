using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Enums;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Product : IProduct, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_Product()
        {
            context = new VMContext();
        }

        
        public bool IsAssetTag(string tag)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(tag))
            {
                if (context.Inventories.Where(inventory => inventory.AssetTag == tag).Select(inventory => inventory.AssetTag).Count() > 0)
                    flag = true;
            }
            return flag;
        }

        
        public Inventory IsDuplicateAssetTag(string tag)
        {
            Inventory inventory1 = null;
            if (!string.IsNullOrEmpty(tag))
                inventory1 = context.Inventories.Where(inventory => inventory.AssetTag == tag).FirstOrDefault();
            return inventory1;
        }

        
        public Guid IsAssetTag(string tag, Guid RecId)
        {
            Guid guid = Guid.Empty;
            if (!string.IsNullOrEmpty(tag))
            {
                if (RecId != Guid.Empty)
                    guid = context.Inventories.Where(inventory => inventory.AssetTag == tag && inventory.Id != RecId).Select(inventory => inventory.Id).FirstOrDefault();
                else
                    guid = context.Inventories.Where(inventory => inventory.AssetTag == tag).Select(inventory => inventory.Id).FirstOrDefault();
            }
            return guid;
        }

        
        public List<Product> GetProductList(Guid Id)
        {
            return context.Products.OrderBy(product => product.Name).Where(product => product.Manufacturer.Id == Id).ToList();
        }

        
        public int GetInventoryCount()
        {
            return context.Inventories.Select(inventory => inventory).Count();
        }

        
        public Product GetProduct(Guid Id)
        {
            return context.Products.Find(Id);
        }

        
        public void DeleteProduct(Guid Id)
        {
            DeleteProduct(GetProduct(Id));
        }

        
        public void DeleteProduct(Product rec)
        {
            context.Products.Remove(rec);
        }

        
        public Account GetAccount(Guid Id)
        {
            return context.Accounts.Find(Id);
        }

        
        public void InsertUpdate(Product rec)
        {
            if (rec.Id == Guid.Empty)
                context.Products.Add(rec);
            else
                context.Entry(rec).State = EntityState.Modified;
        }

        
        public List<Inventory> GetInventoryByAccount(Guid Id)
        {
            return context.Inventories.OrderByDescending(inventory => inventory.DateAssigned).Where(inventory => inventory.AccountID == Id).ToList();
        }

        
        public List<InventoryLog> GetLogListByID(Guid id)
        {
            return context.InventoryLogs.OrderByDescending(inventoryLog => inventoryLog.Timestamp).Where(inventoryLog => inventoryLog.InvIdx == id).ToList();
        }

        
        public List<Inventory> GetInventoryList(Guid Id)
        {
            return context.Inventories.OrderBy(inventory => inventory.PurchaseDate).Where(inventory => inventory.Product.Id == Id).ToList();
        }

        
        public List<Inventory> GetUnassignedList(Guid Id)
        {
            return context.Inventories.OrderByDescending(inventory => inventory.PurchaseDate).Where(inventory => inventory.Product.Id == Id && inventory.AccountID == Guid.Empty).ToList();
        }

        
        public void LogAsset(InventoryLog rec)
        {
            rec.Timestamp = DateTime.Now;
            context.InventoryLogs.Add(rec);
            Save();
        }

        
        public Inventory GetInventory(Guid Id)
        {
            return context.Inventories.Find(Id);
        }

        
        public void DeleteInventory(Guid Id)
        {
            DeleteInventory(GetInventory(Id));
        }

        
        public void LogAsset(INV_ACTION action, Inventory rec)
        {
            this.LogAsset(new InventoryLog()
            {
                Action = action,
                AssetTag = rec.AssetTag,
                AccountID = rec.AccountID,
                Battery = 0,
                DiskFree = 0,
                FileCount = 0,
                InvIdx = rec.Id,
                SerialNumber = rec.SerialNumber
            });
        }

        
        public void DeleteInventory(Inventory rec)
        {
            context.Inventories.Remove(rec);
            LogAsset(INV_ACTION.DELETE, rec);
        }

        
        public void InsertUpdate(Inventory rec)
        {
            if (rec.Id == Guid.Empty)
            {
                context.Inventories.Add(rec);
                LogAsset(INV_ACTION.ADD, rec);
            }
            else
                context.Entry(rec).State = EntityState.Modified;
        }

        
        public bool Save()
        {
            bool flag = false;
            using (DbContextTransaction contextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SaveChanges();
                    contextTransaction.Commit();
                    flag = true;
                }
                catch (DbUpdateException ex)
                {
                    contextTransaction.Rollback();
                    Msg += (string)(object)ex.InnerException;
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("RPM_Product_unknown1", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }

        
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
