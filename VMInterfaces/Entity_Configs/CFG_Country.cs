using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Country : EntityTypeConfiguration<Country>
    {
        public CFG_Country()
        {
            HasKey((Country d) => d.Id).Property((Country d) => d.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            StringPropertyConfiguration stringPropertyConfiguration = Property((Country d) => d.Name).IsRequired().HasMaxLength(new int?(64));
            IndexAttribute indexAttribute = new IndexAttribute()
            {
                IsUnique = true
            };
            stringPropertyConfiguration.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute));
            Property((Country d) => d.CountryCode).HasMaxLength(new int?(8));
        }
    }
}