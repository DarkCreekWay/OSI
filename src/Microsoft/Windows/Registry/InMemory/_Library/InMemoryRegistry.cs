using System;
using System.Collections.Generic;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry.InMemory {

    ///<inheritdoc/>
    public class InMemoryRegistry
    : IRegistry {

        BaseKeys _32Bit = new BaseKeys();
        BaseKeys _64Bit = new BaseKeys();
        BaseKeys _default = null;

        /// <summary>
        /// Initializes a new instance of the InMemoryRegistry class.
        /// </summary>
        /// <param name="defaultView">The <see cref="RegistryView"/> to be used as default view type.</param>
        public InMemoryRegistry( RegistryView defaultView ) {

            switch( defaultView ) {

                case RegistryView.Default:
                    throw new ArgumentOutOfRangeException( nameof( defaultView ), "Please specify a concrete RegistryView (32/64 bit) as default" );

                case RegistryView.Registry32:
                    _default = _32Bit;
                    break;

                case RegistryView.Registry64:
                    _default = _64Bit;
                    break;

                default:
                    throw new ArgumentOutOfRangeException( nameof( defaultView ) );

            }

            RegistryHive[] hives = (RegistryHive[])Enum.GetValues( typeof( RegistryHive ) );

            foreach( RegistryHive hive in hives ) {
                _32Bit.Keys.Add( hive, new InMemoryRegistryKey( null, GetNameforHive( hive ) ) );
                _64Bit.Keys.Add( hive, new InMemoryRegistryKey( null, GetNameforHive( hive ) ) );
            }
        }

        ///<inheritdoc/>
        public IRegistryKey OpenBaseKey( RegistryHive hKey, RegistryView view = RegistryView.Default ) {

            BaseKeys keys = GetKeysForView( view );
            return keys.Keys[hKey];

        }

        BaseKeys GetKeysForView( RegistryView view ) {

            switch( view ) {

                case RegistryView.Default:
                    return _default;

                case RegistryView.Registry32:
                    return _32Bit;

                case RegistryView.Registry64:
                    return _64Bit;

                default:
                    throw new ArgumentOutOfRangeException( nameof( view ) );
            }
        }

        static string GetNameforHive( RegistryHive hive ) {

            switch( hive ) {

                case RegistryHive.ClassesRoot:
                    return "HKEY_CLASSES_ROOT";

                case RegistryHive.CurrentConfig:
                    return "HKEY_CURRENT_CONFIG";

                case RegistryHive.CurrentUser:
                    return "HKEY_CURRENT_USER";

                case RegistryHive.LocalMachine:
                    return "HKEY_LOCAL_MACHINE";

                case RegistryHive.PerformanceData:
                    return "HKEY_PERFORMANCE_DATA";

                case RegistryHive.Users:
                    return "HKEY_USERS";

                default:
                    throw new ArgumentOutOfRangeException( nameof( hive ) );
            }
        }

        /// <summary>
        /// Internal class for managing base registry keys.
        /// </summary>
        class BaseKeys {

            public Dictionary<RegistryHive, IRegistryKey> Keys = new Dictionary<RegistryHive, IRegistryKey>();

        }
    }
}
