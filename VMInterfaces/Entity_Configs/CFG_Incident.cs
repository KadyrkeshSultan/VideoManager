using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Incident : EntityTypeConfiguration<Incident>
    {
        public CFG_Incident()
        {
            HasKey(incident => incident.Id).Property(incident => incident.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(incident => incident.Address).HasMaxLength(new int?(128));
            Property(incident => incident.City).HasMaxLength(new int?(64));
            Property(incident => incident.StateProvince).HasMaxLength(new int?(64));
            Property(incident => incident.PostalCode).HasMaxLength(new int?(32));
            Property(incident => incident.GPS).HasMaxLength(new int?(64));
        }
    }
}
