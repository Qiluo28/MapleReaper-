using MapleReaper.Models;
using System.Runtime.InteropServices;

namespace MapleReaper.Libraries
{
    internal static class MemoryReader
    {
        private static IntPtr handle;
        private static IntPtr chatAddress;
        private static IntPtr chatHeightAddress;
        private static IntPtr positionAddress;
        private static IntPtr mapIdAddress;
        //private static IntPtr playerNameAddress;
        private static IntPtr playerCountAddress;
        private static IntPtr monsterCountAddress;

        public static void TrackAddress()
        {
            handle = OpenProcess(0x0010 | 0x0020 | 0x0008, false, State.Process.Id);
            chatAddress = ReadAddress(handle, State.Process.MainModule.BaseAddress, 8329548);
            chatHeightAddress = ReadAddress(handle, State.Process.MainModule.BaseAddress, 8329544);
            positionAddress = ReadAddress(handle, State.Process.MainModule.BaseAddress, 8328936);
            mapIdAddress = ReadAddress(handle, State.Process.MainModule.BaseAddress, 8335052);
            //playerNameAddress = ReadAddress(handle, State.Process.MainModule.BaseAddress, 8359492);
            playerCountAddress = ReadAddress(handle, State.Process.MainModule.BaseAddress, 8328952);
            monsterCountAddress = ReadAddress(handle, State.Process.MainModule.BaseAddress, 8328948);
        }

        public static int GetLoginState()
        {
            return ReadInteger(handle, State.Process.MainModule.BaseAddress, 8328948);
        }

        public static int GetChannel()
        {
            return ReadInteger(handle, State.Process.MainModule.BaseAddress, 8359436);
        }

        public static int GetNumberOfUI()
        {
            return ReadInteger(handle, State.Process.MainModule.BaseAddress, 8349736);
        }

        public static int GetX()
        {
            return ReadInteger(handle, positionAddress, 3776);
        }

        public static int GetY()
        {
            return ReadInteger(handle, positionAddress, 3780);
        }

        public static int GetFacing()
        {
            return ReadInteger(handle, positionAddress, 3796);
        }

        public static int GetChat()
        {
            var pointer = ReadAddress(handle, chatAddress, 128);
            return ReadInteger(handle, pointer, 60);
        }

        public static int GetMonsterCount()
        {
            return ReadInteger(handle, monsterCountAddress, 10);
        }

        public static int GetPlayerCount()
        {
            return ReadInteger(handle, playerCountAddress, 24);
        }

        public static int GetMapId()
        {
            return ReadInteger(handle, mapIdAddress, 1176);
        }

        public static int GetChatHeight()
        {
            return ReadInteger(handle, chatHeightAddress, 2404);
        }

        public static bool IsTypingAccount()
        {
            return ReadInteger(handle, State.Process.MainModule.BaseAddress, 8335708) != 0;
        }

        private static IntPtr ReadAddress(IntPtr handle, IntPtr address, int offset)
        {
            var bytes = new byte[4];
            ReadProcessMemory(handle, address + offset, bytes, 4, out _);
            return new IntPtr(BitConverter.ToInt32(bytes, 0));
        }

        private static int ReadInteger(IntPtr handle, IntPtr address, int offset)
        {
            var bytes = new byte[4];
            ReadProcessMemory(handle, address + offset, bytes, 4, out _);
            return BitConverter.ToInt32(bytes, 0);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int desiredAccess, bool isInherit, int processId);
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr handle, IntPtr address, byte[] buffer, int size, out int bytesRead);
    }
}