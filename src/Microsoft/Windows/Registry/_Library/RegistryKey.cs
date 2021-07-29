using System;
using System.Diagnostics;

using Win32 = Microsoft.Win32;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    /// <summary>
    /// Represents a key-level node in the Windows registry.
    /// </summary>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform( "windows" )]
#endif
    public class RegistryKey
    : IRegistryKey {

        Win32.RegistryKey _win32RegistryKey;
        bool _writable = false;
        bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the RegistryKey class.
        /// </summary>
        protected internal RegistryKey( Win32.RegistryKey win32RegistryKey, bool writable ) {

            Debug.Assert( win32RegistryKey != null );
            _win32RegistryKey = win32RegistryKey;
            _writable = writable;
        }

        /// <inheritdoc/>
        public string Name => _win32RegistryKey.Name;

        /// <inheritdoc/>
        public RegistryView View => (RegistryView)_win32RegistryKey.View;

        /// <inheritdoc/>
        public bool Writable => _writable;

        /// <inheritdoc/>
        public int SubKeyCount => _win32RegistryKey.SubKeyCount;

        /// <inheritdoc/>
        public int ValueCount => _win32RegistryKey.ValueCount;

        /// <inheritdoc/>
        public void Close() => _win32RegistryKey.Close();

        /// <inheritdoc/>
        public void Flush() => _win32RegistryKey.Flush();

        /// <inheritdoc/>
        public string[] GetSubKeyNames() => _win32RegistryKey.GetSubKeyNames();

        /// <inheritdoc/>
        public IRegistryKey OpenSubKey( string name ) {

            Win32.RegistryKey key = _win32RegistryKey.OpenSubKey( name );
            return key == null ? null : new RegistryKey( key, false );
        }

        /// <inheritdoc/>
        public IRegistryKey OpenSubKey( string name, bool writable ) {

            Win32.RegistryKey key = _win32RegistryKey.OpenSubKey( name, writable );
            return key == null ? null : new RegistryKey( key, writable );
        }

        /// <inheritdoc/>
        public IRegistryKey CreateSubKey( string subkey ) {

            Win32.RegistryKey key = _win32RegistryKey.CreateSubKey( subkey );
            return new RegistryKey( key, true );
        }

        /// <inheritdoc/>
        public IRegistryKey CreateSubKey( string subkey, bool writable ) {

            Win32.RegistryKey key = _win32RegistryKey.CreateSubKey( subkey, writable );
            return new RegistryKey( key, writable );
        }

        /// <inheritdoc/>
        public void DeleteSubKey( string subkey ) => _win32RegistryKey.DeleteSubKey( subkey );

        /// <inheritdoc/>
        public void DeleteSubKey( string subkey, bool throwOnMissingSubKey ) => _win32RegistryKey.DeleteSubKey( subkey, throwOnMissingSubKey );

        /// <inheritdoc/>
        public void DeleteSubKeyTree( string subkey ) => _win32RegistryKey.DeleteSubKeyTree( subkey );

        /// <inheritdoc/>
        public void DeleteSubKeyTree( string subkey, bool throwOnMissingSubKey ) => _win32RegistryKey.DeleteSubKeyTree( subkey, throwOnMissingSubKey );

        /// <inheritdoc/>
        public void SetValue( string name, object value ) => _win32RegistryKey.SetValue( name, value );

        /// <inheritdoc/>
        public void SetValue( string name, object value, RegistryValueKind valueKind ) => _win32RegistryKey.SetValue( name, value, (Win32.RegistryValueKind)valueKind );

        /// <inheritdoc/>
        public object GetValue( string name, object defaultValue, RegistryValueOptions options ) => _win32RegistryKey.GetValue( name, defaultValue, (Win32.RegistryValueOptions)options );

        /// <inheritdoc/>
        public object GetValue( string name ) => _win32RegistryKey.GetValue( name );

        /// <inheritdoc/>
        public object GetValue( string name, object defaultValue ) => _win32RegistryKey.GetValue( name, defaultValue );

        /// <inheritdoc/>
        public void DeleteValue( string name ) => _win32RegistryKey.DeleteValue( name );

        /// <inheritdoc/>
        public void DeleteValue( string name, bool throwOnMissingValue ) => _win32RegistryKey.DeleteValue( name, throwOnMissingValue );

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
                _win32RegistryKey.Dispose();
            }

            _disposed = true;
        }

        /// <inheritdoc/>
        public string[] GetValueNames() => _win32RegistryKey.GetValueNames();

        /// <inheritdoc/>
        public RegistryValueKind GetValueKind( string name ) => (RegistryValueKind)_win32RegistryKey.GetValueKind( name );
    }
}
