using System;

namespace IMAPI2.MediaItem
{
	public struct SHFILEINFO
	{
		public IntPtr hIcon;

		public IntPtr iIcon;

		public uint dwAttributes;

		public string szDisplayName;

		public string szTypeName;
	}
}