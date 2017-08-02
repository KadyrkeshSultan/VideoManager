using System;

namespace SlideCtrl2
{
    public class CmdPackageEventArgs : EventArgs
    {
        public readonly string value;

        public CmdPackageEventArgs(string data)
        {
            this.value = data;
        }
    }
}