using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Alias:EntityTypeConfiguration<Alias>
    {
        public CFG_Alias()
        {
            HasKey(alias => alias.Id).Property(alias => alias.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(alias => alias.FirstName).IsRequired().HasMaxLength(new int?(32));
            Property(alias => alias.LastName).IsRequired().HasMaxLength(new int?(32));
            Property(alias => alias.MiddleName).HasMaxLength(new int?(32));
            Property(alias => alias.NickName).HasMaxLength(new int?(64));
        }
    }
}
