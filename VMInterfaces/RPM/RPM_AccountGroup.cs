using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_AccountGroup : IAccountGroup, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_AccountGroup()
        {
            context = new VMContext();
        }

        
        public List<AccountGroup> GetAccountGroupList()
        {
            return context.AccountGroups.OrderBy(accountGroup => accountGroup.Name).ToList();
        }

        
        public AccountGroup GetAccountGroup(Guid Id)
        {
            return context.AccountGroups.Find(Id);
        }

        
        public void DeleteGroup(AccountGroup rec)
        {
            context.AccountGroups.Remove(rec);
        }

        
        public void DeleteGroup(Guid Id)
        {
            DeleteGroup(context.AccountGroups.Find(Id));
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
