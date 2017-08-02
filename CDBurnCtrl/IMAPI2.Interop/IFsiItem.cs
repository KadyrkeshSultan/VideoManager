using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("2C941FD9-975B-59BE-A960-9A2A262853A5")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IFsiItem
	{
		[DispId(13)]
		DateTime CreationTime
		{
			get;
			set;
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