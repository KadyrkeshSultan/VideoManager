using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Person : EntityTypeConfiguration<Person>
    {
        public CFG_Person()
        {
            HasKey(person => person.Id).Property(person => person.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(person => person.FirstName).IsRequired().HasMaxLength(new int?(32));
            Property(person => person.LastName).IsRequired().HasMaxLength(new int?(32));
            Property(person => person.MiddleName).HasMaxLength(new int?(32));
            Property(person => person.Gender).IsRequired().HasMaxLength(new int?(1));
            Property(person => person.SSN).HasMaxLength(new int?(16));
            Property(person => person.HairColor).HasMaxLength(new int?(16));
            Property(person => person.EyeColor).HasMaxLength(new int?(16));
            Property(person => person.PassportID).HasMaxLength(new int?(64));
            Property(person => person.Country).HasMaxLength(new int?(64));
            Property(person => person.LicenseID).HasMaxLength(new int?(64));
            Property(person => person.LicenseState).HasMaxLength(new int?(64));
            Property(person => person.Email).HasMaxLength(new int?(128));
            Property(person => person.PassportPic).HasColumnType("CFG_Person_unknown1");
        }
    }
}
