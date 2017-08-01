using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Snapshot : ISnapshot, IDisposable
    {
        private Guid AccountId;
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_Snapshot()
        {
            AccountId = Guid.Empty;
            context = new VMContext();
        }

        
        public List<Snapshot> GetSnapshots(Guid id)
        {
            return context.Snapshots.Where(snapshot => snapshot.DataFileId == id).OrderBy(snapshot => snapshot.FrameNumber).ToList();
        }

        
        public void DeleteAllSnapshots(Guid Id)
        {
            context.Snapshots.RemoveRange(context.Snapshots.Where(snapshot => snapshot.DataFileId == Id));
        }

        
        public Snapshot GetSnapshot(Guid id)
        {
            return context.Snapshots.Find(id);
        }

        
        public void Delete(Snapshot rec)
        {
            context.Snapshots.Remove(rec);
        }

        
        public void Delete(Guid Id)
        {
            Delete(context.Snapshots.Find(Id));
        }

        
        public void SaveUpdate(Snapshot rec)
        {
            if (rec.Id == Guid.Empty)
                context.Snapshots.Add(rec);
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