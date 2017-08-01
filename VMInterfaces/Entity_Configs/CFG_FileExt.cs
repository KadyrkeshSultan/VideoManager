using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_FileExt : EntityTypeConfiguration<FileExt>
    {
        public CFG_FileExt()
        {
            HasKey(fileExt => fileExt.Id).Property(fileExt => fileExt.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(fileExt => fileExt.Ext).IsRequired().HasMaxLength(new int?(8)).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(fileExt => fileExt.Pattern).HasMaxLength(new int?(64));
        }
    }
}
