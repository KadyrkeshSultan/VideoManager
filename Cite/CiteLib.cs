using LibUsbDotNet.DeviceNotify;
using LibUsbDotNet.DeviceNotify.Info;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Cite
{
	public class CiteLib
	{
		private IDeviceNotifier devNotifier;

		public string AdminPassword
		{
			get;
			set;
		}

		public int BatteryPercent
		{
			get;
			set;
		}

		public int BatteryToFull
		{
			get;
			set;
		}

		public string CamSerial
		{
			get;
			set;
		}

		public string ErrorMsg
		{
			get;
			set;
		}

		public int FreeDiskSpace
		{
			get;
			set;
		}

		public bool IsConnected
		{
			get;
			set;
		}

		public bool IsRegistered
		{
			get;
			set;
		}

		public bool SyncCameraTime
		{
			get;
			set;
		}

		public int TotalDiskSpace
		{
			get;
			set;
		}

		public string USBDrive
		{
			get;
			set;
		}

		public string UserPassword
		{
			get;
			set;
		}

		public CiteLib()
		{
		}

		private void Callback(object sender, CmdCiteEventArgs e)
		{
			if (this.EVT_DevActionCallback != null)
			{
				this.EVT_DevActionCallback(this, e);
			}
		}

		public bool CheckAdminPassword(string pwd, bool mount_usb)
		{
			bool flag = false;
			try
			{
				if (CiteDLL.XX_CheckSystemPwd(this.CamSerial, pwd, mount_usb) == 0)
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}

		public bool CheckUserPassword(string pwd, bool mount_usb)
		{
			bool flag = false;
			try
			{
				if (CiteDLL.XX_CheckUserPwd(this.CamSerial, pwd, mount_usb) == 0)
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}

		private static string ConvertByteArrayToString(byte[] ByteOutput)
		{
			string str = Encoding.ASCII.GetString(ByteOutput);
			return str.Substring(0, str.IndexOf('\0'));
		}

		private void devNotifier_OnDeviceNotify(object sender, DeviceNotifyEventArgs e)
		{
            EventType eventType = e.EventType;
            //e.DeviceType;
            IUsbDeviceNotifyInfo device = e.Device;
            IVolumeNotifyInfo volume = e.Volume;
			if ((int)eventType == 32768 && volume != null)
			{
                this.USBDrive = volume.Letter;
				this.Callback(this, new CmdCiteEventArgs(DEV_ACTIONS.IS_USB));
			}
			if ((int)eventType == 32768)
			{
				if (device.ClassGuid.Equals(Guid.Parse("a5dcbf10-6530-11d2-901f-00c04fb951ed")))
				{
					this.MountDevice();
				}
				if (!string.IsNullOrEmpty(volume.Letter))
				{
					this.USBDrive = volume.Letter;
				}
			}
			if ((int)eventType == 32772)
			{
				this.IsConnected = false;
				this.Callback(this, new CmdCiteEventArgs(DEV_ACTIONS.DISCONNECTED));
			}
		}

		public bool FactoryReset()
		{
			bool flag = false;
			if (this.IsConnected)
			{
				flag = CiteDLL.XX_FactorySettingReset(this.CamSerial);
			}
			return flag;
		}

		public bool Format()
		{
			bool flag = false;
			if (this.IsConnected)
			{
				flag = CiteDLL.XX_Format(this.CamSerial);
			}
			return flag;
		}

		public CameraMenu GetCameraMenu()
		{
			CameraMenu cameraMenu = new CameraMenu();
			int num = 0;
			int num1 = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			try
			{
				if (CiteDLL.XX_GetMenuItemsStatus(this.CamSerial, out num, out num1, out num2, out num3, out num4, out num5, out num6, out num7, out num8, out num9, out num10, out num11, out num12))
				{
					cameraMenu.nEnableVideoResolutionItem = num;
					cameraMenu.nEnableCameraModeSelectionItem = num1;
					cameraMenu.nEnableMotionDetectItem = num2;
					cameraMenu.nEnablePreRecordLastRecordItem = num3;
					cameraMenu.nEnableVioceAnnouncementItem = num4;
					cameraMenu.nEnableButtonSelectSoundItem = num5;
					cameraMenu.nEnableAllVisualIndicatorItem = num6;
					cameraMenu.nEnableLEDIndicatorItem = num7;
					cameraMenu.nEnableLaserIndicatorItem = num8;
					cameraMenu.nEnableInfraredControlItem = num9;
					cameraMenu.nEnableFWVersionItem = num10;
					cameraMenu.nEnableSerialNumberItem = num11;
					cameraMenu.nEnableLanguageItem = num12;
				}
			}
			catch (Exception exception)
			{
				string message = exception.Message;
			}
			return cameraMenu;
		}

		public int GetDateFormat()
		{
			int num = -1;
			int num1 = 0;
			int num2 = 0;
			try
			{
				CiteDLL.XX_GetTime(this.CamSerial, out num1, out num2, out num);
			}
			catch (Exception exception)
			{
				string message = exception.Message;
				num = -1;
			}
			return num;
		}

		public string GetDeviceSerialNumber()
		{
			return this.CamSerial;
		}

		public void GetGPS(out int Open, out int ToFile)
		{
			int num = -1;
			int num1 = -1;
			if (this.IsConnected)
			{
				try
				{
					CiteDLL.XX_GetGPSSetting(this.CamSerial, out num, out num1);
				}
				catch
				{
				}
			}
			Open = num;
			ToFile = num1;
		}

		public Cite_Other GetOtherSettings()
		{
			Cite_Other citeOther = new Cite_Other();
			int num = 0;
			int num1 = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			if (this.IsConnected)
			{
				try
				{
					CiteDLL.XX_GetOtherSetting(this.CamSerial, out num, out num1, out num2, out num3, out num4, out num5, out num6);
					citeOther.pKeyBeep = num;
					citeOther.pSoundIndicator = num1;
					citeOther.pAutoSwitchIR = num2;
					citeOther.pShowID = num3;
					citeOther.pEnableMotionDetect = num4;
					citeOther.Int32_0 = num5;
					citeOther.pFlickerHz = num6;
				}
				catch
				{
				}
			}
			return citeOther;
		}

		public void GetPhotoSettings(out int Res, out int mp)
		{
			int num = -1;
			int num1 = -1;
			if (this.IsConnected)
			{
				CiteDLL.XX_GetPhotographSetting(this.CamSerial, out num, out num1);
			}
			Res = num;
			mp = num1;
		}

		public Cite_Overlay GetTextSettings()
		{
			Cite_Overlay citeOverlay = new Cite_Overlay();
			int num = 0;
			int num1 = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			if (this.IsConnected && CiteDLL.XX_GetTextStampSetting(this.CamSerial, out num, out num1, out num2, out num3, out num4, out num5))
			{
				citeOverlay.AddBadge = num4;
				citeOverlay.AddGPS = num5;
				citeOverlay.TextPos = num;
				citeOverlay.Red = num1;
				citeOverlay.Green = num2;
				citeOverlay.Blue = num3;
			}
			return citeOverlay;
		}

		public Cite_Userinfo GetUserInfo()
		{
			Cite_Userinfo citeUserinfo = new Cite_Userinfo();
			if (this.IsConnected)
			{
				byte[] numArray = new byte[32];
				byte[] numArray1 = new byte[32];
				byte[] numArray2 = new byte[32];
				byte[] numArray3 = new byte[32];
				byte[] numArray4 = new byte[32];
				byte[] numArray5 = new byte[32];
				try
				{
					if (CiteDLL.XX_GetUserInfo(this.CamSerial, numArray, numArray1, numArray2, numArray3, numArray4, numArray5))
					{
						citeUserinfo.DeptName = CiteLib.ConvertByteArrayToString(numArray).Trim();
						citeUserinfo.DeptID = CiteLib.ConvertByteArrayToString(numArray1).Trim();
						citeUserinfo.UserName = CiteLib.ConvertByteArrayToString(numArray2).Trim();
						citeUserinfo.BadgeNum = CiteLib.ConvertByteArrayToString(numArray3).Trim();
						citeUserinfo.FWVer = CiteLib.ConvertByteArrayToString(numArray5).Trim();
						citeUserinfo.UniqueID = CiteLib.ConvertByteArrayToString(numArray4).Trim();
					}
				}
				catch (Exception exception)
				{
					this.ErrorMsg = exception.Message;
				}
			}
			return citeUserinfo;
		}

		public Cite_Video GetVideoSettings()
		{
			Cite_Video citeVideo = new Cite_Video();
			if (this.IsConnected)
			{
				int num = 0;
				int num1 = 0;
				int num2 = 5;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				try
				{
					if (CiteDLL.XX_GetVideoRecordSetting(this.CamSerial, out num, out num1, out num2, out num3, out num4, out num5))
					{
						citeVideo.CycleRecord = num4;
						citeVideo.PreRecord = num5;
						citeVideo.FileType = num3;
						citeVideo.RecordBlock = num2;
						citeVideo.ImageQuality = num1;
						citeVideo.Resolution = num;
					}
				}
				catch
				{
				}
			}
			return citeVideo;
		}

		public void GetVisualIndicators(out int nLEDIndicator, out int nLaserIndicator, out int nSimGpsLedIndicator)
		{
			nLEDIndicator = 0;
			nLaserIndicator = 0;
			nSimGpsLedIndicator = 0;
			if (this.IsConnected)
			{
				try
				{
					CiteDLL.XX_GetViaualIndicatorSetting(this.CamSerial, out nLEDIndicator, out nLaserIndicator, out nSimGpsLedIndicator);
				}
				catch
				{
				}
			}
		}

		public int LoginAdmin(string password)
		{
			int num = -1;
			try
			{
				if (this.IsConnected)
				{
					num = CiteDLL.XX_CheckSystemPwd(this.CamSerial, password);
				}
			}
			catch
			{
			}
			return num;
		}

		public void MountDevice()
		{
			try
			{
				char[,] chrArray = new char[26, 33];
				for (int i = 0; i < 26; i++)
				{
					for (int j = 0; j < 33; j++)
					{
						chrArray[i, j] = '\0';
					}
				}
				if (!CiteDLL.XX_GetDevSerial(chrArray))
				{
					this.Callback(this, new CmdCiteEventArgs(DEV_ACTIONS.NO_ACTION));
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int k = 0; k < 26; k++)
					{
						char[] chrArray1 = new char[33];
						for (int l = 0; l < 33 && chrArray[k, l] != 0; l++)
						{
							chrArray1[l] = chrArray[k, l];
						}
						if (chrArray1[0] != 0)
						{
							stringBuilder.Append(new string(chrArray1));
						}
					}
					this.CamSerial = stringBuilder.ToString().Substring(0, stringBuilder.ToString().IndexOf('\0'));
					if (this.CamSerial.Length > 0)
					{
						int num = 0;
						int num1 = 0;
						if (CiteDLL.XX_GetDeviceBattery(this.CamSerial, out num, out num1))
						{
							this.BatteryPercent = num;
							this.BatteryToFull = num1;
						}
						int num2 = 0;
						int num3 = 0;
						if (CiteDLL.XX_GetDeriveCap(this.CamSerial, out num2, out num3))
						{
							this.TotalDiskSpace = num2;
							this.FreeDiskSpace = num3;
						}
						if (this.SyncCameraTime)
						{
							int dateFormat = this.GetDateFormat();
							CiteDLL.XX_SetTime(this.CamSerial, dateFormat);
						}
						this.IsConnected = true;
						this.Callback(this, new CmdCiteEventArgs(DEV_ACTIONS.IS_CITE));
					}
				}
			}
			catch (Exception exception)
			{
				this.ErrorMsg = exception.Message;
			}
		}

		public bool PowerOff()
		{
			bool flag = false;
			try
			{
				if (CiteDLL.XX_PowerOff(this.CamSerial))
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				string message = exception.Message;
			}
			return flag;
		}

		public bool RebootCamera()
		{
			bool flag = false;
			try
			{
				if (this.IsConnected && CiteDLL.XX_Reboot(this.CamSerial))
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				string message = exception.Message;
			}
			return flag;
		}

		public void RegDeviceEvent(bool b)
		{
			if (this.devNotifier == null)
			{
				this.devNotifier = DeviceNotifier.OpenDeviceNotifier();
			}
			try
			{
				switch (b)
				{
					case false:
					{
						this.IsRegistered = false;
						this.devNotifier.OnDeviceNotify -= (new EventHandler<DeviceNotifyEventArgs>(this.devNotifier_OnDeviceNotify));
						break;
					}
					case true:
					{
						this.IsRegistered = true;
						this.USBDrive = string.Empty;
						this.devNotifier.OnDeviceNotify -= (new EventHandler<DeviceNotifyEventArgs>(this.devNotifier_OnDeviceNotify));
						this.devNotifier.OnDeviceNotify += (new EventHandler<DeviceNotifyEventArgs>(this.devNotifier_OnDeviceNotify));
						break;
					}
				}
			}
			catch
			{
			}
		}

		public bool ResetAdminPassword(string OldPwd, string NewPwd)
		{
			bool flag = false;
			try
			{
				if (this.IsConnected && CiteDLL.XX_SetSystemPwd(this.CamSerial, OldPwd, NewPwd) == 0)
				{
					flag = true;
				}
			}
			catch
			{
			}
			return flag;
		}

		public bool ResetUserPassword(string OldPwd, string NewPwd)
		{
			bool flag = false;
			try
			{
				if (this.IsConnected && CiteDLL.XX_SetUserPwd(this.CamSerial, OldPwd, NewPwd) == 0)
				{
					flag = true;
				}
			}
			catch
			{
			}
			return flag;
		}

		public bool SetCameraMenu(CameraMenu cm)
		{
			bool flag = false;
			try
			{
				if (CiteDLL.XX_EnableMenuItems(this.CamSerial, cm.nEnableVideoResolutionItem, cm.nEnableCameraModeSelectionItem, cm.nEnableMotionDetectItem, cm.nEnablePreRecordLastRecordItem, cm.nEnableVioceAnnouncementItem, cm.nEnableButtonSelectSoundItem, cm.nEnableAllVisualIndicatorItem, cm.nEnableLEDIndicatorItem, cm.nEnableLaserIndicatorItem, cm.nEnableInfraredControlItem, cm.nEnableFWVersionItem, cm.nEnableSerialNumberItem, cm.nEnableLanguageItem))
				{
					flag = true;
				}
			}
			catch
			{
			}
			return flag;
		}

		public bool SetCiteSerialNumber(string sn)
		{
			bool flag = false;
			try
			{
				if (this.IsConnected)
				{
					flag = CiteDLL.XX_SetDeviceSerial(this.CamSerial, sn);
				}
			}
			catch
			{
			}
			return flag;
		}

		public void SetDateFormat(int i)
		{
			try
			{
				CiteDLL.XX_SetTime(this.CamSerial, i);
			}
			catch
			{
			}
		}

		public bool SetGPSSetting(int Open, int ToFile)
		{
			bool flag = false;
			if (this.IsConnected)
			{
				try
				{
					flag = CiteDLL.XX_SetGPSSetting(this.CamSerial, Open, ToFile);
				}
				catch
				{
				}
			}
			return flag;
		}

		public bool SetOtherSettings(Cite_Other cOther)
		{
			bool flag = false;
			if (this.IsConnected)
			{
				flag = CiteDLL.XX_SetOtherSetting(this.CamSerial, cOther.pKeyBeep, cOther.pSoundIndicator, cOther.pAutoSwitchIR, cOther.pShowID, cOther.pEnableMotionDetect, cOther.Int32_0, cOther.pFlickerHz);
			}
			return flag;
		}

		public bool SetPhotoSettings(int Res, int mp)
		{
			bool flag = false;
			if (this.IsConnected)
			{
				try
				{
					flag = CiteDLL.XX_SetPhotographSetting(this.CamSerial, Res, mp);
				}
				catch
				{
				}
			}
			return flag;
		}

		public bool SetTextSettings(Cite_Overlay co)
		{
			bool flag = false;
			if (this.IsConnected)
			{
				flag = CiteDLL.XX_SetTextStampSetting(this.CamSerial, co.TextPos, co.Red, co.Green, co.Blue, co.AddBadge, co.AddGPS);
			}
			return flag;
		}

		public bool SetUserInfo(string DeptName, string DeptID, string UserName, string BadgeNo, string CamNo)
		{
			bool flag = false;
			if (this.IsConnected)
			{
				try
				{
					flag = CiteDLL.XX_SetUserInfo(this.CamSerial, DeptName, DeptID, UserName, BadgeNo, CamNo);
				}
				catch (Exception exception)
				{
					this.ErrorMsg = exception.Message;
				}
			}
			return flag;
		}

		public bool SetVideoSettings(Cite_Video cVid)
		{
			bool flag = false;
			if (this.IsConnected)
			{
				try
				{
					flag = CiteDLL.XX_SetVideoRecordSetting(this.CamSerial, cVid.Resolution, cVid.ImageQuality, cVid.RecordBlock, cVid.FileType, cVid.CycleRecord, cVid.PreRecord);
				}
				catch
				{
				}
			}
			return flag;
		}

		public bool SetVisualIndicators(int nLEDIndicator, int nLaserIndicator, int nSimGpsLedIndicator)
		{
			bool flag = false;
			if (this.IsConnected)
			{
				flag = CiteDLL.XX_SetViaualIndicatorSetting(this.CamSerial, nLEDIndicator, nLaserIndicator, nSimGpsLedIndicator);
			}
			return flag;
		}

		public event CiteLib.DEL_DevActionCallback EVT_DevActionCallback;

		public delegate void DEL_DevActionCallback(object sender, CmdCiteEventArgs e);
	}
}