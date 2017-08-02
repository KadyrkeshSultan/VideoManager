using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void DiscMaster2_NotifyDeviceAddedEventHandler([In] object sender, string uniqueId);
}