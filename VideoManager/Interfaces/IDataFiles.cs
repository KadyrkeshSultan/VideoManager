using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IDataFiles
    {
        void SetAccountId(Guid Id);

        List<DataFile> GetListByDateRange(DateTime d1, DateTime d2);

        List<DataFile> GetListByDateRange(Guid Id, DateTime d1, DateTime d2);

        List<DataFile> GetLastDays(Guid Id, int n);

        void GroupSet(List<DataFile> files, string SetName);

        List<DataFile> GetBySetName(string SetName);

        List<DataFile> GetSetList(Guid AccountID, string SetName, DateTime From, DateTime To);

        List<DataFile> GetSet(Guid AccountID, string SetName);

        List<DataFile> LoadByDate(Guid AccountID, DateTime From, DateTime To);

        DataFile GetDataFile(Guid Id);

        FileMemo GetMemo(Guid Id);

        List<DataFile> QueryDataFile(CatalogFilter filter);

        int GetMemoCount(Guid Id);

        void DeleteDataFile(Guid Id);

        void DeleteDataFile(DataFile rec);

        void SaveUpdate(DataFile rec);

        List<string> GetFileExtensions();

        List<DataFile> GetAll();

        void ViewFileData(Guid Id);

        void AddClassification(DFClass rec);

        List<DFClass> GetClassList(Guid Id);

        void DeleteClassification(Guid Id);

        void DeleteClassification(DFClass rec);

        void DeleteAllClasses(Guid Id);

        List<Classification> GetRetentionClasses();

        List<Guid> GetRetentionFilesByClass(string className, int days);

        List<DataFile> QryGlobalCatalog(bool byFileDate, DateTime sDate, DateTime eDate, string rms, string cad, string set, bool bEvidence);

        List<DataFile> GetPurgeFilesByAccountID(DateTime Start, DateTime End, Guid Id);

        bool Save();
    }
}
