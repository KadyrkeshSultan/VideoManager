using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Account:EntityTypeConfiguration<Account>
    {
        public CFG_Account()
        {
            HasKey((Account d) => d.Id).Property((Account d) => d.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            StringPropertyConfiguration stringPropertyConfiguration = Property((Account d) => d.BadgeNumber).IsRequired().HasMaxLength(new int?(16));
            IndexAttribute indexAttribute = new IndexAttribute()
            {
                IsUnique = true
            };
            stringPropertyConfiguration.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute));
            Property((Account d) => d.FirstName).IsRequired().HasMaxLength(new int?(64));
            Property((Account d) => d.LastName).IsRequired().HasMaxLength(new int?(64));
            Property((Account d) => d.MiddleName).HasMaxLength(new int?(64));
            Property((Account d) => d.Rank).HasMaxLength(new int?(32));
            StringPropertyConfiguration stringPropertyConfiguration1 = Property((Account d) => d.LogonID).IsRequired().HasMaxLength(new int?(32));
            IndexAttribute indexAttribute1 = new IndexAttribute()
            {
                IsUnique = true
            };
            stringPropertyConfiguration1.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute1));
            Property((Account d) => d.Password).IsRequired().HasMaxLength(new int?(32));
            Property((Account d) => d.PIN).HasMaxLength(new int?(8));
            Property((Account d) => d.Photo).HasColumnType("image");
            Property((Account d) => d.Email).HasMaxLength(new int?(64));
        }
    }
}
