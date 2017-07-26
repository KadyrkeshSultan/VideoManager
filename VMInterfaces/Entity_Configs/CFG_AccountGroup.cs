using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_AccountGroup: EntityTypeConfiguration<AccountGroup>
    {
        public CFG_AccountGroup()
        {
            HasKey(accountGroup => accountGroup.Id).Property(accountGroup => accountGroup.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(accountGroup => accountGroup.Name).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_AccountGroup_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(accountGroup => accountGroup.Desc).HasMaxLength(new int?(128));
        }
    }
}
