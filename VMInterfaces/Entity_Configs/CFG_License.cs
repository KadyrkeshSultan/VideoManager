using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_License : EntityTypeConfiguration<License>
    {
        public CFG_License()
        {
            HasKey(license => license.Id).Property(license => license.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
        }
    }
}
