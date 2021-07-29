using System;
using System.Diagnostics;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry.InMemory {

    /// <inheritdoc cref="IRegistryKey"/>
    /// <summary>
    /// Provides a Readonly version of a InMemoryRegistryKey.
    /// </summary>
    /// <remarks>
    /// Tests with the Win32 implementation shows, that it is safe to use a decorating type for read-only access.
    /// Refer to Win32_Registry Tests for details.
    /// </remarks>
    [DebuggerDisplay( "{DebuggerDisplay,nq}" )]
    public class ReadOnlyInMemoryRegistryKey
    : IRegistryKey {

        InMemoryRegistryKey _registryKey = null;

        /// <summary>
        /// Initializes a new instance of the ReadOnlyInMemoryRegistryKey class.
        /// </summary>
        /// <param name="registryKey">The <see cref="InMemoryRegistryKey"/> instance to wrap as read only.</param>

        protected internal ReadOnlyInMemoryRegistryKey( InMemoryRegistryKey registryKey ) {
            _registryKey = registryKey;
        }

        /// <inheritdoc/>
        public string Name => _registryKey.Name;

        /// <inheritdoc/>
        public RegistryView View => _registryKey.View;

        /// <inheritdoc/>
        public bool Writable => false;

        /// <inheritdoc/>
        public int SubKeyCount => _registryKey.SubKeyCount;

        /// <inheritdoc/>
        public int ValueCount => _registryKey.ValueCount;

        /// <inheritdoc/>
        public void Close() {
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Flush() {
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string[] GetSubKeyNames() => _registryKey.GetSubKeyNames();

        /// <inheritdoc/>
        public IRegistryKey OpenSubKey( string name ) => _registryKey.OpenSubKey( name );

        /// <inheritdoc/>
        public IRegistryKey OpenSubKey( string name, bool writable ) => _registryKey.OpenSubKey( name, writable );

        /// <inheritdoc/>
        public IRegistryKey CreateSubKey( string subkey ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public IRegistryKey CreateSubKey( string subkey, bool writable ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public void DeleteSubKey( string subkey ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public void DeleteSubKey( string subkey, bool throwOnMissingSubKey ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public void DeleteSubKeyTree( string subkey, bool throwOnMissingSubKey ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public void DeleteSubKeyTree( string subkey ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public void SetValue( string name, object value ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public void SetValue( string name, object value, RegistryValueKind valueKind ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public object GetValue( string name, object defaultValue, RegistryValueOptions options ) => _registryKey.GetValue( name, default, options );

        /// <inheritdoc/>
        public object GetValue( string name ) => _registryKey.GetValue( name );

        /// <inheritdoc/>
        public object GetValue( string name, object defaultValue ) => _registryKey.GetValue( name, defaultValue );

        /// <inheritdoc/>
        public string[] GetValueNames() => _registryKey.GetValueNames();

        /// <inheritdoc/>
        public RegistryValueKind GetValueKind( string name ) => _registryKey.GetValueKind( name );

        /// <inheritdoc/>
        public void DeleteValue( string name ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        /// <inheritdoc/>
        public void DeleteValue( string name, bool throwOnMissingValue ) => throw new UnauthorizedAccessException( "Registry Key is not writable" );

        bool _disposed = false;

        /// <inheritdoc/>
        public void Dispose() {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <inheritdoc/>
        protected virtual void Dispose( bool disposing ) {

            if( _disposed ) {
                return;
            }

            if( disposing ) {
                // Free managed resources
            }

            _disposed = true;
        }

        [DebuggerBrowsable( DebuggerBrowsableState.Never )]
        string DebuggerDisplay {
            get => $"{Name}, Keys: {SubKeyCount}, Values: {ValueCount}, Writable: false";
        }
    }
}
