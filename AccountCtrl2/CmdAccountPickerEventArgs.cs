using System;

namespace AccountCtrl2
{
    public class CmdAccountPickerEventArgs : EventArgs
    {
        public readonly NodeRecord nodeRec;

        public CmdAccountPickerEventArgs(NodeRecord rec)
        {
            nodeRec = rec;
        }
    }
}
