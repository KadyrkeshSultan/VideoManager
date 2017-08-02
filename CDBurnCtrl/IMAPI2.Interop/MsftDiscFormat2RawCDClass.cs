using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces("DDiscFormat2RawCDEvents")]
	[Guid("27354128-7F64-5B0F-8F00-5D77AFBE261E")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	public class MsftDiscFormat2RawCDClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern MsftDiscFormat2RawCDClass();
	}
}