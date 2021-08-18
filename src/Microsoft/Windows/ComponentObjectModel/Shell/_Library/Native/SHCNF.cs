namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell.Native {

    /// <summary>
    /// Shell Change Notification Flags
    /// </summary>
    /// <remarks>
    /// uFlags &amp; SHCNF.TYPE is an ID which indicates what dwItem1 and dwItem2 mean
    /// </remarks>
    //[Flags]
    public enum SHCNF : uint {

        /// <summary>
        ///dwItem1 and dwItem2 are the addresses of ITEMIDLIST structures that represent the item(s) affected by the change. Each ITEMIDLIST must be relative to the desktop folder.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcnf_idlist">Microsoft Docs</a>
        /// </remarks>
        IDLIST = 0x0000,  // LPITEMIDLIST

        //PATHA               = 0x0001,  // path name (ANSI) - Not defined. We do not support ANSI versions anymore
        //PRINTERA            = 0x0002,  // printer friendly name (ANSI) - Not defined. We do not support ANSI versions anymore

        /// <summary>
        /// The dwItem1 and dwItem2 parameters are DWORD values.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcnf_dword">Microsoft Docs</a>
        /// </remarks>
        DWORD = 0x0003,

        /// <summary>
        /// dwItem1 and dwItem2 are the addresses of null-terminated strings of maximum length MAX_PATH that contain the full path names of the items affected by the change.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcnf_path">Microsoft Docs</a>
        /// </remarks>
        PATH = 0x0005,

        /// <summary>
        /// dwItem1 and dwItem2 are the addresses of null-terminated strings that represent the friendly names of the printer(s) affected by the change.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcnf_printer">Microsoft Docs</a>
        /// </remarks>
        PRINTER = 0x0006,

        /// <summary>
        /// Flags that, when combined bitwise with TYPE, indicate the meaning of the dwItem1 and dwItem2 parameters.
        /// </summary>
        TYPE = 0x00FF,

        /// <summary>
        /// The function should not return until the notification has been delivered to all affected components. As this flag modifies other data-type flags, it cannot be used by itself.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcnf_flush">Microsoft Docs</a>
        /// </remarks>
        FLUSH = 0x1000,

        /// <summary>
        /// The function should begin delivering notifications to all affected components but should return as soon as the notification process has begun. As this flag modifies other data-type flags, it cannot by used by itself. This flag includes FLUSH.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcnf_flushnowait">Microsoft Docs</a>
        /// </remarks>
        FLUSHNOWAIT = 0x3000,

        /// <summary>
        /// Notify clients registered for all children.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcnf_notifyrecursive">Microsoft Docs</a>
        /// </remarks>
        NOTIFYRECURSIVE = 0x10000,

    }
}
