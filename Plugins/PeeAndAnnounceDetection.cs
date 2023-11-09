using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;

namespace MapleReaper.Plugins
{
    internal static class PeeAndAnnounceDetection
    {
        public static async Task Execute()
        {
            await Task.Delay(100);
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000 || MemoryReader.GetNumberOfUI() < 8) return;
            if (OpenCV.TemplateMatch(Resources.ConfirmButton, out var location) == false) return;
            if (OpenCV.TemplateMatch(Resources.CancelButton)) return;
            ReaperSetting.IsFreezing = true;
            var isPee = OpenCV.TemplateMatch(Resources.Pee);
            do
            {
                await Mouse.Move(location.X + 30, location.Y + 15, 100);
                await Mouse.LeftClick(100);
            } while (OpenCV.TemplateMatch(Resources.ConfirmButton, out location));
            while (MemoryReader.GetChat() != -1) await Keyboard.KeyPress(Keys.Enter, 200);
            if (isPee) await Keyboard.Type($"@尿尿 {DateTime.Now.Hour:00}{DateTime.Now.Minute:00}");
            ReaperSetting.IsFreezing = false;
        }
    }
}