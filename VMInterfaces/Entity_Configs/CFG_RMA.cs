using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_RMA : EntityTypeConfiguration<RMA>
    {
        public CFG_RMA()
        {
            HasKey(rma => rma.Id).Property(rma => rma.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(rma => rma.RMA_Number).IsRequired().HasMaxLength(new int?(64));
            Property(rma => rma.TrackingID).HasMaxLength(new int?(64));
            Property(rma => rma.AssetTag).HasMaxLength(new int?(32));
            Property(rma => rma.SerialNumber).HasMaxLength(new int?(32));
            Property(rma => rma.Description).HasMaxLength(new int?(64));
        }
    }
}
