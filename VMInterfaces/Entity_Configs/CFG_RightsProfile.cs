using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_RightsProfile : EntityTypeConfiguration<RightsProfile>
    {
        public CFG_RightsProfile()
        {
            HasKey(rightsProfile => rightsProfile.Id).Property(rightsProfile => rightsProfile.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(rightsProfile => rightsProfile.Name).IsRequired().HasMaxLength(new int?(32)).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(rightsProfile => rightsProfile.Desc).HasMaxLength(new int?(64));
        }
    }
}
