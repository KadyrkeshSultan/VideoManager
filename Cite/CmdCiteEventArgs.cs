using System;

namespace Cite
{
	public class CmdCiteEventArgs : EventArgs
	{
		public readonly DEV_ACTIONS action;

		public CmdCiteEventArgs(DEV_ACTIONS data)
		{
			this.action = data;
		}
	}
}