using System;

namespace VideoManager.Model
{
    class License
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string LicData { get; set; }
    }
}
