using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Manufacturer : IAsset, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_Manufacturer()
        {
            context = new VMContext();
        }

        
        public List<Manufacturer> GetManufacturerList()
        {
            return context.Manufacturers.OrderBy(manufacturer => manufacturer.Name).ToList();
        }

        
        public Manufacturer GetManufacturer(Guid Id)
        {
            return context.Manufacturers.Find(Id);
        }

        
        public void Delete(Guid Id)
        {
            Delete(GetManufacturer(Id));
        }

        
        public void Delete(Manufacturer rec)
        {
            context.Manufacturers.Remove(rec);
        }

        
        public void InsertUpdate(Manufacturer rec)
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
                    Msg += (string)(object)ex.InnerException;
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

        
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
