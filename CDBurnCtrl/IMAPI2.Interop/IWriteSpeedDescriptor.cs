using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("27354144-7F64-5B0F-8F00-5D77AFBE261E")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	public interface IWriteSpeedDescriptor
	{
		[DispId(257)]
		IMAPI_MEDIA_PHYSICAL_TYPE MediaType
		{
			get;
		}

		[DispId(258)]
		bool RotationTypeIsPureCAV
		{
			get;
		}

		[DispId(259)]
		int WriteSpeed
		{
			get;
		}
	}
}