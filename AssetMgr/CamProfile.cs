using System;

namespace AssetMgr
{
    [Serializable]
    public class CamProfile
    {
        public Guid InventoryID {  get;  set; }

        public string TrackingID {  get;  set; }

        public string AssetTag {  get;  set; }

        public string SerialNumber {  get;  set; }

        public DateTime Timestamp {  get;  set; }

        
        public CamProfile()
        {
        }

        
        public CamProfile(Guid InvID, string TrackID, string aTag, string sn)
        {
            this.Timestamp = DateTime.Now;
            this.InventoryID = InvID;
            this.TrackingID = TrackID;
            this.AssetTag = aTag;
            this.SerialNumber = sn;
        }
    }
}
