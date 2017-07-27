using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_RightsProfile : IRightsProfile, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_RightsProfile()
        {
            context = new VMContext();
        }

        
        public List<RightsProfile> GetProfileList()
        {
            return context.RightsProfiles.OrderBy(rightsProfile => rightsProfile.Desc).ToList();
        }

        
        public RightsProfile GetProfile(Guid Id)
        {
            return context.RightsProfiles.Find(Id);
        }

        
        public int GetRights(Guid Id)
        {
            return GetProfile(Id).Rights;
        }

        
        public void DeleteProfile(Guid Id)
        {
            DeleteProfile(GetProfile(Id));
        }

        
        public void DeleteProfile(RightsProfile rec)
        {
            context.RightsProfiles.Remove(rec);
        }

        
        public void InsertUpdate(RightsProfile rec)
        {
            if (rec.Id == Guid.Empty)
                context.RightsProfiles.Add(rec);
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
                    this.context.SaveChanges();
                    contextTransaction.Commit();
                    flag = true;
                }
                catch (DbUpdateException ex)
                {
                    contextTransaction.Rollback();
                    Msg += ex.Message;
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("RPM_RightsProfile_unknowns1", validationError.PropertyName, validationError.ErrorMessage);
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
