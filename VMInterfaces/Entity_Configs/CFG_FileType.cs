using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_FileType : EntityTypeConfiguration<FileType>
    {
        public CFG_FileType()
        {
            HasKey(fileType => fileType.Id).Property((Expression<Func<FileType, Guid>>)(fileType => fileType.Id)).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(fileType => fileType.FileExt).IsRequired().HasMaxLength(new int?(8));
            Property(fileType => fileType.TypeDec).HasMaxLength(new int?(64));
            Property(fileType => fileType.Thumbnail).HasColumnType("image");
        }
    }
}
