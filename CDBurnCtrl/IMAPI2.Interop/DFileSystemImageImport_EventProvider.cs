using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace IMAPI2.Interop
{
	[ClassInterface(ClassInterfaceType.None)]
	internal sealed class DFileSystemImageImport_EventProvider : IDisposable, DFileSystemImageImport_Event
	{
		private Hashtable m_aEventSinkHelpers = new Hashtable();

		private IConnectionPoint m_connectionPoint;

		public DFileSystemImageImport_EventProvider(object pointContainer)
		{
			lock (this)
			{
				Guid gUID = typeof(DFileSystemImageImportEvents).GUID;
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
					foreach (DFileSystemImageImport_SinkHelper value in this.m_aEventSinkHelpers.Values)
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

		~DFileSystemImageImport_EventProvider()
		{
			this.Cleanup();
		}

		public event DFileSystemImageImport_EventHandler UpdateImport
		{
			add
			{
				int num;
				lock (this)
				{
					DFileSystemImageImport_SinkHelper dFileSystemImageImportSinkHelper = new DFileSystemImageImport_SinkHelper(value);
					this.m_connectionPoint.Advise(dFileSystemImageImportSinkHelper, out num);
					dFileSystemImageImportSinkHelper.Cookie = num;
					this.m_aEventSinkHelpers.Add(dFileSystemImageImportSinkHelper.UpdateDelegate, dFileSystemImageImportSinkHelper);
				}
			}
			remove
			{
				lock (this)
				{
					DFileSystemImageImport_SinkHelper item = this.m_aEventSinkHelpers[value] as DFileSystemImageImport_SinkHelper;
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