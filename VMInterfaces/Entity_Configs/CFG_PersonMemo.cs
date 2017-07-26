using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_PersonMemo : EntityTypeConfiguration<PersonMemo>
    {
        public CFG_PersonMemo()
        {
            HasKey(personMemo => personMemo.Id).Property(personMemo => personMemo.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(personMemo => personMemo.AccountName).IsRequired().HasMaxLength(new int?(64));
            Property(personMemo => personMemo.BadgeNumber).IsRequired().HasMaxLength(new int?(16));
            Property(personMemo => personMemo.ShortDesc).IsRequired().HasMaxLength(new int?(64));
        }
    }
}
