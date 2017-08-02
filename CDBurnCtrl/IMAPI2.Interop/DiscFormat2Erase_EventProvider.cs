using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DiscFormat2Erase_EventProvider : IDisposable, DiscFormat2Erase_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DiscFormat2Erase_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(DDiscFormat2EraseEvents).GUID;
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
					foreach (DiscFormat2Erase_SinkHelper value in this.m_aEventSinkHelpers.Values)
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

		~DiscFormat2Erase_EventProvider()
		{
			this.Cleanup();
		}

		public event DiscFormat2Erase_EventHandler Update
		{
			add
			{
				lock (this)
				{
					DiscFormat2Erase_SinkHelper discFormat2EraseSinkHelper = new DiscFormat2Erase_SinkHelper(value);
					int num = -1;
					this.m_connectionPoint.Advise(discFormat2EraseSinkHelper, out num);
					discFormat2EraseSinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(discFormat2EraseSinkHelper.UpdateDelegate, discFormat2EraseSinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DiscFormat2Erase_SinkHelper item = this.m_aEventSinkHelpers[value] as DiscFormat2Erase_SinkHelper;
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