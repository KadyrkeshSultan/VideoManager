using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_Report : EntityTypeConfiguration<Report>
    {
        public CFG_Report()
        {
            HasKey(report => report.Id).Property(report => report.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(report => report.ReportName).IsRequired().HasMaxLength(new int?(64)).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(report => report.ReportDesc).HasMaxLength(new int?(128));
            Property(report => report.ReportURL).HasMaxLength(new int?(256));
        }
    }
}
