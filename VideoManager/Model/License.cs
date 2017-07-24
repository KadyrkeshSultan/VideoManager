using System;

namespace VideoManager.Model
{
    public class License
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string LicData { get; set; }
    }
}
