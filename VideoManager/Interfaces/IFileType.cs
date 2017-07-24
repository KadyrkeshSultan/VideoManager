using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IFileType : IDisposable
    {
        FileType Find(Guid Id);

        bool IsInternal(string ext);

        List<FileType> GetFileTypeList();

        void InsertUpdate(FileType rec);

        void Delete(Guid Id);

        void Delete(FileType rec);

        bool Save();
    }
}
