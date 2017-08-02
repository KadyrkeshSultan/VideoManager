using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[CoClass(typeof(MsftFileSystemImageClass))]
	[Guid("2C941FE1-975B-59BE-A960-9A2A262853A5")]
	public interface MsftFileSystemImage : DFileSystemImage_Event, IFileSystemImage
	{

	}
}