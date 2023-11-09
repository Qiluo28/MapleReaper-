namespace MapleReaper.Models
{
    internal class KeyBinding
    {
        private DateTime time = DateTime.MinValue;
        public Keys? Key { get; set; }
        public double PreDelay { get; set; }
        public double Delay { get; set; }

        public KeyBinding(string key, string preDelay, string delay)
        {
            Key = key switch
            {
                "A" => Keys.A,
                "B" => Keys.B,
                "C" => Keys.C,
                "D" => Keys.D,
                "E" => Keys.E,
                "F" => Keys.F,
                "G" => Keys.G,
                "H" => Keys.H,
                "I" => Keys.I,
                "J" => Keys.J,
                "K" => Keys.K,
                "L" => Keys.L,
                "M" => Keys.M,
                "N" => Keys.N,
                "O" => Keys.O,
                "P" => Keys.P,
                "Q" => Keys.Q,
                "R" => Keys.R,
                "S" => Keys.S,
                "T" => Keys.T,
                "U" => Keys.U,
                "V" => Keys.V,
                "W" => Keys.W,
                "X" => Keys.X,
                "Y" => Keys.Y,
                "Z" => Keys.Z,
                "Delete" => Keys.Delete,
                "End" => Keys.End,
                "Home" => Keys.Home,
                "Insert" => Keys.Insert,
                "Next" => Keys.PageDown,
                "PageUp" => Keys.PageUp,
                "F1" => Keys.F1,
                "F2" => Keys.F2,
                "F3" => Keys.F3,
                "F4" => Keys.F4,
                "F5" => Keys.F5,
                "F6" => Keys.F6,
                "F7" => Keys.F7,
                "F8" => Keys.F8,
                "F9" => Keys.F9,
                "F10" => Keys.F10,
                "F11" => Keys.F11,
                "F12" => Keys.F12,
                "0" => Keys.NumPad0,
                "1" => Keys.NumPad1,
                "2" => Keys.NumPad2,
                "3" => Keys.NumPad3,
                "4" => Keys.NumPad4,
                "5" => Keys.NumPad5,
                "6" => Keys.NumPad6,
                "7" => Keys.NumPad7,
                "8" => Keys.NumPad8,
                "9" => Keys.NumPad9,
                "D0" => Keys.D0,
                "D1" => Keys.D1,
                "D2" => Keys.D2,
                "D3" => Keys.D3,
                "D4" => Keys.D4,
                "D5" => Keys.D5,
                "D6" => Keys.D6,
                "D7" => Keys.D7,
                "D8" => Keys.D8,
                "D9" => Keys.D9,
                "OemMinus" => Keys.OemMinus,
                "Oemplus" => Keys.Oemplus,
                "Oemcomma" => Keys.Oemcomma,
                "OemPeriod" => Keys.OemPeriod,
                "OemQuestion" => Keys.OemQuestion,
                "OemOpenBrackets" => Keys.OemOpenBrackets,
                "OemCloseBrackets" => Keys.OemCloseBrackets,
                _ => null,
            };
            PreDelay = string.IsNullOrEmpty(preDelay) ? 0 : double.Parse(preDelay);
            Delay = string.IsNullOrEmpty(delay) ? 0 : double.Parse(delay);
        }

        public bool IsTimeout()
        {
            if ((DateTime.Now - time).TotalMilliseconds < Delay * 1000) return false;
            time = DateTime.Now;
            return true;
        }
    }
}