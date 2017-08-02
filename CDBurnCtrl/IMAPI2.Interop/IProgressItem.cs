using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("2C941FD5-975B-59BE-A960-9A2A262853A5")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IProgressItem
	{
		[DispId(4)]
		uint BlockCount
		{
			get;
		}

		[DispId(1)]
		string Description
		{
			get;
		}

		[DispId(2)]
		uint FirstBlock
		{
			get;
		}

		[DispId(3)]
		uint LastBlock
		{
			get;
		}
	}
}