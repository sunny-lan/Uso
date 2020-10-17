
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Uso.UWP
{
    public class FileSystemTime 
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);
        public static bool IsAvailable { get; private set; }
        static FileSystemTime()
        {
            try
            {
                long filetime;
                GetSystemTimePreciseAsFileTime(out filetime);
                IsAvailable = true;
            }
            catch (EntryPointNotFoundException)
            {
                IsAvailable = false;
            }
        }

        public static long Now1
        {
            get
            {
                Debug.Assert(IsAvailable);
                long filetime; GetSystemTimePreciseAsFileTime(out filetime);
                return filetime;
            }
        }

        public long Now => FileSystemTime.Now1;

    }
}