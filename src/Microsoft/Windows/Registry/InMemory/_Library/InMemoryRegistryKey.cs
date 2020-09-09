using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry.InMemory {

    /// <inheritdoc cref="IRegistryKey"/>
    /// <remarks>
    /// This type is always writable. Readonly access is realized through returning a ReadOnlyInMemoryRegistryKey instance.
    /// This ensures, that the InMemory implementation of the Registry behaves as much as the Win32 variant.
    /// </remarks>
    [DebuggerDisplay( "{DebuggerDisplay,nq}" )]
    public class InMemoryRegistryKey
    : IRegistryKey {

        const string s_SubkeySeparator = @"\\";

        InMemoryRegistryKey _parent;
        string _name;
        Dictionary<string, InMemoryRegistryKey> _keys = new Dictionary<string, InMemoryRegistryKey>();
        Dictionary<string, RegistryValue> _values = new Dictionary<string, RegistryValue>();

        /// <summary>
        /// Initializes a new instance of the InMemoryRegistryKey class.
        /// </summary>
        /// <param name="parent">The parent key instance.</param>
        /// <param name="name">The name of the Registry key.</param>
        protected internal InMemoryRegistryKey( InMemoryRegistryKey parent, string name ) {

            _parent = parent;
            _name = name;
            Name = BuildFullQualifiedName();
            //Writable = writable;
        }

        #region Properties

        /// <inheritdoc/>
        public string Name {
            get;
            private set;
        }

        /// <inheritdoc/>
        public RegistryView View {
            get;
        } = RegistryView.Default;

        /// <inheritdoc/>
        public bool Writable => true;

        /// <inheritdoc/>
        public int SubKeyCount => _keys.Count;

        /// <inheritdoc/>
        public int ValueCount => _values.Count;

        #endregion

        #region Registry Key Methods

        /// <inheritdoc/>
        public string[] GetSubKeyNames() {

            // Build list of subkey names from the name values, as provided by the client to ensure expected casing.
            string[] result = new string[_keys.Count];
            int index = 0;

            foreach( string internalKey in _keys.Keys ) {
                result[index++] = _keys[internalKey]._name;
            }

            return result;
        }

        /// <inheritdoc/>
        public IRegistryKey OpenSubKey( string subkey ) {
            return OpenSubKey( subkey, false );
        }

        /// <inheritdoc/>
        public IRegistryKey OpenSubKey( string subkey, bool writable ) {

            if( subkey == null ) {
                throw new ArgumentNullException( nameof( subkey ) );
            }

            string[] names = subkey.Split( s_SubkeySeparator.ToCharArray(), options: StringSplitOptions.RemoveEmptyEntries );

            InMemoryRegistryKey current = this;

            foreach( string name in names ) {

                string key = GetSubkeyKey( name );

                if( !current._keys.ContainsKey( key ) ) {
                    return null;
                }

                current = current._keys[key];

            }

            // The real registry allows to open a registry key multiple times.
            // While one instance could be read-only, another could be writable.
            // Decorating the result based on writable value simulates this.
            return writable ? current : new ReadOnlyInMemoryRegistryKey( current ) as IRegistryKey;
        }

        /// <inheritdoc/>
        public IRegistryKey CreateSubKey( string subkey ) => CreateSubKey( subkey, true );

        /// <inheritdoc/>
        public IRegistryKey CreateSubKey( string subkey, bool writable ) {

            // EnsureWritable();

            if( null == subkey ) {
                throw new ArgumentNullException( nameof( subkey ) );
            }

            if( subkey.Length == 0 ) {

                if( !Writable && writable ) {
                    throw new UnauthorizedAccessException( "The current Registry Key was not opened writable" ); // TODO: Validate this behavior with the Win32 RegistryKey implementation of Microsoft
                }
            }

            string[] names = subkey.Split( s_SubkeySeparator.ToCharArray(), options: StringSplitOptions.RemoveEmptyEntries );

            InMemoryRegistryKey current = this;

            foreach( string name in names ) {

                string key = GetSubkeyKey( name );

                if( !current._keys.ContainsKey( key ) ) {
                    current._keys.Add( key, new InMemoryRegistryKey( current, name /*, writable */ ) );
                }

                current = current._keys[key];

            }

            return writable ? current : new ReadOnlyInMemoryRegistryKey( current ) as IRegistryKey;
        }


        /// <inheritdoc/>
        public void DeleteSubKey( string subkey ) => DeleteSubKey( subkey, true );

        /// <inheritdoc/>
        public void DeleteSubKey( string subkey, bool throwOnMissingSubKey ) {

            if( null == subkey ) {
                throw new ArgumentNullException( nameof( subkey ) );
            }

            string[] names = subkey.Split( s_SubkeySeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
            string key;
            InMemoryRegistryKey current = this;

            for( int i = 0; i < names.Length - 1; i++ ) {

                key = GetSubkeyKey( names[i] );

                if( !current._keys.ContainsKey( key ) ) {
                    if( !throwOnMissingSubKey ) {
                        return;
                    }
                    throw new ArgumentException( $"The Subkey parameter {subkey} does not specify a valid registry key" );
                }

                current = current._keys[key];
            }

            key = GetSubkeyKey( names[names.Length - 1] );

            if( !current._keys.ContainsKey( key ) ) {
                if( !throwOnMissingSubKey ) {
                    return;
                }
                throw new ArgumentException( $"The subkey parameter {subkey} does not specify a valid registry key" );
            }

            if( current._keys[key].SubKeyCount > 0 ) {
                throw new InvalidOperationException( $"The specified subkey {subkey} has child subkeys." );
            }

            current._keys.Remove( key );

        }

        /// <inheritdoc/>
        public void DeleteSubKeyTree( string subkey ) => DeleteSubKeyTree( subkey, true );

        /// <inheritdoc/>
        /// <param name="subkey"></param>
        /// <param name="throwOnMissingSubKey"></param>
        public void DeleteSubKeyTree( string subkey, bool throwOnMissingSubKey ) {

            // TODO: Verify, how Microsofts implementation handles the case, when current key is readonly and simulate the behavior.

            if( null == subkey ) {
                throw new ArgumentNullException( nameof( subkey ) );
            }

            string[] names = subkey.Split( s_SubkeySeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );

            // Navigate the relative path as defined by subkey parameter down to the parent of the final key

            string key;
            InMemoryRegistryKey current = this;
            for( int i = 0; i < names.Length - 1; i++ ) {

                key = GetSubkeyKey( names[i] );

                if( !current._keys.ContainsKey( key ) ) {
                    if( !throwOnMissingSubKey ) {
                        return;
                    }
                    throw new ArgumentException( $"The Subkey parameter {subkey} does not specify a valid registry key" );
                }

                current = current._keys[key];
            }

            // Reached final parent level. Handle the referenced key.

            key = GetSubkeyKey( names[names.Length - 1] );
            if( !current._keys.ContainsKey( key ) ) {
                if( !throwOnMissingSubKey ) {
                    return;
                }

                throw new ArgumentException( $"The subkey parameter {subkey} does not specify a valid registry key" );
            }
            current._keys.Remove( key );

        }

        #endregion

        #region Registry Values

        /// <inheritdoc/>
        public void SetValue( string name, object value ) => SetValue( name, value, GetValueKind( value ) );

        /// <inheritdoc/>
        public void SetValue( string name, object value, RegistryValueKind valueKind ) {

            // EnsureWritable();
            if( null == value ) {
                throw new ArgumentNullException( nameof( value ) );
            }

            // TODO: Validate name against Microsoft Rules for Value Keys
            string key = GetValueKey( name );

            if( true == _values.TryGetValue( key, out RegistryValue registryValue ) ) {
                // TODO: Do all the validation of value type, value kind etc. here
            }
            else {

                registryValue = new RegistryValue() {
                    Name = name,
                    Value = value,
                    Kind = valueKind
                };

                _values.Add( key, registryValue );

            }
        }

        /// <inheritdoc/>
        public object GetValue( string name, object defaultValue, RegistryValueOptions options ) {

            bool doNotExpand = false;
            switch( options ) {
                case RegistryValueOptions.None:
                    doNotExpand = false;
                    break;
                case RegistryValueOptions.DoNotExpandEnvironmentNames:
                    doNotExpand = true;
                    break;
                default:
                    throw new ArgumentException( $"{nameof( options )} value {options} not supported" );
            }

            return InternalGetValue( name, defaultValue, doNotExpand );
        }

        /// <inheritdoc/>
        public object GetValue( string name ) => InternalGetValue( name, null, false );

        /// <inheritdoc/>
        public object GetValue( string name, object defaultValue ) => InternalGetValue( name, defaultValue, false );

        /// <inheritdoc/>
        public string[] GetValueNames() {

            string[] result = new string[_values.Count];
            int index = 0;

            foreach( RegistryValue value in _values.Values ) {
                result[index++] = value.Name;
            }

            return result;
        }

        /// <inheritdoc/>
        public RegistryValueKind GetValueKind( string name ) {

            string key = GetValueKey( name );

            if( true == _values.TryGetValue( key, out RegistryValue registryValue ) ) {
                return registryValue.Kind;
            }

            throw new IOException( $"The specified registry-value {name} does not exist." );
        }

        /// <inheritdoc/>
        public void DeleteValue( string name ) => DeleteValue( name, true );

        /// <inheritdoc/>
        public void DeleteValue( string name, bool throwOnMissingValue ) {

            // EnsureWritable();
            string key = GetValueKey( name );

            if( false == _values.ContainsKey( key ) ) {

                if( false == throwOnMissingValue ) {
                    return;
                }

                throw new ArgumentException( $"The specified registry-value {name} does not exist." );
            }
            else {
                _values.Remove( key );
            }
        }

        #endregion

        #region Common Mathods

        /// <inheritdoc/>
        public void Close() {
            // No implementation required. Empty on purpose
        }

        /// <inheritdoc/>
        public void Flush() {
            // No implementation required. Empty on purpose
        }

        /// <inheritdoc/>
        public void Dispose() {
            // We have nothing to dispose, but need to implement the method to fullfill the interface. So it is ok to implement it like this
        }

        #endregion

        object InternalGetValue( string name, object defaultValue, bool doNotExpand ) {

            string key = GetValueKey( name );

            if( false == _values.TryGetValue( key, out RegistryValue registryValue ) ) {
                return defaultValue;
            }

            if( registryValue.Kind != RegistryValueKind.ExpandString || doNotExpand ) {
                return registryValue.Value;
            }

            return Environment.ExpandEnvironmentVariables( (string)registryValue.Value ); // TODO: Decide, if we need a custom Expanding function instead of using the Environment type of the framework
        }

        /// <summary>
        /// Get key for _values Dictionary from Reg-Value Name
        /// </summary>
        /// <param name="name">Name of the Reg-Value</param>
        /// <returns>A key for the _values Dictionary</returns>
        /// <remarks>
        /// This guarantees accessibility of a reg-value in an case-insensitive way
        /// </remarks>
        string GetValueKey( string name ) {

            return name?.ToLowerInvariant() ?? ""; // Coalesce operator ensures, we also get a valid key for the default value, which can be addressed by null or "" as name
        }

        string GetSubkeyKey( string key ) {
            return key.ToLowerInvariant();
        }

        /// <summary>
        /// Gets the RegistryValueKind for a given object based on its type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// <a href="https://referencesource.microsoft.com/#mscorlib/microsoft/win32/registrykey.cs,c23479cd45b3f448"> CalculateValueKind(Object) - Microsoft .NET Framework Reference Source</a>
        /// </remarks>
        RegistryValueKind GetValueKind( object value ) {

            if( value is int ) {
                return RegistryValueKind.DWord;
            }

            if( value is Array ) {

                if( value is byte[] )
                    return RegistryValueKind.Binary;

                if( value is string[] ) {
                    return RegistryValueKind.MultiString;
                }

                throw new ArgumentException( "Bad array type" );
            }

            return RegistryValueKind.String;

        }

        /// <summary>
        /// Builds the full qualified name (rooted path)
        /// </summary>
        /// <returns>The full qualified name.</returns>
        string BuildFullQualifiedName() {

            Stack<InMemoryRegistryKey> keys = new Stack<InMemoryRegistryKey>();
            int capacity = 0;

            InMemoryRegistryKey current = this;

            while( current != null ) {
                capacity += current._name.Length + 1;
                keys.Push( current );
                current = current._parent;
            }

            StringBuilder sb = new StringBuilder( capacity );

            while( keys.Count != 0 ) {
                _ = sb.AppendFormat( "{0}\\", keys.Pop()._name );
            }

            return sb.ToString( 0, capacity - 1 );
        }

        /// <inheritdoc/>
        public IEnumerable<IRegistryKey> Keys {
            get {
                foreach( IRegistryKey key in _keys.Values ) {
                    yield return key;
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<RegistryValue> Values {
            get {
                foreach( RegistryValue value in _values.Values ) {
                    yield return value;
                }
            }
        }
#if DEBUG
        [DebuggerBrowsable( DebuggerBrowsableState.Never )]
        string DebuggerDisplay {
            get => $"{Name}, Keys: {SubKeyCount}, Values: {ValueCount}, Writable: true";
        }
#endif

        #region Internal Classes

        /// <summary>
        /// Internal class for managing a name/value pair.
        /// </summary>
        [DebuggerDisplay( "{DebuggerDisplay,nq}" )]
        public class RegistryValue {

            /// <summary>
            /// The name.
            /// </summary>
            public string Name;

            /// <summary>
            /// The value.
            /// </summary>
            public object Value;

            /// <summary>
            /// The <see cref="RegistryValueKind"/>.
            /// </summary>
            public RegistryValueKind Kind;

            [DebuggerBrowsable( DebuggerBrowsableState.Never )]
            string DebuggerDisplay {
                get => $"{( Name == "" ? "(Default)" : Name )} = {Value}, [{Kind}]";
            }
        }

        #endregion

        // ValidateKeyname impl of MS -> https://referencesource.microsoft.com/#mscorlib/microsoft/win32/registrykey.cs,40afa6bf88a6fd60

    }
}
