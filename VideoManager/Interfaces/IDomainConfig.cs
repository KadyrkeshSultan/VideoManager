using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IDomainConfig : IDisposable
    {
        List<DomainConfig> GetCfgDomains();

        DomainConfig GetDomainConfig(Guid Id);

        void Delete(Guid Id);

        void Delete(DomainConfig rec);

        void InsertUpdate(DomainConfig rec);

        bool Save();
    }
}
