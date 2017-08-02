using System;

namespace SlideCtrl2
{
    public class CmdSlideEventArgs : EventArgs
    {
        public readonly SlideRecord SlideRecord;

        public CmdSlideEventArgs(SlideRecord rec)
        {
            this.SlideRecord = rec;
        }
    }
}