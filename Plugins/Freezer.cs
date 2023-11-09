using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;

namespace MapleReaper.Plugins
{
    internal static class Freezer
    {
        public static async Task Execute()
        {
            await Task.Delay(100);
            if (Settings.Default.IsFreezerEnabled == false) return;
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000) return;
            if (ReaperSetting.IsFreezing == false && MemoryReader.GetPlayerCount() == 0) return;
            if (ReaperSetting.IsFreezing && MemoryReader.GetPlayerCount() == 0)
            {
                ReaperSetting.IsFreezing = false;
                return;
            }
            if (ReaperSetting.IsFreezing == false && MemoryReader.GetPlayerCount() > 0)
            {
                ReaperSetting.IsFreezing = true;
            }
        }
    }
}