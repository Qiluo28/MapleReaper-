using MapleReaper.Libraries;
using MapleReaper.Models;

namespace MapleReaper.Plugins
{
    internal static class Buff
    {
        public static async Task Execute()
        {
            await Task.Delay(100);
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000 || MemoryReader.GetNumberOfUI() > 7) return;
            var buffs = ReaperSetting.Buffs.Where(m => m.IsTimeout()).ToList();
            if (buffs.Count == 0) return;
            ReaperSetting.IsFreezing = true;
            while (ReaperSetting.IsFreezed == false) await Task.Delay(100);
            foreach (var buff in buffs)
            {
                await Keyboard.KeyPress(buff.Key.Value, 100);
                await Keyboard.KeyPress(buff.Key.Value, buff.PreDelay);
            }
            ReaperSetting.IsFreezing = false;
        }
    }
}
