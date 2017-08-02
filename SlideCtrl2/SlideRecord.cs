using VMModels.Model;

namespace SlideCtrl2
{
    public class SlideRecord
    {
        public ACTION Action
        {
            get;
            set;
        }

        public int ClassDays
        {
            get;
            set;
        }

        public DataFile dRecord
        {
            get;
            set;
        }

        public bool IsDelete
        {
            get;
            set;
        }

        public bool IsDescUpdate
        {
            get;
            set;
        }

        public bool IsMemo
        {
            get;
            set;
        }

        public bool IsRet
        {
            get;
            set;
        }

        public bool IsSecure
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public bool IsSet
        {
            get;
            set;
        }

        public bool IsShare
        {
            get;
            set;
        }

        public int SlideNumber
        {
            get;
            set;
        }

        public SlideRecord()
        {
        }
    }
}