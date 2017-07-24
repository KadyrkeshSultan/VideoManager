using System;

namespace VMModels
{
    class FileType
    {
        public Guid Id { get; set; }
        public string FileExt { get; set; }
        public string TypeDec { get; set; }
        public bool IsInternal { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
