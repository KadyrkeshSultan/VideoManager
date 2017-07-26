using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Unity
{
    public static class Network
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PathIsUNC([MarshalAs(UnmanagedType.LPWStr), In] string pszPath);

        public static string IPAddress()
        {
            return null;
        }

        public static bool IsUncPath(string path)
        {
            return Network.PathIsUNC(path);
        }

        public static string FormatPath(string path)
        {
            return null;
        }

        public static string ResolveToUNC(string pPath)
        {
            return null;
        }

        public static string ResolveToRootUNC(string pPath)
        {
            return null;
        }

        public static bool IsNetworkDrive(string pPath)
        {
            return false;
        }

        public static string GetDriveLetter(string pPath)
        {
            return null;
        }

        public static DiskInfoData GetUNCDiskInfo(string UNCPath)
        {
            return null;
        }

        public static bool SetAcl(string destinationDirectory)
        {
            try
            {
                FileSystemRights fileSystemRights = FileSystemRights.FullControl;
                FileSystemAccessRule systemAccessRule1 = new FileSystemAccessRule("Network_unknown1", fileSystemRights, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow);
                DirectoryInfo directoryInfo = new DirectoryInfo(destinationDirectory);
                DirectorySecurity accessControl = directoryInfo.GetAccessControl(AccessControlSections.Access);
                bool modified1 = false;
                accessControl.ModifyAccessRule(AccessControlModification.Set, (AccessRule)systemAccessRule1, out modified1);
                if (!modified1)
                    return false;

                InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
                FileSystemAccessRule systemAccessRule2 = new FileSystemAccessRule("Network_unknown2", fileSystemRights, inheritanceFlags, PropagationFlags.InheritOnly, AccessControlType.Allow);
                bool modified2 = false;
                accessControl.ModifyAccessRule(AccessControlModification.Add, (AccessRule)systemAccessRule2, out modified2);
                if (!modified2)
                    return false;
                directoryInfo.SetAccessControl(accessControl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string ValidatePath(string folder)
        {
            return null;
        }
    }
}

