namespace UploadCtrl
{
    public class CmdEventArgs
    {
        public readonly FileData Record;

        public CmdEventArgs(FileData data)
        {
            this.Record = data;
        }
    }
}
