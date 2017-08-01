using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_GlobalConfig : EntityTypeConfiguration<GlobalConfig>
    {
        public CFG_GlobalConfig()
        {
            HasKey(globalConfig => globalConfig.Id).Property(globalConfig => globalConfig.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(globalConfig => globalConfig.Key).IsRequired().HasMaxLength(new int?(32)).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(globalConfig => globalConfig.Value).HasMaxLength(new int?(256));
            Property(globalConfig => globalConfig.Desc).HasMaxLength(new int?(128));
            Property(globalConfig => globalConfig.DataType).IsRequired().HasMaxLength(new int?(16));
        }
    }
}
