using System;
using System.Collections.Generic;
using System.Text;
using CaptainHook.OpenTK;

namespace CaptainHook
{
    public interface IKeyboardHook : IDisposable
    {
        event EventHandler<KeyboardKeyEventArgs> KeyDown;
        event EventHandler<KeyboardKeyEventArgs> KeyUp;

        void SendKeys(List<Key> keys);
        void SendKeyDown(Key key);
        void SendKeyUp(Key key);

        void StartHook();
        void StopHook();

    }
}
