using System;

namespace VMModels.Model
{
    public class Substation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public Guid DomainCfgID { get; set; }
        public virtual Department Department { get; set; }
    }
}
