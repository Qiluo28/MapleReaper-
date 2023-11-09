using Emgu.CV.Structure;
using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;
using MapleReaper.Services;

namespace MapleReaper.Plugins
{
    internal static class GmDetection
    {
        public static async Task Execute()
        {
            await Task.Delay(100);
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsFreezing || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000) return;
            var height = MemoryReader.GetChatHeight();
            if (height < 2) return;
            var bitmap = OpenCV.CaptureGameToBitmap(new Size(534, height * 13), new Point(1, State.Y + State.Size.Height - height * 13 - 71));
            var binary = OpenCV.InRange(bitmap, new Rgb(255, 255, 0));
            if (OpenCV.TemplateMatch(Resources.PrefixGm, binary) == false) return;
            RabbitMQService.SendAlert();
        }
    }
}