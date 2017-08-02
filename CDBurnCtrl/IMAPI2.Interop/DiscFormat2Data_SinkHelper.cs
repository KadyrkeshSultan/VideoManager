using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public sealed class DiscFormat2Data_SinkHelper : DDiscFormat2DataEvents
	{
		private int m_dwCookie;

		private DiscFormat2Data_EventHandler m_UpdateDelegate;

		public int Cookie
		{
			get
			{
				return this.m_dwCookie;
			}
			set
			{
				this.m_dwCookie = value;
			}
		}

		public DiscFormat2Data_EventHandler UpdateDelegate
		{
			get
			{
				return this.m_UpdateDelegate;
			}
			set
			{
				this.m_UpdateDelegate = value;
			}
		}

		internal DiscFormat2Data_SinkHelper(DiscFormat2Data_EventHandler eventHandler)
		{
			if (eventHandler == null)
			{
				throw new ArgumentNullException("Delegate (callback function) cannot be null");
			}
			this.m_dwCookie = 0;
			this.m_UpdateDelegate = eventHandler;
		}

		public void Update(object sender, object args)
		{
			this.m_UpdateDelegate(sender, args);
		}
	}
}