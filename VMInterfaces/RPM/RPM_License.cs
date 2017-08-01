using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Unity;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_License : ILicense, IDisposable
    {
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_License()
        {
            context = new VMContext();
        }

        
        public int GetLicenseCount()
        {
            int num = 1;
            string licenseData = GetLicenseData();
            if (!string.IsNullOrEmpty(licenseData))
                num = Convert.ToInt32(licenseData.Split('|')[2]);
            return num;
        }

        
        public void CreateDefaultLicense()
        {
            License rec = new License();
            DateTime now = DateTime.Now;
            rec.Timestamp = now;
            Guid guid = Guid.NewGuid();
            rec.LicData = CryptoIO.Encrypt(string.Format("{0}|{1}|1", guid, now));
            SaveUpdate(rec);
            Save();
            using (RPM_GlobalConfig rpmGlobalConfig = new RPM_GlobalConfig())
            {
                rpmGlobalConfig.SaveUpdate(new GlobalConfig()
                {
                    Key = "PRODUCT_KEY",
                    Value = CryptoIO.Encrypt(string.Format("{0}|{1}", Guid.NewGuid(), now)),
                    IsEditable = false,
                    Desc = "C3 Sentinel SKU Product ID",
                    DataType = "STRING"
                });
                rpmGlobalConfig.Save();
            }
        }

        
        public string GetLicenseData()
        {
            try
            {
                return CryptoIO.Decrypt(context.Licenses.OrderByDescending(license => license.Timestamp).FirstOrDefault().LicData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        public bool UpdateLicense(string data)
        {
            bool flag = false;
            try
            {
                string[] strArray1 = GetLicenseData().Split('|');
                string[] strArray2 = CryptoIO.Decrypt(data).Split('|');
                int int32 = Convert.ToInt32(strArray2[2]);
                DateTime now = DateTime.Now;
                using (RPM_GlobalConfig rpmGlobalConfig = new RPM_GlobalConfig())
                {
                    GlobalConfig configRecord = rpmGlobalConfig.GetConfigRecord("PRODUCT_KEY");
                    string[] strArray3 = CryptoIO.Decrypt(configRecord.Value).Split('|');
                    if (strArray2[0].Equals(strArray3[0]))
                    {
                        if (strArray2[1].Equals(strArray3[1]))
                        {
                            if (strArray3[1].Equals(strArray1[1]))
                            {
                                configRecord.Value = CryptoIO.Encrypt(string.Format("{0}|{1}", strArray3[0], now));
                                rpmGlobalConfig.SaveUpdate(configRecord);
                                rpmGlobalConfig.Save();
                                flag = true;
                            }
                        }
                    }
                }
                if (flag)
                {
                    using (new RPM_License())
                    {
                        License rec = new License();
                        rec.Timestamp = now;
                        Guid guid = Guid.NewGuid();
                        rec.LicData = CryptoIO.Encrypt(string.Format("{0}|{1}|{2}", guid, now, int32));
                        SaveUpdate(rec);
                        Save();
                    }
                }
            }
            catch
            {
            }
            return flag;
        }

        
        public string GetRequestKey(int Count)
        {
            string str = string.Empty;
            string licenseData = GetLicenseData();
            if (!string.IsNullOrEmpty(licenseData))
            {
                string[] strArray1 = licenseData.Split('|');
                using (RPM_GlobalConfig rpmGlobalConfig = new RPM_GlobalConfig())
                {
                    str = rpmGlobalConfig.GetConfigValue("PRODUCT_KEY");
                    if (string.IsNullOrEmpty(str))
                        return null;
                    string[] strArray2 = CryptoIO.Decrypt(str).Split('|');
                    if (strArray1[1].Equals(strArray2[1]))
                        str = CryptoIO.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}", DateTime.Now.Millisecond, strArray2[0], strArray1[1], DateTime.Now, Count));
                }
            }
            return str;
        }

        
        public void SaveUpdate(License rec)
        {
            if (rec.Id == Guid.Empty)
                context.Licenses.Add(rec);
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
