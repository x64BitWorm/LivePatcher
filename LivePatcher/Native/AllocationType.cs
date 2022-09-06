using System;

namespace LivePatcher.Native
{
    [Flags]
    public enum AllocationType : uint
    {
        MEM_COMMIT = 0x1000,
        MEM_RESERVE = 0x2000,
        MEM_DECOMMIT = 0x4000,
        MEM_RELEASE = 0x8000,
        MEM_FREE = 0x10000,
        MEM_PRIVATE = 0x20000,
        MEM_MAPPED = 0x40000,
        MEM_RESET = 0x80000,
        MEM_TOP_DOWN = 0x100000,
        MEM_WRITE_WATCH = 0x200000,
        MEM_PHYSICAL = 0x400000,
        MEM_ROTATE = 0x800000,
        MEM_LARGE_PAGES = 0x20000000,
        MEM_4MB_PAGES = 0x80000000
    }
}
