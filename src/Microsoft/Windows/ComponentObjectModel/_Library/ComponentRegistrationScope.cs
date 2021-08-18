namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// The registration scope of a COM component and is backed up by a defined Registry Hive.
    /// </summary>
    public enum ComponentRegistrationScope {

        /// <summary>
        /// The scope of the component is currently unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The component has no scope defined (yet)
        /// </summary>
        None,

        /// <summary>
        /// System scoped components are managed underneath HKEY_LOCAL_MACHINE\Software\Classes
        /// </summary>
        System,

        /// <summary>
        /// User scoped components are managed underneath HKEY_CURRENT_USER\Software\Classes
        /// </summary>
        User,
    }
}
