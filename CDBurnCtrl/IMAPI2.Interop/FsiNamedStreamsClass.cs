using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("C6B6F8ED-6D19-44B4-B539-B159B793A32D")]
	public class FsiNamedStreamsClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern FsiNamedStreamsClass();
	}
}