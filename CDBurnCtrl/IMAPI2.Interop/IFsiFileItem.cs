using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.Interop
{
	[Guid("2C941FDB-975B-59BE-A960-9A2A262853A5")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IFsiFileItem
	{
		[DispId(13)]
		DateTime CreationTime
		{
			get;
			set;
		}

		[DispId(44)]
		IStream Data
		{
			get;
			set;
		}

		[DispId(41)]
		long DataSize
		{
			get;
		}

		[DispId(43)]
		int DataSize32BitHigh
		{
			get;
		}

		[DispId(42)]
		int DataSize32BitLow
		{
			get;
		}

		[DispId(12)]
		string FullPath
		{
			get;
		}

		[DispId(16)]
		bool IsHidden
		{
			get;
			set;
		}

		[DispId(14)]
		DateTime LastAccessedTime
		{
			get;
			set;
		}

		[DispId(15)]
		DateTime LastModifiedTime
		{
			get;
			set;
		}

		[DispId(11)]
		string Name
		{
			get;
		}

		[DispId(17)]
		string FileSystemName(FsiFileSystems fileSystem);

		[DispId(18)]
		string FileSystemPath(FsiFileSystems fileSystem);
	}
}