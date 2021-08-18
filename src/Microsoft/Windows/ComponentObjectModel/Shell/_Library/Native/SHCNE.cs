using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell.Native {

    /// <summary>
    /// Shell Change Notification Events
    /// </summary>
    /// <remarks>
    /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_extended_event">SHChangeNotify function (shlobj_core.h) - Microsoft Docs</a>
    /// </remarks>
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Usage", "CA2217:Do not mark enums with FlagsAttribute", Justification = "IntelliSense false positive" )]
    public enum SHCNE : uint {

        /// <summary>
        /// The name of a nonfolder item has changed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the previous PIDL or name of the item.
        /// dwItem2 contains the new PIDL or name of the item.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_renameitem">Microsoft Docs</a>
        /// </remarks>
        RENAMEITEM = 0x00000001,

        /// <summary>
        /// A nonfolder item has been created.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the item that was created.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_create">Microsoft Docs</a>
        /// </remarks>
        CREATE = 0x00000002,

        /// <summary>
        /// A nonfolder item has been deleted.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the item that was deleted.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_delete">Microsoft Docs</a>
        /// </remarks>
        DELETE = 0x00000004,

        /// <summary>
        /// A folder has been created.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the folder that was created.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_mkdir">Microsoft Docs</a>
        /// </remarks>
        MKDIR = 0x00000008,

        /// <summary>
        /// A folder has been removed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the folder that was removed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_rmdir">Microsoft Docs</a>
        /// </remarks>
        RMDIR = 0x00000010,

        /// <summary>
        /// Storage media has been inserted into a drive.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the root of the drive that contains the new media.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_mediainserted">Microsoft Docs</a>
        /// </remarks>
        MEDIAINSERTED = 0x00000020,

        /// <summary>
        /// Storage media has been removed from a drive.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the root of the drive from which the media was removed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_mediaremoved">Microsoft Docs</a>
        /// </remarks>
        MEDIAREMOVED = 0x00000040,

        /// <summary>
        /// A drive has been removed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the root of the drive that was removed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_driveremoved">Microsoft Docs</a>
        /// </remarks>
        DRIVEREMOVED = 0x00000080,

        /// <summary>
        /// A drive has been added.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the root of the drive that was added.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_driveadd">Microsoft Docs</a>
        /// </remarks>
        DRIVEADD = 0x00000100,

        /// <summary>
        /// A folder on the local computer is being shared via the network.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the folder that is being shared.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_netshare">Microsoft Docs</a>
        /// </remarks>
        NETSHARE = 0x00000200,

        /// <summary>
        /// A folder on the local computer is no longer being shared via the network.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the folder that is no longer being shared.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_netunshare">Microsoft Docs</a>
        /// </remarks>
        NETUNSHARE = 0x00000400,

        /// <summary>
        /// The attributes of an item or folder have changed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the item or folder that has changed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_attributes">Microsoft Docs</a>
        /// </remarks>
        ATTRIBUTES = 0x00000800,

        /// <summary>
        /// The contents of an existing folder have changed, but the folder still exists and has not been renamed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the folder that has changed.
        /// dwItem2 is not used and should be NULL.
        /// If a folder has been created, deleted, or renamed, use <seealso cref="SHCNE.MKDIR"/>, <seealso cref="SHCNE.RMDIR"/>, or <seealso cref="SHCNE.RENAMEFOLDER"/>, respectively.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_updatedir">Microsoft Docs</a>
        /// </remarks>
        UPDATEDIR = 0x00001000,

        /// <summary>
        /// An existing item (a folder or a nonfolder) has changed, but the item still exists and has not been renamed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the item that has changed.
        /// dwItem2 is not used and should be NULL.
        /// If a nonfolder item has been created, deleted, or renamed, use <seealso cref="SHCNE.CREATE"/>, <seealso cref="SHCNE.DELETE"/>, or <seealso cref="SHCNE.RENAMEITEM"/>, respectively, instead.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_updateitem">Microsoft Docs</a>
        /// </remarks>
        UPDATEITEM = 0x00002000,

        /// <summary>
        /// The computer has disconnected from a server.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the server from which the computer was disconnected.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_serverdisconnect">Microsoft Docs</a>
        /// </remarks>
        SERVERDISCONNECT = 0x00004000,

        /// <summary>
        /// An image in the system image list has changed.
        /// <seealso cref="SHCNF.DWORD"/> must be specified in uFlags.
        /// dwItem2 contains the index in the system image list that has changed.
        /// dwItem1 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_updateimage">Microsoft Docs</a>
        /// </remarks>
        UPDATEIMAGE = 0x00008000,

        /// <summary>
        /// Windows XP and later: Not used.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_driveaddgui">Microsoft Docs</a>
        /// </remarks>
        DRIVEADDGUI = 0x00010000,

        /// <summary>
        /// The name of a folder has changed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the previous PIDL or name of the folder.
        /// dwItem2 contains the new PIDL or name of the folder.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_renamefolder">Microsoft Docs</a>
        /// </remarks>
        RENAMEFOLDER = 0x00020000,

        /// <summary>
        /// The amount of free space on a drive has changed.
        /// <seealso cref="SHCNF.IDLIST"/> or <seealso cref="SHCNF.PATH"/> must be specified in uFlags.
        /// dwItem1 contains the root of the drive on which the free space changed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_freespace">Microsoft Docs</a>
        /// </remarks>
        FREESPACE = 0x00040000,

        /// <summary>
        /// Not currently used.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_extended_event">Microsoft Docs</a>
        /// The extended event is identified in dwItem1, packed in LPITEMIDLIST format (same as SHCNF_DWORD packing).
        /// Additional information can be passed in the dwItem2 parameter of SHChangeNotify (called "pidl2" below), which if present, must also be in LPITEMIDLIST format.
        /// Unlike the standard events, the extended events are ORDINALs, so we don't run out of bits.
        /// Extended events follow the SHCNEE_* naming convention.
        /// The dwItem2 parameter varies according to the extended event.
        /// </remarks>
        EXTENDED_EVENT = 0x04000000,

        /// <summary>
        /// A file type association has changed.
        /// <seealso cref="SHCNF.IDLIST"/> must be specified in the uFlags parameter.
        /// dwItem1 and dwItem2 are not used and must be NULL.
        /// This event should also be sent for registered protocols.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_assocchanged">Microsoft Docs</a>
        /// </remarks>
        ASSOCCHANGED = 0x08000000,

        /// <summary>
        /// Specifies a combination of all of the disk event identifiers.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_diskevents">Microsoft Docs</a>
        /// </remarks>
        DISKEVENTS = 0x0002381F,

        /// <summary>
        /// Specifies a combination of all of the global event identifiers.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_globalevents">Microsoft Docs</a>
        /// </remarks>
        GLOBALEVENTS = 0x0C0581E0,

        /// <summary>
        /// All events have occurred.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_allevents">Microsoft Docs</a>
        /// </remarks>
        ALLEVENTS = 0x7FFFFFFF,

        /// <summary>
        /// The specified event occurred as a result of a system interrupt.
        /// As this value modifies other event values, it cannot be used alone.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-shchangenotify#shcne_interrupt">Microsoft Docs</a>
        /// </remarks>
        INTERRUPT = 0x80000000,
    }
}
