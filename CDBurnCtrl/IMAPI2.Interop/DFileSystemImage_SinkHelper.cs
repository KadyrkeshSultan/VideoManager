using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public sealed class DFileSystemImage_SinkHelper : DFileSystemImageEvents
	{
		private int m_dwCookie;

		private DFileSystemImage_EventHandler m_UpdateDelegate;

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

		public DFileSystemImage_EventHandler UpdateDelegate
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

		public DFileSystemImage_SinkHelper(DFileSystemImage_EventHandler eventHandler)
		{
			if (eventHandler == null)
			{
				throw new ArgumentNullException("Delegate (callback function) cannot be null");
			}
			this.m_dwCookie = 0;
			this.m_UpdateDelegate = eventHandler;
		}

		public void Update(object sender, string currentFile, int copiedSectors, int totalSectors)
		{
			this.m_UpdateDelegate(sender, currentFile, copiedSectors, totalSectors);
		}
	}
}