using System;
using System.Runtime.InteropServices;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell.Native {

    // Windows Native

    static class NativeMethods {

        /// <summary>
        /// Notifies the system of an event that an application has performed. An application should use this function if it performs an action that may affect the Shell.
        /// </summary>
        /// <param name="wEventId">Describes the event that has occurred. Typically, only one event is specified at a time. If more than one event is specified, the values contained in the dwItem1 and dwItem2 parameters must be the same, respectively, for all specified events. This parameter can be one or more of the following values:</param>
        /// <param name="uFlags">Flags that, when combined bitwise with SHCNF_TYPE, indicate the meaning of the dwItem1 and dwItem2 parameters. The uFlags parameter must be one of the following values.</param>
        /// <param name="dwItem1">Optional. First event-dependent value.</param>
        /// <param name="dwItem2">Optional. Second event-dependent value.</param>
        /// <remarks>
        /// * [SHChangeNotify function - Microsoft Docs](https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify)
        /// * [SHChangeNotify (shell32) - pinvoke.net](http://pinvoke.net/default.aspx/shell32/SHChangeNotify.html)
        /// </remarks>
        [DllImport( "shell32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        public static extern void SHChangeNotify( uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2 );

        [DllImport( "shell32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        public static extern void SHChangeNotify( SHCNE wEventId, SHCNF uFlags, IntPtr dwItem1, IntPtr dwItem2 );
    }
}
