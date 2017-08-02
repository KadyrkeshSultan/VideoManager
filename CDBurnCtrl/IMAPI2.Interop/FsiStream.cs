using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.Interop
{
	[CoClass(typeof(FsiStreamClass))]
	[Guid("0000000C-0000-0000-C000-000000000046")]
	public interface FsiStream : IStream
	{

	}
}