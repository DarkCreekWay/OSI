namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Defines a common interface for accessing and managing the Registry
    /// </summary>
    /// <remarks>The type declaration is based on the Registry class of the Microsoft ecosystem.</remarks>
    public interface IRegistry {

        /// <summary>
        /// Opens a new <see cref="IRegistryKey"/> that represents the requested <see cref="RegistryHive"/> on the local machine with the specified <see cref="RegistryView"/>.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive"/> to open.</param>
        /// <param name="view">The <see cref="RegistryView"/> to use.</param>
        /// <returns>The requested registry key.</returns>

        IRegistryKey OpenBaseKey( RegistryHive hive, RegistryView view = RegistryView.Default );

    }
}
