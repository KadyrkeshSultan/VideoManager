using System;

namespace IMAPI2.Interop
{
	public struct tagSTATSTG
	{
		public string pwcsName;

		public uint type;

		public _ULARGE_INTEGER cbSize;

		public _FILETIME mtime;

		public _FILETIME ctime;

		public _FILETIME atime;

		public uint grfMode;

		public uint grfLocksSupported;

		public Guid clsid;

		public uint grfStateBits;

		public uint reserved;
	}
}