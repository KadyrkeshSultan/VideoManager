using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_AppExt:EntityTypeConfiguration<AppExt>
    {
        public CFG_AppExt()
        {
            HasKey(appExt => appExt.Id).Property(appExt => appExt.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(appExt => appExt.Ext).IsRequired().HasMaxLength(new int?(8));
            Property(appExt => appExt.ExecType).IsRequired().HasMaxLength(new int?(32));
            Property(appExt => appExt.Desc).HasMaxLength(new int?(64));
            Property(appExt => appExt.Application).HasMaxLength(new int?((int)byte.MaxValue));
        }
    }
}
