using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void DFileSystemImage_EventHandler([In] object sender, string currentFile, int copiedSectors, int totalSectors);
}