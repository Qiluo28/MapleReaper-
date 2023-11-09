using Emgu.CV.Structure;
using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;

namespace MapleReaper.Plugins
{
    internal static class BanDetection
    {
        public static async Task Execute()
        {
            await Task.Delay(100);
            if (Settings.Default.IsBanDetectionEnabled == false) return;
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000) return;
            var height = MemoryReader.GetChatHeight();
            if (height < 2) return;
            using var bitmap = OpenCV.CaptureGameToBitmap(new Size(534, height * 13), new Point(1, State.Y + State.Size.Height - height * 13 - 71));
            using var binary = OpenCV.InRange(bitmap, new Rgb(102, 204, 255));
            if (OpenCV.TemplateMatch(Resources.PrefixBan, binary) == false) return;
            ReaperSetting.IsFreezing = true;
            while (ReaperSetting.IsFreezed == false) await Task.Delay(100);
            await Keyboard.Type("@fm");
        }
    }
}