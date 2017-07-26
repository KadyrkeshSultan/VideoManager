using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Substation : EntityTypeConfiguration<Substation>
    {
        public CFG_Substation()
        {
            HasKey(substation => substation.Id).Property(substation => substation.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(substation => substation.Name).IsRequired().HasMaxLength(new int?(64));
            Property(substation => substation.Address1).HasMaxLength(new int?(128));
            Property(substation => substation.Address2).HasMaxLength(new int?(128));
            Property(substation => substation.City).HasMaxLength(new int?(64));
            Property(substation => substation.State).HasMaxLength(new int?(64));
            Property(substation => substation.PostalCode).HasMaxLength(new int?(32));
            Property(substation => substation.Phone).HasMaxLength(new int?(16));
        }
    }
}
