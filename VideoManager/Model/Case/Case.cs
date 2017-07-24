using System;

namespace VideoManager.Model
{
    class Case
    {
        public Guid Id { get; set; }
        public Guid Classification { get; set; }
        public string CaseNumber { get; set; }
        public int CaseStatus { get; set; }
        public int CaseRating { get; set; }
        public DateTime? CaseCreated { get; set; }
        public DateTime? CaseDate { get; set; }
        public DateTime? CaseCloseDate { get; set; }
        public bool IsPurged { get; set; }
        public DateTime? PurgeTimestamp { get; set; }
        public Guid ClosedByAccount { get; set; }
        public string PurgeFileName { get; set; }
        public bool? IsRetainIndefinite { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool? IsTrangender { get; set; }
        public string SSN { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOD { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public byte[] CasePicture { get; set; }
        public byte[] DLPicture { get; set; }
        public byte[] PassportPicture { get; set; }
        public string PassportID { get; set; }
        public string Country { get; set; }
        public string LicenseID { get; set; }
        public string LicenseState { get; set; }
        public string Email { get; set; }
        public string ResolutionCode { get; set; }
        public string ResolutionDesc { get; set; }
    }
}
