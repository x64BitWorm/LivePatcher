using LivePatcher.Native;
using System;
using System.Runtime.InteropServices;

public static class NativeMethods
{
    [DllImport("kernel32.dll")]
    public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine,
        IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles,
        ProcessCreationFlags dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory,
        ref StartupInfo lpStartupInfo, out ProcessInformation lpProcessInformation);

    [DllImport("kernel32.dll")]
    public static extern uint ResumeThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    public static extern uint SuspendThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(IntPtr hProcess, long lpBaseAddress, byte[] lpBuffer,
        int dwSize, ref int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, long lpBaseAddress, byte[] lpBuffer,
        int dwSize,ref int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    public static extern long VirtualAllocEx(IntPtr hProcess, int lpAddress, uint dwSize,
        AllocationType flAllocationType, PageProtection flProtect);

    [DllImport("kernel32.dll")]
    public static extern int CreateRemoteThread(IntPtr hProcess, uint lpThreadAttributes, uint dwStackSize,
        long lpStartAddress, int lpParameter, uint dwCreationFlags, out ulong lpThreadId);
}
