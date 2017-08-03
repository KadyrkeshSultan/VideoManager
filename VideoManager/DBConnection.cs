using System;
using System.IO;
using System.Windows.Forms;
using VMInterfaces;

namespace VideoManager
{
    public class DBConnection
    {
        public DBConnection()
        {
        }

        public bool method_0()
        {
            bool flag = false;
            if (File.Exists(Path.Combine(VMGlobal.ProfilePath, VMGlobal.ProfileFile)))
            {
                VMGlobal.GetDBConnection();
                flag = true;
            }
            else if ((new RegDB()).ShowDialog() == DialogResult.OK)
            {
                flag = true;
            }
            return flag;
        }

        public bool TestConnection()
        {
            bool flag = false;
            try
            {
                using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
                {
                    if (rPMGlobalConfig.GetConfigValue("PRODUCT_KEY") != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception exception)
            {
                flag = false;
            }
            return flag;
        }
    }
}