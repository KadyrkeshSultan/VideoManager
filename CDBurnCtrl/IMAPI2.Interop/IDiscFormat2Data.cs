using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.Interop
{
	[Guid("27354153-9F64-5B0F-8F00-5D77AFBE261E")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IDiscFormat2Data
	{
		[DispId(257)]
		bool BufferUnderrunFreeDisabled
		{
			get;
			set;
		}

		[DispId(272)]
		string ClientName
		{
			get;
			set;
		}

		[DispId(262)]
		IMAPI_FORMAT2_DATA_MEDIA_STATE CurrentMediaStatus
		{
			get;
		}

		[DispId(271)]
		IMAPI_MEDIA_PHYSICAL_TYPE CurrentPhysicalMediaType
		{
			get;
		}

		[DispId(276)]
		bool CurrentRotationTypeIsPureCAV
		{
			get;
		}

		[DispId(275)]
		int CurrentWriteSpeed
		{
			get;
		}

		[DispId(270)]
		bool DisableConsumerDvdCompatibilityMode
		{
			get;
			set;
		}

		[DispId(269)]
		bool ForceMediaToBeClosed
		{
			get;
			set;
		}

		[DispId(279)]
		bool ForceOverwrite
		{
			get;
			set;
		}

		[DispId(265)]
		int FreeSectorsOnMedia
		{
			get;
		}

		[DispId(268)]
		int LastWrittenAddressOfPreviousSession
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

		[DispId(280)]
		object[] MultisessionInterfaces
		{
			get;
		}

		[DispId(266)]
		int NextWritableAddress
		{
			get;
		}

		[DispId(260)]
		bool PostgapAlreadyInImage
		{
			get;
			set;
		}

		[DispId(256)]
		IDiscRecorder2 Recorder
		{
			get;
			set;
		}

		[DispId(274)]
		bool RequestedRotationTypeIsPureCAV
		{
			get;
		}

		[DispId(273)]
		int RequestedWriteSpeed
		{
			get;
		}

		[DispId(267)]
		int StartAddressOfPreviousSession
		{
			get;
		}

		[DispId(1794)]
		object[] SupportedMediaTypes
		{
			get;
		}

		[DispId(278)]
		object[] SupportedWriteSpeedDescriptors
		{
			get;
		}

		[DispId(277)]
		object[] SupportedWriteSpeeds
		{
			get;
		}

		[DispId(264)]
		int TotalSectorsOnMedia
		{
			get;
		}

		[DispId(263)]
		IMAPI_MEDIA_WRITE_PROTECT_STATE WriteProtectStatus
		{
			get;
		}

		[DispId(513)]
		void CancelWrite();

		[DispId(2049)]
		bool IsCurrentMediaSupported(IDiscRecorder2 Recorder);

		[DispId(2048)]
		bool IsRecorderSupported(IDiscRecorder2 Recorder);

		[DispId(514)]
		void SetWriteSpeed(int RequestedSectorsPerSecond, bool RotationTypeIsPureCAV);

		[DispId(512)]
		void Write(IStream data);
	}
}