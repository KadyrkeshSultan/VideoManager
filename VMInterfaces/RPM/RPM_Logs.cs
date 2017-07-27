using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Logs : ILogs, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_Logs()
        {
            context = new VMContext();
        }

        
        public List<string> GetActionCodes()
        {
            return context.AccountLogs.OrderBy(accountLog => accountLog.Action).Select(accountLog => accountLog.Action).Distinct().ToList();
        }

        
        public List<AccountLog> GetAccountLogsByID_Date(Guid Id, DateTime d1, DateTime d2, string Action)
        {
            if (string.IsNullOrEmpty(Action))
                return context.AccountLogs.Where(accountLog => accountLog.AccountId == Id && accountLog.LogTimestamp >= d1 && accountLog.LogTimestamp <= d2).OrderBy(accountLog => accountLog.LogTimestamp).ToList();
            return context.AccountLogs.Where(accountLog => accountLog.AccountId == Id && accountLog.Action == Action && accountLog.LogTimestamp >= d1 && accountLog.LogTimestamp <= d2).OrderBy(accountLog => accountLog.LogTimestamp).ToList();
        }

        
        public void LogSystem(SystemLog log)
        {
            try
            {
                log.DomainName = VMGlobal.DomainName;
                log.IPAddress = VMGlobal.IPAddress;
                log.LogTimestamp = new DateTime?(DateTime.Now);
                log.MachineAccount = VMGlobal.MachineAccount;
                log.MachineID = VMGlobal.MachineID;
                log.MachineName = VMGlobal.MachineName;
                context.SystemLogs.Add(log);
                Save();
            }
            catch
            {
            }
        }

        
        public void LogAccount(AccountLog log)
        {
            log.DomainName = VMGlobal.DomainName;
            log.IPAddress = VMGlobal.IPAddress;
            log.LogTimestamp = new DateTime?(DateTime.Now);
            log.MachineAccount = VMGlobal.MachineAccount;
            log.MachineID = VMGlobal.MachineID;
            log.MachineName = VMGlobal.MachineName;
            if (VMGlobal.AccountRecord != null)
            {
                log.AccountId = new Guid?(VMGlobal.AccountRecord.Id);
                log.AccountName = VMGlobal.AccountRecord.ToString();
                log.BadgeNumber = VMGlobal.AccountRecord.BadgeNumber;
            }
            else
            {
                log.AccountId = new Guid?(Guid.Empty);
                log.AccountName = "RPM_Logs_unknown1";
                log.BadgeNumber = "RPM_Logs_unknown2";
            }
            try
            {
                context.AccountLogs.Add(log);
                Save();
            }
            catch
            {
            }
        }

        
        public void LogCamera(CameraLog log)
        {
            log.DomainName = VMGlobal.DomainName;
            log.IPAddress = VMGlobal.IPAddress;
            log.LogTimestamp = new DateTime?(DateTime.Now);
            log.MachineAccount = VMGlobal.MachineAccount;
            log.MachineID = VMGlobal.MachineID;
            log.MachineName = VMGlobal.MachineName;
            try
            {
                context.CameraLogs.Add(log);
                Save();
            }
            catch
            {
            }
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
                    Msg += (string)(object)ex.InnerException;
                    contextTransaction.Rollback();
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("RPM_Logs_unknown3", validationError.PropertyName, validationError.ErrorMessage);
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
