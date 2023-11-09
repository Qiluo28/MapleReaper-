using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Plugins;

namespace MapleReaper.Scripts
{
    public delegate void ReaperTask();

    public class ReaperScript
    {
        public ReaperScript()
        {
            ProcessManager.UpdateState();
            ReaperSetting.LoadSettings();
            Task.Run(ReaperLoop);
        }

        private async Task ReaperLoop()
        {
            await PreScript();
            while (ReaperSetting.IsScripting)
            {
                await CheckPause();
                await CheckFreeze();
                await MainScript();
            }
        }

        protected virtual async Task PreScript()
        {
            MesoTransformation.Initialize();
            Rebirth.Initialize();
            Selling.Initialize();
            MapTracking.Initialize();
            StartGame.Initialize();
            await Task.Delay(1);
        }

        protected virtual async Task MainScript()
        {
            await Task.Delay(1);
        }

        private async Task CheckPause()
        {
            while (ReaperSetting.IsPausing)
            {
                ReaperSetting.IsPaused = true;
                await Task.Delay(100);
            }
            ReaperSetting.IsPaused = false;
        }

        private async Task CheckFreeze()
        {
            while (ReaperSetting.IsFreezing)
            {
                ReaperSetting.IsFreezed = true;
                await Task.Delay(100);
            }
            ReaperSetting.IsFreezed = false;
        }

        protected async Task Move(int x)
        {
            if (x < MemoryReader.GetX())
            {
                await MoveLeft(x);
                return;
            }
            await MoveRight(x);
        }

        private async Task MoveLeft(int x)
        {
            Keyboard.ControlKeyDown(Keys.Left);
            while (ReaperSetting.IsScripting && !ReaperSetting.IsPausing && !ReaperSetting.IsFreezing && MemoryReader.GetX() > x) await Task.Delay(1);
            Keyboard.ControlKeyUp(Keys.Left);
        }

        private async Task MoveRight(int x)
        {
            Keyboard.ControlKeyDown(Keys.Right);
            while (ReaperSetting.IsScripting && !ReaperSetting.IsPausing && !ReaperSetting.IsFreezing && MemoryReader.GetX() < x) await Task.Delay(1);
            Keyboard.ControlKeyUp(Keys.Right);
        }
    }
}