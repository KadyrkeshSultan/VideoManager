using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_InventoryLog : EntityTypeConfiguration<InventoryLog>
    {
        public CFG_InventoryLog()
        {
            HasKey(inventoryLog => inventoryLog.Id).Property(inventoryLog => inventoryLog.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(inventoryLog => inventoryLog.AssetTag).HasMaxLength(new int?(32));
            Property(inventoryLog => inventoryLog.SerialNumber).HasMaxLength(new int?(32));
        }
    }
}
