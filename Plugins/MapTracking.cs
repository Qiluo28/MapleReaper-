using MapleReaper.Libraries;
using MapleReaper.Models;

namespace MapleReaper.Plugins
{
    internal class MapTracking
    {
        private static int mapId;

        public static void Initialize()
        {
            mapId = MemoryReader.GetMapId();
        }

        public static async Task Execute()
        {
            await Task.Delay(100);
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == mapId) return;
            ReaperSetting.IsScripting = ReaperSetting.IsFreezing = ReaperSetting.IsPausing = false;
        }
    }
}
