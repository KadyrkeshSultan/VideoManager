using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_AlertEmail:EntityTypeConfiguration<AlertEmail>
    {
        public CFG_AlertEmail()
        {
            HasKey(alertEmail => alertEmail.Id).Property(alertEmail => alertEmail.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(alertEmail => alertEmail.Subject).IsRequired().HasMaxLength(new int?(64));
        }
    }
}
