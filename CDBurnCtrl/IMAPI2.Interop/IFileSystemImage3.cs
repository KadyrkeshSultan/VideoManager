using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("7CFF842C-7E97-4807-8304-910DD8F7C051")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
	public interface IFileSystemImage3
	{
		[DispId(6)]
		IBootOptions BootImageOptions
		{
			get;
			set;
		}

		[DispId(60)]
		object[] BootImageOptionsArray
		{
			get;
			set;
		}

		[DispId(10)]
		int ChangePoint
		{
			get;
		}

		[DispId(61)]
		bool CreateRedundantUdfMetadataFiles
		{
			get;
			set;
		}

		[DispId(8)]
		int DirectoryCount
		{
			get;
		}

		[DispId(7)]
		int FileCount
		{
			get;
		}

		[DispId(14)]
		FsiFileSystems FileSystemsSupported
		{
			get;
		}

		[DispId(13)]
		FsiFileSystems FileSystemsToCreate
		{
			get;
			set;
		}

		[DispId(2)]
		int FreeMediaBlocks
		{
			get;
			set;
		}

		[DispId(5)]
		string ImportedVolumeName
		{
			get;
		}

		[DispId(37)]
		int Int32_0
		{
			get;
			set;
		}

		[DispId(34)]
		int ISO9660InterchangeLevel
		{
			get;
			set;
		}

		[DispId(38)]
		object[] ISO9660InterchangeLevelsSupported
		{
			get;
		}

		[DispId(40)]
		object[] MultisessionInterfaces
		{
			get;
			set;
		}

		[DispId(0)]
		IFsiDirectoryItem Root
		{
			get;
		}

		[DispId(1)]
		int SessionStartBlock
		{
			get;
			set;
		}

		[DispId(30)]
		bool StageFiles
		{
			get;
			set;
		}

		[DispId(11)]
		bool StrictFileSystemCompliance
		{
			get;
			set;
		}

		[DispId(31)]
		object[] UDFRevisionsSupported
		{
			get;
		}

		[DispId(3)]
		int UsedBlocks
		{
			get;
		}

		[DispId(12)]
		bool UseRestrictedCharacterSet
		{
			get;
			set;
		}

		[DispId(4)]
		string VolumeName
		{
			get;
			set;
		}

		[DispId(29)]
		string VolumeNameISO9660
		{
			get;
		}

		[DispId(28)]
		string VolumeNameJoliet
		{
			get;
		}

		[DispId(27)]
		string VolumeNameUDF
		{
			get;
		}

		[DispId(9)]
		string WorkingDirectory
		{
			get;
			set;
		}

		[DispId(18)]
		string CalculateDiscIdentifier();

		[DispId(32)]
		void ChooseImageDefaults(IDiscRecorder2 discRecorder);

		[DispId(33)]
		void ChooseImageDefaultsForMediaType(IMAPI_MEDIA_PHYSICAL_TYPE value);

		[DispId(25)]
		IFsiDirectoryItem CreateDirectoryItem(string Name);

		[DispId(26)]
		IFsiFileItem CreateFileItem(string Name);

		[DispId(15)]
		IFileSystemImageResult CreateResultImage();

		[DispId(16)]
		FsiItemType Exists(string FullPath);

		[DispId(20)]
		FsiFileSystems GetDefaultFileSystemForImport(FsiFileSystems fileSystems);

		[DispId(19)]
		FsiFileSystems IdentifyFileSystemsOnDisc(IDiscRecorder2 discRecorder);

		[DispId(21)]
		FsiFileSystems ImportFileSystem();

		[DispId(22)]
		void ImportSpecificFileSystem(FsiFileSystems fileSystemToUse);

		[DispId(24)]
		void LockInChangePoint();

		[DispId(70)]
		bool ProbeSpecificFileSystem(FsiFileSystems fileSystemToProbe);

		[DispId(23)]
		void RollbackToChangePoint(int ChangePoint);

		[DispId(36)]
		void SetMaxMediaBlocksFromDevice(IDiscRecorder2 discRecorder);
	}
}