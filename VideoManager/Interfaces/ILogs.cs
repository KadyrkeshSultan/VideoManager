using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface ILogs : IDisposable
    {
        void LogSystem(SystemLog log);

        void LogAccount(AccountLog log);

        void LogCamera(CameraLog log);

        bool Save();

        List<AccountLog> GetAccountLogsByID_Date(Guid Id, DateTime d1, DateTime d2, string Action);

        List<string> GetActionCodes();
    }
}
