using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_CaseVehicle: EntityTypeConfiguration<CaseVehicle>
    {
        public CFG_CaseVehicle()
        {
            HasKey(caseVehicle => caseVehicle.Id).Property(caseVehicle => caseVehicle.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(caseVehicle => caseVehicle.Make).HasMaxLength(new int?(32));
            Property(caseVehicle => caseVehicle.Model).HasMaxLength(new int?(32));
            Property(caseVehicle => caseVehicle.Color).HasMaxLength(new int?(32));
            Property(caseVehicle => caseVehicle.VIN).HasMaxLength(new int?(64));
            Property(caseVehicle => caseVehicle.Plate).HasMaxLength(new int?(12));
            Property(caseVehicle => caseVehicle.StateProvince).HasMaxLength(new int?(64));
        }
    }
}
