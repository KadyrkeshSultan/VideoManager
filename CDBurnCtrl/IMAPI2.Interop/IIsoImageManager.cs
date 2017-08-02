using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("6CA38BE5-FBBB-4800-95A1-A438865EB0D4")]
	[TypeLibType(TypeLibTypeFlags.FDispatchable)]
	public interface IIsoImageManager
	{
		[DispId(256)]
		string path
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
			get;
		}

		[DispId(257)]
		FsiStream Stream
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPath(string Val);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetStream([In] FsiStream Data);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Validate();
	}
}