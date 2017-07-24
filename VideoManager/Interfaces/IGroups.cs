﻿using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IGroups : IDisposable
    {
        List<AccountGroup> ListGroups();

        AccountGroup GetGroup(Guid Id);

        void DeleteGroup(Guid Id);

        void DeleteGroup(AccountGroup rec);

        void InsertUpdate(AccountGroup rec);

        void ClearMembers(Guid RecIdx);

        void ClearGroups(Guid ActIdx);

        void AddMember(Guid GrpIdx, Guid ActIdx);

        List<AccountGroup> GetGroupsByAccount(Guid Id);

        bool Save();
    }
}
