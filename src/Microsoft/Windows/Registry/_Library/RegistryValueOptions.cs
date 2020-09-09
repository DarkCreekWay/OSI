using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Specifies optional behavior when retrieving name/value pairs from a registry key.
    /// This enumeration has a <seealso cref="System.FlagsAttribute"/> attribute that allows a bitwise combination of its member values.
    /// </summary>
    /// <remarks>
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvalueoptions">RegistryValueOptions - Microsoft Docs</a>
    /// <a href="https://source.dot.net/#Microsoft.Win32.Registry/Microsoft/Win32/RegistryValueOptions.cs">RegistryValueOptions - .NET Core Source</a>
    /// </remarks>

    [Flags]
    public enum RegistryValueOptions {

        /// <summary>
        /// No optional behavior is specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// A value of type ExpandString is retrieved without expanding its embedded environment variables.
        /// </summary>
        DoNotExpandEnvironmentNames = 1
    }
}