using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("2C941FD6-975B-59BE-A960-9A2A262853A5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface GInterface2
	{
		void Clone(out GInterface2 ppEnum);

		void Next(uint celt, out IProgressItem rgelt, out uint pceltFetched);

		void RemoteNext(uint celt, out IProgressItem rgelt, out uint pceltFetched);

		void Reset();

		void Skip(uint celt);
	}
}