using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_DataFile : EntityTypeConfiguration<DataFile>
    {
        public CFG_DataFile()
        {
            HasKey(dataFile => dataFile.Id).Property(dataFile => dataFile.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(dataFile => dataFile.UNCName).IsRequired().HasMaxLength(new int?(256));
            Property(dataFile => dataFile.UNCPath).IsRequired().HasMaxLength(new int?(256));
            Property(dataFile => dataFile.OriginalFileName).IsRequired().HasMaxLength(new int?(128));
            Property(dataFile => dataFile.FileExtension1).IsRequired().HasMaxLength(new int?(8));
            Property(dataFile => dataFile.FileExtension2).HasMaxLength(new int?(8));
            Property(dataFile => dataFile.Thumbnail).HasColumnType("CFG_DataFile_unknown1");
            Property(dataFile => dataFile.GPS).HasMaxLength(new int?(64));
            Property(dataFile => dataFile.ShortDesc).HasMaxLength(new int?(64));
            Property(dataFile => dataFile.PurgeFileName).HasMaxLength(new int?(256));
            Property(dataFile => dataFile.StoredFileName).IsRequired().HasMaxLength(new int?(256));
            Property(dataFile => dataFile.SetName).HasMaxLength(new int?(32));
            Property(dataFile => dataFile.MachineName).HasMaxLength(new int?(32));
            Property(dataFile => dataFile.MachineAccount).HasMaxLength(new int?(32));
            Property(dataFile => dataFile.LoginID).HasMaxLength(new int?(32));
            Property(dataFile => dataFile.SourcePath).HasMaxLength(new int?(256));
            Property(dataFile => dataFile.UserDomain).HasMaxLength(new int?(32));
            Property(dataFile => dataFile.RMSNumber).HasMaxLength(new int?(32));
            Property(dataFile => dataFile.CADNumber).HasMaxLength(new int?(32));
            Property(dataFile => dataFile.CloudID).HasMaxLength(new int?(64));
            Property(dataFile => dataFile.FileHashCode).HasMaxLength(new int?(64));
        }
    }
}
