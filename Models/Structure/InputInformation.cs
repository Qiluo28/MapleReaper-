using System.Runtime.InteropServices;

namespace MapleReaper.Models.Structure
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct InputInformation
    {
        internal uint InputEventType;
        internal InputUnion InputUnion;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)]
        internal KeyboardInputData KeyboardInputData;
        [FieldOffset(0)]
        internal MouseInputData MouseInputData;
    }

    unsafe internal struct KeyboardInputData
    {
        internal ushort VirtualKey;
        internal ushort ScanKey;
        internal uint KeyboardEventFlag;
        internal uint Time;
        internal ulong* ExtraInfo;
    }

    unsafe internal struct MouseInputData
    {
        internal int X;
        internal int Y;
        internal uint MouseData;
        internal uint MouseEventFlag;
        internal uint Time;
        internal ulong* ExtraInfo;
    }
}