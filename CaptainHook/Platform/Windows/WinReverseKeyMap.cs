using CaptainHook.OpenTK;

namespace CaptainHook.Platform.Windows
{
    internal static class WinReverseKeyMap
    {
        public static int GetCode(Key key)
        {
            switch (key)
            {
                // 0 - 15
                case Key.Unknown: return 0;
                case Key.Escape: return 1;
                case Key.Number1: return 2;
                case Key.Number2: return 3;
                case Key.Number3: return 4;
                case Key.Number4: return 5;
                case Key.Number5: return 6;
                case Key.Number6: return 7;
                case Key.Number7: return 8;
                case Key.Number8: return 9;
                case Key.Number9: return 10;
                case Key.Number0: return 11;
                case Key.Minus: return 12;
                case Key.Plus: return 13;
                case Key.BackSpace: return 14;
                case Key.Tab: return 15;

                // 16-31
                case Key.Q: return 16;
                case Key.W: return 17;
                case Key.E: return 18;
                case Key.R: return 19;
                case Key.T: return 20;
                case Key.Y: return 21;
                case Key.U: return 22;
                case Key.I: return 23;
                case Key.O: return 24;
                case Key.P: return 25;
                case Key.BracketLeft: return 26;
                case Key.BracketRight: return 27;
                case Key.Enter: return 28;
                case Key.ControlLeft: return 29;
                case Key.A: return 30;
                case Key.S: return 31;

                // 32 - 47
                case Key.D: return 32;
                case Key.F: return 33;
                case Key.G: return 34;
                case Key.H: return 35;
                case Key.J: return 36;
                case Key.K: return 37;
                case Key.L: return 38;
                case Key.Semicolon: return 39;
                case Key.Quote: return 40;
                case Key.Grave: return 41;
                case Key.ShiftLeft: return 42;
                case Key.BackSlash: return 43;
                case Key.Z: return 44;
                case Key.X: return 45;
                case Key.C: return 46;
                case Key.V: return 47;

                // 48 - 63
                case Key.B: return 48;
                case Key.N: return 49;
                case Key.M: return 50;
                case Key.Comma: return 51;
                case Key.Period: return 52;
                case Key.Slash: return 53;
                case Key.ShiftRight: return 54;
                case Key.PrintScreen: return 55;
                case Key.AltLeft: return 56;
                case Key.AltRight: return 56;
                case Key.Space: return 57;
                case Key.CapsLock: return 58;
                case Key.F1: return 59;
                case Key.F2: return 60;
                case Key.F3: return 61;
                case Key.F4: return 62;
                case Key.F5: return 63;

                // 64 - 79
                case Key.F6: return 64;
                case Key.F7: return 65;
                case Key.F8: return 66;
                case Key.F9: return 67;
                case Key.F10: return 68;
                case Key.NumLock: return 69;
                case Key.ScrollLock: return 70;
                case Key.Home: return 71;
                case Key.Up: return 72;
                case Key.PageUp: return 73;
                case Key.KeypadMinus: return 74;
                case Key.Left: return 75;
                case Key.Keypad5: return 76;
                case Key.Right: return 77;
                case Key.KeypadPlus: return 78;
                case Key.End: return 79;

                // 80 - 95
                case Key.Down: return 80;
                case Key.PageDown: return 81;
                case Key.Insert: return 82;
                case Key.Delete: return 83;

                case Key.NonUSBackSlash: return 86;
                case Key.F11: return 87;
                case Key.F12: return 88;
                case Key.Pause: return 89;

                case Key.WinLeft: return 91;
                case Key.WinRight: return 92;
                case Key.Menu: return 93;

                case Key.F13: return 100;
                case Key.F14: return 101;
                case Key.F15: return 102;
                case Key.F16: return 103;
                case Key.F17: return 104;
                case Key.F18: return 105;
                case Key.F19: return 106;

                default: return 0;
            }
        }

        public static void ReverseTranslateKey(Key key, bool keyUp, bool isAltDown, out int tscancode, out VirtualKeys tvk, out int tflags, out bool extended)
        {

            extended = false;
            tflags = 0;
            tvk = VirtualKeys.UNKNOWN; // TODO: Implement

            if (keyUp)
                tflags = tflags | ((int)WindowsKeyFlags.KF_UP >> 8);
            if (isAltDown && key != Key.AltLeft && key != Key.AltRight)
                tflags = tflags | ((int)WindowsKeyFlags.KF_ALTDOWN >> 8);
            switch (key)
            {

                /*
                 * The extended-key flag indicates whether the keystroke message originated from one of the additional keys on the enhanced keyboard.
                 * The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters
                 * to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad. The extended-key flag is set if the key is an extended key.
                 */

                //these are keys that have to be given an extended flag
                case Key.Insert: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.Delete: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.Home: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.End: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.PageUp: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.PageDown: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.Left: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.Right: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.Up: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.Down: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;

                case Key.KeypadDivide: key = Key.Slash; extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.KeypadEnter: key = Key.Enter; extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.AltRight:
                    key = Key.AltLeft; extended = true;
                    tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8);
                    if (!keyUp)
                        tflags = tflags | ((int)WindowsKeyFlags.KF_ALTDOWN >> 8);
                    break;
                case Key.AltLeft:
                    key = Key.AltLeft; extended = false;
                    if (!keyUp)
                        tflags = tflags | ((int)WindowsKeyFlags.KF_ALTDOWN >> 8);
                    break;
                case Key.ControlRight: key = Key.ControlLeft; extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.ControlLeft: key = Key.ControlLeft; extended = false; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.ShiftRight:
                    key = Key.ShiftRight; extended = false; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;


                //TODO: Is this broken? Doesn't really matter here...
                //so these keys don't get marked as extended from llhook, but when i replay with sendinput I need to indicate that they are extended
                case Key.ShiftLeft:
                    extended = false;
                    break;


                case Key.LWin: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
                case Key.RWin: extended = true; tflags = tflags | ((int)WindowsKeyFlags.KF_EXTENDED >> 8); break;
            }

            if (!extended)
            {
                switch (key)
                {
                    case Key.Keypad0: key = Key.Insert; break;
                    case Key.Keypad1: key = Key.End; break;
                    case Key.Keypad2: key = Key.Down; break;
                    case Key.Keypad3: key = Key.PageDown; break;
                    case Key.Keypad4: key = Key.Left; break;
                    case Key.Keypad6: key = Key.Right; break;
                    case Key.Keypad7: key = Key.Home; break;
                    case Key.Keypad8: key = Key.Up; break;
                    case Key.Keypad9: key = Key.PageUp; break;
                    case Key.KeypadMultiply: key = Key.PrintScreen; break;
                    case Key.KeypadDecimal: key = Key.Delete; break;
                    case Key.Pause: tvk = VirtualKeys.PAUSE; key = Key.NumLock; break;
                }

            }

            //return our scan-code
            tscancode = GetCode(key);


        }
    }
}