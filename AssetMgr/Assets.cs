using System;
using System.IO;
using Unity;
using VMInterfaces;
using VMModels.Model;

namespace AssetMgr
{
    public class Assets
    {
        // TODO : Надо будет поменять имя файла
        private const string ASSETFILE = "C3Sentinel.Dat";

        
        public Assets()
        {
        }

        
        public bool IsProfile(string DriveID)
        {
            return File.Exists(Path.Combine(DriveID, "C3Sentinel.Dat"));
        }

        
        public bool SaveAssetProfile(string DriveID, CamProfile rec)
        {
            FileCrypto.Save((object)rec, Path.Combine(DriveID, "C3Sentinel.Dat"));
            new FileInfo(Path.Combine(DriveID, "C3Sentinel.Dat")).Attributes = FileAttributes.Hidden;
            return IsProfile(DriveID);
        }

        
        public CamProfile ReadAssetProfile(string DriveID)
        {
            CamProfile camProfile = new CamProfile();
            if (IsProfile(DriveID))
                camProfile = (CamProfile)FileCrypto.LoadConfig(Path.Combine(DriveID, "C3Sentinel.Dat")) ?? new CamProfile();
            return camProfile;
        }

        
        public void DeleteProfile(string DriveID)
        {
            if (!IsProfile(DriveID))
                return;
            File.Delete(Path.Combine(DriveID, "C3Sentinel.Dat"));
        }

        
        public Account GetProfileAccount(string DriveID)
        {
            Account account = null;
            Guid id = Guid.Empty;
            CamProfile camProfile = this.ReadAssetProfile(DriveID);
            if (camProfile != null)
            {
                using (RPM_Product rpmProduct = new RPM_Product())
                {
                    Inventory inventory = rpmProduct.GetInventory(camProfile.InventoryID);
                    if (inventory != null)
                        id = inventory.AccountID;
                }
            }
            if (id != Guid.Empty)
            {
                using (RPM_Account rpmAccount = new RPM_Account())
                    account = rpmAccount.GetAccount(id);
            }
            return account;
        }

        
        public Guid GetAccountID(string DriveID)
        {
            Guid guid = Guid.Empty;
            Account profileAccount = this.GetProfileAccount(DriveID);
            if (profileAccount != null)
                guid = profileAccount.Id;
            return guid;
        }

        
        public CamProfile BuildRecord(Guid InvID, string TrackID, string aTag, string sn)
        {
            return new CamProfile()
            {
                Timestamp = DateTime.Now,
                InventoryID = InvID,
                TrackingID = TrackID,
                AssetTag = aTag,
                SerialNumber = sn
            };
        }
    }
}
