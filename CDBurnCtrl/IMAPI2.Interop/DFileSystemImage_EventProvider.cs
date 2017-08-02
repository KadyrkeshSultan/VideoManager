using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DFileSystemImage_EventProvider : IDisposable, DFileSystemImage_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DFileSystemImage_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(DFileSystemImageEvents).GUID;
				(pointContainer as IConnectionPointContainer).FindConnectionPoint(ref gUID, out this.m_connectionPoint);
			}
		}

		private void Cleanup()
		{
			Monitor.Enter(this);
			try
			{
				try
				{
					foreach (DFileSystemImage_SinkHelper value in this.m_aEventSinkHelpers.Values)
					{
						this.m_connectionPoint.Unadvise(value.Cookie);
					}
					this.m_aEventSinkHelpers.Clear();
					Marshal.ReleaseComObject(this.m_connectionPoint);
				}
				catch (SynchronizationLockException synchronizationLockException)
				{
					return;
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
		}

		public void Dispose()
		{
			this.Cleanup();
			GC.SuppressFinalize(this);
		}

		~DFileSystemImage_EventProvider()
		{
			this.Cleanup();
		}

		public event DFileSystemImage_EventHandler Update
		{
			add
			{
				int num;
				lock (this)
				{
					DFileSystemImage_SinkHelper dFileSystemImageSinkHelper = new DFileSystemImage_SinkHelper(value);
					this.m_connectionPoint.Advise(dFileSystemImageSinkHelper, out num);
					dFileSystemImageSinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(dFileSystemImageSinkHelper.UpdateDelegate, dFileSystemImageSinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DFileSystemImage_SinkHelper item = this.m_aEventSinkHelpers[value] as DFileSystemImage_SinkHelper;
					if (item != null)
					{
						this.m_connectionPoint.Unadvise(item.Cookie);
						this.m_aEventSinkHelpers.Remove(item.UpdateDelegate);
					}
				}
			}
		}
	}
}