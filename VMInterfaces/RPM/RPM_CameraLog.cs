using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_CameraLog : ICameraLog, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_CameraLog()
        {
            context = new VMContext();
        }

        
        public List<CameraLog> GetCameraLogByAccountIDDateRange(Guid Id, DateTime d1, DateTime d2)
        {
            return context.CameraLogs.Where(cameraLog => cameraLog.AccountID == Id && cameraLog.LogTimestamp >= d1 && cameraLog.LogTimestamp <= d2).OrderBy(cameraLog => cameraLog.LogTimestamp).ToList();
        }

        
        public List<CameraLog> GetCameraLogByAssetTagDateRange(string tag, DateTime d1, DateTime d2)
        {
            return context.CameraLogs.Where(cameraLog => cameraLog.AssetTag == tag && cameraLog.LogTimestamp >= d1 && cameraLog.LogTimestamp <= d2).OrderBy(cameraLog => cameraLog.LogTimestamp).ToList();
        }

        
        public List<CameraLog> GetCameraLogBySerialNumberDateRange(string sn, DateTime d1, DateTime d2)
        {
            return context.CameraLogs.Where(cameraLog => cameraLog.SerialNumber == sn && cameraLog.LogTimestamp >= d1 && cameraLog.LogTimestamp <= d2).OrderBy(cameraLog => cameraLog.LogTimestamp).ToList();
        }

        
        public List<CameraLog> GetCameraLogByAccountID(Guid Id)
        {
            return context.CameraLogs.Where(cameraLog => cameraLog.AccountID == Id).OrderBy(cameraLog => cameraLog.LogTimestamp).ToList();
        }

        
        public List<CameraLog> GetCameraLogByAssetTag(string tag)
        {
            return context.CameraLogs.Where(cameraLog => cameraLog.AssetTag == tag).OrderBy(cameraLog => cameraLog.LogTimestamp).ToList();
        }

        
        public List<CameraLog> GetCameraLogBySerialNumber(string sn)
        {
            return context.CameraLogs.Where(cameraLog => cameraLog.SerialNumber == sn).OrderBy(cameraLog => cameraLog.LogTimestamp).ToList();
        }

        
        public void SaveUpdate(CameraLog rec)
        {
            if (rec.Id == Guid.Empty)
                context.CameraLogs.Add(rec);
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
                            Msg += string.Format("RPM_CameraLog_unknown1", validationError.PropertyName, validationError.ErrorMessage);
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
