using MapleReaper.Models;
using MapleReaper.Models.Structure;
using System.Runtime.InteropServices;

namespace MapleReaper.Libraries
{
    internal static class Mouse
    {
        public static void Move(int x, int y)
        {
            SetCursorPos(State.X + x, State.Y + y);
        }

        public static async Task Move(int x, int y, int delay)
        {
            SetCursorPos(State.X + x, State.Y + y);
            await Task.Delay(delay);
        }

        public static void Move(Point location)
        {
            SetCursorPos(State.X + location.X, State.Y + location.Y);
        }

        public static async Task Move(Point location, int delay)
        {
            SetCursorPos(State.X + location.X, State.Y + location.Y);
            await Task.Delay(delay);
        }

        public static void LeftClick()
        {
            LeftDown();
            LeftUp();
        }

        public static async Task LeftClick(int delay)
        {
            LeftDown();
            LeftUp();
            await Task.Delay(delay);
        }

        public static void LeftDown()
        {
            var inputInformation = new InputInformation
            {
                InputEventType = 0
            };
            inputInformation.InputUnion.MouseInputData.MouseEventFlag = 0x0002;
            SendInput(1, ref inputInformation, Marshal.SizeOf(inputInformation));
        }

        public static void LeftUp()
        {
            var inputInformation = new InputInformation
            {
                InputEventType = 0
            };
            inputInformation.InputUnion.MouseInputData.MouseEventFlag = 0x0004;
            SendInput(1, ref inputInformation, Marshal.SizeOf(inputInformation));
        }

        public static void RightClick()
        {
            RightDown();
            RightUp();
        }

        public static async Task RightClick(int delay)
        {
            RightDown();
            RightUp();
            await Task.Delay(delay);
        }

        public static void RightDown()
        {
            var inputInformation = new InputInformation
            {
                InputEventType = 0
            };
            inputInformation.InputUnion.MouseInputData.MouseEventFlag = 0x0008;
            SendInput(1, ref inputInformation, Marshal.SizeOf(inputInformation));
        }

        public static void RightUp()
        {
            var inputInformation = new InputInformation
            {
                InputEventType = 0
            };
            inputInformation.InputUnion.MouseInputData.MouseEventFlag = 0x0010;
            SendInput(1, ref inputInformation, Marshal.SizeOf(inputInformation));
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint inputNumber, ref InputInformation inputInformation, int inputSize);
    }
}
