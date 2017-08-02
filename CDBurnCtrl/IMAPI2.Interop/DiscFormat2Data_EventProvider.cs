using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DiscFormat2Data_EventProvider : IDisposable, DiscFormat2Data_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DiscFormat2Data_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(DDiscFormat2DataEvents).GUID;
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
					foreach (DiscFormat2Data_SinkHelper value in this.m_aEventSinkHelpers.Values)
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

		~DiscFormat2Data_EventProvider()
		{
			this.Cleanup();
		}

		public event DiscFormat2Data_EventHandler Update
		{
			add
			{
				int num;
				lock (this)
				{
					DiscFormat2Data_SinkHelper discFormat2DataSinkHelper = new DiscFormat2Data_SinkHelper(value);
					this.m_connectionPoint.Advise(discFormat2DataSinkHelper, out num);
					discFormat2DataSinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(discFormat2DataSinkHelper.UpdateDelegate, discFormat2DataSinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DiscFormat2Data_SinkHelper item = this.m_aEventSinkHelpers[value] as DiscFormat2Data_SinkHelper;
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