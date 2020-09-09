using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    //! IRegistry Extension Methods

    /// <summary>
    /// Provides useful and convinient extension methods for increased productivity while developing registry related code.
    /// </summary>
    public static partial class ExtensionMethods {

        /// <summary>
        /// Gets a value from a given registry hive, registry key and registry name with a single call.
        /// </summary>
        /// <param name="registry">The extended <see cref="IRegistry"/> instance.</param>
        /// <param name="hive">The <see cref="RegistryHive"/> to use.</param>
        /// <param name="view">The <see cref="RegistryView"/> to use.</param>
        /// <param name="subkey">The path to the registryKey to open.</param>
        /// <param name="name">The name of the value to be read.</param>
        /// <param name="defaultValue">The default value to be returned, when the value is not defined.</param>
        /// <returns>The value for a given name of the RegistryKey, when it exists.. Otherwise,the defaultValue gets returned.</returns>
        public static object GetValue( this IRegistry registry, RegistryHive hive, RegistryView view, string subkey, string name, object defaultValue ) {

            using ( IRegistryKey hiveKey = registry.OpenBaseKey( hive, view ) ) {
                using ( IRegistryKey subRegKey = hiveKey.OpenSubKey( subkey ) ) {
                    if ( null == subRegKey ) {
                        throw new NotImplementedException();
                    }

                    return subRegKey.GetValue( name, defaultValue );
                }
            }
        }
    }
}
