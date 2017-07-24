using System;
using System.Collections.Generic;
using VideoManager.Enums;
using VideoManager.Model;

namespace VideoManager.Interface
{
    interface IAlertEmail : IDisposable
    {
        List<AlertEmail> GetEmailList();

        AlertEmail GetAlertEmail(Guid Id);

        AlertEmail GetAlertByType(EmailType eType);

        void Delete(Guid Id);

        void Delete(AlertEmail rec);

        void InsertUpdate(AlertEmail rec);

        bool Save();
    }
}