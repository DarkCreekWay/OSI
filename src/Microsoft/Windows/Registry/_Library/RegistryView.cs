namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Specifies which registry view to target on a 64-bit operating system.
    /// </summary>
    /// <remarks>
    /// On the 64-bit version of Windows, portions of the registry are stored separately for 32-bit and 64-bit applications.
    /// There is a 32-bit view for 32-bit applications and a 64-bit view for 64-bit applications.
    /// You can specify a registry view when you use the OpenBaseKey and OpenRemoteBaseKey methods.
    /// If you request a 64-bit view on a 32-bit operating system, the returned keys will be in the 32-bit view.
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryview">Registry View - Microsoft Docs</a>
    /// <a href="https://source.dot.net/#Microsoft.Win32.Registry/Microsoft/Win32/RegistryView.cs">RegistryView.cs - .NET Core Source</a>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "False Postive triggered by suppression of Rule CA1027" )]
    public enum RegistryView {

        /// <summary>
        /// The default view.
        /// </summary>
        Default = 0x0000,

        /// <summary>
        /// The 32-bit view.
        /// </summary>
        Registry32 = 0x0200,

        /// <summary>
        /// The 64-bit view.
        /// </summary>
        Registry64 = 0x0100,
    }
}
