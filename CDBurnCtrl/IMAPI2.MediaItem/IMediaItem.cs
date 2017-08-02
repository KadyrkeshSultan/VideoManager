using IMAPI2.Interop;
using System;
using System.Drawing;

namespace IMAPI2.MediaItem
{
	internal interface IMediaItem
	{
		Image FileIconImage
		{
			get;
		}

		string Path
		{
			get;
		}

		long SizeOnDisc
		{
			get;
		}

		bool AddToFileSystem(IFsiDirectoryItem rootItem);
	}
}