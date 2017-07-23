using System;

namespace VideoManager.Model
{
    class GlobalConfig
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
        public string DataType { get; set; }
        public bool IsEditable { get; set; }
    }
}
