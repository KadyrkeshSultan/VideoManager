using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DiscMaster2_EventProvider : IDisposable, DiscMaster2_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DiscMaster2_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(GInterface0).GUID;
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
					foreach (DiscMaster2_SinkHelper value in this.m_aEventSinkHelpers.Values)
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

		~DiscMaster2_EventProvider()
		{
			this.Cleanup();
		}

		public event DiscMaster2_NotifyDeviceAddedEventHandler NotifyDeviceAdded
		{
			add
			{
				int num;
				lock (this)
				{
					DiscMaster2_SinkHelper discMaster2SinkHelper = new DiscMaster2_SinkHelper(value);
					this.m_connectionPoint.Advise(discMaster2SinkHelper, out num);
					discMaster2SinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(discMaster2SinkHelper.NotifyDeviceAddedDelegate, discMaster2SinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DiscMaster2_SinkHelper item = this.m_aEventSinkHelpers[value] as DiscMaster2_SinkHelper;
					if (item != null)
					{
						this.m_connectionPoint.Unadvise(item.Cookie);
						this.m_aEventSinkHelpers.Remove(item.NotifyDeviceAddedDelegate);
					}
				}
			}
		}

		public event DiscMaster2_NotifyDeviceRemovedEventHandler NotifyDeviceRemoved
		{
			add
			{
				int num;
				lock (this)
				{
					DiscMaster2_SinkHelper discMaster2SinkHelper = new DiscMaster2_SinkHelper(value);
					this.m_connectionPoint.Advise(discMaster2SinkHelper, out num);
					discMaster2SinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(discMaster2SinkHelper.NotifyDeviceRemovedDelegate, discMaster2SinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DiscMaster2_SinkHelper item = this.m_aEventSinkHelpers[value] as DiscMaster2_SinkHelper;
					if (item != null)
					{
						this.m_connectionPoint.Unadvise(item.Cookie);
						this.m_aEventSinkHelpers.Remove(item.NotifyDeviceRemovedDelegate);
					}
				}
			}
		}
	}
}