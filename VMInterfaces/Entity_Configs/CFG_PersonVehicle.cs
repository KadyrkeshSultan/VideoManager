using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMModels.Model;

namespace VMInterfaces.Entity_Configs
{
    public class CFG_PersonVehicle : EntityTypeConfiguration<PersonVehicle>
    {
        public CFG_PersonVehicle()
        {
            HasKey(personVehicle => personVehicle.Id).Property(personVehicle => personVehicle.Id).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
            Property(personVehicle => personVehicle.Make).HasMaxLength(new int?(32));
            Property(personVehicle => personVehicle.Model).HasMaxLength(new int?(32));
            Property(personVehicle => personVehicle.Color).HasMaxLength(new int?(32));
            Property(personVehicle => personVehicle.VIN).HasMaxLength(new int?(64));
            Property(personVehicle => personVehicle.Plate).HasMaxLength(new int?(12));
            Property(personVehicle => personVehicle.StateProvince).HasMaxLength(new int?(64));
        }
    }
}
