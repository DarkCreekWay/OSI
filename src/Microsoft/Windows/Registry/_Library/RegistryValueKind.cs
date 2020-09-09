namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Specifies the data types to use when storing values in the registry, or identifies the data type of a value in the registry.
    /// </summary>
    /// <remarks>
    /// Some Value Kinds of the native Microsoft Windows Registry are not supported through .NET.
    /// We want to guarantee as much compatibility with the underlaying .NET implementation of Registry access, and so do not implement the missing value kinds either.
    /// Microsoft documentation and implementation details can be found here
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind">Microsoft Docs</a>
    /// <a href="https://source.dot.net/#Microsoft.Win32.Registry/Microsoft/Win32/RegistryValueKind.cs">Source fpr .net core</a>
    /// <a href="https://source.dot.net/#Microsoft.Win32.Registry/Interop.RegistryConstants.cs">Source for .net core</a>
    /// </remarks>
    public enum RegistryValueKind {

        /// <summary>
        /// A null-terminated string. This value is equivalent to the Windows API registry data type REG_SZ.
        /// </summary>
        String = 1,

        /// <summary>
        /// A null-terminated string that contains unexpanded references to environment variables, such as %PATH%, that are expanded when the value is retrieved. This value is equivalent to the Windows API registry data type REG_EXPAND_SZ.
        /// </summary>
        ExpandString = 2,

        /// <summary>
        /// Binary data in any form. This value is equivalent to the Windows API registry data type REG_BINARY.
        /// </summary>
        Binary = 3,

        /// <summary>
        /// A 32-bit binary number. This value is equivalent to the Windows API registry data type REG_DWORD.
        /// </summary>
        DWord = 4,
        /// <summary>
        /// An array of null-terminated strings, terminated by two null characters. This value is equivalent to the Windows API registry data type REG_MULTI_SZ.
        /// </summary>
        ///
        MultiString = 7,

        /// <summary>
        /// A 64-bit binary number. This value is equivalent to the Windows API registry data type REG_QWORD.
        /// </summary>
        QWord = 11,
        /// <summary>
        /// An unsupported registry data type. For example, the Microsoft Windows API registry data type REG_RESOURCE_LIST is unsupported. Use this value to specify that the SetValue(String, Object) method should determine the appropriate registry data type when storing a name/value pair.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No data type.
        /// </summary>
        None = -1,
    }
}
