using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_CaseMemo:EntityTypeConfiguration<CaseMemo>
    {
        public CFG_CaseMemo()
        {
            HasKey(caseMemo => caseMemo.Id).Property(caseMemo => caseMemo.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(caseMemo => caseMemo.AccountName).IsRequired().HasMaxLength(new int?(64));
            Property(caseMemo => caseMemo.BadgeNumber).IsRequired().HasMaxLength(new int?(16));
            Property(caseMemo => caseMemo.ShortDesc).IsRequired().HasMaxLength(new int?(64));
        }
    }
}
