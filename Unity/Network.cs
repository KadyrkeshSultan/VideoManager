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
        public static string FormatPath(string path)
        {
            string str = path;
            if (!path.Contains(":"))
            {
                if (!path.StartsWith("\\\\"))
                {
                    str = string.Concat("\\\\", path);
                }
            }
            else if ((!path.Contains(":") ? false : !path.Substring(2, 1).Equals("\\")))
            {
                str = path.Replace(":", ":\\");
            }
            return str;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

        public static string GetDriveLetter(string pPath)
        {
            if (pPath.StartsWith("\\\\"))
            {
                throw new ArgumentException("A UNC path was passed to GetDriveLetter");
            }
            return Directory.GetDirectoryRoot(pPath).Replace(Path.DirectorySeparatorChar.ToString(), "");
        }

        public static DiskInfoData GetUNCDiskInfo(string UNCPath)
        {
            ulong num;
            ulong num1;
            ulong num2;
            DiskInfoData diskInfoDatum = new DiskInfoData();
            if (Network.GetDiskFreeSpaceEx(UNCPath, out num, out num1, out num2))
            {
                diskInfoDatum.FreeBytesAvailable = num;
                diskInfoDatum.TotalNumbersOfBytes = num1;
                diskInfoDatum.TotalNumbersOfFreeBytes = num2;
            }
            return diskInfoDatum;
        }

        public static bool IsNetworkDrive(string pPath)
        {
            bool flag;
            ManagementObject managementObject = new ManagementObject();
            if (!pPath.StartsWith("\\\\"))
            {
                string driveLetter = Network.GetDriveLetter(pPath);
                managementObject.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", driveLetter));
                uint num = Convert.ToUInt32(managementObject["DriveType"]);
                managementObject = null;
                flag = num == 4;
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        public static bool IsUncPath(string path)
        {
            return Network.PathIsUNC(path);
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = false)]
        internal static extern bool PathIsUNC([In] string pszPath);

        public static string ResolveToRootUNC(string pPath)
        {
            string directoryRoot;
            ManagementObject managementObject = new ManagementObject();
            if (!pPath.StartsWith("\\\\"))
            {
                string driveLetter = Network.GetDriveLetter(pPath);
                managementObject.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", driveLetter));
                uint num = Convert.ToUInt32(managementObject["DriveType"]);
                string str = Convert.ToString(managementObject["ProviderName"]);
                managementObject = null;
                directoryRoot = (num != 4 ? string.Concat(driveLetter, Path.DirectorySeparatorChar) : str);
            }
            else
            {
                directoryRoot = Directory.GetDirectoryRoot(pPath);
            }
            return directoryRoot;
        }

        public static string ResolveToUNC(string pPath)
        {
            string str;
            if (!pPath.StartsWith("\\\\"))
            {
                string rootUNC = Network.ResolveToRootUNC(pPath);
                str = (!pPath.StartsWith(rootUNC) ? pPath.Replace(Network.GetDriveLetter(pPath), rootUNC) : pPath);
            }
            else
            {
                str = pPath;
            }
            return str;
        }

        public static bool SetAcl(string destinationDirectory)
        {
            bool flag;
            try
            {
                FileSystemRights fileSystemRight = (FileSystemRights)0;
                fileSystemRight = FileSystemRights.FullControl;
                FileSystemAccessRule fileSystemAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow);
                DirectoryInfo directoryInfo = new DirectoryInfo(destinationDirectory);
                DirectorySecurity accessControl = directoryInfo.GetAccessControl(AccessControlSections.Access);
                bool flag1 = false;
                accessControl.ModifyAccessRule(AccessControlModification.Set, fileSystemAccessRule, out flag1);
                if (flag1)
                {
                    fileSystemAccessRule = new FileSystemAccessRule("Users", fileSystemRight, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow);
                    flag1 = false;
                    accessControl.ModifyAccessRule(AccessControlModification.Add, fileSystemAccessRule, out flag1);
                    if (flag1)
                    {
                        directoryInfo.SetAccessControl(accessControl);
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception exception)
            {
                flag = false;
            }
            return flag;
        }

        public static string smethod_0()
        {
            string str = (
                from o in (IEnumerable<IPAddress>)Dns.GetHostEntry(Dns.GetHostName()).AddressList
                where o.AddressFamily == AddressFamily.InterNetwork
                select o).First<IPAddress>().ToString();
            return str;
        }

        public static string ValidatePath(string folder)
        {
            string str = folder;
            if (!str.Contains(":"))
            {
                if (!str.StartsWith("\\\\"))
                {
                    str = string.Concat("\\\\", str);
                }
            }
            else if ((!str.Contains(":") ? false : !str.Contains(":\\")))
            {
                str = str.Replace(":", ":\\");
            }
            return str;
        }
    }
}