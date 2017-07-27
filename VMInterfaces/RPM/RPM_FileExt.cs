using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_FileExt : IFileExt, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_FileExt()
        {
            context = new VMContext();
        }

        
        public List<FileExt> GetFileExtensions()
        {
            return context.FileExts.OrderBy(fileExt => fileExt.Ext).ToList();
        }

        
        public void Delete(Guid Id)
        {
            Delete(context.FileExts.Find(Id));
        }

        
        public void Delete(FileExt rec)
        {
            context.FileExts.Remove(rec);
        }

        
        public void InsertUpdate(FileExt rec)
        {
            if (rec.Id == Guid.Empty)
                context.FileExts.Add(rec);
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
                            Msg += string.Format("RPM_FileExt_unknown1", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }

        
        public void Dispose()
        {
            context.Dispose();
        }

        
        public FileExt Find(Guid Id)
        {
            return this.context.FileExts.Find((object)Id);
        }
    }
}
