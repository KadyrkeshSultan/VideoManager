using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;


namespace VMInterfaces.Entity_Configs
{
    public class CFG_Case: EntityTypeConfiguration<Case>
    {
        public CFG_Case()
        {
            HasKey(@case => @case.Id).Property(@case => @case.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(@case => @case.CaseNumber).IsRequired().HasMaxLength(new int?(32)).HasColumnAnnotation("CFG_Case_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(@case => @case.PurgeFileName).HasMaxLength(new int?(256));
            Property(@case => @case.FirstName).IsRequired().HasMaxLength(new int?(32));
            Property(@case => @case.LastName).IsRequired().HasMaxLength(new int?(32));
            Property(@case => @case.MiddleName).HasMaxLength(new int?(32));
            Property(@case => @case.Gender).IsRequired().HasMaxLength(new int?(1));
            Property(@case => @case.SSN).HasMaxLength(new int?(16));
            Property(@case => @case.HairColor).HasMaxLength(new int?(16));
            Property(@case => @case.EyeColor).HasMaxLength(new int?(16));
            Property(@case => @case.PassportID).HasMaxLength(new int?(64));
            Property(@case => @case.Country).HasMaxLength(new int?(64));
            Property(@case => @case.LicenseID).HasMaxLength(new int?(64));
            Property(@case => @case.LicenseState).HasMaxLength(new int?(64));
            Property(@case => @case.Email).HasMaxLength(new int?(128));
            Property(@case => @case.ResolutionCode).HasMaxLength(new int?(32));
            Property(@case => @case.ResolutionDesc).HasMaxLength(new int?(64));
            Property(@case => @case.CasePicture).HasColumnType("CFG_Case_unknown2");
            Property(@case => @case.DLPicture).HasColumnType("CFG_Case_unknown3");
            Property(@case => @case.PassportPicture).HasColumnType("CFG_Case_unknown4");
        }
    }
}
