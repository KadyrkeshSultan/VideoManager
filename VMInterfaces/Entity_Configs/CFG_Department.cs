using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Department : EntityTypeConfiguration<Department>
    {
        public CFG_Department()
        {
            HasKey((Department d) => d.Id).Property((Department d) => d.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            StringPropertyConfiguration stringPropertyConfiguration = Property((Department d) => d.Name).IsRequired().HasMaxLength(new int?(64));
            IndexAttribute indexAttribute = new IndexAttribute()
            {
                IsUnique = true
            };
            stringPropertyConfiguration.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute));
            Property((Department d) => d.Address1).HasMaxLength(new int?(128));
            Property((Department d) => d.Address2).HasMaxLength(new int?(128));
            Property((Department d) => d.City).HasMaxLength(new int?(64));
            Property((Department d) => d.State).HasMaxLength(new int?(64));
            Property((Department d) => d.PostalCode).HasMaxLength(new int?(32));
            Property((Department d) => d.Phone1).HasMaxLength(new int?(16));
            Property((Department d) => d.Phone2).HasMaxLength(new int?(16));
            Property((Department d) => d.Logo).HasColumnType("image");
        }
    }
}