using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ComEventInterface(typeof(DWriteEngine2Events), typeof(DWriteEngine2_EventProvider))]
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public interface DWriteEngine2_Event
	{
		event DWriteEngine2_EventHandler Update;
	}
}