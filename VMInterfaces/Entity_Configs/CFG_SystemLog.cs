using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_SystemLog : EntityTypeConfiguration<SystemLog>
    {
        public CFG_SystemLog()
        {
            HasKey(systemLog => systemLog.Id).Property(systemLog => systemLog.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(systemLog => systemLog.DomainName).IsRequired().HasMaxLength(new int?(64));
            Property(systemLog => systemLog.MachineName).IsRequired().HasMaxLength(new int?(64));
            Property(systemLog => systemLog.MachineAccount).IsRequired().HasMaxLength(new int?(32));
            Property(systemLog => systemLog.IPAddress).IsRequired().HasMaxLength(new int?(48));
            Property(systemLog => systemLog.MachineID).IsRequired().HasMaxLength(new int?(64));
            Property(systemLog => systemLog.Action).IsRequired().HasMaxLength(new int?(16));
        }
    }
}
