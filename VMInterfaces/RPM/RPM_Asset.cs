using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Asset : IAssets, IDisposable
    {
        private VMContext context;
        public string Msg { get; set; }

        public RPM_Asset()
        {
            context = new VMContext();
        }

        public void DeleteManufacturer(Manufacturer rec)
        {
            context.Manufacturers.Remove(rec);
        }

        public void DeleteManufacturer(Guid Id)
        {
            DeleteManufacturer(context.Manufacturers.Find(Id));
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Guid GetAccountByTrackingID(string Id)
        {
            Guid guid = Guid.Empty;
            if (context.Inventories.Where(inventory => inventory.TrackingID == Id).FirstOrDefault()!= null)
                guid = context.Inventories.Where(inventory => inventory.TrackingID == Id).FirstOrDefault().AccountID;
            return guid;
        }

        public Manufacturer GetManufacturer(Guid id)
        {
            return context.Manufacturers.Find(id);
        }

        public List<Manufacturer> GetManufacturerList()
        {
            return context.Manufacturers.OrderBy(manufacturer => manufacturer.Name).ToList();
        }

        public void InsertUpdateManufacturer(Manufacturer rec)
        {
            if (rec.Id == Guid.Empty)
                context.Manufacturers.Add(rec);
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
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("Property: {0} Error: {1}\n", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }
    }
}
