using System;
using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[Guid("27354132-7F64-5B0F-8F00-5D77AFBE261E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDiscRecorder2Ex
	{
		void GetAdapterDescriptor(out IntPtr data, ref uint byteSize);

		uint GetByteAlignmentMask();

		void GetDeviceDescriptor(out IntPtr data, ref uint byteSize);

		void GetDiscInformation(out IntPtr discInformation, ref uint byteSize);

		void GetFeaturePage(IMAPI_FEATURE_PAGE_TYPE requestedFeature, sbyte currentFeatureOnly, out IntPtr featureData, ref uint byteSize);

		uint GetMaximumNonPageAlignedTransferSize();

		uint GetMaximumPageAlignedTransferSize();

		void GetModePage(IMAPI_MODE_PAGE_TYPE requestedModePage, IMAPI_MODE_PAGE_REQUEST_TYPE requestType, out IntPtr modePageData, ref uint byteSize);

		void GetSupportedFeaturePages(sbyte currentFeatureOnly, out IntPtr featureData, ref uint byteSize);

		void GetSupportedModePages(IMAPI_MODE_PAGE_REQUEST_TYPE requestType, out IntPtr modePageTypes, out uint validPages);

		void GetSupportedProfiles(sbyte currentOnly, out IntPtr profileTypes, out uint validProfiles);

		void GetTrackInformation(uint address, IMAPI_READ_TRACK_ADDRESS_TYPE addressType, out IntPtr trackInformation, ref uint byteSize);

		void ReadDvdStructure(uint format, uint address, uint layer, uint agid, out IntPtr data, out uint Count);

		void SendCommandGetDataFromDevice(byte[] Cdb, uint CdbSize, byte[] SenseBuffer, uint Timeout, byte[] Buffer, uint BufferSize, out uint BufferFetched);

		void SendCommandNoData(byte[] Cdb, uint CdbSize, byte[] SenseBuffer, uint Timeout);

		void SendCommandSendDataToDevice(byte[] Cdb, uint CdbSize, byte[] SenseBuffer, uint Timeout, byte[] Buffer, uint BufferSize);

		void SendDvdStructure(uint format, byte[] data, uint Count);

		void SetModePage(IMAPI_MODE_PAGE_REQUEST_TYPE requestType, byte[] data, uint byteSize);
	}
}