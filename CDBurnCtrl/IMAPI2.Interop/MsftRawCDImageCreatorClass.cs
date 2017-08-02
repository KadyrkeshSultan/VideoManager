using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("25983561-9D65-49CE-B335-40630D901227")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	public class MsftRawCDImageCreatorClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern MsftRawCDImageCreatorClass();
	}
}