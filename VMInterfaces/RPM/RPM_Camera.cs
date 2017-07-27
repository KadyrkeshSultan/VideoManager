using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Camera : ICamera, IDisposable
    {
        private VMContext context;

        public string Msg { get; set; }

        public RPM_Camera()
        {
            context = new VMContext();
        }

        public void DeleteFolder(Guid Id)
        {
            DeleteFolder(context.CameraFolders.Find(Id));
        }

        public void DeleteFolder(CameraFolder rec)
        {
            context.CameraFolders.Remove(rec);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public CameraFolder Find(Guid Id)
        {
            return context.CameraFolders.Find(Id);
        }

        public List<CameraFolder> GetFolderList()
        {
            return context.CameraFolders.OrderBy(cameraFolder => cameraFolder.Folder).ToList();
        }

        public bool Save()
        {
            bool flag = false;
            using (DbContextTransaction contextTransaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    this.context.SaveChanges();
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
                            Msg += string.Format("RMP_Camera_unknown1", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }

        public void SaveUpdate(CameraFolder rec)
        {
            if (rec.Id == Guid.Empty)
                context.CameraFolders.Add(rec);
            else
                context.Entry(rec).State = EntityState.Modified;
        }
    }
}
