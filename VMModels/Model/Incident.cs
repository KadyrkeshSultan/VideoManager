using System;

namespace VMModels
{
    class Incident
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string GPS { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Memo { get; set; }
        public int OrderId { get; set; }
    }
}
