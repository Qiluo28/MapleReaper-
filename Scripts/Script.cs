using MapleReaper.Libraries;
using MapleReaper.Models;

namespace MapleReaper.Scripts
{
    internal class Script : ReaperScript
    {
        private bool anchor;
        private int attackMaxCount;
        private int attackCount;
        private int anchorLeft;
        private int anchorRight;

        public Script() : base() { }

        protected override async Task PreScript()
        {
            await base.PreScript();
            anchor = false;
            attackMaxCount = 50;
            attackCount = 0;
            anchorLeft = MemoryReader.GetX();
            anchorRight = anchorLeft + 40;
        }

        protected override async Task MainScript()
        {
            if (attackCount == attackMaxCount)
            {
                anchor = !anchor;
                await Move(anchor ? anchorLeft : anchorRight);
                attackCount = 0;
            }
            await Keyboard.KeyPress(ReaperSetting.Attack.Key.Value, ReaperSetting.Attack.Delay);
            attackCount++;
        }
    }
}