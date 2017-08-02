using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[CoClass(typeof(MsftIsoImageManagerClass))]
	[Guid("6CA38BE5-FBBB-4800-95A1-A438865EB0D4")]
	public interface MsftIsoImageManager : IIsoImageManager
	{

	}
}