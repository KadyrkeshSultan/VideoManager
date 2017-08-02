using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("F7FB4B9B-6D96-4D7B-9115-201B144811EF")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface GInterface3 : IFsiDirectoryItem
	{
		[DispId(36)]
		void AddTreeWithNamedStreams(string sourceDirectory, bool includeBaseDirectory);
	}
}