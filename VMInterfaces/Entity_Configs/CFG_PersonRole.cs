using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_PersonRole : EntityTypeConfiguration<PersonRole>
    {
        public CFG_PersonRole()
        {
            HasKey(personRole => personRole.Id).Property(personRole => personRole.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(personRole => personRole.RoleName).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_PersonRole_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
        }
    }
}
