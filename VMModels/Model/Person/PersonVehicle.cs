using System;

namespace VMModels.Model
{
    public class PersonVehicle
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string Plate { get; set; }
        public string StateProvince { get; set; }
        public string Memo { get; set; }
    }
}
