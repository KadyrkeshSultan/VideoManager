using System;
using System.Runtime.InteropServices;

namespace Cite
{
	public class USB
	{
		private const int OPEN_EXISTING = 3;

		private const uint GENERIC_READ = 2147483648;

		private const uint GENERIC_WRITE = 1073741824;

		private const uint IOCTL_STORAGE_EJECT_MEDIA = 2967560;

		public USB()
		{
		}

		[DllImport("kernel32", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int CloseHandle(IntPtr handle);

		[DllImport("kernel32", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr CreateFile(string filename, uint desiredAccess, uint shareMode, IntPtr securityAttributes, int creationDisposition, int flagsAndAttributes, IntPtr templateFile);

		[DllImport("kernel32", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int DeviceIoControl(IntPtr deviceHandle, uint ioControlCode, IntPtr inBuffer, int inBufferSize, IntPtr outBuffer, int outBufferSize, ref int bytesReturned, IntPtr overlapped);

		public bool EjectDrive(char driveLetter)
		{
			string str = string.Concat("\\\\.\\", driveLetter, ":");
			IntPtr intPtr = USB.CreateFile(str, driveLetter, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
			if ((long)intPtr == -1L)
			{
				return false;
			}
			int num = 0;
			USB.DeviceIoControl(intPtr, 2967560, IntPtr.Zero, 0, IntPtr.Zero, 0, ref num, IntPtr.Zero);
			USB.CloseHandle(intPtr);
			return true;
		}
	}
}