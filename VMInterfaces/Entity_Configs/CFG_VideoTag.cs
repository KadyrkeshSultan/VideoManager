using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_VideoTag : EntityTypeConfiguration<VideoTag>
    {
        public CFG_VideoTag()
        {
            HasKey(videoTag => videoTag.Id).Property(videoTag => videoTag.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(videoTag => videoTag.ShortDesc).HasMaxLength(new int?(64));
        }
    }
}
