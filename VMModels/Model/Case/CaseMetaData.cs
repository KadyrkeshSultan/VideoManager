using System;

namespace VMModels.Model
{
    public class CaseMetaData
    {
        public Guid Id { get; set; }
        public string DataType { get; set; }
        public string DataPrompt { get; set; }
        public string DataValue { get; set; }
    }
}
