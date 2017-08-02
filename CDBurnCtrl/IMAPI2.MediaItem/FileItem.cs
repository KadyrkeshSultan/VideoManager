using IMAPI2.Interop;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace IMAPI2.MediaItem
{
	internal class FileItem : IMediaItem
	{
		private const long SECTOR_SIZE = 2048L;

		private long m_fileLength;

		private string filePath;

		private Image fileIconImage;

		private string displayName;

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
				return this.filePath;
			}
		}

		public long SizeOnDisc
		{
			get
			{
				if (this.m_fileLength <= 0L)
				{
					return 0L;
				}
				return (this.m_fileLength / 2048L + 1L) * 2048L;
			}
		}

		public FileItem(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException("The file added to FileItem was not found!", path);
			}
			this.filePath = path;
			FileInfo fileInfo = new FileInfo(this.filePath);
			this.displayName = fileInfo.Name;
			this.m_fileLength = fileInfo.Length;
			SHFILEINFO sHFILEINFO = new SHFILEINFO();
			Win32.SHGetFileInfo(this.filePath, 0, ref sHFILEINFO, (uint)Marshal.SizeOf(sHFILEINFO), 257);
			IconConverter iconConverter = new IconConverter();
			Icon icon = null;
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
			IStream stream = null;
			try
			{
				try
				{
					Win32.SHCreateStreamOnFileW(this.filePath, 32, ref stream);
					if (stream != null)
					{
						rootItem.AddFile(this.displayName, stream);
						flag = true;
						return flag;
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					MessageBox.Show(exception.Message, "Error adding file", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				return false;
			}
			finally
			{
				if (stream != null)
				{
					Marshal.FinalReleaseComObject(stream);
				}
			}
			return flag;
		}

		public override string ToString()
		{
			return this.displayName;
		}
	}
}