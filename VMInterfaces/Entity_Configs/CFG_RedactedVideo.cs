using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_RedactedVideo : EntityTypeConfiguration<RedactedVideo>
    {
        public CFG_RedactedVideo()
        {
            HasKey(redactedVideo => redactedVideo.Id).Property(redactedVideo => redactedVideo.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(redactedVideo => redactedVideo.UNCPath).IsRequired().HasMaxLength(new int?(256));
            Property(redactedVideo => redactedVideo.Thumbnail).HasColumnType("CFG_RedactedVideo_unknown1");
            Property(redactedVideo => redactedVideo.Title).IsRequired().HasMaxLength(new int?(32));
            Property(redactedVideo => redactedVideo.Desc).HasMaxLength(new int?(64));
            Property(redactedVideo => redactedVideo.FileName).IsRequired().HasMaxLength(new int?(128));
            Property(redactedVideo => redactedVideo.MachineName).HasMaxLength(new int?(32));
            Property(redactedVideo => redactedVideo.MachineAccount).HasMaxLength(new int?(32));
            Property(redactedVideo => redactedVideo.LoginID).HasMaxLength(new int?(32));
            Property(redactedVideo => redactedVideo.UserDomain).HasMaxLength(new int?(32));
        }
    }
}
