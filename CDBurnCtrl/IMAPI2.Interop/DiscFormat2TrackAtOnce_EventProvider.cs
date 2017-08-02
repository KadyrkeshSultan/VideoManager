using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DiscFormat2TrackAtOnce_EventProvider : IDisposable, DiscFormat2TrackAtOnce_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DiscFormat2TrackAtOnce_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(DDiscFormat2TrackAtOnceEvents).GUID;
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
					foreach (DiscFormat2TrackAtOnce_SinkHelper value in this.m_aEventSinkHelpers.Values)
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

		~DiscFormat2TrackAtOnce_EventProvider()
		{
			this.Cleanup();
		}

		public event DiscFormat2TrackAtOnce_EventHandler Update
		{
			add
			{
				int num;
				lock (this)
				{
					DiscFormat2TrackAtOnce_SinkHelper discFormat2TrackAtOnceSinkHelper = new DiscFormat2TrackAtOnce_SinkHelper(value);
					this.m_connectionPoint.Advise(discFormat2TrackAtOnceSinkHelper, out num);
					discFormat2TrackAtOnceSinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(discFormat2TrackAtOnceSinkHelper.UpdateDelegate, discFormat2TrackAtOnceSinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DiscFormat2TrackAtOnce_SinkHelper item = this.m_aEventSinkHelpers[value] as DiscFormat2TrackAtOnce_SinkHelper;
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