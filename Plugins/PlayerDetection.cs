using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;
using MapleReaper.Services;

namespace MapleReaper.Plugins
{
    internal static class PlayerDetection
    {
        private static DateTime cooldown = DateTime.MinValue;

        public static async Task Execute()
        {
            await Task.Delay(50);
            if (ReaperSetting.IsScripting == false || ReaperSetting.IsPausing) return;
            if (MemoryReader.GetLoginState() == 0 || MemoryReader.GetMapId() == 910000000) return;
            if (MemoryReader.GetPlayerCount() == 0) return;
            RabbitMQService.SendWarning();
            if (DateTime.Now - cooldown < TimeSpan.FromSeconds(5)) return;
            var height = MemoryReader.GetChatHeight();
            if (height < 2) return;
            var bitmap = OpenCV.CaptureGameToBitmap(State.Size);
            //var bitmap = OpenCV.CaptureGameToBitmap(new Size(535, height * 13), new Point(1, State.Y + State.Size.Height - height * 13 - 71));
            var filename = $"{Settings.Default.Id}-{DateTime.Now:MMddHHmmss}.png";
            bitmap.Save(filename);
            cooldown = DateTime.Now;
        }
    }
}
