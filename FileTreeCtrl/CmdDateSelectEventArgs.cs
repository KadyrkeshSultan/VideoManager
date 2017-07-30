using System;

namespace FileTreeCtrl
{
    public class CmdDateSelectEventArgs : EventArgs
    {
        public readonly string date;

        public CmdDateSelectEventArgs(string dt)
        {
            date = dt;
        }
    }
}
