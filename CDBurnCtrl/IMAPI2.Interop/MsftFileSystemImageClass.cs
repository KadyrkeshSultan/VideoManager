using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces("DFileSystemImageEvents\0DFileSystemImageImportEvents")]
	[Guid("2C941FC5-975B-59BE-A960-9A2A262853A5")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	public class MsftFileSystemImageClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern MsftFileSystemImageClass();
	}
}