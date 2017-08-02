using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces("DDiscFormat2TrackAtOnceEvents")]
	[Guid("27354129-7F64-5B0F-8F00-5D77AFBE261E")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	public class MsftDiscFormat2TrackAtOnceClass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		public extern MsftDiscFormat2TrackAtOnceClass();
	}
}