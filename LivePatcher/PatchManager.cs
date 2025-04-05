using System;
using static NativeMethods;
using System.Runtime.InteropServices;
using System.Diagnostics;
using LivePatcher.Native;

namespace LivePatcher
{
    public class PatchManager
    {
        private ProcessInformation _process;

        public PatchManager() {}

        public void LoadFile(string path)
        {
            StartupInfo si = default;
            _process = default;
            si.cb = (uint)Marshal.SizeOf(typeof(StartupInfo));
            if (!CreateProcess(path, null, IntPtr.Zero, IntPtr.Zero, false,
                ProcessCreationFlags.CREATE_SUSPENDED, IntPtr.Zero, null, ref si, out _process))
            {
                _process.dwProcessId = 0;
            }
        }

        public void RunProcess()
        {
            ResumeThread(_process.hThread);
        }

        public void WriteMemory(long address, byte[] data)
        {
            int written = 0;
            WriteProcessMemory(_process.hProcess, address, data, data.Length, ref written);
        }

        public byte[] ReadMemory(long address, int size)
        {
            byte[] result = new byte[size];
            int read = 0;
            ReadProcessMemory(_process.hProcess, address, result, size, ref read);
            return result;
        }

        public long Allocate(int size)
        {
            return VirtualAllocEx(_process.hProcess, 0, (uint)size, AllocationType.MEM_RESERVE | AllocationType.MEM_COMMIT,
                PageProtection.PAGE_EXECUTE_READWRITE);
        }

        public ulong StartThread(long address)
        {
            ulong threadId;
            CreateRemoteThread(_process.hProcess, 0, 0, address, 0, 0, out threadId);
            return threadId;
        }

        public long DllOffset(string dll)
        {
            var process = Process.GetProcessById((int)_process.dwProcessId);
            for (var i = 0; i < process.Modules.Count; i++)
            {
                if(process.Modules[i].ModuleName.Equals(dll, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (long)process.Modules[i].BaseAddress;
                }
            }
            return 0;
        }
    }
}
