using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_FileMemo : EntityTypeConfiguration<FileMemo>
    {
        public CFG_FileMemo()
        {
            HasKey(fileMemo => fileMemo.Id).Property(fileMemo => fileMemo.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(fileMemo => fileMemo.AccountName).IsRequired().HasMaxLength(new int?(64));
            Property(fileMemo => fileMemo.BadgeNumber).IsRequired().HasMaxLength(new int?(16));
            Property(fileMemo => fileMemo.ShortDesc).HasMaxLength(new int?(64));
        }
    }
}
