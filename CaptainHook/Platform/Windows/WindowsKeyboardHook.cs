using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using CaptainHook.OpenTK;


namespace CaptainHook.Platform.Windows
{
    public class WindowsKeyboardHook : KeyboardHookBase
    {

        public const int WH_KEYBOARD_LL = 13;
        private IntPtr _user32LibraryHandle;
        private IntPtr _windowsKeyboardHookHandle;
        private NativeApi.HookProc _keyboardHookProc;
        private bool _started;
        public override void StartHook()
        {
            if (_started)
                return;

            _windowsKeyboardHookHandle = IntPtr.Zero;
            _keyboardHookProc = LowLevelKeyboardProc; // we must keep alive proc, because GC is not aware about SetWindowsHookEx behaviour. If you remove this, the program will crash when the garbage collector reaps it.

            _user32LibraryHandle = IntPtr.Zero;

            _user32LibraryHandle = NativeApi.LoadLibrary("User32");
            if (_user32LibraryHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            _windowsKeyboardHookHandle = NativeApi.SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardHookProc, _user32LibraryHandle, 0);
            if (_windowsKeyboardHookHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

        }

        public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {

            bool fEatKeyStroke = false;

            var wparamTyped = wParam.ToInt32();
            if (Enum.IsDefined(typeof(WindowsKeyboardState), wparamTyped))
            {
                object o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
                
                LowLevelKeyboardInputEvent p = (LowLevelKeyboardInputEvent)o;

                var eventArguments = new KeyboardHookEventArgs(p, (WindowsKeyboardState)wparamTyped);

                var scancode = eventArguments.KeyboardData.HardwareScanCode;
                var vkey = (VirtualKeys)eventArguments.KeyboardData.VirtualCode;

                var flags = eventArguments.KeyboardData.Flags;
                var extended = ((flags) & ((int)WindowsKeyFlags.KF_EXTENDED >> 8)) > 0;

                var is_valid = true;
                Key key = WinKeyMap.TranslateKey(scancode, vkey, extended, false, out is_valid);

                if (is_valid)
                {

                    if (eventArguments.KeyboardState == WindowsKeyboardState.KeyDown || eventArguments.KeyboardState == WindowsKeyboardState.SysKeyDown)
                    {
                        OnKeyDown(key);
 
                        fEatKeyStroke = KeyDownArgs.Handled;
                    }
                    if (eventArguments.KeyboardState == WindowsKeyboardState.KeyUp || eventArguments.KeyboardState == WindowsKeyboardState.SysKeyUp)
                    {
                        OnKeyUp(key);

                        fEatKeyStroke = KeyUpArgs.Handled;
                    }
                }



            }

            return fEatKeyStroke ? (IntPtr)1 : NativeApi.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }



        public override void SendKeyDown(Key key)
        {

            KeyboardState.SetKeyState(key, true);
            var inputs = GenerateKeyDown(key);
            NativeApi.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
        public override void SendKeyUp(Key key)
        {
            
            KeyboardState.SetKeyState(key, false);
            var inputs = GenerateKeyUp(key);
            
            NativeApi.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private INPUT[] GenerateKeyDown(Key key)
        {
            int tscancode;
            VirtualKeys tvk;
            int tflags;
            var keyup = false;
            var altDown = KeyboardState.IsKeyDown(Key.AltLeft) || KeyboardState.IsKeyDown(Key.AltRight);
            bool extended;
            WinReverseKeyMap.ReverseTranslateKey(key, keyup, altDown, out tscancode, out tvk, out tflags, out extended);

           
            var dwFlags = 0x0008;
            if (extended)
                dwFlags = dwFlags | 0x0001;

           
            INPUT[] inputs;
            if (extended)
            {
                inputs = new[]
                {
                    new INPUT
                    {
                        type = NativeApi.INPUT_KEYBOARD,

                        u = new InputUnion
                        {
                            ki = new KEYBDINPUT()
                            {
                                wScan = (ushort) 0xe0,
                                wVk = (ushort) 0,
                                dwFlags = (ushort) dwFlags,
                                dwExtraInfo = NativeApi.GetMessageExtraInfo()
                            }
                        }
                    },
                    new INPUT
                    {
                        type = NativeApi.INPUT_KEYBOARD,

                        u = new InputUnion
                        {
                            ki = new KEYBDINPUT()
                            {
                                wScan = (ushort) tscancode,
                                wVk = (ushort) tvk,
                                dwFlags = (ushort) dwFlags,
                                dwExtraInfo = NativeApi.GetMessageExtraInfo()
                            }
                        }
                    }
                };
            }
            else
            {
                inputs = new[]
                {
                    new INPUT
                    {
                        type = NativeApi.INPUT_KEYBOARD,

                        u = new InputUnion
                        {
                            ki = new KEYBDINPUT()
                            {
                                wScan = (ushort) tscancode,
                                wVk = (ushort) tvk,
                                dwFlags = (ushort) dwFlags,
                                dwExtraInfo = NativeApi.GetMessageExtraInfo()
                            }
                        }
                    }
                };
            }

            return inputs;

            
        }
        public INPUT[] GenerateKeyUp(Key key)
        {
            int tscancode;
            VirtualKeys tvk;
            int tflags;
            var keyup = true;
            var altDown = KeyboardState.IsKeyDown(Key.AltLeft) || KeyboardState.IsKeyDown(Key.AltRight);
            bool extended;
            WinReverseKeyMap.ReverseTranslateKey(key, keyup, altDown, out tscancode, out tvk, out tflags, out extended);

            bool sysKey = (!altDown && key == Key.AltLeft) || (!altDown && key == Key.AltRight) || ((key != Key.AltLeft && key != Key.AltRight && altDown));

            var dwFlags = 0x0008 | 0x0002;
            if (extended)
                dwFlags = dwFlags | 0x0001;


            INPUT[] inputs;
            if (extended)
            {
                inputs = new[]
                {
                    new INPUT
                    {
                        type = NativeApi.INPUT_KEYBOARD,

                        u = new InputUnion
                        {
                            ki = new KEYBDINPUT()
                            {
                                wScan = (ushort) 0xe0,
                                wVk = (ushort) 0,
                                dwFlags = (ushort) dwFlags,
                                dwExtraInfo = NativeApi.GetMessageExtraInfo()
                            }
                        }
                    },
                    new INPUT
                    {
                        type = NativeApi.INPUT_KEYBOARD,

                        u = new InputUnion
                        {
                            ki = new KEYBDINPUT()
                            {
                                wScan = (ushort) tscancode,
                                wVk = (ushort) tvk,
                                dwFlags = (ushort) dwFlags,
                                dwExtraInfo = NativeApi.GetMessageExtraInfo()
                            }
                        }
                    }
                };
            }
            else
            {
                inputs = new[]
                {
                    new INPUT
                    {
                        type = NativeApi.INPUT_KEYBOARD,

                        u = new InputUnion
                        {
                            ki = new KEYBDINPUT()
                            {
                                wScan = (ushort) tscancode,
                                wVk = (ushort) tvk,
                                dwFlags = (ushort) dwFlags,
                                dwExtraInfo = NativeApi.GetMessageExtraInfo()
                            }
                        }
                    }
                };
            }

            return inputs;
        }

        public override void SendKeys(List<Key> keys)
        {
            List<INPUT> inputs=new List<INPUT>();
            foreach (var key in keys)
            {
                inputs.AddRange(GenerateKeyDown(key));
                inputs.AddRange(GenerateKeyUp(key));
            }
            NativeApi.SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
        }


        public override void StopHook()
        {
            if (!_started)
                return;

            if (_windowsKeyboardHookHandle == IntPtr.Zero) return;
            if (!NativeApi.UnhookWindowsHookEx(_windowsKeyboardHookHandle))
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }
            _windowsKeyboardHookHandle = IntPtr.Zero;

            // ReSharper disable once DelegateSubtraction
            _keyboardHookProc -= LowLevelKeyboardProc;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopHook();
            }
        }

        ~WindowsKeyboardHook()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
