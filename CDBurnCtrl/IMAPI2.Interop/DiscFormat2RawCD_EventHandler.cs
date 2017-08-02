using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void DiscFormat2RawCD_EventHandler([In] object sender, [In] object progress);
}