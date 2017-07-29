using System;

namespace AccountCtrl2
{
    public class NodeRecord
    {
        public NodeType RecType {  get;  set; }

        public string Name {  get;  set; }

        public string BadgeNumber {  get;  set; }

        public Guid RecIdx {  get;  set; }

        public Guid SubIdx {  get;  set; }

        public int ImgIdx {  get;  set; }
        
    }
}
