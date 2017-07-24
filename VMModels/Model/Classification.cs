using System;

namespace VMModels
{
    class Classification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsRetention { get; set; }
        public int Days { get; set; }
    }
}
