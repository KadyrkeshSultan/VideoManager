using System;
using System.Collections.Generic;
using VMModels.Enums;
using VMModels.Model;

namespace VMInterfaces
{
    public interface IAlertEmail : IDisposable
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