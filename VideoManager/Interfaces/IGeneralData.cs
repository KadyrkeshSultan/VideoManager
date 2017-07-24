using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IGeneralData : IDisposable
    {
        List<Classification> GetClassificationList();

        Classification GetClassification(Guid Id);

        void DeleteClassification(Guid Id);

        void DeleteClassification(Classification rec);

        void InsertUpdateClassification(Classification rec);

        bool Save();
    }
}
