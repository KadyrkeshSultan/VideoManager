using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Manufacturer : EntityTypeConfiguration<Manufacturer>
    {
        public CFG_Manufacturer()
        {
            HasKey(manufacturer => manufacturer.Id).Property(manufacturer => manufacturer.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(manufacturer => manufacturer.Name).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_Manufacturer_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(manufacturer => manufacturer.Address).HasMaxLength(new int?(128));
            Property(manufacturer => manufacturer.Contact).HasMaxLength(new int?(64));
            Property(manufacturer => manufacturer.CSZ).HasMaxLength(new int?(128));
            Property(manufacturer => manufacturer.Phone).HasMaxLength(new int?(16));
            Property(manufacturer => manufacturer.Web).HasMaxLength(new int?(128));
        }
    }
}
