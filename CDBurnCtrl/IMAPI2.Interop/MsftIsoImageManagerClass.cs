using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("CEEE3B62-8F56-4056-869B-EF16917E3EFC")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	public class MsftIsoImageManagerClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern MsftIsoImageManagerClass();
	}
}