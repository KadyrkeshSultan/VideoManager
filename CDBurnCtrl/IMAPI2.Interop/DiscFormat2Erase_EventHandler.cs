using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void DiscFormat2Erase_EventHandler([In] object sender, [In] int elapsedSeconds, [In] int estimatedTotalSeconds);
}