using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces("DWriteEngine2Events")]
	[Guid("2735412C-7F64-5B0F-8F00-5D77AFBE261E")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	public class MsftWriteEngine2Class
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern MsftWriteEngine2Class();
	}
}