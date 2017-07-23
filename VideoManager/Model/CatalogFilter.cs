using System;
using VideoManager.Enums;

namespace VideoManager.Model
{
    class CatalogFilter
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Classification { get; set; }
        public bool IsSecurityFilter { get; set; }
        public SECURITY SecurityLevel { get; set; }
        public string FileType { get; set; }
        public int Rating { get; set; }
        public string WordPhrase { get; set; }
        public string RMS { get; set; }
        public string CAD { get; set; }
        public bool IsEvidence { get; set; }
    }
}
