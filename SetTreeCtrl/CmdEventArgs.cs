using System;

namespace SetTreeCtrl
{
    public class CmdEventArgs : EventArgs
    {
        public readonly NodeRecord NodeRec;

        public CmdEventArgs(NodeRecord nRec)
        {
            this.NodeRec = nRec;
        }
    }
}