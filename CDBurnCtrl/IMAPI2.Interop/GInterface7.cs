using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[CoClass(typeof(MsftDiscFormat2EraseClass))]
	[Guid("27354156-8F64-5B0F-8F00-5D77AFBE261E")]
	public interface GInterface7 : DiscFormat2Erase_Event, IDiscFormat2Erase
	{

	}
}