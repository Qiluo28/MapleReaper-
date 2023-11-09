using MapleReaper.Models;
using MapleReaper.Models.Hook;
using System.Diagnostics;

namespace MapleReaper.Plugins
{
    internal static class DisconnectDetection
    {
        public static async Task Execute()
        {
            await Task.Delay(50);
            var process = Process.GetProcesses().FirstOrDefault(m => m.MainWindowTitle == "MapleStory");
            if (process is null) return;
            var rect = Hook.GetWindowRect(process.MainWindowHandle);
            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;
            if (width != 346 || height != 117) return;
            process.Kill(true);
            State.Process = null;
        }
    }
}
