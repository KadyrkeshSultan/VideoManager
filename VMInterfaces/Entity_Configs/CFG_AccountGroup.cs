using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_AccountGroup : EntityTypeConfiguration<AccountGroup>
    {
        public CFG_AccountGroup()
        {
            HasKey((AccountGroup d) => d.Id).Property((AccountGroup d) => d.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            StringPropertyConfiguration stringPropertyConfiguration = Property((AccountGroup d) => d.Name).IsRequired().HasMaxLength(new int?(64));
            IndexAttribute indexAttribute = new IndexAttribute()
            {
                IsUnique = true
            };
            stringPropertyConfiguration.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute));
            Property((AccountGroup d) => d.Desc).HasMaxLength(new int?(128));
        }
    }
}