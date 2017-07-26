using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_UserRank : EntityTypeConfiguration<UserRank>
    {
        public CFG_UserRank()
        {
            HasKey(userRank => userRank.Id).Property(userRank => userRank.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(userRank => userRank.Rank).IsRequired().HasMaxLength(new int?(32)).HasColumnAnnotation("CFG_UserRank_unknown1", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
        }
    }
}
