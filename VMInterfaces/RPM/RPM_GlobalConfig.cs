using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_GlobalConfig : IGlobalConfig, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_GlobalConfig()
        {
            this.context = new VMContext();
        }

        
        public List<GlobalConfig> GetConfigList()
        {
            return context.GlobalCfg.Where(globalConfig => globalConfig.IsEditable == true).OrderBy(globalConfig => globalConfig.Key).ToList();
        }

        
        public List<GlobalConfig> GetConfigListByDesc()
        {
            return context.GlobalCfg.Where(globalConfig => globalConfig.IsEditable == true).OrderBy(globalConfig => globalConfig.Desc).ToList();
        }

        
        public string GetConfigValue(string Key, string Default)
        {
            string str = Default;
            GlobalConfig configRecord = GetConfigRecord(Key.ToUpper());
            if (configRecord != null)
                str = configRecord.Value;
            return str;
        }

        
        public string GetConfigValue(string Key)
        {
            string empty = string.Empty;
            GlobalConfig configRecord = GetConfigRecord(Key.ToUpper());
            if (configRecord != null)
                empty = configRecord.Value;
            return empty;
        }

        
        public GlobalConfig GetConfigRecord(string Key)
        {
            GlobalConfig globalConfig1 = new GlobalConfig();
            try
            {
                globalConfig1 = context.GlobalCfg.Where(globalConfig => globalConfig.Key.Equals(Key)).SingleOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return globalConfig1;
        }

        
        public void Dispose()
        {
            context.Dispose();
        }

        
        public void SaveUpdate(GlobalConfig rec)
        {
            if (rec.Id == Guid.Empty)
                this.context.GlobalCfg.Add(rec);
            else
                this.context.Entry<GlobalConfig>(rec).State = EntityState.Modified;
            this.Save();
        }

        
        public bool Save()
        {
            bool flag = false;
            using (DbContextTransaction contextTransaction = this.context.Database.BeginTransaction())
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
                    string message = ex.Message;
                    ex.InnerException.ToString();
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("RPM_GlobalConfig_unknown1", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }
    }
}
