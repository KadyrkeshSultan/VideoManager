using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("2C941FD7-975B-59BE-A960-9A2A262853A5")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IProgressItems
	{
		[DispId(1)]
		int Count
		{
			get;
		}

		[DispId(4)]
		GInterface2 EnumProgressItems
		{
			get;
		}

		[DispId(0)]
		IProgressItem this[int Index]
		{
			get;
		}

		[DispId(-4)]
		[TypeLibFunc(65)]
		IEnumerator GetEnumerator();

		[DispId(2)]
		IProgressItem ProgressItemFromBlock(uint block);

		[DispId(3)]
		IProgressItem ProgressItemFromDescription(string Description);
	}
}