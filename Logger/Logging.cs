using System;
using VMInterfaces;
using VMModels.Model;

namespace Logger
{
    public static class Logging
    {
        
        public static void WriteSystemLog(VMGlobal.LOG_ACTION action, string memo, Guid Id)
        {
            using (RPM_Logs rpmLogs = new RPM_Logs())
                rpmLogs.LogSystem(new SystemLog()
                {
                    Action = action.ToString(),
                    Memo = memo
                });
        }

        
        public static void WriteAccountLog(VMGlobal.LOG_ACTION action, string memo, Guid Id)
        {
            if (Id != Guid.Empty)
            {
                using (RPM_Account rpmAccount = new RPM_Account())
                {
                    Account account = rpmAccount.GetAccount(Id);
                    memo = string.Format("Logger_Logging_unknown1", account.ToString(), account.BadgeNumber) + memo;
                }
            }
            AccountLog log = new AccountLog();
            log.Action = action.ToString();
            log.Memo = memo;
            using (RPM_Logs rpmLogs = new RPM_Logs())
                rpmLogs.LogAccount(log);
        }

        
        public static void WriteCameraLog(CameraLog log)
        {
        }
    }
}
