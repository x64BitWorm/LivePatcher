using System;
namespace LivePatcher.Native
{
    public struct ProcessInformation
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }
}
