using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Department : EntityTypeConfiguration<Department>
    {
        public CFG_Department()
        {
            HasKey(department => department.Id).Property(department => department.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(department => department.Name).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("CFG_Department_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(department => department.Address1).HasMaxLength(new int?(128));
            Property(department => department.Address2).HasMaxLength(new int?(128));
            Property(department => department.City).HasMaxLength(new int?(64));
            Property(department => department.State).HasMaxLength(new int?(64));
            Property(department => department.PostalCode).HasMaxLength(new int?(32));
            Property(department => department.Phone1).HasMaxLength(new int?(16));
            Property(department => department.Phone2).HasMaxLength(new int?(16));
            Property(department => department.Logo).HasColumnType("CFG_Department_unknown2");
        }
    }
}
