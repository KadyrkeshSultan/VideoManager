using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.Interop
{
	[Guid("25983550-9D65-49CE-B335-40630D901227")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface GInterface4
	{
		[DispId(259)]
		bool DisableGaplessAudio
		{
			get;
			set;
		}

		[DispId(265)]
		object[] ExpectedTableOfContents
		{
			get;
		}

		[DispId(262)]
		GInterface5 this[int trackIndex]
		{
			[DispId(262)]
			get;
		}

		[DispId(264)]
		int LastUsedUserSectorInImage
		{
			get;
		}

		[DispId(260)]
		string MediaCatalogNumber
		{
			get;
			set;
		}

		[DispId(263)]
		int NumberOfExistingTracks
		{
			get;
		}

		[DispId(256)]
		IMAPI_FORMAT2_RAW_CD_DATA_SECTOR_TYPE ResultingImageType
		{
			get;
			set;
		}

		[DispId(261)]
		int StartingTrackNumber
		{
			get;
			set;
		}

		[DispId(257)]
		int StartOfLeadout
		{
			get;
		}

		[DispId(258)]
		int StartOfLeadoutLimit
		{
			get;
			set;
		}

		[DispId(514)]
		void AddSpecialPregap([In] IStream data);

		[DispId(515)]
		void AddSubcodeRWGenerator([In] IStream subcode);

		[DispId(513)]
		int AddTrack(IMAPI_CD_SECTOR_TYPE dataType, [In] IStream data);

		[DispId(512)]
		IStream CreateResultImage();
	}
}