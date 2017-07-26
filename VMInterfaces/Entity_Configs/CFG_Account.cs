using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Account:EntityTypeConfiguration<Account>
    {
        public CFG_Account()
        {
            HasKey(account => account.Id).Property(account => account.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(account => account.BadgeNumber).IsRequired().HasMaxLength(new int?(16)).HasColumnAnnotation("CFG_account_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(account => account.FirstName).IsRequired().HasMaxLength(new int?(64));
            Property(account => account.LastName).IsRequired().HasMaxLength(new int?(64));
            Property(account => account.MiddleName).HasMaxLength(new int?(64));
            Property(account => account.Rank).HasMaxLength(new int?(32));
            Property(account => account.LogonID).IsRequired().HasMaxLength(new int?(32)).HasColumnAnnotation("CFG_account_unknown2", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(account => account.Password).IsRequired().HasMaxLength(new int?(32));
            Property(account => account.PIN).HasMaxLength(new int?(8));
            Property(account => account.Photo).HasColumnType("CFG_account_unknown3");
            Property(account => account.Email).HasMaxLength(new int?(64));
        }
    }
}
