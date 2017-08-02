using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.Interop
{
	[Guid("2C941FD8-975B-59BE-A960-9A2A262853A5")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IFileSystemImageResult
	{
		[DispId(4)]
		int BlockSize
		{
			get;
		}

		[DispId(5)]
		string DiscId
		{
			get;
		}

		[DispId(1)]
		IStream ImageStream
		{
			get;
		}

		[DispId(2)]
		IProgressItems ProgressItems
		{
			get;
		}

		[DispId(3)]
		int TotalBlocks
		{
			get;
		}
	}
}