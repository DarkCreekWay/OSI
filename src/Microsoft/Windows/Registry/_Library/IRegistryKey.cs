using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Defines a common interface for a registry keys.
    /// </summary>
    /// <remarks>
    /// The interface declaration is based on the RegistryKey class of the Microsoft .NET ecosystem.<br/>
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey">RegistryKey - Microsoft Docs</a>
    ///
    /// The Microsoft RegistryKey class implementation implements the IDisposable pattern.
    /// In order to ensure maximum compatibility to existing code, the IDisposable inteface is part of this declaration too.
    /// </remarks>

    public interface IRegistryKey : IDisposable {

        #region Properties

        /// <summary>
        /// Retrieves the name of the key.
        /// </summary>
        /// <remarks>
        /// The absolute (qualified) name of the key.
        /// </remarks>

        string Name {
            get;
        }

        /// <summary>
        /// Gets the view that was used to create the registry key.
        /// </summary>

        RegistryView View {
            get;
        }

        /// <summary>
        /// Retrieves, if the key is opened as writable.
        /// </summary>

        bool Writable {
            get;
        }

        /// <summary>
        /// Retrieves the count of subkeys of the current key.
        /// </summary>

        int SubKeyCount {
            get;
        }

        /// <summary>
        /// Retrieves the count of values in the key.
        /// </summary>

        int ValueCount {
            get;
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Closes the key and flushes it to disk if its contents have been modified.
        /// </summary>
        /// <remarks>
        /// Calling this method on system keys will have no effect, because system keys are never closed.
        /// This method does nothing if you call it on an instance of RegistryKey that is already closed.
        /// </remarks>

        void Close();

        /// <summary>
        /// Writes all the attributes of the specified open registry key into the registry.
        /// </summary>
        /// <remarks>It is not necessary to call Flush to write out changes to a key.
        /// Registry changes are flushed to disk when the registry uses its lazy flusher.
        /// Lazy flushing occurs automatically and regularly after a system-specified time interval.
        /// Registry changes are also flushed to disk at system shutdown.
        /// Unlike Close, the Flush function returns only when all the data has been written to the registry.
        /// The Flush function might also write out parts of or all of the other keys.
        /// Calling this function excessively can have a negative effect on an application's performance.
        /// An application should only call Flush if it must be absolute certain that registry changes are recorded to disk.
        /// In general, Flush rarely, if ever, need be used.</remarks>

        void Flush();

        #endregion

        #region Registry Key Methods

        /// <summary>
        /// Retrieves an array of strings that contains all the subkey names.
        /// </summary>
        /// <returns>An array of strings that contains the names of the subkeys for the current key.</returns>

        string[] GetSubKeyNames();

        /// <summary>
        /// Retrieves a subkey as read-only.
        /// </summary>
        /// <param name="name">The name or path of the subkey to open as read-only.</param>
        /// <returns>The subkey requested, or null if the operation failed.</returns>

        IRegistryKey OpenSubKey( string name );

        /// <summary>
        /// Retrieves a specified subkey, and specifies whether write access is to be applied to the key.
        /// </summary>
        /// <param name="name">Name or path of the subkey to open.</param>
        /// <param name="writable">Set to true if you need write access to the key.</param>
        /// <returns>The subkey requested, or null if the operation failed.</returns>
        /// <remarks>
        /// <para>
        ///     <item><a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey.opensubkey#Microsoft_Win32_RegistryKey_OpenSubKey_System_String_System_Boolean_">OpenSubKey(String, Boolean) - Microsofr Docs</a></item>
        /// </para>
        /// <para>
        ///     <item><a href="https://referencesource.microsoft.com/#mscorlib/microsoft/win32/registrykey.cs,c23479cd45b3f448">OpenSubKey(String, Boolean) - Microsoft .NET Framework Reference Source</a></item>
        /// </para>
        /// </remarks>

        IRegistryKey OpenSubKey( string name, bool writable );

        // Overloads not declared yet, as they require additional types and enums to be defined first.
        // IRegistryKey OpenSubKey( string name, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck, System.Security.AccessControl.RegistryRights rights );
        // IRegistryKey OpenSubKey( string name, System.Security.AccessControl.RegistryRights rights );
        // IRegistryKey OpenSubKey( string name, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck );

        /// <summary>
        /// Creates a new subkey or opens an existing subkey for write access.
        /// </summary>
        /// <param name="subkey">The name or path of the subkey to create or open. This string is not case-sensitive.</param>
        /// <returns>The newly created subkey, or null if the operation failed. If a zero-length string is specified for subkey, the current RegistryKey object is returned.</returns>

        IRegistryKey CreateSubKey( string subkey );

        /// <summary>
        /// Creates a new subkey or opens an existing subkey with the specified access.
        /// </summary>
        /// <param name="subkey">The name or path of the subkey to create or open. This string is not case-sensitive.</param>
        /// <param name="writable">true to indicate the new subkey is writable; otherwise, false.</param>
        /// <returns>The newly created subkey, or null if the operation failed. If a zero-length string is specified for subkey, the current RegistryKey object is returned.</returns>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey.createsubkey#Microsoft_Win32_RegistryKey_CreateSubKey_System_String_System_Boolean_">CreateSubKey(String, Boolean) - Microsoft Docs</a>
        /// </remarks>

        IRegistryKey CreateSubKey( string subkey, bool writable );

        // Overloads not declared yet, as they require additional types and enums to be defined first.
        // IRegistryKey CreateSubKey( string subkey, bool writable, Microsoft.Win32.RegistryOptions options ); // Probably also requires a property exposed for the RegistryOptions used ?
        // IRegistryKey CreateSubKey (string subkey, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck);
        // IRegistryKey CreateSubKey (string subkey, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck, System.Security.AccessControl.RegistrySecurity registrySecurity);
        // IRegistryKey CreateSubKey (string subkey, bool writable, Microsoft.Win32.RegistryOptions options);
        // IRegistryKey CreateSubKey (string subkey, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck, Microsoft.Win32.RegistryOptions registryOptions);
        // IRegistryKey CreateSubKey (string subkey, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck, Microsoft.Win32.RegistryOptions registryOptions, System.Security.AccessControl.RegistrySecurity registrySecurity);

        /// <summary>
        /// Deletes the specified subkey.
        /// </summary>
        /// <param name="subkey">he name of the subkey to delete. This string is not case-sensitive.</param>

        void DeleteSubKey( string subkey );

        /// <summary>
        /// Deletes the specified subkey, and specifies whether an exception is raised if the subkey is not found.
        /// </summary>
        /// <param name="subkey">The name of the subkey to delete. This string is not case-sensitive.</param>
        /// <param name="throwOnMissingSubKey">Indicates whether an exception should be raised if the specified subkey cannot be found. If this argument is true and the specified subkey does not exist, an exception is raised. If this argument is false and the specified subkey does not exist, no action is taken.</param>

        void DeleteSubKey( string subkey, bool throwOnMissingSubKey );

        /// <summary>
        /// Deletes the specified subkey and any child subkeys recursively, and specifies whether an exception is raised if the subkey is not found.
        /// </summary>
        /// <param name="subkey">The name of the subkey to delete. This string is not case-sensitive.</param>
        /// <param name="throwOnMissingSubKey">Indicates whether an exception should be raised if the specified subkey cannot be found. If this argument is true and the specified subkey does not exist, an exception is raised. If this argument is false and the specified subkey does not exist, no action is taken.</param>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey.deletesubkeytree#Microsoft_Win32_RegistryKey_DeleteSubKeyTree_System_String_System_Boolean_">DeleteSubKeyTree(String, Boolean) - Microsoft Docs</a>
        /// </remarks>

        void DeleteSubKeyTree( string subkey, bool throwOnMissingSubKey );

        /// <summary>
        /// Deletes a subkey and any child subkeys recursively.
        /// </summary>
        /// <param name="subkey">The subkey to delete. This string is not case-sensitive.</param>

        void DeleteSubKeyTree( string subkey );

        #endregion

        #region Registry Value Methods

        /// <summary>
        /// Sets the specified name/value pair.
        /// </summary>
        /// <param name="name">The name of the value to store.</param>
        /// <param name="value">The data to be stored.</param>

        void SetValue( string name, object value );

        /// <summary>
        /// Sets the value of a name/value pair in the registry key, using the specified registry data type.
        /// </summary>
        /// <param name="name">The name of the value to be stored.</param>
        /// <param name="value">The data to be stored.</param>
        /// <param name="valueKind">The registry data type to use when storing the data.</param>

        void SetValue( string name, object value, RegistryValueKind valueKind );

        /// <summary>
        /// Retrieves the value associated with the specified name and retrieval options. If the name is not found, returns the default value that you provide.
        /// </summary>
        /// <param name="name">The name of the value to retrieve. This string is not case-sensitive.</param>
        /// <param name="defaultValue">The value to return if name does not exist.</param>
        /// <param name="options">One of the enumeration values that specifies optional processing of the retrieved value.</param>
        /// <returns>The value associated with name, processed according to the specified options, or defaultValue if name is not found.</returns>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey.getvalue?#Microsoft_Win32_RegistryKey_GetValue_System_String_System_Object_Microsoft_Win32_RegistryValueOptions_">GetValue(String, Object, RegistryValueOptions) - Microsoft Docs</a>
        /// <a href="https://referencesource.microsoft.com/#mscorlib/microsoft/win32/registrykey.cs,73f844a5763f47ae">GetValue(String, Object, RegistryValueOptions) - Microsoft .NET Framework Reference Source</a>
        /// </remarks>

        object GetValue( string name, object defaultValue, RegistryValueOptions options );

        /// <summary>
        /// Retrieves the value associated with the specified name. Returns null if the name/value pair does not exist in the registry.
        /// </summary>
        /// <param name="name">The name of the value to retrieve. This string is not case-sensitive.</param>
        /// <returns>The value associated with name, or null if name is not found.</returns>

        object GetValue( string name );

        /// <summary>
        /// Retrieves the value associated with the specified name. If the name is not found, returns the default value that you provide.
        /// </summary>
        /// <param name="name">The name of the value to retrieve. This string is not case-sensitive.</param>
        /// <param name="defaultValue">The value to return if name does not exist.</param>
        /// <returns>The value associated with name, with any embedded environment variables left unexpanded, or defaultValue if name is not found.</returns>

        object GetValue( string name, object defaultValue );

        /// <summary>
        /// Retrieves an array of strings that contains all the value names associated with this key.
        /// </summary>
        /// <returns>An array of strings that contains the value names for the current key.</returns>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey.getvaluenames">RegistryKey.GetValueNames Method (Microsoft Docs)</a>
        /// </remarks>

        string[] GetValueNames();

        /// <summary>
        /// Retrieves the registry data type of the value associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the value whose registry data type is to be retrieved. This string is not case-sensitive.</param>
        /// <returns>The registry data type of the value associated with name.</returns>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey.getvaluekind">RegistryKey.GetValueKind(String) Method (Microsoft Docs)</a>
        /// <a href="https://referencesource.microsoft.com/#mscorlib/microsoft/win32/registrykey.cs,40afa6bf88a6fd60">RegistryKey.GetValueKind(String) Method - Microsoft .NET Framework Reference Source</a>
        /// </remarks>

        RegistryValueKind GetValueKind( string name );

        /// <summary>
        /// Deletes the specified value from this key, and throws an exception if the value is not found.
        /// </summary>
        /// <param name="name">The name of the value to delete.</param>

        void DeleteValue( string name );

        /// <summary>
        /// Deletes the specified value from this key, and specifies whether an exception is raised if the value is not found.
        /// </summary>
        /// <param name="name">The name of the value to delete.</param>
        /// <param name="throwOnMissingValue">Indicates whether an exception should be raised if the specified value cannot be found. If this argument is true and the specified value does not exist, an exception is raised. If this argument is false and the specified value does not exist, no action is taken.</param>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registrykey.deletesubkey#Microsoft_Win32_RegistryKey_DeleteSubKey_System_String_System_Boolean_">DeleteSubKey(String, Boolean) - Microsoft Docs</a>
        /// <a href="https://referencesource.microsoft.com/#mscorlib/microsoft/win32/registrykey.cs,40afa6bf88a6fd60">DeleteSubkey(String, Boolean) - Microsoft .NET Framework Reference Source</a>
        /// </remarks>

        void DeleteValue( string name, bool throwOnMissingValue );

        #endregion

    }
}
