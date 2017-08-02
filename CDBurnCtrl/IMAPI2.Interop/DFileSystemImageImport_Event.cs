using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ComEventInterface(typeof(DFileSystemImageImportEvents), typeof(DFileSystemImageImport_EventProvider))]
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public interface DFileSystemImageImport_Event
	{
		event DFileSystemImageImport_EventHandler UpdateImport;
	}
}