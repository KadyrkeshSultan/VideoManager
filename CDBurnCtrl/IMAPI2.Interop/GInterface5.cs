using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("25983551-9D65-49CE-B335-40630D901227")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface GInterface5
	{
		[DispId(262)]
		bool AudioHasPreemphasis
		{
			get;
			set;
		}

		[DispId(261)]
		IMAPI_CD_TRACK_DIGITAL_COPY_SETTING DigitalAudioCopySetting
		{
			get;
			set;
		}

		[DispId(260)]
		string ISRC
		{
			get;
			set;
		}

		[DispId(257)]
		int SectorCount
		{
			get;
		}

		[DispId(259)]
		IMAPI_CD_SECTOR_TYPE SectorType
		{
			get;
		}

		[DispId(256)]
		int StartingLba
		{
			get;
		}

		[DispId(263)]
		object[] TrackIndexes
		{
			get;
		}

		[DispId(258)]
		int TrackNumber
		{
			get;
		}

		[DispId(512)]
		void AddTrackIndex(int lbaOffset);

		[DispId(513)]
		void ClearTrackIndex(int lbaOffset);
	}
}