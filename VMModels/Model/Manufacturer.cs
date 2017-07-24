using System;
using System.Collections.Generic;

namespace VMModels.Model
{
    public class Manufacturer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CSZ { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }
        public string Web { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
