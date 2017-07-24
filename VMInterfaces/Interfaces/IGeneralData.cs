using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface IGeneralData : IDisposable
    {
        List<Classification> GetClassificationList();

        Classification GetClassification(Guid Id);

        void DeleteClassification(Guid Id);

        void DeleteClassification(Classification rec);

        void InsertUpdateClassification(Classification rec);

        bool Save();
    }
}
