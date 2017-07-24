using System;

namespace VMModels.Model
{
    public class RightsProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Rights { get; set; }
    }
}
