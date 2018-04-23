using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CaptainHook;
using CaptainHook.OpenTK;
using CaptainHook.Platform;

namespace BadKeyboard
{
    class Program
    {
        static void Main(string[] args)
        {
            var keyWatcher = new KeyboardWatcher();


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                CaptainHook.Platform.Windows.Msg msg;
                while (CaptainHook.Platform.Windows.NativeApi.GetMessage(out msg, IntPtr.Zero, 0, 0) > 0)
                {
                    CaptainHook.Platform.Windows.NativeApi.TranslateMessage(ref msg);
                    CaptainHook.Platform.Windows.NativeApi.DispatchMessage(ref msg);
                }
            }
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    //Cocoa run loop



            //    var runLoop = CF.CFRunLoopGetMain();
            //    if (runLoop == IntPtr.Zero)
            //    {
            //        runLoop = CF.CFRunLoopGetCurrent();
            //    }
            //    if (runLoop == IntPtr.Zero)
            //    {

            //        throw new InvalidOperationException();
            //    }

            //    //TODO: this is functional but wrong. no exit condition? will need to re-engineer this to support starting and stopping

            //    while (true)
            //    {
            //        CF.CFRunLoopRunInMode(CF.RunLoopModeDefault, 0, false);
            //    }
            //}
        }
    }

    public class KeyboardWatcher
    {
        private IKeyboardHook _hook;
        private bool _soTriggeredRightNow;

        

        public KeyboardWatcher()
        {
            _hook = Hook.Instance;

            _hook.KeyUp += _hook_KeyUp;
            _hook.KeyDown += _hook_KeyDown;
            _hook.StartHook();
            
        }

        StringBuilder _sb=new StringBuilder();
        DateTime _lastKeyEvent=new DateTime();
        private void _hook_KeyDown(object sender, CaptainHook.OpenTK.KeyboardKeyEventArgs e)
        {
            //i'm just going to watch the keyup event
        }

        
        private void _hook_KeyUp(object sender, CaptainHook.OpenTK.KeyboardKeyEventArgs e)
        {

            if (e.Key == Key.Space)
            {
                _sb.Clear();
                return;
                
            }
            
            if (e.Key == Key.BackSpace )
            {
                if(_sb.Length > 0)
                    _sb.Remove(_sb.Length - 1, 1);
                
                return;
            }

            if((int)e.Key<83 || (int)e.Key > 108)
                return;

            var timeSinceLastKey = DateTime.Now - _lastKeyEvent;
            _lastKeyEvent = DateTime.Now;

            if (timeSinceLastKey.TotalSeconds > 1)
            {
                _sb.Clear();              
            }
            //we are continously typing
            _sb.Append(Enum.GetName(typeof(Key), e.Key));
            if (_sb.ToString().Equals("TRUMP"))
            {
                var keys = new List<Key>();
                for (int i = 0; i < _sb.Length; i++)
                {
                    keys.Add(Key.BackSpace);
                }
                keys.AddRange(new[] { Key.D, Key.U, Key.M, Key.B, Key.A, Key.S, Key.S });
                //we could generalize this and load from a dictionary.
                _hook.SendKeys(keys);
            }
            if (_sb.ToString().Equals("JAVASCRIPT"))
            {
                var keys = new List<Key>();
                for (int i = 0; i < _sb.Length; i++)
                {
                    keys.Add(Key.BackSpace);
                }
                keys.AddRange(new []{Key.G, Key.A, Key.R, Key.B, Key.A, Key.G, Key.E});
                //we could generalize this and load from a dictionary.
                _hook.SendKeys(keys);
            }

            if (_sb.ToString().Equals("NODEJS"))
            {
                var keys = new List<Key>();
                for (int i = 0; i < _sb.Length; i++)
                {
                    keys.Add(Key.BackSpace);
                }
                keys.AddRange(new[] { Key.C, Key.G, Key.I });
                //we could generalize this and load from a dictionary.
                _hook.SendKeys(keys);
            }

        }
    }
}
