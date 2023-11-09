using MapleReaper.Properties;

namespace MapleReaper.Models
{
    internal static class ReaperSetting
    {
        public static bool IsScripting { get; set; }
        public static bool IsPausing { get; set; }
        public static bool IsPaused { get; set; }
        public static bool IsFreezing { get; set; }
        public static bool IsFreezed { get; set; }
        public static KeyBinding Attack { get; set; }
        public static List<KeyBinding> Buffs { get; set; }

        public static void LoadSettings()
        {
            IsScripting = true;
            IsPausing = false;
            IsPaused = false;
            IsFreezing = false;
            IsFreezed = false;
            Attack = new KeyBinding(Settings.Default.AttackKey, string.Empty, Settings.Default.AttackDelay);
            Buffs = new List<KeyBinding>();
            if (string.IsNullOrEmpty(Settings.Default.BuffKeys)) return;
            var buffKeys = Settings.Default.BuffKeys.Split(',');
            var buffPreDelays = Settings.Default.BuffPreDelays.Split(',');
            var buffDelays = Settings.Default.BuffDelays.Split(',');
            for (var i = 0; i < buffKeys.Length; i++)
            {
                Buffs.Add(new KeyBinding(buffKeys[i], buffPreDelays[i], buffDelays[i]));
            }
        }
    }
}