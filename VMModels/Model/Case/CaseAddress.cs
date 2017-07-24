using System;

namespace VMModels.Model
{
    public class CaseAddress
    {
        public Guid Id { get; set; }
        public bool IsPrimary { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
    }
}
