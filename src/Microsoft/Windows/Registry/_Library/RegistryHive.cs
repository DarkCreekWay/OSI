namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Represents the possible values for a top-level registry node.
    /// </summary>
    /// <remarks>
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryhive">RegistryHive - Microsoft Docs</a>
    /// <a href="https://source.dot.net/#Microsoft.Win32.Registry/Microsoft/Win32/RegistryHive.cs">RegistryHive.cs - .NET Core Source</a>
    /// </remarks>
    public enum RegistryHive {

        /// <summary>
        /// Represents the HKEY_CLASSES_ROOT base key.
        /// </summary>
        ClassesRoot = unchecked((int)0x80000000),

        /// <summary>
        /// Represents the HKEY_CURRENT_USER base key.
        /// </summary>
        CurrentUser = unchecked((int)0x80000001),

        /// <summary>
        /// Represents the HKEY_LOCAL_MACHINE base key.
        /// </summary>
        LocalMachine = unchecked((int)0x80000002),

        /// <summary>
        /// Represents the HKEY_USERS base key.
        /// </summary>
        Users = unchecked((int)0x80000003),

        /// <summary>
        /// Represents the HKEY_PERFORMANCE_DATA base key.
        /// </summary>
        PerformanceData = unchecked((int)0x80000004),

        /// <summary>
        /// Represents the HKEY_CURRENT_CONFIG base key.
        /// </summary>
        CurrentConfig = unchecked((int)0x80000005),
    }
}

