using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CaptainHook.Platform.Windows;

namespace CaptainHook
{
    public static class Hook
    {
        private static IKeyboardHook _instance = null;
        private static readonly object _lockObj=new object();

        public static IKeyboardHook Instance
        {
            get
            {
                lock (_lockObj)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        _instance = new WindowsKeyboardHook();
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        throw new PlatformNotSupportedException("");
                        //_instance = new OsxKeyboardHook();
                    }
                    else
                    {
                        throw new PlatformNotSupportedException("");
                    }

                    return _instance;
                }
            }
           
        }
    }
}
