using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Enums;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_AlertEmail : IAlertEmail, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_AlertEmail()
        {
            context = new VMContext();
        }

        
        public List<AlertEmail> GetEmailList()
        {
            return context.AlertEmails.OrderBy(alertEmail => alertEmail.Subject).ToList();
        }

        
        public AlertEmail GetAlertByType(EmailType eType)
        {
            return context.AlertEmails.FirstOrDefault(alertEmail => (int)alertEmail.EmailType == (int)eType);
        }

        
        public AlertEmail GetAlertEmail(Guid Id)
        {
            return context.AlertEmails.Find(Id);
        }

        
        public void Delete(Guid Id)
        {
            Delete(GetAlertEmail(Id));
        }

        
        public void Delete(AlertEmail rec)
        {
            context.AlertEmails.Remove(rec);
        }

        
        public void InsertUpdate(AlertEmail rec)
        {
            if (rec.Id == Guid.Empty)
                context.AlertEmails.Add(rec);
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
