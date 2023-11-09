using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;

namespace MapleReaper.Plugins
{
    internal static class Rebirth
    {
        private static DateTime cooldown;

        public static void Initialize()
        {
            cooldown = DateTime.MinValue;
        }

        public static async Task Execute()
        {
            await Task.Delay(100);
            if (Settings.Default.IsRebirthEnabled == false) return;
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000 || MemoryReader.GetNumberOfUI() > 7) return;
            if ((DateTime.Now - cooldown).TotalSeconds < double.Parse(Settings.Default.RebirthDelay)) return;
            ReaperSetting.IsFreezing = true;
            while (ReaperSetting.IsFreezed == false) await Task.Delay(100);
            if (await Keyboard.Type(Settings.Default.RebirthCommand)) cooldown = DateTime.Now;
            ReaperSetting.IsFreezing = false;
        }
    }
}
