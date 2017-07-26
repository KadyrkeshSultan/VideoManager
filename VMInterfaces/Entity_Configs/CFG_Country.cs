using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Country : EntityTypeConfiguration<Country>
    {
        public CFG_Country()
        {
            HasKey(country => country.Id).Property(country => country.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(country => country.Name).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_Country_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(country => country.CountryCode).HasMaxLength(new int?(8));
        }
    }
}
