using System;

namespace VideoManager.Model
{
    class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public byte[] Picture { get; set; }
        public string Gender { get; set; }
        public bool IsTransgender { get; set; }
        public string Race { get; set; }
        public DateTime? DOB { get; set; }
        public string SSN { get; set; }
        public string LicenseID { get; set; }
        public string LicenseState { get; set; }
        public string PassportID { get; set; }
        public byte[] PassportPic { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
    }
}
