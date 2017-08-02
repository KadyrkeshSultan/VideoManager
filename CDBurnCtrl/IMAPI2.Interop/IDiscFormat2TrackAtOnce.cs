using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.Interop
{
	[Guid("27354154-8F64-5B0F-8F00-5D77AFBE261E")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IDiscFormat2TrackAtOnce
	{
		[DispId(258)]
		bool BufferUnderrunFreeDisabled
		{
			get;
			set;
		}

		[DispId(270)]
		string ClientName
		{
			get;
			set;
		}

		[DispId(267)]
		IMAPI_MEDIA_PHYSICAL_TYPE CurrentPhysicalMediaType
		{
			get;
		}

		[DispId(274)]
		bool CurrentRotationTypeIsPureCAV
		{
			get;
		}

		[DispId(273)]
		int CurrentWriteSpeed
		{
			get;
		}

		[DispId(263)]
		bool DoNotFinalizeMedia
		{
			get;
			set;
		}

		[DispId(266)]
		object[] ExpectedTableOfContents
		{
			get;
		}

		[DispId(261)]
		int FreeSectorsOnMedia
		{
			get;
		}

		[DispId(1793)]
		bool MediaHeuristicallyBlank
		{
			get;
		}

		[DispId(1792)]
		bool MediaPhysicallyBlank
		{
			get;
		}

		[DispId(259)]
		int NumberOfExistingTracks
		{
			get;
		}

		[DispId(256)]
		IDiscRecorder2 Recorder
		{
			get;
			set;
		}

		[DispId(272)]
		bool RequestedRotationTypeIsPureCAV
		{
			get;
		}

		[DispId(271)]
		int RequestedWriteSpeed
		{
			get;
		}

		[DispId(1794)]
		object[] SupportedMediaTypes
		{
			get;
		}

		[DispId(276)]
		object[] SupportedWriteSpeedDescriptors
		{
			get;
		}

		[DispId(275)]
		object[] SupportedWriteSpeeds
		{
			get;
		}

		[DispId(260)]
		int TotalSectorsOnMedia
		{
			get;
		}

		[DispId(262)]
		int UsedSectorsOnMedia
		{
			get;
		}

		[DispId(513)]
		void AddAudioTrack(IStream data);

		[DispId(514)]
		void CancelAddTrack();

		[DispId(2049)]
		bool IsCurrentMediaSupported(IDiscRecorder2 Recorder);

		[DispId(2048)]
		bool IsRecorderSupported(IDiscRecorder2 Recorder);

		[DispId(512)]
		void PrepareMedia();

		[DispId(515)]
		void ReleaseMedia();

		[DispId(516)]
		void SetWriteSpeed(int RequestedSectorsPerSecond, bool RotationTypeIsPureCAV);
	}
}