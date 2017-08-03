using System;
using System.Runtime.InteropServices;

namespace Cite
{
	public static class CiteDLL
	{
		private const string _dll = "UMDSC.DLL";

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int XX_CheckSystemPwd(string pSerial, string pPwd);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int XX_CheckSystemPwd(string pSerial, string pOldPwd, bool bSwitchToUDisk);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int XX_CheckUserPwd(string pSerial, string pOldPwd, bool bSwitchToUDisk);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_EnableMenuItems(string pSerial, int nEnableVideoResolutionItem, int nEnableCameraModeSelectionItem, int nEnableMotionDetectItem, int nEnablePreRecordLastRecordItem, int nEnableVioceAnnouncementItem, int nEnableButtonSelectSoundItem, int nEnableAllVisualIndicatorItem, int nEnableLEDIndicatorItem, int nEnableLaserIndicatorItem, int nEnableInfraredControlItem, int nEnableFWVersionItem, int nEnableSerialNumberItem, int nEnableLanguageItem);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_FactorySettingReset(string pSerial);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_Format(string pSerial);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetDeriveCap(string pSerial, out int pDeriveCap, out int pFreeCap);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetDeviceBattery(string pSerial, out int pBatteryPercent, out int pNeedTimeForBatteryFull);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetDevSerial([Out] char[,] Items);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetGPSSetting(string pSerial, out int pAlwaysOpen, out int pSaveCoordinatesToFile);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetMenuItemsStatus(string pSerial, out int pEnableVideoResolutionItem, out int pEnableCameraModeSelectionItem, out int pEnableMotionDetectItem, out int pEnablePreRecordLastRecordItem, out int pEnableVioceAnnouncementItem, out int pEnableButtonSelectSoundItem, out int pEnableAllVisualIndicatorItem, out int pEnableLEDIndicatorItem, out int pEnableLaserIndicatorItem, out int pEnableInfraredControlItem, out int pEnableFWVersionItem, out int pEnableSerialNumberItem, out int pEnableLanguageItem);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetOtherSetting(string pSerial, out int pKeyBeep, out int pSoundIndicator, out int pAutoSwitchIR, out int pShowID, out int pEnableMotionDetect, out int int_0, out int pFlickerHz);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetPhotographSetting(string pSerial, out int pResolution, out int pCaptureQtyOneTime);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetTextStampSetting(string pSerial, out int pStampPos, out int pStampColorRed, out int pStampColorGreen, out int pStampColorBlue, out int pAddBadgeNo, out int pAddGpsCoordinate);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetTime(string pSerial, out int pLowDateTime, out int pHightDateTime, out int pDateFormat);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi, ExactSpelling=false)]
		public static extern bool XX_GetUserInfo(string pSerial, [In][Out] byte[] pDeptName, [In][Out] byte[] pDeptID, [In][Out] byte[] pUserName, [In][Out] byte[] pBadgeNo, [In][Out] byte[] byte_0, [In][Out] byte[] byte_1);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetViaualIndicatorSetting(string pSerial, out int pLEDIndicator, out int pLaserIndicator, out int pSimGpsLedIndicator);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_GetVideoRecordSetting(string pSerial, out int pResolution, out int pImageQuality, out int pRecordBlock, out int pVideoFileType, out int pCycleRecord, out int pPreRecordExtRecordSeconds);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_PowerOff(string pSerial);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_Reboot(string pSerial);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetDeviceSerial(string pSerial, string pNewSerial);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetGPSSetting(string pSerial, int nAlwaysOpen, int nSaveCoordinatesToFile);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetOtherSetting(string pSerial, int nKeyBeep, int nSoundIndicator, int nAutoSwitchIR, int nShowID, int nEnableMotionDetect, int int_0, int pFlickerHz);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetPhotographSetting(string pSerial, int nResolution, int nCaptureQtyOneTime);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi, ExactSpelling=false)]
		public static extern int XX_SetSystemPwd(string pSerial, string pOldPwd, string pNewPwd);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetTextStampSetting(string pSerial, int nStampPos, int nStampColorRed, int nStampColorGreen, int nStampColorBlue, int nAddBadgeNo, int nAddGpsCoordinate);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetTime(string pSerial, int nDateFormat);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi, ExactSpelling=false)]
		public static extern bool XX_SetUserInfo(string pSerial, string DeptName, string DeptID, string UserName, string Badge, string CamSerial);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi, ExactSpelling=false)]
		public static extern int XX_SetUserPwd(string pSerial, string pOldPwd, string pNewPwd);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetViaualIndicatorSetting(string pSerial, int nLEDIndicator, int nLaserIndicator, int nSimGpsLedIndicator);

		[DllImport("UMDSC.DLL", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool XX_SetVideoRecordSetting(string pSerial, int nResolution, int nImageQuality, int nRecordBlock, int nVideoFileType, int nCycleRecord, int nPreRecordExtRecordSeconds);
	}
}