using System;
using System.Collections.Generic;
using VideoManager.Enums;
using VideoManager.Model;

namespace DAL
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