using System.Diagnostics;

namespace MapleReaper.Models
{
    internal static class State
    {
        public static Process Process { get; set; }
        public static Size Size { get; set; }
        public static int X { get; set; }
        public static int Y { get; set; }
    }
}