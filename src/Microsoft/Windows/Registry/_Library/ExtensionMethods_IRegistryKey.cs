using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    //! IRegistrKey Extension Methods

    public static partial class ExtensionMethods {

        /// <summary>
        /// Tests, if the registry Key has any subkeys or values.
        /// </summary>
        /// <param name="registryKey">The extended registry key instance</param>
        /// <returns>true, if registry key has no subkeys and values. Othervise false.</returns>
        [DebuggerStepThrough]
        public static bool IsEmpty( this IRegistryKey registryKey ) {
            return registryKey.SubKeyCount == 0 && registryKey.ValueCount == 0;
        }

        /// <summary>
        /// Tests, if a given subkey of the current registry key is empty.
        /// </summary>
        /// <param name="registryKey">The extended registry key instance</param>
        /// <param name="subKey">The name of the subkey to test.</param>
        /// <returns>true, if the given subkey was not found or the subkey has no subkeys and no values. returns false otherwise.</returns>
        [DebuggerStepThrough]
        public static bool SubKeyIsEmpty( this IRegistryKey registryKey, string subKey ) {

            bool result = true;

            using( IRegistryKey key = registryKey.OpenSubKey( subKey ) ) {

                if( null == key ) {
                    return true;
                }

                result = key.IsEmpty();
                key.Close();
            }

            return result;

        }

        /// <summary>
        /// Sets a name/value pair to string value. <see cref="RegistryValueKind.String"/>
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the Name/Value pair</param>
        /// <param name="value">The value to be set.</param>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void SetString( this IRegistryKey registryKey, string name, string value ) {
            registryKey.SetValue( name, value, RegistryValueKind.String );
        }

        /// <summary>
        /// Returns the content of the named value as string or null.
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the Name/Value pair.</param>
        /// <returns>The value of the Name/Value pair as string or null.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string GetString( this IRegistryKey registryKey, string name ) {

            return registryKey.Get<string>( name );
        }

        /// <summary>
        /// Gets the value of a name/value pair as string or a default value.
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="defaultValue">The default value to be used as fallback.</param>
        /// <returns>The value of the Name/Value pair as string, if found. Otherwise, the defaultValue is returned.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string GetString( this IRegistryKey registryKey, string name, string defaultValue = "" ) {

            return registryKey.Get( name, defaultValue );
        }

        /// <summary>
        /// Gets the value of a name/value pair as string and optionally expand it or a default value
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="defaultValue">The default value to be used as fallback.</param>
        /// <param name="expand">Set to true, if the value should be expanded.</param>
        /// <returns>The value of the Name/Value pair as string, which is optionally expanded. Otherwise, the defaultValue is returned.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string GetString( this IRegistryKey registryKey, string name, string defaultValue, bool expand ) {

            RegistryValueOptions options = expand ? RegistryValueOptions.None : RegistryValueOptions.DoNotExpandEnvironmentNames;

            return (string)registryKey.GetValue( name, defaultValue, options );

        }

        /// <summary>
        /// Gets the unexpanded value of a name/value pair of type <see cref="RegistryValueKind.ExpandString"/>.
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="defaultValue">The default value to be used as fallback.</param>
        /// <returns>If found, The unexpanded value of the name/value pair as string, otherwise the default value.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string GetUnexpandedString( this IRegistryKey registryKey, string name, string defaultValue = "" ) {

            return registryKey.GetString( name, defaultValue, false );
        }

        /// <summary>
        /// Sets the value of a name/value pair as <see cref="RegistryValueKind.ExpandString"/>.
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="value">The value to set.</param>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void SetExpandableString( this IRegistryKey registryKey, string name, string value ) {
            registryKey.SetValue( name, value, RegistryValueKind.ExpandString );
        }

        /// <summary>
        /// Setsa name/value pair as REG_DWORD (<see cref="RegistryValueKind.DWord"/>)
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="value">The value to be set.</param>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void SetInt( this IRegistryKey registryKey, string name, int value ) {
            registryKey.SetValue( name, value, RegistryValueKind.DWord );
        }

        /// <summary>
        /// Gets the value of a name/value pair as int.
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="defaultValue">The default value to be used as fallback.</param>
        /// <returns>If found, The value of the name/value pair as int, otherwise the default value.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static int GetInt( this IRegistryKey registryKey, string name, int defaultValue = int.MinValue ) {

            return registryKey.Get( name, defaultValue );
        }

        /// <summary>
        /// Gets the value of a name/value pair as int.
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <returns>The value of the name/value pair as int.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static int GetInt( this IRegistryKey registryKey, string name ) {
            return registryKey.Get<int>( name );
        }

        /// <summary>
        /// Gets the value of a name/value pair as T.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <returns>The value of the name/value pair as int.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T Get<T>( this IRegistryKey registryKey, string name ) {
            return (T)registryKey.GetValue( name );
        }

        /// <summary>
        /// Gets the value of a name/value pair as T.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="defaultValue">The default value to be used as fallback.</param>
        /// <returns>If found, The value of the name/value pair as int, otherwise the default value.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T Get<T>( this IRegistryKey registryKey, string name, T defaultValue ) {
            return (T)registryKey.GetValue( name, defaultValue );
        }

        #region Registry Value Typed Extension Methods

        /// <summary>
        /// Gets the value of a name/value pair of type REG_SZ (<see cref="RegistryValueKind.String"/>) as string or a fallback value.
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="defaultValue">The default value to be used as fallback.</param>
        /// <returns>If found, The value of the name/value pair as string, otherwise the default value.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string Get_REG_SZ( this IRegistryKey registryKey, string name, string defaultValue = "" ) {
            return (string)registryKey.GetValue( name, defaultValue );
        }

        /// <summary>
        /// Sets a name/value pair as REG_SZ (<see cref="RegistryValueKind.String"/>)
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="value">The value to set.</param>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Set_REG_SZ( this IRegistryKey registryKey, string name, string value ) {
            registryKey.SetValue( name, value, RegistryValueKind.String );
        }

        /// <summary>
        /// Gets the value of a name/value pair of type REG_SZ (<see cref="RegistryValueKind.String"/>) as string or a fallback value
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="defaultValue">The default value to be used as fallback.</param>
        /// <param name="expand">Set to true, if the value should be expanded.</param>
        /// <returns>If found, the value of the Name/Value pair as string, which is optionally expanded. Otherwise, the defaultValue.</returns>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string Get_REG_EXP_SZ( this IRegistryKey registryKey, string name, string defaultValue = "", bool expand = true ) {
            return registryKey.GetString( name, defaultValue, expand );
        }

        /// <summary>
        /// Sets a name/value pair as REG_Exp_SZ (<see cref="RegistryValueKind.ExpandString"/>)
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="value">The value to be set.</param>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Set_REG_EXP_SZ( this IRegistryKey registryKey, string name, string value ) {
            registryKey.SetValue( name, value, RegistryValueKind.ExpandString );
        }

        /// <summary>
        /// Sets a name/value pair as REG_DWORD (<see cref="RegistryValueKind.DWord"/>)
        /// </summary>
        /// <param name="registryKey">The extended <see cref="IRegistryKey"/> instance.</param>
        /// <param name="name">The name of the name/value pair.</param>
        /// <param name="value">The value to be set.</param>
        [DebuggerStepThrough]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Set_REG_DWORD( this IRegistryKey registryKey, string name, int value ) {
            registryKey.SetValue( name, value, RegistryValueKind.DWord );
        }

        #endregion
    }
}

// TODO: Add more extensions methods like SetREG_EXP_SZ, GetREG_EXP_SZ, etc.
