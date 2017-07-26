using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Snapshot : EntityTypeConfiguration<Snapshot>
    {
        public CFG_Snapshot()
        {
            HasKey(snapshot => snapshot.Id).Property(snapshot => snapshot.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(snapshot => snapshot.UNCName).IsRequired().HasMaxLength(new int?(256));
            Property(snapshot => snapshot.UNCPath).IsRequired().HasMaxLength(new int?(256));
            Property(snapshot => snapshot.StoredFileName).IsRequired().HasMaxLength(new int?(128));
            Property(snapshot => snapshot.FileExtension).IsRequired().HasMaxLength(new int?(8));
            Property(snapshot => snapshot.Thumbnail).HasColumnType("CFG_Snapshot_unknown1");
            Property(snapshot => snapshot.GPS).HasMaxLength(new int?(64));
            Property(snapshot => snapshot.FileHash).HasMaxLength(new int?(64));
        }
    }
}
