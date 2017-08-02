using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ComEventInterface(typeof(DDiscMaster2Events), typeof(DiscMaster2_EventProvider))]
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public interface DiscMaster2_Event
	{
		event DiscMaster2_NotifyDeviceAddedEventHandler NotifyDeviceAdded;

		event DiscMaster2_NotifyDeviceRemovedEventHandler NotifyDeviceRemoved;
	}
}