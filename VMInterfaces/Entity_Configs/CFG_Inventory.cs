using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Inventory : EntityTypeConfiguration<Inventory>
    {
        public CFG_Inventory()
        {
            HasKey(inventory => inventory.Id).Property(inventory => inventory.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(inventory => inventory.TrackingID).HasMaxLength(new int?(64));
            Property(inventory => inventory.AssetTag).HasMaxLength(new int?(32));
            Property(inventory => inventory.SerialNumber).HasMaxLength(new int?(32));
            Property(inventory => inventory.RMA_Number).IsRequired().HasMaxLength(new int?(64));
        }
    }
}
