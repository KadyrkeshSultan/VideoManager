using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("2C941FDA-975B-59BE-A960-9A2A262853A5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumFsiItems
	{
		void Clone(out IEnumFsiItems ppEnum);

		void Next(uint celt, out IFsiItem rgelt, out uint pceltFetched);

		void RemoteNext(uint celt, out IFsiItem rgelt, out uint pceltFetched);

		void Reset();

		void Skip(uint celt);
	}
}