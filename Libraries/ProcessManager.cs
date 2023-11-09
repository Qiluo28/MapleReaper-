using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Models.Hook;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MapleReaper.Libraries
{
    internal static class ProcessManager
    {
        private static Hook.WinEventDelegate winEvent;
        private static GCHandle gcHandle;
        private static IntPtr winEventHook;

        public static bool UpdateState()
        {
            if (gcHandle.IsAllocated) gcHandle.Free();
            if (Hook.IsHooked) Hook.WinEventUnhook(winEventHook);
            var processes = Process.GetProcesses().Where(m => string.IsNullOrEmpty(m.MainWindowTitle) == false);
            var process = processes.FirstOrDefault(m
            => m.MainWindowTitle.Contains('谷')
            || m.MainWindowTitle.Contains("MapleStory")
            || m.MainWindowTitle.Contains("海賊王"));
            if (process is null) return false;
            State.Process = process;
            MemoryReader.TrackAddress();
            winEvent = new Hook.WinEventDelegate(WinEventCallback);
            gcHandle = GCHandle.Alloc(winEvent);
            var threadId = Hook.GetWindowThread(State.Process.MainWindowHandle);
            var rect = Hook.GetWindowRect(State.Process.MainWindowHandle);
            winEventHook = Hook.WinEventHookOne(Events.EVENT_OBJECT_LOCATIONCHANGE, winEvent, (uint)State.Process.Id, threadId);
            State.X = rect.Left;
            State.Y = rect.Top;
            State.Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            Hook.IsHooked = true;
            return true;
        }

        public static void Focus()
        {
            SetForegroundWindow(State.Process.MainWindowHandle);
            Task.Delay(500).Wait();
        }

        private static void WinEventCallback(IntPtr eventHook, Events eventType, IntPtr handle, ObjectIds idObject, long idChild, uint eventThread, uint eventTime)
        {
            if (handle == State.Process.MainWindowHandle && eventType == Events.EVENT_OBJECT_LOCATIONCHANGE && idObject == (ObjectIds)NativeMethods.SelfChildId)
            {
                var rect = Hook.GetWindowRect(handle);
                State.X = rect.Left;
                State.Y = rect.Top;
                State.Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
    }
}