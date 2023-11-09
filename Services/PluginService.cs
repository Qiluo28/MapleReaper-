using MapleReaper.Models;
using MapleReaper.Plugins;

namespace MapleReaper.Services
{
    internal static class PluginService
    {
        private static MapleReaperForm form;
        public static void Start(MapleReaperForm form)
        {
            PluginService.form = form;
            Task.Run(PluginLoop);
        }

        public static async Task PluginLoop()
        {
            while (true)
            {
                form.SetLabel("斷線檢測");
                await DisconnectDetection.Execute();
                form.SetLabel("開啟遊戲");
                await StartGame.Execute();
                form.SetLabel("兌幣");
                if (State.Process is null) continue;
                await MesoTransformation.Execute();
                form.SetLabel("BAN偵測");
                await BanDetection.Execute();
                form.SetLabel("尿尿與公告偵測");
                await PeeAndAnnounceDetection.Execute();
                form.SetLabel("GM偵測");
                await GmDetection.Execute();
                form.SetLabel("路人偵測");
                await PlayerDetection.Execute();
                form.SetLabel("遇人停止");
                await Freezer.Execute();
                form.SetLabel("放技能");
                await Buff.Execute();
                form.SetLabel("輪迴");
                await Rebirth.Execute();
                form.SetLabel("賣裝");
                await Selling.Execute();
                form.SetLabel("自動登入");
                await Login.Execute();
                form.SetLabel("地圖追蹤");
                await MapTracking.Execute();
                form.SetLabel("介面重整");
                await UIManager.Execute();
            }
        }
    }
}