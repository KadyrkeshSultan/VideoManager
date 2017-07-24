using System;
using System.Collections.Generic;

namespace VideoManager.Model
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public byte[] Logo { get; set; }
        public virtual List<Substation> Substations { get; set; }
    }
}
