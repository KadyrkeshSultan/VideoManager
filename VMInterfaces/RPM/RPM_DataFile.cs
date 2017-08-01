using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_DataFile : IDataFiles, IDisposable
    {
        private Guid AccountId = Guid.Empty;

        private VMContext context = new VMContext();

        public string Msg
        {
            get;
            set;
        }

        public RPM_DataFile()
        {
        }

        public void AddClassification(DFClass rec)
        {
            if (rec.Id == Guid.Empty)
            {
                WriteLog(VMGlobal.LOG_ACTION.SAVE, string.Format("Classification: {0}", rec.Name), AccountId);
                context.DFClasses.Add(rec);
                return;
            }
            WriteLog(VMGlobal.LOG_ACTION.UPDATE, string.Format("{0}", rec.Name), this.AccountId);
            context.Entry(rec).State = EntityState.Modified;
        }

        public void DeleteAllClasses(Guid Id)
        {
            context.DFClasses.RemoveRange(
                from x in context.DFClasses
                where x.DataFileId == Id
                select x);
        }

        public void DeleteClassification(Guid Id)
        {
            DbSet<DFClass> dbSet0 = context.DFClasses;
            object[] id = new object[] { Id };
            DeleteClassification(dbSet0.Find(id));
        }

        public void DeleteClassification(DFClass rec)
        {
            context.DFClasses.Remove(rec);
        }

        public void DeleteDataFile(Guid Id)
        {
            DeleteDataFile(GetDataFile(Id));
        }

        public void DeleteDataFile(DataFile rec)
        {
            WriteLog(VMGlobal.LOG_ACTION.DELETE, string.Format("File {0}\n{1}{2}", rec.Id, rec.StoredFileName, rec.FileExtension), rec.AccountId);
            context.DataFiles.Remove(rec);
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public List<DataFile> GetAll()
        {
            this.WriteLog(VMGlobal.LOG_ACTION.LIST, "Files - List All", Guid.Empty);
            IOrderedQueryable<DataFile> dataFiles =
                from c in context.DataFiles
                where c.IsPurged == false
                orderby c.FileTimestamp
                select c;
            return dataFiles.ToList();
        }

        public List<DataFile> GetBySetName(string SetName)
        {
            IQueryable<DataFile> dataFiles =
                from c in context.DataFiles
                where c.IsPurged == false && c.SetName.Equals(SetName)
                select c;
            WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("Set By Name: {0}", SetName), Guid.Empty);
            return dataFiles.ToList();
        }

        public List<DFClass> GetClassList(Guid Id)
        {
            IOrderedQueryable<DFClass> dbSet0 =
                from c in context.DFClasses
                where c.DataFileId == Id
                orderby c.Name
                select c;
            return dbSet0.ToList();
        }

        public DataFile GetDataFile(Guid Id)
        {
            return context.DataFiles.Find(new object[] { Id });
        }

        public List<string> GetFileExtensions()
        {
            List<string> strs = new List<string>();
            IQueryable<string> dataFiles =
                from c in context.DataFiles
                group c.FileExtension by c.FileExtension into ext
                select ext.FirstOrDefault();
            return dataFiles.ToList();
        }

        public List<DataFile> GetLastDays(Guid Id, int n)
        {
            DateTime now = DateTime.Now;
            DateTime dateTime = now.AddDays((double)(-n));
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("Files By Last Days: {0} - Days\n {1} to {2}", n, now, dateTime), Id);
            return (
                from e in context.DataFiles
                where e.AccountId.Equals(Id) && e.IsPurged == false && (e.FileTimestamp >= dateTime) && (e.FileTimestamp <= now)
                select e).ToList();
        }

        public List<DataFile> GetListByDateRange(DateTime d1, DateTime d2)
        {
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("Files by Date Range: {0} to {1}", d1, d2), Guid.Empty);
            return (
                from e in context.DataFiles
                where (e.FileTimestamp >= d1) && e.IsPurged == false && (e.FileTimestamp <= d2)
                select e).ToList();
        }

        public List<DataFile> GetListByDateRange(Guid Id, DateTime d1, DateTime d2)
        {
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("Files by Date Range: {0} to {1}", d1, d2), Id);
            return (
                from e in context.DataFiles
                where e.AccountId.Equals(Id) && e.IsPurged == false && (e.FileTimestamp >= d1) && (e.FileTimestamp <= d2)
                orderby e.FileTimestamp descending
                select e).ToList();
        }

        public FileMemo GetMemo(Guid Id)
        {
            DbSet<FileMemo> fileMemos = context.FileMemos;
            object[] id = new object[] { Id };
            FileMemo fileMemo = fileMemos.Find(id);
            object[] shortDesc = new object[] { fileMemo.ShortDesc, fileMemo.Timestamp, fileMemo.DataFile.Id, fileMemo.DataFile.ShortDesc };
            WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("Memo: {0}\nMemo Date: {1}\nData File ID: {2}\nData File Desc: {3}", shortDesc), Guid.Empty);
            return fileMemo;
        }

        public int GetMemoCount(Guid Id)
        {
            return context.FileMemos.Count((FileMemo t) => t.DataFile.Id == Id);
        }

        public List<DataFile> GetPurgeFilesByAccountID(DateTime Start, DateTime End, Guid Id)
        {
            IOrderedQueryable<DataFile> dataFiles =
                from c in context.DataFiles
                where c.IsPurged == true && (c.AccountId == Id) && (c.PurgeTimestamp >= Start) && (c.PurgeTimestamp <= End)
                orderby c.PurgeTimestamp
                select c;
            return dataFiles.ToList();
        }

        public List<Classification> GetRetentionClasses()
        {
            IOrderedQueryable<Classification> classifications =
                from c in context.Classifications
                where c.IsRetention
                orderby c.Name
                select c;
            return classifications.ToList();
        }

        public List<Guid> GetRetentionFilesByClass(string className, int days)
        {
            DateTime dateTime = DateTime.Now.AddDays((double)(-days));
            IQueryable<Guid> dataFiles =
                from c in context.DataFiles
                where (c.Classification == className) && c.IsPurged == false && !c.IsIndefinite && (c.FileAddedTimestamp < dateTime)
                select c.Id;
            return dataFiles.ToList();
        }

        public List<DataFile> GetSet(Guid AccountID, string SetName)
        {
            IQueryable<DataFile> dataFiles =
                from c in context.DataFiles
                where (c.AccountId == AccountID) && (c.SetName == SetName) && c.IsPurged == false
                select c;
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("Get Set By Name: {0}", SetName), AccountID);
            return dataFiles.ToList();
        }

        public List<DataFile> GetSetList(Guid AccountID, string SetName, DateTime From, DateTime To)
        {
            List<DataFile> dataFiles = new List<DataFile>();
            IQueryable<DataFile> dataFiles1 =
                from c in context.DataFiles
                where (c.AccountId == AccountID) && c.SetName.Contains(SetName) && (c.FileTimestamp >= From) && (c.FileTimestamp <= To) && c.IsPurged == false
                group c by c.SetName into set
                select set.FirstOrDefault<DataFile>();
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("Get Set List: {0}\nFrom: {1} To: {2}", SetName, From, To), AccountID);
            return dataFiles1.ToList<DataFile>();
        }

        public void GroupSet(List<DataFile> files, string SetName)
        {
        }

        public List<DataFile> LoadByDate(Guid AccountID, DateTime From, DateTime To)
        {
            List<DataFile> dataFiles = new List<DataFile>();
            IQueryable<DataFile> dataFiles1 =
                from c in context.DataFiles
                where (c.AccountId == AccountID) && (c.FileAddedTimestamp >= From) && (c.FileAddedTimestamp <= To) && c.IsPurged == false
                select c;
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("Catalog Files by Date: {0} To: {1}", From, To), AccountID);
            return dataFiles1.ToList();
        }

        public List<DataFile> QryGlobalCatalog(bool byFileDate, DateTime sDate, DateTime eDate, string rms, string cad, string set, bool bEvidence)
        {
            string str = rms;
            string str1 = cad;
            string str2 = set;
            IQueryable<DataFile> rMSNumber = null;
            rMSNumber = (!byFileDate ?
                from c in context.DataFiles
                where (c.FileAddedTimestamp >= sDate) && (c.FileAddedTimestamp <= eDate)
                select c :
                from c in context.DataFiles
                where (c.FileTimestamp >= sDate) && (c.FileTimestamp <= eDate)
                select c);
            if (!string.IsNullOrEmpty(str))
            {
                if (!str.Contains("*"))
                {
                    rMSNumber =
                        from c in rMSNumber
                        where c.RMSNumber == str
                        select c;
                }
                else
                {
                    str = str.Replace("*", "");
                    rMSNumber =
                        from c in rMSNumber
                        where c.RMSNumber.Contains(str)
                        select c;
                }
            }
            if (!string.IsNullOrEmpty(str1))
            {
                if (!str1.Contains("*"))
                {
                    rMSNumber =
                        from c in rMSNumber
                        where c.CADNumber == str1
                        select c;
                }
                else
                {
                    str1 = str1.Replace("*", "");
                    rMSNumber =
                        from c in rMSNumber
                        where c.CADNumber.Contains(str1)
                        select c;
                }
            }
            if (!string.IsNullOrEmpty(str2))
            {
                if (!str2.Contains("*"))
                {
                    rMSNumber =
                        from c in rMSNumber
                        where c.SetName == str2
                        select c;
                }
                else
                {
                    str2 = str2.Replace("*", "");
                    rMSNumber =
                        from c in rMSNumber
                        where c.SetName.Contains(str2)
                        select c;
                }
            }
            if (bEvidence)
            {
                rMSNumber =
                    from c in rMSNumber
                    where c.IsEvidence
                    select c;
            }
            rMSNumber =
                from c in rMSNumber
                orderby c.AccountId
                select c;
            return rMSNumber.ToList();
        }

        public List<DataFile> QueryDataFile(CatalogFilter filter)
        {
            List<DataFile> dataFiles = new List<DataFile>();
            IQueryable<DataFile> fileExtension =
                from c in context.DataFiles
                where c.AccountId.Equals(filter.AccountID)
                select c;
            if (!string.IsNullOrEmpty(filter.FileType))
            {
                fileExtension =
                    from c in fileExtension
                    where c.FileExtension == filter.FileType
                    select c;
            }
            if (!string.IsNullOrEmpty(filter.Classification))
            {
                fileExtension =
                    from c in fileExtension
                    where c.Classification == filter.Classification
                    select c;
            }
            if (!string.IsNullOrEmpty(filter.RMS))
            {
                fileExtension =
                    from c in fileExtension
                    where c.RMSNumber == filter.RMS
                    select c;
            }
            if (!string.IsNullOrEmpty(filter.CAD))
            {
                fileExtension =
                    from c in fileExtension
                    where c.CADNumber == filter.CAD
                    select c;
            }
            if (filter.Rating > 0)
            {
                fileExtension =
                    from c in fileExtension
                    where c.Rating == filter.Rating
                    select c;
            }
            if (filter.IsSecurityFilter)
            {
                fileExtension =
                    from c in fileExtension
                    where (int)c.Security == (int)filter.SecurityLevel
                    select c;
            }
            if (filter.IsEvidence)
            {
                fileExtension =
                    from c in fileExtension
                    where c.IsEvidence == filter.IsEvidence
                    select c;
            }
            if (!string.IsNullOrEmpty(filter.WordPhrase))
            {
                fileExtension =
                    from c in fileExtension
                    where c.ShortDesc.Contains(filter.WordPhrase)
                    select c;
            }
            fileExtension =
                from c in fileExtension
                where c.IsPurged == (bool?)false
                select c;
            fileExtension =
                from c in fileExtension
                where (c.FileTimestamp >= filter.StartDate) && (c.FileTimestamp <= filter.EndDate)
                orderby c.FileTimestamp descending
                select c;
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("{0} to {1}\nClassification: {2}", filter.StartDate, filter.EndDate, filter.Classification), filter.AccountID);
            return fileExtension.ToList<DataFile>();
        }

        public bool Save()
        {
            bool flag = false;
            using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                    flag = true;
                }
                catch (DbUpdateException dbUpdateException1)
                {
                    DbUpdateException dbUpdateException = dbUpdateException1;
                    dbContextTransaction.Rollback();
                    string message = dbUpdateException.Message;
                    dbUpdateException.InnerException.ToString();
                }
                catch (DbEntityValidationException dbEntityValidationException1)
                {
                    DbEntityValidationException dbEntityValidationException = dbEntityValidationException1;
                    dbContextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in dbEntityValidationException.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                        {
                            RPM_DataFile rPMDataFile = this;
                            rPMDataFile.Msg = string.Concat(rPMDataFile.Msg, string.Format("Property: {0} Error: {1}\n", validationError.PropertyName, validationError.ErrorMessage));
                        }
                    }
                }
            }
            return flag;
        }

        public void SaveUpdate(DataFile rec)
        {
            if (rec.Id != Guid.Empty)
            {
                WriteLog(VMGlobal.LOG_ACTION.UPDATE, string.Format("{0}{1}", rec.StoredFileName, rec.FileExtension), rec.AccountId);
                context.Entry(rec).State = EntityState.Modified;
                return;
            }
            object[] storedFileName = new object[] { rec.StoredFileName, rec.FileExtension, rec.OriginalFileName, rec.SourcePath };
            WriteLog(VMGlobal.LOG_ACTION.UPLOAD, string.Format("{0}{1}\n{2}\n{3}", storedFileName), rec.AccountId);
            context.DataFiles.Add(rec);
        }

        public void SetAccountId(Guid Id)
        {
            AccountId = Id;
        }

        void System.IDisposable.Dispose()
        {
            context.Dispose();
        }

        public void ViewFileData(Guid Id)
        {
            DbSet<DataFile> dataFiles = context.DataFiles;
            object[] id = new object[] { Id };
            DataFile dataFile = dataFiles.Find(id);
            WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("{0}\n{1}{2}", dataFile.ShortDesc, dataFile.StoredFileName, dataFile.FileExtension), dataFile.AccountId);
        }

        private void WriteLog(VMGlobal.LOG_ACTION action, string memo, Guid Id)
        {
            if (Id != Guid.Empty)
            {
                DbSet<Account> accounts = context.Accounts;
                object[] id = new object[] { Id };
                Account account = accounts.Find(id);
                memo = string.Concat(string.Format("{0} [{1}]\n", account.ToString(), account.BadgeNumber), memo);
            }
            AccountLog accountLog = new AccountLog()
            {
                Action = action.ToString(),
                Memo = memo
            };
            using (RPM_Logs rPMLog = new RPM_Logs())
            {
                rPMLog.LogAccount(accountLog);
            }
        }
    }
}