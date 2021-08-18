namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell.Native {

    /// <summary>
    /// Shell Change Notification Extended Events.
    /// </summary>
    /// <remarks>
    /// These events are ordinals.
    /// According to Microsoft documentation, the Extended Events are not used.
    /// Documentation was taken from the Windows SDK header files (ShlObj_core.h)
    /// </remarks>
    public enum SHCNEE
        : long {

        /// <summary>
        /// pidl2 is the changed folder.
        /// </summary>
        ORDERCHANGED = 2L,

        /// <summary>
        /// pidl2 is a SHChangeProductKeyAsIDList
        /// </summary>
        MSI_CHANGE = 4L,

        /// <summary>
        /// pidl2 is a SHChangeProductKeyAsIDList
        /// </summary>
        MSI_UNINSTALL = 5L,
    }
}
