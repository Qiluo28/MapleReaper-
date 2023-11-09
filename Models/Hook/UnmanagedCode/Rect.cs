using System.Runtime.InteropServices;

namespace MapleReaper.Models.Hook
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        internal int Left;
        internal int Top;
        internal int Right;
        internal int Bottom;
    }
}