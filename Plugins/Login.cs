using MapleReaper.Libraries;
using MapleReaper.Models;
using System.Diagnostics;

namespace MapleReaper.Plugins
{
    internal static class Login
    {
        public static async Task Execute()
        {
            await Task.Delay(50);
            var process = Process.GetProcesses().FirstOrDefault(m => m.MainWindowTitle == "MapleStory");
            if (process is not null) return;
            if (MemoryReader.GetLoginState() != 0) return;
            if (ReaperSetting.IsScripting)
            {
                ReaperSetting.IsFreezing = true;
                while (ReaperSetting.IsFreezed == false) await Task.Delay(100);
            }
            State.Process.Kill(true);
            State.Process = null;
        }
    }
}