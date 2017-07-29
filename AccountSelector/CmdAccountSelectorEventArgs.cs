using System;

namespace AccountSelector
{
    public class CmdAccountSelectorEventArgs : EventArgs
    {
        public readonly Guid value;

        public CmdAccountSelectorEventArgs(Guid data)
        {
            value = data;
        }
    }
}
