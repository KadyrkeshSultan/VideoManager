using System;

namespace IMAPI2.Interop
{
	[Flags]
	public enum FsiFileSystems
	{
		FsiFileSystemNone = 0,
		FsiFileSystemISO9660 = 1,
		FsiFileSystemJoliet = 2,
		FsiFileSystemUDF = 4,
		FsiFileSystemUnknown = 1073741824
	}
}