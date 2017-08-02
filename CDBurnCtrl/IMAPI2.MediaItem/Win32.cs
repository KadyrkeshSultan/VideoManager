using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IMAPI2.MediaItem
{
	internal class Win32
	{
		public const uint SHGFI_ICON = 256;

		public const uint SHGFI_LARGEICON = 0;

		public const uint SHGFI_SMALLICON = 1;

		public const uint FILE_ATTRIBUTE_NORMAL = 128;

		public const uint STGM_DELETEONRELEASE = 67108864;

		public const uint STGM_SHARE_DENY_WRITE = 32;

		public const uint STGM_SHARE_DENY_NONE = 64;

		public const uint STGM_READ = 0;

		public Win32()
		{
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool DestroyIcon(IntPtr handle);

		[DllImport("shlwapi.dll", CharSet=CharSet.Unicode, ExactSpelling=true, PreserveSig=false)]
		public static extern void SHCreateStreamOnFileW(string fileName, uint mode, ref IStream stream);

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
	}
}