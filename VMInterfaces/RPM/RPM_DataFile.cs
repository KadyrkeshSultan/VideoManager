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
        private Guid AccountId;
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_DataFile()
        {
            AccountId = Guid.Empty;
            context = new VMContext();
        }

        
        public void AddClassification(DFClass rec)
        {
            if (rec.Id == Guid.Empty)
            {
                WriteLog(VMGlobal.LOG_ACTION.SAVE, string.Format("RPM_DataFile_unknown1", rec.Name), AccountId);
                context.DFClasses.Add(rec);
            }
            else
            {
                this.WriteLog(VMGlobal.LOG_ACTION.UPDATE, string.Format("RPM_DataFile_unknown2", rec.Name), AccountId);
                context.Entry(rec).State = EntityState.Modified;
            }
        }

        
        public List<DFClass> GetClassList(Guid Id)
        {
            return context.DFClasses.Where(dfClass => dfClass.DataFileId == Id).OrderBy(dfClass => dfClass.Name).ToList();
        }

        
        public List<Classification> GetRetentionClasses()
        {
            return context.Classifications.Where(classification => classification.IsRetention == true).OrderBy(classification => classification.Name).ToList();
        }

        
        public List<Guid> GetRetentionFilesByClass(string className, int days)
        {
            DateTime purgeDate = DateTime.Now.AddDays(-days);
            return context.DataFiles.Where(dataFile => dataFile.Classification == className && dataFile.IsPurged == false && dataFile.IsIndefinite == false && dataFile.FileAddedTimestamp < purgeDate).Select(dataFile => dataFile.Id).ToList();
        }

        
        public void DeleteClassification(Guid Id)
        {
            DeleteClassification(context.DFClasses.Find(Id));
        }

        
        public void DeleteClassification(DFClass rec)
        {
            context.DFClasses.Remove(rec);
        }

        
        public void DeleteAllClasses(Guid Id)
        {
            context.DFClasses.RemoveRange(context.DFClasses.Where(dfClass => dfClass.DataFileId == Id));
        }

        
        public List<DataFile> GetPurgeFilesByAccountID(DateTime Start, DateTime End, Guid Id)
        {
            return context.DataFiles.Where(dataFile => dataFile.IsPurged == true && dataFile.AccountId == Id && dataFile.PurgeTimestamp >= Start && dataFile.PurgeTimestamp <= End).OrderBy(dataFile => dataFile.PurgeTimestamp).ToList();
        }

        
        public List<DataFile> QryGlobalCatalog(bool byFileDate, DateTime sDate, DateTime eDate, string rms, string cad, string set, bool bEvidence)
        {
            IQueryable<DataFile> source;
            if (byFileDate)
                source = context.DataFiles.Where(dataFile => dataFile.FileTimestamp >= sDate && dataFile.FileTimestamp <= eDate);
            else
                source = context.DataFiles.Where(dataFile => dataFile.FileAddedTimestamp >= sDate && dataFile.FileAddedTimestamp <= eDate);
            if (!string.IsNullOrEmpty(rms))
            {
                if (rms.Contains("RPM_DataFile_unknown3"))
                {
                    rms = rms.Replace("RPM_DataFile_unknown4", "");
                    source = source.Where(dataFile => dataFile.RMSNumber.Contains(rms));
                }
                else
                    source = source.Where(dataFile => dataFile.RMSNumber == rms);
            }
            if (!string.IsNullOrEmpty(cad))
            {
                if (cad.Contains("RPM_DataFile_unknown5"))
                {
                    cad = cad.Replace("RPM_DataFile_unknown6", "");
                    source = source.Where(dataFile => dataFile.CADNumber.Contains(cad));
                }
                else
                    source = source.Where(dataFile => dataFile.CADNumber == cad);
            }
            if (!string.IsNullOrEmpty(set))
            {
                if (set.Contains("RPM_DataFile_unknown7"))
                {
                    set = set.Replace("RPM_DataFile_unknown8", "");
                    source = source.Where(dataFile => dataFile.SetName.Contains(set));
                }
                else
                    source = source.Where(dataFile => dataFile.SetName == set);
            }
            if (bEvidence)
                source = source.Where(dataFile => dataFile.IsEvidence == true);
            return source.OrderBy(dataFile => dataFile.AccountId).ToList();
        }

        
        public void SetAccountId(Guid Id)
        {
            AccountId = Id;
        }

        
        private void WriteLog(VMGlobal.LOG_ACTION action, string memo, Guid Id)
        {
            if (Id != Guid.Empty)
            {
                Account account = context.Accounts.Find(Id);
                memo = string.Format("RPM_DataFile_unknown9", account.ToString(), account.BadgeNumber) + memo;
            }
            AccountLog log = new AccountLog();
            log.Action = action.ToString();
            log.Memo = memo;
            using (RPM_Logs rpmLogs = new RPM_Logs())
                rpmLogs.LogAccount(log);
        }

        
        public List<DataFile> GetSetList(Guid AccountID, string SetName, DateTime From, DateTime To)
        {
            List<DataFile> dataFileList = new List<DataFile>();
            IQueryable<DataFile> source = context.DataFiles.Where(dataFile => dataFile.AccountId == AccountID && dataFile.SetName.Contains(SetName) && dataFile.FileTimestamp >= From && dataFile.FileTimestamp <= To && dataFile.IsPurged == false).GroupBy(dataFile => dataFile.SetName).Select(grouping => grouping.FirstOrDefault());
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_DataFile_unknown10", SetName, From, To), AccountID);
            return source.ToList();
        }

        
        public List<DataFile> LoadByDate(Guid AccountID, DateTime From, DateTime To)
        {
            List<DataFile> dataFileList = new List<DataFile>();
            IQueryable<DataFile> source = context.DataFiles.Where(dataFile => dataFile.AccountId == AccountID && dataFile.FileAddedTimestamp >= From && dataFile.FileAddedTimestamp <= To && dataFile.IsPurged == false);
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_DataFile_unknown11", From, To), AccountID);
            return source.ToList();
        }

        
        public List<DataFile> GetSet(Guid AccountID, string SetName)
        {
            IQueryable<DataFile> source = context.DataFiles.Where(dataFile => dataFile.AccountId == AccountID && dataFile.SetName == SetName && dataFile.IsPurged == false);
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_DataFile_unknown12", SetName), AccountID);
            return source.ToList();
        }

        
        public FileMemo GetMemo(Guid Id)
        {
            FileMemo fileMemo = context.FileMemos.Find(Id);
            WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("RPM_DataFile_unknown13", fileMemo.ShortDesc, fileMemo.Timestamp, fileMemo.DataFile.Id, fileMemo.DataFile.ShortDesc), Guid.Empty);
            return fileMemo;
        }

        
        public int GetMemoCount(Guid Id)
        {
            return context.FileMemos.Count(fileMemo => fileMemo.DataFile.Id == Id);
        }

        
        public void ViewFileData(Guid Id)
        {
            DataFile dataFile = context.DataFiles.Find(Id);
            WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("RPM_DataFile_unknown14", dataFile.ShortDesc, dataFile.StoredFileName, dataFile.FileExtension), dataFile.AccountId);
        }

        
        public List<DataFile> QueryDataFile(CatalogFilter filter)
        {
            List<DataFile> dataFileList = new List<DataFile>();
            IQueryable<DataFile> source1 = context.DataFiles.Where(dataFile => dataFile.AccountId.Equals(filter.AccountID));
            if (!string.IsNullOrEmpty(filter.FileType))
                source1 = source1.Where(dataFile => dataFile.FileExtension == filter.FileType);
            if (!string.IsNullOrEmpty(filter.Classification))
                source1 = source1.Where(dataFile => dataFile.Classification == filter.Classification);
            if (!string.IsNullOrEmpty(filter.RMS))
                source1 = source1.Where(dataFile => dataFile.RMSNumber == filter.RMS);
            if (!string.IsNullOrEmpty(filter.CAD))
                source1 = source1.Where(dataFile => dataFile.CADNumber == filter.CAD);
            if (filter.Rating > 0)
                source1 = source1.Where(dataFile => dataFile.Rating == filter.Rating);
            if (filter.IsSecurityFilter)
                source1 = source1.Where(dataFile => (int)dataFile.Security == (int)filter.SecurityLevel);
            if (filter.IsEvidence)
                source1 = source1.Where(dataFile => dataFile.IsEvidence == filter.IsEvidence);
            if (!string.IsNullOrEmpty(filter.WordPhrase))
                source1 = source1.Where(dataFile => dataFile.ShortDesc.Contains(filter.WordPhrase));
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_DataFile_unknown15", filter.StartDate, filter.EndDate, filter.Classification), filter.AccountID);
            return source1.Where(dataFile => dataFile.IsPurged == false).Where(dataFile => dataFile.FileTimestamp >= filter.StartDate && dataFile.FileTimestamp <= filter.EndDate).OrderByDescending(dataFile => dataFile.FileTimestamp).ToList();
        }

        
        public List<DataFile> GetListByDateRange(DateTime d1, DateTime d2)
        {
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_DataFile_unknown16", d1, d2), Guid.Empty);
            return context.DataFiles.Where(dataFile => dataFile.FileTimestamp >= d1 && dataFile.IsPurged == false && dataFile.FileTimestamp <= d2).ToList();
        }

        
        public List<DataFile> GetListByDateRange(Guid Id, DateTime d1, DateTime d2)
        {
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_DataFile_unknown17", d1, d2), Id);
            return context.DataFiles.Where(dataFile => dataFile.AccountId.Equals(Id) && dataFile.IsPurged == false && dataFile.FileTimestamp >= d1 && dataFile.FileTimestamp <= d2).OrderByDescending(dataFile => dataFile.FileTimestamp).ToList();
        }

        
        public List<DataFile> GetAll()
        {
            WriteLog(VMGlobal.LOG_ACTION.LIST, "RPM_DataFile_unknown18", Guid.Empty);
            return context.DataFiles.Where(dataFile => dataFile.IsPurged == false).OrderBy(dataFile => dataFile.FileTimestamp).ToList();
        }

        
        public List<string> GetFileExtensions()
        {
            List<string> stringList = new List<string>();
            return context.DataFiles.GroupBy(dataFile => dataFile.FileExtension, dataFile => dataFile.FileExtension2).Select(grouping => grouping.FirstOrDefault()).ToList();
        }

        
        public List<DataFile> GetLastDays(Guid Id, int n)
        {
            DateTime CurrentDT = DateTime.Now;
            DateTime StartDT = CurrentDT.AddDays(-n);
            WriteLog(VMGlobal.LOG_ACTION.LIST, string.Format("RPM_DataFile_unknown19", n, CurrentDT, StartDT), Id);
            return context.DataFiles.Where(dataFile => dataFile.AccountId.Equals(Id) && dataFile.IsPurged == false && dataFile.FileTimestamp >= StartDT && dataFile.FileTimestamp <= CurrentDT).ToList();
        }

        
        public void GroupSet(List<DataFile> files, string SetName)
        {
        }

        
        public List<DataFile> GetBySetName(string SetName)
        {
            IQueryable<DataFile> source = context.DataFiles.Where(dataFile => dataFile.IsPurged == false && dataFile.SetName.Equals(SetName));
            WriteLog(VMGlobal.LOG_ACTION.VIEW, string.Format("RPM_DataFile_unknown20", SetName), Guid.Empty);
            return source.ToList();
        }

        
        public DataFile GetDataFile(Guid Id)
        {
            return context.DataFiles.Find(Id);
        }

        
        public void DeleteDataFile(Guid Id)
        {
            DeleteDataFile(GetDataFile(Id));
        }

        
        public void DeleteDataFile(DataFile rec)
        {
            WriteLog(VMGlobal.LOG_ACTION.DELETE, string.Format("RPM_DataFile_unknown21", rec.Id, rec.StoredFileName, rec.FileExtension), rec.AccountId);
            context.DataFiles.Remove(rec);
        }

        
        public void SaveUpdate(DataFile rec)
        {
            if (rec.Id == Guid.Empty)
            {
                WriteLog(VMGlobal.LOG_ACTION.UPLOAD, string.Format("RPM_DataFile_unknown22", rec.StoredFileName, rec.FileExtension, rec.OriginalFileName, rec.SourcePath), rec.AccountId);
                context.DataFiles.Add(rec);
            }
            else
            {
                WriteLog(VMGlobal.LOG_ACTION.UPDATE, string.Format("RPM_DataFile_unknown23", rec.StoredFileName, rec.FileExtension), rec.AccountId);
                context.Entry(rec).State = EntityState.Modified;
            }
        }

        
        public bool Save()
        {
            bool flag = false;
            using (DbContextTransaction contextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SaveChanges();
                    contextTransaction.Commit();
                    flag = true;
                }
                catch (DbUpdateException ex)
                {
                    contextTransaction.Rollback();
                    string message = ex.Message;
                    ex.InnerException.ToString();
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("RPM_DataFile_unknown21", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }

        
        public void Dispose()
        {
            context.Dispose();
        }

        
        void IDisposable.Dispose()
        {
            context.Dispose();
        }
    }
}
