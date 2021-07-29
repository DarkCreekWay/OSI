using Win32 = Microsoft.Win32;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Provides access to the Microsoft Windows Registry.
    /// </summary>
    /// <remarks>
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registry">Registry Class</a>
    /// </remarks>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform( "windows" )]
#endif
    public class WindowsRegistry
    : IRegistry {

        ///<inheritdoc/>
        public IRegistryKey OpenBaseKey( RegistryHive hKey, RegistryView view ) {

            Win32.RegistryKey hiveKey = Win32.RegistryKey.OpenBaseKey( (Win32.RegistryHive)hKey, (Win32.RegistryView)view );

            return new RegistryKey( hiveKey, true );

            // TODO: Test assumption that a registry hive key is opened as writable implicitly by MS code.
        }
    }
}
