using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[CoClass(typeof(FsiDirectoryItemClass))]
	[Guid("F7FB4B9B-6D96-4D7B-9115-201B144811EF")]
	public interface FsiDirectoryItem : IFsiDirectoryItem, GInterface3
	{

	}
}