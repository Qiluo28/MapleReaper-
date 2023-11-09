using MapleReaper.Models;
using MapleReaper.Models.Structure;
using System.Runtime.InteropServices;
using System.Text;

namespace MapleReaper.Libraries
{
    internal static class Keyboard
    {
        public static void ControlKeyDown(Keys key)
        {
            var inputInformation = new InputInformation { InputEventType = 1 };
            inputInformation.InputUnion.KeyboardInputData.KeyboardEventFlag = 0x0001;
            inputInformation.InputUnion.KeyboardInputData.VirtualKey = (ushort)key;
            SendInput(1, ref inputInformation, Marshal.SizeOf(inputInformation));
        }

        public static async Task ControlKeyDown(Keys key, int delay)
        {
            ControlKeyDown(key);
            await Task.Delay(delay);
        }

        public static void ControlKeyUp(Keys key)
        {
            var inputInformation = new InputInformation { InputEventType = 1 };
            inputInformation.InputUnion.KeyboardInputData.KeyboardEventFlag = 0x0002 | 0x0001;
            inputInformation.InputUnion.KeyboardInputData.VirtualKey = (ushort)key;
            SendInput(1, ref inputInformation, Marshal.SizeOf(inputInformation));
        }

        public static async Task ControlKeyUp(Keys key, int delay)
        {
            ControlKeyUp(key);
            await Task.Delay(delay);
        }

        public static void ControlKeyPress(Keys key)
        {
            ControlKeyDown(key);
            ControlKeyUp(key);
        }

        public static async Task ControlKeyPress(Keys key, int delay)
        {
            ControlKeyDown(key);
            await ControlKeyUp(key, delay);
        }

        public static void KeyDown(Keys key)
        {
            var longParameter = (MapVirtualKey((ushort)key, 0) << 16) + 1;
            PostMessage(State.Process.MainWindowHandle, 0x100, (int)key, longParameter);
        }

        public static async Task KeyDown(Keys key, int delay)
        {
            KeyDown(key);
            await Task.Delay(delay);
        }

        public static void KeyUp(Keys key)
        {
            var longParameter = (MapVirtualKey((ushort)key, 0) << 16) + 1;
            PostMessage(State.Process.MainWindowHandle, 0x101, (int)key, longParameter);
        }

        public static async Task KeyUp(Keys key, int delay)
        {
            KeyUp(key);
            await Task.Delay(delay);
        }

        public static void KeyPress(Keys key)
        {
            KeyDown(key);
            KeyUp(key);
        }

        public static async Task KeyPress(Keys key, int delay)
        {
            KeyDown(key);
            await KeyUp(key, delay);
        }

        public static async Task KeyPress(Keys key, double delay)
        {
            KeyDown(key);
            await KeyUp(key, (int)delay);
        }

        public static async Task CopyPaste(string text)
        {
            var source = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                try
                {
                    Clipboard.SetDataObject(text, true, 5, 200);
                    source.SetResult(null);
                }
                catch (Exception e)
                {
                    source.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            await ControlKeyDown(Keys.ControlKey, 200);
            await KeyPress(Keys.V, 100);
            await ControlKeyUp(Keys.ControlKey, 100);
            return;
        }

        public static async Task<bool> Type(params string[] messages)
        {
            foreach (var message in messages)
            {
                while (MemoryReader.GetChat() != GetLength(message))
                {
                    if (MemoryReader.GetNumberOfUI() > 7) return false;
                    if (MemoryReader.GetChat() == -1) await KeyPress(Keys.Enter, 200);
                    if (MemoryReader.GetChat() != 0) await Clear();
                    await CopyPaste(message);
                }
                while (MemoryReader.GetChat() != -1)
                {
                    if (MemoryReader.GetNumberOfUI() > 7) return false;
                    await KeyPress(Keys.Enter, 200);
                }
            }
            return true;
        }

        public static async Task Clear()
        {
            await KeyPress(Keys.End, 200);
            await ControlKeyDown(Keys.ShiftKey, 200);
            await KeyPress(Keys.Home, 200);
            await ControlKeyUp(Keys.ShiftKey, 200);
            await KeyPress(Keys.Back, 200);
        }

        public static int GetLength(string message)
        {
            if (message.Length == 0) return 0;
            var ascii = new ASCIIEncoding();
            var length = 0; 
            var bytes = ascii.GetBytes(message);
            for (var i = 0; i < bytes.Length; i++)
            {
                if ((int)bytes[i] == 63) length += 2;
                else length += 1;
            }
            return length;
        }

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        private static extern long PostMessage(IntPtr handle, ushort message, int wordParameter, long longParameter);
        [DllImport("user32.dll", EntryPoint = "MapVirtualKey")]
        private static extern long MapVirtualKey(long virtualKeyCode, long mapType);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint inputNumber, ref InputInformation inputInformation, int inputSize);
    }
}