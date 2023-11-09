using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;

namespace MapleReaper.Plugins
{
    internal static class MesoTransformation
    {
        private static DateTime cooldown;

        public static void Initialize()
        {
            cooldown = DateTime.Now;
        }

        public static async Task Execute()
        {
            await Task.Delay(100);
            if (Settings.Default.IsMesoTransformationEnabled == false) return;
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000 || MemoryReader.GetNumberOfUI() > 7) return;
            if ((DateTime.Now - cooldown).TotalMinutes < double.Parse(Settings.Default.MesoTransformationDelay)) return;
            ReaperSetting.IsFreezing = true;
            while (ReaperSetting.IsFreezed == false) await Task.Delay(100);
            Point location;
            while (OpenCV.TemplateMatch(Resources.Inventory, out location) == false) await Keyboard.KeyPress(Keys.I, 200);
            await Mouse.Move(location.X + 145, location.Y + 30, 200);
            await Mouse.LeftClick(200);
            if (MemoryReader.GetNumberOfUI() > 7) return;
            while (OpenCV.TemplateMatch(Resources.Shopkeeper) == false)
            {
                if (OpenCV.TemplateMatch(Resources.ConfirmButton, out location))
                {
                    await Mouse.Move(location.X + 30, location.Y + 15, 100);
                    await Mouse.LeftClick(100);
                }
                OpenCV.TemplateMatch(Resources.Mumu, out location);
                await Mouse.Move(location.X + 1, location.Y + 1, 100);
                await Mouse.Move(location.X + 2, location.Y + 2, 100);
                await Mouse.Move(location.X + 3, location.Y + 3, 100);
                await Mouse.Move(location.X + 4, location.Y + 4, 100);
                await Mouse.LeftClick(200);
                await Mouse.LeftClick(200);
            }
            await Mouse.Move(385, 310, 200);
            Mouse.LeftDown();
            await Task.Delay(200);
            await Mouse.Move(385, 480, 200);
            Mouse.LeftUp();
            await Task.Delay(200);
            await Mouse.Move(195, 460, 200);
            while (OpenCV.TemplateMatch(Resources.CancelButton) == false) await Mouse.LeftClick(100);
            await Keyboard.CopyPaste("5");
            await Keyboard.KeyPress(Keys.Enter, 200);
            await Mouse.Move(775, 75, 200);
            while (OpenCV.TemplateMatch(Resources.Shopkeeper)) await Keyboard.KeyPress(Keys.Escape, 200);
            cooldown = DateTime.Now;
            ReaperSetting.IsFreezing = false;
        }
    }
}
