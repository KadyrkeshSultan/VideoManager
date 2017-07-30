using System;
using VMInterfaces;
using VMModels.Model;

namespace AppGlobal
{
    public static class StoreCheck
    {
        
        public static void CheckAccountStorage()
        {
            SetPath(Global.GlobalAccount.SubStation_RecId);
        }

        
        private static void SetPath(Guid SubstationID)
        {
            Guid Id = Guid.Empty;
            if (!(SubstationID != Guid.Empty))
                return;
            using (RPM_Dept rpmDept = new RPM_Dept())
                Id = rpmDept.GetSubstation(SubstationID).DomainCfgID;
            if (Id != Guid.Empty)
            {
                using (RPM_DomainConfig rpmDomainConfig = new RPM_DomainConfig())
                {
                    DomainConfig domainConfig = rpmDomainConfig.GetDomainConfig(Id);
                    Global.UNCServer = domainConfig.UNCRoot;
                    Global.RelativePath = domainConfig.UNCPath;
                }
            }
            else
            {
                Global.UNCServer = Global.PrimaryUNCServer;
                Global.RelativePath = Global.PrimaryRelativePath;
            }
        }

        
        public static void CheckAccountStorage(Guid AccountID)
        {
            Guid SubstationID = Guid.Empty;
            using (RPM_Account rpmAccount = new RPM_Account())
                SubstationID = rpmAccount.GetAccount(AccountID).SubStation_RecId;
            SetPath(SubstationID);
        }
    }
}
