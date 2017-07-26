using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_PersonAddress : EntityTypeConfiguration<PersonAddress>
    {
        public CFG_PersonAddress()
        {
            HasKey(personAddress => personAddress.Id).Property(personAddress => personAddress.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(personAddress => personAddress.Address1).HasMaxLength(new int?(64));
            Property(personAddress => personAddress.Address2).HasMaxLength(new int?(64));
            Property(personAddress => personAddress.City).HasMaxLength(new int?(64));
            Property(personAddress => personAddress.StateProvince).HasMaxLength(new int?(64));
            Property(personAddress => personAddress.PostalCode).HasMaxLength(new int?(32));
            Property(personAddress => personAddress.Country).HasMaxLength(new int?(64));
            Property(personAddress => personAddress.Phone1).HasMaxLength(new int?(16));
            Property(personAddress => personAddress.Phone2).HasMaxLength(new int?(16));
        }
    }
}
