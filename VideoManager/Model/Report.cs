using System;

namespace VideoManager.Model
{
    public class Report
    {
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public string ReportName { get; set; }
        public string ReportDesc { get; set; }
        public string ReportURL { get; set; }
        public int SecurityLevel { get; set; }
    }
}
