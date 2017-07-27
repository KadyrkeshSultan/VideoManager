using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_Dept : IDepartment, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_Dept()
        {
            context = new VMContext();
        }

        
        public List<Department> GetDeptList()
        {
            return context.Departments.OrderBy(department => department.Name).ToList();
        }

        
        public Department GetDept(Guid Id)
        {
            return context.Departments.Find(Id);
        }

        
        public void DeleteDept(Guid Id)
        {
            DeleteDept(GetDept(Id));
        }

        
        public void DeleteDept(Department rec)
        {
            context.Database.ExecuteSqlCommand(string.Format("RPM_Dept_unknown1", Guid.Empty, Guid.Empty, rec.Id));
            context.Departments.Remove(rec);
        }

        
        public void InsertUpdate(Department rec)
        {
            if (rec.Id == Guid.Empty)
                context.Departments.Add(rec);
            else
                context.Entry(rec).State = EntityState.Modified;
        }

        
        public List<Substation> GetSubstationList(Guid Id)
        {
            return context.Substations.Where(substation => substation.Department.Id == Id).OrderBy(substation => substation.Name).ToList();
        }

        
        public Substation GetSubstation(Guid Id)
        {
            return context.Substations.Find(Id);
        }

        
        public void DeleteSub(Guid Id)
        {
            DeleteSub(GetSubstation(Id));
        }

        
        public void DeleteSub(Substation rec)
        {
            context.Substations.Remove(rec);
        }

        
        public void InsertUpdate(Substation rec)
        {
            if (rec.Id == Guid.Empty)
                context.Substations.Add(rec);
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
                    Msg += ex.Message;
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("RPM_Dept_unknown2", validationError.PropertyName, validationError.ErrorMessage);
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
