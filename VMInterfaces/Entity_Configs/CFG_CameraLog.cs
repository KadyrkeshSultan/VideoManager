using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_CameraLog: EntityTypeConfiguration<CameraLog>
    {
        public CFG_CameraLog()
        {
            HasKey(cameraLog => cameraLog.Id).Property(cameraLog => cameraLog.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(cameraLog => cameraLog.AccountName).IsRequired().HasMaxLength(new int?(64));
            Property(cameraLog => cameraLog.BadgeNumber).IsRequired().HasMaxLength(new int?(16));
            Property(cameraLog => cameraLog.DomainName).IsRequired().HasMaxLength(new int?(64));
            Property(cameraLog => cameraLog.MachineName).IsRequired().HasMaxLength(new int?(64));
            Property(cameraLog => cameraLog.MachineAccount).IsRequired().HasMaxLength(new int?(32));
            Property(cameraLog => cameraLog.IPAddress).IsRequired().HasMaxLength(new int?(48));
            Property(cameraLog => cameraLog.MachineID).IsRequired().HasMaxLength(new int?(64));
            Property(cameraLog => cameraLog.Action).IsRequired().HasMaxLength(new int?(16));
            Property(cameraLog => cameraLog.SerialNumber).HasMaxLength(new int?(48));
            Property(cameraLog => cameraLog.AssetTag).HasMaxLength(new int?(48));
        }
    }
}
