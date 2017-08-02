using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.Interop
{
	[Guid("2C941FDC-975B-59BE-A960-9A2A262853A5")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IFsiDirectoryItem
	{
		[DispId(1)]
		int Count
		{
			get;
		}

		[DispId(13)]
		DateTime CreationTime
		{
			get;
			set;
		}

		[DispId(2)]
		IEnumFsiItems EnumFsiItems
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

		[DispId(0)]
		IFsiItem this[string path]
		{
			get;
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

		[DispId(33)]
		void Add(IFsiItem Item);

		[DispId(30)]
		void AddDirectory(string path);

		[DispId(31)]
		void AddFile(string path, IStream fileData);

		[DispId(32)]
		void AddTree(string sourceDirectory, bool includeBaseDirectory);

		[DispId(17)]
		string FileSystemName(FsiFileSystems fileSystem);

		[DispId(18)]
		string FileSystemPath(FsiFileSystems fileSystem);

		[DispId(-4)]
		[TypeLibFunc(65)]
		System.Collections.IEnumerator GetEnumerator();

		[DispId(34)]
		void Remove(string path);

		[DispId(35)]
		void RemoveTree(string path);
	}
}