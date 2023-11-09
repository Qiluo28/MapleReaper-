using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;

namespace MapleReaper.Plugins
{
    internal static class UIManager
    {
        public static async Task Execute()
        {
            await Task.Delay(100);
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000) return;
            var count = MemoryReader.GetNumberOfUI();
            if (count == 7) return;
            if (count < 7 && OpenCV.TemplateMatch(Resources.Inventory) == false) await Keyboard.KeyPress(Keys.I, 200);
            if (count > 7 && OpenCV.TemplateMatch(Resources.MonsterManual)) await Keyboard.KeyPress(Keys.Escape, 200);
        }
    }
}