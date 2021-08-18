namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Defines the server type of a component.
    /// </summary>
    public enum ComponentServerType {

        /// <summary>
        /// The server type is not defined.
        /// </summary>
        /// <remarks>Only useful for comparisons etc. Do not try to register a server with the Undefined type.</remarks>
        Undefined,

        /// <summary>
        /// InprocServer (16-bit)
        /// </summary>
        /// <remarks>
        /// Inproc servers are packaged as Dlls.
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/com/inprocserver">InprocServer Key (Microsoft Docs)</a>
        /// </remarks>
        InprocServer,

        /// <summary>
        /// InProcServer32
        /// </summary>
        /// <remarks>
        /// Inproc servers are packaged as Dlls.
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/com/inprocserver32">InprocServer32 Key (Microsoft Docs)</a>
        /// </remarks>
        InprocServer32,

        /// <summary>
        /// LocalServer (16-bit)
        /// </summary>
        /// <remarks>
        /// Local Servers are packaged as executable.
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/com/localserver">LocalServer Key (Microsoft Docs)</a>
        /// </remarks>
        LocalServer,

        /// <summary>
        /// LocalServer32
        /// </summary>
        /// <remarks>
        /// Local Servers are packaged as executable.
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/com/localserver32">LocalServer32 Key (Microsoft Docs)</a>
        /// </remarks>
        LocalServer32,
    }
}
