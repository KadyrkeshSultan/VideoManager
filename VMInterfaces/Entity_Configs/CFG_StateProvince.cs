using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_StateProvince : EntityTypeConfiguration<StateProvince>
    {
        public CFG_StateProvince()
        {
            HasKey(stateProvince => stateProvince.Id).Property(stateProvince => stateProvince.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(stateProvince => stateProvince.Name).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_StateProvince_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
        }
    }
}
