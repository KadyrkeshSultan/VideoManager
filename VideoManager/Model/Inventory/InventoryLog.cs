using System;
using VideoManager.Enums;

namespace VideoManager.Model
{
    public class InventoryLog
    {
        public Guid Id { get; set; }
        public Guid InvIdx { get; set; }
        public Guid AccountID { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public INV_ACTION Action { get; set; }
        public int Battery { get; set; }
        public int DiskFree { get; set; }
        public int FileCount { get; set; }
    }
}
