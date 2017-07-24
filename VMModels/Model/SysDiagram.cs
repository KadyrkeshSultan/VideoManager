using System;

namespace VMModels.Model
{
    public class SysDiagram
    {
        public string Id { get; set; }
        public DateTime LogTimestamp { get; set; }
        public string DomainName { get; set; }
        public string MachineName { get; set; }
        public string MachineAccount { get; set; }
        public string IPAddress { get; set; }
        public string MachineID { get; set; }
        public string Memo { get; set; }
        public string Action { get; set; }
    }
}
