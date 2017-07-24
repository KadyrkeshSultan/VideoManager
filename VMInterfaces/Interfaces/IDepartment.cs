using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    public interface IDepartment : IDisposable
    {
        List<Department> GetDeptList();

        Department GetDept(Guid Id);

        void DeleteDept(Guid Id);

        void DeleteDept(Department rec);

        void InsertUpdate(Department rec);

        List<Substation> GetSubstationList(Guid Id);

        Substation GetSubstation(Guid Id);

        void DeleteSub(Guid Id);

        void DeleteSub(Substation rec);

        void InsertUpdate(Substation rec);

        bool Save();
    }
}
