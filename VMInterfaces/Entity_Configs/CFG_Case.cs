using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Case : EntityTypeConfiguration<Case>
    {
        public CFG_Case()
        {
            HasKey((Case d) => d.Id).Property((Case d) => d.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            StringPropertyConfiguration stringPropertyConfiguration = Property((Case d) => d.CaseNumber).IsRequired().HasMaxLength(new int?(32));
            IndexAttribute indexAttribute = new IndexAttribute()
            {
                IsUnique = true
            };
            stringPropertyConfiguration.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute));
            Property((Case d) => d.PurgeFileName).HasMaxLength(new int?(256));
            Property((Case d) => d.FirstName).IsRequired().HasMaxLength(new int?(32));
            Property((Case d) => d.LastName).IsRequired().HasMaxLength(new int?(32));
            Property((Case d) => d.MiddleName).HasMaxLength(new int?(32));
            Property((Case d) => d.Gender).IsRequired().HasMaxLength(new int?(1));
            Property((Case d) => d.SSN).HasMaxLength(new int?(16));
            Property((Case d) => d.HairColor).HasMaxLength(new int?(16));
            Property((Case d) => d.EyeColor).HasMaxLength(new int?(16));
            Property((Case d) => d.PassportID).HasMaxLength(new int?(64));
            Property((Case d) => d.Country).HasMaxLength(new int?(64));
            Property((Case d) => d.LicenseID).HasMaxLength(new int?(64));
            Property((Case d) => d.LicenseState).HasMaxLength(new int?(64));
            Property((Case d) => d.Email).HasMaxLength(new int?(128));
            Property((Case d) => d.ResolutionCode).HasMaxLength(new int?(32));
            Property((Case d) => d.ResolutionDesc).HasMaxLength(new int?(64));
            Property((Case d) => d.CasePicture).HasColumnType("image");
            Property((Case d) => d.DLPicture).HasColumnType("image");
            Property((Case d) => d.PassportPicture).HasColumnType("image");
        }
    }
}