using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Evidence : EntityTypeConfiguration<Evidence>
    {
        public CFG_Evidence()
        {
            HasKey(evidence => evidence.Id).Property(evidence => evidence.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(evidence => evidence.ItemType).HasMaxLength(new int?(64));
            Property(evidence => evidence.ItemDesc).HasMaxLength(new int?(128));
            Property(evidence => evidence.FileNumber).HasMaxLength(new int?(32));
            Property(evidence => evidence.Picture).HasColumnType("CFG_Evidence_unknown1");
        }
    }
}
