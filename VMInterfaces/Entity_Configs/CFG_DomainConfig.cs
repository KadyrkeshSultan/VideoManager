using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_DomainConfig : EntityTypeConfiguration<DomainConfig>
    {
        public CFG_DomainConfig()
        {
            HasKey(domainConfig => domainConfig.Id).Property(domainConfig => domainConfig.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(domainConfig => domainConfig.ProfileName).IsRequired().HasMaxLength(new int?(32)).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(domainConfig => domainConfig.ServerName).IsRequired().HasMaxLength(new int?(32));
            Property(domainConfig => domainConfig.Description).HasMaxLength(new int?(128));
            Property(domainConfig => domainConfig.UNCRoot).HasMaxLength(new int?(256));
            Property(domainConfig => domainConfig.UNCPath).HasMaxLength(new int?(256));
            Property(domainConfig => domainConfig.PurgePath).HasMaxLength(new int?(256));
            Property(domainConfig => domainConfig.RecoveryPath).HasMaxLength(new int?(256));
            Property(domainConfig => domainConfig.DomainLogon).HasMaxLength(new int?(32));
            Property(domainConfig => domainConfig.LogonPassword).HasMaxLength(new int?(32));
            Property(domainConfig => domainConfig.AlertSubject).HasMaxLength(new int?(64));
        }
    }
}
