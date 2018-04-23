using System;
using System.Collections.Generic;
using System.Text;
using CaptainHook.OpenTK;

namespace CaptainHook.Platform
{
    public abstract class KeyboardHookBase : IKeyboardHook
    {
        public event EventHandler<KeyboardKeyEventArgs> KeyDown = delegate { };
        public event EventHandler<KeyboardKeyEventArgs> KeyUp = delegate { };
        public abstract void StartHook();
        public abstract void StopHook();

        protected readonly KeyboardKeyEventArgs KeyDownArgs = new KeyboardKeyEventArgs();
        protected readonly KeyboardKeyEventArgs KeyUpArgs = new KeyboardKeyEventArgs();

        public abstract void SendKeyDown(Key key);
        public abstract void SendKeyUp(Key key);
        public abstract void SendKeys(List<Key> keys);
        protected KeyboardState KeyboardState = new KeyboardState();

        protected void OnKeyDown(Key key)
        {
            KeyboardState.SetKeyState(key, true);

            var e = KeyDownArgs;
            e.Keyboard = KeyboardState;
            e.Key = key;
            
            e.Handled = false;
            KeyDown(this, e);
        }


        protected void OnKeyUp(Key key)
        {
            KeyboardState.SetKeyState(key, false);

            var e = KeyUpArgs;
            e.Keyboard = KeyboardState;
            e.Key = key;
            
            e.Handled = false;        
            KeyUp(this, e);
        }


        public abstract void Dispose();
    }
}
