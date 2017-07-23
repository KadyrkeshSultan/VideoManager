using System;

namespace VideoManager.Model
{
    class CameraLog
    {
        public Guid Id { get; set; }
        public DateTime? LogTimestamp { get; set; }
        public string DomainName { get; set; }
        public string MachineName { get; set; }
        public string MachineAccount { get; set; }
        public string IPAddress { get; set; }
        public string MachineID { get; set; }
        public string SerialNumber { get; set; }
        public string AssetTag { get; set; }
        public Guid AccountID { get; set; }
        public string AccountName { get; set; }
        public string BudgeNumber { get; set; }
        public int Battery { get; set; }
        public double DiskSpace { get; set; }
        public string Memo { get; set; }
        public string Action { get; set; }
        public int FileCount { get; set; }
    }
}
