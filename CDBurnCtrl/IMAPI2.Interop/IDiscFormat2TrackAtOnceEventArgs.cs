using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("27354140-7F64-5B0F-8F00-5D77AFBE261E")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IDiscFormat2TrackAtOnceEventArgs
	{
		[DispId(769)]
		IMAPI_FORMAT2_TAO_WRITE_ACTION CurrentAction
		{
			get;
		}

		[DispId(768)]
		int CurrentTrackNumber
		{
			get;
		}

		[DispId(770)]
		int ElapsedTime
		{
			get;
		}

		[DispId(264)]
		int FreeSystemBuffer
		{
			get;
		}

		[DispId(258)]
		int LastReadLba
		{
			get;
		}

		[DispId(259)]
		int LastWrittenLba
		{
			get;
		}

		[DispId(771)]
		int RemainingTime
		{
			get;
		}

		[DispId(257)]
		int SectorCount
		{
			get;
		}

		[DispId(256)]
		int StartLba
		{
			get;
		}

		[DispId(262)]
		int TotalSystemBuffer
		{
			get;
		}

		[DispId(263)]
		int UsedSystemBuffer
		{
			get;
		}
	}
}