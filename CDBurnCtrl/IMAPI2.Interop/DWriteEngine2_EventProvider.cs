using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DWriteEngine2_EventProvider : IDisposable, DWriteEngine2_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DWriteEngine2_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(GInterface1).GUID;
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
					foreach (DWriteEngine2_SinkHelper value in this.m_aEventSinkHelpers.Values)
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

		~DWriteEngine2_EventProvider()
		{
			this.Cleanup();
		}

		public event DWriteEngine2_EventHandler Update
		{
			add
			{
				int num;
				lock (this)
				{
					DWriteEngine2_SinkHelper dWriteEngine2SinkHelper = new DWriteEngine2_SinkHelper(value);
					this.m_connectionPoint.Advise(dWriteEngine2SinkHelper, out num);
					dWriteEngine2SinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(dWriteEngine2SinkHelper.UpdateDelegate, dWriteEngine2SinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DWriteEngine2_SinkHelper item = this.m_aEventSinkHelpers[value] as DWriteEngine2_SinkHelper;
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