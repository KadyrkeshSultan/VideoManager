using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_AccountLog :EntityTypeConfiguration<AccountLog>
    {
        public CFG_AccountLog()
        {
            HasKey(accountLog => accountLog.Id).Property(accountLog => accountLog.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(accountLog => accountLog.AccountName).IsRequired().HasMaxLength(new int?(64));
            Property(accountLog => accountLog.BadgeNumber).IsRequired().HasMaxLength(new int?(16));
            Property(accountLog => accountLog.DomainName).IsRequired().HasMaxLength(new int?(64));
            Property(accountLog => accountLog.MachineName).IsRequired().HasMaxLength(new int?(64));
            Property(accountLog => accountLog.MachineAccount).IsRequired().HasMaxLength(new int?(32));
            Property(accountLog => accountLog.IPAddress).IsRequired().HasMaxLength(new int?(48));
            Property(accountLog => accountLog.MachineID).IsRequired().HasMaxLength(new int?(64));
            Property(accountLog => accountLog.Action).IsRequired().HasMaxLength(new int?(16));
        }
    }
}
