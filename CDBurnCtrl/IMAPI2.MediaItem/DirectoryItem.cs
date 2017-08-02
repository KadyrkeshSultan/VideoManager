using IMAPI2.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IMAPI2.MediaItem
{
	internal class DirectoryItem : IMediaItem
	{
		private List<IMediaItem> mediaItems = new List<IMediaItem>();

		private string m_directoryPath;

		private string displayName;

		private Image fileIconImage;

		public Image FileIconImage
		{
			get
			{
				return this.fileIconImage;
			}
		}

		public string Path
		{
			get
			{
				return this.m_directoryPath;
			}
		}

		public long SizeOnDisc
		{
			get
			{
				long sizeOnDisc = 0L;
				foreach (IMediaItem mediaItem in this.mediaItems)
				{
					sizeOnDisc += mediaItem.SizeOnDisc;
				}
				return sizeOnDisc;
			}
		}

		public DirectoryItem(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				throw new FileNotFoundException("The directory added to DirectoryItem was not found!", directoryPath);
			}
			this.m_directoryPath = directoryPath;
			this.displayName = (new FileInfo(this.m_directoryPath)).Name;
			string[] files = Directory.GetFiles(this.m_directoryPath);
			for (int i = 0; i < (int)files.Length; i++)
			{
				string str = files[i];
				this.mediaItems.Add(new FileItem(str));
			}
			string[] directories = Directory.GetDirectories(this.m_directoryPath);
			for (int j = 0; j < (int)directories.Length; j++)
			{
				string str1 = directories[j];
				this.mediaItems.Add(new DirectoryItem(str1));
			}
			SHFILEINFO sHFILEINFO = new SHFILEINFO();
			Win32.SHGetFileInfo(this.m_directoryPath, 0, ref sHFILEINFO, (uint)Marshal.SizeOf(sHFILEINFO), 257);
			IconConverter iconConverter = new IconConverter();
			Icon icon = Icon.FromHandle(sHFILEINFO.hIcon);
			try
			{
				this.fileIconImage = (Image)iconConverter.ConvertTo(icon, typeof(Image));
			}
			catch (NotSupportedException notSupportedException)
			{
			}
			Win32.DestroyIcon(sHFILEINFO.hIcon);
		}

		public bool AddToFileSystem(IFsiDirectoryItem rootItem)
		{
			bool flag;
			try
			{
				rootItem.AddTree(this.m_directoryPath, true);
				flag = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				MessageBox.Show(exception.Message, "Error adding folder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				flag = false;
			}
			return flag;
		}

		public override string ToString()
		{
			return this.displayName;
		}
	}
}