using System;

namespace IMAPI2.Interop
{
	public enum IMAPI_PROFILE_TYPE
	{
		IMAPI_PROFILE_TYPE_INVALID = 0,
		IMAPI_PROFILE_TYPE_NON_REMOVABLE_DISK = 1,
		IMAPI_PROFILE_TYPE_REMOVABLE_DISK = 2,
		IMAPI_PROFILE_TYPE_MO_ERASABLE = 3,
		IMAPI_PROFILE_TYPE_MO_WRITE_ONCE = 4,
		IMAPI_PROFILE_TYPE_AS_MO = 5,
		IMAPI_PROFILE_TYPE_CDROM = 8,
		IMAPI_PROFILE_TYPE_CD_RECORDABLE = 9,
		IMAPI_PROFILE_TYPE_CD_REWRITABLE = 10,
		IMAPI_PROFILE_TYPE_DVDROM = 16,
		IMAPI_PROFILE_TYPE_DVD_DASH_RECORDABLE = 17,
		IMAPI_PROFILE_TYPE_DVD_RAM = 18,
		IMAPI_PROFILE_TYPE_DVD_DASH_REWRITABLE = 19,
		IMAPI_PROFILE_TYPE_DVD_DASH_RW_SEQUENTIAL = 20,
		IMAPI_PROFILE_TYPE_DVD_DASH_R_DUAL_SEQUENTIAL = 21,
		IMAPI_PROFILE_TYPE_DVD_DASH_R_DUAL_LAYER_JUMP = 22,
		IMAPI_PROFILE_TYPE_DVD_PLUS_RW = 26,
		IMAPI_PROFILE_TYPE_DVD_PLUS_R = 27,
		IMAPI_PROFILE_TYPE_DDCDROM = 32,
		IMAPI_PROFILE_TYPE_DDCD_RECORDABLE = 33,
		IMAPI_PROFILE_TYPE_DDCD_REWRITABLE = 34,
		IMAPI_PROFILE_TYPE_DVD_PLUS_RW_DUAL = 42,
		IMAPI_PROFILE_TYPE_DVD_PLUS_R_DUAL = 43,
		IMAPI_PROFILE_TYPE_BD_ROM = 64,
		IMAPI_PROFILE_TYPE_BD_R_SEQUENTIAL = 65,
		IMAPI_PROFILE_TYPE_BD_R_RANDOM_RECORDING = 66,
		IMAPI_PROFILE_TYPE_BD_REWRITABLE = 67,
		IMAPI_PROFILE_TYPE_HD_DVD_ROM = 80,
		IMAPI_PROFILE_TYPE_HD_DVD_RECORDABLE = 81,
		IMAPI_PROFILE_TYPE_HD_DVD_RAM = 82,
		IMAPI_PROFILE_TYPE_NON_STANDARD = 65535
	}
}