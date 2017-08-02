using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ComEventInterface(typeof(DDiscFormat2RawCDEvents), typeof(DiscFormat2RawCD_EventProvider))]
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public interface DiscFormat2RawCD_Event
	{
		event DiscFormat2RawCD_EventHandler Update;
	}
}