using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_CaseMetaData : EntityTypeConfiguration<CaseMetaData>
    {
        public CFG_CaseMetaData()
        {
            HasKey(caseMetaData => caseMetaData.Id).Property(caseMetaData => caseMetaData.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(caseMetaData => caseMetaData.DataType).IsRequired().HasMaxLength(new int?(16));
            Property(caseMetaData => caseMetaData.DataPrompt).IsRequired().HasMaxLength(new int?(64));
            Property(caseMetaData => caseMetaData.DataValue).HasMaxLength(new int?(256));
        }
    }
}
