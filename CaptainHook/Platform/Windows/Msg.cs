using System;
using System.Collections.Generic;


namespace CaptainHook.Platform.Windows
{
    public struct Msg
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point pt;
    }
}
