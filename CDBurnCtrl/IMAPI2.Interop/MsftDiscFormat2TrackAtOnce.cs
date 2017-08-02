using System.Runtime.InteropServices;

namespace IMAPI2.Interop
{
	[CoClass(typeof(MsftDiscFormat2TrackAtOnceClass))]
	[Guid("27354154-8F64-5B0F-8F00-5D77AFBE261E")]
	public interface MsftDiscFormat2TrackAtOnce : DiscFormat2TrackAtOnce_Event, IDiscFormat2TrackAtOnce
	{

	}
}