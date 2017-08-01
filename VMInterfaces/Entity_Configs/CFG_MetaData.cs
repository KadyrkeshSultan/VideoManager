using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_MetaData : EntityTypeConfiguration<MetaData>
    {
        public CFG_MetaData()
        {
            HasKey(metaData => metaData.Id).Property(metaData => metaData.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(metaData => metaData.DataType).IsRequired().HasMaxLength(new int?(16));
            Property(metaData => metaData.DataPrompt).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
        }
    }
}
