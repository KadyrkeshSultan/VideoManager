using System;
using System.Runtime.CompilerServices;

namespace SetTreeCtrl
{
    public class NodeRecord
    {
        public int ImgIdx
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Guid RecIdx
        {
            get;
            set;
        }

        public NodeType RecType
        {
            get;
            set;
        }

        public NodeRecord()
        {
        }
    }
}