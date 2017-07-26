using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_CameraFolder:EntityTypeConfiguration<CameraFolder>
    {
        public CFG_CameraFolder()
        {
            HasKey(cameraFolder => cameraFolder.Id).Property(cameraFolder => cameraFolder.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(cameraFolder => cameraFolder.Folder).IsRequired().HasMaxLength(new int?(256)).HasColumnAnnotation("CFG_CameraFolder_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
        }
    }
}
