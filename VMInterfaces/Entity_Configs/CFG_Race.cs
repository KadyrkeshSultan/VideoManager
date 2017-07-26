using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Race : EntityTypeConfiguration<Race>
    {
        public CFG_Race()
        {
            HasKey(race => race.Id).Property(race => race.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(race => race.Name).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_Race_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
        }
    }
}
