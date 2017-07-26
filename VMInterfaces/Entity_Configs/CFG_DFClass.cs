using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_DFClass : EntityTypeConfiguration<DFClass>
    {
        public CFG_DFClass()
        {
            HasKey(dfClass => dfClass.Id).Property(dfClass => dfClass.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(dfClass => dfClass.Name).HasMaxLength(new int?(64));
        }
    }
}
