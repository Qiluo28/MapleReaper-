using System.Runtime.InteropServices;

namespace MapleReaper.Models.Hook
{
    public static class NativeMethods
    {
        [DllImport("DWMAPI.DLL", SetLastError = true)]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, Attributes dwAttribute, out Rect pvAttribute, int cbAttribute);

        [DllImport("USER32.DLL", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("USER32.DLL")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr voidProcessId);

        [DllImport("USER32.DLL", SetLastError = false)]
        public static extern IntPtr SetWinEventHook(Events eventMin, Events eventMax, IntPtr hmodWinEventProc, Hook.WinEventDelegate pfnWinEventProc, uint idProcess, uint idThread, Flags dwFlags);

        [DllImport("USER32.DLL", SetLastError = false)]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        public static long SelfChildId = 0;
        public static Flags WinEventHookInternalFlags = Flags.WINEVENT_OUTOFCONTEXT | Flags.WINEVENT_SKIPOWNPROCESS | Flags.WINEVENT_SKIPOWNTHREAD;
    }
}