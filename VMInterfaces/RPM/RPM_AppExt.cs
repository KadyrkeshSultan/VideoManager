using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_AppExt : IAppExt, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_AppExt()
        {
            context = new VMContext();
        }

        
        public List<AppExt> GetList()
        {
            return context.AppExts.OrderBy(appExt => appExt.Ext).ToList();
        }

        
        public void Delete(Guid Id)
        {
            Delete(context.AppExts.Find(Id));
        }

        
        public void Delete(AppExt rec)
        {
            context.AppExts.Remove(rec);
        }

        
        public AppExt Find(Guid Id)
        {
            return context.AppExts.Find(Id);
        }

        
        public void InsertUpdate(AppExt rec)
        {
            if (rec.Id == Guid.Empty)
                context.AppExts.Add(rec);
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
                    string message = ex.Message;
                    ex.InnerException.ToString();
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
