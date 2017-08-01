using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_FileType : IFileType, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_FileType()
        {
            this.context = new VMContext();
        }

        
        public FileType Find(Guid Id)
        {
            return context.FileTypes.Find(Id);
        }

        
        public List<FileType> GetFileTypeList()
        {
            return context.FileTypes.OrderBy(fileType => fileType.FileExt).ToList();
        }

        
        public bool IsInternal(string ext)
        {
            try
            {
                return context.FileTypes.First(fileType => fileType.FileExt == ext).IsInternal;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        
        public void InsertUpdate(FileType rec)
        {
            if (rec.Id == Guid.Empty)
                context.FileTypes.Add(rec);
            else
                context.Entry(rec).State = EntityState.Modified;
        }

        
        public void Delete(Guid Id)
        {
            Delete(Find(Id));
        }

        
        public void Delete(FileType rec)
        {
            context.FileTypes.Remove(rec);
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
