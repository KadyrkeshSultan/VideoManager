using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("2C941FCC-975B-59BE-A960-9A2A262853A5")]
	public class FileSystemImageResultClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern FileSystemImageResultClass();
	}
}