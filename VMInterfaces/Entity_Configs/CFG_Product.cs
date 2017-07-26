using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Product : EntityTypeConfiguration<Product>
    {
        public CFG_Product()
        {
            HasKey(product => product.Id).Property(product => product.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(product => product.Desc).HasMaxLength(new int?(64));
            Property(product => product.Name).IsRequired().HasMaxLength(new int?(32));
            Property(product => product.Model).IsRequired().HasMaxLength(new int?(32));
        }
    }
}
