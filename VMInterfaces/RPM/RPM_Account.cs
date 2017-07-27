using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Account : IAccount, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_Account()
        {
            context = new VMContext();
        }

        
        private void WriteLog(VMGlobal.LOG_ACTION action, string memo)
        {
            try
            {
                AccountLog log = new AccountLog();
                log.Action = action.ToString();
                log.Memo = memo;
                using (RPM_Logs rpmLogs = new RPM_Logs())
                    rpmLogs.LogAccount(log);
            }
            catch
            {
            }
        }

        
        public void ClearGroups(Guid Id)
        {
            try
            {
                context.Database.ExecuteSqlCommand(string.Format("RMP_Account_unknown1", Id));
                WriteLog(VMGlobal.LOG_ACTION.DELETE, "RMP_Account_unknown2");
            }
            catch
            {
            }
        }

        
        public bool UpdatePassword(Guid Id, string OldPwd, string NewPwd)
        {
            bool flag = false;
            try
            {
                Account account = GetAccount(Id);
                if (account.Password.Equals(OldPwd))
                {
                    account.Password = NewPwd;
                    InsertUpdate(account);
                    Save();
                    flag = true;
                    WriteLog(VMGlobal.LOG_ACTION.PASSWORD, "RMP_Account_unknown3");
                }
            }
            catch
            {
            }
            return flag;
        }

        
        public bool UpdatePIN(Guid Id, string OldPIN, string NewPIN)
        {
            bool flag = false;
            try
            {
                Account account = GetAccount(Id);
                if (account.PIN.Equals(OldPIN))
                {
                    account.PIN = NewPIN;
                    InsertUpdate(account);
                    Save();
                    flag = true;
                    WriteLog(VMGlobal.LOG_ACTION.UPDATE, "RMP_Account_unknown4");
                }
            }
            catch
            {
            }
            return flag;
        }

        
        public string GetDeptName(Guid Id)
        {
            string str = string.Empty;
            try
            {
                Department department = context.Departments.Find(Id);
                if (department != null)
                {
                    str = department.Name;
                    WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("RMP_Account_unknown5", str));
                }
            }
            catch
            {
            }
            return str;
        }

        
        public string GetSubstationName(Guid Id)
        {
            string str = string.Empty;
            try
            {
                Substation substation = context.Substations.Find(Id);
                if (substation != null)
                {
                    str = substation.Name;
                    WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("RMP_Account_uknown6", str));
                }
            }
            catch
            {
            }
            return str;
        }

        
        public int AccountCount()
        {
            return context.Accounts.Where(account => account.SystemRole == 0).Count();
        }

        
        public void ClearInventory(Guid Id)
        {
            try
            {
                context.Inventories.SqlQuery(string.Format("RMP_Account_unknown7", Guid.Empty, Id));
                WriteLog(VMGlobal.LOG_ACTION.DELETE, "RMP_Account_unknown8");
            }
            catch
            {
            }
        }

        
        public List<Account> GetAccountList()
        {
            WriteLog(VMGlobal.LOG_ACTION.LIST, "RMP_Account_unknown8");
            return context.Accounts.OrderBy(account => account.LastName).ToList();
        }

        
        public List<Account> AccountSearch(string badge, string lastName)
        {
            List<Account> accountList = new List<Account>();
            try
            {
                int num = 0;
                if (!string.IsNullOrEmpty(badge))
                    num = 1;
                if (!string.IsNullOrEmpty(lastName))
                    num += 2;
                WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_Account_unknown9", badge, lastName));
                switch (num)
                {
                    case 1:
                        accountList = context.Accounts.Where(account => account.BadgeNumber == badge).ToList();
                        break;
                    case 2:
                        accountList = context.Accounts.Where(account => account.LastName == lastName).ToList();
                        break;
                    case 3:
                        accountList = context.Accounts.Where(account => account.BadgeNumber == badge && account.LastName == lastName).ToList();
                        break;
                }
            }
            catch
            {
            }
            return accountList;
        }

        
        public List<Account> GetUnassignedList()
        {
            return context.Accounts.Where(account => account.Dept_RecId == Guid.Empty && account.SubStation_RecId == Guid.Empty).OrderBy(account => account.LastName).ToList();
        }

        
        public List<Account> GetAccountList(Guid id, Guid sId)
        {
            if (sId == Guid.Empty)
                return context.Accounts.Where(account => account.Dept_RecId == id).OrderBy(account => account.LastName).ToList();
            return context.Accounts.Where(account => account.Dept_RecId == id && account.SubStation_RecId == sId).OrderBy(account => account.LastName).ToList();
        }

        
        public List<Account> GetAlertList()
        {
            return context.Accounts.Where(account => account.IsAlertEmail == true).OrderBy(account => account.LastName).ToList();
        }

        
        public void Logout()
        {
            try
            {
                if (VMGlobal.AccountRecord == null)
                    return;
                WriteLog(VMGlobal.LOG_ACTION.LOGOUT, string.Format("RPM_Account_unknown10", VMGlobal.AccountRecord.ToString(), VMGlobal.AccountRecord.BadgeNumber));
            }
            catch
            {
            }
        }

        
        public Account Authenticate(string loginId, string Pwd)
        {
            AccountLog accountLog = new AccountLog();
            Account account1 = context.Accounts.Where(account => account.LogonID == loginId && account.Password == Pwd && account.IsEnabled == true).SingleOrDefault();
            Account account2 = account1;
            if (account2.Expiration.HasValue)
            {
                DateTime dateTime = account2.Expiration.Value;
                if (account2.IsExpires.Value && dateTime < DateTime.Now)
                    account2 = null;
            }
            if (account2 == null)
                return (Account)null;
            VMGlobal.AccountRecord = account2;
            WriteLog(VMGlobal.LOG_ACTION.LOGON, string.Format("RPM_Account_unknown11", account2.ToString(), account2.BadgeNumber, loginId));
            return account1;
        }

        
        public Account GetAccount(Guid id)
        {
            return context.Accounts.Find(id);
        }

        
        public void Delete(Guid Id)
        {
            Delete(GetAccount(Id));
        }

        
        public void Delete(Account rec)
        {
            WriteLog(VMGlobal.LOG_ACTION.DELETE, string.Format("RPM_Account_unknown11", rec.BadgeNumber, rec.ToString()));
            context.Accounts.Remove(rec);
        }

        
        public void InsertUpdate(Account rec)
        {
            try
            {
                if (rec.Id == Guid.Empty)
                {
                    WriteLog(VMGlobal.LOG_ACTION.SAVE, string.Format("RPM_Account_unknown12", rec.BadgeNumber, rec.ToString()));
                    context.Accounts.Add(rec);
                }
                else
                {
                    WriteLog(VMGlobal.LOG_ACTION.UPDATE, string.Format("RPM_Account_unknown13", rec.BadgeNumber, rec.ToString()));
                    context.Entry(rec).State = EntityState.Modified;
                }
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
                    contextTransaction.Rollback();
                    Msg += (string)(object)ex.InnerException;
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("RPM_Account_unknown14", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }

        
        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
