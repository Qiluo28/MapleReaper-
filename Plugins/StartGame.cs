using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Models.Hook;
using MapleReaper.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MapleReaper.Plugins
{
    internal static class StartGame
    {
        private static Hook.WinEventDelegate winEvent;
        private static GCHandle gcHandle;
        private static IntPtr winEventHook;
        private static int channel;

        public static void Initialize()
        {
            channel = MemoryReader.GetChannel();
        }

        public static async Task Execute()
        {
            await Task.Delay(50);
            if (State.Process is not null) return;
            var filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "*登入器.exe", SearchOption.AllDirectories);
            if (filePaths.Length == 0) return;
            State.Process = new Process();
            State.Process.StartInfo.Verb = "runas";
            State.Process.StartInfo.FileName = filePaths[0];
            State.Process.Start();
            while (Process.GetProcesses().Any(m => m.MainWindowTitle.Contains("登入器")) == false) await Task.Delay(1000);
            var rect = Hook.GetWindowRect(State.Process.MainWindowHandle);
            State.X = rect.Left;
            State.Y = rect.Top;
            while (Process.GetProcesses().Any(m => m.MainWindowTitle.Contains("登入器")))
            {
                await Mouse.Move(730, 250, 200);
                await Mouse.LeftClick(5000);
            }
            while (Process.GetProcesses().Any(m
            => m.MainWindowTitle.Contains('谷')
            || m.MainWindowTitle.Contains("MapleStory")
            || m.MainWindowTitle.Contains("海賊王")) == false) await Task.Delay(1000);
            State.Process = Process.GetProcesses().FirstOrDefault(m
            => m.MainWindowTitle.Contains('谷')
            || m.MainWindowTitle.Contains("MapleStory")
            || m.MainWindowTitle.Contains("海賊王"));
            if (gcHandle.IsAllocated) gcHandle.Free();
            if (Hook.IsHooked) Hook.WinEventUnhook(winEventHook);
            winEvent = new Hook.WinEventDelegate(WinEventCallback);
            gcHandle = GCHandle.Alloc(winEvent);
            var threadId = Hook.GetWindowThread(State.Process.MainWindowHandle);
            rect = Hook.GetWindowRect(State.Process.MainWindowHandle);
            winEventHook = Hook.WinEventHookOne(Events.EVENT_OBJECT_LOCATIONCHANGE, winEvent, (uint)State.Process.Id, threadId);
            State.X = rect.Left;
            State.Y = rect.Top;
            State.Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            Hook.IsHooked = true;
            MemoryReader.TrackAddress();
            while (MemoryReader.IsTypingAccount() == false) await Task.Delay(500);
            await Mouse.Move(550, 275, 200);
            await Mouse.LeftClick(200);
            await Keyboard.Clear();
            await Keyboard.CopyPaste(Settings.Default.Account);
            await Mouse.Move(550, 305, 200);
            await Mouse.LeftClick(200);
            await Keyboard.CopyPaste(Settings.Default.Password);
            while (OpenCV.TemplateMatch(Resources.ChannelSelection) == false) await Keyboard.KeyPress(Keys.Enter, 2000);
            await SelectChannel();
            while (MemoryReader.GetLoginState() == 0) await Keyboard.KeyPress(Keys.Enter, 2000);
            MemoryReader.TrackAddress();
            await Keyboard.Type("", "@gashaponmega", "@tsmega");
            if (ReaperSetting.IsScripting)
            {
                ReaperSetting.IsFreezing = false;
            }
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

        private static async Task SelectChannel()
        {
            channel = channel >= 0 && channel <= 19 ? channel : new Random().Next(1, 20);
            await Mouse.Move(channel % 4 * 93 + 280, channel / 4 * 31 + 325, 200);
            await Mouse.LeftClick(200);
        }
    }
}
