using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_CaseAddress:EntityTypeConfiguration<CaseAddress>
    {
        public CFG_CaseAddress()
        {
            HasKey(caseAddress => caseAddress.Id).Property(caseAddress => caseAddress.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(caseAddress => caseAddress.Address1).HasMaxLength(new int?(64));
            Property(caseAddress => caseAddress.Address2).HasMaxLength(new int?(64));
            Property(caseAddress => caseAddress.City).HasMaxLength(new int?(64));
            Property(caseAddress => caseAddress.StateProvince).HasMaxLength(new int?(64));
            Property(caseAddress => caseAddress.PostalCode).HasMaxLength(new int?(32));
            Property(caseAddress => caseAddress.Country).HasMaxLength(new int?(64));
            Property(caseAddress => caseAddress.Phone1).HasMaxLength(new int?(16));
            Property(caseAddress => caseAddress.Phone2).HasMaxLength(new int?(16));
        }
    }
}
