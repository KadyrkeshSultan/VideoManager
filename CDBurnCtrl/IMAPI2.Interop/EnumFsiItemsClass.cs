using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("2C941FC6-975B-59BE-A960-9A2A262853A5")]
	public class EnumFsiItemsClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern EnumFsiItemsClass();
	}
}