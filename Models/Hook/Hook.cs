using System.Runtime.InteropServices;

namespace MapleReaper.Models.Hook
{
    public class Hook
    {
        public delegate void WinEventDelegate(
            IntPtr winEventHook,
            Events eventType,
            IntPtr handle,
            ObjectIds objectId,
            long childId,
            uint eventThread,
            uint eventTime
        );

        public static bool IsHooked = false;

        public static IntPtr WinEventHookRange(Events eventFrom, Events eventTo, WinEventDelegate eventDelegate, uint idProcess, uint idThread)
        {
            return NativeMethods.SetWinEventHook(eventFrom, eventTo, IntPtr.Zero, eventDelegate, idProcess, idThread, NativeMethods.WinEventHookInternalFlags);
        }

        public static IntPtr WinEventHookOne(Events eventId, WinEventDelegate eventDelegate, uint idProcess, uint idThread)
        {
            return NativeMethods.SetWinEventHook(eventId, eventId, IntPtr.Zero, eventDelegate, idProcess, idThread, NativeMethods.WinEventHookInternalFlags);
        }

        public static bool WinEventUnhook(IntPtr hWinEventHook)
        {
            return NativeMethods.UnhookWinEvent(hWinEventHook);
        }

        public static uint GetWindowThread(IntPtr hWnd)
        {
            return NativeMethods.GetWindowThreadProcessId(hWnd, IntPtr.Zero);
        }

        public static Rect GetWindowRect(IntPtr hWnd)
        {
            NativeMethods.DwmGetWindowAttribute(hWnd, Attributes.DWMWA_EXTENDED_FRAME_BOUNDS, out Rect rect, Marshal.SizeOf<Rect>());
            return rect;
        }
    }
}