using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DiscFormat2RawCD_EventProvider : IDisposable, DiscFormat2RawCD_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DiscFormat2RawCD_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(DDiscFormat2RawCDEvents).GUID;
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
					foreach (DiscFormat2RawCD_SinkHelper value in this.m_aEventSinkHelpers.Values)
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

		~DiscFormat2RawCD_EventProvider()
		{
			this.Cleanup();
		}

		public event DiscFormat2RawCD_EventHandler Update
		{
			add
			{
				int num;
				lock (this)
				{
					DiscFormat2RawCD_SinkHelper discFormat2RawCDSinkHelper = new DiscFormat2RawCD_SinkHelper(value);
					this.m_connectionPoint.Advise(discFormat2RawCDSinkHelper, out num);
					discFormat2RawCDSinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(discFormat2RawCDSinkHelper.UpdateDelegate, discFormat2RawCDSinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DiscFormat2RawCD_SinkHelper item = this.m_aEventSinkHelpers[value] as DiscFormat2RawCD_SinkHelper;
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