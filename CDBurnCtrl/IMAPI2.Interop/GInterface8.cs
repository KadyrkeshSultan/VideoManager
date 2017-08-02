using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[CoClass(typeof(MsftDiscFormat2RawCDClass))]
	[Guid("27354155-8F64-5B0F-8F00-5D77AFBE261E")]
	public interface GInterface8 : DiscFormat2RawCD_Event, IDiscFormat2RawCD
	{

	}
}