using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    internal class CFG_CameraFolder : EntityTypeConfiguration<CameraFolder>
    {
        public CFG_CameraFolder()
        {
            HasKey((CameraFolder d) => d.Id).Property((CameraFolder d) => d.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            StringPropertyConfiguration stringPropertyConfiguration = Property((CameraFolder d) => d.Folder).IsRequired().HasMaxLength(new int?(256));
            IndexAttribute indexAttribute = new IndexAttribute()
            {
                IsUnique = true
            };
            stringPropertyConfiguration.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute));
        }
    }
}