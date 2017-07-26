using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Classification : EntityTypeConfiguration<Classification>
    {
        public CFG_Classification()
        {
            HasKey(classification => classification.Id).Property(classification => classification.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(classification => classification.Name).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_Classification_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(classification => classification.Code).HasMaxLength(new int?(16));
        }
    }
}
