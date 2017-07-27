using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_GeneralData : IGeneralData, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_GeneralData()
        {
            this.context = new VMContext();
        }

        
        public List<Classification> GetClassificationList()
        {
            return context.Classifications.OrderBy(classification => classification.Name).ToList();
        }

        
        public Classification GetClassification(Guid Id)
        {
            return context.Classifications.Find(Id);
        }

        
        public void DeleteClassification(Guid Id)
        {
            DeleteClassification(GetClassification(Id));
        }

        
        public void DeleteClassification(Classification rec)
        {
            context.Classifications.Remove(rec);
        }

        
        public void InsertUpdateClassification(Classification rec)
        {
            if (rec.Id == Guid.Empty)
                context.Classifications.Add(rec);
            else
                context.Entry(rec).State = EntityState.Modified;
        }

        
        public bool Save()
        {
            bool flag = false;
            try
            {
                this.context.SaveChanges();
                flag = true;
            }
            catch (DbUpdateException ex)
            {
                Msg += (string)(object)ex.InnerException;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                        Msg += string.Format("RPM_GeneralData_unknown1", validationError.PropertyName, validationError.ErrorMessage);
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
