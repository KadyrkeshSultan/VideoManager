using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("27354123-7F64-5B0F-8F00-5D77AFBE261E")]
	public class MsftWriteSpeedDescriptorClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern MsftWriteSpeedDescriptorClass();
	}
}