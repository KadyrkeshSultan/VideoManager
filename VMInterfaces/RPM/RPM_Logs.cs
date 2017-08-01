using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Logs : ILogs, IDisposable
    {
        private VMContext context = new VMContext();

        public string Msg
        {
            get;
            set;
        }

        public RPM_Logs()
        {
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public List<AccountLog> GetAccountLogsByID_Date(Guid Id, DateTime d1, DateTime d2, string Action)
        {
            if (string.IsNullOrEmpty(Action))
            {
                IOrderedQueryable<AccountLog> accountLogs =
                    from c in context.AccountLogs
                    where (c.AccountId == Id) && (c.LogTimestamp >= d1) && (c.LogTimestamp <= d2)
                    orderby c.LogTimestamp
                    select c;
                return accountLogs.ToList();
            }
            IOrderedQueryable<AccountLog> accountLogs1 =
                from c in context.AccountLogs
                where (c.AccountId == Id) && (c.Action == Action) && (c.LogTimestamp >= d1) && (c.LogTimestamp <= d2)
                orderby c.LogTimestamp
                select c;
            return accountLogs1.ToList();
        }

        public List<string> GetActionCodes()
        {
            IQueryable<string> strs = (
                from c in context.AccountLogs
                orderby c.Action
                select c.Action).Distinct();
            return strs.ToList();
        }

        public void LogAccount(AccountLog log)
        {
            log.DomainName = VMGlobal.DomainName;
            log.IPAddress = VMGlobal.IPAddress;
            log.LogTimestamp = new DateTime?(DateTime.Now);
            log.MachineAccount = VMGlobal.MachineAccount;
            log.MachineID = VMGlobal.MachineID;
            log.MachineName = VMGlobal.MachineName;
            if (VMGlobal.AccountRecord == null)
            {
                log.AccountId = new Guid?(Guid.Empty);
                log.AccountName = "n/a";
                log.BadgeNumber = "n/a";
            }
            else
            {
                log.AccountId = new Guid?(VMGlobal.AccountRecord.Id);
                log.AccountName = VMGlobal.AccountRecord.ToString();
                log.BadgeNumber = VMGlobal.AccountRecord.BadgeNumber;
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

        public bool Save()
        {
            bool flag = false;
            using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                    flag = true;
                }
                catch (DbUpdateException dbUpdateException1)
                {
                    DbUpdateException dbUpdateException = dbUpdateException1;
                    RPM_Logs rPMLog = this;
                    rPMLog.Msg = string.Concat(rPMLog.Msg, dbUpdateException.InnerException);
                    dbContextTransaction.Rollback();
                }
                catch (DbEntityValidationException dbEntityValidationException1)
                {
                    DbEntityValidationException dbEntityValidationException = dbEntityValidationException1;
                    dbContextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in dbEntityValidationException.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                        {
                            RPM_Logs rPMLog1 = this;
                            rPMLog1.Msg = string.Concat(rPMLog1.Msg, string.Format("Property: {0} Error: {1}\n", validationError.PropertyName, validationError.ErrorMessage));
                        }
                    }
                }
            }
            return flag;
        }
    }
}