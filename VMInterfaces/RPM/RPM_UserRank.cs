using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_UserRank : IUserRank, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_UserRank()
        {
            context = new VMContext();
        }

        
        public List<UserRank> GetRankList()
        {
            return context.UserRanks.OrderBy(userRank => userRank.Rank).ToList();
        }

        
        public void Dispose()
        {
            context.Dispose();
        }

        
        public void Delete(Guid Id)
        {
            Delete(context.UserRanks.Find(Id));
        }

        
        public void Delete(UserRank rec)
        {
            context.UserRanks.Remove(rec);
        }

        
        public UserRank GetRank(Guid Id)
        {
            return context.UserRanks.Find(Id);
        }

        
        public void InsertUpdate(UserRank rec)
        {
            if (rec.Id == Guid.Empty)
                context.UserRanks.Add(rec);
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
    }
}
