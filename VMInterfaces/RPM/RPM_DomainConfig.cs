using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_DomainConfig : IDomainConfig, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_DomainConfig()
        {
            this.context = new VMContext();
        }

        
        public List<DomainConfig> GetCfgDomains()
        {
            return context.DomainCfg.OrderBy(domainConfig => domainConfig.ProfileName).ToList();
        }

        
        public DomainConfig GetDomainConfig(Guid Id)
        {
            return context.DomainCfg.Find(Id);
        }

        
        public void Delete(Guid Id)
        {
            Delete(GetDomainConfig(Id));
        }

        
        public void Delete(DomainConfig rec)
        {
            context.DomainCfg.Remove(rec);
            context.Database.ExecuteSqlCommand(string.Format("RPM_DomainConfig_unknown1", Guid.Empty, rec.Id));
        }

        
        public void InsertUpdate(DomainConfig rec)
        {
            if (rec.Id == Guid.Empty)
                context.DomainCfg.Add(rec);
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
                            Msg += string.Format("RPM_DomainConfig_unknown2", validationError.PropertyName, validationError.ErrorMessage);
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
