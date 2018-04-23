using System.ComponentModel;
using CaptainHook.OpenTK;

namespace CaptainHook.Platform.Windows
{
    public class KeyboardHookEventArgs : HandledEventArgs
    {
        public WindowsKeyboardState KeyboardState { get; private set; }
        public LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        public KeyboardHookEventArgs(
            LowLevelKeyboardInputEvent keyboardData,
            WindowsKeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}