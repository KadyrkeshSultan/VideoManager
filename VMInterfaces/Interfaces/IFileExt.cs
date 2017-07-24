using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface IFileExt : IDisposable
    {
        List<FileExt> GetFileExtensions();

        void Delete(Guid Id);

        void Delete(FileExt rec);

        void InsertUpdate(FileExt rec);

        FileExt Find(Guid Id);

        bool Save();
    }
}
