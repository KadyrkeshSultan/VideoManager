using System;

namespace VideoManager.Model
{
    class RMA
    {
        public Guid Id { get; set; }
        public string RMA_Number { get; set; }
        public DateTime? RMA_Date { get; set; }
        public string TrackingID { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string Description { get; set; }
        public bool IsRepair { get; set; }
        public string Memo { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompleteDate { get; set; }
    }
}
