using System;

namespace VideoManager.Model
{
    public class DomainConfig
    {
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public string ProfileName { get; set; }
        public string ServerName { get; set; }
        public string Description { get; set; }
        public string UNCRoot { get; set; }
        public string UNCPath { get; set; }
        public string PurgePath { get; set; }
        public string RecoveryPath { get; set; }
        public string DomainLogon { get; set; }
        public string LogonPassword { get; set; }
        public int StorageThreshold { get; set; }
        public Guid SubstationID { get; set; }
        public Guid GroupID { get; set; }
        public bool IsAlert { get; set; }
        public string AlertSubject { get; set; }
        public string AlertBody { get; set; }
        public string DistList { get; set; }
    }
}
