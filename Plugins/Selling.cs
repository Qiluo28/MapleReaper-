using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;

namespace MapleReaper.Plugins
{
    internal static class Selling
    {
        private static DateTime cooldown;
        private static int count;

        public static void Initialize()
        {
            cooldown = DateTime.MinValue;
        }

        public static async Task Execute()
        {
            await Task.Delay(50);
            if (Settings.Default.IsSellingEnabled == false) return;
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000 || MemoryReader.GetNumberOfUI() > 7) return;
            if ((DateTime.Now - cooldown).TotalSeconds < double.Parse(Settings.Default.SellingDelay)) return;
            ReaperSetting.IsFreezing = true;
            while (ReaperSetting.IsFreezed == false) await Task.Delay(100);
            var result = count++ % 2 == 0 ? await Keyboard.Type(Settings.Default.SellingCommand1) : await Keyboard.Type(Settings.Default.SellingCommand2);
            if (result) cooldown = DateTime.Now;
            ReaperSetting.IsFreezing = false;
        }
    }
}
