using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IAccount :IDisposable
    {
        List<Account> GetAccountList();

        List<Account> GetAccountList(Guid id, Guid sId);

        List<Account> GetUnassignedList();

        List<Account> AccountSearch(string badge, string lastName);

        List<Account> GetAlertList();

        Account GetAccount(Guid id);

        bool UpdatePassword(Guid Id, string OldPwd, string NewPwd);

        bool UpdatePIN(Guid Id, string OldPIN, string NewPIN);

        void Delete(Guid Id);

        void Delete(Account rec);

        void ClearInventory(Guid Id);

        void ClearGroups(Guid Id);

        void InsertUpdate(Account rec);

        Account Authenticate(string loginId, string Pwd);

        string GetDeptName(Guid Id);

        string GetSubstationName(Guid Id);

        int AccountCount();

        void Logout();

        bool Save();
    }
}
