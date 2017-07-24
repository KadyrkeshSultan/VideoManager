using System;

namespace VMModels.Model
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public string TrackingID { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; } 
        public DateTime? DateAssingned { get; set; }
        public Guid AccountID { get; set; }
        public string RMA_Number { get; set; }
        public virtual Product Product { get; set; }
    }
}
