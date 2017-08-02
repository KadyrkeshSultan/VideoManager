using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ComEventInterface(typeof(DFileSystemImageEvents), typeof(DFileSystemImage_EventProvider))]
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public interface DFileSystemImage_Event
	{
		event DFileSystemImage_EventHandler Update;
	}
}