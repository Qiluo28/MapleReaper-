using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MapleReaper.Libraries
{
    internal static class KeyboardHook
    {
        private delegate IntPtr KeyboardHookHandler(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate Task KeyboardHookCallback(Keys key);
        private static KeyboardHookHandler hookHandler;
        public static event KeyboardHookCallback KeyDown;
        public static event KeyboardHookCallback KeyUp;
        private static IntPtr hookID = IntPtr.Zero;

        public static void KeyboardHookInit()
        {
            hookHandler = HookFunc;
            hookID = SetHook(hookHandler);
        }

        public static void KeyboardHookOff()
        {
            UnhookWindowsHookEx(hookID);
        }

        public static void AddCallBack(KeyboardHookCallback callback)
        {
            KeyUp += callback;
        }

        private static IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int iwParam = wParam.ToInt32();
                if (KeyDown is not null && (iwParam == 0x100 || iwParam == 0x104)) KeyDown((Keys)Marshal.ReadInt32(lParam));
                if (KeyUp is not null && (iwParam == 0x101 || iwParam == 0x105)) KeyUp((Keys)Marshal.ReadInt32(lParam)).Wait();
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(KeyboardHookHandler proc)
        {
            using var module = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            return SetWindowsHookEx(13, proc, GetModuleHandle(module.ModuleName), 0);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookHandler lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}