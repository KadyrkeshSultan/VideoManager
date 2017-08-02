using System.Collections;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[CoClass(typeof(FsiNamedStreamsClass))]
	[Guid("ED79BA56-5294-4250-8D46-F9AECEE23459")]
	public interface FsiNamedStreams : IEnumerable, IFsiNamedStreams
	{

	}
}