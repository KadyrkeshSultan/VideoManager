using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Groups : IGroups, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_Groups()
        {
            context = new VMContext();
        }

        
        public List<AccountGroup> ListGroups()
        {
            return context.AccountGroups.OrderBy(accountGroup => accountGroup.Name).ToList();
        }

        
        public AccountGroup GetGroup(Guid Id)
        {
            return context.AccountGroups.Find(Id);
        }

        
        public void DeleteGroup(Guid Id)
        {
            DeleteGroup(GetGroup(Id));
        }

        
        public void DeleteGroup(AccountGroup rec)
        {
            context.AccountGroups.Remove(rec);
        }

        
        public void ClearMembers(Guid RecIdx)
        {
            context.Database.ExecuteSqlCommand(string.Format("RPM_Groups_unknown1", RecIdx));
        }

        
        public void ClearGroups(Guid ActIdx)
        {
            context.Database.ExecuteSqlCommand(string.Format("RPM_Groups_unknown2", ActIdx));
        }

        
        public void AddMember(Guid GrpIdx, Guid ActIdx)
        {
            context.Database.ExecuteSqlCommand(string.Format("RPM_Groups_unknown3", ActIdx, GrpIdx));
        }

        
        public List<AccountGroup> GetGroupsByAccount(Guid Id)
        {
            List<Account> list = context.Accounts.Where(account => account.Id == Id).ToList();
            List<AccountGroup> accountGroupList = new List<AccountGroup>();
            if (list.Count > 0)
            {
                Account account = list[0];
                if (account != null)
                    accountGroupList = account.AccountGroups.ToList();
            }
            return accountGroupList;
        }

        
        public void InsertUpdate(AccountGroup rec)
        {
            if (rec.Id == Guid.Empty)
                context.AccountGroups.Add(rec);
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
                            Msg += string.Format("RPM_Groups_unknown4", validationError.PropertyName, validationError.ErrorMessage);
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
